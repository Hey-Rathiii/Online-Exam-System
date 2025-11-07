<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewResult.aspx.cs" Inherits="IIMTONLINEEXAM.Student.ViewResult" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>View Exam Result | IIMT Online Exam</title>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- ✅ Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- 🌸 Custom CSS Styling -->
    <style>
        body {
            background: linear-gradient(135deg, #e3f2fd, #f8f9fa);
            font-family: 'Segoe UI', sans-serif;
        }

        .container-box {
            background-color: #fff;
            border-radius: 15px;
            box-shadow: 0px 4px 20px rgba(0,0,0,0.1);
            padding: 30px;
            margin-top: 70px;
        }

        h2 {
            text-align: center;
            color: #0d6efd;
            font-weight: 700;
            margin-bottom: 25px;
        }

        .table {
            border-radius: 10px;
            overflow: hidden;
        }

        .table th {
            background-color: #0d6efd;
            color: white;
            text-align: center;
        }

        .table td {
            text-align: center;
            vertical-align: middle;
        }

        /* 🔹 Button Styles */
        .btn {
            border-radius: 25px;
            font-weight: 500;
            padding: 10px 20px;
            transition: all 0.3s ease;
        }

        .btn:hover {
            transform: scale(1.05);
            box-shadow: 0px 4px 10px rgba(0,0,0,0.2);
        }

        /* 🔹 Logout Button (Top Right) */
        .logout-btn {
            position: fixed;
            top: 20px;
            right: 30px;
            z-index: 1000;
            border-radius: 30px;
            font-weight: bold;
            padding: 8px 18px;
            background-color: #dc3545;
            color: white;
            border: none;
        }

        .logout-btn:hover {
            background-color: #bb2d3b;
            box-shadow: 0px 3px 10px rgba(255,0,0,0.3);
        }

        /* 🔹 Download Section */
        .download-section {
            display: flex;
            justify-content: center;
            gap: 15px;
            margin-top: 25px;
        }

        hr {
            margin: 30px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <!-- 🔴 Logout Button -->
        <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="logout-btn" OnClick="btnLogout_Click" />

        <div class="container container-box">
            <h2>Your Exam Report</h2>

            <!-- ✅ Exam Details Table -->
            <asp:GridView ID="gvExamReport" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped">
                <Columns>
                    <asp:BoundField DataField="QuestionText" HeaderText="Question" />
                    <asp:BoundField DataField="OptionA" HeaderText="Option A" />
                    <asp:BoundField DataField="OptionB" HeaderText="Option B" />
                    <asp:BoundField DataField="OptionC" HeaderText="Option C" />
                    <asp:BoundField DataField="OptionD" HeaderText="Option D" />
                    <asp:BoundField DataField="SelectedOption" HeaderText="Selected Option" />
                    <asp:BoundField DataField="CorrectAnsOpion" HeaderText="Correct Answer" />
                    <asp:BoundField DataField="SelectionStatus" HeaderText="Your Answer" />
                    <asp:BoundField DataField="MarksObtained" HeaderText="Marks" />
                </Columns>
            </asp:GridView>

            <!-- ✅ Summary Table -->
            <asp:GridView ID="GridViewStatics" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped">
                <Columns>
                    <asp:BoundField DataField="TotalMarksObtained" HeaderText="Total Marks" />
                    <asp:BoundField DataField="TotalPossibleMarks" HeaderText="Possible Marks" />
                    <asp:BoundField DataField="Percentage" HeaderText="Percentage" />
                    <asp:BoundField DataField="Result" HeaderText="Result" />
                </Columns>
            </asp:GridView>

            <!-- ✅ Download Buttons -->
            <div class="download-section">
                <%--<asp:Button ID="btnDownloadExcel" runat="server" Text="Download Excel" CssClass="btn btn-success" OnClick="btnDownloadExcel_Click" />--%>
                <asp:Button ID="btnDownloadCSV" runat="server" Text="Download CSV" CssClass="btn btn-info" OnClick="btnDownloadCSV_Click" />
                <asp:Button ID="btnDownloadPDF" runat="server" Text="Download PDF" CssClass="btn btn-danger" OnClick="btnDownloadPDF_Click" />
            </div>

            <hr />
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-3 d-block text-center"></asp:Label>
        </div>
    </form>
</body>
</html>
