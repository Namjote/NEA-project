<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ai_settings.aspx.vb" Inherits="NEA_project.ai_settings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body {
            background-color: #1b1818;
            color: white;
            font-size: 20px;
            font-family: sans-serif;
        }

        #submit_btn {
            color: white;
            background-color: #333333;
            border: none;
            margin-left: 15px;
            font-size: 20px;
        }

        #stockfish_elo_txt {
            color: white;
            background-color: #333333;
            border: none;
        }

        form {
            margin-left: 400px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <p>
            Enter strength of AI in ELO below (between 1-3000):</p>
        <p>
            <asp:TextBox ID="stockfish_elo_txt" runat="server" Height="37px" Width="439px"></asp:TextBox>
            <asp:Button ID="submit_btn" runat="server" Text="Submit" Height="40px" Width="163px" />
        </p>
    </form>
</body>
</html>
