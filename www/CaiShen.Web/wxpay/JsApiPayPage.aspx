<%@ Page Language="C#" AutoEventWireup="true" Inherits="JsApiPayPage" Codebehind="JsApiPayPage.aspx.cs" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>微信支付</title>
    <style type="text/css">
        .font_content {font-size: 14px; color: #FF6600; }
        .font_title {font-size: 16px; color: #FF0000; font-weight: bold; }
        table {width:90%;margin:20px auto; }
        table td {line-height:30px; }
    </style>
</head>

<script type="text/javascript">

    //调用微信JS api 支付
    function jsApiCall()
    {

        WeixinJSBridge.invoke(
        'getBrandWCPayRequest',
        <%=wxJsApiParam%>,//josn串
                    function (res)
                    {
                        WeixinJSBridge.log(res.err_msg);
                        //alert(res.err_code + res.err_desc + res.err_msg);
                        if (res.err_msg == "get_brand_wcpay_request:ok") {
                            //alert("微信支付成功!");
                            location.href="/MySuccess.aspx?trade_no=<%=Request.QueryString["trade_no"] %>&total_fee=<%=pay_total_fee %>";
                        } else if (res.err_msg == "get_brand_wcpay_request:cancel") {
                            //alert("用户取消支付!");
                        } else {
                            alert(res.err_msg);
                            //alert("支付失败!");
                        }

                    }
                    );
                }

                function callpay()
                {
                    if (typeof('WeixinJSBridge') == "undefined")
                    {
                        if (document.addEventListener)
                        {
                            document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                        }
                        else if (document.attachEvent)
                        {
                            document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                            document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                        }
                    }
                    else
                    {
                        jsApiCall();
                    }
                }
               
</script>

<body style="background-color:#fff;min-height:100%;">
    <form runat="server">
        <header id="header">
            <div class="header_l header_return"><a class="ico_10" href="javascript:history.go(-1);">返回 </a></div>
            <h1>确认付款 </h1>
        </header>
        <table align="center"  cellpadding="5" cellspacing="0">
            <tr>
                <td class="font_content" align="right">交易单号：</td>
                <td class="font_content" align="left">
                    <%=out_trade_no %></td>
            </tr>
            <tr>
                <td class="font_content" align="right">交易金额：</td>
                <td class="font_content" align="left"><%=pay_total_fee %> 元</td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="submit" runat="server" Text="立即支付" OnClientClick="javascript:callpay();return false;" Style="width: 210px; height: 40px; border-radius: 10px; background-color: #ac0510; border: 0px #FE6714 solid; cursor: pointer; color: white; font-size: 16px;" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="color: #FF0000; font-size: 16px; text-align:center;">
                        <asp:Literal ID="LitMessage" runat="server"></asp:Literal></div>
                </td>
            </tr>
             <tr>
                <td colspan="2">
                   &nbsp;
                </td>
            </tr>
        </table>
    </form>
    <script src="../js/jquery-1.7.2.min.js"></script>
    <script src="../js/common.js"></script>
</body>
</html>
