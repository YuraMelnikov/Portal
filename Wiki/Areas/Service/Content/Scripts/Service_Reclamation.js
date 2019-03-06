$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/Service_Reclamation/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td><a href="#" onclick="return getbyID(' + item.id + ')"><span class="glyphicon glyphicon-pencil"></span></a></td>';
                html += '<td>' + item.id + '</td>';
                html += '<td>';
                for (i in item.pz_Names) {
                    html += item.pz_Names[i].name + '; ';
                }
                html += '</td>';
                html += '<td>' + item.dateAdd + '</td>';
                html += '<td>' + item.description + '</td>';
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
        id_Service_TypeDocument: $('#id_Service_TypeDocument').val(),
        dateAdd: $('#dateAdd').val(),
        id_Service_ReclamationWhoAdd: $('#id_Service_ReclamationWhoAdd').val(),
        numberDocument: $('#numberDocument').val(),
        dateDocument: $('#dateDocument').val(),
        description: $('#description').val(),
        dateClose: $('#dateClose').val(),
        addMission: $('#addMission').val(),
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        service_TypeReclamation: $('#service_TypeReclamation').val()
};
    $.ajax({
        url: "/Service_Reclamation/Add",
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
        url: "/Service_Reclamation/getbyID/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#id').val(result.id);

            var html1 = '';

            for (i in result.service_ReclamationInfo) {
                html1 += '<tr>';
                html1 += '<td>' + result.service_ReclamationInfo[i].Id + '</td>';
                html1 += '<td>' + result.service_ReclamationInfo[i].TextData + '</td>';
                html1 += '</tr>';
            }
            $('#pZ_PlanZakaz').val(result.pZ_PlanZakaz); 
            $('#pZ_PlanZakaz').chosen();
            $('#pZ_PlanZakaz').trigger('chosen:updated');
            $('#service_TypeReclamation').val(result.service_TypeReclamation);
            $('#service_TypeReclamation').chosen();
            $('#service_TypeReclamation').trigger('chosen:updated');
            $('#id_Service_TypeDocument').val(result.id_Service_TypeDocument);
            $('#dateAdd').val(result.dateAdd);
            $('#id_Service_ReclamationWhoAdd').val(result.id_Service_ReclamationWhoAdd);
            $('#numberDocument').val(result.numberDocument);
            $('#dateDocument').val(result.dateDocument);
            $('#description').val(result.description);
            $('.tbodyt').html(html1);
            $('#dateClose').val(result.dateClose);
            $('#addMission').val(result.addMission);
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
        id_Service_TypeDocument: $('#id_Service_TypeDocument').val(),
        id_Service_ReclamationWhoAdd: $('#id_Service_ReclamationWhoAdd').val(),
        numberDocument: $('#numberDocument').val(),
        dateDocument: $('#dateDocument').val(),
        description: $('#description').val(),
        pZ_PlanZakaz: $('#pZ_PlanZakaz').val(),
        service_TypeReclamation: $('#service_TypeReclamation').val()
    };
    $.ajax({
        url: "/Service_Reclamation/Update",
        data: JSON.stringify(typeObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#id').val("");
            $('#id_Service_TypeDocument').val("");
            $('#id_Service_ReclamationWhoAdd').val("");
            $('#numberDocument').val("");
            $('#dateDocument').val("");
            $('#description').val("");
            $('#pZ_PlanZakaz').val("");
            $('#service_TypeReclamation').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    $('#id').val("");
    $('#id_Service_TypeDocument').val("");
    $('#id_Service_ReclamationWhoAdd').val("");
    $('#numberDocument').val("");
    $('#dateDocument').val("");
    $('#description').val("");
    $('#pZ_PlanZakaz').val("");
    $('#pZ_PlanZakaz').chosen();
    $('#pZ_PlanZakaz').trigger('chosen:updated');
    $('#service_TypeReclamation').val("");
    $('#service_TypeReclamation').chosen();
    $('#service_TypeReclamation').trigger('chosen:updated');
    $('#dateAdd').val("");
    $('#description').val("");
    $('#dateClose').val("");
    $('#addMission').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#name').css('border-color', 'lightgrey');
    $('#active').css('border-color', 'lightgrey');
}

function validate() {
    var isValid = true;
    if ($('#pZ_PlanZakaz').val().length === 0) {
        $('#pZ_PlanZakaz').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#pZ_PlanZakaz').css('border-color', 'lightgrey');
    }
    if ($('#service_TypeReclamation').val().length === 0) {
        $('#service_TypeReclamation').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#service_TypeReclamation').css('border-color', 'lightgrey');
    }
    if ($('#id_Service_TypeDocument').val().length === 0) {
        $('#id_Service_TypeDocument').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_Service_TypeDocument').css('border-color', 'lightgrey');
    }
    if ($('#id_Service_ReclamationWhoAdd').val().length === 0) {
        $('#id_Service_ReclamationWhoAdd').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#id_Service_ReclamationWhoAdd').css('border-color', 'lightgrey');
    }
    if ($('#numberDocument').val().trim() === "") {
        $('#numberDocument').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#numberDocument').css('border-color', 'lightgrey');
    }
    if ($('#dateDocument').val().trim() === "") {
        $('#dateDocument').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#dateDocument').css('border-color', 'lightgrey');
    }
    if ($('#description').val().trim() === "") {
        $('#description').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#description').css('border-color', 'lightgrey');
    }
    return isValid;
}