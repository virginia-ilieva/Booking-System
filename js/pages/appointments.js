//<![CDATA[
$(document).ready(function () {
    setPage();
    openCalendar(true);
})
function setPage() {
    getClinicOptions();
    setNavigation();
}
function openCalendar(newWindow) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            if (newWindow) {
                $('#showCalendar').click();
            }
            $('#appCalendar').html('');
            $('#appCalendar').fullCalendar({
                header: { left: 'today prev,next', center: 'title', right: 'month,agendaWeek,agendaDay' },
                defaultView: 'agendaWeek', firstDay: 1,
                minTime: 7, maxTime: 18, allDaySlot: false,
                events: function (start, end, callback) {
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: '/OnlineBooking/appointments.aspx/getAppointments',
                        contentType: "application/json; charset=utf-8",
                        data: '{ clinicID: ' + $('#Clinics').val() + ', startDate: "' + start + '", endDate: "' + end + '"}',
                        dataType: "json",
                        success: function (data) {
                            var json = data.d;
                            var events = [];
                            for (record in json) {
                                var starTime = new Date(json[record].StartTimeString);
                                console.log(starTime);
                                var endTime = new Date(json[record].EndTimeString);
                                console.log(endTime);
                                events.push({
                                    id: json[record].ID,
                                    start: starTime,
                                    end: endTime,
                                    title: json[record].Firstname + " " + json[record].Lastname + ", " + json[record].Phone + ", " + json[record].Email + ", " + json[record].TherapyName + ", " + json[record].ClinicName + ", " + json[record].Comments,
                                    allDay: false
                                });
                            }
                            callback(events);
                        }
                    });
                },
                eventClick: function (calEvent, jsEvent, view) {
                    $('#btnAppointment').click();
                    $('#ClientSearch').addClass('hidden');
                    loadPageElements();
                    loadAppointment(calEvent.id);
                },
                dayClick: function (date, allDay, jsEvent, view) {
                    $('#btnAppointment').click();
                    $('#ClientSearch').removeClass('hidden');
                    $('#clientAppointment').addClass('hidden');
                    $('#appointmentID').val(0);
                    $('#AppDate').val(dateFormat(date));
                    $('#StartTime').val(timeFormat(date));
                    loadPageElements();
                }
            })
            jQuery('body').hideLoading();
        }
    );
    //$('.fc-header-center tr').prepend('<td><h2 class="fc-header-title" style="padding-right:10px;"></h2></td>');
    //getClinicName();
}
function getClinicName() { 
    var myClinicName = $('#Clinics :selected').text();
    if (myClinicName == 'Select clinic') { myClinicName = 'Clinic not selected'; }
    $('.fc-header-center tr td:first h2').html('').append(myClinicName);
}
function getClinicOptions() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/appointments.aspx/getClinicList',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
            var options = '<option value="0">Select clinic</option>';
            for (record in json) {
                options += '<option value="' + json[record].ID + '">' + json[record].Name + '</option>';
            }
            $('#Clinics').html(options);
        }
    });
}
function loadPageElements() {
    $('#AppDate').datepicker({ dateFormat: 'dd-M-yy' });
    $('#StartTime').timepicker({ ampm: true, stepMinute: 15 });
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/clients.aspx/getClinicList',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
            var options = '<option value="">Select one</option>';
            for (record in json) {
                options += '<option value="' + json[record].ID + '">' + json[record].Name + '</option>';
            }
            $('#AppClinics').html(options);
        }
    });
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/clients.aspx/getTreatmentsList',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
            var options = '<option value="">Select one</option>';
            for (record in json) {
                options += '<option value="' + json[record].ID + '">' + json[record].Name + '</option>';
            }
            $('#Therapies').html(options);
        }
    });
}
function loadAppointment(id) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/appointments.aspx/loadAppointment',
                contentType: "application/json; charset=utf-8",
                data: '{ appointmentID: ' + id + '}',
                dataType: "json",
                success: function (data) {
                    var json = data.d;
                    $('#appointmentID').val(json.ID);
                    $('#AppDate').val(stringDateFormat(json.AppointmentDateString));
                    $('#StartTime').val(stringTimeFormat(json.StartTimeString));
                    $('#AppClinics').val(json.ClinicID);
                    $('#Therapies').val(json.ThreatmentID);
                    $('#Status').val(json.Status);
                    $('#Comments').val(json.Comments);
                    clientID = json.UserID;
                }
            });
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clients.aspx/loadClientDetails',
                data: '{clientID: ' + clientID + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = (data.d); 
                    $('#clientID').val(json.ID);
                    $('#AppFirstname').val(json.Firstname);
                    $('#AppLastname').val(json.Lastname);
                }
            });
            jQuery('body').hideLoading();
        }
    );
    var clientID;
}
function getClients() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/appointments.aspx/getClients',
                data: '{ firstname: "' + $('#SearchFirstname').val() + '", lastname: "' + $('#SearchLastname').val() + '", phone: "' + $('#SearchPhone').val() + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#searchResults').html(data.d);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
function createUserAppointment(clientID, firstname, lastname) {
    $('#clientID').val(clientID);
    $('#AppFirstname').val(firstname);
    $('#AppLastname').val(lastname);
    $('#ClientSearch').addClass('hidden');
    $('#clientAppointment').removeClass('hidden');
}
function createAppointment() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clients.aspx/createAppointment',
                data: '{ appointmentID: ' + $('#appointmentID').val() + ', clientID: ' + $('#clientID').val() + ', clinicID:' + $('#AppClinics').val() + ', treatmentID:' + $('#Therapies').val() + ', appDate:"' + $('#AppDate').val() + '", startTime:"' + $('#StartTime').val() + '",comments:"' + $('#ctl00_mainpage_Comments').val() + '", status: ' + $('#Status').val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = (data.d);
                    alert(json);
                    $('#appointment_popup').fadeOut(function () {
                        $('#appointment_popup a.close').remove();  //fade them both out
                        $("#appointment_popup .formError").each(function () { $(this).remove(); }) // remove errors
                    });
                    openCalendar(false);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
//]]>