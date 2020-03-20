$(document).ready(function () {
    $('#pageData').hide();
    $('#intHide').hide();
    if (userGroupId !== 1) {
        $('#btnAddNewReclamations').hide();
    }
    document.getElementById('pageData').innerHTML = 1;
    startPage();
});

function loadData(listId) {
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
    { "title": "№", "data": "id", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Ред", "data": "editLink", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "См", "data": "viewLink", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "Заказ/ы", "data": "orders", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Покупатель", "data": "client", "autowidth": true, "bSortable": true },
    { "title": "Тип", "data": "types", "autowidth": true, "bSortable": true },
    { "title": "Описание", "data": "text", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Причина возникновения", "data": "causes", "autowidth": true, "bSortable": false },
    { "title": "Прим.", "data": "description", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Дата открытия", "data": "dateOpen", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Дата получения", "data": "dateGet", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Дата закрытия", "data": "dateClose", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Папка (IE)", "data": "folder", "autowidth": true, "bSortable": false }
];

var objTableRem = [
    { "title": "№", "data": "id", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Описание", "data": "text", "autowidth": true, "bSortable": false },
    { "title": "Ответственное СП", "data": "devision", "autowidth": true, "bSortable": false }
];

var objTableAnswer = [
    { "title": "Дата", "data": "dateAnswer", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Сотрудник", "data": "userAnswer", "autowidth": true, "bSortable": false },
    { "title": "Текст", "data": "textAnswer", "autowidth": true, "bSortable": false },
    { "title": "Ред", "data": "editLinkAnsw", "autowidth": true, "bSortable": false, "className": 'text-center' }
];

function startPage() {
    var id = 0;
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/ActiveList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
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
    $("#tableRem").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/RemList/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objTableRem,
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
    $("#tableAnswers").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/GetTableAnswers/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objTableAnswer,
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
    var table = $('#reclamationTable').DataTable();
    table.destroy();
    $('#reclamationTable').empty();
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/ActiveList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
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

function updateRemList(id) {
    var table = $('#tableRem').DataTable();
    table.destroy();
    $('#tableRem').empty();
    $("#tableRem").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/RemList/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "searching": false,
        "processing": true,
        "columns": objTableRem,
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
    var table = $('#reclamationTable').DataTable();
    table.destroy();
    $('#reclamationTable').empty();
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/CloseList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
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
    var table = $('#reclamationTable').DataTable();
    table.destroy();
    $('#reclamationTable').empty();
    $("#reclamationTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/AllList",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[11, "asc"]],
        "processing": true,
        "columns": objViewList,
        "rowCallback": function (row, data, index) {
            if (data.dateClose === 'null') {
                $('td', row).css('background-color', '#ebaca2');
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

function clearTextBox() {
    $("#partRem").hide();
    updateRemList(0);
    $("#btnAdd").show();
    $("#btnUpdate").hide();
    clearColor();
    clearTextBoxRem();
    $('#contacts').val("");
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
    $('#answerText').prop('disabled', true);
    $('#reclamationModal').modal('show');
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
    var objRemark = {
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        id_Reclamation_Type: $('#id_Reclamation_Type').val(),
        id_ServiceRemarksCause: $('#id_ServiceRemarksCause').val(),
        datePutToService: $('#datePutToService').val(),
        dateClose: $('#dateClose').val(),
        folder: $('#folder').val(),
        contacts: $('#contacts').val(),
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
            $('#reclamationModal').modal('hide');
            $('#reclamationTable').DataTable().ajax.reload(null, false);
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
            $('#btnRemove').show();
            $("#partRem").show();
            $("#btnUpdate").show();
            GetTableAnswers(id);
            updateRemList(id);
            $('#answerText').prop('disabled', false);
            $("#pZ_PlanZakaz").val(result.pZ_PlanZakaz).trigger("chosen:updated");
            $("#id_Reclamation_Type").val(result.id_Reclamation_Type).trigger("chosen:updated");
            $("#id_ServiceRemarksCause").val(result.id_ServiceRemarksCause).trigger("chosen:updated");
            $('#numberReclamation').val(result.numberReclamation);
            $('#dateTimeCreate').val(result.dateTimeCreate);
            $('#contacts').val(result.contacts);
            $('#userCreate').val(result.userCreate);
            $('#datePutToService').val(result.datePutToService);
            if (result.dateClose !== 'null')
                $('#dateClose').val(result.dateClose);
            else
                $('#dateClose').val("");
            $('#folder').val(result.folder);
            $('#id').val(result.id);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#answerText').val(result.answerText);
            $('#answerHistiryText').val(result.answerHistiryText);
            $('#reclamationModal').modal('show');
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function getView(id) {
    clearTextBox();
    var myVal = userGroupId;
    $.ajax({
        cache: false,
        url: "/Marks/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result.dateClose === null)
                $('#dateClose').val(result.dateClose);
            else
                $('#dateClose').val("");
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
    var res = validate();
    if (res === false) {
        return false;
    }
    var tmp = document.getElementById('id').innerHTML;
    var objRemark = {
        id: $('#id').val(),
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        id_Reclamation_Type: $('#id_Reclamation_Type').val(),
        id_ServiceRemarksCause: $('#id_ServiceRemarksCause').val(),
        datePutToService: $('#datePutToService').val(),
        dateClose: $('#dateClose').val(),
        folder: $('#folder').val(),
        contacts: $('#contacts').val(),
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
            clearTextBoxRem();
            updateRemList(result);
            $('#reclamationTable').DataTable().ajax.reload(null, false);
            $('#tableAnswers').DataTable().ajax.reload(null, false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}

function copyLink() {
    var urlPatch = $('#folder').val();
    if (window.clipboardData) { // this is for Internet Explorer
        window.clipboardData.setData("Text", urlPatch);
    }
    else { // this is for Edge, Firefox, Chrome and Safari; this also works with IE, but it does not work as smoothly as above code causing the page to jump around
        var t = document.createElement("textarea"); // create textarea element
        t.value = urlPatch; // set its value to the data to copy
        t.style.position = "absolute";
        t.style.display = "inline";
        t.style.width = t.style.height = t.style.padding = 0;
        t.setAttribute("readonly", ""); // textarea is readonly
        document.body.appendChild(t); // append the textarea element - may be better to append to the object being clicked
        t.select(); // select the data in the text area
        document.execCommand("copy"); // IMPORTANT: "copy" works as a result of user events, like "click" event
        document.body.removeChild(t); // remove the textarea element
    }
    return false;
}

function openNoCloseModal() {
    $('#btnGetExcelNoCloseReclamation').show();
    $('#npZ_PlanZakaz').val("");
    $('#npZ_PlanZakaz').chosen();
    $('#npZ_PlanZakaz').trigger('chosen:updated');
    $('#excelNoCloseReclamatonModal').modal('show');
}

function createAnClosePZ() {
    var res = validatePZList();
    if (res === false) {
        return false;
    }
    var objRemark = {
        npZ_PlanZakaz: $('#npZ_PlanZakaz').val()
    };
    $.ajax({
        cache: false,
        url: "/Marks/CreateAnClosePZ/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function () {
            $('#excelNoCloseReclamatonModal').modal('hide');
        },
        error: function () {
            $('#excelNoCloseReclamatonModal').modal('hide');
        }
    });
}

function validatePZList() {
    var isValid = true;
    if ($('#npZ_PlanZakaz').val().length === 0) {
        $('#npZ_PlanZakaz').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#npZ_PlanZakaz').css('border-color', 'lightgrey');
    }
    return isValid;
}

function clearTextBoxRem() {
    clearColorRem();
    $('#answerText').val("");
    $('#typeRem').val("");
    $('#devRem').val("");
    $('#textRem').val("");
    $('#pfRem').val("");
    $('[name="technicalAdviceRem"]:checked').prop('checked', false);
}

function clearColorRem() {
    $('#typeRem').css('border-color', 'lightgrey');
    $('#devRem').css('border-color', 'lightgrey');
    $('#textRem').css('border-color', 'lightgrey');
    $('#pfRem').css('border-color', 'lightgrey');
}

function validateRem() {
    var isValid = true;
    clearColorRem();
    if ($('#typeRem').val().trim() === "") {
        $('#typeRem').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#typeRem').css('border-color', 'lightgrey');
    }
    if ($('#devRem').val().trim() === "") {
        $('#devRem').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#devRem').css('border-color', 'lightgrey');
    }
    if ($('#textRem').val().trim() === "") {
        $('#textRem').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#textRem').css('border-color', 'lightgrey');
    }
    if ($('#pfRem').val().trim() === "") {
        $('#pfRem').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#pfRem').css('border-color', 'lightgrey');
    }
    return isValid;
}

function addNewRemarkOTK() {
    var res = validateRem();
    if (res === false) {
        return false;
    }
    var objRemark = {
        id: $('#id').val(),
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        typeRem: $('#typeRem').val(),
        devRem: $('#devRem').val(),
        textRem: $('#textRem').val(),
        pfRem: $('#pfRem').val(),
        technicalAdviceRem: $('#technicalAdviceRem').is(":checked")
    };
    $.ajax({
        cache: false,
        url: "/Marks/AddRem",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            clearTextBoxRem();
            updateRemList(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetTableAnswers(id) {
    var table = $('#tableAnswers').DataTable();
    table.destroy();
    $('#tableAnswers').empty();
    $("#tableAnswers").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Marks/GetTableAnswers/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objTableAnswer,
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

function remove() {
    var objRemove = {
        id: $('#id').val()
    };
    $.ajax(
        {
            cache: false,
            url: "/Marks/Remove/" + id,
            data: JSON.stringify(objRemove),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#reclamationTable').DataTable().ajax.reload(null, false);
                $('#btnRemove').hide();
                $('#reclamationModal').modal('hide');
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}

function getAnsw(id) {
    $('#idAnsw').val("");
    $('#textAnws').val("");
    $.ajax({
        cache: false,
        url: "/Marks/GetAnsw/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idAnsw').val(result.idAnsw);
            $('#textAnws').val(result.textAnws);
            $('#answerModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function removeAnsw() {
    var objRemove = {
        idAnsw: $('#idAnsw').val()
    };
    $.ajax(
        {
            cache: false,
            url: "/Marks/RemoveAnsw/",
            data: JSON.stringify(objRemove),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#tableAnswers').DataTable().ajax.reload(null, false);
                $('#answerModal').modal('hide');
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}

function updateAnsw() {
    var objRemark = {
        idAnsw: $('#idAnsw').val(),
        textAnws: $('#textAnws').val()
    };
    $.ajax({
        cache: false,
        url: "/Marks/UpdateAnsw/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#answerModal').modal('hide');
            $('#idAnsw').val("");
            $('#textAnws').val("");
            $('#tableAnswers').DataTable().ajax.reload(null, false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}