﻿$(document).ready(function () {
    $('#btnExpert').hide();
    startMenu();
    $('#pageData').hide();
});

function loadData(listId) {
    document.getElementById('pageData').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        $('#btnExpert').hide();
        activeTA();
    }
    else if (listId === 2 || listId === "2") {
        $('#btnExpert').hide();
        protocols();
    }
    else if (listId === 3 || listId === "3") {
        $('#btnExpert').hide();
        allDataProtocols();
    }
    else if (listId === 4 || listId === "4") {
        $('#btnExpert').hide();
        GetRemarksActiveWorkList();
    }
    else {
        $('#btnExpert').hide();
        activeTA();
    }
}

var objRemarksList = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "LinkToEdit", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "Orders", "autowidth": true, "bSortable": false },
    { "title": "Описание", "data": "TextReclamation", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Решение", "data": "Decision", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Прим.", "data": "DescriptionReclamation", "autowidth": true, "bSortable": false },
    { "title": "Направил на ТС", "data": "UserToTA", "autowidth": true, "bSortable": true },
    { "title": "Создал", "data": "UserCreate", "autowidth": true, "bSortable": true },
    { "title": "Ответственное СП", "data": "DevisionReclamation", "autowidth": true, "bSortable": true }
];

var objAdviceTask = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "№", "data": "idTask", "autowidth": true, "bSortable": true },
    { "title": "Задача", "data": "textTask", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Результат", "data": "textAnswer", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Срок", "data": "deadline", "autowidth": true, "bSortable": true },
    { "title": "Закрыта", "data": "dateComplited", "autowidth": true, "bSortable": false },
    { "title": "Исполнитель", "data": "answers", "autowidth": true, "bSortable": true }
];

function GetAdviseTasks(id) {
    var table = $('#taskTable').DataTable();
    table.destroy();
    $('#taskTable').empty();
    $("#taskTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetAdviseTasks/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[4, "asc"]],
        "processing": true,
        "columns": objAdviceTask,
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
}

function GetAdviceTask(id) {
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/GetAdviceTask/" + id,
        typr: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idAdviceTask').val(result.idAdviceTask);
            $('#adviceUser').val(result.adviceUser);
            $('#adviceDeadline').val(result.adviceDeadline);
            $('#adviceTextTask').val(result.adviceTextTask);
            $('#adviceAnswerTask').val(result.adviceAnswerTask);
            $('#dateComplitedTask').val(result.dateComplitedTask);
            $('#adviceTaskModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function UpdateAdviceTask() {
    var objRemark = {
        idAdviceTask: $('#idAdviceTask').val(),
        adviceDeadline: $('#adviceDeadline').val(),
        adviceTextTask: $('#adviceTextTask').val(),
        dateComplitedTask: $('#dateComplitedTask').val(),
        adviceAnswerTask: $('#adviceAnswerTask').val()
    };
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/UpdateAdviceTask/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#adviceTaskModal').modal('hide');
            $('#taskTable').DataTable().ajax.reload(null, false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

var objRemarksListNoEdit = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "Заказ", "data": "Orders", "autowidth": true, "bSortable": false },
    { "title": "Описание", "data": "TextReclamation", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Решение", "data": "Decision", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Прим.", "data": "DescriptionReclamation", "autowidth": true, "bSortable": false },
    { "title": "Направил на ТС", "data": "UserToTA", "autowidth": true, "bSortable": true },
    { "title": "Создал", "data": "UserCreate", "autowidth": true, "bSortable": true },
    { "title": "Ответственное СП", "data": "DevisionReclamation", "autowidth": true, "bSortable": true }
];

var objProtocol = [
    { "title": "№", "data": "Id_Protocol", "autowidth": true, "bSortable": true },
    { "title": "См", "data": "LinkToView", "autowidth": true, "bSortable": false },
    { "title": "Дата заседания", "data": "DateProtocol", "autowidth": true, "bSortable": true },
    { "title": "Кол-во рекламаций", "data": "CountReclamation", "autowidth": true, "bSortable": true }
];

var objNoClose = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "LinkToEdit", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "Orders", "autowidth": true, "bSortable": false },
    { "title": "Описание", "data": "TextReclamation", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Решение", "data": "Decision", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Прим.", "data": "DescriptionReclamation", "autowidth": true, "bSortable": false },
    { "title": "Ответственный исполнитель", "data": "Id_AspNetUserResponsible", "autowidth": true, "bSortable": true },
    { "title": "Срок", "data": "Deadline", "autowidth": true, "bSortable": true }
];

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetActiveTA",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "rowCallback": function (row, data, index) {
            if (data.Decision === "") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
            }
        },
        "columns": objRemarksListNoEdit,
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
    $("#taskTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetAdviseTasks/" + 50,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objAdviceTask,
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
}

