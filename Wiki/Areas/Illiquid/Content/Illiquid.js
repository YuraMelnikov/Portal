$(document).ready(function () {
    
});

function OpeningSKUModal() {
    $('#skuModal').modal('show');
}

function LoadingSku() {
    var data = new FormData();
    var files = document.getElementById('fileSku').files;
    for (var x = 0; x < files.length; x++) {
        data.append(files[x].name, files[x]);
    }
    $.ajax({
        type: "POST",
        url: "/Illiq/LoadingSku",
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            $('#skuModal').modal('hide');
        }
    });
}