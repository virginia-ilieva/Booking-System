<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="treatments.aspx.vb" Inherits="AyurvedaHouse_BookingSystem.treatments1" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
<script type="text/javascript" src="js/pages/treatments.js"></script>
<style type="text/css">
    .Status { font-family:Calibri; }
</style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" runat="server">
    <div style="width:470px;">  
    <h3>Manage Treatments</h3>
    <!-- Treatment List -->   
    <asp:ScriptManager id="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upTreatments" runat="server">
        <ContentTemplate>
            <asp:Repeater ID="TreatmentsList" runat="server">
            <HeaderTemplate>
                <div class="right">
                    <a href="#?w=500" rel="treatment_popup" class="poplight"><input type="button" class="btn add" title="Create new treatment." value="Add Therapy" onclick="return showPopup(0);"/></a>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="acc_trigger">
                    <table id="treatmentHeader"><tr>
                        <td style="width:385px;"><span><%# Eval("Name") & " - "%></span><label class="Status"><%# Eval("Status")%></label></td>    
                        <td class="right" style="width:40px;">
                            <input type="button" title="Reactivate treatment." class="gridButton refresh" onclick="reactivateTreatment(<%# Eval("ID")%>);" />
                            <a href="#?w=500" rel="treatment_popup" class="poplight"><input type="button" title="Edit treatment." id="btnEdit" class="gridButton edit" onclick='return showPopup(<%# Eval("ID")%>);' /></a>
                        </td>
                    </tr></table>
                </div>
                <div class="acc_container">
                    <table>
                        <tr><td class="tdLabel">Duration: </td><td class="Duration"><%# Eval("Duration")%></td></tr>  
                        <tr><td class="tdLabel">Price: </td><td>$<%# Eval("Price")%></td></tr>        
                        <tr><td class="tdLabel">Description: </td><td><%# Eval("Description")%></td></tr>
                    </table>
                </div>  
            </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel><!-- Treatment List -->    
    <input id="itemID" type="text" runat="server" class="hidden" clientidmode="Static" />
    <input id="btnRefreshTreatments" type="button" runat="server" class="hidden" clientidmode="Static" />
    </div>

    <!-- Treatment Popup -->
    <div id="treatment_popup" class="popup_block">
        <div class="title"><span>Therapy Details</span></div>
        <label for="Name" class="label">Therapy:</label>
        <input id="Name" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Description" class="label" >Description:</label>
        <asp:TextBox id="Description" runat="server" CssClass="validate[required] textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
        <label for="Price" class="label" >Price:</label>
        <input id="Price" type="text" class="validate[required] textbox right" clientidmode="Static" /><br />
        <label for="Duration" class="label" >Duration:</label>
        <select id="Duration" class="validate[required,funcCall[validateSelect]] dropdown" clientidmode="Static">
            <option value="0">Select one</option>
            <option value="15">15 min</option>
            <option value="30">30 min</option>
            <option value="45">45 min</option>
            <option value="60">1h</option>
            <option value="75">1h 15min</option>
            <option value="90">1h 30min</option>
            <option value="105">1h 45min</option>
            <option value="120">2h</option>
            <option value="135">2h 15min</option>
            <option value="150">2h 30min</option>
            <option value="165">2h 45min</option>
            <option value="180">3h</option>
            <option value="1">All day</option>
        </select><br />
        <label for="Status" class="label" >Status:</label>
        <select id="Status" class="validate[required,funcCall[validateSelect]] dropdown" clientidmode="Static">
            <option value="0">Select status</option>
            <option value="1">Active</option>
            <option value="2">Inactive</option>
        </select><br />
        <div class="right">
            <input type="submit" id="btnSave" title="Save therapy." value="Save" class="btn save close" />
        </div>
    </div><!-- End Treatment Popup -->
  
</asp:Content>