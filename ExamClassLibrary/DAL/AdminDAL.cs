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
    public class AdminDAL
    {
        public bool RegisterAdmin(AdminDTO admin)
        {
            SqlConnection conn = null;

            try
            {
                conn = DBHelper.Instance.GetConnection();

                using (SqlCommand cmd = new SqlCommand("Admin_Register", conn))
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
    }
}
