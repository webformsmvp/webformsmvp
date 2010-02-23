<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="ViewsAddedInPageInit.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.ViewsAddedInPageInit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <h1>View User Controls Being Added in Page_Init</h1>
    <p>This page demonstrates a user control with a declared presenter binding being dynamically loaded
       into the page during the Page_Init phase. This is a common scenario in CMS platforms, e.g. DNN.</p>
    <asp:Panel ID="dynamicallyLoadedControlsPlaceholder" runat="server"></asp:Panel>
</asp:Content>