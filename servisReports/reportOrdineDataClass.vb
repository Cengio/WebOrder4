Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Collections
Imports Microsoft.VisualBasic
Imports System
Imports System.Text


Namespace reportordini

    Public Class orderHeaderReportCollection
        Inherits ArrayList
        Implements ITypedList
        Public Function GetItemProperties(listAccessors() As System.ComponentModel.PropertyDescriptor) As System.ComponentModel.PropertyDescriptorCollection Implements System.ComponentModel.ITypedList.GetItemProperties
            If listAccessors IsNot Nothing AndAlso listAccessors.Length > 0 Then
                Dim listAccessor As PropertyDescriptor = listAccessors(listAccessors.Length - 1)
                If listAccessor.PropertyType.Equals(GetType(orderLinesReportCollection)) Then
                    Return TypeDescriptor.GetProperties(GetType(orderLineReport))
                ElseIf listAccessor.PropertyType.Equals(GetType(quantitaNonDisponibileReportCollection)) Then
                    Return TypeDescriptor.GetProperties(GetType(quantitaNonDisponibileReport))
                End If
            End If
            Return TypeDescriptor.GetProperties(GetType(orderHeaderReport))
        End Function
        Public Function GetListName(listAccessors() As System.ComponentModel.PropertyDescriptor) As String Implements System.ComponentModel.ITypedList.GetListName
            Return "orders"
        End Function
    End Class

    Public Class orderLinesReportComparer
        Implements IComparer
        Private _linesOrderByField As String = ""
        Public Sub New(ByVal linesOrderByField As String)
            _linesOrderByField = linesOrderByField
        End Sub
        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            If _linesOrderByField = "Descrizione" Then
                Return String.Compare(x.Descrizione, y.Descrizione)
            ElseIf _linesOrderByField = "LineID" Then
                Return String.Compare(x.LineID, y.LineID)
            ElseIf _linesOrderByField = "BinCode" Then
                Return String.Compare(x.BinCode, y.BinCode)
            Else
                Return String.Compare(x.LineID, y.LineID)
            End If
        End Function
    End Class

    Public Class orderHeaderReport
        Private orderLines_Renamed As New orderLinesReportCollection()
        Private quantitaNonDisponibili_Renamed As New quantitaNonDisponibileReportCollection()
        Public Property Code() As String = String.Empty
        Public Property CompanyName() As String = String.Empty
        Public Property CustomerNo() As String = String.Empty
        Public Property OrderDate() As String = String.Empty
        Public Property DATA_ULTIMA_MODIFICA() As String = String.Empty
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
        '2:completo -> puoi importarlo
        '0:salvato
        '1:sospeso
        '3:in lavorazione (in carico al magazziniere)
        '4:spedito
        Public Property nomecliente() As String = String.Empty
        Public Property indirizzocliente() As String = String.Empty
        Public Property cittacliente() As String = String.Empty
        Public Property provinciacliente As String = String.Empty
        Public Property nomespedizione() As String = String.Empty
        Public Property indirizzospedizione() As String = String.Empty
        Public Property cittaspedizione() As String = String.Empty
        Public Property provinciaspedizione() As String = String.Empty
        Public Property metododipagamento() As String = String.Empty
        Public Property terminedipagamento() As String = String.Empty
        Public Property spedizioniere As String = String.Empty
        Public Property capcliente() As String = String.Empty
        Public Property capspedizione() As String = String.Empty
        Public Property telefono() As String = String.Empty
        Public Property fax() As String = String.Empty
        Public Property cellulare() As String = String.Empty
        Public Property categoriaMerceologica() As String = String.Empty
        Public Property totaleIVA() As String = String.Empty
        Public Property totaleIMPONIBILE() As String = String.Empty
        Public Property totaleIMPONIBILEnetto() As String = String.Empty
        Public Property totaleOrdine() As String = String.Empty
        Public Property scontoPagamentoPerc() As String = String.Empty
        Public Property scontoTotale() As String = String.Empty
        Public Property speseTrasporto() As String = String.Empty
        Public Property speseIncasso() As String = String.Empty
        Public Property QRcode() As String = CompanyName & "-" & Code & "-" & CustomerNo
        Public Property valoreVoucher() As String = String.Empty
        Public Property PRENOTAZIONE As Integer = 0
        Public Property DATA_EVASIONE() As String = String.Empty
        Public Property quantitaNonDisponibiliCount As Integer = 0

        Public ReadOnly Property orderLines() As orderLinesReportCollection
            Get
                Return orderLines_Renamed
            End Get
        End Property

        Public Sub Add(ByVal orderline As orderLineReport)
            orderLines_Renamed.Add(orderline)
        End Sub

        Public ReadOnly Property quantitaNonDisponibili() As quantitaNonDisponibileReportCollection
            Get
                Return quantitaNonDisponibili_Renamed
            End Get
        End Property

        Public Sub Add(ByVal quantitaNonDisponibile As quantitaNonDisponibileReport)
            quantitaNonDisponibili_Renamed.Add(quantitaNonDisponibile)
        End Sub

        Public Property reportLogo As String
        Public Property reportFooterDescription As String

        Public Sub New(ByVal codordine As String)
            Me.Code = codordine
        End Sub

    End Class

    Public Class orderLinesReportCollection
        Inherits ArrayList
        Implements ITypedList
        Public Function GetItemProperties(listAccessors() As System.ComponentModel.PropertyDescriptor) As System.ComponentModel.PropertyDescriptorCollection Implements System.ComponentModel.ITypedList.GetItemProperties
            Return TypeDescriptor.GetProperties(GetType(orderLineReport))
        End Function
        Public Function GetListName(listAccessors() As System.ComponentModel.PropertyDescriptor) As String Implements System.ComponentModel.ITypedList.GetListName
            Return "orderLines"
        End Function
    End Class

    Public Class orderLineReport
        Public Property OrderCode() As String = String.Empty
        Public Property ItemCode() As String = String.Empty
        Public Property RowDiscount() As Integer = 0
        Public Property LineID() As Integer = 0
        Public Property OldItemCode() As String = String.Empty
        Public Property CompanyName() As String = String.Empty
        Public Property UnitPrice As String
        Public Property Quantity As String
        Public Property DiscountQty() As Integer = 0 '
        Public Property Imported() As Integer = 0
        Public Property BinCode() As String = String.Empty
        Public Property QtyToShip As Double
        Public Property LotNo() As String = String.Empty
        Public Property FormulaSconto() As String = String.Empty
        Public Property LineNo() As Integer = 0
        Public Property Farmadati() As String = String.Empty
        Public Property Descrizione() As String = String.Empty
        Public Property Formato() As String = String.Empty
        Public Property IVA() As Integer = 0
        Public Property ScontoRiga() As String = String.Empty
        Public Property TotaleRiga() As String = String.Empty
        Public Sub New(ByVal lineid As Integer)
            Me.LineID = lineid
        End Sub
    End Class

    Public Class quantitaNonDisponibileReportCollection
        Inherits ArrayList
        Implements ITypedList
        Public Function GetItemProperties(listAccessors() As System.ComponentModel.PropertyDescriptor) As System.ComponentModel.PropertyDescriptorCollection Implements System.ComponentModel.ITypedList.GetItemProperties
            Return TypeDescriptor.GetProperties(GetType(quantitaNonDisponibileReport))
        End Function
        Public Function GetListName(listAccessors() As System.ComponentModel.PropertyDescriptor) As String Implements System.ComponentModel.ITypedList.GetListName
            Return "quantitaNonDisponibili"
        End Function
    End Class

    Public Class quantitaNonDisponibileReport
        Public Property orderCode() As String = String.Empty
        Public Property idCarrello() As Int64 = 0
        Public Property itemCode() As String = String.Empty
        Public Property Descrizione() As String = String.Empty
        Public Property quantity() As Double = 0
    End Class

End Namespace