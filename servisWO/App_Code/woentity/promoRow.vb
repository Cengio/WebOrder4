Public Class promoRow
    Public Property ID() As Long = 0
    Public Property PROMO_CODE() As String = String.Empty
    Public Property CT_CODE() As String = String.Empty
    Public Property ITEM_CODE() As String = String.Empty
    Public Property ITEM_CODE_LOT() As String = String.Empty
    Public Property QUANTITY_MIN() As Integer = 0
    Public Property QUANTITY_GIFT() As Integer = 0
    Public Property DISCOUNT_PERCENT() As Integer = 0
    Public Property PROMO_DATA_INIZIO() As DateTime = New Date(1900, 1, 1)
    Public Property PROMO_DATA_FINE() As DateTime = New Date(1900, 1, 1)
    Public Property DESCRIPTION() As String = String.Empty
    Public Property INOUT() As Integer = 0 '0=IN, 1=OUT, 2=INOUT
    Public Property ATTIVATA() As Boolean = False
End Class
