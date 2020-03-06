
var isWaitForResponse = false;
function send_fn(url, data, callback){
	if(isWaitForResponse) {
		return;
	}else {
        isWaitForResponse = true;
	}
    $.ajax({
        type:"post",
        url:url,
        data:data,
        dataType:'json',
        success:function(data){
            callback(data);
            setTimeout(function (args) {
                isWaitForResponse = false;
            },1000);
        },
        error:function(){
            popup_fn("网络异常，请稍后再试！");
            setTimeout(function (args) {
                isWaitForResponse = false;
            },500);
        }
    });
}

function popup_fn(msg){
    $('.common_popUp p').html(msg).parent().fadeIn(30).removeClass('hidden');
    var time=setInterval(function(){
        $('.common_popUp').fadeOut(30).addClass('hidden');
        clearInterval(time);
    },3000);
}

function alertEx(message) {
    layer.open({
        style: 'max-width:478px',
        content: message,
        btn: '关闭'
    });
}

function loading() {
    return layer.open({
        type: 2,
        shadeClose: false
    });
}

function loading2(message) {
    if (message == null || message == "") {
        return loading();
    }
    return layer.open({
        type: 2,
        shadeClose: false,
        content: message
    });
}

function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg)) {
        return unescape(arr[2]);
    }
    else {
        return null;
    }
}


