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

namespace GRACEChart
{
    public partial class MainChart : BaseForm
    {   
        String[] GLDASData = System.IO.Directory.GetFiles("../../../../gracedata/modeltimeseries/GLDAS");
        String[] RL05Data = System.IO.Directory.GetFiles("../../../../gracedata/modeltimeseries/RL05");

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

        public void showGLDAS(Graphics g)
        {
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
    }
}
