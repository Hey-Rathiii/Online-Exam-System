<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakeExam.aspx.cs" Inherits="IIMTONLINEEXAM.Student.TakeExam" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Take Your Exam | IIMT Online Exam</title>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap & Fonts -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;600;700;800&display=swap" rel="stylesheet">

    <style>
        body {
            background: linear-gradient(180deg, #E0F2FE 0%, #BFDBFE 100%);
            font-family: 'Poppins', sans-serif;
            min-height: 100vh;
            margin: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            color: #1E3A8A;
        }

        .exam-container {
            width: 90%;
            max-width: 900px;
            background: #E0ECFF;
            border-radius: 18px;
            padding: 25px 30px;
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }

        .exam-header h2 {
            text-align: center;
            font-weight: 700;
            font-size: 1.8rem;
            color: #1E3A8A;
            margin-bottom: 15px;
        }

        .progress {
            width: 100%;
            height: 18px;
            border-radius: 10px;
            background: rgba(30, 58, 138, 0.1);
            margin-bottom: 20px;
        }

        .progress-bar-custom {
            background: linear-gradient(90deg, #3B82F6, #60A5FA);
            color: #fff;
            font-weight: 700;
            font-size: 0.8rem;
            transition: width 0.5s ease;
        }

        .question-card {
            width: 100%;
            background: linear-gradient(135deg, #BFDBFE, #93C5FD);
            border-radius: 14px;
            padding: 20px;
            text-align: center;
            color: #1E3A8A;
            box-shadow: 0 4px 15px rgba(30, 58, 138, 0.15);
        }

        .question-text {
            font-size: 1.3rem;
            font-weight: 600;
            margin-bottom: 15px;
        }

        .options-area {
            display: flex;
            flex-direction: column;
            gap: 8px;
            align-items: center;
        }

        .options {
            width: 80%;
            background: #DBEAFE;
            border-radius: 10px;
            padding: 10px 12px;
            text-align: left;
            transition: all 0.3s ease;
            cursor: pointer;
            border: 1px solid transparent;
        }

        .options:hover {
            background: #BFDBFE;
            border: 1px solid #3B82F6;
            transform: translateY(-2px);
        }

        .options input[type="radio"] {
            accent-color: #1E40AF;
            transform: scale(1.2);
            margin-right: 8px;
        }

        .options label {
            font-size: 1rem;
            font-weight: 600;
            color: #1E3A8A;
            cursor: pointer;
        }

        .marks-info {
            margin-top: 10px;
            font-weight: 600;
            font-size: 0.95rem;
            color: #1E3A8A;
        }

        .button-group {
            margin-top: 20px;
            text-align: center;
        }

        .btn-custom {
            background: linear-gradient(135deg, #3B82F6, #1E40AF);
            border: none;
            color: white;
            font-weight: 600;
            border-radius: 25px;
            padding: 8px 20px;
            font-size: 0.9rem;
            margin: 8px;
            transition: all 0.3s ease;
            box-shadow: 0 4px 12px rgba(30, 58, 138, 0.3);
        }

        .btn-custom:hover {
            transform: translateY(-2px);
            background: linear-gradient(135deg, #2563EB, #1E40AF);
        }

        .message-area {
            margin-top: 15px;
            text-align: center;
            font-weight: 600;
            font-size: 1rem;
            color: #1E3A8A;
        }

        @media (max-width: 768px) {
            .exam-container { width: 95%; padding: 20px; }
            .options { width: 100%; }
            .question-text { font-size: 1.1rem; }
            .btn-custom { padding: 6px 15px; font-size: 0.85rem; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="exam-container">
            <div class="exam-header">
                <h2>📝 Take Your Exam</h2>
            </div>

            <div class="progress">
                <div class="progress-bar progress-bar-custom" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                    <asp:Label ID="lblProgress" runat="server" />
                </div>
            </div>

            <asp:FormView ID="fvQuestion" runat="server"
                AllowPaging="true"
                DataKeyNames="QuestionID"
                PagerSettings-Visible="false"
                OnDataBound="fvQuestion_DataBound"
                OnPageIndexChanging="fvQuestion_PageIndexChanging">
                <ItemTemplate>
                    <div class="question-card">
                        <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />
                        <div class="question-text"><%# Eval("QuestionText") %></div>

                        <div class="options-area">
                            <div class="options"><asp:RadioButton ID="rbA" runat="server" GroupName="opt" Text='<%# Eval("OptionA") %>' /></div>
                            <div class="options"><asp:RadioButton ID="rbB" runat="server" GroupName="opt" Text='<%# Eval("OptionB") %>' /></div>
                            <div class="options"><asp:RadioButton ID="rbC" runat="server" GroupName="opt" Text='<%# Eval("OptionC") %>' /></div>
                            <div class="options"><asp:RadioButton ID="rbD" runat="server" GroupName="opt" Text='<%# Eval("OptionD") %>' /></div>
                        </div>

                        <div class="marks-info">
                            Marks: <%# Eval("Marks") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:FormView>

            <div class="button-group">
                <asp:Button ID="btnPrev" runat="server" Text="⬅ Previous" CssClass="btn btn-custom" OnClick="btnPrev_Click" />
                <asp:Button ID="btnNext" runat="server" Text="Next ➡" CssClass="btn btn-custom" OnClick="btnNext_Click" />
                <asp:Button ID="btnFinish" runat="server" Text="Finish ✅" CssClass="btn btn-custom"
                    OnClientClick="return confirm('Are you sure you want to submit your exam?');"
                    OnClick="btnFinish_Click" />
            </div>

            <asp:Button ID="btnViewResult" runat="server" Text="View Result"
                CssClass="btn btn-success mt-3"
                Visible="False"
                OnClick="btnViewResult_Click" />

            <div class="message-area">
                <asp:Label ID="lblMsg" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
