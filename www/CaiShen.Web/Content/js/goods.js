function BindEvent() {
    //点击属性值
    $PropertyWrapper.delegate("li.value>span", "click", function () {
        var $val_item = $(this);//操作属性值项
        var $prop_item = $(this).parent().parent();//操作属性项
        //$val_item.toggleClass("active");
        if ($val_item.hasClass("active")) {
            $val_item.removeClass("active");
            _propertyJson[$prop_item.data("index")].values[$val_item.data("index")].select = false;
            if (!$val_item.siblings().hasClass("active"))//都没有选中
            {
                _propertyJson[$prop_item.data("index")].select = false;
            }
        }
        else {

            $val_item.addClass("active");
            _propertyJson[$prop_item.data("index")].select = true;
            _propertyJson[$prop_item.data("index")].values[$val_item.data("index")].select = true;
        }

        //点击属性值后，清空sku
        _skulistJson = [];
        ShowSKUHtml();
        //console.log(JSON.stringify(_propertyJson));
    });

    //SKU批量赋值
    $("body").delegate("#btn_batch_sku", "click", function () {
        var price = $("[name='sku_price_batch']").val();
        var count = $("[name='sku_count_batch']").val();
        var costprice = $("[name='sku_costprice_batch']").val();
        var shopprice = $("[name='sku_shopprice_batch']").val();
        var shopcode = $("[name='sku_shopcode_batch']").val();
        var volume = $("[name='sku_volume_batch']").val();
        var weight = $("[name='sku_weight_batch']").val();
        var squareweight = $("[name='sku_squareweight_batch']").val();
        var shoppricerate = $("[name='sku_shoppricerate_batch']").val();
        var clientpricerate = $("[name='sku_clientpricerate_batch']").val();
        var expandarea = $("[name='sku_expandarea_batch']").val();

        if (price != "") {
            $SkuWrapper.find("[name='sku_price']").val(price);
            $.each(_skulistJson, function (index, data) {
                data.Price = price;
            });
        }
        if (costprice != "") {
            $SkuWrapper.find("[name='sku_costprice']").val(costprice);
            $.each(_skulistJson, function (index, data) {
                data.CostPrice = costprice;
            });
        }
        if (shopprice != "") {
            $SkuWrapper.find("[name='sku_shopprice']").val(shopprice);
            $.each(_skulistJson, function (index, data) {
                data.ShopPrice = shopprice;
            });
        }
        if (count != "") {
            $SkuWrapper.find("[name='sku_count']").val(count);
            $.each(_skulistJson, function (index, data) {
                data.Count = count;
            });
        }
        if (shopcode != "") {
            $SkuWrapper.find("[name='sku_shopcode']").val(shopcode);
            $.each(_skulistJson, function (index, data) {
                data.ShopCode = shopcode;
            });
        }
        if (volume != "") {
            $SkuWrapper.find("[name='sku_volume']").val(volume);
            $.each(_skulistJson, function (index, data) {
                data.Volume = volume;
            });
        }
        if (weight != "") {
            $SkuWrapper.find("[name='sku_weight']").val(weight);
            $.each(_skulistJson, function (index, data) {
                data.Weight = weight;
            });
        }
        if (squareweight != "") {
            $SkuWrapper.find("[name='sku_squareweight']").val(squareweight);
            $.each(_skulistJson, function (index, data) {
                data.SquareWeight = squareweight;
            });
        }
        if (shoppricerate != "") {
            $SkuWrapper.find("[name='sku_shoppricerate']").val(shoppricerate);
            $.each(_skulistJson, function (index, data) {
                data.ShopPriceRate = shoppricerate;
            });
        }
        if (clientpricerate != "") {
            $SkuWrapper.find("[name='sku_clientpricerate']").val(clientpricerate);
            $.each(_skulistJson, function (index, data) {
                data.ClientPriceRate = clientpricerate;
            });
        }
        if (expandarea != "") {
            $SkuWrapper.find("[name='sku_expandarea']").val(expandarea);
            $.each(_skulistJson, function (index, data) {
                data.ExpandArea = expandarea;
            });
        }
        //console.log(JSON.stringify(_skulistJson));

    });

    //SKU-价格被改动赋值
    $SkuWrapper.delegate("[name='sku_price']", "change", function () {
        _skulistJson[$(this).data("skuindex")].Price = $(this).val();
    });
    //SKU-成本价格被改动赋值
    $SkuWrapper.delegate("[name='sku_costprice']", "change", function () {
        _skulistJson[$(this).data("skuindex")].CostPrice = $(this).val();
    });
    //SKU-加盟商价格被改动赋值
    $SkuWrapper.delegate("[name='sku_shopprice']", "change", function () {
        _skulistJson[$(this).data("skuindex")].ShopPrice = $(this).val();
    });
    //SKU-数量被改动赋值
    $SkuWrapper.delegate("[name='sku_count']", "change", function () {
        _skulistJson[$(this).data("skuindex")].Count = $(this).val();
    });
    //SKU-商家编码被改动赋值
    $SkuWrapper.delegate("[name='sku_shopcode']", "change", function () {
        _skulistJson[$(this).data("skuindex")].ShopCode = $(this).val();
    });
    //SKU-体积
    $SkuWrapper.delegate("[name='sku_volume']", "change", function () {
        _skulistJson[$(this).data("skuindex")].Volume = $(this).val();
    });
    //SKU-重量
    $SkuWrapper.delegate("[name='sku_weight']", "change", function () {
        _skulistJson[$(this).data("skuindex")].Weight = $(this).val();
    });
    //SKU-平方克重
    $SkuWrapper.delegate("[name='sku_squareweight']", "change", function () {
        _skulistJson[$(this).data("skuindex")].SquareWeight = $(this).val();
    });
    //SKU-门店价格比
    $SkuWrapper.delegate("[name='sku_shoppricerate']", "change", function () {
        _skulistJson[$(this).data("skuindex")].ShopPriceRate = $(this).val();
    });
    //SKU-终端价比
    $SkuWrapper.delegate("[name='sku_clientpricerate']", "change", function () {
        _skulistJson[$(this).data("skuindex")].ClientPriceRate = $(this).val();
    });
    //SKU-面积
    $SkuWrapper.delegate("[name='sku_expandarea']", "change", function () {
        _skulistJson[$(this).data("skuindex")].ExpandArea = $(this).val();
    });

    //修改属性值
    $PropertyWrapper.delegate(":text", "change", function () {
        var $val_item = $(this);//操作属性值项
        var $prop_item = $(this).parent().parent();//操作属性项
        _propertyJson[$prop_item.data("index")].values[$val_item.data("index")][$val_item.data("pv")] = $val_item.val();
    });
}


