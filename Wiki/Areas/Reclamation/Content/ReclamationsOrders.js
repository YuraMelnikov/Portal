$(document).ready(function () {
    planZakazDevisionNotSh();
});

var objOrder = [
    { "title": "См.", "data": "OpenLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Контрактное наименование", "data": "ContractName", "autowidth": true, "bSortable": false },
    { "title": "Наименование по ТУ", "data": "TuName", "autowidth": true, "bSortable": false },
    { "title": "Заказчик", "data": "Client", "autowidth": true, "bSortable": true },
    { "title": "МТР №", "data": "Mtr", "autowidth": true, "bSortable": false },
    { "title": "ОЛ №", "data": "Ol", "autowidth": true, "bSortable": true },
    { "title": "Ошибок", "data": "ReclamationCount", "autowidth": true, "bSortable": true },
    { "title": "Активных", "data": "ReclamationActive", "autowidth": true, "bSortable": true },
    { "title": "Закрытых", "data": "ReclamationClose", "autowidth": true, "bSortable": true }
];

function planZakazDevisionNotSh() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionNotSh",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "order": [[1, "desc"]],
        "columns": objOrder,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function planZakazDevisionSh() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionSh",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "order": [[1, "desc"]],
        "columns": objOrder,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function planZakazDevisionAll() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionAll",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "order": [[1, "desc"]],
        "columns": objOrder,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}