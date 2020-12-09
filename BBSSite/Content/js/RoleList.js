$(function () {
    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    //var oButtonInit = new ButtonInit();
    //oButtonInit.Init();
});
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#TabList').bootstrapTable({
            url: '/admins/Users/Get_RoleList_Data',//请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            height: 480,
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: true,
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            clickToSelect: true,                //是否启用点击选中行
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [
            {
                field: 'ID',
                title: 'ID',
                width:"120px"
            },
            {
                field: 'RoleName',
                title: '角色名称',
                width: "300px"
            },
            {
                field: 'ColumnCount',
                title: '包含栏目',
                width: "120px"
            },
            {
                field: 'operate',
                title: '操作',
                align: 'center',
                events: operateEvents,
                formatter: operateFormatter
            }]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            search: $(".search>input").val()
        };
        return temp;
    };
    return oTableInit;
};
function operateFormatter(value, row, index) {
    return [
        '<a type="button" class="RoleOfB btn btn-default btn-sm" style="margin-right:15px;">编辑</a>',
        '<a type="button" class="RoleOfA btn btn-default btn-sm" style="margin-right:15px;">编辑权限</a>'        
    ].join('')
}
window.operateEvents = {
    'click .RoleOfA': function (e, value, row, index) {
        $.ajax({
            url: "/admins/Users/EditRoleJoinColumn",
            type: "post",
            dataType: "json",
            async: true,
            data: { "RoleID": row.ID },
            complete: function () {

            },
            success: function (data, testStatus) {
                if (data.ResultStatus != "1") {
                    alert(data.ResultMsg);
                }
                else {
                    var List = JSON.parse(data.ResultMsg);
                    var itemListRows = "";
                    $.each(List, function (index, jsonEntity) {
                        var IsChecked = jsonEntity.JoinID > 0 ? "checked='true'" : "";
                        var span1 = "<span class=\"item-list-radio\"><input type=\"checkbox\" " + IsChecked + " value=\"" + jsonEntity.ID + "\" onclick=\"OnSaveRoleJoinColumn(this," + jsonEntity.JoinID + ")\"/></span>";
                        var span2 = "<span class=\"item-list-text\">" + jsonEntity.ColumnName + "</span>";
                        itemListRows += "<div>" + span1 + span2 + "</div>";
                    })
                    $("#item-list").html(itemListRows);
                    $("#SettingBanzhu").prop("RoleID", row.ID);
                    $("#SettingBanzhu").modal('show');
                }
            }
        });
    },
    'click .RoleOfB': function (e, value, row, index) {
        window.location.href = "/Admins/Users/EditRole/" + row.ID;
    }
}

function OnSaveRoleJoinColumn(obj, JoinID) {
    var ColumnID = obj.value;
    var Isadd = obj.checked;
    var RoleID = $("#SettingBanzhu").prop("RoleID");
    $.ajax({
        url: "/admins/Users/OnSaveRoleJoinColumn",
        type: "post",
        dataType: "json",
        async: true,
        data: { "RoleID": RoleID, "ColumnID": ColumnID, "JoinID": JoinID, "Isadd": Isadd },
        complete: function () {

        },
        success: function (data, testStatus) {
            if (data.ResultStatus != "1") {
                alert(data.ResultMsg);
            }
        }
    });
}

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};
    oInit.Init = function () {
        //初始化页面上面的按钮事件

    };
    return oInit;
};