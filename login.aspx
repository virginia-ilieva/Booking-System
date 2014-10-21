<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/main.Master" CodeBehind="login.aspx.vb" Inherits="AyurvedaHouse_BookingSystem.login" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadPage" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainPage" runat="server">
<div style="width:450px">
    <div class="title"><span>Log In</span></div> 
    <div style="padding:10px; border:1px solid Gray; background-color:#eee;">
        <table align="center">
            <tr>
                <td><label for="txtUserName" class="label">Username:</label></td>
                <td><input id="txtUserName" runat="server" type="text" class="textbox" clientidmode="Static"/></td>
            </tr>
            <tr>
                <td><label for="txtPassword" class="label">Password:</label></td>
                <td><input id="txtPassword" runat="server" type="password" class="textbox" clientidmode="Static"/></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="btnCreateAccount" runat="server" PostBackUrl="~/signup.aspx" 
                        Font-Size="10px" ForeColor="Black" CausesValidation="False">Create Account</asp:LinkButton> | 
                    <asp:LinkButton ID="btnForgotPassword" runat="server" PostBackUrl="~/passwordRecovery.aspx" 
                        Font-Size="10px" ForeColor="Black" CausesValidation="False">Forgot your password?</asp:LinkButton><br /><br />
                </td>
            </tr>
            <tr>
                <td class="right" colspan="2">
                    <input id="btnLogin" runat="server" type="submit" title="Login" value="Login" class="btn lock"/>
                </td>
            </tr>
        </table>
    </div>
</div>
</asp:Content>
