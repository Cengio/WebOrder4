Imports System.Data
Imports System.Data.SqlClient

Public Class promoManager

    Public Function getPromoRowsByCtCodeItemCode(ByVal ctcode As String, ByVal itemcode As String, Optional ByVal itemcodelot As String = "", Optional ByVal dataOrdine As DateTime = Nothing, Optional ByVal cd As intCanaleDirezione = Nothing) As List(Of promoRow)
        Dim pr As New List(Of promoRow)
        Dim dm As New datamanager
        Dim dt As DataTable = dm.getPromoRowsByCtCodeItemCode(ctcode, itemcode, itemcodelot, dataOrdine, cd)
        For Each r As DataRow In dt.Rows
            Dim desc As String = ""
            If r("QUANTITY_MIN") > 0 Then
                desc &= r("QUANTITY_MIN").ToString
            End If
            If r("QUANTITY_GIFT") > 0 Then
                desc &= IIf(desc <> "", "+", "") & r("QUANTITY_GIFT").ToString
            End If
            If r("DISCOUNT_PERCENT") > 0 Then
                desc &= IIf(desc <> "", "+", "") & r("DISCOUNT_PERCENT").ToString & "%"
            End If
            pr.Add(New promoRow With {.ID = r("ID"),
                                      .PROMO_CODE = r("PROMO_CODE"),
                                      .DESCRIPTION = desc,
                                      .QUANTITY_MIN = r("QUANTITY_MIN"),
                                      .QUANTITY_GIFT = r("QUANTITY_GIFT"),
                                      .DISCOUNT_PERCENT = r("DISCOUNT_PERCENT"),
                                      .ATTIVATA = r("Attivata"),
                                      .INOUT = r("InOut")
            })
            pr.Sort(Function(x, y) x.QUANTITY_MIN.CompareTo(y.QUANTITY_MIN) And x.QUANTITY_GIFT.CompareTo(y.QUANTITY_GIFT))
        Next
        Return pr
        dm = Nothing
    End Function

    Public Function getPromoHeaderByCtCode(ByVal ctcode As String, Optional ByVal dataOrdine As DateTime = Nothing, Optional ByVal cd As intCanaleDirezione = Nothing, Optional ByVal totaleOrdine As Double = -1, Optional ByVal idprofilo As Int64 = 0) As List(Of promoHeader)
        Dim pr As New List(Of promoHeader)
        Dim dm As New datamanager
        Dim dt As DataTable = dm.getPromoHeaderByCtCode(ctcode, dataOrdine, cd, totaleOrdine, idprofilo)
        For Each r As DataRow In dt.Rows
            Dim desc As String = ""
            pr.Add(New promoHeader With {.ID = r("ID"),
                                      .PROMO_CODE = r("PROMO_CODE"),
                                      .CT_CODE = r("CT_CODE"),
                                      .TOTAL_ORDER_MIN = r("TOTAL_ORDER_MIN"),
                                      .TOTAL_ORDER_MAX = r("TOTAL_ORDER_MAX"),
                                      .DISCOUNT_PERCENT = r("DISCOUNT_PERCENT"),
                                      .PROMO_DATA_INIZIO = r("PROMO_DATA_INIZIO"),
                                      .PROMO_DATA_FINE = r("PROMO_DATA_FINE"),
                                      .ATTIVATA = r("Attivata"),
                                      .INOUT = r("InOut"),
                                      .IDPROFILO = r("idprofilo")
            })
        Next
        Return pr
        dm = Nothing
    End Function

    Public Function getCanaleDirezioneByOrderType(ByVal orderType As String) As intCanaleDirezione
        Dim cd As New intCanaleDirezione
        Select Case orderType
            Case "Telefonata in Ingresso"
                cd.IntCanale = canaleInterazione.TELEFONO
                cd.IntDirezione = direzioneInterazione.ENTRATA
            Case "Telefonata in Uscita"
                cd.IntCanale = canaleInterazione.TELEFONO
                cd.IntDirezione = direzioneInterazione.USCITA
            Case "Ordine via Fax"
                cd.IntCanale = canaleInterazione.FAX
                cd.IntDirezione = direzioneInterazione.ENTRATA
            Case "Ordine via Email"
                cd.IntCanale = canaleInterazione.EMAIL
                cd.IntDirezione = direzioneInterazione.ENTRATA
            Case "Visita Agente"
                cd.IntCanale = canaleInterazione.AGENTE
                cd.IntDirezione = direzioneInterazione.USCITA
        End Select
        Return cd
    End Function

    Public Function existsPromoLotto(ByVal ctcode As String, ByVal itemcode As String, Optional ByVal dataOrdine As DateTime = Nothing, Optional ByVal cd As intCanaleDirezione = Nothing) As Boolean
        Dim result As Boolean
        Dim dm As New datamanager
        result = dm.existsPromoLotto(ctcode, itemcode, dataOrdine, cd)
        dm = Nothing
        Return result
    End Function



End Class
