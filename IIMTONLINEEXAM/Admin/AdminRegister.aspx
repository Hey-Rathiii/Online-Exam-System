<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRegister.aspx.cs"
    Inherits="IIMTONLINEEXAM.Admin.AdminRegister" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Registration | IIMT Online Exam</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Lottie -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/lottie-web/5.7.4/lottie.min.js"></script>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <style>

        /* 🌌 FULL DARK EMERALD AURORA BACKGROUND */
        body {
            margin: 0;
            height: 100vh;
            padding: 30px;
            overflow: hidden;
            background: radial-gradient(circle at 25% 25%, #00241a, #004d40, #00695c, #003c3c);
            background-size: 400% 400%;
            animation: auroraMove 15s ease infinite;

            display: flex;
            justify-content: center;
            align-items: center;

            font-family: "Inter", sans-serif;
        }

        @keyframes auroraMove {
            0% { background-position: 0% 0%; }
            50% { background-position: 100% 80%; }
            100% { background-position: 0% 0%; }
        }

        /* 🌈 FLOATING BLUR BLOBS */
        .blob {
            position: absolute;
            width: 350px;
            height: 350px;
            border-radius: 50%;
            filter: blur(120px);
            opacity: 0.45;
            animation: blobFloat 18s infinite ease-in-out;
        }

        .blob1 { background: #ff4ecd; top: 5%; left: 10%; }
        .blob2 { background: #4fc3f7; bottom: 10%; right: 10%; }
        .blob3 { background: #9d4edd; bottom: 35%; left: 30%; }

        @keyframes blobFloat {
            0%,100% { transform: translateY(0px); }
            50% { transform: translateY(-60px); }
        }

        /* 🟩 GLASS CARD */
        .glass-card {
            width: 550px;
            height: 680px;
            padding: 35px;

            background: rgba(255,255,255,0.12);
            backdrop-filter: blur(22px);
            border: 1px solid rgba(255,255,255,0.25);

            border-radius: 40px;
            box-shadow: 0 0 2px rgba(255,255,255,0.9),
                        0 25px 80px rgba(0,0,0,0.50);

            display: flex;
            flex-direction: column;
            align-items: center;

            animation: fadeUp 1s ease;
            overflow-y: auto;
        }

        @keyframes fadeUp {
            from { opacity: 0; transform: translateY(25px); }
            to { opacity: 1; transform: translateY(0); }
        }

        /* 🎨 LOTTIE ICON */
        #lottie-admin {
            width: 140px;
            margin-bottom: 10px;
        }

        /* HEADINGS */
        .title {
            text-align: center;
            font-size: 30px;
            color: white;
            font-weight: 700;
            margin-bottom: 4px;
        }

        .subtitle {
            text-align: center;
            color: #ffffffcc;
            margin-bottom: 22px;
            font-size: 15px;
        }

        /* INPUTS */
        .form-control {
            width: 100%;
            padding: 14px;
            border-radius: 14px;
            background: rgba(255,255,255,0.25);
            border: 1px solid rgba(255,255,255,0.35);
            color: #fff;
            margin-bottom: 14px;
        }

        .form-control::placeholder {
            color: #ffffffaa;
        }

        .form-control:focus {
            background: rgba(255,255,255,0.35);
            border-color: #fff;
            box-shadow: 0 0 12px #ffffffaa;
        }

        /* PASSWORD TOGGLE */
        .password-wrapper {
            width: 100%;
            position: relative;
        }

        .toggle-password {
            position: absolute;
            right: 12px;
            top: 50%;
            transform: translateY(-50%);
            color: #fff;
            cursor: pointer;
            font-size: 18px;
        }

        /* BUTTON */
        .btn-register {
            width: 100%;
            padding: 14px;
            border: none;
            border-radius: 16px;

            background: linear-gradient(145deg, #7209b7, #f72585);
            color: white;
            font-size: 16px;
            font-weight: 600;

            box-shadow: 0 8px 28px rgba(247,37,133,0.45);
            margin-top: 10px;
            transition: 0.3s ease;
        }

        .btn-register:hover {
            transform: scale(1.05);
            box-shadow: 0 12px 32px rgba(247,37,133,0.55);
        }

        .error-text {
            color: #ffd1d1;
            text-align: center;
            margin-top: 10px;
        }

        .reg-link {
            margin-top: 15px;
            text-align: center;
            color: #fff;
            font-size: 14px;
        }

        .reg-link a {
            color: white;
            font-weight: 600;
            text-decoration: underline;
        }

    </style>
</head>

<body>

    <!-- BACKGROUND BLOBS -->
    <div class="blob blob1"></div>
    <div class="blob blob2"></div>
    <div class="blob blob3"></div>

    <!-- Lottie Animation -->
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            lottie.loadAnimation({
                container: document.getElementById("lottie-admin"),
                renderer: "svg",
                loop: true,
                autoplay: true,
                path: "https://assets7.lottiefiles.com/packages/lf20_puciaact.json"
            });
        });
    </script>

    <form id="form1" runat="server">

        <div class="glass-card">
            
            <div id="lottie-admin"></div>

            <div class="title">Admin Registration</div>
            <div class="subtitle">Create an admin account to manage exams</div>

            <!-- Full Name -->
            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"
                placeholder="Full Name" />

            <!-- Email -->
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                TextMode="Email" placeholder="Email Address" />

            <!-- Password -->
            <div class="password-wrapper">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                    TextMode="Password" placeholder="Password" />
                <i class="bi bi-eye-slash toggle-password" toggle="<%= txtPassword.ClientID %>"></i>
            </div>

            <!-- Confirm Password -->
            <div class="password-wrapper">
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control"
                    TextMode="Password" placeholder="Confirm Password" />
                <i class="bi bi-eye-slash toggle-password" toggle="<%= txtConfirmPassword.ClientID %>"></i>
            </div>

            <!-- Contact -->
            <asp:TextBox ID="txtContact" runat="server" CssClass="form-control"
                placeholder="Contact Number" />

            <!-- Submit -->
            <asp:Button ID="btnRegister" runat="server" Text="Register"
                CssClass="btn-register" OnClick="btnRegister_Click" />

            <asp:Label ID="lblMessage" runat="server" CssClass="error-text"></asp:Label>

            <div class="reg-link">
                Already have an account? <a href="AdminLogin.aspx">Sign In</a>
            </div>

        </div>

    </form>

    <!-- PASSWORD TOGGLE SCRIPT -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <script>
        $(".toggle-password").click(function () {
            let input = $("#" + $(this).attr("toggle"));
            input.attr("type", input.attr("type") === "password" ? "text" : "password");
            $(this).toggleClass("bi-eye bi-eye-slash");
        });
    </script>

</body>
</html>
