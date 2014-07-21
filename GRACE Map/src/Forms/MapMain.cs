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
            Progress.Value = 0;

            Thread thread = new Thread(ReadData);
            thread.Name = "Read Data Thread";
            thread.Start();
        }

        private void ReadData()
        {
            //** GET FILE COUNT **//
            string[] files = Directory.GetFiles("../../../../../gracedata/", "2002-09*.latlon", SearchOption.TopDirectoryOnly);
            int filen = 1; //current file number
            Progress.Invoke(new MethodInvoker(delegate { Progress.Maximum = files.Length; }));

            //** INITIALIZE BIN COUNTS **//
            int[,] bins = new int[Structs.CoercedBin.BinsLon, Structs.CoercedBin.BinsLat];
            for (int i = 0; i < Structs.CoercedBin.BinsLon; i++)
            {
                for (int j = 0; j < Structs.CoercedBin.BinsLat; j++)
                {
                    bins[i, j] = 0;
                }
            }
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

    }
}
