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
            { "title": "Ред", "data": "Id", "autowidth": true },
            { "title": "См", "data": "IdRead", "autowidth": true },
            { "title": "Тип продукции", "data": "ProductType", "autowidth": true },
            { "title": "Дата открытия", "data": "DateCreate", "autowidth": true },
            //{ "title": "Статус", "data": "StatusOrder", "autowidth": true },
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
            //{ "title": "С/ф №", "data": "SF", "autowidth": true },
            { "title": "Дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true }
            //{ "title": "Дата доставки", "data": "dateDostavki", "autowidth": true },
            //{ "title": "Дата приемки", "data": "datePriemki", "autowidth": true },
            //{ "title": "Дата оплаты", "data": "dateOplat", "autowidth": true }
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
            { "title": "Ред", "data": "Id", "autowidth": true },
            { "title": "См", "data": "IdRead", "autowidth": true },
            { "title": "Тип продукции", "data": "ProductType", "autowidth": true },
            { "title": "Дата открытия", "data": "DateCreate", "autowidth": true },
            //{ "title": "Статус", "data": "StatusOrder", "autowidth": true },
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
            //{ "title": "С/ф №", "data": "SF", "autowidth": true },
            { "title": "Дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true }
            //{ "title": "Дата доставки", "data": "dateDostavki", "autowidth": true },
            //{ "title": "Дата приемки", "data": "datePriemki", "autowidth": true },
            //{ "title": "Дата оплаты", "data": "dateOplat", "autowidth": true }
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
            { "title": "Ред", "data": "Id", "autowidth": true },
            { "title": "См", "data": "IdRead", "autowidth": true },
            { "title": "Тип продукции", "data": "ProductType", "autowidth": true },
            { "title": "Дата открытия", "data": "DateCreate", "autowidth": true },
            //{ "title": "Статус", "data": "StatusOrder", "autowidth": true },
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
            //{ "title": "С/ф №", "data": "SF", "autowidth": true },
            { "title": "Дата отгрузки", "data": "dataOtgruzkiBP", "autowidth": true }
            //{ "title": "Дата доставки", "data": "dateDostavki", "autowidth": true },
            //{ "title": "Дата приемки", "data": "datePriemki", "autowidth": true },
            //{ "title": "Дата оплаты", "data": "dateOplat", "autowidth": true }
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
        VN_NN: $('#VN_NN').val(),
        TypeShip: $('#TypeShip').val(),
        criticalDateShip: $('#criticalDateShip').val(),
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
        VN_NN: $('#VN_NN').val(),
        TypeShip: $('#TypeShip').val(),
        criticalDateShip: $('#criticalDateShip').val(),
        DescriptionGruzopoluchatel: $('#DescriptionGruzopoluchatel').val()
    };
    $.ajax({
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
    $('#btnUpdateOrders').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}