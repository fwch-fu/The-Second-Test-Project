function Replying(ForumSecondID) {
    if (!success) {//验证是否登录
        if (confirm('回复前请先登录,点击确定将跳转登录页面')) {
            //跳转到登录页
            window.location.href = "/Account/Login/" + AbsolutePath;
        }
        return false;//返回false
    }
    //取出当前楼层定义的用于呈现“发表回复”功能的div容器
    var ReplayTextAreaBox_X = $("#ReplayTextAreaBox_" + ForumSecondID);
    //使用字符串拼接“发表回复”的各个控件
    var StartContent = "<div class=\"ReplyTextAreaContent\">";
    StartContent += "<form action=\"/HomeSave/ReplyContent\" method=\"post\">";
    StartContent += "<textarea class=\"ReplyTextArea\"  id=\"ReplyTextArea\" ";
    StartContent += "name=\"ReplyTextArea\">回复内容</textarea>";
    StartContent += "<input type=\"submit\" value=\"发表\" ";
    StartContent += "onclick=\"return RplyOn('ReplyTextArea','MaxContent')\"/>";
    StartContent += "<a href=\"javascript:void(0)\" class=\"CloseReply\" onclick";
    StartContent += "=\"CloseReply('ReplayTextAreaBox_" + ForumSecondID + "')\">收起发表</a>";
    StartContent += "<input type=\"hidden\" name=\"content\" id=\"MaxContent\" value=\"\" />";
    StartContent += "<input type=\"hidden\" name=\"curid\" ";
    StartContent += "value=\"" + $("#curid").val() + "\" />";
    StartContent += "<input type=\"hidden\" name=\"ReplySequenceID\" ";
    StartContent += "value=\"" + ForumSecondID + "\"/>";
    StartContent += "</form></div>";
    //将html编码字符串追加到容器中
    ReplayTextAreaBox_X.append($(StartContent));
}
function RplyOn(TextAreaID, ContentID) {
    var GetReplyTextArea = $("#" + TextAreaID);
    var content = GetReplyTextArea.val();
    if (content === '') {
        alert("请输入内容");
        return false;
    }
    else {
        $("#" + ContentID).val($.base64.btoa(content, true));
        return true;
    }
}
function ReplyMining(ForumSecondID, ShowBlockID) {
    if (!success && confirm('回复前请先登录,确定跳转登录页面')) {
        window.location.href = "/Account/Login/" + AbsolutePath;
        return false;
    }
    var ReplayTextAreaBox_X = $("#ReplyMining_" + ShowBlockID);
    var StartContent = "<form action=\"/HomeSave/ReplyContent\" method=\"post\">";
    StartContent += "<textarea class=\"ReplyMiningTextArea\" id=\"ReplyMiningTextArea\"></textarea>";
    StartContent += "<input type=\"submit\" value=\"回复\" onclick=\"return RplyOn('ReplyMiningTextArea','MinContent')\" />";
    StartContent += "<a href=\"javascript:void(0)\" onclick=\"CloseReply('ReplyMining_" + ShowBlockID + "')\" class=\"CloseReply\">收起回复</a>";
    StartContent += "<input type=\"hidden\" name=\"content\" id=\"MinContent\" value=\"\" />";
    StartContent += "<input type=\"hidden\" name=\"curid\" value=\"" + $("#curid").val() + "\" />";
    StartContent += "<input type=\"hidden\" name=\"ReplySequenceID\" value=\"" + ForumSecondID + "\"/>";
    StartContent += "</form>";
    ReplayTextAreaBox_X.append($(StartContent));
}
function CloseReply(CloseDivID) {
    $("#" + CloseDivID).empty();
}
function WarningInfo(ForumSecondID) {

}

$(document).ready(function () {

});

function OnSubWarningInfo() {
    var AjaxSubmitOpera = {
        type: "post",
        dataType: "json",
        data: {},
        url: "/HomeSave/SubWarningInfo",
        success: function (ResultJson) {
            alert(ResultJson.Content);
            switch (ResultJson.Status) {
                case 1:
                    document.getElementById("close9").click();
                    break;
                case 0:
                    alert(ResultJson.Content);
                    break;
                case -1:
                    alert(ResultJson.Content);
                    break;
            }
        }
    }
    $("#SubWarningInfo").ajaxSubmit(AjaxSubmitOpera);
}
function SetForumID(objID, ForumID, ForumTypeID) {
    if (!success && confirm('回复前请先登录,确定跳转登录页面')) {
        window.location.href = "/Account/Login/" + AbsolutePath;
        return false;
    }
    if ($("#" + objID).attr("IsClick") == "false") {
        var ShowBox = new PopupLayer({
            trigger: "#" + objID, popupBlk: "#blk9", closeBtn: "#close9", useOverlay: true, useFx: true,
            offsets: {
                x: 0,
                y: -41
            }
        });
        ShowBox.doEffects = function (way) {
            if (way == "open") {
                this.popupLayer.css({ opacity: 0.3 }).show(400, function () {
                    this.popupLayer.animate({
                        left: ($(document).width() - this.popupLayer.width()) / 2,
                        top: (document.documentElement.clientHeight - this.popupLayer.height()) / 2 + $(document).scrollTop(),
                        opacity: 0.8
                    }, 1000, function () { this.popupLayer.css("opacity", 1) }.binding(this));
                }.binding(this));
            }
            else {
                this.popupLayer.animate({
                    left: this.trigger.offset().left,
                    top: this.trigger.offset().top,
                    opacity: 0.1
                }, { duration: 500, complete: function () { this.popupLayer.css("opacity", 1); this.popupLayer.hide() }.binding(this) });
            }
        }
        $("#" + objID).attr("IsClick", "true");
        document.getElementById(objID).click();
    }
    else {
        $("#ForumID").val(ForumID);
        $("#ForumTypeID").val(ForumTypeID);
    }
}
function Onfloortext() {
    var floortext = $("#floortext").val();
    window.location.href = "#tbl_" + floortext;
}