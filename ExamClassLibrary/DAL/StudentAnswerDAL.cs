using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamClassLibrary.Model;
using ExamLibrary.DAL;

namespace ExamClassLibrary.DAL
{
    public class StudentAnswerDAL
    {
        public static bool InsertStudentAnswer(int StudentID, int ExamID, int QuestionID, char GivenAnswerOption)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertStudentAnswer", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentID", StudentID);
                    cmd.Parameters.AddWithValue("@ExamID", ExamID);
                    cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                    cmd.Parameters.AddWithValue("@GivenAnswerOption", GivenAnswerOption);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Optional: log or display the exception for debugging
                Console.WriteLine("Error inserting student answer: " + ex.Message);
                return false;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }
    }
}

