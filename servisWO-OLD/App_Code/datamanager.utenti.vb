Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager


#Region "Gestione autenticazione"


    Public Function authenticate(ByVal user As String, ByVal pass As String) As user
        Dim conn As SqlConnection = Nothing
        Dim u As New user
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter("SELECT * FROM [WU_users] WHERE [User ID]=@webuser AND [Password]=@webuserpassword AND Blocked=0", conn)
            da.SelectCommand.Parameters.Add("@webuser", SqlDbType.VarChar, 50).Value = user
            da.SelectCommand.Parameters.Add("@webuserpassword", SqlDbType.VarChar, 50).Value = pass
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim r As DataRow = ds.Tables(0).Rows(0)
                u.authenticated = True
                u.cognome = r("Cognome")
                u.nome = r("Nome")
                u.email = r("Email")
                u.userID = r("User ID")
                u.userCode = r("User Code")
                u.company = r("Company")
                u.password = r("Password")
                u.blocked = r("Blocked")
                u.note = r("Note")
                u.dateLastLogin = r("Date_LastLogin")
                u.dateCreazione = r("Date_Creazione")
                u.userProfile = getProfileByUserCode(u.userCode)
                If u.userProfile.blocked = 0 Then updUserLastLogin(u.userCode)
            Else
                u.authenticated = False
            End If
            Return u
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return u
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Sub updUserLastLogin(ByVal usercode As Integer)
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [WU_users] SET Date_LastLogin=getdate() WHERE [User Code]=@userCode"
            conn.Open()
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@userCode", SqlDbType.BigInt).Value = usercode
            cmd.ExecuteNonQuery()
            regUserAudit(usercode, userAuditEvent.LOGIN)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Public Sub regUserAudit(ByVal usercode As Integer, ByVal e As userAuditEvent)
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "INSERT INTO [WU_audit]([User Code],[Event Date],[Event]) VALUES (@userCode,getdate(),@event)"
            conn.Open()
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@userCode", SqlDbType.BigInt).Value = usercode
            cmd.Parameters.Add("@event", SqlDbType.VarChar, 50).Value = e.ToString
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Public Function getProfileByUserCode(ByVal userCode As Integer) As userProfile
        Dim conn As New SqlConnection(WORconnectionString)
        Dim p As New userProfile
        Try
            conn.Open()
            Dim dt As New DataTable
            Dim sql As String = "SELECT p.idProfile, p.[description] as ProfileDescription, p.blocked, pr.roleCode,pr.allowed, r.[description] AS roleDescription  "
            sql &= " FROM WU_users u INNER JOIN WU_usersProfiles up On u.[User Code]=up.[User Code] "
            sql &= " INNER JOIN WU_profiles p On up.idProfile=p.idProfile INNER JOIN WU_profilesRoles pr On p.idProfile=pr.idProfile INNER JOIN WU_roles r On pr.roleCode=r.roleCode "
            sql &= " WHERE u.[User Code]=@userCode ORDER BY r.[description]"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@userCode", SqlDbType.BigInt).Value = userCode
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                p.idProfile = dt.Rows(0).Item("idProfile")
                p.description = dt.Rows(0).Item("ProfileDescription")
                p.blocked = dt.Rows(0).Item("blocked")
                For Each r As DataRow In dt.Rows
                    p.userRoles.Add(New userRole With {.roleCode = r("roleCode"), .description = r("roleDescription"), .allowed = r("allowed")})
                Next
            End If
            Return p
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return p
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getIdProfileByUserCode(ByVal userCode As Integer) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim sql As String = "Select [idProfile] FROM [WU_usersProfiles] WHERE [User Code]=@userCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@userCode", SqlDbType.BigInt).Value = userCode
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function isProfileBlocked(ByVal idProfile As Integer) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim sql As String = "Select blocked FROM [WU_profiles] WHERE [idProfile]=@idProfile"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = idProfile
            If cmd.ExecuteScalar = 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

#End Region


