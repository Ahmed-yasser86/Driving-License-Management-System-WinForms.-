using DVLD.Applications;
using DVLD.DriverLicense;
using DVLD.Drivers;
using DVLD.Licenses.DetainLicenes;
using DVLD.Licenses.Renew_Licenses;
using DVLD.Licenses.ReplaceDrivingLicense;
using DVLD.Login;
using DVLD.Tests;
using DVLD.Tests.SchadualTest;
using DVLD.User;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
//Application.Run(new frmLisDetainedLicenses());
       //   Application.Run(new frmAddUpdateUser()); 
         Application.Run(new frmLogin());
         


        }
    }
}
