<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IIMTONLINEEXAM.Admin.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:HiddenField ID="hdnActiveTab" runat="server" />
 <div class="container mt-4">
     <h3 class="mb-4">🧾 Admin Dashboard</h3>

     <!-- Tab Navigation -->
     <ul class="nav nav-tabs" id="adminTab" role="tablist">
         <li class="nav-item" role="presentation">
             <button class="nav-link active" id="subject-tab" data-bs-toggle="tab" data-bs-target="#subject" type="button" role="tab">Subject</button>
         </li>
         <li class="nav-item " role="presentation">
             <button class="nav-link " id="exam-tab" data-bs-toggle="tab" data-bs-target="#exam" type="button" role="tab">Exam</button>
         </li>
         <li class="nav-item" role="presentation">
             <button class="nav-link" id="question-tab" data-bs-toggle="tab" data-bs-target="#question" type="button" role="tab">Question</button>
         </li>
     </ul>

     
     <!-- Tab Content -->
     <div class="tab-content border p-4" id="adminTabContent">
         <!-- Subject Tab -->
         <div class="tab-pane fade show active" id="subject" role="tabpanel" aria-labelledby="subject-tab">
             <h5>Create Subject</h5>
             <!-- Add your Subject form here -->
             <asp:Panel ID="pnlAddSubject" runat="server">
                 <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
                 <div class="form-group">
                     <asp:TextBox ID="txtSubjectName" runat="server" CssClass="form-control" placeholder="Enter Subject Name"></asp:TextBox>
                 </div>
                 <asp:Button ID="btnAddSubject" runat="server" Text="Add Subject" CssClass="btn btn-primary mt-2" OnClick="btnAddSubject_Click" />
                 <hr />

                 <asp:GridView ID="gvSubjects" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered mt-3"
                     DataKeyNames="SubjectID"
                     OnRowEditing="gvSubjects_RowEditing"
                     OnRowUpdating="gvSubjects_RowUpdating"
                     OnRowCancelingEdit="gvSubjects_RowCancelingEdit"
                     OnRowCommand="gvSubjects_RowCommand">
                     <Columns>
                         <asp:BoundField DataField="SubjectID" HeaderText="ID" ReadOnly="True" />
                         <asp:BoundField DataField="SubjectName" HeaderText="Subject Name" />
                         <asp:BoundField DataField="IsActive" HeaderText="Status" ReadOnly="True" />
                         <asp:CommandField ShowEditButton="True" />
                         <asp:TemplateField HeaderText="Actions">
                             <ItemTemplate>
                                 <asp:LinkButton ID="lnkToggleStatus" runat="server"
                                     CommandName="ToggleStatus"
                                     CommandArgument='<%# Eval("SubjectID") + "|" + Eval("IsActive") %>'
                                     Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Deactivate" : "Activate" %>' />

                                 <asp:LinkButton ID="lnkDelete" runat="server"
                                     CommandName="DeleteSubject"
                                     CommandArgument='<%# Eval("SubjectID") %>'
                                     Text="Delete"
                                     OnClientClick="return confirm('Are you sure you want to delete this subject?');"
                                     CssClass="text-danger" />

                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                 </asp:GridView>
             </asp:Panel>
         </div>

         <!-- Exam Tab -->
         <div class="tab-pane fade " id="exam" role="tabpanel" aria-labelledby="exam-tab">
             <%-- <asp:Button ID="btnShowExam" runat="server" Text="Load Exam" OnClick="btnShowExam_Click" Style="display: none;" />--%>

             <h5>Create Exam</h5>
             <!-- Add your Exam form here -->
             <asp:Panel ID="pnlExams" runat="server">
                 <h5>Add New Exam</h5>
                 <table class="table table-borderless w-75">
                     <tr>
                         <td>Subject:</td>
                         <td>
                             <asp:DropDownList ID="ddlSubjects" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                 <asp:ListItem Text="-- Select Subject --" Value="" />
                             </asp:DropDownList>
                         </td>
                     </tr>
                     <tr>
                         <td>Exam Title:</td>
                         <td>
                             <asp:TextBox ID="txtExamTitle" runat="server" CssClass="form-control" /></td>
                     </tr>
                     <tr>
                         <td>Exam Date:</td>
                         <td>
                             <asp:TextBox ID="txtExamDate" runat="server" TextMode="Date" CssClass="form-control" /></td>
                     </tr>
                     <tr>
                         <td>Start Time:</td>
                         <td>
                             <asp:TextBox ID="txtStartTime" runat="server" TextMode="Time" CssClass="form-control" /></td>
                     </tr>
                     <tr>
                         <td>End Time:</td>
                         <td>
                             <asp:TextBox ID="txtEndTime" runat="server" TextMode="Time" CssClass="form-control" /></td>
                     </tr>
                     <tr>
                         <td>Duration (mins):</td>
                         <td>
                             <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" /></td>
                     </tr>
                     <tr>
                         <td></td>
                         <td>
                             <asp:Button ID="btnAddExam" runat="server" Text="Add Exam" CssClass="btn btn-primary" OnClick="btnAddExam_Click" />
                         </td>
                     </tr>
                 </table>

                 <hr />

                 <h5>All Exams</h5>


                 <asp:GridView ID="gvExams" runat="server" AutoGenerateColumns="False"
                     OnRowEditing="gvExams_RowEditing"
                     OnRowCancelingEdit="gvExams_RowCancelingEdit"
                     OnRowUpdating="gvExams_RowUpdating"
                     DataKeyNames="ExamID"
                     CssClass="table table-bordered">
                     <Columns>
                         <asp:TemplateField HeaderText="Subject">
                             <ItemTemplate>
                                 <%# Eval("SubjectName") %>
                             </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:DropDownList ID="ddlSubjectEdit" runat="server" />
                             </EditItemTemplate>
                         </asp:TemplateField>

                         <asp:TemplateField HeaderText="Title">
                             <ItemTemplate>
                                 <%# Eval("ExamTitle") %>
                             </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:TextBox ID="txtExamTitleEdit" runat="server" Text='<%# Eval("ExamTitle") %>' CssClass="form-control" />
                             </EditItemTemplate>
                         </asp:TemplateField>

                         <asp:TemplateField HeaderText="Date">
                             <ItemTemplate>
                                 <%# Eval("ExamDate", "{0:yyyy-MM-dd}") %>
                             </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:TextBox ID="txtExamDateEdit" runat="server" Text='<%# Eval("ExamDate", "{0:yyyy-MM-dd}") %>' CssClass="form-control" TextMode="Date" />
                             </EditItemTemplate>
                         </asp:TemplateField>

                         <asp:TemplateField HeaderText="Start Time">
                             <ItemTemplate>
                                 <%# Eval("StartTime") %>
                             </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:TextBox ID="txtStartTimeEdit" runat="server" Text='<%# Eval("StartTime") %>' CssClass="form-control" TextMode="Time" />
                             </EditItemTemplate>
                         </asp:TemplateField>

                         <asp:TemplateField HeaderText="End Time">
                             <ItemTemplate>
                                 <%# Eval("EndTime") %>
                             </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:TextBox ID="txtEndTimeEdit" runat="server" Text='<%# Eval("EndTime") %>' CssClass="form-control" TextMode="Time" />
                             </EditItemTemplate>
                         </asp:TemplateField>

                         <asp:TemplateField HeaderText="Duration">
                             <ItemTemplate>
                                 <%# Eval("DurationMinutes") %>
                             </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:TextBox ID="txtDurationEdit" runat="server" Text='<%# Eval("DurationMinutes") %>' CssClass="form-control" />
                             </EditItemTemplate>
                         </asp:TemplateField>

                         <asp:CommandField ShowEditButton="True" />
                     </Columns>
                 </asp:GridView>


             </asp:Panel>
         </div>

         <!-- Question Tab -->
         <div class="tab-pane fade" id="question" role="tabpanel" aria-labelledby="question-tab">
             <%--<asp:Button ID="btnShowQue" runat="server" type="button"  Text="Add Exam" CssClass="btn btn-primary" OnClick="btnShowQue_Click" Style="display: none;"/>--%>
                 <div>
                     <h5>Create Question</h5>
                     <!-- Add your Question form here -->
                     <p>Question and answer creation form goes here.</p>
                 </div>
                 

             <h3>Insert New Question</h3>
             <asp:Panel ID="pnlInsertQuestion" runat="server">
                 <table class="question-form-table">

                     <tr>
                         <td>Exam:</td>
                         <td>
                             <asp:DropDownList ID="ddlExams" runat="server" AutoPostBack="true"
                                 OnSelectedIndexChanged="ddlExams_SelectedIndexChanged">
                             </asp:DropDownList>

                         </td>
                     </tr>
                     <tr>
                         <td>Question:</td>
                         <td>
                             <asp:TextBox ID="txtQuestion" runat="server" TextMode="MultiLine" Width="400px" Height="60px"></asp:TextBox></td>
                     </tr>
                     <tr>
                         <td>Option A:</td>
                         <td>
                             <asp:TextBox ID="txtOptionA" runat="server"></asp:TextBox></td>
                     </tr>
                     <tr>
                         <td>Option B:</td>
                         <td>
                             <asp:TextBox ID="txtOptionB" runat="server"></asp:TextBox></td>
                     </tr>
                     <tr>
                         <td>Option C:</td>
                         <td>
                             <asp:TextBox ID="txtOptionC" runat="server"></asp:TextBox></td>
                     </tr>

                     <tr>
                         <td>Option D:</td>
                         <td>
                             <asp:TextBox ID="txtOptionD" runat="server"></asp:TextBox></td>
                     </tr>
                     <tr>
                         <td>Correct Option (A/B/C/D):</td>
                         <td>
                             <asp:TextBox ID="txtCorrectOption" runat="server" MaxLength="1"></asp:TextBox></td>
                     </tr>
                     <tr>
                         <td>Marks:</td>
                         <td>
                             <asp:TextBox ID="txtMarks" runat="server"></asp:TextBox></td>
                     </tr>
                     <tr>
                         <td colspan="2">
                             <asp:Button ID="btnSaveQuestion" runat="server" Text="Save Question" CssClass="btn-save-question" OnClick="btnSaveQuestion_Click" />
                         </td>
                     </tr>
                 </table>
                 <%-- <asp:Button ID="btnQue" runat="server" />--%>
             </asp:Panel>

             <hr />

             <h3>All Questions</h3>
             <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="False"
                 DataKeyNames="QuestionID"
                 OnRowEditing="gvQuestions_RowEditing"
                 OnRowUpdating="gvQuestions_RowUpdating"
                 OnRowCancelingEdit="gvQuestions_RowCancelingEdit"
                 OnRowDeleting="gvQuestions_RowDeleting"
                 CssClass="table table-bordered">

                 <Columns>


                     <asp:TemplateField HeaderText="Question Text">
                         <ItemTemplate>
                             <%# Eval("QuestionText") %>
                         </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="txtQuestionText" runat="server" Text='<%# Bind("QuestionText") %>' CssClass="form-control" />
                         </EditItemTemplate>
                     </asp:TemplateField>


                     <asp:TemplateField HeaderText="Option A">
                         <ItemTemplate>
                             <%# Eval("OptionA") %>
                         </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="txtOptionA" runat="server" Text='<%# Bind("OptionA") %>' CssClass="form-control" />
                         </EditItemTemplate>
                     </asp:TemplateField>


                     <asp:TemplateField HeaderText="Option B">
                         <ItemTemplate>
                             <%# Eval("OptionB") %>
                         </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="txtOptionB" runat="server" Text='<%# Bind("OptionB") %>' CssClass="form-control" />
                         </EditItemTemplate>
                     </asp:TemplateField>

                     <asp:TemplateField HeaderText="Option C">
                         <ItemTemplate>
                             <%# Eval("OptionC") %>
                         </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="txtOptionC" runat="server" Text='<%# Bind("OptionC") %>' CssClass="form-control" />
                         </EditItemTemplate>
                     </asp:TemplateField>


                     <asp:TemplateField HeaderText="Option D">
                         <ItemTemplate>
                             <%# Eval("OptionD") %>
                         </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="txtOptionD" runat="server" Text='<%# Bind("OptionD") %>' CssClass="form-control" />
                         </EditItemTemplate>
                     </asp:TemplateField>


                     <asp:TemplateField HeaderText="Correct Option">
                         <ItemTemplate>
                             <%# Eval("CorrectOption") %>
                         </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="txtCorrect" runat="server" Text='<%# Bind("CorrectOption") %>' CssClass="form-control" MaxLength="1" />
                         </EditItemTemplate>
                     </asp:TemplateField>


                     <asp:TemplateField HeaderText="Marks">
                         <ItemTemplate>
                             <%# Eval("Marks") %>
                         </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="txtMarks" runat="server" Text='<%# Bind("Marks") %>' CssClass="form-control" />
                         </EditItemTemplate>
                     </asp:TemplateField>


                     <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />

                 </Columns>
             </asp:GridView>
             </div>
         </div>
     </div>

 <%-- <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<!-- Bootstrap JS (needed for tab switching) -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>--%>    
 <script type="text/javascript">
    
     $(document).ready(function () {
       
       const activeTab = $('#<%= hdnActiveTab.ClientID %>').val();
    if (activeTab) {
        const triggerEl = document.querySelector(`[data-bs-target="${activeTab}"]`);
        if (triggerEl) {
            new bootstrap.Tab(triggerEl).show();
        }
    }
    const tabTriggerEls = document.querySelectorAll('#adminTab button[data-bs-toggle="tab"]');
    tabTriggerEls.forEach(el => {
        el.addEventListener('shown.bs.tab', function (event) {
            const selectedTab = event.target.getAttribute("data-bs-target");
            $('#<%= hdnActiveTab.ClientID %>').val(selectedTab);
        });
    });
});
     $(function () {
         // Duration calculation
         $('#<%= txtStartTime.ClientID %>, #<%= txtEndTime.ClientID %>').on('change', function () {
        const start = $('#<%= txtStartTime.ClientID %>').val();
        const end = $('#<%= txtEndTime.ClientID %>').val();

        if (start && end) {
            const startTime = new Date(`1970-01-01T${start}`);
            const endTime = new Date(`1970-01-01T${end}`);

            if (endTime > startTime) {
                const durationMinutes = Math.floor((endTime - startTime) / 60000);
                $('#<%= txtDuration.ClientID %>').val(durationMinutes);
            } else {
                $('#<%= txtDuration.ClientID %>').val('');
                alert("End time must be after start time.");
            }
        }
    });

    $('#<%= btnAddExam.ClientID %>').click(function (e) {
        let isValid = true;

        // Reset styles
        $('input, select').css("border", "1px solid #ccc");

        const subject = $('#<%= ddlSubjects.ClientID %>');
    const title = $('#<%= txtExamTitle.ClientID %>');
    const date = $('#<%= txtExamDate.ClientID %>');
    const start = $('#<%= txtStartTime.ClientID %>');
    const end = $('#<%= txtEndTime.ClientID %>');
    const duration = $('#<%= txtDuration.ClientID %>');

    // Subject validation
    if (!subject.val()) {
        subject.css("border", "2px solid red");
        isValid = false;
    }

    // Title
    if (!title.val().trim()) {
        title.val("");
        title.attr("placeholder", "Exam title is required");
        title.css("border", "2px solid red");
        isValid = false;
    }

    // Date
    if (!date.val()) {
        date.attr("placeholder", "Exam date is required");
        date.css("border", "2px solid red");
        isValid = false;
    }

    // Start time
    if (!start.val()) {
        start.attr("placeholder", "Start time required");
        start.css("border", "2px solid red");
        isValid = false;
    }

    // End time
    if (!end.val()) {
        end.attr("placeholder", "End time required");
        end.css("border", "2px solid red");
        isValid = false;
    }

    // Duration
    if (!duration.val().trim()) {
        duration.val("");
        duration.attr("placeholder", "Enter duration");
        duration.css("border", "2px solid red");
        isValid = false;
    } else if (isNaN(duration.val().trim())) {
        duration.val("");
        duration.attr("placeholder", "Duration must be a number");
        duration.css("border", "2px solid red");
        isValid = false;
    }

    // Time validation logic
    if (start.val() && end.val()) {
        const [startH, startM] = start.val().split(":").map(Number);
        const [endH, endM] = end.val().split(":").map(Number);

        const startTotal = startH * 60 + startM;
        const endTotal = endH * 60 + endM;
        const diff = endTotal - startTotal;

        if (diff <= 0) {
            end.val("");
            end.attr("placeholder", "End time must be after start");
            end.css("border", "2px solid red");
            isValid = false;
        } else if (diff < 60) {
            end.val("");
            end.attr("placeholder", "Minimum 1 hour gap required");
            end.css("border", "2px solid red");
            isValid = false;
        } else if (diff > 180) {
            alert("Maximum allowed exam duration is 3 hours.");
            end.val("");
            end.attr("placeholder", "Max allowed is 3 hours");
            end.css("border", "2px solid red");
            isValid = false;
        }
    }

    if (!isValid) {
        e.preventDefault();
    }
});

    window.onload = function () {
        // Optional: disable past dates in the calendar popup
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0');
        var yyyy = today.getFullYear();

        var minDate = yyyy + '-' + mm + '-' + dd;
        var examDateInput = document.getElementById('<%= txtExamDate.ClientID %>');
        if (examDateInput) {
            examDateInput.setAttribute("min", minDate);
        }
    };




    $(document).on('input', '[id$=txtMarks]', function () {
        this.value = this.value.replace(/[^0-9]/g, ''); // remove non-numeric
    });

     });

 </script>




</asp:Content>
