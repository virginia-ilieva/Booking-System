Imports System.Web.Services
Imports System.Web.Script.Services

Public Class _appointments : Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Security.UserRole = Integer.Parse(SecurityRole.Client) Or Security.UserRole = "" Then
            Security.UserName = Nothing
            Security.Password = Nothing
            Security.UserRole = Nothing
            Response.Redirect("~\login.aspx")
        End If
    End Sub

    <WebMethod()>
    Public Shared Function getClinicList() As Clinics
        Dim myClinics As New Clinics
        Dim theClinics As New Clinics
        Try
            myClinics.OrderBy = "Name"
            theClinics = myClinics.Read()
            Return theClinics
        Finally
            If myClinics IsNot Nothing Then myClinics = Nothing
            If theClinics IsNot Nothing Then theClinics = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Shared Function getAppointments(ByVal clinicID As Integer, ByVal startDate As String, ByVal endDate As String) As AppointmentEvents
        Dim myAppointments As New AppointmentEvents
        Dim Filter As New StringBuilder(String.Empty)
        Try
            If clinicID > 0 Then
                Filter.Append(String.Format(" AND ClinicID = {0}", clinicID))
            End If
            Dim startPeriod As New Date(startDate.Substring(11, 4), _appointments.getMonth(startDate.Substring(4, 3)), startDate.Substring(8, 2))
            Dim endPeriod As New Date(endDate.Substring(11, 4), _appointments.getMonth(endDate.Substring(4, 3)), endDate.Substring(8, 2))
            Filter.Append(String.Format(" AND AppointmentDate BETWEEN '{0}' AND '{1}'", startPeriod, endPeriod))
            myAppointments.Filter = Filter.ToString.Substring(5)
            Dim theAppointments As New AppointmentEvents
            theAppointments = myAppointments.Read()
            Return theAppointments
        Catch ex As Exception

        Finally
            If myAppointments IsNot Nothing Then myAppointments = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Shared Function loadAppointment(ByVal appointmentID As Integer) As Appointment
        Dim myAppointment As New Appointment(appointmentID)
        Return myAppointment
    End Function

    <WebMethod()>
    Public Shared Function getClients(ByVal firstname As String, ByVal lastname As String, ByVal phone As String) As String
        Dim returnString As String = ""
        Dim theClients As New Users
        Dim myClients As New Users
        Try
            Dim clientsFilter As New StringBuilder(String.Empty)
            Dim fieldsFilter As String = ""
            If Not firstname = "" Then
                fieldsFilter += String.Format(" AND Firstname LIKE '%{0}%'", firstname)
            End If
            If Not lastname = "" Then
                fieldsFilter += String.Format(" AND Lastname LIKE '%{0}%'", lastname)
            End If
            If Not phone = "" Then
                fieldsFilter += String.Format(" AND Phone LIKE '%{0}%'", phone)
            End If
            If fieldsFilter.Length > 0 Then
                clientsFilter.Append(String.Format(" AND ( {0} )", fieldsFilter.Substring(5)))
            Else
                clientsFilter.Append(" AND ID = 0")
            End If
            myClients.OrderBy = "Firstname, Lastname"
            myClients.Filter = String.Format("Role = {0} AND Status = {1} {2}", Integer.Parse(SecurityRole.Client), Integer.Parse(TheStatus.Active), clientsFilter.ToString)
            theClients = myClients.Read()
            For Each theClient As User In theClients
                returnString += String.Format("<div class=""acc_trigger""><table id=""clientHeader""><tr><td style=""width:385px;""><span>{0}</span></td><td class=""right"" style=""width:40px;""><input type=""button"" title=""Edit client."" id=""btnEdit"" class=""gridButton edit"" onclick=""createUserAppointment({1}, '{2}', '{3}');"" /></a></td></tr></table></div>", theClient.Fullname, theClient.ID, theClient.Firstname, theClient.Lastname)
            Next
        Finally
            If myClients IsNot Nothing Then myClients = Nothing
            If theClients IsNot Nothing Then theClients = Nothing
        End Try
        Return returnString
    End Function

    Shared Function getMonth(ByVal month As String) As Integer
        Dim returnMonth As Integer
        Select Case (month)
            Case Is = "Jan"
                returnMonth = 1
            Case Is = "Feb"
                returnMonth = 2
            Case Is = "Mar"
                returnMonth = 3
            Case Is = "Apr"
                returnMonth = 4
            Case Is = "May"
                returnMonth = 5
            Case Is = "Jun"
                returnMonth = 6
            Case Is = "Jul"
                returnMonth = 7
            Case Is = "Aug"
                returnMonth = 8
            Case Is = "Sep"
                returnMonth = 9
            Case Is = "Oct"
                returnMonth = 10
            Case Is = "Nov"
                returnMonth = 11
            Case Is = "Dec"
                returnMonth = 12
        End Select
        Return returnMonth
    End Function
End Class