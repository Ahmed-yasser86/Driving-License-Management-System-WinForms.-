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
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Data.SqlTypes;

namespace DVLD_DataAccess
{
    public class clsTestData
    {


        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT PassedTestCount = count(TestTypeID)
                         FROM Tests INNER JOIN
                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						 where LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestResult=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    PassedTestCount = ptCount;
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

            return PassedTestCount;



        }


        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            string Query = "SELECT TestAppointmentID, TestResult, Notes, CreatedByUserID FROM Tests WHERE TestID = @TestID";
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            // Add the parameter for TestID
            cmd.Parameters.Add(new SqlParameter("@TestID", SqlDbType.Int) { Value = TestID });

            try
            {
                connection.Open();
                using (SqlDataReader Reader = cmd.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        // Assign values from the database to the ref parameters
                        TestAppointmentID = Reader.GetInt32(Reader.GetOrdinal("TestAppointmentID"));
                        TestResult = Reader.GetBoolean(Reader.GetOrdinal("TestResult"));
                        Notes = Reader.IsDBNull(Reader.GetOrdinal("Notes")) ? string.Empty : Reader.GetString(Reader.GetOrdinal("Notes"));
                        CreatedByUserID = Reader.GetInt32(Reader.GetOrdinal("CreatedByUserID"));

                        return true; // Data found and assigned
                    }
                    else
                    {
                        return false; // No data found for the given TestID
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close(); // Ensure the connection is closed
                }
            }

            return false;
        }


        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(
     int PersonID, int LicenseClassID, int TestTypeID,
     ref int TestID, ref int TestAppointmentID, ref bool TestResult,
     ref string Notes, ref int CreatedByUserID)
        {
            string Query = @"
        SELECT TOP 1 
            Tests.TestID, 
            Tests.TestAppointmentID, 
            Tests.TestResult, 
            Tests.Notes, 
            Tests.CreatedByUserID 
        FROM Tests 
        INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID 
        INNER JOIN LocalDrivingLicenseApplications ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID 
        INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID 
        WHERE Applications.ApplicantPersonID = @PersonID 
        AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID 
        AND TestAppointments.TestTypeID = @TestTypeID 
        ORDER BY Tests.TestAppointmentID DESC";

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            // Add parameters
            cmd.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.Int) { Value = PersonID });
            cmd.Parameters.Add(new SqlParameter("@LicenseClassID", SqlDbType.Int) { Value = LicenseClassID });
            cmd.Parameters.Add(new SqlParameter("@TestTypeID", SqlDbType.Int) { Value = TestTypeID });

            try
            {
                connection.Open();
                using (SqlDataReader Reader = cmd.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        // Get column indices using GetOrdinal
                        int testIDOrdinal = Reader.GetOrdinal("TestID");
                        int testAppointmentIDOrdinal = Reader.GetOrdinal("TestAppointmentID");
                        int testResultOrdinal = Reader.GetOrdinal("TestResult");
                        int notesOrdinal = Reader.GetOrdinal("Notes");
                        int createdByUserIDOrdinal = Reader.GetOrdinal("CreatedByUserID");

                        // Assign values from the database to the ref parameters
                        TestID = Reader.GetInt32(testIDOrdinal);
                        TestAppointmentID = Reader.GetInt32(testAppointmentIDOrdinal);
                        TestResult = Reader.GetBoolean(testResultOrdinal);

                        // Handle nullable Notes column
                        if (Reader.IsDBNull(notesOrdinal))
                        {
                            Notes = string.Empty; // Default value for NULL
                        }
                        else
                        {
                            Notes = Reader.GetString(notesOrdinal);
                        }

                        CreatedByUserID = Reader.GetInt32(createdByUserIDOrdinal);

                        return true; // Data found and assigned
                    }
                    else
                    {
                        return false; // No data found for the given criteria
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                //   throw new Exception("An error occurred while retrieving test information.", ex);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close(); // Ensure the connection is closed
                }
            }

            return false;
        }

        public static DataTable GetAllTests()
            {

                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT * FROM Tests order by TestID";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)

                    {
                        dt.Load(reader);
                    }

                    reader.Close();


                }

                catch (Exception ex)
                {
                    // Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                return dt;

            }

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            string Query = @"
        INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
        OUTPUT INSERTED.TestID
        VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID)";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                // Initialize the command with the query and the connection
                SqlCommand cmd = new SqlCommand(Query, connection);

                // Add parameters to the SQL command
                cmd.Parameters.Add(new SqlParameter("@TestAppointmentID", SqlDbType.Int) { Value = TestAppointmentID });
                cmd.Parameters.Add(new SqlParameter("@TestResult", SqlDbType.Bit) { Value = TestResult });
                cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.NVarChar) { Value = (object)Notes ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", SqlDbType.Int) { Value = CreatedByUserID });

                try
                {
                    // Open the connection
                    connection.Open();

                    // Execute the command and get the inserted TestID using the OUTPUT clause
                    var insertedTestID = cmd.ExecuteScalar();

                    // Return the inserted TestID
                    return Convert.ToInt32(insertedTestID);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., log the error)
                  //  throw new Exception("An error occurred while adding a new test.", ex);
                }

               
            }
            return -1;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,
                                 string Notes, int CreatedByUserID)
        {
            string query = @"
        UPDATE Tests
        SET 
            TestAppointmentID = @TestAppointmentID,
            TestResult = @TestResult,
            Notes = @Notes,
            CreatedByUserID = @CreatedByUserID
        WHERE TestID = @TestID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@TestID", TestID);
                    cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    cmd.Parameters.AddWithValue("@TestResult", TestResult);
                    cmd.Parameters.AddWithValue("@Notes", Notes ?? string.Empty); // Handle nullable Notes
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    connection.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return false; 
        }




    }
}
