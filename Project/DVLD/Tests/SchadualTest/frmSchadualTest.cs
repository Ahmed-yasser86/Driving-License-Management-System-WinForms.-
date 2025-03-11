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

namespace DVLD.Tests.SchadualTest
{
    public partial class frmSchadualTest: Form
    {

        int LDLApp=-1;
        int appointmentID=-1;
        clsTestType.enTestType TestTypeID;
        public frmSchadualTest(int LDLAppID, clsTestType.enTestType applicationTypeID , int appointmentID=-1)
        {


            InitializeComponent();


            this.LDLApp = LDLAppID;
            this.appointmentID = appointmentID;
            TestTypeID = applicationTypeID;
            
        }

        private void frmSchadualTest_Load(object sender, EventArgs e)
        {

            ctrlScheduleTest1.TestTypeID = TestTypeID;

            ctrlScheduleTest1.LoadInfo(LDLApp, appointmentID);



        }

        private void ctrlScheduleTest1_Load(object sender, EventArgs e)
        {

        }
    }
}
