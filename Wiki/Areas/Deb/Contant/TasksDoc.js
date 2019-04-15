$(document).ready(function () {
    loadData();
});

function loadData() {
    document.getElementById('activeFilt').innerHTML = "0";
    $("#myTable").DataTable({
        "ajax": {
            "url": "/ActiveReport/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "rowCallback": function (row, data, index) {
            if (data.id_Debit_PostingOffType === 1 && data.id_Debit_PostingOnType === 1) {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
            if (data.id_Debit_PostingOffType > 1) {
                $('td', row).css('background-color', '#5bc0de');
                $('td', row).css('color', 'black');
            }
            if (data.id_Debit_PostingOnType > 1) {
                $('td', row).css('background-color', '#5cb85c');
                $('td', row).css('color', 'black');
            }
        },
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
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function NoOprih() {
    document.getElementById('activeFilt').innerHTML = "1";
    $("#myTable").DataTable({
        "ajax": {
            "url": "/ActiveReport/NoOprih",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "rowCallback": function (row, data, index) {
            if (data.id_Debit_PostingOffType === 1 && data.id_Debit_PostingOnType === 1) {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
            if (data.id_Debit_PostingOffType > 1) {
                $('td', row).css('background-color', '#5bc0de');
                $('td', row).css('color', 'black');
            }
            if (data.id_Debit_PostingOnType > 1) {
                $('td', row).css('background-color', '#5cb85c');
                $('td', row).css('color', 'black');
            }
        },
        "columns": [
            { "title": "Ред.", "data": "id", "autowidth": true, "bSortable": false }
            , { "title": "Номер", "data": "PlanZakaz", "autowidth": true, "className": 'text-center' }
            , { "title": "Наименование", "data": "Name", "autowidth": true, "bSortable": false, "class": 'colu-300' }
            , { "title": "Менеджер", "data": "Manager", "autowidth": true, "class": 'colu-300' }
            , { "title": "Заказчик", "data": "Client", "autowidth": true }
            , { "title": "Заказ оприходован", "data": "oprihClose", "autowidth": true, "className": 'text-center', "defaultContent": "", "render": localRUStatus }
            , { "title": "Дата оприходования", "data": "dateOprihPlanFact", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Фактическая дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Номер с/ф", "data": "numberSF", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Договорная дата поставки", "data": "DateSupply", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Наличие претензий", "data": "reclamation", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Дата получения претензии", "data": "openReclamation", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull }
            , { "title": "Дата закрытия претензии", "data": "closeReclamation", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull }
            , { "title": "Прим.:", "data": "description", "autowidth": true, "bSortable": false, "class": 'colu-300' }
        ],
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
    $('#id_Debit_PostingOffType').val("");
    $('#id_Debit_PostingOnType').val("");
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
            $('#openReclamation').val(processNull(result.openReclamation));
            $('#closeReclamation').val(processNull(result.closeReclamation));
            $('#conditionAcceptOrder').val(result.conditionAcceptOrder);
            $('#conditionPay').val(result.conditionPay);
            $('#id_Debit_PostingOnType').val(result.id_Debit_PostingOnType);
            $('#id_Debit_PostingOffType').val(result.id_Debit_PostingOffType);
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
    var res = validateUpdate();
    if (res === false) {
        return false;
    }
    var typeObj = {
        id: $('#id').val(),
        description: $('#description').val(),
        dateOprihPlanFact: $('#dateOprihPlanFact').val(),
        id_Debit_PostingOffType: $('#id_Debit_PostingOffType').val(),
        id_Debit_PostingOnType: $('#id_Debit_PostingOnType').val(),
        oprihClose: $('#oprihClose').is(":checked")
    };
    var active = document.getElementById('activeFilt').innerHTML;
    $.ajax({
        url: "/ActiveReport/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (active === "1") {
                NoOprih();
            }
            else {
                loadData();
            }
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

function validateUpdate() {
    var tmp = $('#id_Debit_PostingOnType').val();
    var tmp1 = $('#id_Debit_PostingOffType').val();
    var tmp3 = $('#description').val().trim();
    var isValid = true;
    if ($('#oprihClose').is(":checked") === true) {
        $('#description').css('border-color', 'lightgrey');
        if ($('#id_Debit_PostingOnType').val() === "1") {
            $('#id_Debit_PostingOnType').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#id_Debit_PostingOnType').css('border-color', 'lightgrey');
        }
        if ($('#id_Debit_PostingOffType').val() !== "1") {
            $('#id_Debit_PostingOffType').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#id_Debit_PostingOffType').css('border-color', 'lightgrey');
        }
        if ($('#dateOprihPlanFact').val().trim() === "") {
            $('#dateOprihPlanFact').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#dateOprihPlanFact').css('border-color', 'lightgrey');
        }
    }
    else {
        if ($('#id_Debit_PostingOnType').val() !== "1") {
            $('#id_Debit_PostingOnType').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#id_Debit_PostingOnType').css('border-color', 'lightgrey');
        }
        if ($('#id_Debit_PostingOffType').val() === "1") {
            $('#id_Debit_PostingOffType').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#id_Debit_PostingOffType').css('border-color', 'lightgrey');
        }
        if ($('#description').val().trim() === "") {
            $('#description').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#description').css('border-color', 'lightgrey');
        }
    }
    return isValid;
}