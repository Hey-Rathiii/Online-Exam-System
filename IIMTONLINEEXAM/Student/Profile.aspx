<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="IIMTONLINEEXAM.Student.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Profile | IIMT Online Exam</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Tailwind -->
    <script src="https://cdn.tailwindcss.com"></script>
    <script>
        tailwind.config = {
            darkMode: 'class',
            theme: {
                extend: {
                    animation: {
                        bgMove: "bgMove 12s ease-in-out infinite alternate",
                    },
                    keyframes: {
                        bgMove: {
                            "0%": { backgroundPosition: "0% 0%" },
                            "50%": { backgroundPosition: "100% 60%" },
                            "100%": { backgroundPosition: "0% 100%" },
                        }
                    },
                    colors: {
                        primary: { 500: '#6366f1' },
                        secondary: { 500: '#10b981' }
                    }
                }
            }
        }
    </script>

    <script src="https://unpkg.com/feather-icons"></script>

    <style>
        body { font-family: 'Inter', sans-serif; }

        .card-animate {
            opacity: 0;
            transform: translateY(18px);
            animation: fadeCard 0.7s ease-out forwards;
        }

        @keyframes fadeCard {
            to { opacity: 1; transform: translateY(0); }
        }

        .is-invalid {
            border-color: #ef4444 !important;
            box-shadow: 0 0 0 1px #ef4444;
        }

        /* smooth round preview img */
        #imgPreview {
            transition: 0.3s ease;
        }
    </style>
</head>

<body class="min-h-screen bg-gray-900 text-gray-100
             bg-gradient-to-br from-gray-900 via-gray-800 to-gray-900
             bg-[length:200%_200%] animate-bgMove">

<form id="form1" runat="server">
    <div class="min-h-screen flex items-center justify-center px-4 py-10">

        <div class="w-full max-w-2xl bg-gray-900/90 border border-gray-800
                    rounded-3xl shadow-2xl px-8 py-10 card-animate">

            <!-- HEADER -->
            <div class="text-center mb-6">
                <div class="mx-auto w-16 h-16 rounded-2xl bg-primary-500/10 
                            border border-primary-500/40 flex items-center justify-center mb-4">
                    <i data-feather="user" class="text-primary-400 w-7 h-7"></i>
                </div>
                <h1 class="text-3xl font-bold bg-gradient-to-r from-primary-500 to-secondary-500 
                           bg-clip-text text-transparent">My Profile</h1>
                <p class="text-gray-400 text-sm mt-1">Manage your account & photo</p>
            </div>

            <!-- PROFILE PICTURE AREA -->
            <div class="flex flex-col items-center mb-8">

                <!-- PREVIEW CIRCLE -->
                <asp:Image ID="imgProfile" runat="server"
                    CssClass="w-32 h-32 rounded-full object-cover border-2 border-primary-500 shadow-lg mb-3"
                    ImageUrl="~/Student/profile-default.png" />

                <!-- LIVE JS PREVIEW (uses a separate img tag) -->
                <img id="imgPreview" src="" class="hidden w-32 h-32 rounded-full object-cover border-2 border-secondary-500 shadow-lg mb-3" />

                <!-- FILE UPLOAD -->
                <label class="cursor-pointer bg-gray-800 hover:bg-gray-700 text-gray-200 
                               px-4 py-2 rounded-xl text-sm shadow-lg transition">
                    <i class="bi bi-upload mr-1"></i> Upload New Photo
                    <asp:FileUpload ID="filePhoto" runat="server" CssClass="hidden" accept="image/*" />
                </label>

                <!-- SAVE PHOTO BUTTON -->
                <asp:Button ID="btnSavePhoto" runat="server" Text="Save Photo"
                    CssClass="mt-3 px-4 py-2 bg-primary-500 hover:bg-primary-600 
                             text-white rounded-xl text-sm shadow-lg transition"
                    OnClick="btnSavePhoto_Click" />

            </div>

            <!-- PROFILE INFO -->
            <div class="grid grid-cols-1 gap-6">

                <!-- NAME -->
                <div>
                    <label class="text-gray-300 text-sm">Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server"
                        CssClass="form-control w-full px-4 py-3 rounded-xl bg-gray-900 
                                 border border-gray-700 text-sm text-gray-100 placeholder-gray-500 
                                 focus:ring-2 focus:ring-primary-500 transition"
                        placeholder="Enter full name" />
                </div>

                <!-- EMAIL -->
                <div>
                    <label class="text-gray-300 text-sm">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="form-control w-full px-4 py-3 rounded-xl bg-gray-900 
                                 border border-gray-700 text-sm text-gray-100 placeholder-gray-500 
                                 focus:ring-2 focus:ring-primary-500 transition"
                        TextMode="Email" placeholder="Email address" />
                </div>

                <!-- CONTACT -->
                <div>
                    <label class="text-gray-300 text-sm">Contact Number</label>
                    <asp:TextBox ID="txtContact" runat="server"
                        CssClass="form-control w-full px-4 py-3 rounded-xl bg-gray-900 
                                 border border-gray-700 text-sm text-gray-100 placeholder-gray-500 
                                 focus:ring-2 focus:ring-primary-500 transition"
                        placeholder="10-digit mobile number" />
                </div>

                <!-- UPDATE PROFILE BUTTON -->
                <asp:Button ID="btnUpdate" runat="server" Text="Update Profile"
                    CssClass="w-full px-4 py-3 mt-2 rounded-xl bg-secondary-500 hover:bg-secondary-600 
                             text-white font-semibold text-sm shadow-lg shadow-secondary-500/30 
                             transition hover:scale-[1.02]"
                    OnClick="btnUpdate_Click" />

                <!-- MESSAGE -->
                <asp:Label ID="lblMessage" runat="server"
                    CssClass="text-center block text-green-400 mt-2 text-sm"></asp:Label>

                <!-- BACK -->
                <div class="text-center mt-4">
                    <a href="Home.aspx" class="text-gray-400 hover:text-primary-400 text-sm font-medium">
                        ← Back to Dashboard
                    </a>
                </div>

            </div>

        </div>
    </div>
</form>

<script>
    feather.replace();

    // PROFILE PHOTO PREVIEW (LIVE)
    document.getElementById("<%= filePhoto.ClientID %>").addEventListener("change", function () {
        const file = this.files[0];
        if (!file) return;

        let reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById("imgPreview").src = e.target.result;
            document.getElementById("imgPreview").classList.remove("hidden");
        };
        reader.readAsDataURL(file);
    });
</script>

<!-- Input validation -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script>
    $('#<%= btnUpdate.ClientID %>').click(function (e) {
        let valid = true;

        const nm = $('#<%= txtFullName.ClientID %>');
        const em = $('#<%= txtEmail.ClientID %>');
        const ph = $('#<%= txtContact.ClientID %>');

        $('.form-control').removeClass('is-invalid');

        const emailR = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const phR = /^[0-9]{10}$/;

        if (!nm.val().trim()) { nm.addClass('is-invalid'); valid = false; }
        if (!emailR.test(em.val().trim())) { em.addClass('is-invalid'); valid = false; }
        if (!phR.test(ph.val().trim())) { ph.addClass('is-invalid'); valid = false; }

        if (!valid) e.preventDefault();
    });
</script>

</body>
</html>
