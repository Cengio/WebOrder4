Public Class carrelloHeader

    Public Enum TIPOCARRELLO
        CARRELLO
        ORDINE
    End Enum

    Public Sub New()
        Me.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO
        ordineHeader = New orderHeader
    End Sub

    Public Property IDCART() As Integer = 0                              'bigint	Unchecked
    Public Property DESCRIPTION() As String = String.Empty                 'varchar(512)	Unchecked
    Public Property CODICECLIENTE As String = String.Empty            '    varchar(30)	Unchecked
    Public Property CODICECONTATTO As String = String.Empty            '    varchar(30)	Unchecked
    Public Property UTENTE_CREAZIONE() As Integer = 0                    'bigint	Unchecked
    Public Property DATA_CREAZIONE() As DateTime = New Date(1900, 1, 1)  'datetime	Unchecked
    Public Property UTENTE_ULTIMA_MODIFICA() As Integer = 0                'bigint	Unchecked
    Public Property AGENTE() As Integer = 0                'bigint	Unchecked
    Public Property DATA_ULTIMA_MODIFICA() As DateTime = New Date(1900, 1, 1) 'datetime	Unchecked
    Public Property BLOCKED() As Integer = 0                                    'bit	Unchecked
    Public Property TIPO() As TIPOCARRELLO
    Public Property SCONTOPAGAMENTO As Double = 0
    Public Property IMPORTOPORTOFRANCO As Double = 0
    Public Property IMPORTOEVADIBILITAORDINE As Double = 0
    Public Property IMPORTOSPESESPEDIZIONE As Double = 0
    Public Property SCONTOHEADER As String = "0"
    Public Property STATUSPRODUZIONE As Integer = 0
    Public Property STATUSMAGAZZINO As Integer = 0 'se = 1 sospeso in magazzino
    Public Property IDPROFILOSCONTO() As Int64 = 0
    Public Property CODICELISTINO As String = String.Empty
    Public Property ORIGINATODAORDINE As String = String.Empty          '    varchar(50)	Unchecked
    Public Property PRENOTAZIONE As Integer = 0
    Public Property DATA_EVASIONE() As DateTime = New Date(1900, 1, 1)

    Public Property ordineHeader() As orderHeader
    Public Function getOrderDate() As DateTime
        If ordineHeader.OrderDate <> "" Then
            Return CDate(Split(ordineHeader.OrderDate, "/")(2) & "-" & Split(ordineHeader.OrderDate, "/")(1) & "-" & Split(ordineHeader.OrderDate, "/")(0))
        Else
            Return Nothing
        End If
    End Function
End Class
