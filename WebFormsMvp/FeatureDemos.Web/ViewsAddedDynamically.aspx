<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="ViewsAddedDynamically.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.ViewsAddedDynamically" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <asp:ScriptManager runat="server" />
    <h1>View User Controls Being Added in Page_Init</h1>
    <p>This page demonstrates a user control with a declared presenter binding being dynamically loaded into the page.</p>
    <asp:UpdatePanel runat="server" ID="mainUpdatePanel" ChildrenAsTriggers="False" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" Text="Load control" ID="LoadDynamicControl" OnClick="LoadDynamicControl_OnClick"/>
            <asp:Panel ID="dynamicallyLoadedControlsPlaceholder" runat="server"></asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>