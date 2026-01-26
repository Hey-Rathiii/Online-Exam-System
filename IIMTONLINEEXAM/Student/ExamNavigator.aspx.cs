using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using ExamClassLibrary.DAL;
using ExamClassLibrary.Model;

namespace IIMTONLINEEXAM.Student
{
    public partial class ExamNavigator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadExams("current");
        }

        // Button Events
        protected void btnCurrent_Click(object sender, EventArgs e)
        {
            SetActiveButton("current");
            LoadExams("current");
        }

        protected void btnUpcoming_Click(object sender, EventArgs e)
        {
            SetActiveButton("upcoming");
            LoadExams("upcoming");
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            SetActiveButton("previous");
            LoadExams("previous");
        }

        private void SetActiveButton(string tab)
        {
            btnCurrent.CssClass = "tab-btn";
            btnUpcoming.CssClass = "tab-btn";
            btnPrevious.CssClass = "tab-btn";

            if (tab == "current") btnCurrent.CssClass += " active";
            if (tab == "upcoming") btnUpcoming.CssClass += " active";
            if (tab == "previous") btnPrevious.CssClass += " active";
        }

        // Load Exam by tab category
        private void LoadExams(string category)
        {
            int studentId = Convert.ToInt32(Session["StudentID"]);

            ExamDTO dto = new ExamDTO
            {
                CreatedBy = -1,
                SubjectID = -1,
                StatusFilter = 1
            };

            DataTable dt = ExamDAL.GetExams(dto);
            List<ExamCard> list = new List<ExamCard>();

            DateTime now = DateTime.Now;

            foreach (DataRow row in dt.Rows)
            {
                DateTime examDate = Convert.ToDateTime(row["ExamDate"]);
                TimeSpan start = (TimeSpan)row["StartTime"];
                TimeSpan end = (TimeSpan)row["EndTime"];

                DateTime examStart = examDate.Date + start;
                DateTime examEnd = examDate.Date + end;

                string cat = examEnd < now ? "previous"
                          : examStart > now ? "upcoming"
                          : "current";

                if (cat != category) continue;

                list.Add(new ExamCard
                {
                    ExamID = Convert.ToInt32(row["ExamID"]),
                    SubjectName = row["SubjectName"].ToString(),
                    ExamTitle = row["ExamTitle"].ToString(),
                    ExamDateText = examDate.ToString("dd MMM yyyy"),
                    StartTimeText = examStart.ToString("hh:mm tt"),
                    EndTimeText = examEnd.ToString("hh:mm tt"),
                    CanStart = (cat == "current"),
                    CanViewResult = (cat == "previous" &&
                                     ExamDAL.HasStudentTakenExam(studentId, Convert.ToInt32(row["ExamID"])))
                });
            }

            rptExams.DataSource = list;
            rptExams.DataBind();
        }

        // Handle Item Commands
        protected void rptExams_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int examId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "StartExam")
                Response.Redirect("ExamInstructions.aspx?ExamID=" + examId);

            if (e.CommandName == "ViewResult")
                Response.Redirect("ViewResult.aspx?ExamID=" + examId);
        }
    }

    public class ExamCard
    {
        public int ExamID { get; set; }
        public string SubjectName { get; set; }
        public string ExamTitle { get; set; }
        public string ExamDateText { get; set; }
        public string StartTimeText { get; set; }
        public string EndTimeText { get; set; }
        public bool CanStart { get; set; }
        public bool CanViewResult { get; set; }
    }
}
