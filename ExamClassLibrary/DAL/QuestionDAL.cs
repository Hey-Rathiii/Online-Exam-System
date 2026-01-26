using System;
using System.Data;
using System.Data.SqlClient;
using ExamClassLibrary.Model;
using ExamLibrary.DAL;

namespace ExamClassLibrary.DAL
{
    public static class QuestionDAL
    {
        public static bool InsertQuestion(InsertQueDTO insert)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertQuestion", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ExamID", insert.ExamID);
                    cmd.Parameters.AddWithValue("@QuestionText", insert.QuestionText);
                    cmd.Parameters.AddWithValue("@OptionA", insert.OptionA);
                    cmd.Parameters.AddWithValue("@OptionB", insert.OptionB);
                    cmd.Parameters.AddWithValue("@OptionC", insert.OptionC);
                    cmd.Parameters.AddWithValue("@OptionD", insert.OptionD);
                    cmd.Parameters.AddWithValue("@CorrectOption", insert.CorrectOption);
                    cmd.Parameters.AddWithValue("@Marks", insert.Marks);

                    SqlParameter outId = new SqlParameter("@NewQuestionID", SqlDbType.Int);
                    outId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outId);

                    cmd.ExecuteNonQuery();

                    insert.QuestionID = (int)outId.Value;  // VERY IMPORTANT

                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }


        public static bool UpdateQuestion(InsertQueDTO queDTO)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateQuestion", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QuestionID", queDTO.QuestionID);
                    cmd.Parameters.AddWithValue("@ExamID", queDTO.ExamID);
                    cmd.Parameters.AddWithValue("@QuestionText", queDTO.QuestionText);
                    cmd.Parameters.AddWithValue("@OptionA", queDTO.OptionA);
                    cmd.Parameters.AddWithValue("@OptionB", queDTO.OptionB);
                    cmd.Parameters.AddWithValue("@OptionC", queDTO.OptionC);
                    cmd.Parameters.AddWithValue("@OptionD", queDTO.OptionD);
                    cmd.Parameters.AddWithValue("@CorrectOption", queDTO.CorrectOption);
                    cmd.Parameters.AddWithValue("@Marks", queDTO.Marks);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch 
            {
                // log ex
                return false;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }

        public static bool DeleteQuestion(int questionId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteQuestion", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QuestionID", questionId);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch 
            {
                // log ex
                return false;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }

        public static DataTable GetQuestionsByExamId(InsertQueDTO dto)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetQuestionsByExamId", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamID", dto.ExamID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
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
        }


    }
}
