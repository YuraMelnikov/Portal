var colorPlanData = '#e2c3f9';
var colorFactData = '#cc3184';
var titleDiagrammColor = '#000';

var colorStackLabels = titleDiagrammColor;
var titleFontSize = "14px";
var colorMinusData = '#fa292a';
var colorBgBasicLine = '#fa292a';
var colorBg10Line = '#fae596';
var colorBg20Line = '#3fb0ac';
var colorBg30Line = '#717171';

function UpdateThem() {
    if ($.cookie("bg_color") === '0') {
        $.cookie("bg_color", '1', { expires: 60 });
        colorPlanData = '#e3e3e3';
        colorFactData = '#3fb0ac';
        titleDiagrammColor = '#717171';
        colorStackLabels = '#717171';
    }
    else {
        $.cookie("bg_color", '0', { expires: 60 });
        colorPlanData = '#e2c3f9';
        colorFactData = '#cc3184';
        titleDiagrammColor = '#000';
        colorStackLabels = '#000';
    }
    LoadData(9);
}

$(document).ready(function () {
    if ($.cookie("bg_color") === '1') {
        colorPlanData = '#e3e3e3';
        colorFactData = '#3fb0ac';
        titleDiagrammColor = '#717171';
        colorStackLabels = '#717171';
    }
    HideAllTables();
    StartMenu();
    LoadData(9);
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
    else if (id === 9) {
        if (leavelUser === 0) {
            return 0;
        }
        else if (leavelUser === 1 || leavelUser === '1') {
            GetTextNamePeriod();
            GetTimeSheet();
            getPeriodReport();
            GetSummaryWageFundWorker();
            $('#hideSummaryData').hide();
            $('#hideAccuredStandart').hide();
            $('#speedDevisionForManager').hide();
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
            GetNhUsersThisQua();
            GetCoefWorker();
            GetCoefWorkerG();
            getRemainingWorkAll();
            getRemainingWork();
            getRemainingWorkDevisionAll();
            getRemainingDevisionWork();
            getRemainingWorkAllE();
            getRemainingWorkE();
            $('#dashboardBody').show();
        }
        else if (leavelUser === 2 || leavelUser === '2') {
            GetTextNamePeriod();
            GetTimeSheet();
            getPeriodReport();
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
            GetNhUsersThisQua();
            GetCoefWorker();
            GetCoefWorkerG();
            getRemainingWorkAll();
            getRemainingWork();
            getRemainingWorkDevisionAll();
            getRemainingDevisionWork();
            getRemainingWorkAllE();
            getRemainingWorkE();
            $('#dashboardBody').show();
        }
        else {
            return 0;
        }
    }
    else if (id === 10) {
        HideAllTables();
        $('#dashboardBody').show();
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
    $('#dashboardBody').hide();
}

function GetTextNamePeriod() {
    $.ajax({
        cache: false,
        url: "/CMK/GetTextNamePeriod/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("textNamePeriod1").textContent = result[0];
            document.getElementById("textNamePeriod2").textContent = result[1];
            document.getElementById("textNamePeriod3").textContent = result[2];
        }
    });
}

var objRemarksList = [
    { "title": "См.", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "Ид.", "data": "idRemark", "autowidth": true, "bSortable": false },
    { "title": "Оценка", "data": "count", "autowidth": true, "bSortable": true },
    { "title": "Ответственный", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "textData", "autowidth": true, "bSortable": false }
];

var objNH = [
    { "title": "№ заказа", "data": "orderNumber", "autowidth": true, "bSortable": false },
    { "title": "Наименование задачи", "data": "taskName", "autowidth": true, "bSortable": false },
    { "title": "Исполнитель", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "НЧ", "data": "normH", "autowidth": true, "bSortable": true },
    { "title": "Тр-ты", "data": "work", "autowidth": true, "bSortable": false }
];
 
function StartMenu() {   
    $("#ramarksUserTable").DataTable({
        "ajax": {
            "cache": false, 
            "url": "/CMK/GetRamarksUsersList/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[3, "asc"]],
        "processing": true,
        "columns": objRemarksList,
        "scrollY": '75vh',
        "searching": false,
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true
    });
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
    });
}

