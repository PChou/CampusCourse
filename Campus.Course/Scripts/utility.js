/// <reference path="libs/jquery-1.8.3.js" />


function DateDiff(beginTime, endTime, excludeWeekend, holidays) {
    /// <summary>计算两个时间相差的天数</summary>
    /// <param name="beginTime" type="string">开始时间[yyyy-MM-dd]</param>
    /// <param name="endTime" type="string">结束时间[yyyy-MM-dd]</param>
    /// <param name="excludeWeekend" type="bool">排除周末</param>
    /// <param name="holidays" type="Array">节假日列表</param>
    /// <returns type="int" />

    var dateSplit, _sDate, _begin, _end, Days;
    dateSplit = beginTime.split("-");
    _sDate = dateSplit[0] + '/' + dateSplit[1] + '/' + dateSplit[2];
    _begin = new Date(_sDate);
    dateSplit = endTime.split("-");
    _end = new Date(dateSplit[0] + '/' + dateSplit[1] + '/' + dateSplit[2]);
    Days = parseInt((_end - _begin) / 1000 / 60 / 60 / 24);

    if (Days >= 0) {

        var dayAllLength = Days + 1;

        if (!excludeWeekend) return dayAllLength;

        var weekendCount = 0, weekend;
        var holidayCount = 0, workdayCount = 0;

        for (var i = 1; i <= dayAllLength; i++) {
            weekend = new Date(_sDate).getDay();
            if (weekend == 0 || weekend == 6) {
                weekendCount++;
            }

            var _day = new Date(_sDate).valueOf();

            //后面一天
            _day = _day + (24 * 60 * 60 * 1000);

            _day = new Date(_day);

            _sDate = _day.getFullYear() + '/' + (_day.getMonth() + 1) + '/' + _day.getDate();
        }

        for (var i = 0; i < holidays.length; i++) {
            if (holidays[i].Type == 0) {
                workdayCount++;
            } else {
                holidayCount++;
            }
        }

        return dayAllLength - weekendCount - holidayCount + workdayCount;
    }

    return -1;

}

function nextDate(today) {
    var dateSplit = today.split("-");
    var _sDate = dateSplit[0] + '/' + dateSplit[1] + '/' + dateSplit[2];
    var _day = new Date(_sDate).valueOf();
    _day = _day + (24 * 60 * 60 * 1000);
    _day = new Date(_day);
    return _day.getFullYear() + '/' + (_day.getMonth() + 1) + '/' + _day.getDate();
}

function prevDate(today) {
    var dateSplit = today.split("-");
    var _sDate = dateSplit[0] + '/' + dateSplit[1] + '/' + dateSplit[2];
    var _day = new Date(_sDate).valueOf();
    _day = _day - (24 * 60 * 60 * 1000);
    _day = new Date(_day);
    return _day.getFullYear() + '/' + (_day.getMonth() + 1) + '/' + _day.getDate();
}

function isWeekend(day, holidays) {
    var dateSplit = day.split("-");
    var _sDate = dateSplit[0] + '/' + dateSplit[1] + '/' + dateSplit[2];
    var date = new Date(_sDate);
    var weekend = date.getDay();

    var isWeekend = weekend == 0 || weekend == 6;

    for (var i = 0; i < holidays.length; i++) {
        if (holidays[i].Term.getTime() == date.getTime()) {
            if (holidays[i].Type == 0) return false;
        }
    }
    return isWeekend;
}

function isHoliday(day, holidays) {
    var dateSplit = day.split("-");
    var _sDate = dateSplit[0] + '/' + dateSplit[1] + '/' + dateSplit[2];
    var date = new Date(_sDate);

    for (var i = 0; i < holidays.length; i++) {
        if (holidays[i].Term.getTime() == date.getTime()) {
            return holidays[i].Type == 1 || holidays[i].Type == 2;
        }
    }
}

//获取指定年月的天数
function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

