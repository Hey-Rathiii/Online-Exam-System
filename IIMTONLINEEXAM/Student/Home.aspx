<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="IIMTONLINEEXAM.Student.Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Dashboard | IIMT Online Exam</title>

    <!-- Tailwind -->
    <script src="https://cdn.tailwindcss.com"></script>
    <script>
        tailwind.config = {
            darkMode: 'class',
            theme: {
                extend: {
                    colors: {
                        primary: { 500: '#6366f1' },
                        secondary: { 500: '#10b981' }
                    }
                }
            }
        }
    </script>

    <!-- Icons -->
    <script src="https://unpkg.com/feather-icons"></script>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <style>
        body {
            font-family: 'Inter', system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
        }

        .card-enter {
            opacity: 0;
            transform: translateY(30px);
            animation: cardIn 0.65s ease-out forwards;
        }

        @keyframes cardIn {
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .dash-card {
            transform: translateY(15px);
            opacity: 0;
            transition: all 0.35s ease;
        }

        .dash-card.visible {
            transform: translateY(0);
            opacity: 1;
        }
    </style>
</head>
<body class="bg-gray-950 text-gray-100 min-h-screen">

    <form id="form1" runat="server">
        <div class="min-h-screen flex items-center justify-center px-4 py-10">
            <div class="w-full max-w-5xl bg-gray-900/80 border border-gray-800 rounded-3xl shadow-2xl card-enter px-8 py-8 lg:px-10 lg:py-10">
                
                <!-- Header -->
                <header class="mb-8">
                    <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                        <div>
                            <h1 class="text-3xl md:text-4xl font-bold bg-gradient-to-r from-primary-500 to-secondary-500 bg-clip-text text-transparent">
                                Student Dashboard
                            </h1>
                            <p class="text-sm text-gray-400 mt-1">
                                Manage your exams, profile and results from one place.
                            </p>
                        </div>
                        <div class="text-right">
                            <span class="block text-xs uppercase tracking-wide text-gray-500">Welcome</span>
                            <span class="text-sm text-gray-200 font-medium">
                                <asp:Label ID="lblStudentGreeting" runat="server" Text="Hello, Student!"></asp:Label>
                            </span>
                        </div>
                    </div>
                </header>

                <!-- Cards -->
                <section class="grid grid-cols-1 md:grid-cols-3 gap-5 mb-6">
                    <!-- All Exams -->
                    <a href="ExamNavigator.aspx" class="dash-card group bg-gradient-to-br from-gray-900 to-slate-900/80 border border-gray-800 rounded-2xl px-5 py-6 shadow-lg hover:shadow-2xl hover:border-primary-500/60 transition">
                        <div class="flex items-center justify-between mb-4">
                            <div class="inline-flex items-center justify-center w-11 h-11 rounded-xl bg-primary-500/15 border border-primary-500/40">
                                <i data-feather="edit-3" class="w-5 h-5 text-primary-400"></i>
                            </div>
                            <span class="text-xs font-semibold text-primary-300 bg-primary-500/10 px-2.5 py-1 rounded-full">
                                Exams
                            </span>
                        </div>
                        <h3 class="text-lg font-semibold text-gray-50">Show All Exams</h3>
                        <p class="mt-1 text-xs text-gray-400">
                            View and attempt current, previous and upcoming exams.
                        </p>
                    </a>

                    <!-- Profile -->
                    <a href="Profile.aspx" class="dash-card group bg-gradient-to-br from-gray-900 to-slate-900/80 border border-gray-800 rounded-2xl px-5 py-6 shadow-lg hover:shadow-2xl hover:border-secondary-500/60 transition">
                        <div class="flex items-center justify-between mb-4">
                            <div class="inline-flex items-center justify-center w-11 h-11 rounded-xl bg-secondary-500/15 border border-secondary-500/40">
                                <i data-feather="user" class="w-5 h-5 text-secondary-400"></i>
                            </div>
                            <span class="text-xs font-semibold text-secondary-300 bg-secondary-500/10 px-2.5 py-1 rounded-full">
                                Profile
                            </span>
                        </div>
                        <h3 class="text-lg font-semibold text-gray-50">View Profile</h3>
                        <p class="mt-1 text-xs text-gray-400">
                            Manage your personal details and contact information.
                        </p>
                    </a>

                    <!-- Results -->
                    <a href="Result.aspx" class="dash-card group bg-gradient-to-br from-gray-900 to-slate-900/80 border border-gray-800 rounded-2xl px-5 py-6 shadow-lg hover:shadow-2xl hover:border-amber-500/60 transition">
                        <div class="flex items-center justify-between mb-4">
                            <div class="inline-flex items-center justify-center w-11 h-11 rounded-xl bg-amber-500/15 border border-amber-500/40">
                                <i data-feather="bar-chart-2" class="w-5 h-5 text-amber-300"></i>
                            </div>
                            <span class="text-xs font-semibold text-amber-200 bg-amber-500/10 px-2.5 py-1 rounded-full">
                                Results
                            </span>
                        </div>
                        <h3 class="text-lg font-semibold text-gray-50">My Results</h3>
                        <p class="mt-1 text-xs text-gray-400">
                            Track your performance and view detailed exam scores.
                        </p>
                    </a>
                </section>

                <!-- Logout -->
                <div class="mt-4 flex justify-end">
                    <asp:Button ID="btnLogout" runat="server" Text="Logout"
                        CssClass="inline-flex items-center gap-2 px-5 py-2.5 rounded-xl bg-red-500/90 hover:bg-red-600 text-white text-sm font-semibold shadow-lg shadow-red-500/30 transition-transform transform hover:scale-[1.02]"
                        OnClick="btnLogout_Click" />
                </div>
            </div>
        </div>
    </form>

    <script>
        feather.replace();

        // Show card animations staggered
        document.addEventListener('DOMContentLoaded', function () {
            const cards = document.querySelectorAll('.dash-card');
            cards.forEach((card, index) => {
                setTimeout(() => {
                    card.classList.add('visible');
                }, 120 * index);
            });
        });
    </script>
</body>
</html>
