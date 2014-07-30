using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GRACEdata;
using System.IO.Pipes;
using System.IO;
namespace GRACEChart
{
    public partial class MainChart : BaseForm
    {   
        String[] GLDASData = System.IO.Directory.GetFiles("../../../../../gracedata/modeltimeseries/GLDAS");
        String[] RL05Data = System.IO.Directory.GetFiles("../../../../../gracedata/modeltimeseries/RL05");
        String[] GRACEData = System.IO.Directory.GetFiles("../../../../../gracedata/groundtrack");

   
        public MainChart()
        {
            InitializeComponent();
            Location.SelectedItem = "ACC";
            GLDAS.Enabled = true;
            RL05.Enabled = true;
            GRACE.Enabled = true;
            SaveImage.Enabled = false;
            Graphics g = this.CreateGraphics();
            if(GLDAS.Checked)
            {
                showGLDAS(g);
            }

            if(RL05.Checked)
            {
                showRL05(g);
            }

            if (GRACE.Checked)
            {
                showGRACE(g);
            }
        }
        //*SHOW GRAPHS*//
        public void showGLDAS(Graphics g)
        {
            //Color a = Color.FromArgb(a, r, g, b);
            System.Drawing.Color a = Color.Green;  
        }
        public void showRL05(Graphics g)
        {
            System.Drawing.Color b = Color.DarkBlue;
        }
        public void showGRACE(Graphics g)
        {
            System.Drawing.Color c = Color.Crimson;
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
        private void GLDAS_CheckedChanged(object sender, EventArgs e)
        {

        }
        
        //*READ GLDAS DATA*// 
        List<Point> gldas = new List<Point>();
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

                    double t = Convert.ToDouble(parameters[2]);
                    DateTime time = new DateTime(0, DateTimeKind.Utc);
                    time = time.AddYears(2007);
                    time = time.AddHours(t);
                    int ti = Convert.ToInt32(time);
                    int height = (int)Convert.ToDouble(parameters[4]);
                    Point p = new Point(ti, height);
                    gldas.Add(p);
                }
            } 
        }

        
        //*READ RL-05 DATA*//
        List<Point> rl05 = new List<Point>();
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

                    double t = Convert.ToDouble(parameters[2]);
                    DateTime time = new DateTime(0, DateTimeKind.Utc);
                    time = time.AddYears(2007);
                    time = time.AddHours(t);
                    int ti = Convert.ToInt32(time);
                    int height = (int)Convert.ToDouble(parameters[4]);
                    Point p = new Point(ti, height);
                    rl05.Add(p);
                }
            }
        }
        //*READ GRACE*//
        List<Point> grace = new List<Point>();
        List<Point> graceGLDAS = new List<Point>();
        List<Point> graceRL05 = new List<Point>();
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

                     double t = Convert.ToDouble(parameters[0]);
                     DateTime time = new DateTime(0, DateTimeKind.Utc);
                     time = time.AddYears(1999);
                     time = time.AddHours(t);
                     int ti = Convert.ToInt32(time);

                     int height = 0;

                     if (GLDAS.Checked)
                     {
                         foreach (string file2 in GLDASData)
                         {
                             StreamReader reader2 = new StreamReader(file);
                             while (!reader.EndOfStream)
                             {
                                 string s2 = reader.ReadLine();
                                 string[] parameters2 = s.Split(' ');
                                 int count2 = parameters.Length;
                                 height = (int)Convert.ToDouble(parameters[4]);
                                 Point p = new Point(ti, height);
                                 graceGLDAS.Add(p);
                             }
                         }
                     }
                     else if (RL05.Checked)
                     {
                         foreach (string file3 in RL05Data)
                         {
                             StreamReader reader3 = new StreamReader(file);
                             while (!reader.EndOfStream)
                             {
                                 string s3 = reader.ReadLine();
                                 string[] parameters3 = s.Split(' ');
                                 int count3 = parameters.Length;
                                 height = (int)Convert.ToDouble(parameters[4]);
                                 Point p = new Point(ti, height);
                                 graceRL05.Add(p);
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
                                 height = (int)Convert.ToDouble(parameters[4]);
                                 Point p = new Point(ti, height);
                                 graceGLDAS.Add(p);
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
                                 height = (int)Convert.ToDouble(parameters[4]);
                                 Point p = new Point(ti, height);
                                 graceRL05.Add(p);
                             }
                         }
                         
                     }
                     else
                     {
                         height = 0;                     
                         Point p = new Point(ti, height);
                         grace.Add(p);
                     }

                }
            }
        }

    }
}
