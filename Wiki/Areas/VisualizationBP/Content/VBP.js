var speedometerRed1To = 700;
var speedometerYellowTo = 1000;
var speedometerGreenTo = 1250;

$(document).ready(function () {
    getPeriodReport();
    getGanttProjects();
    getSmTy();
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
            }
            var pointWidthForGantt = 14;
            var widthGanttSize = pointWidthForGantt * lenghtElements * 1.5;
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
                    exportButtonTitle: "Экспорт",
                    printButtonTitle: "Печать",
                    rangeSelectorFrom: "С",
                    rangeSelectorTo: "По",
                    rangeSelectorZoom: "Период",
                    downloadPNG: 'Скачать PNG',
                    downloadJPEG: 'Скачать JPEG',
                    downloadPDF: 'Скачать PDF',
                    downloadSVG: 'Скачать SVG',
                    printChart: 'Напечатать график',
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
                    pointFormat: '<span>Rented To: {point.rentedTo}</span><br/><span>From: {point.start:%e. %b}</span><br/><span>To: {point.end:%e. %b}</span>'
                },
                xAxis: {
                    tickInterval: 1000 * 60 * 60 * 24 * 30, 
                    min: getMinDate(),
                    max: getMaxDate(),
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - 5
                        }
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
                                text: 'Дата отгрузки'
                            },
                            categories: map(series, function (s) {
                                return dateFormat('%e. %b', s.dataOtgruzkiBP);
                            })
                        }, {
                            title: {
                                text: '№ заказа'
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
    var day = 1000 * 60 * 60 * 24;
    today = today - 90 * day;
    var tmp = new Date(today);
    var minDate = new Date(tmp.getFullYear(), tmp.getMonth(), 1, 0, 0, 0, 0);
    return minDate.getTime();
}

function getMaxDate() {
    var today = new Date();
    var day = 1000 * 60 * 60 * 24;
    today = today.setDate(150);
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

function getSmTy(){
    getSmTy1();
    getSmTy2();
    getSmTy3();
    getSmTy4();
    getSmTy5();
    getSmTy6();
    getSmTy7();
    getSmTy8();
    getSmTy9();
    getSmTy10();
    getSmTy11();
    getSmTy12();
}

function getSmTy1() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/1",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {




            var gaugeOptions = {

                chart: {
                    type: 'solidgauge'
                },
            
                title: null,
            
                pane: {
                    center: ['50%', '85%'],
                    size: '140%',
                    startAngle: -90,
                    endAngle: 90,
                    background: {
                        backgroundColor:
                            Highcharts.defaultOptions.legend.backgroundColor || '#EEE',
                        innerRadius: '60%',
                        outerRadius: '100%',
                        shape: 'arc'
                    }
                },
            
                tooltip: {
                    enabled: false
                },
            
                // the value axis
                yAxis: {
                    stops: [
                        [0.9, '#DF5353'], // red
                        [0.5, '#DDDF0D'], // yellow
                        [0.1, '#55BF3B'] // green
                    ],
                    lineWidth: 0,
                    minorTickInterval: null,
                    tickAmount: 2,
                    title: {
                        y: -70
                    },
                    labels: {
                        y: 16
                    }
                },
            
                plotOptions: {
                    solidgauge: {
                        dataLabels: {
                            y: 5,
                            borderWidth: 0,
                            useHTML: true
                        }
                    }
                }
            };
            
            // The speed gauge
            var chartSpeed = Highcharts.chart('container-speed', Highcharts.merge(gaugeOptions, {
                yAxis: {
                    min: 0,
                    max: 1400,
                    title: {
                        text: 'Speed'
                    }
                },
            
                credits: {
                    enabled: false
                },
            
                series: [{
                    name: 'Speed',
                    data: [80],
                    dataLabels: {
                        format:
                            '<div style="text-align:center">' +
                            '<span style="font-size:25px">{y}</span><br/>' +
                            '<span style="font-size:12px;opacity:0.4">km/h</span>' +
                            '</div>'
                    },
                    tooltip: {
                        valueSuffix: ' km/h'
                    }
                }]
            
            }));
            
            // Bring life to the dials
            setInterval(function () {
                // Speed
                var point,
                    newVal,
                    inc;
            
                if (chartSpeed) {
                    point = chartSpeed.series[0].points[0];
                    inc = Math.round((Math.random() - 0.5) * 100);
                    newVal = point.y + inc;
            
                    if (newVal < 0 || newVal > 200) {
                        newVal = point.y - inc;
                    }
            
                    point.update(newVal);
                }
            
                // RPM
                if (chartRpm) {
                    point = chartRpm.series[0].points[0];
                    inc = Math.random() - 0.5;
                    newVal = point.y + inc;
            
                    if (newVal < 0 || newVal > 5) {
                        newVal = point.y - inc;
                    }
            
                    point.update(newVal);
                }
            }, 2000);
            


















            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }    
            });
            Highcharts.chart('sppedometrThisYear1', {
                chart: {
                    type: 'solidgauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}

function getSmTy2() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/2",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear2', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy3() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/3",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear3', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy4() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/4",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear4', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy5() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/5",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear5', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy6() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/6",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear6', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy7() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/7",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear7', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy8() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/8",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear8', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy9() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/9",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear9', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy10() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/10",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear10', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy11() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/11",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear11', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}
function getSmTy12() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/12",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var dataInSpeedometr = new Array();
            dataInSpeedometr[0] = result[0].data;
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('sppedometrThisYear12', {
                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    text: ''
                },
                pane: {
                    startAngle: -150,
                    endAngle: 150,
                    background: [{
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#FFF'],
                                [1, '#333']
                            ]
                        },
                        borderWidth: 0,
                        outerRadius: '109%'
                    }, {
                        backgroundColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#333'],
                                [1, '#FFF']
                            ]
                        },
                        borderWidth: 1,
                        outerRadius: '107%'
                    }, {
                    }, {
                        backgroundColor: '#DDD',
                        borderWidth: 0,
                        outerRadius: '105%',
                        innerRadius: '103%'
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 1500,
                    minorTickInterval: 'auto',
                    minorTickWidth: 1,
                    minorTickLength: 10,
                    minorTickPosition: 'inside',
                    minorTickColor: '#666',
                    tickPixelInterval: 40,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 10,
                    tickColor: '#666',
                    labels: {
                        step: 2,
                        rotation: 'auto'
                    },
                    plotBands: [{
                        from: 0,
                        to: speedometerRed1To,
                        color: '#DF5353' // red
                    }, {
                        from: speedometerRed1To,
                        to: speedometerYellowTo,
                        color: '#DDDF0D' // yellow
                    }, {
                        from: speedometerYellowTo,
                        to: speedometerGreenTo,
                        color: '#55BF3B' // green
                    }, {
                        from: speedometerGreenTo,
                        to: 1500,
                        color: '#DF5353' // red
                    }]
                },
                series: [{
                    name: '',
                    data: dataInSpeedometr,
                    tooltip: {
                        valueSuffix: ' тыс. ХСС'
                    }
                }]
            });
        }
    });
}