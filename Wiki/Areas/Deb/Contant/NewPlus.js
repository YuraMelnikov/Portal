function getbyID(Id) {
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
    $.ajax({
        cache: false,
        url: "/Upload/Get/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);
            $('#id_PZ_PlanZakaz').val(result.id_PZ_PlanZakaz);
            $('#cost').val(result.cost);
            $('#dateCreate').val(result.dateCreate);
            $('#dateGetMoney').val(result.dateGetMoney);

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
        cost: $('#cost').val(),
        dateGetMoney: $('#dateGetMoney').val()
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
            $('#cost').val("");
            $('#dateGetMoney').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
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