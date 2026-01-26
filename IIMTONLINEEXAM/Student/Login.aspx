<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IIMTONLINEEXAM.Student.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Login | IIMT Online Exam</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Lottie -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/lottie-web/5.7.4/lottie.min.js"></script>

    <style>

        /* AURORA BACKGROUND */
       
body {
    margin: 0;
    height: 100vh;
    padding: 30px;
    overflow: hidden;

    /* 🐍 DARK EMERALD NINJA THEME */
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

        /* FLOATING BLUR BLOBS */
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

        /* PERFECT SQUARE GLASS BOX */
        .glass-card {
            width: 550px;
            height: 550px; /* FULL SQUARE */
            padding: 40px;

            display: flex;
            flex-direction: column;
            align-items: center;

            border-radius: 25px;

            background: rgba(255,255,255,0.10);
            backdrop-filter: blur(22px);
            border: 1px solid rgba(255,255,255,0.25);

            box-shadow:
                0 0 2px rgba(255,255,255,0.9),
                0 25px 80px rgba(0,0,0,0.50);

            animation: fadeUp 1s ease;
        }

        @keyframes fadeUp {
            from { opacity: 0; transform: translateY(25px); }
            to { opacity: 1; transform: translateY(0); }
        }

        .title {
            text-align: center;
            color: #fff;
            font-size: 32px;
            font-weight: 700;
            margin-top: 5px;
        }

        .subtitle {
            text-align: center;
            color: #ffffffcc;
            margin-bottom: 20px;
        }

        #lottie-login {
            width: 140px;
            margin-bottom: 15px;
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
            background: rgba(255,255,255,0.4);
            border-color: #fff;
            box-shadow: 0 0 10px #ffffffaa;
        }

        /* BUTTON */
        .btn-login {
            margin-top: 5px;
            padding: 14px;
            width: 100%;
            border-radius: 14px;
            background: linear-gradient(145deg, #7209b7, #f72585);
            border: none;
            color: #fff;
            font-size: 15px;
            font-weight: 600;
            box-shadow: 0 8px 20px rgba(247,37,133,0.4);
        }

        .btn-login:hover {
            transform: scale(1.05);
            box-shadow: 0 12px 25px rgba(247,37,133,0.55);
        }

        .text-danger {
            text-align: center;
            font-size: 14px;
            margin-top: 8px;
            color: #ffd1d1;
        }

        .register-text {
            margin-top: 15px;
            text-align: center;
            color: #fff;
        }

        .register-text a {
            color: #fff;
            text-decoration: underline;
            font-weight: 600;
        }

    </style>
</head>

<body>

    <!-- BACKGROUND BLOBS -->
    <div class="blob blob1"></div>
    <div class="blob blob2"></div>
    <div class="blob blob3"></div>

    <!-- LOTTIE LOADER -->
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            lottie.loadAnimation({
                container: document.getElementById("lottie-login"),
                renderer: "svg",
                loop: true,
                autoplay: true,
                path: "https://assets7.lottiefiles.com/packages/lf20_puciaact.json"
            });
        });
    </script>

    <!-- LOGIN FORM SQUARE BOX -->
    <form id="form1" runat="server">
        <div class="glass-card">

            <div id="lottie-login"></div>

            <div class="title">Student Login</div>
            <div class="subtitle">Sign in to continue to Online Exam</div>

            <asp:TextBox ID="txtEmail" runat="server"
                CssClass="form-control"
                TextMode="Email"
                placeholder="Email Address" />

            <asp:TextBox ID="txtPassword" runat="server"
                CssClass="form-control"
                TextMode="Password"
                placeholder="Password" />

            <asp:Button ID="btnLogin" runat="server" Text="Login"
                CssClass="btn-login"
                OnClick="btnLogin_Click" />

            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block" />

            <div class="register-text">
                New here? <a href="StudentRegistration.aspx">Create an account</a>
            </div>

        </div>
    </form>

</body>
</html>
