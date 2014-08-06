using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using GRACEdata;
using System.IO.Pipes;
using System.IO;
namespace GRACEChart
{
    public partial class MainChart : BaseForm
    {   
        String[] GLDASData = new String[] {};
        String[] RL05Data = new String[] {};
        List<String> GRACEData = new List<string>();
        List<PointF> gldas = new List<PointF>();
        List<PointF> rl05 = new List<PointF>();
        List<PointF> grace = new List<PointF>();
        List<PointF> graceGLDAS = new List<PointF>();
        List<PointF> graceRL05 = new List<PointF>();
        Graphics g;
        int max;
        int max2;
        int filen;
        double x;
        double y;
        double size;
        double lat1;
        double lat2;
        double lon1;
        double lon2;
        String lastitem = "NaSTy";
        double datamin = double.PositiveInfinity;
        double datamax = double.NegativeInfinity;
        public MainChart()
        {
            InitializeComponent();
            Location.SelectedItem = "ACC";
            GLDAS.Enabled = true;
            RL05.Enabled = true;
            GRACE.Enabled = true;
            SaveImage.Enabled = true;
            LocationLabel.Visible = false;
            MaxText.Visible = false;
            MinText.Visible = false;
            ZeroLabel.Visible = false;



            /** GET LIST OF YEAR / MONTH **/
            List<string> ym = new List<string>();
            string[] list = Directory.GetFiles("../../../../../gracedata/groundtrack/", "*.latlon", SearchOption.TopDirectoryOnly);
            foreach (string file in list)
            {
                FileInfo fi = new FileInfo(file);
                if ((!ym.Contains(fi.Name.Substring(0, 7))) && (Convert.ToInt32(fi.Name.Substring(0, 4)) >= 2008) && (Convert.ToInt32(fi.Name.Substring(0, 4)) <= 2009))
                {
                    if (Convert.ToInt32(fi.Name.Substring(0, 4)) == 2009)
                    {
                        if (Convert.ToInt32(fi.Name.Substring(5, 2)) >= 2) { break; }
                    }
                    ym.Add(fi.Name.Substring(0, 7));
                }
            }
            string[] search = System.IO.Directory.GetFiles("../../../../../gracedata/groundtrack/");

            foreach (string i in search)
            {
                FileInfo info = new FileInfo(i);
                if (ym.Contains(info.Name.Substring(0, 7))) { GRACEData.Add(i); }
            }

            size = 3.00;
            lat1 = y + (size / 2);
            lat2 = y - (size / 2);
            lon1 = x - (size / 2);
            lon2 = x + (size / 2);
            filen = 0;

        } 
            
        public void Search()
        {
            String seltext = "";
            Location.Invoke(new MethodInvoker(delegate { seltext = Location.SelectedItem.ToString(); }));

            String itemname = "ACC01";
            if (seltext == "ACC")
            {
                itemname = "ACC01";
                x = 30.0;
                y = -60.0;
            }
            else if (seltext == "Amazon")
            {
                itemname = "Amazon01";
                x = 305.0;
                y = -2.5;
            }
            else if (seltext == "Antarctic")
            {
                itemname = "Antarct01";
                x = 305.0;
                y = -79.0;
            }
            else if (seltext == "Baltic Sea")
            {
                itemname = "BalticSea";
                x = 19.0;
                y = 57.5;
            }
            else if (seltext == "Baltic Sea 02")
            {
                itemname = "BalticSea02";
                x = 18.0;
                y = 56.0;
            }
            else if (seltext == "Bangladesh")
            {
                itemname = "Bangladesh";
                x = 90.0;
                y = 24.0;
            }
            else if (seltext == "Black Sea")
            {
                itemname = "BlackSea";
                x = 31.0;
                y = 43.5;
            }
            else if (seltext == "Black Sea 02")
            {
                itemname = "BlackSea02";
                x = 30.0;
                y = 43.0;
            }
            else if (seltext == "Columbia")
            {
                itemname = "Columbia";
                x = 241.0;
                y = 46.0;
            }
            else if (seltext == "Congo")
            {
                itemname = "Congo";
                x = 23.0;
                y = -12.0;
            }
            else if (seltext == "East Siberia")
            {
                itemname = "EastSib01";
                x = 170.0;
                y = 72.0;
            }
            else if (seltext == "Guinea")
            {
                itemname = "Guinea";
                x = 351.0;
                y = 8.0;
            }
            else if (seltext == "Gulf Carpentaria")
            {
                itemname = "GulfCarpen";
                x = 139.0;
                y = -15.0;
            }
            else if (seltext == "Hudson Bay")
            {
                itemname = "HudsonBay";
                x = 272.5;
                y = 60.0;
            }
            else if (seltext == "Hudson Bay 02")
            {
                itemname = "HudsonBay02";
                x = 272.5;
                y = 58.0;
            }
            else if (seltext == "Mediterranean")
            {
                itemname = "Mediterr01";
                x = 19.0;
                y = 36.0;
            }
            else if (seltext == "Mekong")
            {
                itemname = "Mekong";
                x = 105.0;
                y = 12.0;
            }
            else if (seltext == "NCP")
            {
                itemname = "NCP";
                x = 117.0;
                y = 34.5;
            }
            else if (seltext == "Ob")
            {
                itemname = "Ob";
                x = 69.0;
                y = 61.5;
            }
            else if (seltext == "Orinoco")
            {
                itemname = "Orinoco";
                x = 292.0;
                y = 5.0;
            }
            else if (seltext == "Pearl River")
            {
                itemname = "PearlRiver";
                x = 113.0;
                y = 23.0;
            }
            else if (seltext == "Sao Paulo")
            {
                itemname = "SaoPaulo";
                x = 312.0;
                y = -22.0;
            }
            else if (seltext == "StNewFL")
            {
                itemname = "StNewFL01";
                x = 301.0;
                y = 45.0;
            }
            else if (seltext == "Victoria")
            {
                itemname = "Victoria";
                x = 32.0;
                y = -2.0;
            }
            GLDASData = System.IO.Directory.GetFiles("../../../../../gracedata/modeltimeseries/GLDAS", itemname + "*");
            RL05Data = System.IO.Directory.GetFiles("../../../../../gracedata/modeltimeseries/RL05", itemname + "*");
            max = GLDASData.Length + RL05Data.Length + GRACEData.Count;
            max2 = gldas.Count + rl05.Count + grace.Count + graceGLDAS.Count + graceRL05.Count;
            Progress.Value = 0;
            filen = 0;
            Progress.Maximum = max + max2;
        }

