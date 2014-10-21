Imports System.Web.Mail
Imports System.Web.Services
Imports System.Web.Script.Services

Public Class admin : Inherits System.Web.UI.Page

    ''' <summary>
    ''' Runs every time the page is loaded
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Security.UserRole = Integer.Parse(SecurityRole.Client) Or Security.UserRole = Integer.Parse(SecurityRole.Worker) Or Security.UserRole = "" Then
            Security.UserName = Nothing
            Security.Password = Nothing
            Security.UserRole = Nothing
            Response.Redirect("~\login.aspx")
        End If
        If Not IsPostBack Then
            getNotifications()
        End If
    End Sub

    ''' <summary>
    ''' Loads the list of notifications
    ''' </summary>
    ''' <remarks></remarks>
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


    ''' <summary>
    ''' Load admin details
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadAdminDetails() As User
        Dim myUsers As New Users
        Dim admin As New Users
        Try
            myUsers.Filter = String.Format("Role = {0}", Integer.Parse(SecurityRole.Admin))
            admin = myUsers.Read()
            If admin.Count = 1 Then
                Return admin.Item(0)
            End If
        Finally
            If myUsers IsNot Nothing Then myUsers = Nothing
            If admin IsNot Nothing Then admin = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Saves the admin details to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveAdminDetails(ByVal adminID As Integer, ByVal firstname As String, ByVal lastname As String, ByVal username As String, ByVal password As String, ByVal phone As String, ByVal email As String) As Boolean
        Dim myWorker As New User
        Try
            If adminID > 0 Then
                myWorker.Read(adminID)
                With myWorker
                    .Firstname = firstname
                    .Lastname = lastname
                    .Username = username
                    .Password = password
                    .Phone = phone
                    .Email = email
                    .Save()
                    Return True
                End With
            Else
                Return False
            End If
        Finally
            If myWorker IsNot Nothing Then myWorker = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Load notification details
    ''' </summary>
    ''' <param name="notificationID">Integer: the ID of the selected notification</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadNotificationDetails(ByVal notificationID As Integer) As Notification
        Dim myNotification As New Notification(notificationID)
        Try
            Return myNotification
        Finally
            If myNotification IsNot Nothing Then myNotification = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Saves the notification to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveNotificationDetails(ByVal notificationID As Integer, ByVal description As String, ByVal send As Boolean) As Boolean
        Dim myNotification As New Notification
        Try
            If notificationID > 0 Then
                myNotification.Read(notificationID)
            Else
                myNotification.Created = Today
            End If
            myNotification.BodyContent = description
            myNotification.Save()
            If send Then
                Dim myAdmins As New Users
                Dim theAdmins As New Users
                myAdmins.Filter = String.Format("Role = {0}", Integer.Parse(SecurityRole.Admin))
                theAdmins = myAdmins.Read()
                Dim myClients As New Users
                Dim theClients As New Users
                myClients.Filter = String.Format("Role = {0} AND Newsletter = '{1}'", Integer.Parse(SecurityRole.Client), "True")
                theClients = myClients.Read()
                For Each theClient As User In theClients
                    'Creates new instance of the mail message class
                    Dim mailMessage As New MailMessage()
                    Dim confirmMessage As New MailMessage()
                    'Add values to the mail message
                    mailMessage.To = theClient.Email
                    mailMessage.From = theAdmins.Item(0).Email.Trim()
                    mailMessage.Subject = "Ayurveda House Notification"
                    mailMessage.Body = "Message:  " & Chr(13) & description
                    'Sends message to the support teem and confirnation to the client
                    SmtpMail.SmtpServer = "127.0.0.1"
                    SmtpMail.Send(mailMessage)
                Next
            End If
            Return True
        Catch ex As Exception
            If myNotification IsNot Nothing Then myNotification = Nothing
        Finally
            If myNotification IsNot Nothing Then myNotification = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Deletes a notification from the database
    ''' </summary>
    ''' <param name="notificationID">Integer: the ID of the selected notification</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function deleteNotification(ByVal notificationID As Integer) As Boolean
        Dim myNotification As New Notification(notificationID)
        Try
            myNotification.Delete()
            Return True
        Finally
            If myNotification IsNot Nothing Then myNotification = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Load Terms Of Use Details
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadTermsOfUseDetails() As TermsOfUse
        Dim myTermsOfUses As New TermsOfUses
        Dim theTermsOfUses As New TermsOfUses
        Try
            theTermsOfUses = myTermsOfUses.Read()
            If theTermsOfUses.Count = 1 Then
                Return theTermsOfUses.Item(0)
            End If
        Finally
            If myTermsOfUses IsNot Nothing Then myTermsOfUses = Nothing
            If theTermsOfUses IsNot Nothing Then theTermsOfUses = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Save Terms Of Use Details
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveTermsOfUseDetails(ByVal termID As Integer, ByVal description As String) As Boolean
        Dim myTermsOfUse As New TermsOfUse(termID)
        Try
            myTermsOfUse.BodyContent = description
            myTermsOfUse.Created = Today
            myTermsOfUse.Save()
        Finally
            If myTermsOfUse IsNot Nothing Then myTermsOfUse = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Sends message describing the bug
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function sendBugReport(ByVal adminID As Integer, ByVal title As String, ByVal priority As Integer, ByVal description As String) As Boolean
        Dim myWorker As New User
        Dim myPriority As String = ""
        Select Case priority
            Case 1
                myPriority = "High"
            Case 2
                myPriority = "Medium"
            Case 3
                myPriority = "Low"
        End Select
        Try
            If adminID > 0 Then
                Dim myAdmins As New Users
                Dim theAdmins As New Users
                myAdmins.Filter = String.Format("Role = {0}", Integer.Parse(SecurityRole.Admin))
                theAdmins = myAdmins.Read()
                myWorker.Read(adminID)
                'Creates new instance of the mail message class
                Dim mailMessage As New MailMessage()
                Dim confirmMessage As New MailMessage()
                'Add values to the mail message
                mailMessage.To = "info@newstyleproject.com"
                mailMessage.From = theAdmins.Item(0).Email
                mailMessage.Subject = "Ayurveda House Support: " & title
                mailMessage.Body = "From:  " & Chr(13) & myWorker.Email & vbCrLf & vbCrLf & "Priority:  " & Chr(13) & myPriority & vbCrLf & vbCrLf & "Message:  " & Chr(13) & description
                'sends message to the support teem and confirnation to the client
                SmtpMail.SmtpServer = "127.0.0.1"
                SmtpMail.Send(mailMessage)
                Return True
            Else
                Return False
            End If
        Finally
            If myWorker IsNot Nothing Then myWorker = Nothing
        End Try
    End Function

    Private Sub btnRefreshNotifications_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefreshNotifications.ServerClick
        ''Security.updateMenu(Me.Master)
        getNotifications()
        Me.upNotifications.DataBind()
    End Sub
End Class