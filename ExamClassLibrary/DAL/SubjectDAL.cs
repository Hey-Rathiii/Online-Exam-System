using ExamClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamLibrary.DAL
{
    public class SubjectDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static DataTable GetAllSubjects(SubjectDTO subject)
        {
            DataTable dtSubjects = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllSubjects", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(@"StatusFilter", subject.statusFilter);
                    cmd.Parameters.AddWithValue(@"AdminID", subject.AdminID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtSubjects);
                    }
                    
                }
            }
              catch (Exception ex)
              {
                 // Log the exception (not implemented here)
                 return null;
              }
              finally
              {
                 DBHelper.Instance.CloseConnection();
              }
            return dtSubjects;
        }

        public static int AddSubject(SubjectDTO subDTO)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertSubject", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectName", subDTO.SubjectName);
                    cmd.Parameters.AddWithValue("@CreatedBy", subDTO.CreatedBy);
                    cmd.ExecuteNonQuery();
                    return 1;
                }
            }
            catch (SqlException ex)
            {
                // Log the exception (not implemented here)
                if (ex.Number == 50000)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }

        public static bool DeleteSubject(SubjectDTO sub)
        {
            try

            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteSubject", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                    cmd.Parameters.AddWithValue("@AdminID", sub.AdminID);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return false;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }

        }
    }
}
    