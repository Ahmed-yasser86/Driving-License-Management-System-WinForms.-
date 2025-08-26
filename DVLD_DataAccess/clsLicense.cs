using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsCountryData;
using System.Net;
using System.Security.Policy;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {


        public static DataTable GetAllLicenses()
        {
            string query = @"
             SELECT *
             FROM Licenses";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dataTable.Load(reader); 
                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }

            return dataTable;
        }

        public static DataTable GetAll_Driver_lLicenseClasses(int DriverID)
        {
            // Define the SQL query to retrieve all licenses
            string query = @"SELECT     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses.ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN
                                LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            Order By IsActive Desc, ExpirationDate Desc";
            // Create a DataTable to hold the results
            DataTable dataTable = new DataTable();

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // Create a command object with the query and connection
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@DriverID", DriverID);

                       
                        connection.Open();

                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if(reader.HasRows)
                            dataTable.Load(reader); 
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }

            return dataTable;
        }






        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT LicenseID FROM Licenses " +
                           "INNER JOIN Drivers ON Drivers.DriverID = Licenses.DriverID " +
                           "WHERE IsActive = 1 " +
                           "AND Licenses.LicenseClass = @LicenseClassID " +  // Changed to LicenseClass
                           "AND Drivers.PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);  // Changed parameter name

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            finally
            {
                connection.Close();
            }

            return LicenseID;
        }


        public static int AddNewLicense( int ApplicationID, int DriverID, int LicenseClass,
       DateTime IssueDate, DateTime ExpirationDate, string Notes,
       float PaidFees, bool IsActive, short IssueReason, int CreatedByUserID)
        {
            string query = @"
        INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, 
                              Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
        VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, 
                @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
        SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                       
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@ApplicationID", SqlDbType.Int) { Value = ApplicationID });
                        cmd.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = DriverID });
                        cmd.Parameters.Add(new SqlParameter("@LicenseClass", SqlDbType.Int) { Value = LicenseClass });
                        cmd.Parameters.Add(new SqlParameter("@IssueDate", SqlDbType.DateTime) { Value = IssueDate });
                        cmd.Parameters.Add(new SqlParameter("@ExpirationDate", SqlDbType.DateTime) { Value = ExpirationDate });
                        cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.NVarChar) { Value = Notes ?? (object)DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@PaidFees", SqlDbType.Float) { Value = PaidFees });
                        cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = IsActive });
                        cmd.Parameters.Add(new SqlParameter("@IssueReason", SqlDbType.SmallInt) { Value = IssueReason });
                        cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", SqlDbType.Int) { Value = CreatedByUserID });

                        int newLicenseID = Convert.ToInt32(cmd.ExecuteScalar());
                        return newLicenseID;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                    return -1;
                }
            }
        }

        public static bool Update(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
      DateTime IssueDate, DateTime ExpirationDate, string Notes,
      float PaidFees, bool IsActive, short IssueReason, int CreatedByUserID)
        {
            string query = @"
        UPDATE Licenses
        SET ApplicationID = @ApplicationID,
            DriverID = @DriverID,
            LicenseClass = @LicenseClass,
            IssueDate = @IssueDate,
            ExpirationDate = @ExpirationDate,
            Notes = @Notes,
            PaidFees = @PaidFees,
            IsActive = @IsActive,
            IssueReason = @IssueReason,
            CreatedByUserID = @CreatedByUserID
        WHERE LicenseID = @LicenseID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@LicenseID", SqlDbType.Int) { Value = LicenseID });
                        cmd.Parameters.Add(new SqlParameter("@ApplicationID", SqlDbType.Int) { Value = ApplicationID });
                        cmd.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = DriverID });
                        cmd.Parameters.Add(new SqlParameter("@LicenseClass", SqlDbType.Int) { Value = LicenseClass });
                        cmd.Parameters.Add(new SqlParameter("@IssueDate", SqlDbType.DateTime) { Value = IssueDate });
                        cmd.Parameters.Add(new SqlParameter("@ExpirationDate", SqlDbType.DateTime) { Value = ExpirationDate });
                        cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.NVarChar) { Value = Notes ?? (object)DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@PaidFees", SqlDbType.Float) { Value = PaidFees });
                        cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = IsActive });
                        cmd.Parameters.Add(new SqlParameter("@IssueReason", SqlDbType.SmallInt) { Value = IssueReason });
                        cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", SqlDbType.Int) { Value = CreatedByUserID });

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }


        public static bool GetLicenseInfo(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
     ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
     ref float PaidFees, ref bool IsActive, ref short IssueReason, ref int CreatedByUserID)
        {
            string query = @"
        SELECT *
        FROM Licenses
        WHERE LicenseID = @LicenseID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@LicenseID", SqlDbType.Int) { Value = LicenseID });

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ApplicationID = reader.GetInt32(reader.GetOrdinal("ApplicationID"));
                                DriverID = reader.GetInt32(reader.GetOrdinal("DriverID"));
                                LicenseClass = reader.GetInt32(reader.GetOrdinal("LicenseClass"));
                                IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate"));
                                ExpirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"));
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? string.Empty : reader.GetString(reader.GetOrdinal("Notes"));
                                PaidFees = (float)reader.GetDecimal(reader.GetOrdinal("PaidFees"));
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                                IssueReason = (short)(reader.GetOrdinal("IssueReason"));
                                CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                                return true; 
                            }
                            else
                            {
                                return false; 
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                        return false;
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            
                            connection.Close();
                        }
                    }
                }
            }
        }


        public static bool DeActivateLicense(int licenseID)
        {
            string query = @"
        UPDATE Licenses
        SET IsActive = 0
        WHERE LicenseID = @LicenseID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@LicenseID", SqlDbType.Int) { Value = licenseID });

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                    return false;
                }
            }
        }
      



    }
}
