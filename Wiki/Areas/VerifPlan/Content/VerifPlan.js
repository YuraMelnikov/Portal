$(document).ready(function () {
    startMenu();
});

var objList = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа", "data": "id_PZ_PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Плановый срок передачи на проверку", "data": "planDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Фактическая дата передачи на проверку", "data": "factDate", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Дата приемки изделия ОТК", "data": "appDate", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Прим. гл. инженера", "data": "planDescription", "autowidth": true, "bSortable": false },
    { "title": "Прим. произв.", "data": "factDescription", "autowidth": true, "bSortable": false },
    { "title": "Прим. ОТК", "data": "appDescription", "autowidth": true, "bSortable": false },
    { "title": "Прогнозная дата проверки (рук. произв.)", "data": "fixedDateForKO", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Плановая дата проверки (prj)", "data": "verificationDateInPrj", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": false }
];

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListActive/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objList,
        //"rowCallback": function (row, data, index) {
        //    if (data.coefficient === 0 || data.coefficient === "0") {
        //        $('td', row).css('background-color', '#f45b5b');
        //        $('td', row).css('color', 'white');
        //        $('a', row).css('color', 'white');
        //    }
        //},
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

function listActive() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListActive/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objList,
        //"rowCallback": function (row, data, index) {
        //    if (data.coefficient === 0 || data.coefficient === "0") {
        //        $('td', row).css('background-color', '#f45b5b');
        //        $('td', row).css('color', 'white');
        //        $('a', row).css('color', 'white');
        //    }
        //},
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

function listClose() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListClose/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objList,
        //"rowCallback": function (row, data, index) {
        //    if (data.coefficient === 0 || data.coefficient === "0") {
        //        $('td', row).css('background-color', '#f45b5b');
        //        $('td', row).css('color', 'white');
        //        $('a', row).css('color', 'white');
        //    }
        //},
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
    var idUserGroup = getUserGroup();
    $.ajax({
        cache: false,
        url: "/Verific/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#factDate').css('border-color', 'lightgrey');
            $('#appDate').css('border-color', 'lightgrey');
            $('#fixedDateForKO').css('border-color', 'lightgrey');
            $('#id').val("");
            $('#id_PZ_PlanZakaz').val("");
            $('#fixed').val("");
            $('#state').val("");
            $('#fixetFirstDate').val("");
            $('#planDate').val("");
            $('#planDescription').val("");
            $('#factDate').val("");
            $('#factDescription').val("");
            $('#appDate').val("");
            $('#appDescription').val("");
            $('#verificationDateInPrj').val("");
            $('#fixedDateForKO').val("");
            $('#dateSh').val("");
            $('#id').val(result.id);
            $('#id_PZ_PlanZakaz').val(result.id_PZ_PlanZakaz);
            $('#fixed').val(result.fixed);
            $('#state').val(result.state);
            $('#fixetFirstDate').val(processNull(result.fixetFirstDate));
            $('#planDate').val(processNull(result.planDate));
            $('#planDescription').val(result.planDescription);
            $('#factDate').val(processNull(result.factDate));
            $('#factDescription').val(result.factDescription);
            $('#appDate').val(processNull(result.appDate));
            $('#appDescription').val(result.appDescription);
            $('#verificationDateInPrj').val(processNull(result.verificationDateInPrj));
            $('#fixedDateForKO').val(processNull(result.fixedDateForKO));
            $('#dateSh').val(processNull(result.dateSh));
            $('#btnUpdateTE').hide();
            $('#btnUpdateOTK').hide();
            $('#btnUpdateTM').hide();
            $('#planDate').prop('disabled', true);
            $('#planDescription').prop('disabled', true);
            $('#factDate').prop('disabled', true);
            $('#factDescription').prop('disabled', true);
            $('#fixedDateForKO').prop('disabled', true);
            $('#appDate').prop('disabled', true);
            $('#appDescription').prop('disabled', true);
            if (idUserGroup === 1 || idUserGroup === '1') {
                $('#btnUpdateOTK').show();
                $('#appDate').prop('disabled', false);
                $('#appDescription').prop('disabled', false);
            }
            else if (idUserGroup === 2 || idUserGroup === '2') {
                $('#btnUpdateTE').show();
                $('#planDate').prop('disabled', false);
                $('#planDescription').prop('disabled', false);
            }
            else if (idUserGroup === 3 || idUserGroup === '3') {
                $('#btnUpdateTM').show();
                $('#factDate').prop('disabled', false);
                $('#factDescription').prop('disabled', false);
                $('#fixedDateForKO').prop('disabled', false);
            }
            $('#verifModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function getUserGroup() {
    $.ajax({
        cache: false,
        url: "/Verific/GetUserGroup/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            return result;
        },
        error: function (errormessage) {
            return 0;
        }
    });
}

function updateTE() {
    var opjTE = {
        id: $('#id').val()
        , planDate: $('#planDate').val()
        , planDescription: $('#planDescription').val()
        , dateSh: $('#dateSh').val()
    };
    var res = validTE(opjTE.dateSh);
    if (res === false) {
        return false;
    }
    $.ajax({
        cache: false,
        url: "/Verific/UpdateTE/",
        data: JSON.stringify(opjTE),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#verifModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validTE(dateSh) {
    var isValid = true;
    var monthDate = dateSh.getMonth();
    var day = dateSh.getDate();
    var timeInMs = Date.now();
    if (timeInMs.getDate() <= 15) {
        if (monthDate <= timeInMs.getMonth()) {
            if ($('#planDate').val().trim() === "") {
                $('#planDate').css('border-color', 'Red');
                isValid = false;
            }
            else {
                $('#planDate').css('border-color', 'lightgrey');
            }
        }
    }
    else {
        if (monthDate <= timeInMs.getMonth() + 1) {
            if ($('#planDate').val().trim() === "") {
                $('#planDate').css('border-color', 'Red');
                isValid = false;
            }
            else {
                $('#planDate').css('border-color', 'lightgrey');
            }
        }
    }
    return isValid;
}

function updateOTK() {
    var opjOTK = {
        id: $('#id').val()
        , appDate: $('#appDate').val()
        , appDescription: $('#appDescription').val()
    };
    var res = validOTK();
    if (res === false) {
        return false;
    }
    $.ajax({
        cache: false,
        url: "/Verific/UpdateOTK/",
        data: JSON.stringify(opjOTK),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#verifModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validOTK() {
    var isValid = true;
    if ($('#appDate').val().trim() === "") {
        $('#appDate').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#appDate').css('border-color', 'lightgrey');
    }
    return isValid;
}

function updateTM() {
    var opjTM = {
        id: $('#id').val()
        , factDate: $('#factDate').val()
        , factDescription: $('#factDescription').val()
        , fixedDateForKO: $('#fixedDateForKO').val()
    };
    var res = validTM();
    if (res === false) {
        return false;
    }
    $.ajax({
        cache: false,
        url: "/Verific/UpdateTM/",
        data: JSON.stringify(opjTM),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#verifModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validTM() {
    var isValid = true;
    if ($('#factDate').val().trim() === "") {
        $('#factDate').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#factDate').css('border-color', 'lightgrey');
    }
    if ($('#fixedDateForKO').val().trim() === "") {
        $('#fixedDateForKO').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#fixedDateForKO').css('border-color', 'lightgrey');
    }
    return isValid;
}