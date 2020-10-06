var colorStackLabels = '#000';
var colorFactData = '#000';
var titleDiagrammColor = '#000'; 
var titleFontSize = "12px";
var monthArray = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

var colorRate = '#DA291CFF';
var colorSSM = "#56A8CBFF";
var colorMin = "#d68900";
var colorProfit = "#2BAE66FF";
var colorSSW = "#05DFD7";
var colorPK = "#EC7373"; 
var colorPI = "#D8B5B5";
var colorIK = "#FFE196";

var colorFirstYear = "#56A8CBFF";
var colorSecondYear = "#DA291CFF"; 
var colorThreeYear = "#53A567FF";

var colorOneLenght = "#05DFD7";

var colorDiff = '#4a47a3';

$(document).ready(function () {
    getPeriodReport();
    GetGeneralD();
    GetPF();
    GetCustomerData();
    GetN();
    GetNLast120();
    GetDSVN();
    GetNCustomer();
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
                generalRateArray[i] = result[i].Rate / 1000;
                generalSSMArray[i] = result[i].SSM / 1000;
                generalSSWArray[i] = result[i].SSW / 1000;
                generalIKArray[i] = result[i].IK / 1000;
                generalPKArray[i] = result[i].PK / 1000;
                generalPIArray[i] = result[i].PI / 1000;
                generalProfitArray[i] = result[i].Profit / 1000;
                generalUnRate[i] = (result[i].SSM / 1000) + (result[i].SSW / 1000) + (result[i].IK / 1000) + (result[i].PK / 1000) + (result[i].PI / 1000);
                generalMonthNum[i] = result[i].MonthNum;
                if (thisQua !== result[i].Quart) {
                    quartArray.push(result[i].Quart);
                    thisQua = result[i].Quart;
                    if (thisQua !== "") {
                        quartRateArray.push(countQuaRate / 1000);
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
            quartRateArray.push(countQuaRate / 1000);
            quartRateArray.shift();
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });  

            var ismouseover = false;
            const mouseOver = function () {
                if (!ismouseover) {
                    this.chart.series.forEach(function (s) {
                        s.update({
                            dataLabels: {
                                allowOverlap: true,
                            }
                        }, true, false, false);
                    });
                    ismouseover = true;
                }
            };

            const mouseOut = function () {
                this.chart.series.forEach(function (s) {
                    s.setState('');
                    s.update({
                        dataLabels: {
                            allowOverlap: false,
                        }
                    }, true, false, false);
                });
                ismouseover = false;
            }

            Highcharts.chart('generalD', { 
                title: {
                    text: 'Издержки заказов (тыс. / привязка к тр-там производства)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                chart: {
                    styledMode: false
                }, 
                legend: {
                    itemEvents: {
                        mouseover: mouseOver,
                        mouseout: mouseOut
                    }
                },
                xAxis: {
                    categories: generalMonthArray,
                    style: {
                        width: '100px'
                    }
                },
                tooltip: {
                    pointFormat: "{point.y:,.3f}"
                },
                series: [
                    {
                        name: 'C/C мат.',
                        data: generalSSMArray,
                        color: '#56A8CBFF'
                    },
                    {
                        name: 'С/С з/п ПО',
                        data: generalSSWArray,
                        color: colorSSW
                    },
                    {
                        name: '% по кредит.',
                        data: generalPKArray,
                        color: colorPK,
                        marker: {
                            enabled: true
                        }
                    },
                    {
                        name: 'ПИ',
                        data: generalPIArray,
                        color: colorPI
                    },
                    {
                        name: 'Ком. изд.',
                        data: generalIKArray,
                        color: colorIK
                    },
                    {
                        name: 'Издержки',
                        data: generalUnRate,
                        color: colorSecondYear
                    },
                    {
                        name: 'Выручка',
                        data: generalRateArray,
                        color: '#53A567FF'
                    },
                    {
                        name: 'НОП',
                        data: generalProfitArray,
                        color: colorProfit
                    }
                ],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true,
                            allowOverlap: false,
                            format: "{point.y:,.0f}"
                        },
                        enableMouseTracking: true
                    },
                    series: {
                        stickyTracking: false,
                        events: {
                            mouseOver: mouseOver,
                            mouseOut: mouseOut
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
                title: {
                    text: 'Прибыль по месяцам (тыс. / привязка к тр-там производства)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                tooltip: {
                    pointFormat: "{point.y:,.3f}"
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
                        data: firstYearRate,
                        color: colorFirstYear
                    },
                    {
                        name: secondYearName,
                        data: secondYearRate,
                        color: colorSecondYear
                    },
                    {
                        name: threeYearName,
                        data: threeYearRate,
                        color: colorThreeYear
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
                            format: "{point.y:,.0f}",  
                            style: {
                                color: colorStackLabels
                            }
                        }
                    }
                }
            });
            Highcharts.chart('rateQuart', {
                title: {
                    text: 'Прибыль квартальная (тыс. / привязка к тр-там производства)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                tooltip: {
                    pointFormat: "{point.y:,.3f}"
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
                        data: quartRateArray,
                        color: colorThreeYear
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
                            format: "{point.y:,.0f}",  
                            style: {
                                color: colorStackLabels
                            }
                        }
                    }
                }
            });
            Highcharts.chart('sumRateToYear', {
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
                        data: firstSProfitArray,
                        color: colorFirstYear
                    },
                    {
                        name: secondYearName,
                        data: secondSProfitArray,
                        color: colorSecondYear
                    },
                    {
                        name: threeYearName,
                        data: threeSProfitArray,
                        color: colorThreeYear
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
                            format: "{point.y:,.0f}",  
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
            var lenghtResult = Object.keys(result).length;
            var date = new Date();
            var tYear = date.getFullYear();
            var lYear = tYear - 1;
            var monthArray = new Array();
            var pSSWArray = new Array();
            var fSSWArray = new Array();
            var pPKArray = new Array();
            var fPKArray = new Array();
            var pPIArray = new Array();
            var fPIArray = new Array();
            var pIKArray = new Array();
            var fIKArray = new Array();
            var pSSMArray = new Array();
            var fSSMArray = new Array();
            var fS1Array = new Array();
            var fS11Array = new Array();
            var fS12Array = new Array();
            var fS2Array = new Array();
            var psSSWArray = new Array();
            var fsSSWArray = new Array();
            var psPKArray = new Array();
            var fsPKArray = new Array();
            var psPIArray = new Array();
            var fsPIArray = new Array();
            var psIKArray = new Array();
            var fsIKArray = new Array();
            var psSSMArray = new Array();
            var fsSSMArray = new Array();
            var fsS1Array = new Array();
            var fsS11Array = new Array();
            var fsS12Array = new Array();
            var fsS2Array = new Array();
            var switchBack = 0;
            var tpsSSWArray = new Array();
            var tfsSSWArray = new Array();
            var tpsPKArray = new Array();
            var tfsPKArray = new Array();
            var tpsPIArray = new Array();
            var tfsPIArray = new Array();
            var tpsIKArray = new Array();
            var tfsIKArray = new Array();
            var tpsSSMArray = new Array();
            var tfsSSMArray = new Array();
            var tfsS1Array = new Array();
            var tfsS11Array = new Array();
            var tfsS12Array = new Array();
            var tfsS2Array = new Array();
            var tyMonthArray = new Array();
            var lyMonthArray = new Array();  

            var tspsSSW = 0;
            var tsfsSSW = 0;
            var tsosSSW = 0;
            var tspsPK = 0;
            var tsfsPK = 0;
            var tsosPK = 0;
            var tspsPI = 0;
            var tsfsPI = 0;
            var tsosPI = 0;
            var tspsIK = 0;
            var tsfsIK = 0;
            var tsosIK = 0;
            var tspsSSM = 0;
            var tsfsSSM = 0;
            var tsosSSM = 0;
            var tsfsS1 = 0;
            var tsfsS11 = 0;
            var tsfsS12 = 0;
            var tsfsS2 = 0;
            var lspsSSW = 0;
            var lsfsSSW = 0.0;
            var lsosSSW = 0;
            var lspsPK = 0;
            var lsfsPK = 0;
            var lsosPK = 0;
            var lspsPI = 0;
            var lsfsPI = 0;
            var lsosPI = 0;
            var lspsIK = 0;
            var lsfsIK = 0;
            var lsosIK = 0;
            var lspsSSM = 0;
            var lsfsSSM = 0;
            var lsosSSM = 0;
            var lsfsS1 = 0;
            var lsfsS11 = 0;
            var lsfsS12 = 0;
            var lsfsS2 = 0;
            var psSSW = 0;
            var fsSSW = 0;
            var psPK = 0;
            var fsPK = 0;
            var psPI = 0;
            var fsPI = 0;
            var psIK = 0;
            var fsIK = 0;
            var psSSM = 0;
            var fsSSM = 0;
            var fsS1 = 0;
            var fsS11 = 0;
            var fsS12 = 0;
            var fsS2 = 0;
            var pFullty = 0;
            var fFullty = 0;
            var pFullly = 0;
            var fFullly = 0;

            for (var i = 0; i < lenghtResult; i++) {
                monthArray.push(result[i].Month);
                pSSWArray.push(Math.round(result[i].PSSW));
                fSSWArray.push(Math.round(result[i].FSSW));
                pPKArray.push(Math.round(result[i].PPK));
                fPKArray.push(Math.round(result[i].FPK));
                pPIArray.push(Math.round(result[i].PPI));
                fPIArray.push(Math.round(result[i].FPI));
                pIKArray.push(Math.round(result[i].PIK));
                fIKArray.push(Math.round(result[i].FIK));
                pSSMArray.push(Math.round(result[i].PSSM));
                fSSMArray.push(Math.round(result[i].FSSM));
                fS1Array.push(Math.round(result[i].FS1));
                fS11Array.push(Math.round(result[i].FS11));
                fS12Array.push(Math.round(result[i].FS12));
                fS2Array.push(Math.round(result[i].FS2));

                if (result[i].Year === lYear) {
                    psSSW += result[i].PSSW;
                    fsSSW += result[i].FSSW;
                    psPK += result[i].PPK;
                    fsPK += result[i].FPK;
                    psPI += result[i].PPI;
                    fsPI += result[i].FPI;
                    psIK += result[i].PIK;
                    fsIK += result[i].FIK;
                    psSSM += result[i].PSSM;
                    fsSSM += result[i].FSSM;
                    fsS1 += result[i].FS1;
                    fsS11 += result[i].FS11;
                    fsS12 += result[i].FS12;
                    fsS2 += result[i].FS2;
                    pFullly += result[i].PFull;
                    fFullly += result[i].FFull;

                    lyMonthArray.push(result[i].Month);
                    psSSWArray.push(Math.round(psSSW));
                    fsSSWArray.push(Math.round(fsSSW));
                    psPKArray.push(Math.round(psPK));
                    fsPKArray.push(Math.round(fsPK));
                    psPIArray.push(Math.round(psPI));
                    fsPIArray.push(Math.round(fsPI));
                    psIKArray.push(Math.round(psIK));
                    fsIKArray.push(Math.round(fsIK));
                    psSSMArray.push(Math.round(psSSM));
                    fsSSMArray.push(Math.round(fsSSM));
                    fsS1Array.push(Math.round(fsS1));
                    fsS11Array.push(Math.round(fsS11));
                    fsS12Array.push(Math.round(fsS12));
                    fsS2Array.push(Math.round(fsS2));
                    lspsSSW += result[i].PSSW;
                    lsfsSSW += result[i].FSSW;
                    lspsPK += result[i].PPK;
                    lsfsPK += result[i].FPK;
                    lspsPI += result[i].PPI;
                    lsfsPI += result[i].FPI;
                    lspsIK += result[i].PIK;
                    lsfsIK += result[i].FIK;
                    lspsSSM += result[i].PSSM;
                    lsfsSSM += result[i].FSSM;
                    lsfsS1 += result[i].FS1;
                    lsfsS11 += result[i].FS11;
                    lsfsS12 += result[i].FS12;
                    lsfsS2 += result[i].FS2;

                }
                if (result[i].Year === tYear) {
                    if (switchBack === 0) {
                        psSSW = 0;
                        fsSSW = 0;
                        psPK = 0;
                        fsPK = 0;
                        psPI = 0;
                        fsPI = 0;
                        psIK = 0;
                        fsIK = 0;
                        psSSM = 0;
                        fsSSM = 0;
                        fsS1 = 0;
                        fsS11 = 0;
                        fsS12 = 0;
                        fsS2 = 0;
                        switchBack = 1;
                    }
                    psSSW += result[i].PSSW;
                    fsSSW += result[i].FSSW;
                    psPK += result[i].PPK;
                    fsPK += result[i].FPK;
                    psPI += result[i].PPI;
                    fsPI += result[i].FPI;
                    psIK += result[i].PIK;
                    fsIK += result[i].FIK;
                    psSSM += result[i].PSSM;
                    fsSSM += result[i].FSSM;
                    fsS1 += result[i].FS1;
                    fsS11 += result[i].FS11;
                    fsS12 += result[i].FS12;
                    fsS2 += result[i].FS2;
                    tyMonthArray.push(result[i].Month);
                    tpsSSWArray.push(Math.round(psSSW));
                    tfsSSWArray.push(Math.round(fsSSW));
                    tpsPKArray.push(Math.round(psPK));
                    tfsPKArray.push(Math.round(fsPK));
                    tpsPIArray.push(Math.round(psPI));
                    tfsPIArray.push(Math.round(fsPI));
                    tpsIKArray.push(Math.round(psIK));
                    tfsIKArray.push(Math.round(fsIK));
                    tpsSSMArray.push(Math.round(psSSM));
                    tfsSSMArray.push(Math.round(fsSSM));
                    tfsS1Array.push(Math.round(fsS1));
                    tfsS11Array.push(Math.round(fsS11));
                    tfsS12Array.push(Math.round(fsS12));
                    tfsS2Array.push(Math.round(fsS2));
                    tspsSSW += result[i].PSSW;
                    tsfsSSW += result[i].FSSW;
                    tspsPK += result[i].PPK;
                    tsfsPK += result[i].FPK;
                    tspsPI += result[i].PPI;
                    tsfsPI += result[i].FPI;
                    tspsIK += result[i].PIK;
                    tsfsIK += result[i].FIK;
                    tspsSSM += result[i].PSSM;
                    tsfsSSM += result[i].FSSM;
                    tsfsS1 += result[i].FS1;
                    tsfsS12 += result[i].FS11;
                    tsfsS12 += result[i].FS12;
                    tsfsS2 += result[i].FS2;
                    pFullty += result[i].PFull;
                    fFullty += result[i].FFull;
                }
            }
            lsosSSW = lsfsSSW - lspsSSW;
            lsosPK = lsfsPK - lspsPK;
            lsosPI = lsfsPI - lspsPI;
            lsosIK = lsfsIK - lspsIK;
            lsosSSM = lsfsSSM - lspsSSM;
            tsosSSW = tsfsSSW - tspsSSW;
            tsosPK = tsfsPK - tspsPK;
            tsosPI = tsfsPI - tspsPI;
            tsosIK = tsfsIK - tspsIK;
            tsosSSM = tsfsSSM - tspsSSM;
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: pSSWArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fSSWArray,
                        color: colorSecondYear
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
            Highcharts.chart('yLSSW', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: lYear.toString() + ' з/п производства с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: lyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psSSWArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fsSSWArray,
                        color: colorSecondYear
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
            Highcharts.chart('yTSSW', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: tYear.toString() + ' з/п производства с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: tyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsSSWArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: tfsSSWArray,
                        color: colorSecondYear
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
            Highcharts.chart('mPK', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: '% по кредиту  (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: pPKArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fPKArray,
                        color: colorSecondYear
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
            Highcharts.chart('yLPK', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: lYear.toString() + ' % по кредиту с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: lyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psPKArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fsPKArray,
                        color: colorSecondYear
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
            Highcharts.chart('yTPK', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: tYear.toString() + ' % по кредиту с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: tyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsPKArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: tfsPKArray,
                        color: colorSecondYear
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
            Highcharts.chart('mPI', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Постоянные издержки  (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: pPIArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fPIArray,
                        color: colorSecondYear
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
            Highcharts.chart('yLPI', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: lYear.toString() + ' Постоянные издержки с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: lyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psPIArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fsPIArray,
                        color: colorSecondYear
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
            Highcharts.chart('yTPI', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: tYear.toString() + ' Постоянные издержки с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: tyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsPIArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: tfsPIArray,
                        color: colorSecondYear
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
            Highcharts.chart('mIK', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Коммерческие издержки - транспорт  (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: pIKArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fIKArray,
                        color: colorSecondYear
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
            Highcharts.chart('yLIK', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: lYear.toString() + ' Коммерческие издержки - транспорт с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: lyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psIKArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fsIKArray,
                        color: colorSecondYear
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
            Highcharts.chart('yTIK', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: tYear.toString() + ' Коммерческие издержки - транспорт с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: tyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsIKArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: tfsIKArray,
                        color: colorSecondYear
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
            Highcharts.chart('mSSM', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'С/С материалов  (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: pSSMArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fSSMArray,
                        color: colorSecondYear
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
            Highcharts.chart('yLSSM', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: lYear.toString() + ' С/С материалов с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: lyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psSSMArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: fsSSMArray,
                        color: colorSecondYear
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
            Highcharts.chart('yTSSM', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: tYear.toString() + ' С/С материалов с накоплением (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: tyMonthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsSSMArray,
                        color: colorThreeYear
                    },
                    {
                        name: 'Факт',
                        data: tfsSSMArray,
                        color: colorSecondYear
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
            Highcharts.chart('mS1', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Ком. изд. - претензии / исп. фин. инфрастуктуры (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray
                },
                series: [
                    {
                        name: 'Всего',
                        data: fS1Array,
                        color: colorSecondYear
                    }, {
                        name: 'Претензии',
                        data: fS12Array,
                        color: '#56A8CBFF'
                    }, {
                        name: 'Комиссия от выручки',
                        data: fS11Array,
                        color: colorIK
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
            Highcharts.chart('yLS1', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: lYear.toString() + ' Ком. изд. - претензии / исп. фин. инфрастуктуры (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: lyMonthArray
                },
                series: [
                    {
                        name: 'Всего',
                        data: fsS1Array,
                        color: colorSecondYear
                    }, {
                        name: 'Претензии',
                        data: fsS12Array,
                        color: '#56A8CBFF'
                    }, {
                        name: 'Комиссия от выручки',
                        data: fsS11Array,
                        color: colorIK
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
            Highcharts.chart('yTS1', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: tYear.toString() + ' Ком. изд. - претензии / исп. фин. инфрастуктуры (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: tyMonthArray
                },
                series: [
                    {
                        name: 'Всего',
                        data: tfsS1Array,
                        color: colorSecondYear
                    }, {
                        name: 'Комиссия от выручки',
                        data: tfsS11Array,
                        color: colorIK
                    }, {
                        name: 'Претензии',
                        data: tfsS12Array,
                        color: '#56A8CBFF'
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
            Highcharts.chart('mS2', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Условно ПИ - командировки (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray
                },
                series: [
                    {
                        name: 'Факт',
                        data: fS2Array,
                        color: colorSecondYear
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
            Highcharts.chart('yLS2', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: lYear.toString() + ' Условно ПИ - командировки (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: lyMonthArray
                },
                series: [
                    {
                        name: 'Факт',
                        data: fsS2Array,
                        color: colorSecondYear
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
            Highcharts.chart('yTS2', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'area'
                },
                title: {
                    text: tYear.toString() + ' Условно ПИ - командировки (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: tyMonthArray
                },
                series: [
                    {
                        name: 'Факт',
                        data: tfsS2Array,
                        color: colorSecondYear
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
            Highcharts.chart('cilSSW', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
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
                    categories: [lYear, tYear]
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'План',
                    data: [Math.round(lspsSSW), Math.round(tspsSSW)],
                    color: colorFirstYear
                }, {
                    name: 'Факт',
                    data: [Math.round(lsfsSSW), Math.round(tsfsSSW)],
                    color: colorSecondYear
                }, {
                    name: 'Откл.',
                    data: [Math.round(lsosSSW), Math.round(tsosSSW)],
                    color: colorThreeYear
                }],
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            color: "black",
                            style: {
                                textOutline: false
                            }
                        }
                    }
                },
            });
            Highcharts.chart('cilPK', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: '% по кредиту (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [lYear, tYear]
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'План',
                    data: [Math.round(lspsPK), Math.round(tspsPK)],
                    color: colorFirstYear
                }, {
                    name: 'Факт',
                    data: [Math.round(lsfsPK), Math.round(tsfsPK)],
                    color: colorSecondYear
                }, {
                    name: 'Откл.',
                    data: [Math.round(lsosPK), Math.round(tsosPK)],
                    color: colorThreeYear
                }],
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            color: "black",
                            style: {
                                textOutline: false
                            }
                        }
                    }
                },
            });
            Highcharts.chart('cilPI', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Постоянные издержки (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [lYear, tYear]
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'План',
                    data: [Math.round(lspsPI), Math.round(tspsPI)],
                    color: colorFirstYear
                }, {
                    name: 'Факт',
                    data: [Math.round(lsfsPI), Math.round(tsfsPI)],
                    color: colorSecondYear
                }, {
                    name: 'Откл.',
                    data: [Math.round(lsosPI), Math.round(tsosPI)],
                    color: colorThreeYear
                }],
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            color: "black",
                            style: {
                                textOutline: false
                            }
                        }
                    }
                },
            });
            Highcharts.chart('cilIK', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Ком. изд. - транспорт (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [lYear, tYear]
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'План',
                    data: [Math.round(lspsIK), Math.round(tspsIK)],
                    color: colorFirstYear
                }, {
                    name: 'Факт',
                    data: [Math.round(lsfsIK), Math.round(tsfsIK)],
                    color: colorSecondYear
                }, {
                    name: 'Откл.',
                    data: [Math.round(lsosIK), Math.round(tsosIK)],
                    color: colorThreeYear
                }],
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            color: "black",
                            style: {
                                textOutline: false
                            }
                        }
                    }
                },
            });
            Highcharts.chart('cilSSM', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'С/С материалов (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [lYear, tYear]
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'План',
                    data: [Math.round(lspsSSM), Math.round(tspsSSM)],
                    color: colorFirstYear
                }, {
                    name: 'Факт',
                    data: [Math.round(lsfsSSM), Math.round(tsfsSSM)],
                    color: colorSecondYear
                }, {
                    name: 'Откл.',
                    data: [Math.round(lsosSSM), Math.round(tsosSSM)],
                    color: colorThreeYear
                }],
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            color: "black",
                            style: {
                                textOutline: false
                            }
                        }
                    }
                },
            });
            Highcharts.chart('cilS1', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Ком. изд. - претензии / исп. фин. инфрастуктуры (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [lYear, tYear]
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Факт',
                    data: [Math.round(lsfsS1), Math.round(tsfsS1)],
                    color: colorSecondYear
                }],
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            color: "black",
                            style: {
                                textOutline: false
                            }
                        }
                    }
                }
            });
            Highcharts.chart('cilS2', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Условно ПИ - командировки (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [Math.round(lYear), Math.round(tYear)]
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'Факт',
                    data: [Math.round(lsfsS2), Math.round(tsfsS2)],
                    color: colorSecondYear
                }],
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            color: "black",
                            style: {
                                textOutline: false
                            }
                        }
                    }
                }
            });
            var a1 = pFullly;
            var a2 = pFullty;
            var b1 = fFullly;
            var b2 = fFullty;
            var c1 = fFullly - pFullly;
            var c2 = fFullty - pFullty;
            Highcharts.chart('cilFull', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Итого (тыс.)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [lYear, tYear]
                },
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'План',
                    data: [Math.round(a1), Math.round(a2)],
                    color: colorFirstYear
                }, {
                    name: 'Факт',
                    data: [Math.round(b1), Math.round(b2)],
                    color: colorSecondYear
                }, {
                    name: 'Откл.',
                    data: [Math.round(c1), Math.round(c2)],
                    color: colorThreeYear
                }],
                plotOptions: {
                    column: {
                        dataLabels: {
                            enabled: true,
                            color: "black",
                            style: {
                                textOutline: false
                            }
                        }
                    }
                }
            });
        }
    });
}

