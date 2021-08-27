<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/195209760/19.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T828659)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/BatchEditEditItemAndDataItemTemplates/Default.aspx) (VB: [Default.aspx](./VB/BatchEditEditItemAndDataItemTemplates/Default.aspx))
* [Default.aspx.cs](./CS/BatchEditEditItemAndDataItemTemplates/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/BatchEditEditItemAndDataItemTemplates/Default.aspx.vb))
* [BatchEditController.js](./CS/BatchEditEditItemAndDataItemTemplates/BatchEditController.js) (VB: [BatchEditController.js](./VB/BatchEditEditItemAndDataItemTemplates/BatchEditController.js))
<!-- default file list end -->
# ASPxGridView  - Batch Edit - How to use EditItemTemplate with DataItemTemplate.
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/195209760/)**
<!-- run online end -->

This example shows how to use DataItemTemplate and EditItemTemplate in Batch Edit mode. The templates use ASPxRatingControl as content. 

### Follow these steps: 

1. Add ASPxGridView to your page and configure it as follows:
```aspx
<dx:ASPxGridView ID="ASPxGridView1" runat="server" KeyFieldName="ID" ClientInstanceName="grid" DataSourceID="ObjectDataSource1">	
	...
	<SettingsEditing Mode="Batch">
		<BatchEditSettings EditMode="Cell" AllowRegularDataItemTemplate="true" StartEditAction="FocusedCellClick" />
	</SettingsEditing>
	<ClientSideEvents BatchEditStartEditing="OnBatchEditStartEditing"
		BatchEditEndEditing="OnBatchEditEndEditing"
		FocusedCellChanging="OnFocusedCellChanging" Init="OnGridInit" />
</dx:ASPxGridView>
```
>Note the values of the [AllowRegularDataItemTemplate][1] and [StartEditAction][2] properties.

2. Add controls to the DataItemTemplate and EditItemTemplate. Since DataItemTemplate generates multiple instances of a control, the control's ClientInstanceName need to be set dynamically. ClientInstanceName for the control in EditItemTemplate is set statically.

```aspx
 <DataItemTemplate>
	<dx:ASPxRatingControl ID="ratingControl" runat="server"
		ClientInstanceName='<%# "DataItemRateControl"+ Container.VisibleIndex %>' 
			ItemCount="5" Value='<%# Convert.ToInt32(Eval("RatingValue")) %>'>
			<ClientSideEvents ItemClick="OnItemMouseClick_DataItem"
				 ItemMouseOver="OnItemMouseOver_DataItem" ItemMouseOut="OnItemMouseOut_DataItem" />
	</dx:ASPxRatingControl>
</DataItemTemplate>
<EditItemTemplate>
	<dx:ASPxRatingControl ID="ratingControl" runat="server" ClientInstanceName="EditItemRateControl" ItemCount="5">
		<ClientSideEvents ItemClick="OnItemMouseClick_EditItem" Init="OnRateControlInit_EditItem" />
	</dx:ASPxRatingControl>
</EditItemTemplate>
```

3. Add handler for the client-side [BatchEditStartEditing][3] event to set the ASPxRatingControl value inside the EditItemTemplate when editing starts. To save the selected value in the grid when editing ends, use [OnBatchEditEndEditing][4].
```javascript
function OnBatchEditStartEditing(s, e) {
    EditItemRateControl.SetValue(e.rowValues[s.GetColumnByField("RatingValue").index].value);
}
function OnBatchEditEndEditing(s, e) {
    var templateColumn = s.GetColumnByField("RatingValue");
    if (!e.rowValues.hasOwnProperty(templateColumn.index))
        return;
    var cellInfo = e.rowValues[templateColumn.index];
    cellInfo.value = EditItemRateControl.GetValue();
    SetRateControlValueByRowIndex_DataItem(e.visibleIndex, cellInfo.value);
}
```
4. Add the client-side ItemMouseClick handler for controls in EditItemTemplate and DataItemTemplate separately.
When the EditItemTemplate's ASPxRatingControl item is clicked, finish editing with the [batchEditApi.EndEdit][5] method. When the event occurs in ASPxRatingControl from DataItemTemplate, set a cell value with the [batchEditApi.SetCellValue][6] method.

```javascript
function OnItemMouseClick_EditItem(s, e) {
    grid.batchEditApi.EndEdit();
}
function OnItemMouseClick_DataItem(s, e) {
    grid.batchEditApi.SetCellValue(currentFocusedCell.itemVisibleIndex, currentFocusedCell.column.index, s.GetValue());
}
```
5. Add keyboard navigation to the grid. Handle the ASPxClientGridView.Init event and subscribe to the keydown event there. Then, process different key codes in this handler: 
```javascript
function OnGridInit(s, e) {
    ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "keydown", function (evt) {
        return OnKeyDown(evt, s);
    });
}
function OnGridViewKeyDown(evt, grid) {
    if (typeof (event) != "undefined" && event != null)
        evt = event;
    if (!grid.InCallback() && NeedProcessDocumentKeyDown(evt)) {
        if (evt.shiftKey && evt.keyCode == 9 /*Shift + tab */) {
            setTimeout(function () {
                grid.batchEditApi.MoveFocusBackward();
            }, 0);
        } else if (evt.keyCode == 9 /*Tab key*/) {
            setTimeout(function () {
                grid.batchEditApi.MoveFocusForward();
            }, 0);
        }  
    }
}
```
6. Use the approach from p.5 to add keyboard support in ASPxRatingControl:
```javascript
function OnRateControlInit_EditItem(s, e) {
    ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "keydown", function (evt) {
        return OnRatingControlKeyDown(evt, s);
    });
}
function OnRatingControlKeyDown(evt, ratingControl) {
    if (typeof (event) != "undefined" && event != null)
        evt = event;
    if (!ratingControl.InCallback() && NeedProcessDocumentKeyDown(evt)) {
        if (evt.keyCode == 32 /*Space bar*/) {
            setTimeout(function () {
                MoveFocusToNextStar();
            }, 0);
        }
    }
}
```


[1]: https://documentation.devexpress.com/AspNet/DevExpress.Web.GridBatchEditSettings.AllowRegularDataItemTemplate.property
[2]: https://documentation.devexpress.com/AspNet/DevExpress.Web.GridBatchEditSettings.StartEditAction.property
[3]: https://docs.devexpress.com/AspNet/js-ASPxClientGridView.BatchEditStartEditing
[4]: https://docs.devexpress.com/AspNet/js-ASPxClientGridView.BatchEditEndEditing
[5]: https://docs.devexpress.com/AspNet/js-ASPxClientGridViewBatchEditApi.EndEdit
[6]: https://docs.devexpress.com/AspNet/js-ASPxClientGridViewBatchEditApi.SetCellValue(visibleIndex-columnFieldNameOrId-value)
