using System;

namespace IIMTONLINEEXAM.Student
{
    public partial class ExamInstructions : System.Web.UI.Page
    {
        protected int ExamId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // First time page loads → examId MUST come from QueryString
                if (Request.QueryString["ExamID"] == null)
                {
                    Response.Redirect("ExamNavigator.aspx");
                    return;
                }

                ViewState["ExamID"] = Request.QueryString["ExamID"];
            }

            // After postback, always use ViewState
            if (ViewState["ExamID"] == null)
            {
                Response.Redirect("ExamNavigator.aspx");
                return;
            }

            ExamId = Convert.ToInt32(ViewState["ExamID"]);
        }

        protected void chkAgree_CheckedChanged(object sender, EventArgs e)
        {
            btnStart.Enabled = chkAgree.Checked;
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            Response.Redirect("TakeExam.aspx?ExamID=" + ViewState["ExamID"].ToString());
        }
    }
}
