$(document).ready(function () {
    getPeriodReport();
    getGanttProjects();
    getSmTy1();
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
            for (var i = 0; i < Object.keys(myJSON).length; i++) {
                for (var j = 0; j < Object.keys(myJSON[i].Deals).length; j++) {
                    myJSON[i].Deals[j].From = converDateJSON(myJSON[i].Deals[j].From);
                    myJSON[i].Deals[j].To = converDateJSON(myJSON[i].Deals[j].To);
                }
                myJSON[i].DataOtgruzkiBP = converDateJSON(myJSON[i].DataOtgruzkiBP);
                myJSON[i].ContractDateComplited = converDateJSON(myJSON[i].ContractDateComplited);
            }
            var heightLen = Object.keys(myJSON).length * 14 * 1.2 + 'px';
            var pointWidthForGantt = 12;
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
                    contractDateComplited: myJSON.ContractDateComplited,
                    failure: myJSON.Failure,
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
                                fontSize: pointWidthForGantt - 3,
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
                    pointFormat: '<span>ХСС: {point.rentedTo}</span><br/><span>С: {point.start:%e. %b}</span><br/><span>По: {point.end:%e. %b}</span>'
                },
                xAxis: {
                    tickInterval: 1000 * 60 * 60 * 24 * 30,
                    min: getMinDate(),
                    max: getMaxDate(),
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - 2
                        }
                    }
                },
                yAxis: {
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - 2
                        }
                    },
                    type: 'category',
                    grid: {
                        columns: [{
                            title: {
                                text: 'Заказ'
                            },
                            categories: map(series, function (s) {
                                return s.name;
                            })
                        },{
                            title: {
                                text: 'Срок'
                            },
                            categories: map(series, function (s) {
                                return dateFormat('%e. %b', s.contractDateComplited);
                            })
                        },{
                            title: {
                                text: 'Откл'
                            },
                            categories: map(series, function (s) {
                                if(s.failure < 0){
                                    return '<span style="fill: red; font-weight:bold;">'  + s.failure + '</span>';
                                }
                                else{
                                    return s.failure;
                                }
                            })
                        },{
                            title: {
                                text: 'Отгрузка'
                            },
                            categories: map(series, function (s) {
                                return dateFormat('%e. %b', s.dataOtgruzkiBP);
                            })
                        }]
                    }
                },
                chart: {
                    height: heightLen
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

function getSmTy1() {
    $.ajax({
        url: "/VBP/GetSppedometrThisYear1Month/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {





            Highcharts.chart('sppedometrThisYear1', {

                chart: {
                  type: 'gauge',
                  plotBackgroundColor: null,
                  plotBackgroundImage: null,
                  plotBorderWidth: 0,
                  plotShadow: false
                },
              
                title: {
                  text: 'Speedometer'
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
                    // default background
                  }, {
                    backgroundColor: '#DDD',
                    borderWidth: 0,
                    outerRadius: '105%',
                    innerRadius: '103%'
                  }]
                },
              
                // the value axis
                yAxis: {
                  min: 0,
                  max: 200,
              
                  minorTickInterval: 'auto',
                  minorTickWidth: 1,
                  minorTickLength: 10,
                  minorTickPosition: 'inside',
                  minorTickColor: '#666',
              
                  tickPixelInterval: 30,
                  tickWidth: 2,
                  tickPosition: 'inside',
                  tickLength: 10,
                  tickColor: '#666',
                  labels: {
                    step: 2,
                    rotation: 'auto'
                  },
                  title: {
                    text: 'km/h'
                  },
                  plotBands: [{
                    from: 0,
                    to: 120,
                    color: '#55BF3B' // green
                  }, {
                    from: 120,
                    to: 160,
                    color: '#DDDF0D' // yellow
                  }, {
                    from: 160,
                    to: 200,
                    color: '#DF5353' // red
                  }]
                },
              
                series: [{
                  name: 'Speed',
                  data: [80],
                  tooltip: {
                    valueSuffix: ' km/h'
                  }
                }]
              
              },
              // Add some life
              function (chart) {
                if (!chart.renderer.forExport) {
                  setInterval(function () {
                    var point = chart.series[0].points[0],
                      newVal,
                      inc = Math.round((Math.random() - 0.5) * 20);
              
                    newVal = point.y + inc;
                    if (newVal < 0 || newVal > 200) {
                      newVal = point.y - inc;
                    }
              
                    point.update(newVal);
              
                  }, 3000);
                }
              });









        }
    });
}