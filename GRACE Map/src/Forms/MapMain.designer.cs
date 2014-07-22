﻿namespace GRACEMap
{
    partial class MapMain
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
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.Status = new System.Windows.Forms.Label();
            this.OverMap = new System.Windows.Forms.PictureBox();
            this.ReadNow = new System.Windows.Forms.Button();
            this.gridsize = new System.Windows.Forms.NumericUpDown();
            this.GridsizeText = new System.Windows.Forms.Label();
            this.Filter = new System.Windows.Forms.TextBox();
            this.FilterText = new System.Windows.Forms.Label();
            this.Border.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.BottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OverMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridsize)).BeginInit();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.Controls.Add(this.FilterText);
            this.Border.Controls.Add(this.Filter);
            this.Border.Controls.Add(this.GridsizeText);
            this.Border.Controls.Add(this.gridsize);
            this.Border.Controls.Add(this.ReadNow);
            this.Border.Controls.Add(this.OverMap);
            this.Border.Controls.Add(this.BottomPanel);
            this.Border.Size = new System.Drawing.Size(803, 516);
            this.Border.Controls.SetChildIndex(this.TopPanel, 0);
            this.Border.Controls.SetChildIndex(this.IconBox, 0);
            this.Border.Controls.SetChildIndex(this.BottomPanel, 0);
            this.Border.Controls.SetChildIndex(this.OverMap, 0);
            this.Border.Controls.SetChildIndex(this.ReadNow, 0);
            this.Border.Controls.SetChildIndex(this.gridsize, 0);
            this.Border.Controls.SetChildIndex(this.GridsizeText, 0);
            this.Border.Controls.SetChildIndex(this.Filter, 0);
            this.Border.Controls.SetChildIndex(this.FilterText, 0);
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(803, 30);
            // 
            // Title
            // 
            this.Title.Size = new System.Drawing.Size(142, 17);
            this.Title.Text = "GRACE Frequency Map";
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
            this.Minimize.Visible = false;
            // 
            // IconBox
            // 
            this.IconBox.Image = global::GRACEMap.Properties.Resources.ASCube_16xLG;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.Progress);
            this.BottomPanel.Controls.Add(this.Status);
            this.BottomPanel.Location = new System.Drawing.Point(-1, 486);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(802, 26);
            this.BottomPanel.TabIndex = 37;
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
            this.Status.Image = global::GRACEMap.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
            this.Status.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Status.Location = new System.Drawing.Point(3, 6);
            this.Status.MinimumSize = new System.Drawing.Size(0, 14);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(222, 14);
            this.Status.TabIndex = 35;
            this.Status.Text = "       Ready to read GRACE groundtrack data!";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OverMap
            // 
            this.OverMap.BackgroundImage = global::GRACEMap.Properties.Resources.World_Map;
            this.OverMap.Location = new System.Drawing.Point(0, 85);
            this.OverMap.Name = "OverMap";
            this.OverMap.Size = new System.Drawing.Size(801, 400);
            this.OverMap.TabIndex = 38;
            this.OverMap.TabStop = false;
            this.OverMap.Paint += new System.Windows.Forms.PaintEventHandler(this.OverMap_Paint);
            // 
            // ReadNow
            // 
            this.ReadNow.AutoEllipsis = true;
            this.ReadNow.BackColor = System.Drawing.Color.Transparent;
            this.ReadNow.FlatAppearance.BorderSize = 0;
            this.ReadNow.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ReadNow.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.ReadNow.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ReadNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReadNow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadNow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ReadNow.Image = global::GRACEMap.Properties.Resources.StatusAnnotations_Play_32xLG_color;
            this.ReadNow.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ReadNow.Location = new System.Drawing.Point(6, 36);
            this.ReadNow.Name = "ReadNow";
            this.ReadNow.Size = new System.Drawing.Size(245, 41);
            this.ReadNow.TabIndex = 40;
            this.ReadNow.TabStop = false;
            this.ReadNow.Text = " Read GRACE Data";
            this.ReadNow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ReadNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ReadNow.UseVisualStyleBackColor = false;
            this.ReadNow.Click += new System.EventHandler(this.ReadData_Click);
            // 
            // gridsize
            // 
            this.gridsize.DecimalPlaces = 2;
            this.gridsize.Location = new System.Drawing.Point(698, 54);
            this.gridsize.Maximum = new decimal(new int[] {
            899,
            0,
            0,
            65536});
            this.gridsize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.gridsize.Name = "gridsize";
            this.gridsize.Size = new System.Drawing.Size(89, 20);
            this.gridsize.TabIndex = 41;
            this.gridsize.TabStop = false;
            this.gridsize.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // GridsizeText
            // 
            this.GridsizeText.AutoSize = true;
            this.GridsizeText.Location = new System.Drawing.Point(695, 36);
            this.GridsizeText.Name = "GridsizeText";
            this.GridsizeText.Size = new System.Drawing.Size(91, 13);
            this.GridsizeText.TabIndex = 42;
            this.GridsizeText.Text = "Gridsize (degrees)";
            // 
            // Filter
            // 
            this.Filter.Location = new System.Drawing.Point(540, 54);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(143, 20);
            this.Filter.TabIndex = 43;
            this.Filter.TabStop = false;
            this.Filter.Text = "2002-08";
            // 
            // FilterText
            // 
            this.FilterText.AutoSize = true;
            this.FilterText.Location = new System.Drawing.Point(537, 36);
            this.FilterText.Name = "FilterText";
            this.FilterText.Size = new System.Drawing.Size(109, 13);
            this.FilterText.TabIndex = 44;
            this.FilterText.Text = "Year-Month-Day Filter";
            // 
            // MapMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 516);
            this.Name = "MapMain";
            this.Text = "GRACE Frequency Mapper";
            this.title = "GRACE Frequency Map";
            this.Border.ResumeLayout(false);
            this.Border.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.BottomPanel.ResumeLayout(false);
            this.BottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OverMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridsize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.PictureBox OverMap;
        protected System.Windows.Forms.Button ReadNow;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.NumericUpDown gridsize;
        private System.Windows.Forms.Label GridsizeText;
        private System.Windows.Forms.Label FilterText;
        private System.Windows.Forms.TextBox Filter;

    }
}