function GetRamarksUsersList() {
    var table = $('#ramarksUserTable').DataTable();
    table.destroy();
    $('#ramarksUserTable').empty();
    $("#ramarksUserTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMK/GetRamarksUsersList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[3, "asc"]], 
        "processing": true,
        "columns": objRemarksList,
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
    $('#ramarksUserModal').modal('show');
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
        "scrollCollapse": true
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
            $('#periodSpeedUser').val(result.periodSpeedUser);
            $('#coefSpeedUser').val(result.coefSpeedUser);
            $('#speedUserModal').modal('show');
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
        k: $('#coefSpeedUser').val().replace('.', ',')
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
        "scrollCollapse": true
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
                    text: 'ФОТ заказов и НИОКРов',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    data: planArray,
                    color: colorPlanData
                }, {
                    name: 'Факт',
                    data: factArray,
                    color: colorFactData
                }]
            });
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
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    data: planArray,
                    color: colorPlanData
                }, {
                    name: 'Факт',
                    data: factArray,
                    color: colorFactData
                }]
            });
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
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    data: planArray,
                    color: colorPlanData
                }, {
                    name: 'Факт',
                    data: factArray,
                    color: colorFactData
                }]
            });
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
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    data: planArray,
                    color: colorPlanData
                }, {
                    name: 'Факт',
                    data: factArray,
                    color: colorFactData
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
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    data: planArray,
                    color: colorPlanData
                }, {
                    name: 'Факт',
                    data: factArray,
                    color: colorFactData
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
            planArray.push(result[1].Plan);
            planArray.push(result[2].Plan);
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('overflowsBujet', {
                legend: {
                    enabled: false
                },
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
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    data: planArray,
                    color: colorFactData
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
            if (counter === 0) {
                $('#hideAccuredG').hide();
            }
            else {
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
                            "font-size": titleFontSize,
                            "color": titleDiagrammColor
                        }
                    },
                    yAxis: {
                        title: {
                            enabled: false
                        },
                        stackLabels: {
                            enabled: true,
                            style: {
                                color: colorStackLabels
                            }
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
                        data: planArray,
                        color: colorPlanData
                    }, {
                        name: 'Факт',
                        data: factArray,
                        color: colorFactData
                    }]
                });
            }

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetNhUsersThisQua() {
    $.ajax({
        url: "/CMK/GetNhUsersThisQua/",
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
            Highcharts.chart('nhUsersThisQua', {
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
                    text: 'Нормачасы сотрудников',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    data: planArray,
                    color: colorPlanData
                }, {
                    name: 'Факт',
                    data: factArray,
                    color: colorFactData
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
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
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
                    data: dataArray,
                    color: colorFactData
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            style:{
                                color: colorStackLabels
                            }
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
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
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
                    data: dataArray,
                    color: colorFactData
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            style:{
                                color: colorStackLabels
                            }
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
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
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
                    data: dataArray,
                    color: colorFactData
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            style:{
                                color: colorStackLabels
                            }
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
            if (result === 1 || result === '1') {
                ManpowerUsersInMonth1();
            }
            else if (result === 'dkv@katek.by') {
                $('#speed1').hide();
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-8').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Добыш Константин Викторович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'jmv@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-10').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Жук Марина Владимировна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'rvi@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#container1-16').show();
                $.ajax({
                    url: "/CMK/GetUsersMMP1_16/",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        var normHoure = 0;
                        var normHoureFact = 0;
                        var dataArrayPlan = 0;
                        var dataArray10 = 0;
                        var dataArray20 = 0;
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
                        }
                        Highcharts.chart('container1-16', {
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
                                categories: ['Рачкевич Виталий Игоревич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'mpa@katek.by') {
                $('#container1-5').show();
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Маляревич Павел Анатольевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'faa@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-7').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Фейгина Анастасия Аркадьевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'Kuchynski@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-12').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Кучинский Андрей Юрьевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'fvs@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed16').hide();
                $('#container1-15').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Филончик Валентина Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'tmv@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-14').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Тиханский Максим Васильевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'nrf@katek.by') {
                $('#container1-6').show();
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Носик Роман Федорович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'voaa@katek.by') {
                $('#speed1').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-2').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Волкова Алена Александровна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'kaav@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-4').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Кальчинский Александр Владимирович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'lks@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-11').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Климович Ксения Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'jdo@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-9').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Жибуль Дмитрий Олегович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'goa@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-3').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Глебик Оксана Анатольевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'vi@katek.by') {
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed13').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-1').show();
                $.ajax({
                    url: "/CMK/GetUsersMMP1_1/",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {

                        var normHoure = 0;
                        var normHoureFact = 0;
                        var dataArrayPlan = 0;
                        var dataArray10 = 0;
                        var dataArray20 = 0;
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Васюхневич Илья']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'tys@katek.by') {
                $('#speed1').hide();
                $('#speed2').hide();
                $('#speed3').hide();
                $('#speed4').hide();
                $('#speed5').hide();
                $('#speed6').hide();
                $('#speed7').hide();
                $('#speed8').hide();
                $('#speed9').hide();
                $('#speed10').hide();
                $('#speed11').hide();
                $('#speed12').hide();
                $('#speed14').hide();
                $('#speed15').hide();
                $('#speed16').hide();
                $('#container1-13').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Тимашкова Юлия Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else {
                return 0;
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
            if (result === 1 || result === '1') {
                ManpowerUsersInMonth2();
            }
            else if (result === 'dkv@katek.by') {
                $('#container2-8').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Добыш Константин Викторович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'jmv@katek.by') {
                $('#container2-10').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Жук Марина Владимировна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'rvi@katek.by') {
                $('#container2-16').show();
                $.ajax({
                    url: "/CMK/GetUsersMMP2_16/",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        var normHoure = 0;
                        var normHoureFact = 0;
                        var dataArrayPlan = 0;
                        var dataArray10 = 0;
                        var dataArray20 = 0;
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
                        }
                        Highcharts.chart('container2-16', {
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
                                categories: ['Рачкевич Виталий Игоревич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'mpa@katek.by') {
                $('#container2-5').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Маляревич Павел Анатольевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'faa@katek.by') {
                $('#container2-7').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Фейгина Анастасия Аркадьевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'Kuchynski@katek.by') {
                $('#container2-12').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Кучинский Андрей Юрьевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'fvs@katek.by') {
                $('#container2-15').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Филончик Валентина Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'tmv@katek.by') {
                $('#container2-14').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Тиханский Максим Васильевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'nrf@katek.by') {
                $('#container2-6').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Носик Роман Федорович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'voaa@katek.by') {
                $('#container2-2').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Волкова Алена Александровна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'kaav@katek.by') {
                $('#container2-4').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Кальчинский Александр Владимирович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'lks@katek.by') {
                $('#container2-11').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Климович Ксения Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'jdo@katek.by') {
                $('#container2-9').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Жибуль Дмитрий Олегович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'goa@katek.by') {
                $('#container2-3').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Глебик Оксана Анатольевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'vi@katek.by') {
                $('#container2-1').show();
                $.ajax({
                    url: "/CMK/GetUsersMMP2_1/",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        var normHoure = 0;
                        var normHoureFact = 0;
                        var dataArrayPlan = 0;
                        var dataArray10 = 0;
                        var dataArray20 = 0;
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Васюхневич Илья']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'tys@katek.by') {
                $('#container2-13').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Тимашкова Юлия Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else {
                return 0;
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
            else if (result === 'dkv@katek.by') {
                $('#container3-8').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Добыш Константин Викторович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'jmv@katek.by') {
                $('#container3-10').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Жук Марина Владимировна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'rvi@katek.by') {
                $('#container3-16').show();
                $.ajax({
                    url: "/CMK/GetUsersMMP3_16/",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        var normHoure = 0;
                        var normHoureFact = 0;
                        var dataArrayPlan = 0;
                        var dataArray10 = 0;
                        var dataArray20 = 0;
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
                        }
                        Highcharts.chart('container3-16', {
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
                                categories: ['Рачкевич Виталий Игоревич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'mpa@katek.by') {
                $('#container3-5').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Маляревич Павел Анатольевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'faa@katek.by') {
                $('#container3-7').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Фейгина Анастасия Аркадьевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'Kuchynski@katek.by') {
                $('#container3-12').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Кучинский Андрей Юрьевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'fvs@katek.by') {
                $('#container3-15').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Филончик Валентина Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'tmv@katek.by') {
                $('#container3-14').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Тиханский Максим Васильевич']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'nrf@katek.by') {
                $('#container3-6').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Носик Роман Федорович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'voaa@katek.by') {
                $('#container3-2').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Волкова Алена Александровна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'kaav@katek.by') {
                $('#container3-4').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Кальчинский Александр Владимирович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'lks@katek.by') {
                $('#container3-11').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Климович Ксения Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'jdo@katek.by') {
                $('#container3-9').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Жибуль Дмитрий Олегович']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'goa@katek.by') {
                $('#container3-3').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Глебик Оксана Анатольевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'vi@katek.by') {
                $('#container3-1').show();
                $.ajax({
                    url: "/CMK/GetUsersMMP3_1/",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        var normHoure = 0;
                        var normHoureFact = 0;
                        var dataArrayPlan = 0;
                        var dataArray10 = 0;
                        var dataArray20 = 0;
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Васюхневич Илья']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else if (result === 'tys@katek.by') {
                $('#container3-13').show();
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
                        var dataArray30 = 0;
                        for (var i = 0; i < 1; i++) {
                            normHoure = result[i].normHoure;
                            normHoureFact = result[i].normHoureFact;
                            dataArrayPlan = result[i].plan;
                            dataArray10 = result[i].plan10;
                            dataArray20 = result[i].plan20;
                            dataArray30 = result[i].plan30;
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
                                categories: ['Тимашкова Юлия Сергеевна']
                            },
                            yAxis: {
                                min: 0,
                                max: dataArray30,
                                labels: {
                                    enabled: false
                                },
                                plotBands: [{
                                    from: 0,
                                    to: dataArrayPlan,
                                    color: colorBgBasicLine,
                                    label: {
                                        "text": dataArrayPlan,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArrayPlan,
                                    to: dataArray10,
                                    color: colorBg10Line,
                                    label: {
                                        "text": dataArray10,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray10,
                                    to: dataArray20,
                                    color: colorBg20Line,
                                    label: {
                                        "text": dataArray20,
                                        align: 'right',
                                        x: 10,
                                        y: -10
                                    }
                                }, {
                                    from: dataArray20,
                                    to: dataArray30,
                                    color: colorBg30Line,
                                    label: {
                                        "text": dataArray30,
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
            else {
                return 0;
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
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Васюхневич Илья']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Волкова Алена Александровна']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Глебик Оксана Анатольевна']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Добыш Константин Викторович']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Жибуль Дмитрий Олегович']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Жук Марина Владимировна']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Кальчинский Александр Владимирович']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Климович Ксения Сергеевна']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Кучинский Андрей Юрьевич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Маляревич Павел Анатольевич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Носик Роман Федорович']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
        url: "/CMK/GetUsersMMP1_16/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container1-16', {
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
                    categories: ['Рачкевич Виталий Игоревич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Тимашкова Юлия Сергеевна']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Тиханский Максим Васильевич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Фейгина Анастасия Аркадьевна']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['Филончик Валентина Сергеевна']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
    $.ajax({
        url: "/CMK/GetUsersMMP2_1/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
        url: "/CMK/GetUsersMMP2_16/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container2-16', {
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
    $.ajax({
        url: "/CMK/GetUsersMMP3_1/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
        url: "/CMK/GetUsersMMP3_16/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container3-16', {
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
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
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    labels:{
                        enabled: false
                    },
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: colorBgBasicLine,
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: colorBg10Line,
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: colorBg20Line,
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: colorBg30Line,
                            label: {
                                "text": dataArray30,
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
            var userAccured = 0;
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
            var tax = new Array();
            var rate = new Array();
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
                tax.push(result[i].Tax);
                rate.push(result[i].Rate);
            }
            var heightChart = 500;
            if (counter < 2) {
                heightChart = 220;
                userAccured = result[0].UserAccured;
                document.getElementById("textUserAccuredPlan").textContent = "Расчетная премия: " + userAccured.toString();
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('usersAccuredPlan', {
                chart: {
                    type: 'bar',
                    height: heightChart
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'Плановые начисления сотрудника/ов',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                },{
                    name: 'Налоги',
                    data: tax
                },{
                    name: 'Оклад',
                    data: rate
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
            var userAccured = 0;
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
            var tax = new Array();
            var rate = new Array();
            for (var i = 0; i < counter; i++) {
                ciliricName.push(result[i].CiliricName);
                bonusReversed.push(result[i].BonusReversed);
                accrued.push(result[i].Accrued);
                accruedg.push(result[i].Accruedg);
                manager.push(result[i].Manager);
                bonusQuality.push(result[i].BonusQuality);
                speed.push(result[i].Speed);
                optimization.push(result[i].Optimization);
                teach.push(result[i].Teach);
                tax.push(result[i].Tax);
                rate.push(result[i].Rate);
            }
            var heightChart = 500;
            if (counter < 2) {
                heightChart = 220;
                userAccured = result[0].UserAccured;
                document.getElementById("textAccuredUserFact").textContent = "Расчетная премия: " + userAccured.toString();
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('usersAccuredFact', {
                chart: {
                    type: 'bar',
                    height: heightChart
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: 'Фактические начисления сотрудника/ов',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                }, {
                    name: 'Начисления ГИПов',
                    data: accruedg
                }, {
                    name: 'Бонус за отработку',
                    data: speed
                }, {
                    name: 'Остаток бонусного фонда',
                    data: bonusReversed
                }, {
                    name: 'Бонус за качество',
                    data: bonusQuality
                }, {
                    name: 'Бонус за оптимизацию',
                    data: optimization
                }, {
                    name: 'Начисление за обучение',
                    data: teach
                }, {
                    name: 'Руководительские начисления',
                    data: manager
                }, {
                    name: 'Налоги',
                    data: tax
                }, {
                    name: 'Оклад',
                    data: rate
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetTimeSheet() {
    $.ajax({
        url: "/CMK/GetTimesheet/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = result[0].stepDate;
            var dateArray = new Array();
            for (var i = 1; i < lenghtArrayResult + 1; i++) {
                dateArray.push(result[i].date);
            }
            var usersArray = new Array();
            lenghtArrayResult = Object.keys(result).length;
            for (i = 1; i < lenghtArrayResult; i = i + result[0].stepDate) {
                usersArray.push(result[i].user);
            }
            var dataArray = new Array();
            var dataForArray = new Array();
            for (i = 1; i < lenghtArrayResult; i++) {
                dataForArray = [result[i].stepDate, result[i].stepUser, result[i].data];
                dataArray.push(dataForArray);
            }
            Highcharts.chart('timesheetContainer', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'heatmap'
                },
                title: {
                    text: 'Отработанное время сотрудниками',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: dateArray
                },
                yAxis: {
                    categories: usersArray,
                    title: null
                },
                colorAxis: {
                    minColor: "#fff",
                    maxColor: colorFactData
                },
                series: [{
                    name: 'ч.',
                    borderWidth: 0,
                    data: dataArray,
                    dataLabels: {
                        enabled: true,
                        color: titleDiagrammColor,
                        style: {
                            textOutline: "0px contrast"
                        }
                    }
                }],
                tooltip:{
                    enabled: false
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetCoefWorker() {
    $.ajax({
        url: "/CMK/GetCoefWorker/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var counter = Object.keys(result).length;
            var workerArray = new Array();
            var planArray = new Array();
            for (var i = 0; i < counter; i++) {
                workerArray.push(result[i].FullName);
                planArray.push(result[i].Data);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('coefWorker', {
                legend: {
                    enabled: false
                },
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
                    text: 'Коэф. качества инженеров-конструкторов',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },
                    max: 1,
                    min: 0,
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    name: 'Коэф.',
                    data: planArray,
                    color: colorFactData
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetCoefWorkerG() {
    $.ajax({
        url: "/CMK/GetCoefWorkerG/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var counter = Object.keys(result).length;
            var workerArray = new Array();
            var planArray = new Array();
            for (var i = 0; i < counter; i++) {
                workerArray.push(result[i].FullName);
                planArray.push(result[i].Data);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('coefWorkerG', {
                legend: {
                    enabled: false
                },
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
                    text: 'Коэф. качества ГИПов',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },
                    max: 1,
                    min: 0,
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: colorStackLabels
                        }
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
                    name: 'Коэф.',
                    data: planArray,
                    color: colorFactData
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getRemainingWorkAll() {
    $.ajax({
        url: "/ReportPage/GetRemainingWorkAll/",
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
            Highcharts.chart('remainingWorkAll', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Оставшиеся тр-ты с учетом НИОКРов (сотрудник)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray
                },
                series: [{
                    color: colorFactData,
                    name: 'НЧ',
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
                            enabled: true,
                            format: '{point.y}'
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

function getRemainingWork() {
    $.ajax({
        url: "/ReportPage/GetRemainingWork/",
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
            Highcharts.chart('remainingWork', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Оставшиеся тр-ты по заказам (сотрудник)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray
                },
                series: [{
                    color: colorFactData,
                    name: 'НЧ',
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
                            enabled: true,
                            format: '{point.y}'
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

function getRemainingWorkDevisionAll() {
    $.ajax({
        url: "/ReportPage/GetRemainingDevisionWorkAll/",
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
            Highcharts.chart('remainingWorkDevisionAll', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Оставшиеся тр-ты с учетом НИОКРов (бюро)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray
                },
                series: [{
                    color: colorFactData,
                    name: 'НЧ',
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
                            enabled: true,
                            format: '{point.y}'
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

function getRemainingDevisionWork() {
    $.ajax({
        url: "/ReportPage/GetRemainingDevisionWork/",
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
            Highcharts.chart('remainingWorkDevision', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Оставшиеся тр-ты по заказам (бюро)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray
                },
                series: [{
                    color: colorFactData,
                    name: 'НЧ',
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
                            enabled: true,
                            format: '{point.y}'
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

function getRemainingWorkAllE() {
    $.ajax({
        url: "/ReportPage/GetRemainingWorkAllE/",
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
            Highcharts.chart('remainingWorkAllE', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Оставшиеся тр-ты с учетом НИОКРов (сотрудник)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray
                },
                series: [{
                    color: colorFactData,
                    name: 'НЧ',
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
                            enabled: true,
                            format: '{point.y}'
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

function getRemainingWorkE() {
    $.ajax({
        url: "/ReportPage/GetRemainingWorkE/",
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
            Highcharts.chart('remainingWorkE', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Оставшиеся тр-ты по заказам (сотрудник)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray
                },
                series: [{
                    color: colorFactData,
                    name: 'НЧ',
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
                            enabled: true,
                            format: '{point.y}'
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

function GetReclamationView(id) {
    $.ajax({
        cache: false,
        url: "/Remarks/GetReclamation/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#numberReclamation').val('Замечание №: ' + result.id);
            $('#id_Reclamation_Type').val(result.id_Reclamation_Type);
            $('#id_DevisionReclamation').val(result.id_DevisionReclamation);
            $('#id_Reclamation_CountErrorFirst').val(result.id_Reclamation_CountErrorFirst);
            $('#id_Reclamation_CountErrorFinal').val(result.id_Reclamation_CountErrorFinal);
            $('#vid_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_DevisionCreate').val(result.id_DevisionCreate);
            $('#vdateTimeCreate').val(result.dateTimeCreate);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#timeToSearch').val(result.timeToSearch);
            $('#timeToEliminate').val(result.timeToEliminate);
            $('#close').prop('checked', result.close);
            $('#gip').prop('checked', result.gip);
            $('#closeDevision').prop('checked', result.closeDevision);
            $('#PCAM').val(result.PCAM);
            $('#answerHistiryText').val(result.answerHistiryText);
            $('#editManufacturing').prop('checked', result.editManufacturing);
            $('#editManufacturingIdDevision').val(result.editManufacturingIdDevision);
            $('#id_PF').val(result.id_PF);
            $('#technicalAdvice').prop('checked', result.technicalAdvice);
            $('#id_AspNetUsersError').val(result.id_AspNetUsersError);
            $('#reloadDevision').val("");
            $('#reload').prop('checked', false);
            $('#id').prop('disabled', true);
            $('#id_Reclamation_Type').prop('disabled', true);
            $('#id_DevisionReclamation').prop('disabled', true);
            $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
            $('#id_Reclamation_CountErrorFinal').prop('disabled', true);
            $('#id_AspNetUsersCreate').prop('disabled', true);
            $('#id_DevisionCreate').prop('disabled', true);
            $('#dateTimeCreate').prop('disabled', true);
            $('#text').prop('disabled', true);
            $('#description').prop('disabled', true);
            $('#timeToSearch').prop('disabled', true);
            $('#timeToEliminate').prop('disabled', true);
            $('#close').prop('disabled', true);
            $('#gip').prop('disabled', true);
            $('#closeDevision').prop('disabled', true);
            $('#PCAM').prop('disabled', true);
            $('#editManufacturing').prop('disabled', true);
            $('#editManufacturingIdDevision').prop('disabled', true);
            $('#id_PF').prop('disabled', true);
            $('#technicalAdvice').prop('disabled', true);
            $('#id_AspNetUsersError').prop('disabled', true);
            $('#reloadDevision').prop('disabled', true);
            $('#reload').prop('disabled', true);
            $('#answerHistiryText').prop('disabled', true);
            $('#answerText').prop('disabled', true);
            $('#trash').prop('disabled', true);
            $('#pZ_PlanZakaz').prop('disabled', true);
            $("#pZ_PlanZakaz").val(result.pZ_PlanZakaz).trigger("chosen:updated");
            $('#viewReclamation').modal('show');
            $('#btnUpdate').hide();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}