        //*START*//
        private void DrawButton_Click(object sender, EventArgs e)
        {
            Progress.Refresh();
            LocationLabel.Text = Convert.ToString(Location.SelectedItem);
            LocationLabel.Visible = true;
            g.Clear(Color.Transparent);
            Chart.Refresh();
            Location.Enabled = false;
            GLDAS.Enabled = false;
            RL05.Enabled = false;
            GRACE.Enabled = false;
            Zero.Enabled = false;
            DrawButton.Enabled = false;
            SaveImage.Enabled = false;
            MaxText.Visible = false;
            MinText.Visible = false;
            ZeroLabel.Visible = false;
            Thread thread = new Thread(ReadAllData);
            thread.IsBackground = true;
            thread.Name = "Read Data Thread";
            thread.Start();
        }
        public void ReadAllData()
        {
            String seltext = "";
            Location.Invoke(new MethodInvoker(delegate { seltext = Location.SelectedItem.ToString(); }));
            if(lastitem != seltext)
            {
                datamin = double.MaxValue;
                datamax = double.MinValue;
                gldas.Clear();
                rl05.Clear();
                grace.Clear();
                graceGLDAS.Clear();
                graceRL05.Clear();

                this.Invoke(new MethodInvoker(delegate { Search(); }));
                readRL05();
                readGLDAS();
                PostProcess();
                readGRACE();
            }

            if (GLDAS.Checked)
            {
                showGLDAS(g);
            }

            if (RL05.Checked)
            {
                showRL05(g);
            }

            if (GRACE.Checked)
            {
                showGRACE(g);
            }

            if (Zero.Checked)
            {
                drawZeroLine(g);
            }

            this.Invoke(new MethodInvoker(delegate
            {
                ZeroLabel.Location = new Point(ZeroLabel.Location.X, 82);
            }));

            this.Invoke(new MethodInvoker(delegate
            { moveZero(); }));

            this.Invoke(new MethodInvoker(delegate
            {
                MaxText.Text = datamax.ToString("F2");
                MinText.Text = datamin.ToString("F2");
                MaxText.Visible = true;
                MinText.Visible = true;
                ZeroLabel.Visible = true;
            }));

             //** EXIT **//
            SetProgress(Progress.Maximum);
            this.Invoke(new MethodInvoker(delegate
            {
                DrawButton.Enabled = true;
                CloseForm.Enabled = true;
                SaveImage.Enabled = true;
                Location.Enabled = true;
                GLDAS.Enabled = true;
                RL05.Enabled = true;
                GRACE.Enabled = true;
                Zero.Enabled = true;
                if (SaveImage.Checked)
                {
                    save();
                }
                SetStatus("Completed Successfully!");
                lastitem = Location.SelectedItem.ToString();
            }));
        }
    