//开始时间小于结束时间的检测
//计算得到总长度和结算长度
function CheckDateGroup(group) {

    var calendars = $('[data-calendar-group="' + group + '"]');

    var beginTime = calendars.filter('[data-calendar-role="begin"]').val(),
            endTime = calendars.filter('[data-calendar-role="end"]').val();

    var beginLengthSelect = $('#ddlLeaveBeginLength'),
            endLengthSelect = $('#ddlLeaveEndLength');

    if (beginTime != "" && endTime != "") {

        //开始时间=结束时间时，要限定结束长度的选择
        if ($.trim(beginTime) == $.trim(endTime)) {
            var beginLength = beginLengthSelect.val();
            endLengthSelect.val(beginLength).attr('disabled', 'disabled');
            $('#hfLeaveEndLength').val(beginLength);
            $('#hfLeaveEndLengthStatus').val('disabled');
        } else {
            endLengthSelect.removeAttr('disabled');
            $('#hfLeaveEndLength').val(endLengthSelect.val());
            $('#hfLeaveEndLengthStatus').val('');
        }

        var allLength = DateDiff(beginTime, endTime, false);

        var isWorkingDays = $('#txtVacationDayType').val() == "" || $('#txtVacationDayType').val() == "工作日";

        var holidays = GetCaledarsFromTo(beginTime, endTime);
        var settlementLength = DateDiff(beginTime, endTime, isWorkingDays, holidays);

        //工作日半天假期
        if (+beginLengthSelect.val() == 4) {
            allLength -= 0.5;
            if (!isWeekend(beginTime, holidays) && !isHoliday(beginTime, holidays)) settlementLength -= 0.5;
        }

        //工作日半天假期
        if (+endLengthSelect.val() == 4) {
            if (!endLengthSelect.is(':disabled')) {
                allLength -= 0.5;
                if (!isWeekend(endTime, holidays) && !isHoliday(beginTime, holidays)) settlementLength -= 0.5;
            }
        }

        if (allLength < 0) {
            alert('结束时间不能早于开始时间！');
            calendars.filter('[data-calendar-role="end"]').val('');
            $('#txtLeaveAllTimeLength').val('');
            $('#hfLeaveAllTimeLength').val('')
            $('#txtLeaveTimeLength').val('');
            $('#hfLeaveTimeLength').val('');
        } else {
            $('#txtLeaveAllTimeLength').val(allLength);
            $('#hfLeaveAllTimeLength').val(allLength);
            $('#txtLeaveTimeLength').val(settlementLength);
            $('#hfLeaveTimeLength').val(settlementLength);
        }
    }
};

//页面必填选项的检查
function ClientValidate(button) {
    var container = $(button).parents('body');
    var errorMsg = "";
    container.find('span[data-required-info]').each(function () {
        if ($(this).is(':visible')) {
            var input = $(this).parents('.parameter_name').next('.parameter_input'),
            inputEle = input.find('input');
            if (inputEle.length == 0) inputEle = input.find('select');

            if (inputEle.attr('type') == 'radio') {
                var groupName = inputEle.attr('name');
                if ($("input[type=radio][name=" + groupName + "]:checked").length == 0) {
                    errorMsg += "<span>" + $(this).attr('data-required-info') + "</br></span>";
                }
            }
            else if (inputEle.attr('id') == "txtEmergencyTel") {
                if (inputEle.val() == "") {
                    errorMsg += "<span>" + $(this).attr('data-required-info') + "</br></span>";
                }
                //else if (!CheckPhoneTel(inputEle)) {
                //    errorMsg += "<span>紧急联系电话格式不正确!</br></span>";
                //}
            } else if (inputEle.val() == "" || inputEle.val() == null) {
                errorMsg += "<span>" + $(this).attr('data-required-info') + "</br></span>";
            }
        }
    });


    //if (container.find('#txtTemporaryAgencyTel').length) {
    //    if ($('#txtTemporaryAgencyTel').val() != "") {
    //        if (!CheckExtensionTel($('#txtTemporaryAgencyTel'))) errorMsg += "<span>请输入4位分机号!</span>"
    //    }
    //}

    showPageError(container, errorMsg);

    return errorMsg == "";
}

function showPageError(container, errorMsg) {
    var closeButton = "<a class='block_button fr closeError'>关闭错误信息</a>";
    if (errorMsg != "") {
        container.find(".errorField").html('').append(closeButton).append(errorMsg).show();
        container.find('.margin_1140_center').css({ top: container.find(".errorField").height() + 10 + 'px' });
        $(document).scrollTop(0);

        container.find(".errorField .closeError").bind('click', function () {
            container.find(".errorField").hide();
            container.find('.margin_1140_center').css({ top: '0px' });
        });
    } else {
        container.find(".errorField").hide();
        container.find('.margin_1140_center').css({ top: '0px' });
    }
}

