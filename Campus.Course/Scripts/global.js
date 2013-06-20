/// <reference path="utility.js" />
$(function () {

    //头部menuBar的点击效果
    $(".menuBar ul >li.normal").bind("mouseover", function () {
        $(this).find('ol.subMenu').show();
    });
    $(".menuBar ul >li.normal").bind("mouseout", function () {
        $(this).find('ol.subMenu').hide();
    });


    //注册日期控件
    $('input.calendar:not([data-calendar-group])').die('click').live('click', function () {
        WdatePicker({
            el: $(this).attr('id')
        });
    });

    $('input.calendar.limitThisYear.startTime').die('click').live('click', function () {
        var now = new Date(), maxDate = now.getFullYear() + '-12-31';
        var that = this;
        CheckHasCalendarOfNextYear(function (res) {
            if (res) {
                if (res.toLowerCase() == "true") {//下一年的放假安排已经出炉
                    maxDate = now.getFullYear() + 1 + '-12-31';
                }
                WdatePicker({
                    el: $(that).attr('id'),
                    minDate: now.getFullYear() + '-' + (now.getMonth() + 1) + '-' + now.getDate(),
                    maxDate: maxDate,
                    startDate: now.getFullYear() + '-' + (now.getMonth() + 1) + '-' + now.getDate()
                });
            }
        });
    });

    $('input.calendar.limitThisYear.endTime').die('click').live('click', function () {
        var group = $(this).attr('data-calendar-group'),
            startTime = $('input.calendar[data-calendar-group="' + group + '"]').val();

        var leaveType = $('#ddlVacationType option:selected').text();
        var now = new Date();
        if (startTime) {
            var startYear = startTime.split('-')[0];
            var startMonth = startTime.split('-')[1];
            var minDate = startTime;
            var maxDate = startYear + '-12-31';
            if (leaveType == "哺乳假") {
                maxDate = startYear + '-' + startMonth + '-' + daysInMonth(startMonth, startYear);
            }
            WdatePicker({
                el: $(this).attr('id'),
                minDate: minDate,
                maxDate: maxDate,
                startDate: minDate
            });
        }
    });

    $('input.calendar.startTime:not(.limitThisYear)').die('click').live('click', function () {
        var that = this;
        WdatePicker({
            el: $(that).attr('id')
        });
    });

    $('input.calendar.endTime:not(.limitThisYear)').die('click').live('click', function () {
        var group = $(this).attr('data-calendar-group'),
            startTime = $('input.calendar[data-calendar-group="' + group + '"]').val();

        WdatePicker({
            el: $(this).attr('id'),
            minDate: startTime,
            startDate: startTime
        });
    });


    $('.WdayTable td').die('click').live('click', function () {
        $(':focus').blur();
    });


    //我的成员树桩结构的展开与关闭
    $('.employee_content').bind('click', function () {
        var parent = $(this).parent('li');
        var child_emplyee = parent.children('ul');
        var content = parent.children('.employee_content');
        var arrow_icon = content.children('.icon_arrow_small_bottom_algin');
        arrow_icon.toggleClass('icon_arrow_down_small').toggleClass('icon_arrow_right_small');
        if (parent.hasClass('first_level')) {
            arrow_icon = content.children('.icon_arrow_big_bottom_algin');
            arrow_icon.toggleClass('icon_arrow_down_big').toggleClass('icon_arrow_right_big');
        }

        child_emplyee.toggle();

    });


    //我的成员树桩结构双击打开信息
    $('.employee_content').bind('dblclick', function () {
        $('.employee_content').removeClass('active');
        $(this).addClass('active');
    });

    //左边菜单栏点击效果,
    //并确定下面是否有搜索选项
    //以及搜索的上下文
    $('.menu_options>li').die('click').live('click', function () {
        var container = $(this).parents('.left_menu');
        $(this).addClass('active').siblings().removeClass('active');

        //搜索选项的可见性
        var targetPage = $(this).attr('data-search-for');
        var searchBox = container.find('.left_menu_search_options');

        searchBox.hide();
        searchBox.each(function () {
            if ($.inArray(targetPage, $(this).attr('data-search-for').split(',')) >= 0) {
                $(this).show();
                $(this).find('.searchButton').attr('data-target-page', targetPage);
                return;
            }
        });
    });

    //开始搜索
    $('.left_menu_search_options a.searchButton').die('click').live('click', function () {

        var searchBox = $(this).parents('.left_menu_search_options'),
            targetPage = $(this).attr('data-target-page'),
            requestUrl = targetPage + '?Action=Search&';

        searchBox.find('[data-parameter]').each(function () {
            requestUrl += $(this).attr('data-parameter') + '=' + $(this).val() + '&';
        });

        $(this).attr('href', requestUrl);
    });

    //信息查询之后的分tab显示切换
    $('.form_options>li').live('click', function () {
        var container = $(this).parents('.right_content');
        $(this).addClass('active').siblings().removeClass('active');
        var target_form_id = $(this).attr('data-form-target');

        container.find('.form_main_content').hide();
        $('#' + target_form_id).find('table.table_form').first().show();
        $('#' + target_form_id).show();

        $('table[data-resize="true"]').each(function () {
            if ($(this).is(':visible')) {
                $(this).colResizable({
                    liveDrag: true
                });
            }
        });
    });

});






