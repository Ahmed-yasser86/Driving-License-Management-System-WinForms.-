using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.RealesDetainedLicense
{
    public partial class frmReleaseDetainedLicense: Form
    {
        private int _LisenceID = -1;
        clsDetainedLicense _DetainedLisence;
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }


        public frmReleaseDetainedLicense(int ID)
        {
            InitializeComponent();
            ctrDriverLicenseInfoWithFiltere1.LoadLicenseInfo(ID);
            _LisenceID = ID;
            ctrDriverLicenseInfoWithFiltere1.FilterEnabled = false;

        }


        private void ctrDriverLicenseInfoWithFiltere1_OnLicenseSelected(int obj)
        {


            _LisenceID = obj;

            if ( clsDetainedLicense.IsLicenseDetained(_LisenceID) ) {

                _DetainedLisence = clsDetainedLicense.FindByLicenseID(_LisenceID);

                btnRelease.Enabled = true;


                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.ToString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

                lblDetainID.Text = _DetainedLisence.DetainID.ToString();
                lblLicenseID.Text = _LisenceID.ToString();

                lblCreatedByUser.Text = _DetainedLisence.CreatedByUserInfo.UserName;
                lblDetainDate.Text = clsFormat.DateToShort(_DetainedLisence.DetainDate);
                lblFineFees.Text = _DetainedLisence.FineFees.ToString();
                lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblFineFees.Text)).ToString();


            }
            else
            {
                MessageBox.Show("license is not detained at all");
                return;
            }



        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {

            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = clsLicense.GetLicenseInfo(_LisenceID).DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.LastStatusDate = Application.LastStatusDate;
            Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees;


            if (Application.Save())
            {
                _DetainedLisence.ReleaseApplicationID = Application.ApplicationID;
                _DetainedLisence.ReleasedByUserID = clsGlobal.CurrentUser.UserID;
                if (_DetainedLisence.Save())
                {
                    MessageBox.Show("the licenses has detained");




                    ctrDriverLicenseInfoWithFiltere1.FilterEnabled = false;
                    btnRelease.Enabled = false;
                    llShowLicenseInfo.Enabled = true;
                    return;
                }

            }

            MessageBox.Show("An error occured couldn't detain licesns");


        }

        private void ctrDriverLicenseInfoWithFiltere1_Load(object sender, EventArgs e)
        {

        }
    }
}
