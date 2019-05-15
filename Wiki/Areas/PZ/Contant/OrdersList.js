$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Order/OrdersList",
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

function OrdersListLY(yearCreateOrder) {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Order/OrdersListLY?yearCreateOrder=" + yearCreateOrder,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "bAutoWidth": false,
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

function OrdersListALL() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Order/OrdersListALL",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "bAutoWidth": false,
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

function OrdersListInManufacturing() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Order/OrdersListInManufacturing",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "bAutoWidth": false,
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

function Add() {
    var res = validate();
    if (res === false) {
        return false;
    }
    $("#btnAdd").attr('disabled', true);
    var typeObj = {
        countOrders: $('#countOrders').val(),
        PlanZakaz: $('#PlanZakaz').val(),
        DateCreate: $('#DateCreate').val(),
        Manager: $('#Manager').val(),
        Client: $('#Client').val(),
        id_PZ_OperatorDogovora: $('#id_PZ_OperatorDogovora').val(),
        id_PZ_FIO: $('#id_PZ_FIO').val(),
        Name: $('#Name').val(),
        Description: $('#Description').val(),
        MTR: $('#MTR').val(),
        nomenklaturNumber: $('#nomenklaturNumber').val(),
        timeContract: $('#timeContract').val(),
        timeContractDate: $('#timeContractDate').val(),
        timeArr: $('#timeArr').val(),
        timeArrDate: $('#timeArrDate').val(),
        DateShipping: $('#DateShipping').val(),
        DateSupply: $('#DateSupply').val(),
        Dostavka: $('#Dostavka').val(),
        Cost: $('#Cost').val(),
        costSMR: $('#costSMR').val(),
        costPNR: $('#costPNR').val(),
        ProductType: $('#ProductType').val(),
        OL: $('#OL').val(),
        Zapros: $('#Zapros').val(),
        numZakupki: $('#numZakupki').val(),
        numLota: $('#numLota').val(),
        Gruzopoluchatel: $('#Gruzopoluchatel').val(),
        PostAdresGruzopoluchatel: $('#PostAdresGruzopoluchatel').val(),
        INNGruzopoluchatel: $('#INNGruzopoluchatel').val(),
        OKPOGruzopoluchatelya: $('#OKPOGruzopoluchatelya').val(),
        KodGruzopoluchatela: $('#KodGruzopoluchatela').val(),
        StantionGruzopoluchatel: $('#StantionGruzopoluchatel').val(),
        KodStanciiGruzopoluchatelya: $('#KodStanciiGruzopoluchatelya').val(),
        OsobieOtmetkiGruzopoluchatelya: $('#OsobieOtmetkiGruzopoluchatelya').val(),
        PowerST: $('#PowerST').val(),
        Modul: $('#Modul').val(),
        VN_NN: $('#VN_NN').val(),
        TypeShip: $('#TypeShip').val(),
        criticalDateShip: $('#criticalDateShip').val(),
        DescriptionGruzopoluchatel: $('#DescriptionGruzopoluchatel').val()
    };
    $.ajax({
        cache: false,
        url: "/Order/Add",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#orderModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;

    if ($('#countOrders').val().trim() === "") {
        $('#countOrders').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#countOrders').css('border-color', 'lightgrey');
    }
    if ($('#Manager').val() === null) {
        $('#Manager').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Manager').css('border-color', 'lightgrey');
    }

    if ($('#Client').val() === null) {
        $('#Client').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Client').css('border-color', 'lightgrey');
    }

    if ($('#Name').val().trim() === "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }
    
    if ($('#DateShipping').val().trim() === "") {
        $('#DateShipping').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#DateShipping').css('border-color', 'lightgrey');
    }

    if ($('#DateSupply').val().trim() === "") {
        $('#DateSupply').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#DateSupply').css('border-color', 'lightgrey');
    }

    if ($('#Dostavka').val() === null) {
        $('#Dostavka').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Dostavka').css('border-color', 'lightgrey');
    }

    if ($('#Cost').val().trim() === "") {
        $('#Cost').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Cost').css('border-color', 'lightgrey');
    }
    
    if ($('#ProductType').val() === null) {
        $('#ProductType').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ProductType').css('border-color', 'lightgrey');
    }
    
    if ($('#Zapros').val().trim() === "") {
        $('#Zapros').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Zapros').css('border-color', 'lightgrey');
    }

    return isValid;
}

function clearTextBox() {
    $("#btnAdd").attr('disabled', false);
    $('#Dostavka').val("");
    $('#Modul').val("");
    $('#PowerST').val("");
    $('#VN_NN').val("");
    $('#TypeShip').val("");
    $('#criticalDateShip').val("");
    $('#countOrders').val("");
    $('#PlanZakaz').val("");
    $('#DateCreate').val("");
    $('#Manager').val("");
    $('#Client').val("");
    $('#id_PZ_OperatorDogovora').val("");
    $('#id_PZ_FIO').val("");
    $('#Name').val("");
    $('#Description').val("");
    $('#MTR').val("");
    $('#nomenklaturNumber').val("");
    $('#timeContract').val("");
    $('#timeContractDate').val("");
    $('#timeArr').val("");
    $('#timeArrDate').val("");
    $('#DateShipping').val("");
    $('#DateSupply').val("");
    $('#TypeShip').val("");
    $('#Cost').val("");
    $('#costSMR').val("");
    $('#costPNR').val("");
    $('#ProductType').val("");
    $('#OL').val("");
    $('#Zapros').val("");
    $('#numZakupki').val("");
    $('#numLota').val("");
    $('#Gruzopoluchatel').val("");
    $('#PostAdresGruzopoluchatel').val("");
    $('#INNGruzopoluchatel').val("");
    $('#OKPOGruzopoluchatelya').val("");
    $('#KodGruzopoluchatela').val("");
    $('#StantionGruzopoluchatel').val("");
    $('#KodStanciiGruzopoluchatelya').val("");
    $('#OsobieOtmetkiGruzopoluchatelya').val("");
    $('#DescriptionGruzopoluchatel').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function getbyID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/Order/GetOrder/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#StantionGruzopoluchatel').val(result.StantionGruzopoluchatel);
            $('#KodStanciiGruzopoluchatelya').val(result.KodStanciiGruzopoluchatelya);
            $('#OsobieOtmetkiGruzopoluchatelya').val(result.OsobieOtmetkiGruzopoluchatelya);
            $('#PowerST').val(result.PowerST);
            $('#VN_NN').val(result.VN_NN);
            $('#Modul').val(result.Modul);
            $('#TypeShip').val(result.TypeShip);
            $('#criticalDateShip').val(result.criticalDateShip);
            $('#DescriptionGruzopoluchatel').val(result.DescriptionGruzopoluchatel);
            $('#KodGruzopoluchatela').val(result.KodGruzopoluchatela);
            $('#OKPOGruzopoluchatelya').val(result.OKPOGruzopoluchatelya);
            $('#INNGruzopoluchatel').val(result.INNGruzopoluchatel);
            $('#PostAdresGruzopoluchatel').val(result.PostAdresGruzopoluchatel);
            $('#Gruzopoluchatel').val(result.Gruzopoluchatel);
            $('#numLota').val(result.numLota);
            $('#numZakupki').val(result.numZakupki);
            $('#Zapros').val(result.Zapros);
            $('#OL').val(result.OL);
            $('#ProductType').val(result.ProductType);
            $('#costPNR').val(result.costPNR);
            $('#costSMR').val(result.costSMR);
            $('#Cost').val(result.Cost);
            $('#Dostavka').val(result.Dostavka);
            $('#DateSupply').val(result.DateSupply);
            $('#DateShipping').val(result.DateShipping);
            $('#timeArrDate').val(result.timeArrDate);
            $('#timeArr').val(result.timeArr);
            $('#timeContractDate').val(result.timeContractDate);
            $('#timeContract').val(result.timeContract);
            $('#nomenklaturNumber').val(result.nomenklaturNumber);
            $('#MTR').val(result.MTR);
            $('#Description').val(result.Description);
            $('#Name').val(result.Name);
            $('#id_PZ_FIO').val(result.id_PZ_FIO);
            $('#id_PZ_OperatorDogovora').val(result.id_PZ_OperatorDogovora);
            $('#Client').val(result.Client);
            $('#Manager').val(result.Manager);
            $('#DateCreate').val(result.DateCreate);
            $('#PlanZakaz').val(result.PlanZakaz);
            $('#orderModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Update() {
    var res = validateUpdate();
    if (res === false) {
        return false;
    }
    var typeObj = {
        Id: $('#Id').val(),
        StantionGruzopoluchatel: $('#StantionGruzopoluchatel').val(),
        KodStanciiGruzopoluchatelya: $('#KodStanciiGruzopoluchatelya').val(),
        OsobieOtmetkiGruzopoluchatelya: $('#OsobieOtmetkiGruzopoluchatelya').val(),
        PowerST: $('#PowerST').val(),
        VN_NN: $('#VN_NN').val(),
        TypeShip: $('#TypeShip').val(),
        criticalDateShip: $('#criticalDateShip').val(),
        DescriptionGruzopoluchatel: $('#DescriptionGruzopoluchatel').val(),
        KodGruzopoluchatela: $('#KodGruzopoluchatela').val(),
        OKPOGruzopoluchatelya: $('#OKPOGruzopoluchatelya').val(),
        INNGruzopoluchatel: $('#INNGruzopoluchatel').val(),
        PostAdresGruzopoluchatel: $('#PostAdresGruzopoluchatel').val(),
        PlanZakaz: $('#PlanZakaz').val(),
        DateCreate: $('#DateCreate').val(),
        Manager: $('#Manager').val(),
        Client: $('#Client').val(),
        id_PZ_OperatorDogovora: $('#id_PZ_OperatorDogovora').val(),
        id_PZ_FIO: $('#id_PZ_FIO').val(),
        Name: $('#Name').val(),
        Description: $('#Description').val(),
        MTR: $('#MTR').val(),
        nomenklaturNumber: $('#nomenklaturNumber').val(),
        timeContract: $('#timeContract').val(),
        timeContractDate: $('#timeContractDate').val(),
        timeArr: $('#timeArr').val(),
        timeArrDate: $('#timeArrDate').val(),
        DateShipping: $('#DateShipping').val(),
        DateSupply: $('#DateSupply').val(),
        Dostavka: $('#Dostavka').val(),
        Cost: $('#Cost').val(),
        Modul: $('#Modul').val(),
        costSMR: $('#costSMR').val(),
        costPNR: $('#costPNR').val(),
        ProductType: $('#ProductType').val(),
        Zapros: $('#Zapros').val(),
        OL: $('#OL').val(),
        numZakupki: $('#numZakupki').val(),
        numLota: $('#numLota').val(),
        Gruzopoluchatel: $('#Gruzopoluchatel').val()
    };
    $.ajax({
        cache: false,
        url: "/Order/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#orderModal').modal('hide');
            $('#Id').val("");
            $('#StantionGruzopoluchatel').val("");
            $('#KodStanciiGruzopoluchatelya').val("");
            $('#OsobieOtmetkiGruzopoluchatelya').val("");
            $('#PowerST').val("");
            $('#VN_NN').val("");
            $('#TypeShip').val("");
            $('#criticalDateShip').val("");
            $('#DescriptionGruzopoluchatel').val("");
            $('#KodGruzopoluchatela').val("");
            $('#OKPOGruzopoluchatelya').val("");
            $('#INNGruzopoluchatel').val("");
            $('#PostAdresGruzopoluchatel').val("");
            $('#Gruzopoluchatel').val("");
            $('#numLota').val("");
            $('#numZakupki').val("");
            $('#Zapros').val("");
            $('#OL').val("");
            $('#ProductType').val("");
            $('#costPNR').val("");
            $('#costSMR').val("");
            $('#Cost').val("");
            $('#Dostavka').val("");
            $('#DateSupply').val("");
            $('#DateShipping').val("");
            $('#timeArrDate').val("");
            $('#timeArr').val("");
            $('#timeContractDate').val("");
            $('#timeContract').val("");
            $('#nomenklaturNumber').val("");
            $('#MTR').val("");
            $('#Description').val("");
            $('#Name').val("");
            $('#id_PZ_FIO').val("");
            $('#id_PZ_OperatorDogovora').val("");
            $('#Client').val("");
            $('#Manager').val("");
            $('#DateCreate').val("");
            $('#PlanZakaz').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validateUpdate() {
    var isValid = true;
    if ($('#Manager').val() === null) {
        $('#Manager').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Manager').css('border-color', 'lightgrey');
    }

    if ($('#Client').val() === null) {
        $('#Client').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Client').css('border-color', 'lightgrey');
    }

    if ($('#Name').val().trim() === "") {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }

    if ($('#DateShipping').val().trim() === "") {
        $('#DateShipping').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#DateShipping').css('border-color', 'lightgrey');
    }

    if ($('#DateSupply').val().trim() === "") {
        $('#DateSupply').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#DateSupply').css('border-color', 'lightgrey');
    }

    if ($('#Dostavka').val() === null) {
        $('#Dostavka').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Dostavka').css('border-color', 'lightgrey');
    }

    if ($('#Cost').val().trim() === "") {
        $('#Cost').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Cost').css('border-color', 'lightgrey');
    }

    if ($('#ProductType').val() === null) {
        $('#ProductType').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ProductType').css('border-color', 'lightgrey');
    }

    if ($('#Zapros').val().trim() === "") {
        $('#Zapros').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Zapros').css('border-color', 'lightgrey');
    }

    return isValid;
}

function getbyReadID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/Order/GetOrder/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#StantionGruzopoluchatel').val(result.StantionGruzopoluchatel);
            $('#KodStanciiGruzopoluchatelya').val(result.KodStanciiGruzopoluchatelya);
            $('#OsobieOtmetkiGruzopoluchatelya').val(result.OsobieOtmetkiGruzopoluchatelya);
            $('#PowerST').val(result.PowerST);
            $('#Modul').val(result.Modul);
            $('#VN_NN').val(result.VN_NN);
            $('#TypeShip').val(result.TypeShip);
            $('#criticalDateShip').val(result.criticalDateShip);
            $('#DescriptionGruzopoluchatel').val(result.DescriptionGruzopoluchatel);
            $('#KodGruzopoluchatela').val(result.KodGruzopoluchatela);
            $('#OKPOGruzopoluchatelya').val(result.OKPOGruzopoluchatelya);
            $('#INNGruzopoluchatel').val(result.INNGruzopoluchatel);
            $('#PostAdresGruzopoluchatel').val(result.PostAdresGruzopoluchatel);
            $('#Gruzopoluchatel').val(result.Gruzopoluchatel);
            $('#numLota').val(result.numLota);
            $('#numZakupki').val(result.numZakupki);
            $('#Zapros').val(result.Zapros);
            $('#OL').val(result.OL);
            $('#ProductType').val(result.ProductType);
            $('#costPNR').val(result.costPNR);
            $('#costSMR').val(result.costSMR);
            $('#Cost').val(result.Cost);
            $('#Dostavka').val(result.Dostavka);
            $('#DateSupply').val(result.DateSupply);
            $('#DateShipping').val(result.DateShipping);
            $('#timeArrDate').val(result.timeArrDate);
            $('#timeArr').val(result.timeArr);
            $('#timeContractDate').val(result.timeContractDate);
            $('#timeContract').val(result.timeContract);
            $('#nomenklaturNumber').val(result.nomenklaturNumber);
            $('#MTR').val(result.MTR);
            $('#Description').val(result.Description);
            $('#Name').val(result.Name);
            $('#id_PZ_FIO').val(result.id_PZ_FIO);
            $('#id_PZ_OperatorDogovora').val(result.id_PZ_OperatorDogovora);
            $('#Client').val(result.Client);
            $('#Manager').val(result.Manager);
            $('#DateCreate').val(result.DateCreate);
            $('#PlanZakaz').val(result.PlanZakaz);
            $('#orderModal').modal('show');
            $('#btnGetInfGP').hide();
            $('#btnUpdate').hide();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function UpdateOrders() {
    $("#btnUpdateOrders").attr('disabled', true);
    var typeObj = {
        Orders: $('#Orders').val(),
        Manager: $('#mManager').val(),
        Client: $('#mClient').val(),
        id_PZ_OperatorDogovora: $('#mid_PZ_OperatorDogovora').val(),
        id_PZ_FIO: $('#mid_PZ_FIO').val(),
        Name: $('#mName').val(),
        Description: $('#mDescription').val(),
        MTR: $('#mMTR').val(),
        nomenklaturNumber: $('#mnomenklaturNumber').val(),
        timeContract: $('#mtimeContract').val(),
        timeContractDate: $('#mtimeContractDate').val(),
        timeArr: $('#mtimeArr').val(),
        timeArrDate: $('#mtimeArrDate').val(),
        DateShipping: $('#mDateShipping').val(),
        DateSupply: $('#mDateSupply').val(),
        Dostavka: $('#mDostavka').val(),
        Cost: $('#mCost').val(),
        costSMR: $('#mcostSMR').val(),
        costPNR: $('#mcostPNR').val(),
        ProductType: $('#mProductType').val(),
        OL: $('#mOL').val(),
        Zapros: $('#mZapros').val(),
        numZakupki: $('#mnumZakupki').val(),
        numLota: $('#mnumLota').val(),
        Gruzopoluchatel: $('#mGruzopoluchatel').val(),
        PostAdresGruzopoluchatel: $('#mPostAdresGruzopoluchatel').val(),
        INNGruzopoluchatel: $('#mINNGruzopoluchatel').val(),
        OKPOGruzopoluchatelya: $('#mOKPOGruzopoluchatelya').val(),
        KodGruzopoluchatela: $('#mKodGruzopoluchatela').val(),
        StantionGruzopoluchatel: $('#mStantionGruzopoluchatel').val(),
        KodStanciiGruzopoluchatelya: $('#mKodStanciiGruzopoluchatelya').val(),
        OsobieOtmetkiGruzopoluchatelya: $('#mOsobieOtmetkiGruzopoluchatelya').val(),
        PowerST: $('#mPowerST').val(),
        VN_NN: $('#mVN_NN').val(),
        Modul: $('#mModul').val(),
        TypeShip: $('#mTypeShip').val(),
        criticalDateShip: $('#mcriticalDateShip').val(),
        DescriptionGruzopoluchatel: $('#mDescriptionGruzopoluchatel').val()
    };
    $.ajax({
        cache: false,
        url: "/Order/UpdateOrders",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#ordersModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBoxUpdateOrders() {
    $("#btnUpdateOrders").attr('disabled', false);
    $('#Orders').val("");
    $('#Orders').chosen();
    $('#Orders').trigger('chosen:updated');
    $('#mDostavka').val("");
    $('#mModul').val("");
    $('#mPowerST').val("");
    $('#mVN_NN').val("");
    $('#mTypeShip').val("");
    $('#mcriticalDateShip').val("");
    $('#mcountOrders').val("");
    $('#mPlanZakaz').val("");
    $('#mDateCreate').val("");
    $('#mManager').val("");
    $('#mClient').val("");
    $('#mid_PZ_OperatorDogovora').val("");
    $('#mid_PZ_FIO').val("");
    $('#mName').val("");
    $('#mDescription').val("");
    $('#mMTR').val("");
    $('#mnomenklaturNumber').val("");
    $('#mtimeContract').val("");
    $('#mtimeContractDate').val("");
    $('#mtimeArr').val("");
    $('#mtimeArrDate').val("");
    $('#mDateShipping').val("");
    $('#mDateSupply').val("");
    $('#mTypeShip').val("");
    $('#mCost').val("");
    $('#mcostSMR').val("");
    $('#mcostPNR').val("");
    $('#mProductType').val("");
    $('#mOL').val("");
    $('#mZapros').val("");
    $('#mnumZakupki').val("");
    $('#mnumLota').val("");
    $('#mGruzopoluchatel').val("");
    $('#mPostAdresGruzopoluchatel').val("");
    $('#mINNGruzopoluchatel').val("");
    $('#mOKPOGruzopoluchatelya').val("");
    $('#mKodGruzopoluchatela').val("");
    $('#mStantionGruzopoluchatel').val("");
    $('#mKodStanciiGruzopoluchatelya').val("");
    $('#mOsobieOtmetkiGruzopoluchatelya').val("");
    $('#mDescriptionGruzopoluchatel').val("");
    $('#btnUpdateOrders').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}

function getbyKOID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/Order/GetKOOrder/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#kPlanZakaz').val(result.PlanZakaz);
            $('#nameTU').val(result.nameTU);
            $('#orderKOModal').modal('show');
            $('#btnUpdateKO').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function UpdateKO() {
    var res = validateUpdateKO();
    if (res === false) {
        return false;
    }
    $("#btnUpdateKO").attr('disabled', true);
    var typeObj = {
        Id: $('#Id').val(),
        PlanZakaz: $('#kPlanZakaz').val(),
        nameTU: $('#nameTU').val()
    };
    $.ajax({
        cache: false,
        url: "/Order/UpdateKO",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#orderKOModal').modal('hide');
            $("#btnUpdateKO").attr('disabled', false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validateUpdateKO() {
    var isValid = true;
    if ($('#nameTU').val() === null) {
        $('#nameTU').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#nameTU').css('border-color', 'lightgrey');
    }
    return isValid;
}

function clearTextBoxTableOrders() {
    $("#btnTableOrders").attr('disabled', false);
    $('#tOrders').val("");
    $('#tOrders').chosen();
    $('#tOrders').trigger('chosen:updated');
    $('#btnTableOrders').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function TableOrders() {
    $("#btnTableOrders").attr('disabled', true);
    var typeObj = {
        Id: $('#tOrders').val()
    };
    $.ajax({
        cache: false,
        url: "/Order/TableOrders",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#tablesModal').modal('hide');
            $("#btnTableOrders").attr('disabled', false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetGP(Gruzopoluchatel) {
    Gruzopoluchatel = Gruzopoluchatel.split(' ').join('');
    Gruzopoluchatel = Gruzopoluchatel.split('"').join('');
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Order/GetGP/" + Gruzopoluchatel,
        typr: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        cache: false,
        success: function (result) {
            $('#StantionGruzopoluchatel').val(result.StantionGruzopoluchatel);
            $('#KodStanciiGruzopoluchatelya').val(result.KodStanciiGruzopoluchatelya);
            $('#OsobieOtmetkiGruzopoluchatelya').val(result.OsobieOtmetkiGruzopoluchatelya);
            $('#DescriptionGruzopoluchatel').val(result.DescriptionGruzopoluchatel);
            $('#KodGruzopoluchatela').val(result.KodGruzopoluchatela);
            $('#OKPOGruzopoluchatelya').val(result.OKPOGruzopoluchatelya);
            $('#INNGruzopoluchatel').val(result.INNGruzopoluchatel);
            $('#PostAdresGruzopoluchatel').val(result.PostAdresGruzopoluchatel);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function GetGPOrders(Gruzopoluchatel) {
    Gruzopoluchatel = Gruzopoluchatel.split(' ').join('');
    Gruzopoluchatel = Gruzopoluchatel.split('"').join('');
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Order/GetGP/" + Gruzopoluchatel,
        typr: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        cache: false,
        success: function (result) {
            $('#mStantionGruzopoluchatel').val(result.StantionGruzopoluchatel);
            $('#mKodStanciiGruzopoluchatelya').val(result.KodStanciiGruzopoluchatelya);
            $('#mOsobieOtmetkiGruzopoluchatelya').val(result.OsobieOtmetkiGruzopoluchatelya);
            $('#mDescriptionGruzopoluchatel').val(result.DescriptionGruzopoluchatel);
            $('#mKodGruzopoluchatela').val(result.KodGruzopoluchatela);
            $('#mOKPOGruzopoluchatelya').val(result.OKPOGruzopoluchatelya);
            $('#mINNGruzopoluchatel').val(result.INNGruzopoluchatel);
            $('#mPostAdresGruzopoluchatel').val(result.PostAdresGruzopoluchatel);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}