$(document).ready(function () {
    document.getElementById('pageId').innerHTML = "2";
    document.getElementById('labelList').innerHTML = "Мои задачи";
    $('#pageId').hide();
    $('#teoHide').hide();
    $('#divHideSetup').hide();
    $('#hidePM').hide();
    $('#pmHide').hide();
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
    else if (listId === 10 || listId === "10") {
        debitList();
        document.getElementById('labelList').innerHTML = "Все заказы";
    }
    else if (listId === 11 || listId === "11") {
        debitActiveList();
        document.getElementById('labelList').innerHTML = "Ожидаение оплаты";
    }
    else if (listId === 12 || listId === "12") {
        debitActiveShList();
        document.getElementById('labelList').innerHTML = "Ожидаение оплаты (отгруженные)";
    }
    else if (listId === 7 || listId === "7") {
        tasksPM();
        document.getElementById('labelList').innerHTML = "Задачи по заказам";
    }
    else {
        loadData(1);
    }
}

var objTasks = [
    { "title": "Заказ", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Задача", "data": "taskName", "autowidth": true, "bSortable": true },
    { "title": "Заказчик", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Исполнитель", "data": "user", "autowidth": true, "bSortable": true }
];

var objEditTasks = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Задача", "data": "taskName", "autowidth": true, "bSortable": true },
    { "title": "Заказчик", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Исполнитель", "data": "user", "autowidth": true, "bSortable": true }
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
    , { "title": "Статус", "data": "status", "autowidth": true }
    , { "title": "Дата отгрузки", "data": "dateSh", "autowidth": true }
    , { "title": "№ счет-фактуры", "data": "sf", "autowidth": true }
    , { "title": "Отпускная цена с НДС", "data": "oc", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') }
    , { "title": "Поступило", "data": "ocPu", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '')  }
    , { "title": "Сумма ДЗ", "data": "otcl", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') }
    , { "title": "Кол-во дней на приемку", "data": "KolVoDneyNaPrijemku", "autowidth": true, "className": 'text-center' }
    , { "title": "Условия приемки изделия", "data": "PunktDogovoraOSrokahPriemki", "autowidth": true }
    , { "title": "Условия оплаты", "data": "UslovieOplatyText", "autowidth": true }
];

function startMenu() {
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/MyTasksList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "asc"]],
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
        "order": [[0, "asc"]],
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
        "order": [[0, "asc"]],
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
        "order": [[1, "desc"]],
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

function debitActiveList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/DebitActiveList",
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

function debitActiveShList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/DebitActiveShList",
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
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#letterId').hide();
            $('#letterId').val(id);
            $('#ofile1').val("");
            $('#datePost').val("");
            $('#numPost').val("");
            $('#datePrihod').val("");
            $('#pZ_PlanZakazLetters').empty();
            for (var j = 0; j < data.length; j++) {
                var optionhtml = '<option value="' + data[j].Value + '">' + data[j].Text + '</option>';
                $("#pZ_PlanZakazLetters").append(optionhtml);
                $('#pZ_PlanZakazLetters').trigger("chosen:updated");
            }
            $('#letterModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function getTN() {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetTN/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#tnId').val("");
            $('#numberTN').val("");
            $('#dateTN').val("");
            $('#numberSF').val("");
            $('#dateSF').val("");
            $('#numCMR').val("");
            $('#dateCMR').val("");
            $('#tnModal').modal('show');
            $('#pZ_PlanZakazTN').empty();
            for (var j = 0; j < data.length; j++) {
                var optionhtml = '<option value="' + data[j].Value + '">' + data[j].Text + '</option>';
                $("#pZ_PlanZakazTN").append(optionhtml);
                $('#pZ_PlanZakazTN').trigger("chosen:updated");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updateTN() {
    var res = validTN();
    if (res === false) {
        return false;
    }
    var objTNData = {
        pZ_PlanZakazTN: $('#pZ_PlanZakazTN').val(),
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

function validTN() {
    $('#numberTN').css('border-color', 'lightgrey');
    $('#dateTN').css('border-color', 'lightgrey');
    $('#numberSF').css('border-color', 'lightgrey');
    $('#dateSF').css('border-color', 'lightgrey');
    $('#numCMR').css('border-color', 'lightgrey');
    $('#dateCMR').css('border-color', 'lightgrey');
    var isValid = true;
    if ($('#pZ_PlanZakazTN').val().length === 0) {
        $('#pZ_PlanZakazTN').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#pZ_PlanZakazTN').css('border-color', 'lightgrey');
    }
    if ($('#numberTN').val().trim() === "") {
        $('#numberTN').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#numberTN').css('border-color', 'lightgrey');
    }
    if ($('#dateTN').val().trim() === "") {
        $('#dateTN').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#dateTN').css('border-color', 'lightgrey');
    }
    if ($('#numberSF').val().trim() === "") {
        $('#numberSF').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#numberSF').css('border-color', 'lightgrey');
    }
    if ($('#dateSF').val().trim() === "") {
        $('#dateSF').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#dateSF').css('border-color', 'lightgrey');
    }
    if ($('#numCMR').val().trim() === "") {
        $('#numCMR').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#numCMR').css('border-color', 'lightgrey');
    }
    if ($('#dateCMR').val().trim() === "") {
        $('#dateCMR').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#dateCMR').css('border-color', 'lightgrey');
    }
    return isValid;
}

function getCostSh() {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetCostSh",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#pZ_PlanZakazSF').empty();
            for (var j = 0; j < data.length; j++) {
                var optionhtml = '<option value="' + data[j].Value + '">' + data[j].Text + '</option>';
                $("#pZ_PlanZakazSF").append(optionhtml);
                $('#pZ_PlanZakazSF').trigger("chosen:updated");
            }
            $('#transportSum').val("");
            $('#numberOrder').val("");
            $('#ndsSum').val("");
            $('#currency').val("");
            $('#costShModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updateCostSh() {
    var res = validSh();
    if (res === false) {
        return false;
    }
    var objCashShData = {
        pZ_PlanZakazSF: $('#pZ_PlanZakazSF').val(),
        transportSum: $('#transportSum').val().replace('.', ','),
        numberOrder: $('#numberOrder').val(),
        currency: $('#currency').val(),
        ndsSum: $('#ndsSum').val().replace('.', ',')
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

function validSh() {
    $('#transportSum').css('border-color', 'lightgrey');
    $('#numberOrder').css('border-color', 'lightgrey');
    $('#currency').css('border-color', 'lightgrey');
    $('#ndsSum').css('border-color', 'lightgrey');
    var isValid = true;
    if ($('#pZ_PlanZakazSF').val().length === 0) {
        $('#pZ_PlanZakazSF').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#pZ_PlanZakazSF').css('border-color', 'lightgrey');
    }
    if ($('#transportSum').val().trim() === "") {
        $('#transportSum').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#transportSum').css('border-color', 'lightgrey');
    }
    if ($('#numberOrder').val().trim() === "") {
        $('#numberOrder').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#numberOrder').css('border-color', 'lightgrey');
    }
    if ($('#currency').val().trim() === "") {
        $('#currency').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#currency').css('border-color', 'lightgrey');
    }
    if ($('#ndsSum').val().trim() === "") {
        $('#ndsSum').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ndsSum').css('border-color', 'lightgrey');
    }
    return isValid;
}

var objTasksPM = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "pmOrderPZName", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "СТ", "data": "powerST", "autowidth": true, "bSortable": true },
    { "title": "ВН/НН", "data": "vnnn", "autowidth": true, "bSortable": true },
    { "title": "Габарит", "data": "gbb", "autowidth": true, "bSortable": true },
    { "title": "Регист.", "data": "orderRegist", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "ТЭО", "data": "teoRegist", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "План КБМ", "data": "planKBM", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "План КБЭ", "data": "planKBE", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Прот. КБМ", "data": "prototypeKBM", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Прот. КБЭ", "data": "prototypeKBE", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Внесены прот. КБМ", "data": "prototypeKBMComplited", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Внесены прот. КБЭ", "data": "prototypeKBEComplited", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Договор", "data": "contractComplited", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Ув. о начале произв.", "data": "mailManuf", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Ув. о готовности к отгрузке", "data": "mailSh", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Отгрузка", "data": "sh", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' },
    { "title": "Ув. о поставке", "data": "mailShComplited", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNullPM, "className": 'text-center' }
];

function tasksPM() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $('#hidePM').show();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/TasksPM",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objTasksPM,
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

function processNullPM(data) {
    if (data === 'null') {
        return '??';
    } else if (data === '1990.01.01') {
        return '';
    } else {
        return data;
    }
}

function clearPM() {
    $('#pmOrderPZName').val("");
    $('#idPZ').val("");
    $('#ProductType').val("");
    $('#powerST').val("");
    $('#vnnn').val("");
    $('#gbb').val("");
    $('#orderRegist').val("");
    $('#teoRegist').val("");
    $('#planKBM').val("");
    $('#planKBE').val("");
    $('#prototypeKBM').val("");
    $('#prototypeKBE').val("");
    $('#prototypeKBMComplited').val("");
    $('#prototypeKBEComplited').val("");
    $('#contractComplited').val("");
    $('#mailManuf').val("");
    $('#mailSh').val("");
    $('#mailShComplited').val("");
}

function getPM(id) {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetPM/" + id,
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#idPZ').val(result[0].idPZ);
            $('#ProductType').val(result[0].ProductType);
            $('#pmOrderPZName').val(result[0].pmOrderPZName);
            $('#powerST').val(result[0].powerST);
            $('#vnnn').val(result[0].vnnn);
            $('#gbb').val(result[0].gbb);
            $('#orderRegist').val(result[0].orderRegist);
            $('#teoRegist').val(result[0].teoRegist);
            $('#planKBM').val(processNullPM(result[0].planKBM));
            $('#planKBE').val(processNullPM(result[0].planKBE));
            $('#prototypeKBM').val(processNullPM(result[0].prototypeKBM));
            $('#prototypeKBE').val(processNullPM(result[0].prototypeKBE));
            $('#prototypeKBMComplited').val(processNullPM(result[0].prototypeKBMComplited));
            $('#prototypeKBEComplited').val(processNullPM(result[0].prototypeKBEComplited));
            $('#contractComplited').val(processNullPM(result[0].contractComplited));
            $('#mailManuf').val(processNullPM(result[0].mailManuf));
            $('#mailShComplited').val(processNullPM(result[0].mailShComplited));
            $('#mailSh').val(processNullPM(result[0].mailSh));
            $('#pmOrderModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updatePM() {
    var objPMData = {
        idPZ: $('#idPZ').val()
        , ProductType: $('#ProductType').val()
        , powerST: $('#powerST').val()
        , vnnn: $('#vnnn').val()
        , gbb: $('#gbb').val()
        , planKBM: $('#planKBM').val()
        , planKBE: $('#planKBE').val()
        , prototypeKBM: $('#prototypeKBM').val()
        , prototypeKBE: $('#prototypeKBE').val()
        , prototypeKBMComplited: $('#prototypeKBMComplited').val()
        , prototypeKBEComplited: $('#prototypeKBEComplited').val()
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdatePM/",
        data: JSON.stringify(objPMData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#pmOrderModal').modal('hide');
            $('#tableData').DataTable().ajax.reload(null, false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getLetterPM(id) {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetLetterPM/" + id,
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#idTaskPM').val(id);
            $('#ofile1PM').val("");
            $('#datePost').val("");
            $('#numPost').val("");
            $('#datePrihod').val("");
            $('#pZ_PlanZakazLettersPM').empty();
            for (var j = 0; j < data.length; j++) {
                var optionhtml = '<option value="' + data[j].Value + '">' + data[j].Text + '</option>';
                $("#pZ_PlanZakazLettersPM").append(optionhtml);
                $('#pZ_PlanZakazLettersPM').trigger("chosen:updated");
            }
            $('#letterModalPM').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function getRKD() {
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/GetRKD/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#rkdModal').modal('show');
            $('#pZ_PlanZakazRKD').empty();
            for (var j = 0; j < data.length; j++) {
                var optionhtml = '<option value="' + data[j].Value + '">' + data[j].Text + '</option>';
                $("#pZ_PlanZakazRKD").append(optionhtml);
                $('#pZ_PlanZakazRKD').trigger("chosen:updated");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updateRKD() {
    var objRKDData = {
        pZ_PlanZakazRKD: $('#pZ_PlanZakazRKD').val()
    };
    $.ajax({
        cache: false,
        url: "/AccountsReceivables/UpdateRKD/",
        data: JSON.stringify(objRKDData),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#rkdModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//getDebTask
//updateDebTask
//addDebTask
//removeDebTask