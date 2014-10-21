//<![CDATA[
$(document).ready(function () {
    setPage();
})
var page = '';

function activateAccordion() { 
//Set default open/close settings
    $('.acc_container').hide(); //Hide/close all containers
    //On Click
    $('.acc_trigger').click(function () {
        if ($(this).next().is(':hidden')) { //If immediate next container is closed...
            $('.acc_trigger').removeClass('active').next().slideUp(); //Remove all "active" state and slide up the immediate next container
            $(this).toggleClass('active').next().slideDown(); //Add "active" state to clicked trigger and slide down the immediate next container
        }
        return false; //Prevent the browser jump to the link anchor
    });
}

function setPage() {
    activateAccordion();
    $('.Status').each(function () {
        if ($(this).html() == 1) {
            $(this).html('Active');
            $(this).parent().parent().find('.refresh').addClass('hidden');
        } else {
            $(this).html('Inactive');
        }
    })
    $(function () {
        var maxYear = new Date().getFullYear();
        var minYear = new Date().getFullYear();
        minYear = minYear - 90;
        var yearRange = minYear + ":" + maxYear
        $("#DOB").datepicker({ dateFormat: 'dd-M-yy', changeMonth: true, changeYear: true, yearRange: yearRange });
    });
    setNavigation();
    $('#tabs').tabs();
}
// Resets the fields on the popup window
function resetForm() {
    $('.textbox').val('');
    $('select').val(0);
}
// Activates the validation engine
function activateValidation(container) {
    $("#" + container).validationEngine({
        success: function () {
            if (page == 'client') {
                validateInput();
            } else if (page == 'appointment') {
                createAppointment();
            }
        },
        failure: false
    })
}
// Sets the validation rules for the select fileds
function validateSelect() {
    var result = true;
    if ($('#State').val() == 0) { result = false; }
    if ($('#Clinics').val() == 0) { result = false; }
    if ($('#Therapies').val() == 0) { result = false; }
    return result;
}
// Load client details
function loadClientDetails(id) {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/clients.aspx/loadClientDetails',
        data: '{clientID: ' + id + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
            $('#Firstname').val(json.Firstname);
            $('#Lastname').val(json.Lastname);
            $('#DOB').val(jsonDateFormat(json.DOB));
            $('#Address').val(json.Address);
            $('#City').val(json.City);
            $('#State').val(json.State);
            $('#Postcode').val(json.Postcode);
            $('#Username').val(json.Username);
            $('#Phone').val(json.Phone);
            $('#Email').val(json.Email);
            $('#ContactName').val(json.ContactName);
            $('#ContactPhone').val(json.ContactPhone);
            $('#Relation').val(json.ContactRelation);
        }
    });
}
// Validate the input against duplicate email or username
function validateInput() {
    if (!saveClientDetails()) {
        activateValidation('client_popup');
    } else {
        resetForm();
        alert("The user account was created successfully.");
    }
}
// Save client details
function saveClientDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clients.aspx/saveClientDetails',
                data: '{clientID: ' + $('#ctl00_mainpage_itemID').val() + ', firstname: "' + $('#Firstname').val() + '", lastname: "' + $('#Lastname').val() + '", dob: "' + $('#DOB').val() + '", address: "' + $('#Address').val() + '", city: "' + $('#City').val() + '", state: "' + $('#State').val() + '", postcode: "' + $('#Postcode').val() + '", username: "' + $('#Username').val() + '", phone: "' + $('#Phone').val() + '", email: "' + $('#Email').val() + '", contactName: "' + $('#ContactName').val() + '", contactPhone: "' + $('#ContactPhone').val() + '", relation: "' + $('#Relation').val() + '", newsletter: "' + $('#Newsletter').attr('checked') + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = data.d;
                    if (json == "username") {
                        $('#Username').val('');
                        $('#Password').val('');
                        $('#ConfirmPassword').val('');
                        alert("This username is already in use.");
                        return false;
                    } else if (json == "email") {
                        $('#Email').val('');
                        alert("The user with this details already exists.");
                        return false;
                    } else {
                        $('#ctl00_mainpage_btnRefreshClients').click();
                        $('a.close').click();
                        return true;
                    }
                },
                error: function () { return false; }
            });
            jQuery('body').hideLoading();
        }
    ); 
}
// Gets the appointment case history and notes
//function getAppointmentDetails(appointmentID, containerID) {
function getAppointmentDetails() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/clients.aspx/getAppointmentDetails',
        data: '{clientID: ' + $('#ctl00_mainpage_itemID').val() + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#CaseHistory').html(data.d);
            $('#caseHistoryTabs').tabs();
        }
    });
}
// Load client case history
//function loadClientCaseHistory(id) {
//    $.ajax({
//        type: "POST",
//        async: false,
//        url: '/OnlineBooking/clients.aspx/loadClientCaseHistory',
//        data: '{clientID: ' + id + '}',
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {
//            var json = (data.d);
//            var appointments = '';
//            $('#CaseHistory').html('');
//            for (record in json) {
//                appointments = '<div class="acc_trigger"><table id="appointmentHeader"><tr id="appointment-' + json[record].ID + '"><td style="width:900px;"><span>' + jsonDateFormat(json[record].AppointmentDate) + ' ' + jsonTimeFormat(json[record].StartTime) + '</span></td></tr></table></div><div class="acc_container" id="details-' + json[record].ID + '"></div>';
//                $('#CaseHistory').append(appointments);
//                getAppointmentDetails(json[record].ID, 'details-' + json[record].ID);
//            }
//        }
//    });
//}
function showPopup(id) {
    $('#ctl00_mainpage_itemID').val(id);
    if (id > 0) {
        jQuery('body').showLoading();
        jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', { },
            function () {
                loadClientDetails(id);
                //loadClientCaseHistory(id);
                getAppointmentDetails();
                activateAccordion();
                activateValidation('client_popup');
                $('#tabs ul').removeClass('hidden');
                jQuery('body').hideLoading();
            }
        );
        } else {
            activateValidation('client_popup');
            $('#tabs ul').addClass('hidden');
            resetForm();
    }
}
function showCaseHistoryPopup(appointmentID, clientID, threatmentID) {
    $('#createHistoryTabs').tabs();
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $('#caseAppointmentID').val(appointmentID);
            $('#caseClientID').val(clientID);
            $('#caseThreatmentID').val(threatmentID);
            $("#caseDate").datepicker({ dateFormat: 'dd-M-yy' });
            jQuery('body').hideLoading();
        }
    );     
}
function showAppointmentPopup(id) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $('#ctl00_mainpage_itemID').val(id);
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clients.aspx/loadClientDetails',
                data: '{clientID: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = (data.d); 
                    $('#AppFirstname').val(json.Firstname);
                    $('#AppLastname').val(json.Lastname);
                }
            });
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
                    $('#Clinics').html(options);
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
            jQuery('body').hideLoading();
        }
    ); 
}
function createAppointment() {
    jQuery('body').showLoading();S
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clients.aspx/createAppointment',
                data: '{ appointmentID: ' + $('#appointmentID').val() + ', clientID: ' + $('#ctl00_mainpage_itemID').val() + ', clinicID:' + $('#Clinics').val() + ', treatmentID:' + $('#Therapies').val() + ', appDate:"' + $('#AppDate').val() + '", startTime:"' + $('#StartTime').val() + '",comments:"' + $('#ctl00_mainpage_Comments').val() + '", status: 1 }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = (data.d);
                    alert(json);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}

