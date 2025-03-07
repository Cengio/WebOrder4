Public Class unloadlostquantity
    Public Property ID() As Integer = 0                       ' [int] IDENTITY(1,1) NOT NULL,
    Public Property ItemCode() As String = String.Empty       ' [varchar](20) NOT NULL,
    Public Property LostQuantity() As Integer = 0            ' [int] NOT NULL,
    Public Property LotNo() As String = String.Empty         ' [varchar](20) NOT NULL,
    Public Property LocationCode() As String = String.Empty     ' [varchar](10) NOT NULL,
    Public Property InventoryCause() As String = String.Empty     ' [varchar](50) NOT NULL,
    Public Property User() As String = String.Empty            ' [varchar](30) NOT NULL,
    Public Property BinCode() As String = String.Empty          ' [varchar](20) NOT NULL,
    Public Property OrderNo() As String = String.Empty         ' [varchar](20) NOT NULL,
    Public Property RowNo() As String = String.Empty          ' [int] NOT NULL,
    Public Property Ditta() As String = String.Empty           ' [varchar](2) NOT NULL,
    Public Property Close() As Integer = 0                     ' [tinyint] NOT NULL,
    Public Property CloseRestore() As Integer = 0              ' [tinyint] NOT NULL,
    Public Property DateEntry() As DateTime = CDate("1900-01-01")
End Class
