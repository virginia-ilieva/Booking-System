Imports System.Web.Mail
Imports System.Web.Services
Imports System.Web.Script.Services

Public Class manageClients : Inherits System.Web.UI.Page

    ''' <summary>
    ''' Runs every time the page is loaded
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Security.UserRole = Integer.Parse(SecurityRole.Client) Or Security.UserRole = "" Then
            Security.UserName = Nothing
            Security.Password = Nothing
            Security.UserRole = Nothing
            Response.Redirect("~\login.aspx")
        End If
        If Not IsPostBack Then
            Me.SearchFirstname.Value = ""
            Me.SearchLastname.Value = ""
            Me.SearchPhone.Value = ""
            Me.manageClientsContainer.Visible = False
            getClients()
        Else
            Me.manageClientsContainer.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Loads a list of the existing clients
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getClients()
        Dim theClients As New Users
        Dim myClients As New Users
        Try
            Dim clientsFilter As New StringBuilder(String.Empty)
            Dim fieldsFilter As String = ""
            If Not Me.SearchFirstname.Value = "" Then
                fieldsFilter += String.Format(" AND Firstname LIKE '%{0}%'", SearchFirstname.Value)
            End If
            If Not Me.SearchLastname.Value = "" Then
                fieldsFilter += String.Format(" AND Lastname LIKE '%{0}%'", SearchLastname.Value)
            End If
            If Not Me.SearchPhone.Value = "" Then
                fieldsFilter += String.Format(" AND Phone LIKE '%{0}%'", SearchPhone.Value)
            End If
            If fieldsFilter.Length > 0 Then
                clientsFilter.Append(String.Format(" AND ( {0} )", fieldsFilter.Substring(5)))
            Else
                clientsFilter.Append(" AND ID = 0")
            End If
            myClients.OrderBy = "Firstname, Lastname"
            myClients.Filter = String.Format("Role = {0} AND Status = {1} {2}", Integer.Parse(SecurityRole.Client), Integer.Parse(TheStatus.Active), clientsFilter.ToString)
            theClients = myClients.Read()
            Me.ClientsList.DataSource = theClients
            Me.ClientsList.DataBind()
        Finally
            If myClients IsNot Nothing Then myClients = Nothing
            If theClients IsNot Nothing Then theClients = Nothing
        End Try
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
    Public Shared Function getTreatmentsList() As Treatments
        Dim myTreatments As New Treatments
        Dim theTreatments As New Treatments
        Try
            myTreatments.OrderBy = "Name"
            theTreatments = myTreatments.Read()
            Return theTreatments
        Finally
            If myTreatments IsNot Nothing Then myTreatments = Nothing
            If theTreatments IsNot Nothing Then theTreatments = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Load client details
    ''' </summary>
    ''' <param name="clientID">Integer: the ID of the selected client</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadClientDetails(ByVal clientID As Integer) As User
        Dim myClient As New User(clientID)
        Try
            Return myClient
        Finally
            If myClient IsNot Nothing Then myClient = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Saves the client to the database
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveClientDetails(ByVal clientID As Integer, ByVal firstname As String, ByVal lastname As String, ByVal address As String, ByVal city As String, ByVal state As String, ByVal postcode As String, ByVal phone As String, ByVal email As String, ByVal username As String, ByVal dob As Date, ByVal contactName As String, ByVal contactPhone As String, ByVal relation As String, ByVal newsletter As Boolean) As String
        Dim result As String = checkForExistingUser(firstname, lastname, email, username, clientID)
        If result = "" Then
            Dim myClient As New User
            Try
                If clientID > 0 Then
                    myClient.Read(clientID)
                End If
                With myClient
                    .Firstname = firstname
                    .Lastname = lastname
                    .Address = address
                    .City = city
                    .State = state
                    .Postcode = postcode
                    .Phone = phone
                    .Email = email
                    .Username = username
                    .Password = CreateRandomPassword(8)
                    .DOB = dob
                    .ContactName = contactName
                    .ContactPhone = contactPhone
                    .ContactRelation = relation
                    .Newsletter = newsletter
                    .SignupDate = Today
                    .Status = TheStatus.Active
                    .Role = SecurityRole.Client
                    .Save()
                End With
                If clientID = 0 Then
                    SendEmail(myClient.Username, myClient.Password, myClient.Email, myClient.Firstname)
                End If
            Finally
                If myClient IsNot Nothing Then myClient = Nothing
            End Try
        End If
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="PasswordLength">Integer: the length of the newly created password</param>
    ''' <returns>String: the newly created password</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateRandomPassword(ByVal PasswordLength As Integer) As String
        Dim _allowedChars As String = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789"
        Dim randomNumber As New Random()
        Dim chars(PasswordLength - 1) As Char
        Dim allowedCharCount As Integer = _allowedChars.Length
        For i As Integer = 0 To PasswordLength - 1
            chars(i) = _allowedChars.Chars(CInt(Fix((_allowedChars.Length) * randomNumber.NextDouble())))
        Next i
        Return New String(chars)
    End Function

    ''' <summary>
    ''' Check if a user with the same details already exists in the database
    ''' </summary>
    ''' <param name="firstname">String</param>
    ''' <param name="lastname">String</param>
    ''' <param name="email">String</param>
    ''' <param name="username">String</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Shared Function checkForExistingUser(ByVal firstname As String, ByVal lastname As String, ByVal email As String, ByVal username As String, ByVal userID As Integer) As String
        Dim result As String = ""
        Dim myUsers As New Users
        Dim theUsers As New List(Of User)
        myUsers.Filter = String.Format("LOWER(Firstname) = '{0}' AND LOWER(Lastname) = '{1}' AND LOWER(Email) = '{2}' AND ID <> {3}", firstname.ToLower, lastname.ToLower, email.ToLower, userID)
        theUsers = myUsers.Read()
        If theUsers.Count > 0 Then
            result = "email"
            Return result
            Exit Function
        End If
        myUsers.Filter = String.Format("LOWER(Username) = '{0}' AND ID <> {1}", username.ToLower, userID)
        theUsers = myUsers.Read()
        If theUsers.Count > 0 Then
            result = "username"
            Return result
            Exit Function
        End If
        Return result
    End Function

    Public Shared Sub SendEmail(ByVal userName As String, ByVal password As String, ByVal email As String, ByVal firstName As String)

        ''Creates new instance of the mail message class
        'Dim mailMessage As MailMessage = New MailMessage()

        ''Add values to the mail message
        'mailMessage.IsBodyHtml = True
        'mailMessage.From = New MailAddress("virginia_ilieva@yahoo.com")
        'mailMessage.To.Add(New MailAddress(email))
        'mailMessage.Subject = "Online booking system - Login details"
        'mailMessage.Body = "Hello " & firstName & ".<br /><br />A new account for our online booking system was created. Please, find your login details below:<br /><br />Username - " & userName & "<br />Password - " & password & "<br /><br />If you want to login now, please <a href=""http://localhost:1032/login.aspx"">click here.</a><br /><br />Kind regards,<br /><br />Ayurveda House"

        'Try
        '    'Sends message to the client
        '    Dim smtpClient As SmtpClient = New SmtpClient("smtp.three.com.au")
        '    smtpClient.Send(mailMessage)
        'Catch ex As Exception

        'End Try

    End Sub

    Private Sub btnRefreshClients_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefreshClients.ServerClick
        getClients()
        Me.upClients.DataBind()
    End Sub

    Private Sub btnSearch_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.ServerClick
        getClients()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function createAppointment(ByVal appointmentID As Integer, ByVal clientID As Integer, ByVal clinicID As Integer, ByVal treatmentID As Integer, ByVal appDate As Date, ByVal startTime As Date, ByVal comments As String, ByVal status As Integer) As String
        Dim result As String
        Try
            result = manageClients.checkClinicAvailability(clinicID, treatmentID, appDate, startTime, appointmentID)
            If result = "Available" Then
                Dim myAppointment As Appointment
                If appointmentID > 0 Then
                    myAppointment = New Appointment(appointmentID)
                Else
                    myAppointment = New Appointment
                End If
                Dim myTreatment As New Treatment(treatmentID)
                myAppointment.UserID = clientID
                myAppointment.ClinicID = clinicID
                myAppointment.ThreatmentID = treatmentID
                myAppointment.AppointmentDate = appDate
                Dim theStartDate As New Date(appDate.Year, appDate.Month, appDate.Day, startTime.Hour, startTime.Minute, startTime.Second)
                myAppointment.StartTime = theStartDate
                myAppointment.EndTime = myAppointment.StartTime.AddMinutes(myTreatment.Duration)
                myAppointment.Comments = comments
                myAppointment.Status = status
                myAppointment.Save()
                If myAppointment.Status = 1 Then

                    Dim myClient As New User(clientID)
                    Dim myClinic As New Clinic(clinicID)
                    Dim myAdmins As New Users
                    Dim theAdmins As New Users
                    myAdmins.Filter = String.Format("Role = {0}", Integer.Parse(SecurityRole.Admin))
                    theAdmins = myAdmins.Read()
                    'Creates new instance of the mail message class
                    Dim mailMessage As New MailMessage()
                    Dim confirmMessage As New MailMessage()
                    'Add values to the mail message
                    mailMessage.To = myClient.Email
                    mailMessage.From = theAdmins.Item(0).Email
                    mailMessage.Bcc = theAdmins.Item(0).Email
                    mailMessage.Subject = "Ayurveda clinic - Appointment details"
                    mailMessage.Body = "Appointment details:  " & Environment.NewLine & Environment.NewLine & "Date and time: " & myAppointment.AppointmentDate.ToString("dd-MMM-yy") & " " & myAppointment.StartTime.ToString("hh:mm tt") & Environment.NewLine & "Clinic details: " & myClinic.Name & " - " & myClinic.Address & "," & myClinic.City & "," & myClinic.State & " " & myClinic.Postcode & Environment.NewLine & myClinic.Phone & ", " & myClinic.Email
                    'Sends message to the support teem and confirnation to the client
                    SmtpMail.SmtpServer = "127.0.0.1"
                    SmtpMail.Send(mailMessage)
                    result = "The appointment was created successfully. An email with the appointment's details was sent to the client's email address."
                Else
                    result = "The appointment was canceled successfully"
                End If
            End If
            Return result
        Finally

        End Try
    End Function

    Private Shared Function checkClinicAvailability(ByVal clinicID As Integer, ByVal treatmentID As Integer, ByVal appDate As Date, ByVal time As Date, ByVal appointmentID As Integer) As String
        Dim myTreatment As New Treatment(treatmentID)
        Dim myClinic As New Clinic(clinicID)
        Dim myRooms As New ClinicRooms()
        Dim result As String = ""
        Dim workHours As Boolean = False
        Dim workdayResult As Boolean = False
        Dim rooms As Integer = 0
        Try
            '' Gets the available rooms for the selected clinic
            myRooms.Filter = String.Format("ClinicID = {0} AND Status = {1}", clinicID, Integer.Parse(TheStatus.Active))
            Dim theRooms As New ClinicRooms
            theRooms = myRooms.Read()
            '' Loops through the rooms
            For Each theRoom As ClinicRoom In theRooms
                Dim myAvailabilities As New Availabilities
                Dim theAvailabilities As New Availabilities
                theAvailabilities = myAvailabilities.ReadByRoomID(theRoom.ID)
                '' Loops through the available working days for the room
                For Each theAvailability As Availability In theAvailabilities
                    If theAvailability.Workday = Integer.Parse(appDate.DayOfWeek) Then
                        '' If the room is open on this day checks if the appointment is within the working hours of the room
                        workdayResult = True
                        If theAvailability.StartTime.TimeOfDay < time.TimeOfDay And theAvailability.EndTime.TimeOfDay > time.AddMinutes(myTreatment.Duration).TimeOfDay Then
                            workHours = True
                            rooms += 1
                            Exit For
                        Else
                            workHours = False
                        End If
                    Else
                        workdayResult = False
                    End If
                Next
            Next
            '' Checks if there are any rooms available for the selected day and time
            If rooms > 0 Then
                '' If there are rooms available it filters the appointments in this clinis scheduled within the timeframe for the new appointment
                Dim myAppointments As New Appointments
                Dim theAppointments As New Appointments
                myAppointments.Filter = String.Format("(ClinicID = {0} AND AppointmentDate = '{1:yyyy-MM-dd}' AND StartTime BETWEEN '{1:yyyy-MM-dd} {2:hh:mm tt}' AND '{1:yyyy-MM-dd} {3:hh:mm tt}' OR EndTime BETWEEN '{1:yyyy-MM-dd} {2:hh:mm tt}' AND '{1:yyyy-MM-dd} {3:hh:mm tt}') AND ID <> {4}", clinicID, appDate, time, time.AddMinutes(myTreatment.Duration), appointmentID)
                theAppointments = myAppointments.Read()
                If theAppointments.Count >= rooms Then
                    '' If there are more appointments scheduled than rooms available shows error message
                    result = String.Format("The clinic is not available in the selected time. Please select another time or call {0} for help", myClinic.Phone)
                Else
                    '' If there is available time for the new appointment it saves it
                    result = "Available"
                End If
            Else
                '' If there are no rooms available shows error message
                If workdayResult = False Then
                    result = String.Format("The clinic is not open on this day. Please select another day of the week or call {0} for help", myClinic.Phone)
                Else
                    result = String.Format("The clinic is not open at the selected time. Please select another time or call {0} for help", myClinic.Phone)
                End If
            End If
            Return result
        Catch ex As Exception

        Finally
            If myTreatment IsNot Nothing Then myTreatment = Nothing
            If myClinic IsNot Nothing Then myClinic = Nothing
            If myRooms IsNot Nothing Then myRooms = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Load client case history
    ''' </summary>
    ''' <param name="clientID">Integer: the ID of the selected client</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function loadClientCaseHistory(ByVal clientID As Integer) As Appointments
        Dim myAppointments As New Appointments
        Try
            myAppointments.OrderBy = "AppointmentDate DESC"
            Return myAppointments.ReadByClientID(clientID)
        Finally
            If myAppointments IsNot Nothing Then myAppointments = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Gets the appointment case history and notes
    ''' </summary>
    ''' <param name="clientID">Integer: the ID of the selected appointment</param>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function getAppointmentDetails(ByVal clientID As Integer) As String
        Dim returnString As New StringBuilder(String.Empty)
        ''Dim myAppointment As New Appointment(appointmentID)
        Dim myClient As New User(clientID)
        ''Dim myTreatment As New Treatment(myAppointment.ThreatmentID)
        ''im myClinic As New Clinic(myAppointment.ClinicID)
        Dim myCaseHistories As New CaseHistories
        Try
            ''returnString.Append(String.Format("Client: {0}, Clinic: {1}, Therapy: {2}, <br /> Comments: {3} <br /><br />", myClient.Fullname, myClinic.Name, myTreatment.Name, myAppointment.Comments))
            Dim theCaseHistories As New CaseHistories
            theCaseHistories = myCaseHistories.ReadByClientID(clientID)
            If theCaseHistories.Count = 1 Then
                Dim myCaseHistory As CaseHistory = theCaseHistories.Item(0)
                Dim myCaseNotes As New CaseNotes
                Dim theCaseNotes As New CaseNotes
                theCaseNotes = myCaseNotes.ReadByCaseID(myCaseHistory.ID)
                Dim caseNotes As String = ""
                For Each theNote As CaseNote In theCaseNotes
                    Dim theUser As New User(theNote.UserID)
                    caseNotes += String.Format("<tr><td><strong>{0} - {1}</strong><br />{2}</td></tr>", theUser.Fullname, theNote.Created.ToString("dd-MMM-yy h:mm tt"), theNote.Notes)
                Next
                returnString.Append(String.Format("<table><tr><td><div id=""caseHistoryTabs""><ul><li><a href=""#caseSymptoms"">Symptoms</a></li><li><a href=""#casePractitioners"">Practitioners</a></li><li><a href=""#caseHistory"">History</a></li><li><a href=""#caseDiagnosis"">Diagnosis</a></li><li><a href=""#caseSuggestions"">Suggestions</a></li><li><a href=""#userNotes"">Review notes</a></li></ul><div id=""caseSymptoms""><table><tr><td class=""label""><strong>Date</strong></td><td>{20}</td></tr><tr><td class=""label""><strong>Symptoms</strong></td><td>{0}</td></tr></table></div><div id=""casePractitioners""><table><tr><td class=""label""><strong>Medications</strong></td><td>{1}</td></tr><tr><td class=""label""><strong>Practitioners</strong></td><td>{2}</td></tr><tr><td class=""label""><strong>Therapies</strong></td><td>{3}</td></tr></table></div><div id=""caseHistory""><table><tr><td class=""label""><strong>Health History</strong></td><td>{4}</td></tr><tr><td class=""label""><strong>Family History</strong></td><td>{5}</td></tr></table></div><div id=""caseDiagnosis""><table><tr><td class=""label""><strong>Prakrti</strong></td><td>{6}</td></tr><tr><td class=""label""><strong>Vikrti</strong></td><td>{7}</td></tr><tr><td class=""label""><strong>Agni</strong></td><td>{8}</td></tr><tr><td class=""label""><strong>Malas</strong></td><td>{9}</td></tr><tr><td class=""label""><strong>Ojas</strong></td><td>{10}</td></tr><tr><td class=""label""><strong>Manas</strong></td><td>{11}</td></tr><tr><td class=""label""><strong>Srotas</strong></td><td>{12}</td></tr><tr><td class=""label""><strong>Comments</strong></td><td>{13}</td></tr></table></div><div id=""caseSuggestions""><table><tr><td class=""label""><strong>Suggested Therapies</strong></td><td>{14}</td></tr><tr><td class=""label""><strong>Suggested Herbs</strong></td><td>{15}</td></tr><tr><td class=""label""><strong>Suggested Plan</strong></td><td>{16}</td></tr><tr><td class=""label""><strong>Suggested Cleanses</strong></td><td>{17}</td></tr></table></div><div id=""userNotes""><table id=""caseNotesTable"">{18}</table></div></div><div><label for=""caseNotes"" class=""label"">Review notes:</label><TEXTAREA id=""caseNotes"" NAME=""caseNotes"" ROWS=""10"" COLS=""20"" class=""textbox multiline"" ></TEXTAREA><div id=""saveNotes"" class=""right""><input type=""button"" id=""btnNotes"" title=""Save notes."" value=""Save"" class=""btn save"" onclick=""saveNotes({19});"" /></div></div></td></tr></table>", myCaseHistory.Symptoms, myCaseHistory.Medications, myCaseHistory.Practitioners, myCaseHistory.Therapies, myCaseHistory.HealthHistory, myCaseHistory.FamilyHistory, myCaseHistory.Prakrti, myCaseHistory.Vikrti, myCaseHistory.Agni, myCaseHistory.Malas, myCaseHistory.Ojas, myCaseHistory.Manas, myCaseHistory.Srotas, myCaseHistory.Comments, myCaseHistory.SuggestedTherapies, myCaseHistory.SuggestedHerbs, myCaseHistory.SuggestedPlan, myCaseHistory.SuggestedCleanses, caseNotes, myCaseHistory.ID, myCaseHistory.Created.ToString("dd-MMM-yy")))
            ElseIf theCaseHistories.Count = 0 Then
                returnString.Append(String.Format("<div class=""right""><input type=""button"" id=""btnNewCaseHistory"" title=""Create client's case history."" value=""Create Case History"" class=""btn add"" onclick=""$('#btnCreateCaseHistory').click(); showCaseHistoryPopup({0});"" /></div>", clientID))
            End If
            Return returnString.ToString
        Finally
            If myClient IsNot Nothing Then myClient = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Saves the client's case history to the database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveCaseHistory(ByVal clientID As Integer, ByVal caseDate As Date, ByVal symptoms As String, ByVal medications As String, ByVal practitioners As String, ByVal therapies As String, ByVal healthHistory As String, ByVal familyHistory As String, ByVal prakrti As String, ByVal vikrti As String, ByVal agni As String, ByVal malas As String, ByVal ojas As String, ByVal manas As String, ByVal srotas As String, ByVal comments As String, ByVal suggestedTherapies As String, ByVal suggestedHerbs As String, ByVal suggestedPlan As String, ByVal suggestedCleanses As String) As Boolean
        Dim result As Boolean = False
        Dim myCaseHistory As New CaseHistory
        Try
            myCaseHistory.UserID = Security.userID
            ''myCaseHistory.AppointmentID = appointmentID
            myCaseHistory.ClientID = clientID
            ''myCaseHistory.ThreatmentID = threatmentID
            If caseDate.Year = 1 Then
                caseDate = Today
            End If
            myCaseHistory.Created = caseDate
            myCaseHistory.Symptoms = symptoms
            myCaseHistory.Medications = medications
            myCaseHistory.Practitioners = practitioners
            myCaseHistory.Therapies = therapies
            myCaseHistory.HealthHistory = healthHistory
            myCaseHistory.FamilyHistory = familyHistory
            myCaseHistory.Prakrti = prakrti
            myCaseHistory.Vikrti = vikrti
            myCaseHistory.Agni = agni
            myCaseHistory.Malas = malas
            myCaseHistory.Ojas = ojas
            myCaseHistory.Manas = manas
            myCaseHistory.Srotas = srotas
            myCaseHistory.Comments = comments
            myCaseHistory.SuggestedTherapies = suggestedTherapies
            myCaseHistory.SuggestedHerbs = suggestedHerbs
            myCaseHistory.SuggestedPlan = suggestedPlan
            myCaseHistory.SuggestedCleanses = suggestedCleanses
            myCaseHistory.Save()
            result = True
        Finally
            If myCaseHistory IsNot Nothing Then myCaseHistory = Nothing
        End Try
        Return result
    End Function
    ''' <summary>
    ''' Saves case notes into the database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function saveNotes(ByVal caseID As Integer, ByVal notes As String) As String
        Dim returnString As String = ""
        Dim myCaseNote As New CaseNote
        Try
            myCaseNote.CaseID = caseID
            myCaseNote.UserID = Security.userID
            myCaseNote.Notes = notes
            myCaseNote.Created = Now
            myCaseNote.Save()
            Dim myCaseNotes As New CaseNotes
            Dim theCaseNotes As New CaseNotes
            theCaseNotes = myCaseNotes.ReadByCaseID(caseID)
            For Each theNote As CaseNote In theCaseNotes
                Dim theUser As New User(theNote.UserID)
                returnString += String.Format("<tr><td><strong>{0} - {1}</strong><br />{2}</td></tr>", theUser.Fullname, theNote.Created.ToString("dd-MMM-yy"), theNote.Notes)
            Next
        Finally
            If myCaseNote IsNot Nothing Then myCaseNote = Nothing
        End Try
        Return returnString
    End Function

End Class