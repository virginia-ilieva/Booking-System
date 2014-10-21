<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="clients.aspx.vb" Inherits="AyurvedaHouse_BookingSystem.manageClients" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
<script type="text/javascript" src="/OnlineBooking/js/pages/clients.js"></script>
<style type="text/css">
    .Status { font-family:Calibri; }
    .title { margin-top:30px; margin-bottom:15px; }
    .title span { font-size:20px; }
    #saveButton { padding: 10px; }
    #createHistoryTabs .multiline { width: 400px; }
    #tabs .acc_trigger { width: 940px; }
    #tabs .acc_container { width: 925px; }
    #caseHistoryTabs { width: 940px; margin-bottom:15px; }
    #userNotes { padding: 0 10px; }
    #userNotes td { text-align:left; padding: 10px; }
</style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" runat="server">
    <div style="width:470px;">
    <!-- Clients Search -->   
    <h3>Clients Search</h3>
    <div style="width:270px">
        <label for="SearchFirstname" class="label">Firstname:</label>
        <input id="SearchFirstname" type="text" class="textbox" clientidmode="Static" runat="server" /><br />
        <label for="SearchLastname" class="label">Lastname:</label>
        <input id="SearchLastname" type="text" class="textbox" clientidmode="Static" runat="server" /><br />
        <label for="SearchPhone" class="label" >Phone:</label>
        <input id="SearchPhone" type="text" class="textbox" clientidmode="Static" runat="server" /><br /><br />
        <div class="right">
            <input type="submit" id="btnSearch" title="Search clients." value="Search" class="btn search" runat="server" />
        </div><br />
    </div><!-- End Clients Search -->   
    <!-- Clients List -->
    <div id="manageClientsContainer" runat="server">
    <h3>Manage Clients</h3>   
    <asp:ScriptManager id="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upClients" runat="server">
        <ContentTemplate>
            <asp:Repeater ID="ClientsList" runat="server">
            <HeaderTemplate>
                <div class="right">
                    <a href="#?w=500" rel="client_popup" class="poplight"><input type="button" class="btn add" title="Create new client." value="Add Client" onclick="return showPopup(0);"/></a>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="acc_trigger">
                    <table id="clientHeader"><tr>
                        <td style="width:385px;"><span><%# Eval("Fullname")%></span></td>    
                        <td class="right" style="width:40px;">
                            <a href="#?w=500" rel="appointment_popup" class="poplight"><input type="button" title="Add appointment." id="btnAppointment" class="gridButton cal" onclick='showAppointmentPopup(<%# Eval("ID")%>);' /></a>
                            <a href="#?w=1000" rel="client_popup" class="poplight"><input type="button" title="Edit client." id="btnEdit" class="gridButton edit" onclick='return showPopup(<%# Eval("ID")%>);' /></a>
                        </td>
                    </tr></table>
                </div>
                <div class="acc_container">
                    <table>
                        <tr><td class="tdLabel">Name: </td><td><%# Eval("Fullname")%></td></tr> 
                        <tr><td class="tdLabel">Date of Birth: </td><td><%# Eval("DOB", "{0:dd-MMM-yy}")%></td></tr> 
                        <tr><td class="tdLabel">Address: </td><td><%# Eval("Address") & ", " & Eval("City") & ", " & Eval("State") & " " & Eval("Postcode")%></td></tr>                        <tr><td class="tdLabel">Phone: </td><td><%# Eval("Phone")%></td></tr>
                        <tr><td class="tdLabel">Email: </td><td><%# Eval("Email")%></td></tr>
                        <tr><td class="tdLabel" colspan="2"><br />Emergency Contact Details:</td></tr>
                        <tr><td class="tdLabel">Name: </td><td><%# Eval("ContactName")%></td></tr>
                        <tr><td class="tdLabel">Phone: </td><td><%# Eval("ContactPhone")%></td></tr>        
                        <tr><td class="tdLabel">Relation: </td><td><%# Eval("ContactRelation")%></td></tr>
                    </table>
                </div>  
            </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel></div><!-- END Clients List -->    
    <input id="itemID" type="text" runat="server" class="hidden" clientidmode="Static" />
    <input id="btnRefreshClients" type="button" runat="server" class="hidden" clientidmode="Static" />
    </div>

    <!-- Client Popup -->
    <div id="client_popup" class="popup_block">
        <div id="tabs">
            <ul>
                <li><a href="#ClientDetails">Client Details</a></li>
                <li><a href="#CaseHistory">Case History</a></li>
            </ul>
            <div id="ClientDetails">
                <label for="Firstname" class="label">Firstname:</label>
                <input id="Firstname" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="Lastname" class="label">Lastname:</label>
                <input id="Lastname" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="DOB" class="label">Date of Birth:</label>
                <input id="DOB" type="text" class="validate[required] textbox" clientidmode="Static"readonly /><br />
                <label for="Address" class="label">Address:</label>
                <input id="Address" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="City" class="label">City:</label>
                <input id="City" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="State" class="label">State:</label>
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
                <label for="Postcode" class="label">Postcode:</label>
                <input id="Postcode" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="Phone" class="label">Phone:</label>
                <input id="Phone" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="Email" class="label">Email:</label>
                <input id="Email" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="Username" class="label">Username:</label>
                <input id="Username" type="text" class="validate[required] textbox" clientidmode="Static" /><br /><br />
                <label><strong>Emergency Contact Details</strong></label><br />
                <label for="ContactName">Contact Name:</label>
                <input id="ContactName" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="ContactPhone">Contact Phone:</label>
                <input id="ContactPhone" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
                <label for="Relation" class="label">Relation:</label>
                <input id="Relation" type="text" class="validate[required] textbox" clientidmode="Static" /><br /><br />
                <label for="Newsletter">Sign up for free newsletter:</label>
                <input id="Newsletter" type="checkbox" clientidmode="Static" checked="checked" />
                <div class="right">
                    <input type="submit" id="btnSave" title="Save client." value="Save" onclick="page = 'client';"
                        class="btn save close" />
                </div>
            </div>
            <div id="CaseHistory">
                <div id="appointmentsList"></div>
            </div>
        </div>
    </div>
        <!-- End Client Popup -->
  
    <!-- Appointment Popup -->
    <div id="appointment_popup" class="popup_block">
        <div class="title"><span>Create Appointment</span></div>
        <input id="appointmentID" type="text" class="hidden" clientidmode="Static" value="0"/>
        <input id="AppClientID" type="text" class="hidden" clientidmode="Static" value="0" />
        <label for="AppFirstname" class="label">Firstname:</label>
        <input id="AppFirstname" type="text" class="textbox" clientidmode="Static" readonly/><br />
        <label for="AppLastname" class="label">Lastname:</label>
        <input id="AppLastname" type="text" class="textbox" clientidmode="Static" readonly/><br />
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
        <div class="right">
            <input type="submit" id="btnCreateAppointment" title="Create appointment." value="Save" class="btn save" onclick="page = 'appointment'; activateValidation('appointment_popup');" />
        </div>
    </div>

    <!-- Case History Popup -->
    <a href="#?w=650" rel="caseHistory_popup" class="poplight"><input type="button" id="btnCreateCaseHistory" class="hidden" /></a>
    <div id="caseHistory_popup" class="popup_block">
        <div id="createHistoryTabs">
            <ul>
                <li><a href="#Symptoms">Symptoms</a></li>
                <li><a href="#Practitioners">Practitioners</a></li>
                <li><a href="#History">History</a></li>
                <li><a href="#Diagnosis">Diagnosis</a></li>
                <li><a href="#Suggestions">Suggestions</a></li>
            </ul>
            <div id="Symptoms">
                <input id="caseAppointmentID" type="text" class="hidden" clientidmode="Static" />
                <input id="caseClientID" type="text" class="hidden" clientidmode="Static" />
                <input id="caseThreatmentID" type="text" class="hidden" clientidmode="Static" />
                <label for="caseDate" class="label">Date:</label>
                <input id="caseDate" type="text" class="textbox" clientidmode="Static" readonly /><br />
                <label for="caseSymptoms" class="label">Symptoms:</label>
                <asp:TextBox id="caseSymptoms" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
            </div>
            <div id="Practitioners">
                <label for="caseMedications" class="label">Medications:</label>
                <asp:TextBox id="caseMedications" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="casePractitioners" class="label">Practitioners:</label>
                <asp:TextBox id="casePractitioners" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseTherapies" class="label">Therapies:</label>
                <asp:TextBox id="caseTherapies" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
            </div>
            <div id="History">
                <label for="caseHealthHistory" class="label">Health History:</label>
                <asp:TextBox id="caseHealthHistory" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseFamilyHistory" class="label">Family History:</label>
                <asp:TextBox id="caseFamilyHistory" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
            </div>
            <div id="Diagnosis">
                <label for="casePrakrti" class="label">Prakrti:</label>
                <input id="casePrakrti" type="text" class="textbox" clientidmode="Static" /><br />
                <label for="caseVikrti" class="label">Vikrti:</label>
                <input id="caseVikrti" type="text" class="textbox" clientidmode="Static" /><br />
                <label for="caseAgni" class="label">Agni:</label>
                <asp:TextBox id="caseAgni" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseMalas" class="label">Malas:</label>
                <asp:TextBox id="caseMalas" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseOjas" class="label">Ojas:</label>
                <asp:TextBox id="caseOjas" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseManas" class="label">Manas:</label>
                <asp:TextBox id="caseManas" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseSrotas" class="label">Srotas:</label>
                <asp:TextBox id="caseSrotas" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseComments" class="label">Comments:</label>
                <asp:TextBox id="caseComments" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
            </div>
            <div id="Suggestions">
                <label for="caseSuggestedTherapies" class="label">Suggested Therapies:</label>
                <asp:TextBox id="caseSuggestedTherapies" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseSuggestedHerbs" class="label">Suggested Herbs:</label>
                <asp:TextBox id="caseSuggestedHerbs" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseSuggestedPlan" class="label">Suggested Plan:</label>
                <asp:TextBox id="caseSuggestedPlan" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
                <label for="caseSuggestedCleanses" class="label">Suggested Cleanses:</label>
                <asp:TextBox id="caseSuggestedCleanses" runat="server" CssClass="textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
            </div>
        </div>
        <div id="saveButton" class="right">
            <input type="button" id="btnSaveCaseHistory" title="Save case history." value="Save" class="btn save" onclick="saveCaseHistory();" />
        </div>
    </div>
</asp:Content>
