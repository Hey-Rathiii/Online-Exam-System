<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html>
<head>
    <title>Disqualified</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>

<body class="bg-gray-900 text-gray-100 flex items-center justify-center min-h-screen">

    <div class="bg-red-600/20 border border-red-500/50 p-10 rounded-2xl text-center max-w-lg">
        <h1 class="text-4xl font-bold text-red-400 mb-4">❌ Disqualified</h1>

        <p class="text-gray-300 text-lg mb-4">
            You were removed from the exam due to a security violation.
        </p>

        <p class="text-sm text-gray-400 mb-6">
            Reason: <span class="text-red-300">
                <% Response.Write(Request.QueryString["reason"]); %>
            </span>
        </p>

        <a href="Home.aspx" class="px-6 py-3 bg-primary-500 hover:bg-primary-600 text-white rounded-xl">
            Return to Dashboard
        </a>
    </div>

</body>
</html>
