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
            string query = @"
        INSERT INTO Drivers ( PersonID, CreatedByUserID, CreatedDate)
        VALUES ( @PersonID, @CreatedByUserID, @CreatedDate)";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.Int) { Value = PersonID });
                        cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", SqlDbType.Int) { Value = CreatedByUserID });
                        cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = dateTime });



                        object DriverID = cmd.ExecuteScalar();

                        if(DriverID != null && DriverID!=DBNull.Value)
                        return (int)DriverID;
                    }
                    catch (Exception ex)
                    {
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

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = DriverID });
                        cmd.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.Int) { Value = PersonID });
                        cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", SqlDbType.Int) { Value = CreatedByUserID });
                        cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = dateTime });

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("An error occurred while updating the driver.", ex);
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




        public static bool GetDriverByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime dateTime)
        {
            string query = @"
        SELECT PersonID, CreatedByUserID, CreatedDate
        FROM Drivers
        WHERE DriverID = @DriverID";

           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = DriverID });

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                                CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                                dateTime = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
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
                        MessageBox.Show(ex.Message);
                        //  throw new Exception("An error occurred while retrieving the driver.", ex);
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
        public bool DeleteDriver(int DriverID)
        {
            string query = @"
        DELETE FROM Drivers
        WHERE DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@DriverID", SqlDbType.Int) { Value = DriverID });

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occurred while deleting the driver.", ex);
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




        public static DataTable GetAllDrivers()
        {
           
            string query = @"
        SELECT DriverID, PersonID, CreatedByUserID, CreatedDate
        FROM Drivers";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                             dataTable.Load(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                       // throw new Exception("An error occurred while retrieving all drivers.", ex);
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



        public static DataTable GetAllDriversForDriversList()
        {
            string query = @"
        SELECT *
        FROM Drivers_View";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            dataTable.Load(dataReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // throw new Exception("An error occurred while retrieving all drivers.", ex);
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




        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            string query = @"
        SELECT DriverID, CreatedByUserID, CreatedDate
        FROM Drivers
        WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        cmd.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.Int) { Value = PersonID });

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DriverID = reader.GetInt32(reader.GetOrdinal("DriverID"));
                                CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
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
                        // Log the error or rethrow the exception
                        //throw new Exception("An error occurred while retrieving driver information by PersonID.", ex);
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
