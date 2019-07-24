$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/DSetup/List",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "bDestroy": true,
        "bAutoWidth": false,
        "columns": [
            { "title": "Номер", "data": "PlanZakaz", "autowidth": true }
            , { "title": "Менеджер", "data": "Manager", "autowidth": true }
            , { "title": "Заказчик", "data": "Client", "autowidth": true }
            , { "title": "Кол-во дней на приемку", "data": "KolVoDneyNaPrijemku", "autowidth": true }
            , { "title": "Условия приемки изделия", "data": "PunktDogovoraOSrokahPriemki", "autowidth": true }
            , { "title": "Условия оплаты", "data": "UslovieOplatyText", "autowidth": true }
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