// 01 - Admin
// 02 - Users
// 00 - All

$(document).ready(function () {
    StartMenu();
    if (userGroupId === 0) {
        $('#dComponentData').hide();
        $('#dModelData').hide();
        $('#dSymbolData').hide();
    }
});

var objComponent = [
    { "title": "!", "data": "editLink", "autowidth": true, "bSortable": true },
    //{ "title": "ID", "data": "ID", "autowidth": true, "bSortable": false },
    { "title": "ENTRY", "data": "ENTRY", "autowidth": true, "bSortable": true },
    { "title": "VERSION", "data": "VERSION", "autowidth": true, "bSortable": false },
    { "title": "DeviceLetterCode", "data": "DeviceLetterCode", "autowidth": false, "bSortable": true },
    { "title": "Class", "data": "Class", "autowidth": true, "bSortable": false },
    { "title": "ENTRYTYP", "data": "ENTRYTYP", "autowidth": true, "bSortable": false },
    { "title": "Что изменено", "data": "note", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "Description", "data": "Description", "autowidth": true, "bSortable": false },
    { "title": "LPNTR", "data": "LPNTR", "autowidth": true, "bSortable": false },
    { "title": "VSTATUS", "data": "VSTATUS", "autowidth": true, "bSortable": false },
    { "title": "VersionText", "data": "VersionText", "autowidth": true, "bSortable": false },
    { "title": "DATEMAIN", "data": "DATEMAIN", "autowidth": true, "bSortable": false },
    { "title": "LASTUPDT", "data": "LASTUPDT", "autowidth": true, "bSortable": false },
    { "title": "MPNTR", "data": "MPNTR", "autowidth": true, "bSortable": false },
    { "title": "Flags", "data": "Flags", "autowidth": true, "bSortable": false },
    { "title": "ArticleNumber", "data": "ArticleNumber", "autowidth": true, "bSortable": false },
    { "title": "Supplier", "data": "Supplier", "autowidth": true, "bSortable": false },
    { "title": "SPNTR", "data": "SPNTR", "autowidth": true, "bSortable": false },
    { "title": "Class_main", "data": "Class_main", "autowidth": true, "bSortable": false },
    { "title": "1c7", "data": "katek_1C", "autowidth": true, "bSortable": false },
    { "title": "Тип РКД", "data": "katek_type_rkd", "autowidth": true, "bSortable": false },
    { "title": "Имя РКД", "data": "katek_name_rkd", "autowidth": true, "bSortable": false },
    { "title": "Уд.", "data": "removeLink", "autowidth": true, "bSortable": false }
];

var objModel = [
    { "title": "!", "data": "editLink", "autowidth": true, "bSortable": true },
    //{ "title": "ID", "data": "ID", "autowidth": true, "bSortable": false },
    { "title": "Entry", "data": "Entry", "autowidth": true, "bSortable": true },
    { "title": "Type", "data": "Type", "autowidth": true, "bSortable": false },
    { "title": "Class", "data": "Class", "autowidth": true, "bSortable": false },
    { "title": "Description", "data": "Description", "autowidth": true, "bSortable": false },
    { "title": "Что изменено", "data": "note", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "LASTUPDT", "data": "LASTUPDT", "autowidth": true, "bSortable": false },
    { "title": "DATEMAIN", "data": "DATEMAIN", "autowidth": true, "bSortable": false },
    { "title": "Уд.", "data": "removeLink", "autowidth": true, "bSortable": false }
];

var objSymbol = [
    { "title": "!", "data": "editLink", "autowidth": true, "bSortable": true },
    //{ "title": "ID", "data": "ID", "autowidth": true, "bSortable": false },
    { "title": "ENTRY", "data": "ENTRY", "autowidth": true, "bSortable": true },
    { "title": "VERSION", "data": "VERSION", "autowidth": true, "bSortable": false },
    { "title": "Norm", "data": "Norm", "autowidth": true, "bSortable": false },
    { "title": "Что изменено", "data": "note", "autowidth": true, "bSortable": false, "class": 'colu-200' },
    { "title": "LSHDES", "data": "LSHDES", "autowidth": true, "bSortable": false },
    { "title": "LSHTYP", "data": "LSHTYP", "autowidth": true, "bSortable": false },
    { "title": "ENTRYTYP", "data": "ENTRYTYP", "autowidth": true, "bSortable": false },
    { "title": "LPNTR", "data": "LPNTR", "autowidth": true, "bSortable": false },
    { "title": "VSTATUS", "data": "VSTATUS", "autowidth": true, "bSortable": false },
    { "title": "VersionText", "data": "VersionText", "autowidth": true, "bSortable": false },
    { "title": "DATEMAIN", "data": "DATEMAIN", "autowidth": true, "bSortable": false },
    { "title": "LASTUPDT", "data": "LASTUPDT", "autowidth": true, "bSortable": false },
    { "title": "Class", "data": "Class", "autowidth": true, "bSortable": false },
    { "title": "Description", "data": "Description", "autowidth": true, "bSortable": false },
    { "title": "SYMINDEX", "data": "SYMINDEX", "autowidth": true, "bSortable": false },
    { "title": "FLAGS2", "data": "FLAGS2", "autowidth": true, "bSortable": false },
    { "title": "FLAGS", "data": "FLAGS", "autowidth": true, "bSortable": false },
    { "title": "Уд.", "data": "removeLink", "autowidth": true, "bSortable": false }
];

function StartMenu() {
    $("#tableComponentData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/E3Series/GetTableComponentData",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objComponent,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#tableModelData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/E3Series/GetTableModelData",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objModel,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("#tableSymbolData").DataTable({
        "ajax": {
            "cache": false,
            "url": "/E3Series/GetTableSymbolData",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "desc"]],
        "processing": true,
        "columns": objSymbol,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
}

function PostModel(ID) {
    $.ajax({
        cache: false,
        url: "/E3Series/PostModel/" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
        }
    });
}

function RemoveModel(ID) {

}

function PostComponent(ID) {
    $.ajax({
        cache: false,
        url: "/E3Series/PostComponent/" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
        }
    });
}

function RemoveComponent(ID) {

}

function PostSymbol(ID) {
    $.ajax({
        cache: false,
        url: "/E3Series/PostSymbol/" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
        }
    });
}

function RemoveSymbol(ID) {

}