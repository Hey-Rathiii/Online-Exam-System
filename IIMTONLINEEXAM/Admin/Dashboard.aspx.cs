using ExamClassLibrary.DAL;
using ExamLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamClassLibrary.Model;

namespace IIMTONLINEEXAM.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSubjects();
                LoadSubjectsInDropDown();
                LoadExams();

                LoadExamsinDropdown();

                int examId = Convert.ToInt32(ddlExams.SelectedValue);
                 LoadQuestionsByExamID(examId);
            }
        }

        #region Subject
        private void LoadSubjects()
        {
            SubjectDTO sub = new SubjectDTO();
            sub.statusFilter = -1; // -1 means all
            sub.AdminID = Convert.ToInt32(Session["AdminID"]); // Ensure AdminID from session
            DataTable dt = SubjectDAL.GetAllSubjects(sub); // -1 means all
            gvSubjects.DataSource = dt;
            gvSubjects.DataBind();

        }


        protected void btnAddSubject_Click(object sender, EventArgs e)
        {
            SubjectDTO subjectDTO = new SubjectDTO();
            string subjectName = txtSubjectName.Text.Trim();
            if (!string.IsNullOrEmpty(txtSubjectName.Text))
            {
                subjectDTO.SubjectName = txtSubjectName.Text;
                subjectDTO.CreatedBy = Convert.ToInt32(Session["AdminID"]); // Ensure AdminID from session
                int success = SubjectDAL.AddSubject(subjectDTO);
                if (success==1)
                {
                    lblMessage.Text = "Subject added successfully.";
                    lblMessage.CssClass = "text-success";
                }
                else if (success !=1)
                {
                    lblMessage.Text = "Failed to add subject.";
                    lblMessage.CssClass = "text-danger";
                }
                else
                {
                    lblMessage.Text = "Subject already exists.";
                    
                }
                txtSubjectName.Text = ""; // Clear input field
                LoadSubjects(); // Reload subjects grid
                LoadSubjectsInDropDown(); // Reload subjects dropdown
            }




        }

        protected void gvSubjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteSubject")
            {
                //int subjectId = Convert.ToInt32(e.CommandArgument);
                
                // Ensure AdminID from session
                SubjectDTO subjectDTO = new SubjectDTO();
                subjectDTO.SubjectID = Convert.ToInt32(e.CommandArgument);

                subjectDTO.AdminID = Convert.ToInt32(Session["AdminID"]); // Ensure AdminID from session
                //int adminId = Convert.ToInt32(Session["AdminID"]); // Ensure AdminID from session

                bool deleted = SubjectDAL.DeleteSubject(subjectDTO);

                if (deleted)
                {
                    lblMessage.Text = "Subject deleted successfully.";
                    lblMessage.CssClass = "text-success";
                }
                else
                {
                    lblMessage.Text = "Failed to delete subject. Make sure you have permissions.";
                    lblMessage.CssClass = "text-danger";
                }

                LoadSubjects();// Refresh GridView
               // LoadSubjectsInDropDown(); // Refresh dropdown if applicable
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
            //int subjectId = Convert.ToInt32(gvSubjects.DataKeys[e.RowIndex].Value);

            //GridViewRow row = gvSubjects.Rows[e.RowIndex];
            //string updatedName = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();

            //if (!string.IsNullOrEmpty(updatedName))
            //{
            //    bool updated = SubjectDAL.UpdateSubject(subjectId, updatedName);
            //    if (updated)
            //    {
            //        gvSubjects.EditIndex = -1;
            //        LoadSubjects();
            //    }
            //    else
            //    {
            //        // Optional: Show error message
            //    }
            //}
        }

        #endregion

        #region Exam
        private void LoadSubjectsInDropDown()
        {
            SubjectDTO subjectdto = new SubjectDTO();
            subjectdto.statusFilter = -1; // -1 means all
            subjectdto.adminId = Convert.ToInt32(Session["AdminID"]); // Ensure AdminID from session
            ddlSubjects.Items.Clear();
            DataTable subjects = SubjectDAL.GetAllSubjects(subjectdto);
            ddlSubjects.DataSource = subjects;
            ddlSubjects.DataTextField = "SubjectName";
            ddlSubjects.DataValueField = "SubjectID";
            ddlSubjects.DataBind();

            ddlSubjects.Items.Insert(0, new ListItem("-- Select Subject --", ""));
        }

        //This method returns the all list of the updated Subjects from the database by ajax call

        //[System.Web.Services.WebMethod]
        //public static List<string> GetSubjects()
        //{
        //    List<string> subjects = new List<string>();

        //    // Assuming SubjectDAL.GetSubjects(1) returns a DataTable with SubjectName column

        //    DataTable dtSubjects = SubjectDAL.GetSubjects(1, Convert.ToInt32(Session["AdminID"]));

        //    if (dtSubjects != null && dtSubjects.Rows.Count > 0)
        //    {
        //        foreach (DataRow row in dtSubjects.Rows)
        //        {
        //            if (row["SubjectName"] != DBNull.Value)
        //            {
        //                subjects.Add(row["SubjectName"].ToString());
        //            }
        //        }
        //    }

        //    return subjects;
        //}



        protected void LoadExams()
        {
            ExamDTO examDTO = new ExamDTO();
            examDTO.statusFilter = 1; // 0 for All, 1 for Active, 2 for Inactive
            examDTO.adminId = Convert.ToInt32(Session["AdminID"]); // Ensure AdminID from session
            DataTable dt = ExamDAL.GetExams(examDTO); // Pass object instead of raw parameters
            gvExams.DataSource = dt;
            gvExams.DataBind();

            if (gvExams.EditIndex >= 0)
            {
                GridViewRow editRow = gvExams.Rows[gvExams.EditIndex];
                DropDownList ddlSubjectEdit = (DropDownList)editRow.FindControl("ddlSubjectEdit");
                SubjectDTO subjectDTO = new SubjectDTO();
                subjectDTO.statusFilter = -1; // -1 means all
                subjectDTO.AdminID = Convert.ToInt32(Session["AdminID"]); // Ensure AdminID from session
                ddlSubjectEdit.DataSource = SubjectDAL.GetAllSubjects(subjectDTO);
                ddlSubjectEdit.DataTextField = "SubjectName";
                ddlSubjectEdit.DataValueField = "SubjectID";
                ddlSubjectEdit.DataBind();

                int selectedSubjectId = Convert.ToInt32(DataBinder.Eval(editRow.DataItem, "SubjectID"));
                ddlSubjectEdit.SelectedValue = selectedSubjectId.ToString();
            }
        }


        protected void btnAddExam_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlSubjects.SelectedValue))
            {
                // Optional: Show message to select subject
                return;
            }

            ExamDTO examDTO = new ExamDTO();
            examDTO.subjectId = Convert.ToInt32(ddlSubjects.SelectedValue);
            examDTO.subjectName = ddlSubjects.SelectedItem.Text;
            examDTO.examTitle = txtExamTitle.Text.Trim();
            examDTO.examDate = Convert.ToDateTime(txtExamDate.Text);
            examDTO.startTime = TimeSpan.Parse(txtStartTime.Text);
            examDTO.endTime = TimeSpan.Parse(txtEndTime.Text);
            examDTO.durationMinutes = int.Parse(txtDuration.Text);
            

            bool success = ExamDAL.InsertExam(examDTO);

            if (success)
            {
                // Optionally clear input fields
                txtExamTitle.Text = txtExamDate.Text = txtStartTime.Text = txtEndTime.Text = txtDuration.Text = "";
                LoadExams();
                LoadExamsinDropdown();
            }
        }

        protected void gvExams_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvExams.EditIndex = e.NewEditIndex;
            LoadExams(); // Reload exams in edit mode
        }

        protected void gvExams_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvExams.EditIndex = -1;
            LoadExams(); // Reload normal view
        }

        protected void gvExams_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvExams.Rows[e.RowIndex];
            ExamDTO examDTO = new ExamDTO();
            examDTO.examId= Convert.ToInt32(gvExams.DataKeys[e.RowIndex].Value);
            examDTO.subjectId = Convert.ToInt32(((DropDownList)row.FindControl("ddlSubjectEdit")).SelectedValue);
            examDTO.examTitle = ((TextBox)row.FindControl("txtExamTitleEdit")).Text;
            examDTO.examDate = Convert.ToDateTime(((TextBox)row.FindControl("txtExamDateEdit")).Text);
            examDTO.startTime = TimeSpan.Parse(((TextBox)row.FindControl("txtStartTimeEdit")).Text);
            examDTO.endTime = TimeSpan.Parse(((TextBox)row.FindControl("txtEndTimeEdit")).Text);
            examDTO.durationMinutes = Convert.ToInt32(((TextBox)row.FindControl("txtDurationEdit")).Text);

            

            bool updated = ExamDAL.UpdateExam(examDTO);

            gvExams.EditIndex = -1;
            LoadExams();
        }
