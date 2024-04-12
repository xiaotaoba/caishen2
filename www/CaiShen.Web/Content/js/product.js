var $brandbox = $("#brandbox");
var $brandlist = $("#brandlist");
var $txtbrand = $("#txtBrand");
var $btnSetBrand = $("#btnSetBrand");
var $btnShowBrand = $("#btnShowBrand");
var myDialog;

$(function () {

    $btnShowBrand.click(function () {
        myDialog = $.dialog({
            title: "服务商",
            lock: true,
            content: $brandbox.get(0),
            init: function () {
                $btnSetBrand.click(function () {
                    //alert("即将关闭！");
                    var selectBrand = getBrandSelected();
                    //alert("selectBrand:" + selectBrand);

                    $txtbrand.val(selectBrand);
                    myDialog.close();
                });
            }
        });
    });


    initBrand();


});

function initBrand() {
    var brandArr = $txtbrand.val().split(',');

    $brandlist.find("li").each(function () {
        var $brandli = $(this);
        var bid = $brandli.data("id");
        //alert("index:" + $.inArray(bid+"", brandArr))
        if ($.inArray(bid + "", brandArr) > -1) {
            $brandli.find(":checkbox").attr("checked", true);;
        }
    });
}
function getBrandSelected() {
    var brandids = "";
    $brandlist.find("li").each(function () {
        var $brandli = $(this);
        if ($brandli.find(":checkbox").is(":checked")) {
            brandids = brandids + $brandli.data("id") + ","
        }
    });
    return brandids;
}

