$(document).ready(function () {
    getPeriodReport();
    getTablePlan();
    getGanttProjects();
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
    { "title": "% ожидаемого освоения к плану", "data": "inThisMonthPercent", "autowidth": true, "bSortable": false, "className": 'text-center' }
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

    cars = [{
        model: 'Nissan Leaf',
        current: 0,
        deals: [{
            rentedTo: 'Lisa Star',
            from: today - 1 * day,
            to: today + 2 * day
        }, {
            rentedTo: 'Shane Long',
            from: today - 3 * day,
            to: today - 2 * day
        }, {
            rentedTo: 'Jack Coleman',
            from: today + 5 * day,
            to: today + 6 * day
        }]
    }, {
        model: 'Jaguar E-type',
        current: 0,
        deals: [{
            rentedTo: 'Martin Hammond',
            from: today - 2 * day,
            to: today + 1 * day
        }, {
            rentedTo: 'Linda Jackson',
            from: today - 2 * day,
            to: today + 1 * day
        }, {
            rentedTo: 'Robert Sailor',
            from: today + 2 * day,
            to: today + 6 * day
        }]
    }, {
        model: 'Volvo V60',
        current: 0,
        deals: [{
            rentedTo: 'Mona Ricci',
            from: today + 0 * day,
            to: today + 3 * day
        }, {
            rentedTo: 'Jane Dockerman',
            from: today + 3 * day,
            to: today + 4 * day
        }, {
            rentedTo: 'Bob Shurro',
            from: today + 6 * day,
            to: today + 8 * day
        }]
    }, {
        model: 'Volkswagen Golf',
        current: 0,
        deals: [{
            rentedTo: 'Hailie Marshall',
            from: today - 1 * day,
            to: today + 1 * day
        }, {
            rentedTo: 'Morgan Nicholson',
            from: today - 3 * day,
            to: today - 2 * day
        }, {
            rentedTo: 'William Harriet',
            from: today + 2 * day,
            to: today + 3 * day
        }]
    }, {
        model: 'Peugeot 208',
        current: 0,
        deals: [{
            rentedTo: 'Harry Peterson',
            from: today - 1 * day,
            to: today + 2 * day
        }, {
            rentedTo: 'Emma Wilson',
            from: today + 3 * day,
            to: today + 4 * day
        }, {
            rentedTo: 'Ron Donald',
            from: today + 5 * day,
            to: today + 6 * day
        }]
    }];
    series = cars.map(function (car, i) {
        var data = car.deals.map(function (deal) {
            return {
                id: 'deal-' + i,
                rentedTo: deal.rentedTo,
                start: deal.from,
                end: deal.to,
                y: i
            };
        });
        return {
            name: car.model,
            data: data,
            current: car.deals[car.current]
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
        title: {
            text: 'Ход изготовления изделий'
        },
        tooltip: {
            pointFormat: '<span>Rented To: {point.rentedTo}</span><br/><span>From: {point.start:%e. %b}</span><br/><span>To: {point.end:%e. %b}</span>'
        },
        xAxis: {
            currentDateIndicator: true
        },
        yAxis: {
            type: 'category',
            grid: {
                columns: [{
                    title: {
                        text: 'Model'
                    },
                    categories: map(series, function (s) {
                        return s.name;
                    })
                }, {
                    title: {
                        text: 'Rented To'
                    },
                    categories: map(series, function (s) {
                        return s.current.rentedTo;
                    })
                }, {
                    title: {
                        text: 'From'
                    },
                    categories: map(series, function (s) {
                        return dateFormat('%e. %b', s.current.from);
                    })
                }, {
                    title: {
                        text: 'To'
                    },
                    categories: map(series, function (s) {
                        return dateFormat('%e. %b', s.current.to);
                    })
                }]
            }
        }
    });
}