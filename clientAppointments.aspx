<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="clientAppointments.aspx.vb" Inherits="AyurvedaHouse_BookingSystem.clientAppointments" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
    <script type="text/javascript" src="/OnlineBooking/js/pages/clientAppointments.js"></script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="mainpage" runat="server">
    <div style="padding-right: 5px; width:450px;">
        <h3>The page is under construction!</h3>
    </div>
<%--<div style="width:470px;">
    <div class="title"><span>View Appointments</span></div>  
    <!-- Appointments List -->   
    <asp:ScriptManager id="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upAppointments" runat="server">
        <ContentTemplate>
            <asp:Repeater ID="AppointmentsList" runat="server">
            <HeaderTemplate>
                <div class="right">
                    <a href="#?w=500" rel="appointment_popup" class="poplight"><input type="button" class="btn add" title="Create new appointment." value="Create Appointment" onclick="return showAppointmentPopup(0);"/></a>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="acc_trigger">
                    <table id="appointmentHeader"><tr>
                        <td style="width:385px;"><span><%# Eval("AppointmentDate", "{0:dd-MMM-yyyy}") + " " + Eval("StartTime", "{0:hh:mm tt}") & " - "%></span><label class="Status"><%# getThreatment(Eval("ThreatmentID"))%></label></td>
                        </td>
                        <td class="right" style="width:40px;">
                            <input type="button" title="Send feedback." class="gridButton mail hidden" onclick="sendFeedback(<%# Eval("ID")%>);" />
                            <a href="#?w=500" rel="appointment_popup" class="poplight"><input type="button" title="Edit appointment." id="btnEdit" class="gridButton edit" onclick='return showAppointmentPopup(<%# Eval("ID")%>);' /></a>
                        </td>
                    </tr></table>
                </div>
                <div class="acc_container">
                    <table> 
                        <tr><td class="tdLabel">Date: </td><td><%# Eval("AppointmentDate", "{0:dd-MMM-yyyy}")%></td></tr>
                        <tr><td class="tdLabel">Start Time: </td><td><%# Eval("StartTime", "{0:hh:mm tt}")%></td></tr>
                        <tr><td class="tdLabel">Clinic: </td><td><%# getClinic(Eval("ClinicID"))%></td></tr>
                        <tr><td class="tdLabel">Therapy: </td><td><%# getThreatment(Eval("ThreatmentID"))%></td></tr>
                    </table>
                </div>  
            </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel><!-- Appointments List -->    
    <input id="itemID" type="text" runat="server" class="hidden" clientidmode="Static" />
    <input id="btnRefreshTreatments" type="button" runat="server" class="hidden" clientidmode="Static" />
</div>
<!-- Appointment Popup -->
    <div id="appointment_popup" class="popup_block">
        <div class="title"><span>Create Appointment</span><br /><br />
            Enter the details below and click the &quot;Save&quot; button to save the appointment.
        </div>
        <label for="AppDate" class="label">Date:</label>
        <input id="AppDate" type="text" class="validate[required] textbox" clientidmode="Static" readonly /><br />
        <label for="StartTime" class="label">Start time:</label>
        <input id="StartTime" type="text" class="textbox" clientidmode="Static" readonly /><br />
        <label for="Clinics" class="label">Clinics:</label>
        <select id="Clinics" class="dropdown" clientidmode="Static"></select><br />
        <label for="Therapies" class="label">Therapies:</label>
        <select id="Therapies" class="dropdown" clientidmode="Static"></select><br />
        <label for="Comments" class="label">Comments:</label>
        <asp:TextBox id="Comments" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
        <label for="termsConditions">Agree with the <a href="">terms and conditions</a></label>
        <input id="termsConditions" type="checkbox" onchange="checkState();" /><br />
        <div class="right">
            <input type="submit" id="btnCreateAppointment" title="Create appointment." value="Save" class="btn save" onclick="page = 'appointment';" disabled="disabled" />
        </div>
    </div><!-- End Appointment Popup -->--%>
</asp:Content>
