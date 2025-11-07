<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" MasterPageFile="~/Student/Student.Master" Inherits="IIMTONLINEEXAM.Student.Dashboard" %>

<asp:Content ID="StdDashboard" runat="server" ContentPlaceHolderID="ContentPlaceHolder1" >
        <link rel="stylesheet" href="../Styles/StudentDashBoard.css" />
    <div runat="server" id="examsContainer" class="examsContainer">
        <h1>Your Exams</h1>
    </div>
    <script type="text/javascript">
        // Refresh the page every 1 minute (60000 milliseconds)
        setInterval(function () {                                                                                                                                                       
            location.reload(); // Reloads the current page
        }, 60000);


        function showMessage(ExamID) {
            var key = ".msg_" + ExamID;
            document.querySelector(key).innerText = 'Exam should be started 5 minutes before.'
            setTimeout(function () {
                document.querySelector(key).innerText = ''
            }, 2000);

        }

        function showTimeUpMessage(ExamID) {
            var key = ".msg_" + ExamID;
            document.querySelector(key).innerText = "Time's Up This test is Live!";
            setTimeout(function () {

                document.querySelector(key).innerText = ''
            }, 2000);

        }

        function showStartMessage(ExamID) {
            var el = document.querySelector(".msg_" + ExamID);
            if (el) {
                el.innerText = "Now you can start your exam";
                setTimeout(function () {
                    el.innerText = '';
                }, 20000);
            } else {
                console.warn("Element not found: .msg_" + ExamID);
            }
        } 
    </script>

</asp:Content>
