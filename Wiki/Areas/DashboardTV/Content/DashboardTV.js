$(document).ready(function () {
    getPeriodReport();
    getTablePlan();
    getGanttProjects();
    getTablePlanChack();
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

var objTableData = [
    { "title": "План на начало месяца", "data": "monthPlan", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "Освоено на сегодняшний день", "data": "inThisDay", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "% выполнения к мес. плану", "data": "inThisDayPercent", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "Ожидаемое освоение материалов", "data": "inThisMonth", "autowidth": true, "bSortable": false, "className": 'text-center', render: $.fn.dataTable.render.number(',', '.', 0, '') },
    { "title": "", "data": "glyphicon1", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "% ожидаемого освоения к плану", "data": "inThisMonthPercent", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "", "data": "glyphicon", "autowidth": true, "bSortable": false, "className": 'text-center' }
];

function getTablePlan() {
    $("#tablePlan").DataTable({
        "ajax": {
            "cache": false,
            "url": "/DashboardTVC/GetTablePlan",
            "type": "POST",
            "datatype": "json"
        },
        "columns": objTableData,
        "paging": false,
        "searching": false,
        "info": false,
        "ordering": false
    });
}

var maxStep = 0;

function getGanttProjects() {
    GetSteps();
    step = 0;
    setInterval(function () {
        step++;
        if (step > maxStep) {
            step = 1;
        }
        if (step === 1) {
            getGanttProjects1();
        }
        if (step === 2) {
            getGanttProjects2();
        }
        if (step === 3) {
            getGanttProjects3();
        }
    }, 10000);
}

function GetSteps() {
    $.ajax({
        url: "/DashboardTVC/GetProjectsPortfolio/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            maxStep = result.length / 3;
            if (maxStep > 3) {
                maxStep = 3;
            }
        }
    });
}

function getGanttProjects1() {
    $.ajax({
        url: "/DashboardTVC/GetProjectsPortfolio/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSON = JSON.parse(JSON.stringify(result));
            for (var i = 0; i < 33; i++) {
                for (var p = 0; p < Object.keys(myJSON[i].Deals).length; p++) {
                    myJSON[i].Deals[p].From = converDateJSON(myJSON[i].Deals[p].From);
                    myJSON[i].Deals[p].To = converDateJSON(myJSON[i].Deals[p].To);
                }
                myJSON[i].DataOtgruzkiBP = converDateJSON(myJSON[i].DataOtgruzkiBP);
            }
            var pointWidthForGantt = 650 / Object.keys(myJSON).length * 0.75;
            pointWidthForGantt = 14;
            var widthGanttSize = 700;
            var today = new Date(),
                day = 1000 * 60 * 60 * 24,
                map = Highcharts.map,
                dateFormat = Highcharts.dateFormat,
                series,
                seriesDT,
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
            series = series.slice(0, 33);
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
                                fontSize: pointWidthForGantt - 4,
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
                            "fontSize": pointWidthForGantt - 4
                        }
                    }
                },
                yAxis: {
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - 4
                        }
                    },
                    type: 'category',
                    grid: {
                        columns: [{
                            title: {
                                text: 'Отгрузка'
                            },
                            categories: map(series, function (s) {
                                return dateFormat('%e. %b', s.dataOtgruzkiBP);
                            })
                        }, {
                            title: {
                                text: 'Заказ'
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

function getGanttProjects2() {
    $.ajax({
        url: "/DashboardTVC/GetProjectsPortfolio2/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {

            var rc = result.slice(0, 1);
            result = result.slice(33, 66);
            rc = rc.concat(result);

            var myJSON = JSON.parse(JSON.stringify(rc));
            for (var i = 0; i < 33; i++) {
                for (var p = 0; p < Object.keys(myJSON[i].Deals).length; p++) {
                    myJSON[i].Deals[p].From = converDateJSON(myJSON[i].Deals[p].From);
                    myJSON[i].Deals[p].To = converDateJSON(myJSON[i].Deals[p].To);
                }
                myJSON[i].DataOtgruzkiBP = converDateJSON(myJSON[i].DataOtgruzkiBP);
            }
            var pointWidthForGantt = 650 / Object.keys(myJSON).length * 0.75;
            pointWidthForGantt = 14;
            var widthGanttSize = 700;
            var today = new Date(),
                day = 1000 * 60 * 60 * 24,
                map = Highcharts.map,
                dateFormat = Highcharts.dateFormat,
                series,
                seriesDT,
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
            series = series.slice(0, 33);
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
                                fontSize: pointWidthForGantt - 4,
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
                            "fontSize": pointWidthForGantt - 4
                        }
                    }
                },
                yAxis: {
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - 4
                        }
                    },
                    type: 'category',
                    grid: {
                        columns: [{
                            title: {
                                text: 'Отгрузка'
                            },
                            categories: map(series, function (s) {
                                return dateFormat('%e. %b', s.dataOtgruzkiBP);
                            })
                        }, {
                            title: {
                                text: 'Заказ'
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

function getGanttProjects3() {
    $.ajax({
        url: "/DashboardTVC/GetProjectsPortfolio3/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var rc = result.slice(0, 1);
            result = result.slice(66, result.length);
            rc = rc.concat(result);
            var myJSON = JSON.parse(JSON.stringify(rc));
            for (var i = 0; i < myJSON.length; i++) {
                for (var p = 0; p < Object.keys(myJSON[i].Deals).length; p++) {
                    myJSON[i].Deals[p].From = converDateJSON(myJSON[i].Deals[p].From);
                    myJSON[i].Deals[p].To = converDateJSON(myJSON[i].Deals[p].To);
                }
                myJSON[i].DataOtgruzkiBP = converDateJSON(myJSON[i].DataOtgruzkiBP);
            }
            var pointWidthForGantt = 650 / Object.keys(myJSON).length * 0.75;
            pointWidthForGantt = 14;
            var widthGanttSize = 700;
            var today = new Date(),
                day = 1000 * 60 * 60 * 24,
                map = Highcharts.map,
                dateFormat = Highcharts.dateFormat,
                series,
                seriesDT,
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
            series = series.slice(0, 33);
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
                                fontSize: pointWidthForGantt - 4,
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
                            "fontSize": pointWidthForGantt - 4
                        }
                    }
                },
                yAxis: {
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - 4
                        }
                    },
                    type: 'category',
                    grid: {
                        columns: [{
                            title: {
                                text: 'Отгрузка'
                            },
                            categories: map(series, function (s) {
                                return dateFormat('%e. %b', s.dataOtgruzkiBP);
                            })
                        }, {
                            title: {
                                text: 'Заказ'
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
    today = today - 60 * day;
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

var objPlanChack = [
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "Заказчик", "data": "customer", "autowidth": true, "bSortable": false },
    { "title": "Требуемый срок передачи на проверку", "data": "planDate", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "Ожидаемая дата передачи на проверку", "data": "factDate", "autowidth": true, "bSortable": false, "className": 'text-center' },
    { "title": "Отклонение", "data": "deviation", "autowidth": true, "bSortable": false, "className": 'text-center' }
];

function getTablePlanChack() {
    $("#tablePlanChack").DataTable({
        "ajax": {
            "cache": false,
            "url": "/DashboardTVC/GetTablePlanChack",
            "type": "POST",
            "datatype": "json"
        },
        "columns": objPlanChack,
        "rowCallback": function (row, data, index) {
            if (data.deviation < 0) {
                $('td', row).eq(4).addClass('highlightColor');
            }
        },
        "paging": false,
        "searching": false,
        "info": false,
        "ordering": false
    });
}