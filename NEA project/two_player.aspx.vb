Public Class chess_game
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub piece_clicked(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click, ImageButton2.Click, ImageButton3.Click, ImageButton4.Click, ImageButton5.Click, ImageButton6.Click, ImageButton7.Click, ImageButton8.Click, ImageButton9.Click, ImageButton10.Click, ImageButton11.Click, ImageButton12.Click, ImageButton13.Click, ImageButton14.Click, ImageButton15.Click, ImageButton16.Click, ImageButton17.Click, ImageButton18.Click, ImageButton19.Click, ImageButton20.Click, ImageButton21.Click, ImageButton22.Click, ImageButton23.Click, ImageButton24.Click, ImageButton25.Click, ImageButton26.Click, ImageButton27.Click, ImageButton28.Click, ImageButton29.Click, ImageButton30.Click, ImageButton31.Click, ImageButton32.Click, ImageButton33.Click, ImageButton34.Click, ImageButton35.Click, ImageButton36.Click, ImageButton37.Click, ImageButton38.Click, ImageButton39.Click, ImageButton40.Click, ImageButton41.Click, ImageButton42.Click, ImageButton43.Click, ImageButton44.Click, ImageButton45.Click, ImageButton46.Click, ImageButton47.Click, ImageButton48.Click, ImageButton49.Click, ImageButton50.Click, ImageButton51.Click, ImageButton52.Click, ImageButton53.Click, ImageButton54.Click, ImageButton55.Click, ImageButton56.Click, ImageButton57.Click, ImageButton58.Click, ImageButton59.Click, ImageButton60.Click, ImageButton61.Click, ImageButton62.Click, ImageButton63.Click, ImageButton64.Click
        If checking_for_checkmate2() = True Then ' after the player has made a move, check if they have won the game
            MsgBox("CHECKMATE! - " & Session.Item("player_turn") & " has lost")
            Response.Redirect("main_menu.aspx")
        End If

        If Session.Item("choosing_piece_to_promote") = True Then
            Exit Sub ' This routine is only for non-promotion actions. Go to the promotion subtroutine below
        End If

        Dim ImageButton_clicked As ImageButton = DirectCast(sender, ImageButton)
        Dim Location_clicked As Integer
        Dim is_king_moving As Boolean = False

        If sender.ID.Length = 12 Then ' If the length of ID is 12, then the position is a one digit number, else its a two digit number. 
            Location_clicked = CInt(sender.ID.Substring(11, 1))
        Else
            Location_clicked = CInt(sender.ID.Substring(11, 2))
        End If

        Dim ImageButton_clicked_ImageURL As String = ImageButton_clicked.ImageUrl
        Dim old_background_colour As Drawing.Color = ImageButton_clicked.BackColor
        ImageButton_clicked.BackColor = Drawing.Color.Red

        If Session.Item("sender_is_piece_to_move") = True Then
            ' Sessions to store piece to move
            Session("start_button") = ImageButton_clicked ' The actual button object clicked
            Session("start_URL") = ImageButton_clicked_ImageURL ' The button's image url path
            Session("start_position") = Location_clicked ' The buttons position on the board (1-64)
            Session("sender_is_piece_to_move") = False ' The next button click will be the position to move to
            Dim start_coordinate = New Integer() {(Location_clicked - 1) Mod 8, (Location_clicked - 1) \ 8}
            Dim start_piece_type As String = ImageButton_clicked_ImageURL.Substring(16, 1)
            end_piece_highlight(ImageButton_clicked, Location_clicked)
        Else
            ImageButton_clicked.BackColor = old_background_colour
            reset_highlights()
            ' Sessions to store position to move to
            Session("end_button") = ImageButton_clicked
            Session("end_URL") = ImageButton_clicked_ImageURL
            Session("end_position") = Location_clicked
            Session("sender_is_piece_to_move") = True ' The next button click will be the piece to move



            If Session("start_button").ImageUrl <> "" Then
                If Session("start_button").ImageUrl.Substring(16, 1) = "K" Then
                    is_king_moving = True ' This is required by the check function 
                End If
            End If

            ' if a legal move is identified
            If calc_legal_moves(Session.Item("start_button"), Session.Item("end_button"), CInt(Session.Item("start_position") - 1), CInt(Session.Item("end_position") - 1)) = True Then
                ' and it doesn't cause check
                If check(CInt(Session.Item("start_position") - 1), CInt(Session.Item("end_position") - 1), Session.Item("start_button"), Session.Item("end_button"), is_king_moving) = False Then
                    ' legal move that doesn't cause check can be carried out
                    capture(ImageButton_clicked, CInt(Session.Item("start_position") - 1), CInt(Session.Item("end_position") - 1))
                    switch_player(True) ' swaps players turn once a valid move is made
                Else
                    MsgBox(Session.Item("player_turn") + "'s king is in check")
                End If
            End If
        End If
    End Sub

    ' This function updates the image URL's of buttons on the chess game when a move is made
    ' End position and start position are 0 indexed
    Sub capture(ByVal end_btn As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer, Optional ByVal castling As Boolean = False)
        Dim chess_board As String(,) = Session.Item("chess_board")
        Dim start_coordinate = New Integer() {start_position Mod 8, start_position \ 8} ' converts button number into co-ordinate form
        Dim end_coordinate = New Integer() {end_position Mod 8, end_position \ 8}
        Dim start_button As ImageButton = FindControl("ImageButton" & (start_position + 1)) ' Locates the button on the page when a position is given
        If castling = True Then
            ' This is needed because the start_URL in the session is the king's URL because that was the start button that initiated castling
            end_btn.ImageUrl = "~/chess pieces/" & Session.Item("player_turn") & "R.png"
        Else
            end_btn.ImageUrl = Session.Item("start_URL")
        End If
        promotion_check(start_button, end_position) ' Check if a player's pawn is at the enemy's starting rank
        start_button.ImageUrl = "" ' Piece has left its position so the start position is now an empty square
        chess_board(start_coordinate(0), start_coordinate(1)) = Nothing
        ' concatenates the type of piece and its colour i.e. bR for black rook
        chess_board(end_coordinate(0), end_coordinate(1)) = end_btn.ImageUrl.Substring(15, 1) + end_btn.ImageUrl.Substring(16, 1)
        Session("chess_board") = chess_board

        If Session.Item("target_pawn") IsNot Nothing Then
            Dim target_pawn As ImageButton
            target_pawn = Session.Item("target_pawn")
            target_pawn.ImageUrl = ""
            Session("target_pawn") = Nothing
        End If
    End Sub

    Function calc_legal_moves(ByVal start_piece As ImageButton, ByVal end_piece As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer, Optional ByVal checking_for_check As Boolean = False, Optional ByVal highlight As Boolean = False)
        Dim end_piece_colour As Char = ""
        Dim start_piece_type As Char = ""
        Dim start_piece_colour As Char = ""

        If end_piece.ImageUrl <> "" Then
            end_piece_colour = end_piece.ImageUrl.Substring(15, 1)
        End If

        If start_piece.ImageUrl <> "" Then
            start_piece_type = start_piece.ImageUrl.Substring(16, 1)
            start_piece_colour = start_piece.ImageUrl.Substring(15, 1)
        End If

        If start_piece_colour <> Session.Item("player_turn") Then ' A player is trying to make a move when it is not their turn
            Return False
        End If

        Dim start_coordinate = New Integer() {start_position Mod 8, start_position \ 8} ' converts button number into co-ordinate form. {column, row}
        Dim end_coordinate = New Integer() {end_position Mod 8, end_position \ 8}
        Dim knight_translations = New Integer(,) {{1, 2}, {2, 1}, {-1, 2}, {-2, 1}, {2, -1}, {1, -2}, {-2, -1}, {-1, -2}} ' L-shaped translations
        Dim black_pawn_translations = New Integer(,) {{0, 1}} ' Two arrays for each piece as pawns cannot move backwards...
        Dim white_pawn_translations = New Integer(,) {{0, -1}}
        Dim black_pawn_first_move_translations = New Integer(,) {{0, 1}, {0, 2}} ' First inner array is for moves after first move. Second array is for first moves only
        Dim white_pawn_first_move_translations = New Integer(,) {{0, -1}, {0, -2}} ' Two arrays for each piece as pawns cannot move backwards...
        Dim black_pawn_kill_translations = New Integer(,) {{-1, 1}, {1, 1}}
        Dim white_pawn_kill_translations = New Integer(,) {{-1, -1}, {1, -1}}
        Dim bishop_translations = New Integer(,) {{1, 1}, {-1, -1}, {2, 2}, {-2, -2}, {3, 3}, {-3, -3}, {4, 4}, {-4, -4}, {5, 5}, {-5, -5}, {6, 6}, {-6, -6}, {7, 7}, {-7, -7}, {8, 8}, {-8, -8}, {-1, 1}, {1, -1}, {-2, 2}, {2, -2}, {-3, 3}, {3, -3}, {-4, 4}, {4, -4}, {-5, 5}, {5, -5}, {-6, 6}, {6, -6}, {-7, 7}, {7, -7}, {-8, 8}, {8, -8}}
        Dim rook_translations = New Integer(,) {{0, 1}, {0, -1}, {0, 2}, {0, -2}, {0, 3}, {0, -3}, {0, 4}, {0, -4}, {0, 5}, {0, -5}, {0, 6}, {0, -6}, {0, 7}, {0, -7}, {0, 8}, {0, -8}, {1, 0}, {-1, 0}, {2, 0}, {-2, 0}, {3, 0}, {-3, 0}, {4, 0}, {-4, 0}, {5, 0}, {-5, 0}, {6, 0}, {-6, 0}, {7, 0}, {-7, 0}, {8, 0}, {-8, 0}}
        Dim king_translations = New Integer(,) {{0, -1}, {1, -1}, {1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}}
        Dim king_castling_translations = New Integer(,) {{2, 0}, {-2, 0}} ' Translations only for king, not the rooks. Rooks are moved by the castling function
        Dim black_en_passant_translations = New Integer(,) {{0, 1}, {-1, 1}, {1, 1}}
        Dim white_en_passant_translations = New Integer(,) {{0, -1}, {-1, -1}, {1, -1}}
        Dim pawn_first_move_tracker As Integer(,) = Session.Item("pawn_first_move_tracker")

        If end_piece_colour = Session.Item("player_turn") And checking_for_check = False Then ' Player is moving a piece onto a position that has one of their own pieces on it
            Return False
        ElseIf start_piece_type = vbNullChar Then ' Player is "attempting" to move an empty space
            Return False
        ElseIf start_piece_type = "P" Then
            If start_piece_colour = "b" Then  ' Black pawns
                If end_piece_colour = vbNullChar Then ' Moving to empty square
                    If Session.Item("b_en_passant_status") = True And checking_for_check = False Then

                        Return en_passant(black_en_passant_translations, start_coordinate, end_coordinate, start_position, highlight)
                    ElseIf pawn_first_move_tracker(0, start_coordinate(0)) = 1 And start_coordinate(1) = 1 And Translate(black_pawn_first_move_translations, start_coordinate, end_coordinate) = True And checking_for_check = False Then

                        ' If its the pawns first move
                        If highlight = False And checking_for_check = False Then
                            en_passant_check(start_coordinate) ' will allow enemy to do en passant if true
                            pawn_first_move_tracker(0, start_coordinate(0)) = 0
                            Session("pawn_first_move_tracker") = pawn_first_move_tracker
                        End If
                        Return True
                    Else
                        Return Translate(black_pawn_translations, start_coordinate, end_coordinate) ' Moves after first move
                    End If
                ElseIf end_piece_colour = "w" Or checking_for_check = True Then ' Attacking 
                    Return Translate(black_pawn_kill_translations, start_coordinate, end_coordinate)
                End If

            ElseIf start_piece_colour = "w" Then ' White pawns
                If end_piece_colour = vbNullChar Then ' Moving to empty square
                    If Session.Item("w_en_passant_status") = True And checking_for_check = False Then
                        Return en_passant(white_en_passant_translations, start_coordinate, end_coordinate, start_position, highlight)
                    ElseIf pawn_first_move_tracker(1, start_coordinate(0)) = 1 And start_coordinate(1) = 6 And Translate(white_pawn_first_move_translations, start_coordinate, end_coordinate) = True And checking_for_check = False Then
                        ' If its the pawns first move
                        If highlight = False And checking_for_check = False Then
                            en_passant_check(start_coordinate) ' will allow enemy to do en passant if true
                            pawn_first_move_tracker(1, start_coordinate(0)) = 0
                            Session("pawn_first_move_tracker") = pawn_first_move_tracker
                        End If
                        Return True
                    Else
                        Return Translate(white_pawn_translations, start_coordinate, end_coordinate) ' Moves after first move
                    End If
                ElseIf end_piece_colour = "b" Or checking_for_check = True Then ' Attacking 
                    Return Translate(white_pawn_kill_translations, start_coordinate, end_coordinate)
                End If
            End If
        ElseIf start_piece_type = "N" And Translate(knight_translations, start_coordinate, end_coordinate, True) = True Then ' The 4th parameter "true" is optinal, telling translate this is a knight.
            Return True
        ElseIf start_piece_type = "B" And Translate(bishop_translations, start_coordinate, end_coordinate) = True Then
            Return True
        ElseIf start_piece_type = "R" And Translate(rook_translations, start_coordinate, end_coordinate) = True Then
            If Session("player_turn") = "b" And (start_coordinate(0) = 0 And start_coordinate(1) = 0) Then
                Session("queen_side_b") = True
            ElseIf Session("player_turn") = "w" And (start_coordinate(0) = 0 And start_coordinate(1) = 7) Then
                Session("king_side_b") = True
            ElseIf Session("player_turn") = "w" And (start_coordinate(0) = 7 And start_coordinate(1) = 0) Then
                Session("queen_side_w") = True
            ElseIf Session("player_turn") = "w" And (start_coordinate(0) = 7 And start_coordinate(1) = 7) Then
                Session("king_side_w") = True
            End If
            Return True
        ElseIf start_piece_type = "Q" And (Translate(bishop_translations, start_coordinate, end_coordinate) = True Or Translate(rook_translations, start_coordinate, end_coordinate) = True) Then
            ' The queen is the combination of bishop and rook valid moves
            Return True
        ElseIf start_piece_type = "K" Then
            If checking_for_check = False And highlight = False Then
                If castling(king_castling_translations, start_coordinate, end_coordinate) = True Then
                    If Session.Item("player_turn") = "b" Then
                        Session("b_can_castle") = False
                    Else
                        Session("w_can_castle") = False
                    End If
                    Return True
                Else
                    Return Translate(king_translations, start_coordinate, end_coordinate)
                End If
            End If
        End If
        Return False
    End Function
    Sub switch_player(ByVal checking As Boolean)
        If checking = True Then
            If Session.Item("player_turn") = "b" Then
                Session.Item("player_turn") = "w"
            Else
                Session.Item("player_turn") = "b"
            End If
        End If
    End Sub
    'optional parameter IsKnight. We need this to tell the translate function to not check if there is a piece blocking it from moving as knights can jump over them.
    'Default value for this optinal parameter is False.
    Function Translate(ByVal piece_translations As Integer(,), ByVal start_coordinate As Integer(), ByVal end_coordinate As Integer(), Optional ByVal IsKnight As Boolean = False)
        Dim possible_move = New Integer() {0, 0}
        Dim possible_move_unit_vector = New Integer() {0, 0}
        Dim chess_board As String(,) = Session.Item("chess_board")
        Dim N As Integer = 0 ' N * possible_move_unit_vector = possible_move

        Dim highlighted_positions_array(63) As Integer
        For index As Integer = 0 To piece_translations.GetLength(0) - 1
            possible_move(0) = start_coordinate(0) + piece_translations(index, 0) ' row
            possible_move(1) = start_coordinate(1) + piece_translations(index, 1) ' column
            possible_move_unit_vector = {0, 0}
            N = 0
            If index = 16 Then
                Console.WriteLine("test")
            End If
            'Calculate the translation unit vector i.e. if (-5, 5) is a valid move for a bishop, the multiple will be N(-1, 1) where N = 5
            If piece_translations(index, 0) <> 0 Then
                possible_move_unit_vector(0) = piece_translations(index, 0) / Math.Abs(piece_translations(index, 0))
                N = Math.Abs(piece_translations(index, 0))
            End If
            If piece_translations(index, 1) <> 0 Then
                possible_move_unit_vector(1) = piece_translations(index, 1) / Math.Abs(piece_translations(index, 1))
                N = Math.Abs(piece_translations(index, 1))
            End If

            If end_coordinate(0) = possible_move(0) And end_coordinate(1) = possible_move(1) Then ' Valid translation identified
                If IsKnight <> True And N <> 1 Then ' Checks if there are pieces in the way of a non-knight piece moving if its moving more than one square
                    For multiple As Integer = 1 To N - 1
                        If chess_board(start_coordinate(0) + (multiple * possible_move_unit_vector(0)), start_coordinate(1) + (multiple * possible_move_unit_vector(1))) <> Nothing Then
                            Return False ' there is a piece in the way of the moving piece
                        End If
                    Next
                End If
                Return True ' the path for the move is clear so its a valid move
            End If
        Next
        Return False
    End Function

    Sub end_piece_highlight(ByVal start_piece As ImageButton, ByVal start_position As Integer)
        Dim end_piece As ImageButton
        For end_position As Integer = 0 To 63
            If end_position = 21 Or end_position = 22 Then
                Console.WriteLine("Hi")
            End If
            Dim end_coordinate = New Integer() {end_position Mod 8, end_position \ 8}
            end_piece = FindControl("ImageButton" & (end_position + 1))

            If calc_legal_moves(start_piece, end_piece, start_position - 1, end_position, False, True) = True Then
                end_piece.BackColor = Drawing.Color.Red
            End If
        Next
    End Sub

    Sub reset_highlights()
        Dim light_square As Drawing.Color = Drawing.Color.FromArgb(227, 193, 111)
        Dim dark_square As Drawing.Color = Drawing.Color.FromArgb(184, 139, 74)
        Dim btn As ImageButton
        Dim btn_position As Integer

        For row As Integer = 1 To 8
            For col As Integer = 1 To 8
                If row Mod 2 <> 0 Then
                    btn_position = ((row - 1) * 8) + col
                    btn = FindControl("ImageButton" & btn_position)
                    If col Mod 2 <> 0 Then
                        btn.BackColor = light_square
                    Else
                        btn.BackColor = dark_square
                    End If
                Else
                    btn_position = ((row - 1) * 8) + col
                    btn = FindControl("ImageButton" & btn_position)
                    If col Mod 2 <> 0 Then
                        btn.BackColor = dark_square
                    Else
                        btn.BackColor = light_square
                    End If
                End If
            Next
        Next

    End Sub

    Function castling(ByVal castling_translations As Integer(,), ByVal start_coordinate As Integer(), ByVal end_coordinate As Integer())
        'Identify which rook is being used for castling

        Dim rook As String
        Dim rook_position As Integer
        Dim empty_space_check = New Integer(,) {{3, 0}, {-4, 0}}
        Dim empty_space_btn As ImageButton
        Dim king_position As Integer = (start_coordinate(1) * 8) + start_coordinate(0) ' zero indexed

        ' Identify which rook is being castled
        If end_coordinate(0) = 2 And end_coordinate(1) = 7 Then
            rook = "queen_side"
            rook_position = 57
        ElseIf end_coordinate(0) = 6 And end_coordinate(1) = 7 Then
            rook = "king_side"
            rook_position = 64
        ElseIf end_coordinate(0) = 2 And end_coordinate(1) = 0 Then
            rook = "queen_side"
            rook_position = 1
        ElseIf end_coordinate(0) = 6 And end_coordinate(1) = 0 Then
            rook = "king_side"
            rook_position = 8
        Else
            Return False ' Player did not castle
        End If
        Dim rook_btn As ImageButton = FindControl("ImageButton" & rook_position)

        'Checking if there is empty spaces between castle and king
        If rook = "king_side" Then
            For i As Integer = 1 To 2  ' Checking if kingside is empty
                empty_space_btn = FindControl("ImageButton" & (king_position + 1 + i))
                If empty_space_btn.ImageUrl <> "" Then
                    Return False
                End If
            Next
        Else
            For i As Integer = 1 To 3 ' Checking if queenside is empty
                empty_space_btn = FindControl("ImageButton" & (king_position + 1 - i))
                If empty_space_btn.ImageUrl <> "" Then
                    Return False
                End If
            Next
        End If

        ' Rules for castling:
        '     1) Your king can not have moved
        If Session.Item(Session.Item("player_turn") & "_can_castle") = False Then
            Return False
        ElseIf Session.Item(rook & "_" & Session.Item("player_turn")) = True Then  ' 2) Your rook can not have moved
            Return False
        Else ' 3) Your king cannot currently be in check/Your king cannot move to or through a square that is under attack
            If rook = "king_side" Then
                For i As Integer = 0 To 2
                    ' checks two squares to the left or right of the king to see if they are under attack
                    If check(king_position, king_position + i, FindControl("ImageButton" & king_position), FindControl("ImageButton" & king_position + i), True) = True Then
                        Return False
                    End If
                Next
            Else
                For i As Integer = 0 To 2
                    If i = -2 Then
                        Console.WriteLine("")
                    End If
                    ' checks two squares to the left or right of the king to see if they are under attack
                    If check(king_position, king_position - i, FindControl("ImageButton" & king_position), FindControl("ImageButton" & king_position - i), True) = True Then
                        Return False
                    End If
                Next
            End If
        End If

        ' Moving king and rook
        If rook = "king_side" Then
            ' Last arguement is optional and tells capture function that we are castling
            capture(FindControl("ImageButton" & (rook_position - 2)), rook_position - 1, rook_position - 3, True)
        Else
            capture(FindControl("ImageButton" & (rook_position + 3)), rook_position - 1, rook_position + 2, True)
        End If
        rook_btn.ImageUrl = ""
        Return True
    End Function

    Sub en_passant_check(ByVal moving_pwn_strt_coord As Integer())
        Dim en_passant_translations = New Integer(,) {}
        Dim attack_colour As Char
        If Session.Item("player_turn") = "w" Then
            en_passant_translations = New Integer(,) {{1, -2}, {-1, -2}}
        Else
            en_passant_translations = New Integer(,) {{-1, 2}, {1, 2}}
        End If
        Dim chess_board As String(,) = Session("chess_board")

        If Session.Item("player_turn") = "b" Then
            attack_colour = "w"
        Else
            attack_colour = "b"
        End If

        For i As Integer = 0 To 1
            ' Make sure that the translations remain within the bounds of the board
            If moving_pwn_strt_coord(0) + en_passant_translations(i, 0) <= 7 And moving_pwn_strt_coord(0) + en_passant_translations(i, 0) >= 0 And moving_pwn_strt_coord(1) + en_passant_translations(i, 1) <= 7 And moving_pwn_strt_coord(1) + en_passant_translations(i, 1) >= 0 Then
                If chess_board(moving_pwn_strt_coord(0) + en_passant_translations(i, 0), moving_pwn_strt_coord(1) + en_passant_translations(i, 1)) = (attack_colour & "P") Then
                    ' Player's pawn has moved two spaces foward and has gone past the line of attack of an enemy pawn
                    Session.Item(attack_colour & "_en_passant_status") = True
                End If
            End If
        Next
    End Sub

    Function en_passant(ByVal en_passant_translations As Integer(,), ByVal start_coordinate As Integer(), ByVal end_coordinate As Integer(), ByVal start_position As Integer, Optional ByVal highlight As Boolean = False)
        Dim possible_en_passant_translation = New Integer() {0, 0}
        Dim pawn_to_capture_coordinate = New Integer() {}
        Dim pawn_to_capture_if_en_passant As ImageButton
        Dim chess_board As String(,) = Session.Item("chess_board")
        possible_en_passant_translation(0) = end_coordinate(0) - start_coordinate(0)
        possible_en_passant_translation(1) = end_coordinate(1) - start_coordinate(1)

        ' For moves doing en passant
        For i As Integer = 0 To en_passant_translations.GetLength(0) - 1
            If en_passant_translations(i, 0) = possible_en_passant_translation(0) And en_passant_translations(i, 1) = possible_en_passant_translation(1) Then ' Player is doing en passant
                ' if the pawn that is attacking (using en passant) is moving right one column, the pawn on the right of it originally will be captured, else pawn on left will be captured
                If possible_en_passant_translation(0) = 1 Then
                    pawn_to_capture_coordinate = New Integer() {(start_position + 1) Mod 8, (start_position + 1) \ 8}
                    pawn_to_capture_if_en_passant = FindControl("ImageButton" & (start_position + 2)) ' pawn on the right of the attacking pawn
                Else
                    pawn_to_capture_coordinate = New Integer() {(start_position - 1) Mod 8, (start_position - 1) \ 8}
                    pawn_to_capture_if_en_passant = FindControl("ImageButton" & (start_position)) ' pawn on the left of the attacking pawn
                End If

                If Translate(en_passant_translations, start_coordinate, end_coordinate) = True Then
                    If highlight = False Then
                        ' If the start and end buttons clicked by the player map to the correct en passant translations
                        'pawn_to_capture_if_en_passant.ImageUrl = ""
                        'chess_board(pawn_to_capture_coordinate(0), pawn_to_capture_coordinate(1)) = Nothing
                        Session("target_pawn") = pawn_to_capture_if_en_passant
                        Session("target_pawn_coordinate") = pawn_to_capture_coordinate

                        'Session("chess_board") = chess_board
                        Session(Session.Item("player_turn") & "_en_passant_status") = False
                        Return True
                    Else
                        Return True
                    End If
                End If
            End If
        Next
        ' for moves NOT doing en passant even if en passant was available
        If Translate(en_passant_translations, start_coordinate, end_coordinate) = True Then ' en passant translations contains normal move translations as well
            If highlight = False Then
                Session(Session.Item("player_turn") & "en_passant_status") = False ' player can only do en passant as soon as it becomes available, and not after
                Return True
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

    ' end_position is zero indexed
    Function check(ByVal start_position As Integer, ByVal end_position As Integer, ByVal start_btn As ImageButton, ByVal end_btn As ImageButton, ByVal is_king_moving As Boolean)

        Dim king_coordinate(1) As Integer
        Dim chess_board As String(,) = Session("chess_board")
        Dim king_position As Integer
        Dim king_button As ImageButton
        Dim temp_start_piece As ImageButton
        Dim original_end_btn_url As String

        ' Locating the king on the board 
        If is_king_moving = True Then
            king_coordinate(0) = end_position Mod 8
            king_coordinate(1) = end_position \ 8
        Else
            For column As Integer = 0 To 7
                For row As Integer = 0 To 7
                    If chess_board(column, row) = Session("player_turn") & "K" Then
                        king_coordinate(0) = column
                        king_coordinate(1) = row
                    End If
                Next
            Next

        End If
        king_position = (king_coordinate(1) * 8) + king_coordinate(0) + 1
        king_button = FindControl("ImageButton" & king_position)

        'temporarily carry out a players move to test if it causes check
        If start_position <> end_position Then
            chess_board(end_position Mod 8, end_position \ 8) = start_btn.ImageUrl.Substring(15, 2)
            chess_board(start_position Mod 8, start_position \ 8) = Nothing
            original_end_btn_url = end_btn.ImageUrl
            end_btn.ImageUrl = start_btn.ImageUrl
            start_btn.ImageUrl = ""
            If Session.Item("target_pawn") IsNot Nothing Then
                Dim target_pawn As ImageButton = Session.Item("target_pawn")
                target_pawn.ImageUrl = Nothing
            End If
        End If

            'Attempt to move every piece on the board to kill the king to see if player is in check
            switch_player(True) ' If it is P1's go, and we are testing if P1 is in check, we need to switch to P2's turn to simulate P2 attacking P1's king
        For temp_start_position As Integer = 1 To 64
            temp_start_piece = FindControl("ImageButton" & temp_start_position) ' Loop through every piece on the board

            If calc_legal_moves(temp_start_piece, king_button, temp_start_position - 1, king_position - 1, True) = True Then
                ' if a legal move from the enemy piece can attack our king
                switch_player(True)
                'Reverse the temporary move as we have finished checking if the move causes check
                reverse_temporary_move(start_position, start_btn, end_position, end_btn, chess_board, original_end_btn_url)
                Return True
            End If
        Next
        switch_player(True)
        'Reverse the temporary move as we have finished checking if the move causes check
        reverse_temporary_move(start_position, start_btn, end_position, end_btn, chess_board, original_end_btn_url)
        Return False
    End Function
    Sub reverse_temporary_move(ByVal start_position As Integer, ByVal start_btn As ImageButton, ByVal end_position As Integer, ByVal end_btn As ImageButton, ByVal chess_board As String(,), ByVal original_end_btn_url As String)
        If start_position <> end_position Then
            ' reverse the changes made to the chess_board array
            chess_board(start_position Mod 8, start_position \ 8) = end_btn.ImageUrl.Substring(15, 2)
            If original_end_btn_url = "" Then
                chess_board(end_position Mod 8, end_position \ 8) = Nothing
            Else
                chess_board(end_position Mod 8, end_position \ 8) = original_end_btn_url.Substring(15, 2)
            End If
            Session("chess_board") = chess_board
            ' reverse the changes made to the image URL's
            start_btn.ImageUrl = end_btn.ImageUrl
            end_btn.ImageUrl = original_end_btn_url

            ' Specific to en passant
            If Session.Item("target_pawn") IsNot Nothing Then
                Dim target_pawn As ImageButton = Session.Item("target_pawn")
                switch_player(True)
                target_pawn.ImageUrl = "~/chess pieces/" & Session.Item("player_turn") & "P.png"
                switch_player(True)
            End If
        End If
    End Sub
    Sub promotion_check(ByVal start_piece As ImageButton, ByVal end_position As Integer)
        Dim end_coordinate_row As Integer = end_position \ 8
        If Session("start_button").ImageUrl.Substring(16, 1) = "P" And (end_coordinate_row = 0 Or end_coordinate_row = 7) Then
            MsgBox("Select a piece in the pink box to promote a pawn")
            ' Pop up window for player to select a bishop, rook, knight or queen
            Session("choosing_piece_to_promote") = True
        End If
    End Sub

    Sub promotion(sender As Object, e As ImageClickEventArgs) Handles Q_promote_btn.Click, N_promote_btn.Click, R_promote_btn.Click, B_promote_btn.Click
        If Session.Item("choosing_piece_to_promote") = False Then
            Exit Sub ' This routine is only for promotion actions
        End If
        switch_player(True)
        Session("choosing_piece_to_promote") = False ' We are going to promote now so we can set it back to false
        Dim promote_btn As ImageButton = DirectCast(sender, ImageButton)
        Dim pawn_to_promote As ImageButton = Session("end_button")
        Dim pawn_position As Integer
        Dim promotion_url As String
        If pawn_to_promote.ID.Length = 12 Then ' If the length of ID is 12, then the position is a one digit number, else its a two digit number. 
            pawn_position = CInt(pawn_to_promote.ID.Substring(11, 1))
        Else
            pawn_position = CInt(pawn_to_promote.ID.Substring(11, 2))
        End If
        pawn_to_promote = FindControl("ImageButton" & pawn_position) ' the players pawn at the enemy's starting rank
        Dim pawn_coordinate = New Integer() {(pawn_position - 1) Mod 8, (pawn_position - 1) \ 8}
        If Session.Item("player_turn") = "b" Then
            promotion_url = promote_btn.ImageUrl.Replace("w", "b") ' The promotion menu pieces are in white so swap to black if its black's turn
        Else
            promotion_url = promote_btn.ImageUrl
        End If
        pawn_to_promote.ImageUrl = promotion_url ' set the pawn to promote URL to the promotion button URL
        Dim chess_board As String(,) = Session.Item("chess_board")
        ' Update the chess_board array
        chess_board(pawn_coordinate(0), pawn_coordinate(1)) = pawn_to_promote.ImageUrl.Substring(15, 1) + pawn_to_promote.ImageUrl.Substring(16, 1)
        Session("chess_board") = chess_board
        switch_player(True)
    End Sub

    ' Problem with this function is that it only checked if the king could move to an empty square to escape check, capture a piece to
    ' escape check, but it did not consider moving a friendly piece to block the attacking piece causing check
    Function checking_for_checkmate()
        Dim check_mate_translations = New Integer(,) {{0, -1}, {1, -1}, {1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}}
        Dim chess_board As String(,) = Session.Item("chess_board")
        Dim king_coordinate(1) As Integer
        Dim king_position As Integer
        Dim end_btn_position As Integer
        Dim end_btn As ImageButton
        Dim king_btn As ImageButton
        Dim friendly_neighbour_counter As Integer
        Dim legal_and_illegal_possible_moves As Integer

        For column As Integer = 0 To 7
            For row As Integer = 0 To 7
                If chess_board(column, row) = Session("player_turn") & "K" Then ' Locate the position of the players king
                    king_coordinate(0) = column
                    king_coordinate(1) = row
                    king_position = (king_coordinate(1) * 8) + king_coordinate(0) + 1
                    king_btn = FindControl("ImageButton" & king_position)
                    For i As Integer = 0 To check_mate_translations.GetLength(0) - 1
                        end_btn_position = ((king_coordinate(1) + check_mate_translations(i, 1)) * 8) + king_coordinate(0) + check_mate_translations(i, 0) + 1
                        If end_btn_position < 1 Or end_btn_position > 64 Then
                            Continue For ' if the square is outside the bounds of the board, skip to next iteration
                        End If
                        legal_and_illegal_possible_moves += 1
                        end_btn = FindControl("ImageButton" & end_btn_position) ' A potential square the king can move to, to escape check
                        If end_btn.ImageUrl <> "" Then
                            If end_btn.ImageUrl.Substring(15, 1) = Session.Item("player_turn") Then
                                friendly_neighbour_counter += 1
                            End If
                        End If

                        If calc_legal_moves(king_btn, end_btn, king_position - 1, end_btn_position - 1) = True Then ' If a legal move for the king is found
                            If check(king_position - 1, end_btn_position - 1, king_btn, end_btn, True) = False Then ' and it avoids check
                                Return False
                            End If
                        End If
                    Next
                End If
            Next
        Next

        ' This stops the program thinking that a king is in checkmate if it is surrounded completely by friendly pieces
        If legal_and_illegal_possible_moves = friendly_neighbour_counter Then
            Return False
        End If
        Return True
    End Function
    'Function calc_legal_moves(ByVal start_piece As ImageButton, ByVal end_piece As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer, Optional ByVal checking_for_check As Boolean = False)
    ' Function check(ByVal start_position As Integer, ByVal end_position As Integer, ByVal start_btn As ImageButton, ByVal end_btn As ImageButton, ByVal is_king_moving As Boolean) ' end_position is zero indexed

    'This function allows the king to move to escape check and allows other pieces to block check.
    Function checking_for_checkmate2()
        Dim start_btn As ImageButton
        Dim end_btn As ImageButton
        Dim is_king_moving As Boolean
        ' This nested for loop attempts to move every piece on the board to every other square
        For start_position As Integer = 1 To 64
            start_btn = FindControl("ImageButton" & start_position)
            is_king_moving = False
            If start_btn.ImageUrl <> "" Then
                If start_btn.ImageUrl.Substring(16, 1) = "K" Then
                    is_king_moving = True ' If the start piece is a king, then the king is moving (used by check function)
                End If
            End If
            For end_position As Integer = 1 To 64
                If start_position = end_position Then
                    Continue For ' We don't want to check if a piece staying in its current position stops checkmate
                End If
                end_btn = FindControl("ImageButton" & end_position)
                If calc_legal_moves(start_btn, end_btn, start_position - 1, end_position - 1, True) = True Then ' if moving the start piece to the end position is legal
                    If check(start_position - 1, end_position - 1, start_btn, end_btn, is_king_moving) = False Then ' and it avoids check
                        Return False ' Then the king is not in checkmate
                    End If
                End If
            Next
        Next
        Return True
    End Function

    Function SelectTranslation(ByVal start_piece_type As String, Optional ByVal player_turn As String = "", Optional ByVal attacking As Boolean = False) As Integer(,)
        Dim knight_translations = New Integer(,) {{1, 2}, {2, 1}, {-1, 2}, {-2, 1}, {2, -1}, {1, -2}, {-2, -1}, {-1, -2}}
        Dim bishop_translations = New Integer(,) {{1, 1}, {-1, -1}, {2, 2}, {-2, -2}, {3, 3}, {-3, -3}, {4, 4}, {-4, -4}, {5, 5}, {-5, -5}, {6, 6}, {-6, -6}, {7, 7}, {-7, -7}, {8, 8}, {-8, -8}, {-1, 1}, {1, -1}, {-2, 2}, {2, -2}, {-3, 3}, {3, -3}, {-4, 4}, {4, -4}, {-5, 5}, {5, -5}, {-6, 6}, {6, -6}, {-7, 7}, {7, -7}, {-8, 8}, {8, -8}}
        Dim rook_translations = New Integer(,) {{0, 1}, {0, -1}, {0, 2}, {0, -2}, {0, 3}, {0, -3}, {0, 4}, {0, -4}, {0, 5}, {0, -5}, {0, 6}, {0, -6}, {0, 7}, {0, -7}, {0, 8}, {0, -8}, {1, 0}, {-1, 0}, {2, 0}, {-2, 0}, {3, 0}, {-3, 0}, {4, 0}, {-4, 0}, {5, 0}, {-5, 0}, {6, 0}, {-6, 0}, {7, 0}, {-7, 0}, {8, 0}, {-8, 0}}
        Dim king_translations = New Integer(,) {{0, -1}, {1, -1}, {1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}}
        Dim queen_translations = New Integer(,) {{0, 1}, {0, -1}, {0, 2}, {0, -2}, {0, 3}, {0, -3}, {0, 4}, {0, -4}, {0, 5}, {0, -5}, {0, 6}, {0, -6}, {0, 7}, {0, -7}, {0, 8}, {0, -8}, {1, 0}, {-1, 0}, {2, 0}, {-2, 0}, {3, 0}, {-3, 0}, {4, 0}, {-4, 0}, {5, 0}, {-5, 0}, {6, 0}, {-6, 0}, {7, 0}, {-7, 0}, {8, 0}, {-8, 0}, {1, 1}, {-1, -1}, {2, 2}, {-2, -2}, {3, 3}, {-3, -3}, {4, 4}, {-4, -4}, {5, 5}, {-5, -5}, {6, 6}, {-6, -6}, {7, 7}, {-7, -7}, {8, 8}, {-8, -8}, {-1, 1}, {1, -1}, {-2, 2}, {2, -2}, {-3, 3}, {3, -3}, {-4, 4}, {4, -4}, {-5, 5}, {5, -5}, {-6, 6}, {6, -6}, {-7, 7}, {7, -7}, {-8, 8}, {8, -8}}
        Dim black_pawn_first_move_translations = New Integer(,) {{0, 1}, {0, 2}}
        Dim white_pawn_first_move_translations = New Integer(,) {{0, -1}, {0, -2}}

        If start_piece_type = "N" Then
            Return knight_translations
        ElseIf start_piece_type = "B" Then
            Return bishop_translations
        ElseIf start_piece_type = "R" Then
            Return rook_translations
        ElseIf start_piece_type = "K" Then
            Return king_translations
        ElseIf start_piece_type = "Q" Then
            Return queen_translations
        ElseIf start_piece_type = "P" Then
            If player_turn = "w" Then
                Return white_pawn_first_move_translations
            ElseIf player_turn = "b" Then
                Return black_pawn_first_move_translations
            End If
        End If
    End Function





End Class