Imports System.Diagnostics
Public Class lean_from_an_ai
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Public Function GetBestMove(fen As String) As String
    '    Dim process As New Process()
    '    process.StartInfo.FileName = "C:/Users/namjo/OneDrive/Documents/Namjote.S.Dulay/A-levels/Computer_Science/Year 13/NEA Namjote Dulay/NEA project/NEA project/stockfish_15.1_win_x64_avx2/stockfish-windows-2022-x86-64-avx2.exe"
    '    process.StartInfo.Arguments = "position fen " & fen & vbCrLf & "go depth 20" ' Change the depth as per your requirement
    '    process.StartInfo.UseShellExecute = False
    '    process.StartInfo.RedirectStandardOutput = True
    '    process.StartInfo.CreateNoWindow = True
    '    process.Start()
    '    Dim output As String = process.StandardOutput.ReadToEnd()
    '    process.WaitForExit()
    '    If process.ExitCode <> 0 Then
    '        Throw New Exception("Stockfish exited with error code " & process.ExitCode)
    '    End If
    '    Dim bestMove As String = Regex.Match(output, "(?<=bestmove\s)[a-h][1-8][a-h][1-8]").Value
    '    If String.IsNullOrEmpty(bestMove) Then
    '        Throw New Exception("Failed to parse best move from output: " & vbCrLf & output)
    '    End If
    '    Return bestMove
    'End Function

    'Protected Sub CalculateBestMove_Click(sender As Object, e As EventArgs) Handles CalculateBestMove.Click
    '    Dim fen As String = "4k2r/6r1/8/8/8/8/3R4/R3K3 w Qk - 0 1" ' Example FEN string
    '    Dim bestMove As String = GetBestMove(fen)
    '    MsgBox("Best move: " & bestMove)
    'End Sub

    'Protected Sub CalculateBestMove_Click(sender As Object, e As EventArgs) Handles CalculateBestMove.Click
    '    Dim fen As String = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1" ' Example FEN string
    '    Dim script As String = "let engine = new Worker('C:/Users/namjo\OneDrive\Documents\Namjote.S.Dulay\A-levels\Computer_Science\Year 13\NEA Namjote Dulay\NEA project\NEA project\Stockfish js\stockfish.js'); " &
    '                           "engine.onmessage = function(event) { " &
    '                           "  let bestMove = event.data.split(' ')[1]; " &
    '                           "  document.getElementById('ResultLabel').innerText = 'Best move: ' + bestMove; " &
    '                           "}; " &
    '                           "engine.postMessage('position fen " & fen & "'); " &
    '                           "engine.postMessage('go depth 20'); "
    '    ClientScript.RegisterStartupScript(Me.GetType(), "CalculateBestMoveScript", script, True)
    'End Sub



End Class