//<![CDATA[
$(document).ready(function () {
    setPage();
})
function setPage() {
    setAccordion();
    setStatus('clinicHeader');
    activateValidation('aspnetForm');
    setNavigation();
}

var page = '';

function setAccordion() {
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
// Activates the validation engine
function activateValidation(tabID) {
    $('#' + tabID).validationEngine({
        success: function () {
            if (page == 'general') {
                saveClinicDetails(); 
            } else if (page == 'room') {
                saveRoomDetails();
                $('#roomAvailability').removeClass('hidden');
            }             
        },
        failure: false
    })
}
// Displays the status field for a given table
function setStatus(table) { 
 $('#' + table + ' .Status').each(function () {
        if ($(this).html() == 1) {
            $(this).html('Active');
            $(this).parent().parent().find('.refresh').addClass('hidden');
        } else if ($(this).html() == 2) {
            $(this).html('Inactive');
        }
    })
}
// Sets the validation rules for the select fileds
function validateSelect() {
    var result = true;
    if ($('#State').val() == 0) { result = false; }
    if ($('#Status').val() == 0) { result = false; }
    return result;
}

// Load clinic details
function loadClinicDetails(id) {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/clinics.aspx/loadClinicDetails',
        data: '{clinicID: ' + id + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
            $('#Name').val(json.Name);
            $('#Address').val(json.Address);
            $('#City').val(json.City);
            $('#State').val(json.State);
            $('#Postcode').val(json.Postcode);
            $('#Phone').val(json.Phone);
            $('#Fax').val(json.Fax);
            $('#Email').val(json.Email);
            $('#Status').val(json.Status);
        }
    });
}
// Load rooms
function loadRooms(id) {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/clinics.aspx/loadRooms',
        data: '{clinicID: ' + id + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = (data.d);
            $('#ClinicRoomsList').html('');
            $('#editRoom').addClass('hidden');
            var rooms = '';
            for (record in json) {
                rooms = '<div class="acc_trigger"><table id="roomHeader"><tr id="room-' + json[record].ID + '"><td style="width:385px;"><span>' + json[record].Name + ' - </span><label class="Status">' + json[record].Status + '</label></td><td class="right" style="width:40px;"><input type="button" title="Reactivate room." class="gridButton refresh" onclick="reactivateRoom(' + json[record].ID + ');" /><input type="button" title="Edit room." id="btnEdit" class="gridButton edit" onclick="openEditRoomScreen(' + json[record].ID + ')" /></td></tr></table></div><div class="acc_container" id="details-' + json[record].ID + '"></div>';
                $('#ClinicRoomsList').append(rooms);
                getRoomAvailability(json[record].ID, 'details-' + json[record].ID, 'info');
            }
        }
    });
}
// Gets the schedule for the selected room
function getRoomAvailability(roomID, pageElement, pagePart) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clinics.aspx/getRoomAvailability',
                data: '{roomID: ' + roomID + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = (data.d);
                    if (pagePart == 'info') {
                        var availabilityStr = '<table>';
                        for (record in json) {
                            availabilityStr += '<tr><td><strong>' + getWeekDay(json[record].Workday) + ':&nbsp;&nbsp;</strong></td><td> ' + stringTimeFormat(json[record].StartTimeString) + ' - ' + stringTimeFormat(json[record].EndTimeString) + '</td></tr>';
                        }
                        availabilityStr += '</table>';
                        $('#' + pageElement).html(availabilityStr);
                    } else if (pagePart == 'editRoom') {
                        var availabilityStr = '<table class="grid"><tr class="rowhead"><th style="width:120px; text-align:center;">Week day</th><th style="width:200px; text-align:center;">Availability</th><th style="width:60px;"></th></tr>';
                        for (record in json) {
                            availabilityStr += '<tr><td><strong>' + getWeekDay(json[record].Workday) + ':&nbsp;&nbsp;</strong></td><td> ' + stringTimeFormat(json[record].StartTimeString) + ' - ' + stringTimeFormat(json[record].EndTimeString) + '</td><td><input type="button" title="Edit availability." id="btnEditAvailability" class="gridButton edit" onclick="editAvailability(' + json[record].ID + ')" /><input type="button" title="Delete availability." id="btndeleteAvailability" class="gridButton delete" onclick="confirmDelete(' + json[record].ID + ');" /></td></tr>';
                        }
                        availabilityStr += '</table>';
                        $('#' + pageElement).html(availabilityStr);
                    }
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
function confirmDelete(id) { 
    var confirmed = confirm("Do you want to delete this record?");
    if (confirmed == true) { deleteAvailability(id); }
}
// Save clinic details
function saveClinicDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clinics.aspx/saveClinicDetails',
                data: '{clinicID: ' + $('#ctl00_mainpage_itemID').val() + ', name: "' + $('#Name').val() + '", address: "' + $('#Address').val() + '", city: "' + $('#City').val() + '", state: "' + $('#State').val() + '", postcode: "' + $('#Postcode').val() + '", phone: "' + $('#Phone').val() + '", fax: "' + $('#Fax').val() + '", email: "' + $('#Email').val() + '", status: ' + $('#Status').val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    $('#ctl00_mainpage_btnRefreshClinics').click();
                    $('#tabs ul li:last').removeClass('hidden');
                    alert("The clinic details are saved successfully.");
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Reactivate clinic details
function reactivateClinic(id) {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/clinics.aspx/reactivateClinic',
        data: '{clinicID: ' + id + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            $('#ctl00_mainpage_btnRefreshClinics').click();
        }
    });
}
// Opens the add/edit clinic room form
function openEditRoomScreen(id) {
    $('#editRoom').removeClass('hidden');
    $('#roomID').val(id);
    getRoomAvailability(id, 'Schedule', 'editRoom')
    $('#StartTime').timepicker({ ampm: true, stepMinute: 30 });
    $('#EndTime').timepicker({ ampm: true, stepMinute: 30 });
    if (id == 0) {
        $('#roomAvailability').addClass('hidden');
        $('#RoomName').val('');
        $('#RoomStatus').val(0);
    } else {
        $('#roomAvailability').removeClass('hidden');
        $.ajax({
            type: "POST",
            async: false,
            url: '/OnlineBooking/clinics.aspx/getRoomDetails',
            data: '{ roomID: ' + id + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var json = data.d;
                $('#RoomName').val(json.Name);
                $('#RoomStatus').val(json.Status);
            }
        });
    }
}
// Saves the room details to the database
function saveRoomDetails() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            var roomID = $('#roomID').val();
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clinics.aspx/saveRoomDetails',
                data: '{ clinicID: ' + $('#ctl00_mainpage_itemID').val() + ', roomID: ' + roomID + ', name: "' + $('#RoomName').val() + '", status: ' + $('#RoomStatus').val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = data.d;
                    if (roomID > 0) {
                        $('#room-' + json.ID).html('<td style="width:385px;"><span>' + json.Name + ' - </span><label class="Status">' + json.Status + '</label></td><td class="right" tyle="width:40px;"><input type="button" title="Reactivate room." class="gridButton refresh" onclick="reactivateRoom(' + json.ID + ');" /><input type="button" title="Edit room." id="btnEdit" class="gridButton edit" onclick="openEditRoomScreen(' + json.ID + ')" /></td>');
                    } else {
                        $('#ClinicRoomsList').append('<div class="acc_trigger"><table id="roomHeader"><tr id="room-' + json.ID + '"><td style="width:385px;"><span>' + json.Name + ' - </span><label class="Status">' + json.Status + '</label></td><td class="right" style="width:40px;"><input type="button" title="Reactivate room." class="gridButton refresh" onclick="reactivateRoom(' + json.ID + ');" /><input type="button" title="Edit room." id="btnEdit" class="gridButton edit" onclick="openEditRoomScreen(' + json.ID + ')" /></td></tr></table></div><div class="acc_container" id="details-' + json.ID + '"></div>');
                    }
                    setStatus('roomHeader');
                    activateValidation();
                    setAccordion();
                    $('#roomID').val(json.ID);
                    $('#roomAvailability').removeClass('hidden');
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Gets the availability details and populates the fields in the availability edit form
function editAvailability(id) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $('#availabilityID').val(id);
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clinics.aspx/getAvailability',
                data: '{ availabilityID: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = data.d;
                    $('#StartTime').val(stringTimeFormat(json.StartTimeString));
                    $('#EndTime').val(stringTimeFormat(json.EndTimeString));
                    $('#WeekDay').val(json.Workday);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Saves the availability to the database
