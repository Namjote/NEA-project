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
        Dim pawn_first_move_tracker = New Integer(,) {{1, 1, 1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1, 1, 1}} ' 1 represents a pawn that has not yet been moved on the second row, 0 represents a pawn that has been moved
        Dim chess_board(7, 7) As String
        Dim first_row = New String() {"R", "N", "B", "Q", "K", "B", "N", "R"}
        For i As Integer = 0 To 7
            chess_board(i, 0) = "b" & first_row(i)
            chess_board(i, 1) = "bP"
            chess_board(i, 7) = "w" & first_row(i)
            chess_board(i, 6) = "wP"
        Next

        ' state of chess board stored in "chess_board" session
        Session("chess_board") = chess_board 'Format will be letter and colour e.g. bN for black knight and wN white knight

        Session("player_turn") = "w" ' black
        Session("first_move") = True ' used by chess game.aspx to determine that the first button clicked is the first move of the game
        Session("pawn_first_move_tracker") = pawn_first_move_tracker
        Session("b_can_castle") = True ' Casteled sesssions represents if a player is eligible to carry out casteling
        Session("w_can_castle") = True
        Session("black_king_moved") = False
        Session("white_king_moved") = False
        Session("king_side_b") = False
        Session("queen_side_b") = False
        Session("king_side_w") = False
        Session("queen_side_w") = False
        Session("b_en_passant_status") = False
        Session("w_en_passant_status") = False
        Session("choosing_piece_to_promote") = False

        Response.Redirect("chess game.aspx")
    End Sub
End Class