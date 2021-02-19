$(document).ready(function () {
    StartMenu();
}); 

var objList = [
    { "title": "№ заказа", "data": "parent", "autowidth": true, "bSortable": true },
    { "title": "Вирт. зак.", "data": "childs", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Заказчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Наименование", "data": "name", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Наименование по ТУ", "data": "nameTU", "autowidth": true, "bSortable": false },
    { "title": "Менеджер", "data": "manager", "autowidth": true, "bSortable": true },
    { "title": "Договор", "data": "contract", "autowidth": true, "bSortable": true },
    { "title": "Дата дог.", "data": "contractDate", "autowidth": true, "bSortable": false },
    { "title": "Спец.", "data": "spetification", "autowidth": true, "bSortable": false },
    { "title": "Дата спец.", "data": "spetificationDate", "autowidth": true, "bSortable": false },
    { "title": "Отгрузка", "data": "shDate", "autowidth": true, "bSortable": true },
    { "title": "Уд.", "data": "removeLink", "autowidth": true, "bSortable": false },
];

function StartMenu() {
    $("#tableList").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CTO/GetList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [0, "desc"],
        "processing": true,
        "columns": objList,
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
}

function Create() {
    CleanerModals();
    $('#CrossModal').modal('show');
}

function CleanerModals() {
    $('#parent').val("");
    $('#childs').chosen();
    $('#childs').trigger('chosen:updated');
}

function Valid() {
    var isValid = true; 
    if ($('#parent').val() === null) {
        isValid = false;
    }
    if ($('#childs').val().length === 0) {
        isValid = false;
    }
    return isValid;
}

function Add() {
    var valid = Valid();
    if (valid === false) {
        return false;
    }
    var obj = {
        parent: $('#parent').val(),
        childs: $('#childs').val()
    };
    $.ajax({
        cache: false,
        url: "/CTO/Add",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#tableList').DataTable().ajax.reload(null, false);
            $('#CrossModal').modal('hide');
        }
    });
}

function Remove(id) {
    $.ajax({
        cache: false,
        url: "/CTO/Remove/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#tableList').DataTable().ajax.reload(null, false);
        }
    });
    return false;
}