<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlinePay.aspx.cs" Inherits="PayNet.OnlinePay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="description" content="" />
    <meta name="keywords" content="" />
    <meta name="author" content="liucc" />
    <meta name="viewport" id="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1, user-scalable=no" />
    <title>在线支付</title>
    <script type="text/javascript" src="js/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="js/layer.js"></script>
    <script type="text/javascript" src="js/zepto.min.js"></script>
    <script type="text/javascript" src="js/scale_fn.js"></script>
    <script type="text/javascript" src="js/sdk.js"></script>
    <link href="Styles/TextStyle.css" rel="stylesheet" />
    <style>
        body {
            text-align: center;
            /*background: #E8ECF0;*/
            background: #E9E9E9;
            font-size: 14px;
        }

        .divBody {
            margin: 0 auto;
            width: 480px;
            margin-top: 30px;
        }

        .divContent {
            background-color: white;
            border: 1px solid #BFBFBF;
            border-radius: 10px;
            box-shadow: -1px 1px 15px #aaaaaa;
        }

        .divHeader {
            text-align: left;
            vertical-align: central;
            background: #F5F5F5;
            padding-left: 10px;
            margin-left: -1px;
            border: 1px solid #BFBFBF;
            border-radius: 8px 8px 0px 0px;
        }

        .divPopupHidden {
            background-color: #E9E9E9;
            position: absolute;
            left: 0px;
            top: 0px;
            height: 100%;
            width: 100%;
            display: none;
            z-index: 99;
        }

        .divPopupShow {
            background-color: #E9E9E9;
            position: absolute;
            left: 0px;
            top: 0px;
            height: 100%;
            width: 100%;
            z-index: 99;
        }

        .tdLeft {
            text-align: left;
        }

        .tdRight {
            text-align: right;
            min-width: 100px;
        }

        .submitStyle {
            text-align: center;
            margin-left: 0px;
            margin-top: 10px;
            width: 150px;
            height: 45px;
            background-color: #4CA76A;
            border: 1px solid #4CA76A;
            border-radius: 8px;
            color: white;
            font-size: 16px;
            cursor: pointer;
        }

        .text {
            margin-left: 8px;
            padding-left: 3px;
            padding-right: 3px;
            height: 27px;
            font-size: 14px;
        }

        .textVaild {
            margin-left: 8px;
            margin-top: 0px;
            padding-left: 2px;
            padding-right: 2px;
            text-align: left;
            vertical-align: central;
            width: 100px;
            height: 27px;
            font-size: 14px;
        }

        .tr {
            height: 38px;
        }

        .hidden {
            width: 0px;
            height: 0px;
            opacity: 0;
            display: none;
        }
    </style>
    <script type="text/javascript">
        /**
         * 支付方式变更
         * */
        function rdoPayClik(param) {
            if (param == null) {
                return;
            }
            var elementPay = document.getElementById("pay_bankcode");
            if (elementPay == null) {
                return;
            }
            elementPay.value = "";

            var elemenValue = param.value;
            if (param.checked) {
                elementPay.value = elemenValue;
                setMoneyList(elemenValue);
            }
        }
        /**
         * 设置金额列表
         * */
        function setMoneyList(payType) {
            setPayMoney("");

            var element = document.getElementById("tablePayMoney");
            if (element == null) {
                return;
            }
            let selectId = "table_" + payType;
            for (var i = 0; i < element.children.length; i++) {
                if (element.children[i].id == selectId) {
                    element.children[i].style.display = "";
                    setDefaultMoney(payType);
                } else {
                    element.children[i].style.display = "none";
                }
            }
        }
        /**
         * 设置默认金额
         */
        function setDefaultMoney(payType) {
            let listElem = document.getElementsByName("rdoPayMoney_" + payType);
            if (listElem == null || listElem.length == 0) {
                return;
            }

            let checkItem = null;
            for (var i = 0; i < listElem.length; i++) {
                if (listElem[i].checked) {
                    checkItem = listElem[i];
                    break;
                }
            }
            if (checkItem == null) {
                listElem[0].checked = true;
                checkItem = listElem[0];
            }
            payMoneyChange(checkItem);
        }

        /**
         * 金额变更
         */
        function payMoneyChange(param) {
            if (param == null) {
                return;
            }
            setPayMoney(param.value);
        }
        /**
         * 设置支付金额
         */
        function setPayMoney(value) {
            var element = document.getElementById("pay_amount");
            if (element != null) {
                element.value = value;
            }
        }

        /**
         * 提交前校验
         * */
        function submitCheck() {
            var group = $('#group').val();
            if (group == null || group == "") {
                alertEx("[客户区组]不能为空.");
                return;
            }
            var accounts = $('#accounts').val();
            if (accounts == null || accounts == "") {
                alertEx("[游戏账号]不能为空.");
                return;
            }

            var pay_bankcode = $('#pay_bankcode').val();
            if (pay_bankcode == null || pay_bankcode == "") {
                alertEx("[支付类型]未选择.");
                return;
            }

            var pay_amount = $('#pay_amount').val();
            if (pay_amount == null || pay_amount == "") {
                alertEx("[支付金额]未选择.");
                return;
            }

            var code = $('#code').val();
            if (code == null || code == "") {
                alertEx("[验证码]不能为空.");
                return;
            }

            var checkCode = getCookie("CheckCode");
            if (checkCode != null && checkCode != "" && checkCode.toLowerCase() != code.toLowerCase()) {
                alertEx("[验证码]输入不正确.");
                return;
            }

            var pay_orderid = $('#pay_orderid').text();
            var group = $('#group').val();
            var account_url = getCookie("LocalUrl");
            var url = account_url + "/sdk/getPostParam";
            var data = { "pay_amount": pay_amount, "pay_orderid": pay_orderid, "pay_bankcode": pay_bankcode, "pay_amount": pay_amount, "group": group, "accounts": accounts, "code": code };
            var index = loading();
            send_fn(url, data, function (result) {
                if (result.status != '1') {
                    layer.close(index);
                    alertEx(result.message);
                    return;
                }
                var postMessage = document.getElementById("postMessage");
                if (postMessage != null) {
                    postMessage.value = result.message;
                }
                var form = document.getElementById("form2");
                if (form == null) {
                    layer.close(index);
                    return;
                }
                var newUrl = account_url + "/" + result.postUrl;
                form.setAttribute("action", newUrl);
                form.submit();
            });
            return;
        }

    </script>
