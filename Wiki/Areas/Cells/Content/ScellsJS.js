$(document).ready(function () {
    startMenu();
    getRemainingRows();
});

function loadData(listId) {
    getRemainingRows();
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
            "url": "/SCells/List/" + 'ZeroPoint',
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.distance === 0 || data.distance === "0") {
                $('td', row).css('background-color', '#f45b5b');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
            }
            else {
                $('a', row).css('color', 'black');
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
    var selectedDevision = $('#listSelections').find('option:selected').text();
    var id = $('#listSelections').find('option:selected').text();
    getRemainingRows();
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/SCells/List/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objList,
        "rowCallback": function (row, data, index) {
            if (data.distance === 0 || data.distance === "0") {
                $('td', row).css('background-color', '#f45b5b');
                $('td', row).css('color', 'white');
                $('a', row).css('color', 'white');
            }
            else {
                $('a', row).css('color', 'black');
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
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#pointModal').modal('hide');
            getRemainingRows();
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

function cleanCells() {
    $('#distanceSelections').val("");
    $('#distanceSelections').css('border-color', 'lightgrey');
    $('#sectionsChosen').val("");
    $('#sectionsChosen').chosen();
    $('#sectionsChosen').trigger('chosen:updated');
    $('#pointsCellsModal').modal('show');
}

function updatePointsCells() {
    var res = validSelections();
    if (res === false) {
        return false;
    }
    var objSelections = {
        sectionsChosen: $('#sectionsChosen').val()
        ,distanceSelections: $('#distanceSelections').val().replace(".", ",")
    };
    $.ajax({
        cache: false,
        url: "/SCells/UpdatePointsCells/",
        data: JSON.stringify(objSelections),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#pointsCellsModal').modal('hide');
            getRemainingRows();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validSelections() {
    var isValid = true;
    if ($('#distanceSelections').val().trim() === "") {
        $('#distanceSelections').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#distanceSelections').css('border-color', 'lightgrey');
    }
    if ($('#sectionsChosen').val().length === 0) {
        $('#sectionsChosen').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#sectionsChosen').css('border-color', 'lightgrey');
    }
    return isValid;
}

function getRemainingRows() {
    $.ajax({
        cache: false,
        url: "/SCells/GetRemainingRows/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            document.getElementById('remainingZeroPoints').innerHTML = "Осталось незаполненных ячеек: " + result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getRow(data) {
    var selectedDevision = $('#sectionsChosen').find('option:selected').text();
    var id = $('#sectionsChosen').find('option:selected').text();
    $.ajax({
        cache: false,
        url: "/SCells/GetRow/" + id,
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#sectionsChosen').val(data).trigger('chosen:updated');
        }
    });
}