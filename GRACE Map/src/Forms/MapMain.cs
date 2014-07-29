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

namespace GRACEMap
{
    public partial class MapMain : GRACEMap.BaseForm
    {
        private Graphics map;
        private Graphics scale;

        public MapMain() : base()
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

            Thread thread = new Thread(ReadData);
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

            //*DOES MAXES.TXT EXIST*//
            if (!File.Exists("../../../maxes.txt"))
            {
                FileStream stream = File.Create("../../../maxes.txt");
                stream.Close();
            }
            int maximum = 0;

            //*READ MAXES.TXT*//
            string[] lines = System.IO.File.ReadAllLines("../../../maxes.txt");

            //*CHECK IF BINSIZE EXISTS*//
            foreach(string line in lines)
            {
                if(Convert.ToDouble(line.Split(' ')[0]) == Globals.gridsize)
                {
                    return Convert.ToInt32(line.Split(' ')[1]);
                }
            }

            //** GET LIST OF YEAR / MONTH **//
            List<string> ym = new List<string>();
            string[] list = Directory.GetFiles("../../../../../gracedata/", "*.latlon", SearchOption.TopDirectoryOnly);
            foreach(string file in list)
            {
                FileInfo fi = new FileInfo(file);
                if(!ym.Contains(fi.Name.Substring(0, 7)))
                {
                    ym.Add(fi.Name.Substring(0,7));
                }
            }

            int filen = 0; //current file number
            Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = list.Length; }));

            foreach (string f in ym)
            {
                //** GET FILE COUNT **//
                string[] files = Directory.GetFiles("../../../../../gracedata/", f + "*.latlon", SearchOption.TopDirectoryOnly);
                SetProgress(filen);

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
                    SetStatus(string.Format("Reading {0} for maximum...", Path.GetFileName(file)));
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
            StreamWriter writer = new StreamWriter("../../../maxes.txt");
            writer.WriteLine(String.Format("{0} {1}", Globals.gridsize, maximum));
            writer.Flush();
            writer.Close();
            return maximum; //largest single bin from largest month
        }

        private void DrawScale(int max)
        {
            for (int i = 0; i < 200; i++)
            {
                Pen pen = new Pen(Utils.BlueToRedScale(i / 2, max, 255));
                scale.DrawLine(pen, i, 0, i, 23);
            }
            this.Invoke(new MethodInvoker(delegate
            {
                Max.Text = max.ToString();
            }));
        }

        private void ReadData()
        {
            //** CLEAR MAP **//
            map.Clear(SystemColors.Control);
            map.DrawImageUnscaled(global::GRACEMap.Properties.Resources.World_Map, 0, 0);

            int max = FindMax();
            SetStatus("Drawing scale...");
            DrawScale(max);

            //** GET FILE COUNT **//
            string[] files = Directory.GetFiles("../../../../../gracedata/", Filter.Text + "*.latlon", SearchOption.TopDirectoryOnly); //2002-09
            int filen = 0; //current file number
            Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = files.Length; }));
            SetProgress(filen);

            //** SET SETTINGS **//
            Globals.gridsize = (double)this.gridsize.Value;
            Structs.PointTime lasttime = new Structs.PointTime();
            Structs.Anchor anchor = Structs.Anchor.Uniform;
            if (!(360 % Globals.gridsize == 0)) { anchor = Structs.Anchor.Center; }

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

            //** WRITE TO BITMAP **//
            SetProgress(Progress.Maximum - 1);
            SetStatus("Drawing map...");

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
                    Color color = Utils.BlueToRedScale(value, max, 200);
                    Brush brush = new SolidBrush(color);
                    map.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);

                }
            }

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
            }));
            return;
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

        private void OverMap_Paint(object sender, PaintEventArgs e)
        {
            map = OverMap.CreateGraphics();
        }

        private void SaveScale_CheckedChanged(object sender, EventArgs e)
        {
            if (SaveScale.Checked)
            {
                ScaleBox.Location = new Point(SaveScale.Location.X, 447);
                DateLabel.Visible = true;
            }
            else
            {
                ScaleBox.Location = new Point(SaveScale.Location.X, 488);
                DateLabel.Visible = false;
            }
        }

        private void Scale_Paint(object sender, PaintEventArgs e)
        {
            scale = Scale.CreateGraphics();
        }

    }
}
