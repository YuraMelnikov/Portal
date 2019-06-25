$(document).ready(function () {
    startMenu();
    if (userGroupId === 1) {
        loadData(2);
    }
});

function loadData(listId) {
    document.getElementById('pageId').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        loadData(1);
    }
    else if (listId === 2 || listId === "2") {
        loadData(2);
    }
    else if (listId === 3 || listId === "3") {
        loadData(3);
    }
    else if (listId === 4 || listId === "4") {
        loadData(4);
    }
    else if (listId === 5 || listId === "5") {
        loadData(5);
    }
    else {
        loadData(1);
    }
}

var objTasks = [
    { "title": "См.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Задача", "data": "taskName", "autowidth": true, "bSortable": true },
    { "title": "Заказчик", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Исполнитель", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Плановая дата", "data": "planDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull }
];

var objEditTasks = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "См.", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Задача", "data": "taskName", "autowidth": true, "bSortable": true },
    { "title": "Заказчик", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Исполнитель", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Плановая дата", "data": "planDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull }
];

var objTEO = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "Выручка без НДС", "data": "Rate", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "С/С материалов", "data": "SSM", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "С/С зп рабочих", "data": "SSR", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Коммерчиские издержки", "data": "IzdKom", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "% по кредиту", "data": "IzdPPKredit", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Постоянные издержки", "data": "PI", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "НОП", "data": "NOP", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "КИ Прочие_Ш", "data": "KI_S", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "КИ Прочие", "data": "KI_prochee", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Отпускная цена без НДС", "data": "OtpuskChena", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Валюта", "data": "Currency", "autowidth": true, "bSortable": true },
    { "title": "Сумма НДС", "data": "NDS", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') }
];

var objContract = [
    { "title": "Номер", "data": "PlanZakaz", "autowidth": true }
    , { "title": "Менеджер", "data": "Manager", "autowidth": true }
    , { "title": "Заказчик", "data": "Client", "autowidth": true }
    , { "title": "Кол-во дней на приемку", "data": "KolVoDneyNaPrijemku", "autowidth": true }
    , { "title": "Условия приемки изделия", "data": "PunktDogovoraOSrokahPriemki", "autowidth": true }
    , { "title": "Условия оплаты", "data": "UslovieOplatyText", "autowidth": true }
];

function startMenu() {
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/TasksList",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTasks,
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

function tasksList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/TasksList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTasks,
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

function myTasksList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/MyTasksList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTasks,
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

function TEOList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/TEOList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objTEO,
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

function contractList() {
    var table = $('#tableData').DataTable();
    table.destroy();
    $('#tableData').empty();
    $("#tableData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/AccountsReceivables/ContractList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objContract,
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

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}

//getTEO
//getTask

//close1OpenInSystem
//close3Prototype




//close2TEO
//close4ContractSetting


