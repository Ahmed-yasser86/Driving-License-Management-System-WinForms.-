namespace DVLD.Licenses.Local_Licenses
{
    partial class showLicenseInfo
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
            this.ctrDriverLicenseInfoe1 = new DVLD.Drivers.Controllls.ctrDriverLicenseInfoe();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ctrDriverLicenseInfoe1
            // 
            this.ctrDriverLicenseInfoe1.Location = new System.Drawing.Point(6, 98);
            this.ctrDriverLicenseInfoe1.Name = "ctrDriverLicenseInfoe1";
            this.ctrDriverLicenseInfoe1.Size = new System.Drawing.Size(867, 342);
            this.ctrDriverLicenseInfoe1.TabIndex = 0;
            this.ctrDriverLicenseInfoe1.Load += new System.EventHandler(this.ctrDriverLicenseInfoe1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(298, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Driver License Info";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(767, 446);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 41);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // showLicenseInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 499);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctrDriverLicenseInfoe1);
            this.Name = "showLicenseInfo";
            this.Text = "showLicenseInfo";
            this.Load += new System.EventHandler(this.showLicenseInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Drivers.Controllls.ctrDriverLicenseInfoe ctrDriverLicenseInfoe1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}