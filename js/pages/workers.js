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
    $('.Status').each(function () {
        if ($(this).html() == 1) {
            $(this).html('Active');
            $(this).parent().parent().find('.refresh').addClass('hidden');
        } else {
            $(this).html('Inactive');
        }
    })
    activateValidation();
    setNavigation();
}
function activateValidation() {
    $("#aspnetForm").validationEngine({
        success: function () { saveWorkerDetails() },
        failure: false
    })
}
function validateSelect() {
    var result = true;
    if ($('#Status').val() == 0) { result = false; }
    return result;
}

// Load worker details
function loadWorkerDetails(id) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/workers.aspx/loadWorkerDetails',
                data: '{workerID: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = (data.d);
                    $('#Firstname').val(json.Firstname);
                    $('#Lastname').val(json.Lastname);
                    $('#Username').val(json.Username);
                    $('#Password').val(json.Password);
                    $('#Phone').val(json.Phone);
                    $('#Email').val(json.Email);
                    $('#Status').val(json.Status);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Save worker details
function saveWorkerDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/workers.aspx/saveWorkerDetails',
                data: '{workerID: ' + $('#ctl00_mainpage_itemID').val() + ', firstname: "' + $('#Firstname').val() + '", lastname: "' + $('#Lastname').val() + '", username: "' + $('#Username').val() + '", password: "' + $('#Password').val() + '", phone: "' + $('#Phone').val() + '", email: "' + $('#Email').val() + '", status: ' + $('#Status').val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    $('#ctl00_mainpage_btnRefreshWorkers').click();
                    activateValidation();
                    $('a.close').click();
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Reactivate worker details
function reactivateWorker(id) {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/workers.aspx/reactivateWorker',
        data: '{workerID: ' + id + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            $('#ctl00_mainpage_btnRefreshWorkers').click();
            activateValidation();
        }
    });
}
// Gets colled when the popup opens
function showPopup(id) {
    $('#ctl00_mainpage_itemID').val(id);
    if (id > 0) {
        loadWorkerDetails(id);
    } else {
        $('#Firstname').val('');
        $('#Lastname').val('');
        $('#Username').val('');
        $('#Password').val('');
        $('#Phone').val('');
        $('#Email').val('');
        $('#Status').val(0);
    }
}
//]]>