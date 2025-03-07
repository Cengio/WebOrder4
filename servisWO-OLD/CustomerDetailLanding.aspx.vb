Public Class CustomerDetailLanding
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim operatorPhone As String = Request.QueryString("operatorPhone")
        Dim VisitorsIPAddr As String = HttpContext.Current.Request.UserHostAddress
        If Left(VisitorsIPAddr, 11) = "192.168.11." And Request.QueryString("operatorPhone") <> "" Then
            ' TODO: implementare un sistema di bypass del login utente; si potrebbe aggiungere il telefono
        End If

        ' ora precarico il customer selezionato
        Dim CustomerID As String = Request.QueryString("CustomerID")
        If CustomerID <> "" Then
            Session("SelectedCTNO") = ""
            Session("infoCustomerNo") = CustomerID
            Response.Redirect("customerDetails.aspx")
        End If



    End Sub

End Class