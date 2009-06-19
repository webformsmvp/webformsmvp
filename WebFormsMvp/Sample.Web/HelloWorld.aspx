<%@ Page Title="ASP.NET MVP: Hello World" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="HelloWorld.aspx.cs" Inherits="WebFormsMvp.Sample.Web.HelloWorld" %>
<%@ Register Src="~/Controls/HelloWorldControl.ascx" TagPrefix="uc" TagName="HelloWorld" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <h1>Hello World Demo</h1>
    <uc:HelloWorld runat="server" />
</asp:Content>