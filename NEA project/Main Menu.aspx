<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Main Menu.aspx.vb" Inherits="NEA_project.Main_Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btn_create_game" runat="server" Text="Create game" Width="119px" />
        </div>
        <p>
            <asp:Button ID="btn_leaderboards" runat="server" Text="Leaderboards" />
        </p>
    </form>
</body>
</html>
