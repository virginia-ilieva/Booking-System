Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class Clinic

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

    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _Address As String
    Public Property Address() As String
        Get
            Return _Address
        End Get
        Set(ByVal value As String)
            _Address = value
        End Set
    End Property

    Private _City As String
    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Private _State As String
    Public Property State() As String
        Get
            Return _State
        End Get
        Set(ByVal value As String)
            _State = value
        End Set
    End Property

    Private _Postcode As String
    Public Property Postcode() As String
        Get
            Return _Postcode
        End Get
        Set(ByVal value As String)
            _Postcode = value
        End Set
    End Property

    Private _Phone As String
    Public Property Phone() As String
        Get
            Return _Phone
        End Get
        Set(ByVal value As String)
            _Phone = value
        End Set
    End Property

    Private _Fax As String
    Public Property Fax() As String
        Get
            Return _Fax
        End Get
        Set(ByVal value As String)
            _Fax = value
        End Set
    End Property

    Private _Email As String
    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("SELECT * FROM Clinics WHERE ID = {0}", theID), myConnection)
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
            myCommand.CommandText = String.Format("UPDATE Clinics SET Name = '{0}', Address = '{1}', City = '{2}', State = '{3}', Postcode = '{4}', Phone = '{5}', Fax = '{6}', Email = '{7}', Status = {8} WHERE( ID = {9} )", Me.Name, Me.Address, Me.City, Me.State, Me.Postcode, Me.Phone, Me.Fax, Me.Email, Me.Status, Me.ID)
        Else
            myCommand.CommandText = String.Format("INSERT INTO Clinics (Name, Address, City, State, Postcode, Phone, Fax, Email, Status) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8})", Me.Name, Me.Address, Me.City, Me.State, Me.Postcode, Me.Phone, Me.Fax, Me.Email, Me.Status)
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("DELETE FROM Clinics WHERE ID = {0}", Me.ID), myConnection)
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
        Me.Name = ""
        Me.Address = ""
        Me.City = ""
        Me.State = ""
        Me.Postcode = ""
        Me.Phone = ""
        Me.Fax = ""
        Me.Email = ""
        Me.Status = TheStatus.Inactive
    End Sub

    ''' <summary>
    ''' Passes the data from the database to the object's properties
    ''' </summary>
    ''' <param name="dr">SQLDaraReader: holds the data for the object</param>
    ''' <remarks></remarks>
    Private Sub Parse(ByRef dr As SqlDataReader)
        Me.ID = Helper.getIntegerValue(dr, "ID")
        Me.Name = Helper.getStringValue(dr, "Name")
        Me.Address = Helper.getStringValue(dr, "Address")
        Me.City = Helper.getStringValue(dr, "City")
        Me.State = Helper.getStringValue(dr, "State")
        Me.Postcode = Helper.getStringValue(dr, "Postcode")
        Me.Phone = Helper.getStringValue(dr, "Phone")
        Me.Fax = Helper.getStringValue(dr, "Fax")
        Me.Email = Helper.getStringValue(dr, "Email")
        Me.Status = Helper.getIntegerValue(dr, "Status")
    End Sub

#End Region

End Class

Public Class Clinics : Inherits List(Of Clinic)

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
        Dim myClinics As New Clinics
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM Clinics")
        If Not Me.Filter = "" Then
            sql.Append(String.Format(" WHERE '{0}'", Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY '{0}'", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "Clinics")
            Dim row As DataRow
            For Each row In ds.Tables("Clinics").Rows
                Dim theClinic As New Clinic
                theClinic.ID = row("ID")
                theClinic.Name = row("Name")
                theClinic.Address = row("Address")
                theClinic.City = row("City")
                theClinic.State = row("State")
                theClinic.Postcode = row("Postcode")
                theClinic.Phone = row("Phone")
                theClinic.Fax = Helper.getString(row("Fax"))
                theClinic.Email = Helper.getString(row("Email"))
                theClinic.Status = row("Status")
                myClinics.Add(theClinic)
            Next
        End Using
        Return myClinics

    End Function

End Class