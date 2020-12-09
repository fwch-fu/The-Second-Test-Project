function PagingClick(CurrentPageIndex) {
    window.location.href = actionUrl + "/" + ($("#curid").length > 0 ? $("#curid").val() : 0) + "/" + CurrentPageIndex;
}
function GotoPageIndexClick(obj) {
    var InputPageNumberVal = $("#InputPageNumber").val();
    if (InputPageNumberVal == "") {
        alert("请输入有效的页码!");
    }
    window.location.href = actionUrl + "/" + ($("#curid").length > 0 ? $("#curid").val() : 0) + "/" + $("#InputPageNumber").val();
}
function PagingInputBlur(obj) { }
window.onload = function () {
    $("#InputPageNumber").keypress(function (event) {
        var keyCode = event.which;
        if (keyCode == 46 || (keyCode >= 48 && keyCode <= 57) || keyCode == 8) { return true; }
        else { return false; }
    }).focus(function () { this.style.imeMode = "disabled"; });
}