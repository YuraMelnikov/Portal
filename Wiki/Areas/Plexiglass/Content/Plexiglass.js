// 01 - KO
// 02 - OS
// 03 - All

$(document).ready(function () {
    StartMenu();
    if (userGroupId === 1) {
        $('#btnAddOrder').show();
        $('#dTableKOOrder').show();
        $('#dTableOSOrder').hide();
        $('#dTableAllOrder').show();
    }
    else if (userGroupId === 2) {
        $('#btnAddOrder').hide();
        $('#dTableKOOrder').hide();
        $('#dTableOSOrder').show();
        $('#dTableAllOrder').show();
    }
    else {
        $('#btnAddOrder').hide();
        $('#dTableKOOrder').hide();
        $('#dTableOSOrder').hide();
        $('#dTableAllOrder').show();
    }
}); 

var objPositionsOrder = [
    { "title": "№ поз.", "data": "positionNum", "autowidth": true, "bSortable": true },
    { "title": "№ заказа", "data": "id_PlexiglassOrder", "autowidth": true, "bSortable": false },
    { "title": "Обозначение", "data": "designation", "autowidth": true, "bSortable": true },
    { "title": "Наименование", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Индекс", "data": "index", "autowidth": true, "bSortable": false },
    { "title": "Кол-во", "data": "quentity", "autowidth": true, "bSortable": true },
    { "title": "Площадь единицы, м3", "data": "square", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Штрихкод", "data": "barcode", "autowidth": true, "bSortable": true }
];

var objKOOrdersList = [
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "№ план-заказа", "data": "PZ_PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Полуфабрикат", "data": "CMO_TypeProduct", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "datetimeCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "AspNetUsers", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "PlexiglassCompany", "autowidth": true, "bSortable": true },
    { "title": "Срок поставки", "data": "continueDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Прим.", "data": "note", "autowidth": true, "bSortable": false },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false },
    { "title": "Удалить", "data": "remove", "autowidth": true, "bSortable": false }
]; 

var objOSOrdersList = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "№ план-заказа", "data": "PZ_PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Полуфабрикат", "data": "CMO_TypeProduct", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "datetimeCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "AspNetUsers", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "PlexiglassCompany", "autowidth": true, "bSortable": true },
    { "title": "Срок поставки", "data": "continueDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Прим.", "data": "note", "autowidth": true, "bSortable": false },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false }
]; 

var objOrdersList = [
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "№ план-заказа", "data": "PZ_PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Полуфабрикат", "data": "CMO_TypeProduct", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "datetimeCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "AspNetUsers", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "PlexiglassCompany", "autowidth": true, "bSortable": true },
    { "title": "Срок поставки", "data": "continueDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Дата поставки", "data": "finishDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Прим.", "data": "note", "autowidth": true, "bSortable": false },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false }
]; 

function StartMenu() {
    $("#tableKOOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Plexiglas/GetOrders",
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "desc"],
        "processing": true,
        "columns": objKOOrdersList,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#tableOSOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Plexiglas/GetOrders",
            "type": "POST",
            "datatype": "json"
        },
        "order": [1, "desc"],
        "processing": true,
        "columns": objOSOrdersList,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#tableAllOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Plexiglas/GetReportOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "desc"],
        "processing": true,
        "columns": objOrdersList,
        "rowCallback": function (row, data, index) {
            if (data.finishDate !== "null") {
                $('td', row).css('background-color', '#c1e6be');
            }
        },
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#tablePositionsOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Plexiglas/GetPositionsOrder/" + 0,
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "desc"],
        "processing": true,
        "columns": objPositionsOrder,
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

