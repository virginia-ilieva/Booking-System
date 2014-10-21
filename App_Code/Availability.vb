Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class Availability

#Region "Properties"

    Private _StartTimeString As String
    Public Property StartTimeString() As String
        Get
            Return _StartTimeString
        End Get
        Set(ByVal value As String)
            _StartTimeString = value
        End Set
    End Property

    Private _EndTimeString As String
    Public Property EndTimeString() As String
        Get
            Return _EndTimeString
        End Get
        Set(ByVal value As String)
            _EndTimeString = value
        End Set
    End Property

    Private _ID As Integer
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Private _RoomID As Integer
    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
        End Set
    End Property

    Private _StartTime As Date
    Public Property StartTime() As Date
        Get
            Return _StartTime
        End Get
        Set(ByVal value As Date)
            _StartTime = value
        End Set
    End Property

    Private _EndTime As Date
    Public Property EndTime() As Date
        Get
            Return _EndTime
        End Get
        Set(ByVal value As Date)
            _EndTime = value
        End Set
    End Property

    Private _Workday As Integer
    Public Property Workday() As Integer
        Get
            Return _Workday
        End Get
        Set(ByVal value As Integer)
            _Workday = value
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("SELECT * FROM Availability WHERE ID = {0}", theID), myConnection)
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
            myCommand.CommandText = String.Format("UPDATE Availability SET RoomID = {0}, StartTime = '{1}', EndTime = '{2}', Workday = {3} WHERE( ID = {4} )", Me.RoomID, Convert.ToDateTime(Me.StartTime).ToString("hh:mm tt"), Convert.ToDateTime(Me.EndTime).ToString("hh:mm tt"), Me.Workday, Me.ID)
        Else
            myCommand.CommandText = String.Format("INSERT INTO Availability (RoomID, StartTime, EndTime, Workday) VALUES ({0}, '{1}', '{2}', {3})", Me.RoomID, Convert.ToDateTime(Me.StartTime).ToString("hh:mm tt"), Convert.ToDateTime(Me.EndTime).ToString("hh:mm tt"), Me.Workday)
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("DELETE FROM Availability WHERE ID = {0}", Me.ID), myConnection)
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
        Me.RoomID = 0
        Me.StartTime = Now
        Me.StartTimeString = ""
        Me.EndTime = Now
        Me.EndTimeString = ""
        Me.Workday = 0
    End Sub

    ''' <summary>
    ''' Passes the data from the database to the object's properties
    ''' </summary>
    ''' <param name="dr">SQLDaraReader: holds the data for the object</param>
    ''' <remarks></remarks>
    Private Sub Parse(ByRef dr As SqlDataReader)
        Me.ID = Helper.getIntegerValue(dr, "ID")
        Me.RoomID = Helper.getIntegerValue(dr, "RoomID")
        Me.StartTime = Helper.getDateTimeValue(dr, "StartTime")
        Me.StartTimeString = Me.StartTime.ToString
        Me.EndTime = Helper.getDateTimeValue(dr, "EndTime")
        Me.EndTimeString = Me.EndTime.ToString
        Me.Workday = Helper.getIntegerValue(dr, "Workday")
    End Sub

#End Region

End Class

Public Class Availabilities : Inherits List(Of Availability)

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

    Public Function Read() As Availabilities
        Dim myAvailabilities As New Availabilities
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM Availability")
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
            da.Fill(ds, "Availability")
            Dim row As DataRow
            For Each row In ds.Tables("Availability").Rows
                Dim theAvailability As New Availability
                theAvailability.ID = row("ID")
                theAvailability.RoomID = row("RoomID")
                theAvailability.StartTime = row("StartTime")
                theAvailability.StartTimeString = theAvailability.StartTime.ToString
                theAvailability.EndTime = row("EndTime")
                theAvailability.EndTimeString = theAvailability.EndTime.ToString
                theAvailability.Workday = row("Workday")
                myAvailabilities.Add(theAvailability)
            Next
        End Using
        Return myAvailabilities
    End Function

    Public Function ReadByRoomID(ByVal roomID As Integer) As Availabilities
        Dim myAvailabilities As New Availabilities
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append(String.Format("SELECT * FROM Availability WHERE RoomID = {0} ORDER BY Workday,StartTime", roomID))
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "Availability")
            Dim row As DataRow
            For Each row In ds.Tables("Availability").Rows
                Dim theAvailability As New Availability
                theAvailability.ID = row("ID")
                theAvailability.RoomID = row("RoomID")
                theAvailability.StartTime = row("StartTime")
                theAvailability.StartTimeString = theAvailability.StartTime.ToString
                theAvailability.EndTime = row("EndTime")
                theAvailability.EndTimeString = theAvailability.EndTime.ToString
                theAvailability.Workday = row("Workday")
                myAvailabilities.Add(theAvailability)
            Next
        End Using
        Return myAvailabilities
    End Function
End Class