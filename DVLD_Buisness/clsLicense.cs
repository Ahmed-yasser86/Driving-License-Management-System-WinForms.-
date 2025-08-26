using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using DVLD_DataAccess;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Buisness
{
    public class clsLicense
    {

       
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };
        public enum enMode { AddNew = 0, Update = 1 };

        public enMode Mode = enMode.AddNew;

        public clsLocalDrivingLicenseApplication LocalDrivingLicenseInfo;

        public clsLicenseClass LicenseClassIfo { get; }
        public clsDriver DriverInfo {get;}
        public   string Notes { set; get; }
        public  float PaidFees { set; get; }
        public  bool IsActive { set; get; }
        public  int CreatedByUserID { set; get; } 
        public  DateTime IssueDate { set; get; }
        public  DateTime ExpirationDate { set; get; }
        public  int LicenseClass { set; get; }
        public  int ApplicationID { set; get; }
        public  int LicenseID { set; get; }
        public int DriverID { set; get; }






        public enIssueReason IssueReason { set; get; }
        public string IssueReasonText
        { get 
            {

                switch (IssueReason)
                {

                    case enIssueReason.FirstTime:
                        return "First Time";
                    case enIssueReason.Renew:
                        return "Renew";
                    case enIssueReason.DamagedReplacement:
                        return "Replacement for Damaged";
                    case enIssueReason.LostReplacement:
                        return "Replacement for Lost";
                    default:
                        return "First Time";

                }
                } 
        }
       

        public clsLicense()

        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.MinValue;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = false;
            this.IssueReason = (enIssueReason) 1;
            this.CreatedByUserID = CreatedByUserID;

        }

        public clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes,
            float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)

        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            this.LicenseClassIfo = clsLicenseClass.Find(this.LicenseClass);
          //  this.DetainedInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);

            Mode = enMode.Update;
        }


        public bool IsLicenseExpired()
        {

            return (this.ExpirationDate < DateTime.Now);
        }

        bool _AddNewLicense()
        {


            this.LicenseID = clsLicenseData.AddNewLicense(
        this.ApplicationID,
        this.DriverID,
        this.LicenseClass,
        this.IssueDate,
        this.ExpirationDate,
        this.Notes,
        this.PaidFees,
        this.IsActive,
        (short)this.IssueReason,
        this.CreatedByUserID
    );


            return (this.LicenseID != -1);

        }

        bool _UpdateLicense()
        {
            return clsLicenseData.Update(this.LicenseID, this.ApplicationID,
        this.DriverID,
        this.LicenseClass,
        this.IssueDate,
        this.ExpirationDate,
        this.Notes,
        this.PaidFees,
        this.IsActive,
        (short)this.IssueReason,// this should be byte instead of short but bec if i edited i will ned to edite it in many places and that will lead to many bugs so i decided to edited later
        this.CreatedByUserID);

        }

        static public clsLicense GetLicenseInfo(int LicenseID)
        {
            /*int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
     ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
     ref float PaidFees, ref bool IsActive, ref short IssueReason, ref int CreatedByUserI*/
            // Initialize variables
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = false;
            int CreatedByUserID = 0;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.MaxValue;
            int LicenseClass = -1;
            int ApplicationID = -1;
            int DriverID = -1;
            short IssueReason = 1;
            // Call GetLicenseInfo and pass variables by reference
            if (clsLicenseData.GetLicenseInfo(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass,
                ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason,
                ref CreatedByUserID))
            {
                return new clsLicense(LicenseID,  ApplicationID,  DriverID, LicenseClass,
                IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason) IssueReason,
                CreatedByUserID);
            }
            return null;

        }

        static public DataTable GetAllLicenses()
        {

            return clsLicenseData.GetAllLicenses();
        }

        static public DataTable GetDriverLicenses(int DriverID)
        {

            return clsLicenseData.GetAll_Driver_lLicenseClasses(DriverID);
        }

        public bool DeactiveateLicense()
        {


            return clsLicenseData.DeActivateLicense(this.LicenseID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicense();

            }

            return false;
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {

            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);

        }

        public clsLicense RenewLicense(string Notes , int CreatedByUserID)
        {
            /*
            this.PaidFees = PaidFees;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            Mode = enMode.Update;
        }
             
             */
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.LastStatusDate = Application.LastStatusDate;
            Application.CreatedByUserID = CreatedByUserID;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees;
            if (Application.Save())
            {
                clsLicense NewLicense = new clsLicense();


                NewLicense.ApplicationID = Application.ApplicationID;
                NewLicense.DriverID = this.DriverID;
                NewLicense.LicenseClass = this.LicenseClass;
                NewLicense.IssueDate = DateTime.Now;

                int DefaultValidityLength = this.LicenseClassIfo.DefaultValidityLength;

                NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
                NewLicense.Notes = Notes;
                NewLicense.PaidFees = this.LicenseClassIfo.ClassFees;
                NewLicense.IsActive = true;
                NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
                NewLicense.CreatedByUserID = CreatedByUserID;


                if (NewLicense.Save())
                {

                    this.DeactiveateLicense();
                    return NewLicense;
                }
            }
            
                return null;
         
             
        }

        public clsLicense ReplaceLicense(string Notes, int CreatedByUserID, clsApplication.enApplicationType RepalcmentReason)
        {

            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            if (RepalcmentReason == clsApplication.enApplicationType.ReplaceLostDrivingLicense)
            {
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
            }
            else if (RepalcmentReason == clsApplication.enApplicationType.ReplaceDamagedDrivingLicense)
            {
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            }
            Application.LastStatusDate = Application.LastStatusDate;
            Application.CreatedByUserID = CreatedByUserID;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees;
            if (Application.Save())
            {
                clsLicense ReplacedLicense = new clsLicense();


                ReplacedLicense.ApplicationID = Application.ApplicationID;
                ReplacedLicense.DriverID = this.DriverID;
                ReplacedLicense.LicenseClass = this.LicenseClass;
                ReplacedLicense.IssueDate = DateTime.Now;

                int DefaultValidityLength = this.LicenseClassIfo.DefaultValidityLength;

                ReplacedLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
                ReplacedLicense.Notes = Notes;
                ReplacedLicense.PaidFees = this.LicenseClassIfo.ClassFees;
                ReplacedLicense.IsActive = true;
                ReplacedLicense.IssueReason = clsLicense.enIssueReason.Renew;
                ReplacedLicense.CreatedByUserID = CreatedByUserID;


                if (ReplacedLicense.Save())
                {

                    this.DeactiveateLicense();
                    return ReplacedLicense;
                }
            }

            return null;


        }







    }
}
