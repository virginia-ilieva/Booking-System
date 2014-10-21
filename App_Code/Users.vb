Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class User

#Region "Properties"

    Private _ID As Integer
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Private _Firstname As String
    Public Property Firstname() As String
        Get
            Return _Firstname.Trim
        End Get
        Set(ByVal value As String)
            _Firstname = value
        End Set
    End Property

    Private _Lastname As String
    Public Property Lastname() As String
        Get
            Return _Lastname.Trim
        End Get
        Set(ByVal value As String)
            _Lastname = value
        End Set
    End Property

    Private _Fullname As String
    Public ReadOnly Property Fullname() As String
        Get
            Return Me.Firstname & " " & Me.Lastname
        End Get
    End Property

    Private _Address As String
    Public Property Address() As String
        Get
            Return _Address.Trim
        End Get
        Set(ByVal value As String)
            _Address = value
        End Set
    End Property

    Private _City As String
    Public Property City() As String
        Get
            Return _City.Trim
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Private _State As String
    Public Property State() As String
        Get
            Return _State.Trim
        End Get
        Set(ByVal value As String)
            _State = value
        End Set
    End Property

    Private _Postcode As String
    Public Property Postcode() As String
        Get
            Return _Postcode.Trim
        End Get
        Set(ByVal value As String)
            _Postcode = value
        End Set
    End Property

    Private _ContactName As String
    Public Property ContactName() As String
        Get
            Return _ContactName.Trim
        End Get
        Set(ByVal value As String)
            _ContactName = value
        End Set
    End Property

    Private _ContactRelation As String
    Public Property ContactRelation() As String
        Get
            Return _ContactRelation.Trim
        End Get
        Set(ByVal value As String)
            _ContactRelation = value
        End Set
    End Property

    Private _ContactPhone As String
    Public Property ContactPhone() As String
        Get
            Return _ContactPhone.Trim
        End Get
        Set(ByVal value As String)
            _ContactPhone = value
        End Set
    End Property

    Private _Username As String
    Public Property Username() As String
        Get
            Return _Username.Trim
        End Get
        Set(ByVal value As String)
            _Username = value
        End Set
    End Property

    Private _Password As String
    Public Property Password() As String
        Get
            Return _Password.Trim
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    Private _Phone As String
    Public Property Phone() As String
        Get
            Return _Phone.Trim
        End Get
        Set(ByVal value As String)
            _Phone = value
        End Set
    End Property

    Private _Email As String
    Public Property Email() As String
        Get
            Return _Email.Trim
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property

    Private _DOB As DateTime
    Public Property DOB() As DateTime
        Get
            Return _DOB
        End Get
        Set(ByVal value As DateTime)
            _DOB = value
        End Set
    End Property

    Private _SignupDate As DateTime
    Public Property SignupDate() As DateTime
        Get
            Return _SignupDate
        End Get
        Set(ByVal value As DateTime)
            _SignupDate = value
        End Set
    End Property

    Private _Role As Integer
    Public Property Role() As Integer
        Get
            Return _Role
        End Get
        Set(ByVal value As Integer)
            _Role = value
        End Set
    End Property

    Private _Status As Integer
    Public Property Status() As Integer
        Get
            Return _Status
        End Get
        Set(ByVal value As Integer)
            _Status = value
        End Set
    End Property

    Private _Newsletter As Boolean
    Public Property Newsletter() As Boolean
        Get
            Return _Newsletter
        End Get
        Set(ByVal value As Boolean)
            _Newsletter = value
        End Set
    End Property

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Createa a new empty object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.Reset()
    End Sub

    ''' <summary>
    ''' Creates a new object from the database
    ''' </summary>
    ''' <param name="myID"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal myID As Integer)
        Me.New()
        Me.ID = myID
        Me.Read(myID)
    End Sub

#End Region

