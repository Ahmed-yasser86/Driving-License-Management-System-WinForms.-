using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Properties;
using DVLD_Buisness;
using DVLD.Classes;
using System.CodeDom;

namespace DVLD.Tests
{
    public partial class ctrlSecheduledTest: UserControl
    {

        clsTestAppointment TestAppointment;

        clsTestType.enTestType _TestTypeID;

        clsLocalDrivingLicenseApplication LocalDrivingLicense;

        public int TestAppointmentID { set; get; }




        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {

                    case clsTestType.enTestType.VisionTest:
                        {
                            gbTestType.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }

                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbTestType.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            gbTestType.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;


                        }
                }
            }
        }

        public void LoadInfo(int TestAppointmentID)
        {
            TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            if (TestAppointment == null)
            {
                MessageBox.Show("We are Sorry Can not Find Test Appointment , an error occured !");

                return;
            }


                this.TestAppointmentID = TestAppointmentID;
             
            
            LocalDrivingLicense = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(TestAppointment.LocalDrivingLicenseApplicationID);
            if (TestAppointment == null)
            {
                MessageBox.Show("we are sorry we couldn't find local driving license application");
                return;
            }


            TestTypeID = TestAppointment.TestTypeID;
           
           
                



            lblLocalDrivingLicenseAppID.Text = LocalDrivingLicense.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = LocalDrivingLicense.LicenseClassInfo.ClassName;
            lblFullName.Text = LocalDrivingLicense.ApplicantFullName.ToString();
            lblTrial.Text = LocalDrivingLicense.TotalTrialsPerTest(TestTypeID).ToString();
            lblDate.Text = TestAppointment.AppointmentDate.ToString();
            lblFees.Text = TestAppointment.PaidFees.ToString();
            if(TestAppointment.TestID != -1)
            {
                lblTestID.Text = TestAppointment.TestID.ToString();
            }
            else
            {
                lblTestID.Text = "Not Taken Yet";
            }


                

            
            


        }

        public ctrlSecheduledTest()
        {
            InitializeComponent();
        }

        private void gbTestType_Enter(object sender, EventArgs e)
        {

        }
    }
}
