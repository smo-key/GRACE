namespace GRACEMap
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
            this.Border.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.Size = new System.Drawing.Size(656, 412);
            // 
            // TopPanel
            // 
            this.TopPanel.Size = new System.Drawing.Size(656, 30);
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
            this.CloseForm.Location = new System.Drawing.Point(628, 3);
            this.ToolTips.SetToolTip(this.CloseForm, "Close");
            // 
            // Minimize
            // 
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.Location = new System.Drawing.Point(604, 3);
            // 
            // IconBox
            // 
            this.IconBox.Image = global::GRACEMap.Properties.Resources.ASCube_16xLG;
            // 
            // MapMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 412);
            this.Name = "MapMain";
            this.Text = "FindNXT";
            this.title = "GRACE Frequency Map";
            this.Border.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

    }
}