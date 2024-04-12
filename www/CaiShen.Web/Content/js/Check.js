
/*
判断后台是否选择了操作项
*/
function checkData() {
    var idlist = "";
    var Item = document.getElementsByName("checkboxlist");
    for (var i = 0; i < Item.length; i++) {
        if (Item[i].type == "checkbox" && Item[i].checked == true) {
            idlist += Item[i].value + ",";
        }
    }
    if (idlist == "") {
        alert("请至少选择一条要操作的数据！");
        return false;
    }
    else {
        if (confirm("删除将无法恢复，您确定要删除所选数据吗？")) {
            return true;
        }
    }
    return false;
}

/*
判断后台是否选择了操作项
*/
function checkSelected() {
    var idlist = "";
    var Item = document.getElementsByName("checkboxlist");
    for (var i = 0; i < Item.length; i++) {
        if (Item[i].type == "checkbox" && Item[i].checked == true) {
            idlist += Item[i].value + ",";
        }
    }
    if (idlist == "") {
        alert("请至少选择一条要操作的数据！");
        return false;
    }
    return true;
}

/*
设置:操作项全选或全部不选
*/
function checkAll() {
    var Item = document.getElementById("CheckboxAll");
    var ch = document.getElementsByTagName("input");
    for (var i = 0; i < ch.length; i++) {
        if (ch[i].type == "checkbox") {
            ch[i].checked = Item.checked;
        }
    }
}