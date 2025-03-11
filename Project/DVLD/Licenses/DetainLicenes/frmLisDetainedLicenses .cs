using DVLD.Licenses.RealesDetainedLicense;
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
    public partial class frmLisDetainedLicenses: Form
    {


        DataTable _dtDetainedLicenses;
        public frmLisDetainedLicenses()
        {
            InitializeComponent();
        }
      
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "None")
            {

                cbIsReleased.Visible = false;
                txtFilterValue.Visible = false;

            }


            if (cbFilterBy.Text == "Is Released")
            {

                cbIsReleased.Visible = true;
                txtFilterValue.Visible = false;

            }


            if(cbFilterBy.Text!= "None"&&cbFilterBy.Text!= "Is Released")
            {
                txtFilterValue.Visible = true;
                cbIsReleased.Visible = false;

            }


        }

        private void frmLisDetainedLicenses_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;

            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();

            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;
            
            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[0].Width = 90;

                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[1].Width = 90;

                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[2].Width = 160;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 110;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 110;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 160;

                dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                dgvDetainedLicenses.Columns[6].Width = 90;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 330;

                dgvDetainedLicenses.Columns[8].HeaderText = "Rlease App.ID";
                dgvDetainedLicenses.Columns[8].Width = 150;

            }

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {


            string Filter = "";

            switch (txtFilterValue.Text)
            {
                case "None":
                    Filter = "None";
                    break;
                case "DetainID":
                    Filter = "DetainID";
                    break;
                case "Is Released":
                    Filter = "IsReleased";
                    break;
                case "National No.":
                    Filter = "NationalNo";
                    break;
                case "Full Name":
                    Filter = "FullName";
                    break;
                case "Release Application ID":
                    Filter = "ReleaseApplicationID";
                    break;
                default:
                    Filter = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || Filter == "None")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                return;
            }


            if (Filter == "DetainID" || Filter == "ReleaseApplicationID")
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", Filter, txtFilterValue.Text.Trim());
            else
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", Filter, txtFilterValue.Text.Trim());

            }
            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;

        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsReleased";
            string FilterValue = cbIsReleased.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }


            if (FilterValue == "All")
                _dtDetainedLicenses.DefaultView.RowFilter = "";
            else
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);
            }

            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;

        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.Show();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.Show();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( cbFilterBy.Text.Trim()== "Detain ID" || cbFilterBy.Text.Trim() == "Release Application ID")
            {




            }
        }
    }
}
