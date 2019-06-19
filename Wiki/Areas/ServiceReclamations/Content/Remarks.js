$(document).ready(function () {
    if (userGroupId !== 1) {
        $('#btnAddNewReclamations').hide();
    }
    activeReclamation();
});

function loadData(listId) {
    clearTextBox();
    document.getElementById('pageData').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        activeReclamation();
    }
    else if (listId === 2 || listId === "2") {
        closeReclamation();
    }
    else if (listId === 3 || listId === "3") {
        allReclamation();
    }
}

var objViewList = [
    { "title": "№", "data": "id", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "См", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа/ов", "data": "orders", "autowidth": true, "bSortable": true },
    { "title": "Покупатель", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Тип", "data": "types", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "text", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Причина возникновения", "data": "causes", "autowidth": true, "bSortable": false },
    { "title": "Прим.", "data": "description", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Дата открытия", "data": "dateOpen", "autowidth": true, "bSortable": true },
    { "title": "Дата получения", "data": "dateGet", "autowidth": true, "bSortable": true },
    { "title": "Дата закрытия", "data": "dateClose", "autowidth": true, "bSortable": true },
    { "title": "Папка (IE)", "data": "folder", "autowidth": true, "bSortable": false }
];

function activeReclamation() {
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/ActiveList",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objViewList,
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
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/CloseList",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objViewList,
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
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/AllList",
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": objViewList,
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

function clearTextBox() {
    $("#btnAdd").attr('disabled', false);
    $("#btnAdd").show();
    $("#btnUpdate").hide();
    clearColor();
    $('#numberReclamation').val("");
    $('#dateTimeCreate').val("");
    $('#userCreate').val("");
    $('#datePutToService').val("");
    $('#dateClose').val("");
    $('#folder').val("");
    $('#text').val("");
    $('#description').val("");
    $('#answerText').val("");
    $('#answerHistiryText').val("");
    $('#pZ_PlanZakaz').val("");
    $('#pZ_PlanZakaz').chosen();
    $('#pZ_PlanZakaz').trigger('chosen:updated');
    $('#id_Reclamation_Type').val("");
    $('#id_Reclamation_Type').chosen();
    $('#id_Reclamation_Type').trigger('chosen:updated');
    $('#id_ServiceRemarksCause').val("");
    $('#id_ServiceRemarksCause').chosen();
    $('#id_ServiceRemarksCause').trigger('chosen:updated');
    $("#reclamationModal").show();
}

function clearColor() {
    $('#datePutToService').css('border-color', 'lightgrey');
    $('#text').css('border-color', 'lightgrey');
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
        id_ServiceRemarksCause: $('#id_ServiceRemarksCause').val(),
        datePutToService: $('#datePutToService').val(),
        dateClose: $('#dateClose').val(),
        folder: $('#folder').val(),
        text: $('#text').val(),
        description: $('#description').val()
    };
    $.ajax({
        cache: false,
        url: "/Marks/Add",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData(document.getElementById('pageData').innerHTML);
            $('#reclamationModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    clearColor();
    var isValid = true;
    if ($('#pZ_PlanZakaz').val().length === 0) {
        $('#pZ_PlanZakaz').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#pZ_PlanZakaz').css('border-color', 'lightgrey');
    }
    if ($('#id_Reclamation_Type').val().length === 0) {
        $('#id_Reclamation_Type').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_Reclamation_Type').css('border-color', 'lightgrey');
    }
    if ($('#id_ServiceRemarksCause').val().length === 0) {
        $('#id_ServiceRemarksCause').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_ServiceRemarksCause').css('border-color', 'lightgrey');
    }

    if ($('#datePutToService').val().trim() === "") {
        $('#datePutToService').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#datePutToService').css('border-color', 'lightgrey');
    }
    if ($('#text').val().trim() === "") {
        $('#text').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#text').css('border-color', 'lightgrey');
    }
    return isValid;
}

function get(id) {
    clearTextBox();
    var myVal = userGroupId;
    $.ajax({
        cache: false,
        url: "/Marks/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $("#pZ_PlanZakaz").val(result.pZ_PlanZakaz).trigger("chosen:updated");
            $("#id_Reclamation_Type").val(result.id_Reclamation_Type).trigger("chosen:updated");
            $("#id_ServiceRemarksCause").val(result.id_ServiceRemarksCause).trigger("chosen:updated");
            $('#numberReclamation').val(result.numberReclamation);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#userCreate').val(result.userCreate);
            $('#datePutToService').val(result.datePutToService);
            $('#dateClose').val(result.dateClose);
            $('#folder').val(result.folder);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#answerText').val(result.answerText);
            $('#answerHistiryText').val(result.answerHistiryText);
            $('#reclamationModal').modal('show');
            if (userGroupId === 1 || userGroupId === '1')
                $('#btnUpdate').show();
            else
                $('#btnUpdate').hide();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function getView(id){
    clearTextBox();
    var myVal = userGroupId;
    $.ajax({
        cache: false,
        url: "/Marks/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $("#pZ_PlanZakaz").val(result.pZ_PlanZakaz).trigger("chosen:updated");
            $("#id_Reclamation_Type").val(result.id_Reclamation_Type).trigger("chosen:updated");
            $("#id_ServiceRemarksCause").val(result.id_ServiceRemarksCause).trigger("chosen:updated");
            $('#numberReclamation').val(result.numberReclamation);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#userCreate').val(result.userCreate);
            $('#datePutToService').val(result.datePutToService);
            $('#dateClose').val(result.dateClose);
            $('#folder').val(result.folder);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#answerText').val(result.answerText);
            $('#answerHistiryText').val(result.answerHistiryText);
            $('#reclamationModal').modal('show');
            $('#btnUpdate').hide();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function update() {
    var objRemark = {
        id: $('#id').val(),
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        id_Reclamation_Type: $('#id_Reclamation_Type').val(),
        id_ServiceRemarksCause: $('#id_ServiceRemarksCause').val(),
        datePutToService: $('#datePutToService').val(),
        dateClose: $('#dateClose').val(),
        folder: $('#folder').val(),
        text: $('#text').val(),
        description: $('#description').val(),
        answerText: $('#answerText').val()
    };
    $.ajax({
        cache: false,
        url: "/Marks/Update/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#reclamationModal').modal('hide');
            loadData(document.getElementById('pageData').innerHTML);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}