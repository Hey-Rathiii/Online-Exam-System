using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamClassLibrary.DAL;
using ExamClassLibrary.Model;
using ExamLibrary.DAL;
using OnlineExamSystem.Helper;
using static ExamClassLibrary.DAL.StudentAnswerDAL;

namespace IIMTONLINEEXAM.Student
{
    public partial class TakeExam : System.Web.UI.Page
    {
        private const string S_Questions = "DT_Questions";
        private const string S_Answers = "ANS_DICT";
        private int _examId;

        protected void Page_Load(object sender, EventArgs e)
        {

            int ExamID = HttpUtility.UrlDecode(Request.QueryString["examId"]) != null ? int.Parse(CryptoHelper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["examId"]))) : 0;
            _examId = ExamID;

            if (!IsPostBack)
            {
                LoadQuestions(_examId);
                BindFormView();
            }
        }

        private void LoadQuestions(int examId)
        {
            InsertQueDTO queDTO = new InsertQueDTO();
            queDTO.ExamId = examId;

            DataTable dt = QuestionDAL.GetQuestionsByExamId(queDTO);

            Session[S_Questions] = dt;
            Session[S_Answers] = new Dictionary<int, string>();
        }

        private DataTable CurrentQuestions()
        {
            return Session[S_Questions] as DataTable;
        }

        private Dictionary<int, string> Answers()
        {
            return Session[S_Answers] as Dictionary<int, string>;
        }

        private void BindFormView()
        {
            DataTable dt = CurrentQuestions();
            if (dt == null || dt.Rows.Count == 0)
            {
                lblProgress.Text = "No questions found.";
                fvQuestion.DataSource = null;
                fvQuestion.DataBind();
                btnPrev.Visible = btnNext.Visible = btnFinish.Visible = false;
                return;
            }

            if (fvQuestion.PageIndex < 0) fvQuestion.PageIndex = 0;
            if (fvQuestion.PageIndex > dt.Rows.Count - 1) fvQuestion.PageIndex = dt.Rows.Count - 1;

            fvQuestion.DataSource = dt;
            fvQuestion.DataBind();

            lblProgress.Text = $"Question {fvQuestion.PageIndex + 1} of {dt.Rows.Count}";

            // Button visibility control
            btnPrev.Visible = fvQuestion.PageIndex > 0;
            btnNext.Visible = fvQuestion.PageIndex < (dt.Rows.Count - 1);
            btnFinish.Visible = fvQuestion.PageIndex == (dt.Rows.Count - 1);
           // btnViewResult.Visible = fvQuestion.PageIndex == (dt.Rows.Count - 1); // ✅ Show only on last question
        }

        protected void fvQuestion_DataBound(object sender, EventArgs e)
        {
            if (fvQuestion.Row == null) return;

            var hf = (HiddenField)fvQuestion.FindControl("hfQuestionID");
            if (hf == null || string.IsNullOrWhiteSpace(hf.Value)) return;
            int qid = int.Parse(hf.Value);

            var rbA = (RadioButton)fvQuestion.FindControl("rbA");
            var rbB = (RadioButton)fvQuestion.FindControl("rbB");
            var rbC = (RadioButton)fvQuestion.FindControl("rbC");
            var rbD = (RadioButton)fvQuestion.FindControl("rbD");

            rbA.Checked = rbB.Checked = rbC.Checked = rbD.Checked = false;

            lblMsg.Text = ""; // Clear message for each question

            if (Answers().TryGetValue(qid, out string saved) && !string.IsNullOrEmpty(saved))
            {
                switch (saved)
                {
                    case "A": rbA.Checked = true; break;
                    case "B": rbB.Checked = true; break;
                    case "C": rbC.Checked = true; break;
                    case "D": rbD.Checked = true; break;
                }
                //lblMsg.Text = "Answer saved ✅"; // Show only if answer exists
            }
        }

        private void SaveCurrentSelection()
        {
            if (fvQuestion.Row == null) return;

            var hf = (HiddenField)fvQuestion.FindControl("hfQuestionID");
            var rbA = (RadioButton)fvQuestion.FindControl("rbA");
            var rbB = (RadioButton)fvQuestion.FindControl("rbB");
            var rbC = (RadioButton)fvQuestion.FindControl("rbC");
            var rbD = (RadioButton)fvQuestion.FindControl("rbD");

            if (hf == null || string.IsNullOrWhiteSpace(hf.Value)) return;

            int qid = int.Parse(hf.Value);
            string selected = rbA.Checked ? "A" : rbB.Checked ? "B" : rbC.Checked ? "C" : rbD.Checked ? "D" : null;

            if (!string.IsNullOrEmpty(selected))
            {
                Answers()[qid] = selected;
                lblMsg.Text = "A";
            }
            else
            {
                // Remove saved answer if nothing is selected
                if (Answers().ContainsKey(qid))
                {
                    Answers().Remove(qid);
                }
                lblMsg.Text = "";
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            SaveCurrentSelection();
            DataTable dt = CurrentQuestions();
            if (dt == null) return;

            if (fvQuestion.PageIndex < dt.Rows.Count - 1)
            {
                fvQuestion.PageIndex++;
                BindFormView();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            SaveCurrentSelection();
            if (fvQuestion.PageIndex > 0)
            {
                fvQuestion.PageIndex--;
                BindFormView();
            }
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            SaveCurrentSelection(); // Save last question answer

            DataTable dt = CurrentQuestions();
            if (dt == null || dt.Rows.Count == 0) return;

            // Make sure student ID is in session
            if (Session["StudentID"] == null)
            {
                lblMsg.Text = "⚠ Student not logged in.";
                return;
            }

            int studentId = Convert.ToInt32(Session["StudentID"]);

            // Loop through all questions in session and save answers
            foreach (DataRow row in dt.Rows)
            {
                int qid = Convert.ToInt32(row["QuestionID"]);
                string chosen = null;

                Answers().TryGetValue(qid, out chosen);

                if (!string.IsNullOrEmpty(chosen))
                {
                    StudentAnswerDAL.InsertStudentAnswer(studentId, _examId, qid, chosen[0]);
                }
                else
                {
                    // Save as null/empty if student didn't answer
                    StudentAnswerDAL.InsertStudentAnswer(studentId, _examId, qid, '\0');
                }
            }

            // ✅ Show success message instead of redirecting
            lblMsg.Text = "🎉 You have successfully submitted your exam!<br/>" +
                          "Do you want to see your result?";
            lblMsg.ForeColor = System.Drawing.Color.Green;
            btnViewResult.Visible = true;

            // Disable navigation buttons
            btnPrev.Visible = false;
            btnNext.Visible = false;
            btnFinish.Visible = false;

            
            //Response.Redirect($"ViewResult.aspx?examId={_examId}");
        }

        protected void fvQuestion_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {
            SaveCurrentSelection();
            fvQuestion.PageIndex = e.NewPageIndex;
            BindFormView();
        }

        protected void btnViewResult_Click(object sender, EventArgs e)
        {
            

            // Response.Redirect("~/Student/ViewResult.aspx?examId=" + _examId);

            string encryptedExamId = CryptoHelper.Encrypt(_examId.ToString());

           

            string url = $"~/Student/ViewResult.aspx?examId={HttpUtility.UrlEncode(encryptedExamId)}";
            Response.Redirect(url);


        }

    }
}
