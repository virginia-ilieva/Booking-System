Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class ClinicRoom

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

    Private _ClinicID As Integer
    Public Property ClinicID() As Integer
        Get
            Return _ClinicID
        End Get
        Set(ByVal value As Integer)
            _ClinicID = value
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("SELECT * FROM ClinicRooms WHERE ID = {0}", theID), myConnection)
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
            myCommand.CommandText = String.Format("UPDATE ClinicRooms SET ClinicID = {0}, Name = '{1}', Status = {2} WHERE( ID = {3} )", Me.ClinicID, Me.Name, Me.Status, Me.ID)
        Else
            myCommand.CommandText = String.Format("INSERT INTO ClinicRooms (ClinicID, Name, Status) VALUES ({0}, '{1}', {2}); SELECT SCOPE_IDENTITY()", Me.ClinicID, Me.Name, Me.Status)
        End If
        myCommand = New SqlCommand(myCommand.CommandText, myConnection)
        myConnection.Open()
        Me.ID = CType(myCommand.ExecuteScalar(), Integer)
        myConnection.Close()
    End Sub

    ''' <summary>
    ''' Deletes an oblect from the database
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("DELETE FROM ClinicRooms WHERE ID = {0}", Me.ID), myConnection)
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
        Me.ClinicID = 0
        Me.Name = ""
        Me.Status = TheStatus.Inactive
    End Sub

    ''' <summary>
    ''' Passes the data from the database to the object's properties
    ''' </summary>
    ''' <param name="dr">SQLDaraReader: holds the data for the object</param>
    ''' <remarks></remarks>
    Private Sub Parse(ByRef dr As SqlDataReader)
        Me.ID = Helper.getIntegerValue(dr, "ID")
        Me.ClinicID = Helper.getIntegerValue(dr, "ClinicID")
        Me.Name = Helper.getStringValue(dr, "Name")
        Me.Status = Helper.getIntegerValue(dr, "Status")
    End Sub

#End Region

End Class

Public Class ClinicRooms : Inherits List(Of ClinicRoom)

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

    Public Function Read() As ClinicRooms
        Dim myClinicRooms As New ClinicRooms
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM ClinicRooms")
        If Not Me.Filter = "" Then
            sql.Append(String.Format(" WHERE {0}", Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY '{0}'", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "ClinicRooms")
            Dim row As DataRow
            For Each row In ds.Tables("ClinicRooms").Rows
                Dim theClinicRoom As New ClinicRoom
                theClinicRoom.ID = row("ID")
                theClinicRoom.ClinicID = row("ClinicID")
                theClinicRoom.Name = row("Name")
                theClinicRoom.Status = row("Status")
                myClinicRooms.Add(theClinicRoom)
            Next
        End Using
        Return myClinicRooms
    End Function

    Public Function ReadByClinicID(ByVal clinicID As Integer) As ClinicRooms
        Dim myClinicRooms As New ClinicRooms
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM ClinicRooms")
        Me.Filter = String.Format("ClinicID = {0}", clinicID)
        If Not Me.Filter = "" Then
            sql.Append(String.Format(" WHERE {0}", Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY '{0}'", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "ClinicRooms")
            Dim row As DataRow
            For Each row In ds.Tables("ClinicRooms").Rows
                Dim theClinicRoom As New ClinicRoom
                theClinicRoom.ID = row("ID")
                theClinicRoom.ClinicID = row("ClinicID")
                theClinicRoom.Name = row("Name")
                theClinicRoom.Status = row("Status")
                myClinicRooms.Add(theClinicRoom)
            Next
        End Using
        Return myClinicRooms
    End Function
End Class