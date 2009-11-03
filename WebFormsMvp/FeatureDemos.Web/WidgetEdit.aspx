<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="WidgetEdit.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.WidgetEdit" %>
<%@ Register Src="~/Controls/EditWidgetControl.ascx" TagPrefix="uc" TagName="EditWidget" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>2-Way Data-binding Demo: Widget CRUD</h1>
    <uc:EditWidget runat="server" />
</asp:Content>