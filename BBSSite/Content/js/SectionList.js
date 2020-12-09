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
            url: '/admins/Section/Get_SectionList_Data',//请求后台的URL（*）
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
                width: "120px"
            },
            {
                field: 'AreaName',
                title: '板块名称',
                width: "300px"
            },
            {
                field: 'UserName',
                title: '所属版主',
                width: "300px"
            },
            {
                field: 'operate',
                title: '操作',
                align: 'center',
                width: "300px",
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
        '<button type="button" class="RoleOfC btn btn-default btn-sm" style="margin-right:15px;">编辑</button>',
        '<button type="button" class="RoleOfA btn btn-default btn-sm" style="margin-right:15px;">查看子专区</button>',
        '<button type="button" class="RoleOfB btn btn-default btn-sm" style="margin-right:15px;">重设版主</button>'
    ].join('')
}
window.operateEvents = {
    'click .RoleOfA': function (e, value, row, index) {
        location.href = "/Admins/Section/SectionChildList/" + row.ID;
    },
    'click .RoleOfB': function (e, value, row, index) {
        $("#SettingBanzhu").prop("ForumAreaID", row.ID);
        $("#SettingBanzhu").modal('show');
    },
    'click .RoleOfC': function (e, value, row, index) {
        window.location.href = "/Admins/Section/EditSection/" + row.ID;
    }
}

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};
    oInit.Init = function () {
        //初始化页面上面的按钮事件

    };
    return oInit;
};

function SettingModerator(UserID) {
    var ForumAreaID = $("#SettingBanzhu").prop("ForumAreaID");
    $.ajax({
        url: "/admins/Section/SettingModerator",
        type: "post",
        dataType: "json",
        async: true,
        data: { "UserID": UserID, "ForumAreaID": ForumAreaID },
        complete: function () {

        },
        success: function (data, testStatus) {
            if (data.ResultStatus != "1") {
                alert(data.ResultMsg);
            }
            else {
                window.location.href = "/admins/Section/SectionList";
            }
        }
    });
}