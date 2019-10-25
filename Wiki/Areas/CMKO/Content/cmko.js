$(document).ready(function () {
    $('#optimizationTable').hide();
    StartMenu();
});

var objOptimization = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Предложил", "data": "autor", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "textData", "autowidth": true, "bSortable": false, "class": 'colu-300' },
    { "title": "Период (для учета)", "data": "period", "autowidth": true, "bSortable": true }
];

function StartMenu() {
    $("#optimizationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetOptimizationList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objOptimization,
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

function LoadOptimizationTable() {
    var table = $('#optimizationTable').DataTable();
    table.destroy();
    $('#optimizationTable').empty();
    $("#optimizationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetOptimizationList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objOptimization,
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

function AddOptimization() {
    var res = ValidOptimization();
    if (res === false){
        return false;
    }
    $('#btnAddOptimization').attr('disabled', true);
    var addObjOptm = {
        id: $('#idOptimization').val(),
        id_AspNetUsersIdea: $('#autor').val(),
        description: $('#textData').val(),
        id_CMKO_PeriodResult: $('#period').val()
    };
    $.ajax({
        url: "/CMK/AddOptimization",
        data: JSON.stringify(addObjOptm),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#optimizationTable').DataTable().ajax.reload(null, false);
            $('#optimizationModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetOptimization(id) {
    GetBtnOptimizationUpdateRemove();
    $.ajax({
        cache: false,
        url: "/CMK/GetOptimization/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idOptimization').val(result.idOptimization);
            $('#userCreate').val(result.userCreate);
            $('#dateCreate').va(result.dateCreate);
            $('#autor').val(result.autor);
            $('#period').val(result.period);
            $('#textData').val(result.textData);
            $('#optimizationModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UpdateOptimization() {
    var res = ValidOptimization();
    if (res === false){
        return false;
    }
    var updateObjOptm = {
        id: $('#idOptimization').val(),
        id_AspNetUsersIdea: $('#autor').val(),
        description: $('#textData').val(),
        id_CMKO_PeriodResult: $('#period').val()
    };
    $.ajax({
        cache: false,
        url: "/CMK/UpdateOptimization",
        data: JSON.stringify(updateObjOptm),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#optimizationTable').DataTable().ajax.reload(null, false);
            $('#optimizationModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function RemoveOptimization() {
    var removeObjOptm = {
        id: $('#idOptimization').val()
    };
    $.ajax({
        cache: false,
        url: "/CMK/RemoveOptimization",
        data: JSON.stringify(removeObjOptm),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#optimizationTable').DataTable().ajax.reload(null, false);
            $('#optimizationModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function ValidOptimization() {
    if ($('#autor').val() === null) {
        $('#autor').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#autor').css('border-color', 'lightgrey');
    }
    if ($('#period').val() === null) {
        $('#period').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#period').css('border-color', 'lightgrey');
    }
    if ($('#textData').val().trim() === "") {
        $('#textData').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#textData').css('border-color', 'lightgrey');
    }
}

function ClearOptimization() {
    GetBtnOptimizationAdd();
    $('#idOptimization').val("");
    $('#userCreate').val("");
    $('#dateCreate').val("");
    $('#autor').val("");
    $('#period').val("");    
    $('#period').val("");
    $('#textData').val("");
    $('#autor').css('border-color', 'lightgrey');
    $('#period').css('border-color', 'lightgrey');
    $('#textData').css('border-color', 'lightgrey');
}

function GetBtnOptimizationAdd() {
    $('#btnAddOptimization').attr('disabled', false);
    $('#btnAddOptimization').show();
    $('#btnUpdateOptimization').hide();
    $('#btnRemoveOptimization').hide();
}

function GetBtnOptimizationUpdateRemove() {
    $('#btnAddOptimization').hide();
    $('#btnUpdateOptimization').show();
    $('#btnRemoveOptimization').show();
}