//只允许输入数字
function focusNumberInput(element) {
    var keycode = event.keyCode;

    if (keycode > 57 || keycode < 47) {
        if (keycode == 45)
            return true;
        return false;
    }
}

function focusDecimalInput(element) {
    var keycode = event.keyCode;

    if (keycode > 57 || keycode < 47) {
        if (keycode == 46)
            return true;
        return false;
    }
}

function ForbiddenChinese(element) {
    var value = $(element).val();
    if (value) {
        $(element).val(value.replace(/[^\w\.\-\/]/ig, ''));
    }
    return true;
}

//检查符合手机和电话格式
function CheckPhoneTel(ele) {
    var tel = $(ele).val();
    var regex_mobile = /^1\d{10}$/,
        regex_tel = new RegExp("^(0[0-9]{2,3}\-)([2-9][0-9]{6,7})$");
    if (tel == "") {
        args.IsValid = false;
    } else {
        if (regex_mobile.test(tel) || regex_tel.test(tel)) {
            return true;
        }
        return false;
    }
}

//检查4位分机号
function CheckExtensionTel(ele) {
    var tel = $(ele).val();

    var regex_tel = /^\d{4}$/;
    if (regex_tel.test(tel)) {
        return true;
    }
    return false;
}

function EmailValCheck(val) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    return reg.test(val);
}

//限定输入区域的最大输入长度
function checkTextAreaMaxLength(textBox, e, length) {

    var mLen = textBox["MaxLength"];
    if (null == mLen)
        mLen = length;

    var maxLength = parseInt(mLen);
    if (!checkSpecialKeys(e)) {
        if (textBox.value.length > maxLength - 1) {
            if (window.event)//IE
            {
                e.returnValue = false;
                return false;
            }
            else//Firefox
                e.preventDefault();
        }
    }
}

function checkSpecialKeys(e) {
    if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
        return false;
    else
        return true;
}

//关闭本页面并刷新父页面
function RefreshParentAndCloseSelf() {
    if (window.opener != null) {
        window.opener.location.reload();
    }

    window.open('', '_self', '');
    window.close();
}
//获取休假日历列表
function GetCalendarList(callback) {
    $.ajax({
        url: "/UserControls/Services/CalendarAjax.ashx?category=GetCalendarList",
        dataType: "json",
        success: function (res) {
            if (res) {
                //将/Date(1245398693390)/转化成Date类型
                for (var i = 0; i < res.length; i++) {
                    var date = res[i]["Term"];
                    var re = /-?\d+/;
                    var m = re.exec(date);
                    var d = new Date(parseInt(m[0]));
                    res[i]["Term"] = d;
                }
            }
            callback(res);
        }
    });
}

//获取从from到to的节假日安排
//非异步请求
function GetCaledarsFromTo(from, to) {
    var calendars;
    $.ajax({
        url: "/UserControls/Services/CalendarAjax.ashx?category=GetCaledarsFromTo&from=" + from + "&to=" + to,
        dataType: "json",
        async: false,
        success: function (res) {
            if (res) {
                //将/Date(1245398693390)/转化成Date类型
                for (var i = 0; i < res.length; i++) {
                    var date = res[i]["Term"];
                    var re = /-?\d+/;
                    var m = re.exec(date);
                    var d = new Date(parseInt(m[0]));
                    res[i]["Term"] = d;
                }
            }
            calendars = res;
        }
    });
    return calendars;
}

//检查下一年的休假安排是否已经存在于数据库
function CheckHasCalendarOfNextYear(callback) {
    $.ajax({
        url: "/UserControls/Services/CalendarAjax.ashx?category=CheckNextYearCalendar",
        success: function (res) {
            callback(res);
        }
    });
}

//获取指定年份的假期安排
function GetYearCalendar(year, callback) {
    $.ajax({
        url: "/UserControls/Services/CalendarAjax.ashx?category=GetYearCalendar&year=" + year,
        success: function (res) {
            if (res) {
                //将/Date(1245398693390)/转化成Date类型
                for (var i = 0; i < res.length; i++) {
                    var date = res[i]["Term"];
                    var re = /-?\d+/;
                    var m = re.exec(date);
                    var d = new Date(parseInt(m[0]));
                    res[i]["Term"] = d;
                }
            }
            callback(res);
        }
    });
}

