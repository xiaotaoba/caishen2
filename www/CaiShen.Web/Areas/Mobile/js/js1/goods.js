/*  产品属性相关 */
var $propertylist = $("#propertylist");
var $propertylistMask = $(".propertylist_mask");
//属性SKU价格
var $price = $("#prop_price");
//总价
var $totalprice = $("#total_price");
var _shopprice = 0;
var _costprice = 0;
var _priceModel = {};
var $totalpriceTxt = $(".total_price");
//平均单价
var $unitprice = $("#unit_price");
//单独计价属性总价
var _addprice = 0;
//其他附加费
var _otheraddprice = 0;
//设计费用
var _designFee = 0;
var _existGoodsSKUJson;//产品SKU
var _selectPropertyValue = [];
var _selectPropertyValueText = [];
var _selectSkuItem = {};
//购买数量针对SKU
var _skucount = 1;
//单品数量
var _goodscount = 1;
//input数值乘积-非数量以及单独计价
var _inputcount = 1;

//获取产品SKU json
function GetGoodsSKUData() {

    $.ajax({
        type: 'POST',
        url: '/Mobile/Goods/GetSKUJson',
        data: { "goodsid": goodsId },
        dataType: "json",
        async: false,
        cache: false,
        success: function (data) {
            _existGoodsSKUJson = data;
            //console.log("_existGoodsSKUJson:" + JSON.stringify(_existGoodsSKUJson));
        }
    });
}

