<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="workers.aspx.vb" Inherits="AyurvedaHouse_BookingSystem._workers" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
<script type="text/javascript" src="js/pages/workers.js"></script>
<style type="text/css">
    .Status { font-family:Calibri; }
</style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" runat="server">
    <div style="width:470px;">
    <h3>Manage Workers</h3>
    <!-- Workers List -->   
    <asp:ScriptManager id="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upWorkers" runat="server">
        <ContentTemplate>
            <asp:Repeater ID="WorkersList" runat="server">
            <HeaderTemplate>
                <div class="right">
                    <a href="#?w=500" rel="worker_popup" class="poplight"><input type="button" class="btn add" title="Create new worker." value="Add Worker" onclick="return showPopup(0);"/></a>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="acc_trigger">
                    <table id="workerHeader"><tr>
                        <td style="width:385px;"><span><%# Eval("Fullname") & " - "%></span><label class="Status"><%# Eval("Status")%></label></td>    
                        <td class="right" style="width:40px;">
                            <input type="button" title="Reactivate worker." class="gridButton refresh" onclick="reactivateWorker(<%# Eval("ID")%>);" />
                            <a href="#?w=500" rel="worker_popup" class="poplight"><input type="button" title="Edit worker." id="btnEdit" class="gridButton edit" onclick='return showPopup(<%# Eval("ID")%>);' /></a>
                        </td>
                    </tr></table>
                </div>
                <div class="acc_container">
                    <table>
                        <tr><td class="tdLabel">Name: </td><td><%# Eval("fullname")%></td></tr>  
                        <tr><td class="tdLabel">Username: </td><td><%# Eval("Username")%></td></tr>        
                        <tr><td class="tdLabel">Password: </td><td><%# Eval("Password")%></td></tr>   
                        <tr><td class="tdLabel">Phone: </td><td><%# Eval("Phone")%></td></tr>
                        <tr><td class="tdLabel">Email: </td><td><%# Eval("Email")%></td></tr>
                    </table>
                </div>  
            </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel><!-- Workers List -->    
    <input id="itemID" type="text" runat="server" class="hidden" clientidmode="Static" />
    <input id="btnRefreshWorkers" type="button" runat="server" class="hidden" clientidmode="Static" />
    </div>

    <!-- Worker Popup -->
    <div id="worker_popup" class="popup_block">
        <div class="title"><span>Worker Details</span></div>
        <label for="Firstname" class="label">Firstname:</label>
        <input id="Firstname" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Lastname" class="label">Lastname:</label>
        <input id="Lastname" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Username" class="label" >Username:</label>
        <input id="Username" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Password" class="label" >Password:</label>
        <input id="Password" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Phone" class="label" >Phone:</label>
        <input id="Phone" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Email" class="label" >Email:</label>
        <input id="Email" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
        <label for="Status" class="label" >Status:</label>
        <select id="Status" class="validate[required,funcCall[validateSelect]] dropdown" clientidmode="Static">
            <option value="0">Select status</option>
            <option value="1">Active</option>
            <option value="2">Inactive</option>
        </select><br />
        <div class="right">
            <input type="submit" id="btnSave" title="Save worker." value="Save" class="btn save close" />
        </div>
    </div><!-- End Worker Popup -->
  
</asp:Content>
