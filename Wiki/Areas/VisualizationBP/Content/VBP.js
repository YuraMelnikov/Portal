var speedometerRed1To = 700;
var speedometerYellowTo = 1000;
var speedometerGreenTo = 1250;
var sizeTextLabetGraphic = '13px';

$(document).ready(function () {
    getPeriodReport();
    getGanttProjects();
    GetHSSPlanToYear();
    GetRatePlanToYear();
});

var objTableData = [
    { "title": "План на начало месяца", "data": "monthPlan", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "Освоено на сегодняшний день", "data": "inThisDay", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "% выполнения к мес. плану", "data": "inThisDayPercent", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "Ожидаемое освоение материалов", "data": "inThisMonth", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "", "data": "glyphicon1", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "% ожидаемого освоения к плану", "data": "inThisMonthPercent", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "", "data": "glyphicon", "autowidth": true, "bSortable": false, "className": 'text-center' }
];

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

function getGanttProjects() {
    $.ajax({
        url: "/DashboardTVC/GetProjectsPortfolio/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSON = JSON.parse(JSON.stringify(result));
            var lenghtElements = Object.keys(myJSON).length;
            for (var i = 0; i < lenghtElements; i++) {
                var lenghtElementsDeals = Object.keys(myJSON[i].Deals).length;
                for (var j = 0; j < lenghtElementsDeals; j++) {
                    myJSON[i].Deals[j].From = converDateJSON(myJSON[i].Deals[j].From);
                    myJSON[i].Deals[j].To = converDateJSON(myJSON[i].Deals[j].To);
                }
                myJSON[i].DataOtgruzkiBP = converDateJSON(myJSON[i].DataOtgruzkiBP);
                myJSON[i].ContractDateComplited = converDateJSON(myJSON[i].ContractDateComplited);
            }
            var pointWidthForGantt = 14;
            var widthGanttSize = pointWidthForGantt * lenghtElements * 1.4;
            var today = new Date(),
                day = 1000 * 60 * 60 * 24,
                map = Highcharts.map,
                dateFormat = Highcharts.dateFormat,
                series,
                cars;
            today.setUTCHours(0);
            today.setUTCMinutes(0);
            today.setUTCSeconds(0);
            today.setUTCMilliseconds(0);
            today = today.getTime();
            series = myJSON.map(function (myJSON, i) {
                var data = myJSON.Deals.map(function (deal) {
                    return {
                        id: 'deal-' + i,
                        rentedTo: deal.TCPM,
                        start: deal.From,
                        end: deal.To,
                        color: deal.Color,
                        dependency: 'prototype',
                        name: renderToNullString(deal.TCPM, deal.Milestone),
                        pointWidth: pointWidthForGantt,
                        milestone: deal.Milestone,
                        y: i
                    };
                });
                return {
                    dataOtgruzkiBP: myJSON.DataOtgruzkiBP,

                    failure: myJSON.Failure,
                    contractDateComplited: myJSON.ContractDateComplited,


                    name: myJSON.OrderNumber,
                    color: myJSON.Color,
                    data: data,
                    current: myJSON.Deals[myJSON.Current]
                };
            });
            Highcharts.setOptions({
                lang: {
                    loading: 'Загрузка...',
                    months: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
                    weekdays: ['Воскресенье', 'Понедельник', 'Вторник', 'Среда', 'Четверг', 'Пятница', 'Суббота'],
                    shortMonths: ['Янв', 'Фев', 'Март', 'Апр', 'Май', 'Июнь', 'Июль', 'Авг', 'Сент', 'Окт', 'Нояб', 'Дек'],
                    rangeSelectorFrom: "С",
                    rangeSelectorTo: "По",
                    rangeSelectorZoom: "Период",
                    Week: 'Нед.',
                    Start: 'Начало'
                },
                credits: {
                    enabled: false
                }
            });
            Highcharts.ganttChart('projectPortfolio', {
                series: series,
                plotOptions: {
                    series: {
                        animation: false,
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}',
                            style: {
                                color: "contrast",
                                fontSize: pointWidthForGantt - 5,
                                fontWeight: "bold",
                                textOutline: "1px contrast"
                            }
                        },
                        allowPointSelect: true
                    }
                },
                title: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                tooltip: {
                    pointFormat: '<span>ХСС: {point.rentedTo}</span><br/><span>Начало: {point.start:%e. %b}</span><br/><span>Окончание: {point.end:%e. %b}</span>'
                },
                xAxis: {
                    tickInterval: 1000 * 60 * 60 * 24 * 30, 
                    tickPixelInterval: 50,
                    min: getMinDate(),
                    max: getMaxDate(),
                    labels: {
                        align: 0,
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - 5
                        }
                    },
                    grid:{
                        cellHeight: 1
                    }
                },
                yAxis: {
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - 5
                        }
                    },
                    type: 'category',
                    grid: {
                        columns: [{
                            title: {
                                text: 'Отгрузка',
                                style: {
                                    "color": "#0d233a",
                                    "fontSize": pointWidthForGantt - 5
                                }
                            },
                            categories: map(series, function (s) {
                                return dateFormat('%e. %b', s.dataOtgruzkiBP);
                            })
                        }, {
                            title: {
                                text: 'Откл.',
                                style: {
                                    "color": "#0d233a",
                                    "fontSize": pointWidthForGantt - 5
                                }
                            },
                            categories: map(series, function (s) {
                                return s.failure;
                            })
                        }, {
                            title: {
                                text: 'КС',
                                style: {
                                    "color": "#0d233a",
                                    "fontSize": pointWidthForGantt - 5
                                }
                            },
                            categories: map(series, function (s) {
                                return dateFormat('%e. %b', s.contractDateComplited);
                            })
                        }, {
                            title: {
                                text: 'Заказ',
                                style: {
                                    "color": "#0d233a",
                                    "fontSize": pointWidthForGantt - 5
                                }
                            },
                            categories: map(series, function (s) {
                                return s.name;
                            })
                        }]
                    }
                },
                chart: {
                    height: widthGanttSize + 'px'
                }
            });
        }
    });
}

