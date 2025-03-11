using DVLD.Classes;
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
using static DVLD_Buisness.clsLicense;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications.Local_Driving_License
{
    public partial class FrmIssueLicenseForTheFirstTime: Form
    {


        clsLocalDrivingLicenseApplication localDrivingLicenseApplication;
        private int _LocalDrivingLicensesApplicationID;
        private string _Notes;
        private  clsLicense _License;
        public FrmIssueLicenseForTheFirstTime(int LocalDringLicenseAppID)
        {
            _LocalDrivingLicensesApplicationID = LocalDringLicenseAppID;
            InitializeComponent();
        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FrmIssueLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {

            if (localDrivingLicenseApplication == null)
            {
                MessageBox.Show("Can not Find local driving liscense");
                return;
                    
            }
            if (localDrivingLicenseApplication.IsLicenseIssued()) {
                MessageBox.Show("sorry we can not preform this operation u have an exstisting license");
                return;
            };
           if(!localDrivingLicenseApplication.PassedAllTests())
            {
                MessageBox.Show("sorry we can not preform this operation u have not passed all the tests");
                return;
            }






            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicensesApplicationID);


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseID = localDrivingLicenseApplication.IssueLicenseForTheFirtTime(richTextBox1.Text.Trim(), clsGlobal.CurrentUser.UserID);
            if (LicenseID == -1)
            {
                MessageBox.Show("Sorry an error has occured we couldn't  add new license");
                return;
            }


            MessageBox.Show("License Issued Succsesfully");
            return;


        }


        private void ctrlDrivingLicenseApplicationInfo1_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
