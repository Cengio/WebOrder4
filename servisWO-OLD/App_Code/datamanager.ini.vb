Imports System.Data
Imports System.Data.SqlClient


Partial Public Class datamanager

    Private NAVconnectionString As String = [String].Empty
    Private WORconnectionString As String = [String].Empty
    Private workingCompany As String = [String].Empty

    Public Sub New()
        'NAVconnectionString = FileIni.Read(System.Web.HttpContext.Current.Server.MapPath("~//App_Data/servis.ini"), "database_connection", "NAVconnectionString")
        'WORconnectionString = FileIni.Read(System.Web.HttpContext.Current.Server.MapPath("~//App_Data/servis.ini"), "database_connection", "WORconnectionString")
        'workingCompany = FileIni.Read(System.Web.HttpContext.Current.Server.MapPath("App_Data/servis.ini"), "working_company", "companyNAME")
        'spostati i parametri in web.config
        NAVconnectionString = ConfigurationManager.ConnectionStrings("NAVconnectionString").ToString
        WORconnectionString = ConfigurationManager.ConnectionStrings("WORconnectionString").ToString
        'workingCompany = ConfigurationManager.AppSettings("workingCompany")
        If Not IsNothing(HttpContext.Current.Session("wkcManager")) Then
            workingCompany = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).code
        End If


        'locationCode = GetParametroSito("magazzinodefault")
    End Sub

    Public ReadOnly Property GetNAVconnectionString() As String
        Get
            Return NAVconnectionString
        End Get
    End Property

    Public ReadOnly Property GetWORconnectionString() As String
        Get
            Return WORconnectionString
        End Get
    End Property

    Public ReadOnly Property GetWorkingCompany() As String
        Get
            Return workingCompany
        End Get
    End Property

    'Public ReadOnly Property GetlocationCode() As String
    '    Get
    '        Return locationCode
    '    End Get
    'End Property

    Public Function GetCompany(Optional code As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT * FROM workingCompany WHERE status = 1"
            If code <> "" Then
                sql &= " AND code=@code"
            End If
            Dim da As New SqlDataAdapter(sql, conn)
            If code <> "" Then
                da.SelectCommand.Parameters.Add("@code", SqlDbType.VarChar, 30).Value = code
            End If
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

End Class
