<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditWidgetControl.ascx.cs" Inherits="WebFormsMvp.FeatureDemos.Web.Controls.EditWidgetControl" %>
<div class="edit-widget">
    <asp:FormView runat="server" DataSourceID="widgetDataSource" DefaultMode="ReadOnly"
        DataKeyNames="Id" AllowPaging="true">
        <ItemTemplate>
            <dl>
                <dt>ID:</dt><dd><%# Eval("Id") %></dd>
                <dt>Name:</dt><dd><%# Eval("Name") %></dd>
                <dt>Description:</dt><dd><%# Eval("Description") %></dd>
            </dl>
            <asp:Button runat="server" CommandName="New" Text="New" />
            <asp:Button runat="server" CommandName="Edit" Text="Edit" />
            <asp:Button runat="server" CommandName="Delete" Text="Delete" />
        </ItemTemplate>
        <EditItemTemplate>
            <fieldset>
                <legend>Widget Details</legend>
                <ol>
                    <li class="widget-id">
                        <asp:Label runat="server" AssociatedControlID="widgetId">ID:</asp:Label>
                        <asp:TextBox runat="server" ID="widgetId" Text='<%# Bind("Id") %>' ReadOnly="true" />
                    </li>
                    <li class="widget-name">
                        <asp:Label runat="server" AssociatedControlID="widgetName">Name:</asp:Label>
                        <asp:TextBox runat="server" ID="widgetName" Text='<%# Bind("Name") %>' />
                        <div class="validation">
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="Edit"
                                ControlToValidate="widgetName" Display="Dynamic"
                                ErrorMessage="Please enter a value for Name" />
                        </div>
                    </li>
                    <li class="widget-name">
                        <asp:Label runat="server" AssociatedControlID="widgetDescription">Description:</asp:Label>
                        <asp:TextBox runat="server" ID="widgetDescription" Text='<%# Bind("Description") %>' TextMode="MultiLine" />
                        <div class="validation">
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="Edit"
                                ControlToValidate="widgetDescription" Display="Dynamic"
                                ErrorMessage="Please enter a value for Description" />
                        </div>
                    </li>
                    <li class="action save">
                        <asp:Button runat="server" CommandName="Update" Text="Save" ValidationGroup="Edit" />
                    </li>
                    <li class="action cancel">
                        <asp:Button runat="server" CommandName="Cancel" Text="Cancel" />
                    </li>
                </ol>
            </fieldset>
        </EditItemTemplate>
        <InsertItemTemplate>
            <fieldset>
                <legend>New Widget Details</legend>
                <ol>
                    <li class="widget-name">
                        <asp:Label runat="server" AssociatedControlID="widgetName">Name:</asp:Label>
                        <asp:TextBox runat="server" ID="widgetName" Text='<%# Bind("Name") %>' />
                    </li>
                    <li class="widget-name">
                        <asp:Label runat="server" AssociatedControlID="widgetDescription">Description:</asp:Label>
                        <asp:TextBox runat="server" ID="widgetDescription" Text='<%# Bind("Description") %>' TextMode="MultiLine" />
                    </li>
                    <li class="action save">
                        <asp:Button runat="server" CommandName="Insert" Text="Save" />
                    </li>
                    <li class="action cancel">
                        <asp:Button runat="server" CommandName="Cancel" Text="Cancel" />
                    </li>
                </ol>
            </fieldset>
        </InsertItemTemplate>
        <EmptyDataTemplate>
            <p class="empty">Widget could not be found</p>
        </EmptyDataTemplate>
    </asp:FormView>
    
    <mvp:PageDataSource ID="widgetDataSource" runat="server"
        EnablePaging="true"
        DataObjectTypeName="WebFormsMvp.FeatureDemos.Logic.Data.Widget"
        ConflictDetection="CompareAllValues"
        OldValuesParameterFormatString="original{0}"
        SelectMethod="GetWidgets"
        SelectCountMethod="GetWidgetsCount"
        UpdateMethod="UpdateWidget"
        InsertMethod="InsertWidget"
        DeleteMethod="DeleteWidget">
    </mvp:PageDataSource>
</div>