//组合已选属性值 + 计算附加价格 + _inputcount
//参数：$click_pv_item 是当前点击属性值，可以为空
function InitSelectPropertyValue($click_pv_item) {

    CheckSelectAllAndActiveUseful($click_pv_item);

    _addprice = 0;
    _inputcount = 1;
    _selectPropertyValue = [];
    _selectPropertyValueText = [];
    $propertylist.find(".property .active").each(function (index) {
        var $pv_item = $(this);
        _selectPropertyValue[index] = { nameid: $pv_item.data("nid"), valueid: $pv_item.data("vid") };
        _selectPropertyValueText[index] = { name: $pv_item.data("pname"), value: "" };
        if ($pv_item.hasClass("select")) {
            //select展示
            _selectPropertyValueText[index].value = $pv_item.find("option:selected").text();
        }
        else if ($pv_item.data("showtype") == 1) {
            //input属性
            var $pv_item_input = $pv_item.find("input");
            _selectPropertyValueText[index].value = $.trim($pv_item_input.data("pvname")) + $pv_item_input.val() + $.trim($pv_item_input.data("pvunit"));
        }
        else {
            //text属性
            _selectPropertyValueText[index].value = $pv_item.text();
        }


        //如果是单独计价
        if ($pv_item.data("isprice") == 1) {

            //文本
            if ($pv_item.data("showtype") == 0) {
                _addprice += parseFloat($pv_item.data("price"));
            }
            else { //输入框
                _addprice += parseFloat($pv_item.data("price")) * parseInt($pv_item.find("input").val());
            }
        }
        else if ($pv_item.data("isnum") == 1) {
            //如果是数量属性，改变数量
            //文本
            if ($pv_item.data("showtype") == 0) {
                _goodscount = parseInt($pv_item.text());
                _skucount = 1;
            }
            else { //输入框
                _skucount = parseInt($pv_item.find("input").val());
                _goodscount = parseInt($pv_item.find("input").val()) * parseInt($pv_item.find("input").data("multiple"));
            }
        }
        else if ($pv_item.data("showtype") == 1) {
            //非数量属性 & 非计价属性，统计所有数字乘积
            _inputcount *= parseInt($pv_item.find("input").val());
        }

    });
    //显示价格
    ShowSKUPrice();
    $(".property_select").text("已选:“" + GetSelectPropertyValueText() + "”");
    $(".property_select_default>span").text("已选:“" + GetSelectPropertyValueText() + "”");
    //JoinSelectPropertyValueText();
}
///获取已选属性值Text
function GetSelectPropertyValueText() {
    var properties = "";
    if (_selectPropertyValueText.length > 0) {
        $.each(_selectPropertyValueText, function (index, data) {
            properties += data.value + ",";
        });
    }
    return properties.substr(0, properties.length - 1);
}
//检查是否存在选择SKU的组合,禁用不可用选项
//参数：$click_pv_item，当前点击的属性值，为空说明是刚进入页面
function CheckAllSKUIsEnable($click_pv_item) {
    var sku_item = [];
    //console.log("$click_pv_item:" + $click_pv_item);
    //点击属性值
    if ($click_pv_item != null && $click_pv_item != undefined) {
        var $p_item = $click_pv_item.parent().parent();//点击的属性
        //激活同属性下其他选项
        $click_pv_item.siblings("span").removeClass("disable");
        $click_pv_item.siblings("span").find("input").removeAttr("disabled");
        $click_pv_item.siblings("span").find("select").removeAttr("disabled");

        var properties1 = $click_pv_item.data("nid") + ":" + $click_pv_item.data("vid");
        var skuContainProperties1 = GetExistProperties(properties1);

        //循环组合其他单个属性的值（只组合2个属性的值，不和刚进入一样组合所有属性的值）
        $propertylist.find(".property").not($p_item).find("li>span").each(function (index) {
            var $pv_item = $(this);
            var properties2 = $pv_item.data("nid") + ":" + $pv_item.data("vid")

            //console.log("properties1：" + properties1 + ",properties2:" + properties2);
            //存在可用SKU组合
            if (IsExistEnableProperties(properties2, skuContainProperties1)) {
                $pv_item.removeClass("disable");
                $pv_item.find("input").removeAttr("disabled");
                $pv_item.find("select").removeAttr("disabled");
            }
            else {//禁用
                $pv_item.addClass("disable");
                $pv_item.find("input").attr("disabled", "disabled");
                $pv_item.find("select").attr("disabled", "disabled");

                //被禁用是已选项，转下一个属性值选中
                if ($pv_item.hasClass("active")) {
                    $pv_item.removeClass("active");
                    if ($pv_item.next("span").size() > 0) {
                        $pv_item.next("span").addClass("active");
                        $pv_item.next("span").removeClass("disable");
                        $pv_item.next("span").find("input").removeAttr("disabled");
                        $pv_item.next("span").find("select").removeAttr("disabled");
                    }
                    else {
                        $pv_item.siblings("span").eq(0).addClass("active");
                        $pv_item.siblings("span").eq(0).removeClass("disable");
                        $pv_item.siblings("span").eq(0).find("input").removeAttr("disabled");
                        $pv_item.siblings("span").eq(0).find("select").removeAttr("disabled");
                    }
                }
            }

        });
    }
        //循环属性值组合，禁用不可用属性值
    else {
        $propertylist.find("ul.property").each(function (p_index) {
            var $p_item = $(this);
            //循环属性选项（值）
            $p_item.find("span").each(function (index) {
                var $pv_item_current = $(this);
                var current_index = 0;
                var return_sku = {};
                //当前属性值与其他属性已选择值 组合看是否存在
                $propertylist.find(".property").not($p_item).find(".active").each(function (index) {
                    var $pv_item = $(this);
                    sku_item[index] = { nameid: $pv_item.data("nid"), valueid: $pv_item.data("vid") };
                    current_index = index;
                });
                current_index = current_index + 1;
                sku_item[current_index] = { nameid: $pv_item_current.data("nid"), valueid: $pv_item_current.data("vid") };

                return_sku = GetSKUItem(sku_item);
                if (return_sku == null || return_sku.Count == 0 || return_sku.Price == 0) {
                    $pv_item_current.addClass("disable");
                    $pv_item_current.find("input").attr("disabled", "disabled");
                    $pv_item_current.find("select").attr("disabled", "disabled");
                    //if ($pv_item_current.hasClass("active"))//当前选中不可用
                    //{
                    //    $pv_item_current.removeClass("active");
                    //    if ($pv_item_current.next().length == 0) {
                    //        $pv_item_current.siblings().first().addClass("active");
                    //    }
                    //    else { 
                    //        $pv_item_current.next().addClass("active");
                    //    }
                    //}
                }
                else {
                    $pv_item_current.removeClass("disable");
                    $pv_item_current.find("input").removeAttr("disabled");
                    $pv_item_current.find("select").removeAttr("disabled");
                }
            });
        });
    }
}

