$(document).ready(function () {
    
});

var objOrders = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ИД.", "data": "id", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "№ Заказа/ов", "data": "orderNum", "autowidth": true, "bSortable": true },
    { "title": "Создана", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Дата создания", "data": "dateCreate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull, "className": 'text-center' },
    { "title": "Дата закрытия", "data": "ver", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Дата удаления", "data": "dateOpen", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Статус", "data": "contractDate", "autowidth": true, "bSortable": true, "className": 'text-center' }
];

function ClearOrderField() {
    $('#ordersList').val("");
    $('#ordersList').chosen();
    $('#ordersList').trigger('chosen:updated');
}

function CreateNewOrder() {
    ClearOrderField();
    SetupBtnForCrete();
}

function SetupBtnForCrete() {
    $('#btnAdd').show();
    $('#btnRemove').hide();
    $('#btnUpdate').hide();
}

function SetupForEdit() {
    $('#btnAdd').hide();
    $('#btnRemove').show();
    $('#btnUpdate').show();
}

function Add() {
    var res = Valid();
    if (res === false) {
        return false;
    }
    var obj = {
        ordersList: $('#ordersList').val()
    };
    $.ajax({
        cache: false,
        url: "/OrderFeTable/Add",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#ordersTable').DataTable().ajax.reload(null, false);
            $('#orderModal').modal('hide');
        }
    });
}

function Valid() {
    var isValid = true;
    if ($('#ordersList').val().length === 0) {
        $('#ordersList').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ordersList').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Get(id) {
    ClearOrderField();
    $.ajax({
        cache: false,
        url: "/OrderFeTable/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $("#id_OrdersTables").val(result.id_OrdersTables);
            $("#ordersList").val(result.ordersList).trigger("chosen:updated");
            SetupForEdit();
            $('#orderModal').modal('show');
        }
    });
}

function Update() {
    var res = Valid();
    if (res === false) {
        return false;
    }
    var obj = {
        id_OrdersTables: $('#id_OrdersTables').val(),
        ordersList: $('#ordersList').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/Update",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#ordersTable').DataTable().ajax.reload(null, false);
            $("#orderModal").modal('hide');
        }
    });
}

function Remove() {
    var obj = {
        id_OrdersTables: $('#id_OrdersTables').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/Remove",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#ordersTable').DataTable().ajax.reload(null, false);
            $("#orderModal").modal('hide');
        }
    });
}

//LoadData