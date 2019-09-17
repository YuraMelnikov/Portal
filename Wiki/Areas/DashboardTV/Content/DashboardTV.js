$(document).ready(function () {
    getPeriodReport();
    getTablePlan();
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

function getTablePlan() {
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
            { "title": "% ожидаемого освоения к плану", "data": "inThisMonthPercent", "autowidth": true, "bSortable": false, "className": 'text-center' }
        ],
        "paging": false,
        "searching": false,
        "info": false
    });
}