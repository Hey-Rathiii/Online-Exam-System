<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="IIMTONLINEEXAM.Student.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Student Dashboard | IIMT Online Exam</title>

    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <style>
        body {
    height: 100%;
    margin: 0;
    padding: 0;
    font-family: 'Segoe UI', sans-serif;
    background: linear-gradient(to right, #83a4d4, #b6fbff);
    color: #333;
}

/* Container card for the dashboard */
.dashboard-container {
    max-width: 1000px;
    margin: 60px auto;
    background: #ffffffee;
    padding: 50px;
    border-radius: 20px;
    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
    animation: fadeIn 0.7s ease-in-out;
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
    font-size: 30px;
    text-align: center;
    margin-bottom: 20px;
    color: #2c3e50;
}

h5 {
    font-weight: 500;
    text-align: center;
    margin-bottom: 30px;
    color: #2c3e50;
}

/* Cards inside dashboard */
.dashboard-links a {
    text-decoration: none;
}

.dashboard-card {
    background: #f4f7fc;
    color: #333;
    border-radius: 16px;
    padding: 25px 15px;
    text-align: center;
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.08);
    transition: all 0.3s ease-in-out;
    height: 100%;
}

.dashboard-card:hover {
    background: #d0ebff;
    transform: translateY(-6px);
}

.dashboard-card i {
    color: #2c7be5;
    font-size: 2.5rem;
    margin-bottom: 10px;
}

.dashboard-card h4 {
    margin-top: 10px;
    font-size: 18px;
    font-weight: 600;
}

/* Logout Button */
.btn-logout {
    background-color: #2980b9;
    border: none;
    color: white;
    font-weight: 600;
    padding: 10px 25px;
    border-radius: 8px;
    transition: background-color 0.3s ease;
}

.btn-logout:hover {
    background-color: #1f6391;
}

@media (max-width: 768px) {
    .dashboard-container {
        padding: 30px 20px;
    }

    h1 {
        font-size: 26px;
    }
}


    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container dashboard-container">
            <h1>👋 Welcome to IIMT Online Exam Portal</h1>
            <h5 class="text-center mb-4" style="color: #fff;">
                <asp:Label ID="lblStudentGreeting" runat="server" Text="Hello, Student!"></asp:Label>
            </h5>

            <div class="row dashboard-links mb-4">
                <div class="col-md-4 mb-3">
                    <a href="StartExam.aspx">
                        <div class="dashboard-card">
                            <i class="bi bi-pencil-fill" style="font-size: 2rem;"></i>
                            <h4>Start Exam</h4>
                        </div>
                    </a>
                </div>
                <div class="col-md-4 mb-3">
                    <a href="Profile.aspx">
                        <div class="dashboard-card">
                            <i class="bi bi-person-circle" style="font-size: 2rem;"></i>
                            <h4>View Profile</h4>
                        </div>
                    </a>
                </div>
                <div class="col-md-4 mb-3">
                    <a href="Result.aspx">
                        <div class="dashboard-card">
                            <i class="bi bi-bar-chart-fill" style="font-size: 2rem;"></i>
                            <h4>My Results</h4>
                        </div>
                    </a>
                </div>
            </div>

            <div class="text-center">
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn btn-logout px-4 py-2" OnClick="btnLogout_Click" />
            </div>
        </div>
    </form>

    <!-- Bootstrap Icons -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
</body>
</html>
