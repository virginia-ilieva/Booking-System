Imports System.Web.Services
Imports System.Web.Script.Services

Public Class treatments1 : Inherits System.Web.UI.Page

    ''' <summary>
    ''' 'Sets the ID for the selected treatment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property TreatmentID As Integer
        Get
            Dim ID As Integer = 0
            If Me.ViewState("TreatmentID") IsNot Nothing Then
                Integer.TryParse(Me.ViewState("TreatmentID"), ID)
            End If
            Return ID
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("TreatmentID") = value
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
            getTreatments()
        End If
    End Sub

    ''' <summary>
    ''' Loads the list of treatments
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getTreatments()
        Dim theTreatments As New List(Of Treatment)
        Dim myTreatments As New Treatments
        Try
            theTreatments = myTreatments.Read()
            Me.TreatmentsList.DataSource = theTreatments
            Me.TreatmentsList.DataBind()
        Finally
            If myTreatments IsNot Nothing Then myTreatments = Nothing
            If theTreatments IsNot Nothing Then theTreatments = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Load treatment details
    ''' </summary>
    ''' <param name="treatmentID">Integer: the ID of the selected treatment</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadTreatmentDetails(ByVal treatmentID As Integer) As Treatment
        Dim myTreatment As New Treatment(treatmentID)
        Try
            Return myTreatment
        Finally
            If myTreatment IsNot Nothing Then myTreatment = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Saves the treatment to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveTreatmentDetails(ByVal treatmentID As Integer, ByVal name As String, ByVal duration As Integer, ByVal description As String, ByVal price As Double, ByVal status As Integer) As Boolean
        Dim myTreatment As New Treatment
        Try
            If treatmentID > 0 Then
                myTreatment.Read(treatmentID)
            End If
            With myTreatment
                .Name = name
                .Duration = duration
                .Description = description
                .Price = price
                .Status = status
                .Save()
                Return True
            End With
        Finally
            If myTreatment IsNot Nothing Then myTreatment = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Reactivates the treatment to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function reactivateTreatment(ByVal treatmentID As Integer) As Boolean
        Dim myTreatment As New Treatment
        Try
            If treatmentID > 0 Then
                myTreatment.Read(treatmentID)
            End If
            With myTreatment
                .Status = TheStatus.Active
                .Save()
                Return True
            End With
        Finally
            If myTreatment IsNot Nothing Then myTreatment = Nothing
        End Try
    End Function

    Private Sub btnRefreshTreatments_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefreshTreatments.ServerClick
        ''Security.updateMenu(Me.Master)
        getTreatments()
        Me.upTreatments.DataBind()
    End Sub
End Class