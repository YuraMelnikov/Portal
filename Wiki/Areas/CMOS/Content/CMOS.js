// 01 - OS
// 02 - KO
// 03 - All

$(document).ready(function () {
    StartMenu();
    if (userGroupId === 1) {
        $('#btnAddOrder').show();
        $('#dTableNoPlaningPreOrder').show();
        $('#dTableNoPlaningOrder').show();
        $('#dTableTNOrder').show();
        $('#dTableNoClothingOrder').show();
        $('#dFullReport').show();


        //после проверки удалить!
        $('#btnAddPreOrder').show();
        $('#btnReOrder').show();
    }
    else if (userGroupId === 2) {
        $('#btnAddPreOrder').show();
        $('#btnReOrder').show();
        $('#dTableNoPlaningPreOrder').show();
        $('#dFullReport').show();
    }
    else {
        $('#dShortReport').show();
    }
});

var objFullReport = [
    { "title": "Ид.", "data": "id", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Позиция/и", "data": "positions", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Номер ТН", "data": "tnNumber", "autowidth": true, "bSortable": true },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true },
    { "title": "Начало", "data": "startDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Окончание", "data": "finishDate", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Вес", "data": "summryWeight", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Цена, б/НДС (BYN)", "data": "cost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Стоимость, б/НДС (BYN)", "data": "factCost", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Папка заказа", "data": "folder", "autowidth": true }
];

var objSmallReport = [
    { "title": "Ид.", "data": "id", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Позиция/и", "data": "positions", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Номер ТН", "data": "tnNumber", "autowidth": true, "bSortable": true },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true },
    { "title": "Начало", "data": "startDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Окончание", "data": "finishDate", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Вес", "data": "summryWeight", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Цена, б/НДС (BYN)", "data": "cost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Стоимость, б/НДС (BYN)", "data": "factCost", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Папка заказа", "data": "folder", "autowidth": true }
];

var objPreOrdersList = [
    { "title": "Ид.", "data": "id", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Позиция", "data": "positionName", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Удалить", "data": "remove", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
];

var objNoPlaningOrders = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": true },
    { "title": "Ид.", "data": "id", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Позиция/и", "data": "positionName", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Ответ до", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": true },
    { "title": "Удалить", "data": "order", "autowidth": true, "bSortable": true }
];

var objTNOrders = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": true },
    { "title": "Ид.", "data": "id", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Позиция/и", "data": "positionName", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Срок поступл.", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": true },
    { "title": "Удалить", "data": "order", "autowidth": true, "bSortable": true }
];

var objOrders = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": true },
    { "title": "Ид.", "data": "id", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Позиция/и", "data": "positionName", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Срок поступл.", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "№ накладной", "data": "tn", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": true },
    { "title": "Удалить", "data": "order", "autowidth": true, "bSortable": true }
];

function StartMenu() {
    $("#tableNoPlaningPreOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSArea/GetTableNoPlaningPreOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objPreOrdersList,
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
    $("#tableNoPlaningOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSArea/GetTableNoPlaningOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objNoPlaningOrders,
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
    $("#tableTNOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSArea/GetTableTNOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objTNOrders,
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
    $("#tableNoClothingOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSArea/GetTableNoClothingOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objOrders,
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
    $("#fullReport").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSArea/GetTableOrders",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objFullReport,
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
    $("#shortReport").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSArea/GetTableOrders",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objSmallReport,
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

function CleanerModals() {
    $('#idOrder').val("");
    $('#typeProductId').val("");
    $('#pzList').val("");
    $('#pzList').chosen();
    $('#pzList').trigger('chosen:updated');
    $('#customerId').val("");
    $('#pzListForBackorder').val("");
    $('#pzListForBackorder').chosen();
    $('#pzListForBackorder').trigger('chosen:updated');
    $('#customerOrderId').val("");
    $('#preordersList').val("");
    $('#preordersList').chosen();
    $('#preordersList').trigger('chosen:updated');
    $('#aspNetUsersCreateId').val("");
    $('#dateTimeCreate').val("");
    $('#workDate').val("");
    $('#manufDate').val("");
    $('#finDate').val("");
    $('#numberTN').val("");
    $('#cost').val("");
    $('#factCost').val("");
    $('#cources').val("");
    $('#planWeight').val("");
    $('#factWeightTN').val("");
    $('#factWeightDoc').val("");
}

function CreatePreOrder() {
    CleanerModals();
    $('#btnAddPreOrderModal').show();
    $('#creatingPreOrderModal').modal('show');
}

function CreateBackorder() {
    CleanerModals();
    $('#btnAddBackorderModal').show();
    $('#creatingBackorderModal').modal('show');
}

function CreateOrder() {
    CleanerModals();
    $('#btnAddOrderModal').show();
    $('#orderModal').modal('show');
}

function AddPreOrder() {
    var obj = {
        pzList: $('#pzList').val(),
        typeProductId: $('#typeProductId').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOSArea/AddPreOrder",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#btnAddPreOrderModal').hide();
            $('#fullReport').DataTable().ajax.reload(null, false);
            $('#creatingPreOrderModal').modal('hide');
        }
    });
}

function AddBackorder() {
    var obj = {
        pzListBackorder: $('#pzListBackorder').val(),
        customerBackorder: $('#customerBackorder').val(),
        fileBackorder: $('#fileBackorder').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOSArea/AddBackorder",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#btnAddBackorderModal').hide();
            $('#fullReport').DataTable().ajax.reload(null, false);
            $('#creatingBackorderModal').modal('hide');
        }
    });
}

function AddOrder() {
    var obj = {
        preordersList: $('#preordersList').val(),
        customerOrderId: $('#customerOrderId').val(),
        workDate: $('#workDate').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOSArea/AddBackorder",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#btnAddOrderModal').hide();
            $('#dTableNoPlaningOrder').DataTable().ajax.reload(null, false);
            $('#orderModal').modal('hide');
        }
    });
}

