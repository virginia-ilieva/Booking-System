<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ManagementMenu.ascx.vb" Inherits="Controls_ManagementMenu" %>
<div>
    <asp:HyperLink ID="lnkEmployees" runat="server" Text="Workers" NavigateUrl="~/workers.aspx"></asp:HyperLink><br />
    <asp:HyperLink ID="lnkBookingObjects" runat="server" Text="Clinics" NavigateUrl="~/clinics.aspx"></asp:HyperLink><br />
    <asp:HyperLink ID="lnkTherapies" runat="server" Text="Treatments" NavigateUrl="~/treatments.aspx"></asp:HyperLink><br />
  </div>
