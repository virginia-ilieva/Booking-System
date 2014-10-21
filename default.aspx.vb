Imports System.Web.Services

Public Class _default : Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getNotifications()
    End Sub

    Private Sub getNotifications()
        Dim theNotifications As New Notifications
        Dim myNotifications As New Notifications
        Try
            theNotifications.OrderBy = "Created"
            theNotifications = myNotifications.Read()
            Me.NotificationsList.DataSource = theNotifications
            Me.NotificationsList.DataBind()
        Finally
            If myNotifications IsNot Nothing Then myNotifications = Nothing
            If theNotifications IsNot Nothing Then theNotifications = Nothing
        End Try
    End Sub

    <WebMethod()>
    Public Shared Function updateMenu() As String
        If Security.UserRole = Nothing Then
            '' Delete all user info if there is anything left
            Security.UserName = Nothing
            Security.Password = Nothing
            Security.UserRole = Nothing
            Return ""
        Else
            Return Security.UserRole
        End If
    End Function
End Class