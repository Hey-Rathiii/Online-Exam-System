using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using ExamClassLibrary.DAL;
using OnlineExamSystem.Helper;
using OfficeOpenXml;
//using OfficeOpenXml.License;        
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace IIMTONLINEEXAM.Student
{
    public partial class ViewResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            

            if (!IsPostBack)
            {
                int studentId = 0;
                int examId = 0;

                // Get studentId and examId from QueryString or Session (set these before redirecting here)
                if (Session["StudentID"] != null && Request.QueryString["ExamID"] != null)
                {

                    studentId = Convert.ToInt32(Session["StudentID"]);
                    string decExamId = CryptoHelper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["ExamID"]));
                    int.TryParse(decExamId, out examId);

                    if (studentId > 0 && examId > 0)
                    {
                        BindExamReport(studentId, examId);
                    }
                    else
                    {
                        lblMessage.Text = "Invalid Student ID or Exam ID.";
                    }
                }
                else
                {
                    lblMessage.Text = "Student ID or Exam ID is missing.";
                }
            }
        }

        private void BindExamReport(int studentId, int examId)
        {
  
               DataSet ds=   StudentResultDAL.GetStudentExamReport(studentId, examId);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvExamReport.DataSource = ds.Tables[0];
                gvExamReport.DataBind();
                lblMessage.Text = "";

                GridViewStatics.DataSource = ds.Tables[1];
                GridViewStatics.DataBind(); 
            }
            else
            {
                lblMessage.Text = "No exam report found for the given Student and Exam.";
            }
            
        }


        //protected void btnDownloadExcel_Click(object sender, EventArgs e)
        //{
        //    ExportToExcel("ExamReport.xlsx");
        //}

        protected void btnDownloadCSV_Click(object sender, EventArgs e)
        {
            ExportToCSV("ExamReport.csv");
        }

        protected void btnDownloadPDF_Click(object sender, EventArgs e)
        {
            ExportToPDF("ExamReport.pdf");
        }

        //private void ExportToExcel(string fileName)
        //{
        //    DataTable dt = GetReportData();
        //    if (dt == null) return;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using (ExcelPackage pck = new ExcelPackage())
        //    {
        //        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Exam Report");
        //        ws.Cells["A1"].LoadFromDataTable(dt, true);
        //       // ws.Cells[ws.Dimension.Address].AutoFitColumns();

        //        Response.Clear();
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //        Response.BinaryWrite(pck.GetAsByteArray());
        //        Response.End();
        //    }
        //}

        private void ExportToCSV(string fileName)
        {
            DataTable dt = GetReportData();
            if (dt == null) return;

            StringBuilder sb = new StringBuilder();

            // Add header
            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            // Add rows
            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").ToArray();
                sb.AppendLine(string.Join(",", fields));
            }

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.Write(sb.ToString());
            Response.End();
        }

        private void ExportToPDF(string fileName)
        {
            DataTable dt = GetReportData();
            if (dt == null) return;

            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
                PdfWriter.GetInstance(pdfDoc, ms);
                pdfDoc.Open();

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                foreach (DataColumn c in dt.Columns)
                {
                    table.AddCell(new Phrase(c.ColumnName));
                }

                foreach (DataRow r in dt.Rows)
                {
                    foreach (var cell in r.ItemArray)
                    {
                        table.AddCell(new Phrase(cell.ToString()));
                    }
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }

        private DataTable GetReportData()
        {
            if (gvExamReport.DataSource is DataTable dt)
                return dt;

            // If grid data was databound earlier
            DataTable data = new DataTable();
            foreach (TableCell cell in gvExamReport.HeaderRow.Cells)
                data.Columns.Add(cell.Text);

            foreach (GridViewRow row in gvExamReport.Rows)
            {
                DataRow dr = data.NewRow();
                for (int i = 0; i < row.Cells.Count; i++)
                    dr[i] = row.Cells[i].Text;
                data.Rows.Add(dr);
            }
            return data;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear session and redirect to login page
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Student/Login.aspx");
        }

    }
}
