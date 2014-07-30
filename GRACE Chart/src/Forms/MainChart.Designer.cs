namespace GRACEChart
{
    partial class MainChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ChartLib.ChartPen chartPen1 = new ChartLib.ChartPen();
            ChartLib.ChartPen chartPen2 = new ChartLib.ChartPen();
            ChartLib.ChartPen chartPen3 = new ChartLib.ChartPen();
            ChartLib.ChartPen chartPen4 = new ChartLib.ChartPen();
            this.SaveImage = new System.Windows.Forms.CheckBox();
            this.GLDAS = new System.Windows.Forms.CheckBox();
            this.RL05 = new System.Windows.Forms.CheckBox();
            this.DrawButton = new System.Windows.Forms.Button();
            this.GRACE = new System.Windows.Forms.CheckBox();
            this.LocationText = new System.Windows.Forms.Label();
            this.Location = new System.Windows.Forms.ComboBox();
            this.xAxis = new System.Windows.Forms.Label();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.Status = new System.Windows.Forms.Label();
            this.Chart = new ChartLib.Chart();
            this.yAxis = new System.Windows.Forms.Label();
            this.yAxis3 = new System.Windows.Forms.Label();
            this.yAxis2 = new System.Windows.Forms.Label();
            this.Border.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.Controls.Add(this.yAxis2);
            this.Border.Controls.Add(this.yAxis3);
            this.Border.Controls.Add(this.yAxis);
            this.Border.Controls.Add(this.BottomPanel);
            this.Border.Controls.Add(this.xAxis);
            this.Border.Controls.Add(this.Location);
            this.Border.Controls.Add(this.LocationText);
            this.Border.Controls.Add(this.GRACE);
            this.Border.Controls.Add(this.DrawButton);
            this.Border.Controls.Add(this.RL05);
            this.Border.Controls.Add(this.GLDAS);
            this.Border.Controls.Add(this.SaveImage);
            this.Border.Controls.Add(this.Chart);
            this.Border.Size = new System.Drawing.Size(803, 571);
            this.Border.Paint += new System.Windows.Forms.PaintEventHandler(this.Border_Paint);
            this.Border.Controls.SetChildIndex(this.TopPanel, 0);
            this.Border.Controls.SetChildIndex(this.IconBox, 0);
            this.Border.Controls.SetChildIndex(this.Chart, 0);
            this.Border.Controls.SetChildIndex(this.SaveImage, 0);
            this.Border.Controls.SetChildIndex(this.GLDAS, 0);
            this.Border.Controls.SetChildIndex(this.RL05, 0);
            this.Border.Controls.SetChildIndex(this.DrawButton, 0);
            this.Border.Controls.SetChildIndex(this.GRACE, 0);
            this.Border.Controls.SetChildIndex(this.LocationText, 0);
            this.Border.Controls.SetChildIndex(this.Location, 0);
            this.Border.Controls.SetChildIndex(this.xAxis, 0);
            this.Border.Controls.SetChildIndex(this.BottomPanel, 0);
            this.Border.Controls.SetChildIndex(this.yAxis, 0);
            this.Border.Controls.SetChildIndex(this.yAxis3, 0);
            this.Border.Controls.SetChildIndex(this.yAxis2, 0);
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(803, 30);
            // 
            // Title
            // 
            this.Title.Location = new System.Drawing.Point(258, 6);
            this.Title.Size = new System.Drawing.Size(287, 17);
            this.Title.Text = "GRACE Frequency Chart Analytic Tools (GFCATS)";
            // 
            // CloseForm
            // 
            this.CloseForm.FlatAppearance.BorderSize = 0;
            this.CloseForm.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseForm.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.CloseForm.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseForm.Location = new System.Drawing.Point(775, 3);
            this.ToolTips.SetToolTip(this.CloseForm, "Close");
            // 
            // Minimize
            // 
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.Location = new System.Drawing.Point(751, 3);
            // 
            // SaveImage
            // 
            this.SaveImage.AutoSize = true;
            this.SaveImage.Location = new System.Drawing.Point(708, 181);
            this.SaveImage.Name = "SaveImage";
            this.SaveImage.Size = new System.Drawing.Size(83, 17);
            this.SaveImage.TabIndex = 50;
            this.SaveImage.Text = "Save Image";
            this.SaveImage.UseVisualStyleBackColor = true;
            // 
            // GLDAS
            // 
            this.GLDAS.AutoSize = true;
            this.GLDAS.ForeColor = System.Drawing.Color.Green;
            this.GLDAS.Location = new System.Drawing.Point(706, 82);
            this.GLDAS.Name = "GLDAS";
            this.GLDAS.Size = new System.Drawing.Size(92, 17);
            this.GLDAS.TabIndex = 52;
            this.GLDAS.Text = "Show GLDAS";
            this.GLDAS.UseVisualStyleBackColor = true;
            this.GLDAS.CheckedChanged += new System.EventHandler(this.GLDAS_CheckedChanged);
            // 
            // RL05
            // 
            this.RL05.AutoSize = true;
            this.RL05.ForeColor = System.Drawing.Color.DarkBlue;
            this.RL05.Location = new System.Drawing.Point(706, 105);
            this.RL05.Name = "RL05";
            this.RL05.Size = new System.Drawing.Size(85, 17);
            this.RL05.TabIndex = 53;
            this.RL05.Text = "Show RL-05";
            this.RL05.UseVisualStyleBackColor = true;
            // 
            // DrawButton
            // 
            this.DrawButton.AutoEllipsis = true;
            this.DrawButton.BackColor = System.Drawing.Color.Transparent;
            this.DrawButton.FlatAppearance.BorderSize = 0;
            this.DrawButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.DrawButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.DrawButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.DrawButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DrawButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DrawButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DrawButton.Image = global::GRACEChart.Properties.Resources.StatusAnnotations_Play_32xLG_color;
            this.DrawButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.DrawButton.Location = new System.Drawing.Point(11, 35);
            this.DrawButton.Name = "DrawButton";
            this.DrawButton.Size = new System.Drawing.Size(158, 41);
            this.DrawButton.TabIndex = 54;
            this.DrawButton.TabStop = false;
            this.DrawButton.Text = "  Draw Graphs";
            this.DrawButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DrawButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DrawButton.UseVisualStyleBackColor = false;
            // 
            // GRACE
            // 
            this.GRACE.AutoSize = true;
            this.GRACE.ForeColor = System.Drawing.Color.Crimson;
            this.GRACE.Location = new System.Drawing.Point(706, 128);
            this.GRACE.Name = "GRACE";
            this.GRACE.Size = new System.Drawing.Size(93, 17);
            this.GRACE.TabIndex = 55;
            this.GRACE.Text = "Show GRACE";
            this.GRACE.UseVisualStyleBackColor = true;
            // 
            // LocationText
            // 
            this.LocationText.AutoSize = true;
            this.LocationText.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LocationText.Location = new System.Drawing.Point(498, 35);
            this.LocationText.Name = "LocationText";
            this.LocationText.Size = new System.Drawing.Size(48, 13);
            this.LocationText.TabIndex = 58;
            this.LocationText.Text = "Location";
            // 
            // Location
            // 
            this.Location.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Location.FormattingEnabled = true;
            this.Location.Items.AddRange(new object[] {
            "ACC",
            "Amazon",
            "Antarctic",
            "Baltic Sea",
            "Bangladesh",
            "Balck Sea",
            "Columbia",
            "Congo",
            "East siberia",
            "Guinea",
            "Gulf Carpentaria",
            "Hudson Bay",
            "Mediterranean Sea",
            "Mekong",
            "NCP",
            "Ob",
            "Orinoco",
            "Pearl River",
            "Sao Paulo",
            "StLNewFL",
            "Victoria"});
            this.Location.Location = new System.Drawing.Point(501, 55);
            this.Location.Name = "Location";
            this.Location.Size = new System.Drawing.Size(144, 21);
            this.Location.TabIndex = 59;
            // 
            // xAxis
            // 
            this.xAxis.AutoSize = true;
            this.xAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.xAxis.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.xAxis.Location = new System.Drawing.Point(351, 522);
            this.xAxis.Name = "xAxis";
            this.xAxis.Size = new System.Drawing.Size(66, 17);
            this.xAxis.TabIndex = 60;
            this.xAxis.Text = "Time (hr)";
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.Progress);
            this.BottomPanel.Controls.Add(this.Status);
            this.BottomPanel.Location = new System.Drawing.Point(0, 542);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(802, 26);
            this.BottomPanel.TabIndex = 61;
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(597, 4);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(200, 18);
            this.Progress.TabIndex = 36;
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.BackColor = System.Drawing.Color.Transparent;
            this.Status.ForeColor = System.Drawing.Color.Green;
            this.Status.Image = global::GRACEChart.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
            this.Status.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Status.Location = new System.Drawing.Point(3, 6);
            this.Status.MinimumSize = new System.Drawing.Size(0, 14);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(191, 14);
            this.Status.TabIndex = 35;
            this.Status.Text = "       Ready To Read Time Series Data!";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Chart
            // 
            this.Chart.AbsoluteMaxValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.Chart.AbsoluteMinValue = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.Chart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Chart.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.Chart.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.Chart.Location = new System.Drawing.Point(90, 82);
            this.Chart.Name = "Chart";
            this.Chart.PerfChartStyle.AntiAliasing = true;
            chartPen1.Color = System.Drawing.Color.Black;
            chartPen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen1.Width = 1F;
            this.Chart.PerfChartStyle.AvgLinePen = chartPen1;
            this.Chart.PerfChartStyle.BackgroundColorBottom = System.Drawing.Color.WhiteSmoke;
            this.Chart.PerfChartStyle.BackgroundColorTop = System.Drawing.Color.WhiteSmoke;
            chartPen2.Color = System.Drawing.Color.Black;
            chartPen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen2.Width = 1F;
            this.Chart.PerfChartStyle.ChartLinePen = chartPen2;
            chartPen3.Color = System.Drawing.Color.Silver;
            chartPen3.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen3.Width = 1F;
            this.Chart.PerfChartStyle.HorizontalGridPen = chartPen3;
            this.Chart.PerfChartStyle.ShowAverageLine = true;
            this.Chart.PerfChartStyle.ShowHorizontalGridLines = true;
            this.Chart.PerfChartStyle.ShowVerticalGridLines = true;
            chartPen4.Color = System.Drawing.Color.Silver;
            chartPen4.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen4.Width = 1F;
            this.Chart.PerfChartStyle.VerticalGridPen = chartPen4;
            this.Chart.ScaleMode = ChartLib.ScaleMode.Absolute;
            this.Chart.showunit = false;
            this.Chart.Size = new System.Drawing.Size(610, 419);
            this.Chart.TabIndex = 25;
            this.Chart.TimerInterval = 100;
            this.Chart.TimerMode = ChartLib.TimerMode.Disabled;
            this.Chart.unit = " cm";
            // 
            // yAxis
            // 
            this.yAxis.AutoSize = true;
            this.yAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.yAxis.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.yAxis.Location = new System.Drawing.Point(11, 273);
            this.yAxis.Name = "yAxis";
            this.yAxis.Size = new System.Drawing.Size(46, 17);
            this.yAxis.TabIndex = 62;
            this.yAxis.Text = "Water";
            this.yAxis.Click += new System.EventHandler(this.yAxis_Click);
            // 
            // yAxis3
            // 
            this.yAxis3.AutoSize = true;
            this.yAxis3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.yAxis3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.yAxis3.Location = new System.Drawing.Point(11, 307);
            this.yAxis3.Name = "yAxis3";
            this.yAxis3.Size = new System.Drawing.Size(36, 17);
            this.yAxis3.TabIndex = 63;
            this.yAxis3.Text = "(cm)";
            // 
            // yAxis2
            // 
            this.yAxis2.AutoSize = true;
            this.yAxis2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.yAxis2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.yAxis2.Location = new System.Drawing.Point(11, 290);
            this.yAxis2.Name = "yAxis2";
            this.yAxis2.Size = new System.Drawing.Size(49, 17);
            this.yAxis2.TabIndex = 64;
            this.yAxis2.Text = "Height";
            // 
            // MainChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 571);
            this.Name = "MainChart";
            this.Text = "MainChart";
            this.title = "GRACE Frequency Chart Analytic Tools (GFCATS)";
            this.Border.ResumeLayout(false);
            this.Border.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.BottomPanel.ResumeLayout(false);
            this.BottomPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ChartLib.Chart Chart;
        private System.Windows.Forms.CheckBox RL05;
        private System.Windows.Forms.CheckBox GLDAS;
        private System.Windows.Forms.CheckBox SaveImage;
        protected System.Windows.Forms.Button DrawButton;
        private System.Windows.Forms.CheckBox GRACE;
        private System.Windows.Forms.Label LocationText;
        private System.Windows.Forms.ComboBox Location;
        private System.Windows.Forms.Label xAxis;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Label yAxis;
        private System.Windows.Forms.Label yAxis2;
        private System.Windows.Forms.Label yAxis3;
    }
}