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

namespace DVLD.Licenses.International_Driving_license
{
    public partial class frmInternationalDrivingLicense: Form
    {

       private int _LicesnesID;

        clsLicense _Licesnes;
        clsInternationalLicense InternationalLicense;
       private int _InternationalLicenseID;
        public frmInternationalDrivingLicense()
        {
            InitializeComponent();
        }

        private void frmInternationalDrivingLicense_Load(object sender, EventArgs e)
        {

            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

        }

        private void gpApplicationInfo_Enter(object sender, EventArgs e)
        {

        }

        private void ctrDriverLicenseInfoWithFiltere1_OnLicenseSelected(int obj)
        {
            _LicesnesID = obj;
            clsLicense license = clsLicense.GetLicenseInfo(_LicesnesID);
            if (clsLicenseClass.Find(license.LicenseClass).LicenseClassID!=3){

                MessageBox.Show("sorry Your License Is Not class 3 u Can not get international license so");
                return;

            }



            if (ctrDriverLicenseInfoWithFiltere1.SelectedLicenseInfo.IsActive)
            {

            int  ActiveInternaionalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(_Licesnes.DriverID);
                if (ActiveInternaionalLicenseID !=-1) 
                
                {
                    
                    MessageBox.Show("sorry You already have a license, Can not get anthore international license so");
                    llShowLicenseInfo.Enabled = true;
                    _InternationalLicenseID = ActiveInternaionalLicenseID;
                    btnIssueLicense.Enabled = false;
                    return;
                }





            }
            else
            {
                MessageBox.Show("sorry Your License Is Not Active u Can not get international license so");
                return;
            }


            btnIssueLicense.Enabled = true;
            



        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {




            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            InternationalLicense.ApplicantPersonID = ctrDriverLicenseInfoWithFiltere1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            InternationalLicense.DriverID = ctrDriverLicenseInfoWithFiltere1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrDriverLicenseInfoWithFiltere1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);

            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueLicense.Enabled = false;
            ctrDriverLicenseInfoWithFiltere1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;




        }

        private void ctrDriverLicenseInfoWithFiltere1_Load(object sender, EventArgs e)
        {

        }
    }
}
