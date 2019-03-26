$(document).ready(function () {

});

function getbyID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/Upload/GetbyID/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#mdateGetMoney').val(result.dateGetMoney);
            $('#mcost').val(result.cost);
            $('#editUploadCost').modal('show');
            $('#btnUpdate').show();
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
        mcost: $('#mcost').val(),
        mdateGetMoney: $('#mdateGetMoney').val()
    };
    $.ajax({
        url: "/Upload/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#editUploadCost').modal('hide');
            $('#id').val("");
            $('#mcost').val("");
            $('#mdateGetMoney').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });
    Refresh();
}

function validate() {
    var isValid = true;
    if ($('#cost').val() > 0) {
        $('#cost').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#cost').css('border-color', 'lightgrey');
    }
    
    return isValid;
}

function Refresh() {
    window.parent.location = window.parent.location.href;
}