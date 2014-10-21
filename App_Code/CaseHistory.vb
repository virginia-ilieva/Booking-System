Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class CaseHistory

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

    'Private _AppointmentID As Integer
    'Public Property AppointmentID() As Integer
    '    Get
    '        Return _AppointmentID
    '    End Get
    '    Set(ByVal value As Integer)
    '        _AppointmentID = value
    '    End Set
    'End Property

    Private _ClientID As Integer
    Public Property ClientID() As Integer
        Get
            Return _ClientID
        End Get
        Set(ByVal value As Integer)
            _ClientID = value
        End Set
    End Property

    'Private _ThreatmentID As Integer
    'Public Property ThreatmentID() As Integer
    '    Get
    '        Return _ThreatmentID
    '    End Get
    '    Set(ByVal value As Integer)
    '        _ThreatmentID = value
    '    End Set
    'End Property

    Private _Created As Date
    Public Property Created() As Date
        Get
            Return _Created
        End Get
        Set(ByVal value As Date)
            _Created = value
        End Set
    End Property

    Private _Symptoms As String
    Public Property Symptoms() As String
        Get
            Return _Symptoms.Trim
        End Get
        Set(ByVal value As String)
            _Symptoms = value
        End Set
    End Property

    Private _Medications As String
    Public Property Medications() As String
        Get
            Return _Medications.Trim
        End Get
        Set(ByVal value As String)
            _Medications = value
        End Set
    End Property

    Private _Practitioners As String
    Public Property Practitioners() As String
        Get
            Return _Practitioners.Trim
        End Get
        Set(ByVal value As String)
            _Practitioners = value
        End Set
    End Property

    Private _Therapies As String
    Public Property Therapies() As String
        Get
            Return _Therapies.Trim
        End Get
        Set(ByVal value As String)
            _Therapies = value
        End Set
    End Property

    Private _HealthHistory As String
    Public Property HealthHistory() As String
        Get
            Return _HealthHistory.Trim
        End Get
        Set(ByVal value As String)
            _HealthHistory = value
        End Set
    End Property

    Private _FamilyHistory As String
    Public Property FamilyHistory() As String
        Get
            Return _FamilyHistory.Trim
        End Get
        Set(ByVal value As String)
            _FamilyHistory = value
        End Set
    End Property

    Private _Prakrti As String
    Public Property Prakrti() As String
        Get
            Return _Prakrti.Trim
        End Get
        Set(ByVal value As String)
            _Prakrti = value
        End Set
    End Property

    Private _Vikrti As String
    Public Property Vikrti() As String
        Get
            Return _Vikrti.Trim
        End Get
        Set(ByVal value As String)
            _Vikrti = value
        End Set
    End Property

    Private _Agni As String
    Public Property Agni() As String
        Get
            Return _Agni.Trim
        End Get
        Set(ByVal value As String)
            _Agni = value
        End Set
    End Property

    Private _Malas As String
    Public Property Malas() As String
        Get
            Return _Malas.Trim
        End Get
        Set(ByVal value As String)
            _Malas = value
        End Set
    End Property

    Private _Ojas As String
    Public Property Ojas() As String
        Get
            Return _Ojas.Trim
        End Get
        Set(ByVal value As String)
            _Ojas = value
        End Set
    End Property

    Private _Manas As String
    Public Property Manas() As String
        Get
            Return _Manas.Trim
        End Get
        Set(ByVal value As String)
            _Manas = value
        End Set
    End Property

    Private _Srotas As String
    Public Property Srotas() As String
        Get
            Return _Srotas.Trim
        End Get
        Set(ByVal value As String)
            _Srotas = value
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

    Private _SuggestedTherapies As String
    Public Property SuggestedTherapies() As String
        Get
            Return _SuggestedTherapies.Trim
        End Get
        Set(ByVal value As String)
            _SuggestedTherapies = value
        End Set
    End Property

    Private _SuggestedHerbs As String
    Public Property SuggestedHerbs() As String
        Get
            Return _SuggestedHerbs.Trim
        End Get
        Set(ByVal value As String)
            _SuggestedHerbs = value
        End Set
    End Property

    Private _SuggestedPlan As String
    Public Property SuggestedPlan() As String
        Get
            Return _SuggestedPlan.Trim
        End Get
        Set(ByVal value As String)
            _SuggestedPlan = value
        End Set
    End Property

    Private _SuggestedCleanses As String
    Public Property SuggestedCleanses() As String
        Get
            Return _SuggestedCleanses.Trim
        End Get
        Set(ByVal value As String)
            _SuggestedCleanses = value
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("SELECT * FROM CaseHistory WHERE ID = {0}", theID), myConnection)
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
            myCommand.CommandText = String.Format("UPDATE CaseHistory SET UserID = {0}, ClientID = {1}, Symptoms = '{2}', Medications = '{3}', Practitioners = '{4}', Therapies = '{5}', HealthHistory = '{6}', FamilyHistory = '{7}', Prakrti = '{8}', Vikrti = '{9}', Agni = '{10}', Malas = '{11}', Ojas = '{12}', Manas = '{13}', Srotas = '{14}', Comments = '{15}', SuggestedTherapies = '{16}', SuggestedHerbs = '{17}', SuggestedPlan = '{18}', SuggestedCleanses = '{19}' WHERE( ID = {20} )", Me.UserID, Me.ClientID, Me.Symptoms, Me.Medications, Me.Practitioners, Me.Therapies, Me.HealthHistory, Me.FamilyHistory, Me.Prakrti, Me.Vikrti, Me.Agni, Me.Malas, Me.Ojas, Me.Srotas, Me.Comments, Me.SuggestedTherapies, Me.SuggestedHerbs, Me.SuggestedPlan, Me.SuggestedCleanses, Me.ID)
        Else
            myCommand.CommandText = String.Format("INSERT INTO CaseHistory (UserID, ClientID, Created, Symptoms , Medications, Practitioners, Therapies, HealthHistory, FamilyHistory, Prakrti, Vikrti, Agni, Malas, Ojas, Manas, Srotas, Comments, SuggestedTherapies, SuggestedHerbs, SuggestedPlan, SuggestedCleanses) VALUES ({0}, {1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}')", Me.UserID, Me.ClientID, Me.Created, Me.Symptoms, Me.Medications, Me.Practitioners, Me.Therapies, Me.HealthHistory, Me.FamilyHistory, Me.Prakrti, Me.Vikrti, Me.Agni, Me.Malas, Me.Ojas, Me.Manas, Me.Srotas, Me.Comments, Me.SuggestedTherapies, Me.SuggestedHerbs, Me.SuggestedPlan, Me.SuggestedCleanses)
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
        Dim myCommand As SqlCommand = New SqlCommand(String.Format("DELETE FROM CaseHistory WHERE ID = {0}", Me.ID), myConnection)
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
        ''Me.AppointmentID = 0
        Me.ClientID = 0
        ''Me.ThreatmentID = 0
        Me.Created = Today
        Me.Symptoms = ""
        Me.Medications = ""
        Me.Practitioners = ""
        Me.Therapies = ""
        Me.HealthHistory = ""
        Me.FamilyHistory = ""
        Me.Prakrti = ""
        Me.Vikrti = ""
        Me.Agni = ""
        Me.Malas = ""
        Me.Ojas = ""
        Me.Manas = ""
        Me.Srotas = ""
        Me.Comments = ""
        Me.SuggestedTherapies = ""
        Me.SuggestedHerbs = ""
        Me.SuggestedPlan = ""
        Me.SuggestedCleanses = ""
    End Sub

    ''' <summary>
    ''' Passes the data from the database to the object's properties
    ''' </summary>
    ''' <param name="dr">SQLDaraReader: holds the data for the object</param>
    ''' <remarks></remarks>
    Private Sub Parse(ByRef dr As SqlDataReader)
        Me.ID = Helper.getIntegerValue(dr, "ID")
        Me.UserID = Helper.getIntegerValue(dr, "UserID")
        ''Me.AppointmentID = Helper.getIntegerValue(dr, "AppointmentID")
        Me.ClientID = Helper.getIntegerValue(dr, "ClientID")
        ''Me.ThreatmentID = Helper.getIntegerValue(dr, "ThreatmentID")
        Me.Created = Helper.getDateTimeValue(dr, "Created")
        Me.Symptoms = Helper.getStringValue(dr, "Symptoms")
        Me.Medications = Helper.getStringValue(dr, "Medications")
        Me.Practitioners = Helper.getStringValue(dr, "Practitioners")
        Me.Therapies = Helper.getStringValue(dr, "Therapies")
        Me.HealthHistory = Helper.getStringValue(dr, "HealthHistory")
        Me.FamilyHistory = Helper.getStringValue(dr, "FamilyHistory")
        Me.Prakrti = Helper.getStringValue(dr, "Prakrti")
        Me.Vikrti = Helper.getStringValue(dr, "Vikrti")
        Me.Agni = Helper.getStringValue(dr, "Agni")
        Me.Malas = Helper.getStringValue(dr, "Malas")
        Me.Ojas = Helper.getStringValue(dr, "Ojas")
        Me.Manas = Helper.getStringValue(dr, "Manas")
        Me.Srotas = Helper.getStringValue(dr, "Srotas")
        Me.Comments = Helper.getStringValue(dr, "Comments")
        Me.SuggestedTherapies = Helper.getStringValue(dr, "SuggestedTherapies")
        Me.SuggestedHerbs = Helper.getStringValue(dr, "SuggestedHerbs")
        Me.SuggestedPlan = Helper.getStringValue(dr, "SuggestedPlan")
        Me.SuggestedCleanses = Helper.getStringValue(dr, "SuggestedCleanses")
    End Sub

#End Region

End Class

Public Class CaseHistories : Inherits List(Of CaseHistory)

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

    Public Function Read() As Casehistories
        Dim myCaseHistories As New CaseHistories
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM CaseHistory")
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
            da.Fill(ds, "CaseHistory")
            Dim row As DataRow
            For Each row In ds.Tables("CaseHistory").Rows
                Dim theCaseHistory As New CaseHistory
                theCaseHistory.ID = row("ID")
                theCaseHistory.UserID = row("UserID")
                ''theCaseHistory.AppointmentID = row("AppointmentID")
                theCaseHistory.ClientID = row("ClientID")
                ''theCaseHistory.ThreatmentID = row("ThreatmentID")
                theCaseHistory.Created = row("Created")
                theCaseHistory.Symptoms = row("Symptoms")
                theCaseHistory.Medications = row("Medications")
                theCaseHistory.Practitioners = row("Practitioners")
                theCaseHistory.Therapies = row("Therapies")
                theCaseHistory.HealthHistory = row("HealthHistory")
                theCaseHistory.FamilyHistory = row("FamilyHistory")
                theCaseHistory.Prakrti = row("Prakrti")
                theCaseHistory.Vikrti = row("Vikrti")
                theCaseHistory.Agni = row("Agni")
                theCaseHistory.Malas = row("Malas")
                theCaseHistory.Ojas = row("Ojas")
                theCaseHistory.Manas = row("Manas")
                theCaseHistory.Srotas = row("Srotas")
                theCaseHistory.Comments = row("Comments")
                theCaseHistory.SuggestedTherapies = row("SuggestedTherapies")
                theCaseHistory.SuggestedHerbs = row("SuggestedHerbs")
                theCaseHistory.SuggestedPlan = row("SuggestedPlan")
                theCaseHistory.SuggestedCleanses = row("SuggestedCleanses")
                myCaseHistories.Add(theCaseHistory)
            Next
        End Using
        Return myCaseHistories
    End Function

    Public Function ReadByAppointmentID(ByVal appointmentID As Integer) As CaseHistories
        Dim myCaseHistories As New CaseHistories
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM CaseHistory")
        If Me.Filter = "" Then
            sql.Append(String.Format(" WHERE AppointmentID = {0}", appointmentID))
        Else
            sql.Append(String.Format(" WHERE AppointmentID = {0} AND {1}", appointmentID, Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY '{0}'", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "CaseHistory")
            Dim row As DataRow
            For Each row In ds.Tables("CaseHistory").Rows
                Dim theCaseHistory As New CaseHistory
                theCaseHistory.ID = row("ID")
                theCaseHistory.UserID = row("UserID")
                ''theCaseHistory.AppointmentID = row("AppointmentID")
                theCaseHistory.ClientID = row("ClientID")
                ''theCaseHistory.ThreatmentID = row("ThreatmentID")
                theCaseHistory.Created = row("Created")
                theCaseHistory.Symptoms = row("Symptoms")
                theCaseHistory.Medications = row("Medications")
                theCaseHistory.Practitioners = row("Practitioners")
                theCaseHistory.Therapies = row("Therapies")
                theCaseHistory.HealthHistory = row("HealthHistory")
                theCaseHistory.FamilyHistory = row("FamilyHistory")
                theCaseHistory.Prakrti = row("Prakrti")
                theCaseHistory.Vikrti = row("Vikrti")
                theCaseHistory.Agni = row("Agni")
                theCaseHistory.Malas = row("Malas")
                theCaseHistory.Ojas = row("Ojas")
                theCaseHistory.Manas = row("Manas")
                theCaseHistory.Srotas = row("Srotas")
                theCaseHistory.Comments = row("Comments")
                theCaseHistory.SuggestedTherapies = row("SuggestedTherapies")
                theCaseHistory.SuggestedHerbs = row("SuggestedHerbs")
                theCaseHistory.SuggestedPlan = row("SuggestedPlan")
                theCaseHistory.SuggestedCleanses = row("SuggestedCleanses")
                myCaseHistories.Add(theCaseHistory)
            Next
        End Using
        Return myCaseHistories
    End Function

    Public Function ReadByClientID(ByVal clientID As Integer) As CaseHistories
        Dim myCaseHistories As New CaseHistories
        Dim myConnection As New SqlConnection
        myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultDB").ConnectionString
        Dim sql As New StringBuilder(String.Empty)
        sql.Append("SELECT * FROM CaseHistory")
        If Me.Filter = "" Then
            sql.Append(String.Format(" WHERE ClientID = {0}", clientID))
        Else
            sql.Append(String.Format(" WHERE ClientID = {0} AND {1}", clientID, Me.Filter))
        End If
        If Not Me.OrderBy = "" Then
            sql.Append(String.Format(" ORDER BY '{0}'", Me.OrderBy))
        End If
        Dim myCommand As SqlCommand = New SqlCommand(sql.ToString, myConnection)
        Using myCommand
            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(myCommand)
            da.Fill(ds, "CaseHistory")
            Dim row As DataRow
            For Each row In ds.Tables("CaseHistory").Rows
                Dim theCaseHistory As New CaseHistory
                theCaseHistory.ID = row("ID")
                theCaseHistory.UserID = row("UserID")
                ''theCaseHistory.AppointmentID = row("AppointmentID")
                theCaseHistory.ClientID = row("ClientID")
                ''theCaseHistory.ThreatmentID = row("ThreatmentID")
                theCaseHistory.Created = row("Created")
                theCaseHistory.Symptoms = row("Symptoms")
                theCaseHistory.Medications = row("Medications")
                theCaseHistory.Practitioners = row("Practitioners")
                theCaseHistory.Therapies = row("Therapies")
                theCaseHistory.HealthHistory = row("HealthHistory")
                theCaseHistory.FamilyHistory = row("FamilyHistory")
                theCaseHistory.Prakrti = row("Prakrti")
                theCaseHistory.Vikrti = row("Vikrti")
                theCaseHistory.Agni = row("Agni")
                theCaseHistory.Malas = row("Malas")
                theCaseHistory.Ojas = row("Ojas")
                theCaseHistory.Manas = row("Manas")
                theCaseHistory.Srotas = row("Srotas")
                theCaseHistory.Comments = row("Comments")
                theCaseHistory.SuggestedTherapies = row("SuggestedTherapies")
                theCaseHistory.SuggestedHerbs = row("SuggestedHerbs")
                theCaseHistory.SuggestedPlan = row("SuggestedPlan")
                theCaseHistory.SuggestedCleanses = row("SuggestedCleanses")
                myCaseHistories.Add(theCaseHistory)
            Next
        End Using
        Return myCaseHistories
    End Function


End Class