///查询sku
///test_sku_item：属性name:value组合
function GetSKUItem(test_sku_item) {
    var skuItemData = { Properties: "" };
    var return_sku_item = { Count: 0, Price: 0, Properties: "" };
    if (test_sku_item.length > 0) {
        $.each(test_sku_item, function (index, data) {
            if (data != null && data != undefined) {
                skuItemData.Properties += data.nameid + ":" + data.valueid + ";";
            }
        });
    }
    $.each(_existGoodsSKUJson, function (index, data) {

        //存在SKU
        if (EqualProperties(skuItemData.Properties, data.Properties)) {
            return_sku_item = data;
            //console.log(JSON.stringify(return_sku_item));
            return false;
        }
    });
    return return_sku_item;
}
///合并已选属性值
function JoinSelectPropertyValue() {
    var skuItem = { Properties: "" };
    if (_selectPropertyValue.length > 0) {
        $.each(_selectPropertyValue, function (index, data) {
            skuItem.Properties += data.nameid + ":" + data.valueid + ";";
        });
    }
    //console.log(JSON.stringify(skuItem));
    return skuItem;
}
///合并已选属性值Text
function JoinSelectPropertyValueText() {
    var skuItem = { Properties: "" };
    if (_selectPropertyValueText.length > 0) {
        $.each(_selectPropertyValueText, function (index, data) {
            if (data.name == "烫金") {
                skuItem.Properties += data.name + ":" + data.value + "(" + _tj_txt + ")" + "_";
            }
            else {
                skuItem.Properties += data.name + ":" + data.value + "_";
            }
        });
    }
    //console.log(JSON.stringify(skuItem));
    //location.hash = skuItem.Properties;
    return skuItem.Properties;
}

///获取当前选中属性SKU价格
function GetSKUPrice() {
    var price = 0;
    var selectedData = JoinSelectPropertyValue();
    $.each(_existGoodsSKUJson, function (index, data) {
        //console.log( "data:" + JSON.stringify(data));
        //console.log("selectedData:" + JSON.stringify(selectedData) + ",data:" + JSON.stringify(data));

        //if (data.Properties == selectedData.Properties) {
        //相等
        if (EqualProperties(selectedData.Properties, data.Properties)) {
            _selectSkuItem = data;
            price = data.Price;
            //console.log("_selectSkuItem:" + JSON.stringify(_selectSkuItem));
            return false;
        }
    });
    if (price == 0) {
        price = defautlPrice;
    }
    return price;
}

//判断属性值对是否相等
//selectedDataProperties：选择属性组合的直对,格式：nid:vid;nid:vid;
//dataProperties：存在的单个sku的属性组合;
function EqualProperties(selectedDataProperties, dataProperties) {
    //console.log("selectedDataProperties:" + selectedDataProperties + ",dataProperties:" + dataProperties);
    var selectPropertiesArr = "";
    var currentPropertiesArr = "";

    if (selectedDataProperties == dataProperties) {
        return true;
    }

    //选中
    if (selectedDataProperties != "" && selectedDataProperties.indexOf(";") > -1)//包含属性:值数据
    {
        selectPropertiesArr = selectedDataProperties.split(";");
    }
    //当前
    if (dataProperties != "" && dataProperties.indexOf(";") > -1)//包含属性:值数据
    {
        currentPropertiesArr = dataProperties.split(";");
    }
    //console.log("selectPropertiesArr.length:" + selectPropertiesArr.length + ",currentPropertiesArr.length:" + currentPropertiesArr.length);
    if (selectPropertiesArr.length != currentPropertiesArr.length) {
        return false;
    }
    //拆分比较所有属性直对是否相等，如果存在不相等返回false,否则返回true;
    for (var i = 0; i < selectPropertiesArr.length; i++) {
        if (selectPropertiesArr[i] == "") {
            continue;
        }
        //if(!$.inArray(selectPropertiesArr[0], currentPropertiesArr))
        //{
        //    return false;
        //}
        var isExist = false;
        for (var j = 0; j < currentPropertiesArr.length; j++) {
            //alert("selectPropertiesArr[" + i + "]" + selectPropertiesArr[i] + "," + " currentPropertiesArr["+j+"]" + currentPropertiesArr[j]);
            if (selectPropertiesArr[i] == currentPropertiesArr[j]) {
                isExist = true;
                break;
            }
        }
        if (isExist == false) {
            return false;
        }
    }
    return true;
}
//判断SKU集合propertiesArr中是否存在包含properties2的SKU项
//properties2,格式：nid:vid;
function IsExistEnableProperties(properties2, propertiesArr) {
    var currentPropertiesArr = [];
    var isExist = false;
    var returnValue = false;

    $.each(propertiesArr, function (index, data) {
        var dataProperties = data.Properties;
        if (dataProperties != "" && dataProperties.indexOf(";") > -1)//包含属性:值数据
        {
            currentPropertiesArr = dataProperties.split(";");
        }
        for (var j = 0; j < currentPropertiesArr.length; j++) {
            if (currentPropertiesArr[j] == properties2 && data.Count > 0 && data.Price > 0) {
                isExist = true;
                //console.log("isExist");
                break;
            }
        }
        if (isExist) {
            returnValue = true;
            //console.log("returnValue");
            return false;
        }
    });

    return returnValue;
}
//获取包含properties1的SKU项
//properties1：单个属性直对,格式：nid:vid;
function GetExistProperties(properties1) {
    var currentPropertiesArr = [];
    var containProperties1 = [];

    //包含properties1的SKU
    $.each(_existGoodsSKUJson, function (index, data) {
        var dataProperties = data.Properties;
        if (dataProperties != "" && dataProperties.indexOf(";") > -1)//包含属性:值数据
        {
            currentPropertiesArr = dataProperties.split(";");
        }
        for (var j = 0; j < currentPropertiesArr.length; j++) {
            if (currentPropertiesArr[j] == properties1) {
                containProperties1.push(data);
                //console.log("push1");
                break;
            }
        }
    });
    return containProperties1;
}

