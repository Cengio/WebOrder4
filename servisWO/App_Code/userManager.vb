
Public Enum userAuditEvent
    LOGIN
    LOGOUT
End Enum

Public Class userProfile
    Public Property idProfile As Integer = 0
    Public Property description As String = String.Empty
    Public Property blocked As Integer = 1
    Public Property userRoles As List(Of userRole) = New List(Of userRole)
End Class


Public Class userRole
    Public Property roleCode As String = String.Empty
    Public Property description As String = String.Empty
    Public Property allowed As Integer = 0
End Class


Public Class user

    Public Property userCode() As Integer = 0
    Public Property company() As String = String.Empty
    Public Property userID As String = String.Empty
    Public Property password() As String = String.Empty
    Public Property nome() As String = String.Empty
    Public Property cognome() As String = String.Empty
    Public Property blocked() As Integer = 0
    Public Property email() As String = String.Empty
    Public Property dateLastLogin() As DateTime = New Date(1900, 1, 1)
    Public Property dateCreazione() As DateTime = New Date(1900, 1, 1)
    Public Property note() As String = String.Empty
    Public Property authenticated As Boolean = False
    Public Property userProfile() As userProfile = New userProfile
    Public ReadOnly Property nomeCompleto As String
        Get
            Return (cognome & " " & nome)
        End Get
    End Property
    Public ReadOnly Property isAuthenticated As Boolean
        Get
            Return (authenticated And userProfile.blocked = 0)
        End Get
    End Property

    'check sui ruoli utente
    Public ReadOnly Property iSusersadmin As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "usersadmin" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSwebsiteadmin As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "websiteadmin" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSzeroprice As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "zeroprice" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSpromorigacombo As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "promorigacombo" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSpromorigascomerce As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "promorigascomerce" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSpromorigascoperc As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "promorigascoperc" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSordini As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "ordini" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iScercadimensioni As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "cercadimensioni" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSnorevisioneordine As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "norevisioneordine" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSrevisoreordini As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "revisoreordini" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSmagazzino As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "magazzino" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSmagazzinoadmin As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "magazzinoadmin" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSmagazzinoONLY As Boolean
        Get
            If (iSmagazzino Or iSmagazzinoadmin) And Not (iSordini Or iSusersadmin Or iSwebsiteadmin Or iSproduzione) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property iSagente As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "agente" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSproduzione As Boolean
        Get
            Return (From ur In userProfile.userRoles Where ur.roleCode = "produzione" And ur.allowed = 1 Select ur).Any()
        End Get
    End Property
    Public ReadOnly Property iSproduzioneONLY As Boolean
        Get
            If (iSproduzione) And Not (iSordini Or iSusersadmin Or iSwebsiteadmin Or iSmagazzino Or iSmagazzinoadmin) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
End Class


