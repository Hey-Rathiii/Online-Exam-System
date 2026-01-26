using System;
using System.Data;
using System.Data.SqlClient;
using ExamClassLibrary.Model;
using ExamLibrary.DAL;

namespace ExamClassLibrary.DAL
{
    public static class ExamDAL
    {
        public static bool InsertExam(ExamDTO exam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertExam", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = exam.SubjectID;
                    cmd.Parameters.Add("@SubjectName", SqlDbType.NVarChar, 200).Value = (object)exam.SubjectName ?? DBNull.Value;
                    cmd.Parameters.Add("@ExamTitle", SqlDbType.NVarChar, 200).Value = exam.ExamTitle;
                    cmd.Parameters.Add("@ExamDate", SqlDbType.Date).Value = exam.ExamDate.Date;
                    cmd.Parameters.Add("@StartTime", SqlDbType.Time).Value = exam.StartTime;
                    cmd.Parameters.Add("@EndTime", SqlDbType.Time).Value = exam.EndTime;
                    cmd.Parameters.Add("@DurationMinutes", SqlDbType.Int).Value = exam.DurationMinutes;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = exam.CreatedBy;

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

        public static DataTable GetExams(ExamDTO exam)
        {
            DataTable dtExams = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetExams", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@StatusFilter", SqlDbType.Int).Value = exam.StatusFilter;
                    cmd.Parameters.Add("@AdminID", SqlDbType.Int).Value = exam.CreatedBy;
                    cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = exam.SubjectID;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtExams);
                    }
                }
            }
            catch 
            {
                // log ex
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

                    cmd.Parameters.Add("@ExamID", SqlDbType.Int).Value = exam.ExamID;
                    cmd.Parameters.Add("@SubjectID", SqlDbType.Int).Value = exam.SubjectID;
                    cmd.Parameters.Add("@ExamTitle", SqlDbType.NVarChar, 200).Value = exam.ExamTitle;
                    cmd.Parameters.Add("@ExamDate", SqlDbType.Date).Value = exam.ExamDate.Date;
                    cmd.Parameters.Add("@StartTime", SqlDbType.Time).Value = exam.StartTime;
                    cmd.Parameters.Add("@EndTime", SqlDbType.Time).Value = exam.EndTime;
                    cmd.Parameters.Add("@DurationMinutes", SqlDbType.Int).Value = exam.DurationMinutes;

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

        public static bool SetExamStatus(ExamDTO exam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_SetExamStatus", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamID", exam.ExamID);
                    cmd.Parameters.AddWithValue("@IsActive", exam.IsActive);

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
        public static bool HasStudentTakenExam(int studentId, int examId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_CheckStudentExamTaken", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@ExamID", examId);

                    SqlParameter ret = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    ret.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    return ((int)ret.Value == 1);
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
        public static bool IsExamCurrentlyActive(int examId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetExamById", DBHelper.Instance.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamId", examId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count == 0)
                            return false;

                        DateTime examDate = Convert.ToDateTime(dt.Rows[0]["ExamDate"]);
                        TimeSpan start = (TimeSpan)dt.Rows[0]["StartTime"];
                        TimeSpan end = (TimeSpan)dt.Rows[0]["EndTime"];

                        DateTime startTime = examDate.Date + start;
                        DateTime endTime = examDate.Date + end;

                        DateTime now = DateTime.Now;

                        return now >= startTime && now <= endTime;  // ✔ Active exam
                    }
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


    }
}