function AddOrder() {
    var files = document.getElementById('fileOrder').files;
    var valid = ValidCreatingOrder(files.length);
    if (valid === false) {
        return false;
    }
    $('#btnAddOrderModal').hide();
    $('#loaderOrder').show();
    var data = new FormData();
    data.append($('#pzList').val(), $('#typeProductId').val());
    data.append($('#customerNewOrderId').val(), $('#typeProductId').val());
    for (var x = 0; x < files.length; x++) {
        data.append(files[x].name, files[x]);
    }
    $.ajax({
        type: "POST",
        url: "/Plexiglas/AddOrder",
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            if (result !== "0") {
                $('#btnAddOrderModal').show();
                $('#loaderOrder').hide();
                $('#errorMessage').val(result);
            }
            else {
                $('#tableKOOrder').DataTable().ajax.reload(null, false);
                $('#tableAllOrder').DataTable().ajax.reload(null, false);
                $('#errorMessage').val("");
                $('#creatingOrderModal').modal('hide');
            }
        }
    });
}

function ValidCreatingOrder(lenghtFile) {
    var isValid = true;
    if (lenghtFile === 0) {
        isValid = false;
    }
    if ($('#pzList').val().length === 0) {
        isValid = false;
    }
    if ($('#typeProductId').val() === null) {
        $('#typeProductId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#typeProductId').css('border-color', 'lightgrey');
    }
    if ($('#customerNewOrderId').val() === null) {
        $('#customerNewOrderId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#customerNewOrderId').css('border-color', 'lightgrey');
    }
    return isValid;
}

function GetOrder(id) {
    CleanerModals();
    $.ajax({
        cache: false,
        url: "/Plexiglas/GetOrder/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#PZ_PlanZakaz').val(result.PZ_PlanZakaz);
            $('#CMO_TypeProduct').val(result.CMO_TypeProduct);
            $('#AspNetUsers').val(result.AspNetUsers);
            $('#PlexiglassCompany').val(result.PlexiglassCompany);
            $('#datetimeCreate').val(result.datetimeCreate);
            $('#continueDate').val(result.continueDate);
            $('#finishDate').val(result.finishDate);
            $('#note').val(result.note);
            $('#btnUpdateOrder').show();
            $('#UpdateOrderModal').modal('show');
        }
    });
}

function CleanerModals() {
    $('#btnAddOrderModal').hide(); 
    $('#pzList').val("");
    $('#pzList').chosen();
    $('#pzList').trigger('chosen:updated');
    $('#id').val("");
    $('#typeProductId').val("");
    $('#PZ_PlanZakaz').val("");
    $('#CMO_TypeProduct').val("");
    $('#AspNetUsers').val("");
    $('#PlexiglassCompany').val("");
    $('#datetimeCreate').val("");
    $('#continueDate').val("");
    $('#finishDate').val("");
    $('#note').val("");
    $('#fileOrder').val("");
    $('#errorMessage').val("");
}

function UpdateOrder() {
    var obj = {
        id: $('#id').val(),
        continueDate: $('#continueDate').val(),
        finishDate: $('#finishDate').val()
        //note: $('#note').val()
    };
    $.ajax({
        cache: false,
        url: "/Plexiglas/UpdateOrder",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#btnUpdateOrder').hide();
            $('#tableOSOrder').DataTable().ajax.reload(null, false);
            $('#tableAllOrder').DataTable().ajax.reload(null, false);
            $('#UpdateOrderModal').modal('hide'); 
        }
    });
}

function RemoveOrder(id) {
    $.ajax({
        cache: false,
        url: "/Plexiglas/RemoveOrder/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#tableKOOrder').DataTable().ajax.reload(null, false);
            $('#tableAllOrder').DataTable().ajax.reload(null, false);
        }
    });
    return false;
}

function GetPositionsOrder(id) {
    $("#tablePositionsOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Plexiglas/GetPositionsOrder/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "asc"]],
        "bAutoWidth": false,
        "columns": objPositionsOrder,
        "scrollY": '70vh',
        "searching": false,
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
    $('#positionsOrderModal').modal('show');
}

function CreateOrder() {
    CleanerModals();
    $('#btnAddOrderModal').show();
    $('#creatingOrderModal').modal('show');
    $('#loaderOrder').hide();
}

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}