function AllChoose(obj, ColumnCode) {
    $("input[ColumnCode='" + ColumnCode + "']").each(function () {
        this.checked = obj.checked;
    });
}
function Choose(obj) {
    var ColumnCode = obj.attributes["ColumnCode"].value;
    $("input[OneColumnCode='" + ColumnCode + "']").each(function () {
        if (obj.checked) {
            this.checked = true;
        } else {
            var IsTrue = false;
            $("input[ColumnCode='" + ColumnCode + "']").each(function () {
                if (!IsTrue) {
                    if (this.checked) {
                        IsTrue = true;
                    }
                }
            });
            if (!IsTrue) {
                this.checked = false;
            }
        }
    });
}