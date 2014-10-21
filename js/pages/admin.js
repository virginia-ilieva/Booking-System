//<![CDATA[
$(document).ready(function () {
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
    activateValidation('aspnetForm');
    setPage();
})

var adminID = 0;
var page = '';

// Activates the validation engine
function activateValidation(tabID) {
    $("#" + tabID).validationEngine({
        success: function () {
            if (page == 'general') {
                saveAdminDetails();
            } else if (page == 'notification') {
                saveNotificationDetails();
            } else if (page == 'terms') {
                saveTermsOfUseDetails();
            } else if (page == 'bug') {
                sendBugReport();
            }
        },
        failure: false
    })
}
// Sets the default values on the page
function setPage() {
    setNavigation();
    $('#tabs').tabs();
    resetGeneralDetailsForm();
    resetReportBugsForm();
    getAdminDetails();
    loadTermsOfUseDetails();
}
// Empties the general details form
function resetGeneralDetailsForm() {
    $('#Firstname').val('');
    $('#Lastname').val('');
    $('#Username').val('');
    $('#Password').val('');
    $('#Phone').val('');
    $('#Email').val('');
}
// Empties the report bugs form
function resetReportBugsForm() {
    $('#BugTitle').val('');
    $('#BugPriority').val(0);
    $('#ctl00_mainpage_BugDescription').val('');
}
// Reads the admin detauils from the database
function getAdminDetails() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/Admin.aspx/loadAdminDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
            if (json != null) {
                adminID = json.ID;
                $('#Firstname').val(json.Firstname);
                $('#Lastname').val(json.Lastname);
                $('#Username').val(json.Username);
                $('#Password').val(json.Password);
                $('#Phone').val(json.Phone);
                $('#Email').val(json.Email);
            } else {
                resetGeneralDetailsForm();
                alert('There was a problem retreaving the admin details, please contact the support team.');
            }
        }
    });
}
// Writes the admin details to the database
function saveAdminDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/Admin.aspx/saveAdminDetails',
                data: '{adminID: ' + adminID + ', firstname: "' + $('#Firstname').val() + '", lastname: "' + $('#Lastname').val() + '", username: "' + $('#Username').val() + '", password: "' + $('#Password').val() + '", phone: "' + $('#Phone').val() + '", email: "' + $('#Email').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    jQuery('body').hideLoading();
                    alert('Your details has been saved successfully.');
                },
                error: function () {
                    jQuery('body').hideLoading();
                    alert("ERROR: Your details couldn't be saved. Please try again.");
                }
            });
            activateValidation();
        }
    );
}
// Sends bug report to the support team
function sendBugReport() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/Admin.aspx/sendBugReport',
                data: '{adminID: ' + adminID + ', title: "' + $('#BugTitle').val() + '", priority: ' + $('#BugPriority').val() + ', description: "' + $('#ctl00_mainpage_BugDescription').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    resetReportBugsForm();
                    alert('Your message has been send successfully. A member of our support team will contact you to confirm the complain. Please allow us 2 - 3 days to contact you if the problem is not marked as urgent.');
                },
                error: function () {
                    alert("ERROR: Your message couldn't be send. Please try again.");
                }
            });
            activateValidation();
            jQuery('body').hideLoading();
        }
    );
}
// Reads the notification details from the database
function loadNotificationDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/Admin.aspx/loadNotificationDetails',
                contentType: "application/json; charset=utf-8",
                data: '{notificationID: ' + $('#ctl00_mainpage_itemID').val() + '}',
                dataType: "json",
                success: function (data) {
                    var json = (data.d);
                    $('#ctl00_mainpage_NotificationDescription').val(json.BodyContent);
                    $('#sendToClients').val(false);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Save notification details
function saveNotificationDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/Admin.aspx/saveNotificationDetails',
                data: '{notificationID: ' + $('#ctl00_mainpage_itemID').val() + ', description: "' + $('#ctl00_mainpage_NotificationDescription').val() + '", send: ' + $('#sendToClients').attr('checked') + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    $('#ctl00_mainpage_btnRefreshNotifications').click();
                    $('a.close').click();
                }
            });
            activateValidation();
            jQuery('body').hideLoading();
        }
    );    
}
// Deletes notification
function deleteNotification(id) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/Admin.aspx/deleteNotification',
                data: '{notificationID: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    $('#ctl00_mainpage_btnRefreshNotifications').click();
                }
            });
            activateValidation();
            jQuery('body').hideLoading();
        }
    );
}
// Reads the terms of use details from the database
function loadTermsOfUseDetails() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/Admin.aspx/loadTermsOfUseDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
            $('#ctl00_mainpage_termID').val(json.ID);
            $('#ctl00_mainpage_TermsOfUseDescription').val(json.BodyContent);

        }
    });
}
// Save Terms Of Use Details
function saveTermsOfUseDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/Admin.aspx/saveTermsOfUseDetails',
                data: '{termID: ' + $('#ctl00_mainpage_termID').val() + ', description: "' + $('#ctl00_mainpage_TermsOfUseDescription').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    alert('Your details has been saved successfully.');
                }
            });
            activateValidation();
            jQuery('body').hideLoading();
        }
    );
}
// Gets colled when the popup opens
function showPopup(id) {
    $('#ctl00_mainpage_itemID').val(id);
    if (id > 0) {
        loadNotificationDetails(id);
    } else {
        $('#ctl00_mainpage_NotificationDescription').val('');
        $('#sendToClients').val(false);
    }
}
//]]>