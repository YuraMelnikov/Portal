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
        $('#btnOpeningMaterialsCModal').show();
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

var objPositionsPreorder = [
    { "title": "№ поз.", "data": "positionNum", "autowidth": true, "bSortable": true },
    { "title": "№ заказа", "data": "CMOSPreOrderId", "autowidth": true, "bSortable": false },
    { "title": "Обозначение", "data": "designation", "autowidth": true, "bSortable": true },
    { "title": "Наименование", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Индекс", "data": "index", "autowidth": true, "bSortable": false },
    { "title": "Вес", "data": "weight", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Кол-во", "data": "quantity", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Кол-во (склад)", "data": "quantity8", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Суммарный вес", "data": "summaryWeight", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Цвет RAL", "data": "color", "autowidth": true, "bSortable": true },
    { "title": "Покрытие", "data": "coating", "autowidth": true, "bSortable": true },
    { "title": "Прим.", "data": "note", "autowidth": true, "bSortable": true }
];

var objFullReport = [
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Позиция/и", "data": "positions", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Номер ТН", "data": "tnNumber", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true },
    { "title": "Начало", "data": "startDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Окончание", "data": "finishDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Цена, BYN", "data": "cost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Стоимость, BYN", "data": "factCost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Ставка за кг,USD", "data": "rate", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Папка заказа", "data": "folder", "autowidth": true, "bSortable": false }
];

var objSmallReport = [
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Позиция/и", "data": "positions", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Номер ТН", "data": "tnNumber", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true },
    { "title": "Начало", "data": "startDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Окончание", "data": "finishDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Папка заказа", "data": "folder", "autowidth": true }
];

var objPreOrdersList = [
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Позиция", "data": "positionName", "autowidth": true, "bSortable": true },
    { "title": "Вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false },
    { "title": "Позиции", "data": "positionsList", "autowidth": true, "bSortable": false },
    { "title": "Удалить", "data": "remove", "autowidth": true, "bSortable": false },
];

var objNoPlaningOrders = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Позиция/и", "data": "positionName", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Ответ до", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Расчетная цена, BYN", "data": "planingCost", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Ставка за кг, USD", "data": "rate", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false },
    { "title": "Удалить", "data": "remove", "autowidth": true, "bSortable": false }
];

var objTNOrders = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Позиция/и", "data": "positionName", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Срок поступл.", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Расчетная цена, BYN", "data": "planingCost", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Ставка за кг, USD", "data": "rate", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false },
    { "title": "Удалить", "data": "remove", "autowidth": true, "bSortable": false }
];

var objOrders = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Позиция/и", "data": "positionName", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Срок поступл.", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "№ накладной", "data": "tn", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false },
    { "title": "Удалить", "data": "remove", "autowidth": true, "bSortable": false }
];

