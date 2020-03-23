var colorStackLabels = '#3fb0ac';
var colorFactData = '#3fb0ac';
var titleDiagrammColor = '#000';
var titleFontSize = "14px";

$(document).ready(function () {
    getPeriodReport();
});

function getPeriodReport() {
    $.ajax({
        url: "/BP/GetPeriodReport/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("periodReportString").textContent = result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetGeneralD() {
    $.ajax({
        url: "/DashboardDD/GetGeneralD/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {


            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }


            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('generalD', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'Издержки заказов (тыс. / привязка к тр-там производства)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    name: 'ХСС',
                    data: dataArray,
                    color: colorFactData
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                color: colorStackLabels
                            }
                        }
                    }
                }
            });
        }
    });
}