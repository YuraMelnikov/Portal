$(document).ready(function () {
    document.getElementById('pageId').innerHTML = "2";
    document.getElementById('labelList').innerHTML = "Мои задачи";
    $('#pageId').hide();
    $('#teoHide').hide();
    $('#divHideSetup').hide();
    startMenu();
});

function loadData(listId) {
    document.getElementById('pageId').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        tasksList();
        document.getElementById('labelList').innerHTML = "Активные задачи";
    }
    else if (listId === 2 || listId === "2") {
        myTasksList();
        document.getElementById('labelList').innerHTML = "Мои задачи";
    }
    else if (listId === 3 || listId === "3") {
        tasksCloseList();
        document.getElementById('labelList').innerHTML = "Закрытые задачи";
    }
    else if (listId === 4 || listId === "4") {
        TEOList();
        document.getElementById('labelList').innerHTML = "ТЭО";
    }
    else if (listId === 5 || listId === "5") {
        contractList();
        document.getElementById('labelList').innerHTML = "Договорные условия";
    }
    else if (listId === 6 || listId === "6") {
        debitList();
        document.getElementById('labelList').innerHTML = "Поступление средств";
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
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
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
            "url": "/AccountsReceivables/MyTasksList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[4, "asc"]],
        "processing": true,
        "columns": objEditTasks,
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
        "order": [[4, "asc"]],
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
        "order": [[5, "asc"]],
        "processing": true,
        "columns": objEditTasks,
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
        "order": [[4, "asc"]],
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
        "order": [[1, "desc"]],
        "rowCallback": function (row, data, index) {
            if (data.SSM === 0) {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
        },
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
        "order": [[1, "desc"]],
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
            $('#tableData').DataTable().ajax.reload(null, false);
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
            $('#Id_PlanZakaz').val("");
            $('#teoPlanZakaz').val("");
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
            $('#idTEO').val(result.idTEO);
            $('#Id_PlanZakaz').val(result.Id_PlanZakaz);
            $('#teoPlanZakaz').val(result.teoPlanZakaz);
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
}

function updateTEO() {
    var kur = $('#KursValuti').val().replace(",", ".");
    kur = kur * 10000;
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
        , KursValuti: kur
        , Id_PlanZakaz: $('#Id_PlanZakaz').val()
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
            $('#tableData').DataTable().ajax.reload(null, false);
            $('#teoModal').modal('hide');
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
            $('#pzSetup').val("");
            $('#idSetup').val("");
            $('#id_PZ_PlanZakaz').val("");
            $('#KolVoDneyNaPrijemku').val("");
            $('#PunktDogovoraOSrokahPriemki').val("");
            $('#UslovieOplatyText').val("");
            $('#UslovieOplatyInt').val("");
            $('#TimeNaRKD').val("");
            $('#RassmotrenieRKD').val("");
            $('#SrokZamechanieRKD').val("");
            $('#userTP').val("");
            $('#idSetup').val(result.idSetup);
            $('#id_PZ_PlanZakaz').val(result.id_PZ_PlanZakaz);
            $('#pzSetup').val(result.pzSetup);
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
}

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
        , id_PZ_PlanZakaz: $('#id_PZ_PlanZakaz').val()
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
            $('#tableData').DataTable().ajax.reload(null, false);
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
            $('#letterTaskName').val(result.letterTaskName);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updateLetter() {
    var objLetterData = {
        id: $('#letterId').val(),
        orders: $('#orders').val()
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateLetter/",
        data: JSON.stringify(objLetterData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#tableData').DataTable().ajax.reload(null, false);
            $('#letterModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getTN() {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetTN/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#tnId').val("");
            $('#numberTN').val("");
            $('#dateTN').val("");
            $('#numberSF').val("");
            $('#dateSF').val("");
            $('#numCMR').val("");
            $('#dateCMR').val("");
            $('#tnModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updateTN() {
    var objTNData = {
        id: $('#tnId').val(),
        numberTN: $('#numberTN').val(),
        dateTN: $('#dateTN').val(),
        numberSF: $('#numberSF').val(),
        dateSF: $('#dateSF').val(),
        numCMR: $('#numCMR').val(),
        dateCMR: $('#dateCMR').val()
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateTN/",
        data: JSON.stringify(objTNData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#tableData').DataTable().ajax.reload(null, false);
            $('#tnModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getCostSh(id) {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetCostSh/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#costShId').val("");
            $('#transportSum').val("");
            $('#numberOrder').val("");
            $('#ndsSum').val("");
            $('#currency').val("");
            $('#costShId').val(result.id);
            $('#transportSum').val(result.transportSum);
            $('#numberOrder').val(result.numberOrder);
            $('#ndsSum').val(result.ndsSum);
            $('#currency').val(result.currency);
            $('#costShModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updateCostSh() {
    var objCashShData = {
        id: $('#costShId').val(),
        transportSum: $('#transportSum').val(),
        numberOrder: $('#numberOrder').val(),
        ndsSum: $('#ndsSum').val(),
        currency: $('#currency').val()
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateCostSh/",
        data: JSON.stringify(objCashShData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#tableData').DataTable().ajax.reload(null, false);
            $('#costShModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}