$(document).ready(function () {
    loadData();
});

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
        "columns": [
            { "title": "Ред", "data": "EditLinkJS", "autowidth": true, "bSortable": false },
            { "title": "См", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },

            { "title": "Заводской номер", "data": "PlanZakaz", "autowidth": true, "bSortable": false },
            { "title": "№ рекл.", "data": "Id_Reclamation", "autowidth": true, "bSortable": false },
            { "title": "СП", "data": "Devision", "autowidth": true, "bSortable": false },
            { "title": "Тип", "data": "Type", "autowidth": true, "bSortable": false },
            { "title": "Описание", "data": "Text", "autowidth": true, "bSortable": false },
            { "title": "Прим.", "data": "Description", "autowidth": true, "bSortable": false },
            { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": false }



            


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
