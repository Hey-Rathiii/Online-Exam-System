using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using ExamClassLibrary.DAL;
using ExamClassLibrary.Model;

namespace IIMTONLINEEXAM.Student
{
    public partial class TakeExam : System.Web.UI.Page
    {
        private const string S_Questions = "DT_Questions";
        private const string S_Answers = "ANS_DICT";
        private const string S_EndTime = "EXAM_END";

        private const int DEFAULT_MINUTES = 30;

        private int examId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StudentID"] == null)
                Response.Redirect("~/Student/Login.aspx");

            if (Session["Disqualified"] != null)
                Response.Redirect("Disqualified.aspx");

            string id = Request.QueryString["ExamID"];
            if (id == null) Response.Redirect("ExamNavigator.aspx");

            examId = Convert.ToInt32(id);

            if (!IsPostBack)
            {
                DateTime end;

                if (Session[S_EndTime] == null)
                {
                    end = DateTime.Now.AddMinutes(DEFAULT_MINUTES);
                    Session[S_EndTime] = end;
                }
                else end = (DateTime)Session[S_EndTime];

                hfEndTime.Value = end.ToString("o");

                LoadQuestions();
                BindFormView();
            }
            else
            {
                if (Session[S_EndTime] != null)
                    hfEndTime.Value = ((DateTime)Session[S_EndTime]).ToString("o");
            }
        }

        private void LoadQuestions()
        {
            InsertQueDTO dto = new InsertQueDTO { ExamID = examId };
            DataTable dt = QuestionDAL.GetQuestionsByExamId(dto);

            Session[S_Questions] = dt;
            Session[S_Answers] = new Dictionary<int, string>();
        }

        private DataTable Questions => Session[S_Questions] as DataTable;
        private Dictionary<int, string> Answers => Session[S_Answers] as Dictionary<int, string>;

        private void BindFormView()
        {
            DataTable dt = Questions;

            fvQuestion.DataSource = dt;
            fvQuestion.DataBind();

            lblProgress.Text = $"{fvQuestion.PageIndex + 1} / {dt.Rows.Count}";

            btnPrev.Visible = fvQuestion.PageIndex > 0;
            btnNext.Visible = fvQuestion.PageIndex < dt.Rows.Count - 1;
            btnFinish.Visible = fvQuestion.PageIndex == dt.Rows.Count - 1;
        }

        protected void fvQuestion_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {
            SaveAnswer();
            fvQuestion.PageIndex = e.NewPageIndex;
            BindFormView();
        }

        protected void fvQuestion_DataBound(object sender, EventArgs e)
        {
            if (fvQuestion.Row == null) return;

            HiddenField hf = fvQuestion.FindControl("hfQuestionID") as HiddenField;
            int qid = Convert.ToInt32(hf.Value);

            if (Answers.TryGetValue(qid, out string ans))
            {
                ((RadioButton)fvQuestion.FindControl("rbA")).Checked = ans == "A";
                ((RadioButton)fvQuestion.FindControl("rbB")).Checked = ans == "B";
                ((RadioButton)fvQuestion.FindControl("rbC")).Checked = ans == "C";
                ((RadioButton)fvQuestion.FindControl("rbD")).Checked = ans == "D";
            }
        }

        private void SaveAnswer()
        {
            if (fvQuestion.Row == null) return;

            int qid = Convert.ToInt32(((HiddenField)fvQuestion.FindControl("hfQuestionID")).Value);

            string selected =
                ((RadioButton)fvQuestion.FindControl("rbA")).Checked ? "A" :
                ((RadioButton)fvQuestion.FindControl("rbB")).Checked ? "B" :
                ((RadioButton)fvQuestion.FindControl("rbC")).Checked ? "C" :
                ((RadioButton)fvQuestion.FindControl("rbD")).Checked ? "D" : null;

            var ans = Answers;

            if (selected == null) ans.Remove(qid);
            else ans[qid] = selected;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            SaveAnswer();
            fvQuestion.PageIndex++;
            BindFormView();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            SaveAnswer();
            fvQuestion.PageIndex--;
            BindFormView();
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            SaveAnswer();

            int sid = Convert.ToInt32(Session["StudentID"]);
            DataTable dt = Questions;

            int total = 0, scored = 0;

            foreach (DataRow row in dt.Rows)
            {
                int qid = Convert.ToInt32(row["QuestionID"]);
                int marks = Convert.ToInt32(row["Marks"]);
                string correct = row["CorrectOption"].ToString();

                total += marks;

                Answers.TryGetValue(qid, out string sel);
                if (!string.IsNullOrEmpty(sel) && sel == correct)
                    scored += marks;

                char ans = string.IsNullOrEmpty(sel) ? '\0' : sel[0];

                StudentAnswerDAL.InsertStudentAnswer(sid, examId, qid, ans);
            }

            decimal percent = (decimal)scored * 100 / total;

            StudentResultDAL.SaveFinalResult(sid, examId, scored, total, percent);

            lblMsg.Text = "🎉 Exam submitted successfully!";
            btnPrev.Visible = btnNext.Visible = btnFinish.Visible = false;
            btnViewResult.Visible = true;
        }

        protected void btnViewResult_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Student/ViewResult.aspx?ExamID={examId}");
        }
    }
}
