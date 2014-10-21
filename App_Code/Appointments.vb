Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class Appointment

#Region "Properties"

    Private _AppointmentDateString As String
    Public Property AppointmentDateString() As String
        Get
            Return _AppointmentDateString
        End Get
        Set(ByVal value As String)
            _AppointmentDateString = value
        End Set
    End Property

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

    Private _UserID As Integer
    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
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

    Private _ThreatmentID As Integer
    Public Property ThreatmentID() As Integer
        Get
            Return _ThreatmentID
        End Get
        Set(ByVal value As Integer)
            _ThreatmentID = value
        End Set
    End Property

    Private _AppointmentDate As Date
    Public Property AppointmentDate() As Date
        Get
            Return _AppointmentDate
        End Get
        Set(ByVal value As Date)
            _AppointmentDate = value
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

    Private _Comments As String
    Public Property Comments() As String
        Get
            Return _Comments.Trim
        End Get
        Set(ByVal value As String)
            _Comments = value
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("SELECT * FROM Appointments WHERE ID = {0}", theID), myConnection)
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
            myCommand.CommandText = String.Format("UPDATE Appointments SET UserID = {0}, ClinicID = {1}, ThreatmentID = {2}, AppointmentDate = '{3}', StartTime = '{4}', EndTime = '{5}', Comments = '{6}', Status = {8} WHERE( ID = {7} )", Me.UserID, Me.ClinicID, Me.ThreatmentID, Convert.ToDateTime(Me.AppointmentDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(Me.StartTime).ToString("yyyy-MM-dd hh:mm tt"), Convert.ToDateTime(Me.EndTime).ToString("yyyy-MM-dd hh:mm tt"), Me.Comments, Me.ID, Me.Status)
        Else
            myCommand.CommandText = String.Format("INSERT INTO Appointments (UserID, ClinicID, ThreatmentID, AppointmentDate, StartTime, EndTime, Comments, Status) VALUES ({0}, {1}, {2}, '{3}', '{4}', '{5}', '{6}', {7})", Me.UserID, Me.ClinicID, Me.ThreatmentID, Convert.ToDateTime(Me.AppointmentDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(Me.StartTime).ToString("yyyy-MM-dd hh:mm tt"), Convert.ToDateTime(Me.EndTime).ToString("yyyy-MM-dd hh:mm tt"), Me.Comments, Me.Status)
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("DELETE FROM Appointments WHERE ID = {0}", Me.ID), myConnection)
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
        Me.ClinicID = 0
        Me.ThreatmentID = 0
        Me.AppointmentDate = Today
        Me.AppointmentDateString = ""
        Me.StartTime = Now
        Me.StartTimeString = ""
        Me.EndTime = Now
        Me.EndTimeString = ""
        Me.Comments = ""
        Me.Status = 1
    End Sub

    ''' <summary>
    ''' Passes the data from the database to the object's properties
    ''' </summary>
    ''' <param name="dr">SQLDaraReader: holds the data for the object</param>
    ''' <remarks></remarks>
    Private Sub Parse(ByRef dr As SqlDataReader)
        Me.ID = Helper.getIntegerValue(dr, "ID")
        Me.UserID = Helper.getIntegerValue(dr, "UserID")
        Me.ClinicID = Helper.getIntegerValue(dr, "ClinicID")
        Me.ThreatmentID = Helper.getIntegerValue(dr, "ThreatmentID")
        Me.AppointmentDate = Helper.getDateTimeValue(dr, "AppointmentDate")
        Me.AppointmentDateString = Me.AppointmentDate.ToString
        Me.StartTime = Helper.getDateTimeValue(dr, "StartTime")
        Me.StartTimeString = Me.StartTime.ToString
        Me.EndTime = Helper.getDateTimeValue(dr, "EndTime")
        Me.EndTimeString = Me.EndTime.ToString
        Me.Comments = Helper.getStringValue(dr, "Comments")
        Me.Status = Helper.getIntegerValue(dr, "Status")
    End Sub

#End Region

End Class

Public Class Appointments : Inherits List(Of Appointment)

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

    Public Function Read() As Appointments
        Dim myAppointments As New Appointments
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM Appointments")
        If Me.Filter = "" Then
            sql.Append(" WHERE Status = 1")
        Else
            sql.Append(String.Format(" WHERE Status = 1 AND {0}", Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY '{0}'", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "Appointments")
            Dim row As DataRow
            For Each row In ds.Tables("Appointments").Rows
                Dim theAppointment As New Appointment
                theAppointment.ID = row("ID")
                theAppointment.UserID = row("UserID")
                theAppointment.ClinicID = row("ClinicID")
                theAppointment.ThreatmentID = row("ThreatmentID")
                theAppointment.AppointmentDate = row("AppointmentDate")
                theAppointment.AppointmentDateString = theAppointment.AppointmentDate.ToString
                theAppointment.StartTime = row("StartTime")
                theAppointment.StartTimeString = theAppointment.StartTime.ToString
                theAppointment.EndTime = row("EndTime")
                theAppointment.EndTimeString = theAppointment.EndTime.ToString
                theAppointment.Comments = Helper.getString(row("Comments"))
                theAppointment.Status = row("Status")
                myAppointments.Add(theAppointment)
            Next
        End Using
        Return myAppointments
    End Function

    Public Function ReadByClientID(ByVal clientID As Integer) As Appointments
        Dim myAppointments As New Appointments
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM Appointments")
        If Me.Filter = "" Then
            sql.Append(String.Format(" WHERE UserID = {0}", clientID))
        Else
            sql.Append(String.Format(" WHERE UserID = {0} AND Status = 1 AND ({1})", clientID, Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY {0}", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "Appointments")
            Dim row As DataRow
            For Each row In ds.Tables("Appointments").Rows
                Dim theAppointment As New Appointment
                theAppointment.ID = row("ID")
                theAppointment.UserID = row("UserID")
                theAppointment.ClinicID = row("ClinicID")
                theAppointment.ThreatmentID = row("ThreatmentID")
                theAppointment.AppointmentDate = row("AppointmentDate")
                theAppointment.AppointmentDateString = theAppointment.AppointmentDate.ToString
                theAppointment.StartTime = row("StartTime")
                theAppointment.StartTimeString = theAppointment.StartTime.ToString
                theAppointment.EndTime = row("EndTime")
                theAppointment.EndTimeString = theAppointment.EndTime.ToString
                theAppointment.Comments = Helper.getString(row("Comments"))
                theAppointment.Status = row("Status")
                myAppointments.Add(theAppointment)
            Next
        End Using
        Return myAppointments
    End Function
End Class