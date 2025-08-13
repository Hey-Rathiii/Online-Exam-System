using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamLibrary.Model;

namespace ExamLibrary.DAL
{
    public class StudentDAL
    {
        public static bool RegisterStudent(AdminDTO Student)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_StudentRegister", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", Student.FullName);
                    cmd.Parameters.AddWithValue("@Email", Student.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", Student.PasswordHash);
                    cmd.Parameters.AddWithValue("@ContactNumber", Student.ContactNumber);
                    cmd.Parameters.AddWithValue("@ActivationId", Student.ActivationId);

                    //DBHelper.Instance.GetConnection().Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                // Optional: Handle or log specific error like duplicate email
                Console.WriteLine("SQL Error: " + ex.Message);
                return false;
            }
            finally
            {
                DBHelper.Instance.GetConnection().Close();
            }
        }




        public static void isActivation(Guid ActivationId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActivateStudentId", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ActivationId", ActivationId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            finally
            {
                DBHelper.Instance.GetConnection().Close();
            }
        }

        public static int IsStudentValid(string email, string Password)
        {

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_VerifyStudentLoginInfo", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@input_email", email);
                    cmd.Parameters.AddWithValue("@input_password", Password);

                    cmd.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int resultValue = (int)cmd.Parameters["@Result"].Value;

                    return resultValue;

                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }

        public static DataTable GetAllExams()
        {
            DataTable examsTable = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllExams", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();
                    examsTable.Load(reader);
                }
            }
            catch
            {

            }
            finally
            {

            }
            return examsTable;
        }
    }
}
