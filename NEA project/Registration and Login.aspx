<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Registration and Login.aspx.vb" Inherits="NEA_project.Registration_and_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 84px;
        }
        .auto-style3 {
            height: 23px;
        }
        .auto-style4 {
            width: 84px;
            height: 23px;
        }
        .auto-style5 {
            width: 142px;
        }
        .auto-style6 {
            height: 23px;
            width: 142px;
        }
        .auto-style7 {
            width: 155px;
        }
        .auto-style8 {
            height: 23px;
            width: 155px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">Login:</td>
                    <td>&nbsp;</td>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">Username</td>
                    <td>&nbsp;</td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txt_username_login" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">Password</td>
                    <td>&nbsp;</td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txt_password_login" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="auto-style5">
                        <asp:Button ID="btn_login" runat="server" Text="Submit" />
                    </td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">
                        &nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">Register:</td>
                    <td>&nbsp;</td>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style7">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">Username</td>
                    <td>&nbsp;</td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txt_username_register" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style7">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_username_register" ErrorMessage="Please enter a username" ValidationGroup="1"></asp:RequiredFieldValidator>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3"></td>
                    <td class="auto-style4">Password</td>
                    <td class="auto-style3"></td>
                    <td class="auto-style6">
                        <asp:TextBox ID="txt_password_register" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style8">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_password_register" ErrorMessage="Please enter a password" ValidationGroup="1"></asp:RequiredFieldValidator>
                    </td>
                    <td class="auto-style3"></td>
                    <td class="auto-style3"></td>
                    <td class="auto-style3"></td>
                    <td class="auto-style3"></td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style6">
                        <asp:Button ID="btn_register" runat="server" Text="Submit" ValidationGroup="1" />
                    </td>
                    <td class="auto-style8">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style8">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
