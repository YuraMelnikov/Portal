// 01 - OS
// 02 - MKO
// 03 - Admin
// 04 - userKO
// 05 - users
// 06 - users manufacturing

$(document).ready(function () {
    $('#pageId').hide();
    $('#btnAddOrder').hide();
    $('#btnReOrder').hide();
    $('#btnAddSandwichPanel').hide();
    $('#dToWork').hide();
    $('#dToManuf').hide();
    $('#dToCompl').hide();
    $('#dReOrder').hide();
    $('#dApproveTable').hide();
    $('#dCorrectionTable').hide();
    $('#dCustomerTable').hide();
    $('#dGetDateComplitedTable').hide();
    $('#dComplitedTable').hide();
    startMenu();
    if (userGroupId === 1) {
        loadData(2);
    }
});

function loadData(listId) {
    document.getElementById('pageId').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        loadReport();
    }
    else if (listId === 2 || listId === "2") {
        loadOS();
    }
    else if (listId === 3 || listId === "3") {
        loadReportPanel();
    }
    else if (listId === 4 || listId === "4") {
        loadReportPanel();
        loadpanel();
    }
    else {
        loadReport();
    }
}

var objSmallReport = [
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Изг. дн.", "data": "day", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Дата создания", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Дата размещения", "data": "workDateTime", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Дата исполнения", "data": "manufDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Дата поступления", "data": "finDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "№ заявки", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Статус", "data": "status", "autowidth": true, "bSortable": true },
    { "title": "Папка заказа", "data": "folder", "autowidth": true }
];

var objFullReport = [
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Подрядчик", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Изг. дн.", "data": "day", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processZero },
    { "title": "Дата создания", "data": "dateCreate", "autowidth": true, "bSortable": true },
    { "title": "Дата размещения", "data": "workDateTime", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Нач. цена, б/НДС (BYN)", "data": "workCost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Дата исполнения", "data": "manufDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Подтв. цена, б/НДС (BYN)", "data": "manufCost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "Дата поступления", "data": "finDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Стоимость, б/НДС (BYN)", "data": "finCost", "autowidth": true, "bSortable": true, "className": 'text-right', render: $.fn.dataTable.render.number(',', '.', 2, '') },
    { "title": "№ заявки", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Статус", "data": "status", "autowidth": true, "bSortable": true },
    { "title": "Папка заказа", "data": "folder", "autowidth": true }
];

var objWork = [
    { "title": "Ред", "data": "editLink", "autowidth": true, "bSortable": true },
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Дата размещения", "data": "workDateTime", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Дата исполнения", "data": "manufDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "№ заявки", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Папка заказа", "data": "folder", "autowidth": true, "bSortable": true }
];

var objWorkManuf = [
    { "title": "Ред", "data": "editLink", "autowidth": true, "bSortable": true },
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "name", "autowidth": true, "bSortable": true },
    { "title": "Срок", "data": "workDateTime", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Дата исполнения", "data": "manufDate", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "№ заявки", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Папка заказа", "data": "folder", "autowidth": true, "bSortable": true }
];

