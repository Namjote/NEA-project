

Public Class learn_from_an_ai_2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Sub piece_clicked_ai(Optional ByVal sender As Object = Nothing, Optional ByVal e As ImageClickEventArgs = Nothing, Optional ByVal ai_btn_clicked As ImageButton = Nothing, Optional ByVal is_move_from_ai As Boolean = False) Handles ImageButton1.Click, ImageButton2.Click, ImageButton3.Click, ImageButton4.Click, ImageButton5.Click, ImageButton6.Click, ImageButton7.Click, ImageButton8.Click, ImageButton9.Click, ImageButton10.Click, ImageButton11.Click, ImageButton12.Click, ImageButton13.Click, ImageButton14.Click, ImageButton15.Click, ImageButton16.Click, ImageButton17.Click, ImageButton18.Click, ImageButton19.Click, ImageButton20.Click, ImageButton21.Click, ImageButton22.Click, ImageButton23.Click, ImageButton24.Click, ImageButton25.Click, ImageButton26.Click, ImageButton27.Click, ImageButton28.Click, ImageButton29.Click, ImageButton30.Click, ImageButton31.Click, ImageButton32.Click, ImageButton33.Click, ImageButton34.Click, ImageButton35.Click, ImageButton36.Click, ImageButton37.Click, ImageButton38.Click, ImageButton39.Click, ImageButton40.Click, ImageButton41.Click, ImageButton42.Click, ImageButton43.Click, ImageButton44.Click, ImageButton45.Click, ImageButton46.Click, ImageButton47.Click, ImageButton48.Click, ImageButton49.Click, ImageButton50.Click, ImageButton51.Click, ImageButton52.Click, ImageButton53.Click, ImageButton54.Click, ImageButton55.Click, ImageButton56.Click, ImageButton57.Click, ImageButton58.Click, ImageButton59.Click, ImageButton60.Click, ImageButton61.Click, ImageButton62.Click, ImageButton63.Click, ImageButton64.Click

        If Session.Item("choosing_piece_to_promote") = True Then
            Exit Sub ' This routine is only for non-promotion actions. Go to the promotion subtroutine below
        End If

        If Session.Item("sender_is_piece_to_move") = Nothing Then
            Session("sender_is_piece_to_move") = "true"
        End If

        Dim ImageButton_clicked As ImageButton
        Dim Location_clicked As Integer
        Dim is_king_moving As Boolean = False

        If is_move_from_ai = False Then
            ImageButton_clicked = DirectCast(sender, ImageButton)
        Else
            ImageButton_clicked = ai_btn_clicked
        End If

        If ImageButton_clicked.ID.Length = 12 Then ' If the length of ID is 12, then the position is a one digit number, else its a two digit number. 
            Location_clicked = CInt(ImageButton_clicked.ID.Substring(11, 1))
        Else
            Location_clicked = CInt(ImageButton_clicked.ID.Substring(11, 2))
        End If
        Dim ImageButton_clicked_ImageURL As String = ImageButton_clicked.ImageUrl

        'If Session.Item("first_move") = True Then
        '    Session("first_move") = False ' Moves after this will no longer be first move
        '    init_sender_is_piece_to_move() ' initialises the sender_is_piece_to_move to true only once, as after this it alternates from true to false to true....
        '    ' cookie to store piece to move
        'End If

        If Session.Item("sender_is_piece_to_move") = "true" Then
            ' cookie to store piece to move

            Session("start_button") = ImageButton_clicked
            Session("start_URL") = ImageButton_clicked_ImageURL
            Session("start_position") = Location_clicked ' an integer representing the button
            'Session("sender_is_piece_to_move") = False
            Session.Item("sender_is_piece_to_move") = "false"
        Else
            ' cookie to store position to move

            Session("end_button") = ImageButton_clicked
            Session("end_URL") = ImageButton_clicked_ImageURL
            Session("end_position") = Location_clicked ' an integer representing the button
            'Session("sender_is_piece_to_move") = True
            Session.Item("sender_is_piece_to_move") = "true"


            If Session("start_button").ImageUrl <> "" Then
                If Session("start_button").ImageUrl.Substring(16, 1) = "K" Then
                    is_king_moving = True
                End If
            End If

            If calc_legal_moves(Session.Item("start_button"), Session.Item("end_button"), CInt(Session.Item("start_position") - 1), CInt(Session.Item("end_position") - 1)) = True Then
                If check(CInt(Session.Item("start_position") - 1), CInt(Session.Item("end_position") - 1), Session.Item("start_button"), Session.Item("end_button"), is_king_moving) = False Then ' can change parameters
                    capture(ImageButton_clicked, CInt(Session.Item("start_position") - 1), CInt(Session.Item("end_position") - 1))
                    switch_player(True) ' swaps players turn once a valid capture or normal move is made
                    If checking_for_checkmate2() = True Then
                        MsgBox("CHECKMATE! - " & Session.Item("player_turn") & " has lost")
                    End If
                    If Session.Item("full_move_number") >= 50 Then
                        MsgBox("DRAW")
                    End If
                    If Session.Item("player_turn") = "b" Then ' black is the AI
                        send_FEN_to_stockfish()
                        stockfish_AI_move()
                    End If
                End If
            End If
            Session("en_passant_target_square") = "-" ' en passant target square resets after each move according to FEN 
        End If
    End Sub

    Sub send_FEN_to_stockfish()
        Dim chess_board(,) As String = Session.Item("chess_board")
        Dim fen_string As String
        Dim castling_string As String = ""
        Dim piece_char_to_add As String
        Dim consec_empty_square_counter As Integer = 0
        For row As Integer = 0 To 7
            For col As Integer = 0 To 7
                If chess_board(col, row) <> Nothing Then
                    If consec_empty_square_counter <> 0 Then
                        fen_string = fen_string + Replace(consec_empty_square_counter.ToString(), " ", "")
                        consec_empty_square_counter = 0
                    End If
                    If chess_board(col, row).Substring(0, 1) = "b" Then
                        piece_char_to_add = LCase(chess_board(col, row).Substring(1, 1))
                    ElseIf chess_board(col, row).Substring(0, 1) = "w" Then
                        piece_char_to_add = chess_board(col, row).Substring(1, 1)
                    End If
                    fen_string = fen_string + piece_char_to_add
                Else
                    consec_empty_square_counter += 1
                End If

            Next
            If consec_empty_square_counter <> 0 Then
                fen_string = fen_string + Replace(consec_empty_square_counter.ToString(), " ", "")
                consec_empty_square_counter = 0 ' resets after each rank
            End If

            If row = 7 Then
                fen_string = fen_string + " " ' all 8 ranks have been complete
            Else
                fen_string = fen_string + "/" ' start new rank for FEN
            End If
        Next

        fen_string = fen_string + Session.Item("player_turn") + " "

        ' Adds if white or black can castle now or in the future and on what side 
        If Session.Item("w_can_castle") = True Then ' King has not moved
            If Session.Item("king_side_w") = False Then ' Rook on kingside has not moved
                castling_string = castling_string + "K"
            End If
            If Session.Item("queen_side_w") = False Then
                castling_string = castling_string + "Q" ' Rook on queenside has not moved
            End If
        End If
        If Session.Item("b_can_castle") = True Then
            If Session.Item("king_side_b") = False Then
                castling_string = castling_string + "k"
            End If
            If Session.Item("queen_side_b") = False Then
                castling_string = castling_string + "q"
            End If
        End If
        If castling_string <> "" Then
            fen_string = fen_string + castling_string + " "
        Else
            fen_string = fen_string + "- "
        End If
        fen_string = fen_string + Session.Item("en_passant_target_square") + " "
        fen_string = fen_string + Session.Item("half_move_clock").ToString() + " " + Session.Item("full_move_number").ToString()

        Dim fileWriter As System.IO.StreamWriter
        fileWriter = My.Computer.FileSystem.OpenTextFileWriter("C:\Users\namjo\OneDrive\Documents\Namjote.S.Dulay\A-levels\Computer_Science\Year 13\NEA Namjote Dulay\NEA project\NEA project\bin\stockfish_input.txt", False)
        fileWriter.WriteLine(fen_string)
        fileWriter.Close()
        ' Run stockfish in python
        Dim retVal As Process
        retVal = Process.Start("C:\Users\namjo\OneDrive\Documents\Namjote.S.Dulay\A-levels\Computer_Science\Year 13\NEA Namjote Dulay\NEA project\NEA project\bin\stockfish_api.py")
    End Sub
    Function calc_position_from_RankFile(ByVal file As String, ByVal rank As String)
        Dim col As Integer = Asc(LCase(file)) - 97 ' zero-indexed
        Dim row As Integer = (CInt(rank) - 8) * -1
        Return (row * 8) + col + 1
    End Function
    Sub stockfish_AI_move()
        ' Read the positions of the board in AN format after the AI has made a move
        Dim fileReader As System.IO.StreamReader
        fileReader = My.Computer.FileSystem.OpenTextFileReader("C:\Users\namjo\OneDrive\Documents\Namjote.S.Dulay\A-levels\Computer_Science\Year 13\NEA Namjote Dulay\NEA project\NEA project\bin\stockfish_output.txt")
        Dim stockfish_move As String
        stockfish_move = fileReader.ReadToEnd
        'Dim FEN_char As Char
        'Dim FEN_string_index As Integer = 0
        Console.WriteLine(stockfish_move)
        ' Convert AN into co-ordinate/image url format for aspx
        'Dim rook_start_position As Integer
        'Dim king_start_position As Integer
        'Dim rook_end_position As Integer
        'Dim king_end_position As Integer
        'Dim player_turn As String = Session.Item("player_turn")
        Dim start_square As String
        Dim end_square As String
        Dim start_position As Integer
        Dim end_position As Integer
        Dim btn_ai_click As ImageButton

        If stockfish_move.Length = 5 Then ' stockfish api promoted a piece
            Session.Item("ai_promotion_piece_type") = stockfish_move.Substring(4, 1)
        End If

        start_square = stockfish_move.Substring(0, 2)
        end_square = stockfish_move.Substring(2, 2)
        start_position = calc_position_from_RankFile(start_square(0), start_square(1))
        end_position = calc_position_from_RankFile(end_square(0), end_square(1))
        btn_ai_click = FindControl("ImageButton" & start_position)
        piece_clicked_ai(Nothing, Nothing, btn_ai_click, True)
        btn_ai_click = FindControl("ImageButton" & end_position)
        piece_clicked_ai(Nothing, Nothing, btn_ai_click, True)

        Session("ai_promotion_piece_type") = ""
    End Sub

    ' This function updates the image URL's of buttons on the chess game when a move is made
    ' End position and start position are 0 indexed
    Sub capture(ByVal end_btn As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer, Optional ByVal castling As Boolean = False)
        Dim chess_board As String(,) = Session.Item("chess_board")
        Dim start_coordinate = New Integer() {start_position Mod 8, start_position \ 8} ' converts button number into co-ordinate form
        Dim end_coordinate = New Integer() {end_position Mod 8, end_position \ 8}
        Dim start_button As ImageButton = FindControl("ImageButton" & (start_position + 1))

        If castling = True Then
            end_btn.ImageUrl = "~/chess pieces/" & Session.Item("player_turn") & "R.png"
        Else
            If castling = False Then
                If start_button.ImageUrl.Substring(16, 1) = "P" Then
                    Session("half_move_clock") = 0
                ElseIf end_btn.ImageUrl <> "" Then
                    Session("half_move_clock") = 0
                Else
                    Session("half_move_clock") = Session("half_move_clock") + 1
                End If
                Session("half_move_number") = Session("half_move_number") + 1
                If Session("half_move_number") Mod 2 = 0 Then
                    Session("full_move_number") = Session("full_move_number") + 1
                End If
            End If
            end_btn.ImageUrl = Session.Item("start_URL")
        End If

        promotion_check(start_button, end_position)
        start_button.ImageUrl = ""
        chess_board(start_coordinate(0), start_coordinate(1)) = Nothing
        chess_board(end_coordinate(0), end_coordinate(1)) = end_btn.ImageUrl.Substring(15, 1) + end_btn.ImageUrl.Substring(16, 1) ' concatenates the type of piece and its colour i.e. bR for black rook
        Session("chess_board") = chess_board
    End Sub

    Function calc_legal_moves(ByVal start_piece As ImageButton, ByVal end_piece As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer, Optional ByVal checking_for_check As Boolean = False)
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
        Dim black_pawn_translations = New Integer(,) {{0, 1}} ' First inner array is for moves after first move. Second array is for first moves only
        Dim white_pawn_translations = New Integer(,) {{0, -1}} ' Two arrays for each piece as pawns cannot move backwards...
        Dim black_pawn_first_move_translations = New Integer(,) {{0, 1}, {0, 2}} ' First inner array is for moves after first move. Second array is for first moves only
        Dim white_pawn_first_move_translations = New Integer(,) {{0, -1}, {0, -2}} ' Two arrays for each piece as pawns cannot move backwards...
        Dim black_pawn_kill_translations = New Integer(,) {{-1, 1}, {1, 1}}
        Dim white_pawn_kill_translations = New Integer(,) {{-1, -1}, {1, -1}}
        Dim bishop_translations = New Integer(,) {{1, 1}, {-1, -1}, {2, 2}, {-2, -2}, {3, 3}, {-3, -3}, {4, 4}, {-4, -4}, {5, 5}, {-5, -5}, {6, 6}, {-6, -6}, {7, 7}, {-7, -7}, {8, 8}, {-8, -8}, {-1, 1}, {1, -1}, {-2, 2}, {2, -2}, {-3, 3}, {3, -3}, {-4, 4}, {4, -4}, {-5, 5}, {5, -5}, {-6, 6}, {6, -6}, {-7, 7}, {7, -7}, {-8, 8}, {8, -8}}
        Dim rook_translations = New Integer(,) {{0, 1}, {0, -1}, {0, 2}, {0, -2}, {0, 3}, {0, -3}, {0, 4}, {0, -4}, {0, 5}, {0, -5}, {0, 6}, {0, -6}, {0, 7}, {0, -7}, {0, 8}, {0, -8}, {1, 0}, {-1, 0}, {2, 0}, {-2, 0}, {3, 0}, {-3, 0}, {4, 0}, {-4, 0}, {5, 0}, {-5, 0}, {6, 0}, {-6, 0}, {7, 0}, {-7, 0}, {8, 0}, {-8, 0}}
        Dim king_translations = New Integer(,) {{0, -1}, {1, -1}, {1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}}
        Dim king_castling_translations = New Integer(,) {{2, 0}, {-2, 0}}
        Dim black_en_passant_translations = New Integer(,) {{0, 1}, {-1, 1}, {1, 1}}
        Dim white_en_passant_translations = New Integer(,) {{0, -1}, {-1, -1}, {1, -1}}
        Dim pawn_first_move_tracker As Integer(,) = Session.Item("pawn_first_move_tracker")

        If end_piece_colour = Session.Item("player_turn") And checking_for_check = False Then ' Player is moving a piece onto a position that has one of their own pieces on it
            Return False
        ElseIf start_piece_type = vbNullChar Then ' Player is "attempting" to move an empty space
            Return False
        ElseIf start_piece_type = "P" Then
            If start_piece_colour = "b" And checking_for_check = False Then  ' Black pawns
                If end_piece_colour = vbNullChar Then ' Moving to empty square
                    If Session.Item("b_en_passant_status") = True Then
                        Return en_passant(black_en_passant_translations, start_coordinate, end_coordinate, start_position, end_position)
                    ElseIf pawn_first_move_tracker(0, start_coordinate(0)) = 1 And start_coordinate(1) = 1 And Translate(black_pawn_first_move_translations, start_coordinate, end_coordinate, False, True, checking_for_check) = True Then ' If its the pawns first move
                        en_passant_check(start_coordinate) ' will allow enemy to do en passant if true
                        pawn_first_move_tracker(0, start_coordinate(0)) = 0
                        Session("pawn_first_move_tracker") = pawn_first_move_tracker
                        Return True
                    Else
                        Return Translate(black_pawn_translations, start_coordinate, end_coordinate) ' Moves after first move
                    End If
                ElseIf end_piece_colour = "w" Or checking_for_check = True Then ' Attacking 
                    Return Translate(black_pawn_kill_translations, start_coordinate, end_coordinate)
                End If

            ElseIf start_piece_colour = "w" Then ' White pawns
                If end_piece_colour = vbNullChar And checking_for_check = False Then ' Moving to empty square
                    If Session.Item("w_en_passant_status") = True Then
                        Return en_passant(white_en_passant_translations, start_coordinate, end_coordinate, start_position, end_position)
                    ElseIf pawn_first_move_tracker(1, start_coordinate(0)) = 1 And start_coordinate(1) = 6 And Translate(white_pawn_first_move_translations, start_coordinate, end_coordinate, False, True, checking_for_check) = True Then ' If its the pawns first move
                        en_passant_check(start_coordinate) ' will allow enemy to do en passant if true
                        pawn_first_move_tracker(1, start_coordinate(0)) = 0
                        Session("pawn_first_move_tracker") = pawn_first_move_tracker
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
        ElseIf start_piece_type = "Q" And (Translate(bishop_translations, start_coordinate, end_coordinate) = True Or Translate(rook_translations, start_coordinate, end_coordinate) = True) Then ' The queen is the combination of bishop and rook valid moves
            Return True
        ElseIf start_piece_type = "K" Then
            If checking_for_check = False Then
                If castling(king_castling_translations, start_coordinate, end_coordinate) = True Then
                    If Session.Item("player_turn") = "b" Then
                        Session("b_can_castle") = False
                    Else
                        Session("w_can_castle") = False
                    End If
                    Return True
                ElseIf Translate(king_translations, start_coordinate, end_coordinate) = True Then
                    If Session.Item("player_turn") = "b" Then
                        Session("b_can_castle") = False
                    Else
                        Session("w_can_castle") = False
                    End If
                    Return True
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
    'optinal parameter IsKnight. We need this to tell the translate function to not check if there is a piece blocking it from moving as knights can jump over them.
    'Default value for this optinal parameter is False.
    Function Translate(ByVal piece_translations As Integer(,), ByVal start_coordinate As Integer(), ByVal end_coordinate As Integer(), Optional ByVal IsKnight As Boolean = False, Optional ByVal pawn_first_move As Boolean = False, Optional ByVal checking_for_check As Boolean = False)
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

            If pawn_first_move = True And checking_for_check = False Then
                If index = 1 Then ' if pawn translation for moving two squares is in the 1st index of piece_translations
                    Session("en_passant_target_square") = Chr(start_coordinate(0) + 97) + Replace(Str(((start_coordinate(1) + possible_move_LCM(1)) * -1) + 8), " ", "")
                End If
            End If

            If end_coordinate(0) = possible_move(0) And end_coordinate(1) = possible_move(1) Then ' Valid translation identified
                'Calculate the translation lowest multiple i.e. if (-5, 5) is a valid move for a bishop, the multiple will be N(-1, 1) where N = 5
                If IsKnight <> True And N <> 1 Then ' Checks if there are pieces in the way of a non-knight piece moving if its moving more than one square
                    For multiple As Integer = 1 To N - 1
                        If chess_board(start_coordinate(0) + (multiple * possible_move_LCM(0)), start_coordinate(1) + (multiple * possible_move_LCM(1))) <> Nothing Then ' Something wrong with chess_board
                            Return False ' there is a piece in the way of the moving piece
                        End If
                    Next
                End If
                Return True
            End If
        Next
        Return False
    End Function

    Function castling(ByVal castling_translations As Integer(,), ByVal start_coordinate As Integer(), ByVal end_coordinate As Integer()) ' CLEAN THIS UP
        'Identify which rook is being used for castling

        Dim rook As String
        Dim rook_position As Integer
        Dim empty_space_check = New Integer(,) {{3, 0}, {-4, 0}}
        Dim empty_space_btn As ImageButton
        Dim king_position As Integer = (start_coordinate(1) * 8) + start_coordinate(0) ' zero indexed

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
                    If check(king_position, king_position + i, FindControl("ImageButton" & king_position), FindControl("ImageButton" & king_position + i), True) = True Then ' checks two squares to the left or right of the king to see if they are under attack
                        Return False
                    End If
                Next
            Else
                For i As Integer = 0 To 2
                    If i = -2 Then
                        Console.WriteLine("")
                    End If
                    If check(king_position, king_position - i, FindControl("ImageButton" & king_position), FindControl("ImageButton" & king_position - i), True) = True Then ' checks two squares to the left or right of the king to see if they are under attack
                        Return False
                    End If
                Next
            End If
        End If

        ' Moving king and rook
        If rook = "king_side" Then
            capture(FindControl("ImageButton" & (rook_position - 2)), rook_position - 1, rook_position - 3, True) ' Last arguement is optional and tells capture function that we are castling
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
                    Session.Item(attack_colour & "_en_passant_status") = True
                End If
            End If
        Next
    End Sub

    Function en_passant(ByVal en_passant_translations As Integer(,), ByVal start_coordinate As Integer(), ByVal end_coordinate As Integer(), ByVal start_position As Integer, ByVal end_position As Integer)
        Dim possible_en_passant_translation = New Integer() {0, 0}
        Dim pawn_to_capture_coordinate = New Integer() {}
        Dim pawn_to_capture_if_en_passant As ImageButton
        Dim chess_board As String(,) = Session.Item("chess_board")
        possible_en_passant_translation(0) = end_coordinate(0) - start_coordinate(0)
        possible_en_passant_translation(1) = end_coordinate(1) - start_coordinate(1)

        ' FOR MOVES DOING EN PASSANT
        For i As Integer = 0 To en_passant_translations.GetLength(0) - 1
            If en_passant_translations(i, 0) = possible_en_passant_translation(0) And en_passant_translations(i, 1) = possible_en_passant_translation(1) Then ' Player is doing en passant
                ' if the pawn that is attacking using en passant is moving right one column, the pawn on the right of it originally will be captured, else pawn on left will be captured
                If possible_en_passant_translation(0) = 1 Then
                    pawn_to_capture_coordinate = New Integer() {(start_position + 1) Mod 8, (start_position + 1) \ 8}
                    pawn_to_capture_if_en_passant = FindControl("ImageButton" & (start_position + 2))
                Else
                    pawn_to_capture_coordinate = New Integer() {(start_position - 1) Mod 8, (start_position - 1) \ 8}
                    pawn_to_capture_if_en_passant = FindControl("ImageButton" & (start_position))
                End If

                If Translate(en_passant_translations, start_coordinate, end_coordinate) = True Then
                    pawn_to_capture_if_en_passant.ImageUrl = ""
                    chess_board(pawn_to_capture_coordinate(0), pawn_to_capture_coordinate(1)) = Nothing
                    Session("chess_board") = chess_board
                    Session(Session.Item("player_turn") & "_en_passant_status") = False
                    Return True
                End If
            End If
        Next
        ' FOR MOVES NOT DOING EN PASSANT EVEN IF EN PASSANT WAS AVAILABLE
        If Translate(en_passant_translations, start_coordinate, end_coordinate) = True Then
            Session(Session.Item("player_turn") & "en_passant_status") = False
            Return True
        Else
            Return False
        End If
    End Function

    Function check(ByVal start_position As Integer, ByVal end_position As Integer, ByVal start_btn As ImageButton, ByVal end_btn As ImageButton, ByVal is_king_moving As Boolean) ' end_position is zero indexed
        ' Use the chess board session
        ' Carry out every current player possible move for each of their pieces, and add this to an array
        ' Loop through array to see if there is an element in this array that matches king end-
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
            For column As Integer = 0 To 7 ' can be made into a function as its repeated
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
        If start_position <> end_position Then ' interesting error for castling
            chess_board(end_position Mod 8, end_position \ 8) = start_btn.ImageUrl.Substring(15, 2) 'wrong, use the parameter
            chess_board(start_position Mod 8, start_position \ 8) = Nothing
            original_end_btn_url = end_btn.ImageUrl ' interesting error
            end_btn.ImageUrl = start_btn.ImageUrl
            start_btn.ImageUrl = ""
        End If
        'Attempt to move every piece on the board to kill the king to see if player is in check
        switch_player(True)
        For temp_start_position As Integer = 1 To 64
            temp_start_piece = FindControl("ImageButton" & temp_start_position)
            If temp_start_position = 53 Then
                Console.WriteLine("")
            End If

            If calc_legal_moves(temp_start_piece, king_button, temp_start_position - 1, king_position - 1, True) = True Then ' DEBUG THIS - CAUSES STACK OVERFLOW ERROR!!!!!!!!!!!
                'MsgBox("check")
                switch_player(True)
                'Reverse the temporary move as we have finished checking if the move causes check
                ' Could but this into a function as its repeated twice
                If start_position <> end_position Then
                    chess_board(start_position Mod 8, start_position \ 8) = end_btn.ImageUrl.Substring(15, 2)
                    If original_end_btn_url = "" Then
                        chess_board(end_position Mod 8, end_position \ 8) = Nothing
                    Else
                        chess_board(end_position Mod 8, end_position \ 8) = original_end_btn_url.Substring(15, 2)
                    End If
                    start_btn.ImageUrl = end_btn.ImageUrl
                    end_btn.ImageUrl = original_end_btn_url
                End If
                Return True
            End If
        Next
        switch_player(True)
        ' Reverse the temporary move as we have finished checking if the move causes check
        If start_position <> end_position Then
            chess_board(start_position Mod 8, start_position \ 8) = end_btn.ImageUrl.Substring(15, 2)
            If original_end_btn_url = "" Then
                chess_board(end_position Mod 8, end_position \ 8) = Nothing
            Else
                chess_board(end_position Mod 8, end_position \ 8) = original_end_btn_url.Substring(15, 2)
            End If
            start_btn.ImageUrl = end_btn.ImageUrl
            end_btn.ImageUrl = original_end_btn_url
        End If
        Return False
    End Function
    Sub promotion_check(ByVal start_piece As ImageButton, ByVal end_position As Integer)
        If Session.Item("ai_promotion_piece_type") = "" Then ' if its empty, the AI is not promoting to don't check
            Dim end_coordinate_row As Integer = end_position \ 8
            If Session("start_button").ImageUrl.Substring(16, 1) = "P" And (end_coordinate_row = 0 Or end_coordinate_row = 7) Then
                MsgBox("Select a piece in the pink box to promote a pawn")
                ' Pop up window for player to select a bishop, rook, knight or queen
                Session("choosing_piece_to_promote") = True
            End If
        End If
    End Sub

    Sub promotion(sender As Object, e As ImageClickEventArgs) Handles Q_promote_btn.Click, N_promote_btn.Click, R_promote_btn.Click, B_promote_btn.Click ', ImageButton1.Click, ImageButton2.Click, ImageButton3.Click, ImageButton4.Click, ImageButton5.Click, ImageButton6.Click, ImageButton7.Click, ImageButton8.Click, ImageButton9.Click, ImageButton10.Click, ImageButton11.Click, ImageButton12.Click, ImageButton13.Click, ImageButton14.Click, ImageButton15.Click, ImageButton16.Click, ImageButton17.Click, ImageButton18.Click, ImageButton19.Click, ImageButton20.Click, ImageButton21.Click, ImageButton22.Click, ImageButton23.Click, ImageButton24.Click, ImageButton25.Click, ImageButton26.Click, ImageButton27.Click, ImageButton28.Click, ImageButton29.Click, ImageButton30.Click, ImageButton31.Click, ImageButton32.Click, ImageButton33.Click, ImageButton34.Click, ImageButton35.Click, ImageButton36.Click, ImageButton37.Click, ImageButton38.Click, ImageButton39.Click, ImageButton40.Click, ImageButton41.Click, ImageButton42.Click, ImageButton43.Click, ImageButton44.Click, ImageButton45.Click, ImageButton46.Click, ImageButton47.Click, ImageButton48.Click, ImageButton49.Click, ImageButton50.Click, ImageButton51.Click, ImageButton52.Click, ImageButton53.Click, ImageButton54.Click, ImageButton55.Click, ImageButton56.Click, ImageButton57.Click, ImageButton58.Click, ImageButton59.Click, ImageButton60.Click, ImageButton61.Click, ImageButton62.Click, ImageButton63.Click, ImageButton64.Click
        If Session.Item("choosing_piece_to_promote") = False And Session.Item("ai_promotion_piece_type") = "" Then ' if the player or AI are not promoting then exit this sub
            Exit Sub ' This routine is only for promotion actions
        End If
        switch_player(True)
        Dim promote_btn As ImageButton


        If Session.Item("choosing_piece_to_promote") = True Then
            promote_btn = DirectCast(sender, ImageButton)
        ElseIf Session.Item("ai_promotion_piece_type") = "q" Then
            promote_btn = Q_promote_btn
        ElseIf Session.Item("ai_promotion_piece_type") = "n" Then
            promote_btn = N_promote_btn
        ElseIf Session.Item("ai_promotion_piece_type") = "b" Then
            promote_btn = B_promote_btn
        ElseIf Session.Item("ai_promotion_piece_type") = "r" Then
            promote_btn = R_promote_btn
        End If

        Session("choosing_piece_to_promote") = False ' We are going to promote now so we can set it back to false

        Dim pawn_to_promote As ImageButton = Session("end_button")
        Dim pawn_position As Integer
        Dim promotion_url As String
        If pawn_to_promote.ID.Length = 12 Then ' If the length of ID is 12, then the position is a one digit number, else its a two digit number. 
            pawn_position = CInt(pawn_to_promote.ID.Substring(11, 1))
        Else
            pawn_position = CInt(pawn_to_promote.ID.Substring(11, 2))
        End If
        pawn_to_promote = FindControl("ImageButton" & pawn_position)
        Dim pawn_coordinate = New Integer() {(pawn_position - 1) Mod 8, (pawn_position - 1) \ 8}
        If Session.Item("player_turn") = "b" Then
            promotion_url = promote_btn.ImageUrl.Replace("w", "b")
        Else
            promotion_url = promote_btn.ImageUrl
        End If
        MsgBox(promotion_url)
        pawn_to_promote.ImageUrl = promotion_url
        Dim chess_board As String(,) = Session.Item("chess_board")
        chess_board(pawn_coordinate(0), pawn_coordinate(1)) = pawn_to_promote.ImageUrl.Substring(15, 1) + pawn_to_promote.ImageUrl.Substring(16, 1)
        Session("chess_board") = chess_board
        switch_player(True)
        ' When button is selected, hide window/grey it out
    End Sub
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
                If chess_board(column, row) = Session("player_turn") & "K" Then
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
                        end_btn = FindControl("ImageButton" & end_btn_position)
                        If end_btn.ImageUrl <> "" Then
                            If end_btn.ImageUrl.Substring(15, 1) = Session.Item("player_turn") Then
                                friendly_neighbour_counter += 1
                            End If
                        End If
                        ' Function calc_legal_moves(ByVal start_piece As ImageButton, ByVal end_piece As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer, Optional ByVal checking_for_check As Boolean = False)
                        If calc_legal_moves(king_btn, end_btn, king_position - 1, end_btn_position - 1) = True Then
                            If check(king_position - 1, end_btn_position - 1, king_btn, end_btn, True) = False Then
                                Return False
                            End If
                        End If
                    Next
                End If
            Next
        Next
        If legal_and_illegal_possible_moves = friendly_neighbour_counter Then ' interesting error
            Return False
        End If
        Return True
    End Function
    'Function calc_legal_moves(ByVal start_piece As ImageButton, ByVal end_piece As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer, Optional ByVal checking_for_check As Boolean = False)
    ' Function check(ByVal start_position As Integer, ByVal end_position As Integer, ByVal start_btn As ImageButton, ByVal end_btn As ImageButton, ByVal is_king_moving As Boolean) ' end_position is zero indexed
    Function checking_for_checkmate2()
        Dim start_btn As ImageButton
        Dim end_btn As ImageButton
        Dim is_king_moving As Boolean
        For start_position As Integer = 1 To 64
            start_btn = FindControl("ImageButton" & start_position)
            is_king_moving = False
            If start_btn.ImageUrl <> "" Then
                If start_btn.ImageUrl.Substring(16, 1) = "K" Then
                    is_king_moving = True
                End If
            End If
            For end_position As Integer = 1 To 64
                If start_position = 14 Then
                    Console.WriteLine("")
                End If
                If start_position = end_position Then
                    Continue For
                End If
                end_btn = FindControl("ImageButton" & end_position)
                If calc_legal_moves(start_btn, end_btn, start_position - 1, end_position - 1) = True Then
                    If check(start_position - 1, end_position - 1, start_btn, end_btn, is_king_moving) = False Then
                        Return False
                    End If
                End If
            Next
        Next
        Return True
    End Function

End Class