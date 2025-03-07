Imports System.Data
Imports System.Data.SqlClient

Public Class newcustomer
    'Public Property OrderNo() As String = String.Empty              'varchar(20)	
    Public Property CustomerNo() As String = String.Empty           'varchar(20)	
    Public Property Category() As String = String.Empty             'varchar(20)	
    Public Property Name() As String = String.Empty                 'varchar(50)	
    Public Property VATNumber() As String = String.Empty            'varchar(20)	
    Public Property FiscalCode() As String = String.Empty           'varchar(20)	
    Public Property Address() As String = String.Empty              'varchar(50)	
    Public Property AddressNo() As String = String.Empty            'varchar(10)	
    Public Property PostCode() As String = String.Empty             'varchar(20)	
    Public Property City() As String = String.Empty                 'varchar(30)	
    Public Property County() As String = String.Empty               'varchar(30)	
    Public Property Phone1() As String = String.Empty               'varchar(30)	
    Public Property Phone2() As String = String.Empty               'varchar(30)	
    Public Property FaxNo() As String = String.Empty                'varchar(30)	
    Public Property Email() As String = String.Empty                'varchar(80)	
    Public Property CountryCode() As String = String.Empty          'varchar(10)	
    Public Property CD() As String = String.Empty                   'varchar(50)	
    Public Property CIN() As String = String.Empty                  'varchar(25)	
    Public Property ABI() As String = String.Empty                  'varchar(5)	    
    Public Property CAB() As String = String.Empty                  'varchar(5)	    
    Public Property CC() As String = String.Empty                   'varchar(30)	
    Public Property Imported() As Integer = 0                       'tinyint	    
    Public Property Name2() As String = String.Empty                'varchar(50)	
    Public Property Address2() As String = String.Empty             'varchar(50)	
    Public Property StartingDate() As String = String.Empty         'varchar(20)	
    Public Property JustContact() As Integer = 0                    'tinyint	    
    Public Property CreatedBy() As String = String.Empty            'varchar(30)	
    Public Property naturagiuridica() As String = String.Empty      'varchar(50)	
    Public Property ragsocsped1() As String = String.Empty          'varchar(50)	
    Public Property ragsocsped2() As String = String.Empty          'varchar(50)	
    Public Property naturagiuridicasped() As String = String.Empty  'varchar(50)	
    Public Property indirizzosped1() As String = String.Empty       'varchar(50)	
    Public Property indirizzosped2() As String = String.Empty       'varchar(50)	
    Public Property capsped() As String = String.Empty              'varchar(20)	
    Public Property localitasped() As String = String.Empty         'varchar(30)	
    Public Property provinciasped() As String = String.Empty        'varchar(30)	
    Public Property paesesped() As String = String.Empty            'varchar(30)	
    Public Property telefono1sped() As String = String.Empty        'varchar(30)	
    Public Property telefono2sped() As String = String.Empty        'varchar(30)	
    Public Property faxsped() As String = String.Empty              'varchar(30)	
    Public Property emailsped() As String = String.Empty            'varchar(80)	
    Public Property PaymentMethod() As String = String.Empty        'varchar(30)	
    Public Property PaymentTerms() As String = String.Empty         'varchar(30)	
End Class

Public Class naturaGiuridica
    Public Function getList() As DataTable
        Dim dt As New DataTable
        Dim dc As DataColumn = New DataColumn() With {.ColumnName = "Code", .DataType = Type.GetType("System.String")}
        dt.Columns.Add(dc)
        dc = New DataColumn() With {.ColumnName = "Description", .DataType = Type.GetType("System.String")}
        dt.Columns.Add(dc)
        dt.Rows.Add("ND", "Non definito")
        dt.Rows.Add("DITTAINDIVIDUALE", "Ditta individuale")
        dt.Rows.Add("SNC", "S.n.c.")
        dt.Rows.Add("SAS", "S.a.s.")
        dt.Rows.Add("SRL", "S.r.l.")
        dt.Rows.Add("SPA", "S.p.a.")
        dt.Rows.Add("ENTEPUBBLICO", "Ente Pubblico")
        dt.Rows.Add("SCARL", "S.c.a.r.l.")
        dt.Rows.Add("PERSONAFISICA", "Persona fisica")
        Return dt
    End Function
End Class

