$(document).ready(function () {
    activeReclamation();
});

var objRemarksList = [
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

var objRemark = {
    id: $('#id').val(),
    id_Reclamation_Type: $('#id_Reclamation_Type').val(),
    id_DevisionReclamation: $('#id_DevisionReclamation').val(),
    id_Reclamation_CountErrorFirst: $('#id_Reclamation_CountErrorFirst').val(),
    id_Reclamation_CountErrorFinal: $('#id_Reclamation_CountErrorFinal').val(),
    id_PZ_OperatorDogovora: $('#id_PZ_OperatorDogovora').val(),
    id_AspNetUsersCreate: $('#id_AspNetUsersCreate').val(),
    id_DevisionCreate: $('#id_DevisionCreate').val(),
    dateTimeCreate: $('#dateTimeCreate').val(),
    text: $('#text').val(),
    description: $('#description').val(),
    timeToSearch: $('#timeToSearch').val(),
    timeToEliminate: $('#timeToEliminate').val(),
    close: $('#close').val(),
    closeDevision: $('#closeDevision').val(),
    PCAM: $('#PCAM').val(),
    editManufacturing: $('#editManufacturing').val()
};

function activeReclamation() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Remarks/ActiveReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "order": [[2, "desc"]],
        "columns": objRemarksList,
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
        "processing": true,
        "order": [[2, "desc"]],
        "columns": objRemarksList,
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

function Add() {
    var res = validate();
    if (res === false) {
        return false;
    }
    $("#btnAdd").attr('disabled', true);
    var objRemark;
    $.ajax({
        cache: false,
        url: "/Remarks/Add",
        data: JSON.stringify(objRemark),
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
    if ($('#id_Reclamation_Type').val().trim() === "") {
        $('#id_Reclamation_Type').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_Reclamation_Type').css('border-color', 'lightgrey');
    }
    if ($('#id_DevisionReclamation').val() === null) {
        $('#id_DevisionReclamation').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_DevisionReclamation').css('border-color', 'lightgrey');
    }

    if ($('#text').val() === null) {
        $('#text').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#text').css('border-color', 'lightgrey');
    }
    return isValid;
}

function clearTextBox() {
    $("#btnAdd").attr('disabled', false);
    $('#id').val("");
    $('#id_Reclamation_Type').val("");
    $('#id_DevisionReclamation').val("");
    $('#id_Reclamation_CountErrorFirst').val("");
    $('#id_Reclamation_CountErrorFinal').val("");
    $('#id_AspNetUsersCreate').val("");
    $('#id_DevisionCreate').val("");
    $('#dateTimeCreate').val("");
    $('#text').val("");
    $('#description').val("");
    $('#timeToSearch').val("");
    $('#timeToEliminate').val("");
    $('#close').val("");
    $('#gip').val("");
    $('#closeDevision').val("");
    $('#PCAM').val("");
    $('#editManufacturing').val("");
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
        url: "/Remarks/GetOrder/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#id_Reclamation_Type').val(result.id_Reclamation_Type);
            $('#id_DevisionReclamation').val(result.id_DevisionReclamation);
            $('#id_Reclamation_CountErrorFirst').val(result.id_Reclamation_CountErrorFirst);
            $('#id_Reclamation_CountErrorFinal').val(result.id_Reclamation_CountErrorFinal);
            $('#id_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_DevisionCreate').val(result.id_DevisionCreate);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#timeToSearch').val(result.timeToSearch);
            $('#timeToEliminate').val(result.timeToEliminate);
            $('#close').val(result.close);
            $('#gip').val(result.gip);
            $('#closeDevision').val(result.closeDevision);
            $('#PCAM').val(result.PCAM);
            $('#editManufacturing').val(result.editManufacturing);
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
    var res = validate();
    if (res === false) {
        return false;
    }
    objRemark;
    $.ajax({
        cache: false,
        url: "/Remarks/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#orderModal').modal('hide');
            $('#id').val("");
            $('#id_Reclamation_Type').val("");
            $('#id_DevisionReclamation').val("");
            $('#id_Reclamation_CountErrorFirst').val("");
            $('#id_Reclamation_CountErrorFinal').val("");
            $('#id_AspNetUsersCreate').val("");
            $('#id_DevisionCreate').val("");
            $('#dateTimeCreate').val("");
            $('#text').val("");
            $('#description').val("");
            $('#timeToSearch').val("");
            $('#timeToEliminate').val("");
            $('#close').val("");
            $('#gip').val("");
            $('#closeDevision').val("");
            $('#PCAM').val("");
            $('#editManufacturing').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}