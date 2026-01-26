<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportQuestions.aspx.cs" Inherits="IIMTONLINEEXAM.Admin.ImportQuestions" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Import Questions</title>
    <meta charset="utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background: #f5f6fa;
            font-family: 'Segoe UI', sans-serif;
        }
        .card {
            border-radius: 14px;
            box-shadow: 0 6px 14px rgba(0,0,0,0.1);
            border: none;
        }
        .table td {
            white-space: normal !important;
        }
        .table th {
            background: #e9ecef;
        }
    </style>
</head>

<body>

<div class="container py-4">
    <a href="Dashboard.aspx?tab=question" class="btn btn-secondary mb-3">← Back</a>

    <form id="form1" runat="server">
        <div class="card p-4">

            <h3 class="text-primary mb-3">Import Questions (PDF)</h3>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label class="form-label">Subject</label>
                    <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-6">
                    <label class="form-label">Exam</label>
                    <asp:DropDownList ID="ddlExam" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-9">
                    <label class="form-label">Upload PDF</label>
                    <asp:FileUpload ID="filePDF" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-3 d-flex align-items-end">
                    <asp:Button ID="btnExtract" runat="server" Text="Extract Questions" CssClass="btn btn-primary w-100" OnClick="btnExtract_Click" />
                </div>
            </div>

            <asp:Label ID="lblMsg" runat="server" CssClass="fw-bold text-danger mb-3 d-block"></asp:Label>

            <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover">

                <Columns>

                    <asp:TemplateField HeaderText="Question">
                        <ItemTemplate>
                            <asp:Label ID="lblQ" runat="server" Text='<%# Eval("QuestionText") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Option A">
                        <ItemTemplate>
                            <asp:Label ID="lblA" runat="server" Text='<%# Eval("A") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Option B">
                        <ItemTemplate>
                            <asp:Label ID="lblB" runat="server" Text='<%# Eval("B") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Option C">
                        <ItemTemplate>
                            <asp:Label ID="lblC" runat="server" Text='<%# Eval("C") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Option D">
                        <ItemTemplate>
                            <asp:Label ID="lblD" runat="server" Text='<%# Eval("D") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Correct">
                        <ItemTemplate>
                            <asp:Label ID="lblCorrect" runat="server" Text='<%# Eval("CorrectOption") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

            <asp:Button ID="btnSave" runat="server" Text="SAVE ALL QUESTIONS" CssClass="btn btn-success mt-3 w-100" OnClick="btnSave_Click" />

        </div>
    </form>
</div>

</body>
</html>
