<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="default.aspx.vb" Inherits="AyurvedaHouse_BookingSystem._default" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
<style type="text/css">
    .NotificationsList { padding:10px; border:1px solid Gray; background-color:#eee; text-align:justify; }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        setNavigation();
    })
    function logout() {
        $.ajax({
            type: "POST",
            async: false,
            url: '/OnlineBooking/login.aspx/logout',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
            }
        });
    }
</script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" Runat="Server">
    <div style="padding-right: 5px; width:450px;">
        <h3>Welcome to the Appointment Booking System</h3><br />
        <asp:repeater ID="NotificationsList" runat="server">
            <ItemTemplate>
                <div>
                    <div><strong><%# Eval("Created", "{0:dd-MMM-yyyy}")%></strong></div>
                    <div class="NotificationsList"><%# Eval("BodyContent")%></div><br />
                </div>  
            </ItemTemplate>
        </asp:repeater>
        This application allows you to quickly book your favorite therapy.<br /><br />
        Before you can use this application, you need to have an active account. If you don't have an account yet, you can <a href="/OnlineBooking/ErrorPage.aspx">create one now</a>. Otherwise, you'll be asked to <a href="/OnlineBooking/login.aspx">login</a> before you can make the appointment.<br />
    </div>
</asp:Content>