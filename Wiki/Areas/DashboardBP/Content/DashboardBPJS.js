$(document).ready(function () {
    getGanttProjectPortfolio();
    getPeriodReport();
    getHSSPlanToYear();
    getRemainingHss();
    getRetePlan();
    getHSSToDay();
});

function getGanttProjectPortfolio() {
    $.ajax({
        url: "/BP/GetProjectsPortfolio/",
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

function getPeriodReport() {
    $.ajax({
        url: "/BP/GetPeriodReport/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var tmp = result;
            document.getElementById("periodReportString").textContent = result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getHSSPlanToYear() {
    $.ajax({
        url: "/BP/GetHSSPlanToYear/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan = new Array();
            var myJSONFact = new Array();
            myJSONRemainingPlan[0] = result[0];
            myJSONFact[0] = result[1];
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
                    text: 'Плановый годовой ХСС ПО',
                    style: {
                        "font-size": "13px"
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

function getRemainingHss() {
    $.ajax({
        url: "/BP/GetRemainingHss/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan = new Array();
            var myJSONFact = new Array();
            myJSONRemainingPlan[0] = result[0];
            myJSONFact[0] = result[1];
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
            Highcharts.chart('remainingHss', {
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
                    text: 'Оставшийся ХСС ПО',
                    style: {
                        "font-size": "13px"
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
                    name: 'Нехватка',
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
                    name: 'Остаток',
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

function getRetePlan() {
    $.ajax({
        url: "/BP/GetRetePlan/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan1 = new Array();
            var myJSONFact1 = new Array();
            myJSONRemainingPlan1[0] = result[0];
            myJSONFact1[0] = result[1];
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
            Highcharts.chart('retePlan', {
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
                    text: 'Плановый годовой объем выручки',
                    style: {
                        "font-size": "13px"
                    },
                    margin: 0
                },
                xAxis: {
                    categories: [''],
                    visible: false
                },
                yAxis: {
                    min: 0,
                    max: myJSONRemainingPlan1[0] + myJSONFact1[0],
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
                    data: myJSONRemainingPlan1,
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
                    data: myJSONFact1,
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

function getHSSToDay() {
    $.ajax({
        url: "/BP/GetHSSToDay/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSON = JSON.parse(JSON.stringify(result));
            var json = JSON.stringify(result);
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
            Highcharts.chart('containerHssHistory', {
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: '',
                    style: {
                        "fontSize": "0px"
                    }
                },
                subtitle: {
                    text: 'ХСС ПО'
                },
                xAxis: {
                    type: 'datetime'
                },
                yAxis: {
                    title: {
                        enabled: false
                    },
                    min: 0
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    area: {
                        fillColor: {
                            linearGradient: {
                                x1: 0,
                                y1: 0,
                                x2: 0,
                                y2: 1
                            },
                            stops: [
                                [0, Highcharts.getOptions().colors[0]],
                                [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        marker: {
                            radius: 2
                        },
                        lineWidth: 1,
                        states: {
                            hover: {
                                lineWidth: 1
                            }
                        },
                        threshold: null
                    }
                },
                series: [{
                    type: 'area',
                    name: 'ХСС',
                    data: myJSON
                }]
            });

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}