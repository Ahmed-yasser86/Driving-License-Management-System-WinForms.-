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
using System.Data.Common;
using System.Windows.Forms;

namespace DVLD_DataAccess
{
    public class clsDriverData
    {


        public static int AddDriver( int PersonID, int CreatedByUserID, DateTime dateTime)
        {
            // Define the SQL query to insert a new driver
            string query = @"
        INSERT INTO Drivers ( PersonID, CreatedByUserID, CreatedDate)
        VALUES ( @PersonID, @CreatedByUserID, @CreatedDate)";

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
                        cmd.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.Int) { Value = PersonID });
                        cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", SqlDbType.Int) { Value = CreatedByUserID });
                        cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = dateTime });



                        object DriverID = cmd.ExecuteScalar();

                        if(DriverID != null || DriverID!=DBNull.Value)
                        // Execute the query and return the number of rows affected
                        return (int)DriverID;
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                        //throw new Exception("An error occurred while adding the driver.", ex);
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


        public static bool Update(int DriverID, int PersonID, int CreatedByUserID, DateTime dateTime)
        {
            // Define the SQL query to update a driver
            string query = @"
        UPDATE Drivers
        SET PersonID = @PersonID,
            CreatedByUserID = @CreatedByUserID,
            CreatedDate = @CreatedDate
        WHERE DriverID = @DriverID";

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
                        cmd.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = DriverID });
                        cmd.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.Int) { Value = PersonID });
                        cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", SqlDbType.Int) { Value = CreatedByUserID });
                        cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = dateTime });

                        // Execute the query and return true if at least one row was affected
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                        //throw new Exception("An error occurred while updating the driver.", ex);
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




        public static bool GetDriverByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime dateTime)
        {
            // Define the SQL query to retrieve a driver
            string query = @"
        SELECT PersonID, CreatedByUserID, CreatedDate
        FROM Drivers
        WHERE DriverID = @DriverID";

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

                        // Add the DriverID parameter
                        cmd.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = DriverID });

                        // Execute the query and read the results
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assign values to the ref parameters
                                PersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                                CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                                dateTime = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
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
                        MessageBox.Show(ex.Message);
                        // Log the error or rethrow the exception
                        //  throw new Exception("An error occurred while retrieving the driver.", ex);
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
        public bool DeleteDriver(int DriverID)
        {
            // Define the SQL query to delete a driver
            string query = @"
        DELETE FROM Drivers
        WHERE DriverID = @DriverID";

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

                        // Add the DriverID parameter
                        cmd.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = DriverID });

                        // Execute the query and return true if at least one row was affected
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                        throw new Exception("An error occurred while deleting the driver.", ex);
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




        public static DataTable GetAllDrivers()
        {
            // Define the SQL query to retrieve all drivers
            string query = @"
        SELECT DriverID, PersonID, CreatedByUserID, CreatedDate
        FROM Drivers";

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

                        // Execute the query and fill the DataTable
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                             dataTable.Load(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                       // throw new Exception("An error occurred while retrieving all drivers.", ex);
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



        public static DataTable GetAllDriversForDriversList()
        {
            // Define the SQL query to retrieve all drivers
            string query = @"
        SELECT *
        FROM Drivers_View";

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

                        // Execute the query and fill the DataTable
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            dataTable.Load(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error or rethrow the exception
                        // throw new Exception("An error occurred while retrieving all drivers.", ex);
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




        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            // Define the SQL query to retrieve driver information by PersonID
            string query = @"
        SELECT DriverID, CreatedByUserID, CreatedDate
        FROM Drivers
        WHERE PersonID = @PersonID";

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

                        // Add the PersonID parameter
                        cmd.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.Int) { Value = PersonID });

                        // Execute the query and read the results
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assign values to the ref parameters
                                DriverID = reader.GetInt32(reader.GetOrdinal("DriverID"));
                                CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
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
                        //throw new Exception("An error occurred while retrieving driver information by PersonID.", ex);
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
