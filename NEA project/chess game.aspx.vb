Public Class chess_game
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub piece_clicked(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click, ImageButton2.Click, ImageButton3.Click, ImageButton4.Click, ImageButton5.Click, ImageButton6.Click, ImageButton7.Click, ImageButton8.Click, ImageButton9.Click, ImageButton10.Click, ImageButton11.Click, ImageButton12.Click, ImageButton13.Click, ImageButton14.Click, ImageButton15.Click, ImageButton16.Click, ImageButton17.Click, ImageButton18.Click, ImageButton19.Click, ImageButton20.Click, ImageButton21.Click, ImageButton22.Click, ImageButton23.Click, ImageButton24.Click, ImageButton25.Click, ImageButton26.Click, ImageButton27.Click, ImageButton28.Click, ImageButton29.Click, ImageButton30.Click, ImageButton31.Click, ImageButton32.Click, ImageButton33.Click, ImageButton34.Click, ImageButton35.Click, ImageButton36.Click, ImageButton37.Click, ImageButton38.Click, ImageButton39.Click, ImageButton40.Click, ImageButton41.Click, ImageButton42.Click, ImageButton43.Click, ImageButton44.Click, ImageButton45.Click, ImageButton46.Click, ImageButton47.Click, ImageButton48.Click, ImageButton49.Click, ImageButton50.Click, ImageButton51.Click, ImageButton52.Click, ImageButton53.Click, ImageButton54.Click, ImageButton55.Click, ImageButton56.Click, ImageButton57.Click, ImageButton58.Click, ImageButton59.Click, ImageButton60.Click, ImageButton61.Click, ImageButton62.Click, ImageButton63.Click, ImageButton64.Click

        Dim ImageButton_clicked As ImageButton = DirectCast(sender, ImageButton)
        Dim Location_clicked As Integer
        Dim TEST As String = sender.ID.Substring(11, 1)
        If sender.ID.length = 12 Then ' If the length of ID is 12, then the position is a one digit number, else its a two digit number. 
            Location_clicked = CInt(sender.ID.Substring(11, 1))
        Else
            Location_clicked = CInt(sender.ID.Substring(11, 2))
        End If

        Dim Positions(7, 7) As String ' DO WE NEED THIS??????

        Dim ImageButton_clicked_ImageURL As String = ImageButton_clicked.ImageUrl

        'Session("chess_piece") = ""
        'If ImageButton_clicked_ImageURL <> "" Then
        '    Session("chess_piece") = ImageButton_clicked_ImageURL.Substring(15, 2)
        'End If

        Positions = Session("positions") ' DO WE NEED THIS??????

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

                capture(ImageButton_clicked)

                If Session.Item("player_turn") = "b" Then ' swaps players turn once a valid capture or normal move is made
                    Session("player_turn") = "w"
                Else
                    Session("player_turn") = "b"
                End If

            End If
        End If
        ' Calculate, using calc_legal_move(), if position to move is a legal move for piece to move
        ' if it is, run capture function which will either kill a piece or move the piece to an empty position
        '        switch turn of player
        ' else
        '        keep turn of player 
    End Sub
    Sub capture(ByVal end_position As ImageButton)
        end_position.ImageUrl = Session.Item("start_URL")
        Dim start_button As ImageButton = FindControl(Session.Item("start_button").ID)
        start_button.ImageUrl = ""
    End Sub

    Function calc_legal_moves(ByVal start_piece As ImageButton, ByVal end_piece As ImageButton, ByVal start_position As Integer, ByVal end_position As Integer)
        Dim end_piece_colour As Char = ""
        Dim start_piece_type As Char = ""

        If end_piece.ImageUrl <> "" Then
            end_piece_colour = end_piece.ImageUrl.Substring(15, 1)
        End If

        If start_piece.ImageUrl <> "" Then
            start_piece_type = start_piece.ImageUrl.Substring(16, 1)
        End If

        Dim start_coordinate = New Integer() {start_position Mod 8, start_position \ 8} ' converts button number into co-ordinate form
        Dim end_coordinate = New Integer() {end_position Mod 8, end_position \ 8}
        Dim knight_translations = New Integer(,) {{1, 2}, {2, 1}, {-1, 2}, {-2, 1}, {2, -1}, {1, -2}, {-2, -1}, {-1, -2}}
        Console.WriteLine(knight_translations(0, 0))

        If end_piece_colour = Session.Item("player_turn") Then ' Player is moving a piece onto a position that has one of their own pieces on it
            Return False
        ElseIf start_piece_type = "" Then ' Player is "attempting" to move an empty space
            Return False
        ElseIf start_piece_type = "N" Then
            For index As Integer = 0 To knight_translations.GetLength(0) - 1
                If end_coordinate(0) = (start_coordinate(0) + knight_translations(index, 0)) And end_coordinate(1) = (start_coordinate(1) + knight_translations(index, 1)) Then
                    Return True
                End If
            Next
        End If

        Return False
    End Function
    Sub init_sender_is_piece_to_move()
        Dim cookieObject6 As New HttpCookie("sender_is_piece_to_move")
        Session("sender_is_piece_to_move") = True

    End Sub




End Class