<%@ Page Title="ASP.NET MVP: Composite View" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="CompositeView.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.CompositeView" %>
<%@ Register Src="~/Controls/CompositeControl.ascx" TagPrefix="uc" TagName="Composite" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Composite View Demo</h1>
    <uc:Composite runat="server" />
    <uc:Composite runat="server" />
</asp:Content>