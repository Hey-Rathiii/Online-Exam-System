<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamPlatform.aspx.cs" Inherits="IIMTONLINEEXAM.Student.ExamPlatform" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exam Platform</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, sans-serif;
            margin: 0;
            padding: 0;
            background: linear-gradient(135deg, #eef2f3, #d9e4f5);
        }

        /* Container styling */
        .container {
            max-width: 1000px;
            margin: 40px auto;
            padding: 20px;
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1);
        }

        /* Countdown Box */
        .countdown-box {
            text-align: center;
            background: #fdf6e4;
            border: 2px dashed #f0a500;
            padding: 30px;
            border-radius: 12px;
            margin-bottom: 25px;
            animation: fadeIn 1.5s ease-in-out;
        }

        .countdown-box h1 {
            color: #444;
            font-size: 1.6rem;
            margin-bottom: 10px;
        }

        .CountDown {
            font-size: 2.5rem;
            color: #f57c00;
            font-weight: bold;
            animation: pulse 1.2s infinite;
        }

        @keyframes pulse {
            0% { transform: scale(1); }
            50% { transform: scale(1.05); }
            100% { transform: scale(1); }
        }

        /* Instructions Section */
        .exam-instructions {
            background: #f0f8ff;
            border-left: 6px solid #0077b6;
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 20px;
        }

        .exam-instructions h2 {
            color: #0077b6;
            margin-bottom: 15px;
        }

        .exam-instructions ul li {
            margin-bottom: 8px;
            color: #333;
        }

        /* Question Section */
        #questionSection {
            display: none;
            animation: fadeIn 1.2s ease-in;
        }

        h3 {
            color: #333;
        }

        .btn {
            padding: 8px 15px;
            border-radius: 6px;
            border: none;
            margin-right: 10px;
            font-size: 14px;
            cursor: pointer;
        }

        .btn:hover {
            opacity: 0.9;
        }

        .btn-primary {
            background-color: #0077b6;
            color: #fff;
        }

        .btn-secondary {
            background-color: #f0a500;
            color: #fff;
        }

        .btn-success {
            background-color: #4caf50;
            color: #fff;
        }

        /* Timer Label */
        #timerLabel {
            font-size: 18px;
            font-weight: bold;
            color: #d32f2f;
        }

        @keyframes fadeIn {
            0% { opacity: 0; transform: translateY(10px); }
            100% { opacity: 1; transform: translateY(0); }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Countdown Section -->
            <div id="countdownContainer" class="countdown-box">
                <h1>Your Exam will start soon. Please wait...</h1>
                <div class="CountDown">
                    <p class="time">0<%= Session["timeNow"]+":00" %></p>
                </div>
            </div>

            <!-- Instructions -->
            <div class="exam-instructions">
                <h2>📢 Important Exam Guidelines</h2>
                <ul>
                    <li><strong>Be Ready:</strong> The exam will auto-start after the countdown ends.</li>
                    <li><strong>No Refresh:</strong> Do not refresh or close the browser during the exam.</li>
                    <li><strong>Stable Internet:</strong> Ensure a good connection for smooth experience.</li>
                    <li><strong>Single Attempt:</strong> You can attempt only once.</li>
                    <li><strong>Auto-Submit:</strong> The system will submit automatically when time is over.</li>
                </ul>
                <p style="color:#d32f2f; font-weight:bold;">Violation of rules may lead to cancellation of your attempt.</p>
            </div>

            <!-- Question Section -->
            <div id="questionSection">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <div style="margin-bottom: 15px;">
                    Time Remaining: <span id="timerLabel"></span>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <%--<asp:FormView ID="FormView1" runat="server" DataKeyNames="QuestionID" AllowPaging="false"
                            >
                            <ItemTemplate>
                                <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />
                                <div style="padding: 20px;">
                                    <h3><%# Eval("QuestionText") %></h3>
                                    <asp:RadioButtonList ID="rblOptions" runat="server" />
                                    <br />
                                    <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-secondary" OnClick="btnPrevious_Click" />
                                    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnNext_Click" />
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" Visible="false" CssClass="btn btn-success" OnClick="btnSubmit_Click" />
                                </div>
                            </ItemTemplate>
                        </asp:FormView>--%>

                        <asp:FormView ID="FormView1" runat="server" AllowPaging="true"
    OnDataBound="FormView1_DataBound"
    OnPageIndexChanging="FormView1_PageIndexChanging">

    <ItemTemplate>
        <div class="question-container">
            <!-- Question -->
            <h3>Q. <%# Eval("QuestionText") %></h3>

            <!-- Hidden field to keep QuestionId -->
            <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />

            <!-- Options -->
            <asp:RadioButtonList ID="rblOptions" runat="server" RepeatDirection="Vertical" />
        </div>

        <!-- Navigation Buttons -->
        <div class="navigation-buttons">
            <asp:Button ID="btnPrevious" runat="server" Text="Previous"
                OnClick="btnPrevious_Click" />
            <asp:Button ID="btnNext" runat="server" Text="Next"
                OnClick="btnNext_Click" />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit"
                OnClick="btnSubmit_Click" />
        </div>
    </ItemTemplate>

</asp:FormView>


                        <div id="divReport" runat="server">
                            <asp:GridView ID="gvQuestionReport" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />
                            <br />
                            <asp:GridView ID="gvSummaryReport" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>

    <!-- JavaScript for Countdown & Timer -->
    <script type="text/javascript">
        var endTime;
        var timerInterval;
        window.onload = function () {
            if (!window.endTime) {
                endTime = new Date('<%= ((DateTime)Session["ExamStartTime"]).AddHours(1).ToString("MM/dd/yyyy HH:mm:ss") %>');
            }
            startTimer();
        };

        function startTimer() {
            var timerInterval = setInterval(function () {
                var now = new Date().getTime();
                var distance = endTime - now;

                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                document.getElementById("timerLabel").innerHTML = minutes + "m " + seconds + "s ";

                if (distance < 0) {
                    clearInterval(timerInterval);
                    document.getElementById("timerLabel").innerHTML = "Time's up!";
                    alert("Time's up! Submitting exam...");
                    __doPostBack('AutoSubmit', '');
                }
            }, 1000);
        }

        const startTime = <%= Session["timeNow"] %>;
        let time = startTime * 60;
        let showTime = document.querySelector(".time");
        const countdownContainer = document.getElementById("countdownContainer");
        const questionSection = document.getElementById("questionSection");

        const timer = setInterval(updateCountdown, 1000);

        function updateCountdown() {
            let minutes = Math.floor(time / 60);
            let seconds = time % 60;

            minutes = minutes < 10 ? '0' + minutes : minutes;
            seconds = seconds < 10 ? '0' + seconds : seconds;

            showTime.innerHTML = `${minutes}:${seconds}`;
            time--;
            if (time < 0) {
                clearInterval(timer);
                countdownContainer.style.display = "none";
                questionSection.style.display = "block";
            }
        }
    </script>
</body>
</html>