//显示属性html
function ShowPropertyHtml(dataJson) {
    //alert(JSON.stringify(dataJson));
    _propertyJson = dataJson;
    var contentHtml = "";
    if (dataJson.length > 0) {
        $.each(dataJson, function (index, data) {
            var valuedataJson = GetPropertyValueData(data.ID);
            data["values"] = valuedataJson;
            data.select = false;
            contentHtml += "<ul data-index='" + index + "' data-id='" + data.ID + "'><li class='name'>" + data.Prop_Name + "</li><li class='value'>" + GetPropertyValueHtml(valuedataJson, data) + "</li></ul>";
        });
        $PropertyWrapper.show();
        $PropertyWrapper.html(contentHtml);

        //console.log(JSON.stringify(_propertyJson));
    }
    else {
        //$PropertyWrapper.hide();
        $PropertyWrapper.html("");

    }
}

//获得当前产品类型属性 并 显示
function GetPropertyData() {
    var typeid = $GoodsTypes.val();
    if (parseInt(typeid) == 0)
        return null;

    $.ajax({
        type: 'POST',
        url: '/Property/GetPropertyJson',
        data: { "typeid": typeid },
        dataType: "json",
        async: false,
        success: function (data) {
            //console.log("GetPropertyData:"+data)
            ShowPropertyHtml(data);
        }
    });
}
function GetPropertyValueHtml(dataJson, propertyData) {
    var contentHtml = "";
    $.each(dataJson, function (index, data) {
        //var exsitPvData = {};
        var isExistPV = existGoodsPropertyValue(data);
        data.select = false;
        if (isExistPV) {//只有文本属性值才能被选中 20170627 改成 INPUT默认选中
            data.select = true;
            propertyData.select = true;
        }

        if (data.PV_ShowType == 0) {
            contentHtml += "<span " + (isExistPV ? " class='active' " : "") + " data-index='" + index + "' data-id='" + data.ID + "'>" + data.PV_Name + "</span>";
            if (propertyData.Prop_IsPrice == 1)//单独计价
            {
                contentHtml += "单价:<input data-index='" + index + "' data-id='" + data.ID + "' value='" + data.PV_Price + "' data-pv='PV_Price'/>元";
            }
        }
        else {//如果是输入框
            //contentHtml += "<span data-index='" + index + "' data-id='" + data.ID + "'>";

            //20170627 改成 INPUT默认选中
            data.select = true;
            propertyData.select = true;

            contentHtml += "<br/>" + data.PV_Name + "：最小取值:<input data-index='" + index + "'  data-id='" + data.ID + "' value='" + data.PV_Min + "' data-pv='PV_Min'/>";
            contentHtml += "最大取值:<input data-index='" + index + "' data-id='" + data.ID + "' value='" + data.PV_Max + "'  data-pv='PV_Max'/>";
            contentHtml += "倍数:<input data-index='" + index + "' data-id='" + data.ID + "' value='" + data.PV_Multiple + "'  data-pv='PV_Multiple'/>";
            contentHtml += "增减量:<input data-index='" + index + "' data-id='" + data.ID + "' value='" + data.PV_Increment + "'  data-pv='PV_Increment'/>";
            if (propertyData.Prop_IsPrice == 1)//单独计价
            {
                contentHtml += "单价:<input data-index='" + index + "' data-id='" + data.ID + "' value='" + data.PV_Price + "' data-pv='PV_Price'/>元";
            }
        }
        //contentHtml += "单位:<input data-id='" + data.ID + "' value='" + data.PV_Unit + "'/>";
        //contentHtml += "</span>";
    });
    return contentHtml;
}

