var widthOpI = $('.op_wrap i').width();
$('.op_wrap i').css({ 'line-height': widthOpI + 'px', 'height': widthOpI + 'px' });
$('.op_wrap dd').css({ 'line-height': widthOpI + 'px' });

$(window).resize(function () {
    var widthOpI = $('.op_wrap i').width();
    $('.op_wrap i').css({ 'line-height': widthOpI + 'px', 'height': widthOpI + 'px' });
    $('.op_wrap dd').css({ 'line-height': widthOpI + 'px' });
})

var lineLi = 0;
$('.line_box li').each(function (index, demEle) {
    if (index == lineLi * 3 + 2) {
        $(this).css({ 'margin-right': 0 });
        lineLi++;
    }
})
//公共选项部分li间距自适应

//var widthBsbDt = $('.bsb_content dt').width();
//widthBsbDt *= 130 / 208;
//$('.bsb_content dt').height(widthBsbDt + 'px');
//$('.bsb_content dd').height(widthBsbDt + 'px');

//$(window).resize(function () {
//    var widthBsbDt = $('.bsb_content dt').width();
//    widthBsbDt *= 130 / 208;
//    $('.bsb_content dt').height(widthBsbDt + 'px');
//    $('.bsb_content dd').height(widthBsbDt + 'px');
//})
//商学院动态页面dt、dd高度自适应

var hoOpConLi = 0;
$('.ho_opContent li').each(function (index, demEle) {
    if (index == hoOpConLi * 2 + 1) {
        $(this).css({ 'margin-right': 0 });
        hoOpConLi++;
    }
})
//首页option部分li间距自适应


var dnChooseLi = 0;
$('.dn_conhoose li').each(function (index, domEle) {
    if (index == dnChooseLi * 3 + 2) {
        $(this).css({ 'margin-right': 0 });
        dnChooseLi++;
    }
})
//设计云库页面li间距自适应


//$('#vCodeGet').click(function(){
//	var wait = 10;
//	function time(o) {
//		if (wait == 0) {
//			o.removeAttribute("disabled");
//			o.value="获取验证码";
//			wait = 60;
//		}
//		else {
//			o.setAttribute("disabled", true);
//			o.value="重新发送(" + wait + ")";
//			wait--;
//			setTimeout(function() {
//				time(o);
//			}, 1000)
//		}
//	}
//	time(vCodeGet);
//})
//注册页面获取验证码js


//var widthRlDt = $('.rl_box dt').width();
//$('.rl_box dt').height(widthRlDt + 'px');
//$('.rl_box dd').height(widthRlDt + 'px');
//$('.rl_num').css({'line-height':widthRlDt + 'px'});
//$('.rl_lNumber').css({'line-height':widthRlDt + 'px'});
//$('.rl_lIn_name').css({'line-height':widthRlDt + 'px'});
////排行榜页面样式自适应


$('.pd_oB_radio input').click(function () {
    var indexPdInput = $(this).parent().index();

    $('.pd_oB_th>div').eq(indexPdInput).siblings().css({ 'display': 'none' });
    $('.pd_oB_th>div').eq(indexPdInput).css({ 'display': 'block' });
})
//个人资料页面单选框点击

$('.pd_oB_th select').change(function () {
    var htmlPdSelect = $(this).find('option:selected').text();
    $(this).prev().text(htmlPdSelect);
})
//个人资料页面select框效果

$('.pd_department').click(function () {

    //if (department_id == 0)//未设置，弹出选项
    //{
    $('.pd_otherWrap').css({ 'display': 'block' });
    //}
    //else {
    //    alert("暂不支持修改部门");
    //}
})

$('.pd_oClose').click(function () {
    $('.pd_otherWrap').css({ 'display': 'none' });
})
//个人资料页面所属部门区域部分弹出隐藏效果

$('.pd_oFinish').click(function () {
    //if ($('.pd_personnel').attr('checked') == 'checked') {
    //    var textDepartment = $('#select_department').html().replace("|----", "");

    //    if (textDepartment != '请选择部门') {
    //        $('.pd_department').text(textDepartment);
    //        $('.pd_otherWrap').css({ 'display': 'none' });
    //    }
    //    else {
    //        alert('请选择您所在的部门！');
    //    }
    //}
    //else if ($('.pd_distributor').attr('checked') == 'checked') {
        var textProvince = $('#select_province').html();
        var textCity = $('#select_city').html();
        var textArea = $('#select_area').html();

        if (textProvince != '请选择省份') {
            if (textCity != '请选择地级市') {
                if (textArea != '请选择区县') {
                    $('.pd_department').text(textProvince + ' ' + textCity + ' ' + textArea);
                    $('.pd_otherWrap').css({ 'display': 'none' });
                }
                else {
                    alert('请选择您所在的区县！');
                }
            }
            else {
                alert('请选择您所在的城市！');
            }
        }
        else {
            alert('请选择您所在的省份！');
        }
    //}
})
//个人资料页面省市区联动传值判断


$(function () {
    if ($("#ho_banner").size() > 0) {
        TouchSlide({ slideCell: "#ho_banner", titCell: ".hd ul", effect: "leftLoop", autoPlay: true, autoPage: true });
    }

});

window.alert = function (msg,callback) {
    //layer.alert(msg, { icon: 0 }, function (index) {
    //    layer.close(index);
    //    if (msg.indexOf("请先登录") > -1)
    //    {
    //        location.href = "/Mobile/Login?returnurl=" + encodeURI(location.href);
    //    }
    //});
    //信息框
    layer.open({
        content: msg
        , btn: '我知道了'
        , yes: function (index) {
            //location.reload();
            layer.close(index);
            if (msg.indexOf("请先登录") > -1) {
                location.href = "/Mobile/Login?returnurl=" + encodeURI(location.href);
            }
            if (callback)
            {
                callback();
            }
        }
    });


}
window.tips = function (msg) {
    //layer.msg(msg, { icon: 0, time: 1500 });
    //提示
    layer.open({
        content: msg
      , skin: 'msg'
      , time: 2 //2秒后自动关闭
    });
}