Public Class Registration_and_Login
    Inherits System.Web.UI.Page
    Public connectCredentialTable As New DataSet1TableAdapters.credentialsTableAdapter
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub txt_username_TextChanged(sender As Object, e As EventArgs) Handles txt_username_login.TextChanged

    End Sub

    Protected Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        If connectCredentialTable.SelectUsernamePasswordToLogin(txt_username_login.Text, txt_password_login.Text) = 1 Then
            Response.Redirect("Main Menu.aspx")
        Else
            MsgBox("Username or Password is incorrect")
        End If
    End Sub

    Protected Sub btn_register_Click(sender As Object, e As EventArgs) Handles btn_register.Click
        If connectCredentialTable.RegistrationUsernameCheck(txt_username_register.Text) = txt_username_register.Text Then
            MsgBox("Username already taken")
        Else
            connectCredentialTable.RegistrationWrite(txt_username_register.Text, txt_password_register.Text)
        End If
    End Sub
End Class