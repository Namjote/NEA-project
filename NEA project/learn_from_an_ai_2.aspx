<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="learn_from_an_ai_2.aspx.vb" Inherits="NEA_project.learn_from_an_ai_2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .auto-style1 {
            width: 10px;
            height: 10px;
        }
        .auto-style2 {
            width: 500px;
            height: 500px;
            margin: 100px 540px;
            z-index: 1;

        }
        .auto-style3 {
            width: 20px;
            height: 18px;
        }

        #promotion-menu {
            width:250px;
            height:63px;
            background-color: indianred;
            margin-left: 50px;
            margin-bottom: 100px;
            z-index: 1;
            margin-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table id="table_chess_board" class="auto-style2">
                <tr>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/chess pieces/bR.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/chess pieces/bN.png" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/chess pieces/bB.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/chess pieces/bQ.png" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/chess pieces/bK.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/chess pieces/bB.png" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/chess pieces/bN.png" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/chess pieces/bR.png" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/chess pieces/bP.png" />
                    </td>
                    <td class="auto-style3" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/chess pieces/bP.png" />
                    </td>
                    <td class="auto-style3" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton11" runat="server" ImageUrl="~/chess pieces/bP.png" />
                    </td>
                    <td class="auto-style3" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton12" runat="server" ImageUrl="~/chess pieces/bP.png" />
                    </td>
                    <td class="auto-style3" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="~/chess pieces/bP.png" />
                    </td>
                    <td class="auto-style3" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="~/chess pieces/bP.png" />
                    </td>
                    <td class="auto-style3" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton15" runat="server" ImageUrl="~/chess pieces/bP.png" />
                    </td>
                    <td class="auto-style3" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton16" runat="server" ImageUrl="~/chess pieces/bP.png" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton17" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton18" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton19" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton20" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton21" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton22" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton23" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton24" runat="server" Height="60px" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton25" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton26" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton27" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton28" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton29" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton30" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton31" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton32" runat="server" Height="60px" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton33" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton34" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton35" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton36" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton37" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton38" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton39" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton40" runat="server" Height="60px" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton41" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton42" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton43" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton44" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton45" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton46" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton47" runat="server" Height="60px" Width="60px" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton48" runat="server" Height="60px" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton49" runat="server" ImageUrl="~/chess pieces/wP.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton50" runat="server" ImageUrl="~/chess pieces/wP.png" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton51" runat="server" ImageUrl="~/chess pieces/wP.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton52" runat="server" ImageUrl="~/chess pieces/wP.png" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton53" runat="server" ImageUrl="~/chess pieces/wP.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton54" runat="server" ImageUrl="~/chess pieces/wP.png" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton55" runat="server" ImageUrl="~/chess pieces/wP.png" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton56" runat="server" ImageUrl="~/chess pieces/wP.png" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton57" runat="server" ImageUrl="~/chess pieces/wR.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton58" runat="server" ImageUrl="~/chess pieces/wN.png" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton59" runat="server" ImageUrl="~/chess pieces/wB.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton60" runat="server" ImageUrl="~/chess pieces/wQ.png" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton61" runat="server" ImageUrl="~/chess pieces/wK.png" />
                    </td>
                    <td class="auto-style1" style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton62" runat="server" ImageUrl="~/chess pieces/wB.png" />
                    </td>
                    <td style="background-color: #B88B4A">
                        <asp:ImageButton ID="ImageButton63" runat="server" ImageUrl="~/chess pieces/wN.png" />
                    </td>
                    <td style="background-color: #E3C16F">
                        <asp:ImageButton ID="ImageButton64" runat="server" ImageUrl="~/chess pieces/wR.png" />
                    </td>
                </tr>
            </table>
        <div id="promotion-menu">
                <asp:ImageButton ID="Q_promote_btn" runat="server" ImageUrl="~/chess pieces/wQ.png" />
                <asp:ImageButton ID="N_promote_btn" runat="server" ImageUrl="~/chess pieces/wN.png" />
                <asp:ImageButton ID="R_promote_btn" runat="server" ImageUrl="~/chess pieces/wR.png" />
                <asp:ImageButton ID="B_promote_btn" runat="server" ImageUrl="~/chess pieces/wB.png" />
        </div>
    </form>
</body>
</html>
