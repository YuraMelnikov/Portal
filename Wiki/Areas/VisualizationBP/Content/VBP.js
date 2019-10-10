$(document).ready(function () {
    getPeriodReport();
    getGanttProjectPortfolio();
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

function getGanttProjectPortfolio() {
    $.ajax({
        url: "/VBP/GetProjectsPortfolio/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var arrayToJSON = result.projects;
            var myJSON = JSON.parse(JSON.stringify(arrayToJSON));
            var countLine = myJSON.length;
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
            var today = new Date(),
                day = 1000 * 60 * 60 * 24,
                dateFormat = Highcharts.dateFormat,
                defined = Highcharts.defined,
                isObject = Highcharts.isObject,
                reduce = Highcharts.reduce;
            today.setUTCHours(0);
            today.setUTCMinutes(0);
            today.setUTCSeconds(0);
            today.setUTCMilliseconds(0);
            today = today.getTime();
            Highcharts.ganttChart('projectPortfolio', {
                xAxis: {
                    scrollbar: {
                        enabled: true
                    },
                    currentDateIndicator: true,
                    min: today - 7 * day,
                    max: today + 120 * day,
                    labels: {
                        style: {
                            "color": "#666666",
                            "fontSize": "13px"
                        }
                    }
                },
                chart: {
                    height: '900px'
                },
                series: [{
                    pointWidth: 13,
                    name: 'Заказ',
                    data: myJSON
                }],
                yAxis: {
                    type: 'category',
                    grid: {
                        scrollbar: {
                            enabled: false
                        },
                        columns: [{
                            title: {
                                text: 'Заказ'
                            },
                            labels: {
                                format: '{point.name}'
                            },
                            scrollbar: {
                                enabled: false
                            }
                        }, {
                            title: {
                                text: 'Контракт'
                            },
                            labels: {
                                format: '{point.contractDate:%e. %b}'
                            },
                            scrollbar: {
                                enabled: false
                            }

                        }, {
                            title: {
                                text: 'План'
                            },
                            labels: {
                                format: '{point.end:%e. %b}'
                            },
                            scrollbar: {
                                enabled: false
                            }
                        }, {
                            title: {
                                text: 'Откл.'
                            },
                            labels: {
                                formatter: function () {
                                    var point = this.point,
                                        days = 1000 * 60 * 60 * 24,
                                        number = (point.contractDate - point.end) / days;
                                    if (Math.round(number * 100) / 100 < 0) {
                                        return '<span style="fill: red; font-weight:bold;">' + Math.round(number * 100) / 100 + '</span>';
                                    } else {
                                        return Math.round(number * 100) / 100;
                                    }
                                }
                            }
                        }]
                    },
                    scrollbar: {
                        enabled: true
                    },
                    min: 0,
                    max: 40,
                    labels: {
                        style: {
                            "color": "#666666",
                            "fontSize": "10px"
                        }
                    }
                },
                tooltip: {
                    pointFormatter: function () {
                        var point = this,
                            format = '%e. %b',
                            options = point.options,
                            completed = options.completed,
                            amount = isObject(completed) ? completed.amount : completed,
                            status = (amount || 0) * 100 + '%',
                            lines;
                        lines = [{
                            value: point.name,
                            style: 'font-weight: bold;'
                        }, {
                            title: 'Начало',
                            value: dateFormat(format, point.start)
                        }, {
                            visible: !options.milestone,
                            title: 'Окончание',
                            value: dateFormat(format, point.end)
                        }, {
                            visible: !options.milestone,
                            title: 'Контрактный срок',
                            value: dateFormat(format, point.contractDate)
                        }, {
                            title: 'Владелец',
                            value: options.owner || 'unassigned'
                        }];
                        return reduce(lines, function (str, line) {
                            var s = '',
                                style = defined(line.style) ? line.style : 'font-size: 0.8em;';
                            if (line.visible !== false) {
                                s = '<span style="' + style + '">' + (defined(line.title) ? line.title + ': ' : '') + (defined(line.value) ? line.value : '') + '</span><br/>';
                            }
                            return str + s;
                        }, '');
                    }
                }
            });
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
            }
            var pointWidthForGantt = 650 / Object.keys(myJSON).length * 0.6;
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
                                fontSize: pointWidthForGantt - 1,
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
                    //currentDateIndicator: true,
                    min: getMinDate(),
                    max: getMaxDate(),
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt
                        }
                    }
                },
                yAxis: {
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt
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
                    height: '700px'
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