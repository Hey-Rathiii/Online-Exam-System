using ExamClassLibrary.Model;
using ExamLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClassLibrary.DAL
{
    public class QuestionDAL
    {
        public static bool InsertQuestion(InsertQueDTO insert)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertQuestion", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamID", insert.ExamId);
                    cmd.Parameters.AddWithValue("@QuestionText", insert.QuestionText);
                    cmd.Parameters.AddWithValue("@OptionA", insert.optionA);
                    cmd.Parameters.AddWithValue("@OptionB", insert.optionB);
                    cmd.Parameters.AddWithValue("@OptionC", insert.optionC);
                    cmd.Parameters.AddWithValue("@OptionD", insert.optionD);
                    cmd.Parameters.AddWithValue("@CorrectOption", insert.correctOption);
                    cmd.Parameters.AddWithValue("@Marks", insert.marks);

                    cmd.ExecuteNonQuery();
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
                    cmd.Parameters.AddWithValue("@QuestionID", queDTO.QuestionId);
                    cmd.Parameters.AddWithValue("@ExamID", queDTO.ExamId);
                    cmd.Parameters.AddWithValue("@QuestionText", queDTO.QuestionText);
                    cmd.Parameters.AddWithValue("@OptionA", queDTO.optionA);
                    cmd.Parameters.AddWithValue("@OptionB", queDTO.optionB);
                    cmd.Parameters.AddWithValue("@OptionC", queDTO.optionC);
                    cmd.Parameters.AddWithValue("@OptionD", queDTO.optionD);
                    cmd.Parameters.AddWithValue("@CorrectOption", queDTO.correctOption);
                    cmd.Parameters.AddWithValue("@Marks", queDTO.marks);

                    cmd.ExecuteNonQuery();
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

        public static bool DeleteQuestion(InsertQueDTO queDTO)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteQuestion", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QuestionID", queDTO.QuestionId);

                    cmd.ExecuteNonQuery();
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

        public static DataTable GetQuestionsByExamId(InsertQueDTO queDTO)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetQuestionsByExamId", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamID", queDTO.ExamId);

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
