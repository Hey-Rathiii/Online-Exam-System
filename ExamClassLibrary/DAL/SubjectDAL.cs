using System;
using System.Data;
using System.Data.SqlClient;
using ExamClassLibrary.Model;
using ExamLibrary.DAL;

namespace ExamClassLibrary.DAL
{
    public static class SubjectDAL
    {
        // Uses DBHelper.Instance.GetConnection() like your project did.
        // If you want to switch to directly using a connection string, let me know.

        public static DataTable GetAllSubjects(SubjectDTO subject)
        {
            DataTable dtSubjects = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllSubjects", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StatusFilter", subject.StatusFilter);

                    // ⭐ ADD THIS: pass AdminID to stored procedure
                    cmd.Parameters.AddWithValue("@CreatedBy", subject.CreatedBy);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtSubjects);
                    }
                }
            }
            catch
            {
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

                    SqlParameter ret = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    ret.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();
                    return (int)ret.Value; // -1 duplicate, 1 inserted, 0 fail
                }
            }
            catch 
            {
                // log ex
                return 0;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }

        public static int DeleteSubject(SubjectDTO sub)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteSubject", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                    cmd.Parameters.AddWithValue("@AdminID", sub.CreatedBy);

                    SqlParameter ret = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    ret.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();
                    return (int)ret.Value; // 1 success, -2 questions exist, -3 exams exist, 0 fail
                }
            }
            catch 
            {
                // log ex
                return 0;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }

        public static int UpdateSubject(SubjectDTO sub)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateSubject", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                    cmd.Parameters.AddWithValue("@SubjectName", sub.SubjectName);

                    SqlParameter ret = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    ret.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();
                    return (int)ret.Value; // -1 duplicate, 1 updated, 0 fail
                }
            }
            catch 
            {
                // log ex
                return 0;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }
    }
}
