var colorStackLabels = '#3fb0ac';
var colorFactData = '#3fb0ac';
var titleDiagrammColor = '#000';
var titleFontSize = "14px";
var monthArray = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

$(document).ready(function () {
    getPeriodReport();
    GetGeneralD();
    GetPF();
});

function getPeriodReport() {
    $.ajax({
        url: "/BP/GetPeriodReport/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("periodReportString").textContent = result;
        }
    });
}

function GetGeneralD() {
    var generalLenghtArray;
    var generalYearArray;
    var generalThreeYearsArray;
    var generalRateArray;
    var firstYearName;
    var secondYearName;
    var threeYearName;
    var generalMonthArray;
    var generalSSMArray;
    var generalSSWArray;
    var generalIKArray;
    var generalPKArray;
    var generalPIArray;
    var generalProfitArray;
    var generalUnRate;
    generalMonthArray = new Array();
    generalYearArray = new Array();
    generalRateArray = new Array();
    generalSSMArray = new Array();
    generalSSWArray = new Array();
    generalIKArray = new Array();
    generalPKArray = new Array();
    generalPIArray = new Array();
    generalProfitArray = new Array();
    generalUnRate = new Array();
    generalMonthNum = new Array();
    $.ajax({
        url: "/DashboardDD/GetGeneralD/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var quartArray = new Array();
            var quartRateArray = new Array();
            var countQuaRate = 0;
            var thisQua = "";
            generalLenghtArray = Object.keys(result).length;
            for (var i = 0; i < generalLenghtArray; i++) {
                generalMonthArray[i] = result[i].Month;
                generalYearArray[i] = result[i].Year;
                generalRateArray[i] = result[i].Rate;
                generalSSMArray[i] = result[i].SSM;
                generalSSWArray[i] = result[i].SSW;
                generalIKArray[i] = result[i].IK;
                generalPKArray[i] = result[i].PK;
                generalPIArray[i] = result[i].PI;
                generalProfitArray[i] = result[i].Profit;
                generalUnRate[i] = result[i].SSM + result[i].SSW + result[i].IK + result[i].IK + result[i].PK + result[i].PI;
                generalMonthNum[i] = result[i].MonthNum;
                if (thisQua !== result[i].Quart) {
                    quartArray.push(result[i].Quart);
                    thisQua = result[i].Quart;
                    if (thisQua !== "") {
                        quartRateArray.push(countQuaRate);
                        countQuaRate = 0;
                        countQuaRate += result[i].Profit;
                    }
                    else {
                        countQuaRate += result[i].Profit;
                    }
                }
                else {
                    countQuaRate += result[i].Profit;
                }
            }
            quartRateArray.push(countQuaRate);
            quartRateArray.shift();
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('generalD', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
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
                    categories: generalMonthArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [
                    {
                        name: 'C/C мат.',
                        data: generalSSMArray,
                        color: "#910000"
                    },
                    {
                        name: 'С/С з/п ПО',
                        data: generalSSWArray
                    },
                    {
                        name: '% по кредит.',
                        data: generalPKArray
                    },
                    {
                        name: 'ПИ',
                        data: generalPIArray
                    },
                    {
                        name: 'Ком. изд.',
                        data: generalIKArray
                    },
                    {
                        name: 'Издержки',
                        data: generalUnRate
                    },
                    {
                        name: 'Выручка',
                        data: generalRateArray,
                        color: colorFactData
                    },
                    {
                        name: 'НОП',
                        data: generalProfitArray
                    }
                ],
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
            var firstYearRate;
            var secondYearRate;
            var threeYearRate;
            firstYearRate = new Array();
            secondYearRate = new Array();
            threeYearRate = new Array();
            var firstSProfitArray = new Array();
            var secondSProfitArray = new Array();
            var threeSProfitArray = new Array();
            var countSFirst = 0;
            var countSSecond = 0;
            var countSThree = 0;
            var tmpTear = "";
            generalThreeYearsArray = new Array();
            for (var k = 0; k < generalLenghtArray; k++) {
                if (generalYearArray[k] !== tmpTear) {
                    tmpTear = generalYearArray[k];
                    generalThreeYearsArray.push(generalYearArray[k]);
                }
            }
            firstYearName = generalThreeYearsArray[0];
            secondYearName = generalThreeYearsArray[1];
            threeYearName = generalThreeYearsArray[2];
            for (var j = 0; j < generalLenghtArray; j++) {
                if (generalYearArray[j] === firstYearName) {
                    firstYearRate.push(generalProfitArray[j]);
                    countSFirst += generalProfitArray[j];
                    firstSProfitArray.push(countSFirst);
                }
                else if (generalYearArray[j] === secondYearName) {
                    secondYearRate.push(generalProfitArray[j]);
                    countSSecond += generalProfitArray[j];
                    secondSProfitArray.push(countSSecond);
                }
                else {
                    threeYearRate.push(generalProfitArray[j]);
                    countSThree += generalProfitArray[j];
                    threeSProfitArray.push(countSThree);
                }
            }
            Highcharts.chart('rateToMonth', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Прибыль по месяцам (тыс. / привязка к тр-там производства)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [
                    {
                        name: firstYearName,
                        data: firstYearRate
                    },
                    {
                        name: secondYearName,
                        data: secondYearRate
                    },
                    {
                        name: threeYearName,
                        data: threeYearRate
                    }
                ],
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
            Highcharts.chart('rateQuart', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Прибыль квартальная (тыс. / привязка к тр-там производства)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: quartArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [
                    {
                        name: 'Прибыль',
                        data: quartRateArray
                    }
                ],
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
            Highcharts.chart('sumRateToYear', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: 'Прибыль с накоплением (тыс. / привязка к тр-там производства)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [
                    {
                        name: firstYearName,
                        data: firstSProfitArray
                    },
                    {
                        name: secondYearName,
                        data: secondSProfitArray
                    },
                    {
                        name: threeYearName,
                        data: threeSProfitArray
                    }
                ],
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

function GetPF() {
    $.ajax({
        url: "/DashboardDD/GetPF/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var monthArray = new Array();
            var pSSWArray = new Array();
            var fSSWArray = new Array();

            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('mSSW', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'з/п производства (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: generalMonthArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [
                    {
                        name: 'C/C мат.',
                        data: generalSSMArray,
                        color: "#910000"
                    },
                    {
                        name: 'С/С з/п ПО',
                        data: generalSSWArray
                    },
                    {
                        name: '% по кредит.',
                        data: generalPKArray
                    },
                    {
                        name: 'ПИ',
                        data: generalPIArray
                    },
                    {
                        name: 'Ком. изд.',
                        data: generalIKArray
                    },
                    {
                        name: 'Издержки',
                        data: generalUnRate
                    },
                    {
                        name: 'Выручка',
                        data: generalRateArray,
                        color: colorFactData
                    },
                    {
                        name: 'НОП',
                        data: generalProfitArray
                    }
                ],
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