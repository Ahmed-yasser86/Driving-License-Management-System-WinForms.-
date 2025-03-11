namespace DVLD.Tests.SchadualTest
{
    partial class frmSchadualTest
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
            this.ctrlScheduleTest1 = new DVLD.Tests.ctrlScheduleTest();
            this.SuspendLayout();
            // 
            // ctrlScheduleTest1
            // 
            this.ctrlScheduleTest1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlScheduleTest1.LDLAppID = 0;
            this.ctrlScheduleTest1.Location = new System.Drawing.Point(2, 13);
            this.ctrlScheduleTest1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlScheduleTest1.Name = "ctrlScheduleTest1";
            this.ctrlScheduleTest1.RetakeTestID = 0;
            this.ctrlScheduleTest1.Size = new System.Drawing.Size(533, 722);
            this.ctrlScheduleTest1.TabIndex = 0;
            this.ctrlScheduleTest1.TestAppointmentID = 0;
            this.ctrlScheduleTest1.TestID = 0;
            this.ctrlScheduleTest1.TestTypeID = DVLD_Buisness.clsTestType.enTestType.VisionTest;
            this.ctrlScheduleTest1.Load += new System.EventHandler(this.ctrlScheduleTest1_Load);
            // 
            // frmSchadualTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 749);
            this.Controls.Add(this.ctrlScheduleTest1);
            this.Name = "frmSchadualTest";
            this.Text = "frmSchadualTest";
            this.Load += new System.EventHandler(this.frmSchadualTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlScheduleTest ctrlScheduleTest1;
    }
}