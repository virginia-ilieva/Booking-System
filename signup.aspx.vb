Imports System.Web.Services
Imports System.Web.Script.Services

Public Class signup : Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <WebMethod()>
    Public Shared Function saveUser(ByVal firstname As String, ByVal lastname As String, ByVal address As String, ByVal city As String, ByVal state As String, ByVal postcode As String, ByVal phone As String, ByVal email As String, ByVal username As String, ByVal password As String, ByVal dob As Date, ByVal contactName As String, ByVal contactPhone As String, ByVal relation As String, ByVal newsletter As Boolean) As String
        Dim result As String = checkForExistingUser(firstname, lastname, email, username)
        If result = "" Then
            Dim theUser As New User
            With theUser
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
                .SignupDate = Today
                .Role = SecurityRole.Client
                .Status = TheStatus.Active
                .Save()
            End With
        End If
        Return result
    End Function

    Public Shared Function checkForExistingUser(ByVal firstname As String, ByVal lastname As String, ByVal email As String, ByVal username As String) As String
        Dim result As String = ""
        Dim myUsers As New Users
        Dim theUsers As New List(Of User)
        myUsers.Filter = String.Format("LOWER(Firstname) = '{0}' AND LOWER(Lastname) = '{1}' AND LOWER(Email) = '{2}'", firstname.ToLower, lastname.ToLower, email.ToLower)
        theUsers = myUsers.Read()
        If theUsers.Count > 0 Then
            result = "email"
            Return result
            Exit Function
        End If
        myUsers.Filter = String.Format("LOWER(Username) = '{0}'", username.ToLower)
        theUsers = myUsers.Read()
        If theUsers.Count > 0 Then
            result = "username"
            Return result
            Exit Function
        End If
        Return result
    End Function
End Class