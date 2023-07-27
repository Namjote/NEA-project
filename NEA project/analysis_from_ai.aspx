<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="analysis_from_ai.aspx.vb" Inherits="NEA_project.analysis_from_ai" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .class {
            position: absolute;
            background-color: #333333;
            margin-left: 90px;
            margin-top: 90px;
        }

        body {
            background-color: #1b1818;
            color: white;
            font-size: 20px;
            height: 100%;
            font-family: sans-serif;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Table id="Table1" class="table" runat="server" CellPadding="5" CellSpacing="1" Gridlines="Both" BorderColor="white" BorderWidth="1" HorizontalAlign="Justify"></asp:Table>
    </form>
</body>
</html>