//判断是不是选择所有属性，全部选择返回true，否则返回false
function CheckIsSelectAllItem() {
    var flag = true;
    //遍历属性
    $propertylist.children(".property").each(function () {
        var $item = $(this);
        if (!$item.find("span").is(".active")) {
            flag = false;
            return false;
        }
    });
    return flag;
}
//获取一个可用SKU（数量不为0）
//参数：$click_pv_item 需要包含当前点击属性值，可为空
function GetUserfulSKU($click_pv_item) {
    var skuModel = {};
    //查找一个可用的SKU
    $.each(_existGoodsSKUJson, function (index, data) {
        //console.log("data.Price:" + data.Price + ",data.Count:" + data.Count);
        //console.log($propertylist.find("ul.property").size() +" -- "+ data.Properties.split(';').length);
        if (data.Count > 0 && data.Properties != "" && $propertylist.find("ul.property").size() + 1 == data.Properties.split(';').length) {
            if ($click_pv_item != null && $click_pv_item != undefined)//包含当前点击对象
            {
                if (data.Properties.indexOf($click_pv_item.data("vid")) > -1) {
                    skuModel = data;
                    return false;
                }
            }
            else {
                skuModel = data;
                return false;
            }
        }
    });
    //console.log(skuModel)
    return skuModel;
}

//点击属性时，是否是选择所有项，如果是，使用一个可用SKU，做为默认选择项
function CheckSelectAllAndActiveUseful($click_pv_item) {
    if (!CheckIsSelectAllItem())//未选择所有，初始可用值选中
    {
        var skuModel = GetUserfulSKU($click_pv_item);
        if (skuModel != null && skuModel.Properties != undefined) {
            var propertiesArr = skuModel.Properties.split(";");
            for (var i = 0; i < propertiesArr.length; i++) {
                var $pv_item = $("[data-vid='" + propertiesArr[i].split(':')[1] + "']");
                $pv_item.addClass("active");
                if (typeof _size_nid != "undefined" && $pv_item.data("nid") == _size_nid && $pv_item.data("showtype") == 1) {
                    $pv_item.siblings(".text").removeClass("active");
                    $pv_item.siblings(".input").addClass("active");
                }
                else {
                    $pv_item.siblings().removeClass("active");
                }
            }
        }
    }
}