function GetPropertyValueData(propertyid) {

    var returnData;
    if (parseInt(propertyid) == 0)
        return null;

    $.ajax({
        type: 'POST',
        url: '/Property/GetPropertyValueJson',
        data: { "propertyid": propertyid },
        dataType: "json",
        async: false,
        success: function (data) {
            returnData = data;
        }
    });
    return returnData;
}

//获取数据库中已设置产品属性值json
function GetGoodsPropertyValueData() {

    $.ajax({
        type: 'POST',
        url: '/GoodsPropertyValue/GetPropertyValueJson',
        data: { "goodsid": goodsId },
        dataType: "json",
        async: false,
        success: function (data) {
            _existPropertyValueJson = data;
            //console.log(JSON.stringify(_existPropertyValueJson));
        }
    });
}
//是否已存在产品属性值表中 pv_id
function existGoodsPropertyValue(defaultData) {
    var isExist = false;
    if (_existPropertyValueJson != null && _existPropertyValueJson.length > 0) {
        $.each(_existPropertyValueJson, function (index, data) {
            if (data.PropertyValueID == defaultData.ID) {
                //existData = JSON.parse(JSON.stringify(data));;

                defaultData.PV_Min = data.GPV_Min;
                defaultData.PV_Max = data.GPV_Max;
                defaultData.PV_Multiple = data.GPV_Multiple;
                defaultData.PV_Increment = data.GPV_Increment;
                defaultData.PV_Price = data.GPV_Price;
                defaultData.PV_Unit = data.GPV_Unit;
                defaultData.PV_ColorHEX = data.GPV_ColorHEX;
                defaultData.PV_ColorImage = data.GPV_ColorImage;

                isExist = true;
                return false;
            }
        });
    }
    return isExist;
}

