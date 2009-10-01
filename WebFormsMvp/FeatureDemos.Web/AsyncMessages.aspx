<%@ Page Title="Async Tasks Demo" Async="true" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="AsyncMessages.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.AsyncMessages" %>
<%@ Register Src="~/Controls/AsyncMessagesControl.ascx" TagPrefix="uc" TagName="AsyncMessages" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Async Tasks Demo</h1>
    <uc:AsyncMessages runat="server" />
</asp:Content>