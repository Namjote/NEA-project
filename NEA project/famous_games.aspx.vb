Imports System.Data.OleDb
Public Class historical_games
    Inherits System.Web.UI.Page
    Public connectFamousGames As New DataSet1TableAdapters.famous_gamesTableAdapter


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim numrows As Integer
        Dim r As TableRow
        Dim c As TableCell
        Dim title As TableRow
        Dim col_heading As TableCell
        Dim database_row_record As DataTable
        title = New TableRow()

        ' Column names for the table
        col_heading = New TableCell()
        col_heading.Controls.Add(New LiteralControl("ID"))
        title.Cells.Add(col_heading)
        col_heading.Controls.Add(New LiteralControl(" | Year"))
        title.Cells.Add(col_heading)
        col_heading.Controls.Add(New LiteralControl(" | Location"))
        title.Cells.Add(col_heading)
        col_heading.Controls.Add(New LiteralControl(" | Players"))
        title.Cells.Add(col_heading)
        col_heading.Controls.Add(New LiteralControl(" | ECO"))
        title.Cells.Add(col_heading)
        Table1.Rows.Add(title)

        ' number of games stores on the famous games table in the database
        numrows = connectFamousGames.NumOfRowsInfamous_games

        For row = 0 To numrows - 1
            r = New TableRow() ' Create a row
            database_row_record = connectFamousGames.SelectFamousGameFromID(row + 1)
            Dim database_row_array = (From column In database_row_record.AsEnumerable() Select column).ToArray()
            c = New TableCell() ' Create a cell

            c.Controls.Add(New LiteralControl(database_row_array(0).Field(Of Integer)("ID"))) ' Fill a cell with data 
            c.Controls.Add(New LiteralControl(" | "))
            r.Cells.Add(c) ' Add the cell to a row
            c.Controls.Add(New LiteralControl(database_row_array(0).Field(Of Integer)("time")))
            c.Controls.Add(New LiteralControl(" | "))
            r.Cells.Add(c)
            c.Controls.Add(New LiteralControl(database_row_array(0).Field(Of String)("location")))
            c.Controls.Add(New LiteralControl(" | "))
            r.Cells.Add(c)
            c.Controls.Add(New LiteralControl(database_row_array(0).Field(Of String)("players")))
            c.Controls.Add(New LiteralControl(" | "))
            r.Cells.Add(c)
            c.Controls.Add(New LiteralControl(database_row_array(0).Field(Of String)("ECO")))

            r.Cells.Add(c)
            Table1.Rows.Add(r) ' Add the row to the table
        Next row
    End Sub
    Protected Sub submit_Click(sender As Object, e As EventArgs) Handles submit_btn.Click
        Dim moves_string As String
        If IsNumeric(game_id_txt.Text) = True AndAlso CInt(game_id_txt.Text) <= connectFamousGames.NumOfRowsInfamous_games AndAlso CInt(game_id_txt.Text) > 0 Then
            moves_string = connectFamousGames.SelectMovesFromID(game_id_txt.Text)
        Else
            MsgBox("Incorrect game ID")
            Exit Sub
        End If
        Dim str_to_replace As String
        Dim num_of_moves As Integer
        Dim moves_array As String()
        Dim player_turn As Char = "w"
        Dim chess_board(,) As String = Session.Item("chess_board")

        ' Parse through the moves string to get rid of the unrequired data
        For character As Integer = 0 To moves_string.Length - 1
            ' Original move string example --> ... 26. Nxf6+ Nxf6 27. bxa3 Nd5 28. Bxd5 cxd5 29. Ne5 ...
            If moves_string(character) = "." Then
                If moves_string(character) = "+" Then
                    moves_string = Replace(moves_string, "+", "", 1) ' --> ... 26. Nxf6 Nxf6 27. bxa3 Nd5 28. Bxd5 cxd5 29. Ne5 ...
                ElseIf character - 2 > 0 AndAlso "0123456789".Contains(moves_string(character - 2)) = True Then
                    str_to_replace = moves_string.Substring(character - 2, 4) ' --> ... Nxf6 Nxf6 bxa3 Nd5 Bxd5 cxd5 Ne5 ...
                Else
                    str_to_replace = moves_string.Substring(character - 1, 3)
                End If
                moves_string = Replace(moves_string, str_to_replace, "", , 1)
            End If
            If character = moves_string.Length - 1 Then
                Exit For ' We have parsed through the entire string
            End If
        Next

        For character As Integer = 0 To moves_string.Length - 1
            If moves_string(character) = " " Then ' From the new move_string, each space represents a new move
                num_of_moves += 1
            End If
        Next

        Dim move_history(,) As String = Session.Item("move_history")
        Session("move_history") = move_history
        num_of_moves += 1
        Session("num_of_moves") = num_of_moves
        moves_array = moves_string.Split(" ") ' Split the moves_string into indidual moves

        For move_num As Integer = 1 To num_of_moves
            ' Load the previous board positions into the current board positions
            For piece As Integer = 0 To 63
                move_history(move_num, piece) = move_history(move_num - 1, piece)
            Next
            If move_num = 7 Then
                Console.WriteLine("Hi")
            End If
            AN_to_imagebutton(move_num, moves_array, player_turn) ' will update the move_history array for the current move
            move_history = Session.Item("move_history")
            If player_turn = "w" Then
                player_turn = "b"
            Else
                player_turn = "w"
            End If

            ' will update the chess_board array for the current move
            For piece As Integer = 0 To 63
                Dim piece_string As String
                If move_history(move_num, piece) = Nothing Then ' A piece has left this position
                    piece_string = Nothing
                    chess_board(piece Mod 8, piece \ 8) = piece_string
                ElseIf move_history(move_num, piece) <> "" Or move_history(move_num, piece) <> Nothing Then ' A piece has moved to this square
                    piece_string = move_history(move_num, piece).Substring(15, 2)
                    chess_board(piece Mod 8, piece \ 8) = piece_string
                End If
            Next
            Session("chess_board") = chess_board
        Next
        MsgBox("Use the arrows step through the game!")
    End Sub
    Sub AN_to_imagebutton(ByVal move_num As Integer, ByVal moves_array As String(), ByVal player_turn As Char)
        Dim AN As String ' AN is a single move in algebraic notation
        AN = moves_array(move_num - 1)
        AN = AN.Replace("+", "")
        If AN = "c4" Then
            Console.WriteLine("Hi")
        End If
        Dim moves_history(,) As String = Session.Item("move_history")
        Dim end_position As Integer
        Dim start_position As Integer
        Dim file As String
        Dim rank As String
        Dim chess_board As String(,) = Session.Item("chess_board")

        ' If the move is one of the below, the game has ended
        ' Will be outputted on next button click
        If AN = "1/2-1/2" Then
            Session("result") = "draw"
            Exit Sub
        ElseIf AN = "1-0" Then
            Session("result") = "white won"
            Exit Sub
        ElseIf AN = "0-1" Then
            Session("result") = "black won"
            Exit Sub
        End If

        Dim start_piece_type As String
        Dim start_coordinate() As Integer
        Dim end_coordinate() As Integer
        Dim is_knight_moving As Boolean = False

        If AN.Length = 2 Then ' Moving a pawn e.g. "e4"
            file = AN.Substring(0, 1) ' the end position column
            rank = AN.Substring(1, 1) ' the end position row
            start_piece_type = "P" ' since this if statement only deals with pawns, start piece type is always "P"
            end_position = calc_position_from_RankFile(file, rank)
            end_coordinate = New Integer() {(end_position - 1) Mod 8, (end_position - 1) \ 8}

            ' Find a start piece that correctly translates to the end position desribed by the move in AN
            For piece As Integer = 1 To 64
                start_position = piece
                start_coordinate = New Integer() {(start_position - 1) Mod 8, (start_position - 1) \ 8}
                If piece = 51 Then
                    Console.WriteLine("HI")
                End If
                If chess_board(start_coordinate(0), start_coordinate(1)) <> "" AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(0, 1) = player_turn AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(1, 1) = start_piece_type AndAlso Translate(SelectTranslation(start_piece_type, player_turn), start_coordinate, end_coordinate, is_knight_moving) = True Then
                    ' If a valid start piece is found (it is the correct colour, piece type and there is a legal translation from start to end positions
                    Exit For
                End If
            Next piece

            ' Load the start image URL into the end position for a particular move number in move history
            moves_history(move_num, end_position - 1) = "~/chess pieces/" & chess_board(start_coordinate(0), start_coordinate(1)) & ".png"
            ' Assign nothing as the image URL to the start position as the piece has just left this position
            moves_history(move_num, start_position - 1) = Nothing
        End If

        If AN.Length = 4 AndAlso UCase(AN.Substring(0, 1)) <> AN.Substring(0, 1) Then ' Pawns capturing e.g. "exc4"
            file = AN.Substring(2, 1)
            rank = AN.Substring(3, 1)
            start_piece_type = "P"
            end_position = calc_position_from_RankFile(file, rank)
            end_coordinate = New Integer() {(end_position - 1) Mod 8, (end_position - 1) \ 8}

            For piece As Integer = 1 To 64
                start_position = piece
                start_coordinate = New Integer() {(start_position - 1) Mod 8, (start_position - 1) \ 8}

                If chess_board(start_coordinate(0), start_coordinate(1)) <> "" AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(0, 1) = player_turn AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(1, 1) = start_piece_type AndAlso Translate(SelectTranslation(start_piece_type, player_turn, True), start_coordinate, end_coordinate, is_knight_moving) = True Then ' BUG IS HERE
                    Exit For
                End If
            Next piece
            moves_history(move_num, end_position - 1) = "~/chess pieces/" & chess_board(start_coordinate(0), start_coordinate(1)) & ".png"
            moves_history(move_num, start_position - 1) = Nothing
        End If

        If AN.Length = 3 And AN <> "O-O" Then ' Moves without capture and no amiguity on rank or file and not castling e.g. "Nf6"
            file = AN.Substring(1, 1)
            rank = AN.Substring(2, 1)
            start_piece_type = AN.Substring(0, 1)
            end_position = calc_position_from_RankFile(file, rank)
            end_coordinate = New Integer() {(end_position - 1) Mod 8, (end_position - 1) \ 8}

            For piece As Integer = 1 To 64
                start_position = piece
                start_coordinate = New Integer() {(start_position - 1) Mod 8, (start_position - 1) \ 8}

                If start_piece_type = "N" Then
                    is_knight_moving = True
                End If
                If chess_board(start_coordinate(0), start_coordinate(1)) <> "" AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(0, 1) = player_turn AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(1, 1) = start_piece_type AndAlso Translate(SelectTranslation(start_piece_type, player_turn), start_coordinate, end_coordinate, is_knight_moving) = True Then
                    Exit For
                End If
            Next piece
            moves_history(move_num, end_position - 1) = "~/chess pieces/" & chess_board(start_coordinate(0), start_coordinate(1)) & ".png"
            moves_history(move_num, start_position - 1) = Nothing
        End If

        If AN.Length = 4 AndAlso AN.Contains("x") = True AndAlso UCase(AN.Substring(0, 1)) = AN.Substring(0, 1) Then ' Moves with capture and no amiguity on rank or file e.g. "Bxc6"
            file = AN.Substring(2, 1)
            rank = AN.Substring(3, 1)
            start_piece_type = AN.Substring(0, 1)
            end_position = calc_position_from_RankFile(file, rank)
            end_coordinate = New Integer() {(end_position - 1) Mod 8, (end_position - 1) \ 8}

            For piece As Integer = 1 To 64
                start_position = piece
                start_coordinate = New Integer() {(start_position - 1) Mod 8, (start_position - 1) \ 8}

                If start_piece_type = "N" Then
                    is_knight_moving = True
                End If
                If chess_board(start_coordinate(0), start_coordinate(1)) <> "" AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(0, 1) = player_turn AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(1, 1) = start_piece_type AndAlso Translate(SelectTranslation(start_piece_type, player_turn, True), start_coordinate, end_coordinate, is_knight_moving) = True Then
                    Exit For
                End If
            Next piece
            moves_history(move_num, end_position - 1) = "~/chess pieces/" & chess_board(start_coordinate(0), start_coordinate(1)) & ".png"
            moves_history(move_num, start_position - 1) = Nothing
        End If

        Dim start_piece_rank_or_file As String
        Dim start_column As Integer = -1
        Dim start_row As Integer = -1
        If AN.Length = 4 AndAlso AN.Contains("x") = False Then ' Moves without capture and amiguity on rank or file e.g. Nfd2 or N3d2

            file = AN.Substring(2, 1)
            rank = AN.Substring(3, 1)
            start_piece_type = AN.Substring(0, 1)
            end_position = calc_position_from_RankFile(file, rank)
            end_coordinate = New Integer() {(end_position - 1) Mod 8, (end_position - 1) \ 8}
            start_piece_rank_or_file = AN.Substring(1, 1)

            ' Since more than one piece can move to the end position, stockfish has given us the start rank or file 
            ' If we have been given a rank, then IsNumeric is true, else if its a file, its false
            If IsNumeric(start_piece_rank_or_file) = True Then
                start_row = (CInt(start_piece_rank_or_file) - 8) * -1
            Else
                start_column = Asc(start_piece_rank_or_file) - 97
            End If

            For piece As Integer = 1 To 64
                start_position = piece
                start_coordinate = New Integer() {(start_position - 1) Mod 8, (start_position - 1) \ 8}

                If start_piece_type = "N" Then
                    is_knight_moving = True
                End If
                If chess_board(start_coordinate(0), start_coordinate(1)) <> "" AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(0, 1) = player_turn AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(1, 1) = start_piece_type AndAlso Translate(SelectTranslation(start_piece_type, player_turn), start_coordinate, end_coordinate, is_knight_moving) = True Then
                    If start_column = (start_position - 1) Mod 8 Or start_row = (start_position - 1) \ 8 Then ' Removes amiguity if two of the same piece type can go to the end piece
                        Exit For
                    End If
                End If
            Next piece
            moves_history(move_num, end_position - 1) = "~/chess pieces/" & chess_board(start_coordinate(0), start_coordinate(1)) & ".png"
            moves_history(move_num, start_position - 1) = Nothing
        End If

        If AN.Length = 5 And AN.Contains("x") = True Then ' Moves with capture and amiguity on rank or file e.g. Nfxd2 or N3xd2

            file = AN.Substring(3, 1)
            rank = AN.Substring(4, 1)
            start_piece_type = AN.Substring(0, 1)
            end_position = calc_position_from_RankFile(file, rank)
            end_coordinate = New Integer() {(end_position - 1) Mod 8, (end_position - 1) \ 8}
            start_piece_rank_or_file = AN.Substring(1, 1)

            If IsNumeric(start_piece_rank_or_file) = True Then
                start_row = (CInt(start_piece_rank_or_file) - 8) * -1
            Else
                start_column = Asc(start_piece_rank_or_file) - 97
            End If

            For piece As Integer = 1 To 64
                start_position = piece
                start_coordinate = New Integer() {(start_position - 1) Mod 8, (start_position - 1) \ 8}

                If start_piece_type = "N" Then
                    is_knight_moving = True
                End If
                If chess_board(start_coordinate(0), start_coordinate(1)) <> "" AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(0, 1) = player_turn AndAlso chess_board(start_coordinate(0), start_coordinate(1)).Substring(1, 1) = start_piece_type AndAlso Translate(SelectTranslation(start_piece_type, player_turn, True), start_coordinate, end_coordinate, is_knight_moving) = True Then
                    If start_column = (start_position - 1) Mod 8 Or start_row = (start_position - 1) \ 8 Then ' Removes amiguity if two of the same piece type can go to the end piece
                        Exit For
                    End If
                End If
            Next piece
            moves_history(move_num, end_position - 1) = "~/chess pieces/" & chess_board(start_coordinate(0), start_coordinate(1)) & ".png"
            moves_history(move_num, start_position - 1) = Nothing
        End If

        Dim rook_start_position As Integer
        Dim king_start_position As Integer
        Dim rook_end_position As Integer
        Dim king_end_position As Integer
        Dim start_btn As ImageButton

        If AN = "O-O" Or AN = "O-O-O" Then
            If AN = "O-O" Then  ' King-side castle
                If player_turn = "w" Then
                    rook_start_position = 64
                    king_start_position = 61
                Else
                    rook_start_position = 8
                    king_start_position = 5
                End If
                rook_end_position = rook_start_position - 2
                king_end_position = king_start_position + 2
            End If

            If AN = "O-O-O" Then ' Queen-side castle
                If player_turn = "w" Then
                    rook_start_position = 57
                    king_start_position = 61
                Else
                    rook_start_position = 1
                    king_start_position = 5
                End If
                rook_end_position = rook_start_position + 3
                king_end_position = king_start_position - 2
            End If

            ' For the rooks
            start_btn = FindControl("ImageButton" & rook_start_position)
            moves_history(move_num, rook_end_position - 1) = start_btn.ImageUrl
            moves_history(move_num, rook_start_position - 1) = Nothing
            ' For the kings
            start_btn = FindControl("ImageButton" & king_start_position)
            moves_history(move_num, king_end_position - 1) = start_btn.ImageUrl
            moves_history(move_num, king_start_position - 1) = Nothing
        End If
        Session("moves_history") = moves_history
    End Sub

    Function calc_position_from_RankFile(ByVal file As String, ByVal rank As String)
        Dim col As Integer = Asc(file) - 97 ' zero-indexed
        Dim row As Integer = (CInt(rank) - 8) * -1
        Return (row * 8) + col + 1
    End Function

    Function SelectTranslation(ByVal start_piece_type As String, Optional ByVal player_turn As String = "", Optional ByVal attacking As Boolean = False) As Integer(,)
        Dim knight_translations = New Integer(,) {{1, 2}, {2, 1}, {-1, 2}, {-2, 1}, {2, -1}, {1, -2}, {-2, -1}, {-1, -2}}
        Dim black_pawn_kill_translations = New Integer(,) {{-1, 1}, {1, 1}}
        Dim white_pawn_kill_translations = New Integer(,) {{-1, -1}, {1, -1}}
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
            If attacking = True Then
                If player_turn = "w" Then
                    Return white_pawn_kill_translations
                ElseIf player_turn = "b" Then
                    Return black_pawn_kill_translations
                End If
            Else
                If player_turn = "w" Then
                    Return white_pawn_first_move_translations
                ElseIf player_turn = "b" Then
                    Return black_pawn_first_move_translations
                End If
            End If
        End If
    End Function

    Function Translate(ByVal piece_translations As Integer(,), ByVal start_coordinate As Integer(), ByVal end_coordinate As Integer(), Optional ByVal IsKnight As Boolean = False)
        Dim possible_move = New Integer() {0, 0}
        Dim possible_move_LCM = New Integer() {0, 0}
        Dim chess_board As String(,) = Session.Item("chess_board")
        Dim N As Integer = 0

        For index As Integer = 0 To piece_translations.GetLength(0) - 1
            possible_move(0) = start_coordinate(0) + piece_translations(index, 0)
            possible_move(1) = start_coordinate(1) + piece_translations(index, 1)
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
            If index = 23 Then
                Console.WriteLine("hi")
            End If
            If end_coordinate(0) = possible_move(0) And end_coordinate(1) = possible_move(1) Then
                If IsKnight <> True And N <> 1 Then
                    For multiple As Integer = 1 To N - 1
                        If chess_board(start_coordinate(0) + (multiple * possible_move_LCM(0)), start_coordinate(1) + (multiple * possible_move_LCM(1))) <> Nothing Then
                            Return False
                        End If
                    Next
                End If
                Return True
            End If
        Next
        Return False
    End Function

    Protected Sub arrow_left_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton65.Click
        If Session.Item("current_move") <> 0 Then
            ' If its not the first move
            Session("current_move") = Session("current_move") - 1
        Else
            MsgBox(Session.Item("result")) ' Outputs outcome of the game
        End If
        update_board()
    End Sub
    Protected Sub arrow_right_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton66.Click
        If Session.Item("current_move") <> Session.Item("num_of_moves") Then
            ' If its not the last move
            Session("current_move") = Session("current_move") + 1
        Else
            MsgBox(Session.Item("result")) ' Outputs outcome of the game
        End If
        update_board()
    End Sub

    Sub update_board()
        Dim btn As ImageButton
        Dim move_history(,) As String = Session.Item("move_history")
        Dim current_move As Integer = Session.Item("current_move")

        ' Iterate through every button on the chess board
        For i As Integer = 1 To 64
            btn = FindControl("ImageButton" & i)
            If move_history(current_move, i - 1) = Nothing Then
                ' If move history says there is nothing on the square for a particular move turn, then
                ' Set its image url to ""
                btn.ImageUrl = ""
            ElseIf move_history(current_move, i - 1) <> Nothing Then
                ' If move history says there is something on the square for a particular move turn, then
                ' Set its image url to the url stores in move history
                btn.ImageUrl = move_history(current_move, i - 1)
            End If
        Next
    End Sub
End Class