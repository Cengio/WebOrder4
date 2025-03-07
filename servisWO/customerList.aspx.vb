Imports System.Reflection
Imports DevExpress.Web
Imports log4net

Public Class customerList
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub customerList_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If CType(Session("user"), user) IsNot Nothing AndAlso CType(Session("user"), user).isAuthenticated Then
                If Not (CType(Session("user"), user).iSordini) Then
                    ASPxGridView1.Visible = False
                    lb_cTitolo.Visible = False
                End If
            Else
                Session.Abandon()
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        Catch ex As Exception
            Try
                Session.Abandon()
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex2 As Exception
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session("infoCustomerNo") = ""
        End If
    End Sub

    Protected Sub ASPxGridView1_CustomButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonEventArgs) Handles ASPxGridView1.CustomButtonInitialize
        If e.VisibleIndex = -1 Then
            Return
        End If
        Try
            If e.CellType = GridViewTableCommandCellType.Data Then

                Dim Blocked As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Blocked")
                'Dim BlockedDescription As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Description")
                If e.ButtonID = "btnInfoCliente" Then
                    If Blocked = 3 Or Blocked = 2 Or Blocked = 1 Then
                        'BLOCCO ATTIVITA' NEL DETTAGIO CLIENTE
                        'e.Enabled = False
                        'e.Image.ToolTip = "Cliente bloccato: " & BlockedDescription
                        'e.Image.Url = "~/images/user_select_off.png"
                    ElseIf CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine in corso. Chiudere l'attività prima di poter selezionare un altro cliente."
                        e.Image.Url = "~/images/user_select_off.png"
                    End If
                ElseIf e.ButtonID = "btnSelezionaCliente" Then
                    If Blocked = 3 Or Blocked = 2 Or Blocked = 1 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Cliente bloccato"
                        'e.Image.ToolTip = "Cliente bloccato: " & BlockedDescription
                        e.Image.Url = "~/images/orderNEW_off.png"
                    ElseIf CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine in corso. Chiudere l'attività prima di poter selezionare un altro cliente."
                        e.Image.Url = "~/images/orderNEW_off.png"
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
            Dim CustomerNO As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "No_").ToString
            Dim CTNO As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Primary Contact No_").ToString
            If e.ButtonID = "btnSelezionaCliente" Then
                CType(Session("cart"), cartManager).clearCart()
                CType(Session("cart"), cartManager).Header.CODICECLIENTE = CustomerNO
                CType(Session("cart"), cartManager).Header.CODICECONTATTO = CTNO
                CType(Session("cart"), cartManager).loadListino()
                Session("infoCustomerNo") = ""
                Log.Info(CType(Session("user"), user).nomeCompleto & " selected customer: " & CustomerNO & "(" & CTNO & ")")
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderNew.aspx?p=" & Request.QueryString("p"))
            ElseIf e.ButtonID = "btnInfoCliente" Then
                Session("infoCustomerNo") = CustomerNO
                CType(Session("cart"), cartManager).loadListino(CustomerNO)
                Log.Info(CType(Session("user"), user).nomeCompleto & " selected customer: " & CustomerNO & "(" & CTNO & ")")
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/customerDetails.aspx")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub ASPxGridView1_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles ASPxGridView1.HtmlRowPrepared
        If e.RowType <> GridViewRowType.Data Then
            Return
        End If
        Dim Blocked As Integer = e.GetValue("Blocked")
        'Dim BlockedDescription As String = e.GetValue("Description").ToString
        If Blocked = 3 Or Blocked = 2 Or Blocked = 1 Then
            e.Row.BackColor = System.Drawing.Color.FromArgb(255, 90, 90)
            e.Row.ToolTip = "Cliente bloccato"
        Else
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='silver';")
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';")
        End If

    End Sub



End Class