function activeTA() {
    var countRedPosition = 0;
    var countPosition = 0;
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetActiveTA",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "processing": true,
        "rowCallback": function (row, data, index) {
            if (data.Decision === "") {
                countRedPosition++;
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
            }
        },
        "columns": objRemarksList,
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
    countPosition = table.data().count();
    if (countRedPosition === 0 && countPosition > 0) {
        $('#btnExpert').show();
    }
}

function GetRemarksActiveWorkList() {
    var countRedPosition = 0;
    var countPosition = 0;
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetNoCloseTA",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "processing": true,
        "columns": objNoClose,
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

function protocols() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetProtocols",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objProtocol,
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

function allDataProtocols() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetAllRemarks",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objRemarksListNoEdit,
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

function getTAEdit(id) {
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/GetTA/" + id,
        typr: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            disabledFalse();
            $('#id_AspNetUserTask').val("");
            $('#textTask').val("");
            $('#deadlineTask').val("");
            $('#id').val(result.id);
            $('#idR').val(result.id);
            $('#userUploadReclamation').val(result.userUploadReclamation);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#orders').val(result.orders);
            $('#userCreateReclamation').val(result.userCreateReclamation);
            $('#devisionReclamation').val(result.devisionReclamation);
            $('#reclamationText').val(result.reclamationText);
            $('#answerHistiryText').val(result.answerHistiryText);
            $('#id_AspNetUserResponsible').val(result.id_AspNetUserResponsible);
            $('#deadline').val(result.deadline);
            $('#close').prop('checked', result.close);
            $('#viewReclamation').modal('show');
            $('#btnUpdate').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    GetAdviseTasks(id);
    return false;
}

function update() {
    var objRemark = {
        id: $('#id').val(),
        text: $('#text').val(),
        description: $('#description').val(),
        close: $('#close').is(":checked"),
        deadline: $('#deadline').val(),
        id_AspNetUserResponsible: $('#id_AspNetUserResponsible').val()
    };
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/Update/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#viewReclamation').modal('hide');
            $('#myTable').DataTable().ajax.reload(null, false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function disabledFalse() {
    $('#text').prop('disabled', false);
    $('#description').prop('disabled', false);
    $('#id_AspNetUsersCorrect').prop('disabled', false);
    $('#dateCorrect').prop('disabled', false);
}

function disabledTrue() {
    $('#text').prop('disabled', true);
    $('#description').prop('disabled', true);
    $('#id_AspNetUsersCorrect').prop('disabled', true);
    $('#dateCorrect').prop('disabled', true);
}

function createNewProtocol() {
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/CreateNewProtocol/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData(document.getElementById('pageData').innerHTML);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getProtocol(id) {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetProtocol/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objRemarksListNoEdit,
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

function AddTask() {
    $("#btnAddTask").attr('disabled', true);
    var objRemark = {
        id_AspNetUserTask: $('#id_AspNetUserTask').val(),
        textTask: $('#textTask').val(),
        idR: $('#idR').val(),
        deadlineTask: $('#deadlineTask').val()
    };
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/AddTask/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#taskTable').DataTable().ajax.reload(null, false);
            $("#btnAddTask").attr('disabled', false);
            $('#id_AspNetUserTask').val("");
            $('#textTask').val("");
            $('#deadlineTask').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
            $("#btnAddTask").attr('disabled', false);
        }
    });
}