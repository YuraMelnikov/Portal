$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/Kurator/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "order": [1],
        "columns": [
            { "title": "Ред", "data": "Id", "autowidth": true, "bSortable": false },
            { "title": "Фамилия", "data": "fio", "autowidth": true },
            { "title": "Имя", "data": "i", "autowidth": true, "bSortable": false},
            { "title": "Отчество", "data": "o", "autowidth": true, "bSortable": false},
            { "title": "Заказчик", "data": "id_PZ_Client", "autowidth": true },
            { "title": "email", "data": "email", "autowidth": true, "bSortable": false},
            { "title": "Контактные данные", "data": "phone", "autowidth": true, "bSortable": false},
            { "title": "Должность", "data": "position", "autowidth": true, "bSortable": false }
        ],
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function Add() {
    var res = validate();
    if (res === false) {
        return false;
    }
    $("#btnAdd").attr('disabled', true);
    var typeObj = {
        id: $('#id').val(),
        fio: $('#fio').val(),
        i: $('#i').val(),
        o: $('#o').val(),
        id_PZ_Client: $('#id_PZ_Client').val(),
        email: $('#email').val(),
        phone: $('#phone').val(),
        position: $('#position').val()
    };
    $.ajax({
        url: "/Kurator/Add",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#orderModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#fio').val() === null) {
        $('#fio').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#fio').css('border-color', 'lightgrey');
    }
    return isValid;
}

function clearTextBox() {
    $("#btnAdd").attr('disabled', false);
    $('#id').val("");
    $('#fio').val("");
    $('#i').val("");
    $('#o').val("");
    $('#id_PZ_Client').val("");
    $('#email').val("");
    $('#phone').val("");
    $('#position').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function getbyID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Kurator/GetId/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#fio').val(result.fio);
            $('#i').val(result.i);
            $('#o').val(result.o);
            $('#id_PZ_Client').val(result.id_PZ_Client);
            $('#email').val(result.email);
            $('#phone').val(result.phone);
            $('#position').val(result.position);
            $('#orderModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Update() {
    var res = validate();
    if (res === false) {
        return false;
    }
    var typeObj = {
        id: $('#id').val(),
        fio: $('#fio').val(),
        i: $('#i').val(),
        o: $('#o').val(),
        id_PZ_Client: $('#id_PZ_Client').val(),
        email: $('#email').val(),
        phone: $('#phone').val(),
        position: $('#position').val()
    };
    $.ajax({
        url: "/Kurator/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#orderModal').modal('hide');
            $('#id').val("");
            $('#fio').val("");
            $('#i').val("");
            $('#o').val("");
            $('#id_PZ_Client').val("");
            $('#email').val("");
            $('#phone').val("");
            $('#position').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}