Public Class orderHeader
    Public Property Code() As String = String.Empty
    Public Property CompanyName() As String = String.Empty
    Public Property CustomerNo() As String = String.Empty
    Public Property OrderDate() As String = String.Empty
    Public Property Type() As String = String.Empty
    Public Property ShippingAgentCode() As String = String.Empty
    Public Property ShipAddressCode() As String = String.Empty
    Public Property Notes() As String = String.Empty
    Public Property AttachmentPath() As String = String.Empty
    Public Property OperatorCode() As String = String.Empty
    Public Property PackageNum() As Integer = 0
    Public Property OverpackageNum() As Integer = 0
    Public Property User() As String = String.Empty
    Public Property CompletedImported() As Integer = 0
    Public Property IncludeShipCost() As Integer = 0
    Public Property PaymentMethodCode() As String = String.Empty
    Public Property PaymentTermsCode() As String = String.Empty
    Public Property Weight() As Integer = 0
    Public Property OrderNoCtrl() As String = String.Empty
    Public Property UserCtrl() As String = String.Empty
    Public Property Status() As Integer = 0
    Public Property Voucher_Value As Double = 0
End Class
