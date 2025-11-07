<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowExams.aspx.cs" MasterPageFile="~/Student/Student.Master" Inherits="IIMTONLINEEXAM.Student.ShowExams" %>

<asp:Content ID="cntShowExams" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        body {
            background: linear-gradient(135deg, #87CEEB 0%, #4682B4 100%);
            font-family: 'Poppins', sans-serif;
            min-height: 100vh;
            margin: 0;
            padding: 0;
        }
        .exam-container {
            max-width: 1000px;
            margin: 50px auto;
            background: rgba(255, 255, 255, 0.95);
            border-radius: 20px;
            padding: 40px;
            box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
            backdrop-filter: blur(10px);
        }
        .exam-header h3 {
            color: #1E3A8A;
            font-weight: 700;
            font-size: 2.5rem;
            text-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            margin-bottom: 30px;
        }
        .message-label {
            font-size: 1.2rem;
            font-weight: 600;
            color: #DC2626;
        }
        .table-custom {
            border-radius: 15px;
            overflow: hidden;
            box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
        }
        .table-custom thead {
            background: linear-gradient(135deg, #1E40AF 0%, #3B82F6 100%);
            color: white;
            font-weight: 700;
        }
        .table-custom tbody tr {
            transition: all 0.3s ease;
            font-weight: 500;
        }
        .table-custom tbody tr:hover {
            background: linear-gradient(135deg, #93C5FD 0%, #60A5FA 50%);
            color: #1E3A8A;
            transform: scale(1.02);
        }
        .btn-custom {
            background: linear-gradient(135deg, #2563EB 0%, #1D4ED8 100%);
            color: white;
            border: none;
            padding: 8px 20px;
            border-radius: 25px;
            font-weight: 700;
            transition: all 0.3s ease;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
        }
        .btn-custom:hover {
            transform: translateY(-3px);
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.3);
            background: linear-gradient(135deg, #1E40AF 0%, #1E3A8A 100%);
        }
        .btn-disabled {
            background: gray !important;
            cursor: not-allowed !important;
            box-shadow: none !important;
        }
        @media (max-width: 768px) {
            .exam-container {
                margin: 20px;
                padding: 20px;
            }
            .exam-header h3 {
                font-size: 2rem;
            }
            .table-custom {
                font-size: 0.9rem;
            }
        }
    </style>

    <div class="container-fluid">
        <div class="exam-container">
            <div class="exam-header">
                <h3 class="text-center">📘 All Exams</h3>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="message-label text-danger d-block text-center mb-3"></asp:Label>

            <div class="table-responsive">
                <asp:GridView ID="gvExams" runat="server" AutoGenerateColumns="False" CssClass="table table-hover table-bordered text-center align-middle table-custom"
                    OnRowCommand="gvExams_RowCommand" EmptyDataText="No exams available." OnRowDataBound="gvExams_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="SubjectName" HeaderText="Subject" />
                        <asp:BoundField DataField="ExamTitle" HeaderText="Title" />
                        <asp:BoundField DataField="DurationMinutes" HeaderText="Duration (mins)" />

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkStartExam" runat="server" CssClass="btn btn-custom btn-sm"
                                    CommandName="StartExam"
                                    CommandArgument='<%# Eval("ExamID") %>'
                                    Text="Start Exam" />

                                <asp:LinkButton ID="lnkViewResult" runat="server" CssClass="btn btn-success btn-sm ms-2"
                                    CommandName="ViewResult"
                                    CommandArgument='<%# Eval("ExamID") %>'
                                    Text="View Result"
                                    Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