function getMinDate() {
    var today = new Date();
    var tmp = new Date(today);
    var minDate = new Date(tmp.getFullYear(), tmp.getMonth(), 1, 0, 0, 0, 0);
    return minDate.getTime();
}

function getMaxDate() {
    var today = new Date();
    today = today.setDate(120);
    var tmp = new Date(today);
    var minDate = new Date(tmp.getFullYear(), tmp.getMonth(), 1, 0, 0, 0, 0);
    return minDate.getTime();
}

function renderToNullString(text, milestone) {
    if (milestone === true) {
        if (text === 0)
            return '';
        else
            return numeral(text).format('0,0');
    }
    else {
        if (text === 0)
            return '<1';
        else
            return numeral(text).format('0,0');
    }
}

function converDateJSON(MyDate_String_Value) {
    var dat = MyDate_String_Value.replace(/\D+/g, "");
    return Number(dat);
}

function convertToInteger(value) {
    var data = parseInt(value);
    if (data === 0)
        data = '>1';
    return data;
}

function GetHSSPlanToYear() {
    $.ajax({
        url: "/VBP/GetHSSPlanToYear/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan = new Array();
            var myJSONFact = new Array();
            myJSONRemainingPlan[0] = result[0];
            myJSONFact[0] = result[1];
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('hSSPlanToYear', {
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar',
                    height: "100px"
                },
                title: {
                    text: 'ХСС производства',
                    style: {
                        "font-size": sizeTextLabetGraphic
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [''],
                    visible: false
                },
                yAxis: {
                    min: 0,
                    max: myJSONRemainingPlan[0] + myJSONFact[0],
                    title: {
                        enabled: false
                    },
                    tickInterval: 5,
                    visible: false
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'Остаток',
                    data: myJSONRemainingPlan,
                    color: '#910000',
                    dataLabels: {
                        enabled: true,
                        align: 'left',
                        style: {
                            fontWeight: 'bold'
                        },
                        x: 3,
                        verticalAlign: 'middle',
                        overflow: true,
                        crop: false
                    }
                }, {
                    name: 'Факт',
                    data: myJSONFact,
                    color: '#4572A7',
                    dataLabels: {
                        enabled: true,
                        align: 'left',
                        style: {
                            fontWeight: 'bold'
                        },
                        x: 3,
                        verticalAlign: 'middle',
                        overflow: true,
                        crop: false
                    }
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetRatePlanToYear() {
    $.ajax({
        url: "/VBP/GetRatePlanToYear/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan = new Array();
            var myJSONFact = new Array();
            myJSONRemainingPlan[0] = result[0];
            myJSONFact[0] = result[1];
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('ratePlanToYear', {
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar',
                    height: "100px"
                },
                title: {
                    text: 'Прибыль',
                    style: {
                        "font-size": sizeTextLabetGraphic
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [''],
                    visible: false
                },
                yAxis: {
                    min: 0,
                    max: myJSONRemainingPlan[0] + myJSONFact[0],
                    title: {
                        enabled: false
                    },
                    tickInterval: 5,
                    visible: false
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'Остаток',
                    data: myJSONRemainingPlan,
                    color: '#910000',
                    dataLabels: {
                        enabled: true,
                        align: 'left',
                        style: {
                            fontWeight: 'bold'
                        },
                        x: 3,
                        verticalAlign: 'middle',
                        overflow: true,
                        crop: false
                    }
                }, {
                    name: 'Факт',
                    data: myJSONFact,
                    color: '#4572A7',
                    dataLabels: {
                        enabled: true,
                        align: 'left',
                        style: {
                            fontWeight: 'bold'
                        },
                        x: 3,
                        verticalAlign: 'middle',
                        overflow: true,
                        crop: false
                    }
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetRemainingHSS() {
    $.ajax({
        url: "/VBP/GetRemainingHSS/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan = new Array();
            var myJSONFact = new Array();
            myJSONRemainingPlan[0] = result[0];
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('ratePlanToYear', {
                credits: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar',
                    height: "100px"
                },
                title: {
                    text: 'Оставшийся ХСС',
                    style: {
                        "font-size": sizeTextLabetGraphic
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [''],
                    visible: false
                },
                yAxis: {
                    min: 0,
                    max: myJSONRemainingPlan[0],
                    title: {
                        enabled: false
                    },
                    tickInterval: 5,
                    visible: false
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'Остаток',
                    data: myJSONRemainingPlan,
                    color: '#2b908f',
                    dataLabels: {
                        enabled: true,
                        align: 'left',
                        style: {
                            fontWeight: 'bold'
                        },
                        x: 3,
                        verticalAlign: 'middle',
                        overflow: true,
                        crop: false
                    }
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}