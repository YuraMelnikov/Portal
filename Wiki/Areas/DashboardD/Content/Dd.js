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
            var tfsS2Array = new Array();
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
            var tsfsS2 = 0;
            var lspsSSW = 0;
            var lsfsSSW = 0;
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
            var fsS2 = 0;
            for (var i = 0; i < lenghtResult; i++) {
                monthArray.push(result[i].Month);
                pSSWArray.push(result[i].PSSW);
                fSSWArray.push(result[i].FSSW);
                pPKArray.push(result[i].PPK);
                fPKArray.push(result[i].FPK);
                pPIArray.push(result[i].PPI);
                fPIArray.push(result[i].FPI);
                pIKArray.push(result[i].PIK);
                fIKArray.push(result[i].FIK);
                pSSMArray.push(result[i].PSSM);
                fSSMArray.push(result[i].FSSM);
                fS1Array.push(result[i].FS1);
                fS2Array.push(result[i].FS2);
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
                    fsS2 += result[i].FS2;
                    psSSWArray.push(psSSW);
                    fsSSWArray.push(fsSSW);
                    psPKArray.push(psPK);
                    fsPKArray.push(fsPK);
                    psPIArray.push(psPI);
                    fsPIArray.push(fsPI);
                    psIKArray.push(psIK);
                    fsIKArray.push(fsIK);
                    psSSMArray.push(psSSM);
                    fsSSMArray.push(fsSSM);
                    fsS1Array.push(fsS1);
                    fsS2Array.push(fsS2);
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
                        fsS2 = 0;
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
                    fsS2 += result[i].FS2;
                    tpsSSWArray.push(psSSW);
                    tfsSSWArray.push(fsSSW);
                    tpsPKArray.push(psPK);
                    tfsPKArray.push(fsPK);
                    tpsPIArray.push(psPI);
                    tfsPIArray.push(fsPI);
                    tpsIKArray.push(psIK);
                    tfsIKArray.push(fsIK);
                    tpsSSMArray.push(psSSM);
                    tfsSSMArray.push(fsSSM);
                    tfsS1Array.push(fsS1);
                    tfsS2Array.push(fsS2);
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
                    tsfsS2 += result[i].FS2;
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
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fSSWArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psSSMArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fsSSMArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsSSMArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: tfsSSMArray
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
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fPKArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psPKArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fsPKArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsPKArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: tfsPKArray
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
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fPIArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psPIArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fsPIArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsPIArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: tfsPIArray
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
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fIKArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psIKArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fsIKArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsIKArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: tfsIKArray
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
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fSSMArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: psSSMArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: fsSSMArray
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'План',
                        data: tpsSSMArray,
                        color: "#910000"
                    },
                    {
                        name: 'Факт',
                        data: tfsSSMArray
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
                        name: 'Факт',
                        data: fS1Array,
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'Факт',
                        data: fsS1Array,
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'Факт',
                        data: tfsS1Array,
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'Факт',
                        data: fsS2Array,
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
                    categories: monthArray
                },
                series: [
                    {
                        name: 'Факт',
                        data: tfsS2Array,
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
                    data: [lspsSSW, tspsSSW]
                }, {
                        name: 'Факт',
                    data: [lsfsSSW, tsfsSSW]
                }, {
                    name: 'Откл.',
                    data: [lsosSSW, tsosSSW]
                }]
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
                    data: [lspsPK, tspsPK]
                }, {
                    name: 'Факт',
                        data: [lsfsPK, tsfsPK]
                }, {
                    name: 'Откл.',
                        data: [lsosPK, tsosPK]
                }]
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
                    data: [lspsPI, tspsPI]
                }, {
                    name: 'Факт',
                    data: [lsfsPI, tsfsPI]
                }, {
                    name: 'Откл.',
                    data: [lsosPI, tsosPI]
                }]
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
                    data: [lspsIK, tspsIK]
                }, {
                    name: 'Факт',
                    data: [lsfsIK, tsfsIK]
                }, {
                    name: 'Откл.',
                    data: [lsosIK, tsosIK]
                }]
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
                    data: [lspsSSM, tspsSSM]
                }, {
                    name: 'Факт',
                        data: [lsfsSSM, tsfsSSM]
                }, {
                    name: 'Откл.',
                        data: [lsosSSM, tsosSSM]
                }]
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
                    data: [lsfsS1, tsfsS1]
                }]
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
                    data: [lsfsS2, tsfsS2]
                }]
            });
            var a1 = lspsSSW + lspsPK + lspsPI + lspsIK + lspsSSM;
            var a2 = tspsSSW + tspsPK + tspsPI + tspsIK + tspsSSM;
            var b1 = lsfsSSW + lsfsPK + lsfsPI + lsfsIK + lsfsSSM + lsfsS1 + lsfsS2;
            var b2 = tsfsSSW + tsfsPK + tsfsPI + tsfsIK + tsfsSSM + tsfsS1 + tsfsS2;
            var c1 = lsosSSW + lsosPK + lsosPI + lsosIK + lsosSSM;
            var c2 = tsosSSW + tsosPK + tsosPI + tsosIK + tsosSSM;
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
                    data: [a1, a2]
                }, {
                    name: 'Факт',
                    data: [b1, b2]
                }, {
                    name: 'Откл.',
                    data: [c1, c2]
                }]
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
          
         
        }
    });
}