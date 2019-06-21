$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/DTasks/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": [
            { "title": "Номер", "data": "PlanZakaz", "autowidth": true }
            , { "title": "Менеджер", "data": "Manager", "autowidth": true, "class": 'colu-200' }
            , { "title": "Заказчик", "data": "Client", "autowidth": true, "class": 'colu-200' }
            , { "title": "Наименование", "data": "Name", "autowidth": true, "bSortable": false }
            , { "title": "Фактическая дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true, "bSortable": true, "className": 'text-center' }
            , { "title": "Договорная дата поставки", "data": "DateSupply", "autowidth": true, "bSortable": true, "className": 'text-center' }
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