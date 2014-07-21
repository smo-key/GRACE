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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapMain));
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.Status = new System.Windows.Forms.Label();
            this.OverMap = new System.Windows.Forms.PictureBox();
            this.ReadData = new System.Windows.Forms.Button();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.gridsize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
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
            this.Border.Controls.Add(this.label1);
            this.Border.Controls.Add(this.gridsize);
            this.Border.Controls.Add(this.ReadData);
            this.Border.Controls.Add(this.OverMap);
            this.Border.Controls.Add(this.BottomPanel);
            this.Border.Size = new System.Drawing.Size(800, 511);
            this.Border.Controls.SetChildIndex(this.TopPanel, 0);
            this.Border.Controls.SetChildIndex(this.IconBox, 0);
            this.Border.Controls.SetChildIndex(this.BottomPanel, 0);
            this.Border.Controls.SetChildIndex(this.OverMap, 0);
            this.Border.Controls.SetChildIndex(this.ReadData, 0);
            this.Border.Controls.SetChildIndex(this.gridsize, 0);
            this.Border.Controls.SetChildIndex(this.label1, 0);
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(800, 30);
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
            this.CloseForm.Location = new System.Drawing.Point(772, 3);
            this.ToolTips.SetToolTip(this.CloseForm, "Close");
            // 
            // Minimize
            // 
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.Location = new System.Drawing.Point(748, 3);
            // 
            // IconBox
            // 
            this.IconBox.Image = global::GRACEMap.Properties.Resources.ASCube_16xLG;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.Progress);
            this.BottomPanel.Controls.Add(this.Status);
            this.BottomPanel.Location = new System.Drawing.Point(-1, 484);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(802, 26);
            this.BottomPanel.TabIndex = 37;
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
            this.Status.Text = "       Ready to read GRACE groundtrack data.";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OverMap
            // 
            this.OverMap.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OverMap.BackgroundImage")));
            this.OverMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OverMap.Location = new System.Drawing.Point(-1, 85);
            this.OverMap.Name = "OverMap";
            this.OverMap.Size = new System.Drawing.Size(800, 400);
            this.OverMap.TabIndex = 38;
            this.OverMap.TabStop = false;
            // 
            // ReadData
            // 
            this.ReadData.AutoEllipsis = true;
            this.ReadData.BackColor = System.Drawing.Color.Transparent;
            this.ReadData.FlatAppearance.BorderSize = 0;
            this.ReadData.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ReadData.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.ReadData.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ReadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReadData.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ReadData.Image = global::GRACEMap.Properties.Resources.StatusAnnotations_Play_32xLG_color;
            this.ReadData.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ReadData.Location = new System.Drawing.Point(6, 36);
            this.ReadData.Name = "ReadData";
            this.ReadData.Size = new System.Drawing.Size(245, 41);
            this.ReadData.TabIndex = 40;
            this.ReadData.TabStop = false;
            this.ReadData.Text = " Read GRACE Data";
            this.ReadData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ReadData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ReadData.UseVisualStyleBackColor = false;
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(595, 4);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(200, 18);
            this.Progress.TabIndex = 36;
            // 
            // gridsize
            // 
            this.gridsize.DecimalPlaces = 2;
            this.gridsize.Location = new System.Drawing.Point(711, 39);
            this.gridsize.Maximum = new decimal(new int[] {
            89,
            0,
            0,
            0});
            this.gridsize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.gridsize.Name = "gridsize";
            this.gridsize.Size = new System.Drawing.Size(76, 20);
            this.gridsize.TabIndex = 41;
            this.gridsize.TabStop = false;
            this.gridsize.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(710, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "degree Gridsize";
            // 
            // MapMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 511);
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
        protected System.Windows.Forms.Button ReadData;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.NumericUpDown gridsize;
        private System.Windows.Forms.Label label1;

    }
}