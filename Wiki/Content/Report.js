﻿$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/CMO2/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": [
            { "title": "Позиция", "data": "positionNames", "autowidth": true }
            , { "title": "Подрядчик", "data": "client", "autowidth": true }
            , { "title": "Первая цена, б/НДС (BYN)", "data": "costFirst", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') }
            , { "title": "Изг. дн.", "data": "day", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Стоимость, б/НДС (BYN)", "data": "costFinal", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') }
            , { "title": "Дата размещения", "data": "dateStart", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Дата исполнения", "data": "datePlanComplited", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Дата поступления", "data": "dateComplited", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull }
        ],
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}