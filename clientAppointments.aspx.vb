Imports System.Web.Services
Imports System.Web.Script.Services

Public Class clientAppointments : Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Security.UserRole = Integer.Parse(SecurityRole.Admin) Or Security.UserRole = Integer.Parse(SecurityRole.Worker) Or Security.UserRole = "" Then
            Security.UserName = Nothing
            Security.Password = Nothing
            Security.UserRole = Nothing
            Response.Redirect("~\login.aspx")
        End If
        If Not IsPostBack Then
            getAppointments()
        End If
    End Sub

    ''' <summary>
    ''' Loads the list of appointments
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getAppointments()
        'Dim theAppointments As New List(Of Appointment)
        'Dim myAppointments As New Appointments
        'Try
        '    theAppointments = myAppointments.Read()
        '    Me.AppointmentsList.DataSource = theAppointments
        '    Me.AppointmentsList.DataBind()
        'Finally
        '    If myAppointments IsNot Nothing Then myAppointments = Nothing
        '    If theAppointments IsNot Nothing Then theAppointments = Nothing
        'End Try
    End Sub

    Public Function getThreatment(ByVal threatmentID As Integer) As String
        Dim myThreatment As New Treatment(threatmentID)
        Dim threatmentInfo As New StringBuilder(String.Empty)
        threatmentInfo.Append(String.Format("{0}<br />{1}", myThreatment.Name, myThreatment.Description))
        Return threatmentInfo.ToString
    End Function

    Public Function getClinic(ByVal clinicID As Integer) As String
        Dim myClinic As New Clinic(clinicID)
        Dim clinicInfo As New StringBuilder(String.Empty)
        clinicInfo.Append(String.Format("{0} - {1}, {2}, {3} {4}<br />PH: {5}", myClinic.Name, myClinic.Address, myClinic.City, myClinic.State, myClinic.Postcode, myClinic.Phone))
        Return clinicInfo.ToString
    End Function
End Class