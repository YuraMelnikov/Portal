$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/OTK/ActiveReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "columns": [
            { "title": "Ред", "data": "Id", "autowidth": true, "bSortable": false },
            { "title": "См", "data": "IdRead", "autowidth": true, "bSortable": false },
            { "title": "Заводской номер", "data": "PlanZakaz", "autowidth": true, "className": 'text-center' },
            { "title": "Дата открытия", "data": "DateCreate", "autowidth": true, "bSortable": false, "className": 'text-center' },
            //{ "title": "Статус", "data": "StatusOrder", "autowidth": true },
            { "title": "Заказчик", "data": "NameSort", "class": 'colu-200' },
            { "title": "Контрактное наименование", "data": "Name", "bSortable": false, "class": 'colu-300' },
            { "title": "Наименование по ТУ", "data": "nameTU", "bSortable": false, "class": 'colu-200' },
            { "title": "Опросный лист №", "data": "OL", "autowidth": true, "bSortable": false },
            { "title": "Договор №", "data": "timeContract", "autowidth": true, "bSortable": false },
            { "title": "Дата договора", "data": "timeContractDate", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull },
            { "title": "Приложение №", "data": "timeArr", "autowidth": true, "bSortable": false },
            { "title": "Дата приложения", "data": "timeArrDate", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull },
            { "title": "Требуемая дата поставки", "data": "DateShipping", "autowidth": true, "bSortable": false, "className": 'text-center' },
            { "title": "Требуемая дата отгрузки", "data": "DateSupply", "autowidth": true, "bSortable": false, "className": 'text-center' },
            { "title": "Оператор договора", "data": "OperatorDogovora", "bSortable": false },
            { "title": "Куратор договора", "data": "KuratorDogovora", "bSortable": false },
            { "title": "Способ доставки", "data": "typeDostavka", "autowidth": true, "bSortable": false },
            { "title": "Контрактная цена, без НДС", "data": "Cost", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
            { "title": "Стоимость ШМР, без НДС", "data": "costSMR", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
            { "title": "Стоимость ПНР, без НДС", "data": "costPNR", "autowidth": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
            //{ "title": "С/ф №", "data": "SF", "autowidth": true },
            { "title": "Код МТР Заказчика", "data": "MTR", "autowidth": true, "bSortable": false },
            { "title": "Номенклатурный номер", "data": "nomenklaturNumber", "autowidth": true, "bSortable": false },
            { "title": "Менеджер", "data": "Manager", "fixedColumns": true, "width": '500px', "class": 'colu-200' },
            { "title": "Примечание", "data": "Description", "autowidth": true, "bSortable": false, "class": 'colu-200' },
            { "title": "Закупка №", "data": "numZakupki", "autowidth": true, "bSortable": false },
            { "title": "Лот №", "data": "numLota", "autowidth": true, "bSortable": false },
            { "title": "Запрос №", "data": "Zapros", "autowidth": true, "bSortable": false, "className": 'text-center' },
            { "title": "Тип продукции", "data": "ProductType", "autowidth": true, "bSortable": false },
            { "title": "Дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true, "bSortable": false, "className": 'text-center' }
            //{ "title": "Дата доставки", "data": "dateDostavki", "autowidth": true },
            //{ "title": "Дата приемки", "data": "datePriemki", "autowidth": true },
            //{ "title": "Дата оплаты", "data": "dateOplat", "autowidth": true }
        ],
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "fixedColumns": {
            "leftColumns": 3
        },
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        },
        initComplete: function () {
            this.api().columns([3, 4, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 25, 26, 24, 27]).every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>');
                });
            });
        }
    });
}