//禁用页面按钮
function disableButtons(element) {
    var container = $(element).parents('.operate_buttons');
    container.find('input[type="button"]').each(function (index, ele) {
        $(ele).attr('disabled', 'disabled');
    });
}

//打开输入拒绝意见页面
function OpenRejectComment(ele, fromPage) {
    var item = $(ele).parents('tr');
    var appid = item.find('#hfApplicationID').val();
    var taskid = item.find('#hfTaskId').val();
    var actorID = $('#hfCurrentUserID').val();
    window.showModalDialog("/Pages/RejectComment.aspx?FromPage=" + fromPage + "&ActorID=" + actorID + "&AppID=" + appid + '&TaskID=' + taskid, window);
}

//修改左侧菜单栏我的待办个数
function ChangeMyPendingCount(count) {
    if (count == 0) {
        $(window.parent.document).find('#lbNum').text('');
    }
    if (count > 0) {
        $(window.parent.document).find('#lbNum').text('(' + count + ')');
    }

}

//点击左边指点的菜单项
function SelectLeftMenu(targetHref) {
    if (targetHref) {
        $('.menu_options').children('li').removeClass('active');
        $('.menu_options').children('li[data-search-for="' + targetHref + '"]').addClass('active');
        $('iframe[name="right"]').attr("src", targetHref);
        $('.searchButton').attr('data-target-page', targetHref);
    }
}

//点击头部指定的tab项
function SelectTopTab(targetTab) {
    if (targetTab) {
        var text = $('.form_options').children('li[data-form-target="' + targetTab + '"]').text();
        $("#hfMenu").val($.trim(text));
    }
}

function idCardUpdate(_str) {
    //转换为18位身份证号

    var idCard18;
    var regIDCard15 = /^(\d){15}$/;
    if (regIDCard15.test(_str)) {
        var nTemp = 0;
        var ArrInt = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8,
				4, 2);
        var ArrCh = new Array('1', '0', 'X', '9', '8', '7', '6', '5', '4', '3',
				'2');
        _str = _str.substr(0, 6) + '1' + '9' + _str.substr(6, _str.length - 6);
        for (var i = 0; i < _str.length; i++) {
            nTemp += parseInt(_str.substr(i, 1)) * ArrInt[i];
        }
        _str += ArrCh[nTemp % 11];
        idCard18 = _str;
    } else {
        idCard18 = "#";
    }
    return idCard18;
}

//验证身份证
function validateFirIdCard(value) {
    var iSum = 0;
    var info = "";
    var sId;
    var aCity = {
        11: "北京",
        12: "天津",
        13: "河北",
        14: "山西",
        15: "内蒙",
        21: "辽宁",
        22: "吉林",
        23: "黑龙",
        31: "上海",
        32: "江苏",
        33: "浙江",
        34: "安徽",
        35: "福建",
        36: "江西",
        37: "山东",
        41: "河南",
        42: "湖北",
        43: "湖南",
        44: "广东",
        45: "广西",
        46: "海南",
        50: "重庆",
        51: "四川",
        52: "贵州",
        53: "云南",
        54: "西藏",
        61: "陕西",
        62: "甘肃",
        63: "青海",
        64: "宁夏",
        65: "新疆",
        71: "台湾",
        81: "香港",
        82: "澳门",
        91: "国外"
    };
    //如果输入的为15位数字,则先转换为18位身份证号
    if (value.length == 15)
        sId = idCardUpdate(value);
    else
        sId = value;
    if (!/^\d{17}(\d|x)$/i.test(sId)) {
        return false;
    }
    sId = sId.replace(/x$/i, "a");
    //非法地区
    if (aCity[parseInt(sId.substr(0, 2))] == null) {
        return false;
    }
    var sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2))
			+ "-" + Number(sId.substr(12, 2));
    var d = new Date(sBirthday.replace(/-/g, "/"));
    //非法生日
    if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d
			.getDate())) {
        return false;
    }
    for (var i = 17; i >= 0; i--) {
        iSum += (Math.pow(2, i) % 11) * parseInt(sId.charAt(17 - i), 11);
    }
    if (iSum % 11 != 1) {
        return false;
    }
    return true;
}

