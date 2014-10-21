Imports System.Web.Services
Imports System.Web.Script.Services

Public Class _clinics : Inherits System.Web.UI.Page

    ''' <summary>
    ''' 'Sets the ID for the selected clinic
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property ClinicID As Integer
        Get
            Dim ID As Integer = 0
            If Me.ViewState("ClinicID") IsNot Nothing Then
                Integer.TryParse(Me.ViewState("ClinicID"), ID)
            End If
            Return ID
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("ClinicID") = value
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
            getClinics()
        End If
    End Sub

    ''' <summary>
    ''' Gets list of clinics
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getClinics()
        Dim theClinics As New List(Of Clinic)
        Dim myClinics As New Clinics
        Try
            theClinics = myClinics.Read()
            Me.ClinicsList.DataSource = theClinics
            Me.ClinicsList.DataBind()
        Finally
            If myClinics IsNot Nothing Then myClinics = Nothing
            If theClinics IsNot Nothing Then theClinics = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Load clinic details
    ''' </summary>
    ''' <param name="clinicID">Integer: the ID of the selected clinic</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadClinicDetails(ByVal clinicID As Integer) As Clinic
        Dim myClinic As New Clinic(clinicID)
        Try
            Return myClinic
        Finally
            If myClinic IsNot Nothing Then myClinic = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Load Rooms
    ''' </summary>
    ''' <param name="clinicID">Integer: the ID of the selected clinic</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadRooms(ByVal clinicID As Integer) As ClinicRooms
        Dim myRooms As New ClinicRooms
        Try
            Return myRooms.ReadByClinicID(clinicID)
        Finally
            If myRooms IsNot Nothing Then myRooms = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Saves the clinic to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveClinicDetails(ByVal clinicID As Integer, ByVal name As String, ByVal address As String, ByVal city As String, ByVal state As String, ByVal postcode As String, ByVal phone As String, ByVal fax As String, ByVal email As String, ByVal status As Integer) As Boolean
        Dim myClinic As New Clinic
        Try
            If clinicID > 0 Then
                myClinic.Read(clinicID)
            End If
            With myClinic
                .Name = name
                .Address = address
                .City = city
                .State = state
                .Postcode = postcode
                .Phone = phone
                .Fax = fax
                .Email = email
                .Status = status
                .Save()
                Return True
            End With
        Finally
            If myClinic IsNot Nothing Then myClinic = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Reactivates the clinic to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function reactivateClinic(ByVal clinicID As Integer) As Boolean
        Dim myClinic As New Clinic
        Try
            If clinicID > 0 Then
                myClinic.Read(clinicID)
            End If
            With myClinic
                .Status = TheStatus.Active
                .Save()
                Return True
            End With
        Finally
            If myClinic IsNot Nothing Then myClinic = Nothing
        End Try
    End Function

    Private Sub btnRefreshClinics_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefreshClinics.ServerClick
        getClinics()
        Me.upClinics.DataBind()
    End Sub

    ''' <summary>
    ''' Displays the clinic rooms and their schedule on the info page
    ''' </summary>
    ''' <param name="ID">Integer: the clinicID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getOpenHours(ByVal ID As Integer) As String
        Dim result As New StringBuilder(String.Empty)
        Dim myRooms As New ClinicRooms
        myRooms.Filter = String.Format("ClinicID = {0}", ID)
        Dim theRooms As New List(Of ClinicRoom)
        Dim myWorkdays As New Availabilities
        Dim theWorkdays As New List(Of Availability)
        theRooms = myRooms.Read()
        If theRooms.Count > 0 Then
            result.Append("<table>")
            For Each theRoom As ClinicRoom In theRooms
                result.Append(String.Format("<tr><td colspan=""2""><strong>{0}</strong></td></tr>", theRoom.Name))
                myWorkdays.Filter = String.Format("RoomID = {0}", theRoom.ID)
                myWorkdays.OrderBy = "Workday"
                theWorkdays = myWorkdays.Read()
                If theWorkdays.Count > 0 Then
                    For Each theWorkday As Availability In theWorkdays
                        Dim myWorkDay As Integer = theWorkday.Workday + 1
                        If myWorkDay > 7 Then myWorkDay = 1
                        result.Append(String.Format("<tr><td>{0}: </td><td>{1} - {2}</td></tr>", WeekdayName(myWorkDay), theWorkday.StartTime.ToString("h:mm tt"), theWorkday.EndTime.ToString("h:mm tt")))
                    Next
                End If
            Next
            result.Append("</table>")
        End If
        Return result.ToString
    End Function

    ''' <summary>
    ''' Gets the details of the selected room
    ''' </summary>
    ''' <param name="roomID">Integer</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function getRoomDetails(ByVal roomID As Integer) As ClinicRoom
        Dim theRoom As New ClinicRoom(roomID)
        Return theRoom
    End Function

    ''' <summary>
    ''' Saves the room details to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveRoomDetails(ByVal clinicID As Integer, ByVal roomID As Integer, ByVal name As String, ByVal status As Integer) As ClinicRoom
        Dim myRoom As New ClinicRoom
        Try
            If roomID > 0 Then
                myRoom.Read(roomID)
            End If
            With myRoom
                .ClinicID = clinicID
                .Name = name
                .Status = status
                .Save()
                .ID = .ID
                Return myRoom
            End With
        Finally
            If myRoom IsNot Nothing Then myRoom = Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="roomID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function getRoomAvailability(ByVal roomID As Integer) As Availabilities
        Dim myAvailability As New Availabilities
        Dim theAvailability As New Availabilities
        Try
            theAvailability = myAvailability.ReadByRoomID(roomID)
            Return theAvailability
        Catch ex As Exception

        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="availabilityID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function getAvailability(ByVal availabilityID As Integer) As Availability
        Dim myAvailability As New Availability(availabilityID)
        Return myAvailability
    End Function

    ''' <summary>
    ''' Saves the room details to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveAvailability(ByVal availabilityID As Integer, ByVal roomID As Integer, ByVal startTime As Date, ByVal endTime As Date, ByVal workday As Integer) As Availability
        Dim myAvailability As New Availability
        Try
            If availabilityID > 0 Then
                myAvailability.Read(availabilityID)
            End If
            With myAvailability
                .RoomID = roomID
                .StartTime = startTime
                .EndTime = endTime
                .Workday = workday
                .Save()
                Return myAvailability
            End With
        Finally
            If myAvailability IsNot Nothing Then myAvailability = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Deletes the room details to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function deleteAvailability(ByVal availabilityID As Integer) As Integer
        Dim myAvailability As New Availability(availabilityID)
        Try
            myAvailability.Delete()
            Return myAvailability.RoomID
        Finally
            If myAvailability IsNot Nothing Then myAvailability = Nothing
        End Try
    End Function
End Class