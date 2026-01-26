<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ExamInstructions.aspx.cs"
    Inherits="IIMTONLINEEXAM.Student.ExamInstructions"
    MasterPageFile="~/Student/Student.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .card {
            background: #1e293b;
            color: white;
            padding: 30px;
            border-radius: 16px;
            max-width: 700px;
            margin: 40px auto;
        }
        .title {
            font-size: 28px;
            margin-bottom: 10px;
            font-weight: bold;
            color: #38bdf8;
            text-align: center;
        }
        .btn {
            background: #6366f1;
            border: none;
            padding: 12px 25px;
            border-radius: 10px;
            color: white;
            font-size: 18px;
            cursor: pointer;
            width: 100%;
            margin-top: 20px;
        }
        .btn[disabled] {
            background: gray;
            cursor: not-allowed;
        }
    </style>

    <div class="card">

        <div class="title">📘 Exam Instructions</div>

        <ul>
            <li>➡️ The exam will run in single-window mode.</li>
            <li>➡️ Don’t minimize or switch tabs more than 2 times.</li>
            <li>➡️ Don’t refresh or close the browser once started.</li>
            <li>➡️ Submit answers before timer ends.</li>
        </ul>

        <br />

        <asp:CheckBox ID="chkAgree" runat="server"
            Text=" I have read and agree to the instructions"
            AutoPostBack="true"
            OnCheckedChanged="chkAgree_CheckedChanged"
            ForeColor="White" />

        <br /><br />

        <asp:Button ID="btnStart" runat="server"
            Text="Start Exam"
            CssClass="btn"
            Enabled="false"
            OnClick="btnStart_Click" />

        <asp:Label ID="lblMsg" runat="server"
            ForeColor="Yellow" />
    </div>

</asp:Content>
