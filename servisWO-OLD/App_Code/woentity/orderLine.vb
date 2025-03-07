Public Class orderLine
    Implements ICloneable
    Public Property OrderCode() As String = String.Empty ' [varchar](20) NOT NULL,
    Public Property ItemCode() As String = String.Empty ' varchar](20) NOT NULL,
    Public Property RowDiscount() As Integer = 0 ' [tinyint] NOT NULL,
    Public Property LineID() As Integer = 0 ' [int] NOT NULL,
    Public Property OldItemCode() As String = String.Empty ' [varchar](20) NOT NULL,
    Public Property CompanyName() As String = String.Empty ' [varchar](30) NOT NULL,
    Public Property UnitPrice As Double ' [decimal](38, 20) NOT NULL,
    Public Property Quantity As Double ' [decimal](38, 20) NOT NULL,
    Public Property DiscountQty() As Integer = 0 ' [int] NOT NULL,
    Public Property Imported() As Integer = 0 ' [tinyint] NOT NULL,
    Public Property BinCode() As String = String.Empty ' [varchar](20) NOT NULL,
    Public Property QtyToShip As Double '  [decimal](38, 20) NOT NULL,
    Public Property LotNo() As String = String.Empty ' [varchar](20) NOT NULL,
    Public Property FormulaSconto() As String = String.Empty ' [varchar](50) NOT NULL,
    Public Property LineNo() As Integer = 0 ' [int] NOT NULL,
    Public Property OriginalQty As Double = 0 ' [decimal](38, 20) NOT NULL,
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return Me.MemberwiseClone
    End Function
End Class
