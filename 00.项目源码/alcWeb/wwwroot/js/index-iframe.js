layui.config({
    base: "Scripts/"
});

layui.use(['form', 'element', 'layer', 'jquery'], function () {
    var form = layui.form;
    var element = layui.element;
    var $ = layui.$;
    var layer = layui.layer;
    var tabFilter = 'bodyTab';//添加窗口的filter
    var openTabNum = 10;//最大可打开窗口数量
    var changeRefreshStr = window.sessionStorage.getItem("changeRefresh");
    var menu = [], curNav, delMenu;
    //获取左侧菜单
    getData("");
    function getData(json) {
        $.getJSON('/Admin/AdminMenu/GetMenu', function (data) {
            var dataStr = getNavBar(data);
            $(".navBar ul").append(dataStr).height($(window).height() - 210);

            //重新渲染左侧菜单
            element.render('nav');
            $(window).resize(function () {
                $(".navBar").height($(window).height() - 210);
            })
        })
    }
    //生成左侧菜单
    function getNavBar(data) {
        debugger
        var ulHtml = '';
        for (var i = 0; i < data.length; i++) {
            if (data[i].spread || data[i].spread == undefined) {
                ulHtml += '<li class="layui-nav-item layui-nav-itemed">';
            } else {
                ulHtml += '<li class="layui-nav-item">';
            }
            if (data[i].children != undefined && data[i].children.length > 0) {
                ulHtml += '<a>';
                if (data[i].icon != undefined && data[i].icon != '') {
                    if (data[i].icon.indexOf("icon-") != -1) {
                        ulHtml += '<i class="seraph ' + data[i].icon + '" data-icon="' + data[i].icon + '"></i>';
                    } else {
                        ulHtml += '<i class="layui-icon iconfont" data-icon="' + data[i].icon + '">' + data[i].icon + '</i>';
                    }
                }
                ulHtml += '<cite>' + data[i].title + '</cite>';
                ulHtml += '<span class="layui-nav-more"></span>';
                ulHtml += '</a>';
                ulHtml += '<dl class="layui-nav-child">';
                //for (var j = 0; j < data[i].children.length; j++) {
                //    if (data[i].children[j].target == "_blank") {
                //        ulHtml += '<dd><a data-url="' + data[i].children[j].href + '" target="' + data[i].children[j].target + '">';
                //    } else {
                //        ulHtml += '<dd><a data-url="' + data[i].children[j].href + '">';
                //    }
                //    if (data[i].children[j].icon != undefined && data[i].children[j].icon != '') {
                //        if (data[i].children[j].icon.indexOf("icon-") != -1) {
                //            ulHtml += '<i class="seraph ' + data[i].children[j].icon + '" data-icon="' + data[i].children[j].icon + '"></i>';
                //        } else {
                //            ulHtml += '<i class="layui-icon" data-icon="' + data[i].children[j].icon + '">' + data[i].children[j].icon + '</i>';
                //        }
                //    }
                //    ulHtml += '<cite>' + data[i].children[j].title + '</cite>';/*</a></dd>';*/
                //    ulHtml += '<span class="layui-nav-more"></span>';
                //    ulHtml += '</a>';
                //    ulHtml += '</dd >';
                //}
                ulHtml += getChild(data[i].children);
                ulHtml += "</dl>";
            } else {
                if (data[i].target == "_blank") {
                    ulHtml += '<a data-url="' + data[i].href + '" target="' + data[i].target + '">';
                } else {
                    ulHtml += '<a data-url="' + data[i].href + '">';
                }
                if (data[i].icon != undefined && data[i].icon != '') {
                    if (data[i].icon.indexOf("icon-") != -1) {
                        ulHtml += '<i class="seraph ' + data[i].icon + '" data-icon="' + data[i].icon + '"></i>';
                    } else {
                        ulHtml += '<i class="layui-icon" data-icon="' + data[i].icon + '">' + data[i].icon + '</i>';
                    }
                }
                ulHtml += '<cite>' + data[i].title + '</cite></a>';
            }
            ulHtml += '</li>';
        }
        return ulHtml;
    }
    
    //生成无限级子菜单
    function getChild(parent) {
        var ulHtml = '';
        if (parent != undefined && parent.length > 0) {
            for (var i = 0; i < parent.length; i++){
                if (parent[i].target == "_blank") {
                    if (parent[i].children != undefined && parent[i].children.length > 0){
                        ulHtml += '<dd><a>';
                    } else {
                        ulHtml += '<dd><a data-url="' + parent[i].href + '" target="' + parent[i].target + '">';
                    }
                } else if (parent[i].href != '' && parent[i].href != undefined) {
                    ulHtml += '<dd><a data-url="' + parent[i].href + '">';
                } else {
                    ulHtml += '<dd><a>';
                }
                if (parent[i].icon != undefined && parent[i].icon != '') {
                    if (parent[i].icon.indexOf("icon-") != -1){
                        ulHtml += '<i class="seraph ' + parent[i].icon + '" data-icon="' + parent[i].icon + '"></i>';
                    } else {
                        ulHtml += '<i class="layui-icon" data-icon="' + parent[i].icon + '">' + parent[i].icon + '</i>';
                    }
                }
                ulHtml += '<cite>' + parent[i].title + '</cite>';
                if (parent[i].children != undefined && parent[i].children.length > 0) {
                    ulHtml += '<span class="layui-nav-more"></span></a>';
                    //继续生成子级菜单
                    ulHtml += '<dl class="layui-nav-child">';
                    ulHtml += getChild(parent[i].children);
                    ulHtml += "</dl>";
                } else {
                    ulHtml += '</a>';
                }
                ulHtml += '</dd >';
            }
        }
        return ulHtml;
    }


    var fix = true;
    //隐藏左侧导航
    $("#navBar").mouseover(function () {
        if($(".topLevelMenus li.layui-this a").data("url")){
        	layer.msg("此栏目状态下左侧菜单不可展开");  //主要为了避免左侧显示的内容与顶部菜单不匹配
        	return false;
        }
        if (fix == false) {
            $(".layui-layout-admin").removeClass("layadmin-side-shrink  qc-aside-hidden qc-aside-hover");
        }


        //渲染顶部窗口
        element.render();

    }).mouseout(function () {
        if ($(".topLevelMenus li.layui-this a").data("url")) {
            layer.msg("此栏目状态下左侧菜单不可展开");  //主要为了避免左侧显示的内容与顶部菜单不匹配
            return false;
        }

        if (fix == false) {
            $(".layui-layout-admin").addClass("layadmin-side-shrink  qc-aside-hidden qc-aside-hover");
        }


        //渲染顶部窗口
        element.render();
    });


    //隐藏左侧导航
    $(".hideMenu").click(function () {
        if ($(".topLevelMenus li.layui-this a").data("url")) {
            layer.msg("此栏目状态下左侧菜单不可展开");  //主要为了避免左侧显示的内容与顶部菜单不匹配
            return false;
        }
        $(".layui-layout-admin").toggleClass("layadmin-side-shrink  qc-aside-hidden qc-aside-hover");

        if (fix == true) {
            fix = false;
        }
        else {
            fix = true;
        }
        //渲染顶部窗口
        element.render();
    })

    ////底部按钮鼠标的移入移出  
    //$(".hideMenu22").mouseover(function () {
    //    $(".layui-layout-admin").removeClass("layadmin-side-shrink");

    //}).mouseout(function () {
    //    $(".layui-layout-admin").addClass("layadmin-side-shrink");

    //}); 

    // 添加新窗口
    $("body").on("click", ".layui-nav .layui-nav-item a:not('.mobileTopLevelMenus .layui-nav-item a')", function () {
        //如果不存在子级
        if ($(this).siblings().length == 0) {
            addTab($(this));
        }
        $(this).parent("li").siblings().removeClass("layui-nav-itemed");
    })


    //右侧内容tab操作
    var tabIdIndex = 0;
    function addTab(_this) {
        if (window.sessionStorage.getItem("menu")) {
            menu = JSON.parse(window.sessionStorage.getItem("menu"));
        }
        if (_this.attr("target") == "_blank") {
            window.open(_this.attr("data-url"));
        } else if (_this.attr("data-url") != undefined) {
            var title = '';
            if (_this.find("i.seraph,i.layui-icon").attr("data-icon") != undefined) {
                if (_this.find("i.seraph").attr("data-icon") != undefined) {
                    title += '<i class="seraph ' + _this.find("i.seraph").attr("data-icon") + '"></i>';
                } else {
                    title += '<i class="layui-icon">' + _this.find("i.layui-icon").attr("data-icon") + '</i>';
                }
            }
            //已打开的窗口中不存在
            if (hasTab(_this.find("cite").text()) == -1 && _this.siblings("dl.layui-nav-child").length == 0) {
                if ($(".layui-tab-title.top_tab li").length == openTabNum) {
                    layer.msg('只能同时打开' + openTabNum + '个选项卡哦。不然系统会卡的！');
                    return;
                }
                tabIdIndex++;
                title += '<cite>' + _this.find("cite").text() + '</cite>';
                title += '<i class="layui-icon layui-unselect layui-tab-close" data-id="' + tabIdIndex + '">&#x1006;</i>';
                element.tabAdd(tabFilter, {
                    title: title,
                    content: "<iframe src='" + _this.attr("data-url") + "' data-id='" + tabIdIndex + "'></iframe>",
                    id: new Date().getTime()
                });
                //当前窗口内容
                var curmenu = {
                    "icon": _this.find("i.seraph").attr("data-icon") != undefined ? _this.find("i.seraph").attr("data-icon") : _this.find("i.layui-icon").attr("data-icon"),
                    "title": _this.find("cite").text(),
                    "href": _this.attr("data-url"),
                    "layId": new Date().getTime()
                };
                menu.push(curmenu);
                window.sessionStorage.setItem("menu", JSON.stringify(menu)); //打开的窗口
                window.sessionStorage.setItem("curmenu", JSON.stringify(curmenu));  //当前的窗口
                element.tabChange(tabFilter, getLayId(_this.find("cite").text()));
                //tabMove(); //顶部窗口是否可滚动
            } else {
                //当前窗口内容
                var curmenu = {
                    "icon": _this.find("i.seraph").attr("data-icon") != undefined ? _this.find("i.seraph").attr("data-icon") : _this.find("i.layui-icon").attr("data-icon"),
                    "title": _this.find("cite").text(),
                    "href": _this.attr("data-url")
                };
                changeRegresh(_this.parent('.layui-nav-item').index());
                window.sessionStorage.setItem("curmenu", JSON.stringify(curmenu));  //当前的窗口
                element.tabChange(tabFilter, getLayId(_this.find("cite").text()));
                //tabMove(); //顶部窗口是否可滚动
            }
        }
    }

    //通过title获取lay-id
    function getLayId(title) {
        var layId;
        $(".layui-tab-title.top_tab li").each(function () {
            if ($(this).find("cite").text() == title) {
                layId = $(this).attr("lay-id");
            }
        })
        return layId;
    }

    //通过title判断tab是否存在
    function hasTab(title) {
        var tabIndex = -1;
        $(".layui-tab-title.top_tab li").each(function () {
            if ($(this).find("cite").text() == title) {
                tabIndex = 1;
            }
        })
        return tabIndex;
    }

    //是否点击窗口切换刷新页面
    function changeRegresh(index) {
        if (changeRefreshStr == "true") {
            $(".clildFrame .layui-tab-item").eq(index).find("iframe")[0].contentWindow.location.reload();
        }
    }

    //清除缓存
    $(".clearCache").click(function () {
        window.sessionStorage.clear();
        window.localStorage.clear();
        var index = layer.msg('清除缓存中，请稍候', { icon: 16, time: false, shade: 0.8 });
        setTimeout(function () {
            layer.close(index);
            layer.msg("缓存清除成功！");
        }, 1000);
    })

    //切换后获取当前窗口的内容
    $("body").on("click", ".top_tab li", function () {
        var curmenu = '';
        var menu = JSON.parse(window.sessionStorage.getItem("menu"));
        if (window.sessionStorage.getItem("menu")) {
            curmenu = menu[$(this).index() - 1];
        }
        if ($(this).index() == 0) {
            window.sessionStorage.setItem("curmenu", '');
        } else {
            window.sessionStorage.setItem("curmenu", JSON.stringify(curmenu));
            if (window.sessionStorage.getItem("curmenu") == "undefined") {
                //如果删除的不是当前选中的tab,则将curmenu设置成当前选中的tab
                if (curNav != JSON.stringify(delMenu)) {
                    window.sessionStorage.setItem("curmenu", curNav);
                } else {
                    window.sessionStorage.setItem("curmenu", JSON.stringify(menu[liIndex - 1]));
                }
            }
        }
        element.tabChange(tabFilter, $(this).attr("lay-id")).init();
        changeRegresh($(this).index());
        //setTimeout(function () {
        //    bodyTab.tabMove();
        //}, 100);
    })

    //删除tab
    $("body").on("click", ".top_tab li i.layui-tab-close", function () {
        //删除tab后重置session中的menu和curmenu
        liIndex = $(this).parent("li").index();
        var menu = JSON.parse(window.sessionStorage.getItem("menu"));
        if (menu != null) {
            //获取被删除元素
            delMenu = menu[liIndex - 1];
            var curmenu = window.sessionStorage.getItem("curmenu") == "undefined" ? undefined : window.sessionStorage.getItem("curmenu") == "" ? '' : JSON.parse(window.sessionStorage.getItem("curmenu"));
            if (JSON.stringify(curmenu) != JSON.stringify(menu[liIndex - 1])) {  //如果删除的不是当前选中的tab
                // window.sessionStorage.setItem("curmenu",JSON.stringify(curmenu));
                curNav = JSON.stringify(curmenu);
            } else {
                if ($(this).parent("li").length > liIndex) {
                    window.sessionStorage.setItem("curmenu", curmenu);
                    curNav = curmenu;
                } else {
                    window.sessionStorage.setItem("curmenu", JSON.stringify(menu[liIndex - 1]));
                    curNav = JSON.stringify(menu[liIndex - 1]);
                }
            }
            menu.splice((liIndex - 1), 1);
            window.sessionStorage.setItem("menu", JSON.stringify(menu));
        }
        element.tabDelete(tabFilter, $(this).parent("li").attr("lay-id")).init();

    })

    //刷新当前
    $(".refresh").on("click", function () {  //此处添加禁止连续点击刷新一是为了降低服务器压力，另外一个就是为了防止超快点击造成chrome本身的一些js文件的报错(不过貌似这个问题还是存在，不过概率小了很多)

        if ($(this).hasClass("refreshThis")) {
            $(this).removeClass("refreshThis");
            $(".clildFrame .layui-tab-item.layui-show").find("iframe")[0].contentWindow.location.reload();
            setTimeout(function () {
                $(".refresh").addClass("refreshThis");
            }, 2000)
        } else {
            layer.msg("您点击的速度超过了服务器的响应速度，还是等两秒再刷新吧！");
        }
    })

    //关闭其他
    $(".closePageOther").on("click", function () {
        if ($("#top_tabs li").length > 2 && $("#top_tabs li.layui-this cite").text() != "首页") {
            var menu = JSON.parse(window.sessionStorage.getItem("menu"));
            $("#top_tabs li").each(function () {
                if ($(this).attr("lay-id") != '' && !$(this).hasClass("layui-this")) {
                    element.tabDelete(tabFilter, $(this).attr("lay-id")).init();
                    //此处将当前窗口重新获取放入session，避免一个个删除来回循环造成的不必要工作量
                    for (var i = 0; i < menu.length; i++) {
                        if ($("#top_tabs li.layui-this cite").text() == menu[i].title) {
                            menu.splice(0, menu.length, menu[i]);
                            window.sessionStorage.setItem("menu", JSON.stringify(menu));
                        }
                    }
                }
            })
        } else if ($("#top_tabs li.layui-this cite").text() == "首页" && $("#top_tabs li").length > 1) {
            $("#top_tabs li").each(function () {
                if ($(this).attr("lay-id") != '' && !$(this).hasClass("layui-this")) {
                    element.tabDelete(tabFilter, $(this).attr("lay-id")).init();
                    window.sessionStorage.removeItem("menu");
                    menu = [];
                    window.sessionStorage.removeItem("curmenu");
                }
            })
        } else {
            layer.msg("没有可以关闭的窗口了@_@");
        }

    })
    //关闭全部
    $(".closePageAll").on("click", function () {
        if ($("#top_tabs li").length > 1) {
            $("#top_tabs li").each(function () {
                if ($(this).attr("lay-id") != '') {
                    element.tabDelete(tabFilter, $(this).attr("lay-id")).init();
                    window.sessionStorage.removeItem("menu");
                    menu = [];
                    window.sessionStorage.removeItem("curmenu");
                }
            })
        } else {
            layer.msg("没有可以关闭的窗口了@_@");
        }

    })

})


