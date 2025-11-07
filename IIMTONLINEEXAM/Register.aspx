<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="IIMTONLINEEXAM.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register | Login</title>
    <link href="Register.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/particles.js/2.0.0/particles.min.js"></script>

    <style>
        body, html {
            margin: 0;
            padding: 0;
            height: 100%;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            overflow: hidden;
        }

        #particles-js {
            position: absolute;
            width: 100%;
            height: 100%;
            background: #0d0d0d;
            z-index: -1;
        }

        .hero {
            height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .glass {
            background: rgba(255, 255, 255, 0.05);
            border-radius: 20px;
            border: 1px solid rgba(255, 215, 0, 0.4);
            backdrop-filter: blur(12px);
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.6);
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            max-width: 450px;
            width: 95%;
        }
        .glass:hover {
            transform: translateY(-6px);
            box-shadow: 0 12px 40px rgba(255, 215, 0, 0.4);
        }

        .text-gold {
            color: #ffd700;
            font-weight: bold;
            letter-spacing: 1px;
        }

        /* === Tabs === */
        .tab-buttons {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin-bottom: 20px;
        }
        .tab-btn {
            background: none;
            border: none;
            color: #aaa;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: color 0.3s ease, border-bottom 0.3s ease;
            padding-bottom: 5px;
        }
        .tab-btn.active {
            color: #ffd700;
            border-bottom: 2px solid #ffd700;
        }

        /* === Buttons inside tabs === */
        .button-group {
            display: flex;
            justify-content: space-evenly;
            gap: 20px;
            flex-wrap: wrap;
        }

        .btn-gold {
            display: inline-block;
            padding: 12px 28px;
            color: #0d0d0d;
            background: linear-gradient(135deg, #ffd700, #ffcc00);
            border-radius: 50px;
            font-weight: bold;
            text-decoration: none;
            transition: all 0.3s ease;
            box-shadow: 0 4px 12px rgba(255, 215, 0, 0.3);
        }
        .btn-gold:hover {
            transform: translateY(-3px) scale(1.05);
            box-shadow: 0 6px 20px rgba(255, 215, 0, 0.6);
        }

        .btn-back {
            display: inline-block;
            margin-top: 20px;
            color: #ffd700;
            text-decoration: none;
            font-size: 14px;
            transition: color 0.3s ease;
        }
        .btn-back:hover {
            color: #fff;
        }

        .tab-content {
            display: none;
        }
        .tab-content.active {
            display: block;
            animation: fadeIn 0.5s ease;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }

        @media(max-width: 500px) {
            .btn-gold {
                width: 100%;
                text-align: center;
            }
        }
    </style>
</head>
<body>
    <!-- Background Particles -->
    <div id="particles-js"></div>

    <div class="hero">
        <div class="container text-center">
            <!-- Glass Card -->
            <div class="glass p-5">

                <!-- Tabs -->
                <div class="tab-buttons">
                    <button class="tab-btn active" onclick="showTab('register')">Register</button>
                    <button class="tab-btn" onclick="showTab('login')">Login</button>
                </div>

                <!-- Register Tab -->
                <div id="register" class="tab-content active">
                    <h1 class="text-gold mb-4">Register As</h1>
                    <div class="button-group">
                        <a href="Admin/AdminRegister.aspx" class="btn-gold">Admin</a>
                        <a href="Student/StudentRegistration.aspx" class="btn-gold">Student</a>
                    </div>
                </div>

                <!-- Login Tab -->
                <div id="login" class="tab-content">
                    <h1 class="text-gold mb-4">Login As</h1>
                    <div class="button-group">
                        <a href="Admin/Login.aspx" class="btn-gold">Admin</a>
                        <a href="Student/Login.aspx" class="btn-gold">Student</a>
                    </div>
                </div>

                <!-- Back -->
                <div class="mt-4">
                    <a href="LandingPage.aspx" class="btn-back">⬅ Back to Main Menu</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Particles Config -->
    <script>
        particlesJS("particles-js", {
            "particles": {
                "number": { "value": 100 },
                "size": { "value": 3 },
                "move": { "speed": 1 },
                "line_linked": { "enable": true, "distance": 150, "color": "#ffd700" },
                "color": { "value": "#ffd700" }
            }
        });

        // Tab switching
        function showTab(tabId) {
            document.querySelectorAll('.tab-btn').forEach(btn => btn.classList.remove('active'));
            document.querySelectorAll('.tab-content').forEach(tab => tab.classList.remove('active'));

            document.querySelector(`[onclick="showTab('${tabId}')"]`).classList.add('active');
            document.getElementById(tabId).classList.add('active');
        }
    </script>
</body>
</html>
