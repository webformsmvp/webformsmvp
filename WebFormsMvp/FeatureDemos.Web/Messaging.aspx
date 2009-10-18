<%@ Page Title="ASP.NET Web Forms MVP: Cross Presenter Messaging" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="Messaging.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.Messaging" %>
<%@ Register Src="~/Controls/Messaging1Control.ascx" TagPrefix="uc" TagName="Messaging1" %>
<%@ Register Src="~/Controls/Messaging2Control.ascx" TagPrefix="uc" TagName="Messaging2" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Cross Presenter Messaging Demo</h1>
    <uc:Messaging1 runat="server" />
    <uc:Messaging2 runat="server" />
</asp:Content>