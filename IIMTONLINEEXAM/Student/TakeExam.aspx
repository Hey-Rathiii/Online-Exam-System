<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakeExam.aspx.cs" Inherits="IIMTONLINEEXAM.Student.TakeExam" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Take Your Exam | IIMT Online Exam</title>

    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

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

    <style>
        body { font-family: 'Inter', sans-serif; }
    </style>
</head>

<body class="min-h-screen bg-gray-900 text-gray-100">

<form id="form1" runat="server" class="py-10 px-4">

    <!-- FULLSCREEN OVERLAY -->
    <div id="fsOverlay" class="fixed inset-0 bg-black/80 flex items-center justify-center z-50">
        <button onclick="startExamFullscreen()" 
                class="px-10 py-5 text-xl bg-primary-500 hover:bg-primary-600 text-white rounded-2xl shadow-xl">
            Start Exam – Enter Fullscreen
        </button>
    </div>

    <div class="max-w-3xl mx-auto bg-gray-900/80 backdrop-blur-xl border border-gray-800 
                shadow-2xl rounded-3xl p-8">

        <!-- TOP BAR -->
        <div class="flex flex-wrap justify-between items-center mb-6 gap-3 text-sm">
            <div class="flex items-center gap-2">
                <span class="text-gray-400">⏳ Time Left:</span>
                <span class="font-semibold text-secondary-500">
                    <asp:Label ID="lblTimer" runat="server" Text="--:--"></asp:Label>
                </span>
            </div>

            <div class="flex items-center gap-2">
                <span class="text-gray-400">📌 Question:</span>
                <span class="font-semibold text-primary-500">
                    <asp:Label ID="lblProgress" runat="server" />
                </span>
            </div>

            <div class="flex items-center gap-2">
                <span class="text-gray-400">⚠ Warnings:</span>
                <span class="font-semibold text-red-400">
                    <asp:Label ID="lblWarnings" runat="server" Text="0 / 3" />
                </span>
            </div>
        </div>

        <!-- TIMER PROGRESS -->
        <div class="w-full bg-gray-700/40 rounded-full h-2.5 mb-6 overflow-hidden">
            <div id="progressInner" class="h-full bg-gradient-to-r from-primary-500 to-secondary-500"
                 style="width: 0%;"></div>
        </div>

        <asp:HiddenField ID="hfEndTime" runat="server" />

        <!-- FORMVIEW -->
        <asp:FormView ID="fvQuestion" runat="server"
            AllowPaging="true"
            DataKeyNames="QuestionID"
            PagerSettings-Visible="false"
            OnDataBound="fvQuestion_DataBound"
            OnPageIndexChanging="fvQuestion_PageIndexChanging">

            <ItemTemplate>
                <div class="bg-gray-800/60 border border-gray-700 rounded-2xl p-6 shadow-xl mb-8">

                    <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />

                    <div class="text-xl font-semibold text-primary-300 mb-4">
                        <%# Eval("QuestionText") %>
                    </div>

                    <div class="space-y-3">

                        <label class="flex items-center p-4 bg-gray-700/40 rounded-xl border border-gray-600 
                                      cursor-pointer hover:border-primary-500">
                            <asp:RadioButton ID="rbA" runat="server" GroupName="opt" />
                            <span class="ml-3 text-gray-100"><%# Eval("OptionA") %></span>
                        </label>

                        <label class="flex items-center p-4 bg-gray-700/40 rounded-xl border border-gray-600 
                                      cursor-pointer hover:border-primary-500">
                            <asp:RadioButton ID="rbB" runat="server" GroupName="opt" />
                            <span class="ml-3 text-gray-100"><%# Eval("OptionB") %></span>
                        </label>

                        <label class="flex items-center p-4 bg-gray-700/40 rounded-xl border border-gray-600 
                                      cursor-pointer hover:border-primary-500">
                            <asp:RadioButton ID="rbC" runat="server" GroupName="opt" />
                            <span class="ml-3 text-gray-100"><%# Eval("OptionC") %></span>
                        </label>

                        <label class="flex items-center p-4 bg-gray-700/40 rounded-xl border border-gray-600 
                                      cursor-pointer hover:border-primary-500">
                            <asp:RadioButton ID="rbD" runat="server" GroupName="opt" />
                            <span class="ml-3 text-gray-100"><%# Eval("OptionD") %></span>
                        </label>

                    </div>

                    <div class="text-sm text-gray-400 mt-3">Marks: <%# Eval("Marks") %></div>

                </div>
            </ItemTemplate>

        </asp:FormView>

        <!-- BUTTONS -->
        <div class="flex flex-wrap justify-center gap-4 mt-6">

            <asp:Button ID="btnPrev" runat="server" Text="⬅ Previous"
                CssClass="px-6 py-3 bg-gray-700 hover:bg-gray-600 text-gray-200 rounded-xl shadow-md"
                OnClientClick="allowPostback();"
                OnClick="btnPrev_Click" />

            <asp:Button ID="btnNext" runat="server" Text="Next ➡"
                CssClass="px-6 py-3 bg-primary-500 hover:bg-primary-600 text-white rounded-xl shadow-md"
                OnClientClick="allowPostback();"
                OnClick="btnNext_Click" />

            <asp:Button ID="btnFinish" runat="server" Text="Finish ✅"
                CssClass="px-6 py-3 bg-secondary-500 hover:bg-secondary-600 text-white rounded-xl shadow-md"
                OnClientClick="allowPostback(); return confirm('Are you sure you want to submit your exam?');"
                OnClick="btnFinish_Click" />

        </div>

        <div class="text-center mt-5">
            <asp:Button ID="btnViewResult" runat="server" Text="View Result"
                Visible="False"
                CssClass="px-6 py-3 bg-green-600 text-white rounded-xl shadow-md"
                OnClick="btnViewResult_Click" />
        </div>

        <div class="text-center text-secondary-400 mt-4 text-sm">
            <asp:Label ID="lblMsg" runat="server" />
        </div>

    </div>

