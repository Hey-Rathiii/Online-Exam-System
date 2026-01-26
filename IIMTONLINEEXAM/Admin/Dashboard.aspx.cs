using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamClassLibrary.Model;
using ExamClassLibrary.DAL;

namespace IIMTONLINEEXAM.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔐 Session check
            if (Session["AdminID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // ⭐ Check if user returned from AI Question Generator page
            // Example: Dashboard.aspx?tab=question
            string tabQuery = Request.QueryString["tab"];

            if (!IsPostBack)
            {
                // Load first time only
                LoadSubjects();
                LoadSubjectsInDropDown();
                LoadExamsDropdownForQuestions();

                gvExams.Visible = false;

                // ⭐ Restore tab via query parameter (from Back button)
                if (!string.IsNullOrEmpty(tabQuery))
                {
                    hdnActiveTab.Value = "#" + tabQuery;

                    string script =
                        $"(function(){{ const btn = document.querySelector(\"[data-bs-target='#{tabQuery}']\"); if(btn) new bootstrap.Tab(btn).show(); }})();";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "restTabQuery", script, true);
                }
            }
            else
            {
                // ⭐ Preserve selected exam in dropdown during postback
                string selectedExam = ddlExamsQuestion.SelectedValue;

                LoadExamsDropdownForQuestions();

                if (ddlExamsQuestion.Items.FindByValue(selectedExam) != null)
                    ddlExamsQuestion.SelectedValue = selectedExam;

                // ⭐ Restore tabs normally (user navigating inside dashboard)
                string last = hdnActiveTab.Value;

                if (!string.IsNullOrEmpty(last))
                {
                    string script =
                        $"(function(){{ const btn = document.querySelector(\"[data-bs-target='{last}']\"); if(btn) new bootstrap.Tab(btn).show(); }})();";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "restoreTab", script, true);
                }
            }
        }



        #region Subjects
        private void LoadSubjects()
        {
            var sub = new SubjectDTO
            {
                StatusFilter = -1,
                CreatedBy = Convert.ToInt32(Session["AdminID"])
            };

            DataTable dt = SubjectDAL.GetAllSubjects(sub);
            gvSubjects.DataSource = dt;
            gvSubjects.DataBind();
        }



        protected void btnAddSubject_Click(object sender, EventArgs e)
        {
            string subjectName = txtSubjectName.Text.Trim();

            SubjectDTO dto = new SubjectDTO
            {
                SubjectName = subjectName,
                CreatedBy = Convert.ToInt32(Session["AdminID"])
            };

            int result = SubjectDAL.AddSubject(dto);

            if (result == -1)
            {
                ShowSubjectMessage("danger", "Subject already exists.");
                return;
            }

            if (result == 1)
            {
                ShowSubjectMessage("success", "Subject added successfully!");
                txtSubjectName.Text = "";
                LoadSubjects();
                LoadSubjectsInDropDown();
                LoadExamsDropdownForQuestions();
            }
            else
            {
                ShowSubjectMessage("danger", "Failed to add subject.");
            }
        }

        protected void gvSubjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteSubject")
            {
                int sid = Convert.ToInt32(e.CommandArgument);
                SubjectDTO dto = new SubjectDTO
                {
                    SubjectID = sid,
                    CreatedBy = Convert.ToInt32(Session["AdminID"])
                };

                int res = SubjectDAL.DeleteSubject(dto);

                if (res == 1) ShowSubjectMessage("success", "Subject deleted successfully!");
                else if (res == -2) ShowSubjectMessage("danger", "Cannot delete subject — questions exist.");
                else if (res == -3) ShowSubjectMessage("danger", "Cannot delete subject — exams exist.");
                else ShowSubjectMessage("danger", "Failed to delete subject.");

                LoadSubjects();
                LoadSubjectsInDropDown();
                LoadExamsDropdownForQuestions();
            }
        }

        protected void gvSubjects_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSubjects.EditIndex = e.NewEditIndex;
            LoadSubjects();
        }

        protected void gvSubjects_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSubjects.EditIndex = -1;
            LoadSubjects();
        }

        protected void gvSubjects_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvSubjects.Rows[e.RowIndex];
            TextBox txtEdit = (TextBox)row.FindControl("txtEditSubject");
            int subjectId = Convert.ToInt32(gvSubjects.DataKeys[e.RowIndex].Value);

            SubjectDTO dto = new SubjectDTO
            {
                SubjectID = subjectId,
                SubjectName = txtEdit.Text.Trim()
            };

            int res = SubjectDAL.UpdateSubject(dto);

            if (res == 1) ShowSubjectMessage("success", "Subject updated successfully!");
            else if (res == -1) ShowSubjectMessage("danger", "Another subject with the same name already exists.");
            else ShowSubjectMessage("danger", "Failed to update subject.");

            gvSubjects.EditIndex = -1;
            LoadSubjects();
            LoadSubjectsInDropDown();
            LoadExamsDropdownForQuestions();
        }

        private void ShowSubjectMessage(string type, string text)
        {
            string script = $@"
                showInlineMsg('subjectError','{(type == "success" ? "success" : "danger")}','{text}');
            ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "subMsg", script, true);
        }
        #endregion

        #region Exams
        private void LoadSubjectsInDropDown()
        {
            SubjectDTO dto = new SubjectDTO { StatusFilter = -1, CreatedBy = Convert.ToInt32(Session["AdminID"]) };

            ddlSubjects.Items.Clear();
            DataTable dt = SubjectDAL.GetAllSubjects(dto);

            ddlSubjects.DataSource = dt;
            ddlSubjects.DataTextField = "SubjectName";
            ddlSubjects.DataValueField = "SubjectID";
            ddlSubjects.DataBind();

            ddlSubjects.Items.Insert(0, new ListItem("-- Select Subject --", ""));
        }

        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlSubjects.SelectedValue))
            {
                gvExams.Visible = false;
                return;
            }

            LoadExamsBySubject(Convert.ToInt32(ddlSubjects.SelectedValue));
        }

        private void LoadExamsBySubject(int subjectId)
        {
            ExamDTO dto = new ExamDTO
            {
                SubjectID = subjectId,
                CreatedBy = Convert.ToInt32(Session["AdminID"]),
                StatusFilter = -1
            };

            DataTable dt = ExamDAL.GetExams(dto);

            if (dt == null || dt.Rows.Count == 0)
            {
                gvExams.Visible = false;
                string script = $"showExamMsg('warning','No exams found for this subject');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", script, true);
                return;
            }

            gvExams.Visible = true;
            gvExams.DataSource = dt;
            gvExams.DataBind();
        }

        protected void btnAddExam_Click(object sender, EventArgs e)
        {
            // Server-side safety checks (client already validates)
            if (string.IsNullOrEmpty(ddlSubjects.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "m1", "showExamMsg('danger','Please select a subject');", true);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtExamTitle.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "m2", "showExamMsg('danger','Exam title is required');", true);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtExamDate.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "m3", "showExamMsg('danger','Exam date is required');", true);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtStartTime.Text) || string.IsNullOrWhiteSpace(txtEndTime.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "m4", "showExamMsg('danger','Start and End time are required');", true);
                return;
            }
          
            // parse safely
            if (!DateTime.TryParse(txtExamDate.Text, out DateTime examDate))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mdate", "showExamMsg('danger','Invalid exam date');", true);
                return;
            }

            if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan startTime) || !TimeSpan.TryParse(txtEndTime.Text, out TimeSpan endTime))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mtime", "showExamMsg('danger','Invalid time format');", true);
                return;
            }

            if (endTime <= startTime)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mtimediff", "showExamMsg('danger','End time must be after start time');", true);
                return;
            }
            int durationMinutes = (int)(endTime - startTime).TotalMinutes;
            txtDuration.Text = durationMinutes.ToString();  // reflect back

            int adminId = Convert.ToInt32(Session["AdminID"]);

            ExamDTO dto = new ExamDTO
            {
                SubjectID = Convert.ToInt32(ddlSubjects.SelectedValue),
                SubjectName = ddlSubjects.SelectedItem.Text,
                ExamTitle = txtExamTitle.Text.Trim(),
                ExamDate = examDate,
                StartTime = startTime,
                EndTime = endTime,
                DurationMinutes = durationMinutes,
                CreatedBy = adminId
            };

            if (ExamDAL.InsertExam(dto))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "m6", "showExamMsg('success','Exam added successfully');", true);
                // reset
                txtExamTitle.Text = txtExamDate.Text = txtStartTime.Text = txtEndTime.Text = txtDuration.Text = "";
                LoadExamsBySubject(dto.SubjectID);
                LoadExamsDropdownForQuestions();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "m7", "showExamMsg('danger','Failed to add exam');", true);
            }
        }

        protected void gvExams_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvExams.EditIndex = e.NewEditIndex;
            LoadExamsBySubject(Convert.ToInt32(ddlSubjects.SelectedValue));
        }

        protected void gvExams_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvExams.EditIndex = -1;
            LoadExamsBySubject(Convert.ToInt32(ddlSubjects.SelectedValue));
        }

        protected void gvExams_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvExams.Rows[e.RowIndex];

            int examId = Convert.ToInt32(gvExams.DataKeys[e.RowIndex]["ExamID"]);
            int subjectId = Convert.ToInt32(gvExams.DataKeys[e.RowIndex]["SubjectID"]);

            string title = ((TextBox)row.FindControl("txtExamTitleEdit")).Text.Trim();
            DateTime date = DateTime.Parse(((TextBox)row.FindControl("txtExamDateEdit")).Text);

            string sText = ((TextBox)row.FindControl("txtStartTimeEdit")).Text;
            string eText = ((TextBox)row.FindControl("txtEndTimeEdit")).Text;

            if (!TimeSpan.TryParse(sText, out TimeSpan startTime) ||
                !TimeSpan.TryParse(eText, out TimeSpan endTime))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "up1",
                    "showExamMsg('danger','Invalid time format.');", true);
                return;
            }

            if (endTime <= startTime)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "up2",
                    "showExamMsg('danger','End time must be after start time.');", true);
                return;
            }

            int duration = (int)(endTime - startTime).TotalMinutes;

            // 🛑 NEW VALIDATION — SAME RULE AS INSERT PAGE
            if (duration > 180)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "upDur",
                    "showExamMsg('danger','Exam duration cannot exceed 3 hours (180 minutes).');", true);
                return;
            }

            ExamDTO dto = new ExamDTO
            {
                ExamID = examId,
                SubjectID = subjectId,
                ExamTitle = title,
                ExamDate = date,
                StartTime = startTime,
                EndTime = endTime,
                DurationMinutes = duration
            };

            if (ExamDAL.UpdateExam(dto))
            {
                gvExams.EditIndex = -1;
                LoadExamsBySubject(subjectId);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "up3",
                    "showExamMsg('success','Exam updated successfully!');", true);
                LoadExamsDropdownForQuestions();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "up4",
                    "showExamMsg('danger','Failed to update exam.');", true);
            }
        }


        protected void gvExams_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleStatus")
            {
                string[] a = e.CommandArgument.ToString().Split('|');
                if (a.Length < 2) return;

                int examId = Convert.ToInt32(a[0]);
                bool current = Convert.ToBoolean(a[1].ToString().Trim());

                ExamDTO dto = new ExamDTO
                {
                    ExamID = examId,
                    IsActive = !current
                };

                if (ExamDAL.SetExamStatus(dto))
                {
                    LoadExamsBySubject(Convert.ToInt32(ddlSubjects.SelectedValue));
                    LoadExamsDropdownForQuestions();
                }
            }
        }
        #endregion

        #region Questions
        private void LoadExamsDropdownForQuestions()
        {
            ExamDTO exdto = new ExamDTO
            {
                StatusFilter = -1,
                CreatedBy = Convert.ToInt32(Session["AdminID"]),
                SubjectID = -1  // FIXED: -1 loads ALL subjects
            };

            DataTable dt = ExamDAL.GetExams(exdto);

            ddlExamsQuestion.Items.Clear();
            ddlExamsQuestion.Items.Add(new ListItem("-- Select Exam --", "0"));

            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    int id = Convert.ToInt32(r["ExamID"]);
                    string title = $"{r["SubjectName"]} - {r["ExamTitle"]}";

                    ddlExamsQuestion.Items.Add(new ListItem(title, id.ToString()));
                }
            }
        }





        protected void ddlExamsQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlExamsQuestion.SelectedValue, out int selectedExamId) && selectedExamId > 0)
            {
                LoadQuestionsByExamID(selectedExamId);
            }
            else
            {
                gvQuestions.DataSource = null;
                gvQuestions.DataBind();
            }
        }


        protected void btnSaveQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                // --- MINIMUM SAFETY CHECK (NO POPUP, NO VALIDATION) ---
                if (ddlExamsQuestion.SelectedIndex == 0)
                    return; // JS will already block this

                int marks = 0;
                if (!int.TryParse(txtMarks.Text.Trim(), out marks))
                    return; // JS already validated, just stop

                string correctOpt = txtCorrectOption.Text.Trim().ToUpper();
                if (correctOpt != "A" && correctOpt != "B" && correctOpt != "C" && correctOpt != "D")
                    return; // JS will handle
                            // ---------------------------------------------------------

                // DTO object
                InsertQueDTO dto = new InsertQueDTO
                {
                    ExamID = Convert.ToInt32(ddlExamsQuestion.SelectedValue),
                    QuestionText = txtQuestion.Text.Trim(),
                    OptionA = txtOptionA.Text.Trim(),
                    OptionB = txtOptionB.Text.Trim(),
                    OptionC = txtOptionC.Text.Trim(),
                    OptionD = txtOptionD.Text.Trim(),
                    CorrectOption = correctOpt,
                    Marks = marks
                };

                bool result = QuestionDAL.InsertQuestion(dto);

                if (result)
                {
                    ShowQuestionMessage("success", "Question added successfully!");

                    // keep exam selected
                    ddlExamsQuestion.SelectedValue = dto.ExamID.ToString();

                    // clear inputs
                    txtQuestion.Text = "";
                    txtOptionA.Text = "";
                    txtOptionB.Text = "";
                    txtOptionC.Text = "";
                    txtOptionD.Text = "";
                    txtCorrectOption.Text = "";
                    txtMarks.Text = "";

                    LoadQuestionsByExamID(dto.ExamID);
                }
                else
                {
                    ShowQuestionMessage("danger", "Failed to add question. Try again.");
                }
            }
            catch { }
        }



        private void ShowQuestionMessage(string type, string text)
        {
            string script = $"showInlineMsg('questionMsg','{(type == "success" ? "success" : "danger")}','{text}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msgQ", script, true);
        }

        private void LoadQuestionsByExamID(int examID)
        {
            ExamClassLibrary.Model.InsertQueDTO dto = new ExamClassLibrary.Model.InsertQueDTO();
            dto.ExamID = examID;

            DataTable dt = QuestionDAL.GetQuestionsByExamId(dto);

            gvQuestions.DataSource = dt;
            gvQuestions.DataBind();
        }


        protected void gvQuestions_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvQuestions.EditIndex = e.NewEditIndex;
            if (int.TryParse(ddlExamsQuestion.SelectedValue, out int examId) && examId > 0)
                LoadQuestionsByExamID(examId);
        }

        protected void gvQuestions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvQuestions.EditIndex = -1;
            if (int.TryParse(ddlExamsQuestion.SelectedValue, out int examId) && examId > 0)
                LoadQuestionsByExamID(examId);
        }

        protected void gvQuestions_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (!int.TryParse(ddlExamsQuestion.SelectedValue, out int examId)) return;

            int questionId = Convert.ToInt32(gvQuestions.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvQuestions.Rows[e.RowIndex];

            InsertQueDTO dto = new InsertQueDTO
            {
                QuestionID = questionId,
                ExamID = examId,
                QuestionText = ((TextBox)row.FindControl("txtQuestionText")).Text.Trim(),
                OptionA = ((TextBox)row.FindControl("txtA")).Text.Trim(),
                OptionB = ((TextBox)row.FindControl("txtB")).Text.Trim(),
                OptionC = ((TextBox)row.FindControl("txtC")).Text.Trim(),
                OptionD = ((TextBox)row.FindControl("txtD")).Text.Trim(),
                CorrectOption = ((TextBox)row.FindControl("txtCorrect")).Text.Trim(),
            };

            if (!int.TryParse(((TextBox)row.FindControl("txtMarks")).Text.Trim(), out int marks))
            {
                ShowQuestionMessage("danger", "Invalid marks value.");
                return;
            }
            dto.Marks = marks;

            if (QuestionDAL.UpdateQuestion(dto))
            {
                gvQuestions.EditIndex = -1;
                LoadQuestionsByExamID(examId);
                ShowQuestionMessage("success", "Question updated successfully!");
            }
            else
            {
                ShowQuestionMessage("danger", "Failed to update question.");
            }
        }

        protected void gvQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int qid = Convert.ToInt32(gvQuestions.DataKeys[e.RowIndex].Value);
            bool deleted = QuestionDAL.DeleteQuestion(qid);

            if (deleted) ShowQuestionMessage("success", "Question deleted successfully!");
            else ShowQuestionMessage("danger", "Failed to delete question.");

            if (int.TryParse(ddlExamsQuestion.SelectedValue, out int examId) && examId > 0)
                LoadQuestionsByExamID(examId);
        }
        #endregion
    }
}
