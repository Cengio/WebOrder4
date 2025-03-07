Public Class promoHeader
    Public Property ID() As Long = 0
    Public Property PROMO_CODE() As String = String.Empty
    Public Property CT_CODE() As String = String.Empty
    Public Property TOTAL_ORDER_MIN() As Double = 0
    Public Property TOTAL_ORDER_MAX() As Double = 0
    Public Property DISCOUNT_PERCENT() As Integer = 0
    Public Property PROMO_DATA_INIZIO() As DateTime = New Date(1900, 1, 1)
    Public Property PROMO_DATA_FINE() As DateTime = New Date(1900, 1, 1)
    Public Property INOUT() As Integer = 0 '0=IN, 1=OUT, 2=INOUT
    Public Property ATTIVATA() As Boolean = False
    Public Property IDPROFILO() As Int64 = 0
End Class