function GetBirthdayFromIDCard(value) {
    if (validateFirIdCard(value)) {
        var sId = '';
        if (value.length == 15)
            sId = idCardUpdate(value);
        else
            sId = value;
        sId = sId.replace(/x$/i, "a");
        var sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2))
			+ "-" + Number(sId.substr(12, 2));
        var d = new Date(sBirthday.replace(/-/g, "/"));

        return d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
    } else {
        return "";
    }
}

function GetGenderFromIDCard(value) {
    if (validateFirIdCard(value)) {

        var card = idCardUpdate(value);
        if (parseInt(value.charAt(16)) % 2) return "男";
        return "女";

    } else {
        return "";
    }
}

//验证号码
function ValidatePhoneNum(num) {
    var reg = /^([0-9]|[-]){0,29}$/;
    return reg.test(num);
}

function ValidateNumber(num) {
    var reg = /^([0-9]){0,29}$/;
    return reg.test(num);
}

//批量操作时对多选框的判断
function CheckPassCheckBox(tableID) {
    var container = $('#' + tableID),
        checkboxs = container.find('.pending_list_item input[type="checkbox"]'),
        checked = container.find('.pending_list_item input[type="checkbox"]:checked');

    if (checkboxs.length != 0 && checked.length == 0) alert("请勾选审批数据!");

    return checked.length > 0;
}

function confirmOver60(days, message) {
    /// <summary>病事假超过60天弹出确认框</summary>
    var type = $.trim($('#ddlVacationType option:selected').text());
    var bsjList = ["有薪病假", "扣薪病假", "事假"];
    if ($.inArray(type, bsjList) >= 0) {
        if ((+days) + (+$('#txtLeaveTimeLength').val()) * 8 > 60 * 8) {
            return confirm(message);
        }
    }
    return true;
}

//Compare date if date1 > date2 return true
function CompareTime(date1, date2) {
    var date1_year = date1.substring(0, 4);
    var date1_month = date1.substring(5, 7) - 1;
    var date1_day = date1.substring(8, 10);
    var time1 = new Date(date1_year, date1_month, date1_day);
    var date2_year = date2.substring(0, 4);
    var date2_month = date2.substring(5, 7) - 1;
    var date2_day = date2.substring(8, 10);
    var time2 = new Date(date2_year, date2_month, date2_day);
    return time1 > time2;
}

//forbidde Enter key press
function Forbidden() {
    var keycode = event.keyCode;
    if (keycode == 13) {
        return false;
    }
}

function marqueeMove(containerID, contentID, copyContentID,speed2) {
    //图片滚动
    var speed = 20;
    if (speed2 != undefined && typeof speed2 == "number")
        speed = speed2;
    var container = document.getElementById(containerID);
    var content = document.getElementById(contentID);
    var copyContent = document.getElementById(copyContentID);
    //if need scroll
    if (copyContent.offsetTop > container.clientHeight) {
        copyContent.innerHTML = content.innerHTML;

        function boxTop() {
            if (copyContent.offsetTop - container.scrollTop <= 0) {
                container.scrollTop -= content.offsetHeight
            }
            else { container.scrollTop++ }
        }
        //function boxRight() {
        //    if (container.scrollLeft <= 0) { container.scrollLeft += copyContent.offsetWidth }
        //    else { container.scrollLeft-- }
        //}
        //function boxBottom() {
        //    if (content.offsetTop - container.scrollTop >= 0) { container.scrollTop += copyContent.offsetHeight }
        //    else { container.scrollTop-- }
        //}
        //function boxLeft() {
        //    if (copyContent.offsetWidth - container.scrollLeft <= 0) { container.scrollLeft -= content.offsetWidth }
        //    else { container.scrollLeft++ }
        //}

        var MoveTop = setInterval(boxTop, speed);
        container.onmouseover = function () { clearInterval(MoveTop); };
        container.onmouseout = function () { MoveTop = setInterval(boxTop, speed) };
    }

}

//prototype extend
if (!Array.prototype.filter) {
    Array.prototype.filter = function (fun /*, thisp*/) {
        "use strict";

        if (this == null)
            throw new TypeError();

        var t = Object(this);
        var len = t.length >>> 0;
        if (typeof fun != "function")
            throw new TypeError();

        var res = [];
        var thisp = arguments[1];
        for (var i = 0; i < len; i++) {
            if (i in t) {
                var val = t[i]; // in case fun mutates this
                if (fun.call(thisp, val, i, t))
                    res.push(val);
            }
        }

        return res;
    };
}