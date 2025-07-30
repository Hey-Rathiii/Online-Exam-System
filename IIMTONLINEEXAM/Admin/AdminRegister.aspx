<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRegister.aspx.cs" Inherits="IIMTONLINEEXAM.Admin.AdminRegister" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Registration</title>
    <!-- Bootstrap CDN -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .register-container {
            max-width: 500px;
            margin: 50px auto;
            padding: 25px;
            background-color: #f8f9fa;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .form-label {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="register-container">
                <h3 class="text-center mb-4">Admin Registration</h3>

                <div class="mb-3">
                    <label for="txtFullName" class="form-label">Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                </div>

                <div class="mb-3">
                    <label for="txtContact" class="form-label">Contact Number</label>
                    <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" />
                </div>

                <div class="text-center">
                    <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-primary w-100" Text="Register Admin" OnClick="btnRegister_Click" />
                </div>

                <div class="mt-3 text-danger text-center">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
