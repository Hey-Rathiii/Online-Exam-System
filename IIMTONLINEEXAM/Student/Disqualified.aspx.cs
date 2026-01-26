using System;
using ExamClassLibrary.DAL;

namespace IIMTONLINEEXAM.Student
{
    public partial class Disqualified : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // BLOCK DIRECT ACCESS
            if (Session["StudentID"] == null)
                Response.Redirect("~/Student/Login.aspx");

            if (Session["Disqualified"] == null)
                Response.Redirect("ExamNavigator.aspx");

            string examIdString = Request.QueryString["examId"];
            if (string.IsNullOrEmpty(examIdString))
                Response.Redirect("ExamNavigator.aspx");

            int studentId = Convert.ToInt32(Session["StudentID"]);
            int examId = Convert.ToInt32(examIdString);

            // UPDATE DB — Mark disqualified only once
            if (Session["DisqualifiedSaved"] == null)
            {
                StudentResultDAL.MarkStudentAsDisqualified(studentId, examId);
                Session["DisqualifiedSaved"] = true;
            }
        }
    }
}
