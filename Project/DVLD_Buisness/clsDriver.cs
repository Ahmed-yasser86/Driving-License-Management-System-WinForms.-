using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public class clsDriver
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int   DriverID { set; get; }
        public  int   PersonID { set; get; }
        public   int     CreatedByUserID { set; get; }
        public  DateTime dateTime { set; get; }

        public clsPerson PersonInfo { get; }

        public clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime dateTime)

        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.dateTime = dateTime;
            PersonInfo = clsPerson.Find(PersonID);
            Mode = enMode.Update;

                    }

        public clsDriver()

        {

            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.dateTime = DateTime.Now;

            Mode = enMode.AddNew;
        }


        public bool _UpdateDriver() {


            return clsDriverData.Update(this.DriverID, this.PersonID, this.CreatedByUserID, this.dateTime);
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime dateTime = DateTime.MinValue;

            if (clsDriverData.GetDriverByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref dateTime))
            {
                return new clsDriver( DriverID,PersonID ,CreatedByUserID, dateTime);
            }
            return null;
        }

        private  bool _AddNewDriver()
        {

            this.DriverID  = clsDriverData.AddDriver(this.PersonID, this.CreatedByUserID, this.dateTime);

            return DriverID!=-1;
        }

        public static clsDriver FindByPersonID(int PersonID)
        {

            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime dateTime = DateTime.MinValue;

            if( clsDriverData.GetDriverInfoByPersonID(PersonID ,ref DriverID,ref CreatedByUserID, ref dateTime))
            {
                return new clsDriver( DriverID, PersonID,  CreatedByUserID,  dateTime);
            }
            return null;
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        }


        public static DataTable GetAllDriversFromLists()
        {
            return clsDriverData.GetAllDriversForDriversList();
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }

       
       static public DataTable GetAllDriverLicense(int DriverID)
        {


            return clsLicense.GetDriverLicenses(DriverID);
        }


    }
}
