using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamClassLibrary.DAL;
using ExamLibrary.DAL;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace ExamClassLibrary.DAL
{
    public  class StudentResultDAL
    {
        /// <summary>
        /// Fetches the exam report (per-question + summary) for a student
        /// </summary>
        public  static  DataSet GetStudentExamReport(int studentId, int examId)
        {
           DataSet ds = new DataSet();
          //  DataTable dt = new DataTable(); 

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentExamReport", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@ExamId", examId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds); // Table[0] = question-wise details, Table[1] = summary
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching student exam report: " + ex.Message);
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }

            return ds;
        }
        public static void MarkStudentAsDisqualified(int studentId, int examId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_DisqualifyStudent", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@ExamId", examId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error marking student disqualified: " + ex.Message);
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }

        public static void SaveFinalResult(int studentId, int examId, int totalMarks, int possibleMarks, decimal percentage)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_SaveFinalResult", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@ExamId", examId);
                    cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                    cmd.Parameters.AddWithValue("@PossibleMarks", possibleMarks);
                    cmd.Parameters.AddWithValue("@Percentage", percentage);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving final result: " + ex.Message);
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }


    }
}