        //*SAVE*//
        public void save()
        {
            SetStatus("Saving Image...");
            SaveFrame("../../../../output/chart");
        }

        //*SAVE FRAME*//
        private void SaveFrame(string name)
        {
            SetStatus("Saving Image...");
            Rectangle b = this.Bounds;
            Rectangle bounds = new Rectangle(b.Left + 2, b.Top + 76,800, 459);

            this.Invoke(new MethodInvoker(delegate
            {
                this.TopMost = true;
                dragenabled = false;
            }));
            File.Delete(name + ".png");
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            bitmap.Save(name + ".png", System.Drawing.Imaging.ImageFormat.Png);
            

            this.Invoke(new MethodInvoker(delegate
            {
                this.TopMost = false;
                dragenabled = true;
            }));
        }

        //*DRAWS LINE AT Y = 0*//
        public void drawZeroLine(Graphics g)
        {
            Pen myPen = new Pen(Color.Black);
            PointF a = new PointF(0, 0);
            PointF b = new PointF(8784, 0);
            PointF p1 = adjustedPoint(a);
            PointF p2 = adjustedPoint(b);
            g.DrawLine(myPen, p1, p2);
        }
        public void PostProcess()
        {
            for (int k = 0; k < gldas.Count; k++)
            {
                PointF p = new PointF(adjustedPoint(gldas[k]).X, adjustedPoint(gldas[k]).Y);
                gldas[k] = p;
            }
            for (int k = 0; k < rl05.Count; k++)
            {
                PointF p = new PointF(adjustedPoint(rl05[k]).X, adjustedPoint(rl05[k]).Y);
                rl05[k] = p;
            }   
        }

        //*READ GLDAS DATA*// 
        public void readGLDAS()
        {
            foreach (String file in GLDASData)
            {
                StreamReader reader = new StreamReader(file);
                while (!reader.EndOfStream)
                {
                    string s = reader.ReadLine();
                    string[] parameters = s.Split(' ');
                    int count = parameters.Length;

                    int t = Convert.ToInt32(parameters[3]);
                    float height = float.Parse(parameters[9]);
                    PointF p = new PointF(t, height);
                    gldas.Add(p);
                    if (p.Y > datamax) { datamax = p.Y; }
                    else if (p.Y < datamin) { datamin = p.Y; }
                }
                filen++;
                SetStatus(string.Format("Reading {0}...", Path.GetFileName(file)));
                SetProgress(filen);
            }
        }

        //*READ RL-05 DATA*//
        public void readRL05()
        {
            foreach (String file in RL05Data)
            {
                StreamReader reader = new StreamReader(file);
                while (!reader.EndOfStream)
                {
                    string s = reader.ReadLine();
                    string[] parameters = s.Split(' ');
                    int count = parameters.Length;

                    int t = Convert.ToInt32(parameters[3]);
                    float height = float.Parse(parameters[9]);
                    PointF p = new PointF(t, height);
                    rl05.Add(p);
                    if (p.Y > datamax) { datamax = p.Y; }
                    else if (p.Y < datamin) { datamin = p.Y; }
                }
                filen++;
                SetStatus(string.Format("Reading {0}...", Path.GetFileName(file)));
                SetProgress(filen);
            }
        }