#Region "Gestione utenti"

    Public Function getUsersList() As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "Select u.*,up.idProfile,[Password] As confermaPassword,0 As inviaEmail FROM WU_users u left join WU_usersProfiles up On u.[User Code]=up.[User Code] ORDER BY u.[Cognome],u.[Nome]"
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getProfileDescription(ByVal idProfile As Integer) As String
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim sql As String = "Select (Case When blocked=0 Then [description] Else ([description] + ' [blocked]') END) as [description] FROM [WU_profiles] WHERE [idProfile]=@idProfile"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = idProfile
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getProfilesList(Optional ByVal includeBlockedProfile As Boolean = False) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim da As New SqlDataAdapter
            Dim cmd As New SqlCommand
            Dim ds As New DataSet()
            Dim sql As String = "SELECT * FROM [WU_profiles]" & IIf(includeBlockedProfile, "", " WHERE blocked=0")
            cmd.CommandText = sql
            cmd.Connection = conn
            da.SelectCommand = cmd
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function addUser(u As user) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Dim transaction As SqlTransaction = conn.BeginTransaction(IsolationLevel.Serializable)
        Try
            Dim sql As String = "INSERT INTO [WU_users] ("
            sql &= "  [Company]"
            sql &= " ,[User ID]"
            sql &= " ,[Password]"
            sql &= " ,[Nome]"
            sql &= " ,[Cognome]"
            sql &= " ,[Blocked]"
            sql &= " ,[Email]"
            sql &= " ,[Date_LastLogin]"
            sql &= " ,[Date_Creazione]"
            sql &= " ,[Note]"
            sql &= ")"
            sql &= " VALUES ("
            sql &= "@Company,"
            sql &= "@UserID,"
            sql &= "@Password,"
            sql &= "@Nome,"
            sql &= "@Cognome,"
            sql &= "@Blocked,"
            sql &= "@Email,"
            sql &= "@Date_LastLogin,"
            sql &= "GETDATE(),"
            sql &= "@Note"
            sql &= ");"
            sql &= " Select Scope_Identity()"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Transaction = transaction
            cmd.Parameters.Add("@Company", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 50).Value = u.userID
            cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = u.password
            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 50).Value = u.nome
            cmd.Parameters.Add("@Cognome", SqlDbType.VarChar, 50).Value = u.cognome
            cmd.Parameters.Add("@Blocked", SqlDbType.TinyInt).Value = u.blocked
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = u.email
            cmd.Parameters.Add("@Date_LastLogin", SqlDbType.DateTime).Value = u.dateLastLogin
            cmd.Parameters.Add("@Note", SqlDbType.VarChar).Value = u.note
            Dim newuserCode As Integer = cmd.ExecuteScalar

            If u.userProfile.idProfile > 0 Then
                sql = "INSERT INTO WU_usersProfiles([User Code],[idProfile]) VALUES (@userCode, @idProfile)"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@userCode", SqlDbType.BigInt).Value = newuserCode
                cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = u.userProfile.idProfile
                cmd.ExecuteNonQuery()
            End If

            transaction.Commit()
            Return True
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            Return False
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function updUser(u As user) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Dim transaction As SqlTransaction = conn.BeginTransaction(IsolationLevel.Serializable)
        Try
            Dim sql As String = "UPDATE [WU_users] SET "
            sql &= "  [Cognome]=@Cognome"
            sql &= " ,[Nome]=@Nome"
            sql &= " ,[Blocked]=@Blocked"
            sql &= " ,[Email]=@Email"
            sql &= " ,[Note]=@Note"
            sql &= " WHERE [User Code]=@UserCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Transaction = transaction
            cmd.Parameters.Add("@Cognome", SqlDbType.VarChar, 50).Value = u.cognome
            cmd.Parameters.Add("@Nome", SqlDbType.VarChar, 50).Value = u.nome
            cmd.Parameters.Add("@Blocked", SqlDbType.TinyInt).Value = u.blocked
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = u.email
            cmd.Parameters.Add("@Note", SqlDbType.VarChar).Value = u.note
            cmd.Parameters.Add("@UserCode", SqlDbType.BigInt).Value = u.userCode
            cmd.ExecuteNonQuery()

            If u.password <> "" Then
                sql = "UPDATE [WU_users] SET [Password]=@Password WHERE [User Code]=@UserCode"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = u.password
                cmd.Parameters.Add("@UserCode", SqlDbType.BigInt).Value = u.userCode
                cmd.ExecuteNonQuery()
            End If

            sql = "DELETE FROM  WU_usersProfiles WHERE [User Code]=@UserCode"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@userCode", SqlDbType.BigInt).Value = u.userCode
            cmd.ExecuteNonQuery()

            If u.userProfile.idProfile > 0 Then
                sql = "INSERT INTO WU_usersProfiles([User Code],[idProfile]) VALUES (@userCode, @idProfile)"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@userCode", SqlDbType.BigInt).Value = u.userCode
                cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = u.userProfile.idProfile
                cmd.ExecuteNonQuery()
            End If

            transaction.Commit()
            Return True
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            Return False
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function esisteUserID(ByVal UserID As String) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT COUNT(*) FROM [WU_users] WHERE [User ID]=@UserID"
            cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 50).Value = UserID
            cmd.CommandText = sql
            cmd.Connection = conn
            Return CBool(cmd.ExecuteScalar)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function