function StartMenu() {
    $("#tableNoPlaningPreOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetTableNoPlaningPreOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "desc"],
        "processing": true,
        "columns": objPreOrdersList,
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
    $("#tableNoPlaningOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetTableNoPlaningOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [1, "desc"],
        "processing": true,
        "columns": objNoPlaningOrders,
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
    $("#tableTNOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetTableTNOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [1, "desc"],
        "processing": true,
        "columns": objTNOrders,
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
    $("#tableNoClothingOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetTableNoClothingOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [1, "desc"],
        "processing": true,
        "columns": objTNOrders,
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
    $("#fullReport").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetTableOrders",
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "desc"],
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
            "url": "/CMOSS/GetTableOrders",
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "desc"],
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
    $("#tablePositionsPreorder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetPositionsPreorder/" + 0,
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "desc"],
        "processing": true,
        "columns": objPositionsPreorder,
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
    $('#typeProductId').css('border-color', 'lightgrey');
    $('#idOrder').val("");
    $('#typeProductId').val("");
    $('#pzList').val("");
    $('#pzList').chosen();
    $('#pzList').trigger('chosen:updated');
    $('#customerId').val("");
    $('#pzListBackorder').val("");
    $('#pzListBackorder').chosen();
    $('#pzListBackorder').trigger('chosen:updated');
    $('#customerBackorder').val("");
    $('#customerOrderId').val("");
    $('#preordersList').val("");
    $('#aspNetUsersCreateId').val("");
    $('#dateTimeCreate').val("");
    $('#workDate').val("");
    $('#manufDate').val("");
    $('#finDate').val("");
    $('#numberTN').val("");
    $('#cost').val("");
    $('#factCost').val("");
    $('#planWeight').val("");
    $('#factWeightTN').val("");
    $('#curency').val("");
    $('#rate').val("");
    $('#factWeightTN').val("");
    $('#filePreorder').val("");
    $('#fileBackorder').val("");
}

function CreatePreOrder() {
    CleanerModals();
    $('#btnAddPreOrderModal').show();
    $('#creatingPreOrderModal').modal('show');
    $('#loaderPreorder').hide();
}

function CreateBackorder() {
    CleanerModals();
    $('#btnAddBackorderModal').show();
    $('#creatingBackorderModal').modal('show');
    $('#loaderBackorder').hide();
}

function AddPreOrder() {
    var files = document.getElementById('filePreorder').files;
    var valid = ValidCreatingPreOrder(files.length);
    if (valid === false) {
        return false;
    }
    $('#btnAddPreOrderModal').hide();
    $('#loaderPreorder').show();
    var data = new FormData();
    data.append($('#pzList').val(), $('#typeProductId').val());
    for (var x = 0; x < files.length; x++) {
        data.append(files[x].name, files[x]);
    }
    $.ajax({
        type: "POST",
        url: "/CMOSS/AddPreOrder",
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $('#tableNoPlaningPreOrder').DataTable().ajax.reload(null, false);
            $('#fullReport').DataTable().ajax.reload(null, false);
            $('#creatingPreOrderModal').modal('hide');
        }
    });
}

function ValidCreatingPreOrder(lenghtFile) {
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
    return isValid;
}

function AddBackorder() {
    var files = document.getElementById('fileBackorder').files;
    var valid = ValidCreatingBackorder(files.length);
    if (valid === false) {
        return false;
    }
    $('#btnAddBackorderModal').hide();
    $('#loaderBackorder').show();
    var data = new FormData();
    data.append($('#pzListBackorder').val(), $('#customerBackorder').val());
    for (var x = 0; x < files.length; x++) {
        data.append(files[x].name, files[x]);
    }
    $.ajax({
        type: "POST",
        url: "/CMOSS/AddBackorder",
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $('#tableNoPlaningOrder').DataTable().ajax.reload(null, false);
            $('#fullReport').DataTable().ajax.reload(null, false);
            $('#creatingBackorderModal').modal('hide');
        }
    });
}

function ValidCreatingBackorder(lenghtFile) {
    var isValid = true;
    if (lenghtFile !== 1) {
        isValid = false;
    }
    if ($('#pzListBackorder').val().length === 0) {
        isValid = false;
    }
    if ($('#customerBackorder').val() === null) {
        $('#customerBackorder').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#customerBackorder').css('border-color', 'lightgrey');
    }
    return isValid;
}

function CreateOrder() {
    $('#btnAddNewOrderModal').show();
    $('#customerNewOrderId').val("");
    $('#workDateNew').val("");
    $('#id_CMOSPreorder').val("");
    $('#id_CMOSPreorder').chosen();
    $('#id_CMOSPreorder').trigger('chosen:updated');
    $('#newOrderModal').modal('show');
    $('#loaderOrder').hide();
}

function AddOrder() {
    var valid = ValidCreatingOrder();
    if (valid === false) {
        return false;
    }
    $('#btnAddNewOrderModal').hide();
    var obj = {
        preordersList: $('#id_CMOSPreorder').val(),
        customerOrderId: $('#customerNewOrderId').val(),
        workDate: $('#workDateNew').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOSS/AddOrder",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //cleaning orders list
            $('#tableNoPlaningPreOrder').DataTable().ajax.reload(null, false);
            $('#tableNoPlaningOrder').DataTable().ajax.reload(null, false);
            UpdatePreordersList();
            $('#newOrderModal').modal('hide');
        }
    });
}

function ValidCreatingOrder() {
    var isValid = true;
    if ($('#id_CMOSPreorder').val().length === 0) {
        isValid = false;
    }
    if ($('#customerNewOrderId').val() === null) {
        $('#customerNewOrderId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#customerNewOrderId').css('border-color', 'lightgrey');
    }
    if ($('#workDateNew').val() === "") {
        $('#workDateNew').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#workDateNew').css('border-color', 'lightgrey');
    }
    return isValid;
}