function startMenu() {
    if (userGroupId === 4 || userGroupId === 2) {
        $('#btnAddOrder').show();
        $('#btnReOrder').show();
        $('#btnAddSandwichPanel').show();
    }
    $("#reportTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ReportTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[3, "desc"]],
        "processing": true,
        "columns": objSmallReport,
        "rowCallback": function (row, data, index) {
            if (data.status === "Не отправлен") {
                $('td', row).css('background-color', '#ebaca2');
            }
            else if (data.status === "Ожидание сроков") {
                $('td', row).css('background-color', '#ffc87c');
            }
            else if (data.status === "Производится") {
                $('td', row).css('background-color', '#a6e9d7');
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
    $("#reOrderTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToReOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objWork,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#toWorkTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToWork",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objWork,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#toManufTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToManuf",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objWorkManuf,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#toCloseTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToClose",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objWork,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#approveTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnApproveTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#correctionTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnCorrectionTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#customerTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnCustomerTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#getDateComplitedTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnGetDateComplited",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#complitedTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnComplitedTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

var objSandwichPanel = [
    { "title": "Ред.", "data": "edit", "autowidth": true, "bSortable": false },
    { "title": "№ заявки", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "№ заказа/ов", "data": "pz", "autowidth": true, "bSortable": true },
    { "title": "Дата размещения", "data": "dateCreate", "autowidth": true, "bSortable": false },
    { "title": "Дата согласования", "data": "dateApprove", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Дата отправки подрядчику", "data": "dateToCustomer", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Ожидаемый срок поставки", "data": "datePlanComplited", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Фактический срок поставки", "data": "dateComplited", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Подрядчик", "data": "customerName", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull }
];

var objSandwichPanelReport = [
    { "title": "№ заявки", "data": "order", "autowidth": true, "bSortable": true },
    { "title": "№ заказа/ов", "data": "pz", "autowidth": true, "bSortable": true },
    { "title": "Дата размещения", "data": "dateCreate", "autowidth": true, "bSortable": false },
    { "title": "Дата согласования", "data": "dateApprove", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Дата отправки подрядчику", "data": "dateToCustomer", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Ожидаемый срок поставки", "data": "datePlanComplited", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Фактический срок поставки", "data": "dateComplited", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull },
    { "title": "Статус", "data": "state", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Подрядчик", "data": "customerName", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull },
    { "title": "Папка", "data": "folder", "autowidth": true, "bSortable": false, "defaultContent": "", "render": processNull }
];

function sandwichPanelReport() {
    var table = $('#reportTable').DataTable();
    table.destroy();
    $('#reportTable').empty();
    $("#reportTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/SandwichPanelReport",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanelReport,
        "rowCallback": function (row, data, index) {
            if (data.state === "На проверке") {
                $('td', row).css('background-color', '#f28f43');
            }
            else if (data.state === "На исправлении") {
                $('td', row).css('background-color', '#AA4643');
                $('td', row).css('color', '#FFFFFF');
            }
            else if (data.state === "Ожидание отправки") {
                $('td', row).css('background-color', '#f7a35c');
            }
            else if (data.state === "Ожидание сроков") {
                $('td', row).css('background-color', '#a6c96a');
            }
            else if (data.state === "В производстве") {
                $('td', row).css('background-color', '#89A54E');
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

function onApproveTable() {
    var table = $('#approveTable').DataTable();
    table.destroy();
    $('#approveTable').empty();
    $("#approveTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnApproveTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "rowCallback": function (row, data, index) {
            if (data.state === "На проверке") {
                $('td', row).css('background-color', '#f28f43');
            }
            else if (data.state === "На исправлении") {
                $('td', row).css('background-color', '#AA4643');
                $('td', row).css('color', '#FFFFFF');
            }
            else if (data.state === "Ожидание отправки") {
                $('td', row).css('background-color', '#f7a35c');
            }
            else if (data.state === "Ожидание сроков") {
                $('td', row).css('background-color', '#a6c96a');
            }
            else if (data.state === "В производстве") {
                $('td', row).css('background-color', '#89A54E');
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

function onCorrectionTable() {
    var table = $('#correctionTable').DataTable();
    table.destroy();
    $('#correctionTable').empty();
    $("#correctionTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnCorrectionTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "rowCallback": function (row, data, index) {
            if (data.state === "На проверке") {
                $('td', row).css('background-color', '#f28f43');
            }
            else if (data.state === "На исправлении") {
                $('td', row).css('background-color', '#AA4643');
                $('td', row).css('color', '#FFFFFF');
            }
            else if (data.state === "Ожидание отправки") {
                $('td', row).css('background-color', '#f7a35c');
            }
            else if (data.state === "Ожидание сроков") {
                $('td', row).css('background-color', '#a6c96a');
            }
            else if (data.state === "В производстве") {
                $('td', row).css('background-color', '#89A54E');
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

function onCustomerTable() {
    var table = $('#customerTable').DataTable();
    table.destroy();
    $('#customerTable').empty();
    $("#customerTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnCustomerTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "rowCallback": function (row, data, index) {
            if (data.state === "На проверке") {
                $('td', row).css('background-color', '#f28f43');
            }
            else if (data.state === "На исправлении") {
                $('td', row).css('background-color', '#AA4643');
                $('td', row).css('color', '#FFFFFF');
            }
            else if (data.state === "Ожидание отправки") {
                $('td', row).css('background-color', '#f7a35c');
            }
            else if (data.state === "Ожидание сроков") {
                $('td', row).css('background-color', '#a6c96a');
            }
            else if (data.state === "В производстве") {
                $('td', row).css('background-color', '#89A54E');
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

function onGetDateComplitedTable() {
    var table = $('#getDateComplitedTable').DataTable();
    table.destroy();
    $('#getDateComplitedTable').empty();
    $("#getDateComplitedTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnGetDateComplited",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "rowCallback": function (row, data, index) {
            if (data.state === "На проверке") {
                $('td', row).css('background-color', '#f28f43');
            }
            else if (data.state === "На исправлении") {
                $('td', row).css('background-color', '#AA4643');
                $('td', row).css('color', '#FFFFFF');
            }
            else if (data.state === "Ожидание отправки") {
                $('td', row).css('background-color', '#f7a35c');
            }
            else if (data.state === "Ожидание сроков") {
                $('td', row).css('background-color', '#a6c96a');
            }
            else if (data.state === "В производстве") {
                $('td', row).css('background-color', '#89A54E');
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

function onComplitedTable() {
    var table = $('#complitedTable').DataTable();
    table.destroy();
    $('#complitedTable').empty();
    $("#complitedTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/OnComplitedTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objSandwichPanel,
        "rowCallback": function (row, data, index) {
            if (data.state === "На проверке") {
                $('td', row).css('background-color', '#f28f43');
            }
            else if (data.state === "На исправлении") {
                $('td', row).css('background-color', '#AA4643');
                $('td', row).css('color', '#FFFFFF');
            }
            else if (data.state === "Ожидание сроков") {
                $('td', row).css('background-color', '#a6c96a');
            }
            else if (data.state === "Ожидание отправки") {
                $('td', row).css('background-color', '#f7a35c');
            }
            else if (data.state === "В производстве") {
                $('td', row).css('background-color', '#89A54E');
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

function smallReport() {
    var table = $('#reportTable').DataTable();
    table.destroy();
    $('#reportTable').empty();
    $("#reportTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ReportTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[3, "desc"]],
        "processing": true,
        "columns": objSmallReport,
        "rowCallback": function (row, data, index) {
            if (data.status === "Не отправлен") {
                $('td', row).css('background-color', '#ebaca2');
            }
            else if (data.status === "Ожидание сроков") {
                $('td', row).css('background-color', '#ffc87c');
            }
            else if (data.status === "Производится") {
                $('td', row).css('background-color', '#a6e9d7');
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

function fullReport() {
    var table = $('#reportTable').DataTable();
    table.destroy();
    $('#reportTable').empty();
    $("#reportTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ReportTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[3, "desc"]],
        "processing": true,
        "columns": objFullReport,
        "rowCallback": function (row, data, index) {
            if (data.status === "Не отправлен") {
                $('td', row).css('background-color', '#ebaca2');
            }
            else if (data.status === "Ожидание сроков") {
                $('td', row).css('background-color', '#ffc87c');
            }
            else if (data.status === "Производится") {
                $('td', row).css('background-color', '#a6e9d7');
            }
            if (data.finCost - data.manufCost > 100) {
                $('td', row).eq(9).addClass('highlightColor');
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

function toWork() {
    var table = $('#toWorkTable').DataTable();
    table.destroy();
    $('#toWorkTable').empty();
    $("#toWorkTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToWork",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objWork,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function toManuf() {
    var table = $('#toManufTable').DataTable();
    table.destroy();
    $('#toManufTable').empty();
    $("#toManufTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToManuf",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objWorkManuf,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function toClose() {
    var table = $('#toCloseTable').DataTable();
    table.destroy();
    $('#toCloseTable').empty();
    $("#toCloseTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToClose",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[3, "asc"]],
        "processing": true,
        "columns": objWork,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function toReOrder() {
    var table = $('#reOrderTable').DataTable();
    table.destroy();
    $('#reOrderTable').empty();
    $("#reOrderTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/CMOArea/ToReOrder",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "desc"]],
        "processing": true,
        "columns": objWork,
        "paging": false,
        "info": false,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function desckTopOS() {
    fullReport();
    toWork();
    toManuf();
    toClose();
    toReOrder();
}

function loadReport() {
    if (userGroupId < 4) {
        $('#dToWork').hide();
        $('#dToManuf').hide();
        $('#dToCompl').hide();
        $('#dReOrder').hide();
        $('#dApproveTable').hide();
        $('#dCorrectionTable').hide();
        $('#dCustomerTable').hide();
        $('#dGetDateComplitedTable').hide();
        $('#dComplitedTable').hide();
        fullReport();
    }
    else {
        $('#dToWork').hide();
        $('#dToManuf').hide();
        $('#dToCompl').hide();
        $('#dReOrder').hide();
        $('#dApproveTable').hide();
        $('#dCorrectionTable').hide();
        $('#dCustomerTable').hide();
        $('#dGetDateComplitedTable').hide();
        $('#dComplitedTable').hide();
        smallReport();
    }
}

function loadOS() {
    if (userGroupId === 1 || userGroupId === '1') {
        $('#dToWork').show();
        $('#dToManuf').show();
        $('#dToCompl').show();
        $('#dReOrder').show();
        $('#dApproveTable').hide();
        $('#dCorrectionTable').hide();
        $('#dCustomerTable').hide();
        $('#dGetDateComplitedTable').hide();
        $('#dComplitedTable').hide();
        desckTopOS();
    }
    else {
        $('#dToWork').hide();
        $('#dToManuf').hide();
        $('#dToCompl').hide();
        $('#dReOrder').hide();
        $('#dApproveTable').hide();
        $('#dCorrectionTable').hide();
        $('#dCustomerTable').hide();
        $('#dGetDateComplitedTable').hide();
        $('#dComplitedTable').hide();
        smallReport();
    }
}

function loadReportPanel() {
    $('#dToWork').hide();
    $('#dToManuf').hide();
    $('#dToCompl').hide();
    $('#dReOrder').hide();
    $('#dApproveTable').hide();
    $('#dCorrectionTable').hide();
    $('#dCustomerTable').hide();
    $('#dGetDateComplitedTable').hide();
    $('#dComplitedTable').hide();
    sandwichPanelReport();
}

function loadpanel() {
    if (userGroupId === 1 || userGroupId === "1") {
        $('#dToWork').hide();
        $('#dToManuf').hide();
        $('#dToCompl').hide();
        $('#dReOrder').hide();
        $('#dApproveTable').hide();
        $('#dCorrectionTable').hide();
        $('#dCustomerTable').show();
        $('#dGetDateComplitedTable').show();
        $('#dComplitedTable').show();
        onCustomerTable();
        onGetDateComplitedTable();
        onComplitedTable();
    }
    else if (userGroupId === 6 || userGroupId === "6") {
        $('#dToWork').hide();
        $('#dToManuf').hide();
        $('#dToCompl').hide();
        $('#dReOrder').hide();
        $('#dApproveTable').show();
        $('#dCorrectionTable').hide();
        $('#dCustomerTable').hide();
        $('#dGetDateComplitedTable').hide();
        $('#dComplitedTable').hide();
        onApproveTable();
    }
    else if (userGroupId === 4 || userGroupId === 2 || userGroupId === "4" || userGroupId === "2") {
        $('#dToWork').hide();
        $('#dToManuf').hide();
        $('#dToCompl').hide();
        $('#dReOrder').hide();
        $('#dApproveTable').hide();
        $('#dCorrectionTable').show();
        $('#dCustomerTable').hide();
        $('#dGetDateComplitedTable').hide();
        $('#dComplitedTable').hide();
        onCorrectionTable();
    }
    else {
        $('#dToWork').hide();
        $('#dToManuf').hide();
        $('#dToCompl').hide();
        $('#dReOrder').hide();
        $('#dApproveTable').hide();
        $('#dCorrectionTable').hide();
        $('#dCustomerTable').hide();
        $('#dGetDateComplitedTable').hide();
        $('#dComplitedTable').hide();
        sandwichPanelReport();
    }
}

function clearTextBox() {
    $('#id_PlanZakaz').prop('disabled', false);
    $('#id_CMO_TypeProduct').prop('disabled', false);
    $('#id_CMO_Company').prop('disabled', false);
    $('#workIn').prop('disabled', true);
    $('#workDateTime').prop('disabled', true);
    $('#workCost').prop('disabled', true);
    $('#workComplitet').prop('disabled', true);
    $('#manufIn').prop('disabled', true);
    $('#manufDate').prop('disabled', true);
    $('#manufCost').prop('disabled', true);
    $('#manufComplited').prop('disabled', true);
    $('#finIn').prop('disabled', true);
    $('#finDate').prop('disabled', true);
    $('#finCost').prop('disabled', true);
    $('#finComplited').prop('disabled', true);
    $('#id_PlanZakaz').val("");
    $('#id_CMO_TypeProduct').val("");
    $('#id_CMO_Company').val("");
    $('#workIn').prop('checked', false);
    $('#workDateTime').val("");
    $('#workCost').val("");
    $('#workComplitet').prop('checked', false);
    $('#manufIn').prop('checked', false);
    $('#manufDate').val("");
    $('#manufCost').val("");
    $('#manufComplited').prop('checked', false);
    $('#finIn').prop('checked', false);
    $('#finDate').val("");
    $('#finCost').val("");
    $('#finComplited').prop('checked', false);
    $('#oid_PlanZakaz').val("");
    $('#oid_PlanZakaz').chosen();
    $('#oid_PlanZakaz').trigger('chosen:updated');
    $('#oid_CMO_TypeProduct').val("");
    $('#oid_CMO_TypeProduct').chosen();
    $('#oid_CMO_TypeProduct').trigger('chosen:updated');
    $('#Ofile1').val("");
    $('#roid_PlanZakaz').val("");
    $('#roid_PlanZakaz').chosen();
    $('#roid_PlanZakaz').trigger('chosen:updated');
    $('#roid_CMO_TypeProduct').val("");
    $('#roid_CMO_TypeProduct').chosen();
    $('#roid_CMO_TypeProduct').trigger('chosen:updated');
    $('#spid_PlanZakaz').val("");
    $('#spid_PlanZakaz').chosen();
    $('#spid_PlanZakaz').trigger('chosen:updated');
    $('#rofile1').val("");
    $('#spfile1').val("");
    $('#workDateTime').css('border-color', 'lightgrey');
    $('#workCost').css('border-color', 'lightgrey');
    $('#manufDate').css('border-color', 'lightgrey');
    $('#manufCost').css('border-color', 'lightgrey');
    $('#finDate').css('border-color', 'lightgrey');
    $('#finCost').css('border-color', 'lightgrey');
    $('#datetimePlanComplited').css('border-color', 'lightgrey');
    $('#datetimeComplited').css('border-color', 'lightgrey');
    $('#numberOrder').css('border-color', 'lightgrey');
    $('#spdateTimeCreate').val("");
    $('#spid_AspNetUsers_Create').val("");
    $('#datetimeToCorrection').val("");
    $('#datetimeUploadNewVersion').val("");
    $('#datetimeToCustomer').val("");
    $('#datetimePlanComplited').val("");
    $('#datetimeComplited').val("");
    $('#numberOrder').val("");
    $('#onApprove').prop('checked', false);
    $('#onCorrection').prop('checked', false);
    $('#onCustomer').prop('checked', false);
    $('#onGetDateComplited').prop('checked', false);
    $('#onComplited').prop('checked', false);
}

function clearTextBoxUpdate() {
    $('#id_PlanZakaz').prop('disabled', true);
    $('#id_CMO_TypeProduct').prop('disabled', true);
    $('#id_CMO_Company').prop('disabled', false);
    $('#workIn').prop('disabled', true);
    $('#workDateTime').prop('disabled', false);
    $('#workCost').prop('disabled', false);
    $('#workComplitet').prop('disabled', true);
    $('#manufIn').prop('disabled', true);
    $('#manufDate').prop('disabled', false);
    $('#manufCost').prop('disabled', false);
    $('#manufComplited').prop('disabled', true);
    $('#finIn').prop('disabled', true);
    $('#finDate').prop('disabled', false);
    $('#finCost').prop('disabled', false);
    $('#finComplited').prop('disabled', true);
    $('#id_PlanZakaz').val("");
    $('#id_CMO_TypeProduct').val("");
    $('#id_CMO_Company').val("");
    $('#workIn').prop('checked', false);
    $('#workDateTime').val("");
    $('#workCost').val("");
    $('#workComplitet').prop('checked', false);
    $('#manufIn').prop('checked', false);
    $('#manufDate').val("");
    $('#manufCost').val("");
    $('#manufComplited').prop('checked', false);
    $('#finIn').prop('checked', false);
    $('#finDate').val("");
    $('#finCost').val("");
    $('#finComplited').prop('checked', false);
    $('#id_CMO_TypeProduct').val("");
    $('#id_CMO_TypeProduct').chosen();
    $('#id_CMO_TypeProduct').trigger('chosen:updated');
    $('#id_PlanZakaz').val("");
    $('#id_PlanZakaz').chosen();
    $('#id_PlanZakaz').trigger('chosen:updated');
    $('#spid_PlanZakaz').val("");
    $('#spid_PlanZakaz').chosen();
    $('#spid_PlanZakaz').trigger('chosen:updated');
    $('#workDateTime').css('border-color', 'lightgrey');
    $('#workCost').css('border-color', 'lightgrey');
    $('#manufDate').css('border-color', 'lightgrey');
    $('#manufCost').css('border-color', 'lightgrey');
    $('#finDate').css('border-color', 'lightgrey');
    $('#finCost').css('border-color', 'lightgrey');
    $('#datetimePlanComplited').css('border-color', 'lightgrey');
    $('#datetimeComplited').css('border-color', 'lightgrey');
    $('#numberOrder').css('border-color', 'lightgrey');
    $('#spdateTimeCreate').val("");
    $('#spid_AspNetUsers_Create').val("");
    $('#datetimeToCorrection').val("");
    $('#datetimeUploadNewVersion').val("");
    $('#datetimeToCustomer').val("");
    $('#datetimePlanComplited').val("");
    $('#datetimeComplited').val("");
    $('#numberOrder').val("");
    $('#onApprove').prop('checked', false);
    $('#onCorrection').prop('checked', false);
    $('#onCustomer').prop('checked', false);
    $('#onGetDateComplited').prop('checked', false);
    $('#onComplited').prop('checked', false);
}

function get(id) {
    clearTextBoxUpdate();
    $.ajax({
        cache: false,
        url: "/CMOArea/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $("#id_PlanZakaz").val(result.id_PlanZakaz).trigger("chosen:updated");
            $("#id_CMO_TypeProduct").val(result.id_CMO_TypeProduct).trigger("chosen:updated");
            $('#id_CMO_Company').val(result.id_CMO_Company);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#id_AspNetUsers_Create').val(result.id_AspNetUsers_Create);
            $('#workIn').prop('checked', result.workIn);
            $('#workDateTime').val(result.workDateTime);
            $('#workCost').val(result.workCost);
            $('#workComplitet').prop('checked', result.workComplitet);
            $('#manufIn').prop('checked', result.manufIn);
            $('#manufDate').val(result.manufDate);
            $('#manufCost').val(result.manufCost);
            $('#manufComplited').prop('checked', result.manufComplited);
            $('#finIn').prop('checked', result.finIn);
            $('#id').val(result.id);
            $('#finDate').val(result.finDate);
            $('#finCost').val(result.finCost);
            $('#finComplited').prop('checked', result.finComplited);
            $('#osModal').modal('show');
            $('#btnUpdate').show();
            var countDA = 0;
            var tmp = $('#workIn').is(":checked");
            if ($('#workIn').is(":checked") === false)
                countDA = 1;
            else if ($('#manufIn').is(":checked") === false)
                countDA = 2;
            else if ($('#finIn').is(":checked") === false)
                countDA = 3;
            deactivatedModalOS(countDA);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });
    return false;
}

function getRe(id) {
    clearTextBoxUpdate();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/CMOArea/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $("#Reid_PlanZakaz").val(result.id_PlanZakaz).trigger("chosen:updated");
            $('#Reid').val(result.id);
            $('#RedateTimeCreate').val(result.dateTimeCreate);
            $('#Reid_AspNetUsers_Create').val(result.id_AspNetUsers_Create);
            $('#reOrderModalClose').modal('show');
            $('#btnUpdateRe').show();
            var countDA = 0;
            var tmp = $('#workIn').is(":checked");
            if ($('#workIn').is(":checked") === false)
                countDA = 1;
            else if ($('#manufIn').is(":checked") === false)
                countDA = 2;
            else if ($('#finIn').is(":checked") === false)
                countDA = 3;
            deactivatedModalOS(countDA);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });
    return false;
}

function deactivatedModalOS(countDA) {
    if (countDA === 1) {
        $('#workIn').prop('disabled', true);
        $('#workDateTime').prop('disabled', false);
        $('#workCost').prop('disabled', false);
        $('#workComplitet').prop('disabled', true);
        $('#manufIn').prop('disabled', true);
        $('#manufDate').prop('disabled', true);
        $('#manufCost').prop('disabled', true);
        $('#manufComplited').prop('disabled', true);
        $('#finIn').prop('disabled', true);
        $('#finDate').prop('disabled', true);
        $('#finCost').prop('disabled', true);
        $('#finComplited').prop('disabled', true);
    }
    else if (countDA === 2) {
        $('#workIn').prop('disabled', true);
        $('#workDateTime').prop('disabled', true);
        $('#workCost').prop('disabled', true);
        $('#workComplitet').prop('disabled', true);
        $('#manufIn').prop('disabled', true);
        $('#manufDate').prop('disabled', false);
        $('#manufCost').prop('disabled', false);
        $('#manufComplited').prop('disabled', true);
        $('#finIn').prop('disabled', true);
        $('#finDate').prop('disabled', true);
        $('#finCost').prop('disabled', true);
        $('#finComplited').prop('disabled', true);
    }
    else if (countDA === 3) {
        $('#workIn').prop('disabled', true);
        $('#workDateTime').prop('disabled', true);
        $('#workCost').prop('disabled', true);
        $('#workComplitet').prop('disabled', true);
        $('#manufIn').prop('disabled', true); 
        $('#manufCost').prop('disabled', true);
        $('#manufComplited').prop('disabled', true);
        $('#finIn').prop('disabled', true);
        $('#finDate').prop('disabled', false);
        $('#finCost').prop('disabled', false);
        $('#finComplited').prop('disabled', true);
    }
    else {
        $('#workIn').prop('disabled', true);
        $('#workDateTime').prop('disabled', true);
        $('#workCost').prop('disabled', true);
        $('#workComplitet').prop('disabled', true);
        $('#manufIn').prop('disabled', true);
        $('#manufDate').prop('disabled', true);
        $('#manufCost').prop('disabled', true);
        $('#manufComplited').prop('disabled', true);
        $('#finIn').prop('disabled', true);
        $('#finDate').prop('disabled', true);
        $('#finCost').prop('disabled', true);
        $('#finComplited').prop('disabled', true);
    }
}

function update() {
    var res = validateUpdate();
    if (res === false) {
        return false;
    }
    var typeObj = {
        id: $('#id').val(),
        id_CMO_Company: $('#id_CMO_Company').val(),
        workIn: $('#workIn').is(":checked"),
        workDateTime: $('#workDateTime').val(),
        workCost: $('#workCost').val().replace('.', ','),
        workComplitet: $('#workComplitet').is(":checked"),
        manufIn: $('#manufIn').is(":checked"),
        manufDate: $('#manufDate').val(),
        manufCost: $('#manufCost').val().replace('.', ','),
        manufComplited: $('#manufComplited').is(":checked"),
        finIn: $('#finIn').is(":checked"),
        finDate: $('#finDate').val(),
        finCost: $('#finCost').val().replace('.', ','),
        finComplited: $('#finComplited').is(":checked")
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadOS();
            $('#osModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateReOrder() {
    var id = $('#Reid').val();
    $.ajax({
        cache: false,
        url: "/CMOArea/UpdateReOrder/" + id,
        typr: "post",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            loadOS();
            $('#reOrderModalClose').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validateUpdate() {
    isValid = true;
    if ($('#workIn').is(":checked") === false) {
        if ($('#workDateTime').val().length < 5) {
            $('#workDateTime').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#workDateTime').css('border-color', 'lightgrey');
        }
        if ($('#workCost').val() < 1) {
            $('#workCost').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#workCost').css('border-color', 'lightgrey');
        }

        var tmp = $('#id_CMO_Company').val();
        if ($('#id_CMO_Company').val() === null) {
            $('#id_CMO_Company').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#id_CMO_Company').css('border-color', 'lightgrey');
        }
    }
    else if ($('#manufIn').is(":checked") === false) {
        if ($('#manufDate').val().length < 5) {
            $('#manufDate').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#manufDate').css('border-color', 'lightgrey');
        }
        if ($('#manufCost').val() < 1) {
            $('#manufCost').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#manufCost').css('border-color', 'lightgrey');
        }
        if ($('#id_CMO_Company').val().length === 0) {
            $('#id_CMO_Company').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#id_CMO_Company').css('border-color', 'lightgrey');
        }
    }
    else if ($('#finIn').is(":checked") === false) {
        if ($('#id_CMO_Company').val().length === 0) {
            $('#id_CMO_Company').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#id_CMO_Company').css('border-color', 'lightgrey');
        }
    }
    return isValid;
}

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}

function processZero(data) {
    if (data === 0) {
        return '';
    } else {
        return data;
    }
}

function postPanelToKO() {
    var typeObj = {
        spid: $('#spid').val(),
        id_SandwichPanelCustomer: $('#id_SandwichPanelCustomer').val(),
        datetimePlanComplited: $('#datetimePlanComplited').val(),
        datetimeComplited: $('#datetimeComplited').val(),
        numberOrder: $('#numberOrder').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/PostPanelToKO",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#reportTable').DataTable().ajax.reload(null, false);
            $('#approveTable').DataTable().ajax.reload(null, false);
            $('#OSSPModalView').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function postPanelToWork() {
    var typeObj = {
        spid: $('#spid').val(),
        id_SandwichPanelCustomer: $('#id_SandwichPanelCustomer').val(),
        datetimePlanComplited: $('#datetimePlanComplited').val(),
        datetimeComplited: $('#datetimeComplited').val(),
        numberOrder: $('#numberOrder').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/PostPanelToWork",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#reportTable').DataTable().ajax.reload(null, false);
            $('#approveTable').DataTable().ajax.reload(null, false);
            $('#OSSPModalView').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function postPanelToManufacturing() {
    var typeObj = {
        spid: $('#spid').val(),
        id_SandwichPanelCustomer: $('#id_SandwichPanelCustomer').val(),
        datetimePlanComplited: $('#datetimePlanComplited').val(),
        datetimeComplited: $('#datetimeComplited').val(),
        numberOrder: $('#numberOrder').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/PostPanelToManufacturing",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#reportTable').DataTable().ajax.reload(null, false);
            $('#correctionTable').DataTable().ajax.reload(null, false);
            $('#OSSPModalView').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function postPanelToCustomer() {
    var res = ValidPostToCustomer();
    if (res === false) {
        return false;
    }
    var typeObj = {
        spid: $('#spid').val(),
        id_SandwichPanelCustomer: $('#id_SandwichPanelCustomer').val(),
        datetimePlanComplited: $('#datetimePlanComplited').val(),
        datetimeComplited: $('#datetimeComplited').val(),
        numberOrder: $('#numberOrder').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/PostPanelToCustomer",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#reportTable').DataTable().ajax.reload(null, false);
            $('#customerTable').DataTable().ajax.reload(null, false);
            $('#getDateComplitedTable').DataTable().ajax.reload(null, false);
            $('#complitedTable').DataTable().ajax.reload(null, false);
            $('#OSSPModalView').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function postPanelToPlanComplited() {
    var res = ValidPostPanelToCustomer();
    if (res === false) {
        return false;
    }
    var typeObj = {
        spid: $('#spid').val(),
        id_SandwichPanelCustomer: $('#id_SandwichPanelCustomer').val(),
        datetimePlanComplited: $('#datetimePlanComplited').val(),
        datetimeComplited: $('#datetimeComplited').val(),
        numberOrder: $('#numberOrder').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/PostPanelToPlanComplited",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#reportTable').DataTable().ajax.reload(null, false);
            $('#customerTable').DataTable().ajax.reload(null, false);
            $('#getDateComplitedTable').DataTable().ajax.reload(null, false);
            $('#complitedTable').DataTable().ajax.reload(null, false);
            $('#OSSPModalView').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function postPanelToComplited() {
    var res = ValidPostPanelToComplited();
    if (res === false) {
        return false;
    }
    var typeObj = {
        spid: $('#spid').val(),
        id_SandwichPanelCustomer: $('#id_SandwichPanelCustomer').val(),
        datetimePlanComplited: $('#datetimePlanComplited').val(),
        datetimeComplited: $('#datetimeComplited').val(),
        numberOrder: $('#numberOrder').val()
    };
    $.ajax({
        cache: false,
        url: "/CMOArea/PostPanelToComplited",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#reportTable').DataTable().ajax.reload(null, false);
            $('#customerTable').DataTable().ajax.reload(null, false);
            $('#getDateComplitedTable').DataTable().ajax.reload(null, false);
            $('#complitedTable').DataTable().ajax.reload(null, false);
            $('#OSSPModalView').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function ValidPostToCustomer() {
    isValid = true;
    if ($('#id_SandwichPanelCustomer').val() === null) {
        $('#id_SandwichPanelCustomer').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_SandwichPanelCustomer').css('border-color', 'lightgrey');
    }
    return isValid;
}

function ValidPostPanelToCustomer() {
    isValid = true;
    if ($('#id_SandwichPanelCustomer').val() === null) {
        $('#id_SandwichPanelCustomer').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_SandwichPanelCustomer').css('border-color', 'lightgrey');
    }
    if ($('#datetimePlanComplited').val().length < 5) {
        $('#datetimePlanComplited').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#datetimePlanComplited').css('border-color', 'lightgrey');
    }
    return isValid;
}

function ValidPostPanelToComplited() {
    isValid = true;
    if ($('#id_SandwichPanelCustomer').val() === null) {
        $('#id_SandwichPanelCustomer').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_SandwichPanelCustomer').css('border-color', 'lightgrey');
    }
    if ($('#datetimeComplited').val().length < 5) {
        $('#datetimeComplited').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#datetimeComplited').css('border-color', 'lightgrey');
    }
    if ($('#numberOrder').val().length < 1) {
        $('#numberOrder').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#numberOrder').css('border-color', 'lightgrey');
    }
    return isValid;
}

function getSandwichPanel(id) {
    clearTextBoxUpdate();
    $.ajax({
        cache: false,
        url: "/CMOArea/GetSandwichPanel/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#btnPanelToKO').hide();
            $('#btnPanelToWork').hide();
            $('#btnPanelToManufacturing').hide();
            $('#btnPanelToCustomer').hide();
            $('#btnPanelToPlanComplited').hide();
            $('#btnPanelToComplited').hide();
            $('#spid_PlanZakaz2').val(result.spid_PlanZakaz2);
            $('#spid').val(result.spid);
            $('#id_SandwichPanelCustomer').val(result.id_SandwichPanelCustomer);
            $('#spdateTimeCreate').val(result.spdateTimeCreate);
            $('#spid_AspNetUsers_Create').val(result.spid_AspNetUsers_Create);
            $('#datetimeToCorrection').val(processNull(result.datetimeToCorrection));
            $('#datetimeUploadNewVersion').val(processNull(result.datetimeUploadNewVersion));
            $('#datetimeToCustomer').val(processNull(result.datetimeToCustomer));
            $('#datetimePlanComplited').val(processNull(result.datetimePlanComplited));
            $('#datetimeComplited').val(processNull(result.datetimeComplited));
            $('#numberOrder').val(result.numberOrder);
            $('#onApprove').prop('checked', result.onApprove);
            $('#onCorrection').prop('checked', result.onCorrection);
            $('#onCustomer').prop('checked', result.onCustomer);
            $('#onGetDateComplited').prop('checked', result.onGetDateComplited);
            $('#onComplited').prop('checked', result.onComplited);
            if (result.onApprove === true) {
                $('#btnPanelToKO').show();
                $('#btnPanelToWork').show();
            }
            else if (result.onCorrection === true)
                $('#btnPanelToManufacturing').show();
            else if (result.onCustomer === true)
                $('#btnPanelToCustomer').show();
            else if (result.onGetDateComplited === true)
                $('#btnPanelToPlanComplited').show();
            else if (result.onComplited === true)
                $('#btnPanelToComplited').show();
            $('#OSSPModalView').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}