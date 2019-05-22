$(document).ready(function () {
    startMenu();
    $('#pageData').hide();
});

function loadData(listId) {
    document.getElementById('pageData').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        activeTA();
    }
    else if (listId === 2 || listId === "2") {
        protocols();
    }
    else if (listId === 3 || listId === "3") {
        allDataProtocols();
    }
    else {
        activeTA();
    }
}

var objRemarksList = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "Ред", "data": "LinkToEdit", "autowidth": true, "bSortable": false },
    { "title": "См", "data": "LinkToView", "autowidth": true, "bSortable": false },
    { "title": "Заказ", "data": "Orders", "autowidth": true, "bSortable": false },
    { "title": "Описание", "data": "TextReclamation", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Ответ/ы", "data": "Answers", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Решение", "data": "Decision", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Прим.", "data": "DescriptionReclamation", "autowidth": true, "bSortable": false },
    { "title": "Направил на ТС", "data": "UserToTA", "autowidth": true, "bSortable": true },
    { "title": "Создал", "data": "UserCreate", "autowidth": true, "bSortable": true },
    { "title": "Ответственное СП", "data": "DevisionReclamation", "autowidth": true, "bSortable": true }
];

var objProtocol = [
    { "title": "№", "data": "Id_Reclamation", "autowidth": true, "bSortable": true },
    { "title": "См", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Word", "data": "ViewLinkJS", "autowidth": true, "bSortable": false },
    { "title": "Дата заседания", "data": "PlanZakaz", "autowidth": true, "bSortable": true },
    { "title": "Кол-во рекламаций", "data": "PlanZakaz", "autowidth": true, "bSortable": true }
];

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetActiveTA",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "rowCallback": function (row, data, index) {
            if (data.Decision === "") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
        },
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

function activeTA() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetActiveTA",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "rowCallback": function (row, data, index) {
            if (data.Decision === "") {
                $('td', row).css('background-color', '#d9534f');
                $('td', row).css('color', 'white');
            }
        },
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

function protocols() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetProtocols",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "processing": true,
        "columns": objProtocol,
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

function allDataProtocols() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/TechnicalAdvice/GetAllRemarks",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
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

function getTAEdit(id) {
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/GetTA/" + id,
        typr: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            disabledFalse();
            $('#id').val(result.id);
            $('#userUploadReclamation').val(result.userUploadReclamation);
            $('#text').val(result.text);
            $('#description').val(result.description);
            $('#orders').val(result.orders);
            $('#userCreateReclamation').val(result.userCreateReclamation);
            $('#devisionReclamation').val(result.devisionReclamation);
            $('#reclamationText').val(result.reclamationText);
            $('#answerHistiryText').val(result.answerHistiryText);
            $('#viewReclamation').modal('show');
            $('#btnUpdate').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function getTAView(id) {
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/GetTA/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            disabledTrue();
            $('#id').val(result.Reclamation_TechnicalAdvice.id);
            $('#userUploadReclamation').val(result.Reclamation_TechnicalAdvice.AspNetUsers.CiliricalName);
            $('#text').val(result.Reclamation_TechnicalAdvice.text);
            $('#description').val(result.Reclamation_TechnicalAdvice.description);
            $('#orders').val(result.ReclamationViwers.PlanZakaz);
            $('#userCreateReclamation').val(result.ReclamationViwers.UserCreate);
            $('#devisionReclamation').val(result.ReclamationViwers.Devision);
            $('#reclamationText').val(result.ReclamationViwers.Text);
            $('#answerHistiryText').val(result.ReclamationViwers.Answers);
            $('#viewReclamation').modal('show');
            $('#btnUpdate').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function downloadProtocol(id) {

}

function getProtocol(id) {

}

function createNewProtocol() {

}

function update() {
    var objRemark = {
        id: $('#id').val(),
        text: $('#text').val(),
        description: $('#description').val()
    };
    $.ajax({
        cache: false,
        url: "/TechnicalAdvice/Update/",
        data: JSON.stringify(objRemark),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#viewReclamation').modal('hide');
            loadData(document.getElementById('pageData').innerHTML);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function disabledFalse() {
    $('#text').prop('disabled', false);
    $('#description').prop('disabled', false);
    $('#id_AspNetUsersCorrect').prop('disabled', false);
    $('#dateCorrect').prop('disabled', false);
}

function disabledTrue() {
    $('#text').prop('disabled', true);
    $('#description').prop('disabled', true);
    $('#id_AspNetUsersCorrect').prop('disabled', true);
    $('#dateCorrect').prop('disabled', true);
}