Public Class Main_Menu
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btn_create_game_Click(sender As Object, e As EventArgs) Handles btn_create_game.Click
        Response.Redirect("Create game.aspx") 'hi
    End Sub

    Protected Sub btn_leaderboards_Click(sender As Object, e As EventArgs) Handles btn_leaderboards.Click
        Response.Redirect("Leaderboards.aspx")
    End Sub
End Class