#Region "SQL Statements"

    ''' <summary>
    ''' Reads an existing object from the database
    ''' </summary>
    ''' <param name="theID">Integer: the ID of the object</param>
    ''' <remarks></remarks>
    Public Sub Read(ByVal theID As Integer)
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("SELECT * FROM Users WHERE ID = {0}", theID), myConnection)
        Dim myReader As SqlDataReader = Nothing
        myConnection.Open()
        myReader = myCommand.ExecuteReader()
        If myReader.HasRows Then
            myReader.Read()
            Me.Parse(myReader)
        End If
        myConnection.Close()
    End Sub

    ''' <summary>
    ''' Saves the object to the database
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Save()
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim myCommand As New SqlCommand
        Dim myReader As SqlDataReader = Nothing
        If Me.ID > 0 Then
            myCommand.CommandText = String.Format("UPDATE Users SET Firstname = '{0}', Lastname = '{1}', Address = '{2}', City = '{3}', State = '{4}', Postcode = '{5}', ContactName = '{6}', ContactRelation = '{7}', ContactPhone = '{8}', Username = '{9}', Password = '{10}', Phone = '{11}', Email = '{12}', DOB = '{13}', SignupDate = '{14}', Role = {15}, Status = {16}, Newsletter = '{17}' WHERE( ID = {18})", Me.Firstname, Me.Lastname, Me.Address, Me.City, Me.State, Me.Postcode, Me.ContactName, Me.ContactRelation, Me.ContactPhone, Me.Username, Me.Password, Me.Phone, Me.Email, Convert.ToDateTime(Me.DOB).ToString("yyyy-MM-dd"), Convert.ToDateTime(Me.SignupDate).ToString("yyyy-MM-dd"), Me.Role, Me.Status, Me.Newsletter, Me.ID)
        Else
            myCommand.CommandText = String.Format("INSERT INTO Users (Firstname, Lastname, Address, City, State, Postcode, ContactName, ContactRelation, ContactPhone, Username, Password, Phone, Email, DOB, SignupDate, Role, Status, Newsletter) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', {15}, {16}, '{17}')", Me.Firstname, Me.Lastname, Me.Address, Me.City, Me.State, Me.Postcode, Me.ContactName, Me.ContactRelation, Me.ContactPhone, Me.Username, Me.Password, Me.Phone, Me.Email, Convert.ToDateTime(Me.DOB).ToString("yyyy-MM-dd"), Convert.ToDateTime(Me.SignupDate).ToString("yyyy-MM-dd"), Me.Role, Me.Status, Me.Newsletter)
        End If
        myCommand = New SqlCommand(myCommand.CommandText, myConnection)
        myConnection.Open()
        myReader = myCommand.ExecuteReader()
        myConnection.Close()
    End Sub

    ''' <summary>
    ''' Deletes an oblect from the database
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("DELETE FROM Users WHERE ID = {0}", Me.ID), myConnection)
        Dim myReader As SqlDataReader = Nothing
        myConnection.Open()
        myReader = myCommand.ExecuteReader()
        myConnection.Close()
    End Sub

#End Region

#Region "Helpers"

    ''' <summary>
    ''' Sets the default values for the object
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Reset()
        Me.ID = 0
        Me.Firstname = ""
        Me.Lastname = ""
        Me.Address = ""
        Me.City = ""
        Me.State = ""
        Me.Postcode = ""
        Me.ContactName = ""
        Me.ContactRelation = ""
        Me.ContactPhone = ""
        Me.Username = ""
        Me.Password = ""
        Me.Phone = ""
        Me.Email = ""
        Me.DOB = Today
        Me.SignupDate = Today
        Me.Role = 0
        Me.Status = TheStatus.Inactive
        Me.Newsletter = False
    End Sub

    ''' <summary>
    ''' Passes the data from the database to the object's properties
    ''' </summary>
    ''' <param name="dr">SQLDaraReader: holds the data for the object</param>
    ''' <remarks></remarks>
    Private Sub Parse(ByRef dr As SqlDataReader)
        Me.ID = Helper.getIntegerValue(dr, "ID")
        Me.Firstname = Helper.getStringValue(dr, "Firstname")
        Me.Lastname = Helper.getStringValue(dr, "Lastname")
        Me.Address = Helper.getStringValue(dr, "Address")
        Me.City = Helper.getStringValue(dr, "City")
        Me.State = Helper.getStringValue(dr, "State")
        Me.Postcode = Helper.getStringValue(dr, "Postcode")
        Me.ContactName = Helper.getStringValue(dr, "ContactName")
        Me.ContactRelation = Helper.getStringValue(dr, "ContactRelation")
        Me.ContactPhone = Helper.getStringValue(dr, "ContactPhone")
        Me.Username = Helper.getStringValue(dr, "Username")
        Me.Password = Helper.getStringValue(dr, "Password")
        Me.Phone = Helper.getStringValue(dr, "Phone")
        Me.Email = Helper.getStringValue(dr, "Email")
        Me.DOB = Helper.getDateTimeValue(dr, "DOB")
        Me.SignupDate = Helper.getDateTimeValue(dr, "SignupDate")
        Me.Role = Helper.getIntegerValue(dr, "Role")
        Me.Status = Helper.getIntegerValue(dr, "Status")
        Me.Newsletter = Helper.getBooleanValue(dr, "Newsletter")
    End Sub

#End Region

End Class

Public Class Users : Inherits List(Of User)

    Public Sub New()
        MyBase.New()
        Me.Reset()
    End Sub

    Private _OrderBy As String
    Public Property OrderBy() As String
        Get
            Return Me._OrderBy
        End Get
        Set(ByVal value As String)
            Me._OrderBy = value
        End Set
    End Property

    Private _Filter As String
    Public Property Filter() As String
        Get
            Return Me._Filter
        End Get
        Set(ByVal value As String)
            Me._Filter = value
        End Set
    End Property

    Public Overridable Sub Reset()
        Me.Filter = String.Empty
        Me.OrderBy = String.Empty
    End Sub

    Public Function Read()
        Dim myUsers As New Users
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM Users")
        If Not Me.Filter = "" Then
            sql.Append(String.Format(" WHERE {0}", Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY {0}", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "Users")
            Dim row As DataRow
            For Each row In ds.Tables("Users").Rows
                Dim theUser As New User
                theUser.ID = row("ID")
                theUser.Firstname = row("Firstname")
                theUser.Lastname = row("Lastname")
                theUser.Address = Helper.getString(row("Address"))
                theUser.City = Helper.getString(row("City"))
                theUser.State = Helper.getString(row("State"))
                theUser.Postcode = Helper.getString(row("Postcode"))
                theUser.ContactName = Helper.getString(row("ContactName"))
                theUser.ContactRelation = Helper.getString(row("ContactRelation"))
                theUser.ContactPhone = Helper.getString(row("ContactPhone"))
                theUser.Username = row("Username")
                theUser.Password = row("Password")
                theUser.Phone = Helper.getString(row("Phone"))
                theUser.Email = Helper.getString(row("Email"))
                theUser.DOB = Helper.getDate(row("DOB"))
                theUser.SignupDate = Helper.getDate(row("SignupDate"))
                theUser.Role = row("Role")
                theUser.Status = row("Status")
                theUser.Newsletter = row("Newsletter")
                myUsers.Add(theUser)
            Next
        End Using
        Return myUsers
    End Function

End Class