var speedometerRed1To = 700;
var speedometerYellowTo = 1000;
var speedometerGreenTo = 1250;
var sizeTextLabetGraphic = '11px';
var marginForTitle = -10;
var heightForStatusLine = '69px';
var minusPxTextForGantt = 5;
var redZoneReamainingHSS = 2000;
var pointWidthForGantt = 14;
var heightTableTasks = '190px';
var heightTableComments = '600px';

var cardArray = new Array();

$(document).ready(function () {
    getPeriodReport();
    getGanttProjects();
    GetHSSPlanToYear();
    GetRatePlanToYear();
    GetRemainingHSS();
    GetTaskThisDayTable();
    GetVarianceTasksTable();
    getRemainingWorkE();
    getRemainingWork();
    GetCountComments();
    GetCommentsList();
    GetWorkpowerManufacturing();
    GetNoPlaningHSS();
    GetPrjCart(1845);
});

var objTableTaskData = [
    { "title": "Заказ", "data": "orderNumber", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Задача", "data": "taskName", "autowidth": true, "bSortable": false },
    { "title": "БНачало", "data": "basicStartDate", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Начало", "data": "startDate", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "БОконч.", "data": "basicFinishDate", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Оконч.", "data": "finishDate", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Исполнитель", "data": "executorName", "autowidth": true, "bSortable": true },
    { "title": "Тр (ч)", "data": "remainingWork", "autowidth": true, "bSortable": false, "className": 'text-center' }
];

function GetPrjCart(id) {
    GetPrjContractDate(id);
    GetPercentDevisionComplited(id);
    GetProjectTasksStates(id);
    CreateTaskCard();

    $('#orderModal').modal('show');
}

function GetPrjContractDate(id) {
    $.ajax({
        url: "/VBP/GetPrjContractDate/" + id,
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("orderNumber").textContent = "Заводской номер изделия: " + result.data[0].orderNumber;
            document.getElementById("prjContractName").textContent = "Контрактное (договорное) наименование: " + result.data[0].prjContractName;
            document.getElementById("prjName").textContent = "Наименивание по ТУ: " + result.data[0].prjName;
            document.getElementById("prjContractDateSh").textContent = "Контрактный срок отгрузки: " + result.data[0].prjContractDateSh;
            document.getElementById("prjDateSh").textContent = "Плановый срок отгрузки: " + result.data[0].prjDateSh;
            document.getElementById("prjShState").textContent = "Откл.: " + result.data[0].prjShState;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetPercentDevisionComplited(id){
    $.ajax({
        url: "/VBP/GetPercentDevisionComplited/" + id,
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var complitedArray = new Array();
            var remainingArray = new Array();
            var countDevision = 3;
            for(var i = 0; i < countDevision; i++){
                complitedArray.push(result.devisionsArray[i].PercentComplited);
                remainingArray.push(result.devisionsArray[i].PercentRemainingWork);
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('percentDevisionComplited', {
                legend: {
                    enabled: false
                },
                chart: {
                    type: 'bar',
                    height: 150
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                title: {
                    margin: 0,
                    text: '% завершения',
                    style: {
                        "font-size": "12px",
                        "color": '#717171'
                    }
                },
                yAxis: {
                    title: {
                        enabled: false
                    },    
                    stackLabels: {
                        enabled: true,
                        style: {
                            color: '#717171'
                        }
                    },
                    max: 100,
                    min: 0
                },
                xAxis: {
                    categories: ['КБМ', 'КБЭ', 'ПО']
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            style: {
                                fontSize: "0px",
                                textOutline: "0px contrast"
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'Завершено',
                    data: complitedArray,
                    color: '#3fb0ac'
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetProjectTasksStates(id) {
    $.ajax({
        url: "/VBP/GetProjectTasksStates/" + id, 
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {

            var countedCard = 0;
            for(var i = 0; i < 5; i++) {
                countedCard = result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates.length;
                for(var j = 0; j < countedCard; j++){
                    cardArray.push(result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].Name);
                }
            }
            CreateTaskCard(cardArray.length);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function CreateTaskCard(counterStep){
    var basicBlock = "";
    for(var i = 0; i < counterStep; i++){
        basicBlock += GetStandartCardTask(cardArray[i]);
    }
    document.getElementById("tasksCardPool").innerHTML = basicBlock;
}

function GetStandartCardTask(cardName){
    var lastDivBlock = "</div>";
    var firstDivBlock = "<div class=" + '\u0022' +  "col-lg-2"  + '\u0022' +  ">";
    var cardBlock = "<div class=" + '\u0022' +  "card card-default"  + '\u0022' +  ">";
    cardBlock += "<div class=" + '\u0022' +  "card-header"  + '\u0022' +  ">";
    cardBlock += cardName + lastDivBlock;
    cardBlock += "<div class=" + '\u0022' +  "card-body"  + '\u0022' +  ">";
    return firstDivBlock + cardBlock + lastDivBlock + lastDivBlock + lastDivBlock;
} 


function GetBurnDownChartDevM(id) {

}

function GetBurnDownChartDevE(id) {
    
}

function GetBurnDownChartManufac(id) { 
    
}

function GetPrjComments(id) {

}

function GetPrjCriticalRoad(id) {

}









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
                myJSON[i].RemainingDuration = myJSON[i].RemainingDuration;
                myJSON[i].PercentComplited = myJSON[i].PercentComplited;
                myJSON[i].Duration = myJSON[i].Duration;
            }
            var today = new Date(),
                map = Highcharts.map,
                dateFormat = Highcharts.dateFormat,
                series;
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
                    remainingDuration: myJSON.RemainingDuration,
                    duration: myJSON.Duration,
                    percentComplited: myJSON.PercentComplited,
                    contractDateComplited: myJSON.ContractDateComplited,
                    name: "<a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getSandwichPanel('" + myJSON.OrderNumber + "')" + '\u0022' + myJSON.OrderNumber +"</a>",
                    //name:  myJSON.OrderNumber,
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
                                fontSize: pointWidthForGantt - minusPxTextForGantt,
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
                xAxis: [{
                    min: getMinDate(),
                    max: getMaxDate(),
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - minusPxTextForGantt
                        }
                    },
                    type: 'category',
                    gridLineWidth: 1
                }, {
                    visible: false,
                    opposite: false
                }],
                yAxis: {
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - minusPxTextForGantt
                        }
                    },
                    type: 'category',
                    grid: {
                        columns: [{
                            title: {
                                text: 'Отгрузка',
                                style: {
                                    "color": "#0d233a",
                                    "fontSize": pointWidthForGantt - minusPxTextForGantt
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
                                    "fontSize": pointWidthForGantt - minusPxTextForGantt
                                }
                            },
                            categories: map(series, function (s) {
                                return s.failure;
                            })
                            }, {
                                title: {
                                    text: '%',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return s.percentComplited;
                                })
                            },
                            {
                                title: {
                                    text: 'Ост. длит.',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return s.remainingDuration;
                                })
                            },
                            {
                                title: {
                                    text: 'Длит.',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return s.duration;
                                })
                            },
                            {
                            title: {
                                text: 'КС',
                                style: {
                                    "color": "#0d233a",
                                    "fontSize": pointWidthForGantt - minusPxTextForGantt
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
                                    "fontSize": pointWidthForGantt - minusPxTextForGantt
                                }
                            },
                            categories: map(series, function (s) {
                                return s.name;
                            }),
                            scrollbar: {
                                enabled: true,
                                showFull: false
                            }
                        }
                        ]
                    },
                    max: 20
                },
                chart: {
                    height: '515px'
                }
            });
        }
    });
}

function getMinDate() {
    var today = new Date();
    var tmp = new Date(today);
    var minDate = new Date(tmp.getFullYear(), tmp.getMonth(), 2, 0, 0, 0, 0);
    return minDate.getTime();
}

function getMaxDate() {
    var today = new Date();
    today = today.setDate(90);
    var tmp = new Date(today);
    var maxDate = new Date(tmp.getFullYear(), tmp.getMonth(), 31, 0, 0, 0, 0);
    return maxDate.getTime();
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
                    height: heightForStatusLine
                },
                title: {
                    align: 'left',
                    text: 'ХСС (тыс.)',
                    style: {
                        "font-size": sizeTextLabetGraphic
                    },
                    margin: marginForTitle
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
                    height: heightForStatusLine
                },
                title: {
                    align: 'left',
                    text: 'Прибыль (тыс.)',
                    style: {
                        "font-size": sizeTextLabetGraphic
                    },
                    margin: marginForTitle
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

function GetRemainingHSS() {
    $.ajax({
        url: "/VBP/GetRemainingHSS/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan = new Array();
            myJSONRemainingPlan[0] = result[0];
            var colorLen = '#2b908f';
            if (result[0] < redZoneReamainingHSS) {
                colorLen = '#910000';
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('remainingHSS', {
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
                    height: heightForStatusLine
                },
                title: {
                    align: 'left',
                    text: 'Ост. ХСС (тыс.)',
                    style: {
                        "font-size": sizeTextLabetGraphic
                    },
                    margin: marginForTitle
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
                    color: colorLen,
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

function GetNoPlaningHSS() {
    $.ajax({
        url: "/VBP/GetNoPlaningHSS/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan = new Array();
            myJSONRemainingPlan[0] = result[0];
            var colorLen = '#2b908f';
            if (result[0] > 0) {
                colorLen = '#910000';
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('noPlaningHSS', {
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
                    height: heightForStatusLine
                },
                title: {
                    align: 'left',
                    text: 'Неспланир. ХСС (тыс.)',
                    style: {
                        "font-size": sizeTextLabetGraphic
                    },
                    margin: marginForTitle
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
                    name: 'Неспланировано',
                    data: myJSONRemainingPlan,
                    color: colorLen,
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

function GetTaskThisDayTable() {
    var today = new Date();
    var dateString = ConvertDateToGlobalShortString(today);
    $("#tableTasksThisDay").DataTable({
        "dom": '<"toolbar1">frtip',
        "ajax": {
            "cache": false,
            "url": "/VBP/GetTaskThisDayTable",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objTableTaskData,
        "rowCallback": function (row, data, index) {
            if (data.basicStartDate === dateString) {
                $('td', row).eq(2).addClass('colorTdTaskThisDay');
            }
            if (data.basicFinishDate === dateString) {
                $('td', row).eq(4).addClass('colorTdTaskThisDay');
            }
        },
        "cache": false,
        "async": false,
        "scrollY": heightTableTasks,
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи",
            "search": "Поиск"
        }
    });
    $("div.toolbar1").html('<b>Планируемое начало/окончание работ</b>');
}

function GetVarianceTasksTable() {
    var today = moment();
    var tomorrow = moment(today).add(-1, 'days');
    var dateString = ConvertDateToGlobalShortString(tomorrow._d);
    $("#tableVarianceTasks").DataTable({
        "dom": '<"toolbar">frtip',
        "ajax": {
            "cache": false,
            "url": "/VBP/GetVarianceTasksTable/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objTableTaskData,
        "rowCallback": function (row, data, index) {
            if (data.basicStartDate === dateString) {
                $('td', row).eq(2).addClass('colorTdVarianceTaskThisDay');
            }
            if (data.basicFinishDate === dateString) {
                $('td', row).eq(4).addClass('colorTdVarianceTaskThisDay');
            }
        },
        "cache": false,
        "async": false,
        "scrollY": heightTableTasks,
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи"
        }
    });
    $("div.toolbar").html('<b>Срывы планируемого начала/окончания задач:</b>');
}

function getRemainingWorkE() {
    $.ajax({
        url: "/ReportPage/GetRemainingWorkE/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('remainingWorkE', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Оставшиеся тр-ты КБЭ',
                    style: {
                        "font-size": "13px"
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    color: '#4572A7',
                    name: 'НЧ',
                    data: dataArray
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.y}'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getRemainingWork() {
    $.ajax({
        url: "/ReportPage/GetRemainingWork/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('remainingWork', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Оставшиеся тр-ты КБМ',
                    style: {
                        "font-size": "13px"
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    color: '#4572A7',
                    name: 'НЧ',
                    data: dataArray
                }],
                yAxis: {
                    title: {
                        enabled: false
                    }
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.y}'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetCountComments() {
    $.ajax({
        url: "/VBP/GetCountComments/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById('commentsLink').innerHTML = 'Комментарии: ' + result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

var objCommentsForTable = [
    { "title": "Заказ", "data": "orderNumber", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Задача", "data": "taskName", "autowidth": true, "bSortable": false },
    { "title": "Комментарий", "data": "notes", "autowidth": true, "bSortable": false },
    { "title": "Исполнитель", "data": "workerName", "autowidth": true, "bSortable": true }
];

function GetCommentsListNow() {
    var table = $('#commentsTable').DataTable();
    table.destroy();
    $('#commentsTable').empty();
    $("#commentsTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/VBP/GetCommentsList/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objCommentsForTable,
        "cache": false,
        "async": false,
        "scrollY": heightTableComments,
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи"
        }
    });
    $('#commentsModal').modal('show'); 
}

function GetCommentsList() {
    $("#commentsTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/VBP/GetCommentsList/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objCommentsForTable,
        "cache": false,
        "async": false,
        "scrollY": heightTableComments,
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи"
        }
    });

}

var objWorkpowerManufacturingTable = [
    { "title": "Участок", "data": "devision", "autowidth": true, "bSortable": true },
    { "title": "Кол-во (prj)", "data": "countPrj", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Тр-ты (prj)", "data": "workPrj", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Трудодни", "data": "workDay", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Коэф. прочих", "data": "coefDefaultWork", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Режим работы", "data": "workMode", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Период", "data": "period", "autowidth": true, "bSortable": true, "className": 'text-center' }
];

function GetWorkpowerManufacturing() {
    $("#workpowerManufacturingTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/VBP/GetWorkpowerManufacturing/",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            { "targets": 5 }
        ],
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objWorkpowerManufacturingTable,
        "cache": false,
        "async": false,
        "scrollY": heightTableComments,
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true,
        "language": {
            "zeroRecords": "Отсутствуют записи",
            "infoEmpty": "Отсутствуют записи"
        }
    });
}

