//<![CDATA[
$(document).ready(function () {
    setPage();
})
function setPage() {
    $(function () {
        var maxYear = new Date().getFullYear();
        var minYear = new Date().getFullYear();
        minYear = minYear - 90;
        var yearRange = minYear + ":" + maxYear
        $("#DOB").datepicker({ dateFormat: 'dd-M-yy', changeMonth: true, changeYear: true, yearRange: yearRange });
    });
    activateValidation();
    setNavigation();
    loadClientDetails();
}
// Activates the validation engine
function activateValidation() {
    $("#aspnetForm").validationEngine({
        success: function () {
            validateInput();
        },
        failure: false
    })
}
// Sets the validation rules for the select fileds
function validateSelect() {
    var result = true;
    if ($('#State').val() == 0) { result = false; }
    return result;
}
// Load client details
function loadClientDetails() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/clientAccount.aspx/loadClientDetails',
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
            $('#Password').val(json.Password);
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
        activateValidation();
    } else {
        alert("The user account was created successfully.");
    }
}
// Save client details
function saveClientDetails() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/clientAccount.aspx/saveClientDetails',
        data: '{firstname: "' + $('#Firstname').val() + '", lastname: "' + $('#Lastname').val() + '", dob: "' + $('#DOB').val() + '", address: "' + $('#Address').val() + '", city: "' + $('#City').val() + '", state: "' + $('#State').val() + '", postcode: "' + $('#Postcode').val() + '", username: "' + $('#Username').val() + '", password: "' + $('#Password').val() + '", phone: "' + $('#Phone').val() + '", email: "' + $('#Email').val() + '", contactName: "' + $('#ContactName').val() + '", contactPhone: "' + $('#ContactPhone').val() + '", relation: "' + $('#Relation').val() + '", newsletter: "' + $('#Newsletter').attr('checked') + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = data.d;
            if (json == "username") {
                $('#Username').val('');
                $('#Password').val('');
                $('#ConfirmPassword').val('');
                alert("This username is already registered in our system. Please provide another username.");
                return false;
            } else if (json == "email") {
                $('#Email').val('');
                alert("This email is already registered in our system. Please provide another email address.");
                return false;
            } else {
                return true;
                alert('Your information was updated successfully.');
            }
        },
        error: function () { return false; }
    });
}
//]]>