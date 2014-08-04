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
        String[] GLDASData;
        String[] RL05Data;
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
        bool isRead = false;
        String item;

        public MainChart()
        {
            InitializeComponent();
            Location.SelectedItem = "ACC";
            GLDAS.Enabled = true;
            RL05.Enabled = true;
            GRACE.Enabled = true;
            SaveImage.Enabled = true;
            GLDAS.Checked = true;
            RL05.Checked = true;
            GRACE.Checked = true;

            /** GET LIST OF YEAR / MONTH **/
            List<string> ym = new List<string>();
            string[] list = Directory.GetFiles("../../../../../gracedata/groundtrack/", "*.latlon", SearchOption.TopDirectoryOnly);
            foreach (string file in list)
            {
                FileInfo fi = new FileInfo(file);
                if ((!ym.Contains(fi.Name.Substring(0, 4))) && (Convert.ToInt32(fi.Name.Substring(0, 4)) >= 2008))
                {
                    ym.Add(fi.Name.Substring(0, 4));
                }
            }
            string[] search = System.IO.Directory.GetFiles("../../../../../gracedata/groundtrack/");

            foreach (string i in search)
            {
                FileInfo info = new FileInfo(i);
                if (ym.Contains(info.Name.Substring(0, 4))) { GRACEData.Add(i); }
            }
            size = Convert.ToDouble(Binsize.Value);
            lat1 = y + (size/2);
            lat2 = y - (size / 2);
            lon1 = x - (size / 2);
            lon2 = x + (size / 2);

            if (Convert.ToString(Location.SelectedItem).Equals("ACC"))
            {
                item = "ACC01";
                x = 30.0;
                y = -60.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Amazon"))
            {
                item = "Amazon01";
                x = 305.0;
                y = -2.5;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Antarctic"))
            {
                item = "Antarct01";
                x = 305.0;
                y = -79.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Baltic Sea"))
            {
                item = "BalticSea";
                x = 19.0;
                y = 57.5;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Baltic Sea 02"))
            {
                item = "BalticSea02";
                x = 18.0;
                y = 56.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Bangladesh"))
            {
                item = "Bangladesh";
                x = 90.0;
                y = 24.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Black Sea"))
            {
                item = "BlackSea";
                x = 31.0;
                y = 43.5;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Black Sea 02"))
            {
                item = "BlackSea02";
                x = 30.0;
                y = 43.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Columbia"))
            {
                item = "Columbia";
                x = 241.0;
                y = 46.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Congo"))
            {
                item = "Congo";
                x = 23.0;
                y = -12.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("East Siberia"))
            {
                item = "EastSib01";
                x = 170.0;
                y = 72.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Guinea"))
            {
                item = "Guinea";
                x = 351.0;
                y = 8.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Gulf Carpentaria"))
            {
                item = "GulfCarpen";
                x = 139.0;
                y = -15.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Hudson Bay"))
            {
                item = "HudsonBay";
                x = 272.5;
                y = 60.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Hudson Bay 02"))
            {
                item = "HudsonBay02";
                x = 272.5;
                y = 58.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Mediterranean"))
            {
                item = "Mediterr01";
                x = 19.0;
                y = 36.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Mekong"))
            {
                item = "Mekong";
                x = 105.0;
                y = 12.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("NCP"))
            {
                item = "NCP";
                x = 117.0;
                y = 34.5;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Ob"))
            {
                item = "Ob";
                x = 69.0;
                y = 61.5;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Orinoco"))
            {
                item = "Orinoco";
                x = 292.0;
                y = 5.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Pearl River"))
            {
                item = "PearlRiver";
                x = 113.0;
                y = 23.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Sao Paulo"))
            {
                item = "SaoPaulo";
                x = 312.0;
                y = -22.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("StNewFL"))
            {
                item = "StNewFL01";
                x = 301.0;
                y = 45.0;
            }
            if (Convert.ToString(Location.SelectedItem).Equals("Victoria"))
            {
                item = "Victoria";
                x = 32.0;
                y = -2.0;
            }

            GLDASData = System.IO.Directory.GetFiles("../../../../../gracedata/modeltimeseries/GLDAS", item + "*");
            RL05Data = System.IO.Directory.GetFiles("../../../../../gracedata/modeltimeseries/RL05", item + "*");
            max = GLDASData.Length + RL05Data.Length + GRACEData.Count;
            max2 = gldas.Count + rl05.Count + grace.Count + graceGLDAS.Count + graceRL05.Count;
            filen = 0;
        }
               

        //*START*//
        private void DrawButton_Click(object sender, EventArgs e)
        {
            Location.Enabled = false;
            GLDAS.Enabled = false;
            RL05.Enabled = false;
            GRACE.Enabled = false;
            DrawButton.Enabled = false;
            Progress.Maximum = max + max2;

            Thread thread = new Thread(ReadAllData);
            thread.IsBackground = true;
            thread.Name = "Read Data Thread";
            thread.Start();
        }
        public void ReadAllData()
        {
            if (!isRead)
            {
                readGLDAS();
                readRL05();
                readGRACE();
                isRead = true;
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

             //** EXIT **//
            SetProgress(Progress.Maximum);
            this.Invoke(new MethodInvoker(delegate
            {
                DrawButton.Enabled = true;
                SetStatus("Completed Successfully!");
                CloseForm.Enabled = true;
                GLDAS.Enabled = true;
                RL05.Enabled = true;
                GRACE.Enabled = true;
                SaveImage.Enabled = false;
                GLDAS.Checked = true;
                RL05.Checked = true;
                GRACE.Checked = true;
                Location.Enabled = true;

            }));
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
                    gldas.Add(adjustedPoint(p));
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
                    rl05.Add(adjustedPoint(p));
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
                    TimeSpan diff = start - time;
                    int t = (int)(Math.Floor(diff.TotalHours));

                    float height;
                    if()
                    if (GLDAS.Checked && !RL05.Checked)
                    {
                        foreach (string file2 in GLDASData)
                        {
                            StreamReader reader2 = new StreamReader(file);
                            while (!reader.EndOfStream)
                            {
                                string s2 = reader.ReadLine();
                                string[] parameters2 = s.Split(' ');
                                int count2 = parameters.Length;
                                height = float.Parse(parameters[9]);
                                PointF p = new PointF(t, height);
                                graceGLDAS.Add(adjustedPoint(p));
                            }
                        }
                    }
                    else if (RL05.Checked && !GLDAS.Checked)
                    {
                        foreach (string file3 in RL05Data)
                        {
                            StreamReader reader3 = new StreamReader(file);
                            while (!reader.EndOfStream)
                            {
                                string s3 = reader.ReadLine();
                                string[] parameters3 = s.Split(' ');
                                int count3 = parameters.Length;
                                height = float.Parse(parameters[9]);
                                PointF p = new PointF(t, height);
                                graceRL05.Add(adjustedPoint(p));
                            }
                        }
                    }
                    else if (GLDAS.Checked && RL05.Checked)
                    {
                        foreach (string file2 in GLDASData)
                        {
                            StreamReader reader2 = new StreamReader(file);
                            while (!reader.EndOfStream)
                            {
                                string s2 = reader.ReadLine();
                                string[] parameters2 = s.Split(' ');
                                int count2 = parameters.Length;
                                height = float.Parse(parameters[9]);
                                PointF p = new PointF(t, height);
                                graceGLDAS.Add(adjustedPoint(p));
                            }
                        }
                        foreach (string file3 in RL05Data)
                        {
                            StreamReader reader3 = new StreamReader(file);
                            while (!reader.EndOfStream)
                            {
                                string s3 = reader.ReadLine();
                                string[] parameters3 = s.Split(' ');
                                int count3 = parameters.Length;
                                height = float.Parse(parameters[9]);
                                PointF p = new PointF(t, height);
                                graceRL05.Add(adjustedPoint(p));
                            }
                        }

                    }
                    else
                    {
                        height = 0.0f;
                        PointF p = new PointF(t, height);
                        grace.Add(adjustedPoint(p));
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
            SolidBrush myBrush = new SolidBrush(Color.Green);
            Pen myPen = new Pen(Color.Green);
            for (int k = 0; k < gldas.Count - 1; k++)
            {
                g.DrawLine(myPen, gldas[k], gldas[k + 1]);
                SetStatus("Creating GLDAS Graph");
                SetProgress(filen);
            }
        }
        public void dontShowGLDAS(Graphics g)
        {
            Color a = Color.FromArgb(0, 1, 1, 1);
            SolidBrush myBrush = new SolidBrush(a);
            Pen myPen = new Pen(a);
            for (int k = 0; k < gldas.Count - 1; k++)
            {
                g.DrawLine(myPen, gldas[k], gldas[k + 1]);
                SetStatus("Creating GLDAS Graph");
                SetProgress(filen);
            }
            BringToFront();
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
        public void dontShowRL05(Graphics g)
        {
            Color b = Color.FromArgb(0, 1, 1, 1);
            SolidBrush myBrush = new SolidBrush(b);
            Pen myPen = new Pen(b);
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
            if (!GLDAS.Checked)
            {
                if (!RL05.Checked)
                {
                    foreach (PointF p in grace)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                    }
                    for (int k = 0; k < grace.Count - 1; k++)
                    {
                        g.DrawLine(myPen, grace[k], grace[k + 1]);
                    }
                }
                else
                {
                    for (int k = 0; k < graceRL05.Count - 1; k++)
                    {
                        g.DrawLine(myPen, graceRL05[k], graceRL05[k + 1]);
                    }
                    foreach (PointF p in graceRL05)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                    }
                }
               
            }
            else
            {
                if (!RL05.Checked)
                {
                    for (int k = 0; k < graceGLDAS.Count - 1; k++)
                    {
                        g.DrawLine(myPen, graceGLDAS[k], graceGLDAS[k + 1]);
                    }
                    foreach (PointF p in graceGLDAS)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                    }
                }
                else
                {
                    for (int k = 0; k < graceRL05.Count - 1; k++)
                    {
                        g.DrawLine(myPen, graceRL05[k], graceRL05[k + 1]);
                    }
                    foreach (PointF p in graceRL05)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                    }

                    for (int k = 0; k < graceGLDAS.Count - 1; k++)
                    {
                        g.DrawLine(myPen, graceGLDAS[k], graceGLDAS[k + 1]);
                    }
                    foreach (PointF p in graceGLDAS)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                    }
                }
            }

        }
        public void dontShowGRACE(Graphics g)
        {
            Color c = Color.FromArgb(0, 1, 1, 1);
            SolidBrush myBrush = new SolidBrush(c);
            Pen myPen = new Pen(c);
            if (!GLDAS.Checked)
            {
                if (!RL05.Checked)
                {
                    foreach (PointF p in grace)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                    for (int k = 0; k < grace.Count - 1; k++)
                    {
                        g.DrawLine(myPen, grace[k], grace[k + 1]);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                }
                else
                {
                    for (int k = 0; k < graceRL05.Count - 1; k++)
                    {
                        g.DrawLine(myPen, graceRL05[k], graceRL05[k + 1]);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                    foreach (PointF p in graceRL05)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                }

            }
            else
            {
                if (!RL05.Checked)
                {
                    for (int k = 0; k < graceGLDAS.Count - 1; k++)
                    {
                        g.DrawLine(myPen, graceGLDAS[k], graceGLDAS[k + 1]);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                    foreach (PointF p in graceGLDAS)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                }
                else
                {
                    for (int k = 0; k < graceRL05.Count - 1; k++)
                    {
                        g.DrawLine(myPen, graceRL05[k], graceRL05[k + 1]);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                    foreach (PointF p in graceRL05)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }

                    for (int k = 0; k < graceGLDAS.Count - 1; k++)
                    {
                        g.DrawLine(myPen, graceGLDAS[k], graceGLDAS[k + 1]);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                    foreach (PointF p in graceGLDAS)
                    {
                        g.FillRectangle(myBrush, p.X - 1, p.Y - 1, p.X + 2, p.Y + 2);
                        SetStatus("Creating GRACE Graph");
                        SetProgress(filen);
                    }
                }
            }

        }

        public PointF adjustedPoint(PointF a)
        {
            float x = (float)a.X * (float)Chart.Width / 8764.0f;
            float y = ((((float)a.Y * (float)Chart.Height / 6.0f))+(float)Chart.Height/2.0f); 
            return new PointF(x, y);
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
    }
}
