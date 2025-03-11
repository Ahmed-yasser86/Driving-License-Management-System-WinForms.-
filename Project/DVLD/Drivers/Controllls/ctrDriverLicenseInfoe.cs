using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;
using DVLD.Classes;
using System.IO;

namespace DVLD.Drivers.Controllls
{
    public partial class ctrDriverLicenseInfoe: UserControl
    {

        private int _LicenseID;
        private clsLicense _License;

        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        { get { return _License; } }

        public ctrDriverLicenseInfoe()
        {
            InitializeComponent();
        }


        private void ctrDriverLicenseInfo_Load(object sender, EventArgs e)
        {

        }



        void _LoadImage()
        {

            string ImagePath = (_License.DriverInfo.PersonInfo.ImagePath);

            if (ImagePath==null || ImagePath == "")
            {
                pbPersonImage.Image = (_License.DriverInfo.PersonInfo.Gendor == 0 ? Properties.Resources.Male_512 : Properties.Resources.Female_512);
                return;
            }


            if (!File.Exists(ImagePath))
            {
                MessageBox.Show("Sorry I Couldn'tFind ur image");
                pbPersonImage.Image = (_License.DriverInfo.PersonInfo.Gendor == 0 ? Properties.Resources.Male_512 : Properties.Resources.Female_512);
                return;
            }


            pbPersonImage.Load( ImagePath);



        }




               public void LoadInfo(int LicenseID){
            _LicenseID = LicenseID;
            _License = clsLicense.GetLicenseInfo(_LicenseID);
            if (_License == null)
            {
                MessageBox.Show("Could not find License ID = " + _LicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            lblLicenseID.Text = _License.LicenseID.ToString();
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";
           // lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            lblClass.Text = _License.LicenseClassIfo.ClassName;
            lblFullName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = _License.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = _License.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblDateOfBirth.Text = clsFormat.DateToShort(_License.DriverInfo.PersonInfo.DateOfBirth);

            lblDriverID.Text = _License.DriverID.ToString();
            lblIssueDate.Text = clsFormat.DateToShort(_License.IssueDate);
            lblExpirationDate.Text = clsFormat.DateToShort(_License.ExpirationDate);
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = _License.Notes == "" ? "No Notes" : _License.Notes;

            _LoadImage();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
