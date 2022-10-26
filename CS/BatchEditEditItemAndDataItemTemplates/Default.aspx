<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v19.1, Version=19.1.16.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASPxGridView-Batch-Edit-How-to-use-EditItemTemplate-with-DataItemTemplate</title>
    <script type="text/javascript" src="BatchEditController.js"></script>
    <style>
        .ratingControl {
            float:right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxGridView ID="ASPxGridView1" runat="server" KeyFieldName="ID" ClientInstanceName="grid" Width="350" DataSourceID="ObjectDataSource1">
                <Columns>
                    <dx:GridViewCommandColumn VisibleIndex="0" ShowDeleteButton="True" />
                    <dx:GridViewDataColumn FieldName="ID" VisibleIndex="1">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="RatingValue" VisibleIndex="2">
                        <DataItemTemplate>
                            <dx:ASPxRatingControl ID="ratingControl" runat="server" ClientInstanceName='<%# "DataItemRateControl"+ Container.VisibleIndex %>'
                                ItemCount="5" Value='<%# Convert.ToInt32(Eval("RatingValue")) %>' CssClass="ratingControl">
                                <ClientSideEvents ItemClick="OnItemMouseClick_DataItem" ItemMouseOver="OnItemMouseOver_DataItem" ItemMouseOut="OnItemMouseOut_DataItem" />
                            </dx:ASPxRatingControl>
                        </DataItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxRatingControl ID="ratingControl" runat="server" ClientInstanceName="EditItemRateControl" ItemCount="5">
                                <ClientSideEvents ItemClick="OnItemMouseClick_EditItem" Init="OnRateControlInit_EditItem" />
                            </dx:ASPxRatingControl>
                        </EditItemTemplate>
                    </dx:GridViewDataColumn>
                </Columns>
                <SettingsEditing Mode="Batch">
                    <BatchEditSettings EditMode="Cell" AllowRegularDataItemTemplate="true" StartEditAction="FocusedCellClick" />
                </SettingsEditing>
                <ClientSideEvents BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing" FocusedCellChanging="OnFocusedCellChanging" Init="OnGridInit" />
            </dx:ASPxGridView>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetProducts" TypeName="DataModel" DataObjectTypeName="Product" UpdateMethod="UpdateProducts" InsertMethod="InsertProduct" DeleteMethod="RemoveProduct" />
        </div>
    </form>
</body>
</html>
