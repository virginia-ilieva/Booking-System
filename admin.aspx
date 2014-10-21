<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="admin.aspx.vb" Inherits="AyurvedaHouse_BookingSystem.admin" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
<script type="text/javascript" src="/OnlineBooking/js/pages/admin.js"></script>
<style type="text/css">
    #tabs { width:470px; }
    .multiline { height:250px; }
    #NotificationDescription { width:380px; }
</style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" runat="server">
    <div id="tabs">
        <ul>
            <li><a href="#GeneralDetails">General Details</a></li>
            <li><a href="#Notifications">Notifications</a></li>
            <li><a href="#TermsOfUse">Terms Of Use</a></li>
            <li><a href="#ReportBugs">Report Bugs</a></li>
        </ul>
        <div id="GeneralDetails">
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
            <div class="right">
                <input type="submit" id="btnSave" title="Save changes." value="Save" class="btn save" onclick="page = 'general'; activateValidation('GeneralDetails');" />
            </div>
        </div>
        <div id="Notifications">
            <!-- Notifications List -->   
            <asp:ScriptManager id="scriptmanager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upNotifications" runat="server">
                <ContentTemplate>
                    <asp:Repeater ID="NotificationsList" runat="server">
                    <HeaderTemplate>
                        <div class="right">
                            <a href="#?w=500" rel="notification_popup" class="poplight"><input type="button" class="btn add" title="Create new notification." value="Add Notification" onclick="return showPopup(0);"/></a>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="acc_trigger">
                            <table id="notificationHeader"><tr>
                                <td style="width:385px;"><span><%# Eval("Created", "{0:dd-MMM-yyyy}") & " - " & If(Eval("BodyContent").ToString.Length > 30, Eval("BodyContent").ToString.Substring(0, 30), Eval("BodyContent")) & " ... "%></span></td>    
                                <td class="right" style="width:40px;">
                                    <a href="#?w=500" rel="notification_popup" class="poplight"><input type="button" title="Edit notification." id="btnEdit" class="gridButton edit" onclick='return showPopup(<%# Eval("ID")%>);' /></a>
                                    <input type="button" title="Delete notification." class="gridButton delete" onclick='deleteNotification(<%# Eval("ID")%>)' />
                                </td>
                            </tr></table>
                        </div>
                        <div class="acc_container">
                            <div><%# Eval("BodyContent")%></div>
                        </div>  
                    </ItemTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel><!-- End Notifications List -->    
            <input id="itemID" type="text" runat="server" class="hidden" clientidmode="Static" />
            <input id="btnRefreshNotifications" type="button" runat="server" class="hidden" clientidmode="Static" />
        </div>
        <!-- Notifications Popup -->
        <div id="notification_popup" class="popup_block">
            <div class="title"><span>Notification Details</span></div>
            <label for="NotificationDescription" class="label">Description:</label>
            <asp:TextBox id="NotificationDescription" runat="server" CssClass="validate[required] textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
            <label for="sendToClients">Send this notification to the clients </label>
            <input id="sendToClients" type="checkbox" /><br />
            <div class="right">
                <input type="submit" id="btnSaveNotification" title="Save notification." value="Save" class="btn save close" onclick="page = 'notification'; activateValidation('notification_popup');" />
            </div>
        </div><!-- End Notifications Popup -->
        <!-- Terms of use -->
        <div id="TermsOfUse">
            <input id="termID" type="text" runat="server" class="hidden" clientidmode="Static" />
            <label for="TermsOfUseDescription" class="label">Description:</label>
            <asp:TextBox id="TermsOfUseDescription" runat="server" CssClass="validate[required] textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br />
            <div class="right">
                <input type="submit" id="btnSaveTermsOfUse" title="Save terms of use." value="Save" class="btn save" onclick="page = 'terms'; activateValidation('TermsOfUse');" />
            </div>
        </div><!-- End Terms of use -->
        <div id="ReportBugs">
            <label for="BugTitle" class="label">Title:</label>
            <input id="BugTitle" type="text" class="validate[required] textbox" clientidmode="Static" /><br />
            <label for="BugPriority" class="label">Priority:</label>
            <select id="BugPriority" class="validate[required,funcCall[validateSelect]] dropdown" clientidmode="Static">
                <option value="0">Select priority</option>
                <option value="1">High</option>
                <option value="2">Medium</option>
                <option value="3">Low</option>
            </select><br />
            <label for="BugDescription" class="label">Description:</label>
            <asp:TextBox id="BugDescription" runat="server" CssClass="validate[required] textbox multiline" TextMode="MultiLine" clientidmode="Static" /><br /><br />
            <div class="right">
                <input type="submit" id="btnReportBug" title="Send report." value="Send" class="btn mail" onclick="page = 'bug'; activateValidation('ReportBugs');" />
            </div>
        </div>
    </div>
</asp:Content>
