<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="passwordRecovery.aspx.vb" Inherits="AyurvedaHouse_BookingSystem.passwordRecovery" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
<script type="text/javascript">
    //<![CDATA[
    $(document).ready(function () {
        $("#aspnetForm").validationEngine({
            success: function () { FindUserAndSendEmail() },
            failure: false
        })
    })

    function FindUserAndSendEmail() {
        $.ajax({
            type: "POST",
            async: false,
            url: '/passwordRecovery.aspx/FindUserAndSendEmail',
            data: '{ firstName: "' + $('#txtFirstName').val() + '", lastName: "' + $('#txtLastName').val() + '", email: "' + $('#txtEmail').val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (!data) {alert("The user was not found.");
                } else { alert("An email with your password has been sent to your email address."); }
                    $('#txtFirstName').val('');
                    $('#txtLastName').val('');
                    $('#txtEmail').val('');
            }
        });
    } 
    //]]>
</script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" runat="server">
    <div style="padding-right: 5px; width: 460px;">
        <p>Please, submit the information below and we will send you an email with your login details.</p>
        
       <table align="center">
            <tr>
                <td><label for="txtFirstName" runat="server" class="label">Firstname: </label></td>
                <td><input id="txtFirstName" type="text" runat="server" class="validate[required] textbox" clientidmode="Static"/></td>
            </tr>
            <tr>
                <td><label for="txtLastName" runat="server" class="label">Lastname: </label></td>
                <td><input id="txtLastName" type="text" runat="server" class="validate[required] textbox" clientidmode="Static"/></td>
            </tr>
            <tr>
                <td><label for="txtEmail" runat="server" class="label">Email: </label></td>
                <td><input id="txtEmail" type="text" runat="server" class="validate[required] textbox" clientidmode="Static"/><br /><br /></td>
            </tr>
            <tr>
                <td class="right" colspan="2">
                    <input id="btnSubmit" type="submit" runat="server" value="Submit" class="btn mail"/>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
