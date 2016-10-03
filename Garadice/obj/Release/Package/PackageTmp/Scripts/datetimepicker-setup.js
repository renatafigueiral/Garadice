$(function () {

    $('#Start').datetimepicker({
        //inline: true,
        sideBySide: true,
        calendarWeeks: true,
        daysOfWeekDisabled: [0, 6],
        disabledDates: [
            moment("10/31/2016"),
            moment("12/25/2016"),
            moment("12/26/2016"),
            moment("12/27/2016")
        ],
        format: 'DD/MM/YYYY HH:mm'
    });




    //$('#datetimepicker1').datetimepicker({
    //    //inline: true,
    //    sideBySide: true,
    //    calendarWeeks: true,
    //    daysOfWeekDisabled: [0, 6],
    //    disabledDates: [
    //        moment("10/31/2016"),
    //        moment("12/25/2016"),
    //        moment("12/26/2016"),
    //        moment("12/27/2016")
    //    ],

    //    //format: 'DD/MM/YYYY HH:mm a'
    //});


    //$('#End').datetimepicker({
    //    //inline: true,
    //    sideBySide: true,
    //    calendarWeeks: true,
    //    daysOfWeekDisabled: [0, 6],
    //    disabledDates: [
    //        moment("10/31/2016"),
    //        moment("12/25/2016"),
    //        moment("12/26/2016"),
    //        moment("12/27/2016")
    //    ],

    //    //format: 'DD/MM/YYYY HH:mm a'
    //});
    $('#datetimepicker3').datetimepicker({
        format: 'HH:mm'
    });
    $('#EstimatedDuration').datetimepicker({
        format: 'HH:mm',
        //mintDate: "01/01/2000 01:00"
    });
    $('#NumberOfHours').datetimepicker({
        format: 'HH:mm'
    });
    //$('#StartTime').datetimepicker({
    //    format: 'HH:mm'
    //});
});