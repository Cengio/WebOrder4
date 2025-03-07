Imports DevExpress.Web


Public Class magAssegnaOrdini
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSmagazzinoadmin Then    'utente amministratore
        Else
            Try
                Session.Abandon()
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End If

        If Not Page.IsPostBack Then
            Call caricaMagazzinieri()
            grid.DataBind()
        End If

        Dim dm2 As New datamanager
        Call dm2.verificaCertificazioneOrdiniNuociCliente()
        dm2 = Nothing

        If Not comboMagazzinieri.SelectedItem Is Nothing Then
            Dim codiceMagazziniere As Integer = comboMagazzinieri.SelectedItem.Value
            If codiceMagazziniere = 0 Then
                grid.SettingsDataSecurity.AllowEdit = False
            Else
                grid.SettingsDataSecurity.AllowEdit = True
            End If
        End If
    End Sub

    Private Sub caricaMagazzinieri()
        Dim dm As New datamanager
        comboMagazzinieri.DataSource = dm.getMagazzinieri()
        comboMagazzinieri.ValueField = "Code"
        comboMagazzinieri.TextField = "Description"
        comboMagazzinieri.DataBind()
        comboMagazzinieri.SelectedIndex = 0
        dm = Nothing
    End Sub

    Protected Sub grid_DataBinding(sender As Object, e As EventArgs) Handles grid.DataBinding
        Dim dm As New datamanager
        If Not comboMagazzinieri.SelectedItem Is Nothing Then
            Dim codiceMagazziniere As Integer = comboMagazzinieri.SelectedItem.Value
            grid.DataSource = dm.getOrdiniAssegnati(codiceMagazziniere)
        End If
        dm = Nothing
    End Sub

    Protected Sub btnFiltra_Click(sender As Object, e As EventArgs) Handles btnFiltra.Click
        grid.DataBind()
    End Sub

    Protected Sub grid_BatchUpdate(sender As Object, e As DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs) Handles grid.BatchUpdate
        For Each args In e.UpdateValues
            Call UpdateItem(args.Keys, args.NewValues)
        Next args
        e.Handled = True
    End Sub

    Protected Sub UpdateItem(ByVal keys As OrderedDictionary, ByVal newValues As OrderedDictionary)
        Dim dm As New datamanager
        Dim assOrdine As New assegnazioneOrdine
        assOrdine.ordineCodice = Convert.ToInt32(keys("Code"))
        assOrdine.magazziniereCodice = comboMagazzinieri.SelectedItem.Value
        assOrdine.prioritaRaccolta = newValues("prioritaRaccolta")
        assOrdine.assegnato = newValues("assegnato")
        dm.assegnaOrdine(assOrdine)
        dm = Nothing
    End Sub

    Protected Sub grid_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles grid.CustomColumnDisplayText
        If e.VisibleRowIndex < 0 Then
            Return
        End If
        If e.Column.Name = "ragsociale" Then
            Dim dm As New datamanager
            e.DisplayText = dm.GetCustomerName(e.GetFieldValue("CustomerNo"))
            dm = Nothing
        End If
        If e.Column.FieldName = "assegnato" Then
            If grid.SettingsDataSecurity.AllowEdit = False Then
                If e.Value = 1 Then
                    e.DisplayText = "Si"
                Else
                    e.DisplayText = "No"
                End If
            End If
        End If
        If e.Column.FieldName = "prioritaRaccolta" Then
            If grid.SettingsDataSecurity.AllowEdit = False Then
                If e.Value = 1 Then
                    e.DisplayText = "Si"
                Else
                    e.DisplayText = "No"
                End If
            End If
        End If
        If e.Column.FieldName = "dataAssegnazione" Then
            If e.Value = New Date(1900, 1, 1) Then
                e.DisplayText = ""
            End If
        End If
    End Sub

    Protected Sub grid_HtmlDataCellPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableDataCellEventArgs) Handles grid.HtmlDataCellPrepared
        If e.VisibleIndex >= 0 Then
            If e.DataColumn.FieldName <> "prioritaRaccolta" And e.DataColumn.FieldName <> "assegnato" Then
                e.Cell.Attributes.Add("onclick", "event.cancelBubble = true")
            End If
        End If
    End Sub

    Protected Sub ASPxGridView2_Init(sender As Object, e As EventArgs)
        Dim dm As New datamanager
        Dim gridLines As ASPxGridView = DirectCast(sender, ASPxGridView)
        gridLines.DataSource = dm.getLinesOrdiniMagazzino(gridLines.GetMasterRowKeyValue(), "BinCode")
        gridLines.DataBind()
        dm = Nothing
    End Sub

    Protected Sub ASPxGridView2_CustomColumnDisplayText(sender As Object, e As ASPxGridViewColumnDisplayTextEventArgs)
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

End Class