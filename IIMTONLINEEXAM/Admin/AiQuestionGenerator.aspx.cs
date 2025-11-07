using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamClassLibrary.DAL;
using ExamClassLibrary.Model;
using ExamLibrary.DAL;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace IIMTONLINEEXAM.Admin
{
    public partial class AiQuestionGenerator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSubjects();
            }
        }

        private void LoadSubjects()
        {
            SubjectDTO dto = new SubjectDTO();
            dto.statusFilter = -1;
            dto.AdminID = Convert.ToInt32(Session["AdminID"]);

            DataTable dt = SubjectDAL.GetAllSubjects(dto);

            ddlSubjectsAI.DataSource = dt;
            ddlSubjectsAI.DataTextField = "SubjectName";
            ddlSubjectsAI.DataValueField = "SubjectID";
            ddlSubjectsAI.DataBind();
            ddlSubjectsAI.Items.Insert(0, "-- Select Subject --");
        }

        private void LoadExams()
        {
            ExamDTO dto = new ExamDTO();
            dto.statusFilter = -1;
            dto.adminId = Convert.ToInt32(Session["AdminID"]);

            DataTable dt = ExamDAL.GetExams(dto);

            ddlExamsAI.DataSource = dt;
            ddlExamsAI.DataTextField = "ExamTitle";
            ddlExamsAI.DataValueField = "ExamID";
            ddlExamsAI.DataBind();
            ddlExamsAI.Items.Insert(0, "-- Select Exam --");
        }

        protected void ddlSubjectsAI_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExams();
        }

        protected async void btnGenerate_Click(object sender, EventArgs e)
        {
            lblOutput.Text = "";

            if (ddlSubjectsAI.SelectedIndex <= 0)
            {
                lblOutput.Text = "⚠️ Please select a subject first!";
                return;
            }

            if (ddlExamsAI.SelectedIndex <= 0)
            {
                lblOutput.Text = "⚠️ Please select an exam first!";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTopic.Text))
            {
                lblOutput.Text = "⚠️ Enter a topic!";
                return;
            }

            string subject = ddlSubjectsAI.SelectedItem.Text;
            string topic = txtTopic.Text.Trim();

            string prompt = $@"
Generate 5 MCQ questions for the subject: {subject}, topic: {topic}.

Format:

Q1) Question text?
A) Option 1
B) Option 2
C) Option 3
D) Option 4
Answer: Random Alphabet

No explanation, no extra text.
";

            try
            {
                string apiKey = System.Configuration.ConfigurationManager.AppSettings["OpenAIKey"];
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    var body = new
                    {
                        model = "gpt-4.1-mini",
                        messages = new[] { new { role = "user", content = prompt } }
                    };

                    string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    lblOutput.Text = "⏳ Generating questions, please wait...";

                    HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();

                    JObject json = JObject.Parse(result);
                    string aiText = json["choices"][0]["message"]["content"].ToString();

                    string[] lines = aiText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    StringBuilder currentQuestion = new StringBuilder();
                    DataTable dtQuestions = new DataTable();
                    dtQuestions.Columns.Add("QuestionText");

                    foreach (string line in lines)
                    {
                        if (line.StartsWith("Q") && currentQuestion.Length > 0)
                        {
                            dtQuestions.Rows.Add(currentQuestion.ToString().Trim());
                            currentQuestion.Clear();
                        }
                        currentQuestion.AppendLine(line);
                    }

                    if (currentQuestion.Length > 0)
                        dtQuestions.Rows.Add(currentQuestion.ToString().Trim());

                    gvQuestions.DataSource = dtQuestions;
                    gvQuestions.DataBind();

                    lblOutput.Text = $"✅ Generated {dtQuestions.Rows.Count} questions. Select the ones you want and click 'Save Selected Questions'.";
                }
            }
            catch (Exception ex)
            {
                lblOutput.Text = "❌ Error: " + ex.Message;
            }
        }

        protected void btnSaveSelected_Click(object sender, EventArgs e)
        {
            var selectedQuestions = new System.Collections.Generic.List<string>();

            foreach (GridViewRow row in gvQuestions.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                Literal lit = (Literal)row.FindControl("litQuestion");

                if (cb != null && cb.Checked && lit != null)
                {
                    selectedQuestions.Add(lit.Text.Replace("<br/>", "\n"));
                }
            }

            if (selectedQuestions.Count == 0)
            {
                lblOutput.Text = "⚠️ Please select at least one question!";
                return;
            }

            int examId = Convert.ToInt32(ddlExamsAI.SelectedValue);
            int savedCount = 0;

            try
            {
                foreach (string qBlock in selectedQuestions)
                {
                    if (SaveQuestionToDatabase(qBlock, examId))
                        savedCount++;
                }

                gvQuestions.DataSource = null;
                gvQuestions.DataBind();

                lblOutput.Text = $"✅ {savedCount} questions saved successfully!";
            }
            catch (Exception ex)
            {
                lblOutput.Text = "❌ Error while saving: " + ex.Message;
            }
        }

        private bool SaveQuestionToDatabase(string questionBlock, int examId)
        {
            string[] lines = questionBlock.Split('\n');
            string questionText = "";
            string optionA = "", optionB = "", optionC = "", optionD = "", correctOption = "";
            int marks = 10;

            foreach (string line in lines)
            {
                string l = line.Trim();

                if (l.StartsWith("Q"))
                    questionText = l.Substring(l.IndexOf(")") + 1).Trim();
                else if (l.StartsWith("A)") || l.StartsWith("A."))
                    optionA = l.Substring(2).Trim();
                else if (l.StartsWith("B)") || l.StartsWith("B."))
                    optionB = l.Substring(2).Trim();
                else if (l.StartsWith("C)") || l.StartsWith("C."))
                    optionC = l.Substring(2).Trim();
                else if (l.StartsWith("D)") || l.StartsWith("D."))
                    optionD = l.Substring(2).Trim();
                else if (l.StartsWith("Answer:", StringComparison.OrdinalIgnoreCase))
                    correctOption = l.Substring(7).Trim();
            }

            if (string.IsNullOrEmpty(questionText) || string.IsNullOrEmpty(correctOption))
                return false;

            using (SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["DBConcetion"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertQuestion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamID", examId);
                    cmd.Parameters.AddWithValue("@QuestionText", questionText);
                    cmd.Parameters.AddWithValue("@OptionA", optionA);
                    cmd.Parameters.AddWithValue("@OptionB", optionB);
                    cmd.Parameters.AddWithValue("@OptionC", optionC);
                    cmd.Parameters.AddWithValue("@OptionD", optionD);
                    cmd.Parameters.AddWithValue("@CorrectOption", correctOption);
                    cmd.Parameters.AddWithValue("@Marks", marks);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return true;
        }
    }
}
