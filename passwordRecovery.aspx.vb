Imports System.Net.Mail
Imports System.Web.Services
Imports System.Web.Script.Services

Public Class passwordRecovery : Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    <WebMethod()>
    Public Shared Function FindUserAndSendEmail(ByVal firstName As String, ByVal lastName As String, ByVal email As String) As Boolean
        Dim result As Boolean = False
        Dim theUsers As New List(Of User)
        Dim myUsers As New Users
        myUsers.Filter = String.Format("Firstname = '{0}' AND Lastname = '{1}' AND Email = '{2}'", firstName, lastName, email)
        theUsers = myUsers.Read()
        If theUsers.Count > 0 Then
            For i As Integer = 0 To theUsers.Count - 1
                passwordRecovery.SendEmail(theUsers.Item(i).Username, theUsers.Item(i).Password, theUsers.Item(i).Email, theUsers.Item(i).Firstname)
                result = True
            Next
        End If
        Return result
    End Function

    Public Shared Sub SendEmail(ByVal userName As String, ByVal password As String, ByVal email As String, ByVal firstName As String)

        'Creates new instance of the mail message class
        Dim mailMessage As MailMessage = New MailMessage()

        'Add values to the mail message
        mailMessage.IsBodyHtml = True
        mailMessage.From = New MailAddress("virginia_ilieva@yahoo.com")
        mailMessage.To.Add(New MailAddress(email))
        mailMessage.Subject = "Login details"
        mailMessage.Body = "Hello " & firstName & ".<br /><br />Thank you for submiting your information. Please, find your login details below:<br /><br />Username - " & userName & "<br />Password - " & password & "<br /><br />If you want to login now, please <a href=""http://localhost:1032/login.aspx"">click here.</a><br /><br />Kind regards,<br /><br />Ayurveda House"

        Try
            'Sends message to the client
            Dim smtpClient As SmtpClient = New SmtpClient("smtp.three.com.au")
            smtpClient.Send(mailMessage)
        Catch ex As Exception

        End Try

    End Sub
End Class