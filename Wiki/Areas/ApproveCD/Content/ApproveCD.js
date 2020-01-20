//1 - KO
//2 - TP
//3 - All

var vhScrollY = '50vh';

$(document).ready(function () {
    StartMenu();
    LoadData(1);
});

function LoadData(id) {
    if (id === 1 || id === "1") {
        GetNoApproveTable();
        GetTasksTable();
        GetNotCloseQuestionsTable();
    }
    if (id === 2 || id === "2") {
        GetApproveTable();
    }
    if (id === 3 || id === "3") {
        GetNotCloseQuestionsTable();
    }
    if (id === 4 || id === "4") {
        GetCloseQuestionsTable();
    }
}

var objOrders = [
    { "title": "См.", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": false },
    { "title": "Состояние", "data": "state", "autowidth": true, "bSortable": false },
    { "title": "ГИП КБМ", "data": "gm", "autowidth": true, "bSortable": false },
    { "title": "ГИП КБЭ", "data": "ge", "autowidth": true, "bSortable": false },
    { "title": "Заказчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Открыт", "data": "dateOpen", "autowidth": true, "bSortable": true },
    { "title": "Контрактный срок", "data": "contractDate", "autowidth": true, "bSortable": true },
    { "title": "Текущая вер.", "data": "ver", "autowidth": true, "bSortable": true }
];

var objQuestions = [
    { "title": "См.", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": false },
    { "title": "Ид. вопр.", "data": "idQue", "autowidth": true, "bSortable": false },
    { "title": "Вопрос", "data": "que", "autowidth": true, "bSortable": false },
    { "title": "Ход обсуждения", "data": "queData", "autowidth": true, "bSortable": false },
    { "title": "Создан", "data": "createDate", "autowidth": true, "bSortable": true },
    { "title": "Кем создан", "data": "createUser", "autowidth": true, "bSortable": true }
];

var objTasks = [
    { "title": "Дата", "data": "dateAction", "autowidth": true, "bSortable": false },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": false },
    { "title": "Описание", "data": "action", "autowidth": true, "bSortable": false },
    { "title": "Ответственный", "data": "user", "autowidth": true, "bSortable": false },
    { "title": "Срок", "data": "deadline", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull }
];

function StartMenu() {
    $("#ordersTable").DataTable({
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
        "ajax": {
            "cache": false,
            "url": "/Approve/GetTasksTable/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objTasks,
        "rowCallback": function (row, data, index) {
            if (data.distance === 2 || data.typeTask === "2") {
                $('td', row).css('background-color', '#FF4500');
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
}

function GetNoApproveTable() {
    var table = $('#ordersTable').DataTable();
    table.destroy();
    $('#ordersTable').empty();
    $("#ordersTable").DataTable({
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
        "order": [[0, "asc"]],
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

function GetTasksTableByOrder() {

}

function GetOrderByIdForView(id) {

}

function GetOrderByIdForEdit(id) {

}

function UpdateOrder() {

}

function AddQuestion() {

}

function GetQuestionByIdForView() {

}

function GetQuestionByIdForEdit() {

}

function UpdateQuestion() {

}

function ValidAddQuestion() {

}

function ValidUpdateQuestion() {

}