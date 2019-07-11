$(document).ready(function () {
    var topStr = "";
    var secondStr = ""; // YourEye, OtherStone
    var thirdStr = "";
    var href = document.location.href;
    if (href.indexOf("/Layout/") >= 0) {
        // layout
        topStr = "../";
        secondStr = "";
        thirdStr = "../Adm/";
    }
    else if (href.indexOf("/Info/") >= 0) {
        // info
        topStr = "../";
        secondStr = "../Layout/";
        thirdStr = "../Adm/";
    }
    else if (href.indexOf("/Adm/") >= 0) {
        // adm
        topStr = "../";
        secondStr = "../Layout/";
    } else {
        secondStr = "Layout/";
        thirdStr = "Adm/";
    }

    // 初始化界面的内容
    var nav = '<nav class="navbar navbar-inverse" role="navigation">\
        <div class="navbar-header">\
            <a class="navbar-brand" href="#">Hello C# World！</a>\
        </div>\
        <div>\
            <ul class="nav navbar-nav">\
                <li class="active"><a href="'+ topStr + 'index.html">首页</a></li>\
                <li><a style="color:red;" href="' + secondStr + 'csharp_create_code_online.html">C#在线代码生成器</a></li>\
                <li><a style="color:orange;" href="' + secondStr + 'csharp_online_tools.html">在线工具集</a></li>\
                <li><a style="color:yellow;" href="' + secondStr + 'friendly_dll.html">三方类库</a></li>\
                <li><a style="color:green;" href="' + secondStr + 'share_dll_download.html">共享DLL下载</a></li>\
                <li><a style="color:cyan;" href="' + secondStr + 'csharp_code_download.html">Helper代码</a></li>\
                <li><a style="color:blue;" href="' + secondStr + 'csharp_find_new.html">新发现</a></li>\
                <li><a style="color:purple;" href="' + secondStr + 'recommend_and_share.html">关注前言</a></li>\
                <li><a style="color:white;" href="' + secondStr + 'csharp_api.html">API接口</a></li>\
                <li><a style="color:white;" href="' + secondStr + 'share_soft.html">共享程序</a></li>\
                <li><a href="' + secondStr + 'seg_and_message.html">建议&留言</a></li>\
                <li><a href="' + secondStr + 'about_and_thanks.html">关于&帮助</a></li>\
            </ul>\
        </div>\
    </nav>';

    $("#nav").html(nav);
    var sitemap = '<div class="container text-center">\
        <ol class="breadcrumb">\
            <li>站点地图：</li>\
            <li class="active"><a href="'+ topStr + 'index.html">首页</a></li>\
            <li><a href="' + secondStr + 'csharp_create_code_online.html">C#在线代码生成器</a></li>\
                <li><a href="' + secondStr + 'csharp_online_tools.html">在线工具集</a></li>\
                <li><a href="' + secondStr + 'friendly_dll.html">三方类库</a></li>\
                <li><a href="' + secondStr + 'share_dll_download.html">共享DLL下载</a></li>\
                <li><a href="' + secondStr + 'csharp_code_download.html">Helper代码</a></li>\
                <li><a href="' + secondStr + 'csharp_find_new.html">新发现</a></li>\
                <li><a href="' + secondStr + 'recommend_and_share.html">关注前言</a></li>\
                <li><a href="' + secondStr + 'csharp_api.html">API接口</a></li>\
                <li><a href="' + secondStr + 'seg_and_message.html">建议&留言</a></li>\
                <li><a href="' + secondStr + 'about_and_thanks.html">关于&帮助</a></li>\
        </ol>\
        <ol class="breadcrumb">\
            <li>友情链接：</li>\
            <li><a href="http://www.cnblogs.com/">博客园</a></li>\
            <li><a href="http://code.taobao.org/">TaoCode</a></li>\
            <li><a href="http://fanyi.baidu.com/">百度翻译</a></li>\
            <li><a href="http://www.5lulu.com/tec/Online_js.html">在线js格式化</a></li>\
            <li><a href="http://www.csdn.net/">CSDN</a></li>\
        </ol>\
    </div>';

    $("#sitemap").html(sitemap);

    var fotter = "<p class='text-center'>Copy right by supperlitt&nbsp;&nbsp;<a href='" + thirdStr + "login.html'>Go</a></p>";
    $("#fotter").html(fotter);
});

var _hmt = _hmt || [];
(function () {
    var hm = document.createElement("script");
    hm.src = "//hm.baidu.com/hm.js?9b7477d6d8a88766201e8848cae16448";
    var s = document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(hm, s);
})();