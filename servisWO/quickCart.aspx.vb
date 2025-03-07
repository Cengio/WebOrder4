Imports System.Globalization
Imports DevExpress.Web

Public Class quickCart
    Inherits System.Web.UI.Page
    Private Sub quickCart_Init(sender As Object, e As EventArgs) Handles Me.Init
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
            gridQuickCart.DataBind()
        End If
    End Sub

    Protected Sub gridQuickCart_DataBinding(sender As Object, e As EventArgs) Handles gridQuickCart.DataBinding
        Dim lines As List(Of carrelloLine) = CType(Session("cart"), cartManager).carrelloLinesSorted()

        '20200226 - gestione sconto pagamento su righe
        '20200424 - Gestione scontoScaglioni
        Dim groupedLines = lines.GroupBy(Function(l) New With {Key l.ordineLine.ItemCode, Key l.ordineLine.RowDiscount, Key l.SCONTORIGA, Key l.SCONTOHEADER, Key l.SCONTOPAGAMENTO, Key l.DESCRIZIONE, Key l.ordineLine.UnitPrice}).
                               Select(Function(group) New With {
                                  .ItemCode = group.Key.ItemCode,
                                  .RowDiscount = group.Key.RowDiscount,
                                  .FormulaSconto = getFormulaSconto(group.Key.SCONTORIGA, group.Key.SCONTOHEADER, group.Key.SCONTOPAGAMENTO),
                                  .DESCRIZIONE = group.Key.DESCRIZIONE,
                                  .UnitPrice = group.Key.UnitPrice,
                                  .DiscountQty = group.Sum(Function(c) c.ordineLine.DiscountQty),
                                  .Quantity = group.Sum(Function(c) c.ordineLine.Quantity),
                                  .TOTALERIGA = group.Sum(Function(t) t.TOTALERIGA),
                                  .prezzofinale = getPrezzoFinale(group.Key.UnitPrice, group.Key.SCONTORIGA, group.Key.SCONTOHEADER, group.Key.SCONTOPAGAMENTO) * IIf(group.Key.RowDiscount = 0, 1, 0)
        })
        'Dim groupedLines = lines.GroupBy(Function(l) New With {Key l.ordineLine.ItemCode, Key l.ordineLine.RowDiscount, Key l.ordineLine.FormulaSconto, Key l.DESCRIZIONE, Key l.ordineLine.UnitPrice}).
        '                       Select(Function(group) New With {
        '                          .ItemCode = group.Key.ItemCode,
        '                          .RowDiscount = group.Key.RowDiscount,
        '                          .FormulaSconto = group.Key.FormulaSconto.Replace("-", "+"),
        '                          .DESCRIZIONE = group.Key.DESCRIZIONE,
        '                          .UnitPrice = group.Key.UnitPrice,
        '                          .DiscountQty = group.Sum(Function(c) c.ordineLine.DiscountQty),
        '                          .Quantity = group.Sum(Function(c) c.ordineLine.Quantity),
        '                          .TOTALERIGA = group.Sum(Function(t) t.TOTALERIGA),
        '                          .prezzofinale = getPrezzoFinale(group.Key.UnitPrice, group.Key.FormulaSconto) * IIf(group.Key.RowDiscount = 0, 1, 0)
        '})
        gridQuickCart.DataSource = groupedLines
    End Sub

    Private Function getPrezzoFinale(unitprice As Double, scontoRiga As Integer, scontoHeader As String, scontoPagamento As Integer) As Double
        Dim prezzofinale As Double = unitprice
        If scontoRiga > 0 Then
            prezzofinale = prezzofinale - (prezzofinale / 100 * scontoRiga)
        End If
        If scontoHeader <> "0" And scontoHeader <> "" Then
            Dim scontiH() As String = scontoHeader.ToString.Split("+")
            For i = 0 To scontiH.Count - 1
                If IsNumeric(scontiH(i)) Then
                    prezzofinale = prezzofinale - (prezzofinale / 100 * scontiH(i))
                End If
            Next
        End If
        If scontoPagamento > 0 Then
            prezzofinale = prezzofinale - (prezzofinale / 100 * scontoPagamento)
        End If
        Return prezzofinale
    End Function

    Private Function getFormulaScontoSenzaScontoPagamento(scontoRiga As Integer, scontoHeader As String) As String
        Dim arr() As String
        Dim res As String = ""
        arr = {scontoRiga, scontoHeader}
        res = String.Join("+", arr.Where(Function(x) x <> "0"))
        Return res
    End Function

    Private Function getFormulaSconto(scontoRiga As Integer, scontoHeader As String, scontoPagamento As Integer) As String
        Dim arr() As String
        Dim res As String = ""
        arr = {scontoRiga, scontoHeader, scontoPagamento}
        res = String.Join("+", arr.Where(Function(x) x <> "0"))
        Return res
    End Function

    Protected Sub gridQuickCart_CustomColumnDisplayText(sender As Object, e As ASPxGridViewColumnDisplayTextEventArgs) Handles gridQuickCart.CustomColumnDisplayText
        If e.Column.FieldName = "Quantity" Then
            Dim RowDiscount As Integer = e.GetFieldValue(e.VisibleRowIndex, "RowDiscount")
            Dim DiscountQty As Integer = e.GetFieldValue(e.VisibleRowIndex, "DiscountQty")
            If RowDiscount = 1 Then
                e.DisplayText = DiscountQty
            End If
        ElseIf e.Column.FieldName = "FormulaSconto" Then
            If e.GetFieldValue(e.VisibleRowIndex, "RowDiscount") = 1 Then
                e.DisplayText = "100"
            End If
        End If
    End Sub

    Protected Sub gridQuickCart_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles gridQuickCart.HtmlRowPrepared
        If e.RowType <> GridViewRowType.Data Then
            Return
        End If
        Dim RowDiscount As Integer = e.GetValue("RowDiscount")
        Dim FormulaSconto As String = e.GetValue("FormulaSconto")
        If RowDiscount = 1 Or (IsNumeric(FormulaSconto) AndAlso Convert.ToDecimal(FormulaSconto, CultureInfo.CreateSpecificCulture("en-us")) > 0) Or FormulaSconto.Contains("+") Then
            e.Row.BackColor = System.Drawing.Color.LightGreen
        End If
    End Sub

End Class