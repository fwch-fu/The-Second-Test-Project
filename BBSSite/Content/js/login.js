window.onload = function () {
    $("#age").keypress(function (event) {
        var keyCode = event.which;
        if (keyCode == 46 || (keyCode >= 48 && keyCode <= 57) || keyCode == 8) { return true; }
        else { return false; }
    }).focus(function () { this.style.imeMode = "disabled"; });
}
function subRegister() {
    var username = $("#UserName").val();
    var password = $("#UserPassword").val();
    var repassword = $("#repassword").val();
    var wxname = $("#NickName").val();
    var sex = $("#SexID").val();
    var age = $("#Age").val();
    var email = $("#Email").val();
    var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;

    if (username == "" || username.length < 5) {
        alert("用户名不得为空且不能少于五位英文加数字");
    } else if (password == null || password.length < 5) {
        alert("密码不得为空且不能少于五位英文加数字");
    } else if (password != repassword) {
        alert("两次密码不一致");
    } else if (wxname == "" || wxname.length > 20) {
        alert("姓名不得为空且不能大于十位中文或英文");
    } else if (sex == "") {
        alert("性别格式错误")
    } else if (age == "") {
        alert("年龄输入错误")
    } else if (email == "" || !reg.test(email)) {
        alert("邮箱格式不正确")
    } else {
        return true;
    }
    return false;
}