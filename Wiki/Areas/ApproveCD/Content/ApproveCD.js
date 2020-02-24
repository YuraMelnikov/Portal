//1 - KO
//2 - TP
//3 - All
//4 - Admin

var vhScrollY = '50vh';

$(document).ready(function () {
    $('#BtnAddQuestion').hide();
    $('#BtnAddTask').hide();
    $('#hideIdOrder').hide();
    StartMenu();
    LoadData(1);
});

function LoadData(id) {
    if (id === 1 || id === "1") {
        document.getElementById("labelOrdersTable").textContent = "Несогласованные заказы";
        document.getElementById("labelQuestionsTable").textContent = "Активные вопросы";
        document.getElementById("labelActionsTable").textContent = "Лента событий";
        GetNoApproveTable();
        GetTasksTable();
        GetNotCloseQuestionsTable();
    }
    if (id === 2 || id === "2") {
        document.getElementById("labelOrdersTable").textContent = "Согласованные заказы";
        GetApproveTable();
    }
    if (id === 3 || id === "3") {
        document.getElementById("labelQuestionsTable").textContent = "Активные вопросы";
        GetNotCloseQuestionsTable();
    }
    if (id === 4 || id === "4") {
        document.getElementById("labelQuestionsTable").textContent = "Закрытые вопросы";
        GetCloseQuestionsTable();
    }
}

