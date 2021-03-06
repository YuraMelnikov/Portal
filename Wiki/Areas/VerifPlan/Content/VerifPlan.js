﻿$(document).ready(function () {
    startMenu();
});

var objList = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа", "data": "id_PZ_PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": false },
    { "title": "Плановая дата начала проверки (prj)", "data": "verificationDateInPrj", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Плановый срок передачи на проверку", "data": "planDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processIsNullReturnMaxDate },
    { "title": "Фактическая дата передачи на проверку", "data": "factDate", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Дата начала приемки изделия ОТК", "data": "appDate", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Прогнозная дата начала проверки (рук. произв.)", "data": "fixedDateForKO", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Прим. гл. инженера", "data": "planDescription", "autowidth": true, "bSortable": false },
    { "title": "Прим. произв.", "data": "factDescription", "autowidth": true, "bSortable": false },
    { "title": "Прим. ОТК", "data": "appDescription", "autowidth": true, "bSortable": false }
];

var objListLog = [
    { "title": "Дата", "data": "dateAction", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Пользователь", "data": "user", "autowidth": true, "bSortable": false },
    { "title": "Событие", "data": "actionText", "autowidth": true, "bSortable": false }
];

function loadData(listId) {
    if (listId === 1 || listId === "1") {
        listActive();
    }
    else if (listId === 2 || listId === "2") {
        listClose();
    }
    else {
        loadReport();
    }
}

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListActive/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[3, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.state === "Срок не зафиксирован") {
                $('td', row).css('background-color', '#cd5c5c');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
                $(row).find('td:eq(4)').css('color', '#cd5c5c');
            }
            else if (data.state === "Срок зафиксирован") {
                $('td', row).css('background-color', '#3fb0ac');
            }
            else if (data.state === "Сдан ПО") {
                $('td', row).css('background-color', '#fae596');
            }
            if (data.fixedDateForKO !== "null") {
                $(row).find('td:eq(7)').css('background-color', '#FFFF73');
            }
        },
        "cache": false,
        "async": false,
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
    $("#myTableLog").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/GetTableLog/" + 0,
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objListLog,
        "cache": false,
        "async": false,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function listActive() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListActive/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[3, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.state === "Срок не зафиксирован") {
                $('td', row).css('background-color', '#cd5c5c');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
                $(row).find('td:eq(4)').css('color', '#cd5c5c');
            }
            else if (data.state === "Срок зафиксирован") {
                $('td', row).css('background-color', '#3fb0ac');
            }
            else if (data.state === "Сдан ПО") {
                $('td', row).css('background-color', '#fae596');
            }
            if (data.fixedDateForKO !== "null") {
                $(row).find('td:eq(7)').css('background-color', '#FFFF73');
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
}

function listClose() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListClose/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[3, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.state === "Срок не зафиксирован") {
                $('td', row).css('background-color', '#cd5c5c');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
                $(row).find('td:eq(4)').css('color', '#cd5c5c');
            }
            else if (data.state === "Срок зафиксирован") {
                $('td', row).css('background-color', '#3fb0ac');
            }
            else if (data.state === "Сдан ПО") {
                $('td', row).css('background-color', '#fae596');
            }
            if (data.fixedDateForKO !== "null") {
                $(row).find('td:eq(7)').css('background-color', '#FFFF73');
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
}

