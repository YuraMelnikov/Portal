//type view
//'1' - OTK
//'2' - Manager KO

$(document).ready(function () {
    activeReclamation();
});

var objRemarksList = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "EditLinkJS", "autowidth": true, "bSortable": false },
    { "title": "См", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "СП", "data": "Devision", "autowidth": true, "bSortable": false },
    { "title": "Описание", "data": "Text", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Прим.", "data": "Description", "autowidth": true, "bSortable": false },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": false, "class": 'colu-200' },
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
    id_AspNetUsersCreate: $('#id_AspNetUsersCreate').val(),
    id_DevisionCreate: $('#id_DevisionCreate').val(),
    dateTimeCreate: $('#dateTimeCreate').val(),
    text: $('#text').val(),
    description: $('#description').val(),
    timeToSearch: $('#timeToSearch').val(),
    timeToEliminate: $('#timeToEliminate').val(),
    close: $('#close').val(),
    gip: $('gip').val(),
    closeDevision: $('#closeDevision').val(),
    PCAM: $('#PCAM').val(),
    editManufacturing: $('#editManufacturing').val(),
    editManufacturingIdDevision: $('#editManufacturingIdDevision').val(),
    id_PF: $('#id_PF').val(),
    pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
    technicalAdvice: $('#technicalAdvice').val()
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
        //"order": [[2, "desc"]],
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
        //"order": [[2, "desc"]],
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
    var objRemark = {
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        id_Reclamation_Type: $('#id_Reclamation_Type').val(),
        close: $('#close').is(":checked"),
        id_DevisionReclamation: $('#id_DevisionReclamation').val(),
        id_AspNetUsersError: $('#id_AspNetUsersError').val(),
        id_Reclamation_CountErrorFirst: $('#id_Reclamation_CountErrorFirst').val(),
        timeToSearch: $('#timeToSearch').val(),
        timeToEliminate: $('#timeToEliminate').val(),
        text: $('#text').val(),
        description: $('#description').val(),
        id_PF: $('#id_PF').val(),
        PCAM: $('#PCAM').val(),
        closeDevision: $('#closeDevision').is(":checked"),
        gip: $('#gip').is(":checked"),
        trash: $('#trash').is(":checked"),
        editManufacturingIdDevision: $('#editManufacturingIdDevision').val(),
        editManufacturing: $('#editManufacturing').is(":checked"),
        technicalAdvice: $('#technicalAdvice').is(":checked")
    };
    $.ajax({
        cache: false,
        url: "/Remarks/Add",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            activeReclamation();
            $('#viewReclamation').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#id_DevisionReclamation').val() !== null && $('#id_AspNetUsersError').val() !== null) {
        $('#id_DevisionReclamation').css('border-color', 'Red');
        $('#id_AspNetUsersError').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#editManufacturingIdDevision').css('border-color', 'lightgrey');
    }
    if ($('#editManufacturing').is(":checked") === true && $('#editManufacturingIdDevision').val() === null) {
        $('#editManufacturingIdDevision').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#editManufacturingIdDevision').css('border-color', 'lightgrey');
    }
    if ($('#pZ_PlanZakaz').val().length === 0) {
        $('#pZ_PlanZakaz').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#pZ_PlanZakaz').css('border-color', 'lightgrey');
    }
    if ($('#id_Reclamation_Type').val() === null) {
        $('#id_Reclamation_Type').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_Reclamation_Type').css('border-color', 'lightgrey');
    }
    if ($('#id_DevisionReclamation').val() === null && $('#id_AspNetUsersError').val() === null) {
        $('#id_DevisionReclamation').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_DevisionReclamation').css('border-color', 'lightgrey');
    }
    if ($('#text').val().trim() === "") {
        $('#text').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#text').css('border-color', 'lightgrey');
    }
    if ($('#id_PF').val() === null) {
        $('#id_PF').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_PF').css('border-color', 'lightgrey');
    }
    if ($('#reloadDevision').val() === null && $('#reload').is(":checked") === true) {
        $('#reloadDevision').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#reloadDevision').css('border-color', 'lightgrey');
    }
    return isValid;
}

