using ExamLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamLibrary.DAL
{
    public class AdminLoginResult
    {
        public int ResultCode { get; set; }
        public int? AdminId { get; set; }
    }
    public class AdminDAL
    {
        public bool RegisterAdmin(AdminDTO admin)
        {
            SqlConnection conn = null;

            try
            {
                conn = DBHelper.Instance.GetConnection();

                using (SqlCommand cmd = new SqlCommand("AdminRegister", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@FullName", admin.FullName);
                    cmd.Parameters.AddWithValue("@Email", admin.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", admin.PasswordHash);

                    cmd.Parameters.AddWithValue("@ContactNumber", (object)admin.ContactNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedDate", admin.CreatedDate == DateTime.MinValue ? (object)DBNull.Value : admin.CreatedDate);
                    cmd.Parameters.AddWithValue("@IsActive", admin.IsActive);
                    cmd.Parameters.AddWithValue("@ActivationId", admin.ActivationId == Guid.Empty ? (object)DBNull.Value : admin.ActivationId);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Email already exists"))
                {
                    throw new Exception("This email is already registered.");
                }

                throw new Exception("An error occurred while registering admin.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error: " + ex.Message, ex);
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }

        public static void IsActivation(Guid ActivationId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActivateAdminId", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActivationId", ActivationId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }
        public static AdminLoginResult IsAdminValid(string email, string password)
        {
            var result = new AdminLoginResult();

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_VerifyAdminLoginInfo", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    cmd.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@admin_id", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    result.ResultCode = Convert.ToInt32(cmd.Parameters["@Result"].Value);

                    if (result.ResultCode == 2)
                    {
                        result.AdminId = Convert.ToInt32(cmd.Parameters["@admin_id"].Value);
                    }
                }
            }
            catch
            {
                result.ResultCode = 0; // treat as invalid
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }

            return result;
        }

    }
}