</head>
<body>
    <form id="form1" action="#" runat="server" method="post">
        <div id="divBody" class="divBody" runat="server" visible="true">
            <div class="divContent">
                <div class="divHeader">
                    <span style="vertical-align: central; line-height: 35px;">充值中心</span>
                </div>
                <div style="border-right: 1px solid #B6B4B6; padding-left: 10px; padding-right: 10px; padding-bottom: 10px;">
                    <table>
                        <tr class="tr">
                            <td class="tdRight">订单号：</td>
                            <td class="tdLeft">
                                <label id="pay_orderid" runat="server" style="margin-left: 6px;" />
                            </td>
                        </tr>
                        <tr class="tr">
                            <td class="tdRight">客户区组：</td>
                            <td class="tdLeft">
                                <asp:TextBox ID="group" MaxLength="30" runat="server" Enabled="false"
                                    CssClass="text" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdRight">支付类型：</td>
                            <td class="tdLeft">
                                <asp:Label runat="server" ID="tablePayType" />
                            </td>
                        </tr>
                        <tr class="tr">
                            <td class="tdRight">游戏账号：</td>
                            <td class="tdLeft">
                                <asp:TextBox ID="accounts" MaxLength="30" runat="server" Enabled="false" CssClass="text" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdRight">充值金额：</td>
                            <td class="tdLeft">
                                <asp:Label runat="server" ID="tablePayMoney" />
                            </td>
                        </tr>
                        <tr class="tr" style="margin-top: 8px;">
                            <td class="tdRight">验证码：</td>
                            <td class="tdLeft">
                                <div style="float: left;">
                                    <div style="float: left; vertical-align: central;">
                                        <asp:TextBox ID="code" MaxLength="4" runat="server" CssClass="textVaild" />
                                    </div>
                                    <div style="float: left;">
                                        <img id="vcodeimg" hspace="0" vspace="0"
                                            style="cursor: pointer; border: solid 1px #A9D2FE; margin-left: 8px;" onclick="this.src='ValidateCode.aspx?time=' + Math.random()"
                                            title="点击刷新验证码" align="absMiddle" src="ValidateCode.aspx?time=' + Math.random()" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 80px; margin-bottom: 20px;">
                            <td colspan="2" style="text-align: center;">
                                <input type="button" class="submitStyle" onclick="submitCheck()" value="确 认 支 付" />
                            </td>
                        </tr>
                    </table>

                    <asp:TextBox ID="pay_bankcode" runat="server" CssClass="hidden" />
                    <asp:TextBox ID="pay_amount" runat="server" CssClass="hidden" />
                </div>
            </div>
        </div>
    </form>
    <form id="form2" action="#" method="post" class="hidden">
        <input type="hidden" id="postMessage" name="postMessage" class="hidden" />
    </form>

    <script type="text/javascript">
        window.onload = function () {
            let listElem = document.getElementsByName("rdoPay");
            if (listElem == null || listElem.length == 0) {
                return;
            }
            let checkItem = null;
            for (var i = 0; i < listElem.length; i++) {
                if (listElem[i].checked) {
                    checkItem = listElem[i];
                    break;
                }
            }
            if (checkItem == null) {
                listElem[0].checked = true;
                checkItem = listElem[0];
            }
            setMoneyList(checkItem.value);
        }
    </script>

</body>
</html>

