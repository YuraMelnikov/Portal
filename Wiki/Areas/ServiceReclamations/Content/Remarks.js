$(document).ready(function () {
    if (userGroupId !== 1) {
        $('#btnAddNewReclamations').hide();
    }
    activeReclamation();
});

function loadData(listId) {
    clearTextBox();
    document.getElementById('pageData').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        activeReclamation();
    }
    else if (listId === 2 || listId === "2") {
        closeReclamation();
    }
    else if (listId === 3 || listId === "3") {
        allReclamation();
    }
}

var objViewList = [
    { "title": "№", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "См", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа/ов", "data": "orders", "autowidth": true, "bSortable": true },
    { "title": "Покупатель", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Тип", "data": "types", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "text", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Причина возникновения", "data": "causes", "autowidth": true, "bSortable": false },
    { "title": "Прим.", "data": "description", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Дата открытия", "data": "dateOpen", "autowidth": true, "bSortable": true },
    { "title": "Дата получения", "data": "dateGet", "autowidth": true, "bSortable": true },
    { "title": "Дата закрытия", "data": "dateClose", "autowidth": true, "bSortable": true },
    { "title": "Папка (IE)", "data": "folder", "autowidth": true, "bSortable": false }
];

function activeReclamation() {
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/ActiveList",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objViewList,
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

function closeReclamation() {
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/CloseList",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objViewList,
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

function allReclamation() {
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/AllList",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objViewList,
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

//reclamationModal

//clearTextBox

//add

//get(id)

//getView(id)

//update

//validate

//?NewReclamation