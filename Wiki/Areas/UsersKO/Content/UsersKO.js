$(document).ready(function () {
    startMenu();
});

var objList = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ФИО сотрудника", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Бюро сотрудника", "data": "devision", "autowidth": true, "bSortable": true },
    { "title": "Период", "data": "period", "autowidth": true, "bSortable": true },
    { "title": "Коэффициент", "data": "coefficient", "autowidth": true, "bSortable": true }
];

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/UserKO/List/" + '2019.07',
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.coefficient === 0 || data.coefficient === "0") {
                $('td', row).css('background-color', '#f45b5b');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
            }
        },
        "cache": false,
        "async": false,
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

function list(data) {
    var id = $('#listSelections').find('option:selected').text();
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/UserKO/List/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.coefficient === 0 || data.coefficient === "0") {
                $('td', row).css('background-color', '#f45b5b');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
            }
        },
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

function getPoint(id) {
    $.ajax({
        cache: false,
        url: "/UserKO/GetPoint/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#ks').css('border-color', 'lightgrey');
            $('#ids').val("");
            $('#CiliricalName').val("");
            $('#period').val("");
            $('#ks').val("");
            $('#ids').val(result.ids);
            $('#CiliricalName').val(result.CiliricalName);
            $('#period').val(result.period);
            $('#ks').val(result.ks);
            $('#pointModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function updatePoint() {
    var res = valid();
    if (res === false) {
        return false;
    }
    var objPoint = {
        ids:$('#ids').val()
        ,ks:$('#ks').val().replace(".", ",")
    };
    $.ajax({
        cache: false,
        url: "/UserKO/UpdatePoint/",
        data: JSON.stringify(objPoint),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#pointModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function valid() {
    var isValid = true;
    if ($('#ks').val().trim() === "") {
        $('#ks').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ks').css('border-color', 'lightgrey');
    }
    return isValid;
}