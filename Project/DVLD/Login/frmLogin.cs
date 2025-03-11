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
using Microsoft.Win32;
using DVLD.Properties;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;
namespace DVLD.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            string EventLg = "DVLDApp";
            clsUser user= clsUser.FindByUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());




            if (user != null) 
            { 

                if (chkRememberMe.Checked )
                {
                    //store username and password
                   //    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                    string KeyPath = @"HKEY_CURRENT_USER\Software\DVLDApp";
                    string ValueUserName = txtUserName.Text.Trim();
                    string ValuePassowrd =  txtPassword.Text.Trim();
                    Registry.SetValue(KeyPath, "UserName" , ValueUserName);
                    Registry.SetValue(KeyPath, "Pass", ValuePassowrd);

                }
                else
                {
                    //store empty username and password
                    // clsGlobal.RememberUsernameAndPassword("", "");
                    string KeyPath = @"HKEY_CURRENT_USER\Software\DVLDApp";
                    string Value = "";
               //     Registry.SetValue(KeyPath, "UserName","");
                 //   Registry.SetValue(KeyPath, "Pass", "");

                    try

                    {
                        using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                        {
                            try
                            {
                                using (RegistryKey Key = baseKey.OpenSubKey(KeyPath, true))
                                {


                                   
                                    if (Key != null)
                                    {

                                        Key.DeleteValue("UserName");
                                        Key.DeleteValue("Pass");
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Registry key keyPath  not found");
                                    }

                                }
                            }
                            catch(UnauthorizedAccessException ex)
                            {


                                if (!EventLog.SourceExists(EventLg))
                                {

                                    EventLog.CreateEventSource(EventLg, "Application");

                                }

                                EventLog.WriteEntry(EventLg,( "Error Occured while saving login info "+ ex.Message), EventLogEntryType.Warning);

                                MessageBox.Show("UnauthorizedAccessException: Run the program with administrative privileges.");
                            }

                        }

                    }catch(Exception ex)
                    {
                        MessageBox.Show("error occured while deleting login info");
                    }



                }

                if (!user.IsActive )
                {

                    txtUserName.Focus();
                    MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                 clsGlobal.CurrentUser = user;
                 this.Hide();
                 frmMain frm = new frmMain(this);
                 frm.ShowDialog();


            } else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // string UserName = "", Password = "";

            string KeyPath = @"HKEY_CURRENT_USER\Software\DVLDApp";

          string ValueUserName = Registry.GetValue(KeyPath, "UserName", null) as string;
          string ValuePassowrd=  Registry.GetValue(KeyPath, "Pass", null) as string;


            if (ValueUserName!="" && ValuePassowrd!="")
            {
                txtUserName.Text = ValueUserName;//UserName;
                txtPassword.Text = ValuePassowrd;//Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
