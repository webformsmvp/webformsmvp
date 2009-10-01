<%@ Page Title="ASP.NET MVP: Redirect" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="Redirect.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.Redirect" %>
<%@ Register Src="~/Controls/RedirectControl.ascx" TagPrefix="uc" TagName="Redirect" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Redirect From Presenter Demo</h1>
    <uc:Redirect runat="server" />
</asp:Content>