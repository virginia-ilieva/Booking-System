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
    $('.Duration').each(function () {
        var hours = $(this).html();
        var duration;
        if ($(this).html() >= 60) {
            if ((hours % 60) > 0) {
                duration = Math.ceil(hours / 60) + 'h ' + (hours % 60) + 'min';
            } else {
                duration = Math.ceil(hours / 60) + 'h';
            }
            $(this).html(duration);
        } else {
            duration = hours + 'min'
            $(this).html(duration);
        }
    })
    $("#aspnetForm").validationEngine({
        success: function () { saveTreatmentDetails() },
        failure: false
    })
    setNavigation();
}

function validateSelect() {
    var result = true;
    if ($('#Duration').val() == 0) { result = false; }
    if ($('#Status').val() == 0) { result = false; }
    return result;
}

// Load treatment details
function loadTreatmentDetails(id) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/treatments.aspx/loadTreatmentDetails',
                data: '{treatmentID: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = (data.d);
                    $('#Name').val(json.Name);
                    $('#Description').val(json.Description);
                    $('#Price').val(json.Price);
                    $('#Duration').val(json.Duration);
                    $('#Status').val(json.Status);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Save treatment details
function saveTreatmentDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/treatments.aspx/saveTreatmentDetails',
                data: '{treatmentID: ' + $('#ctl00_mainpage_itemID').val() + ', name: "' + $('#Name').val() + '", duration: ' + $('#Duration').val() + ', description: "' + $('#ctl00_mainpage_Description').val() + '", price: ' + $('#Price').val() + ', status: ' + $('#Status').val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    $('#ctl00_mainpage_btnRefreshTreatments').click();
                    $('a.close').click();
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Reactivate treatment details
function reactivateTreatment(id) {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/treatments.aspx/reactivateTreatment',
        data: '{treatmentID: ' + id + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            $('#ctl00_mainpage_btnRefreshTreatments').click();
        }
    });
}
function showPopup(id) {
    $('#ctl00_mainpage_itemID').val(id);
    if (id > 0) {
        loadTreatmentDetails(id);
    } else {
        $('#Name').val('');
        $('#Description').val('');
        $('#Price').val('');
        $('#Duration').val(0);
        $('#Status').val(0);
    }
}
//]]>