//公用绑定事件
function BindEventBase() {

    //改变select属性值
    $propertylist.delegate("li.value select", "change", function () {
        var $item = $(this);
        $item.parent().data("vid", $(this).val());

        //如果是数量属性，改变数量
        if ($item.parent().data("isnum") == 1) {
            _skucount = 1;// $item.val();
            _goodscount = parseInt($item.val());
        }

        InitSelectPropertyValue();

    });

    //改变input属性值
    $propertylist.delegate("li.value [type='number']", "change", function () {
        var $item = $(this);
        if ($item.val() == "") {
            $item.val("1");
        }
        else if (Number($item.val()) < Number($item.attr("min"))) {
            alert("值必须大于或等于" + $item.attr("min"));
            $item.val($item.attr("min"));
        }
        else if (Number($item.val()) > Number($item.attr("max"))) {
            alert("值必须小于或等于" + $item.attr("max"));
            $item.val($item.attr("max"));
        }

        //如果是数量属性，改变数量
        if ($item.parent().data("isnum") == 1) {
            _skucount = parseInt($item.val());
            _goodscount = parseInt($item.val()) * parseInt($item.data("multiple"));
        }
        //_skucount = parseInt($item.val());
        InitSelectPropertyValue();

    });

    //点击设计稿值
    $propertylist.delegate(".designfile li.value>span", "click", function () {
        var $val_item = $(this);//操作属性值项
        _designFee = parseFloat($val_item.data("fee"));
        if ($val_item.hasClass("active")) {
        }
        else {
            $val_item.addClass("active");
            //同属性下值取消选中
            $val_item.siblings().removeClass("active");
        }
        if ($val_item.data("value") == 1) {
            $val_item.siblings("b").text("选择“有设计稿”付款后请上传设计源文件");
        }
        else {
            $val_item.siblings("b").html('<a href="/Mobile/Category?design=1" target="_blank">如需设计,请点击</a>');
        }

        ShowSKUPrice();
    });

    //显示购买选择属性
    $(".show_btn_buy").click(function () {
        addToCartGoOrder();
    });
    //显示加入购物车选择属性
    $(".show_btn_cart").click(function () {
        showPropertylistMask();
        //addToCart(function (cartid) { flyToCart(event); });
    });
    //隐藏选择属性
    $propertylistMask.click(function () {
        hidePropertylistMask();
    });

    //立即购买
    $(".btn_buy").click(function () {
        //location.href = "@Url.Action("Index", "Cart")";
        addToCartGoOrder();
        //hidePropertylistMask();
    });
    //加入购物车
    $(".btn_cart").click(function (event) {
        //location.href = "@Url.Action("Index", "Cart")";
        addToCart(function (cartid) { flyToCart(event); });
        //hidePropertylistMask();

    });

    //完成选择
    $(".btn_done").click(function (event) {
        hidePropertylistMask();
    });
    //选择产品属性
    $(".property_select_default").click(function () {
        showPropertylistMask();
    });
    //关闭产品属性选择层
    $(".propertylist_close").click(function () {
        hidePropertylistMask();
    });
}

function hidePropertylistMask() {
    $propertylist.hide();
    $propertylistMask.hide();
    $propertylist.removeClass();
}
function showPropertylistMask() {
    $propertylist.show();
    $propertylistMask.show();
    $propertylist.removeClass().addClass("isshow_cart");
}

//获取购物车信息
function change_goods_number_response() {

    $.ajax({
        url: "/Mobile/Cart/GetCartTJ",
        type: "GET",
        async: true,
        cache: false,
        data: { "GoodsID": goodsId, "ShopID": _shopId, "SKUID": _selectSkuItem.ID, "Properties": "", Count: _skucount },
        success: function (result) {
            var json_rs = result;// eval("(" + result + ")");
            if (json_rs == null || json_rs.status == null) {//返回错误
                //showAlert("warning", "系统错误！", null);
                //alert("系统错误！");
            }
            else if (json_rs.status == "success") {
                //$("#head_cart em").text(json_rs.totalCount)//更新数量
                $(".icon-gouwuche em").text(json_rs.totalCount)//更新数量
            }
            else {
                alert(json_rs.msg);
            }

        },
        error: function (xmlhttp) {
            // alert("系统错误！<br/>" + xmlhttp.responseText);
        }
    });
}

