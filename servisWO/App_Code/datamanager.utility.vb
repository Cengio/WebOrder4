Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function TestDbConnection(Optional ByVal connname As String = "") As Boolean
        If connname <> "" Then
            Dim conn As SqlConnection = Nothing
            Try
                conn = New SqlConnection(connname)
                conn.Open()
                Return True
            Catch
                Return False
            Finally
                conn.Close()
            End Try
        Else
            Return False
        End If
    End Function

    Public Function GetNumrecords(ByVal nometab As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim sql As String = "Select COUNT (*) FROM " & nometab
            Dim cmd As New SqlCommand(sql, conn)
            Return cmd.ExecuteScalar().ToString()
        Catch
            Throw
        Finally
            conn.Close()
        End Try
    End Function

    Function ToEng(ByVal DateIta)
        'Return DateIta
        DateIta = Split(DateIta, "/")
        'ToEng = DateIta(1) & "/" & DateIta(0) & "/" & DateIta(2)
        ToEng = DateIta(2) & DateIta(1) & DateIta(0)
        Return ToEng
    End Function

    Function cInt2(ByVal strInput) As Integer
        strInput = Trim(strInput)
        If strInput = "" Or strInput Is Nothing Or Not IsNumeric(strInput) Then strInput = "0"
        cInt2 = CInt(strInput)
    End Function

    Function cDbl2(ByVal strInput) As String
        strInput = Trim(strInput)
        strInput = Replace(strInput, ".", ",")
        If strInput = "" Or strInput Is Nothing Or Not IsNumeric(strInput) Then strInput = "0"
        cDbl2 = Replace(CDbl(strInput), ",", ".")
    End Function

    Private Function IntToStr(ByVal Int As Integer) As String
        Return CStr(Int)
    End Function

    Public Function IsValidEmailFormat(ByVal emailAddress As String) As Boolean
        Return Regex.IsMatch(emailAddress, "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")
    End Function

    Public Function EnumToDataTable(ByVal EnumObject As Type, ByVal KeyField As String, ByVal ValueField As String, Optional ByVal itemEscluso As String = "") As DataTable
        Dim oData As DataTable = Nothing
        Dim oRow As DataRow = Nothing
        Dim oColumn As DataColumn = Nothing
        If KeyField.Trim() = String.Empty Then
            KeyField = "KEY"
        End If
        If ValueField.Trim() = String.Empty Then
            ValueField = "VALUE"
        End If
        oData = New DataTable
        oColumn = New DataColumn(KeyField, GetType(System.Int32))
        oData.Columns.Add(KeyField)
        oColumn = New DataColumn(ValueField, GetType(System.String))
        oData.Columns.Add(ValueField)
        For Each iEnumItem As Object In [Enum].GetValues(EnumObject)
            If Not (itemEscluso <> "" AndAlso itemEscluso = iEnumItem.ToString) Then
                oRow = oData.NewRow()
                oRow(KeyField) = CType(iEnumItem, Int32)
                oRow(ValueField) = iEnumItem.ToString()
                oData.Rows.Add(oRow)
            End If
        Next
        Return oData
    End Function

    Public Function GetDatabaseDateTime() As DateTime
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "Select GETDATE()"
            'Dim sql As String = "SELECT CONVERT(smalldatetime, GETDATE())"
            Dim cmd As New SqlCommand(sql, conn)
            Dim d As DateTime = cmd.ExecuteScalar()
            Dim result As DateTime = New Date(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second)
            Return result
        Catch
            Throw
        Finally
            conn.Close()
        End Try
    End Function

End Class
