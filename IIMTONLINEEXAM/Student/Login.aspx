<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IIMTONLINEEXAM.Student.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   
    <title>Student Login | IIMT Online Exam</title>

    <!-- Bootstrap CDN -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style>
        html, body {
            height: 100%;
            margin: 0;
            font-family: 'Segoe UI', sans-serif;
            background: linear-gradient(to right, #83a4d4, #b6fbff);
        }

        .login-wrapper {
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }

        .login-card {
            background: #ffffffee;
            padding: 40px;
            border-radius: 16px;
            width: 100%;
            max-width: 450px;
            box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
            animation: fadeIn 0.6s ease-in-out;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }

        h2 {
            font-size: 26px;
            text-align: center;
            margin-bottom: 30px;
            color: #2c3e50;
        }

        .form-label {
            font-weight: 600;
        }

        .btn-primary {
            background-color: #2980b9;
            border: none;
        }

        .btn-primary:hover {
            background-color: #1f6391;
        }

        .register-link {
            text-align: center;
            margin-top: 20px;
        }

        .is-invalid {
            border-color: #e74c3c !important;
        }

        .text-danger, .text-success {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-wrapper">
            <div class="login-card">
                <h2>🔐 Student Login</h2>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter email" />
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter password" />
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-3 d-block" />

                <div class="register-link">
                    <span>Don’t have an account?</span> <a href="StudentRegistration.aspx">Register Here</a>
                </div>
                
            </div>
        </div>
    </form>

    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Client-side Validation -->
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= btnLogin.ClientID %>').click(function (e) {
                let email = $('#<%= txtEmail.ClientID %>').val().trim();
                let password = $('#<%= txtPassword.ClientID %>').val().trim();

                let isValid = true;
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

                $('.form-control').removeClass('is-invalid');

                if (email === '') {
                    $('#<%= txtEmail.ClientID %>').addClass('is-invalid').attr('placeholder', 'Email required');
                    isValid = false;
                } else if (!emailRegex.test(email)) {
                    $('#<%= txtEmail.ClientID %>').addClass('is-invalid').val('').attr('placeholder', 'Invalid email');
                    isValid = false;
                }

                if (password === '') {
                    $('#<%= txtPassword.ClientID %>').addClass('is-invalid').attr('placeholder', 'Password required');
                    isValid = false;
                }

                if (!isValid) {
                    e.preventDefault(); // stop postback
                }
            });
        });
    </script>
</body>
</html>
