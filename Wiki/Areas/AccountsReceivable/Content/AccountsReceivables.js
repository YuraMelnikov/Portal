$(document).ready(function () {
    startMenu();
    if (userGroupId === 1) {
        loadData(2);
    }
});

function loadData(listId) {
    document.getElementById('pageId').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        loadReport();
    }
    else if (listId === 2 || listId === "2") {
        loadOS();
    }
    else {
        loadReport();
    }
}

var objSmallReport = [
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Изг. дн.", "data": "day", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Дата создания", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Дата размещения", "data": "workDateTime", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Дата исполнения", "data": "manufDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Дата поступления", "data": "finDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "№ заявки", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Статус", "data": "status", "autowidth": true, "bSortable": true },
    { "title": "Папка заказа", "data": "folder", "autowidth": true }
];