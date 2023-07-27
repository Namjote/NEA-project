Imports System.Security.Cryptography
Public Class Registration_and_Login
    Inherits System.Web.UI.Page
    Structure MD5
        Dim username As String
        Dim password As String
    End Structure
    Public connectCredentialTable As New DataSet1TableAdapters.credentialsTableAdapter
    Public connectAIgamesTable As New DataSet1TableAdapters.ai_gamesTableAdapter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        ' If the username inputted and the hash of the password is found in the database, then the credentials are correct
        If connectCredentialTable.SelectUsernamePasswordToLogin(txt_username_login.Text, GenerateHash(txt_password_login.Text)) = 1 Then
            Session("username") = txt_username_login.Text
            connectAIgamesTable.Resetai_gamesForAGivenUser(Session.Item("username"))
            Response.Redirect("main_menu.aspx") ' Send the user to the main menu
        Else
            MsgBox("Username or Password is incorrect")
        End If
    End Sub

    Protected Sub btn_register_Click(sender As Object, e As EventArgs) Handles btn_register.Click
        ' if the username inputted is already found in the credentials table in the database, then the user must make another username
        If connectCredentialTable.RegistrationUsernameCheck(txt_username_register.Text) = txt_username_register.Text Then
            MsgBox("Username already taken")
        Else
            If String.IsNullOrWhiteSpace(txt_username_register.Text) = True Or String.IsNullOrWhiteSpace(txt_password_register.Text) = True Then
                MsgBox("Please enter a username and password") ' Username or password inputted is blank or whitespace
            Else
                ' Username is unique so write the username and the hash of the password to the credentials table 
                connectCredentialTable.RegistrationWrite(txt_username_register.Text, GenerateHash(txt_password_register.Text))
                connectAIgamesTable.AddRowTo_ai_games(txt_username_register.Text)
            End If
        End If
    End Sub

    ' Hashing the passwords proves extra security to my stakeholder details.
    ' Unauthorised access to my credentials table means malicious actors cannot read stakeholder passwords
    Private Function GenerateHash(ByVal SourceText As String) As String
        Dim md5Object As New MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(SourceText)
        bytesToHash = md5Object.ComputeHash(bytesToHash) 'convert to md5 object

        For Each b As Byte In bytesToHash
            GenerateHash &= b.ToString("x2")
        Next
        Return GenerateHash
    End Function

End Class