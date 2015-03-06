namespace RegulatedNoise
{
    partial class SplashScreenForm
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
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblRegulatedNoise = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.Location = new System.Drawing.Point(49, 148);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(551, 21);
            this.lblSubtitle.TabIndex = 5;
            this.lblSubtitle.Text = "-=- Built-in OCR -=- Price Analysis -=- Commander\'s Log -=- Web Control -=-";
            // 
            // lblRegulatedNoise
            // 
            this.lblRegulatedNoise.AutoSize = true;
            this.lblRegulatedNoise.Font = new System.Drawing.Font("Segoe UI", 60F, System.Drawing.FontStyle.Bold);
            this.lblRegulatedNoise.Location = new System.Drawing.Point(19, 39);
            this.lblRegulatedNoise.Name = "lblRegulatedNoise";
            this.lblRegulatedNoise.Size = new System.Drawing.Size(641, 106);
            this.lblRegulatedNoise.TabIndex = 4;
            this.lblRegulatedNoise.Text = "RegulatedNoise";
            // 
            // SplashScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 313);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblRegulatedNoise);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(681, 313);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(681, 313);
            this.Name = "SplashScreenForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblRegulatedNoise;
    }
}