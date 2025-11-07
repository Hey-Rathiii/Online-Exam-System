<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="AiQuestionGenerator.aspx.cs" Inherits="IIMTONLINEEXAM.Admin.AiQuestionGenerator" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AI Question Generator</title>
    <meta charset="utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background: linear-gradient(135deg, #e3f2fd, #f8f9fa);
            font-family: 'Segoe UI', sans-serif;
        }

        .card {
            border-radius: 1.25rem;
            box-shadow: 0 5px 25px rgba(0, 0, 0, 0.1);
        }

        .page-title {
            text-align: center;
            font-weight: 700;
            color: #0d6efd;
            margin-bottom: 20px;
        }

        .form-label {
            font-weight: 600;
        }

        .btn-primary, .btn-success {
            border-radius: 50px;
            padding: 10px 24px;
        }

        .table {
            border-radius: 0.5rem;
            overflow: hidden;
        }

        /* Loader Overlay */
        #loaderOverlay {
            display: none;
            position: fixed;
            inset: 0;
            background-color: rgba(255, 255, 255, 0.9);
            z-index: 9999;
            justify-content: center;
            align-items: center;
            flex-direction: column;
            transition: opacity 0.3s ease;
            opacity: 0;
        }

        #loaderOverlay.visible {
            display: flex;
            opacity: 1;
        }

        #loaderOverlay.fading {
            opacity: 0;
        }

        #loaderOverlay .spinner-border {
            width: 3.5rem;
            height: 3.5rem;
            color: #0d6efd;
        }

        #loaderOverlay span {
            margin-top: 12px;
            font-weight: 600;
            color: #0d6efd;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

        <div class="container py-5">
            <div class="card p-4 mx-auto" style="max-width: 850px;">
                <h2 class="page-title">AI Question Generator</h2>

                <div class="row mb-3">
                    <div class="col-md-6">
                        <label class="form-label">Select Subject</label>
                        <asp:DropDownList ID="ddlSubjectsAI" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSubjectsAI_SelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label">Select Exam</label>
                        <asp:DropDownList ID="ddlExamsAI" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                </div>

                <div class="row mb-4">
                    <div class="col-md-12">
                        <label class="form-label">Enter Topic</label>
                        <asp:TextBox ID="txtTopic" runat="server" CssClass="form-control" placeholder="e.g. Photosynthesis"></asp:TextBox>
                    </div>
                </div>

                <div class="text-center mb-3">
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Questions"
                        CssClass="btn btn-primary fw-bold" OnClick="btnGenerate_Click" OnClientClick="showLoader();" />
                </div>

                <asp:Label ID="lblOutput" runat="server" CssClass="fw-semibold text-center d-block mb-3 text-danger"></asp:Label>

                <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover text-center">
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Question">
                            <ItemTemplate>
                                <asp:Literal ID="litQuestion" runat="server" Text='<%# Eval("QuestionText").ToString().Replace("\n", "<br/>") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <div class="text-center mt-3">
                    <asp:Button ID="btnSaveSelected" runat="server" Text="Save Selected Questions"
                        CssClass="btn btn-success fw-bold" OnClick="btnSaveSelected_Click" />
                </div>
            </div>
        </div>

        <!-- Loader Overlay -->
        <div id="loaderOverlay" aria-hidden="true">
            <div class="spinner-border" role="status"></div>
            <span>Generating AI Questions... Please wait</span>
        </div>

        <script type="text/javascript">
            (function () {
                var overlay = document.getElementById('loaderOverlay');
                var prm = null;

                window.showLoader = function () {
                    if (!overlay) return;
                    overlay.style.display = 'flex';
                    setTimeout(function () {
                        overlay.classList.add('visible');
                        overlay.setAttribute('aria-hidden', 'false');
                    }, 10);
                };

                window.hideLoader = function () {
                    if (!overlay) return;
                    overlay.classList.add('fading');
                    overlay.classList.remove('visible');
                    setTimeout(function () {
                        overlay.style.display = 'none';
                        overlay.classList.remove('fading');
                        overlay.setAttribute('aria-hidden', 'true');
                    }, 350);
                };

                if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    prm = Sys.WebForms.PageRequestManager.getInstance();

                    prm.add_beginRequest(function () {
                        showLoader();
                    });

                    prm.add_endRequest(function () {
                        hideLoader();
                    });
                }
            })();
        </script>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
