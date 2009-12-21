<%@ Page Title="ASP.NET MVP: Page View" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="PageView.aspx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.PageView" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Page View Demo</h1>
    <p>In this demo, the page itself is acting as the view. We prefer to use controls but this approach works too.</p>
    <div class="hello-world">
        <%# Model.Message %>
    </div>
</asp:Content>