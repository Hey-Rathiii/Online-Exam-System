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
    public class ExamDAL
    {
        public static bool InsertExam(ExamDTO exam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertExam", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectID", exam.subjectId);
                    cmd.Parameters.AddWithValue("@SubjectName", exam.subjectName);
                    cmd.Parameters.AddWithValue("@ExamTitle", exam.examTitle);
                    cmd.Parameters.AddWithValue("@ExamDate", exam.examDate.Date);
                    cmd.Parameters.AddWithValue("@StartTime", exam.startTime);
                    cmd.Parameters.AddWithValue("@EndTime", exam.endTime);
                    cmd.Parameters.AddWithValue("@DurationMinutes", exam.durationMinutes);

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

        public static DataTable GetExams(ExamDTO exam)
        {
            DataTable dtExams = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetExams", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StatusFilter", exam.statusFilter);
                   // cmd.Parameters.AddWithValue("@AdminID", exam.adminId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtExams);
                    }
                }
            }
            catch
            {
                // Optional: log exception
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }

            return dtExams;
        }


        public static bool UpdateExam(ExamDTO exam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateExam", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamID", exam.examId);
                    cmd.Parameters.AddWithValue("@SubjectID", exam.subjectId);
                    cmd.Parameters.AddWithValue("@ExamTitle", exam.examTitle);
                    cmd.Parameters.AddWithValue("@ExamDate", exam.examDate);
                    cmd.Parameters.AddWithValue("@StartTime", exam.startTime);
                    cmd.Parameters.AddWithValue("@EndTime", exam.endTime);
                    cmd.Parameters.AddWithValue("@DurationMinutes", exam.durationMinutes);

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

        public static bool SetExamStatus(ExamDTO exam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_SetExamStatus", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamID", exam.examId);
                    cmd.Parameters.AddWithValue("@IsActive", exam.isActive);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                // Log exception if needed
                return false;
            }
            finally
            {
                DBHelper.Instance.CloseConnection();
            }
        }



    }
}
