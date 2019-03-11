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

function OrdersListLY(yearCreateOrder) {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/Order/OrdersListLY?yearCreateOrder=" + yearCreateOrder,
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

function OrdersListALL() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/Order/OrdersListALL",
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

function Add() {
    var res = validate();
    if (res === false) {
        return false;
    }
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
        TypeShip: $('#TypeShip').val(),
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
        DescriptionGruzopoluchatel: $('#DescriptionGruzopoluchatel').val()
    };
    $.ajax({
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

    if ($('#Manager').val().trim() === "") {
        $('#Manager').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Manager').css('border-color', 'lightgrey');
    }

    if ($('#Client').val().trim() === "") {
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

    if ($('#TypeShip').val().trim() === "") {
        $('#TypeShip').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#TypeShip').css('border-color', 'lightgrey');
    }

    if ($('#Cost').val().trim() === "") {
        $('#Cost').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Cost').css('border-color', 'lightgrey');
    }
    
    if ($('#ProductType').val().trim() === "") {
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