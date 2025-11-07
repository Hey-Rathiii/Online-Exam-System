using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetValidUntilExpires(false);
                Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

                if (Request["__EVENTTARGET"] == "AutoSubmit")
                {
                    InsertQuestionAnswer();
                    // TODO: Save selected answers to session or DB
                    //Response.Write("<script>alert('Responses submitted!');</script>");
                }

                if (!IsPostBack)
            {
                //  Get ExamId from querystring (encrypted key)
                string encryptedExamId = Request.QueryString["key"];
                int examId = int.Parse(CryptoHelper.Decrypt(encryptedExamId));

                ViewState["ExamID"] = examId;

                // Start time for timer
                Session["ExamStartTime"] = DateTime.Now;

                    LoadQuestions(Convert.ToInt32(ViewState["ExamID"]));
            }
        }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show a message, etc.)
                Response.Write("<script>alert('An error occurred: " + ex.Message + "');</script>");
            }
        }

        //private void LoadQuestions()
        //{
        //    throw new NotImplementedException();
        //}

        private void LoadQuestions(int ExamID)
        {
            InsertQueDTO dto = new InsertQueDTO();
            dto.ExamId = ExamID;// Convert.ToInt32(ViewState["ExamID"]);

            DataTable questionsData = QuestionDAL.GetQuestionsByExamId(dto);

            if (questionsData != null && questionsData.Rows.Count > 0)
            {
                FormView1.DataSource = questionsData;
                FormView1.DataBind();
            }

            UpdateButtonsVisibility();
        }

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
                    int questionId = Convert.ToInt32(hf.Value);
                    if (SelectedAnswers.ContainsKey(questionId))
                    {
                        rbl.SelectedValue = SelectedAnswers[questionId];
                    }
                }
            }

            UpdateButtonsVisibility();
        }


        private void UpdateButtonsVisibility()
        {
            Button btnPrevious = (Button)FormView1.FindControl("btnPrevious");
            Button btnNext = (Button)FormView1.FindControl("btnNext");
            Button btnSubmit = (Button)FormView1.FindControl("btnSubmit");

            if (btnPrevious != null && btnNext != null && btnSubmit != null && FormView1.PageCount > 0)
            {
                btnPrevious.Visible = (FormView1.PageIndex > 0);
                btnNext.Visible = (FormView1.PageIndex < FormView1.PageCount - 1);
                btnSubmit.Visible = (FormView1.PageIndex == FormView1.PageCount - 1);
            }
        }




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
            // ✅ Collect answers
            int examId = Convert.ToInt32(ViewState["ExamID"]);
            int studentId = Convert.ToInt32(Session["StudentID"]);

            // Here you would loop through FormView items or keep answers in Session
            // Example: Save to database via stored procedure sp_SaveStudentAnswer

            Response.Redirect("Report.aspx?exam=" + CryptoHelper.Encrypt(examId.ToString()) +
                              "&student=" + CryptoHelper.Encrypt(studentId.ToString()));
        }

        private Dictionary<int, string> SelectedAnswers
        {
            get
            {
                if (Session["SelectedAnswers"] == null)
                {
                    Session["SelectedAnswers"] = new Dictionary<int, string>();
                }
                return (Dictionary<int, string>)Session["SelectedAnswers"];
            }
            set
            {
                Session["SelectedAnswers"] = value;
            }
        }

        private void SaveCurrentAnswer()
        {
            HiddenField hf = (HiddenField)FormView1.FindControl("hfQuestionID");
            RadioButtonList rbl = (RadioButtonList)FormView1.FindControl("rblOptions");

            if (hf != null && rbl != null && !string.IsNullOrEmpty(rbl.SelectedValue))
            {
                //int questionId = Convert.ToInt32(hf.Value);
                //SelectedAnswers[questionId] = rbl.SelectedValue;
                int questionId = Convert.ToInt32(hf.Value);
                string selected = rbl.SelectedValue;

                if (SelectedAnswers.ContainsKey(questionId))
                    SelectedAnswers[questionId] = selected;
                else
                    SelectedAnswers.Add(questionId, selected);
            }
        }

        private void insertAnswers()
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            int ExamID = Convert.ToInt32(ViewState["ExamID"]);

            foreach (KeyValuePair<int, string> entry in SelectedAnswers)
            {
                int questionId = entry.Key;
                string selectedOption = entry.Value;


                if (!string.IsNullOrEmpty(selectedOption))
                {
                    char selectedChar = selectedOption[0]; // Convert string to char
                    bool isInsert = StudentDAL.insertStudentAnswer(StudentID, ExamID, questionId, selectedChar);
                }
                //BindReport(StudentID, ExamID);

            }

        }
        private void InsertQuestionAnswer()
        {
            SaveCurrentAnswer();
            insertAnswers();

            int studentId = Convert.ToInt32(Session["StudentID"]);
            int examId = Convert.ToInt32(ViewState["ExamID"]);
            string encryptedExamId = CryptoHelper.Encrypt(examId.ToString());
            string encryptedStudentId = CryptoHelper.Encrypt(studentId.ToString());
            Response.Redirect($"Report.aspx?exam={encryptedExamId}&student={encryptedStudentId}");
            Response.Write("<script>alert('Responses submitted!');</script>");
        }


    }
}
