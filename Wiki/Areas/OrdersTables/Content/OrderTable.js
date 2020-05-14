$(document).ready(function () {
    LoadData("active");
});

var objOrders = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ИД.", "data": "id", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "№ Заказа/ов", "data": "orderNum", "autowidth": true, "bSortable": true },
    { "title": "Создана", "data": "user", "autowidth": true, "bSortable": true, "className": 'text-center'  },
    { "title": "Дата создания", "data": "dateCreate", "autowidth": true, "bSortable": true, "className": 'text-center'  },
    { "title": "Дата закрытия", "data": "dateClose", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Дата удаления", "data": "dateRemove", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Уд.", "data": "removeLink", "autowidth": true, "bSortable": true, "className": 'text-center' }
];

//function StartMenu() {
//    $("#orderModal").DataTable({
//        "dom": 'Bfrtip',
//        "buttons": [
//            'copyHtml5',
//            'excelHtml5',
//            'csvHtml5'
//        ],
//        "ajax": {
//            "cache": false,
//            "url": "/Remarks/ActiveReclamation",
//            "type": "POST",
//            "datatype": "json"
//        },
//        "bDestroy": true,
//        "order": [[0, "desc"]],
//        "processing": true,
//        "columns": objRemarksList,
//        "rowCallback": function (row, data, index) {
//            if (data.Close !== "активная") {
//                $('td', row).css('background-color', '#d9534f');
//                $('td', row).css('color', 'white');
//            }
//        },
//        "scrollY": '75vh',
//        "scrollX": true,
//        "paging": false,
//        "info": false,
//        "scrollCollapse": true,
//        "language": {
//            "zeroRecords": "Отсутствуют записи",
//            "infoEmpty": "Отсутствуют записи",
//            "search": "Поиск"
//        }
//    });
//}

function LoadData(type) {
    var table = $('#ordersTable').DataTable();
    if (type === "active") {
        table.destroy();
        $('#ordersTable').empty();
        $("#ordersTable").DataTable({
            "ajax": {
                "cache": false,
                "url": "/OrderFeTable/ListActive/",
                "type": "POST",
                "datatype": "json"
            },
            "bDestroy": true,
            "order": [[2, "asc"]],
            "processing": true,
            "columns": objOrders,
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
    else {
        table.destroy();
        $('#ordersTable').empty();
        $("#ordersTable").DataTable({
            "ajax": {
                "cache": false,
                "url": "/OrderFeTable/ListClose/",
                "type": "POST",
                "datatype": "json"
            },
            "bDestroy": true,
            "order": [[2, "asc"]],
            "processing": true,
            "columns": objOrders,
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
}

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