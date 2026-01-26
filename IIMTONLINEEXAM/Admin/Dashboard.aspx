<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IIMTONLINEEXAM.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnActiveTab" runat="server" />

    <div class="container mt-4">
        <h3 class="mb-4">🧾 Admin Dashboard</h3>

        <!-- inline helper to show inline alerts -->
        <script>
            function showInlineMsg(id, type, msg) {
                const d = document.getElementById(id);
                d.className = "alert alert-" + type;
                d.innerText = msg;
                d.style.display = "block";
                setTimeout(() => d.style.display = "none", 4000);
            }
        </script>
      <style >
/* GLOBAL BG */
/* PAGE BACKGROUND */
body {
    background: #0f1424 !important;
    font-family: 'Segoe UI', sans-serif;
}

/* MAIN OUTER WRAPPER */
.container {
    background: rgba(255, 255, 255, 0);
}

/* PAGE HEADER TITLE */
.page-title, h3, h5 {
    color: #c9d6ff !important;
}

/* PANEL / BOX OUTER */
.tab-pane {
    background: linear-gradient(135deg, #1c2841, #202b46);
    padding: 30px;
    border-radius: 16px;
    border: 1px solid rgba(180, 200, 255, 0.2);
    margin-top: 20px;
}

/* SECTION SUB-TITLES */
h5 {
    font-size: 20px;
    color: #e5ecff !important;
    margin-bottom: 15px;
}

/* ALL INPUT BOXES */
input[type="text"], 
input[type="date"],
input[type="time"], 
select, 
textarea {
    background: rgba(255, 255, 255, 0.08) !important;
    border: 1px solid rgba(255, 255, 255, 0.18) !important;
    color: #dfe6ff !important;
    border-radius: 10px !important;
    padding: 10px !important;
}

input::placeholder,
textarea::placeholder {
    color: #9bb0cc !important;
}

select option {
    background: #1a2338;
    color: #e0e6ff;
}

/* GRIDVIEW DARK MODE */
table {
    background: rgba(255, 255, 255, 0.03) !important;
}

.table th {
    background: rgba(255, 255, 255, 0.08) !important;
    color: #cdd9ff !important;
}

.table td {
    background: rgba(255, 255, 255, 0.04) !important;
    color: #e9eeff !important;
}

/* LINKS */
a, .btn-link {
    color: #6fb6ff !important;
    font-weight: 600;
}

/* DELETE LINK */
.text-danger, a.text-danger {
    color: #ff6b6b !important;
}

/* BUTTONS */
.btn-primary {
    background: linear-gradient(90deg, #007bff, #00bfff) !important;
    border: none !important;
    padding: 10px 25px !important;
    font-weight: 600 !important;
    border-radius: 12px !important;
}

.btn-primary:hover {
    transform: scale(1.05);
    background: linear-gradient(90deg, #0095ff, #22d3ff) !important;
}

/* AI + IMPORT BUTTONS */
#btnAI,
.btn-primary-small {
    background: linear-gradient(90deg, #3b82f6, #06b6d4) !important;
    color: white !important;
    border-radius: 12px !important;
    font-weight: 600;
    padding: 8px 18px;
}

/* TABS */
.nav-tabs .nav-link {
    color: #a3b8ff !important;
    background: transparent !important;
    border: none !important;
    margin-right: 20px;
    font-size: 18px;
}

.nav-tabs .nav-link.active {
    background-color: #2d3b57 !important;
    border-radius: 10px !important;
    color: #6ea8ff !important;
    font-weight: 700;
    padding: 8px 18px;
    border: 1px solid rgba(150, 170, 255, 0.3) !important;
}


</style >


        <%--<a href="Dashboard.aspx?tab=question" class="btn btn-secondary">← Back to Questions
        </a>--%>

        <!-- TABS -->
        <ul class="nav nav-tabs" id="adminTab" role="tablist">
            <li class="nav-item">
                <button class="nav-link active" id="subject-tab" data-bs-toggle="tab" data-bs-target="#subject" type="button">Subject</button>
            </li>
            <li class="nav-item">
                <button class="nav-link" id="exam-tab" data-bs-toggle="tab" data-bs-target="#exam" type="button">Exam</button>
            </li>
            <li class="nav-item">
                <button class="nav-link" id="question-tab" data-bs-toggle="tab" data-bs-target="#question" type="button">Question</button>
            </li>
        </ul>

        <div class="tab-content border p-4" id="adminTabContent">

            <!-- SUBJECT TAB -->
            <div class="tab-pane fade show active" id="subject">
                <h5>Create Subject</h5>

                <div id="subjectError" class="fw-bold mt-1 mb-2" style="display: none;"></div>

                <asp:Panel ID="pnlAddSubject" runat="server">
                    <div class="form-group mb-1">
                        <asp:TextBox ID="txtSubjectName" runat="server" CssClass="form-control" placeholder="Enter Subject Name"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnAddSubject" runat="server" Text="Add Subject" CssClass="btn btn-primary mt-2"
                        OnClientClick="return validateSubject();" OnClick="btnAddSubject_Click" />

                    <hr />

                    <asp:GridView ID="gvSubjects" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered mt-3" DataKeyNames="SubjectID"
                        OnRowEditing="gvSubjects_RowEditing" OnRowUpdating="gvSubjects_RowUpdating" OnRowCancelingEdit="gvSubjects_RowCancelingEdit" OnRowCommand="gvSubjects_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="SubjectID" HeaderText="ID" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Subject Name">
                                <ItemTemplate><%# Eval("SubjectName") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditSubject" runat="server" Text='<%# Bind("SubjectName") %>' CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="Edit" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                        OnClientClick='<%# "return validateSubjectEdit(" + Container.DataItemIndex + ");" %>' />
                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteSubject" CommandArgument='<%# Eval("SubjectID") %>'
                                        Text="Delete" CssClass="text-danger" OnClientClick="return confirm('Delete this subject?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>

            <!-- EXAM TAB -->
            <div class="tab-pane fade" id="exam">
                <h5>Create Exam</h5>
                <div id="examMsg" style="display: none" class="fw-bold mb-2"></div>

                <asp:Panel ID="pnlExams" runat="server">
                    <table class="table table-borderless w-75">
                        <tr>
                            <td>Subject:</td>
                            <td>
                                <asp:DropDownList ID="ddlSubjects" runat="server" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="text-danger small" id="errSubject"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>Exam Title:</td>
                            <td>
                                <asp:TextBox ID="txtExamTitle" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="text-danger small" id="errTitle"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>Date:</td>
                            <td>
                                <asp:TextBox ID="txtExamDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                <span class="text-danger small" id="errDate"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>Start Time:</td>
                            <td>
                                <asp:TextBox ID="txtStartTime" runat="server" TextMode="Time"
                                    CssClass="form-control" oninput="autoCalculateDuration()"></asp:TextBox>
                                <span class="text-danger small" id="errStart"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>End Time:</td>
                            <td>
                                <asp:TextBox ID="txtEndTime" runat="server" TextMode="Time"
                                    CssClass="form-control" oninput="autoCalculateDuration()"></asp:TextBox>
                                <span class="text-danger small" id="errEnd"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>Duration:</td>
                            <td>
                                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                <span class="text-danger small" id="errDuration"></span>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnAddExam" runat="server" Text="Add Exam"
                                    CssClass="btn btn-primary"
                                    OnClientClick="return validateExam();"
                                    OnClick="btnAddExam_Click" />
                            </td>
                        </tr>
                    </table>

                    <hr />

                    <asp:GridView ID="gvExams" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered"
                        DataKeyNames="ExamID,SubjectID"
                        Visible="false"
                        OnRowEditing="gvExams_RowEditing"
                        OnRowUpdating="gvExams_RowUpdating"
                        OnRowCancelingEdit="gvExams_RowCancelingEdit"
                        OnRowCommand="gvExams_RowCommand">

                        <Columns>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemTemplate><%# Eval("SubjectName") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSubjectReadOnly" runat="server"
                                        Text='<%# Eval("SubjectName") %>'
                                        CssClass="form-control" ReadOnly="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Exam Title">
                                <ItemTemplate><%# Eval("ExamTitle") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtExamTitleEdit" runat="server"
                                        Text='<%# Eval("ExamTitle") %>' CssClass="form-control" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate><%# Eval("ExamDate", "{0:yyyy-MM-dd}") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtExamDateEdit" runat="server"
                                        Text='<%# Eval("ExamDate","{0:yyyy-MM-dd}") %>'
                                        CssClass="form-control" TextMode="Date" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Start">
                                <ItemTemplate><%# Eval("StartTime") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtStartTimeEdit" runat="server"
                                        Text='<%# Eval("StartTime") %>' CssClass="form-control" TextMode="Time" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="End">
                                <ItemTemplate><%# Eval("EndTime") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEndTimeEdit" runat="server"
                                        Text='<%# Eval("EndTime") %>' CssClass="form-control" TextMode="Time" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Duration (min)">
                                <ItemTemplate><%# Eval("DurationMinutes") %></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDurationEdit" runat="server"
                                        Text='<%# Eval("DurationMinutes") %>' CssClass="form-control" ReadOnly="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:CommandField ShowEditButton="True" />

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkToggle" runat="server"
                                        Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Deactivate" : "Activate" %>'
                                        CommandName="ToggleStatus"
                                        CommandArgument='<%# Eval("ExamID") + "|" + Eval("IsActive") %>'
                                        CssClass="btn btn-warning btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>

            <!-- QUESTION TAB -->
            <div class="tab-pane fade" id="question">
                <div class="d-flex justify-content-end mb-3">
                    <a href="AiQuestionGenerator.aspx" id="btnAI" class="btn btn-primary" >Generate with AI</a>
                    <br />
                    <a href="ImportQuestions.aspx" class="btn btn-primary">📥 Import Questions</a>
                    <br />
                </div>
               
                <h3>Insert New Question</h3>
                <div id="questionMsg" style="display: none"></div>

                <asp:Panel ID="pnlInsertQuestion" runat="server">
                    <table class="question-form-table">
                        <tr>
                            <td>Exam:</td>
                            <td>
                                <!-- renamed dropdown to avoid id clash -->
                                <asp:DropDownList ID="ddlExamsQuestion" runat="server" ClientIDMode="Static"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlExamsQuestion_SelectedIndexChanged">
                                </asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td>Question:</td>
                            <td>
                                <asp:TextBox ID="txtQuestion" runat="server" ClientIDMode="Static" TextMode="MultiLine" Width="400" Height="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Option A:</td>
                            <td>
                                <asp:TextBox ID="txtOptionA" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Option B:</td>
                            <td>
                                <asp:TextBox ID="txtOptionB" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Option C:</td>
                            <td>
                                <asp:TextBox ID="txtOptionC" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Option D:</td>
                            <td>
                                <asp:TextBox ID="txtOptionD" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Correct Option:</td>
                            <td>
                                <asp:TextBox ID="txtCorrectOption" ClientIDMode="Static" runat="server" MaxLength="1"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Marks:</td>
                            <td>
                                <asp:TextBox ID="txtMarks" ClientIDMode="Static" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSaveQuestion" runat="server" Text="Save"
                                    OnClientClick="return validateQuestionInputs();"
                                    OnClick="btnSaveQuestion_Click" />


                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <hr />

                <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" DataKeyNames="QuestionID"
                    OnRowEditing="gvQuestions_RowEditing" OnRowUpdating="gvQuestions_RowUpdating" OnRowCancelingEdit="gvQuestions_RowCancelingEdit" OnRowDeleting="gvQuestions_RowDeleting">

                    <Columns>
                        <asp:TemplateField HeaderText="Question">
                            <ItemTemplate><%# Eval("QuestionText") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtQuestionText" runat="server" Text='<%# Bind("QuestionText") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Option A">
                            <ItemTemplate><%# Eval("OptionA") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtA" runat="server" Text='<%# Bind("OptionA") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Option B">
                            <ItemTemplate><%# Eval("OptionB") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtB" runat="server" Text='<%# Bind("OptionB") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Option C">
                            <ItemTemplate><%# Eval("OptionC") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtC" runat="server" Text='<%# Bind("OptionC") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Option D">
                            <ItemTemplate><%# Eval("OptionD") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtD" runat="server" Text='<%# Bind("OptionD") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Correct">
                            <ItemTemplate><%# Eval("CorrectOption") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCorrect" runat="server" Text='<%# Bind("CorrectOption") %>' CssClass="form-control" MaxLength="1" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Marks">
                            <ItemTemplate><%# Eval("Marks") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMarks" runat="server" Text='<%# Bind("Marks") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <style>
            .input-error {
                border: 2px solid red !important;
                background-color: #ffe6e6 !important;
            }

            .error-text {
                color: red;
                font-size: 0.85rem;
                margin-top: 3px;
                display: block;
            }
        </style>

        <script>


            // Attach live validation when page loads
            window.onload = function () {
                attachLiveValidation();
            };

            // Auto remove error when user types or changes dropdown value
            function attachLiveValidation() {
                const allInputs = document.querySelectorAll("input[type='text'], textarea, select");

                allInputs.forEach(el => {
                    el.addEventListener("input", function () {
                        el.classList.remove("input-error");

                        let next = el.parentNode.querySelector(".error-text");
                        if (next) next.remove();
                    });

                    el.addEventListener("change", function () {
                        el.classList.remove("input-error");

                        let next = el.parentNode.querySelector(".error-text");
                        if (next) next.remove();
                    });
                });
            }

            // Main validation function
            function validateQuestionInputs() {
                let isValid = true;

                // remove previous errors
                document.querySelectorAll(".error-text").forEach(e => e.remove());
                document.querySelectorAll(".input-error").forEach(e => e.classList.remove("input-error"));

                // --- DROPDOWN VALIDATION ---
                const ddlExam = document.getElementById("<%= ddlExamsQuestion.ClientID %>");
                if (ddlExam.value === "0" || ddlExam.value === "") {
                    ddlExam.classList.add("input-error");

                    let err = document.createElement("span");
                    err.className = "error-text";
                    err.innerText = "Please select an exam";
                    ddlExam.parentNode.appendChild(err);

                    isValid = false;
                }

                // --- TEXT FIELD VALIDATION ---
                const fields = [
                    { id: "<%= txtQuestion.ClientID %>", msg: "Please enter the question!" },
                        { id: "<%= txtOptionA.ClientID %>", msg: "Please enter Option A" },
                        { id: "<%= txtOptionB.ClientID %>", msg: "Please enter Option B" },
                        { id: "<%= txtOptionC.ClientID %>", msg: "Please enter Option C" },
                        { id: "<%= txtOptionD.ClientID %>", msg: "Please enter Option D" },
                        { id: "<%= txtCorrectOption.ClientID %>", msg: "Correct option is required" },
                        { id: "<%= txtMarks.ClientID %>", msg: "Marks are required" }
                ];

                fields.forEach(f => {
                    let el = document.getElementById(f.id);

                    if (!el.value.trim()) {
                        el.classList.add("input-error");

                        let err = document.createElement("span");
                        err.className = "error-text";
                        err.innerText = f.msg;
                        el.parentNode.appendChild(err);

                        isValid = false;
                    }
                });

                // --- CORRECT OPTION FORMAT ---
                const correctOptionEl = document.getElementById("<%= txtCorrectOption.ClientID %>");
                let opt = correctOptionEl.value.trim().toUpperCase();

                if (opt && !["A", "B", "C", "D"].includes(opt)) {
                    correctOptionEl.classList.add("input-error");

                    let err = document.createElement("span");
                    err.className = "error-text";
                    err.innerText = "Correct option must be A, B, C or D!";
                    correctOptionEl.parentNode.appendChild(err);

                    isValid = false;
                }

                return isValid;
            }
        </script>




        <style>
            .shake {
                animation: shake .3s;
            }

            @keyframes shake {
                0% {
                    transform: translateX(0);
                }

                25% {
                    transform: translateX(-4px);
                }

                50% {
                    transform: translateX(4px);
                }

                75% {
                    transform: translateX(-4px);
                }

                100% {
                    transform: translateX(0);
                }
            }
        </style>

        <!-- CLIENT-SIDE VALIDATION & UX -->
        <script>
            // SUBJECT validation
            function validateSubject() {
                let input = document.getElementById("<%= txtSubjectName.ClientID %>");
                let msg = document.getElementById("subjectError");
                let name = input.value.trim().toLowerCase();

                msg.style.display = "none";
                input.style.border = "1px solid #ced4da";
                input.classList.remove("shake");

                if (name === "") {
                    msg.innerText = "Please enter a subject name.";
                    msg.style.display = "block";
                    msg.style.color = "red";
                    input.style.border = "2px solid red";
                    input.classList.add("shake");
                    return false;
                }

                let rows = document.querySelectorAll("#<%= gvSubjects.ClientID %> tbody tr td:nth-child(2)");

                for (let r of rows) {
                    if (r.innerText.trim().toLowerCase() === name) {
                        msg.innerText = "Subject already exists.";
                        msg.style.display = "block";
                        msg.style.color = "red";
                        input.style.border = "2px solid red";
                        input.classList.add("shake");
                        return false;
                    }
                }

                return true;
            }

            function validateSubjectEdit(index) {
                let msg = document.getElementById("subjectError");
                msg.style.display = "none";

                let inputs = document.getElementsByName("txtEditSubject");
                let input = inputs[index];
                let newName = input.value.trim().toLowerCase();

                input.style.border = "1px solid #ced4da";
                input.classList.remove("shake");

                if (newName === "") {
                    msg.innerText = "Subject name cannot be empty.";
                    msg.style.color = "red";
                    msg.style.display = "block";
                    input.style.border = "2px solid red";
                    input.classList.add("shake");
                    return false;
                }

                let rows = document.querySelectorAll("#<%= gvSubjects.ClientID %> tbody tr td:nth-child(2)");

                for (let i = 0; i < rows.length; i++) {
                    if (i !== index && rows[i].innerText.trim().toLowerCase() === newName) {
                        msg.innerText = "Subject already exists.";
                        msg.style.color = "red";
                        msg.style.display = "block";
                        input.style.border = "2px solid red";
                        input.classList.add("shake");
                        return false;
                    }
                }
                return true;
            }

            // EXAM duration auto-calc + validation
            function showExamMsg(type, msg) {
                let box = document.getElementById("examMsg");
                box.style.display = "block";
                box.className = "fw-bold alert alert-" + type;
                box.innerText = msg;
                setTimeout(() => box.style.display = "none", 4000);
            }

            function autoCalculateDuration() {
                let start = document.getElementById("<%= txtStartTime.ClientID %>").value;
                let end = document.getElementById("<%= txtEndTime.ClientID %>").value;
                let durationBox = document.getElementById("<%= txtDuration.ClientID %>");

                if (!start || !end) {
                    durationBox.value = "";
                    return;
                }

                let s = start.split(":");
                let e = end.split(":");

                let sMin = (+s[0] * 60) + (+s[1]);
                let eMin = (+e[0] * 60) + (+e[1]);

                let diff = eMin - sMin;

                if (diff <= 0) {
                    durationBox.value = "";
                    showExamMsg("danger", "End time must be after start time");
                    return;
                }

                if (diff > 180) {
                    durationBox.value = "";
                    document.getElementById("<%= txtEndTime.ClientID %>").value = "";
                    showExamMsg("danger", "Exam duration cannot exceed 3 hours!");
                    return;
                }

                durationBox.value = diff;  // SUCCESS
            }


            function validateExam() {
                let ok = true;
                let subject = document.getElementById("<%= ddlSubjects.ClientID %>");
                let title = document.getElementById("<%= txtExamTitle.ClientID %>");
                let date = document.getElementById("<%= txtExamDate.ClientID %>");
                let start = document.getElementById("<%= txtStartTime.ClientID %>");
                let end = document.getElementById("<%= txtEndTime.ClientID %>");
                let duration = document.getElementById("<%= txtDuration.ClientID %>");

                let errSub = document.getElementById("errSubject");
                let errTitle = document.getElementById("errTitle");
                let errDate = document.getElementById("errDate");
                let errStart = document.getElementById("errStart");
                let errEnd = document.getElementById("errEnd");
                let errDuration = document.getElementById("errDuration");

                errSub.innerText = errTitle.innerText = errDate.innerText =
                    errStart.innerText = errEnd.innerText = errDuration.innerText = "";

                subject.style.border = title.style.border = date.style.border =
                    start.style.border = end.style.border = duration.style.border = "1px solid #ced4da";

                if (!subject.value) {
                    errSub.innerText = "Please select a subject.";
                    subject.style.border = "2px solid red";
                    ok = false;
                }
                if (title.value.trim() === "") {
                    errTitle.innerText = "Exam title is required.";
                    title.style.border = "2px solid red";
                    ok = false;
                }
                if (!date.value) {
                    errDate.innerText = "Exam date is required.";
                    date.style.border = "2px solid red";
                    ok = false;
                }
                if (!start.value) {
                    errStart.innerText = "Start time required.";
                    start.style.border = "2px solid red";
                    ok = false;
                }
                if (!end.value) {
                    errEnd.innerText = "End time required.";
                    end.style.border = "2px solid red";
                    ok = false;
                }
                if (duration.value === "" || isNaN(duration.value) || duration.value <= 0) {
                    errDuration.innerText = "Duration is invalid.";
                    duration.style.border = "2px solid red";
                    ok = false;
                }

                return ok;
            }

            // TAB FIX: restore and save active tab + show AI button
            document.addEventListener("DOMContentLoaded", function () {
                // wire start/end for duration calculation
                var s = document.getElementById("<%= txtStartTime.ClientID %>");
                var e = document.getElementById("<%= txtEndTime.ClientID %>");
                if (s) s.addEventListener("change", autoCalculateDuration);
                if (e) e.addEventListener("change", autoCalculateDuration);

                // restore tab (server may store hdnActiveTab)
                var last = document.getElementById("<%= hdnActiveTab.ClientID %>").value;
                if (last) {
                    const tabBtn = document.querySelector(`[data-bs-target='${last}']`);
                    if (tabBtn) new bootstrap.Tab(tabBtn).show();
                }

                // save tab on change
                document.querySelectorAll('#adminTab button[data-bs-toggle="tab"]').forEach(btn => {
                    btn.addEventListener('shown.bs.tab', function (e) {
                        let selected = e.target.getAttribute("data-bs-target");
                        document.getElementById("<%= hdnActiveTab.ClientID %>").value = selected;
                        document.getElementById("btnAI").style.display = (selected === "#question") ? "inline-block" : "none";
                    });
                });
            });

            function enableLiveValidation() {

                const fields = [
                    { el: document.getElementById("<%= ddlSubjects.ClientID %>"), err: document.getElementById("errSubject") },
                    { el: document.getElementById("<%= txtExamTitle.ClientID %>"), err: document.getElementById("errTitle") },
                    { el: document.getElementById("<%= txtExamDate.ClientID %>"), err: document.getElementById("errDate") },
                    { el: document.getElementById("<%= txtStartTime.ClientID %>"), err: document.getElementById("errStart") },
                    { el: document.getElementById("<%= txtEndTime.ClientID %>"), err: document.getElementById("errEnd") },
                    { el: document.getElementById("<%= txtDuration.ClientID %>"), err: document.getElementById("errDuration") }
                ];

                fields.forEach(item => {
                    if (!item.el) return;

                    const eventType = (item.el.tagName === "SELECT" || item.el.type === "date" || item.el.type === "time")
                        ? "change"
                        : "keyup";

                    item.el.addEventListener(eventType, () => {
                        if (item.el.value.trim() !== "") {
                            item.err.innerText = "";
                            item.el.style.border = "2px solid #28a745"; // green border
                        } else {
                            item.el.style.border = "2px solid red";
                        }
                    });
                });
            }

            document.addEventListener("DOMContentLoaded", enableLiveValidation);

        </script>

    </div>
</asp:Content>
