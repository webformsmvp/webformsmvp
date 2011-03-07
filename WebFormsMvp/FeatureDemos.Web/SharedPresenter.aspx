<%@ Page Title="ASP.NET MVP: Shared Presenter" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="SharedPresenter.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.SharedPresenter" %>
<%@ Register Src="~/Controls/SharedPresenterControl.ascx" TagPrefix="uc" TagName="Shared" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Shared Presenter Demo</h1>
    <p>
        Normally, each control instance would have their own presenter instance.
        Shared Presenters lets you attach one presenter instance to multiple
        control instances instead.
    </p>
    <p>
        This is done transparently, without requiring any changes to the view or presenter.
    </p>
    <p>
        If this is working, these two GUIDs will match:
    </p>
    <uc:Shared runat="server" />
    <uc:Shared runat="server" />
</asp:Content>