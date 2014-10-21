'' This class was written for Ayurveda House by Verzhiniya Ilieva - January 2010

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

Public Class Security

    '' Global variables declaration
    Public Shared userID As Integer
    Public Shared FirstName As String = Nothing
    Public Shared LastName As String = Nothing
    Public Shared UserName As String = Nothing
    Public Shared Password As String = Nothing
    Public Shared UserRole As String = Nothing

    Public Shared Function AuthenticateUser(ByVal userName As String, ByVal password As String) As Boolean

        '' Declarations  
        Dim MyDataAdapter As SqlClient.SqlDataAdapter
        Dim MyDataTable As New DataTable
        Dim MyDataRow As DataRow
        Dim strSQL As String = "SELECT ID, UserName, Password FROM Users WHERE Status = 1"

        '' Connect and fill the DataTable   
        Dim MyConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        MyConnection.Open()
        MyDataAdapter = New SqlClient.SqlDataAdapter(strSQL, MyConnection)
        MyDataAdapter.Fill(MyDataTable)

        '' Loop through DataTable   
        For Each MyDataRow In MyDataTable.Rows
            If LCase(Trim(MyDataRow.Item("UserName"))) = LCase(Trim(userName)) And Trim(MyDataRow.Item("Password")) = Trim(password) Then
                MyConnection.Close()
                Security.userID = Trim(MyDataRow.Item("ID"))
                Return True
                Exit Function
            End If
        Next

        '' If the user was not found close the connection and show error message
        MyConnection.Close()
        Return False

    End Function

    '' Finds the user in the database and pulls his role
    Public Shared Function GetUserRole(ByVal userName As String, ByVal password As String) As String

        '' Declarations   
        Dim role As String

        Dim MyDataAdapter As SqlClient.SqlDataAdapter
        Dim MyDataTable As New DataTable
        Dim MyDataRow As DataRow
        Dim strSQL As String = "SELECT UserName, Password, Role FROM Users"

        '' Connect and fill the DataTable   
        Dim MyConnection As New SqlConnection
        MyConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        MyConnection.Open()
        MyDataAdapter = New SqlClient.SqlDataAdapter(strSQL, MyConnection)
        MyDataAdapter.Fill(MyDataTable)

        Try
            '' Loop through DataTable   
            For Each MyDataRow In MyDataTable.Rows
                If LCase(Trim(MyDataRow.Item("UserName"))) = LCase(Trim(userName)) And Trim(MyDataRow.Item("Password")) = Trim(password) Then
                    role = Trim(MyDataRow.Item("Role"))
                    Return role
                    Exit Function
                End If
            Next

            '' If the user was not found close the connection and show error message
            role = Nothing
            Return role

        Catch ex As Exception
            Throw
        Finally
            MyConnection.Close()
        End Try

    End Function

End Class
