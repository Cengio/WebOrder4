Imports System.Diagnostics
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports DevExpress.Web
Imports System.Reflection
Imports log4net

Public Class magOrderToPickList
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub magOrderToPickList_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Public Sub woUpdateCollocazioneDaBCPreview()
        Dim conn As SqlConnection = Nothing
        Dim message As New StringBuilder()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("NAVConnectionString").ConnectionString
            conn = New SqlConnection(connString)
            conn.Open()
            Dim cmd As New SqlCommand("woUpdateCollocazioneDaBCPreview", conn) With {
            .CommandType = CommandType.StoredProcedure
        }

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    ' Directly check the number of fields to determine how to process the row
                    If reader.FieldCount = 1 Then
                        ' If there is only one column, append its content directly
                        message.AppendLine(reader.GetString(0))
                    Else
                        ' If there are two or more columns, attempt to read the second column
                        Try
                            Dim itemCode As String = reader.GetString(0)
                            Dim newBinCode As String = "Unknown"
                            If reader.FieldCount > 1 Then
                                newBinCode = reader.GetString(1)
                            End If
                            message.AppendLine($"- L\'articolo: {itemCode} verrà spostato nella collocazione: {newBinCode}")
                        Catch ex As Exception
                            ' Handle any exceptions that occur while attempting to read the second column
                            message.AppendLine("Error processing data: " & ex.Message)
                        End Try
                    End If
                End While
            End Using


            ' Format the message for JavaScript alert/modal
            Dim formattedMessage As String = message.ToString().Replace(Environment.NewLine, "<br>")
            ClientScript.RegisterStartupScript(Me.GetType(), "showModalScript", $"showModal('{formattedMessage}');", True)
        Catch ex As Exception
            Log.Error("Error executing woUpdateCollocazioneDaBCPreview: ", ex)
            ClientScript.RegisterStartupScript(Me.GetType(), "showModalScript", $"showModal('Error: {ex.Message}');", True)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Public Sub woUpdateCollocazioneDaBC()
        Dim conn As SqlConnection = Nothing
        Dim message As New StringBuilder()
        Try
            Dim connString As String = ConfigurationManager.ConnectionStrings("NAVConnectionString").ConnectionString
            conn = New SqlConnection(connString)
            conn.Open()
            Dim cmd As New SqlCommand("woUpdateCollocazioneDaBC", conn) With {
                .CommandType = CommandType.StoredProcedure
            }
            cmd.ExecuteNonQuery() ' Execute the stored procedure
        Catch ex As Exception
            Log.Error("Error executing woUpdateCollocazioneDaBC: ", ex)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Protected Sub woUpdateCollocazioneDaBCPreview_Click(sender As Object, e As EventArgs)
        Try
            woUpdateCollocazioneDaBCPreview()
            ' Optionally, add any logic here to notify the user of success
        Catch ex As Exception
            ' Handle or display the error appropriately
        End Try
    End Sub

    Protected Sub woUpdateCollocazioneDaBC_Click(sender As Object, e As EventArgs)
        Try
            woUpdateCollocazioneDaBC()
            ' Optionally, add any logic here to notify the user of success
        Catch ex As Exception
            ' Handle or display the error appropriately
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (CType(Session("user"), user).iSmagazzinoadmin Or (CType(Session("user"), user).iSmagazzino And Request.QueryString("t") = 3)) Then
            Session.Abandon()
            Response.Redirect("~/err/err403.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End If

        Dim dm2 As New datamanager
        Call dm2.verificaCertificazioneOrdiniNuociCliente()
        dm2 = Nothing

        If Not IsPostBack Then
            Session("magOrderCode") = ""
            Session("orderCodeReport") = ""
            Session("showReportDataUltimaModifica") = ""
            Session("linesOrderByField") = ""
            Session("carrelloMagazzino") = Nothing
            If CType(Session("user"), user).iSmagazzinoadmin Then
                grid.SettingsDetail.ShowDetailButtons = True
                grid.Columns.Item("NumRighe").Visible = True
                Dim magOrderStatus As String = Request.QueryString("t")
                If Not magOrderStatus Is Nothing AndAlso IsNumeric(magOrderStatus) AndAlso CInt(magOrderStatus) = 2 Then
                    grid.Columns.Item("cambiaStato").Visible = True
                Else
                    grid.Columns.Item("cambiaStato").Visible = False
                End If
            Else
                grid.SettingsDetail.ShowDetailButtons = False
                grid.Columns.Item("NumRighe").Visible = False
                grid.Columns.Item("cambiaStato").Visible = False
            End If
            grid.DataBind()
        End If
        grid.JSProperties("cpreporturl") = ""
    End Sub

    Protected Sub grid_Init(sender As Object, e As EventArgs) Handles grid.Init
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        gridView.JSProperties("cpShowConfirmBox") = False
    End Sub

    Protected Sub grid_DataBinding(sender As Object, e As EventArgs) Handles grid.DataBinding
        Dim dm As New datamanager
        Dim magOrderStatus As String = Request.QueryString("t")
        If Not magOrderStatus Is Nothing AndAlso IsNumeric(magOrderStatus) AndAlso CInt(magOrderStatus) < 10 Then
            If CType(Session("user"), user).iSmagazzinoadmin Then
                grid.DataSource = dm.getOrdiniMagazzinoList(magOrderStatus, -1)
            Else
                grid.DataSource = dm.getOrdiniMagazzinoList(magOrderStatus, CType(Session("user"), user).userCode)
            End If
        End If
        dm = Nothing
    End Sub

    Protected Sub ASPxGridView2_Init(sender As Object, e As EventArgs)
        Dim dm As New datamanager
        Dim gridLines As ASPxGridView = DirectCast(sender, ASPxGridView)
        gridLines.DataSource = dm.getLinesOrdiniMagazzino(gridLines.GetMasterRowKeyValue(), "BinCode")
        gridLines.DataBind()
        dm = Nothing
    End Sub

    Protected Sub grid_CustomButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonEventArgs) Handles grid.CustomButtonInitialize
        If e.VisibleIndex = -1 Then
            Return
        End If
        Try
            If e.CellType = GridViewTableCommandCellType.Data Then
                If e.ButtonID = "btnElaboraOrdine" Then
                    Dim Status As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Status")
                    Dim STATUSMAGAZZINO As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "STATUSMAGAZZINO")
                    If Status = 2 Then 'inviato a magazzino
                        e.Enabled = True
                        e.Image.ToolTip = "Raccogli ordine"
                        e.Image.Url = "~/images/magelabora.png"
                    ElseIf Status = 3 Then 'in lavorazione al magazzino
                        e.Enabled = True
                        If STATUSMAGAZZINO = 1 Then 'ordine sospeso durante la raccolta
                            e.Image.ToolTip = "Ordine sospeso. Riattiva raccolta"
                            e.Image.Url = "~/images/magelabora.png"
                        Else
                            If CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "User") = "" Then
                                e.Image.ToolTip = "Ordine già in lavorazione con nessun magazziniere associato"
                                e.Image.Url = "~/images/maginlavorazione.png"
                            Else
                                e.Image.ToolTip = "Ordine già in lavorazione. Cambia assegnazione magazziniere"
                                e.Image.Url = "~/images/maginlavorazione.png"
                            End If
                            If CType(Session("user"), user).iSmagazzinoadmin Then
                                e.Enabled = True
                            Else
                                e.Enabled = False
                                e.Image.ToolTip = "Funzione disabilitata"
                            End If
                        End If
                    ElseIf Status = 5 Then
                        If IsNumeric(Request.QueryString("cf")) AndAlso Request.QueryString("cf") = 1 Then
                            e.Image.ToolTip = "Ordine da controllare e fatturare"
                            e.Image.Url = "~/images/maginlavorazione.png"
                        End If
                    Else
                        e.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub grid_CustomButtonCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs) Handles grid.CustomButtonCallback
        Try
            If e.VisibleIndex = -1 Then
                Return
            End If
            Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
            Dim Code As String = gridView.GetRowValues(e.VisibleIndex, "Code").ToString
            Dim CodeCustomer As String = gridView.GetRowValues(e.VisibleIndex, "CustomerNo").ToString
            Dim Status As Integer = gridView.GetRowValues(e.VisibleIndex, "Status")
            Dim STATUSMAGAZZINO As Integer = gridView.GetRowValues(e.VisibleIndex, "STATUSMAGAZZINO")
            Dim OrdineUser As Integer = 0
            If gridView.GetRowValues(e.VisibleIndex, "User") <> "" AndAlso IsNumeric(gridView.GetRowValues(e.VisibleIndex, "User")) Then
                OrdineUser = gridView.GetRowValues(e.VisibleIndex, "User")
            End If
            gridView.JSProperties("cpreporturl") = ""
            If Code <> "" Then
                Dim dm As New datamanager
                If e.ButtonID = "btnElaboraOrdine" Then
                    If Status = 2 Then
                        Dim carrelloMagazzino As New cartManager
                        Session("carrelloMagazzino") = carrelloMagazzino.pickOrder(Code, CType(Session("user"), user).userCode)
                        Session("currentLineIndex") = 0
                        Log.Info(CType(Session("user"), user).nomeCompleto & " order " & Code & " picked")
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("magPicking.aspx")
                    ElseIf Status = 3 Then
                        If STATUSMAGAZZINO = 1 Then
                            dm.cambiaSospensioneRaccoltaOrdine(Code, 0, CType(Session("user"), user).userCode)
                            grid.DataBind()
                        Else
                            gridView.JSProperties("cpOrderCode") = Code
                            gridView.JSProperties("cpCustomerCode") = CodeCustomer
                            gridView.JSProperties("cpShowConfirmBox") = True
                        End If
                    ElseIf Status = 5 Then
                        If IsNumeric(Request.QueryString("cf")) AndAlso Request.QueryString("cf") = 1 Then 'controllo e fatturazione
                            Dim cm As New cartManager
                            cm = cm.loadFromDBbyOrderCode(CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Code"), True)
                            Dim carrelloMagazzino As New cartManager
                            Session("carrelloMagazzino") = carrelloMagazzino.pickOrderToShip(Code)
                            Session("currentLineIndex") = 0
                            Log.Info(CType(Session("user"), user).nomeCompleto & " order " & Code & " picked for shipping")
                            DevExpress.Web.ASPxWebControl.RedirectOnCallback("magEndPicking.aspx?cf=1")
                        End If
                    End If
                ElseIf e.ButtonID = "btnReport" Then
                    Session("orderCodeReport") = Code
                    Session("linesOrderByField") = "Descrizione"
                    Session("showReportDataUltimaModifica") = "1"
                    gridView.JSProperties("cpreporturl") = "../orderReportTR.aspx"
                ElseIf e.ButtonID = "btnStatoLavorazione" Then
                    dm.pickOrder(Code, "")
                    Log.Info(CType(Session("user"), user).nomeCompleto & " order " & Code & " set Stato In Lavorazione")
                    grid.DataBind()
                End If
                dm = Nothing
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub grid_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles grid.CustomColumnDisplayText
        Dim dm As New datamanager
        If e.Column.FieldName = "Status" Then
            e.DisplayText = orderstatus.getDescription(e.Value, e.GetFieldValue("CompletedImported"))
            If e.Value = 2 Then
                e.DisplayText = "NUOVO"
            ElseIf e.Value = 3 Then
                If e.GetFieldValue("STATUSMAGAZZINO") = 1 Then
                    e.DisplayText = "RACCOLTA SOSPESA"
                End If
                If e.GetFieldValue("User") = "" Then
                    e.DisplayText = "LAVORAZIONE PRENOTATA"
                End If
            ElseIf e.Value = 5 Then
                'e.DisplayText = orderstatus.getDescription(e.Value, e.GetFieldValue("CompletedImported"))
                'If IsNumeric(Request.QueryString("cf")) AndAlso Request.QueryString("cf") = 1 AndAlso e.Value = 3 Then
                '    Dim cm As New cartManager
                '    cm = cm.loadFromDBbyOrderCode(e.GetFieldValue("Code"))
                '    If cm.Header.ordineHeader.Status = 5 Then
                '        e.DisplayText = "COMPLETAMENTE RACCOLTO"
                '    End If
                'End If
            End If
        End If

        If e.Column.FieldName = "User" Or e.Column.FieldName = "OperatorCode" Then
            If IsNumeric(e.Value) Then
                e.DisplayText = dm.getUserNameSurname(e.Value)
            End If
        End If
        If e.Column.Name = "ragsociale" Then
            e.DisplayText = dm.GetCustomerName(e.GetFieldValue("CustomerNo"))
        End If
        dm = Nothing
    End Sub

    Protected Sub grid_HtmlRowCreated(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles grid.HtmlRowCreated
        If e.KeyValue Is Nothing Then
            Return
        End If
        If IsNumeric(Request.QueryString("cf")) AndAlso Request.QueryString("cf") = 1 Then
            If e.GetValue("Status") = 5 Then
                e.Row.Visible = True
            Else
                e.Row.Visible = False
            End If
        End If
    End Sub

    Protected Sub grid_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles grid.HtmlRowPrepared
        If e.GetValue("STATUSMAGAZZINO") = 1 Then
            e.Row.BackColor = Drawing.Color.Orange
            e.Row.ToolTip = "RACCOLTA ORDINE SOSPESA"
        End If
    End Sub

    Protected Sub ASPxGridView2_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs)
        Dim dm As New datamanager
        If e.Column.FieldName = "Quantity" Then
            e.DisplayText = CInt(e.Value)
        End If
        If e.Column.FieldName = "OriginalQty" Then
            Dim LineIdSource As Integer = dm.getLineIdSource(e.GetFieldValue("OrderCode"), e.GetFieldValue("LineID"))
            If LineIdSource > 0 Then
                e.DisplayText = "Riga " & LineIdSource
            End If
        End If
        dm = Nothing
    End Sub

    Protected Sub CallbackChangeOrderUser_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles CallbackChangeOrderUser.Callback
        Dim code As String = e.Parameter.Split("|")(0)
        Dim CodeCustomer As String = e.Parameter.Split("|")(1)
        Dim codiceMagazziniere As Integer = e.Parameter.Split("|")(2)
        If codiceMagazziniere > 0 Then
            Dim dm As New datamanager
            dm.pickOrder(code, codiceMagazziniere) 'cambio valore al campo User senza prendere l'ordine in carico 
            DevExpress.Web.ASPxWebControl.RedirectOnCallback("magOrderToPickList.aspx?t=3&cf=0")
            dm = Nothing
        End If
    End Sub

    Protected Sub callbackMagazzinieri_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles callbackMagazzinieri.Callback
        Dim dm As New datamanager
        comboOperatore.DataSource = dm.getMagazzinieri(False)
        comboOperatore.ValueField = "Code"
        comboOperatore.TextField = "Description"
        comboOperatore.DataBind()
        comboOperatore.SelectedIndex = 0
        dm = Nothing
    End Sub


End Class