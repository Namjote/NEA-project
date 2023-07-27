Public Class ai_settings
    Inherits System.Web.UI.Page
    Public connectAIgamesTable As New DataSet1TableAdapters.ai_gamesTableAdapter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub submit_btn_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        If IsNumeric(stockfish_elo_txt.Text) = False Then
            MsgBox("Please enter an ELO value between 1 and 3000") ' if its not a number
            Exit Sub
        Else
            Session("stockfish_elo") = CInt(stockfish_elo_txt.Text)
        End If

        If Session("stockfish_elo") > 3000 Or Session("stockfish_elo") < 1 Then
            MsgBox("Please enter an ELO value between 1 and 3000") ' if its not a number between 1 and 3000
        Else
            connectAIgamesTable.ChangeStockfishELO(Session.Item("stockfish_elo"), Session.Item("username"))
            Session("sender_is_piece_to_move") = "true"
            Response.Redirect("learn_from_an_ai_3.aspx")
        End If
    End Sub
End Class