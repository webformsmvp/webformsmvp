<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicallyLoadedControl.ascx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.Controls.DynamicallyLoadedControl" %>
<div class="dynamically-loaded">
    <asp:PlaceHolder runat="server" Visible="<%# PresenterWasBound %>">
        <p class="success">If this content is visible, the presenter was bound successfully after the control was dynamically loaded.</p>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" Visible="<%# !PresenterWasBound %>">
        <p class="failed">If this content is visible, there was a problem in binding the dynamically loaded control's presenter.</p>
    </asp:PlaceHolder>
</div>