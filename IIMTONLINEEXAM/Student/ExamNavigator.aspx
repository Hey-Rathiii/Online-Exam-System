<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ExamNavigator.aspx.cs"
    Inherits="IIMTONLINEEXAM.Student.ExamNavigator"
    MasterPageFile="~/Student/Student.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .tab-btn {
            padding: 10px 25px;
            background: #333;
            color: white;
            border-radius: 20px;
            margin-right: 10px;
            border: none;
            cursor: pointer;
        }
        .tab-btn.active { background: #6366f1; }

        .card {
            background: #1e293b;
            padding: 15px;
            border-radius: 12px;
            margin-bottom: 15px;
            color: white;
        }
        .btn-start { background: #f59e0b; padding: 6px 15px; border:none; border-radius: 10px; color:white; }
        .btn-result { background: #3b82f6; padding: 6px 15px; border:none; border-radius: 10px; color:white; }
    </style>

    <h2 style="color:white; text-align:center; margin-top:20px;">Exam Navigator</h2>
    <br />

    <!-- TABS -->
    <asp:Button ID="btnCurrent" runat="server" Text="Current Exams"
        CssClass="tab-btn active" OnClick="btnCurrent_Click" />

    <asp:Button ID="btnUpcoming" runat="server" Text="Upcoming Exams"
        CssClass="tab-btn" OnClick="btnUpcoming_Click" />

    <asp:Button ID="btnPrevious" runat="server" Text="Previous Exams"
        CssClass="tab-btn" OnClick="btnPrevious_Click" />

    <br /><br />

    <!-- LIST VIEW -->
  <asp:Repeater ID="rptExams" runat="server" OnItemCommand="rptExams_ItemCommand">
    <ItemTemplate>
        <div class="card">
            <h3><%# Eval("ExamTitle") %></h3>
            <p><b>Subject:</b> <%# Eval("SubjectName") %></p>
            <p><b>Date:</b> <%# Eval("ExamDateText") %></p>
            <p><b>Time:</b> <%# Eval("StartTimeText") %> - <%# Eval("EndTimeText") %></p>

            <asp:Button ID="btnStart" runat="server" Text="Start Exam"
                CssClass="btn-start"
                Visible='<%# Eval("CanStart") %>'
                CommandName="StartExam"
                CommandArgument='<%# Eval("ExamID") %>' />

            <asp:Button ID="btnViewResult" runat="server" Text="View Result"
                CssClass="btn-result"
                Visible='<%# Eval("CanViewResult") %>'
                CommandName="ViewResult"
                CommandArgument='<%# Eval("ExamID") %>' />
        </div>
    </ItemTemplate>
</asp:Repeater>


</asp:Content>