function GetCustomerData() {
    $.ajax({
        url: "/DashboardDD/GetCustomerData/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArray = Object.keys(result).length;
            var typeObj;
            var ssmArray = new Array();
            var profitArray = new Array();
            var rateArray = new Array();
            var fssmArray = new Array();
            var c1 = 0;
            var c2 = 0;
            var c3 = 0;
            var c4 = 0;
            for (var i = 0; i < lenghtArray; i++) {
                if (result[i].Ssm > 0) {
                    typeObj = {
                        name: result[i].Customer,
                        y: Math.round(result[i].Ssm / 1000)
                    };
                    ssmArray.push(typeObj);
                    c1 += result[i].Ssm;
                } 
                if (result[i].Profit > 0) {
                    typeObj = {
                        name: result[i].Customer,
                        y: Math.round(result[i].Profit / 1000)
                    };
                    profitArray.push(typeObj);
                    c2 += result[i].Profit;
                }
                if (result[i].Rate > 0) {
                    typeObj = {
                        name: result[i].Customer,
                        y: Math.round(result[i].Rate / 1000)
                    };
                    rateArray.push(typeObj);
                    c3 += result[i].Rate;
                }
                if (result[i].Fssm > 0) {
                    typeObj = {
                        name: result[i].Customer,
                        y: Math.round(result[i].Fssm / 1000)
                    };
                    fssmArray.push(typeObj);
                    c4 += result[i].Fssm;
                }
            }
            document.getElementById("summaryoSSM").textContent = 'Всего: ' + Math.round(c1 / 1000);
            document.getElementById("summaryoPr").textContent = 'Всего: ' + Math.round(c2 / 1000);
            document.getElementById("summaryoRate").textContent = 'Всего: ' + Math.round(c3 / 1000);
            document.getElementById("summaryoFSSM").textContent = 'Всего: ' + Math.round(c4 / 1000); 
            Highcharts.setOptions({
                credits: {
                    enabled: false
                },
                colors: Highcharts.map(Highcharts.getOptions().colors, function (color) {
                    return {
                        radialGradient: {
                            cx: 0.5,
                            cy: 0.3,
                            r: 0.7
                        },
                        stops: [
                            [0, color],
                            [1, Highcharts.color(color).brighten(-0.3).get('rgb')]
                        ]
                    };
                })
            });

            Highcharts.chart('oSSM', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Себестоимость материалов (%) (ТЭО отгружаемых станций в текущем году)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%' 
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name} {point.y}</b>: {point.percentage:.1f} %',
                            connectorColor: 'silver'
                        }
                    }
                },
                series: [{
                    name: 'Доля',
                    data: ssmArray
                }]
            });
            Highcharts.chart('oPr', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'НОП (%) (ТЭО отгружаемых станций в текущем году)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name} {point.y}</b>: {point.percentage:.1f} %',
                            connectorColor: 'silver'
                        }
                    }
                },
                series: [{
                    name: 'Доля',
                    data: profitArray
                }]
            });
            Highcharts.chart('oRate', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Выручка без НДС (%) (ТЭО отгружаемых станций в текущем году) ',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name} {point.y}</b>: {point.percentage:.1f} %',
                            connectorColor: 'silver'
                        }
                    }
                },
                series: [{
                    name: 'Доля',
                    data: rateArray
                }]
            });
            Highcharts.chart('oFSSM', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Фактическиая себестоимость материалов    (%) (ТЭО отгруженых станций в текущем году)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name} {point.y}</b>: {point.percentage:.1f} %',
                            connectorColor: 'silver'
                        }
                    }
                },
                series: [{
                    name: 'Доля',
                    data: fssmArray
                }]
            });
        }
    });
}

