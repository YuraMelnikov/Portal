$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/Upload/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": [
            { "title": "Статус", "data": "status", "autowidth": true }
            , { "title": "Ред.", "data": "edit", "autowidth": true }
            , { "title": "Номер", "data": "PlanZakaz", "autowidth": true }
            , { "title": "Наименование", "data": "Name", "autowidth": true, "bSortable": false }
            , { "title": "Менеджер", "data": "Manager", "autowidth": true }
            , { "title": "Заказчик", "data": "Client", "autowidth": true }
            , { "title": "Фактическая дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true, "bSortable": false }
            , { "title": "Договорная дата поставки", "data": "DateSupply", "autowidth": true, "bSortable": false }
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

function CreateTask38() {
    $("#btnCreateTask38").attr('disabled', true);
    var typeObj = {
        PZ: $('#PZ').val()
    };
    $.ajax({
        url: "/Upload/СreateTask38С",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#createTask38').modal('hide');
            $("#btnCreateTask38").attr('disabled', false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID() {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $('#createTask38').modal('show');
    $('#btnUpdate').show();
    $('#btnAdd').hide();
    return false;
}