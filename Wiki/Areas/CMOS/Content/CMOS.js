// 01 - OS
// 02 - KO
// 03 - All

$(document).ready(function () {
    StartMenu();
    if (userGroupId === 1) {
        $('#btnAddOrder').show();
        $('#dTableNoPlaningPreOrder').show();
        $('#dTableNoPlaningOrder').show();
        $('#dTableNoClothingOrder').show();
        $('#dFullReport').show(); 
        $('#btnOpeningMaterialsCModal').show();
    }
    else if (userGroupId === 2) { 
        $('#btnAddPreOrder').show();
        $('#btnReOrder').show();
        $('#dTableNoPlaningPreOrder').show();
        $('#dFullReport').show();
    }
    else if (userGroupId === 4) {
        $('#dFullReport').show();
    }
    else if (userGroupId === 5) {
        $('#dTableTNOrder').show();
        $('#dFullReport').show();
    }
    else {
        $('#dShortReport').show();
    }
}); 

var objBujet = [
    { "title": "", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Расчет", "data": "plan", "autowidth": true, "bSortable": false },
    { "title": "Факт, расчет", "data": "factAuto", "autowidth": true, "bSortable": true },
    { "title": "Факт, документ", "data": "factDoc", "autowidth": true, "bSortable": true },
]; 

var objPositionsPreorder = [
    { "title": "№ поз.", "data": "positionNum", "autowidth": true, "bSortable": true },
    { "title": "№ заказа", "data": "CMOSPreOrderId", "autowidth": true, "bSortable": false },
    { "title": "Обозначение", "data": "designation", "autowidth": true, "bSortable": true },
    { "title": "Наименование", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Индекс", "data": "index", "autowidth": true, "bSortable": false },
    { "title": "Расчетный вес, кг", "data": "weight", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
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
    { "title": "Номер поступления 1С", "data": "tnNumber", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true },
    { "title": "Начало", "data": "startDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Срок поставки", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "Окончание", "data": "finishDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Расч. вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Цена, BYN", "data": "cost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Ставка за кг,USD", "data": "rate", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Курс USD/BYN", "data": "curency", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Факт. вес, кг", "data": "factWeight", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Стоимость, BYN", "data": "factCost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Расч. стоимость, BYN", "data": "rfactCost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "РСтоимость-Стоимость", "data": "deviation", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Папка заказа", "data": "folder", "autowidth": true, "bSortable": false }
];

var objSmallReport = [
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Позиция/и", "data": "positions", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Номер поступления 1С", "data": "tnNumber", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true },
    { "title": "Начало", "data": "startDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Срок поставки", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "Окончание", "data": "finishDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Папка заказа", "data": "folder", "autowidth": true, "bSortable": false }  
];  

var objPreOrdersList = [
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "№ план-заказа", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Полуфабрикат", "data": "positionName", "autowidth": true, "bSortable": true },
    { "title": "Расчетный вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": true },
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
    { "title": "Курс, USD", "data": "curency", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 4, '') },
    { "title": "Расчетный вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
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
    { "title": "Срок поставки", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Курс, USD", "data": "curency", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 4, '') },
    { "title": "Расчетный вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Расчетная цена, BYN", "data": "planingCost", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Ставка за кг, USD", "data": "rate", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Позиции", "data": "posList", "autowidth": true, "bSortable": false },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false },
    { "title": "Удалить", "data": "remove", "autowidth": true, "bSortable": false }
];

var objNoClothingOrder = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ИД", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Позиция/и", "data": "positionName", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Срок поставки", "data": "dateGetMail", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Курс, USD", "data": "curency", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 4, '') },
    { "title": "Расчетный вес, кг", "data": "summaryWeight", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Вес, кг", "data": "weight", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Расчетная цена, BYN", "data": "planingCost", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Стоимость, BYN", "data": "cost", "autowidth": true, "bSortable": false, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
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
    { "title": "Номер поступления 1С", "data": "tn", "autowidth": true, "bSortable": true },
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
        "order": [4, "asc"],
        "processing": true,
        "columns": objNoClothingOrder,
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
        "order": [6, "desc"],
        "processing": true,
        "columns": objFullReport,
        "rowCallback": function (row, data, index) {
            if (data.state === "Оприходован") {
                $('td', row).css('background-color', '#00A4CCFF'); 
            }
        },
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
    $("#tableBujet").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetBujetList/" + 0,
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objBujet,
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
    //$('#dateTN').val("");
    $('#cost').val("");
    //$('#factCost').val("");
    $('#planWeight').val("");
    $('#curency').val("");
    $('#rate').val("");
    //$('#factWeightTN').val("");
    $('#filePreorder').val("");
    $('#fileBackorder').val("");
    $('#datePlanningGetMaterials').val("");
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
        datePlanningGetMaterials: $('#datePlanningGetMaterials').val(),
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
            //$('#dateTN').val(result.dateTN);
            $('#cost').val(result.cost);
            //$('#factCost').val(result.factCost); 
            $('#planWeight').val(result.planWeight);
            //$('#factWeightTN').val(result.factWeightTN);
            $('#curency').val(result.curency);
            $('#rate').val(result.rate);
            if (result.manufDate === "null") {
                $('#manufDate').prop('disabled', false);
            }
            else if (result.numberTN === null && result.finDate === null) {
                $('#numberTN').prop('disabled', true);
                //$('#dateTN').prop('disabled', true);
                //$('#factWeightTN').prop('disabled', true); 
                //$('#factCost').prop('disabled', true);
            }
            else if (result.numberTN === null) {
                $('#customerOrderId').prop('disabled', true);
                $('#manufDate').prop('disabled', true);
                //$('#rate').prop('disabled', true);
                $('#numberTN').prop('disabled', false); 
                //$('#dateTN').prop('disabled', false);
                //$('#factWeightTN').prop('disabled', false);
                //$('#factCost').prop('disabled', false);
            }
            else {
                //$('#factWeightTN').prop('disabled', false);
                //$('#factCost').prop('disabled', false);
                $('#customerOrderId').prop('disabled', true);
                $('#manufDate').prop('disabled', true);
                $('#finDate').prop('disabled', false);
                $('#numberTN').prop('disabled', false); 
                //$('#rate').prop('disabled', true); 
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
        //dateTN: $('#dateTN').val(),
        //factCost: $('#factCost').val().replace('.', ','),
        //factWeightTN: $('#factWeightTN').val().replace('.', ','), 
        rate: $('#rate').val().replace('.', ',')
    };
    if (obj.numberTN !== "") {
        var valid = ValidTN();
        if (valid === false) {
            return false;
        }
    }
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

function ValidTN() {
    var isValid = true;
    //if ($('#factCost').val() === "0" || $('#factCost').val() === "" || $('#factCost').val() === 0) {
    //    $('#factCost').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#factCost').css('border-color', 'lightgrey');
    //}
    return isValid;
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

function GetBujetList(id) {
    $("#tableBujet").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOSS/GetBujetList/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": objBujet,
        "scrollY": '70vh',
        "searching": false,
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true
    });
    $('#bujetListModal').modal('show');
}