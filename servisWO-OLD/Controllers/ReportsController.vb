Imports System.Web
Imports Telerik.Reporting.Services.WebApi
Imports System.IO

'The class name determines the service URL. 
'ReportsController class name defines /api/reports/ service URL.
Public Class ReportsController
    Inherits ReportsControllerBase

    Shared configurationInstance As Telerik.Reporting.Services.ReportServiceConfiguration

    Shared Sub New()
        'This is the folder that contains the XML (trdx) report definitions
        'In this case this is the app folder
        Dim reportPath = HttpContext.Current.Server.MapPath("~/")

        'Add resolver for trdx report definitions, 
        'then add resolver for class report definitions as fallback resolver; 
        'finally create the resolver and use it in the ReportServiceConfiguration instance.
        Dim resolver = New ReportFileResolver(reportPath) _
                       .AddFallbackResolver(New ReportTypeResolver())

        'Setup the ReportServiceConfiguration
        Dim reportServiceConfiguration As New Telerik.Reporting.Services.ReportServiceConfiguration()
        reportServiceConfiguration.HostAppId = "Html5DemoApp"
        reportServiceConfiguration.ReportResolver = resolver
        reportServiceConfiguration.Storage = New Telerik.Reporting.Cache.File.FileStorage()
        ' reportServiceConfiguration.ReportSharingTimeout = 0
        ' reportServiceConfiguration.ClientSessionTimeout = 15
        configurationInstance = reportServiceConfiguration
    End Sub

    Public Sub New()
        'Initialize the service configuration
        Me.ReportServiceConfiguration = configurationInstance
    End Sub
End Class