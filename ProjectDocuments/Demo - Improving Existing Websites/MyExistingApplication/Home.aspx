<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="MyExistingApplication.Home" %>
<%@ Register TagPrefix="uc" TagName="Welcome" Src="~/WelcomeControl.ascx" %>
<!DOCTYPE html>
<html>
    <head runat="server">
        <title>WebFormsMvp Demo</title>
    </head>
    <body>
        
        <uc:Welcome runat="server" />

    </body>
</html>