function clearTextBox() {
    AllNoDisabled();
    $("#btnAdd").attr('disabled', false);
    $('#id').val("");
    $('#pZ_PlanZakaz').val("");
    $('#pZ_PlanZakaz').chosen();
    $('#pZ_PlanZakaz').trigger('chosen:updated');
    $('#id_Reclamation_Type').val("");
    $('#id_AspNetUsersCreate').val("");
    $('#dateTimeCreate').val("");
    $('#close').prop('checked', false);
    $('#id_DevisionReclamation').val("");
    $('#id_AspNetUsersError').val("");
    $('#id_Reclamation_CountErrorFirst').val("");
    $('#id_Reclamation_CountErrorFinal').val("");
    $('#timeToSearch').val("");
    $('#timeToEliminate').val("");
    $('#text').val("");
    $('#description').val("");
    $('#id_PF').val("");
    $('#PCAM').val("");
    $('#closeDevision').prop('checked', false);
    $('#answerText').val("");
    $('#answerHistiryText').val("");
    $('#reloadDevision').val("");
    $('#reload').prop('checked', false);
    $('#gip').prop('checked', false);
    $('#trash').prop('checked', false);
    $('#editManufacturing').prop('checked', false);
    $('#editManufacturingIdDevision').val("");
    $('#technicalAdvice').prop('checked', false);
    
    $('#id_Reclamation_CountErrorFinal').prop('disabled', true);
    $('#closeDevision').prop('disabled', true);
    $('#answerText').prop('disabled', true);
    $('#answerHistiryText').prop('disabled', true);
    $('#reloadDevision').prop('disabled', true);
    $('#reload').prop('disabled', true);
    $('#trash').prop('disabled', true);

    if (counterDevision === 1) {
        $('#id_AspNetUsersError').prop('disabled', true);
        $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
        $('#gip').prop('disabled', true);
        $('#technicalAdvice').prop('disabled', true);
    }
    else if (counterDevision === 2) {
        $('#close').prop('disabled', true);
        $('#timeToSearch').prop('disabled', true);
        $('#timeToEliminate').prop('disabled', true);
    }
    else {
        $('#id_AspNetUsersError').prop('disabled', true);
        $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
        $('#gip').prop('disabled', true);
        $('#close').prop('disabled', true);
        $('#timeToSearch').prop('disabled', true);
        $('#timeToEliminate').prop('disabled', true);
        $('#technicalAdvice').prop('disabled', true);
    }
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function GetReclamation(id) {
    var myVal = counterDevision;
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/Remarks/GetReclamation/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            AllNoDisabled();
            $('#id').val(result.id);
            $("#pZ_PlanZakaz").val(result.pZ_PlanZakaz).trigger("chosen:updated");
            $('#id_Reclamation_Type').val(result.id_Reclamation_Type);
            $('#id_DevisionReclamation').val(result.id_DevisionReclamation);
            $('#id_Reclamation_CountErrorFirst').val(result.id_Reclamation_CountErrorFirst);
            $('#id_Reclamation_CountErrorFinal').val(result.id_Reclamation_CountErrorFinal);
            $('#vid_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_DevisionCreate').val(result.id_DevisionCreate);
            $('#vdateTimeCreate').val(result.dateTimeCreate);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#timeToSearch').val(result.timeToSearch);
            $('#timeToEliminate').val(result.timeToEliminate);
            $('#close').prop('checked', result.close);
            $('#gip').prop('checked', result.gip);
            $('#closeDevision').prop('checked', result.closeDevision);
            $('#PCAM').val(result.PCAM);
            $('#editManufacturing').prop('checked', result.editManufacturing);
            $('#editManufacturingIdDevision').val(result.editManufacturingIdDevision);
            $('#id_PF').val(result.id_PF);
            $('#technicalAdvice').prop('checked', result.technicalAdvice);
            $('#id_AspNetUsersError').val(result.id_AspNetUsersError);
            $('#reloadDevision').val("");
            $('#reload').prop('checked', false);
            $('#id_Reclamation_CountErrorFinal').prop('disabled', true);
            $('#closeDevision').prop('disabled', true);
            $('#answerHistiryText').prop('disabled', true);
            $('#id_DevisionReclamation').prop('disabled', true);
            if (counterDevision === 1) {
                $('#id_AspNetUsersError').prop('disabled', true);
                $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
                $('#gip').prop('disabled', true);
                $('#technicalAdvice').prop('disabled', true);
                $('#answerText').prop('disabled', true);
                $('#trash').prop('disabled', true);
            }
            else if (counterDevision === 2) {
                $('#close').prop('disabled', true);
                $('#timeToSearch').prop('disabled', true);
                $('#timeToEliminate').prop('disabled', true);
            }
            else {
                $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
                $('#gip').prop('disabled', true);
                $('#close').prop('disabled', true);
                $('#timeToSearch').prop('disabled', true);
                $('#timeToEliminate').prop('disabled', true);
                $('#technicalAdvice').prop('disabled', true);
            }
            $('#viewReclamation').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function GetReclamationView(id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/Remarks/GetReclamation/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#id_Reclamation_Type').val(result.id_Reclamation_Type);
            $('#id_DevisionReclamation').val(result.id_DevisionReclamation);
            $('#id_Reclamation_CountErrorFirst').val(result.id_Reclamation_CountErrorFirst);
            $('#id_Reclamation_CountErrorFinal').val(result.id_Reclamation_CountErrorFinal);
            $('#vid_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_DevisionCreate').val(result.id_DevisionCreate);
            $('#vdateTimeCreate').val(result.dateTimeCreate);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#timeToSearch').val(result.timeToSearch);
            $('#timeToEliminate').val(result.timeToEliminate);
            $('#close').prop('checked', result.close);
            $('#gip').prop('checked', result.gip);
            $('#closeDevision').prop('checked', result.closeDevision);
            $('#PCAM').val(result.PCAM);
            $('#editManufacturing').prop('checked', result.editManufacturing);
            $('#editManufacturingIdDevision').val(result.editManufacturingIdDevision);
            $('#id_PF').val(result.id_PF);
            $('#technicalAdvice').prop('checked', result.technicalAdvice);
            $('#id_AspNetUsersError').val(result.id_AspNetUsersError);
            $('#reloadDevision').val("");
            $('#reload').prop('checked', false);
            $('#id').prop('disabled', true);
            $('#id_Reclamation_Type').prop('disabled', true);
            $('#id_DevisionReclamation').prop('disabled', true);
            $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
            $('#id_Reclamation_CountErrorFinal').prop('disabled', true);
            $('#id_AspNetUsersCreate').prop('disabled', true);
            $('#id_DevisionCreate').prop('disabled', true);
            $('#dateTimeCreate').prop('disabled', true);
            $('#text').prop('disabled', true);
            $('#description').prop('disabled', true);
            $('#timeToSearch').prop('disabled', true);
            $('#timeToEliminate').prop('disabled', true);
            $('#close').prop('disabled', true);
            $('#gip').prop('disabled', true);
            $('#closeDevision').prop('disabled', true);
            $('#PCAM').prop('disabled', true);
            $('#editManufacturing').prop('disabled', true);
            $('#editManufacturingIdDevision').prop('disabled', true);
            $('#id_PF').prop('disabled', true);
            $('#technicalAdvice').prop('disabled', true);
            $('#id_AspNetUsersError').prop('disabled', true);
            $('#reloadDevision').prop('disabled', true);
            $('#reload').prop('disabled', true);
            $('#answerHistiryText').prop('disabled', true);
            $('#answerText').prop('disabled', true);
            $('#trash').prop('disabled', true);
            $('#pZ_PlanZakaz').prop('disabled', true);
            $("#pZ_PlanZakaz").val(result.pZ_PlanZakaz).trigger("chosen:updated");
            $('#viewReclamation').modal('show');
            $('#btnUpdate').hide();
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
    var objRemark = {
        id: $('#id').val(),
        dateTimeCreate: $('#dateTimeCreate').val(),
        id_AspNetUsersCreate: $('#id_AspNetUsersCreate').val(),
        id_DevisionCreate: $('#id_DevisionCreate').val(),
        reloadDevision: $('#reloadDevision').val(),
        id_Reclamation_CountErrorFinal: $('#id_Reclamation_CountErrorFinal').val(),
        reload: $('#reload').is(":checked"),
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        id_Reclamation_Type: $('#id_Reclamation_Type').val(),
        close: $('#close').is(":checked"),
        id_DevisionReclamation: $('#id_DevisionReclamation').val(),
        id_AspNetUsersError: $('#id_AspNetUsersError').val(),
        id_Reclamation_CountErrorFirst: $('#id_Reclamation_CountErrorFirst').val(),
        timeToSearch: $('#timeToSearch').val(),
        timeToEliminate: $('#timeToEliminate').val(),
        text: $('#text').val(),
        description: $('#description').val(),
        id_PF: $('#id_PF').val(),
        PCAM: $('#PCAM').val(),
        closeDevision: $('#closeDevision').is(":checked"),
        gip: $('#gip').is(":checked"),
        trash: $('#trash').is(":checked"),
        editManufacturingIdDevision: $('#editManufacturingIdDevision').val(),
        editManufacturing: $('#editManufacturing').is(":checked"),
        technicalAdvice: $('#technicalAdvice').is(":checked")
    };
    $.ajax({
        cache: false,
        url: "/Remarks/Update/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            activeReclamation();
            $('#viewReclamation').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function AllNoDisabled() {
    $('#id').prop('disabled', false);
    $('#id_Reclamation_Type').prop('disabled', false);
    $('#id_DevisionReclamation').prop('disabled', false);
    $('#id_Reclamation_CountErrorFirst').prop('disabled', false);
    $('#id_Reclamation_CountErrorFinal').prop('disabled', false);
    $('#id_DevisionCreate').prop('disabled', false);
    $('#text').prop('disabled', false);
    $('#description').prop('disabled', false);
    $('#timeToSearch').prop('disabled', false);
    $('#timeToEliminate').prop('disabled', false);
    $('#close').prop('disabled', false);
    $('#gip').prop('disabled', false);
    $('#closeDevision').prop('disabled', false);
    $('#PCAM').prop('disabled', false);
    $('#editManufacturing').prop('disabled', false);
    $('#editManufacturingIdDevision').prop('disabled', false);
    $('#id_PF').prop('disabled', false);
    $('#technicalAdvice').prop('disabled', false);
    $('#id_AspNetUsersError').prop('disabled', false);
    $('#reloadDevision').prop('disabled', false);
    $('#reloadDevision').prop('disabled', false);
    $('#reload').prop('disabled', false);
    $('#pZ_PlanZakaz').prop('disabled', false);
    $('#answerHistiryText').prop('disabled', false);
    $('#answerText').prop('disabled', false);
    $('#trash').prop('disabled', false);
}

function clearid_AspNetUsersError() {
    $('#id_AspNetUsersError').val("");
}

function clearid_DevisionReclamation() {
    $('#id_DevisionReclamation').val("");
}