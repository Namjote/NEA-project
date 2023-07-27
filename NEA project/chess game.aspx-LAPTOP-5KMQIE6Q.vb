Public Class chess_game
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub piece_clicked(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click, ImageButton2.Click, ImageButton3.Click, ImageButton4.Click, ImageButton5.Click, ImageButton6.Click, ImageButton7.Click, ImageButton8.Click, ImageButton9.Click, ImageButton10.Click, ImageButton11.Click, ImageButton12.Click, ImageButton13.Click, ImageButton14.Click, ImageButton15.Click, ImageButton16.Click, ImageButton17.Click, ImageButton18.Click, ImageButton19.Click, ImageButton20.Click, ImageButton21.Click, ImageButton22.Click, ImageButton23.Click, ImageButton24.Click, ImageButton25.Click, ImageButton26.Click, ImageButton27.Click, ImageButton28.Click, ImageButton29.Click, ImageButton30.Click, ImageButton31.Click, ImageButton32.Click, ImageButton33.Click, ImageButton34.Click, ImageButton35.Click, ImageButton36.Click, ImageButton37.Click, ImageButton38.Click, ImageButton39.Click, ImageButton40.Click, ImageButton41.Click, ImageButton42.Click, ImageButton43.Click, ImageButton44.Click, ImageButton45.Click, ImageButton46.Click, ImageButton47.Click, ImageButton48.Click, ImageButton49.Click, ImageButton50.Click, ImageButton51.Click, ImageButton52.Click, ImageButton53.Click, ImageButton54.Click, ImageButton55.Click, ImageButton56.Click, ImageButton57.Click, ImageButton58.Click, ImageButton59.Click, ImageButton60.Click, ImageButton61.Click, ImageButton62.Click, ImageButton63.Click, ImageButton64.Click

        Dim ImageButton_clicked As ImageButton = DirectCast(sender, ImageButton)
        Dim Location_clicked As Integer

        If sender.ID.length = 12 Then ' If the length of ID is 12, then the position is a one digit number, else its a two digit number. 
            Location_clicked = CInt(sender.ID.Substring(11, 1))
        Else
            Location_clicked = CInt(sender.ID.Substring(11, 2))
        End If
        Dim ImageButton_clicked_ImageURL As String = ImageButton_clicked.ImageUrl

        If Session.Item("first_move") = True Then

            Session("first_move") = False ' Moves after this will no longer be first move
            init_sender_is_piece_to_move() ' initialises the sender_is_piece_to_move to true only once, as after this it alternates from true to false to true....
            ' cookie to store piece to move
        End If

        If Session.Item("sender_is_piece_to_move") = True Then
            ' cookie to store piece to move
            Dim cookieObject4 As New HttpCookie("start_position")
            Dim cookieObject7 As New HttpCookie("start_URL")
            Session("start_button") = ImageButton_clicked
            Session("start_URL") = ImageButton_clicked_ImageURL
            Session("start_position") = Location_clicked ' an integer representing the button
            Session("sender_is_piece_to_move") = False
        Else
            ' cookie to store position to move
            Dim cookieObject5 As New HttpCookie("end_position")
            Dim cookieObject8 As New HttpCookie("end_URL")
            Session("end_button") = ImageButton_clicked
            Session("end_URL") = ImageButton_clicked_ImageURL
            Session("end_position") = Location_clicked ' an integer representing the button
            Session("sender_is_piece_to_move") = True

            If calc_legal_moves(Session.Item("start_button"), Session.Item("end_button"), CInt(Session.Item("start_position") - 1), CInt(Session.Item("end_position") - 1)) = True Then

                capture(ImageButton_clicked, CInt(Session.Item("start_position") - 1), CInt(Session.Item("end_position") - 1))

                If Session.Item("player_turn") = "b" Then ' swaps players turn once a valid capture or normal move is made
                    Session("player_turn") = "w"
                Else
                    Session("player_turn") = "b"
                End If

            End If
        End If

    End Sub

    ' This function updates the image URL's of buttons on the chess game when a move is made
    Sub capture(ByVal end_location As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer)
        Dim chess_board As String(,) = Session.Item("chess_board")
        Dim start_coordinate = New Integer() {start_position Mod 8, start_position \ 8} ' converts button number into co-ordinate form
        Dim end_coordinate = New Integer() {end_position Mod 8, end_position \ 8}
        Session("chess_board") = chess_board
        end_location.ImageUrl = Session.Item("start_URL")
        chess_board(start_coordinate(0), start_coordinate(1)) = ""
        chess_board(end_coordinate(0), end_coordinate(1)) = end_location.ImageUrl.Substring(15, 1) + end_location.ImageUrl.Substring(16, 1) ' concatenates the type of piece and its colour i.e. bR for black rook
        Dim start_button As ImageButton = FindControl(Session.Item("start_button").ID)
        start_button.ImageUrl = ""
    End Sub

    Function calc_legal_moves(ByVal start_piece As ImageButton, ByVal end_piece As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer)
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

        If start_piece_colour <> Session("player_turn") Then ' A player is trying to make a move when it is not their turn
            Return False
        End If

        Dim start_coordinate = New Integer() {start_position Mod 8, start_position \ 8} ' converts button number into co-ordinate form
        Dim end_coordinate = New Integer() {end_position Mod 8, end_position \ 8}
        Dim knight_translations = New Integer(,) {{1, 2}, {2, 1}, {-1, 2}, {-2, 1}, {2, -1}, {1, -2}, {-2, -1}, {-1, -2}} ' L-shaped translations
        Dim black_pawn_translations = New Integer(,) {{0, 1}} ' First inner array is for moves after first move. Second array is for first moves only
        Dim white_pawn_translations = New Integer(,) {{0, -1}} ' Two arrays for each piece as pawns cannot move backwards...
        Dim black_pawn_first_move_translations = New Integer(,) {{0, 1}, {0, 2}} ' First inner array is for moves after first move. Second array is for first moves only
        Dim white_pawn_first_move_translations = New Integer(,) {{0, -1}, {0, -2}} ' Two arrays for each piece as pawns cannot move backwards...
        Dim black_pawn_kill_translations = New Integer(,) {{-1, 1}, {1, 1}}
        Dim white_pawn_kill_translations = New Integer(,) {{-1, -1}, {1, -1}}
        Dim bishop_translations = New Integer(,) {{1, 1}, {-1, -1}, {2, 2}, {-2, -2}, {3, 3}, {-3, -3}, {4, 4}, {-4, -4}, {5, 5}, {-5, -5}, {6, 6}, {-6, -6}, {7, 7}, {-7, -7}, {8, 8}, {-8, -8}, {-1, 1}, {1, -1}, {-2, 2}, {2, -2}, {-3, 3}, {3, -3}, {-4, 4}, {4, -4}, {-5, 5}, {5, -5}, {-6, 6}, {6, -6}, {-7, 7}, {7, -7}, {-8, 8}, {8, -8}}
        Dim rook_translations = New Integer(,) {{0, 1}, {0, -1}, {0, 2}, {0, -2}, {0, 3}, {0, -3}, {0, 4}, {0, -4}, {0, 5}, {0, -5}, {0, 6}, {0, -6}, {0, 7}, {0, -7}, {0, 8}, {0, -8}, {1, 0}, {-1, 0}, {2, 0}, {-2, 0}, {3, 0}, {-3, 0}, {4, 0}, {-4, 0}, {5, 0}, {-5, 0}, {6, 0}, {-6, 0}, {7, 0}, {-7, 0}, {8, 0}, {-8, 0}}
        Dim king_translations = New Integer(,) {{0, -1}, {1, -1}, {1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}}
        Dim pawn_first_move_tracker As Integer(,) = Session.Item("pawn_first_move_tracker")

        If end_piece_colour = Session.Item("player_turn") Then ' Player is moving a piece onto a position that has one of their own pieces on it
            Return False
        ElseIf start_piece_type = vbNullChar Then ' Player is "attempting" to move an empty space
            Return False
        ElseIf start_piece_type = "P" Then
            If start_piece_colour = "b" Then ' Black pawns
                If end_piece_colour = vbNullChar Then ' Moving to empty square
                    If pawn_first_move_tracker(0, start_coordinate(0)) = 1 And start_coordinate(1) = 1 And Translate(black_pawn_first_move_translations, start_coordinate, end_coordinate) = True Then ' If its the pawns first move
                        pawn_first_move_tracker(0, start_coordinate(0)) = 0
                        Session("pawn_first_move_tracker") = pawn_first_move_tracker
                        Return True
                    Else
                        Return Translate(black_pawn_translations, start_coordinate, end_coordinate) ' Moves after first move
                    End If
                ElseIf end_piece_colour = "w" Then ' Attacking 
                    Return Translate(black_pawn_kill_translations, start_coordinate, end_coordinate)
                End If
            ElseIf start_piece_colour = "w" Then ' White pawns
                If end_piece_colour = vbNullChar Then ' Moving to empty square
                    If pawn_first_move_tracker(1, start_coordinate(0)) = 1 And start_coordinate(1) = 6 And Translate(white_pawn_first_move_translations, start_coordinate, end_coordinate) = True Then ' If its the pawns first move
                        pawn_first_move_tracker(1, start_coordinate(0)) = 0
                        Session("pawn_first_move_tracker") = pawn_first_move_tracker
                        Return True
                    Else
                        Return Translate(white_pawn_translations, start_coordinate, end_coordinate) ' Moves after first move
                    End If
                ElseIf end_piece_colour = "b" Then ' Attacking 
                    Return Translate(white_pawn_kill_translations, start_coordinate, end_coordinate)
                End If
            End If
            'ElseIf start_piece_type = "P" And start_piece_colour = "b" And end_piece_colour = vbNullChar And Translate(black_pawn_translations, start_coordinate, end_coordinate) = True Then ' Translate function validiates player move positions
            '    Return True
            'ElseIf start_piece_type = "P" And start_piece_colour = "w" And end_piece_colour = vbNullChar And Translate(white_pawn_translations, start_coordinate, end_coordinate) = True Then
            '    Return True
            'ElseIf start_piece_type = "P" And start_piece_colour = "w" And end_piece_colour = "b" And Translate(white_pawn_kill_translations, start_coordinate, end_coordinate) = True Then
            '    Return True
            'ElseIf start_piece_type = "P" And start_piece_type = "b" And end_piece_colour = "w" And Translate(black_pawn_kill_translations, start_coordinate, end_coordinate) = True Then
            '    Return True
        ElseIf start_piece_type = "N" And Translate(knight_translations, start_coordinate, end_coordinate, True) = True Then ' The 4th parameter "true" is optinal, telling translate this is a knight.
            Return True
        ElseIf start_piece_type = "B" And Translate(bishop_translations, start_coordinate, end_coordinate) = True Then
            Return True
        ElseIf start_piece_type = "R" And Translate(rook_translations, start_coordinate, end_coordinate) = True Then
            Return True
        ElseIf start_piece_type = "Q" And (Translate(bishop_translations, start_coordinate, end_coordinate) = True Or Translate(rook_translations, start_coordinate, end_coordinate) = True) Then ' The queen is the combination of bishop and rook valid moves
            Return True
        ElseIf start_piece_type = "K" And Translate(king_translations, start_coordinate, end_coordinate) = True Then
            Return True
        End If

        Return False

    End Function
    'optinal parameter IsKnight. We need this to tell the translate function to not check if there is a piece blocking it from moving as knights can jump over them.
    'Default value for this optinal parameter is False.
    Function Translate(ByVal piece_translations As Integer(,), ByVal start_coordinate As Integer(), ByVal end_coordinate As Integer(), Optional ByVal IsKnight As Boolean = False)
        Dim possible_move = New Integer() {0, 0}
        Dim possible_move_LCM = New Integer() {0, 0} ' possible move lowest common multiple
        Dim chess_board As String(,) = Session.Item("chess_board")
        Dim N As Integer = 0 ' N * possible_move_LCM = possible_move

        For index As Integer = 0 To piece_translations.GetLength(0) - 1
            possible_move(0) = start_coordinate(0) + piece_translations(index, 0) ' row
            possible_move(1) = start_coordinate(1) + piece_translations(index, 1) ' column
            possible_move_LCM = {0, 0}
            N = 0
            If piece_translations(index, 0) <> 0 Then
                possible_move_LCM(0) = piece_translations(index, 0) / Math.Abs(piece_translations(index, 0))
                N = Math.Abs(piece_translations(index, 0))
            End If
            If piece_translations(index, 1) <> 0 Then
                possible_move_LCM(1) = piece_translations(index, 1) / Math.Abs(piece_translations(index, 1))
                N = Math.Abs(piece_translations(index, 1))
            End If

            If end_coordinate(0) = possible_move(0) And end_coordinate(1) = possible_move(1) Then ' Valid translation identified
                'Calculate the translation lowest multiple i.e. if (-5, 5) is a valid move for a bishop, the multiple will be N(-1, 1) where N = 5
                If IsKnight <> True And N <> 1 Then ' Checks if there are pieces in the way of a non-knight piece moving if the piece is making a move which involves traversing more than one step
                    For multiple As Integer = 1 To N - 1
                        If chess_board(start_coordinate(0) + (multiple * possible_move_LCM(0)), start_coordinate(1) + (multiple * possible_move_LCM(1))) <> Nothing Then
                            Return False ' there is a piece in the way of the moving piece
                        End If
                    Next
                End If
                Return True
            End If
        Next
        Return False
    End Function
    Sub init_sender_is_piece_to_move()
        Dim cookieObject6 As New HttpCookie("sender_is_piece_to_move")
        Session("sender_is_piece_to_move") = True
    End Sub




End Class