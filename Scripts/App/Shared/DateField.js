$(function () {
    var daysOfMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    function getMonthDays(year, month) {
        var result = 31;
        if (month === 2) {
            result = (year % 4 === 0 && year % 100 !== 0 || year % 400 === 0) ? 29 : 28;
        } else {
            result = daysOfMonth[month - 1];
        }
        return result;
    }

    $('.tb-date.tb-date-month').change(function (e) {
        var container = $(e.target).closest('.tb-date-container');
        var year = $(container).find('.tb-date-year').val();
        var month = e.target.value;
        var maxday = getMonthDays(year, month);
        const arr = Array.from(Array(maxday), (_, index) => index + 1);
        var opts = $.map(arr, function (item, index) {
            return '<option value="' + item + '">' + item + '</option>';
        });
        $(container).find('.tb-date-day').html(opts);
    });
});