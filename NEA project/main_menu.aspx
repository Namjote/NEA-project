<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="main_menu.aspx.vb" Inherits="NEA_project.Main_Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

        body {
            background-color: #1b1818;
            height: 100%;
            font-family: sans-serif;
        }

        #form1 {
            background-color: #1b1818;
            height: 100%;
            margin-left: 550px;
            margin-top: 300px;
        }

        .btn {
           background-color: #333333;
           font-size: 20px;
           color: white;
           border: none;
        }
       
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button class="btn" ID="btn_two_player_game" runat="server" Text="Two Player" Width="400px" Height="50px" />
        </div>
        <p>
            <asp:Button class="btn" ID="btn_learn_from_an_AI" runat="server" Text="Learn from an AI" Width="400px" Height="50px" />
        </p>
        <p>
            <asp:Button class="btn" ID="btn_historical_games" runat="server" Text="Historical Games" Width="400px" Height="50px"/>
        </p>
            <asp:Button class="btn" ID="btn_analysis" runat="server" Text="Analysis" Width="400px" Height="50px"/>
    </form>
</body>
</html>
