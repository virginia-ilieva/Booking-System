Imports System.Web.Services
Imports System.Web.Script.Services

Public Class _workers : Inherits System.Web.UI.Page

    ''' <summary>
    ''' 'Sets the ID for the selected worker
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property WorkerID As Integer
        Get
            Dim ID As Integer = 0
            If Me.ViewState("WorkerID") IsNot Nothing Then
                Integer.TryParse(Me.ViewState("WorkerID"), ID)
            End If
            Return ID
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("WorkerID") = value
        End Set
    End Property

    ''' <summary>
    ''' Runs every time the page is loaded
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Security.UserRole = Integer.Parse(SecurityRole.Client) Or Security.UserRole = Integer.Parse(SecurityRole.Worker) Or Security.UserRole = "" Then
            Security.UserName = Nothing
            Security.Password = Nothing
            Security.UserRole = Nothing
            Response.Redirect("~\login.aspx")
        End If
        If Not IsPostBack Then
            getWorkers()
        End If
    End Sub

    ''' <summary>
    ''' Loads the list of workers
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getWorkers()
        Dim theWorkers As New List(Of User)
        Dim myWorkers As New Users
        Try
            myWorkers.Filter = String.Format("Role = {0}", Integer.Parse(SecurityRole.Worker))
            theWorkers = myWorkers.Read()
            Me.WorkersList.DataSource = theWorkers
            Me.WorkersList.DataBind()
        Finally
            If myWorkers IsNot Nothing Then myWorkers = Nothing
            If theWorkers IsNot Nothing Then theWorkers = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Load worker details
    ''' </summary>
    ''' <param name="workerID">Integer: the ID of the selected worker</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadWorkerDetails(ByVal workerID As Integer) As User
        Dim myWorker As New User(workerID)
        Try
            Return myWorker
        Finally
            If myWorker IsNot Nothing Then myWorker = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Saves the worker to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveWorkerDetails(ByVal workerID As Integer, ByVal firstname As String, ByVal lastname As String, ByVal username As String, ByVal password As String, ByVal phone As String, ByVal email As String, ByVal status As Integer) As Boolean
        Dim myWorker As New User
        Try
            If workerID > 0 Then
                myWorker.Read(workerID)
            End If
            With myWorker
                .Firstname = firstname
                .Lastname = lastname
                .Username = username
                .Password = password
                .Phone = phone
                .Email = email
                .Role = SecurityRole.Worker
                .Status = status
                .Save()
                Return True
            End With
        Finally
            If myWorker IsNot Nothing Then myWorker = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Reactivates the worker to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function reactivateWorker(ByVal workerID As Integer) As Boolean
        Dim myWorker As New User
        Try
            If workerID > 0 Then
                myWorker.Read(workerID)
            End If
            With myWorker
                .Status = TheStatus.Active
                .Save()
                Return True
            End With
        Finally
            If myWorker IsNot Nothing Then myWorker = Nothing
        End Try
    End Function

    Private Sub btnRefreshWorkers_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefreshWorkers.ServerClick
        ''Security.updateMenu(Me.Master)
        getWorkers()
        Me.upWorkers.DataBind()
    End Sub

End Class