function saveCaseHistory() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clients.aspx/saveCaseHistory',
                data: '{ clientID:' + $('#ctl00_mainpage_itemID').val() + ', caseDate:"' + $('#caseDate').val() + '", symptoms:"' + $('#ctl00_mainpage_caseSymptoms').val() + '", medications:"' + $('#ctl00_mainpage_caseMedications').val() + '", practitioners:"' + $('#ctl00_mainpage_casePractitioners').val() + '", therapies:"' + $('#ctl00_mainpage_caseTherapies').val() + '", healthHistory:"' + $('#ctl00_mainpage_caseHealthHistory').val() + '", familyHistory:"' + $('#ctl00_mainpage_caseFamilyHistory').val() + '", prakrti:"' + $('#casePrakrti').val() + '", vikrti:"' + $('#caseVikrti').val() + '", agni:"' + $('#ctl00_mainpage_caseAgni').val() + '", malas:"' + $('#ctl00_mainpage_caseMalas').val() + '", ojas:"' + $('#ctl00_mainpage_caseOjas').val() + '", manas:"' + $('#ctl00_mainpage_caseManas').val() + '", srotas:"' + $('#ctl00_mainpage_caseSrotas').val() + '", comments:"' + $('#ctl00_mainpage_caseComments').val() + '", suggestedTherapies:"' + $('#ctl00_mainpage_caseSuggestedTherapies').val() + '", suggestedHerbs:"' + $('#ctl00_mainpage_caseSuggestedHerbs').val() + '", suggestedPlan:"' + $('#ctl00_mainpage_caseSuggestedPlan').val() + '", suggestedCleanses:"' + $('#ctl00_mainpage_caseSuggestedCleanses').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d) {
                        alert("The client's case history was saved succesfully"); 
                    } else { alert("There was a problem saving the details. Please report the issue to the support team."); }
                }
            });
            jQuery('body').hideLoading();
        }
    );
}

function saveNotes(caseID) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clients.aspx/saveNotes',
                data: '{caseID: ' + caseID + ', notes: "' + $('#caseNotes').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#caseNotesTable').html(data.d);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
//]]>