// Refreshes the availability table and the room info screen
function saveAvailability() {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            var availabilityID = $('#availabilityID').val();
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clinics.aspx/saveAvailability',
                data: '{ availabilityID: ' + availabilityID + ', roomID:' + $('#roomID').val() + ', startTime:"' + $('#StartTime').val() + '", endTime:"' + $('#EndTime').val() + '", workday:' + $('#WeekDay').val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = data.d;
                    getRoomAvailability(json.RoomID, 'Schedule', 'editRoom');
                    getRoomAvailability(json.RoomID, 'details-' + json.RoomID, 'info');
                    $('#availabilityID').val(0);
                    $('#StartTime').val('');
                    $('#EndTime').val('');
                    $('#WeekDay').val(0);
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Deletes the availability to the database
// Refreshes the availability table and the room info screen
function deleteAvailability(availabilityID) {
    jQuery('body').showLoading();
    jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
        function () {
            $.ajax({
                type: "POST",
                async: false,
                url: '/OnlineBooking/clinics.aspx/deleteAvailability',
                data: '{ availabilityID: ' + availabilityID + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = data.d;
                    getRoomAvailability(json, 'Schedule', 'editRoom');
                    getRoomAvailability(json, 'details-' + json, 'info');
                }
            });
            jQuery('body').hideLoading();
        }
    );
}
// Gets called when the popup opens
function showPopup(id) {
    $('#ctl00_mainpage_itemID').val(id);
    setAccordion();
    if (id > 0) {
        jQuery('body').showLoading();
        jQuery('body').load('/OnlineBooking/img/jQuery/loading.gif', {},
            function () {
                loadClinicDetails(id);
                setStatus('roomHeader');
                jQuery('body').hideLoading();
            }
        );
    } else {
        $('#tabs ul li:last').addClass('hidden');
        $('#Name').val('');
        $('#Address').val('');
        $('#City').val('');
        $('#State').val(0);
        $('#Postcode').val('');
        $('#Phone').val('');
        $('#Fax').val('');
        $('#Email').val('');
        $('#Status').val(0);
    }
}
//]]>