using DVLD.Classes;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Tests
{

  
    public partial class ctrlScheduleTest : UserControl
    {
       
        public  clsTestType.enTestType _TestTypeID;
        private enum enCreationMOde {CreatNew , RetakeTest }

        enCreationMOde CreationMOde = enCreationMOde.CreatNew;
        private enum _Mode { AddNew , Update}
        _Mode mode;

        clsLocalDrivingLicenseApplication LDLapplication;

        public  int LDLAppID  { set; get; }

        clsTestAppointment TestAppointment;

        public int TestAppointmentID { set; get; }

        public int RetakeTestID { set; get; }

        public int TestID { set; get; }

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




        public void LoadInfo(int LocalDrivingLicenseApplicationID,int AppointmentID=-1 )
        {

            LDLAppID = LocalDrivingLicenseApplicationID;
            clsLocalDrivingLicenseApplication LDLDrivingLicense = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);
            LDLapplication = LDLDrivingLicense;
            if(LDLDrivingLicense == null)
            {
                MessageBox.Show("error ocurred couldn't load the local driving license application");
                return;
            }
            
            if (AppointmentID == -1)
            {
                mode = _Mode.AddNew;
            }
            else
            {
                mode = _Mode.Update;
                TestAppointmentID = AppointmentID;
            }



            lblLocalDrivingLicenseAppID.Text = LDLAppID.ToString();
            lblDrivingClass.Text = clsLicenseClass.Find(LDLapplication.LicenseClassInfo.LicenseClassID).ClassName;
            lblFullName.Text = LDLapplication.ApplicantFullName;
            lblTrial.Text = LDLapplication.TotalTrialsPerTest(_TestTypeID).ToString();

            if (LDLapplication.TotalTrialsPerTest(_TestTypeID) > 0)
            {
                CreationMOde = enCreationMOde.RetakeTest;
            }
            else
            {
                CreationMOde = enCreationMOde.CreatNew;
            }



            if(CreationMOde == enCreationMOde.RetakeTest)
            {

                gbRetakeTestInfo.Enabled = true;
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                lblTitle.Text = "Schadual Retake Test";

            }
            else
            {

                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schadual  Test";

            }

            if (mode == _Mode.AddNew)
            {
                TestAppointment = new clsTestAppointment();
                lblFees.Text = clsTestType.Find(_TestTypeID).Fees.ToString();
                dtpTestDate.MinDate = DateTime.Now;


            }
            else
            {

              if(!_LoadAppointment())
                {
                    MessageBox.Show("sorry couldn't load the info please try again");
                    return;
                }

            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) ).ToString();

            if (!_HandlePrviousTestConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandleActiveTestAppointmentConstraint())
                return;

           




        }


      



       private bool _LoadAppointment()
        {

            if(mode == _Mode.Update)
            {

                TestAppointment = clsTestAppointment.Find(TestAppointmentID);


                if (TestAppointment == null)
                { 
                    btnSave.Enabled = false;
                    return false;
                }
                lblFees.Text = clsTestType.Find(_TestTypeID).Fees.ToString();

                if (DateTime.Compare(DateTime.Now, TestAppointment.AppointmentDate) > 0)
                {

                    dtpTestDate.MinDate = TestAppointment.AppointmentDate;

                }
                else
                {
                    dtpTestDate.MinDate = DateTime.Now;

                }

                dtpTestDate.Value = TestAppointment.AppointmentDate;


                if ( TestAppointment.RetakeTestAppInfo.ApplicationID !=-1 )
                {
                    gbRetakeTestInfo.Enabled = true;
                    lblTitle.Text = "Schedule Retake Test";

                    lblRetakeTestAppID.Text = TestAppointment.RetakeTestAppInfo.ApplicationID.ToString();
                    lblRetakeAppFees.Text = TestAppointment.RetakeTestAppInfo.PaidFees.ToString();

                }
                else
                {
                    lblTitle.Text = "Schedule Test";

                    gbRetakeTestInfo.Enabled = false ;
                }

                return true;

            }

            return false;

        }




        private bool _HandleAppointmentLockedConstraint() 
        {
            if (TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked.";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;

            }
            else
                lblUserMessage.Visible = false;

            return true;


        }
      
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (mode == _Mode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(LDLAppID, _TestTypeID))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }

            return true;
        

        }
        private bool _HandlePrviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannno apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    lblUserMessage.Visible = false;

                    return true;

                case clsTestType.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.
                    if (!LDLapplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }


                    return true;

                case clsTestType.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    if (!LDLapplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }


                    return true;

            }
            return true;

        }
        private bool _HandleRetakeApplication()
        {


            if( mode == _Mode.AddNew && CreationMOde == enCreationMOde.RetakeTest  )
            {

                //First Create Applicaiton 
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = LDLapplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!Application.Save())
                {
                    TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                TestAppointment.RetakeTestApplicationID = Application.ApplicationID;

            }

            return true;


        }

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!_HandleRetakeApplication())
                return;

           TestAppointment.TestTypeID = _TestTypeID;
           TestAppointment.LocalDrivingLicenseApplicationID = LDLapplication.LocalDrivingLicenseApplicationID;
           TestAppointment.AppointmentDate = dtpTestDate.Value;
           TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
           TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (TestAppointment.Save())
            {
                mode = _Mode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void gbTestType_Enter(object sender, EventArgs e)
        {

        }
    }
}
