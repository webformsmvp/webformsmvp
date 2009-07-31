<%@ Page Title="ASP.NET MVP: Simple Data" Async="true" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="WidgetLookup.aspx.cs" Inherits="WebFormsMvp.Sample.Web.WidgetLookup" %>
<%@ Register Src="~/Controls/LookupWidgetControl.ascx" TagPrefix="uc" TagName="LookupWidget" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Simple Data Demo: Lookup Widget</h1>
    <uc:LookupWidget runat="server" />
</asp:Content>