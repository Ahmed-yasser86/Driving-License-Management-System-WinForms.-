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
            // Define the SQL query to retrieve all licenses
            string query = @"
             SELECT *
             FROM Licenses";

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
                        // Open the database connection
                        connection.Open();

                        // Execute the query and load the results into the DataTable
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dataTable.Load(reader); // Load data from the reader into the DataTable
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                       // throw new Exception("An error occurred while retrieving all licenses.", ex);
                    }
                    finally
                    {
                        // Ensure the connection is closed
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }

            // Return the DataTable
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

                        // Open the database connection
                        connection.Open();

                        // Execute the query and load the results into the DataTable
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if(reader.HasRows)
                            dataTable.Load(reader); // Load data from the reader into the DataTable
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                        // throw new Exception("An error occurred while retrieving all licenses.", ex);
                    }
                    finally
                    {
                        // Ensure the connection is closed
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }

            // Return the DataTable
            return dataTable;
        }






        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT LicenseID FROM Licenses " +
                           "INNER JOIN Drivers ON Drivers.DriverID = Licenses.DriverID " +
                           "WHERE IsActive = 1 " +
                           "AND Licenses.LicenseClassID = @LicenseClassID " +
                           "AND Drivers.PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

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
                //Console.WriteLine("Error: " + ex.Message);

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
            // Define the SQL query to insert a new license
            string query = @"
        INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, 
                              Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
        VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, 
                @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
        SELECT SCOPE_IDENTITY();"; // Retrieve the newly generated LicenseID

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // Create a command object with the query and connection
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        // Open the database connection
                        connection.Open();

                        // Add parameters to the command
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

                        // Execute the query and retrieve the newly generated LicenseID
                        int newLicenseID = Convert.ToInt32(cmd.ExecuteScalar());
                        return newLicenseID;
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                        //  throw new Exception("An error occurred while adding a new license.", ex);
                    }
                    finally
                    {
                        // Ensure the connection is closed
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
            // Define the SQL query to update a license
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

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // Create a command object with the query and connection
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        // Open the database connection
                        connection.Open();

                        // Add parameters to the command
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

                        // Execute the query and return true if at least one row was affected
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                        return false;
                    }
                    finally
                    {
                        // Ensure the connection is closed
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
            // Define the SQL query to retrieve license information
            string query = @"
        SELECT *
        FROM Licenses
        WHERE LicenseID = @LicenseID";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // Create a command object with the query and connection
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        // Open the database connection
                        connection.Open();

                        // Add the LicenseID parameter
                        cmd.Parameters.Add(new SqlParameter("@LicenseID", SqlDbType.Int) { Value = LicenseID });

                        // Execute the query and read the results
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assign values to the ref parameters
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
                                return true; // Data found
                            }
                            else
                            {
                                return false; // No data found
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        // Log the error or rethrow the exception
                        MessageBox.Show(ex.ToString());
                        return false;
                       // throw new Exception("An error occurred while retrieving license information.", ex);
                    }
                    finally
                    {
                        // Ensure the connection is closed
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
            // Define the SQL query to deactivate a license
            string query = @"
        UPDATE Licenses
        SET IsActive = 0
        WHERE LicenseID = @LicenseID";

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // Create a command object with the query and connection
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        // Open the database connection
                        connection.Open();

                        // Add the LicenseID parameter
                        cmd.Parameters.Add(new SqlParameter("@LicenseID", SqlDbType.Int) { Value = licenseID });

                        // Execute the query and return true if at least one row was affected
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                       // throw new Exception("An error occurred while deactivating the license.", ex);
                    }
                    finally
                    {
                        // Ensure the connection is closed
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
