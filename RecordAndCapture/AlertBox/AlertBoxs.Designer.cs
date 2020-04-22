namespace AlertBox
{
    partial class AlertBoxs
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
            this.components = new System.ComponentModel.Container();
            this.eclipse = new ns1.BunifuElipse(this.components);
            this.timeShow = new System.Windows.Forms.Timer(this.components);
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // eclipse
            // 
            this.eclipse.ElipseRadius = 5;
            this.eclipse.TargetControl = this;
            // 
            // timeShow
            // 
            this.timeShow.Enabled = true;
            this.timeShow.Interval = 10;
            this.timeShow.Tick += new System.EventHandler(this.timeShow_Tick);
            // 
            // timerClose
            // 
            this.timerClose.Tick += new System.EventHandler(this.timerClose_Tick);
            // 
            // AlertBoxs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 50);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AlertBoxs";
            this.Text = "AlertBoxs";
            this.Load += new System.EventHandler(this.AlertBoxs_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ns1.BunifuElipse eclipse;
        private System.Windows.Forms.Timer timeShow;
        private System.Windows.Forms.Timer timerClose;
    }
}

