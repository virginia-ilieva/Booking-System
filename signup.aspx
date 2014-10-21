<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="signup.aspx.vb" Inherits="AyurvedaHouse_BookingSystem.signup" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
<script type="text/javascript">
    //<![CDATA[
    $(document).ready(function () {
        $(function () {
            var maxYear = new Date().getFullYear();
            var minYear = new Date().getFullYear();
            minYear = minYear - 90;
            var yearRange = minYear + ":" + maxYear
            $("#DOB").datepicker({ dateFormat: 'dd-M-yy', changeMonth: true, changeYear: true, yearRange: yearRange });
        });
        activateValidation();
        resetForm();
    })

    function resetForm() {
        $('.textbox').val('');
        $('select').val(0);
    }

    function activateValidation() {
        $("#aspnetForm").validationEngine({
            success: function () { validateInput(); },
            failure: false
        })
    }

    function validateSelect() {
        var result = true;
        if ($('#State').val() == 0) { result = false; }
        return result;
    }

    function validateInput() {
        if (checkPassword()) {
            if (!saveUser()) {
                activateValidation();
            } else {
                resetForm();
                alert("Your account was created successfully.");
            }
        } else { activateValidation(); }
    }

    function checkPassword() {
        var password = $('#Password').val();
        var confirmedPassword = $('#ConfirmPassword').val();
        if (password == confirmedPassword) { return true; } else {
            alert("Password doesnt match.");
            $('#Password').val('');
            $('#ConfirmPassword').val('');
            return false;
        }
    }
    
    function saveUser() {
        $.ajax({
            type: "POST",
            async: false,
            url: '/signup.aspx/saveUser',
            data: '{firstname: "' + $('#Firstname').val() + '", lastname: "' + $('#Lastname').val() + '", address: "' + $('#Address').val() + '", city: "' + $('#City').val() + '", state: "' + $('#State').val() + '", postcode: "' + $('#Postcode').val() + '", phone: "' + $('#Phone').val() + '", email: "' + $('#Email').val() + '", username: "' + $('#Username').val() + '", password: "' + $('#Password').val() + '", dob: "' + $('#DOB').val() + '", contactName: "' + $('#ContactName').val() + '", contactPhone: "' + $('#ContactPhone').val() + '", relation: "' + $('#Relation').val() + '", newsletter: "' + $('#Newsletter').attr('checked') + '"}',
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
                } else { return true; }


            },
            error: function () { return false; }
        });
    } 
    //]]>
</script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" runat="server">
    <div style="width:470px;">
        <div class="title"><span>User Details</span><br /><br />
            Enter the details below and click the &quot;Save&quot; button to create your account.
        </div>
        <label><strong>General Details</strong></label><br />
        <label for="Firstname" class="label">Firstname:</label>
        <input id="Firstname" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Lastname" class="label">Lastname:</label>
        <input id="Lastname" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Address" class="label" >Address:</label>
        <input id="Address" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="City" class="label" >City:</label>
        <input id="City" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="State" class="label" >State:</label>
        <select id="State" class="validate[required,funcCall[validateSelect]] dropdown" clientidmode="Static">
            <option value="0">Select state</option>
            <option value="ACT">ACT</option>
            <option value="NSW">NSW</option>
            <option value="NT">NT</option>
            <option value="QLD">QLD</option>
            <option value="SA">SA</option>
            <option value="TAS">TAS</option>
            <option value="VIC">VIC</option>
            <option value="WA">WA</option>
        </select><br />
        <label for="Postcode" class="label" >Postcode:</label>
        <input id="Postcode" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Phone" class="label" >Phone:</label>
        <input id="Phone" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Email" class="label" >Email:</label>
        <input id="Email" type="text" class="validate[required] textbox" clientidmode="Static" /><br />   
        <label for="DOB" class="label" >Date of Birth:</label>
        <input id="DOB" type="text" class="validate[required] textbox" clientidmode="Static" readonly /><br /> 
        <label for="Username" class="label" >Username:</label>
        <input id="Username" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Password" class="label" >Password:</label>
        <input id="Password" type="password" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="ConfirmPassword">Confirm Password:</label>
        <input id="ConfirmPassword" type="password" class="validate[required] textbox" clientidmode="Static" /><br /><br />
        <label><strong>Emergency Contact Details</strong></label><br />
        <label for="ContactName" class="label" >Contact Name:</label>
        <input id="ContactName" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="ContactPhone" class="label" >Contact Phone:</label>
        <input id="ContactPhone" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Relation" class="label" >Relation:</label>
        <input id="Relation" type="text" class="validate[required] textbox" clientidmode="Static" /><br /><br />  
        <label for="Newsletter">Sign up for free newsletter:</label>
        <input id="Newsletter" type="checkbox" clientidmode="Static" checked="checked" /> 
        <div class="right">
            <input type="submit" id="btnSave" title="Save details." value="Save" class="btn save" />
        </div>
    </div>
    
</asp:Content>
