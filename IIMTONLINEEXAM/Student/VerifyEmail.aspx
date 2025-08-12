<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyEmail.aspx.cs" Inherits="IIMTONLINEEXAM.Student.VerifyEmail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Email Verification | IIMT Online Exam</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style>
        body, html {
            height: 100%;
            margin: 0;
            font-family: 'Segoe UI', sans-serif;
            background: linear-gradient(to right, #83a4d4, #b6fbff);
        }

        .verify-wrapper {
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }

        .verify-card {
            background-color: #ffffffee;
            padding: 40px;
            border-radius: 16px;
            max-width: 500px;
            width: 100%;
            box-shadow: 0 15px 35px rgba(0, 0, 0, 0.1);
            text-align: center;
            animation: fadeIn 0.6s ease-in-out;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }

        h1 {
            font-size: 26px;
            color: #2c3e50;
            margin-bottom: 20px;
        }

        .status-message {
            font-size: 16px;
            color: #2c3e50;
            font-weight: 500;
            margin-top: 15px;
        }

        .back-login {
            margin-top: 30px;
            display: inline-block;
            text-decoration: none;
            padding: 10px 20px;
            background-color: #2980b9;
            color: white;
            border-radius: 8px;
            font-weight: 600;
            transition: 0.3s;
        }

        .back-login:hover {
            background-color: #1f6391;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="verify-wrapper">
            <div class="verify-card">
                <h1>📩 Email Verification</h1>
                <p class="status-message">
                    <asp:Label ID="lblmsg" runat="server" />
                </p>

                <a href="Login.aspx" class="back-login">Go to Login Page</a>
            </div>
        </div>
    </form>
</body>
</html>
