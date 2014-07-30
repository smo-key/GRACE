using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO.Pipes;
using System.IO;
using GRACEdata;
using System.Windows.Media.Imaging;

namespace GRACEMap
{
    public partial class MapMain : GRACEMap.BaseForm
    {
        private Graphics map;
        private Graphics scale;
        private int filen = 0; //current file number

        public MapMain()
            : base()
        {
            InitializeComponent();
        }

        private void ReadData_Click(object sender, EventArgs e)
        {
            ReadNow.Enabled = false;
            gridsize.Enabled = false;
            GridsizeText.Enabled = false;
            Status.Image = global::GRACEMap.Properties.Resources.StatusAnnotations_Play_16xLG;
            Status.Text = "       Initializing...";
            Status.ForeColor = Color.FromArgb(64, 64, 64);
            Filter.Enabled = false;
            FilterText.Enabled = false;
            Progress.Value = 0;
            SaveScale.Enabled = false;
            AllM.Enabled = false;
            AllY.Enabled = false;
            Sensitivity.Enabled = false;
            SensitivityText.Enabled = false;
            DateLabel.Text = Filter.Text;
            DispBack.Enabled = false;
            if (SaveScale.Checked) { DateLabel.Visible = true; } else { DateLabel.Visible = false; }

            Thread thread = new Thread(ReadAllData);
            thread.IsBackground = true;
            thread.Name = "Read Data Thread";
            thread.Start();
        }