//获取数据库中已设置产品SKU json
function GetGoodsSKUData() {

    $.ajax({
        type: 'POST',
        url: '/GoodsSKU/GetSKUJson',
        data: { "goodsid": goodsId },
        dataType: "json",
        async: false,
        success: function (data) {
            _existGoodsSKUJson = data;
            //console.log(JSON.stringify(_existGoodsSKUJson));
        }
    });
}
//是否已存在产品属性值表中 pv_id
function existGoodsSKU(defaultData) {
    var isExist = false;
    if (_existGoodsSKUJson != null && _existGoodsSKUJson.length > 0) {
        $.each(_existGoodsSKUJson, function (index, data) {
            //console.log("data:" + JSON.stringify(data) + ",defaultData:" + JSON.stringify(defaultData));

            if (data.Properties == defaultData.Properties) {
                //console.log("data.Properties：" + data.Properties + ", defaultData.Properties:" + defaultData.Properties);
                defaultData.Price = data.Price;
                defaultData.CostPrice = data.CostPrice;
                defaultData.ShopPrice = data.ShopPrice;
                defaultData.Count = data.Count;
                defaultData.ShopCode = data.ShopCode;
                defaultData.GoodsCode = data.GoodsCode;
                defaultData.Volume = data.Volume;
                defaultData.Weight = data.Weight;
                defaultData.SquareWeight = data.SquareWeight;
                defaultData.ExpandArea = data.ExpandArea;
                defaultData.ShopPriceRate = data.ShopPriceRate;
                defaultData.ClientPriceRate = data.ClientPriceRate;
                defaultData.ID = data.ID;

                isExist = true;
                return false;
            }
            else {
                defaultData.Price = "0";
                defaultData.CostPrice = "0";
                defaultData.ShopPrice = "0";
                defaultData.Count = "10000";
                defaultData.ShopCode = "";
                defaultData.GoodsCode = "";
                defaultData.Volume = "0";
                defaultData.Weight = "0";
                defaultData.SquareWeight = "0";
                defaultData.ExpandArea = "0";
                defaultData.ShopPriceRate = "1.1";
                defaultData.ClientPriceRate = "1.3";
                defaultData.ID = 0;
            }
        });
    }
    else {
        defaultData.Price = "0";
        defaultData.CostPrice = "0";
        defaultData.ShopPrice = "0";
        defaultData.Count = "10000";
        defaultData.ShopCode = "";
        defaultData.GoodsCode = "";
        defaultData.Volume = "0";
        defaultData.Weight = "0";
        defaultData.SquareWeight = "0";
        defaultData.ExpandArea = "0";
        defaultData.ShopPriceRate = "1.1";
        defaultData.ClientPriceRate = "1.3";
        defaultData.ID = 0;
    }
    return isExist;
}


