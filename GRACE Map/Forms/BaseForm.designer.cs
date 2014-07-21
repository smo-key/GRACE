namespace GRACEMap
{
    partial class BaseForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.Border = new System.Windows.Forms.Panel();
            this.IconBox = new System.Windows.Forms.PictureBox();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.Minimize = new System.Windows.Forms.Button();
            this.CloseForm = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.ToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.Border.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.TopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Border
            // 
            this.Border.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Border.Controls.Add(this.IconBox);
            this.Border.Controls.Add(this.TopPanel);
            this.Border.Location = new System.Drawing.Point(0, 0);
            this.Border.Name = "Border";
            this.Border.Size = new System.Drawing.Size(300, 400);
            this.Border.TabIndex = 25;
            // 
            // IconBox
            // 
            this.IconBox.BackColor = System.Drawing.Color.Transparent;
            this.IconBox.Image = global::GRACEMap.Properties.Resources.StatusAnnotations_Play_16xLG;
            this.IconBox.Location = new System.Drawing.Point(6, 6);
            this.IconBox.Name = "IconBox";
            this.IconBox.Size = new System.Drawing.Size(16, 16);
            this.IconBox.TabIndex = 24;
            this.IconBox.TabStop = false;
            this.IconBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.IconBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.SystemColors.Control;
            this.TopPanel.Controls.Add(this.Minimize);
            this.TopPanel.Controls.Add(this.CloseForm);
            this.TopPanel.Controls.Add(this.Title);
            this.TopPanel.Location = new System.Drawing.Point(-1, -1);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(300, 30);
            this.TopPanel.TabIndex = 23;
            this.TopPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.TopPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            // 
            // Minimize
            // 
            this.Minimize.AutoEllipsis = true;
            this.Minimize.BackColor = System.Drawing.Color.Transparent;
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Minimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Minimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Minimize.Image = global::GRACEMap.Properties.Resources.type_16xLG;
            this.Minimize.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Minimize.Location = new System.Drawing.Point(242, 3);
            this.Minimize.Name = "Minimize";
            this.Minimize.Size = new System.Drawing.Size(24, 24);
            this.Minimize.TabIndex = 24;
            this.Minimize.TabStop = false;
            this.Minimize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ToolTips.SetToolTip(this.Minimize, "Minimize");
            this.Minimize.UseVisualStyleBackColor = false;
            this.Minimize.Click += new System.EventHandler(this.Minimize_Click);
            this.Minimize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.Minimize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // CloseForm
            // 
            this.CloseForm.AutoEllipsis = true;
            this.CloseForm.BackColor = System.Drawing.Color.Transparent;
            this.CloseForm.FlatAppearance.BorderSize = 0;
            this.CloseForm.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseForm.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.CloseForm.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseForm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CloseForm.Image = global::GRACEMap.Properties.Resources.Symbols_Critical_16xLG;
            this.CloseForm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CloseForm.Location = new System.Drawing.Point(271, 3);
            this.CloseForm.Name = "CloseForm";
            this.CloseForm.Size = new System.Drawing.Size(24, 24);
            this.CloseForm.TabIndex = 23;
            this.CloseForm.TabStop = false;
            this.CloseForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ToolTips.SetToolTip(this.CloseForm, "Close");
            this.CloseForm.UseVisualStyleBackColor = false;
            this.CloseForm.Click += new System.EventHandler(this.CloseForm_Click);
            this.CloseForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_MouseDown);
            this.CloseForm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_MouseUp);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Title.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Title.Location = new System.Drawing.Point(26, 6);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(32, 17);
            this.Title.TabIndex = 6;
            this.Title.Text = "Title";
            this.Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
            // 
            // ToolTips
            // 
            this.ToolTips.BackColor = System.Drawing.SystemColors.Control;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 400);
            this.Controls.Add(this.Border);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.PopupBase_Activated);
            this.Deactivate += new System.EventHandler(this.PopupBase_Deactivate);
            this.Resize += new System.EventHandler(this.Control_Resize);
            this.Border.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel Border;
        protected System.Windows.Forms.Panel TopPanel;
        protected System.Windows.Forms.Label Title;
        protected System.Windows.Forms.Button CloseForm;
        protected System.Windows.Forms.ToolTip ToolTips;
        protected System.Windows.Forms.Button Minimize;
        protected System.Windows.Forms.PictureBox IconBox;


    }
}
