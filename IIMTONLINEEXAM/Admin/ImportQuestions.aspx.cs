using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using ExamClassLibrary.DAL;
using ExamClassLibrary.Model;

namespace IIMTONLINEEXAM.Admin
{
    public partial class ImportQuestions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
                LoadSubjects();
        }

        private void LoadSubjects()
        {
            ddlSubject.Items.Clear();
            ddlSubject.Items.Add(new ListItem("-- Select --", "0"));

            var dto = new SubjectDTO
            {
                CreatedBy = Convert.ToInt32(Session["AdminID"]),
                StatusFilter = -1
            };

            DataTable dt = SubjectDAL.GetAllSubjects(dto);

            foreach (DataRow row in dt.Rows)
                ddlSubject.Items.Add(new ListItem(row["SubjectName"].ToString(), row["SubjectID"].ToString()));
        }

        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlExam.Items.Clear();
            ddlExam.Items.Add(new ListItem("-- Select --", "0"));

            var dto = new ExamDTO
            {
                SubjectID = Convert.ToInt32(ddlSubject.SelectedValue),
                CreatedBy = Convert.ToInt32(Session["AdminID"]),
                StatusFilter = -1
            };

            DataTable dt = ExamDAL.GetExams(dto);

            foreach (DataRow row in dt.Rows)
                ddlExam.Items.Add(new ListItem(row["ExamTitle"].ToString(), row["ExamID"].ToString()));
        }

        // ---------- Extract PDF ----------
        private string ExtractPdf(string path)
        {
            var sb = new System.Text.StringBuilder();

            using (PdfReader reader = new PdfReader(path))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    string text = PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy());
                    sb.AppendLine(text);
                }
            }

            return sb.ToString();
        }

        // ---------- Extract Button ----------
        protected void btnExtract_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (!filePDF.HasFile)
            {
                lblMsg.Text = "Upload a PDF first.";
                return;
            }

            string ext = System.IO.Path.GetExtension(filePDF.FileName).ToLower();
            if (ext != ".pdf")
            {
                lblMsg.Text = "Only PDF allowed.";
                return;
            }

            string tempDir = Server.MapPath("~/TempUploads");
            if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

            string fileTemp = System.IO.Path.Combine(tempDir, Guid.NewGuid().ToString() + ".pdf");
            filePDF.SaveAs(fileTemp);

            string text = ExtractPdf(fileTemp);
            File.Delete(fileTemp);

            List<QuestionModel> list = Parse(text);

            gvQuestions.DataSource = list;
            gvQuestions.DataBind();

            lblMsg.Text = $"Extracted {list.Count} questions.";
        }


        // ---------- MODEL ----------
        public class QuestionModel
        {
            public string QuestionText { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string CorrectOption { get; set; }
        }


        // ---------- FINAL PARSER WITH CORRECT ANSWER FIX ----------
        private List<QuestionModel> Parse(string text)
        {
            List<QuestionModel> list = new List<QuestionModel>();

            text = text.Replace("\r", "");
            string[] lines = text.Split('\n');

            QuestionModel q = null;
            string currentOpt = null;

            foreach (string raw in lines)
            {
                string line = raw.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Detect question start
                if (Regex.IsMatch(line, @"^(Q?\d+)\.\s"))
                {
                    if (q != null) list.Add(q);

                    q = new QuestionModel();
                    q.CorrectOption = "";
                    q.QuestionText = Regex.Replace(line, @"^(Q?\d+)\.\s*", "").Trim();
                    currentOpt = null;
                    continue;
                }

                if (q == null) continue;

                // Detect correct answer anywhere
                var mCorrect = Regex.Match(line, @"Correct Answer[:\- ]*\s*([A-D])", RegexOptions.IgnoreCase);
                if (mCorrect.Success)
                {
                    q.CorrectOption = mCorrect.Groups[1].Value.ToUpper();
                    line = Regex.Replace(line, @"Correct Answer[:\- ]*\s*[A-D]", "", RegexOptions.IgnoreCase).Trim();
                }

                // Detect options A B C D
                var m = Regex.Match(line, @"^([A-D])\)\s*(.*)");
                if (m.Success)
                {
                    currentOpt = m.Groups[1].Value.ToUpper();
                    string val = m.Groups[2].Value.Trim();

                    if (currentOpt == "A") q.A = val;
                    if (currentOpt == "B") q.B = val;
                    if (currentOpt == "C") q.C = val;
                    if (currentOpt == "D") q.D = val;

                    continue;
                }

                // Continuation lines
                if (currentOpt == null)
                {
                    q.QuestionText += " " + line;
                }
                else
                {
                    if (currentOpt == "A") q.A += " " + line;
                    if (currentOpt == "B") q.B += " " + line;
                    if (currentOpt == "C") q.C += " " + line;
                    if (currentOpt == "D") q.D += " " + line;
                }
            }

            if (q != null) list.Add(q);

            return list;
        }


        // ---------- SAVE ALL ----------
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int saved = 0, failed = 0;
            int examId = Convert.ToInt32(ddlExam.SelectedValue);

            foreach (GridViewRow row in gvQuestions.Rows)
            {
                Label lblQ = (Label)row.FindControl("lblQ");
                Label lblA = (Label)row.FindControl("lblA");
                Label lblB = (Label)row.FindControl("lblB");
                Label lblC = (Label)row.FindControl("lblC");
                Label lblD = (Label)row.FindControl("lblD");
                Label lblCorrect = (Label)row.FindControl("lblCorrect");

                InsertQueDTO dto = new InsertQueDTO
                {
                    ExamID = examId,
                    QuestionText = lblQ.Text,
                    OptionA = lblA.Text,
                    OptionB = lblB.Text,
                    OptionC = lblC.Text,
                    OptionD = lblD.Text,
                    CorrectOption = lblCorrect.Text,
                    Marks = 1
                };

                try
                {
                    if (QuestionDAL.InsertQuestion(dto)) saved++;
                    else failed++;
                }
                catch
                {
                    failed++;
                }
            }

            lblMsg.Text = $"Saved: {saved}, Failed: {failed}";
        }
    }
}
