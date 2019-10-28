$(document).ready(function () {
    $('#optimizationTable').hide();
    $('#teachTable').hide();
    $('#usersTable').hide();
    $('#categoryTable').hide();
    $('#periodsTable').hide();
    $('#calendTable').hide();
    $('#cyrencyTable').hide();
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

var objTeach = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Создано", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Кем создано", "data": "userCreate", "autowidth": true, "bSortable": true },
    { "title": "Учитель", "data": "teacher", "autowidth": true, "bSortable": true },
    { "title": "Обучаемый", "data": "student", "autowidth": true, "bSortable": true },
    { "title": "Прим.", "data": "description", "autowidth": true, "bSortable": false, "class": 'colu-300' },
    { "title": "Период (для учета)", "data": "period", "autowidth": true, "bSortable": true }
];

function StartMenu() {
    $("#categoryTable").DataTable({
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
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objUsers,
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

function AddTeach() {
    var res = ValidTeach();
    if (res === false){
        return false;
    }
    $('#btnAddTeach').attr('disabled', true);
    var addObjTeach = {
        id: $('#idTeach').val(),
        teacherTeach: $('#teacherTeach').val(),
        studentTeach: $('#studentTeach').val(),
        id_CMKO_PeriodResult: $('#periodTeach').val(),
        cost: $('#costTeach').val(),
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
            $('#studentTeach').va(result.studentTeach);
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
        cost: $('#costTeach').val(),
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
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objUsers,
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
            $('#devisionNameUser').va(result.devisionNameUser);
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
        tax: $('#taxUser').val()
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
    if ($('#nameCategory').val() === null) {
        $('#nameCategory').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#nameCategory').css('border-color', 'lightgrey');
    }
    if ($('#selaryCategory').val() === null) {
        $('#selaryCategory').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#selaryCategory').css('border-color', 'lightgrey');
    }
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
            $('#selaryCategory').va(result.selaryCategory);
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
        salary: $('#selaryCategory').val()
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
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
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
    if ($('#namePeriod').val() === null) {
        $('#namePeriod').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#namePeriod').css('border-color', 'lightgrey');
    }
}