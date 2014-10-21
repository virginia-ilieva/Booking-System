Imports System.Web.Services
Imports System.Web.Script.Services

Public Class clientAccount : Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Security.UserRole = Integer.Parse(SecurityRole.Admin) Or Security.UserRole = Integer.Parse(SecurityRole.Worker) Or Security.UserRole = "" Then
            Security.UserName = Nothing
            Security.Password = Nothing
            Security.UserRole = Nothing
            Response.Redirect("~\login.aspx")
        End If
    End Sub

    ''' <summary>
    ''' Load client details
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadClientDetails() As User
        Dim ClientID As Integer
        ClientID = Security.userID
        Dim myClient As New User(ClientID)
        Try
            Return myClient
        Finally
            If myClient IsNot Nothing Then myClient = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Saves the client to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveClientDetails(ByVal firstname As String, ByVal lastname As String, ByVal address As String, ByVal city As String, ByVal state As String, ByVal postcode As String, ByVal phone As String, ByVal email As String, ByVal username As String, ByVal password As String, ByVal dob As Date, ByVal contactName As String, ByVal contactPhone As String, ByVal relation As String, ByVal newsletter As Boolean) As String
        Dim ClientID = Security.userID
        Dim myClient As New User(ClientID)
            Dim result As String = checkForExistingUser(firstname, lastname, email, username)
            If result = "" Then
                Try
                With myClient
                    .Firstname = firstname
                    .Lastname = lastname
                    .Address = address
                    .City = city
                    .State = state
                    .Postcode = postcode
                    .Phone = phone
                    .Email = email
                    .Username = username
                    .Password = password
                    .DOB = dob
                    .ContactName = contactName
                    .ContactPhone = contactPhone
                    .ContactRelation = relation
                    .Newsletter = newsletter
                    .Save()
                End With
                Finally
                    If myClient IsNot Nothing Then myClient = Nothing
                End Try
            End If
            Return result
    End Function

    ''' <summary>
    ''' Check if a user with the same details already exists in the database
    ''' </summary>
    ''' <param name="firstname">String</param>
    ''' <param name="lastname">String</param>
    ''' <param name="email">String</param>
    ''' <param name="username">String</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Shared Function checkForExistingUser(ByVal firstname As String, ByVal lastname As String, ByVal email As String, ByVal username As String) As String
        Dim result As String = ""
        Dim myUsers As New Users
        Dim theUsers As New List(Of User)
        myUsers.Filter = String.Format("LOWER(Firstname) = '{0}' AND LOWER(Lastname) = '{1}' AND LOWER(Email) = '{2}' AND ID <> {3}", firstname.ToLower, lastname.ToLower, email.ToLower, Security.userID)
        theUsers = myUsers.Read()
        If theUsers.Count > 0 Then
            result = "email"
            Return result
            Exit Function
        End If
        myUsers.Filter = String.Format("LOWER(Username) = '{0}' AND ID <> {1}", username.ToLower, Security.userID)
        theUsers = myUsers.Read()
        If theUsers.Count > 0 Then
            result = "username"
            Return result
            Exit Function
        End If
        Return result
    End Function
End Class