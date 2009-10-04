<%@ Page Title="ASP.NET MVP: Shared Presenter" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="SharedPresenter.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.SharedPresenter" %>
<%@ Register Src="~/Controls/SharedPresenterControl.ascx" TagPrefix="uc" TagName="Shared" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Shared Presenter Demo</h1>
    <uc:Shared runat="server" />
    <uc:Shared runat="server" />
</asp:Content>