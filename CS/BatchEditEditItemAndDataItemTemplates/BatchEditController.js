var isDataItemEditing = true;
var currentFocusedCell = -1;

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
function SetRateControlValueByRowIndex_DataItem(rowIndex, rateValue) {
    var rateControl = ASPxClientRatingControl.Cast("DataItemRateControl" + rowIndex);
    if (rateControl)
        rateControl.SetValue(rateValue);
}
function OnItemMouseClick_EditItem(s, e) {
    grid.batchEditApi.EndEdit();
}
function OnItemMouseClick_DataItem(s, e) {
    grid.batchEditApi.SetCellValue(currentFocusedCell.itemVisibleIndex, currentFocusedCell.column.index, s.GetValue());
}
function OnFocusedCellChanging(s, e) {
    currentFocusedCell = e.cellInfo;
    if (isDataItemEditing == false) {
        setTimeout(function () {
            grid.batchEditApi.StartEdit(e.cellInfo.itemVisibleIndex, e.cellInfo.column.index);
        }, 0);
    }
}
function OnGridInit(s, e) {
    ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "keydown", function (evt) {
        return OnGridViewKeyDown(evt, s);
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
function NeedProcessDocumentKeyDown(evt) {
    var evtSrc = ASPxClientUtils.GetEventSource(evt);
    if (evtSrc.tagName == "INPUT")
        return evtSrc.type != "text" || evtSrc.type != "password";
    else
        return evtSrc.tagName != "TEXTAREA";
}
function MoveFocusToNextStar() {
    var activeNode = document.activeElement;
    var activeNodeIndex = activeNode.getAttribute('dxindex');
    var parentNode = activeNode.parentElement;
    var nextNode = parentNode.querySelector('[dxindex="' + (+activeNodeIndex + 1) + '"]');
    if (!nextNode) {
        nextNode = parentNode.querySelector('[dxindex="0"]');
    }
    nextNode.focus();
}
function OnItemMouseOver_DataItem() {
    isDataItemEditing = true;
}
function OnItemMouseOut_DataItem() {
    isDataItemEditing = false;
}
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
