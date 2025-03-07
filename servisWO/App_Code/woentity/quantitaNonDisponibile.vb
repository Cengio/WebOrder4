Public Class quantitaNonDisponibile
    Public Property id() As Int64 = 0
    Public Property company() As String = String.Empty
    Public Property customerNo As String = String.Empty
    Public Property orderCode() As String = String.Empty
    Public Property idCarrello() As Int64 = 0
    Public Property itemCode() As String = String.Empty
    Public Property quantity() As Double = 0
    Public Property dateAdd() As DateTime = New Date(1900, 1, 1)
    Public Property alert() As Integer = 0
End Class
