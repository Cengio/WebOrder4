Public Class carrelloLine
    Implements ICloneable

    Public Sub New()
        ordineLine = New orderLine
    End Sub

    Public Property IDCART As Int64 = 0 '	bigint	Unchecked
    Public Property IDCARTLINE As Int64 = 0 '	bigint	Unchecked
    Public Property IDPROMO As Int64 = 0 'bigint	Unchecked
    Public Property DESCRIZIONE As String = String.Empty
    Public Property DISPONIBILITA As Integer = 0
    Public Property UNITPRICELIST As Double = 0
    Public Property FORMATO As String = String.Empty
    Public Property IVA As String = String.Empty
    Public Property MARCHIO As String = String.Empty
    Public Property COMPOSIZIONE As String = String.Empty
    Public Property TOTALERIGA As Double = 0
    Public Property DISPOLOTTO As Integer = 0
    Public Property ordineLine() As orderLine

    'per magazzino
    Public Property LOADED As Integer = 0
    Public Property LINEIDSOURCE As Integer = 0
    Public Property DATEENTRY As DateTime = New Date(1900, 1, 1)

    '20200226 - gestione sconto pagamento su righe
    Public Property SCONTORIGA As Double = 0
    Public Property SCONTOHEADER As String = "0"
    Public Property SCONTOPAGAMENTO As Double = 0

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return Me.MemberwiseClone
    End Function

End Class
