<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewResult.aspx.cs" Inherits="IIMTONLINEEXAM.Student.ViewResult" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>View Exam Result | IIMT Online Exam</title>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Tailwind -->
    <script src="https://cdn.tailwindcss.com"></script>

    <script>
        tailwind.config = {
            darkMode: 'class',
            theme: {
                extend: {
                    colors: {
                        primary: { 500: "#6366f1" },
                        secondary: { 500: "#10b981" }
                    },
                    animation: {
                        bgMove: "bgMove 12s ease-in-out infinite alternate",
                    },
                    keyframes: {
                        bgMove: {
                            "0%": { backgroundPosition: "0% 0%" },
                            "50%": { backgroundPosition: "100% 60%" },
                            "100%": { backgroundPosition: "0% 100%" },
                        }
                    }
                }
            }
        };
    </script>

    <style>
        body { font-family: 'Inter', sans-serif; }
    </style>
</head>

<body class="min-h-screen bg-gray-900 text-gray-100
             bg-gradient-to-br from-gray-900 via-gray-800 to-gray-900
             bg-[length:200%_200%] animate-bgMove">

<form id="form1" runat="server">

    <!-- LOGOUT BUTTON -->
    <div class="fixed top-5 right-6 z-50">
        <asp:Button ID="btnLogout" runat="server" Text="Logout"
            CssClass="px-5 py-2 bg-red-600 hover:bg-red-700 rounded-xl text-white font-semibold shadow-lg transition"
            OnClick="btnLogout_Click" />
    </div>

    <!-- MAIN CONTAINER -->
    <div class="max-w-5xl mx-auto mt-20 p-8 
                bg-gray-900/80 backdrop-blur-xl border border-gray-800 
                shadow-2xl rounded-3xl">

        <!-- HEADER -->
        <h2 class="text-center text-4xl font-bold mb-8
                   bg-gradient-to-r from-primary-500 to-secondary-500 
                   bg-clip-text text-transparent">
            Your Exam Report
        </h2>

        <!-- EXAM DETAILS TABLE -->
        <div class="overflow-hidden rounded-2xl border border-gray-700 mb-10">
            <asp:GridView ID="gvExamReport" runat="server"
                AutoGenerateColumns="false"
                CssClass="w-full text-sm bg-gray-800 text-gray-200 border-collapse"
                HeaderStyle-CssClass="bg-primary-500 text-white text-center font-semibold"
                RowStyle-CssClass="text-center border-b border-gray-700">
                <Columns>
                    <asp:BoundField DataField="QuestionText" HeaderText="Question" />
                    <asp:BoundField DataField="OptionA" HeaderText="A" />
                    <asp:BoundField DataField="OptionB" HeaderText="B" />
                    <asp:BoundField DataField="OptionC" HeaderText="C" />
                    <asp:BoundField DataField="OptionD" HeaderText="D" />
                    <asp:BoundField DataField="SelectedOption" HeaderText="Selected" />
                    <asp:BoundField DataField="CorrectAnsOpion" HeaderText="Correct" />
                    <asp:BoundField DataField="SelectionStatus" HeaderText="Status" />
                    <asp:BoundField DataField="MarksObtained" HeaderText="Marks" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- SUMMARY TABLE -->
        <div class="overflow-hidden rounded-2xl border border-gray-700 mb-8">
            <asp:GridView ID="GridViewStatics" runat="server"
                AutoGenerateColumns="false"
                CssClass="w-full text-sm bg-gray-800 text-gray-200 border-collapse"
                HeaderStyle-CssClass="bg-secondary-500 text-white text-center font-semibold"
                RowStyle-CssClass="text-center border-b border-gray-700">
                <Columns>
                    <asp:BoundField DataField="TotalMarksObtained" HeaderText="Total Marks" />
                    <asp:BoundField DataField="TotalPossibleMarks" HeaderText="Possible Marks" />
                    <asp:BoundField DataField="Percentage" HeaderText="Percentage" />
                    <asp:BoundField DataField="Result" HeaderText="Result" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- DOWNLOAD BUTTONS -->
        <div class="flex justify-center gap-4 mt-6">
            <asp:Button ID="btnDownloadCSV" runat="server" Text="Download CSV"
                CssClass="px-6 py-3 bg-primary-500 hover:bg-primary-600 text-white rounded-xl font-semibold shadow-lg transition"
                OnClick="btnDownloadCSV_Click" />

            <asp:Button ID="btnDownloadPDF" runat="server" Text="Download PDF"
                CssClass="px-6 py-3 bg-red-600 hover:bg-red-700 text-white rounded-xl font-semibold shadow-lg transition"
                OnClick="btnDownloadPDF_Click" />
        </div>

        <!-- MESSAGE -->
        <div class="text-center mt-6 text-red-400 text-sm font-medium">
            <asp:Label ID="lblMessage" runat="server" />
        </div>

    </div>

</form>
</body>
</html>
