$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/ActiveReport/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": [
            { "title": "Ред.", "data": "id", "autowidth": true, "bSortable": false }
            , { "title": "Номер", "data": "PlanZakaz", "autowidth": true, "className": 'text-center'}
            , { "title": "Наименование", "data": "Name", "autowidth": true, "bSortable": false, "class": 'colu-300'}
            , { "title": "Менеджер", "data": "Manager", "autowidth": true, "class": 'colu-300'}
            , { "title": "Заказчик", "data": "Client", "autowidth": true }
            , { "title": "Заказ оприходован", "data": "oprihClose", "autowidth": true, "className": 'text-center', "defaultContent": "", "render": localRUStatus }
            , { "title": "Дата оприходования", "data": "dateOprihPlanFact", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Фактическая дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true, "bSortable": false, "className": 'text-center'}
            , { "title": "Номер с/ф", "data": "numberSF", "autowidth": true, "bSortable": false, "className": 'text-center'}
            , { "title": "Договорная дата поставки", "data": "DateSupply", "autowidth": true, "bSortable": false, "className": 'text-center'}
            , { "title": "Наличие претензий", "data": "reclamation", "autowidth": true, "bSortable": false, "className": 'text-center'}
            , { "title": "Дата получения претензии", "data": "openReclamation", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull }
            , { "title": "Дата закрытия претензии", "data": "closeReclamation", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull }
            , { "title": "Прим.:", "data": "description", "autowidth": true, "bSortable": false, "class": 'colu-300' }
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
    $('#description').val("");
    $('#oprihClose').prop('checked', true);
    $('#dateOprihPlanFact').val("");

    $('#PlanZakaz').val("");
    $('#Name').val("");
    $('#Manager').val("");
    $('#Client').val("");
    $('#dataOtgruzkiBP').val("");
    $('#DateSupply').val("");

    $('#numberSF').val("");
    $('#reclamation').val("");
    $('#openReclamation').val("");
    $('#closeReclamation').val("");

    $('#costNDS').val("");
    $('#costNotNDS').val("");
    $('#costWithHDS').val("");

    $('#conditionPay').val("");
    $('#conditionAcceptOrder').val("");
    
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function getbyID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/ActiveReport/GetId/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#description').val(result.description);
            $('#oprihClose').prop('checked', result.oprihClose);
            $('#dateOprihPlanFact').val(result.dateOprihPlanFact);

            $('#PlanZakaz').val(result.PlanZakaz);
            $('#Name').val(result.Name);
            $('#Manager').val(result.Manager);
            $('#Client').val(result.Client);
            $('#dataOtgruzkiBP').val(result.dataOtgruzkiBP);
            $('#DateSupply').val(result.DateSupply);

            $('#costNDS').val(result.costNDS);
            $('#costNotNDS').val(result.costNotNDS);
            $('#costWithHDS').val(result.costWithHDS);

            $('#numberSF').val(result.numberSF);
            $('#reclamation').val(result.reclamation);
            $('#openReclamation').val(result.openReclamation);
            $('#closeReclamation').val(result.closeReclamation);

            $('#conditionAcceptOrder').val(result.conditionAcceptOrder);
            $('#conditionPay').val(result.conditionPay);

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
        description: $('#description').val(),
        dateOprihPlanFact: $('#dateOprihPlanFact').val(),
        oprihClose: $('#oprihClose').is(":checked")
    };
    $.ajax({
        url: "/ActiveReport/Update",
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

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}

function localRUStatus(data) {
    if (data === true) {
        return 'Оприходован';
    } else {
        return 'Не оприходован';
    }
}