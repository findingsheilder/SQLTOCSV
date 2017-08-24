Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Partial Class VB
    Inherits System.Web.UI.Page

    Protected Sub ExportCSV(sender As Object, e As EventArgs)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("SELECT * FROM Customers")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)

                        'Build the CSV file data as a Comma separated string.
                        Dim csv As String = String.Empty

                        For Each column As DataColumn In dt.Columns
                            'Add the Header row for CSV file.
                            csv += column.ColumnName + ","c
                        Next

                        'Add new line.
                        csv += vbCr & vbLf

                        For Each row As DataRow In dt.Rows
                            For Each column As DataColumn In dt.Columns
                                'Add the Data rows.
                                csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
                            Next

                            'Add new line.
                            csv += vbCr & vbLf
                        Next

                        'Download the CSV file.
                        Response.Clear()
                        Response.Buffer = True
                        Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv")
                        Response.Charset = ""
                        Response.ContentType = "application/text"
                        Response.Output.Write(csv)
                        Response.Flush()
                        Response.End()
                    End Using
                End Using
            End Using
        End Using
    End Sub
End Class
