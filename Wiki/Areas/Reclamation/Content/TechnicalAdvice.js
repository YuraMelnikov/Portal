$(document).ready(function () {
    startMenu();
    $('#pageData').hide();
});

function loadData(listId) {
    document.getElementById('pageData').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        activeTA();
    }
    else if (listId === 2 || listId === "2") {
        protocols();
    }
    else if (listId === 3 || listId === "3") {
        allDataProtocols();
    }
    else {
        activeTA();
    }
}

var objRemarksList = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "EditLinkJS", "autowidth": true, "bSortable": false },
    { "title": "См", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "Text", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Прим.", "data": "Description", "autowidth": true, "bSortable": false },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Решение", "data": "UserCreate", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Направил на ТС", "data": "UserCreate", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Создал", "data": "UserCreate", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Ответственное СП", "data": "Devision", "autowidth": true, "bSortable": false }
];



var objProtocol = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "См", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Word", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Дата заседания", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Кол-во рекламаций", "data": "PlanZakaz", "autowidth": true, "bSortable": true }
];

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetActiveTA",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objRemarksList,
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

function activeTA() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetActiveTA",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objRemarksList,
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

function protocols() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetProtocols",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objProtocol,
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

function allDataProtocols() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetAllRemarks",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objRemarksList,
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

function getTAEdit(id) {

}

function getTAView(id) {

}

function downloadProtocol(id) {

}

function getProtocol(id) {

}