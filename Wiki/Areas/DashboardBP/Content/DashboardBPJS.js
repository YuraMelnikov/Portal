$(document).ready(function () {
    loadGantt();
});

function loadGantt() {
    $.ajax({
        url: "/BP/GetProjectsPortfolio/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var tmp = JSON.stringify(result.projects);
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

//periodReport