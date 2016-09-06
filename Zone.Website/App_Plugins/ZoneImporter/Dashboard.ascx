<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.ascx.cs" Inherits="Zone.Website.App_Plugins.ZoneImporter.Dashboard" %>

<div id="zoneImporterDashboard">
    <h2>Zone Importer</h2>
    <p>
        This will allow you to overwrite the stored values within the database for property data (cmsPropertyData).
        This is particularly useful when you have old data that may not be compatible with the latest version of Umbraco, or you want to switch to a different format.
        Do not be alarmed if it seems to take ages as there may be a lot of data, however do <strong>use with caution!</strong>
    </p>
    <hr/>
    <p>
        <strong>Available converters: <asp:Label ID="lblConverters" runat="server">Loading...</asp:Label></strong>
    </p>

    <h3>Settings:</h3>
    <p>
        Select a data type to convert.
    </p>
    <p>
        <asp:Label ID="lblDataTypes" runat="server" AssociatedControlID="ddlDataTypes">Data Type</asp:Label>
        <asp:DropDownList ID="ddlDataTypes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDataTypes_SelectedIndexChanged" CssClass="setting-dropdown">
            <asp:ListItem Value="0">-- Select --</asp:ListItem>
        </asp:DropDownList>
    </p>
    <p>
        Select a document type for this data type. 
    </p>
    <p>
        <asp:Label ID="lblDocTypes" runat="server" AssociatedControlID="ddlDocTypes">Document Type</asp:Label>
        <asp:DropDownList ID="ddlDocTypes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDocTypes_SelectedIndexChanged" CssClass="setting-dropdown">
            <asp:ListItem Value="0">-- Select --</asp:ListItem>
        </asp:DropDownList>
    </p>

    <h3>Convert:</h3>
    <p>
        Run the conversion based on the settings above.
    </p>
    <asp:Button ID="btnConvert" runat="server" Enabled="false" Text="Convert data type" CssClass="btn btn-success" Width="150px" OnClick="btnConvert_Click"/>   
    <hr/>
    <div style="margin-right:20px">
        <asp:Panel ID="pnlAlert" runat="server" role="alert" Visible="false">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>
    </div>
</div>