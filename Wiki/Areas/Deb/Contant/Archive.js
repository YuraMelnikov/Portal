$(document).ready(function () {
    loadData();
});

function loadData() {
    $("#myTable").DataTable({
        "ajax": {
            "url": "/ArchiveReport/List",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "columns": [
            { "title": "Cм.", "data": "id", "autowidth": true, "bSortable": false }
            , { "title": "Период", "data": "period", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Дата открытия", "data": "dateTimeCreate", "autowidth": true, "bSortable": false, "className": 'text-center' }
            , { "title": "Дата закрытия", "data": "dateTimeClose", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": processNull }
            , { "title": "Отметка о закрытии", "data": "close", "autowidth": true, "bSortable": false, "className": 'text-center', "defaultContent": "", "render": localRUStatus }
       ],
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
            this.api().columns([3]).every(function () {
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

function localRUStatus(data) {
    if (data === true) {
        return 'Открыт';
    } else {
        return 'Закрыт';
    }
}

function getbyID(id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/ArchiveReport/GetId/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html1 = '';
            for (i in result) {
                html1 += '<tr>';
                html1 += '<td>' + result[i].PlanZakaz + '</td>';
                html1 += '<td>' + result[i].Name + '</td>';
                html1 += '<td>' + result[i].Manager + '</td>';
                html1 += '<td>' + result[i].Client + '</td>';
                html1 += '<td>' + localRUStatusTable(result[i].oprihClose) + '</td>';
                html1 += '<td>' + result[i].dateOprihPlanFact + '</td>';
                html1 += '<td>' + result[i].dataOtgruzkiBP + '</td>';
                html1 += '<td>' + result[i].numberSF + '</td>';
                html1 += '<td>' + result[i].DateSupply + '</td>';
                html1 += '<td>' + result[i].reclamation + '</td>';
                html1 += '<td>' + processNull(result[i].openReclamation) + '</td>';
                html1 += '<td>' + processNull(result[i].closeReclamation) + '</td>';
                html1 += '<td>' + result[i].description + '</td>';
                html1 += '</tr>';
            }
            $('.tbody').html(html1);
            $('#orderModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function localRUStatusTable(dataLoad) {
    if (dataLoad === true) {
        return 'Оприходован';
    } else {
        return 'Не оприходован';
    }
}

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}