Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class AppointmentEvent

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

    Private _Status As Integer
    Public Property Status() As Integer
        Get
            Return _Status
        End Get
        Set(ByVal value As Integer)
            _Status = value
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

    Private _ClinicName As String
    Public Property ClinicName() As String
        Get
            Return _ClinicName.Trim
        End Get
        Set(ByVal value As String)
            _ClinicName = value
        End Set
    End Property

    Private _TherapyName As String
    Public Property TherapyName() As String
        Get
            Return _TherapyName.Trim
        End Get
        Set(ByVal value As String)
            _TherapyName = value
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
        Me.Status = 1
        Me.Comments = ""
        Me.Firstname = ""
        Me.Lastname = ""
        Me.Phone = ""
        Me.Email = ""
        Me.ClinicName = ""
        Me.TherapyName = ""
    End Sub

#End Region

End Class

Public Class AppointmentEvents : Inherits List(Of AppointmentEvent)

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

    Public Function Read() As AppointmentEvents
        Dim myAppointments As New AppointmentEvents
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("select dbo.Appointments.*, dbo.Users.Firstname, dbo.Users.Lastname, dbo.Users.Phone, dbo.Users.Email, dbo.Clinics.Name as ClinicName, dbo.Treatments.Name as TherapyName " + _
            "from dbo.Appointments " + _
            "inner join dbo.Users on Appointments.UserID = Users.ID " + _
            "inner join dbo.Clinics on Appointments.ClinicID = Clinics.ID " + _
            "inner join dbo.Treatments on Appointments.ThreatmentID = Treatments.ID")
        sql.Append(String.Format(" WHERE Appointments.Status = 1 AND {0}", Me.Filter))
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "Table")
            Dim row As DataRow
            For Each row In ds.Tables("Table").Rows
                Dim theAppointment As New AppointmentEvent
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
                theAppointment.Comments = row("Comments")
                theAppointment.Firstname = row("Firstname")
                theAppointment.Lastname = row("Lastname")
                theAppointment.Phone = row("Phone")
                theAppointment.Email = row("Email")
                theAppointment.ClinicName = row("ClinicName")
                theAppointment.TherapyName = row("TherapyName")
                myAppointments.Add(theAppointment)
            Next
        End Using
        Return myAppointments
    End Function
End Class