
Public Class workingCompanyManager

    Public Property code As String

    Public Property dittaid As String

    Public Property webHeaderLogo As String
    Public Property reportLogo As String
    Public Property reportFooterDescription As String
    Public Property prefixOrderFileName As String

    Public Property status As Integer

    Public Sub setWorkingCompany(companyCode As String)
        Dim dm As New datamanager
        Dim dt As System.Data.DataTable = dm.GetCompany(companyCode)
        code = dt.Rows(0).Item("code")
        dittaid = dt.Rows(0).Item("dittaid")
        webHeaderLogo = dt.Rows(0).Item("webHeaderLogo")
        reportLogo = dt.Rows(0).Item("reportLogo")
        reportFooterDescription = dt.Rows(0).Item("reportFooterDescription")
        prefixOrderFileName = dt.Rows(0).Item("prefixOrderFileName")
        dm = Nothing
    End Sub

End Class