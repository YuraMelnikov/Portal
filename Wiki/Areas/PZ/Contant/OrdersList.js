$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/Order/OrdersList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "columns": [
            { "title": "Заводской номер", "data": "PlanZakaz", "autowidth": true },
            { "title": "Тип продукции", "data": "ProductType", "autowidth": true },
            { "title": "Дата открытия", "data": "DateCreate", "autowidth": true },
            { "title": "Статус", "data": "StatusOrder", "autowidth": true },
            { "title": "Менеджер", "data": "Manager", "autowidth": true },
            { "title": "Примечание", "data": "Description", "autowidth": true },
            { "title": "Код МТР Заказчика", "data": "MTR", "autowidth": true },
            { "title": "Номенклатурный номер", "data": "nomenklaturNumber", "autowidth": true },
            { "title": "Контрактное наименование", "data": "Name", "autowidth": true },
            { "title": "Наименование по ТУ", "data": "nameTU", "autowidth": true },
            { "title": "Опросный лист №", "data": "OL", "autowidth": true },
            { "title": "Заказчик", "data": "NameSort", "autowidth": true },
            { "title": "Запрос №", "data": "Zapros", "autowidth": true },
            { "title": "Закупка №", "data": "numZakupki", "autowidth": true },
            { "title": "Лот №", "data": "numLota", "autowidth": true },
            { "title": "Договор №", "data": "timeContract", "autowidth": true },
            { "title": "Дата договора", "data": "timeContractDate", "autowidth": true },
            { "title": "Приложение №", "data": "timeArr", "autowidth": true },
            { "title": "Дата приложения", "data": "timeArrDate", "autowidth": true },
            { "title": "Требуемая дата поставки", "data": "DateShipping", "autowidth": true },
            { "title": "Требуемая дата отгрузки", "data": "DateSupply", "autowidth": true },
            { "title": "Оператор договора", "data": "OperatorDogovora", "autowidth": true },
            { "title": "Куратор договора", "data": "KuratorDogovora", "autowidth": true },
            { "title": "Способ доставки", "data": "typeDostavka", "autowidth": true },
            { "title": "Контрактная цена, без НДС", "data": "Cost", "autowidth": true },
            { "title": "Стоимость ШМР, без НДС", "data": "costSMR", "autowidth": true },
            { "title": "Стоимость ПНР, без НДС", "data": "costPNR", "autowidth": true },
            { "title": "С/ф №", "data": "SF", "autowidth": true },
            { "title": "Дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true },
            { "title": "Дата доставки", "data": "dateDostavki", "autowidth": true },
            { "title": "Дата приемки", "data": "datePriemki", "autowidth": true },
            { "title": "Дата оплаты", "data": "dateOplat", "autowidth": true }
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