function get(id) {
    var idUserGroup = 0;
    $.ajax({
        cache: false,
        url: "/Verific/GetUserGroup/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            idUserGroup = result;
        }
    });
    $.ajax({
        cache: false,
        url: "/Verific/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#factDate').css('border-color', 'lightgrey');
            $('#appDate').css('border-color', 'lightgrey');
            $('#fixedDateForKO').css('border-color', 'lightgrey');
            $('#id').val("");
            $('#id_PZ_PlanZakaz').val("");
            $('#fixed').val("");
            $('#state').val("");
            $('#fixetFirstDate').val("");
            $('#planDate').val("");
            $('#planDescription').val("");
            $('#factDate').val("");
            $('#factDescription').val("");
            $('#appDate').val("");
            $('#appDescription').val("");
            $('#verificationDateInPrj').val("");
            $('#fixedDateForKO').val("");
            $('#dateSh').val("");
            $('#id').val(result.id);
            $('#id_PZ_PlanZakaz').val(result.id_PZ_PlanZakaz);
            $('#fixed').val(result.fixed);
            $('#state').val(result.state);
            $('#fixetFirstDate').val(processNull(result.fixetFirstDate));
            $('#planDate').val(processNull(result.planDate));
            $('#planDescription').val(result.planDescription);
            $('#factDate').val(processNull(result.factDate));
            $('#factDescription').val(result.factDescription);
            $('#appDate').val(processNull(result.appDate));
            $('#appDescription').val(result.appDescription);
            $('#verificationDateInPrj').val(processNull(result.verificationDateInPrj));
            $('#fixedDateForKO').val(processNull(result.fixedDateForKO));
            $('#dateSh').val(processNull(result.dateSh));
            $('#btnUpdateTE').hide();
            $('#btnUpdateOTK').hide();
            $('#btnUpdateTM').hide();
            $('#planDate').prop('disabled', true);
            $('#planDescription').prop('disabled', true);
            $('#factDate').prop('disabled', true);
            $('#factDescription').prop('disabled', true);
            $('#fixedDateForKO').prop('disabled', true);
            $('#appDate').prop('disabled', true);
            $('#appDescription').prop('disabled', true);
            if (idUserGroup === 1 || idUserGroup === '1') {
                if (processNull(result.factDate) !== '') {
                    $('#btnUpdateOTK').show();
                    $('#appDate').prop('disabled', false);
                    $('#appDescription').prop('disabled', false);
                }
            }
            else if (idUserGroup === 2 || idUserGroup === '2') {
                $('#btnUpdateTE').show();
                $('#planDate').prop('disabled', false);
                $('#planDescription').prop('disabled', false);
            }
            else if (idUserGroup === 3 || idUserGroup === '3') {
                if (processNull(result.planDate) !== '') {
                    $('#btnUpdateTM').show();
                    $('#factDate').prop('disabled', false);
                    $('#factDescription').prop('disabled', false);
                    $('#fixedDateForKO').prop('disabled', false);
                }
            }
            getTableLog(id);
            $('#verifModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updateTE() {
    var opjTE = {
        id: $('#id').val()
        , planDate: $('#planDate').val()
        , planDescription: $('#planDescription').val()
        , dateSh: $('#dateSh').val()
    };
    $.ajax({
        cache: false,
        url: "/Verific/UpdateTE/",
        data: JSON.stringify(opjTE),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#verifModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateOTK() {
    var opjOTK = {
        id: $('#id').val()
        , appDate: $('#appDate').val()
        , appDescription: $('#appDescription').val()
    };
    var res = validOTK();
    if (res === false) {
        return false;
    }
    $.ajax({
        cache: false,
        url: "/Verific/UpdateOTK/",
        data: JSON.stringify(opjOTK),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#verifModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validOTK() {
    var isValid = true;
    if ($('#appDate').val().trim() === "") {
        $('#appDate').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#appDate').css('border-color', 'lightgrey');
    }
    return isValid;
}

function updateTM() {
    var res = validPM();
    if (res === false) {
        return false;
    }
    var opjTM = {
        id: $('#id').val()
        , factDate: $('#factDate').val()
        , factDescription: $('#factDescription').val()
        , fixedDateForKO: $('#fixedDateForKO').val()
    };
    $.ajax({
        cache: false,
        url: "/Verific/UpdateTM/",
        data: JSON.stringify(opjTM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#verifModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validPM() {
    var isValid = true;
    var factDate = convertRuDateToDateISO($('#factDate').val());
    var fixedDateForKO = new Date();
    fixedDateForKO.setDate(fixedDateForKO.getDate() - 1);
    if ($('#fixedDateForKO').val() !== '') {
        fixedDateForKO = convertRuDateToDateISO($('#fixedDateForKO').val());
    }
    var today = new Date();
    today.setDate(today.getDate() - 1);
    if (factDate < today) {
        $('#factDate').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#factDate').css('border-color', 'lightgrey');
    }
    if (fixedDateForKO < today) {
        $('#fixedDateForKO').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#fixedDateForKO').css('border-color', 'lightgrey');
    }
    return isValid;
}

function processIsNullReturnMaxDate(data) {
    if (data === 'null') {
        return '2099.01.01';
    } else {
        return data;
    }
}

function getTableLog(id) {
    var table = $('#myTableLog').DataTable();
    table.destroy();
    $('#myTableLog').empty();
    $("#myTableLog").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/GetTableLog/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objListLog,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
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