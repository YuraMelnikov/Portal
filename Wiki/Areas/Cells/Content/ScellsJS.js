$(document).ready(function () {
    startMenu();
});

function loadData(listId) {
    if (listId === 1 || listId === "1") {
        list();
    }
    else {
        list();
    }
}

var objList = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ИД. начальной секции", "data": "sectionIdStart", "autowidth": true, "bSortable": true },
    { "title": "Наименование начальной секции", "data": "name1", "autowidth": true, "bSortable": true },
    { "title": "ИД. конечной секции", "data": "sectionIdFinish", "autowidth": true, "bSortable": true },
    { "title": "Наименование конечной секции", "data": "name2", "autowidth": true, "bSortable": true },
    { "title": "Расстояние (м)", "data": "distance", "autowidth": true, "bSortable": true }
];

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/SCells/List",
            "type": "POST",
            "datatype": "json"
        },
        //"bDestroy": true,
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.distance === 0 && data.distance === "0") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
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

function list() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/SCells/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.distance === 0 && data.distance === "0") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
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
        url: "/SCells/GetPoint/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#distance').css('border-color', 'lightgrey');
            $('#id').val("");
            $('#sectionIdStart').val("");
            $('#name1').val("");
            $('#sectionIdFinish').val("");
            $('#name2').val("");
            $('#distance').val("");
            $('#id').val(result.id);
            $('#sectionIdStart').val(result.sectionIdStart);
            $('#name1').val(result.name1);
            $('#sectionIdFinish').val(result.sectionIdFinish);
            $('#name2').val(result.name2);
            $('#distance').val(result.distance);
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
        id: $('#id').val()
        ,distance: $('#distance').val().replace(".", ",")
    };
    $.ajax({
        cache: false,
        url: "/SCells/UpdatePoint/",
        data: JSON.stringify(objPoint),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result === 0) {
                alert("У Вас недостаточно прав");
            }
            else {
                $('#myTable').DataTable().ajax.reload(null, false);
                $('#pointModal').modal('hide');
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function valid() {
    var isValid = true;
    if ($('#distance').val().trim() === "") {
        $('#distance').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#distance').css('border-color', 'lightgrey');
    }
    return isValid;
}