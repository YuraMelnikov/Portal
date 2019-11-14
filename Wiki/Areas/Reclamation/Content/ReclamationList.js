//type view
//'1' - OTK
//'2' - Manager KO

$(document).ready(function () {
    document.getElementById('labelNamePage').innerHTML = 'Активные';
    startMenu();
    expertHide();
    $('#btnCloseOrder').hide();
    $('#zakazId').hide();
    $('#toExcelModal').hide();
    $('#pageData').hide();
    $('#expertData').hide();
    if (buttonAddActivation === 0)
        $('#btnAddNewReclamation').hide();
});

function loadData(listId) {
    clearTextBox();
    document.getElementById('pageData').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        document.getElementById('labelNamePage').innerHTML = 'Активные замечания';
        $('#labelNamePage').val("Активные замечания");
        expertHide();
        nullpz();
        activeReclamation();
    }
    else if (listId === 2 || listId === "2") {
        document.getElementById('labelNamePage').innerHTML = 'Закрытые замечания';
        expertHide();
        nullpz();
        closeReclamation();
    }
    else if (listId === 3 || listId === "3") {
        document.getElementById('labelNamePage').innerHTML = 'Все замечания';
        expertHide();
        nullpz();
        allReclamation();
    }
    else if (listId === 4 || listId === "4") {
        document.getElementById('labelNamePage').innerHTML = 'Неотгруженные заказы';
        expertHide();
        nullpz();
        planZakazDevisionNotSh();
    }
    else if (listId === 5 || listId === "5") {
        document.getElementById('labelNamePage').innerHTML = 'Отгруженные заказы';
        expertHide();
        nullpz();
        planZakazDevisionSh();
    }
    else if (listId === 6 || listId === "6") {
        document.getElementById('labelNamePage').innerHTML = 'Замечания по заказу';
        expertHide();
        nullpz();
        planZakazDevisionAll();
    }
    else if (listId === 7 || listId === "7") {
        document.getElementById('labelNamePage').innerHTML = 'Итоги ОТК';
        expertShow();
        nullpz();
        reclamationOTK();
    }
    else if (listId === 8 || listId === "8") {
        document.getElementById('labelNamePage').innerHTML = 'Итоги ПО';
        expertShow();
        nullpz();
        reclamationPO();
    }
    else if (listId === 9 || listId === "9") {
        document.getElementById('labelNamePage').innerHTML = 'Созданные мной замечания';
        expertHide();
        nullpz();
        myReclamation();
    }
    else if (listId === 10 || listId === "10") {
        document.getElementById('labelNamePage').innerHTML = 'Исправления ПО';
        expertHide();
        nullpz();
        editManufList();
    }
    else if (listId === 11 || listId === "11") {
        document.getElementById('labelNamePage').innerHTML = 'Активные созданные мной замечания';
        expertHide();
        nullpz();
        activeReclamationMy();
    }
    else if (listId === 12 || listId === "12") {
        document.getElementById('labelNamePage').innerHTML = 'Неотгруженные заказы (только активные созданные мной)';
        expertHide();
        nullpz();
        planZakazDevisionMy();
    }
    else {
        document.getElementById('labelNamePage').innerHTML = 'Активные замечания';
        expertHide();
        nullpz();
        activeReclamation();
    }
}

function pz(id) {
    document.getElementById('zakazId').innerHTML = id;
}

function nullpz() {
    document.getElementById('zakazId').innerHTML = "";
}

function expertHide() {
    document.getElementById('expertData').innerHTML = 0;
    $('#btnExpert').hide();
}

function expertShow() {
    document.getElementById('expertData').innerHTML = 1;
    $('#btnExpert').show();
}

var objRemarksList = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "EditLinkJS", "autowidth": true, "bSortable": false },
    { "title": "См", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "СП", "data": "Devision", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "Text", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Прим.", "data": "Description", "autowidth": true, "bSortable": false },
    { "title": "Полуфабрикат", "data": "PfName", "autowidth": true, "bSortable": true },
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
    closeMKO: $('#closeMKO').val(),
    gip: $('gip').val(),
    closeDevision: $('#closeDevision').val(),
    PCAM: $('#PCAM').val(),
    editManufacturing: $('#editManufacturing').val(),
    editManufacturingIdDevision: $('#editManufacturingIdDevision').val(),
    id_PF: $('#id_PF').val(),
    pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
    technicalAdvice: $('#technicalAdvice').val()
};

function startMenu() {
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/ActiveReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objRemarksList,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
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

function activeReclamation() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/ActiveReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objRemarksList,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
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

function activeReclamationMy() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/ActiveReclamationMy",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objRemarksList,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
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

function closeReclamation() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/CloseReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
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
        }
    });
}

