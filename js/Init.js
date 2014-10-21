function setNavigation() {
    $.ajax({
        type: "POST",
        async: false,
        url: '/OnlineBooking/default.aspx/updateMenu',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var json = data.d;
            switch (json) {
                case '4':
                    $('#client').removeClass('hidden');
                    $('#worker').addClass('hidden');
                    $('#admin').addClass('hidden');
                    $('#ctl00_btnLogin').addClass('hidden');
                    $('#btnLogout').removeClass('hidden');
                    break;
                case '3':
                    $('#client').addClass('hidden');
                    $('#worker').removeClass('hidden');
                    $('#admin').addClass('hidden');
                    $('#ctl00_btnLogin').addClass('hidden');
                    $('#btnLogout').removeClass('hidden');
                    break;
                case '2':
                    $('#client').addClass('hidden');
                    $('#worker').removeClass('hidden');
                    $('#admin').removeClass('hidden');
                    $('#ctl00_btnLogin').addClass('hidden');
                    $('#btnLogout').removeClass('hidden');
                    break;
                case '1':
                    $('#client').removeClass('hidden');
                    $('#worker').removeClass('hidden');
                    $('#admin').removeClass('hidden');
                    $('#ctl00_btnLogin').addClass('hidden');
                    $('#btnLogout').removeClass('hidden');
                    break;
                default:
                    $('#client').addClass('hidden');
                    $('#worker').addClass('hidden');
                    $('#admin').addClass('hidden');
                    $('#ctl00_btnLogin').removeClass('hidden');
                    $('#btnLogout').addClass('hidden');
            }
        }
    });
}

function jsonDateFormat(jsonDt) {
    var jdate = new Date(parseInt(jsonDt.substr(6)));
    var m_names = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    var curr_date = jdate.getDate();
    var curr_month = jdate.getMonth();
    var curr_year = jdate.getFullYear();
    return (curr_date + "-" + m_names[curr_month]
+ "-" + curr_year);
}

function stringDateFormat(date) {
    var jdate = new Date(date);
    var m_names = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    var curr_date = jdate.getDate();
    var curr_month = jdate.getMonth();
    var curr_year = jdate.getFullYear();
    return (curr_date + "-" + m_names[curr_month]
+ "-" + curr_year);
}

function dateFormat(date) {
    var m_names = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    var curr_date = date.getDate();
    var curr_month = date.getMonth();
    var curr_year = date.getFullYear();
    return (curr_date + "-" + m_names[curr_month]
+ "-" + curr_year);
}

function jsonTimeFormat(jsonDt) {
    var jdate = new Date(parseInt(jsonDt.substr(6)));
    var a_p = "";
    var curr_hour = jdate.getHours();
    if (curr_hour < 12) {
        a_p = "AM";
    }
    else {
        a_p = "PM";
    }
    if (curr_hour == 0) {
        curr_hour = 12;
    }
    if (curr_hour > 12) {
        curr_hour = curr_hour - 12;
    }
    var curr_min = jdate.getMinutes();
    if (curr_min < 10) { curr_min = '0' + curr_min; }
    return (curr_hour + " : " + curr_min + " " + a_p);
}

function stringTimeFormat(date) {
    var jdate = new Date(date);
    var a_p = "";
    var curr_hour = jdate.getHours();
    if (curr_hour < 12) {
        a_p = "AM";
    }
    else {
        a_p = "PM";
    }
    if (curr_hour == 0) {
        curr_hour = 12;
    }
    if (curr_hour > 12) {
        curr_hour = curr_hour - 12;
    }
    var curr_min = jdate.getMinutes();
    if (curr_min < 10) { curr_min = '0' + curr_min; }
    return (curr_hour + " : " + curr_min + " " + a_p);
}

function timeFormat(date) {
    var a_p = "";
    var curr_hour = date.getHours();
    if (curr_hour < 12) {
        a_p = "AM";
    }
    else {
        a_p = "PM";
    }
    if (curr_hour == 0) {
        curr_hour = 12;
    }
    if (curr_hour > 12) {
        curr_hour = curr_hour - 12;
    }
    var curr_min = date.getMinutes();
    if (curr_min < 10) { curr_min = '0' + curr_min; }
    return (curr_hour + " : " + curr_min + " " + a_p);
}

function getWeekDay(day) {
    var weekDay = '';
    switch (day) {
        case 1:
            weekDay = 'Monday'; break;
        case 2:
            weekDay = 'Tuesday'; break;
        case 3:
            weekDay = 'Wednesday'; break;
        case 4:
            weekDay = 'Thursday'; break;
        case 5:
            weekDay = 'Friday'; break;
        case 6:
            weekDay = 'Saturday'; break;
        case 7:
            weekDay = 'Sunday'; break;
    }
    return weekDay;
}


