using DVLD.People;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmListDrivers: Form
    {

       private DataTable _DriversDataTable;




        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _DriversDataTable = clsDriver.GetAllDriversFromLists();

            dgvDrivers.DataSource = _DriversDataTable;

            txtFilterValue.Visible = false;

            dgvDrivers.ContextMenuStrip = contextMenuStrip1;

            dgvDrivers.Columns[0].HeaderText = "Driver ID";
            dgvDrivers.Columns[0].Width = 110;

            dgvDrivers.Columns[1].HeaderText = "Person ID";
            dgvDrivers.Columns[1].Width = 120;

            dgvDrivers.Columns[2].HeaderText = "National Number";
            dgvDrivers.Columns[2].Width = 120;

            dgvDrivers.Columns[3].HeaderText = "FullName";
            dgvDrivers.Columns[3].Width = 280;

            dgvDrivers.Columns[4].HeaderText = "Creation Date";
            dgvDrivers.Columns[4].Width = 200;

            dgvDrivers.Columns[5].HeaderText = " Number Of Active Licenses";
            dgvDrivers.Columns[5].Width = 250;

        }



        private void txtFilterValue_TextChanged_1(object sender, EventArgs e)
        {
            string Filter = cbFilterBy.Text.Trim();



            if (txtFilterValue.Text == "" || Filter == "None")
            {

                _DriversDataTable.DefaultView.RowFilter = string.Empty;
            }


            if (Filter == "FullName" || Filter == "NationalNo")
            {
                txtFilterValue.Visible = true;
                _DriversDataTable.DefaultView.RowFilter = string.Format( "[{0}] like '{1}%'", Filter, txtFilterValue.Text.Trim());
            }


            if (Filter == "DriverID" || Filter == "PersonID")
            {
                txtFilterValue.Visible = true;
                if (!string.IsNullOrEmpty(txtFilterValue.Text.Trim()))
                {
                    _DriversDataTable.DefaultView.RowFilter = string.Format("[{0}] = {1}", Filter, Convert.ToInt32(txtFilterValue.Text.Trim()));
                }
                else
                {
                    _DriversDataTable.DefaultView.RowFilter = string.Empty;

                }
            }

            dgvDrivers.DataSource = _DriversDataTable;

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(cbFilterBy.Text.Trim()==""|| cbFilterBy.Text.Trim() == "None")
            {
                txtFilterValue.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true;

            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            //refresh
            frmListDrivers_Load(null, null);

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
