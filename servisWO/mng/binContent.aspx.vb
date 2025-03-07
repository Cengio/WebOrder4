Imports DevExpress.Web

Public Class binContent
    Inherits System.Web.UI.Page
    Private Sub binContent_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSwebsiteadmin Then    'utente amministratore
        Else
            Try
                Session.Abandon()
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim dm As New DataManager
            Call dm.synchBinCodeFromNAV()
            dm = Nothing
            gridBinCode.DataBind()
        End If
    End Sub

    Protected Sub gridBinCode_DataBinding(sender As Object, e As EventArgs) Handles gridBinCode.DataBinding
        Dim dm As New DataManager
        gridBinCode.DataSource = dm.GetBinCodeListFromWOR()
        dm = Nothing
    End Sub

    Protected Sub uploadBinImage_FileUploadComplete(sender As Object, e As DevExpress.Web.FileUploadCompleteEventArgs)
        If e.IsValid Then
            e.UploadedFile.SaveAs(MapPath("~/bincodeimages/" & e.UploadedFile.FileName), True)
        End If
    End Sub

    Protected Sub gridBinCode_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gridBinCode.RowUpdating
        Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim uploadBinImage As ASPxUploadControl = TryCast(grid.FindEditRowCellTemplateControl(grid.Columns("ImageBrowse"), "uploadBinImage"), ASPxUploadControl)
        If uploadBinImage.UploadedFiles IsNot Nothing AndAlso uploadBinImage.UploadedFiles.Count > 0 Then
            e.NewValues("ImageFileName") = uploadBinImage.UploadedFiles(0).FileName
        End If
    End Sub


End Class