        public int FindMax()
        {
            //** SET SETTINGS **//
            Globals.gridsize = (double)this.gridsize.Value;
            Structs.PointTime lasttime = new Structs.PointTime();
            Structs.Anchor anchor = Structs.Anchor.Uniform;
            if (!(360 % Globals.gridsize == 0)) { anchor = Structs.Anchor.Center; }

            string maxestext = "../../../maxmonth.txt";
            int sublen = 7;
            if (Filter.Text.Length < 6) { maxestext = "../../../maxyear.txt"; sublen = 4; }

            //*DOES MAXES.TXT EXIST*//

            if (!File.Exists(maxestext))
            {
                FileStream stream = File.Create(maxestext);
                stream.Close();
            }
            int maximum = 0;

            //*READ MAXES.TXT*//
            string[] lines = System.IO.File.ReadAllLines(maxestext);

            //*CHECK IF BINSIZE EXISTS*//
            foreach (string line in lines)
            {
                if (Convert.ToDouble(line.Split(' ')[0]) == Globals.gridsize)
                {
                    return Convert.ToInt32(line.Split(' ')[1]);
                }
            }

            //** GET LIST OF YEAR / MONTH **//
            List<string> ym = new List<string>();
            string[] list = Directory.GetFiles("../../../../../gracedata/groundtrack/", "*.latlon", SearchOption.TopDirectoryOnly);
            foreach (string file in list)
            {
                FileInfo fi = new FileInfo(file);
                if (!ym.Contains(fi.Name.Substring(0, sublen)))
                {
                    ym.Add(fi.Name.Substring(0, sublen));
                }
            }

            filen = 0; //current file number
            Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = list.Length; }));

            foreach (string f in ym)
            {
                //** GET FILE COUNT **//
                string[] files = Directory.GetFiles("../../../../../gracedata/groundtrack/", f + "*.latlon", SearchOption.TopDirectoryOnly);
                SetProgress(filen);

                //** READ ALL FILES **//
                int[,] bins = GetData(files);

                //** GET RANGE **//
                SetStatus("Parsing range...");
                int max = 0;
                for (int a = 0; a < Structs.CoercedBin.BinsLon; a++)
                {
                    for (int b = 0; b < Structs.CoercedBin.BinsLat + 1; b++)
                    {
                        if (bins[a, b] > max) { max = bins[a, b]; }
                    }
                }
                if (maximum < max) { maximum = max; }
            }

            //WRITE MAX AND BINSIZE TO MAXES.TXT
            StreamWriter writer = new StreamWriter(maxestext);
            writer.WriteLine(String.Format("{0} {1}", Globals.gridsize, maximum));
            writer.Flush();
            writer.Close();
            return maximum; //largest single bin from largest month
        }

        private void ReadAllData()
        {
            if (!(AllM.Checked || AllY.Checked))
            {
                ReadData();
                return;
            }
            if (!SaveImage.Checked)
            {
                Filter.Invoke(new MethodInvoker(delegate { Filter.Text = ""; }));
                ReadData();
                return;
            }

            //create multi-month/year GIF
            int sublen = 7;
            if (Filter.Text.Length < 6) { sublen = 4; }

            //** SET SETTINGS **//
            Globals.gridsize = (double)this.gridsize.Value;
            Structs.Anchor anchor = Structs.Anchor.Uniform;
            if (!(360 % Globals.gridsize == 0)) { anchor = Structs.Anchor.Center; }

            //** GET LIST OF YEAR / MONTH **//
            List<string> ym = new List<string>();
            string[] list = Directory.GetFiles("../../../../../gracedata/groundtrack/", "*.latlon", SearchOption.TopDirectoryOnly);
            foreach (string file in list)
            {
                FileInfo fi = new FileInfo(file);
                if (!ym.Contains(fi.Name.Substring(0, sublen)))
                {
                    ym.Add(fi.Name.Substring(0, sublen));
                }
            }
            Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = list.Length; }));
            filen = 0;
            SetProgress(filen);

            int max = FindMax();
            SetStatus("Drawing scale...");
            DrawScale(max);

            //create GIF
            System.Windows.Media.Imaging.GifBitmapEncoder gEnc = new GifBitmapEncoder();
            File.Delete("../../../output.gif");

            foreach (string f in ym)
            {
                //** CLEAR MAP **//
                map.Clear(SystemColors.Control);
                if (DispBack.Checked) { map.DrawImageUnscaled(global::GRACEMap.Properties.Resources.World_Map, 0, 0); }

                //** GET FILE COUNT **//
                string[] files = Directory.GetFiles("../../../../../gracedata/groundtrack/", f + "*.latlon", SearchOption.TopDirectoryOnly);
                SetProgress(filen);
                SetDate(f);

                //** READ ALL FILES **//
                int[,] bins = GetData(files);

                //** WRITE TO BITMAP **//
                SetStatus("Drawing map...");

                int alpha = 200;
                if (!DispBack.Checked) { alpha = 255; }

                for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
                {
                    for (int j = 0; j < Structs.CoercedBin.BinsLat + 1; j++)
                    {
                        int k = j - Structs.CoercedBin.BinLatCenter;
                        Structs.Point c = Structs.CoercedBin.GetCenter(i, k);
                        Structs.Point size = Structs.CoercedBin.GetSize(c.x, c.y);
                        Structs.AreaBox box = new Structs.AreaBox();
                        switch (anchor)
                        {
                            case Structs.Anchor.Center:
                                box = new Structs.AreaBox(GRACEdata.Utils.coerce(c.x - (size.x / 2), 0, 360),
                                    GRACEdata.Utils.coerce(c.x + (size.x / 2), 0, 360),
                                    GRACEdata.Utils.coerce(c.y - (size.y / 2), -90, 90),
                                    GRACEdata.Utils.coerce(c.y + (size.y / 2), -90, 90));
                                break;
                            case Structs.Anchor.Uniform:
                                box = new Structs.AreaBox(GRACEdata.Utils.coerce(c.x, 0, 360),
                                    GRACEdata.Utils.coerce(c.x + size.x, 0, 360),
                                    GRACEdata.Utils.coerce(c.y, -90, 90),
                                    GRACEdata.Utils.coerce(c.y + size.y, -90, 90));
                                break;
                            default:
                                break;
                        }

                        RectangleF rect = Utils.BinToMap(box);

                        double value = (double)bins[i, j] * 100d / max;
                        Color color = Utils.BlueToRedScale(value, max, (double)Sensitivity.Value, alpha);
                        Brush brush = new SolidBrush(color);
                        map.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);

                    }
                }

                //** ADD TO GIF **//
                this.Invoke(new MethodInvoker(delegate
                {
                    this.TopMost = true;
                    dragenabled = false;
                }));
                
                Rectangle b = this.Bounds;
                Rectangle bounds = new Rectangle(b.Left, b.Top + 102, 801, 400);
                Bitmap gif = new Bitmap(bounds.Width, bounds.Height);
                Graphics g = Graphics.FromImage(gif);
                this.Invoke(new MethodInvoker(delegate
                {
                    this.TopMost = false;
                    dragenabled = true;
                }));

                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        gif.GetHbitmap(),
                        IntPtr.Zero,
                        System.Windows.Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                gEnc.Frames.Add(BitmapFrame.Create(src));
                
            }

            gEnc.Save(new FileStream("../../../output.gif", FileMode.Create));

            //** EXIT **//
            SetProgress(Progress.Maximum);
            this.Invoke(new MethodInvoker(delegate
            {
                ReadNow.Enabled = true;
                gridsize.Enabled = true;
                GridsizeText.Enabled = true;
                Status.Image = global::GRACEMap.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
                Status.Text = "       Completed successfully!";
                Status.ForeColor = Color.Green;
                Filter.Enabled = true;
                FilterText.Enabled = true;
                Sensitivity.Enabled = true;
                SensitivityText.Enabled = true;
                if (AllY.Checked || AllM.Checked)
                {
                    if (AllY.Checked) { AllY.Enabled = true; } else { AllY.Enabled = false; }
                    if (AllM.Checked) { AllM.Enabled = true; } else { AllM.Enabled = false; }
                }
                else
                {
                    AllY.Enabled = true;
                    AllM.Enabled = true;
                }
            }));
            return;
        }

        private int[,] GetData(string[] files)
        {
            Structs.PointTime lasttime = new Structs.PointTime();

            //** INITIALIZE BIN COUNTS **//
            int[,] bins = new int[Structs.CoercedBin.BinsLon, Structs.CoercedBin.BinsLat + 1];
            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat + 1; j++)
                {
                    bins[i, j] = 0;
                }
            }

            //** READ ALL FILES **//
            foreach (string file in files)
            {
                SetProgress(filen);
                SetStatus(string.Format("Reading {0}...", Path.GetFileName(file)));
                filen++;
                StreamReader reader = new StreamReader(file);
                while (!reader.EndOfStream)
                {
                    string s = reader.ReadLine();
                    string[] parameters = s.Split(' ');
                    int count = parameters.Length;

                    DateTime time = GRACEdata.Utils.GetTime(Convert.ToDouble(parameters[0]));
                    double latA = Convert.ToDouble(parameters[1]);
                    double lonA = Convert.ToDouble(parameters[2]);
                    double altA = Convert.ToDouble(parameters[3]);
                    double latB = Convert.ToDouble(parameters[7]);
                    double lonB = Convert.ToDouble(parameters[8]);
                    double altB = Convert.ToDouble(parameters[9]);
                    Structs.GPSData data = new Structs.GPSData(time, latA, lonA, altA, latB, lonB, altB);
                    Structs.GPSBoxed gpsbox = new Structs.GPSBoxed(data, Structs.Satellite.GraceA, Structs.Anchor.Uniform);

                    Structs.PointTime current = new Structs.PointTime(gpsbox.bin.boxcenter, data.time);

                    if (lasttime.point != current.point)
                    {
                        //if just entered bin
                        bins[gpsbox.bin.lonbox, Structs.CoercedBin.BinLatCenter + gpsbox.bin.latbox]++; //add 1 to bin count
                        lasttime = current;
                    }

                }

                System.GC.Collect(); //free some memory
            }

            return bins;
        }

        private void ReadData()
        {
            //** CLEAR MAP **//
            map.Clear(SystemColors.Control);
            if (DispBack.Checked) { map.DrawImageUnscaled(global::GRACEMap.Properties.Resources.World_Map, 0, 0); }

            int max = FindMax();
            SetStatus("Drawing scale...");
            DrawScale(max);

            //** CREATE GIF **/
            Rectangle b = this.Bounds;
            Rectangle bounds = new Rectangle(b.Left, b.Top + 102, 801, 400);
            Bitmap gif = new Bitmap(bounds.Width, bounds.Height);
            Graphics g = Graphics.FromImage(gif);

            //** GET FILE COUNT **//
            string[] files = Directory.GetFiles("../../../../../gracedata/groundtrack/", Filter.Text + "*.latlon", SearchOption.TopDirectoryOnly); //2002-09
            filen = 0;
            Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = files.Length; }));
            SetProgress(filen);

            //** SET SETTINGS **//
            Globals.gridsize = (double)this.gridsize.Value;
            Structs.Anchor anchor = Structs.Anchor.Uniform;
            if (!(360 % Globals.gridsize == 0)) { anchor = Structs.Anchor.Center; }

            //** READ DATA **//
            int[,] bins = GetData(files);

            //** WRITE TO BITMAP **//
            SetProgress(Progress.Maximum - 1);
            SetStatus("Drawing map...");

            int alpha = 200;
            if (!DispBack.Checked) { alpha = 255; }

            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat + 1; j++)
                {
                    int k = j - Structs.CoercedBin.BinLatCenter;
                    Structs.Point c = Structs.CoercedBin.GetCenter(i, k);
                    Structs.Point size = Structs.CoercedBin.GetSize(c.x, c.y);
                    Structs.AreaBox box = new Structs.AreaBox();
                    switch (anchor)
                    {
                        case Structs.Anchor.Center:
                            box = new Structs.AreaBox(GRACEdata.Utils.coerce(c.x - (size.x / 2), 0, 360),
                                GRACEdata.Utils.coerce(c.x + (size.x / 2), 0, 360),
                                GRACEdata.Utils.coerce(c.y - (size.y / 2), -90, 90),
                                GRACEdata.Utils.coerce(c.y + (size.y / 2), -90, 90));
                            break;
                        case Structs.Anchor.Uniform:
                            box = new Structs.AreaBox(GRACEdata.Utils.coerce(c.x, 0, 360),
                                GRACEdata.Utils.coerce(c.x + size.x, 0, 360),
                                GRACEdata.Utils.coerce(c.y, -90, 90),
                                GRACEdata.Utils.coerce(c.y + size.y, -90, 90));
                            break;
                        default:
                            break;
                    }

                    RectangleF rect = Utils.BinToMap(box);

                    double value = (double)bins[i, j] * 100d / max;
                    Color color = Utils.BlueToRedScale(value, max, (double)Sensitivity.Value, alpha);
                    Brush brush = new SolidBrush(color);
                    map.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);

                }
            }

            //save image (only one)
            if (SaveImage.Checked) { SaveFrame("../../../output.gif"); }

            //** EXIT **//
            SetProgress(Progress.Maximum);
            this.Invoke(new MethodInvoker(delegate
            {
                ReadNow.Enabled = true;
                gridsize.Enabled = true;
                GridsizeText.Enabled = true;
                Status.Image = global::GRACEMap.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
                Status.Text = "       Completed successfully!";
                Status.ForeColor = Color.Green;
                Filter.Enabled = true;
                FilterText.Enabled = true;
                Sensitivity.Enabled = true;
                SensitivityText.Enabled = true;
                //DispBack.Enabled = true;
                if (AllY.Checked || AllM.Checked)
                {
                    if (AllY.Checked) { AllY.Enabled = true; } else { AllY.Enabled = false; }
                    if (AllM.Checked) { AllM.Enabled = true; } else { AllM.Enabled = false; }
                }
                else
                {
                    AllY.Enabled = true;
                    AllM.Enabled = true;
                }
            }));
            return;
        }

        private void SaveFrame(string name)
        {
            SetStatus("Saving image...");
            Rectangle b = this.Bounds;
            Rectangle bounds = new Rectangle(b.Left, b.Top + 102, 801, 400);

            this.Invoke(new MethodInvoker(delegate
            {
                this.TopMost = true;
                dragenabled = false;
            }));

            File.Delete(name);

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save(name, System.Drawing.Imaging.ImageFormat.Gif);
            }

            this.Invoke(new MethodInvoker(delegate
            {
                this.TopMost = false;
                dragenabled = true;
            }));
        }

        private void DrawScale(int max)
        {
            for (int i = 0; i < 200; i++)
            {
                Pen pen = new Pen(Utils.BlueToRedScale(i / 2, 100, (double)Sensitivity.Value, 255));
                scale.DrawLine(pen, i, 0, i, 23);
            }
            this.Invoke(new MethodInvoker(delegate
            {
                Max.Text = max.ToString();
            }));
        }

        private void SetDate(string date)
        {
            DateLabel.Invoke(new MethodInvoker(delegate
            {
                DateLabel.Text = date;
            }));
        }

        private void SetProgress(int value)
        {
            Progress.Invoke(new MethodInvoker(delegate { Progress.Value = value; }));
        }
        private void SetStatus(string message)
        {
            Status.Invoke(new MethodInvoker(delegate { Status.Text = "       " + message; }));
        }

        private void Item_MouseDown(object sender, MouseEventArgs e)
        {
            Control s = (Control)sender;
            s.ForeColor = Color.White;
        }
        private void Item_MouseUp(object sender, MouseEventArgs e)
        {
            Control s = (Control)sender;
            s.ForeColor = Color.FromArgb(64, 64, 64);
        }
        new private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            Button s = (Button)sender;
            s.ForeColor = Color.White;
            s.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.HotTrack;
        }
        new private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            Button s = (Button)sender;
            s.ForeColor = Color.FromArgb(64, 64, 64);
            s.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
        }

        internal override void DragEvent(object sender, MouseEventArgs e)
        {
            int l = e.X + this.Left - locx;
            int t = e.Y + this.Top - locy;
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            if (l < rect.X) { l = rect.X; }
            if (t < rect.Y) { t = rect.Y; }
            if (l + this.Width > rect.Right) { l = rect.Right - this.Width; }
            if (t + this.Height > rect.Bottom) { t = rect.Bottom - this.Height; }
            this.Left = l;
            this.Top = t;
        }

        private void OverMap_Paint(object sender, PaintEventArgs e)
        {
            map = OverMap.CreateGraphics();
        }

        private void SaveScale_CheckedChanged(object sender, EventArgs e)
        {
            if (SaveScale.Checked)
            {
                ScaleBox.Location = new Point(ScaleBox.Location.X, 465);
            }
            else
            {
                ScaleBox.Location = new Point(ScaleBox.Location.X, 506);
            }
        }

        private void Scale_Paint(object sender, PaintEventArgs e)
        {
            scale = Scale.CreateGraphics();
        }

        private void AllM_CheckedChanged(object sender, EventArgs e)
        {
            if (AllM.Checked)
            {
                AllY.Enabled = false;
                Filter.Enabled = false;
                Filter.Text = "2002-08";
            }
            else
            {
                AllY.Enabled = true;
                Filter.Enabled = true;
            }
        }

        private void AllY_CheckedChanged(object sender, EventArgs e)
        {
            if (AllY.Checked)
            {
                AllM.Enabled = false;
                Filter.Enabled = false;
                Filter.Text = "2002";
            }
            else
            {
                AllM.Enabled = true;
                Filter.Enabled = true;
            }
        }

        private void DispBack_CheckedChanged(object sender, EventArgs e)
        {
            map.Clear(SystemColors.Control);
            if (DispBack.Checked) { map.DrawImageUnscaled(global::GRACEMap.Properties.Resources.World_Map, 0, 0); }
        }

    }
}