var objOrders = [
    { "title": "См.", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Заказчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Состояние", "data": "state", "autowidth": true, "bSortable": true },
    { "title": "Дата отправки РКД", "data": "dateLastLoad", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull, "className": 'text-center' },
    { "title": "Текущая вер.", "data": "ver", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Дата открытия зак.", "data": "dateOpen", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Контрактный срок", "data": "contractDate", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "ГИП КБМ", "data": "gm", "autowidth": true, "bSortable": true },
    { "title": "ГИП КБЭ", "data": "ge", "autowidth": true, "bSortable": true }
];

var objQuestions = [
    { "title": "См.", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Ид. вопр.", "data": "idQue", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "Вопрос", "data": "que", "autowidth": true, "bSortable": false },
    { "title": "Ход обсуждения", "data": "queData", "autowidth": true, "bSortable": false },
    { "title": "Создан", "data": "createDate", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Кем создан", "data": "createUser", "autowidth": true, "bSortable": true }
];

var objTasks = [
    { "title": "Дата", "data": "dateAction", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Описание", "data": "action", "autowidth": true, "bSortable": false },
    { "title": "Ответственный", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Срок", "data": "deadline", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull }
];

function StartMenu() {
    $('#btnCloseQue').hide();
    if (leavelUser === 4 || leavelUser === '4') {
        $('#BtnAddOrders').show();
        $('#BtnAddQuestion').show();
        $('#BtnAddTask').show();
    }
    if (leavelUser === 1 || leavelUser === '1') {
        $('#BtnAddQuestion').show();
        $('#btnCloseQue').show();
    }
    $("#ordersTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Approve/GetNoApproveTable/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[2, "asc"]],
        "processing": true,
        "columns": objOrders,
        "cache": false,
        "async": false,
        "scrollY": vhScrollY,
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
    $("#questionsTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Approve/GetNotCloseQuestionsTable/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[2, "asc"]],
        "processing": true,
        "columns": objQuestions,
        "cache": false,
        "async": false,
        "scrollY": vhScrollY,
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
    $("#tasksTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Approve/GetTasksTable/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objTasks,
        "rowCallback": function (row, data, index) {
            if (data.distance === 2 || data.typeTask === "2") {
                $('td', row).css('background-color', '#ffff99');
            }
            if (data.distance === 3 || data.typeTask === "3") {
                $('td', row).css('background-color', '#d9d9d9');
            }
        },
        "cache": false,
        "async": false,
        "scrollY": vhScrollY,
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
    $("#concretTaskTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Approve/GetConcretTaskTable/" + 0,
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objTasks,
        "cache": false,
        "async": false,
        "scrollY": vhScrollY,
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

function GetNoApproveTable() {
    var days = new Date();
    days.setDate(days.getDate() - 9);
    var table = $('#ordersTable').DataTable();
    table.destroy();
    $('#ordersTable').empty();
    $("#ordersTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Approve/GetNoApproveTable/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "asc"]],
        "processing": true,
        "columns": objOrders,
        "rowCallback": function (row, data, index) {
            $('td', row).eq(5).addClass('highlightColor');
            var dateJSON = new Date(data.dateLastLoad);
            if (dateJSON < days) {
                $('td', row).eq(5).addClass('xhighlightColor');
            }
        },
        "scrollY": vhScrollY,
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

function GetApproveTable() {
    var table = $('#ordersTable').DataTable();
    table.destroy();
    $('#ordersTable').empty();
    $("#ordersTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Approve/GetApproveTable/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "asc"]],
        "processing": true,
        "columns": objOrders,
        "rowCallback": function (row, data, index) {
            $('td', row).eq(5).addClass('highlightColor');
        },
        "scrollY": vhScrollY,
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

function GetNotCloseQuestionsTable() {
    var table = $('#questionsTable').DataTable();
    table.destroy();
    $('#questionsTable').empty();
    $("#questionsTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Approve/GetNotCloseQuestionsTable/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "asc"]],
        "processing": true,
        "columns": objQuestions,
        "scrollY": vhScrollY,
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

function GetCloseQuestionsTable() {
    var table = $('#questionsTable').DataTable();
    table.destroy();
    $('#questionsTable').empty();
    $("#questionsTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Approve/GetCloseQuestionsTable/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "asc"]],
        "processing": true,
        "columns": objQuestions,
        "scrollY": vhScrollY,
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

function GetTasksTable() {
    var table = $('#tasksTable').DataTable();
    table.destroy();
    $('#tasksTable').empty();
    $("#tasksTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Approve/GetTasksTable/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objTasks,
        "scrollY": vhScrollY,
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

function GetConcretTaskTable(idOrder) {
    var table = $('#concretTaskTable').DataTable();
    table.destroy();
    $('#concretTaskTable').empty();
    $("#concretTaskTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Approve/GetConcretTaskTable/" + idOrder,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objTasks,
        "scrollY": '25vh',
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

function ClearNewOrdersFields() {
    $('#NewOrders').val("");
    $('#NewOrders').chosen();
    $('#NewOrders').trigger('chosen:updated');
}

function AddOrders() {
    var objNewOrders = {
        newOrders: $('#NewOrders').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/AddOrders",
        data: JSON.stringify(objNewOrders),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#NewOrdersModal").modal('hide');
            $('#ordersTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function ClearNewQuestionField() {
    $('#OrdersForQuestion').val("");
    $('#question').val("");
}

function AddQuestion() {
    var res = ValidAddQuestion();
    if (res === false) {
        return false;
    }
    var objNewQuestion = {
        orderIdForQuestion: $('#OrdersForQuestion').val(),
        question: $('#question').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/AddQuestion",
        data: JSON.stringify(objNewQuestion),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#QuestionModal").modal('hide');
            $('#questionsTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function ValidAddQuestion() {
    var isValid = true;
    if ($('#OrdersForQuestion').val() === null) {
        $('#OrdersForQuestion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#OrdersForQuestion').css('border-color', 'lightgrey');
    }
    if ($('#question').val() === '') {
        $('#question').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#question').css('border-color', 'lightgrey');
    }
    return isValid;
}

function AddTask() {
    var res = ValidAddTask();
    if (res === false) {
        return false;
    }
    var objNewTask = {
        ordersForTask: $('#OrdersForTask').val(),
        aSPUsers: $('#ASPUsers').val(),
        deadline: $('#deadline').val(),
        taskData: $('#taskData').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/AddTask",
        data: JSON.stringify(objNewTask),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#TaskModal").modal('hide');
            $('#tasksTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function ClearNewTaskField() {
    $('#OrdersForTask').val("");
    $('#ASPUsers').val("");
    $('#deadline').val("");
    $('#taskData').val("");
}

function ValidAddTask() {
    var isValid = true;
    if ($('#OrdersForTask').val() === null) {
        $('#OrdersForTask').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#OrdersForTask').css('border-color', 'lightgrey');
    }
    if ($('#taskData').val() === '') {
        $('#taskData').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#taskData').css('border-color', 'lightgrey');
    }
    return isValid;
}

function ClearUpdateQue() {
    $('#idQue').val("");
    $('#orderQue').val("");
    $('#dateCreateQue').val("");
    $('#autorQue').val("");
    $('#histQue').val("");
    $('#commitQue').val("");
}

function GetQuestionByIdForView(id) {
    ClearUpdateQue();
    $('#btnUpdateQue').hide();
    $.ajax({
        cache: false,
        url: "/Approve/GetQuestionById/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idQue').val(result.idQue);
            $('#orderQue').val(result.orderQue);
            $('#dateCreateQue').val(result.dateCreateQue);
            $('#autorQue').val(result.autorQue);
            $('#histQue').val(result.histQue);
            $('#questionTextU').val(result.questionTextU);
            $('#UQuestionModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetQuestionByIdForEdit(id) {
    ClearUpdateQue();
    $('#btnUpdateQue').show();
    $.ajax({
        cache: false,
        url: "/Approve/GetQuestionById/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idQue').val(result.idQue);
            $('#orderQue').val(result.orderQue);
            $('#dateCreateQue').val(result.dateCreateQue);
            $('#autorQue').val(result.autorQue);
            $('#histQue').val(result.histQue);
            $('#commitQue').val(result.commitQue);
            $('#questionTextU').val(result.questionTextU);
            $('#UQuestionModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UpdateQuestion() {
    var res = ValidUpdateQuestion();
    if (res === false) {
        return false;
    }
    var objQuestion = {
        idQue: $('#idQue').val(),
        commitQue: $('#commitQue').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/UpdateQuestion",
        data: JSON.stringify(objQuestion),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#UQuestionModal").modal('hide');
            $('#questionsTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function ValidUpdateQuestion() {
    var isValid = true;
    if ($('#commitQue').val() === '') {
        $('#commitQue').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#commitQue').css('border-color', 'lightgrey');
    }
    return isValid;
}

function ClearUOrderModalField() {
    $('#descriptionOrder').val("");
    $('#numVerCD').val("");
    $('#linkKD').val("");
    $('#commitTSToKO').val("");
    $('#commitTS').val("");
    $('#hideIdOrder').val("");
}

function GetOrderByIdForView(id){
    ClearUOrderModalField();
    GetConcretTaskTable(id);
    $('#loadVerDiv').hide();
    $('#ectionTS').hide();
    $('#getCustomer').hide();
    $.ajax({
        cache: false,
        url: "/Approve/GetOrderById/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#hideIdOrder').val(result.hideIdOrder);
            $('#descriptionOrder').val(result.descriptionOrder);
            $('#UOrderModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetOrderByIdForEdit(id) {
    ClearUOrderModalField();
    GetConcretTaskTable(id);
    $('#loadVerDiv').hide();
    $('#ectionTS').hide();
    $('#getCustomer').hide();
    $.ajax({
        cache: false,
        url: "/Approve/GetOrderById/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#hideIdOrder').val(result.hideIdOrder);
            $('#descriptionOrder').val(result.descriptionOrder);
            $('#numVerCD').val(result.numVerCD);
            if (result.showAction === '3') {
                $('#loadVerDiv').show();
            }
            if (result.showAction === '4') {
                $('#ectionTS').show();
            }
            if (result.showAction === '5') {
                $('#getCustomer').show();
            }
            $('#UOrderModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UpdateOrderLoadVer() {
    var res = ValidLoadVer();
    if (res === false) {
        return false;
    }
    var objVer = {
        hideIdOrder: $('#hideIdOrder').val(),
        numVerCD: $('#numVerCD').val(),
        linkKD: $('#linkKD').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/UpdateOrderLoadVer/",
        data: JSON.stringify(objVer),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#UOrderModal").modal('hide');
            $('#ordersTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function ValidLoadVer() {
    var isValid = true;
    if ($('#numVerCD').val() === '') {
        isValid = false;
        $('#numVerCD').css('border-color', 'Red');
    }
    else {
        $('#numVerCD').css('border-color', 'lightgrey');
    }
    if ($('#linkKD').val() === '') {
        isValid = false;
        $('#linkKD').css('border-color', 'Red');
    }
    else {
        $('#linkKD').css('border-color', 'lightgrey');
    }
    return isValid;
}

function ValidReloadKO() {
    var isValid = true;
    if ($('#commitTSToKO').val() === '') {
        $('#commitTSToKO').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#commitTSToKO').css('border-color', 'lightgrey');
    }
    return isValid;
}

function UpdateOrderGetTSToKOUpdate() {
    var res = ValidReloadKO();
    if (res === false) {
        return null;
    }
    var objVer = {
        hideIdOrder: $('#hideIdOrder').val(),
        commitTSToKO: $('#commitTSToKO').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/UpdateOrderGetTSToKOUpdate/",
        data: JSON.stringify(objVer),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#UOrderModal").modal('hide');
            $('#ordersTable').DataTable().ajax.reload(null, false);
            $('#tasksTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function UpdateOrderGetTSToKOComplited() {
    var objVer = {
        hideIdOrder: $('#hideIdOrder').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/UpdateOrderGetTSToKOComplited/",
        data: JSON.stringify(objVer),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#UOrderModal").modal('hide');
            $('#ordersTable').DataTable().ajax.reload(null, false);
            $('#tasksTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function ValidCustomerData() {
    var isValid = true;
    if ($('#commitTS').val() === '') {
        $('#commitTS').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#commitTS').css('border-color', 'lightgrey');
    }
    return isValid;
}

function UpdateOrderGetCustomerUpdate() {
    var res = ValidCustomerData();
    if (res === false) {
        return null;
    }
    var objVer = {
        hideIdOrder: $('#hideIdOrder').val(),
        commitTS: $('#commitTS').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/UpdateOrderGetCustomerUpdate/",
        data: JSON.stringify(objVer),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#UOrderModal").modal('hide');
            $('#ordersTable').DataTable().ajax.reload(null, false);
            $('#tasksTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function UpdateOrderGetCustomerComplited() {
    var objVer = {
        hideIdOrder: $('#hideIdOrder').val(),
        commitTS: $('#commitTS').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/UpdateOrderGetCustomerComplited/",
        data: JSON.stringify(objVer),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#UOrderModal").modal('hide');
            $('#ordersTable').DataTable().ajax.reload(null, false);
            $('#tasksTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}

function CloseQuestion() {
    var objQuestion = {
        idQue: $('#idQue').val(),
        commitQue: $('#commitQue').val()
    };
    $.ajax({
        cache: false,
        url: "/Approve/CloseQuestion",
        data: JSON.stringify(objQuestion),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#UQuestionModal").modal('hide');
            $('#questionsTable').DataTable().ajax.reload(null, false);
        },
        error: function () {
        }
    });
}