function GetPostData() {
    var data = {
        "GoodsID": goodsId,
        "ShopID": _shopId,
        "SKUID": _selectSkuItem.ID,
        "Properties": JoinSelectPropertyValue().Properties,
        "PropertiesName": JoinSelectPropertyValueText(), 
        "Count": _skucount,
        "CartTotalPrice": $totalprice.text(),
        "GoodsCount": _goodscount,
        "IsHasDesignFile": $(".designfile").find(".active").data("value"),
        "DesignFee": _designFee, 
        "HiddenShippingFee": _otheraddprice,
        "TotalShopPrice": _shopprice,
        "TotalCostPrice": _costprice
    }
    if (typeof _priceModel.weight != "undefined") {
        data.Weight = _priceModel.weight
    }
    if (typeof _priceModel.volume != "undefined") {
        data.Volume = _priceModel.volume
    }
    if (typeof _priceModel.unitarea != "undefined") {
        data.UnitArea = _priceModel.unitarea
    }
    return data;
}
//加入购物车+跳转至下单
function addToCartGoOrder(pid) {
    if (!CheckIsSelectAllItem()) {
        showPropertylistMask();
        tips("请选择属性");
        return;
    }
    var price = parseInt($totalprice.text());
    if (price <= 0) {
        tips("操作失败！");
        return;
    }
    $.ajax({
        url: "/Mobile/Cart/BuyNowAdd",
        type: "POST",
        async: true,
        cache: false,
        data: GetPostData(),
        success: function (result) {
            var json_rs = result;//eval("(" + result + ")");
            if (json_rs == null || json_rs.status == null) {//返回错误
                //showAlert("warning", "系统错误！", null);
                // alert("系统错误！");
            }
            else if (json_rs.status == "success") {
                //alert(unescape(decodeURI(json_rs.body))+"");
                //alert("加入成功！");
                //change_goods_number_response();

                //if (callback != null)
                //    callback(json_rs.cartid);
                location.href = "/Mobile/Order?cart=" + json_rs.cartid;
            }
            else {
                alert(json_rs.msg);
            }
        },
        error: function (xmlhttp) {
            //alert("系统错误！<br/>" + xmlhttp.responseText);
        }
    });
    hidePropertylistMask();
}

function flyToCart(event) {
    //$(".flyer-img").remove();
    //var offset = $("#head_cart em").offset();
    //var img = $("#bigPhoto").attr("src");//获取当前点击图片链接
    //var flyer = $('<img class="flyer-img" src="' + img + '"  height="15" width="15">');//抛物体对象
    //var sc_top = $(window).scrollTop();
    ////console.log($(window).scrollTop());
    //// console.log(offset);
    //// console.log(event.pageX + "" + event.pageY); 
    //endTop = offset.top + 10 - sc_top;
    //if (endTop < 0) {
    //    endTop = 2;
    //}
    //flyer.fly({
    //    start: {
    //        left: event.pageX, //抛物体起点横坐标
    //        top: event.pageY - sc_top////抛物体起点纵坐标
    //    },
    //    end: {
    //        left: offset.left + 10, //抛物体终点横坐标
    //        top: endTop //抛物体终点纵坐标
    //    },
    //    onEnd: function () {
    //        //$("#tip").show().animate({ width: '200px' }, 300).fadeOut(500);//成功加入购物车动画效果
    //        this.destory();//销毁抛物体
    //    }
    //});
}

//加入购物车
function addToCart(callback) {

    if (!CheckIsSelectAllItem()) {
        showPropertylistMask();
        tips("请选择属性");
        return;
    }
    var price = parseInt($totalprice.text());
    if (price <= 0) {
        tips("操作失败！");
        return;
    }
    $.ajax({
        url: "/Mobile/Cart/Add",
        type: "POST",
        async: true,
        cache: false,
        data: GetPostData(),
        success: function (result) {
            var json_rs = result;//eval("(" + result + ")");
            if (json_rs == null || json_rs.status == null) {//返回错误
                //showAlert("warning", "系统错误！", null);
                // alert("系统错误！");
            }
            else if (json_rs.status == "success") {
                //alert(unescape(decodeURI(json_rs.body))+"");
                //alert("加入成功！");
                change_goods_number_response();

                if (callback != null)
                    callback(json_rs.cartid);
            }
            else {
                alert(json_rs.msg);
            }
        },
        error: function (xmlhttp) {
            //alert("系统错误！<br/>" + xmlhttp.responseText);
        }
    });
    hidePropertylistMask();
}