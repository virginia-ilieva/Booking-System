<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="clinics.aspx.vb" Inherits="AyurvedaHouse_BookingSystem._clinics" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
<script type="text/javascript" src="/OnlineBooking/js/pages/clinics.js"></script>
<style type="text/css">
    .Status { font-family:Calibri; }
    img.btn_close { margin-top:-15px; }
    .popup_block { padding-top:25px; }
    #btnSave { vertical-align:top; }
</style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" runat="server">
    <div style="width:470px;">
    <h3>Manage Clinics</h3>
    <!-- Clinic List -->
    <asp:ScriptManager id="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upClinics" runat="server">
        <ContentTemplate>
            <asp:Repeater ID="ClinicsList" runat="server">
            <HeaderTemplate>
                <div class="right">
                    <a href="#?w=500" rel="clinic_popup" class="poplight"><input type="button" class="btn add" title="Create new clinic." value="Add Clinic" onclick="$(function () { $('#tabs').tabs(); }); showPopup(0);"/></a>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="acc_trigger">
                    <table id="clinicHeader"><tr>
                        <td style="width:385px;"><span><%# Eval("Name") & " - "%></span><label class="Status"><%# Eval("Status")%></label></td>    
                        <td class="right" style="width:65px;">
                            <input type="button" title="Reactivate clinic." class="gridButton refresh" onclick="reactivateClinic(<%# Eval("ID")%>);" />
                            <a href="#?w=700" rel="clinic_popup" class="poplight"><input type="button" title="Edit clinic." id="btnEdit" class="gridButton edit" onclick='loadRooms(<%# Eval("ID")%>); $(function () { $("#tabs").tabs(); }); showPopup(<%# Eval("ID")%>);' /></a>
                        </td>
                    </tr></table>
                </div>
                <div class="acc_container">
                    <table>
                        <tr><td class="tdLabel">Address: </td><td><%# Eval("Address") & ", " & Eval("City") & ", " & Eval("State") & " " & Eval("Postcode")%></td></tr>  
                        <tr><td class="tdLabel">Phone: </td><td><%# Eval("Phone") & ", <strong>Fax:</strong> " & Eval("Fax")%></td></tr>
                        <tr><td class="tdLabel">Email: </td><td><%# Eval("Email")%></td></tr>        
                        <tr><td colspan="2"><br /></td></tr>        
                        <tr><td colspan="2"><strong>Clinic open hours: </strong></td></tr>        
                        <tr><td colspan="2">
                            <asp:Literal id="openHours" runat="server" Text='<%# getOpenHours(Eval("ID"))%>'></asp:Literal>
                        </td></tr>   
                    </table>
                </div>            
            </ItemTemplate>
        </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel><!-- Clinic List -->    
    <input id="itemID" type="text" runat="server" class="hidden" clientidmode="Static" />
    <input id="btnRefreshClinics" type="button" runat="server" class="hidden" clientidmode="Static" />
    </div>

    <!-- Clinic Popup -->
    <div id="clinic_popup" class="popup_block" style="height: 500px; overflow:scroll;">
    <div id="tabs">
        <ul>
		    <li><a href="#ClinicDetails">Clinic Details</a></li>
		    <li><a href="#ClinicRooms">Clinic Rooms</a></li>
	    </ul>
        <div id="ClinicDetails">
            <label for="Name" class="label">Clinic:</label>
            <input id="Name" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
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
            <label for="Fax" class="label" >Fax:</label>
            <input id="Fax" type="text" class="textbox" clientidmode="Static" /><br />
            <label for="Email" class="label" >Email:</label>
            <input id="Email" type="text" class="textbox" clientidmode="Static" /><br />
            <label for="Status" class="label" >Status:</label>
            <select id="Status" class="validate[required,funcCall[validateSelect]] dropdown" clientidmode="Static">
                <option value="0">Select status</option>
                <option value="1">Active</option>
                <option value="2">Inactive</option>
            </select><br />
            <div class="right">
                <input type="submit" id="btnSave" title="Save clinic." value="Save" class="btn save" onclick="page = 'general'; activateValidation('ClinicDetails');"/>
            </div>
	    </div>
	    <div id="ClinicRooms">
            <div style="width:260px;" class="right">
                <input type="button" class="btn add" title="Create new room." value="Add Room" onclick="openEditRoomScreen(0);"/>
            </div>
            <div id="editRoom" class="hidden">
            <table>
                <tr id="RoomDetails">
                    <td><input id="roomID" type="text" class="hidden" value='0' clientidmode="Static" /><br />
                    <label for="RoomName" class="label">Name:</label>
                    <input id="RoomName" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                    <label for="RoomStatus" class="label" >Status:</label>
                    <select id="RoomStatus" class="validate[required,funcCall[validateSelect]] dropdown" clientidmode="Static">
                        <option value="0">Select status</option>
                        <option value="1">Active</option>
                        <option value="2">Inactive</option>
                    </select><br />
                    <div class="right">
                        <input type="submit" id="btnSaveRoom" title="Save clinic." value="Save" class="btn save" onclick="page = 'room'; activateValidation('RoomDetails');" />
                    </div><br /></td>
                    <td rowspan="2" style="padding:10px;"><div id="Schedule"></div></td>
                </tr>
                <tr>
                    <td><div id="roomAvailability" class="hidden">
                    <div class="title" style="margin-bottom:20px;">Room Availability</div>
                        <input type="text" id="availabilityID" class="hidden" value="0" />
                        <label for="StartTime" class="label">Start time:</label>
                        <input id="StartTime" type="text" class="textbox" clientidmode="Static" readonly /><br />
                        <label for="EndTime" class="label">End time:</label>
                        <input id="EndTime" type="text" class="textbox" clientidmode="Static" readonly /><br />
                        <label for="WeekDay" class="label" >Week day:</label>
                        <select id="WeekDay" class="dropdown" clientidmode="Static">
                            <option value="0">Select  day</option>
                            <option value="1">Monday</option>
                            <option value="2">Tuesday</option>
                            <option value="3">Wednesday</option>
                            <option value="4">Thursday</option>
                            <option value="5">Friday</option>
                            <option value="6">Saturday</option>
                            <option value="7">Sunday</option>
                        </select><br />
                        <div class="right">
                            <input type="button" id="btnAddAvailability" title="Save room availability." value="Save" class="btn save" onclick="saveAvailability();" />
                        </div><br />
                    </div></td>
                </tr>
            </table>  
            </div>
            <div id="ClinicRoomsList"></div>
        </div>
    </div>
    </div><!-- End Clinic Popup -->

</asp:Content>
