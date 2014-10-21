Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class CaseNote

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

    Private _UserID As Integer
    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Private _CaseID As Integer
    Public Property CaseID() As Integer
        Get
            Return _CaseID
        End Get
        Set(ByVal value As Integer)
            _CaseID = value
        End Set
    End Property

    Private _Notes As String
    Public Property Notes() As String
        Get
            Return _Notes.Trim
        End Get
        Set(ByVal value As String)
            _Notes = value
        End Set
    End Property

    Private _Created As Date
    Public Property Created() As Date
        Get
            Return _Created
        End Get
        Set(ByVal value As Date)
            _Created = value
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("SELECT * FROM CaseNotes WHERE ID = {0}", theID), myConnection)
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
            myCommand.CommandText = String.Format("UPDATE CaseNotes SET UserID = {0}, CaseID = {1}, Notes = '{2}' WHERE( ID = {3} )", Me.UserID, Me.CaseID, Me.Notes, Me.ID)
        Else
            myCommand.CommandText = String.Format("INSERT INTO CaseNotes (UserID, CaseID, Notes, Created) VALUES ({0}, {1}, '{2}', '{3}')", Me.UserID, Me.CaseID, Me.Notes, Convert.ToDateTime(Today).ToString("yyyy-MM-dd"))
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("DELETE FROM CaseNotes WHERE ID = {0}", Me.ID), myConnection)
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
        Me.UserID = 0
        Me.CaseID = 0
        Me.Notes = ""
        Me.Created = Today
    End Sub

    ''' <summary>
    ''' Passes the data from the database to the object's properties
    ''' </summary>
    ''' <param name="dr">SQLDaraReader: holds the data for the object</param>
    ''' <remarks></remarks>
    Private Sub Parse(ByRef dr As SqlDataReader)
        Me.ID = Helper.getIntegerValue(dr, "ID")
        Me.UserID = Helper.getIntegerValue(dr, "UserID")
        Me.CaseID = Helper.getIntegerValue(dr, "CaseID")
        Me.Notes = Helper.getStringValue(dr, "notes")
        Me.Created = Helper.getDateTimeValue(dr, "Created")
    End Sub

#End Region

End Class

Public Class CaseNotes : Inherits List(Of casenote)

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

    Public Function Read() As CaseNotes
        Dim myCaseNotes As New CaseNotes
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM CaseNotes")
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
            da.Fill(ds, "CaseNotes")
            Dim row As DataRow
            For Each row In ds.Tables("CaseNotes").Rows
                Dim theCaseNote As New CaseNote
                theCaseNote.ID = row("ID")
                theCaseNote.UserID = row("UserID")
                theCaseNote.CaseID = row("CaseID")
                theCaseNote.Notes = row("Notes")
                theCaseNote.Created = row("Created")
                myCaseNotes.Add(theCaseNote)
            Next
        End Using
        Return myCaseNotes
    End Function

    Public Function ReadByCaseID(ByVal caseID As Integer) As CaseNotes
        Dim myCaseNotes As New CaseNotes
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM CaseNotes")
        If Not Me.Filter = "" Then
            sql.Append(String.Format(" WHERE CaseID = {0} AND {1}", caseID, Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY '{0}'", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "CaseNotes")
            Dim row As DataRow
            For Each row In ds.Tables("CaseNotes").Rows
                Dim theCaseNote As New CaseNote
                theCaseNote.ID = row("ID")
                theCaseNote.UserID = row("UserID")
                theCaseNote.CaseID = row("CaseID")
                theCaseNote.Notes = row("Notes")
                theCaseNote.Created = row("Created")
                myCaseNotes.Add(theCaseNote)
            Next
        End Using
        Return myCaseNotes
    End Function

End Class
