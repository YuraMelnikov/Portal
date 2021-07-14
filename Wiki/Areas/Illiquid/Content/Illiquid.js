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

    $("#tableWorrking").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Illiq/GetTableWorking",
            "type": "POST",
            "datatype": "json"
        },
        "order": [1, "asc"],
        "processing": true,
        "columns": objWorking,
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

var objWorking = [
    { "title": "Ид", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "ИдНеликв", "data": "idNlk", "autowidth": true, "bSortable": true },
    { "title": "Дата", "data": "date", "autowidth": true, "bSortable": true },
    { "title": "Код", "data": "code", "autowidth": true, "bSortable": true },
    { "title": "НО", "data": "max", "autowidth": true, "bSortable": true },
    { "title": "ТМЦ", "data": "materialName", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "До", "data": "queBefore", "autowidth": true, "bSortable": true },
    { "title": "После", "data": "queNext", "autowidth": true, "bSortable": true },
    { "title": "Откл", "data": "otkl", "autowidth": true, "bSortable": true },
    { "title": "Кол-во", "data": "que", "autowidth": true, "bSortable": true },
    { "title": "Сумма USD", "data": "sum", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Причина", "data": "cause", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Прим.", "data": "note", "autowidth": true, "bSortable": true, "class": 'colu-300' },
    { "title": "Изм.норм", "data": "changeNorms", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Заказы", "data": "orders", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Пост.", "data": "added", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Замены", "data": "replacement", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Заявки СН", "data": "sn", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Пост. Х", "data": "addedX", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Уменшен выпуск", "data": "vipusk", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Возврат МОЛом", "data": "vozvratMOL", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "Возврат", "data": "vozvrat", "autowidth": true, "bSortable": true, "class": 'colu-100' },
];

var objNotWorking = [
    { "title": "Ид", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Код", "data": "code", "autowidth": true, "bSortable": true },
    { "title": "НО", "data": "max", "autowidth": true, "bSortable": true },
    { "title": "ТМЦ", "data": "materialName", "autowidth": true, "bSortable": true, "class": 'colu-100' },
    { "title": "До", "data": "queBefore", "autowidth": true, "bSortable": true },
    { "title": "После", "data": "queNext", "autowidth": true, "bSortable": true },
    { "title": "Откл", "data": "que", "autowidth": true, "bSortable": true },
    { "title": "Сумма USD", "data": "sum", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') }
];

function CleanerIlliquidModal() {
    $('#idIlliquid').val("");
    $('#devision').val("");
    $('#typeError').val("");
    $('#noteIlliquid').val("");
}

function GetIlliquid(id) {
    CleanerIlliquidModal();
    $.ajax({
        cache: false,
        url: "/Illiq/GetIlliquid/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idIlliquid').val(result.data[0].id);
            //$('#devision').val(result.devision);
            //$('#typeError').val(result.typeError);
            $('#noteIlliquid').val(result.data[0].noteIlliquid);
            $('#getIlliquidModal').modal('show');
        }
    });
}

function UpdateIlliquid() {
    var obj = {
        idIlliquid: $('#idIlliquid').val(),
        //devision: $('#devision').val(),
        //typeError: $('#typeError').val(),
        noteIlliquid: $('#noteIlliquid').val()
    };
    $.ajax({
        cache: false,
        url: "/Illiq/UpdateIlliquid",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#tableNotWorrking').DataTable().ajax.reload(null, false);
            $('#getIlliquidModal').modal('hide');
        }
    });
}

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
            $('#tableWorrking').DataTable().ajax.reload(null, false);
        }
    });
}