#endregion


#region Questions
private void LoadExamsinDropdown()
{
     ExamDTO examDTO = new ExamDTO();
            examDTO.statusFilter = -1; // 0 for All, 1 for Active, 2 for Inactive
            examDTO.adminId = Convert.ToInt32(Session["AdminID"]); // Ensure AdminID from session
            DataTable dt = ExamDAL.GetExams(examDTO); ; // assumes you already created this method

    ddlExams.DataSource = dt;
    ddlExams.DataTextField = "ExamTitle";
    ddlExams.DataValueField = "ExamID";
    ddlExams.DataBind();

    ddlExams.Items.Insert(0, new ListItem("-- Select Exam --", "0"));
}

protected void btnSaveQuestion_Click(object sender, EventArgs e)
{
    //int examId = Convert.ToInt32(ddlExams.SelectedValue);
    //string question = txtQuestion.Text.Trim();
    //string a = txtOptionA.Text.Trim();
    //string b = txtOptionB.Text.Trim();
    //string c = txtOptionC.Text.Trim();
    //string d = txtOptionD.Text.Trim();
    //string correct = txtCorrectOption.Text.Trim().ToUpper();
    //int marks = Convert.ToInt32(txtMarks.Text.Trim());

    InsertQueDTO insert = new InsertQueDTO();

    insert.ExamId = Convert.ToInt32(ddlExams.SelectedValue);
    insert.QuestionText = txtQuestion.Text.Trim();
            insert.optionA = txtOptionA.Text.Trim();
            insert.optionB = txtOptionB.Text.Trim();
            insert.optionC = txtOptionC.Text.Trim();
            insert.optionD = txtOptionD.Text.Trim();
            insert.correctOption = txtCorrectOption.Text.Trim().ToUpper();
            insert.marks = Convert.ToInt32(txtMarks.Text.Trim());

    bool result = QuestionDAL.InsertQuestion(insert);

    if (result)
    {
        // clear form
        ddlExams.SelectedIndex = 0;
        txtQuestion.Text = txtOptionA.Text = txtOptionB.Text = txtOptionC.Text = txtOptionD.Text = txtCorrectOption.Text = txtMarks.Text = "";

        LoadQuestionsByExamID(insert.ExamId); // reload grid
    }
}
private void LoadQuestionsByExamID(int examID)
{
            InsertQueDTO dTO = new InsertQueDTO();
            dTO.ExamId = examID;
            DataTable dt = QuestionDAL.GetQuestionsByExamId(dTO); // returns all questions with ExamTitle if needed
    gvQuestions.DataSource = dt;
    gvQuestions.DataBind();
}

