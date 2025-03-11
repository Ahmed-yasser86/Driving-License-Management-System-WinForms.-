using DVLD.Classes;
using DVLD.DriverLicense;
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

namespace DVLD.Licenses.DetainLicenes
{
    public partial class frmDetainLicense: Form
    {

        private int _LicenseId;
        clsDetainedLicense _DetainedLicense;
        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void ctrDriverLicenseInfoWithFiltere1_Load(object sender, EventArgs e)
        {




        }

        private void ctrDriverLicenseInfoWithFiltere1_OnLicenseSelected(int obj)
        {
            _LicenseId = obj;


            _DetainedLicense = clsDetainedLicense.FindByLicenseID(_LicenseId);
            clsLicense licenses = clsLicense.GetLicenseInfo(_LicenseId);

            if (!licenses.IsActive) {

                MessageBox.Show("License Is Not Active u Can not Detain It");
                return;
            
            }

            if(_DetainedLicense == null)
            {

                lblLicenseID.Text = _LicenseId.ToString();
                btnDetain.Enabled = true;
            }
            else
            {
                MessageBox.Show("Sorry Licenses Is already Detained");
                return;
            }

        }
      
        private void btnDetain_Click(object sender, EventArgs e)
        {
            ctrDriverLicenseInfoWithFiltere1.FilterEnabled = false;
            _DetainedLicense = new clsDetainedLicense();
            _DetainedLicense.LicenseID = _LicenseId;
            _DetainedLicense.DetainDate = DateTime.Now;
            _DetainedLicense.FineFees = Convert.ToSingle(txtFineFees.Text);
            _DetainedLicense.CreatedByUserID = 1; 
            _DetainedLicense.ReleaseApplicationID = -1;

            if (_DetainedLicense.Save())
            {

                btnDetain.Enabled = false;

                lblDetainDate.Text = _DetainedLicense.DetainDate.ToString();
                lblDetainID.Text = _DetainedLicense.DetainID.ToString();
                lblCreatedByUser.Text = _DetainedLicense.CreatedByUserID.ToString();
            }
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_LicenseId);
            frm.Show();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);

            }
            ;


            if (!clsValidatoin.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);
            }
            ;
        }
    }
}
