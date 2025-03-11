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

namespace DVLD.Licenses.Renew_Licenses
{
    public partial class frmReNewLicenses: Form
    {

        private int _LicenseID = -1;
        private clsLicense OldLicense ;
        private int _NewLicenseID = -1;
        public frmReNewLicenses()
        {
            InitializeComponent();
        }



       



        private void frmReNewLicenses_Load(object sender, EventArgs e)
        {
            ReSetDefualtValues();
        }


        void ReSetDefualtValues()
        {

            lblApplicationDate.Text = "[??????]";
            lblApplicationID.Text = "[??????]";
            lblIssueDate.Text = "[??????]";
            lblRenewedLicenseID.Text = "[??????]";
            lblExpirationDate.Text = "[??????]";
            lblCreatedByUser.Text = "[??????]";
            txtNotes.Text = "[??????]";
            lblApplicationFees.Text = "[??????]";
            lblLicenseFees.Text = "[??????]";
            txtNotes.Text = "";
            lblOldLicenseID.Text = "[??????]";
            lblTotalFees.Text = "[??????]";



        }



        private void ctrDriverLicenseInfoWithFiltere1_OnLicenseSelected(int obj)
        {

            btnSave.Enabled = false;

            _LicenseID = obj;
            lbShowLicenseHistory.Enabled = (_LicenseID!=-1); 

            OldLicense = clsLicense.GetLicenseInfo(_LicenseID);

            if (OldLicense == null)
            {
                ReSetDefualtValues();
                return;

            }

            if (!OldLicense.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }


            if (OldLicense.IsLicenseExpired())
            {
                btnSave.Enabled = true;
                lbShowNewLicenseInfo.Enabled = true;

                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees.ToString();
                lblLicenseFees.Text = clsLicenseClass.Find(OldLicense.LicenseClass).ClassFees.ToString();
                txtNotes.Text = OldLicense.Notes;
                lblOldLicenseID.Text = OldLicense.LicenseID.ToString();
                lblTotalFees.Text = (Convert.ToInt32(lblLicenseFees.Text) + Convert.ToInt32(lblApplicationFees.Text)).ToString();
            }



            else
            {
                MessageBox.Show("Your licenseHas Not Expired Yet You Can not Renew it");

            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            if (OldLicense == null)
            {
                MessageBox.Show("enptty");
            }


            clsLicense NewLicense = OldLicense.RenewLicense(txtNotes.Text.ToString(),clsGlobal.CurrentUser.UserID);
           
            
            if (NewLicense!=null) {

                _NewLicenseID = NewLicense.LicenseID;
                lblApplicationDate.Text = clsApplication.FindBaseApplication(NewLicense.ApplicationID).ApplicationDate.ToString();
                lblApplicationID.Text = NewLicense.ApplicationID.ToString();
                lblIssueDate.Text = NewLicense.IssueDate.ToString();
                lblRenewedLicenseID.Text = NewLicense.LicenseID.ToString();
                lblExpirationDate.Text = DateTime.Now.AddYears(clsLicenseClass.Find(NewLicense.LicenseClass).DefaultValidityLength).ToString();
                lblCreatedByUser.Text = clsUser.FindByUserID( NewLicense.CreatedByUserID).UserName;
                txtNotes.Text = NewLicense.Notes;



            }
            else
            {
                  MessageBox.Show("an error Happened Can not Renew License");
                  return;

            }





            btnSave.Enabled = false;
            lbShowNewLicenseInfo.Enabled = true;


        }

        private void lbShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm =
          new frmShowLicenseInfo(_NewLicenseID);

            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void ctrDriverLicenseInfoWithFiltere1_Load(object sender, EventArgs e)
        {

        }

        private void gpApplicationInfo_Enter(object sender, EventArgs e)
        {

        }

        private void lbShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }

}
