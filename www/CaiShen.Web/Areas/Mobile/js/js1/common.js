function dr_alert(msg) {
    //art.dialog({
    //    icon: "error",
    //    content: msg
    //})
    layer.alert(msg, {
        icon: 0
        //,skin: 'layer-ext-moon' 
    })
}
function dr_confirm_batch(title, formid, controlName) {
    layer.confirm(title, {
        btn: ['确定','取消'] //按钮
    }, function(){
        
        if (controlName == null) {
            controlName = "ids";
        }
        if (formid == null) {
            formid = "#myform";
        }
        else {
            formid = "#" + formid;
        }
        var _data = $(formid).serialize();
        var _url = window.location.href;
        if ((_data.split(controlName)).length - 1 <= 0) {
            //$.dialog.tips("请选中操作项", 2);
            layer.msg('请选中操作项', { icon: 0 });
            return false;
        }
        $(formid).submit();

    }, function () {
        //取消
        //return false;
    });
    return false
}

window.alert = function (msg) {
    layer.alert(msg, { icon: 0 }, function (index) {
        layer.close(index);
        if (msg.indexOf("请先登录") > -1)
        {
            location.href = "/Mobile/Login?returnurl=" + encodeURI(location.href);
        }
    });
}
window.tips = function (msg) {
    layer.msg(msg, { icon: 0,time:1500 });
}

// /*注册事件*/
// if (document.addEventListener) {
// document.addEventListener('DOMMouseScroll', scrollFunc, false);
// }//W3C    
// window.onmousewheel = document.onmousewheel = scrollFunc;//IE/Opera/Chrome 

var $txtKeywords = $("#keyword");
var $btnSo = $(".search .btn_search");
var soTips = "搜索";
//var isOverNav = false;

$(function () {

    ////返回顶部
    //$(".toTop a").click(function () {
    //    $('html,body').animate({ scrollTop: '0px' }, "fast");
    //});

    ////头部分类
    //$("body").not(".home").find("#head_category").hover(function () {
    //    $("#head_category>dd").show();
    //}, function () {
    //    $("#head_category>dd").hide();
    //});


    //切换详情样式
    $(".pro_tabs li").each(function (index) {
        $(this).click(function () {

            $(this).addClass("active");
            $(this).siblings().removeClass("active");
            $(".pro_content>.pro_content_tab").eq(index).show();
            $(".pro_content>.pro_content_tab").eq(index).siblings().hide();
        });
    });

    $(".content>.title>.icon-caidan").click(function () {
        $(".content .top_menu").show();
    });
    $(".content .top_menu>.icon-jiantoul").click(function () {
        $(".content .top_menu").hide();
    });

    if ($("#thumblist").size() > 0)
    { 
        TouchSlide({ slideCell: "#thumblist", titCell: ".hd ul", effect: "leftLoop", autoPlay: true, autoPage: true });
    }
    //$("#thumblist li").each(function (index) {
    //    $(this).click(function (e) {
    //        e.preventDefault();
    //        $(this).addClass("on");
    //        $(this).siblings().removeClass("on");
    //        $("#bigPhoto").attr("src", $(this).find("img").attr("src"));
    //        //$("#zoom1").attr("href", $(this).find("img").attr("src"));
    //        //$(".pro_detail_l .img .jqzoom").eq(index).show().siblings().hide();
    //    });
    //});

    //头部搜索
    $txtKeywords.keypress(function () {
        if (event.keyCode == 13) {
            $btnSo.trigger("click");
            return false;
        }
    });
    $txtKeywords.keydown(function (e) {
        if (e.which == 13) {
            $btnSo.trigger("click");
            return false;
        }
    });

    $btnSo.click(function (e) {
        var key = $txtKeywords.val();
        if (key == soTips || key == "") {
            alert("请输入搜索关键词！");
            e.preventDefault();
            return false;
        }
        location.href = "/Mobile/Search?keywords=" + encodeURI(key);
    });
    //$(".newsCase .lightbox").lightBox();

    //确认框点击事件
    $("[data-confirm]").each(function () {
        var curHref = $(this);
        curHref.click(function (e) {
            if (curHref.data("confirm") == "delete") {
                e.preventDefault();
                //Modal.confirm({ msg: "确定删除？" }).on(function (flag) {
                //    if (flag) {
                //        window.location.href = curHref.attr("href");
                //    }
                //    else {
                //        return false;
                //    }
                //});
                layer.confirm("确定删除？", {
                    btn: ['确定', '取消'] //按钮
                }, function () {
                    window.location.href = curHref.attr("href");
                }, function () {
                    //layer.close();
                });
            }
           else if (curHref.data("confirm") == "down") {
                e.preventDefault();
                layer.confirm("确定要下架吗？", {
                    btn: ['确定', '取消'] //按钮
                }, function () {
                    window.location.href = curHref.attr("href");
                }, function () {
                    //layer.close();
                });
           }
           else if (curHref.data("confirm") == "cancel") {
               e.preventDefault();
               layer.confirm("确定要取消吗？", {
                   btn: ['确定', '取消'] //按钮
               }, function () {
                   window.location.href = curHref.attr("href");
               }, function () {
                   //layer.close();
               });
           }
        });
    });

});


//元素是否完全显示
function isShow(obj) {
    if ($(obj).offset().top - $(document).scrollTop() < $(window).height() - $(obj).height()) {
        return true;
    }
    return false;
}
jQuery.fn.isChildAndSelfOf = function (b) {
    return (this.closest(b).length > 0);
};
jQuery.fn.isChildOf = function (b) {
    return (this.parents(b).length > 0);
};

//保留两位小数  
//功能：不会四舍五入 取小数点后2位 
function toDecimal(x) {
    var f = parseFloat(x);
    if (isNaN(f)) {
        return;
    }
    f = Math.floor(x * 100) / 100;
    return f;
}

function isRealNum(val) {
    // isNaN()函数 把空串 空格 以及NUll 按照0来处理 所以先去除
    if (val === "" || val == null) {
        return false;
    }
    if (!isNaN(val)) {
        return true;
    } else {
        return false;
    }
}