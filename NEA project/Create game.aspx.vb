Public Class Create_game
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btn_create_game_submit_Click(sender As Object, e As EventArgs) Handles btn_create_game_submit.Click
        Dim cookieObject As New HttpCookie("difficulty")
        Dim cookieObject1 As New HttpCookie("clock speed")
        Dim cookieObject2 As New HttpCookie("first_move")
        Dim cookieObject3 As New HttpCookie("player_turn")
        'initialising start of chess board
        Dim positions(7, 7) As String
        Dim first_row = New String() {"R", "N", "B", "Q", "K", "B", "N", "R"}

        For i As Integer = 0 To 7
            positions(0, i) = "b" & first_row(i)
            positions(1, i) = "bP"
            positions(7, i) = "w" & first_row(i)
            positions(6, i) = "wP"
        Next

        Session("positions") = positions 'Format will be letter and colour e.g. bN for black knight and wN white knight

        Session("player_turn") = "b" ' white
        Session("first_move") = True ' used by chess game.aspx to determine that the first button clicked is the first move of the game

        Response.Redirect("chess game.aspx")
    End Sub
End Class