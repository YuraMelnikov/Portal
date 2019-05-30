// 01 - OS
// 02 - MKO
// 03 - Admin
// 04 - userKO
// 05 - users

$(document).ready(function () {
    $('#pageId').hide();
    $('#btnAddOrder').hide();
    $('#btnReOrder').hide();
    $('#toWorkTable').hide();
    $('#toManufTable').hide();
    $('#toCloseTable').hide();
    startMenu();
});

function loadData(listId) {
    document.getElementById('pageId').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        loadReport();
    }
    else if (listId === 2 || listId === "2") {
        loadOS();
    }
    else {
        loadReport();
    }
}

var objSmallReport = [
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Изг. дн.", "data": "day", "autowidth": true, "bSortable": true },
    { "title": "Дата размещения", "data": "workDateTime", "autowidth": true, "bSortable": true },
    { "title": "Дата исполнения", "data": "manufDate", "autowidth": true, "bSortable": true },
    { "title": "Дата поступления", "data": "finDate", "autowidth": true, "bSortable": true },
    { "title": "№ заявки", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Папка заказа", "data": "folder", "autowidth": true, "bSortable": true }
];

var objFullReport = [
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Изг. дн.", "data": "day", "autowidth": true, "bSortable": true },
    { "title": "Дата размещения", "data": "workDateTime", "autowidth": true, "bSortable": true },
    { "title": "Начальная цена, б/НДС (BYN)", "data": "workCost", "autowidth": true, "bSortable": true },
    { "title": "Дата исполнения", "data": "manufDate", "autowidth": true, "bSortable": true },
    { "title": "Подтвержденная цена, б/НДС (BYN)", "data": "manufCost", "autowidth": true, "bSortable": true },
    { "title": "Дата поступления", "data": "finDate", "autowidth": true, "bSortable": true },
    { "title": "Стоимость, б/НДС (BYN)", "data": "finCost", "autowidth": true, "bSortable": true },
    { "title": "№ заявки", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Папка заказа", "data": "folder", "autowidth": true, "bSortable": true }
];

var objWork = [
    { "title": "Ред", "data": "Id_Reclamation", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Описание заявки", "data": "EditLinkJS", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Срок", "data": "Devision", "autowidth": true, "bSortable": true },
    { "title": "Цена/стоимость", "data": "Devision", "autowidth": true, "bSortable": true },
    { "title": "Дата/время размещения", "data": "ViewLinkJS", "autowidth": true, "bSortable": true },
    { "title": "Дата/время создания", "data": "ViewLinkJS", "autowidth": true, "bSortable": true },
    { "title": "Кто разместил", "data": "Devision", "autowidth": true, "bSortable": true },
    { "title": "Excel", "data": "Devision", "autowidth": true, "bSortable": true },
    { "title": "№ заявки", "data": "Devision", "autowidth": true, "bSortable": true }
];

function startMenu() {
    $("#reportTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ReportTable",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objSmallReport,
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

function smallReport() {
    var table = $('#reportTable').DataTable();
    table.destroy();
    $('#reportTable').empty();
    $("#reportTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ReportTable",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objSmallReport,
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

function fullReport() {
    var table = $('#reportTable').DataTable();
    table.destroy();
    $('#reportTable').empty();
    $("#reportTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ReportTable",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objFullReport,
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

function toWork() {
    var table = $('#toWorkTable').DataTable();
    table.destroy();
    $('#toWorkTable').empty();
    $("#toWorkTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToWork",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objWork,
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

function toManuf() {
    var table = $('#toManufTable').DataTable();
    table.destroy();
    $('#toManufTable').empty();
    $("#toManufTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToManuf",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objWork,
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

function toClose() {
    var table = $('#toCloseTable').DataTable();
    table.destroy();
    $('#toCloseTable').empty();
    $("#toCloseTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToClose",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objWork,
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

function desckTopOS() {
    fullReport();
    toWork();
    toManuf();
    toClose();
}

function loadReport() {
    if (userGroupId < 4) {
        $('#toWorkTable').hide();
        $('#toManufTable').hide();
        $('#toCloseTable').hide();
        fullReport();
    }
    else {
        $('#toWorkTable').hide();
        $('#toManufTable').hide();
        $('#toCloseTable').hide();
        smallReport();
    }
}

function loadOS() {
    if (userGroupId === 1) {
        $('#toWorkTable').show();
        $('#toManufTable').show();
        $('#toCloseTable').show();
        desckTopOS();
    }
    else {
        $('#toWorkTable').hide();
        $('#toManufTable').hide();
        $('#toCloseTable').hide();
        smallReport();
    }
}

function clearTextBox() {
    $('#id_PlanZakaz').prop('disabled', false);
    $('#id_CMO_TypeProduct').prop('disabled', false);
    $('#id_CMO_Company').prop('disabled', false);
    $('#workIn').prop('disabled', true);
    $('#workDateTime').prop('disabled', true);
    $('#workCost').prop('disabled', true);
    $('#workComplitet').prop('disabled', true);
    $('#manufIn').prop('disabled', true);
    $('#manufDate').prop('disabled', true);
    $('#manufCost').prop('disabled', true);
    $('#manufComplited').prop('disabled', true);
    $('#finIn').prop('disabled', true);
    $('#finDate').prop('disabled', true);
    $('#finCost').prop('disabled', true);
    $('#finComplited').prop('disabled', true);
    $('#id_PlanZakaz').val("");
    $('#id_CMO_TypeProduct').val("");
    $('#id_CMO_Company').val("");
    $('#workIn').prop('checked', false);
    $('#workDateTime').val("");
    $('#workCost').val("");
    $('#workComplitet').prop('checked', false);
    $('#manufIn').prop('checked', false);
    $('#manufDate').val("");
    $('#manufCost').val("");
    $('#manufComplited').prop('checked', false);
    $('#finIn').prop('checked', false);
    $('#finDate').val("");
    $('#finCost').val("");
    $('#finComplited').prop('checked', false);
}

function clearTextBoxUpdate() {
    $('#id_PlanZakaz').prop('disabled', true);
    $('#id_CMO_TypeProduct').prop('disabled', true);
    $('#id_CMO_Company').prop('disabled', false);
    $('#workIn').prop('disabled', true);
    $('#workDateTime').prop('disabled', false);
    $('#workCost').prop('disabled', false);
    $('#workComplitet').prop('disabled', true);
    $('#manufIn').prop('disabled', true);
    $('#manufDate').prop('disabled', false);
    $('#manufCost').prop('disabled', false);
    $('#manufComplited').prop('disabled', true);
    $('#finIn').prop('disabled', true);
    $('#finDate').prop('disabled', false);
    $('#finCost').prop('disabled', false);
    $('#finComplited').prop('disabled', true);
    $('#id_PlanZakaz').val("");
    $('#id_CMO_TypeProduct').val("");
    $('#id_CMO_Company').val("");
    $('#workIn').prop('checked', false);
    $('#workDateTime').val("");
    $('#workCost').val("");
    $('#workComplitet').prop('checked', false);
    $('#manufIn').prop('checked', false);
    $('#manufDate').val("");
    $('#manufCost').val("");
    $('#manufComplited').prop('checked', false);
    $('#finIn').prop('checked', false);
    $('#finDate').val("");
    $('#finCost').val("");
    $('#finComplited').prop('checked', false);
}

function addOrder() {
    var res = validateCreateOrder();
    if (res === false) {
        return false;
    }
    $("#btnAddOrder").attr('disabled', true);
    var objRemark = {
        id_PlanZakaz: $('#id_PlanZakaz').val(),
        id_CMO_TypeProduct: $('#id_CMO_TypeProduct').val(),
        File1: $('#File1').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/AddOrder",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData(document.getElementById('pageId').innerHTML);
            $('#createOrderModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validateCreateOrder() {
    if ($('#id_PlanZakaz').val().length === 0) {
        $('#id_PlanZakaz').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_PlanZakaz').css('border-color', 'lightgrey');
    }
    if ($('#id_CMO_TypeProduct').val().length === 0) {
        $('#id_CMO_TypeProduct').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_CMO_TypeProduct').css('border-color', 'lightgrey');
    }
    if ($('#File1').val().length === 0) {
        $('#File1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#File1').css('border-color', 'lightgrey');
    }
}

function addReOrder() {
    var res = validateCreateReOrder();
    if (res === false) {
        return false;
    }
    $("#btnAddOrder").attr('disabled', true);
    var objRemark = {
        id_PlanZakaz: $('#id_PlanZakaz').val(),
        id_CMO_Company: $('#id_CMO_Company').val(),
        File1: $('#File1').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/AddReOrder",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData(document.getElementById('pageId').innerHTML);
            $('#createReOrderModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validateCreateReOrder() {
    if ($('#id_PlanZakaz').val().length === 0) {
        $('#id_PlanZakaz').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_PlanZakaz').css('border-color', 'lightgrey');
    }
    if ($('#id_CMO_Company').val().length === 0) {
        $('#id_CMO_Company').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_CMO_Company').css('border-color', 'lightgrey');
    }
    if ($('#File1').val().length === 0) {
        $('#File1').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#File1').css('border-color', 'lightgrey');
    }
}

function get(id) {
    clearTextBoxUpdate();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/CMOArea/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id_PlanZakaz').val(result.id_PlanZakaz);
            $('#id_CMO_TypeProduct').val(result.id_CMO_TypeProduct);
            $('#id_CMO_Company').val(result.id_CMO_Company);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#id_AspNetUsers_Create').val(result.id_AspNetUsers_Create);
            $("#workIn").val(result.workIn).trigger("chosen:updated");
            $('#workDateTime').val(result.workDateTime);
            $('#workCost').val(result.workCost);
            $("#workComplitet").val(result.workComplitet).trigger("chosen:updated");
            $("#manufIn").val(result.manufIn).trigger("chosen:updated");
            $('#manufDate').val(result.manufDate);
            $('#manufCost').val(result.manufCost);
            $("#manufComplited").val(result.manufComplited).trigger("chosen:updated");
            $("#finIn").val(result.finIn).trigger("chosen:updated");
            $('#finDate').val(result.finDate);
            $('#finCost').val(result.finCost);
            $("#finComplited").val(result.finComplited).trigger("chosen:updated");
            $('#osModal').modal('show');
            $('#btnUpdate').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    var countDA = 0;
    if ($('#workIn').is(":checked") === true)
        countDA = 1;
    else if ($('#manufIn').is(":checked") === true)
        countDA = 2;
    else if ($('#finIn').is(":checked") === true)
        countDA = 3;
    deactivatedModalOS(countDA);
    return false;
}

function deactivatedModalOS(countDA) {
    if (countDA === 1) {
        $('#workIn').prop('disabled', true);
        $('#workDateTime').prop('disabled', false);
        $('#workCost').prop('disabled', false);
        $('#workComplitet').prop('disabled', true);
        $('#manufIn').prop('disabled', true);
        $('#manufDate').prop('disabled', true);
        $('#manufCost').prop('disabled', true);
        $('#manufComplited').prop('disabled', true);
        $('#finIn').prop('disabled', true);
        $('#finDate').prop('disabled', true);
        $('#finCost').prop('disabled', true);
        $('#finComplited').prop('disabled', true);
    }
    else if (countDA === 2) {
        $('#workIn').prop('disabled', true);
        $('#workDateTime').prop('disabled', true);
        $('#workCost').prop('disabled', true);
        $('#workComplitet').prop('disabled', true);
        $('#manufIn').prop('disabled', true);
        $('#manufDate').prop('disabled', false);
        $('#manufCost').prop('disabled', false);
        $('#manufComplited').prop('disabled', true);
        $('#finIn').prop('disabled', true);
        $('#finDate').prop('disabled', true);
        $('#finCost').prop('disabled', true);
        $('#finComplited').prop('disabled', true);
    }
    else if (countDA === 3) {
        $('#workIn').prop('disabled', true);
        $('#workDateTime').prop('disabled', true);
        $('#workCost').prop('disabled', true);
        $('#workComplitet').prop('disabled', true);
        $('#manufIn').prop('disabled', true);
        $('#manufDate').prop('disabled', true);
        $('#manufCost').prop('disabled', true);
        $('#manufComplited').prop('disabled', true);
        $('#finIn').prop('disabled', true);
        $('#finDate').prop('disabled', false);
        $('#finCost').prop('disabled', false);
        $('#finComplited').prop('disabled', true);
    }
    else {
        $('#workIn').prop('disabled', true);
        $('#workDateTime').prop('disabled', true);
        $('#workCost').prop('disabled', true);
        $('#workComplitet').prop('disabled', true);
        $('#manufIn').prop('disabled', true);
        $('#manufDate').prop('disabled', true);
        $('#manufCost').prop('disabled', true);
        $('#manufComplited').prop('disabled', true);
        $('#finIn').prop('disabled', true);
        $('#finDate').prop('disabled', true);
        $('#finCost').prop('disabled', true);
        $('#finComplited').prop('disabled', true);
    }
}

function update(id) {
    var res = validateUpdate();
    if (res === false) {
        return false;
    }
    var typeObj = {
        id_CMO_Company: $('#id_CMO_Company').val(),

        workIn: $('#workIn').is(":checked"),
        workDateTime: $('#workDateTime').val(),
        workCost: $('#workCost').val(),
        workComplitet: $('#workComplitet').is(":checked"),

        manufIn: $('#manufIn').is(":checked"),
        manufDate: $('#manufDate').val(),
        manufCost: $('#manufCost').val(),
        manufComplited: $('#manufComplited').is(":checked"),

        finIn: $('#finIn').is(":checked"),
        finDate: $('#finDate').val(),
        finCost: $('#finCost').val(),
        finComplited: $('#finComplited').is(":checked")
    };
    $.ajax({
        url: "/CMOArea/Update" + id,
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadOS();
            $('#osModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validateUpdate() {
    if ($('#workIn').is(":checked") === false) {
        if ($('#workDateTime').val().length === 0) {
            $('#workDateTime').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#workDateTime').css('border-color', 'lightgrey');
        }
        if ($('#workCost').val().length === 0) {
            $('#workCost').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#workCost').css('border-color', 'lightgrey');
        }
    }
    else if ($('#manufIn').is(":checked") === false) {
        if ($('#manufDate').val().length === 0) {
            $('#manufDate').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#manufDate').css('border-color', 'lightgrey');
        }
        if ($('#manufCost').val().length === 0) {
            $('#manufCost').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#manufCost').css('border-color', 'lightgrey');
        }
    }
    else if ($('#finIn').is(":checked") === false) {
        if ($('#finDate').val().length === 0) {
            $('#finDate').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#finDate').css('border-color', 'lightgrey');
        }
        if ($('#finCost').val().length === 0) {
            $('#finCost').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#finCost').css('border-color', 'lightgrey');
        }
    }
}