function GetN() {
    $.ajax({
        url: "/DashboardDD/GetN/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArray = Object.keys(result).length;
            var snArray = new Array();
            var svnArray = new Array();
            var monthArray = new Array();
            for (var i = 0; i < lenghtArray; i++) {
                snArray.push(result[i].Ns);
                svnArray.push(result[i].Nsv);
                monthArray.push(result[i].Month);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('nSN', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Средний НОП (средний показатель за месяц - привязка к дате открытия заказа)',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray
                },
                series: [
                    {
                        name: 'Средний НОП',
                        data: snArray,
                        color: "#910000"
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

function GetNLast120() {
    $.ajax({
        url: "/DashboardDD/GetNLast120/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArray = Object.keys(result).length;
            var snArray = new Array();
            var monthArray = new Array();
            for (var i = 0; i < lenghtArray; i++) {
                snArray.push(result[i].Ns);
                monthArray.push(result[i].Month);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('nSVN', {
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Средневзвешенный НОП (привязка к дате открытия заказа) *',
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
                        name: 'СВНОП',
                        data: snArray 
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

function GetDSVN() {
    $.ajax({
        url: "/DashboardDD/GetDSVN/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArray = Object.keys(result).length;
            var dsvnArray = new Array();
            var monthArray = new Array();
            for (var i = 0; i < lenghtArray; i++) {
                dsvnArray.push(result[i].Dsvn);
                monthArray.push(result[i].Month);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('dSVN', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Динамика среднего взвешенного НОПа **',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: monthArray
                },
                series: [
                    {
                        name: 'Средний показатель',
                        data: dsvnArray,
                        color: "#910000"
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

function GetNCustomer() {
    $.ajax({
        url: "/DashboardDD/GetNCustomer/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArray = Object.keys(result).length;
            var percentArray = new Array();
            var customerArray = new Array();
            for (var i = 0; i < lenghtArray; i++) {
                percentArray.push(result[i].Percent);
                customerArray.push(result[i].Customer);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('nPercent', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'НОП (%) (ТЭО открытых заказов в текущем году) ',
                    style: {
                        "font-size": titleFontSize,
                        "color": titleDiagrammColor
                    },
                    margin: 0
                },
                xAxis: {
                    categories: customerArray
                },
                series: [
                    {
                        name: 'НОП (%)',
                        data: percentArray,
                        color: "#910000"
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