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

namespace DVLD.Licenses.ReplaceDrivingLicense
{


    public partial class FrmReplaceLicense: Form
    {

        private int _LicenseID = -1;
        clsLicense OldLicense;
        int _NewLicenseID;
        public FrmReplaceLicense()
        {
            InitializeComponent();
        }

        private void ctrDriverLicenseInfoWithFiltere1_Load(object sender, EventArgs e)
        {

        }

        private void ctrDriverLicenseInfoWithFiltere1_OnLicenseSelected(int obj)
        {
            btnSave.Enabled = false;

            _LicenseID = obj;
            lbShowLicenseHistory.Enabled = (_LicenseID != -1);

            OldLicense = clsLicense.GetLicenseInfo(_LicenseID);

            if (OldLicense == null)
            {
                return;
            }

            if (!OldLicense.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }


                btnSave.Enabled = true;
                lbShowNewLicenseInfo.Enabled = true;

            if (rbDamagedLicense.Checked)
            {
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).Fees.ToString();
            }
            else
            {
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).Fees.ToString();

            }
            lblLicenseFees.Text = clsLicenseClass.Find(OldLicense.LicenseClass).ClassFees.ToString();
                txtNotes.Text = OldLicense.Notes;
                lblOldLicenseID.Text = OldLicense.LicenseID.ToString();
                lblTotalFees.Text = (Convert.ToInt32(lblLicenseFees.Text) + Convert.ToInt32(lblApplicationFees.Text)).ToString();
            



        }

        private void btnSave_Click(object sender, EventArgs e)
        {



            if (MessageBox.Show("Are you sure you want to replace the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            if (!(rbDamagedLicense.Checked || rbLostLicense.Checked))
            {
                MessageBox.Show("You Need To choose replacment reaseone ");
                return;
            }

            clsLicense NewLicense = OldLicense.RenewLicense(txtNotes.Text.ToString(), clsGlobal.CurrentUser.UserID);


            if (NewLicense != null)
            {

                _NewLicenseID = NewLicense.LicenseID;
                lblApplicationDate.Text = clsApplication.FindBaseApplication(NewLicense.ApplicationID).ApplicationDate.ToString();
                lblApplicationID.Text = NewLicense.ApplicationID.ToString();
                lblIssueDate.Text = NewLicense.IssueDate.ToString();
                lblRenewedLicenseID.Text = NewLicense.LicenseID.ToString();
                lblExpirationDate.Text = DateTime.Now.AddYears(clsLicenseClass.Find(NewLicense.LicenseClass).DefaultValidityLength).ToString();
                lblCreatedByUser.Text = clsUser.FindByUserID(NewLicense.CreatedByUserID).UserName;
                txtNotes.Text = NewLicense.Notes;



            }
            else
            {
                MessageBox.Show("an error Happened Can not replace License");
                return;

            }





            btnSave.Enabled = false;
            lbShowNewLicenseInfo.Enabled = true;
            ctrDriverLicenseInfoWithFiltere1.Enabled = false;



        }

        private void lbShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
       
            frmShowLicenseInfo frm =
          new frmShowLicenseInfo(_NewLicenseID);

            frm.ShowDialog();
        

    }
}
}
