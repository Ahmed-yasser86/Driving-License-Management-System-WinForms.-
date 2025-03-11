using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Buisness.clsTestType;

namespace DVLD.Tests.SchadualTest
{
    public partial class FrmListAppoinments: Form
    {


        int LDLApplicationID;
        DataTable dtListTestAppointment;
        clsTestType.enTestType testType;


        private void _LoadTestTypeImageAndTitle()
        {
            switch (testType)
            {

                case clsTestType.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }

                case clsTestType.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }
        }








        public FrmListAppoinments(int LDLApplicationID, clsTestType.enTestType testType)
        {


            this.LDLApplicationID = LDLApplicationID;
            this.testType = testType;
            InitializeComponent();
        }

        private void FrmListAppoinments_Load(object sender, EventArgs e)
        {

            dataGridView1.ContextMenuStrip = contextMenuStrip1;

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(LDLApplicationID);

            dtListTestAppointment = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(LDLApplicationID, testType);
            dataGridView1.DataSource = dtListTestAppointment;
    }

        private void button1_Click(object sender, EventArgs e)
        {

            frmSchadualTest frm = new frmSchadualTest(LDLApplicationID, testType);
            frm.Show();


            FrmListAppoinments_Load(null, null);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            int TestAppointment = clsTestAppointment.GetLastTestAppointment(LDLApplicationID,testType).TestAppointmentID;
            frmTakeTest frm = new frmTakeTest(TestAppointment, testType);
            frm.ShowDialog();
            FrmListAppoinments_Load(null, null);
        }


      

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmSchadualTest frm = new frmSchadualTest(LDLApplicationID, testType, TestAppointmentID);
            frm.ShowDialog();
            FrmListAppoinments_Load(null, null);
        }
    }
}
