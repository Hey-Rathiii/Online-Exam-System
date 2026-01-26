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

namespace IIMTONLINEEXAM.Student
{
    public partial class ExamPlatform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Prevent refresh cheating
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

                if (Request["__EVENTTARGET"] == "AutoSubmit")
                {
                    InsertQuestionAnswer();
                }

                if (!IsPostBack)
                {
                    string encryptedExamId = Request.QueryString["key"];
                    int examId = int.Parse(CryptoHelper.Decrypt(encryptedExamId));

                    ViewState["ExamID"] = examId;
                    Session["ExamStartTime"] = DateTime.Now;

                    LoadQuestions(examId);
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('An error occurred: " + ex.Message + "');</script>");
            }
        }

        // ---------------------------------------------------------
        // LOAD QUESTIONS
        // ---------------------------------------------------------
        private void LoadQuestions(int examID)
        {
            InsertQueDTO dto = new InsertQueDTO
            {
                ExamID = examID
            };

            DataTable questionsData = QuestionDAL.GetQuestionsByExamId(dto);

            if (questionsData != null && questionsData.Rows.Count > 0)
            {
                FormView1.DataSource = questionsData;
                FormView1.DataBind();
            }

            UpdateButtonsVisibility();
        }

        // ---------------------------------------------------------
        // FORMVIEW BOUND — SET OPTIONS & RESTORE SELECTION
        // ---------------------------------------------------------
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            if (FormView1.CurrentMode == FormViewMode.ReadOnly && FormView1.DataItem != null)
            {
                DataRowView row = (DataRowView)FormView1.DataItem;

                RadioButtonList rbl = (RadioButtonList)FormView1.FindControl("rblOptions");
                if (rbl != null)
                {
                    rbl.Items.Clear();
                    rbl.Items.Add(new ListItem(row["OptionA"].ToString(), "A"));
                    rbl.Items.Add(new ListItem(row["OptionB"].ToString(), "B"));
                    rbl.Items.Add(new ListItem(row["OptionC"].ToString(), "C"));
                    rbl.Items.Add(new ListItem(row["OptionD"].ToString(), "D"));
                }

                HiddenField hf = (HiddenField)FormView1.FindControl("hfQuestionID");

                if (hf != null && rbl != null)
                {
                    int qId = Convert.ToInt32(hf.Value);

                    if (SelectedAnswers.ContainsKey(qId))
                        rbl.SelectedValue = SelectedAnswers[qId];
                }
            }

            UpdateButtonsVisibility();
        }

        // ---------------------------------------------------------
        // BUTTON VISIBILITY CONTROL
        // ---------------------------------------------------------
        private void UpdateButtonsVisibility()
        {
            Button btnPrevious = (Button)FormView1.FindControl("btnPrevious");
            Button btnNext = (Button)FormView1.FindControl("btnNext");
            Button btnSubmit = (Button)FormView1.FindControl("btnSubmit");

            if (btnPrevious != null && btnNext != null && btnSubmit != null)
            {
                btnPrevious.Visible = (FormView1.PageIndex > 0);
                btnNext.Visible = (FormView1.PageIndex < FormView1.PageCount - 1);
                btnSubmit.Visible = (FormView1.PageIndex == FormView1.PageCount - 1);
            }
        }

        // ---------------------------------------------------------
        // PAGE NAVIGATION
        // ---------------------------------------------------------
        protected void FormView1_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {
            SaveCurrentAnswer();
            FormView1.PageIndex = e.NewPageIndex;
            LoadQuestions(Convert.ToInt32(ViewState["ExamID"]));
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer();
            if (FormView1.PageIndex < FormView1.PageCount - 1)
            {
                FormView1.PageIndex++;
                LoadQuestions(Convert.ToInt32(ViewState["ExamID"]));
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            SaveCurrentAnswer();
            if (FormView1.PageIndex > 0)
            {
                FormView1.PageIndex--;
                LoadQuestions(Convert.ToInt32(ViewState["ExamID"]));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int examId = Convert.ToInt32(ViewState["ExamID"]);
            int studentId = Convert.ToInt32(Session["StudentID"]);

            InsertQuestionAnswer();
        }

        // ---------------------------------------------------------
        // STORED SELECTED ANSWERS IN SESSION
        // ---------------------------------------------------------
        private Dictionary<int, string> SelectedAnswers
        {
            get
            {
                if (Session["SelectedAnswers"] == null)
                    Session["SelectedAnswers"] = new Dictionary<int, string>();

                return (Dictionary<int, string>)Session["SelectedAnswers"];
            }
            set
            {
                Session["SelectedAnswers"] = value;
            }
        }

        // SAVE CURRENT QUESTION ANSWER
        private void SaveCurrentAnswer()
        {
            HiddenField hf = (HiddenField)FormView1.FindControl("hfQuestionID");
            RadioButtonList rbl = (RadioButtonList)FormView1.FindControl("rblOptions");

            if (hf != null && rbl != null && !string.IsNullOrEmpty(rbl.SelectedValue))
            {
                int questionId = Convert.ToInt32(hf.Value);
                string selected = rbl.SelectedValue;

                if (SelectedAnswers.ContainsKey(questionId))
                    SelectedAnswers[questionId] = selected;
                else
                    SelectedAnswers.Add(questionId, selected);
            }
        }

        // ---------------------------------------------------------
        // INSERT ANSWERS TO DB
        // ---------------------------------------------------------
        private void InsertQuestionAnswer()
        {
            SaveCurrentAnswer();

            int studentId = Convert.ToInt32(Session["StudentID"]);
            int examId = Convert.ToInt32(ViewState["ExamID"]);

            foreach (var entry in SelectedAnswers)
            {
                int qId = entry.Key;
                char selected = entry.Value[0];

                StudentDAL.insertStudentAnswer(studentId, examId, qId, selected);
            }

            // Redirect to report
            string encryptedExamId = CryptoHelper.Encrypt(examId.ToString());
            string encryptedStudentId = CryptoHelper.Encrypt(studentId.ToString());

            Response.Redirect($"Report.aspx?exam={encryptedExamId}&student={encryptedStudentId}");
        }
    }
}
