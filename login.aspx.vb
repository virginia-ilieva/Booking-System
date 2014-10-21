Imports System.Web.Services
Imports System.Web.Script.Services

Public Class login : Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnLogin_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.ServerClick
        '' Check if the user has an account
        If Security.AuthenticateUser(txtUserName.Value, txtPassword.Value) = True Then
            '' Create session variables
            Application.UnLock()
            Security.UserName = txtUserName.Value
            Security.Password = txtPassword.Value
            Security.UserRole = Security.GetUserRole(txtUserName.Value, txtPassword.Value)
            Application.Lock()
            '' Clear the fields
            txtUserName.Value = Nothing
            txtPassword.Value = Nothing
            If Security.UserRole = SecurityRole.Client Then
                '' Navigate to the Client Appointments page
                Response.Redirect("~\clientAppointments.aspx")
            ElseIf Security.UserRole = SecurityRole.Worker Or Security.UserRole = SecurityRole.Admin Or Security.UserRole = SecurityRole.Developer Then
                '' Navigate to the Appointments page
                Response.Redirect("~\appointments.aspx")
                ''Response.Redirect("~\clients.aspx")
            Else
                '' Navigate to the login page
                Response.Redirect("~\login.aspx")
            End If
        Else
            txtUserName.Value = ""
            txtPassword.Value = ""
            txtUserName.Focus()
        End If
    End Sub

    <WebMethod()>
    Public Shared Function logout() As Boolean
        Security.FirstName = ""
        Security.LastName = ""
        Security.Password = ""
        Security.userID = 0
        Security.UserName = ""
        Security.UserRole = 0
        Return True
    End Function

End Class