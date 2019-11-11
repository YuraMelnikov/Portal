$(document).ready(function () {
    getPeriodReport();
    HideAllTables();
    StartMenu();
    GetSummaryWageFundWorker();
    GetSummaryWageFundManager();
    GetSummaryWageFundG();
    GetRemainingBonus();
    GetWithheldToBonusFund();
    GetOverflowsBujet();
    GetGAccrued();
    GetHSSPO();
    GetHSSKBM();
    GetHSSKBE();
    GetManpowerFirstPeriod();
    GetManpowerSecondPeriod();
    GetManpowerThreePeriod();
    GetAccuredPlan();
    GetAccuredFact();
});

var objOptimization = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Период (для учета)", "data": "period", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Предложил", "data": "autor", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "textData", "autowidth": true, "bSortable": false, "class": 'colu-300' }
];

var objTeach = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Период (для учета)", "data": "period", "autowidth": true, "bSortable": true },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Учитель", "data": "teacher", "autowidth": true, "bSortable": true },
    { "title": "Обучаемый", "data": "student", "autowidth": true, "bSortable": true },
    { "title": "Прим.", "data": "description", "autowidth": true, "bSortable": false, "class": 'colu-300' }
];

function getPeriodReport() {
    $.ajax({
        url: "/BP/GetPeriodReport/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("periodReportString").textContent = result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function LoadData(id) {
    HideAllTables();
    if (id === 1) {
        LoadOptimizationTable();
    }
    else if (id === 2) {
        LoadTeachTable();
    }
    else if (id === 3) {
        LoadUsersTable();
    }
    else if (id === 4) {
        LoadCategoryTable();
    }
    else if (id === 5) {
        LoadPeriodTable();
    }
    else if (id === 6) {
        LoadCalendTable();
    }
    else if (id === 7) {
        LoadCurencyTable();
    }
    else if (id === 8) {
        LoadSpeedUsersTable();
    }

}

function HideAllTables() {
    $('#speedUsersDiv').hide();
    $('#optimizationDiv').hide();
    $('#teachDiv').hide();
    $('#usersDiv').hide();
    $('#categoryDiv').hide();
    $('#periodsDiv').hide();
    $('#calendDiv').hide();
    $('#curencyDiv').hide();
    $('#speedWorkers1').hide();
    $('#speedWorkers2').hide();
    $('#speedWorkers3').hide();
}

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
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#teachTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetTeachList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objTeach,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#usersTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetUsersList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objUsers,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#categoryTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetCategoryList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objCategory,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#periodsTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetPeriodList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objPeriod,
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
    $("#calendTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetCalendList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objCalend,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#cyrencyTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetCurencyList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objCurency,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#speedUsersTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetSpeedUserList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objSpeedUsers,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
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

var objSpeedUsers = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Период", "data": "period", "autowidth": true, "bSortable": true },
    { "title": "ФИО", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Коэф.", "data": "coef", "autowidth": true, "bSortable": true }
];

function LoadSpeedUsersTable() {
    var table = $('#speedUsersTable').DataTable();
    table.destroy();
    $('#speedUsersTable').empty();
    $("#speedUsersTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetSpeedUserList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSpeedUsers,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $('#speedUsersDiv').show();
}

function ValidSpeedUser() {
    isValid = true;
    if ($('#coefSpeedUser').val() === "") {
        $('#coefSpeedUser').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#coefSpeedUser').css('border-color', 'lightgrey');
    }
    return isValid;
}

function GetSpeedUser(id) {
    $('#coefSpeedUser').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/CMK/GetSpeedUser/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idSpeedUser').val(result.idSpeedUser);
            $('#userNameSpeedUser').val(result.userNameSpeedUser);
            $('#periodSpeedUser').va(result.periodSpeedUser);
            $('#coefSpeedUser').va(result.coefSpeedUser);
            $('#speedUserModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UpdatSpeedUser() {
    var res = ValidSpeedUser();
    if (res === false) {
        return false;
    }
    var updateObjSpeedUser = {
        id: $('#idSpeedUser').val(),
        k: $('#coefSpeedUser').val().val().replace('.', ',')
    };
    $.ajax({
        cache: false,
        url: "/CMK/UpdateSpeedUser",
        data: JSON.stringify(updateObjSpeedUser),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#speedUsersTable').DataTable().ajax.reload(null, false);
            $('#speedUserModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
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
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $('#optimizationDiv').show();
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
            $('#dateCreate').val(result.dateCreate);
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
    isValid = true;
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
    return isValid;
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

function LoadTeachTable() {
    var table = $('#teachTable').DataTable();
    table.destroy();
    $('#teachTable').empty();
    $("#teachTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetTeachList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objTeach,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $('#teachDiv').show();
}

function AddTeach() {
    var res = ValidTeach();
    if (res === false){
        return false;
    }
    $('#btnAddTeach').attr('disabled', true);
    var addObjTeach = {
        id: $('#idTeach').val(),
        id_AspNetUsersTeacher: $('#teacherTeach').val(),
        id_AspNetUsersTeach: $('#studentTeach').val(),
        id_CMKO_PeriodResult: $('#periodTeach').val(),
        cost: $('#costTeach').val().replace('.', ','),
        description: $('#descriptionTeach').val()
    };
    $.ajax({
        url: "/CMK/AddTeach",
        data: JSON.stringify(addObjTeach),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#teachTable').DataTable().ajax.reload(null, false);
            $('#teachModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetTeach(id) {
    GetBtnTeachUpdateRemove();
    $.ajax({
        cache: false,
        url: "/CMK/GetTeach/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idTeach').val(result.idTeach);
            $('#teacherTeach').val(result.teacherTeach);
            $('#studentTeach').val(result.studentTeach);
            $('#periodTeach').val(result.periodTeach);
            $('#costTeach').val(result.costTeach);
            $('#descriptionTeach').val(result.descriptionTeach);
            $('#teachModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UpdateTeach() {
    var res = ValidTeach();
    if (res === false){
        return false;
    }
    var updateObjTeach = {
        id: $('#idTeach').val(),
        id_AspNetUsersTeacher: $('#teacherTeach').val(),
        id_CMKO_PeriodResult: $('#periodTeach').val(),
        cost: $('#costTeach').val().replace('.', ','),
        id_AspNetUsersTeach: $('#studentTeach').val(),
        description: $('#descriptionTeach').val()
    };
    $.ajax({
        cache: false,
        url: "/CMK/UpdateTeach",
        data: JSON.stringify(updateObjTeach),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#teachTable').DataTable().ajax.reload(null, false);
            $('#teachModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function RemoveTeach() {
    var removeObjOptm = {
        id: $('#idTeach').val()
    };
    $.ajax({
        cache: false,
        url: "/CMK/RemoveTeach",
        data: JSON.stringify(removeObjOptm),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#teachTable').DataTable().ajax.reload(null, false);
            $('#teachModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function ValidTeach() {
    isValid = true;
    if ($('#teacherTeach').val() === null) {
        $('#teacherTeach').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#teacherTeach').css('border-color', 'lightgrey');
    }
    if ($('#studentTeach').val() === null) {
        $('#studentTeach').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#studentTeach').css('border-color', 'lightgrey');
    }
    if ($('#periodTeach').val() === null) {
        $('#periodTeach').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#periodTeach').css('border-color', 'lightgrey');
    }
    if ($('#costTeach').val().trim() === "") {
        $('#costTeach').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#costTeach').css('border-color', 'lightgrey');
    }
    return isValid;
}

function ClearTeach() {
    GetBtnTeachAdd();
    $('#idTeach').val("");
    $('#teacherTeach').val("");
    $('#studentTeach').val("");
    $('#periodTeach').val("");
    $('#costTeach').val("");    
    $('#descriptionTeach').val("");
    $('#teacherTeach').css('border-color', 'lightgrey');
    $('#studentTeach').css('border-color', 'lightgrey');
    $('#periodTeach').css('border-color', 'lightgrey');
    $('#costTeach').css('border-color', 'lightgrey');
}

function GetBtnTeachAdd() {
    $('#btnAddTeach').attr('disabled', false);
    $('#btnAddTeach').show();
    $('#btnRemoveTeach').hide();
    $('#btnUpdateTeach').hide();
}

function GetBtnTeachUpdateRemove() {
    $('#btnAddTeach').hide();
    $('#btnRemoveTeach').show();
    $('#btnUpdateTeach').show();
}

var objUsers = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "ФИО", "data": "ciliricName", "autowidth": true, "bSortable": true },
    { "title": "Бюро", "data": "devisionName", "autowidth": true, "bSortable": true },
    { "title": "Категория", "data": "category", "autowidth": true, "bSortable": true },
    { "title": "Испытательный срок пройден", "data": "dateToCMKO", "autowidth": true, "bSortable": true },
    { "title": "Налог", "data": "tax", "autowidth": true, "bSortable": false, render: $.fn.dataTable.render.number(',', '.', 2, '')  }
];

function LoadUsersTable() {
    var table = $('#usersTable').DataTable();
    table.destroy();
    $('#usersTable').empty();
    $("#usersTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetUsersList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objUsers,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $('#usersDiv').show();
}

function GetUser(id) {
    $('#taxUser').css('border-color', 'lightgrey');
    $('#categoryUser').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/CMK/GetUser/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idUser').val(result.idUser);
            $('#ciliricNameUser').val(result.ciliricNameUser);
            $('#devisionNameUser').val(result.devisionNameUser);
            $('#categoryUser').val(result.categoryUser);
            $('#dateToCMKO').val(result.dateToCMKO);
            $('#taxUser').val(result.taxUser);
            $('#userModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UpdateUser() {
    var res = ValidUser();
    if (res === false){
        return false;
    }
    var updateObjUser = {
        id: $('#idUser').val(),
        id_CMKO_TaxCatigories: $('#categoryUser').val(),
        dateToCMKO: $('#dateToCMKO').val(),
        tax: $('#taxUser').val().replace('.', ',')
    };
    $.ajax({
        cache: false,
        url: "/CMK/UpdateUser",
        data: JSON.stringify(updateObjUser),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#usersTable').DataTable().ajax.reload(null, false);
            $('#userModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function ValidUser() {
    isValid = true;
    if ($('#categoryUser').val() === null) {
        $('#categoryUser').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#categoryUser').css('border-color', 'lightgrey');
    }
    if ($('#taxUser').val() === null) {
        $('#taxUser').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#taxUser').css('border-color', 'lightgrey');
    }
    return isValid;
}

var objCategory = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Наименование", "data": "nameCategory", "autowidth": true, "bSortable": true },
    { "title": "Оклад", "data": "selaryCategory", "autowidth": true, "bSortable": true, render: $.fn.dataTable.render.number(',', '.', 2, '') }
];

function LoadCategoryTable() {
    var table = $('#categoryTable').DataTable();
    table.destroy();
    $('#categoryTable').empty();
    $("#categoryTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetCategoryList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objCategory,
        "scrollY": '75vh',
        "scrollX": true,
        "searching": false,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $('#categoryDiv').show();
}

function ClearCategory() {
    $('#btnAddCategory').attr('disabled', false);
    $('#btnAddCategory').show();
    $('#btnUpdateCategory').hide();
    $('#idCategory').val("");
    $('#nameCategory').val("");
    $('#selaryCategory').val("");
    $('#nameCategory').css('border-color', 'lightgrey');
    $('#selaryCategory').css('border-color', 'lightgrey');
    $('#categoryModal').modal('show');
}

function AddCategory() {
    var res = ValidCategory();
    if (res === false){
        return false;
    }
    $('#btnAddCategory').attr('disabled', true);
    var addObjCategory = {
        id: $('#idCategory').val(),
        catigoriesName: $('#nameCategory').val(),
        salary: $('#selaryCategory').val().replace('.', ',')
    };
    $.ajax({
        url: "/CMK/AddCategory",
        data: JSON.stringify(addObjCategory),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#categoryTable').DataTable().ajax.reload(null, false);
            $('#categoryModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function ValidCategory() {
    isValid = true;
    if ($('#nameCategory').val() === "") {
        $('#nameCategory').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#nameCategory').css('border-color', 'lightgrey');
    }
    if ($('#selaryCategory').val() === "") {
        $('#selaryCategory').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#selaryCategory').css('border-color', 'lightgrey');
    }
    return isValid;
}

function GetCategory(id) {
    $('#btnAddCategory').hide();
    $('#btnUpdateCategory').show();
    $('#nameCategory').css('border-color', 'lightgrey');
    $('#selaryCategory').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/CMK/GetCategory/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idCategory').val(result.idCategory);
            $('#nameCategory').val(result.nameCategory);
            $('#selaryCategory').val(result.selaryCategory);
            $('#categoryModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UpdateCategory() {
    var res = ValidCategory();
    if (res === false){
        return false;
    }
    var updateObjCategory = {
        id: $('#idCategory').val(),
        catigoriesName: $('#nameCategory').val(),
        salary: $('#selaryCategory').val().replace('.', ',')
    };
    $.ajax({
        cache: false,
        url: "/CMK/UpdateCategory",
        data: JSON.stringify(updateObjCategory),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#categoryTable').DataTable().ajax.reload(null, false);
            $('#categoryModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

var objPeriod = [
    { "title": "Наименование", "data": "period", "autowidth": true, "bSortable": true }
];

function LoadPeriodTable() {
    var table = $('#periodsTable').DataTable();
    table.destroy();
    $('#periodsTable').empty();
    $("#periodsTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetPeriodList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objPeriod,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $('#periodsDiv').show();
}

function ClearPeriod() {
    $('#btnAddPeriod').attr('disabled', false);
    $('#namePeriod').val("");
    $('#namePeriod').css('border-color', 'lightgrey');
    $('#periodModal').modal('show');
}

function AddPeriod() {
    var res = ValidPeriod();
    if (res === false){
        return false;
    }
    $('#btnAddPeriod').attr('disabled', true);
    var addObjPeriod = {
        period: $('#namePeriod').val()
    };
    $.ajax({
        url: "/CMK/AddPeriod",
        data: JSON.stringify(addObjPeriod),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#periodsTable').DataTable().ajax.reload(null, false);
            $('#periodModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function ValidPeriod() {
    isValid = true;
    if ($('#namePeriod').val() === null) {
        $('#namePeriod').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#namePeriod').css('border-color', 'lightgrey');
    }
    return isValid;
}

var objCalend = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Период", "data": "period", "autowidth": true, "bSortable": true },
    { "title": "Рабочее время (ч.)", "data": "timeToOnePerson", "autowidth": true, "bSortable": true, render: $.fn.dataTable.render.number(',', '.', 2, '') }
];

function LoadCalendTable() {
    var table = $('#calendTable').DataTable();
    table.destroy();
    $('#calendTable').empty();
    $("#calendTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetCalendList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objCalend,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $('#calendDiv').show();
}

function ClearCalend() {
    $('#btnAddCalend').attr('disabled', false);
    $('#btnAddCalend').show();
    $('#btnUpdateCalend').hide();
    $('#idCalend').val("");
    $('#periodCalend').val("");
    $('#timeToOnePersonCalend').val("");
    $('#periodCalend').css('border-color', 'lightgrey');
    $('#periodCalend').css('border-color', 'lightgrey');
    $('#calendModal').modal('show');
}

function AddCalend() {
    var res = ValidCalend();
    if (res === false){
        return false;
    }
    $('#btnAddCalend').attr('disabled', true);
    var addObjCalend = {
        period: $('#periodCalend').val(),
        timeToOnePerson: $('#timeToOnePersonCalend').val().replace('.', ',')
    };
    $.ajax({
        url: "/CMK/AddCalend",
        data: JSON.stringify(addObjCalend),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#calendTable').DataTable().ajax.reload(null, false);
            $('#calendModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function ValidCalend() {
    isValid = true;
    if ($('#periodCalend').val() === null) {
        $('#periodCalend').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#periodCalend').css('border-color', 'lightgrey');
    }
    if ($('#timeToOnePersonCalend').val() === null) {
        $('#timeToOnePersonCalend').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#timeToOnePersonCalend').css('border-color', 'lightgrey');
    }
    return isValid;
}

function GetCalend(id) {
    $('#btnAddCalend').hide();
    $('#btnUpdateCalend').show();
    $('#periodCalend').css('border-color', 'lightgrey');
    $('#timeToOnePersonCalend').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/CMK/GetCalend/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idCalend').val(result.idCalend);
            $('#periodCalend').val(result.periodCalend);
            $('#timeToOnePersonCalend').val(result.timeToOnePersonCalend);
            $('#calendModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UpdateCalend() {
    var res = ValidCalend();
    if (res === false){
        return false;
    }
    var updateObjCalend = {
        id: $('#idCalend').val(),
        period: $('#periodCalend').val(),
        timeToOnePerson: $('#timeToOnePersonCalend').val().replace('.', ',')
    };
    $.ajax({
        cache: false,
        url: "/CMK/UpdateCalend",
        data: JSON.stringify(updateObjCalend),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#calendTable').DataTable().ajax.reload(null, false);
            $('#calendModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

var objCurency = [
    { "title": "Дата", "data": "date", "autowidth": true, "bSortable": true },
    { "title": "Курс", "data": "USD", "autowidth": true, "bSortable": true, render: $.fn.dataTable.render.number(',', '.', 2, '') }
];

function LoadCurencyTable() {
    var table = $('#cyrencyTable').DataTable();
    table.destroy();
    $('#cyrencyTable').empty();
    $("#cyrencyTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetCurencyList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objCurency,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $('#curencyDiv').show();
}

function GetSummaryWageFundWorker() {
    $.ajax({
        url: "/CMK/GetSummaryWageFundWorker/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var planArray = new Array();
            var factArray = new Array();
            planArray.push(result[0].Plan);
            planArray.push(result[1].Plan);
            planArray.push(result[2].Plan);
            factArray.push(result[0].Fact);
            factArray.push(result[1].Fact);
            factArray.push(result[2].Fact);
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('summaryWageFundWorker', {
                chart: {
                    type: 'bar',
                    marginBottom: 60
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'ФОТ заказов',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ['Всего', 'КБМ', 'КБЭ']
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'План',
                    data: planArray
                }, {
                    name: 'Факт',
                    data: factArray
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetSummaryWageFundManager() {
    $.ajax({
        url: "/CMK/GetSummaryWageFundManager/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var planArray = new Array();
            var factArray = new Array();
            planArray.push(result[0].Plan);
            planArray.push(result[1].Plan);
            planArray.push(result[2].Plan);
            factArray.push(result[0].Fact);
            factArray.push(result[1].Fact);
            factArray.push(result[2].Fact);
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('summaryWageFundManager', {
                chart: {
                    type: 'bar',
                    marginBottom: 60
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'ФОТ руководителей',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ['КО', 'КБМ', 'КБЭ']
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'План',
                    data: planArray
                }, {
                    name: 'Факт',
                    data: factArray
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetSummaryWageFundG() {
    $.ajax({
        url: "/CMK/GetSummaryWageFundG/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var planArray = new Array();
            var factArray = new Array();
            planArray.push(result[0].Plan);
            planArray.push(result[1].Plan);
            planArray.push(result[2].Plan);
            factArray.push(result[0].Fact);
            factArray.push(result[1].Fact);
            factArray.push(result[2].Fact);
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('summaryWageFundG', {
                chart: {
                    type: 'bar',
                    marginBottom: 60
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'ФОТ ГИПов',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ['Всего', 'КБМ', 'КБЭ']
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'План',
                    data: planArray
                }, {
                    name: 'Факт',
                    data: factArray
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetRemainingBonus() {
    $.ajax({
        url: "/CMK/GetRemainingBonus/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var planArray = new Array();
            var factArray = new Array();
            planArray.push(result[0].Plan);
            planArray.push(result[1].Plan);
            planArray.push(result[2].Plan);
            factArray.push(result[0].Fact);
            factArray.push(result[1].Fact);
            factArray.push(result[2].Fact);
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('remainingBonus', {
                chart: {
                    type: 'bar',
                    marginBottom: 60
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'Остаток бонусного ФОТ',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ['Всего', 'КБМ', 'КБЭ']
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'План',
                    data: planArray
                }, {
                    name: 'Факт',
                    data: factArray
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetWithheldToBonusFund() {
    $.ajax({
        url: "/CMK/GetWithheldToBonusFund/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var planArray = new Array();
            var factArray = new Array();
            planArray.push(result[0].Plan);
            planArray.push(result[1].Plan);
            planArray.push(result[2].Plan);
            factArray.push(result[0].Fact);
            factArray.push(result[1].Fact);
            factArray.push(result[2].Fact);
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('withheldToBonusFund', {
                chart: {
                    type: 'bar',
                    marginBottom: 60
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'Удержано за ошибки',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ['Всего', 'КБМ', 'КБЭ']
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'План',
                    data: planArray
                }, {
                    name: 'Факт',
                    data: factArray
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetOverflowsBujet() {
    $.ajax({
        url: "/CMK/GetOverflowsBujet/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var planArray = new Array();
            //planArray.push(result[0].Plan);
            planArray.push(result[1].Plan);
            planArray.push(result[2].Plan);
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('overflowsBujet', {
                chart: {
                    type: 'bar',
                    marginBottom: 60
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'Перетоки',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ['КБМ', 'КБЭ']
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'Перетоки',
                    data: planArray
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetGAccrued() {
    $.ajax({
        url: "/CMK/GetGAccrued/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var counter = Object.keys(result).length;
            var workerArray = new Array();
            var planArray = new Array();
            var factArray = new Array();
            for (var i = 0; i < counter; i++) {
                workerArray.push(result[i].FullName);
                planArray.push(result[i].Plan);
                factArray.push(result[i].Fact);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('accruedG', {
                chart: {
                    type: 'bar',
                    marginBottom: 60
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'Итоговые начисления ГИПов',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: workerArray
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'План',
                    data: planArray
                }, {
                    name: 'Факт',
                    data: factArray
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetHSSPO() {
    $.ajax({
        url: "/CMK/GetHSSPO/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('hssPO', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'ХСС производства',
                    style: {
                        "font-size": "13px"
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    name: 'ХСС',
                    data: dataArray
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetHSSKBM() {
    $.ajax({
        url: "/CMK/GetHSSKBM/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('hssKBM', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'ХСС КБМ',
                    style: {
                        "font-size": "13px"
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    name: 'ХСС',
                    data: dataArray
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetHSSKBE() {
    $.ajax({
        url: "/CMK/GetHSSKBE/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('hssKBE', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'ХСС КБЭ',
                    style: {
                        "font-size": "13px"
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    name: 'ХСС',
                    data: dataArray
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetManpowerFirstPeriod() {
    $.ajax({
        url: "/CMK/GetManpowerFirstPeriod/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {     
            if(result === 1) {
                ManpowerUsersInMonth1();
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetManpowerSecondPeriod() {
    $.ajax({
        url: "/CMK/GetManpowerSecondPeriod/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {     
            if(result === 1) {
                ManpowerUsersInMonth2();
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetManpowerThreePeriod() {
    $.ajax({
        url: "/CMK/GetManpowerThreePeriod/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {     
            if(result === 1){
                ManpowerUsersInMonth3();
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function ManpowerUsersInMonth1() {
    $('#speedWorkers1').show();
    $.ajax({
        url: "/CMK/GetUsersMMP1_1/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            document.getElementById("periodReportUsersKBMString1").textContent = 'Выработка НЧ КБМ за ' + labelName;
            document.getElementById("periodReportUsersKBEString1").textContent = 'Выработка НЧ КБЭ за ' + labelName;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-1', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Васюхневич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_2/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-2', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Волкова']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_3/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-3', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Глебик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_4/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-4', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Кальчинский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_5/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-5', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Маляревич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_6/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
             var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-6', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Носик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_7/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-7', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Фейгина']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_8/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-8', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Добыш']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_9/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-9', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Жибуль']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_10/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-10', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Жук']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_11/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-11', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Климович']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_12/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-12', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Кучинский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_13/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-13', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Тимашкова']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_14/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-14', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Тиханский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP1_15/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container1-15', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Филонcик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#91e8e1',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#2b908f',
                            label: {
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
} 

function ManpowerUsersInMonth2() {
    $('#speedWorkers2').show();
    $.ajax({
        url: "/CMK/GetUsersMMP2_1/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            document.getElementById("periodReportUsersKBMString2").textContent = 'Выработка НЧ КБМ за ' + labelName;
            document.getElementById("periodReportUsersKBEString2").textContent = 'Выработка НЧ КБЭ за ' + labelName;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-1', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Васюхневич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_2/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-2', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Волкова']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_3/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-3', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Глебик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_4/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-4', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Кальчинский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_5/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-5', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Маляревич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_6/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-6', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Носик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_7/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-7', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Фейгина']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_8/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-8', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Добыш']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_9/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-9', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Жибуль']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_10/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-10', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Жук']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_11/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-11', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Климович']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_12/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-12', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Кучинский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_13/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-13', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Тимашкова']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_14/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-14', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Тиханский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP2_15/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container2-15', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Филонcик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
} 

function ManpowerUsersInMonth3() {
    $('#speedWorkers3').show();
    $.ajax({
        url: "/CMK/GetUsersMMP3_1/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            document.getElementById("periodReportUsersKBMString3").textContent = 'Выработка НЧ КБМ за ' + labelName;
            document.getElementById("periodReportUsersKBEString3").textContent = 'Выработка НЧ КБЭ за ' + labelName;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-1', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Васюхневич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_2/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-2', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Волкова']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_3/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-3', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Глебик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_4/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-4', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Кальчинский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_5/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-5', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Маляревич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_6/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-6', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Носик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_7/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-7', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Фейгина']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_8/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-8', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Добыш']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_9/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-9', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Жибуль']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_10/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-10', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Жук']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_11/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-11', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Климович']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_12/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-12', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Кучинский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_13/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-13', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Тимашкова']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_14/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-14', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Тиханский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $.ajax({
        url: "/CMK/GetUsersMMP3_15/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
            }
            Highcharts.chart('container3-15', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Филонcик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray20,
                    labels: {
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#f45b5b',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#91e8e1',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#2b908f',
                        label: {
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
} 

function GetAccuredPlan() {
    $.ajax({
        url: "/CMK/GetAccuredPlan/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var counter = Object.keys(result).length;
            var ciliricName = new Array();
            var bonusReversed = new Array();
            var accrued = new Array();
            var accruedg = new Array();
            var manager = new Array();
            var bonusQuality = new Array();
            var speed = new Array();
            var optimization = new Array();
            var teach = new Array();
            for(var i = 0; i < counter; i++) {
                ciliricName.push(result[i].CiliricName);
                bonusReversed.push(result[i].BonusReversed);
                accrued.push(result[i].Accrued);
                accruedg.push(result[i].Accruedg);
                manager.push(result[i].Manager);
                bonusQuality.push(result[i].BonusQuality);
                speed.push(result[i].Speed);
                optimization.push(result[i].Optimization);
                teach.push(result[i].Teach);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('usersAccuredPlan', {
                chart: {
                    type: 'bar'
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'Плановые начисления сотрудников',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ciliricName
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'Начисления по заказам',
                    data: accrued
                },{
                    name: 'Начисления ГИПов',
                    data: accruedg
                },{
                    name: 'Бонус за отработку',
                    data: speed
                },{
                    name: 'Остаток бонусного фонда',
                    data: bonusReversed
                },{
                    name: 'Бонус за качество',
                    data: bonusQuality
                },{
                    name: 'Бонус за оптимизацию',
                    data: optimization
                },{
                    name: 'Начисление за обучение',
                    data: teach
                },{
                    name: 'Руководительские начисления',
                    data: manager
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetAccuredFact() {
    $.ajax({
        url: "/CMK/GetAccuredFact/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var counter = Object.keys(result).length;
            var ciliricName = new Array();
            var bonusReversed = new Array();
            var accrued = new Array();
            var accruedg = new Array();
            var manager = new Array();
            var bonusQuality = new Array();
            var speed = new Array();
            var optimization = new Array();
            var teach = new Array();
            for(var i = 0; i < counter; i++) {
                ciliricName.push(result[i].CiliricName);
                bonusReversed.push(result[i].BonusReversed);
                accrued.push(result[i].Accrued);
                accruedg.push(result[i].Accruedg);
                manager.push(result[i].Manager);
                bonusQuality.push(result[i].BonusQuality);
                speed.push(result[i].Speed);
                optimization.push(result[i].Optimization);
                teach.push(result[i].Teach);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('usersAccuredFact', {
                chart: {
                    type: 'bar'
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'Фактические начисления сотрудников',
                    style: {
                        "font-size": "13px"
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },
                    stackLabels: {
                        enabled: true
                    }
                },
                xAxis: {
                    categories: ciliricName
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'Начисления по заказам',
                    data: accrued
                },{
                    name: 'Начисления ГИПов',
                    data: accruedg
                },{
                    name: 'Бонус за отработку',
                    data: speed
                },{
                    name: 'Остаток бонусного фонда',
                    data: bonusReversed
                },{
                    name: 'Бонус за качество',
                    data: bonusQuality
                },{
                    name: 'Бонус за оптимизацию',
                    data: optimization
                },{
                    name: 'Начисление за обучение',
                    data: teach
                },{
                    name: 'Руководительские начисления',
                    data: manager
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}