//显示SKU html
function ShowSKUHtml() {
    var skuContent = { html: "" };
    var skuItem = { "currentline": [], index: 0 };//{ Properties: "", PropertiesName: "", index: 0 };//单个sku
    var propertyJsonSelect = GetPropertyJsonSelect();
    //console.log(JSON.stringify(propertyJsonSelect));
    skuContent.html += "<table class='table table-bordered'><tr>";
    skuContent.html += ShowSKUTitleHtml(propertyJsonSelect) + "</tr><tr>";
    tempSKUHtmlLoop(skuItem, skuContent, 0, propertyJsonSelect);
    skuContent.html += "</tr></table>";
    $SkuWrapper.html(skuContent.html);

    //console.log(JSON.stringify(_skulistJson));

}
//获得当前属性后_剩余展示项数量
function tempGetCountLoop(countObj, index, _propertyAttr) {
    if (index < _propertyAttr.length - 1) {//最底层不需要计算 ，所以 _propertyAttrValues.length-1
        countObj.count = countObj.count * _propertyAttr[index + 1].values.length;
        tempGetCountLoop(countObj, index + 1, _propertyAttr);
    }
}
//循环显示已选属性名称（SKU表头）html
function ShowSKUTitleHtml(_propertyAttr) {
    var skuTitleHtml = "";
    for (var pi = 0; pi < _propertyAttr.length; pi++) {
        skuTitleHtml += "<th>" + _propertyAttr[pi].Prop_Name + "</th>";
    }

    skuTitleHtml += "<th>体积(m³)</th><th>面积(m²)</th><th>重量(kg)</th><th>平方克重(g)</th><th>成本价</th><th>加盟商价比</th><th>加盟商价</th><th>终端价比</th><th>终端价</th><th>库存</th><th>商家编码</th>";
    if (goodsId != 0) {
        skuTitleHtml += "<th>价格配置</th>";
    }
    return skuTitleHtml;
}
//循环 拼接SKU html
function tempSKUHtmlLoop(skuItem, _tempContent, index, _propertyAttr) {
    //console.log("进入tempSKUHtmlLoop：" + index);
    if (index < _propertyAttr.length) {
        for (var pi = 0; pi < _propertyAttr[index].values.length; pi++) {
            var countObj = { count: 1 };
            //if (skuItem.lines[skuItem.index] == undefined) skuItem.lines[skuItem.index] = { Properties: "", PropertiesName: "" };
            if (skuItem.currentline[index] == undefined) skuItem.currentline[index] = { Properties: "", PropertiesName: "", Price: "0", CostPrice: "0", ShopPrice: "0", Count: "0", ShopCode: "", GoodsCode: "", Volume: "0", Weight: "0" };
            //skuItem.lines[skuItem.index].Properties += _propertyAttr[index].ID + ":" + _propertyAttr[index].values[pi].ID + ";";
            //skuItem.lines[skuItem.index].PropertiesName += _propertyAttr[index].Prop_Name + ":" + _propertyAttr[index].values[pi].PV_Name + ";";
            skuItem.currentline[index].Properties = _propertyAttr[index].ID + ":" + _propertyAttr[index].values[pi].ID + ";";
            skuItem.currentline[index].PropertiesName = _propertyAttr[index].Prop_Name + ":" + _propertyAttr[index].values[pi].PV_Name + ";";
            //skuItem.Properties += _propertyAttr[index].ID + ":" + _propertyAttr[index].values[pi].ID + ";";
            //skuItem.PropertiesName += _propertyAttr[index].Prop_Name + ":" + _propertyAttr[index].values[pi].PV_Name + ";";
            //console.log(JSON.stringify(skuItem));

            tempGetCountLoop(countObj, index, _propertyAttr);
            //if (_propertyAttr[index].values[pi].PV_ShowType == 0)// 20170627 调整输入框 需要允许出现
            //{
            _tempContent.html += "<td class='col-2' rowspan='" + countObj.count + "'>" + _propertyAttr[index].values[pi].PV_Name + "</td>";
            //}
            tempSKUHtmlLoop(skuItem, _tempContent, index + 1, _propertyAttr)
            if (index == _propertyAttr.length - 1)//最底层或最后一个属性
            {

                var skuItemObj = JoinSkuItem(skuItem.currentline);
                //console.log("skuItem:" + JSON.stringify(skuItemObj));
                existGoodsSKU(skuItemObj);
                //console.log("skuItem2:" + JSON.stringify(skuItemObj));
                _skulistJson.push(skuItemObj);

                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_volume'  placeholder='体积' value='" + skuItemObj.Volume + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_expandarea'  placeholder='展开面积' value='" + skuItemObj.ExpandArea + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_weight'  placeholder='重量' value='" + skuItemObj.Weight + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_squareweight'  placeholder='平方克重' value='" + skuItemObj.SquareWeight + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_costprice'  placeholder='成本价' value='" + skuItemObj.CostPrice + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_shoppricerate'  placeholder='门店价比' value='" + skuItemObj.ShopPriceRate + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_shopprice'  placeholder='加盟商价' value='" + skuItemObj.ShopPrice + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_clientpricerate'  placeholder='终端价比' value='" + skuItemObj.ClientPriceRate + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_price'  placeholder='终端价' value='" + skuItemObj.Price + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_count'  placeholder='数量' value='" + skuItemObj.Count + "'/></td>";
                _tempContent.html += "<td><input type='text' data-skuindex='" + skuItem.index + "' name='sku_shopcode' placeholder='商家编码' value='" + skuItemObj.ShopCode + "'/></td>";
                if (goodsId != 0) {
                    _tempContent.html += "<td><a href='javascript:void(0)' data-id='" + skuItemObj.ID + "' class='sku_price_set'>配置</a></td>";
                }
                //_tempContent.html += "<td><input type='text' data-id='" + _propertyAttr[index].values[pi].PV_ID + "' placeholder='产品编号'/></td>";
                _tempContent.html += "</tr><tr>"
                //skuItem.GoodsID = goodsId;
                //console.log("_propertyAttr[index].values[pi]" + JSON.stringify(_propertyAttr[index].values[pi]));

                //skuItem.Properties = "";
                //skuItem.PropertiesName = "";
                skuItem.index++;
            }

        }
    }
}

///合并单条sku
function JoinSkuItem(skuItemAttr) {
    var skuItem = { Properties: "", PropertiesName: "" };
    $.each(skuItemAttr, function (index, data) {
        skuItem.Properties += data.Properties;
        skuItem.PropertiesName += data.PropertiesName;
    });
    //console.log(JSON.stringify(skuItem));
    return skuItem;
}


function SaveGoodsSKU() {

    if (_skulistJson != null && _skulistJson.length > 0) {
        //批量处理
        $.ajax({
            type: 'POST',
            url: '/GoodsSKU/AddBatch?GoodsID=' + goodsId,
            data: { skuListJson: JSON.stringify(_skulistJson) },
            dataType: "json",
            async: false,
            success: function (rep) {
            }
        });
    }
}


