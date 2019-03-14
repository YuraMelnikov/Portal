$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/Client/ClientsList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "order": [1],
        "columns": [
            { "title": "Ред", "data": "Id", "autowidth": true, "bSortable": false },
            { "title": "Краткое наименование", "data": "NameSort", "autowidth": true },
            { "title": "Полное наименование", "data": "Name", "autowidth": true, "bSortable": false },
            { "title": "ИНН/УНН", "data": "INN_UNN", "autowidth": true }],
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
        NameSort: $('#NameSort').val(),
        INN_UNN: $('#INN_UNN').val(),
        Name: $('#Name').val(),
        DCCompany: $('#DCCompany').val(),
        GCompany: $('#GCompany').val()
    };
    $.ajax({
        url: "/Client/Add",
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
    if ($('#NameSort').val() === null) {
        $('#NameSort').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NameSort').css('border-color', 'lightgrey');
    }

    if ($('#INN_UNN').val() === null) {
        $('#INN_UNN').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#INN_UNN').css('border-color', 'lightgrey');
    }

    if ($('#Name').val().trim() === "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }
    return isValid;
}

function clearTextBox() {
    $("#btnAdd").attr('disabled', false);
    $('#id').val("");
    $('#NameSort').val("");
    $('#INN_UNN').val("");
    $('#Name').val("");
    $('#GCompany').val("");
    $('#DCCompany').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function getbyID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Client/GetClient/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#NameSort').val(result.NameSort);
            $('#INN_UNN').val(result.INN_UNN);
            $('#Name').val(result.Name);
            $('#GCompany').val(result.GCompany);
            $('#DCCompany').val(result.DCCompany);
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
        NameSort: $('#NameSort').val(),
        INN_UNN: $('#INN_UNN').val(),
        Name: $('#Name').val(),
        GCompany: $('#GCompany').val(),
        DCCompany: $('#DCCompany').val()
    };
    $.ajax({
        url: "/Client/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#orderModal').modal('hide');
            $('#id').val("");
            $('#NameSort').val("");
            $('#INN_UNN').val("");
            $('#Name').val("");
            $('#GCompany').val("");
            $('#DCCompany').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}