<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LookupWidgetControl.ascx.cs" Inherits="WebFormsMvp.Sample.Web.Controls.LookupWidgetControl" %>
<div class="lookup-widget">
  <fieldset><legend>Enter ID or Name of widget</legend>
    <ol>
      <li>
        <asp:Label runat="server" AssociatedControlID="widgetId">ID: </asp:Label>
        <asp:TextBox runat="server" ID="widgetId" MaxLength="9" />
        <div class="validators">
          <asp:CompareValidator runat="server" ControlToValidate="widgetId"
            ValidationGroup="LookupWidget"
            Display="Dynamic" Type="Integer" Operator="DataTypeCheck"
            ErrorMessage="ID must be a valid whole number" />
          <asp:RangeValidator runat="server" ControlToValidate="widgetId"
            ValidationGroup="LookupWidget"
            Display="Dynamic" Type="Integer" MinimumValue="1" MaximumValue="9999999"
            ErrorMessage="ID must be a positive whole number" />
        </div>
      </li>
      <li>
        <asp:Label runat="server" AssociatedControlID="widgetName">Name: </asp:Label>
        <asp:TextBox runat="server" ID="widgetName" MaxLength="255" />
      </li>
    </ol>
    <p>
      <asp:Button runat="server" Text="Find" ValidationGroup="LookupWidget" />
    </p>
  </fieldset>
  <div class="results">
    <asp:DetailsView ID="results" runat="server" DataSourceID="resultsData"
      EmptyDataText="No matching results found"
      Visible="<%# Model.ShowResults %>" />
    <mvp:MvpDataSource ID="resultsData" runat="server"
        SelectEvent="Finding" SelectResult="Widgets">
        <SelectParameters>
            <asp:ControlParameter ControlID="widgetId" Type="Int32" PropertyName="Text" DefaultValue="1" />
            <asp:ControlParameter ControlID="widgetName" Type="String" PropertyName="Text" /> 
        </SelectParameters>
    </mvp:MvpDataSource>
  </div>
</div>