</form>

<!-- ========== SECURITY + TIMER SCRIPT ========== -->
<script>

    /* Allow safe ASP.NET postbacks */
    function allowPostback() {
        window.onbeforeunload = null;
    }

    /* === FULLSCREEN START === */
    function startExamFullscreen() {
        let e = document.documentElement;
        if (e.requestFullscreen) e.requestFullscreen();

        document.getElementById("fsOverlay").style.display = "none";
        startTimer();
    }

    /* === TIMER === */
    let endTime, totalSeconds;
    function startTimer() {

        endTime = new Date(document.getElementById('<%= hfEndTime.ClientID %>').value);
        let lbl = document.getElementById('<%= lblTimer.ClientID %>');
        let bar = document.getElementById("progressInner");

        function tick() {
            let now = new Date();
            let diff = endTime - now;

            if (totalSeconds == null) totalSeconds = Math.floor(diff / 1000);

            if (diff <= 0) {
                lbl.textContent = "00:00";
                allowPostback();
                document.getElementById('<%= btnFinish.ClientID %>').click();
                return;
            }

            let sec = Math.floor(diff / 1000);
            let m = Math.floor(sec / 60);
            let s = sec % 60;

            lbl.textContent = (m < 10 ? "0"+m : m) + ":" + (s < 10 ? "0"+s : s);
            bar.style.width = ((totalSeconds - sec) / totalSeconds * 100) + "%";

            setTimeout(tick, 1000);
        }
        tick();
    }

    /* === ANTI-CHEATING === */
    let warnings = 0, max = 3;
    const lblWarn = document.getElementById('<%= lblWarnings.ClientID %>');

    function warn(reason) {
        warnings++;
        lblWarn.textContent = warnings + " / " + max;
        alert("Warning " + warnings + "/" + max + ":\n" + reason);

        if (warnings >= max) {
            allowPostback();
            window.location.href = "Disqualified.aspx?reason=" + encodeURIComponent(reason);
        }
    }

    document.addEventListener("visibilitychange", () => {
        if (document.hidden) warn("Tab switch / minimized detected.");
    });

    document.addEventListener("fullscreenchange", () => {
        if (!document.fullscreenElement) warn("Exited fullscreen.");
    });

    window.onbeforeunload = function () {
        return "You cannot leave the exam.";
    };

    history.pushState(null, null, location.href);
    window.onpopstate = function () {
        history.go(1);
        warn("Back button pressed.");
    };

    document.addEventListener("contextmenu", e => e.preventDefault());

    document.addEventListener("keydown", e => {
        if (e.key === "F5" || (e.ctrlKey && (e.key === "r" || e.key === "R"))) {
            e.preventDefault(); warn("Refresh blocked.");
        }
        if (e.ctrlKey && ["c", "v", "x"].includes(e.key.toLowerCase())) {
            e.preventDefault(); warn("Copy/Cut/Paste blocked.");
        }
    });

</script>

</body>
</html>
