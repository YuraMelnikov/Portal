$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/CMO2/Report",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": [
            { "title": "Позиция", "data": "status", "autowidth": true }
            , { "title": "Подрядчик", "data": "edit", "autowidth": true }
            , { "title": "Первая цена, б/НДС (BYN)", "data": "PlanZakaz", "autowidth": true, "bSortable": false }
            , { "title": "Изг. дн.", "data": "Name", "autowidth": true, "bSortable": false }
            , { "title": "Стоимость, б/НДС (BYN)", "data": "Manager", "autowidth": true, "bSortable": false }
            , { "title": "Дата размещения", "data": "Client", "autowidth": true, "bSortable": false }
            , { "title": "Дата исполнения", "data": "dataOtgruzkiBP", "autowidth": true, "bSortable": false }
            , { "title": "Дата поступления", "data": "DateSupply", "autowidth": true, "bSortable": false }
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