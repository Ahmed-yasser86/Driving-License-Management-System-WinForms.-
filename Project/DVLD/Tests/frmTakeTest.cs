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

namespace DVLD.Tests
{
    public partial class frmTakeTest: Form
    {

        public int TestAppointmentID;
        private string _Notes;
        private byte _Results;
        private clsTestAppointment _TestAppointment;
        private clsTest _Test;
        clsTestType.enTestType _TestType;
        public frmTakeTest(int TestAppointmentID, clsTestType.enTestType TestType)
        {
             this.TestAppointmentID = TestAppointmentID;
            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            _Test = clsTest.Find(_TestAppointment.TestID);
            _TestType = TestType;
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {



        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlSecheduledTest1.TestTypeID=_TestType;

            lbGBWearningMessage.Visible = false;
            groupBox1.Enabled = false;


            if (_TestAppointment == null)
            {
                MessageBox.Show("Sorry We Can not Load Test appointment Info , an error occured");
                return;
            }
            if (_Test == null || _TestAppointment != null)
            {
                ctrlSecheduledTest1.LoadInfo(TestAppointmentID);
                groupBox1.Enabled = true;
                btnSave.Enabled = true;
                _Test = new clsTest();
                return;
            }
         
            
            

           

            if (_Test == null)
            {
                MessageBox.Show("CouldN't load the test info an error occured");
                _Test = new clsTest();
            }
            if (_Test.TestResult == true)
            {
                radioButton1.Checked = true;
                richTextBox1.Text = _Test.Notes;

            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }


            if (_TestAppointment.IsLocked)
            {

                lbGBWearningMessage.Visible = true;
                groupBox1.Enabled = true;

            }


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _Test.TestAppointmentID = TestAppointmentID;
            _Test.Notes = richTextBox1.Text;
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _Test.TestResult = radioButton1.Checked;
            if (_Test.Save())
            {
                MessageBox.Show("Changes Saved Succsefully");
                btnSave.Enabled = false;

            }
            else
            {
                MessageBox.Show("Changes has not Saved , error occured");

            }



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