#End Region


#Region "Gestione Profili - Ruoli utenti"

    Public Function getAllProfilesList(Optional ByVal withRoleStringList As Boolean = False) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [WU_profiles] ORDER BY blocked"
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            If withRoleStringList Then
                ds.Tables(0).Columns.Add("roleStringList", GetType(String))
                For Each r As DataRow In ds.Tables(0).Rows
                    r("roleStringList") = getAllowedRolesStringListByIdProfile(r("idProfile"))
                Next
            End If
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getAllowedRolesStringListByIdProfile(ByVal idProfile As Integer) As String
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim ds As New DataSet
            Dim sql As String = "select [description] from WU_profilesRoles pr inner join WU_roles r on pr.roleCode=r.roleCode  WHERE allowed=1 and idProfile=@idProfile order by [description]"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = idProfile
            da.Fill(ds)
            Dim list As List(Of String) = (From v In ds.Tables(0).AsEnumerable Select v.Field(Of String)("description")).ToList
            Return String.Join(", ", list)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getRolesList() As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [WU_roles] ORDER BY description"
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getRolesListByIdProfile(idProfile As Integer) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim ds As New DataSet
            Dim sql As String = "select pr.idProfile,pr.roleCode,allowed,[description]  from WU_profilesRoles pr inner join WU_roles r on pr.roleCode=r.roleCode WHERE pr.idProfile=@idProfile ORDER BY r.description"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = idProfile
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function addProfileRoles(u As userProfile) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Dim transaction As SqlTransaction = conn.BeginTransaction(IsolationLevel.Serializable)
        Try
            Dim sql As String = "INSERT INTO [WU_profiles] ("
            sql &= "  [description]"
            sql &= " ,[blocked]"
            sql &= ")"
            sql &= " VALUES ("
            sql &= "@description,"
            sql &= "@blocked"
            sql &= ");"
            sql &= " Select Scope_Identity()"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Transaction = transaction
            cmd.Parameters.Add("@description", SqlDbType.VarChar, 256).Value = u.description
            cmd.Parameters.Add("@blocked", SqlDbType.TinyInt).Value = u.blocked
            Dim newIdProfile As Integer = cmd.ExecuteScalar
            For Each ur As userRole In u.userRoles
                sql = "INSERT INTO [WU_profilesRoles] ([idProfile],[roleCode],[allowed]) VALUES (@idProfile,@roleCode,@allowed);"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = newIdProfile
                cmd.Parameters.Add("@roleCode", SqlDbType.VarChar, 50).Value = ur.roleCode
                cmd.Parameters.Add("@allowed", SqlDbType.TinyInt).Value = ur.allowed
                cmd.ExecuteNonQuery()
            Next
            transaction.Commit()
            Return 1
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function updProfileRoles(u As userProfile) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Dim transaction As SqlTransaction = conn.BeginTransaction(IsolationLevel.Serializable)
        Try
            Dim sql As String = "UPDATE [WU_profiles] SET [description]=@description,[blocked]=@blocked WHERE idProfile=@idProfile"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Transaction = transaction
            cmd.Parameters.Add("@description", SqlDbType.VarChar, 256).Value = u.description
            cmd.Parameters.Add("@blocked", SqlDbType.TinyInt).Value = u.blocked
            cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = u.idProfile
            cmd.ExecuteNonQuery()
            For Each ur As userRole In u.userRoles
                sql = "UPDATE [WU_profilesRoles] SET allowed=@allowed WHERE idProfile=@idProfile AND roleCode=@roleCode"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@allowed", SqlDbType.TinyInt).Value = ur.allowed
                cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = u.idProfile
                cmd.Parameters.Add("@roleCode", SqlDbType.VarChar, 50).Value = ur.roleCode
                cmd.ExecuteNonQuery()
            Next
            transaction.Commit()
            Return 1
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function existUsersWithProfile(ByVal idProfile As Integer) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT COUNT(*) FROM [WU_usersProfiles] WHERE [idProfile]=@idProfile"
            cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = idProfile
            cmd.CommandText = sql
            cmd.Connection = conn
            Return CBool(cmd.ExecuteScalar)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function delProfileRoles(ByVal idProfile As Integer) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Dim transaction As SqlTransaction = conn.BeginTransaction(IsolationLevel.Serializable)
        Try
            Dim sql As String = "DELETE FROM [WU_profilesRoles] WHERE idProfile=@idProfile"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Transaction = transaction
            cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = idProfile
            cmd.ExecuteNonQuery()
            sql = "DELETE FROM [WU_profiles] WHERE idProfile=@idProfile"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@idProfile", SqlDbType.BigInt).Value = idProfile
            cmd.ExecuteNonQuery()
            transaction.Commit()
            Return 1
        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function


