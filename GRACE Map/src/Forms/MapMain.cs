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
    public partial class MapMain : BaseForm
    {
        private Graphics map;

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
            Progress.Value = 0;

            Thread thread = new Thread(ReadData);
            thread.IsBackground = true;
            thread.Name = "Read Data Thread";
            thread.Start();
        }

        private void ReadData()
        {
            //** CLEAR MAP **//
            map.Clear(SystemColors.Control);
            map.DrawImageUnscaled(global::GRACEMap.Properties.Resources.World_Map, 0, 0);

            //** GET FILE COUNT **//
            string[] files = Directory.GetFiles("../../../../../gracedata/", "2002-09*.latlon", SearchOption.TopDirectoryOnly);
            int filen = 0; //current file number
            Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = files.Length; }));
            SetProgress(filen);

            //** SET SETTINGS **//
            Globals.gridsize = (double)this.gridsize.Value;
            Structs.PointTime lasttime = new Structs.PointTime();
            Structs.Anchor anchor = Structs.Anchor.Uniform;
            if (!(360 % Globals.gridsize == 0)) { anchor = Structs.Anchor.Center; }

            //** INITIALIZE BIN COUNTS **//
            int[,] bins = new int[Structs.CoercedBin.BinsLon, Structs.CoercedBin.BinsLat];
            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
                {
                    bins[i, j] = 0;
                }
            }

            //** READ ALL FILES **//
            foreach (string file in files)
            {
                SetProgress(filen);
                SetStatus(string.Format("Reading {0}...", file.ToString()));
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
            SetProgress(filen);
            SetStatus("Drawing map...");

            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
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

                    map.DrawRectangle(new Pen(Color.FromArgb(64, 64, 64, 64)), rect.X, rect.Y, rect.Width, rect.Height);

                }
            }

            //** EXIT **//
            this.Invoke(new MethodInvoker(delegate 
            {
                ReadNow.Enabled = true;
                gridsize.Enabled = true;
                GridsizeText.Enabled = true;
                Status.Image = global::GRACEMap.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
                Status.Text = "       Completed successfully!";
                Status.ForeColor = Color.Green;
                Progress.Maximum = 100;
                Progress.Value = 100;
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

    }
}
