
var Timesheet = (function () {

    var $ajaxCall = function (data, url, type, success, error) {
        $.ajax({
            type: type,
            async: true,
            url: url,
            data: data,
            success: success,
            error: error
        });

    }

    $.date = function (dateObject) {
        var d = new Date(dateObject);
        var day = d.getDate();
        var month = d.getMonth() + 1;
        var year = d.getFullYear();

        var hour = d.getHours();
        var min = d.getMinutes();
        var sec = d.getSeconds();
        if (day < 10) {
            day = "0" + day;
        }
        if (month < 10) {
            month = "0" + month;
        }
        var date = year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;

        return date;
    };

    var checkInClock = function () {
        $("#Timesheet_CheckIn").attr("disabled", "disabled");
        $("#Timesheet_CheckOut").removeAttr("disabled");
        $('.timer').countimer('start');
    }

    var checkInClockWithStorage = function () {
        $("#Timesheet_CheckIn").attr("disabled", "disabled");
        $("#Timesheet_CheckOut").removeAttr("disabled");
        $('.timer').countimer('start');
        setStartTimeInLocalStorage();
    }

    var checkOutClock = function () {
        $("#Timesheet_CheckOut").attr("disabled", "disabled");
        $("#Timesheet_CheckIn").removeAttr("disabled");

        $('.timer').countimer('stop');
        $('.timer').countimer();
    }

    var checkOutClockWithStorage = function () {

        SaveTimeSheet()

    }

    var setStartTimeInLocalStorage = function () {

        localStorage.setItem("IsCheckedIn", JSON.stringify(true));
        localStorage.setItem("CheckedInDateTime", new Date());

    }

    var setEndTimeInLocalStorage = function () {

        localStorage.setItem("IsCheckedIn", JSON.stringify(false));
        localStorage.setItem("CheckedInDateTime", new Date());

    }

    var AlertUpperLimit = function () {

        // if the time ellapsed is 
        //more than * 8 * Hours then it shuld 
        //automatically update the record and stop counter
        var i = 0;
        $('.timer').countimer({
            enableEvents: true
        }).on('minute', function (evt, time) {
            // console.log(time.original.hours);
            if (time.original.hours == 8) {
                SaveTimeSheet();
            }
        });
    }

    var InitializationDefault = function (hour, min, sec, autostart) {

        $('.timer').countimer({

            // Enable the timer events
            enableEvents: true,

            // Auto start on inti
            autoStart: false, //in default changed to false

            // Show hours
            useHours: true,

            // Custom indicator for minutes
            minuteIndicator: '',

            // Custom indicator for seconds
            secondIndicator: '',

            // Separator between each time block
            separator: ':',

            // Number of leading zeros
            leadingZeros: 2,

            // Initial time
            initHours: hour,
            initMinutes: min,
            initSeconds: sec

        });
        if (autostart) {
            checkInClock();
        } else {
            checkOutClock();
        }

    }

    var ReInitializeCounter = function () {
        $('.timer').countimer({

            // Enable the timer events
            enableEvents: true,

            // Auto start on inti
            autoStart: false, //in default changed to false

            // Show hours
            useHours: true,

            // Custom indicator for minutes
            minuteIndicator: '',

            // Custom indicator for seconds
            secondIndicator: '',

            // Separator between each time block
            separator: ':',

            // Number of leading zeros
            leadingZeros: 2,

            // Initial time
            initHours: 0,
            initMinutes: 0,
            initSeconds: 0

        });
    }

    var checkForAlreadyCheckedIn = function () {  //function that finds the how much time has been ellapsed after checked in


        //checking if the local storage is null or not
        if (!(localStorage.getItem("IsCheckedIn") === null) && !(localStorage.getItem("CheckedInDateTime") === null) && JSON.parse(localStorage.getItem("IsCheckedIn"))) {

            var checkedIn = JSON.parse(localStorage.getItem("IsCheckedIn"));
            var startdate = new Date(localStorage.getItem("CheckedInDateTime"));

            var start_actual_time = new Date(startdate);
            var end_actual_time = new Date();

            var diff = end_actual_time - start_actual_time;
            var sec = diff / 1000;

            var h = Math.floor(sec / 3600); //Get whole hours
            sec -= h * 3600;
            var m = Math.floor(sec / 60); //Get remaining minutes
            sec -= m * 60; // remaining is sec
            var s = Math.floor(sec);

            if (start_actual_time.getDate() == end_actual_time.getDate() && JSON.parse(localStorage.getItem("IsCheckedIn"))) {

                // initialization of the default library configuration if already not checked in
                InitializationDefault(
                    h, // default start hour
                    m, // default start min
                    s, // default start sec
                    checkedIn // autostart if already checked in
                );

            }
            else {
                //calling function for atmost *8* hours of Counter
                AlertUpperLimit();
                // initialization of the default library configuration if already not checked in
                InitializationDefault(
                    0, // default start hour
                    0, // default start min
                    0, // default start sec
                    false // autostart if already checked in
                );

                setEndTimeInLocalStorage()
            }

        }
        else {

            // initialization of the default library configuration if already not checked in
            InitializationDefault(
                0, // default start hour
                0, // default start min
                0, // default start sec
                false // autostart if already checked in
            );
        }


    }

    var getTimesheetForToday = function () {

        var model = {}
        var url = "/Timesheet/GetTimesheetByUser";
        $ajaxCall(
            model,
            url,
            'POST',
            function (data) {

                console.log(data);
                if (data.Code == 200) {

                    var data = data.Data;
                    var totSec = 0;
                    var tr = "";
                    var i = 1;
                    $("#TimesheetLog").html(tr);
                    $.each(data, function (index, obj) {
                        totSec = totSec + obj.TimeDuration;
                        var startAt = new Date(parseInt(obj.CheckInAt.substr(6)));
                        var endAt = new Date(parseInt(obj.CheckOutAt.substr(6)));

                        tr = " <tr><th scope=\"row\">" + i + "</th ><td>" + startAt.toString().substr(16, 8) + "</td><td>" + endAt.toString().substr(16, 8) + "</td></tr>"
                        $("#TimesheetLog").append(tr);
                        i++;
                    });

                    var h = Math.floor(totSec / 3600); //Get whole hours
                    totSec -= h * 3600;
                    var m = Math.floor(totSec / 60); //Get remaining minutes
                    totSec -= m * 60; // remaining is sec
                    var s = Math.floor(totSec);

                    if (h < 10) h = '0' + h;
                    if (m < 10) m = '0' + m;
                    if (s < 10) s = '0' + s;

                    var time = h + ":" + m + ":"+ s; 
                    $("#totTime").html(time);
                } else {
                    alert(data.Message);
                }

            },
            function () {
                alert("Unable To CheckOut");
            }
        );
    }

    var SaveTimeSheet = function () {

        var startdate = new Date(localStorage.getItem("CheckedInDateTime"));

        var start_actual_time = new Date(startdate);
        var end_actual_time = new Date();

        var diff = end_actual_time - start_actual_time;
        var sec = diff / 1000;

        var model = {
            CheckInAt: $.date(startdate),
            CheckOutAt: $.date(end_actual_time),
            TimeDuration: Math.floor(sec)
        }
        var url = "/Timesheet/CheckOut";

        $ajaxCall(
            model,
            url,
            'POST',
            function (data) {

                if (data.Code == 200) {

                    $("#Timesheet_CheckOut").attr("disabled", "disabled");
                    $("#Timesheet_CheckIn").removeAttr("disabled");
                    $('.timer').countimer('stop');
                    setEndTimeInLocalStorage(); 
                   
                    getTimesheetForToday();
                    //ReInitializeCounter();
                    //window.location.href = window.location.href;
                } else {
                    alert(data.Message);
                }

            },
            function () {
                alert("Unable To CheckOut");
            }
        );
    }


    var init = function () {
        //function that finds the how much time has been ellapsed after checked in
        checkForAlreadyCheckedIn();

        //calling function for atmost *8* hours of Counter
        AlertUpperLimit();
        getTimesheetForToday();
    }

    // Public Exposed function
    return {
        init: init,
        checkIn: checkInClockWithStorage,
        checkOut: checkOutClockWithStorage
    };
})(); 

