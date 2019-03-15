$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/DTasks/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": [
            { "title": "Номер", "data": "PlanZakaz", "autowidth": true }
            , { "title": "Наименование", "data": "Name", "autowidth": true, "bSortable": false }
            , { "title": "Менеджер", "data": "Manager", "autowidth": true }
            , { "title": "Заказчик", "data": "Client", "autowidth": true }
            , { "title": "Фактическая дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true, "bSortable": false }
            , { "title": "Номер с/ф", "data": "numberSF", "autowidth": true, "bSortable": false }
            , { "title": "Фактическая дата доставки", "data": "datePrihod", "autowidth": true, "bSortable": false }
            , { "title": "Договорная дата поставки", "data": "DateSupply", "autowidth": true, "bSortable": false }
            , { "title": "Наличие претензий", "data": "idReclamation", "autowidth": true, "bSortable": false }
            , { "title": "Дата получения претензии", "data": "openReclamation", "autowidth": true, "bSortable": false }
            , { "title": "Дата закрытия претензии", "data": "closeReclamation", "autowidth": true, "bSortable": false }
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

function clearTextBox() {
    $("#btnAdd").attr('disabled', false);
    $('#id').val("");
    $('#PlanZakaz').val("");
    $('#Name').val("");
    $('#Manager').val("");
    $('#Client').val("");
    $('#dataOtgruzkiBP').val("");
    $('#numberSF').val("");
    $('#datePrihod').val("");
    $('#closeId').prop('checked', false);
    $('#DateSupply').val("");
    $('#idReclamation').val("");
    $('#openReclamation').val("");
    $('#closeReclamation').val("");
    $('#dateClose').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function getbyID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/DTasks/GetId/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#PlanZakaz').val(result.PlanZakaz);
            $('#Name').val(result.Name);
            $('#Manager').val(result.Manager);
            $('#Client').val(result.Client);
            $('#dataOtgruzkiBP').val(result.dataOtgruzkiBP);
            $('#numberSF').val(result.numberSF);
            $('#datePrihod').val(result.datePrihod);
            $('#DateSupply').val(result.DateSupply);
            $('#idReclamation').val(result.idReclamation);
            $('#openReclamation').val(result.openReclamation);
            $('#closeReclamation').val(result.closeReclamation);
            $('#closeId').prop('checked', result.active);
            $('#dateClose').val(result.dateClose);
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
    var typeObj = {
        id: $('#id').val(),
        dateClose: $('#dateClose').val(),
        closeId: $('#closeId').is(":checked")
    };
    $.ajax({
        url: "/DTasks/Update",
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