protected void ddlExams_SelectedIndexChanged(object sender, EventArgs e)
{
    int selectedExamId = Convert.ToInt32(ddlExams.SelectedValue);
    if (selectedExamId > 0)
    {
        LoadQuestionsByExamID(selectedExamId);
    }
    else
    {
        gvQuestions.DataSource = null;
        gvQuestions.DataBind();
    }
}

protected void gvQuestions_RowEditing(object sender, GridViewEditEventArgs e)
{
    gvQuestions.EditIndex = e.NewEditIndex;
    int selectedExamId = Convert.ToInt32(ddlExams.SelectedValue);
    LoadQuestionsByExamID(selectedExamId);
}

protected void gvQuestions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
{
    gvQuestions.EditIndex = -1;
    int selectedExamId = Convert.ToInt32(ddlExams.SelectedValue);
    LoadQuestionsByExamID(selectedExamId);
}

protected void gvQuestions_RowUpdating(object sender, GridViewUpdateEventArgs e)
{
    int selectedExamId = Convert.ToInt32(ddlExams.SelectedValue);
    int questionId = Convert.ToInt32(gvQuestions.DataKeys[e.RowIndex].Value);

    GridViewRow row = gvQuestions.Rows[e.RowIndex];
            InsertQueDTO updateDTO= new InsertQueDTO();

    updateDTO.QuestionText = ((TextBox)row.FindControl("txtQuestionText")).Text.Trim();
   updateDTO.optionA = ((TextBox)row.FindControl("txtOptionA")).Text.Trim();
    updateDTO.optionB = ((TextBox)row.FindControl("txtOptionB")).Text.Trim();
    updateDTO.optionC= ((TextBox)row.FindControl("txtOptionC")).Text.Trim();
    updateDTO.optionD = ((TextBox)row.FindControl("txtOptionD")).Text.Trim();
    updateDTO.correctOption = ((TextBox)row.FindControl("txtCorrect")).Text.Trim();
    updateDTO.marks = Convert.ToInt32(((TextBox)row.FindControl("txtMarks")).Text.Trim());

    QuestionDAL.UpdateQuestion(updateDTO);

    gvQuestions.EditIndex = -1;
    LoadQuestionsByExamID(selectedExamId);
}

protected void gvQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
{
            InsertQueDTO deleteDTO = new InsertQueDTO();
            deleteDTO.QuestionId = Convert.ToInt32(gvQuestions.DataKeys[e.RowIndex].Value);
            
    bool isDeleted = QuestionDAL.DeleteQuestion(deleteDTO); // Make sure this method exists

    if (isDeleted)
    {
        int selectedExamId = Convert.ToInt32(ddlExams.SelectedValue);
        LoadQuestionsByExamID(selectedExamId);
    }
    else
    {
        // Optional: Show error message if deletion fails
    }
}
    }
}
#endregion