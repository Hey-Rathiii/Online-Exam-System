using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineExamSystem.Helper;
using ExamClassLibrary.DAL;


namespace IIMTONLINEEXAM.Student
{
    public partial class ShowExams : Page
    {
        string connStr = @"Data Source=.;Initial Catalog=IIMTONLINEEXAM;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindExams();
            }
        }

        private void BindExams()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllExams", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            if (dt.Rows.Count > 0)
            {
                gvExams.DataSource = dt;
                gvExams.DataBind();
            }
            else
            {
                gvExams.DataSource = null;
                gvExams.DataBind();
                lblMessage.Text = "No exams available.";
            }
        }

        protected void gvExams_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int examId = 0;
            if (!int.TryParse(e.CommandArgument.ToString(), out examId))
            {
                lblMessage.Text = "Invalid exam selected!";
                return;
            }

            if (Session["StudentID"] == null)
            {
                lblMessage.Text = "Please login to continue.";
                return;
            }

            int studentId = (int)Session["StudentID"];

            if (e.CommandName == "StartExam")
            {
                // Redirect to TakeExam page with encrypted examId
                string encryptedExamId = CryptoHelper.Encrypt(examId.ToString());
                string urlExamId = HttpUtility.UrlEncode(encryptedExamId);
                Response.Redirect($"TakeExam.aspx?examId={urlExamId}");
            }
            else if (e.CommandName == "ViewResult")
            {
                // Redirect to ViewResult page with encrypted examId and studentId
                string encryptedExamId = CryptoHelper.Encrypt(examId.ToString());
                string encryptedStudentId = CryptoHelper.Encrypt(studentId.ToString());
                string urlExamId = HttpUtility.UrlEncode(encryptedExamId);
                string urlStudentId = HttpUtility.UrlEncode(encryptedStudentId);
                Response.Redirect($"ViewResult.aspx?examId={urlExamId}&studentId={urlStudentId}");
            }
        }

        protected void gvExams_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["StudentID"] == null)
                {
                    lblMessage.Text = "Please login to start or view exams.";
                    return;
                }

                int studentId = (int)Session["StudentID"];

                // Get ExamID from the current row's DataItem
                int examId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ExamID"));

                // Find buttons in the current row
                LinkButton lnkStartExam = (LinkButton)e.Row.FindControl("lnkStartExam");
                LinkButton lnkViewResult = (LinkButton)e.Row.FindControl("lnkViewResult");

                // Check if student has already taken this exam
                bool hasTaken = ExamDAL.HasStudentTakenExam(studentId, examId);

                if (hasTaken)
                {
                    // Disable Start Exam button
                    lnkStartExam.Enabled = false;
                    lnkStartExam.CssClass += " btn-disabled";

                    // Show View Result button
                    lnkViewResult.Visible = true;
                }
                else
                {
                    lnkStartExam.Enabled = true;
                    lnkStartExam.CssClass = lnkStartExam.CssClass.Replace("btn-disabled", "").Trim();

                    lnkViewResult.Visible = false;
                }
            }
        }
    }
}
