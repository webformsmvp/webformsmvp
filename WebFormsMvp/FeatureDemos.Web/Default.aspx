<%@ Page Title="ASP.NET MVP Home" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.Default" %>
<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
    <h1>ASP.NET Web Forms Model View Presenter</h1>
    <asp:TreeView runat="server" DataSourceID="sitemap" />
    <asp:SiteMapDataSource ID="sitemap" runat="server" ShowStartingNode="false" />
</asp:Content>