function SavePropertyValue() {
    //循环 所有已选属性和额值 
    if (_propertyJson != null && _propertyJson.length > 0) {
        $.each(_propertyJson, function (index, data) {
            //if (data.select == true) {
            var propertyItem = JSON.parse(JSON.stringify(data));//data.slice(0);
            //所有选中属性值
            var propertyItemSelectAll = [];
            //所有未选中属性值
            var propertyItemNoSelectAll = [];
            //所有未选中属性:值对
            var propertyValueNoSelectAll = [];

            for (var i = propertyItem.values.length - 1; i > -1; i--) {
                //选中或 输入框 属性值
                if (propertyItem.values[i].select == true || propertyItem.values[i].PV_ShowType == 1) {
                    propertyItem.values[i].GoodsID = goodsId;
                    propertyItemSelectAll.push(propertyItem.values[i]);//汇总
                }
                else {
                    //未选中则删除 数据库记录
                    propertyItem.values[i].GoodsID = goodsId;
                    propertyItemNoSelectAll.push(propertyItem.values[i]);//汇总

                }
            }

            //=======================================批量处理--开始--=============================================

            if (propertyItemSelectAll != null && propertyItemSelectAll.length > 0) {

                //-----保存产品属性值-----
                //console.log("propertyItemSelectAll:" + JSON.stringify(propertyItemSelectAll));
                $.ajax({
                    type: 'POST',
                    url: '/GoodsPropertyValue/AddBatch?GoodsID=' + goodsId,
                    data: { pvListJson: JSON.stringify(propertyItemSelectAll) },
                    dataType: "json",
                    async: false,
                    success: function (rep) {
                        //console.log(JSON.stringify(rep));
                    }
                });
            }
            //console.log("propertyItemNoSelectAll:" + JSON.stringify(propertyItemNoSelectAll));

            if (propertyItemNoSelectAll != null && propertyItemNoSelectAll.length > 0) {

                //-----删除属性值-----
                $.ajax({
                    type: 'POST',
                    url: '/GoodsPropertyValue/DeleteBatch?GoodsID=' + goodsId,                    data: { pvListJson: JSON.stringify(propertyItemNoSelectAll) },
                    dataType: "json",
                    async: false,
                    success: function (rep) {
                        //console.log(JSON.stringify(rep));
                    }
                });

                //-----删除SKU-----
                //循环所有未选中，组合成属性:值对数组
                $.each(propertyItemNoSelectAll, function (index, data) {
                    propertyValueNoSelectAll.push(data.PropertyID + ":" + data.ID);
                });

                $.ajax({
                    type: 'POST',
                    url: '/GoodsSKU/DeleteBatch?GoodsID=' + goodsId,                    data: { PropertiesJson: JSON.stringify(propertyValueNoSelectAll) },
                    dataType: "json",
                    async: false,
                    success: function (rep) {
                        //console.log(JSON.stringify(rep));
                    }
                });
            }

            //=======================================批量处理--结束--=============================================
        });
    }
}


//检测sku价格或数量是否存在为空项，有空返回true，否则返回false;
function ExistSkuValueEmpty() {
    var returnFlag = false;
    //console.log("ExistSkuValueEmpty:" + JSON.stringify(_skulistJson));
    $.each(_skulistJson, function (index, data) {
        if (data.Price == undefined || data.Price === "" || data.CostPrice == undefined || data.CostPrice === "" || data.ShopPrice == undefined || data.ShopPrice === "" || data.Count == undefined || data.Count === "") {
            returnFlag = true;
            return false;
        }
    });
    return returnFlag;
}

//获得已选择属性项
function GetPropertyJsonSelect() {
    var propertyJsonSelect = [];
    //console.log("_propertyJson:" + _propertyJson);
    if (_propertyJson != undefined && _propertyJson.length > 0) {
        $.each(_propertyJson, function (index, data) {
            if (data.select == true) {
                var propertyItem = JSON.parse(JSON.stringify(data));//data.slice(0);
                for (var i = propertyItem.values.length - 1; i > -1; i--) {
                    if (propertyItem.values[i].select == false) {
                        propertyItem.values.splice(i, 1);
                    }
                }
                //$.each(propertyItem.values, function (val_index, val_data) {
                //});
                propertyJsonSelect.push(propertyItem);
            }
        });
    }
    return propertyJsonSelect;
}