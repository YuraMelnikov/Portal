$(document).ready(function () {
    StartMenu();
});

function StartMenu() {
    $("#tableNotWorrking").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Illiq/GetTableNotWorking",
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "asc"],
        "processing": true,
        "columns": objNotWorking,
        "scrollY": '70vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": true,
        "scrollCollapse": true,
        "fixedColumns": {
            "leftColumns": 3
        },
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

var objNotWorking = [
    { "title": "Ид", "data": "id", "autowidth": true, "bSortable": true },
    //{ "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Код", "data": "code", "autowidth": true, "bSortable": true },
    { "title": "НО", "data": "max", "autowidth": true, "bSortable": true },
    { "title": "ТМЦ", "data": "materialName", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    //{ "title": "Период", "data": "period", "autowidth": true, "bSortable": false },
    { "title": "До", "data": "queBefore", "autowidth": true, "bSortable": true },
    { "title": "После", "data": "queNext", "autowidth": true, "bSortable": true },
    { "title": "Откл", "data": "que", "autowidth": true, "bSortable": true },
    //{ "title": "Sum.", "data": "sum", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Изм.норм", "data": "updateNorm", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Заявки на СН", "data": "sn", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Заказы пост.", "data": "orders", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Поступ.", "data": "added", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Поступ. Х", "data": "addedX", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Замены", "data": "replacment", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Движения", "data": "moveStock", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Причина", "data": "cause", "autowidth": true, "bSortable": true },
    { "title": "СП", "data": "devision", "autowidth": true, "bSortable": true },
    { "title": "Прим.", "data": "note", "autowidth": true, "bSortable": true, "class": 'colu-300' }
];

function OpeningSKUModal() {
    $('#skuModal').modal('show');
}

function OpeningStockModal() {
    $('#stockModal').modal('show');
}

function OpeningGetIlliquidPeriodModal() {
    $('#getIlliquidPeriodModal').modal('show');
}

function LoadingSku() {
    var data = new FormData();
    var files = document.getElementById('fileSku').files;
    for (var x = 0; x < files.length; x++) {
        data.append(files[x].name, files[x]);
    }
    $.ajax({
        type: "POST",
        url: "/Illiq/LoadingSku",
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $('#skuModal').modal('hide');
        }
    });
}

function LoadingStock() {
    var data = new FormData();
    var files = document.getElementById('fileStock').files;
    for (var x = 0; x < files.length; x++) {
        data.append(files[x].name, files[x]);
    }
    $.ajax({
        type: "POST",
        url: "/Illiq/LoadingStock",
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $('#stockModal').modal('hide');
        }
    });
}

function GetIlliquidPeriod() {
    var obj = {
        startDate: $('#startDate').val(),
        finishDate: $('#finishDate').val()
    };
    $.ajax({
        cache: false,
        url: "/Illiq/GetIlliquidPeriod",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#getIlliquidPeriodModal').modal('hide');
        }
    });
}

function AnalisysIlliquid() {
    $.ajax({
        type: "POST",
        url: "/Illiq/AnalisysIlliquid",
        contentType: false,
        processData: false,
        success: function (result) {
            $('#tableNotWorrking').DataTable().ajax.reload(null, false);
            if(result === 1)
                alert('Анализ успешно завершен');
            else
                alert('Анализ завершен с ошибкой');
        }
    });
}