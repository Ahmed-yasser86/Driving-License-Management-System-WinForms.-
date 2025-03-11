using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Local_Licenses
{
    public partial class showLicenseInfo: Form
    {
        private int _LicenseID;

        public showLicenseInfo(int DriverLicenseID)
        {
            InitializeComponent();
            _LicenseID = DriverLicenseID;
        }

        private void ctrDriverLicenseInfoe1_Load(object sender, EventArgs e)
        {
            ctrDriverLicenseInfoe1.LoadInfo(_LicenseID);
        }

        private void showLicenseInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
