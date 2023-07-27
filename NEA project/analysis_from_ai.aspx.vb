Public Class analysis_from_ai
    Inherits System.Web.UI.Page
    Public connectAnalysisTable As New DataSet1TableAdapters.analysisTableAdapter

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
        col_heading.Controls.Add(New LiteralControl("Time"))
        title.Cells.Add(col_heading)
        col_heading.Controls.Add(New LiteralControl(" | Winner"))
        title.Cells.Add(col_heading)
        col_heading.Controls.Add(New LiteralControl(" | ELO"))
        title.Cells.Add(col_heading)
        col_heading.Controls.Add(New LiteralControl(" | Blunders"))
        title.Cells.Add(col_heading)
        col_heading.Controls.Add(New LiteralControl(" | Inaccuracies"))
        title.Cells.Add(col_heading)
        Table1.Rows.Add(title)

        ' number of games played by a user against the AI
        If connectAnalysisTable.GetNumOfGamesPlayedByUser(Session.Item("username")) IsNot Nothing Then
            numrows = connectAnalysisTable.GetNumOfGamesPlayedByUser(Session.Item("username"))

            For row = 0 To numrows - 1
                r = New TableRow() ' Create a row
                database_row_record = connectAnalysisTable.GetAnalysisForUser(Session.Item("username"))
                Dim database_row_array = (From column In database_row_record.AsEnumerable() Select column).ToArray()
                c = New TableCell() ' Create a cell

                c.Controls.Add(New LiteralControl(database_row_array(row).Field(Of String)("Time"))) ' Fill a cell with data 
                c.Controls.Add(New LiteralControl(" | "))
                r.Cells.Add(c) ' Add the cell to a row
                c.Controls.Add(New LiteralControl(database_row_array(row).Field(Of String)("Winner")))
                c.Controls.Add(New LiteralControl(" | "))
                r.Cells.Add(c)
                c.Controls.Add(New LiteralControl(database_row_array(row).Field(Of Integer)("ELO")))
                c.Controls.Add(New LiteralControl(" | "))
                r.Cells.Add(c)
                c.Controls.Add(New LiteralControl(database_row_array(row).Field(Of Integer)("Blunders")))
                c.Controls.Add(New LiteralControl(" | "))
                r.Cells.Add(c)
                c.Controls.Add(New LiteralControl(database_row_array(row).Field(Of Integer)("Inaccuracies")))
                r.Cells.Add(c)
                Table1.Rows.Add(r) ' Add the row to the table
            Next row
        End If
    End Sub

End Class