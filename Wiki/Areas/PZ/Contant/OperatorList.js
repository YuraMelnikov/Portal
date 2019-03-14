$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/Operator/OperatorList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "order": [1],
        "columns": [
            { "title": "Ред", "data": "Id", "autowidth": true, "bSortable": false },
            { "title": "Наименование", "data": "name", "autowidth": true }
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
        name: $('#name').val()
    };
    $.ajax({
        url: "/Operator/Add",
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
    if ($('#name').val() === null) {
        $('#name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#name').css('border-color', 'lightgrey');
    }

    if ($('#id').val() === null) {
        $('#id').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id').css('border-color', 'lightgrey');
    }

    return isValid;
}

function clearTextBox() {
    $("#btnAdd").attr('disabled', false);
    $('#id').val("");
    $('#name').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function getbyID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Operator/GetOperator/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#name').val(result.name);
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
        name: $('#name').val()
    };
    $.ajax({
        url: "/Operator/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#orderModal').modal('hide');
            $('#id').val("");
            $('#name').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}