#End Region


#Region "Funzioni comuni su utenti"

    Public Function esisteUserByCode(ByVal UserCode As Integer) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            If UserCode > 0 Then
                conn.Open()
                Dim cmd As New SqlCommand
                Dim sql As String = "SELECT COUNT(*) FROM [WU_users] WHERE [User Code]=@UserCode"
                cmd.Parameters.Add("@UserCode", SqlDbType.Int).Value = UserCode
                cmd.CommandText = sql
                cmd.Connection = conn
                Return CBool(cmd.ExecuteScalar)
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getUser(Optional ByVal UserCode As Integer = 0) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [WU_users] "
            If UserCode > 0 Then
                sql &= " WHERE [User Code]=@UserCode"
                cmd.Parameters.Add("@UserCode", SqlDbType.BigInt).Value = UserCode
            End If
            sql &= " ORDER BY [Cognome],[Nome]"
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getUserList() As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT [User Code] AS code, Cognome + ' ' + Nome AS description FROM [WU_users] ORDER BY [Cognome],[Nome]"
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getUserNameSurname(ByVal UserCode As Integer) As String
        Dim conn As New SqlConnection(WORconnectionString)
        Dim userNameSurname As String = ""
        Try
            If esisteUserByCode(UserCode) Then
                Dim sql As String = "SELECT Cognome + ' ' + Nome AS userNameSurname FROM [WU_users] WHERE [User Code]=@UserCode"
                conn.Open()
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.Add("@UserCode", SqlDbType.BigInt).Value = UserCode
                Return cmd.ExecuteScalar
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ""
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getAgentiList() As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT u.[User Code] AS code, u.Cognome + ' ' + u.Nome AS [description] FROM wu_users u INNER JOIN WU_usersProfiles up on u.[User Code]=up.[User Code] INNER JOIN WU_profilesRoles pr ON up.idProfile=pr.idProfile WHERE  pr.roleCode='agente'and pr.allowed=1 ORDER BY [description]"
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

#End Region

End Class
