//AddRem

$(document).ready(function () {
    startPage();
});

function loadData(listId) {
    if (listId === 1 || listId === "1") {
        listOrders();
    }
    else {
        listOrders();
    }
}

var objViewList = [
    { "title": "См", "data": "link", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "№ Заказа", "data": "order", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Менеджер", "data": "Manager", "autowidth": true, "bSortable": true },
    { "title": "Покупатель", "data": "Client", "autowidth": true, "bSortable": true },
    { "title": "Наименование", "data": "Name", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Наименование по ТУ", "data": "nameTU", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "МТР", "data": "MTR", "autowidth": true, "bSortable": true },
    { "title": "Запрос №", "data": "Zapros", "autowidth": true, "bSortable": true },
    { "title": "Дата отгрузки", "data": "dateSh", "autowidth": true, "bSortable": true }
];

function startPage() {
    $("#notesTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/NoteOrder/ReportTable",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objViewList,
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

function listOrders() {
    var table = $('#notesTable').DataTable();
    table.destroy();
    $('#notesTable').empty();
    $("#notesTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/NoteOrder/ReportTable",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objViewList,
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
    $('#PlanZakaz').val("");
    $('#DateCreate').val("");
    $('#Manager').val("");
    $('#Client').val("");
    $('#MTR').val("");
    $('#Name').val("");
    $('#Description').val("");
    $('#Cost').val("");
    $('#ProductType').val("");
    $('#OL').val("");
    $('#Zapros').val("");
    $('#Modul').val("");
}

function get(id) {
    clearTextBox();
    $.ajax({
        cache: false,
        url: "/NoteOrder/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#id').val(result.id);
            $('#PlanZakaz').val(result.PlanZakaz);
            $('#DateCreate').val(result.DateCreate);
            $('#Manager').val(result.Manager);
            $('#Client').val(result.Client);
            $('#MTR').val(result.MTR);
            $('#Name').val(result.Name);
            $('#Description').val(result.Description);
            $('#Cost').val(result.Cost);
            $('#ProductType').val(result.ProductType);
            $('#OL').val(result.OL);
            $('#Zapros').val(result.Zapros);
            $('#Modul').val(result.Modul);
            getRemOrder(result.Id);
            $('#orderModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function getRemOrder(Id) {
    $("#tableRem").DataTable({
        "ajax": {
            "cache": false,
            "url": "/NoteOrder/GetRemOrder/" + Id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "bAutoWidth": false,
        "columns": [
            { "title": "Создано", "data": "remCreate", "autowidth": true, "bSortable": false },
            { "title": "Содержание", "data": "remNote", "autowidth": true, "bSortable": false },
            { "title": "Автор", "data": "remUser", "autowidth": true, "bSortable": false }
        ],
        "scrollY": '75vh',
        "searching": false,
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

function addNewRemark() {
    var res = validRem();
    if (res === false) {
        return false;
    }
    var typeObj = {
        Id: $('#Id').val(),
        textRem: $('#textRem').val()
    };
    $.ajax({
        cache: false,
        url: "/NoteOrder/AddRemToOrder",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            getRemOrder(result);
            $('#textRem').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validRem() {
    var isValid = true;
    if ($('#textRem').val().trim() === "") {
        $('#textRem').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#textRem').css('border-color', 'lightgrey');
    }
    return isValid;
}

function clearTextBoxForOrders() {
    $('#pz').val("");
    $('#pz').chosen();
    $('#pz').trigger('chosen:updated');
    $('#mText').val("");
    $('#modalNewNote').modal('show');
}

function AddRem() {
    var res = validRemOrders();
    if (res === false) {
        return false;
    }
    var objRemark = {
        pz: $('#pz').val(),
        mText: $('#mText').val()
    };
    $.ajax({
        cache: false,
        url: "/NoteOrder/AddRem",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#modalNewNote").hide();
            loadData(1);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validRemOrders() {
    var isValid = true;
    if ($('#mText').val().trim() === "") {
        $('#mText').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#mText').css('border-color', 'lightgrey');
    }
    return isValid;
}