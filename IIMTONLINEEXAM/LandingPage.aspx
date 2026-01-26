<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="IIMTONLINEEXAM.LandingPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Exam System</title>

    <!-- External CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://unpkg.com/aos@2.3.1/dist/aos.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Playfair+Display:wght@600&display=swap" rel="stylesheet" />
    
    <!-- Your Custom CSS -->
    <link href="LandingPage.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="particles-js"></div>

        <!-- Navbar -->
        <nav class="navbar navbar-expand-lg navbar-dark fixed-top glass shadow-sm">
            <div class="container-fluid">
                <a class="navbar-brand text-gold" href="#">ONLINE EXAM SYSTEM</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navMenu">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navMenu">
                    <ul class="navbar-nav ms-auto align-items-center">
                        <li class="nav-item"><a class="nav-link text-light" href="#about">About</a></li>
                        <li class="nav-item"><a class="nav-link text-light" href="#contact">Contact</a></li>
                        <li class="nav-item d-flex align-items-center">
                            <a class="btn btn-outline-warning me-2 btn-animate" href="Register.aspx">Register/Login</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <!-- Hero Section -->
        <section class="hero">
            <div class="container text-center">
                <div class="glass p-5" data-aos="zoom-in">
                    <div class="typing-container">Conduct Exams. Analyze. Automate.</div>
                    <p class="lead mt-4">Welcome to the future of online assessments</p>
                    <a href="Register.aspx" class="btn btn-warning btn-lg mt-3 btn-animate">Get Started</a>
                </div>
            </div>
        </section>

        <!-- About -->
        <section id="about" class="py-5">
            <div class="container" data-aos="fade-up">
                <h2 class="text-center text-gold mb-4">About the System</h2>
                <p class="text-center w-75 mx-auto">Our Online Exam System enables seamless, secure and smart assessments for schools, colleges, and organizations. Built with performance, analytics, and automation in mind.</p>
            </div>
        </section>

        <!-- Features -->
        <section id="features" class="py-5 bg-dark">
            <div class="container">
                <h2 class="text-center text-gold mb-5" data-aos="fade-up">Features</h2>
                <div class="row g-4">
                    <div class="col-md-4" data-aos="fade-right">
                        <div class="card p-4 h-100">
                            <h5 class="text-gold">Secure Exams</h5>
                            <p>Protected exam environments with IP tracking and session control.</p>
                        </div>
                    </div>
                    <div class="col-md-4" data-aos="fade-up">
                        <div class="card p-4 h-100">
                            <h5 class="text-gold">Instant Results</h5>
                            <p>Real-time scoring, result publishing, and instant analytics.</p>
                        </div>
                    </div>
                    <div class="col-md-4" data-aos="fade-left">
                        <div class="card p-4 h-100">
                            <h5 class="text-gold">Smart Dashboard</h5>
                            <p>Manage exams, students, and reports with ease using a user-friendly panel.</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Contact -->
        <section id="contact" class="py-5">
            <div class="container" data-aos="fade-up">
                <h2 class="text-center text-gold mb-4">Contact Us</h2>
                <p class="text-center">Need help? Contact us at <a href="mailto:support@iimtonlineexam.com" class="text-warning">support@iimtonlineexam.com</a></p>
            </div>
        </section>

        <!-- Footer -->
        <footer>
            <p class="mb-0">&copy; 2025 <span class="text-gold">Online Exam System</span>. All rights reserved.</p>
        </footer>

        <!-- Scroll to Top Button -->
        <button id="scrollTopBtn">↑</button>
    </form>

    <!-- External JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://unpkg.com/aos@2.3.1/dist/aos.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/particles.js"></script>

    <!-- Your Custom JS -->
    <script src="LandingPage.js"></script>
</body>
</html>
