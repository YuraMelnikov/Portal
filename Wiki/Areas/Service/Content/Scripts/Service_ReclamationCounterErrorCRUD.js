$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/Service_ReclamationCounterError/List",
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
                html += '<td>' + item.count + '</td>';
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
        count: $('#count').val().replace('.', ','),
        active: $('#active').is(":checked")
    };
    $.ajax({
        url: "/Service_ReclamationCounterError/Add",
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
        url: "/Service_ReclamationCounterError/getbyID/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#name').val(result.name);
            $('#count').val(result.count);
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
        count: $('#count').val().replace('.', ','),
        active: $('#active').is(":checked")
    };
    $.ajax({
        url: "/Service_ReclamationCounterError/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#id').val("");
            $('#name').val("");
            $('#count').val("");
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
    $('#count').val("");
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

    if ($('#count').val().trim() === "") {
        $('#count').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#count').css('border-color', 'lightgrey');
    }
    if ($('#count').val().replace(/\s/g, '').length === 0 || isNaN($('#count').val())) {
        $('#count').css('border-color', 'Red');
        isValid = false;
    }
    
    return isValid;
}