        //*READ GRACE DATA*//
        public void readGRACE()
        {
            foreach (string file in GRACEData)
            {
                StreamReader reader = new StreamReader(file);
                while (!reader.EndOfStream)
                {
                    string s = reader.ReadLine();
                    string[] parameters = s.Split(' ');
                    int count = parameters.Length;
                    DateTime time = GRACEdata.Utils.GetTime(Convert.ToDouble(parameters[0]));
                    DateTime start = new DateTime(2008, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    TimeSpan diff = time - start;
                    int t = (int)(Math.Floor(diff.TotalHours));
                    double lat = Convert.ToDouble(parameters[1]);
                    double lon = Convert.ToDouble(parameters[2]);
                    if(lat <= lat1 && lat >= lat2 && lon >= lon1 && lon <= lon2 && t <= 8784)
                    {

                        if (3 % t == 0)
                        {
                            int n = t / 3;
                            float h = gldas[n].Y;
                            PointF p = new PointF(t, h);
                            graceGLDAS.Add(adjustedPoint(p));
                        }
                        else
                        {
                            int n1 = (int)Math.Floor((double)t / (double)3);
                            float h = gldas[n1].Y;
                            PointF p = new PointF(adjustedTime(t), h);
                            graceGLDAS.Add(p);
                        }

                        if (6 % t == 0)
                        {
                            int n = t / 6;
                            float h = rl05[n].Y;
                            PointF p = new PointF(t, h);
                            graceRL05.Add(adjustedPoint(p));
                        }
                        else
                        {
                            int n1 = (int)Math.Floor((double)t / (double)6);
                            float h =rl05[n1].Y;
                            PointF p = new PointF(adjustedTime(t), h);
                            graceRL05.Add(p);
                        }

                        float height = 0.0f;
                        PointF p2 = new PointF(t, height);
                        grace.Add(adjustedPoint(p2));
                    }
                }
                filen++;
                SetStatus(string.Format("Reading {0}...", Path.GetFileName(file)));
                SetProgress(filen);
            }

        }

        //*GLDAS GRAPH*//
        public void showGLDAS(Graphics g)
        {
            SolidBrush myBrush = new SolidBrush(Color.DarkGreen);
            Pen myPen = new Pen(Color.DarkGreen);
            for (int k = 0; k < gldas.Count - 1; k++)
            {
                g.DrawLine(myPen, gldas[k], gldas[k + 1]);
                SetStatus("Creating GLDAS Graph");
                SetProgress(filen);
            }
        }

        //*RL05 GRAPH*//
        public void showRL05(Graphics g)
        {
            SolidBrush myBrush = new SolidBrush(Color.DarkBlue);
            Pen myPen = new Pen(Color.DarkBlue);
            for (int k = 0; k < rl05.Count - 1; k++)
            {
                g.DrawLine(myPen, rl05[k], rl05[k + 1]);
                SetStatus("Creating RL-05 Graph");
                SetProgress(filen);
            }

        }

        //*GRACE GRAPH(S)*//
        public void showGRACE(Graphics g)
        {
            SolidBrush myBrush = new SolidBrush(Color.Crimson);
            Pen myPen = new Pen(Color.Crimson);
            SetStatus("Creating GRACE Graph");
            SetProgress(filen);

            //*DRAW POINTS*//
            if(GLDAS.Checked && RL05.Checked)
            {
                foreach(PointF p in graceGLDAS)
                {
                    g.FillEllipse(myBrush, p.X - 2, p.Y - 2, 4, 4);
                }
                foreach (PointF p2 in graceRL05)
                {
                    g.FillEllipse(myBrush, p2.X - 2, p2.Y - 2, 4, 4);
                }

            }
            if (GLDAS.Checked && !RL05.Checked)
            {
                foreach (PointF p3 in graceGLDAS)
                {
                    g.FillEllipse(myBrush, p3.X - 2, p3.Y - 2, 4, 4);
                }
            }
            if (!GLDAS.Checked && RL05.Checked)
            {
                foreach (PointF p4 in graceRL05)
                {
                    g.FillEllipse(myBrush, p4.X - 2, p4.Y - 2, 4, 4);
                }
            }
            if (!GLDAS.Checked && !RL05.Checked)
            {
                foreach (PointF p5 in grace)
                {
                    g.FillEllipse(myBrush, p5.X - 2, p5.Y - 2, 4, 4);
                }
            }
        }

        //*MOVE ZERO*//
        public void moveZero()
        {
            PointF p = new PointF(0.0f, 0.0f);
            int pixelMax = ZeroLabel.Location.Y + Convert.ToInt32(adjustedPoint(p).Y) - 7;
            ZeroLabel.Location = new Point(ZeroLabel.Location.X, pixelMax);
        }
        public PointF adjustedPoint(PointF a)
        {
            float x = (float)a.X * (float)Chart.Width / 8784.0f;
            float h = (float)(datamax - datamin); //chart height
            float y = -(float)(((a.Y - (datamax)) / h) * Chart.Height + 2); //y / scale height * map height + delta
            return new PointF(x, y);
        }
        public float adjustedTime(float t)
        {
            float x = (float)t * (float)Chart.Width / 8784.0f;
            return x;
        }
        private void Progress_Click(object sender, EventArgs e)
        {

        }
        private void Status_Click(object sender, EventArgs e)
        {

        }
        private void SetProgress(int value)
        {
            Progress.Invoke(new MethodInvoker(delegate { Progress.Value = value; }));
        }
        private void SetStatus(string message)
        {
            Status.Invoke(new MethodInvoker(delegate { Status.Text = "       " + message; }));
        }
        private void Chart_Paint(object sender, PaintEventArgs e)
        {
            g = Chart.CreateGraphics();
        }
        private void Chart_Load(object sender, EventArgs e)
        {

        }
        private void Border_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void yAxis_Click(object sender, EventArgs e)
        {

        }
    }
}
