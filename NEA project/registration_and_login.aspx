<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="registration_and_login.aspx.vb" Inherits="NEA_project.Registration_and_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    </head>
    <style>
        form {
            background-color: #1f1e1e;
            color: white;
            font-size: 20px;
            height: 100%;
        }
        body {
             
            background-color: #1f1e1e;
            color: white;
            font-size: 20px;
            height: 100%;
            font-family: sans-serif;
        }
        
        .submit {
            background-color: #333333;
            font-size: 20px;
            color: white;
            border: none;
        }

        .textbox {
            background-color: #333333;
            font-size: 20px;
            color: white;
            border: none;
            margin-left: 10px;
        }
        .auto-style1 {
            margin-left: 0px;
            background-color: #333333;
            font-size: 20px;
            color: white;
            border: none;
        }
        .auto-style2 {
            margin-left: 13px;
            background-color: #333333;
            font-size: 20px;
            color: white;
            border: none;
        }
    </style>
<body>
    <form id="form1" runat="server">
        <p>Login</p>
        <p>Username:&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox class="textbox" ID="txt_username_login" runat="server" Width="256px" CssClass="auto-style1"></asp:TextBox></p>
        <p>Password:&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox class="textbox" ID="txt_password_login" runat="server" TextMode="Password" Width="255px"></asp:TextBox></p>
        <p><asp:Button ID="btn_login" class="submit" runat="server" Text="Submit" Width="99px" /></p>

        <p> &nbsp;</p>

        <p>Register</p>
        <p>Username:&nbsp;&nbsp;&nbsp;<asp:TextBox class="textbox" ID="txt_username_register" runat="server" Width="237px"></asp:TextBox></p>
        <p>Password:&nbsp;&nbsp;&nbsp;<asp:TextBox class="textbox" ID="txt_password_register" runat="server" TextMode="Password" Width="239px" CssClass="auto-style2"></asp:TextBox></p>
        <p><asp:Button ID="btn_register" class="submit" runat="server" Text="Submit" ValidationGroup="1" /></p>
    </form>
</body>
</html>
