$(document).ready(function () {
    getPeriodReport();
    loadData();
});

function getPeriodReport() {
    $.ajax({
        url: "/BP/GetPeriodReport/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("periodReportString").textContent = result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadData() {
    $("#tablePlan").DataTable({
        "ajax": {
            "cache": false,
            "url": "/DashboardTV/GetTablePlan",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "columns": [
            { "title": "План на начало месяца", "data": "monthPlan", "autowidth": true, "bSortable": false, "className": 'text-center' },
            { "title": "Освоено на сегодняшний день", "data": "inThisDay", "autowidth": true, "bSortable": false, "className": 'text-center' },
            { "title": "% выполнения к мес. плану", "data": "inThisDayPercent", "autowidth": true, "bSortable": false, "className": 'text-center' },
            { "title": "Ожидаемое освоение материалов", "data": "inThisMonth", "autowidth": true, "bSortable": false, "className": 'text-center' },
            { "title": "% ожидаемого освоения к плану", "data": "inThisMonthPercent", "bSortable": false, "className": 'text-center' }
        ],
        "paging": false,
        "searching": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}