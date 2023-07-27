Public Class Main_Menu
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btn_two_player_game_Click(sender As Object, e As EventArgs) Handles btn_two_player_game.Click
        load_sessions()
        Response.Redirect("two_player.aspx")
    End Sub

    Protected Sub btn_learn_from_an_AI_Click(sender As Object, e As EventArgs) Handles btn_learn_from_an_AI.Click
        load_sessions()
        Response.Redirect("ai_settings.aspx")
    End Sub

    Sub load_sessions()
        'initialising start of chess board
        ' 1 represents a pawn that has not yet been moved on the second row, 0 represents a pawn that has been moved
        Dim pawn_first_move_tracker = New Integer(,) {{1, 1, 1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1, 1, 1}}
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
        Session("player_turn") = "w" ' white will start first
        Session("first_move") = True ' used to determine that the first button clicked is the first move of the game
        Session("pawn_first_move_tracker") = pawn_first_move_tracker ' Keeps track of which pawns can move two squares fowards
        Session("b_can_castle") = True ' represents if a player is eligible to carry out castling
        Session("w_can_castle") = True
        Session("black_king_moved") = False ' player cannot castle if their king has moved
        Session("white_king_moved") = False
        Session("king_side_b") = False ' player cannot castle a king_side or queen_side rook if it has moved
        Session("queen_side_b") = False
        Session("king_side_w") = False
        Session("queen_side_w") = False
        Session("b_en_passant_status") = False ' player cannot do en passant until en_passant_check changes the value of this session
        Session("w_en_passant_status") = False
        Session("choosing_piece_to_promote") = False
        Session("sender_is_piece_to_move") = True ' used to distinguish if a button click is a piece to move or a square to move it to
        Session("target_pawn") = Nothing

        ' Needed when constructing FEN for stockfish
        Session("en_passant_target_square") = "-"
        Session("half_move_clock") = 0
        Session("full_move_number") = 1
        Session("half_move_number") = 0
        Session("ai_promotion_piece_type") = ""

        ' Needed by famous games
        Session("current_move") = 0
        ' The maximum theoretical number of moves in a single chess is under 6000 moves
        Dim move_history(6000, 63) As String ' each move has 64 image urls for each square on the board
        For i As Integer = 0 To 63
            If chess_board(i Mod 8, i \ 8) = Nothing Then
                move_history(0, i) = Nothing
            Else
                move_history(0, i) = "~/chess pieces/" & chess_board(i Mod 8, i \ 8) & ".png"
            End If
        Next
        Session("move_history") = move_history
        Session("num_of_moves") = 0

    End Sub

    Protected Sub btn_historical_games_Click(sender As Object, e As EventArgs) Handles btn_historical_games.Click
        load_sessions()
        Response.Redirect("famous_games.aspx")
    End Sub

    Protected Sub btn_leaderboards_Click(sender As Object, e As EventArgs) Handles btn_analysis.Click
        Response.Redirect("analysis_from_ai.aspx")
    End Sub
End Class