function allReclamation() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/AllReclamation",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objRemarksList,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
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
        timeToSearch: $('#timeToSearch').val().replace('.', ','),
        timeToEliminate: $('#timeToEliminate').val().replace('.', ','),
        text: $('#text').val(),
        description: $('#description').val(),
        id_PF: $('#id_PF').val(),
        PCAM: $('#PCAM').val(),
        closeDevision: $('#closeDevision').is(":checked"),
        closeMKO: $('#closeMKO').val(),
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
            $('#myTable').DataTable().ajax.reload(null, false);
            $('#viewReclamation').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    $('#id_DevisionReclamation').css('border-color', 'lightgrey');
    $('#id_AspNetUsersError').css('border-color', 'lightgrey');
    var isValid = true;
    if ($('#reload').is(":checked") === false) {
        if ($('#id_DevisionReclamation').val() === null && $('#id_AspNetUsersError').val() === null) {
            $('#id_DevisionReclamation').css('border-color', 'Red');
            $('#id_AspNetUsersError').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#id_DevisionReclamation').css('border-color', 'lightgrey');
        }
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
    clearColor();
    AllNoDisabled();
    $("#btnAdd").attr('disabled', false);
    $('#vid_AspNetUsersCreate').val("");
    $('#vdateTimeCreate').val("");
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
    $('#btnUpdate').hide();
    $('#btnRemove').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function GetReclamation(id) {
    UpdateDevisionList();
    clearTextBox();
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
            $('#numberReclamation').val('Замечание №: ' + result.id);
            $('#fixedExpert').val(result.fixedExpert);
            $("#pZ_PlanZakaz").val(result.pZ_PlanZakaz).trigger("chosen:updated");
            $('#id_Reclamation_Type').val(result.id_Reclamation_Type);
            $('#id_DevisionReclamation').val(result.id_DevisionReclamation);
            $('#id_Reclamation_CountErrorFirst').val(result.id_Reclamation_CountErrorFirst);
            $('#id_Reclamation_CountErrorFinal').val(result.id_Reclamation_CountErrorFinal);
            $('#vid_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_DevisionCreate').val(result.id_DevisionCreate);
            $('#vdateTimeCreate').val(result.dateTimeCreate);
            $('#closeMKO').val(result.closeMKO);
            $('#answerText').val("");
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#timeToSearch').val(result.timeToSearch);
            $('#timeToEliminate').val(result.timeToEliminate);
            $('#close').prop('checked', result.close);
            $('#gip').prop('checked', result.gip);
            $('#closeDevision').prop('checked', result.closeDevision);
            $('#PCAM').val(result.PCAM);
            $('#answerHistiryText').val(result.answerHistiryText);
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
            $('#btnRemove').hide();
            if (counterDevision === 1) {
                if (result.answerHistiryText.length === 0) {
                    $('#btnRemove').show();
                }
                $('#id_AspNetUsersError').prop('disabled', true);
                $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
                $('#gip').prop('disabled', true);
                $('#answerText').prop('disabled', false);
                $('#trash').prop('disabled', true);
            }
            else if (counterDevision === 2) {
                if ($('#id_DevisionCreate').val() !== devisionUser) {
                    $('#text').prop('disabled', true);
                }
                $('#description').prop('disabled', true);
                $('#close').prop('disabled', true);
                $('#timeToSearch').prop('disabled', true);
                $('#timeToEliminate').prop('disabled', true);
                $('#trash').prop('disabled', false);
                $('#btnRemove').hide();
            }
            else if (counterDevision === 3) {
                $('#text').prop('disabled', true);
                $('#description').prop('disabled', true);
                $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
                $('#gip').prop('disabled', true);
                $('#close').prop('disabled', true);
                $('#timeToSearch').prop('disabled', true);
                $('#timeToEliminate').prop('disabled', true);
                $('#technicalAdvice').prop('disabled', true);
                $('#trash').prop('disabled', false);
                $('#btnRemove').hide();
            }
            else {
                $('#text').prop('disabled', true);
                $('#description').prop('disabled', true);
                $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
                $('#gip').prop('disabled', true);
                $('#close').prop('disabled', true);
                $('#timeToSearch').prop('disabled', true);
                $('#timeToEliminate').prop('disabled', true);
                $('#technicalAdvice').prop('disabled', true);
                $('#trash').prop('disabled', true);
                $('#btnRemove').hide();
            }
            if ($('#technicalAdvice').is(":checked") === true) {
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
    UpdateDevisionList();
    clearTextBox();
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
            $('#numberReclamation').val('Замечание №: ' + result.id);
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
            $('#answerHistiryText').val(result.answerHistiryText);
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

function remove() {
    if (counterDevision !== 1) {
        if ($('#answerText').val() === "" && $('#reload').is(":checked") === false) {
            $('#answerText').css('border-color', 'Red');
            res = false;
        }
        else {
            $('#answerText').css('border-color', 'lightgrey');
        }
    }
    if (document.getElementById('expertData').innerHTML === "1") {
        $('#fixedExpert').val(true);
    }
    var id = $('#id').val();
    var pz = document.getElementById('zakazId').innerHTML;
    var objRemark = {
        id: $('#id').val()
    };
    $.ajax({
        cache: false,
        url: "/Remarks/DeleteOrder/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#viewReclamation').modal('hide');
            if (pz === "")
                $('#myTable').DataTable().ajax.reload(null, false);
            else {
                var id = document.getElementById('zakazId').innerHTML;
                reclamationsPlanZakaz(id);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Update() {
    var res = validate();
    if (counterDevision !== 1) {
        if ($('#answerText').val() === "" && $('#reload').is(":checked") === false) {
            $('#answerText').css('border-color', 'Red');
            res = false;
        }
        else {
            $('#answerText').css('border-color', 'lightgrey');
        }
    }
    if (document.getElementById('expertData').innerHTML === "1") {
        $('#fixedExpert').val(true);
    }
    if (res === false) {
        return false;
    }
    var id = $('#id').val();
    var pz = document.getElementById('zakazId').innerHTML;
    var objRemark = {
        id: $('#id').val(),
        fixedExpert: $('#fixedExpert').val(),
        closeMKO: $('#closeMKO').val(),
        dateTimeCreate: $('#dateTimeCreate').val(),
        id_AspNetUsersCreate: $('#id_AspNetUsersCreate').val(),
        id_DevisionCreate: $('#id_DevisionCreate').val(),
        reloadDevision: $('#reloadDevision').val(),
        id_Reclamation_CountErrorFinal: $('#id_Reclamation_CountErrorFinal').val(),
        reload: $('#reload').is(":checked"),
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        answerText: $('#answerText').val(),
        id_Reclamation_Type: $('#id_Reclamation_Type').val(),
        close: $('#close').is(":checked"),
        id_DevisionReclamation: $('#id_DevisionReclamation').val(),
        id_AspNetUsersError: $('#id_AspNetUsersError').val(),
        id_Reclamation_CountErrorFirst: $('#id_Reclamation_CountErrorFirst').val(),
        timeToSearch: $('#timeToSearch').val().replace('.', ','),
        timeToEliminate: $('#timeToEliminate').val().replace('.', ','),
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
            $('#viewReclamation').modal('hide');
            $('#myTable').DataTable().ajax.reload(null, false);
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

var objOrder = [
    { "title": "См.", "data": "OpenLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Ошибок", "data": "ReclamationCount", "autowidth": true, "bSortable": true },
    { "title": "Активных", "data": "ReclamationActive", "autowidth": true, "bSortable": true },
    { "title": "Закрытых", "data": "ReclamationClose", "autowidth": true, "bSortable": true },
    { "title": "Контрактное наименование", "data": "ContractName", "autowidth": true, "bSortable": false },
    { "title": "Наименование по ТУ", "data": "TuName", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Заказчик", "data": "Client", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "МТР №", "data": "Mtr", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "ОЛ №", "data": "Ol", "autowidth": true, "bSortable": true, "class": 'colu-200' }
];

function planZakazDevisionNotSh() {
    pz(id);
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionNotSh",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "order": [[1, "desc"]],
        "rowCallback": function (row, data, index) {
            if (data.ReclamationActive > 0) {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
        },
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
        }
    });
}

function planZakazDevisionMy() {
    pz(id);
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionNotShMy",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "order": [[1, "desc"]],
        "rowCallback": function (row, data, index) {
            if (data.ReclamationActive > 0) {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
        },
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
        }
    });
}

function planZakazDevisionSh() {
    pz(id);
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionSh",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "order": [[1, "desc"]],
        "rowCallback": function (row, data, index) {
            if (data.ReclamationActive > 0) {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
        },
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
        }
    });
}

function planZakazDevisionAll() {
    pz(id);
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/PlanZakazDevisionAll",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "order": [[1, "desc"]],
        "columns": objOrder,
        "rowCallback": function (row, data, index) {
            if (data.ReclamationActive > 0) {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
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

var objRemarksListExpert = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "LinkToEdit", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "Orders", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "TextReclamation", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Прим.", "data": "DescriptionReclamation", "autowidth": true, "bSortable": true },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Создал", "data": "UserCreate", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Ответственное СП", "data": "DevisionReclamation", "autowidth": true, "bSortable": true },
    { "title": "Оценка Рук. КБ", "data": "LeavelReclamation", "autowidth": true, "bSortable": true },
    { "title": "Оценка эксперта", "data": "LastLeavelReclamation", "autowidth": true, "bSortable": true }
];

function reclamationOTK() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/GetRemarksOTK",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objRemarksListExpert,
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

function reclamationPO() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/GetRemarksPO",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objRemarksListExpert,
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

function GetReclamationExpert(id) {
    var myVal = counterDevision;
    $.ajax({
        cache: false,
        url: "/Remarks/GetReclamation/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').prop('disabled', true);
            $('#id_Reclamation_CountErrorFinal').prop('disabled', false);
            $('#id_Reclamation_Type').prop('disabled', true);
            $('#id_DevisionReclamation').prop('disabled', true);
            $('#id_Reclamation_CountErrorFirst').prop('disabled', true);
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
            if (result.technicalAdvice !== true) {
                $('#technicalAdvice').prop('disabled', false);
            }
            $('#id_AspNetUsersError').prop('disabled', true);
            $('#reloadDevision').prop('disabled', true);
            $('#reload').prop('disabled', true);
            $('#answerHistiryText').prop('disabled', true);
            $('#answerText').prop('disabled', true);
            $('#trash').prop('disabled', true);
            $('#pZ_PlanZakaz').prop('disabled', true);
            $('#id').val(result.id);
            $('#numberReclamation').val('Замечание №: ' + result.id);
            $('#fixedExpert').val(result.fixedExpert);
            $("#pZ_PlanZakaz").val(result.pZ_PlanZakaz).trigger("chosen:updated");
            $('#id_Reclamation_Type').val(result.id_Reclamation_Type);
            $('#id_DevisionReclamation').val(result.id_DevisionReclamation);
            $('#id_Reclamation_CountErrorFirst').val(result.id_Reclamation_CountErrorFirst);
            $('#id_Reclamation_CountErrorFinal').val(result.id_Reclamation_CountErrorFinal);
            $('#vid_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_AspNetUsersCreate').val(result.id_AspNetUsersCreate);
            $('#id_DevisionCreate').val(result.id_DevisionCreate);
            $('#vdateTimeCreate').val(result.dateTimeCreate);
            $('#closeMKO').val(result.closeMKO);
            $('#answerText').val("");
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#timeToSearch').val(result.timeToSearch);
            $('#timeToEliminate').val(result.timeToEliminate);
            $('#close').prop('checked', result.close);
            $('#gip').prop('checked', result.gip);
            $('#closeDevision').prop('checked', result.closeDevision);
            $('#PCAM').val(result.PCAM);
            $('#answerHistiryText').val(result.answerHistiryText);
            $('#editManufacturing').prop('checked', result.editManufacturing);
            $('#editManufacturingIdDevision').val(result.editManufacturingIdDevision);
            $('#id_PF').val(result.id_PF);
            $('#technicalAdvice').prop('checked', result.technicalAdvice);
            $('#id_AspNetUsersError').val(result.id_AspNetUsersError);
            $('#reloadDevision').val("");
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

function expertComplitedAll() {
    $.ajax({
        cache: false,
        url: "/Remarks/ExpertComplitedAll",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData(document.getElementById('pageData').innerHTML);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function reclamationsPlanZakaz(id) {
    idPZ = id;
    pz(id);
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/ReclamationsPlanZakaz/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objRemarksList,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
            document.getElementById('labelNamePage').innerHTML = 'Заказ №: ' + data.OnePZName;
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
    if (closePZReclamation === 1 || closePZReclamation === '1') {
        $('#btnCloseOrder').show();
        $('#btnExpert').hide();
    }
}

function reclamationsPlanZakazMy(id) {
    idPZ = id;
    pz(id);
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/ReclamationsPlanZakazMy/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objRemarksList,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
            document.getElementById('labelNamePage').innerHTML = 'Заказ №: ' + data.OnePZName;
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
    if (closePZReclamation === 1 || closePZReclamation === '1') {
        $('#btnCloseOrder').show();
        $('#btnExpert').hide();
    }
}

function chackList(id) {
    pz(id);
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/ChackList/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[3, "desc"]],
        "processing": true,
        "columns": objRemarksListView,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
            document.getElementById('labelNamePage').innerHTML = 'Заказ №: ' + data.OnePZName;
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

function myReclamation() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/MyReclamation/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objRemarksList,
        "rowCallback": function (row, data, index) {
            if (data.Close !== "активная") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
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

var objRemarksListView = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "См", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "СП", "data": "Devision", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "Text", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Прим.", "data": "Description", "autowidth": true, "bSortable": true },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Создал", "data": "UserCreate", "autowidth": true, "bSortable": true, "class": 'colu-200' },
    { "title": "Ответственный", "data": "UserReclamation", "autowidth": true, "bSortable": true },
    { "title": "Степень ошибки", "data": "LeavelReclamation", "autowidth": true, "bSortable": true }
];

function editManufList() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "dom": 'Bfrtip',
        "buttons": [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        "ajax": {
            "cache": false,
            "url": "/Remarks/EditManufList/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[2, "desc"]],
        "processing": true,
        "columns": objRemarksListView,
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

function clearColor() {
    $('#id_DevisionReclamation').css('border-color', 'lightgrey');
    $('#id_AspNetUsersError').css('border-color', 'lightgrey');
    $('#editManufacturingIdDevision').css('border-color', 'lightgrey');
    $('#editManufacturingIdDevision').css('border-color', 'lightgrey');
    $('#pZ_PlanZakaz').css('border-color', 'lightgrey');
    $('#id_Reclamation_Type').css('border-color', 'lightgrey');
    $('#text').css('border-color', 'lightgrey');
    $('#id_PF').css('border-color', 'lightgrey');
    $('#reloadDevision').css('border-color', 'lightgrey');
}

function clearBox1() {
    $('[name="closeDevision"]:checked').prop('checked', false);
    $('[name="closeMKO"]:checked').prop('checked', false);
    $('[name="gip"]:checked').prop('checked', false);
    $('[name="SMKO"]:checked').prop('checked', false);
}

function clearBox2() {
    $('[name="close"]:checked').prop('checked', false);
    $('[name="closeMKO"]:checked').prop('checked', false);
    $('[name="gip"]:checked').prop('checked', false);
    $('[name="SMKO"]:checked').prop('checked', false);
}

function clearBox3() {
    $('[name="close"]:checked').prop('checked', false);
    $('[name="closeDevision"]:checked').prop('checked', false);
    $('[name="gip"]:checked').prop('checked', false);
    $('[name="SMKO"]:checked').prop('checked', false);
}

function clearBox4() {
    $('[name="close"]:checked').prop('checked', false);
    $('[name="closeDevision"]:checked').prop('checked', false);
    $('[name="closeMKO"]:checked').prop('checked', false);
    $('[name="SMKO"]:checked').prop('checked', false);
}

function clearBox5() {
    $('[name="close"]:checked').prop('checked', false);
    $('[name="closeDevision"]:checked').prop('checked', false);
    $('[name="closeMKO"]:checked').prop('checked', false);
    $('[name="gip"]:checked').prop('checked', false);
}

function disabledBtnCreateReport() {
    document.location.reload(true);
}

function UpdateDevisionListOTK(data) {
    var selectedDevision = $('#id_DevisionReclamation').find('option:selected').text();
    var id = $('#id_Reclamation_Type').find('option:selected').text();
    if (counterDevision === 1 || counterDevision === '1') {
        $.ajax({
            cache: false,
            url: "/Remarks/GetDevisionList/" + id,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (data) {
                var x = document.getElementById("id_DevisionReclamation");
                for (var i = x.children.length; i >= 0; i--) {
                    x.remove(i);
                }
                for (var j = 0; j < data.length; j++) {
                    var optionhtml = '<option value="' + data[j].Value + '">' + data[j].Text + '</option>';
                    $("#id_DevisionReclamation").append(optionhtml);
                }
            }
        });
    }
}

function UpdateDevisionList() {
    $.ajax({
        cache: false,
        url: "/Remarks/GetDevisionList/" + 'All',
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var x = document.getElementById("id_DevisionReclamation");
            for (var i = x.children.length; i >= 0; i--) {
                x.remove(i);
            }
            for (var j = 0; j < data.length; j++) {
                var optionhtml = '<option value="' + data[j].Value + '">' + data[j].Text + '</option>';
                $("#id_DevisionReclamation").append(optionhtml);
            }
        }
    });
}

function closeOrderOTK(id) {
    $.ajax({
        cache: false,
        url: "/Remarks/CloseOrderOTK/" + id,
        typr: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",

        success: function (result) {
            var tmp = result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function addNewRemarkOTK(id) {

}

function noClosedRemarkToExcel() {
    $('#modalNoClosedOrder').modal('show');
}