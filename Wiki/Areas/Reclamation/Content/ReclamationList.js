$(document).ready(function () {
    loadData();
});

var objRemark = [
    { "title": "Ред", "data": "EditLinkJS", "autowidth": true, "bSortable": false },
    { "title": "См", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "СП", "data": "Devision", "autowidth": true, "bSortable": false },
    { "title": "Описание", "data": "Text", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Прим.", "data": "Description", "autowidth": true, "bSortable": false },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Рук. КБ", "data": "AnswersChief", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Создал", "data": "UserCreate", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Ответственный", "data": "UserReclamation", "autowidth": true, "bSortable": true },
    { "title": "Степень ошибки", "data": "LeavelReclamation", "autowidth": true, "bSortable": true }
];

var objOrder = [
    { "title": "См.", "data": "OpenLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Контрактное наименование", "data": "ContractName", "autowidth": true, "bSortable": false },
    { "title": "Наименование по ТУ", "data": "TuName", "autowidth": true, "bSortable": false },
    { "title": "Заказчик", "data": "Client", "autowidth": true, "bSortable": true },
    { "title": "МТР №", "data": "Mtr", "autowidth": true, "bSortable": false },
    { "title": "ОЛ №", "data": "Ol", "autowidth": true, "bSortable": true },
    { "title": "Ошибок", "data": "ReclamationCount", "autowidth": true, "bSortable": true },
    { "title": "Активных", "data": "ReclamationActive", "autowidth": true, "bSortable": true },
    { "title": "Закрытых", "data": "ReclamationClose", "autowidth": true, "bSortable": true }
];

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/ActiveReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "columns": objRemark,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        },
        initComplete: function () {
            this.api().columns([2, 9, 10, 11]).every(function () {
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

function planZakazDevisionNotSh() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionNotSh",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "columns": objOrder,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        },
        initComplete: function () {
            this.api().columns([2, 9, 10, 11]).every(function () {
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

function planZakazDevisionSh() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionSh",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "columns": objOrder,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        },
        initComplete: function () {
            this.api().columns([2, 9, 10, 11]).every(function () {
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

function planZakazDevisionAll() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionAll",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "columns": objOrder,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        },
        initComplete: function () {
            this.api().columns([2, 9, 10, 11]).every(function () {
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

function closeReclamation() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/CloseReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "columns": objRemark,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        },
        initComplete: function () {
            this.api().columns([2, 9, 10, 11]).every(function () {
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