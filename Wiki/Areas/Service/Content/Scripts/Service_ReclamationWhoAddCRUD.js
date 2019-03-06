$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/Service_ReclamationWhoAdd/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td><a href="#" onclick="return getbyID(' + item.id + ')"><span class="glyphicon glyphicon-pencil"></span></a></td>';
                html += '<td>' + item.id + '</td>';
                html += '<td>' + item.name + '</td>';
                if (item.active === true)
                    html += '<td>' + 'Да' + '</td>';
                else
                    html += '<td>' + 'Нет' + '</td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Add() {
    var res = validate();
    if (res === false) {
        return false;
    }
    var typeObj = {
        id: $('#id').val(),
        name: $('#name').val(),
        active: $('#active').is(":checked")
    };
    $.ajax({
        url: "/Service_ReclamationWhoAdd/Add",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID(id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Service_ReclamationWhoAdd/getbyID/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#name').val(result.name);
            $('#active').prop('checked', result.active);
            $('#myModal').modal('show');
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
    var typeObj = {
        id: $('#id').val(),
        name: $('#name').val(),
        active: $('#active').is(":checked")
    };
    $.ajax({
        url: "/Service_ReclamationWhoAdd/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#id').val("");
            $('#name').val("");
            $('#active').is(":checked");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    $('#id').val("");
    $('#name').val("");
    $('#active').prop('checked', true);
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function validate() {
    var isValid = true;
    if ($('#name').val().trim() === "") {
        $('#name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#name').css('border-color', 'lightgrey');
    }
    return isValid;
}