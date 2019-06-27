﻿$(document).ready(function () {
    startMenu();
    if (userGroupId === 1) {
        loadData(2);
    }
});

function loadData(listId) {
    document.getElementById('pageId').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        tasksList(1);
    }
    else if (listId === 2 || listId === "2") {
        myTasksList(2);
    }
    else if (listId === 3 || listId === "3") {
        tasksCloseList(3);
    }
    else if (listId === 4 || listId === "4") {
        TEOList(4);
    }
    else if (listId === 5 || listId === "5") {
        contractList(5);
    }
    else if (listId === 6 || listId === "6") {
        debitList(6);
    }
    else {
        loadData(1);
    }
}

var objTasks = [
    { "title": "Заказ", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Задача", "data": "taskName", "autowidth": true, "bSortable": true },
    { "title": "Заказчик", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Исполнитель", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Плановая дата", "data": "planDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull }
];

var objEditTasks = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Задача", "data": "taskName", "autowidth": true, "bSortable": true },
    { "title": "Заказчик", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Исполнитель", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Плановая дата", "data": "planDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull }
];

var objTEO = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Выручка без НДС", "data": "Rate", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "С/С материалов", "data": "SSM", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "С/С зп рабочих", "data": "SSR", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Коммерчиские издержки", "data": "IzdKom", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "% по кредиту", "data": "IzdPPKredit", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Постоянные издержки", "data": "PI", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "НОП", "data": "NOP", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "КИ Прочие_Ш", "data": "KI_S", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "КИ Прочие", "data": "KI_prochee", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Отпускная цена без НДС", "data": "OtpuskChena", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Валюта", "data": "Currency", "autowidth": true, "bSortable": true },
    { "title": "Сумма НДС", "data": "NDS", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') }
];

var objContract = [
    { "title": "Номер", "data": "PlanZakaz", "autowidth": true }
    , { "title": "Менеджер", "data": "Manager", "autowidth": true }
    , { "title": "Заказчик", "data": "Client", "autowidth": true }
    , { "title": "Кол-во дней на приемку", "data": "KolVoDneyNaPrijemku", "autowidth": true }
    , { "title": "Условия приемки изделия", "data": "PunktDogovoraOSrokahPriemki", "autowidth": true }
    , { "title": "Условия оплаты", "data": "UslovieOplatyText", "autowidth": true }
];

var objDebit = [
     { "title": "Ред.", "data": "edit", "autowidth": true }
    , { "title": "Номер", "data": "PlanZakaz", "autowidth": true }
    , { "title": "Менеджер", "data": "Manager", "autowidth": true }
    , { "title": "Заказчик", "data": "Client", "autowidth": true }
    , { "title": "Кол-во дней на приемку", "data": "KolVoDneyNaPrijemku", "autowidth": true }
    , { "title": "Условия приемки изделия", "data": "PunktDogovoraOSrokahPriemki", "autowidth": true }
    , { "title": "Условия оплаты", "data": "UslovieOplatyText", "autowidth": true }
    , { "title": "Статус", "data": "status", "autowidth": true }
];

function startMenu() {
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/TasksList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTasks,
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

function tasksList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/TasksList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTasks,
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

function myTasksList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/MyTasksList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTasks,
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

function tasksCloseList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/TasksCloseList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTasks,
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

function TEOList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/TEOList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTEO,
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

function contractList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/ContractList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objContract,
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

function debitList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/DebitList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objDebit,
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

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}

//общая форма (галочка)
//1 - зарегистрировать ПЗ
//3 - внести прототипы

function getDefault(id) {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetDefault/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#checkedDefault').prop('checked', false);
            $('#defaultModal').modal('show');
            $('#defaultId').val(result.id);
            $('#defaultTaskName').val(result.taskNmae);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updateDefault() {
    var objDefaultData = {
        id: $('#defaultId').val(),
        checkedDefault: $('#checkedDefault').is(":checked")
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateDefault/",
        data: JSON.stringify(objDefaultData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData(document.getElementById('pageData').innerHTML);
            $('#defaultModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getTEO(id) {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetTEO/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idTEO').val("");
            $('#Currency').val("");
            $('#Rate').val("");
            $('#SSM').val("");
            $('#SSR').val("");
            $('#IzdKom').val("");
            $('#IzdPPKredit').val("");
            $('#PI').val("");
            $('#NOP').val("");
            $('#KI_S').val("");
            $('#KI_prochee').val("");
            $('#OtpuskChena').val("");
            $('#KursValuti').val("");
            $('#NDS').val("");
            $('#idTEO').val(result.id);
            $('#Currency').val(result.Currency);
            $('#Rate').val(result.Rate);
            $('#SSM').val(result.SSM);
            $('#SSR').val(result.SSR);
            $('#IzdKom').val(result.IzdKom);
            $('#IzdPPKredit').val(result.IzdPPKredit);
            $('#PI').val(result.PI);
            $('#NOP').val(result.NOP);
            $('#KI_S').val(result.KI_S);
            $('#KI_prochee').val(result.KI_prochee);
            $('#OtpuskChena').val(result.OtpuskChena);
            $('#KursValuti').val(result.KursValuti);
            $('#NDS').val(result.NDS);
            $('#teoModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
} //?несколько заказов?

function updateTEO() {
    var objTEOData = {
        id: $('#idTEO').val()
        , Currency: $('#Currency').val()
        , Rate: $('#Rate').val()
        , SSM: $('#SSM').val()
        , SSR: $('#SSR').val()
        , IzdKom: $('#IzdKom').val()
        , IzdPPKredit: $('#IzdPPKredit').val()
        , PI: $('#PI').val()
        , NOP: $('#NOP').val()
        , KI_S: $('#KI_S').val()
        , KI_prochee: $('#KI_prochee').val()
        , OtpuskChena: $('#OtpuskChena').val()
        , KursValuti: $('#KursValuti').val()
        , NDS: $('#NDS').val()
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateTEO/",
        data: JSON.stringify(objTEOData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#teoModal').modal('hide');
            loadData(document.getElementById('pageData').innerHTML);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getSetup(id) {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetSetup/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idSetup').val("");
            $('#KolVoDneyNaPrijemku').val("");
            $('#PunktDogovoraOSrokahPriemki').val("");
            $('#UslovieOplatyText').val("");
            $('#UslovieOplatyInt').val("");
            $('#TimeNaRKD').val("");
            $('#RassmotrenieRKD').val("");
            $('#SrokZamechanieRKD').val("");
            $('#userTP').val("");
            $('#idSetup').val(result.id);
            $('#KolVoDneyNaPrijemku').val(result.KolVoDneyNaPrijemku);
            $('#PunktDogovoraOSrokahPriemki').val(result.PunktDogovoraOSrokahPriemki);
            $('#UslovieOplatyText').val(result.UslovieOplatyText);
            $('#UslovieOplatyInt').val(result.UslovieOplatyInt);
            $('#TimeNaRKD').val(result.TimeNaRKD);
            $('#RassmotrenieRKD').val(result.RassmotrenieRKD);
            $('#SrokZamechanieRKD').val(result.SrokZamechanieRKD);
            $('#userTP').val(result.userTP);
            $('#setupModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
} //?несколько заказов?

function updateSetup() {
    var objSetupData = {
        id: $('#idSetup').val()
        , KolVoDneyNaPrijemku: $('#KolVoDneyNaPrijemku').val()
        , PunktDogovoraOSrokahPriemki: $('#PunktDogovoraOSrokahPriemki').val()
        , UslovieOplatyText: $('#UslovieOplatyText').val()
        , UslovieOplatyInt: $('#UslovieOplatyInt').val()
        , TimeNaRKD: $('#TimeNaRKD').val()
        , RassmotrenieRKD: $('#RassmotrenieRKD').val()
        , SrokZamechanieRKD: $('#SrokZamechanieRKD').val()
        , userTP: $('#userTP').val()
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateSetup/",
        data: JSON.stringify(objSetupData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#setupModal').modal('hide');
            loadData(document.getElementById('pageData').innerHTML);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getLetter(id) {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetLetter/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#letterModal').modal('show');
            $('#letterId').val(result.id);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
} //?несколько заказов?

function updateLetter() {
    var objDefaultData = {
        id: $('#letterId').val(),
        checkedDefault: $('#checkedDefault').is(":checked")
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateLetter/",
        data: JSON.stringify(objDefaultData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData(document.getElementById('pageData').innerHTML);
            $('#letterModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//ЖДН
//ТН

function getTN(id) {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetTN/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#tnId').val(result.id);
            $('#tnModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
} //?несколько заказов?

function updateTN() {
    var objDefaultData = {
        id: $('#tnId').val(),
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateTN/",
        data: JSON.stringify(objDefaultData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData(document.getElementById('pageData').innerHTML);
            $('#tnModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}






//приход
//фин. условия поставки

//еще раз проверить все условия