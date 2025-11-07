<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRegister.aspx.cs" Inherits="IIMTONLINEEXAM.Admin.AdminRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Registration | IIMT Online Exam</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Toastify -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/toastify-js/src/toastify.min.css" />
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style>
        body {
            height: 100%;
            margin: 0;
            font-family: 'Segoe UI', sans-serif;
            background: linear-gradient(to right, #ff9a9e, #fad0c4);
        }

        .form-wrapper {
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }

        .register-card {
            background: #ffffffea;
            padding: 40px;
            border-radius: 20px;
            width: 100%;
            max-width: 480px;
            box-shadow: 0 15px 40px rgba(0, 0, 0, 0.15);
            animation: fadeIn 0.7s ease-in-out;
            position: relative;
        }

        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(30px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        h1 {
            text-align: center;
            font-size: 28px;
            color: #2c3e50;
            margin-bottom: 30px;
            font-weight: bold;
        }

            h1::before {
                content: "🛡️ ";
            }

        .form-label {
            font-weight: 600;
            color: #34495e;
        }

        .form-control {
            border-radius: 10px;
        }

        .btn-primary {
            background-color: #c0392b;
            border: none;
            border-radius: 10px;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

            .btn-primary:hover {
                background-color: #96281b;
            }

        .login-link {
            text-align: center;
            margin-top: 18px;
            font-size: 14px;
        }

            .login-link a {
                color: #c0392b;
                text-decoration: none;
                font-weight: 600;
            }

        .is-invalid {
            border-color: #e74c3c !important;
        }

        .password-wrapper {
            position: relative;
        }

            .password-wrapper .form-control {
                padding-right: 42px;
                height: 44px;
            }

        .toggle-password {
            position: absolute;
            top: 50%;
            right: 14px;
            transform: translateY(-50%);
            cursor: pointer;
            font-size: 18px;
            color: #555;
            z-index: 2;
        }

        .toggle-switch {
            position: absolute;
            top: 78px;
            right: 20px;
            cursor: pointer;
            font-size: 20px;
            color: #444;
            z-index: 5;
        }

        .dark-mode {
            background: #1a1a1a !important;
            color: #eee !important;
        }

            .dark-mode .form-control {
                background: #333 !important;
                color: #fff;
                border: 1px solid #555;
            }

            .dark-mode .register-card {
                background: #2a2a2a !important;
            }

            .dark-mode .form-label {
                color: #ccc !important;
            }

            .dark-mode .btn-primary {
                background-color: #e74c3c !important;
            }

            .dark-mode .toggle-switch {
                color: #ccc;
            }

        @media (max-width: 576px) {
            .register-card {
                padding: 30px 20px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-wrapper">
            <div class="register-card">
                <i class="bi bi-moon-stars toggle-switch" id="darkToggle" title="Toggle Dark Mode"></i>
                <h1>Admin Registration</h1>

                <div class="mb-3">
                    <label for="txtFullName" class="form-label">Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Placeholder="Enter Full Name" />
                </div>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter email" />
                </div>

                <div class="mb-3 password-wrapper">
                    <label for="txtPassword" class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Create password" />
                    <i class="bi bi-eye-slash toggle-password" toggle="<%= txtPassword.ClientID %>"></i>
                </div>

                <div class="mb-3 password-wrapper">
                    <label for="txtConfirmPassword" class="form-label">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm password" />
                    <i class="bi bi-eye-slash toggle-password" toggle="<%= txtConfirmPassword.ClientID %>"></i>
                </div>

                <div class="mb-3">
                    <label for="txtContact" class="form-label">Contact Number</label>
                    <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" placeholder="Enter contact number" />
                </div>

                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary w-100 mt-2" OnClick="btnRegister_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="d-none"></asp:Label>

                <div class="login-link">
                    <small>Already registered? <a href="Login.aspx">Sign In</a></small>
                </div>
            </div>
        </div>
    </form>

    <!-- Scripts -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/toastify-js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            // Toggle password
            $(".toggle-password").click(function () {
                const inputId = $(this).attr("toggle");
                const input = $("#" + inputId);
                const isPassword = input.attr("type") === "password";
                input.attr("type", isPassword ? "text" : "password");
                $(this).toggleClass("bi-eye bi-eye-slash");
            });

            // Toggle dark mode
            $("#darkToggle").click(function () {
                $("body").toggleClass("dark-mode");
            });

            // Client-side validation
            $('#<%= btnRegister.ClientID %>').on("click", function (e) {
                let isValid = true;

                const nameBox = $('#<%= txtFullName.ClientID %>');
                const emailBox = $('#<%= txtEmail.ClientID %>');
                const passwordBox = $('#<%= txtPassword.ClientID %>');
                const confirmPasswordBox = $('#<%= txtConfirmPassword.ClientID %>');
                const contactBox = $('#<%= txtContact.ClientID %>');

                const name = nameBox.val().trim();
                const email = emailBox.val().trim();
                const password = passwordBox.val().trim();
                const confirmPassword = confirmPasswordBox.val().trim();
                const contact = contactBox.val().trim();

                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!@#$%^&*]{6,15}$/;
                const phoneRegex = /^[0-9]{10}$/;

                $('.form-control').removeClass('is-invalid');

                if (!name) { nameBox.addClass('is-invalid'); isValid = false; }
                if (!email || !emailRegex.test(email)) { emailBox.addClass('is-invalid'); isValid = false; }
                if (!password || !passwordRegex.test(password)) { passwordBox.addClass('is-invalid'); isValid = false; }
                if (!confirmPassword || confirmPassword !== password) { confirmPasswordBox.addClass('is-invalid'); isValid = false; }
                if (!contact || !phoneRegex.test(contact)) { contactBox.addClass('is-invalid'); isValid = false; }

                if (!isValid) {
                    e.preventDefault();
                    Toastify({
                        text: "Please fix validation errors.",
                        duration: 3500,
                        gravity: "top",
                        position: "center",
                        backgroundColor: "#ff6f61"
                    }).showToast();
                }
            });

            // Allow only numbers in contact
            $('#<%= txtContact.ClientID %>').on('keypress', function (e) {
                const key = e.which ? e.which : e.keyCode;
                if (key < 48 || key > 57) e.preventDefault();
            });

            // Show server message
            const msg = $('#<%= lblMessage.ClientID %>').text().trim();
            if (msg !== '') {
                Toastify({
                    text: msg,
                    duration: 4000,
                    gravity: "top",
                    position: "center",
                    backgroundColor: "#28a745"
                }).showToast();
            }
        });
    </script>
</body>
</html>
