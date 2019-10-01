$(document).ready(function () {
    startMenu();
});

var objList = [
    { "title": "Ид.", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "edit", "autowidth": true, "bSortable": false },
    { "title": "Сотрудник", "data": "user", "autowidth": true, "bSortable": true },
    { "title": "Период", "data": "period", "autowidth": true, "bSortable": true },
    { "title": "Коэф.", "data": "coef", "autowidth": true, "bSortable": false }
];

function startMenu() {
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/ActiveReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objRemarksList,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
        },
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

//getList
//get
//update
//valid