var Report = (function () {

    var $ajaxCall = function (data, url, type, success, error) {
        $.ajax({
            type: type,
            async: true,
            url: url,
            data: data,
            success: success,
            error: error
        });

    }

    $.date = function (dateObject) {
        var d = new Date(dateObject);
        var day = d.getDate();
        var month = d.getMonth() + 1;
        var year = d.getFullYear();

        var hour = d.getHours();
        var min = d.getMinutes();
        var sec = d.getSeconds();
        if (day < 10) {
            day = "0" + day;
        }
        if (month < 10) {
            month = "0" + month;
        }
        var date = year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;

        return date;
    };
 
   
    var getTimesheetReport = function () {

        var model = {}
        var url = "/Timesheet/GetTimesheetByUser";
        $ajaxCall(
            model,
            url,
            'POST',
            function (data) {

                console.log(data);
                if (data.Code == 200) {

                    var data = data.Data;
                    var totSec = 0;
                    var tr = "";
                    var i = 1;
                    $("#TimesheetLog").html(tr);
                    $.each(data, function (index, obj) {
                        totSec = totSec + obj.TimeDuration;
                        var startAt = new Date(parseInt(obj.CheckInAt.substr(6)));
                        var endAt = new Date(parseInt(obj.CheckOutAt.substr(6)));

                        tr = " <tr><th scope=\"row\">" + i + "</th ><td>" + startAt.toString().substr(16, 8) + "</td><td>" + endAt.toString().substr(16, 8) + "</td></tr>"
                        $("#TimesheetLog").append(tr);
                        i++;
                    });

                    var h = Math.floor(totSec / 3600); //Get whole hours
                    totSec -= h * 3600;
                    var m = Math.floor(totSec / 60); //Get remaining minutes
                    totSec -= m * 60; // remaining is sec
                    var s = Math.floor(totSec);

                    if (h < 10) h = '0' + h;
                    if (m < 10) m = '0' + m;
                    if (s < 10) s = '0' + s;

                    var time = h + ":" + m + ":" + s;
                    $("#totTime").html(time);
                } else {
                    alert(data.Message);
                }

            },
            function () {
                alert("Unable To CheckOut");
            }
        );
    } 

    var init = function () {
        
        getTimesheetReport();
    }

    // Public Exposed function
    return {
        init: init 
    };
})(); 

