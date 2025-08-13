using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamLibrary.DAL;
using OnlineExamSystem.Helper;

namespace IIMTONLINEEXAM.Student
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadExams();
            }
        }

        private void LoadExams()
        {
            try
            {
                DataTable examsData = StudentDAL.GetAllExams();

                if (examsData.Rows.Count > 0)
                {
                    foreach (DataRow row in examsData.Rows)
                    {
                        string ExamID = row["ExamID"].ToString();
                        string SubjectName = row["SubjectName"].ToString();
                        string ExamTitle = row["ExamTitle"].ToString();
                        string ExamDate = row["ExamDate"].ToString();
                        string StartTime = row["StartTime"].ToString();
                        string EndTime = row["EndTime"].ToString();
                        string DurationInMinutes = row["DurationInMinutes"].ToString();

                        string disableAttribute = $"onclick=\"showMessage({ExamID})\"";


                        string startTime = StartTime;

                        // Format time
                        DateTime dt = DateTime.ParseExact(StartTime, "HH:mm:ss", null);
                        StartTime = dt.ToString("hh:mm tt");

                        dt = DateTime.ParseExact(EndTime, "HH:mm:ss", null);
                        EndTime = dt.ToString("hh:mm tt");

                        string ID = ExamID;
                        string EncryptedExamID = CryptoHelper.Encrypt(ExamID);
                        string EncodedKey = HttpUtility.UrlEncode(EncryptedExamID);
                        string url = $"ExamplatfeormNew.aspx?key={EncodedKey}";

                        if (DateTime.Now.Date == DateTime.Parse(ExamDate).Date)
                        {
                            string time = DateTime.Now.ToString("HH:mm");

                            int timeNow = (((int.Parse(startTime.Split(':')[0]) % 12 * 60) + (int.Parse(startTime.Split(':')[1]))) -
                                            ((int.Parse(time.Split(':')[0]) % 12 * 60) + (int.Parse(time.Split(':')[1]))));

                            Session["timeNow"] = timeNow;


                            if (timeNow == 5)
                            {
                                string script = $"showStartMessage({ID});";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "callFunction" + ID, script, true);
                            }

                            if (timeNow <= 5 && timeNow >= 0)
                            {
                                Session["timeNow"] = timeNow;
                                disableAttribute = $"onclick=\"window.location.href='{url}'\"";
                            }
                            if (timeNow <= 0)
                            {
                                disableAttribute = $"onclick=\"showTimeUpMessage({ID})\"";
                            }
                        }

                        string divContent = $@"
         <div class='exam' id='exam_{ExamID}'>
             <div class='subject'>
                 <h2>{SubjectName} ({ExamTitle})</h2>
             </div>
             <div class='secondDiv'>
                 <div>
                     <div class='date'>
                         <h3><strong>Exam Date :</strong> {ExamDate.Split(' ')[0]}</h3>
                     </div>
                     <p><strong>Start Time:</strong> {StartTime}</p>
                     <p><strong>End Time:</strong> {EndTime}</p>
                     <p><strong>Duration:</strong> {DurationInMinutes} minutes</p>
                 </div>
                 <div class='showMsg'>
                     <h5 class='msg_{ID}'></h5>
                 </div>
             </div>
             <div class='btnDiv'>
                 <button type='button' {disableAttribute} class='btnStart'>Start</button>
             </div>
         </div>";

                        if (DateTime.Parse(ExamDate) >= DateTime.Now.Date)
                        {
                            examsContainer.InnerHtml += divContent;
                        }

                    }
                }
                else
                {
                    examsContainer.InnerHtml = "<p>No exam available.</p>";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Log or handle error if needed
            }
        }
    }
}