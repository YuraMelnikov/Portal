$(document).ready(function () {
    startMenu();
});

var objList = [
    { "title": "Ид.", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "edit", "autowidth": true, "bSortable": false },
    { "title": "Сотрудник", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Период", "data": "period", "autowidth": true, "bSortable": true },
    { "title": "Коэф.", "data": "coef", "autowidth": true, "bSortable": false }
];

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/ReportPage/GetList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objList,
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

function loadData() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/ReportPage/GetList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objList,
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

function get(id) {
    $.ajax({
        cache: false,
        url: "/ReportPage/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#coef').css('border-color', 'lightgrey');
            $('#id').val(result.id);
            $('#userName').val(result.userName);
            $('#period').val(result.period);
            $('#coef').val(result.coef);
            $('#userCardModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function update() {
    var res = validateUpdate();
    if (res === false) {
        return false;
    }
    var typeObj = {
        id: $('#id').val(),
        coef: $('#coef').val()
    };
    $.ajax({
        cache: false,
        url: "/ReportPage/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#userCardModal').modal('hide');
            $('#Id').val("");
            $('#userName').val("");
            $('#period').val("");
            $('#coef').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validateUpdate() {
    var isValid = true;
    if ($('#coef').val() === null || $('#coef').val() === 0) {
        $('#coef').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#coef').css('border-color', 'lightgrey');
    }
    return isValid;
}