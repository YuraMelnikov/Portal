$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/Upload/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": [
            { "title": "Ред.", "data": "edit", "autowidth": true }
            , { "title": "Номер", "data": "PlanZakaz", "autowidth": true }
            , { "title": "Наименование", "data": "Name", "autowidth": true, "bSortable": false }
            , { "title": "Менеджер", "data": "Manager", "autowidth": true }
            , { "title": "Заказчик", "data": "Client", "autowidth": true }
            , { "title": "Фактическая дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true, "bSortable": false }
            , { "title": "Договорная дата поставки", "data": "DateSupply", "autowidth": true, "bSortable": false }
        ],
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}