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

var objTableData = [
    { "title": "План на начало месяца", "data": "monthPlan", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "Освоено на сегодняшний день", "data": "inThisDay", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "% выполнения к мес. плану", "data": "inThisDayPercent", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "Ожидаемое освоение материалов", "data": "inThisMonth", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "% ожидаемого освоения к плану", "data": "inThisMonthPercent", "autowidth": true, "bSortable": false, "className": 'text-center' }
];

function getTablePlan() {
    $("#tablePlan").DataTable({
        "ajax": {
            "cache": false,
            "url": "/DashboardTVC/GetTablePlan",
            "type": "POST",
            "datatype": "json"
        },
        "columns": objTableData,
        "paging": false,
        "searching": false,
        "info": false,
        "ordering": false
    });
}