$(document).ready(function () {
    if (userGroupId !== 1) {
        $('#btnAddNewReclamations').hide();
    }
    startMenu();
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
    { "title": "Ред", "data": "editLink", "autowidth": true, "bSortable": true },
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Срок", "data": "workDateTime", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Дата исполнения", "data": "manufDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "№ заявки", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Папка заказа", "data": "folder", "autowidth": true, "bSortable": true }
];

function startMenu() {
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/ActiveList",
            "type": "POST",
            "datatype": "json"
        },
        //"order": [[3, "desc"]],
        "processing": true,
        "columns": objViewList,
        //"rowCallback": function (row, data, index) {
        //    if (data.status === "Не отправлен") {
        //        $('td', row).css('background-color', '#ebaca2');
        //    }
        //    else if (data.status === "Ожидание сроков") {
        //        $('td', row).css('background-color', '#ffc87c');
        //    }
        //    else if (data.status === "Производится") {
        //        $('td', row).css('background-color', '#a6e9d7');
        //    }
        //},
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