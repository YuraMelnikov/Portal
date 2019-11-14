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

function getGanttProjects() {
    $.ajax({
        url: "/DashboardTVC/GetProjectsPortfolio/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONDT = JSON.parse(JSON.stringify(result));
            for (var l = 0; l < Object.keys(myJSONDT).length; l++) {
                for (var j = 0; j < Object.keys(myJSONDT[l].Deals).length; j++) {
                    myJSONDT[l].Deals[j].From = converDateJSON(myJSONDT[l].Deals[j].From);
                    myJSONDT[l].Deals[j].To = converDateJSON(myJSONDT[l].Deals[j].To);
                }
                myJSONDT[l].DataOtgruzkiBP = converDateJSON(myJSONDT[l].DataOtgruzkiBP);
            }
            var widthGanttSizeFDT = 14 * Object.keys(myJSONDT).length * 1.5;
            pointWidthForGantt = 14;
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
            seriesDT = myJSONDT.map(function (myJSONDT, i) {
                var data = myJSONDT.Deals.map(function (deal) {
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
                    dataOtgruzkiBP: myJSONDT.DataOtgruzkiBP,
                    name: myJSONDT.OrderNumber,
                    color: myJSONDT.Color,
                    data: data,
                    current: myJSONDT.Deals[myJSONDT.Current]
                };
            });
            var seriesForDesc = seriesDT;
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
            Highcharts.ganttChart('projectPortfolioForDescTop', {
                series: seriesForDesc,
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
                            categories: map(seriesForDesc, function (s) {
                                return dateFormat('%e. %b', s.dataOtgruzkiBP);
                            })
                        }, {
                            title: {
                                text: '№ заказа'
                            },
                                categories: map(seriesForDesc, function (s) {
                                return s.name;
                            })
                        }]
                    }
                },
                chart: {
                    height: widthGanttSizeFDT + 'px'
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