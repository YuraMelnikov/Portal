$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/BP/GetProjectsPortfolio/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSON = JSON.stringify(result);
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
                series: [{
                    data: [{
                        name: 'Order1',
                        id: 'order1',
                        owner: 'Manager1'
                    }, {
                        name: 'Task1',
                        id: 'prepare_building',
                        parent: 'order1',
                        start: today - 2 * day,
                        end: today + 6 * day,
                        completed: {
                            amount: 0.2
                        },
                        owner: 'Linda'
                    }, {
                        name: 'Task2',
                        id: 'inspect_building',
                        dependency: 'prepare_building',
                        parent: 'order1',
                        start: today + 6 * day,
                        end: today + 8 * day,
                        owner: 'Ivy'
                    }, {
                        name: 'Task3',
                        id: 'passed_inspection',
                        dependency: 'inspect_building',
                        parent: 'order1',
                        start: today + 9.5 * day,
                        milestone: true,
                        owner: 'Peter'
                    }]
                }],
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
                            title: '% зав.',
                            value: status
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
                },
                title: {
                    text: 'Портфель проектов'
                },
                xAxis: {
                    currentDateIndicator: true,
                    min: today - 7 * day,
                    max: today + 116 * day,
                    labels: {
                        format: '{value:%W}'
                    },
                    tickInterval: 1000 * 60 * 60 * 24 * 7 // week
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}