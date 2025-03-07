Imports DevExpress.Web

Public Class userProfileMng
    Inherits System.Web.UI.Page
    Private Sub userProfileMng_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSusersadmin Then    'utente amministratore
        Else
            Try
                Session.Abandon()
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            grid_Profiles.DataBind()
        End If
    End Sub

    Protected Sub grid_Profiles_DataBinding(sender As Object, e As EventArgs) Handles grid_Profiles.DataBinding
        Dim dm As New datamanager
        grid_Profiles.DataSource = dm.getAllProfilesList(True)
        dm = Nothing
    End Sub

    Protected Sub cbRuoli_DataBinding(sender As Object, e As EventArgs)
        Dim dm As New datamanager
        Dim list As ASPxCheckBoxList = CType(sender, ASPxCheckBoxList)
        Dim ListContainer As GridViewEditItemTemplateContainer = CType(list.NamingContainer, GridViewEditItemTemplateContainer)
        Dim idProfile As Integer = Convert.ToInt32(DataBinder.Eval(ListContainer.DataItem, "idProfile"))
        If idProfile > 0 Then 'edit
            Dim dt As DataTable = dm.getRolesListByIdProfile(idProfile)
            list.Items.Clear()
            For Each r As DataRow In dt.Rows
                list.Items.Add(New ListEditItem With
                               {.Text = r("description"),
                               .Value = r("roleCode"),
                               .Selected = r("allowed")})
            Next
        Else 'new
            list.DataSource = dm.getRolesList
        End If
        dm = Nothing
    End Sub

    Protected Sub grid_Profiles_RowInserting(sender As Object, e As Data.ASPxDataInsertingEventArgs) Handles grid_Profiles.RowInserting
        e.Cancel = True
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim description As String = e.NewValues("description")
        Dim blocked As Integer = e.NewValues("blocked")
        Dim list As ASPxCheckBoxList = CType(grid.FindEditRowCellTemplateControl(CType(grid.Columns("roleList"), GridViewDataColumn), "cbRuoli"), ASPxCheckBoxList)
        Dim roleList As New List(Of userRole)
        Dim dm As New datamanager
        For Each item As ListEditItem In list.Items
            roleList.Add(New userRole With {.roleCode = item.Value, .allowed = IIf(item.Selected, 1, 0)})
        Next
        dm.addProfileRoles(New userProfile With {.description = description, .blocked = blocked, .userRoles = roleList})
        dm = Nothing
        grid_Profiles.CancelEdit()
        grid_Profiles.DataBind()
    End Sub

    Protected Sub grid_Profiles_RowUpdating(sender As Object, e As Data.ASPxDataUpdatingEventArgs) Handles grid_Profiles.RowUpdating
        e.Cancel = True
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim idProfile As Integer = e.Keys("idProfile")
        Dim description As String = e.NewValues("description")
        Dim blocked As Integer = e.NewValues("blocked")
        Dim list As ASPxCheckBoxList = CType(grid.FindEditRowCellTemplateControl(CType(grid.Columns("roleList"), GridViewDataColumn), "cbRuoli"), ASPxCheckBoxList)
        Dim roleList As New List(Of userRole)
        Dim dm As New datamanager
        For Each item As ListEditItem In list.Items
            roleList.Add(New userRole With {.roleCode = item.Value, .allowed = IIf(item.Selected, 1, 0)})
        Next
        dm.updProfileRoles(New userProfile With {.idProfile = idProfile, .description = description, .blocked = blocked, .userRoles = roleList})
        dm = Nothing
        grid_Profiles.CancelEdit()
        grid_Profiles.DataBind()
    End Sub

    Protected Sub grid_Profiles_CellEditorInitialize(sender As Object, e As ASPxGridViewEditorEventArgs) Handles grid_Profiles.CellEditorInitialize
        e.Editor.SetClientSideEventHandler("KeyPress", "OnTextBoxGRIDKeyPress")
    End Sub

    Protected Sub grid_Profiles_RowDeleting(sender As Object, e As Data.ASPxDataDeletingEventArgs) Handles grid_Profiles.RowDeleting
        e.Cancel = True
        Dim dm As New datamanager
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim idProfile As Integer = e.Keys("idProfile")
        If dm.existUsersWithProfile(idProfile) Then
            grid.JSProperties("cpShowPopupProfiloInUso") = True
        Else
            dm.delProfileRoles(idProfile)
        End If
        dm = Nothing
        grid_Profiles.CancelEdit()
        grid_Profiles.DataBind()
    End Sub

    Protected Sub grid_Profiles_Init(sender As Object, e As EventArgs) Handles grid_Profiles.Init
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        gridView.JSProperties("cpShowPopupProfiloInUso") = False
    End Sub


End Class