//<![CDATA[
$(document).ready(function () {
    setPage();
})
function setPage() {
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
    $("#aspnetForm").validationEngine({
        success: function () {  },
        failure: false
    })
    setNavigation();
    getFeedback();
}
function getFeedback() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/clientAppointment.aspx/getFeedback',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);

        }
    });
}
function showAppointmentPopup(id) {
    $('#AppDate').datepicker({ dateFormat: 'dd-M-yy' });
    $('#StartTime').timepicker({ ampm: true, stepMinute: 15 });
    $.ajax({
        type: "POST",
        async: false,
        url: '/clients.aspx/getClinicList',
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
        url: '/clients.aspx/getTreatmentsList',
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
function createAppointment() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/clients.aspx/createAppointment',
        data: '{clientID: ' + $('#clientID').val() + ', clinicID:' + $('#Clinics').val() + ', treatmentID:' + $('#Therapies').val() + ', appDate:"' + $('#AppDate').val() + '", startTime:"' + $('#StartTime').val() + '",comments:"' + $('#Comments').val() + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
        }
    });
}
//]]>