function GetOrder(id) {
    CleanerModals();
    $('#btnAddOrderModal').hide();
    $.ajax({
        cache: false,
        url: "/CMOSArea/GetOrder/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idOrder').val(result.idOrder);
            $("#preordersList").val(result.id_PlanZakaz).trigger("chosen:updated");
            $('#customerOrderId').val(result.customerOrderId);
            $('#aspNetUsersCreateId').val(result.aspNetUsersCreateId);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#workDate').val(result.workDate);
            $('#manufDate').val(result.manufDate);
            $('#finDate').val(result.finDate);
            $('#numberTN').val(result.numberTN);
            $('#cost').val(result.cost);
            $('#factCost').val(result.factCost);
            $('#cources').val(result.cources);
            $('#planWeight').val(result.planWeight);
            $('#factWeightTN').val(result.factWeightTN);
            $('#factWeightDoc').val(result.factWeightDoc);
            if (result.manufDate === "") {
                $('#manufDate').prop('disabled', false);
            }
            else if (result.numberTN === "") {
                $('#numberTN').prop('disabled', false);
            }
            else {
                $('#finDate').prop('disabled', false);
            }
            $('#btnUpdateOrder').show();
            $('#orderModal').modal('show');
        }
    });
}

function UpdateOrder() {
    var obj = {
        idOrder: $('#idOrder').val(),
        customerOrderId: $('#customerOrderId').val(),
        workDate: $('#workDate').val(),
        manufDate: $('#manufDate').val(),
        finDate: $('#finDate').val(),
        numberTN: $('#numberTN').val(),
        factCost: $('#factCost').val(),
        factWeightTN: $('#factWeightTN').val(),
        factWeightDoc: $('#factWeightDoc').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOSArea/UpdateOrder",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#btnUpdateOrder').hide();
            $('#dTableNoPlaningOrder').DataTable().ajax.reload(null, false);
            $('#dTableTNOrder').DataTable().ajax.reload(null, false);
            $('#dTableNoClothingOrder').DataTable().ajax.reload(null, false);
            $('#dFullReport').DataTable().ajax.reload(null, false);
            $('#orderModal').modal('hide');
        }
    });
}

function RemovePreOrder(id) {
    $.ajax({
        cache: false,
        url: "/CMOArea/RemovePreOrder/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#tableNoPlaningPreOrder').DataTable().ajax.reload(null, false);
        }
    });
    return false;
}

function RemoveOrder(id) {
    $.ajax({
        cache: false,
        url: "/CMOArea/RemoveOrder/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#dTableNoPlaningOrder').DataTable().ajax.reload(null, false);
            $('#dTableTNOrder').DataTable().ajax.reload(null, false);
            $('#dTableNoClothingOrder').DataTable().ajax.reload(null, false);
            $('#dFullReport').DataTable().ajax.reload(null, false);
        }
    });
    return false;
}