<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Leaderboards.aspx.vb" Inherits="NEA_project.leaderboards" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btn_global_leaderboard" runat="server" Text="Global Leaderboard" />
        </div>
        <p>
            <asp:Button ID="btn_personal_statistics" runat="server" Text="Personal Statistics" Width="213px" />
        </p>
        <p>
            &nbsp;</p>
    </form>
</body>
</html>
