Imports DevExpress.Web

Public Class newcustomerList
    Inherits System.Web.UI.Page
    Private Sub newcustomerList_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSordini Then    'utente amministratore
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
            Session("selectedNewCustomer") = ""
        End If
    End Sub

    Protected Sub ASPxGridView1_HtmlRowCreated(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles ASPxGridView1.HtmlRowCreated
        If e.VisibleIndex = -1 Then
            Return
        End If
        Dim CustomerNo As String = e.KeyValue
        Dim dm As New DataManager
        Dim newC As DataTable = dm.GetNewCustomersFromNAV(CustomerNo)
        If newC.Rows.Count > 0 Then ' è stato creato un ordine e verifico se è stato certificato, in tal caso lo nascondo dalla lista nuovi clienti
            If newC.Rows(0).Item("Imported") = 1 Or newC.Rows(0).Item("JustContact") = 1 Then
                e.Row.Visible = False
            End If
        End If
        dm = Nothing
    End Sub

    Protected Sub ASPxGridView1_CustomButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonEventArgs) Handles ASPxGridView1.CustomButtonInitialize
        If e.VisibleIndex = -1 Then
            Return
        End If
        Try
            If e.CellType = GridViewTableCommandCellType.Data Then
                If e.ButtonID = "customerDetails" Then
                    Dim CustomerNo As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CustomerNo").ToString
                    Dim Imported As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Imported")
                    If Imported = 1 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Cliente non modificabile in quanto esiste già ordine associato"
                        e.Image.Url = "~/images/user_select_off.png"
                    End If
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub ASPxGridView1_CustomButtonCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs) Handles ASPxGridView1.CustomButtonCallback
        If e.VisibleIndex = -1 Then
            Return
        End If
        Try
            Dim CustomerNO As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CustomerNo").ToString
            If e.ButtonID = "customerDetails" Then
                Session("selectedNewCustomer") = CustomerNO
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/newcustomerAdd.aspx")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


    Private Sub ASPxGridView1_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles ASPxGridView1.HtmlRowPrepared
        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='silver';")
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';")
    End Sub


End Class