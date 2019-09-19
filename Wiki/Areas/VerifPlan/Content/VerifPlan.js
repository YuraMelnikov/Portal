$(document).ready(function () {
    startMenu();
});

var objList = [
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Плановый срок передачи на проверку", "data": "editLink", "autowidth": true, "bSortable": true },
    { "title": "Фактическая дата передачи на проверку", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Дата приемки изделия ОТК", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Прим. гл. инженера", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Прим. произв.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Прим. ОТК", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Прогнозная дата проверки (рук. произв.)", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Плановая дата проверки (prj)", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "Статус", "data": "editLink", "autowidth": true, "bSortable": false }
];

function startMenu() {
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListActive/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objList,
        //"rowCallback": function (row, data, index) {
        //    if (data.coefficient === 0 || data.coefficient === "0") {
        //        $('td', row).css('background-color', '#f45b5b');
        //        $('td', row).css('color', 'white');
        //        $('a', row).css('color', 'white');
        //    }
        //},
        "cache": false,
        "async": false,
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

function listActive() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListActive/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objList,
        //"rowCallback": function (row, data, index) {
        //    if (data.coefficient === 0 || data.coefficient === "0") {
        //        $('td', row).css('background-color', '#f45b5b');
        //        $('td', row).css('color', 'white');
        //        $('a', row).css('color', 'white');
        //    }
        //},
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

function listClose() {
    var table = $('#myTable').DataTable();
    table.destroy();
    $('#myTable').empty();
    $("#myTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Verific/ListClose/",
            "type": "POST",
            "datatype": "json"
        },
        "bDestroy": true,
        "order": [[1, "asc"]],
        "processing": true,
        "columns": objList,
        //"rowCallback": function (row, data, index) {
        //    if (data.coefficient === 0 || data.coefficient === "0") {
        //        $('td', row).css('background-color', '#f45b5b');
        //        $('td', row).css('color', 'white');
        //        $('a', row).css('color', 'white');
        //    }
        //},
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




function getPoint(id) {
    $.ajax({
        cache: false,
        url: "/Verific/Get/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#factDate').css('border-color', 'lightgrey');
            $('#appDate').css('border-color', 'lightgrey');
            $('#fixedDateForKO').css('border-color', 'lightgrey');
            $('#id').val("");
            $('#id_PZ_PlanZakaz').val("");
            $('#fixed').val("");
            $('#fixetFirstDate').val("");
            $('#planDate').val("");
            $('#planDescription').val("");
            $('#factDate').val("");
            $('#factDescription').val("");
            $('#appDate').val("");
            $('#appDescription').val("");
            $('#verificationDateInPrj').val("");
            $('#fixedDateForKO').val("");
            $('#id').val(result.id);
            $('#id_PZ_PlanZakaz').val(result.id_PZ_PlanZakaz);
            $('#fixed').val(result.fixed);
            $('#fixetFirstDate').val(result.fixetFirstDate);
            $('#planDate').val(result.planDate);
            $('#planDescription').val(result.planDescription);
            $('#factDate').val(result.factDate);
            $('#factDescription').val(result.factDescription);
            $('#appDate').val(result.appDate);
            $('#appDescription').val(result.appDescription);
            $('#verificationDateInPrj').val(result.verificationDateInPrj);
            $('#fixedDateForKO').val(result.fixedDateForKO);
            $('#verifModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//updateTE
//updateOTK
//updateTM


//function updatePoint() {
//    var res = valid();
//    if (res === false) {
//        return false;
//    }
//    var objPoint = {
//        ids: $('#ids').val()
//        , ks: $('#ks').val().replace(".", ",")
//    };
//    $.ajax({
//        cache: false,
//        url: "/UserKO/UpdatePoint/",
//        data: JSON.stringify(objPoint),
//        type: "POST",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            $('#myTable').DataTable().ajax.reload(null, false);
//            $('#pointModal').modal('hide');
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}

//function valid() {
//    var isValid = true;
//    if ($('#ks').val().trim() === "") {
//        $('#ks').css('border-color', 'Red');
//        isValid = false;
//    }
//    else {
//        $('#ks').css('border-color', 'lightgrey');
//    }
//    return isValid;
//}