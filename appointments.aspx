<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="appointments.aspx.vb" Inherits="AyurvedaHouse_BookingSystem._appointments" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
    <script type="text/javascript" src="/OnlineBooking/js/pages/appointments.js"></script>
    <style type="text/css">
        #appointments_popup { width:800px; height:600px; overflow:scroll; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainpage" runat="server">
    <div style="width:470px;"></div>
    <a href="#?w=900" id="showCalendar" rel="appointments_popup" class="poplight"></a>
    <div id="appointments_popup" class="popup_block"> 
        <label for="Clinics" class="label">Clinics:</label>
        <select id="Clinics" class="dropdown" clientidmode="Static" onchange="openCalendar(false);"></select><br /><br />
        <div id="appCalendar"></div>
    </div>
    <!-- Appointment Popup -->
    <a href="#?w=500" rel="appointment_popup" class="poplight"><input type="button" title="Add appointment." id="btnAppointment" class="hidden" /></a>
    <div id="appointment_popup" class="popup_block" style="max-height:550px; overflow:scroll;">
        <!-- Clients Search -->   
        <div id="ClientSearch">
            <div class="title"><span>Clients Search</span></div>
                <div style="width:270px">
                <label for="SearchFirstname" class="label">Firstname:</label>
                <input id="SearchFirstname" type="text" class="textbox" clientidmode="Static" /><br />
                <label for="SearchLastname" class="label">Lastname:</label>
                <input id="SearchLastname" type="text" class="textbox" clientidmode="Static" /><br />
                <label for="SearchPhone" class="label" >Phone:</label>
                <input id="SearchPhone" type="text" class="textbox" clientidmode="Static" /><br /><br />
                <div class="right">
                    <input type="button" id="btnSearch" title="Search clients." value="Search" class="btn search" onclick="getClients();" />
                </div><br />
            </div>
            <div id="searchResults"></div>
        </div><!-- End Clients Search --> 
        <div id="clientAppointment">
            <div class="title"><span>Create Appointment</span></div>
            <input id="appointmentID" type="text" class="hidden" clientidmode="Static" value="0"/>
            <input id="clientID" type="text" class="hidden" clientidmode="Static" />
            <label for="AppFirstname" class="label">Firstname:</label>
            <input id="AppFirstname" type="text" class="textbox" clientidmode="Static" readonly/><br />
            <label for="AppLastname" class="label">Lastname:</label>
            <input id="AppLastname" type="text" class="textbox" clientidmode="Static" readonly/><br />
            <label for="AppDate" class="label">Date:</label>
            <input id="AppDate" type="text" class="validate[required] textbox" clientidmode="Static" readonly /><br />
            <label for="StartTime" class="label">Start time:</label>
            <input id="StartTime" type="text" class="textbox" clientidmode="Static" readonly /><br />
            <label for="AppClinics" class="label">Clinics:</label>
            <select id="AppClinics" class="dropdown" clientidmode="Static"></select><br />
            <label for="Therapies" class="label">Therapies:</label>
            <select id="Therapies" class="dropdown" clientidmode="Static"></select><br />
            <label for="Status" class="label">Status:</label>
            <select id="Status" class="dropdown" clientidmode="Static">
                    <option value="1">Active</option>
                    <option value="2">Canceled</option>
                </select><br />
            <label for="Comments" class="label">Comments:</label>
            <asp:TextBox id="Comments" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
            <div class="right">
                <input type="button" id="btnCreateAppointment" title="Create appointment." value="Save" class="btn save" onclick="createAppointment();" />
            </div>
        </div>
    </div>
</asp:Content>