function GetOrder(id) {
    CleanerModals();
    $('#btnAddOrderModal').hide();
    $.ajax({
        cache: false,
        url: "/CMOSS/GetOrder/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#preordersList').val(result.preordersList);
            $('#idOrder').val(result.idOrder);
            $('#aspNetUsersCreateId').val(result.aspNetUsersCreateId);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#workDate').val(result.workDate);
            $('#manufDate').val(result.manufDate);
            $('#finDate').val(result.finDate);
            $('#customerOrderId').val(result.customerOrderId);
            $('#numberTN').val(result.numberTN);
            $('#cost').val(result.cost);
            $('#factCost').val(result.factCost);
            $('#planWeight').val(result.planWeight);
            $('#factWeightTN').val(result.factWeightTN);
            $('#curency').val(result.curency);
            $('#rate').val(result.rate);
            if (result.manufDate === "null") {
                $('#manufDate').prop('disabled', false);
            }
            else if (result.numberTN === null) {
                $('#numberTN').prop('disabled', false);
                $('#factWeightTN').prop('disabled', false);
                $('#factCost').prop('disabled', false);
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
        manufDate: $('#manufDate').val(),
        finDate: $('#finDate').val(),
        numberTN: $('#numberTN').val(),
        factCost: $('#factCost').val().replace('.', ','),
        factWeightTN: $('#factWeightTN').val().replace('.', ','),
        rate: $('#rate').val().replace('.', ',')
    };
    $.ajax({
        cache: false,
        url: "/CMOSS/UpdateOrder",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#btnUpdateOrder').hide();
            $('#tableNoPlaningOrder').DataTable().ajax.reload(null, false);
            $('#tableNoPlaningPreOrder').DataTable().ajax.reload(null, false);
            $('#tableTNOrder').DataTable().ajax.reload(null, false);
            $('#tableNoClothingOrder').DataTable().ajax.reload(null, false);
            $('#fullReport').DataTable().ajax.reload(null, false);
            $('#orderModal').modal('hide');
        }
    });
}

function RemovePreOrder(id) {
    $.ajax({
        cache: false,
        url: "/CMOSS/RemovePreOrder/" + id,
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
        url: "/CMOSS/RemoveOrder/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#tableNoPlaningOrder').DataTable().ajax.reload(null, false);
            $('#tableTNOrder').DataTable().ajax.reload(null, false);
            $('#tableNoClothingOrder').DataTable().ajax.reload(null, false);
            $('#fullReport').DataTable().ajax.reload(null, false);
        }
    });
    return false;
}

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}

function GetPositionsPreorder(id) {
    $("#tablePositionsPreorder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetPositionsPreorder/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "asc"]],
        "bAutoWidth": false,
        "columns": objPositionsPreorder,
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
    $('#positionsPreorderModal').modal('show');
}

function GetPositionsOrder(id) {
    $("#tablePositionsPreorder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetPositionsOrder/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "asc"]],
        "bAutoWidth": false,
        "columns": objPositionsPreorder,
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
    $('#positionsPreorderModal').modal('show');
}

function OpeningMaterialsCModal() {
    $('#materialsCModal').modal('show');
}

function LoadingMaterialsC() {
    var data = new FormData();
    var files = document.getElementById('fileC').files;
    for (var x = 0; x < files.length; x++) {
        data.append(files[x].name, files[x]);
    }
    $.ajax({
        type: "POST",
        url: "/CMOSS/LoadingMaterialsC",
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $('#materialsCModal').modal('hide');
        }
    });
}

function UpdatePreordersList() {
    $.ajax({
        cache: false,
        url: "/CMOSS/UpdatePreordersList/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var x = document.getElementById("id_CMOSPreorder");
            for (var i = x.children.length; i >= 0; i--) {
                x.remove(i);
            }
            for (var j = 0; j < data.length; j++) {
                var optionhtml = '<option value="' + data[j].Value + '">' + data[j].Text + '</option>';
                $("#id_CMOSPreorder").append(optionhtml);
            }
        }
    });
}