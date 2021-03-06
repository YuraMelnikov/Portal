﻿var sizeTextLabetGraphic = '11px';
var marginForTitle = -10;
var heightForStatusLine = '69px';
var minusPxTextForGantt = 5;
var redZoneReamainingHSS = 2250;
var pointWidthForGantt = 14;
var heightTableTasks = '190px';
var heightTableComments = '600px';
var colorDiagramm = '#3fb0ac'; 

var cardArray = new Array();

var objOrders = [
    { "title": "См.", "data": "viewLink", "autowidth": true, "bSortable": false },
    { "title": "Ред.", "data": "editLink", "autowidth": true, "bSortable": false },
    { "title": "№ заказа", "data": "order", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Заказчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Состояние", "data": "state", "autowidth": true, "bSortable": true },
    { "title": "Дата отправки РКД", "data": "dateLastLoad", "autowidth": true, "bSortable": true, "defaultContent": "", "render": processNull, "className": 'text-center' },
    { "title": "Текущая вер.", "data": "ver", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Дата открытия зак.", "data": "dateOpen", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Контрактный срок", "data": "contractDate", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "ГИП КБМ", "data": "gm", "autowidth": true, "bSortable": true },
    { "title": "ГИП КБЭ", "data": "ge", "autowidth": true, "bSortable": true }
];

$(document).ready(function () {
    getPeriodReport();
    getGanttProjects();
    GetHSSPlanToYear();
    GetRemainingHSS();
    GetTaskThisDayTable();
    GetVarianceTasksTable();
    getRemainingWorkE();
    getRemainingWork();
    GetCountComments();
    GetCommentsList();
    GetWorkpowerManufacturing();
    GetNoPlaningHSS();
    GetPerfomance();
    GetHSSPO();
    $("#tableApproveCD").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Approve/GetNoApproveTable/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[2, "asc"]],
        "processing": true,
        "columns": objOrders,
        "cache": false,
        "async": false,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true
    });
    $("#ordersTable").DataTable({
        "ajax": {
            "cache": false,
            "url": "/Approve/GetNoApproveTable/",
            "type": "POST",
            "datatype": "json"
        },
        "order": [[2, "asc"]],
        "processing": true,
        "columns": objOrders,
        "cache": false,
        "async": false,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "searching": false,
        "info": false,
        "scrollCollapse": true
    });
    $("#tableCMOOrder").DataTable({
        "ajax": {
            "cache": false,
            "url": "/VBP/GetOrderCMOTable/" + 0,
            "type": "POST",
            "datatype": "json"
        },
        "processing": true,
        "columns": cmoTableData,
        "scrollY": '75vh',
        "scrollX": true,
        "paging": false,
        "info": false,
        "scrollCollapse": true
    });
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
    if (id !== "!Итого") {
        document.getElementById('tasksCardPool').innerHTML = '';
        GetPrjContractDate(id);
        GetPercentDevisionComplited(id);
        GetProjectTasksStates(id);
        CreateTaskCard();
        GetCriticalRoadGanttToOrder(id);
        GetOrderCMOTable(id);
        GetBurndownDiagramM(id);
        GetBurndownDiagramE(id);
        GetBurndownDiagramP(id);
        GetTableApproveCD(id);
        $('#orderModal').modal('show');
    }
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
            document.getElementById("prjContractDateSh").textContent = "Договорн. срок отгрузки: " + result.data[0].prjContractDateSh;
            document.getElementById("prjDateSh").textContent =         "Плановый срок отгрузки: " + result.data[0].prjDateSh;
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
                    categories: ['КБМ/ГРМ', 'КБЭ/ГРЭ', 'ПО']
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

class Task {
    remainingWork;
    work;
    startDate;
    finishDate;
    users;
    percentComplited;
    taskName;

    constructor(remainingWork, work, startDate, finishDate, users, taskName) {
        var tmp = '';
        try {
            this.remainingWork = remainingWork;
        }
        catch {
            this.remainingWork = null;
        }
        try {
            this.work = work;
        }
        catch {
            this.work = null;
        }
        try {
            this.startDate = startDate;
        }
        catch {
            this.startDate = null;
        }
        try {
            this.finishDate = finishDate;
        }
        catch {
            this.finishDate = null;
        }
        try {
            this.users = users;
        }
        catch {
            this.users = null;
        }
        try {
            if (work === 0) {
                this.percentComplited = 100;
            }
            else {
                this.percentComplited = (work - remainingWork) / work * 100;
            }
        }
        catch {
            this.percentComplited = null;
        }
        try {
            this.taskName = taskName;
        }
        catch {
            this.taskName = null;
        }
    }

    get remainingWork() {
        return this.remainingWork;
    }
    get work() {
        return this.work;
    }
    get startDate() {
        return this.startDate;
    }
    get finishDate() {
        return this.finishDate;
    }
    get users() {
        return this.users;
    }
    get percentComplited() {
        return this.percentComplited;
    }
    get taskName() {
        return this.taskName;
    }
}

class TaskCard {
    cardName;
    taskArray;

    constructor(cardName) {
        this.cardName = cardName;
        this.taskArray = new Array();
    }
    get cardName() {
        return this.cardName;
    }
}

function GetProjectTasksStates(id) {
    cardArray = new Array();
    $.ajax({
        url: "/VBP/GetProjectTasksStates/" + id, 
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var counterCard = 0;
            var counerTask = 0;
            for(var i = 1; i < 5; i++) {
                counterCard = result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates.length;
                for (var j = 0; j < counterCard; j++) {
                    cardArray.push(new TaskCard(result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].Name));
                    counerTask = result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].ElementDataProjectTasksStates.length;
                    var p = cardArray.length - 1;
                    for (var k = 0; k < counerTask; k++) {
                        try {
                            cardArray[p].taskArray.push(new Task(
                                result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].ElementDataProjectTasksStates[k].RemainingWork,
                                result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].ElementDataProjectTasksStates[k].Work,
                                result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].ElementDataProjectTasksStates[k].StartDate,
                                result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].ElementDataProjectTasksStates[k].FinishDate,
                                result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].ElementDataProjectTasksStates[k].Users,
                                result.projectTasksState.BlockProjectTasksStates[i].ElementProjectTasksStates[j].ElementDataProjectTasksStates[k].Name
                            ));
                        }
                        catch {
                            cardArray[p].taskArray.push(new Task(null, null, null, null, null, null));
                        }
                    }
                }
            }
            CreateTaskCard(cardArray.length);
        } 
    });
}

function CreateTaskCard(counterStep) {
    var counterTasks = 0;
    var bodyBlock = "";
    var basicBlock = "";
    basicBlock = "<div class=" + '\u0022' + "row" + '\u0022' + ">";
    for (var i = 0; i < counterStep; i++) {
        counterTasks = cardArray[i].taskArray.length;
        for (var j = 0; j < counterTasks; j++) {
            if (cardArray[i].taskArray[j].taskName !== null) {
                var dateFinishTask = ParseJsonDate(cardArray[i].taskArray[j].finishDate);
                if (cardArray[i].taskArray[j].percentComplited === 100) {
                    bodyBlock += "<p class=" + '"' + "bg-info" + '"' + ">";
                    bodyBlock += "<span class=" + '"' + "glyphicon glyphicon-ok" + '"' + "></span>" + " | ";
                    bodyBlock += cardArray[i].taskArray[j].taskName + " | " + ConvertDateToGlobalShortString(dateFinishTask);
                }
                else if (cardArray[i].taskArray[j].percentComplited === 0) {
                    bodyBlock += "<p class=" + '"' + "bg-danger" + '"' + ">";
                    bodyBlock += "<span class=" + '"' + "glyphicon glyphicon-ban-circle" + '"' + "></span>" + " | ";
                    bodyBlock += cardArray[i].taskArray[j].taskName + " | " + ConvertDateToGlobalShortString(dateFinishTask) 
                        + " | " + Math.round(cardArray[i].taskArray[j].remainingWork);
                }
                else {
                    bodyBlock += "<p class=" + '"' + "bg-warning" + '"' + ">";
                    bodyBlock += "<span class=" + '"' + "glyphicon glyphicon-flash" + '"' + "></span>" + " | ";
                    bodyBlock += cardArray[i].taskArray[j].taskName + " | " + ConvertDateToGlobalShortString(dateFinishTask)
                        + " | " + Math.round(cardArray[i].taskArray[j].remainingWork);
                }
                bodyBlock += "</p>";
            }
        }
        basicBlock += GetStandartCardTask(cardArray[i].cardName, bodyBlock);
        var tmp = (i + 1) % 6;
        if (i % 6 === 0) {
            basicBlock += "</div>" + "<div class=" + '\u0022' + "row" + '\u0022' + ">";
        }
        bodyBlock = "";
    }
    document.getElementById("tasksCardPool").innerHTML = basicBlock;
}

function GetStandartCardTask(cardName, bodyBlock) {
    var lastDivBlock = "</div>";
    var firstDivBlock = "<div class=" + '\u0022' +  "col-lg-2"  + '\u0022' +  ">";
    var cardBlock = "<div class=" + '\u0022' +  "card card-default"  + '\u0022' +  ">";
    cardBlock += "<div class=" + '\u0022' +  "card-header"  + '\u0022' +  ">";
    cardBlock += cardName + lastDivBlock;
    cardBlock += "<div class=" + '\u0022' + "card-body" + '\u0022' + ">";
    cardBlock += bodyBlock;
    return firstDivBlock + cardBlock + lastDivBlock + lastDivBlock + lastDivBlock;
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
                    if (myJSON[i].Failure < 0) {
                        myJSON[i].Deals[j].Color = "#910000";
                    } 
                    else {
                        myJSON[i].Deals[j].Color = colorDiagramm;
                    }
                }
                if (myJSON[i].Failure < 0) {
                    myJSON[i].Color = "#910000";
                }
                else {
                    myJSON[i].Color = colorDiagramm;
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
                        allowPointSelect: true,
                        events: {
                            click: function (event) {
                                click: GetPrjCart(this.name);
                            }
                        }
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
                    enabled: false
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
                            }]
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
    var minDate = new Date(tmp.getFullYear(), tmp.getMonth(), 1, 0, 0, 0, 0);
    return minDate.getTime();
}

function getMaxDate() {
    var today = new Date();
    today = today.setDate(90);
    var tmp = new Date(today);
    var maxDate = new Date(tmp.getFullYear(), tmp.getMonth(), 30, 0, 0, 0, 0);
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
                    color: colorDiagramm,
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
                    color: colorDiagramm,
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
                    color: colorDiagramm,
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
    { "title": "Период", "data": "period", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Участок", "data": "devision", "autowidth": true, "bSortable": true },
    { "title": "Потребность (чел)", "data": "workMode", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Численность (prj)", "data": "countPrj", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Тр-ты (ч.)", "data": "workPrj", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Трудодни", "data": "workDay", "autowidth": true, "bSortable": true, "className": 'text-center' },
    { "title": "Коэф. прочих", "data": "coefDefaultWork", "autowidth": true, "bSortable": true, "className": 'text-center' }
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

function GetCriticalRoadGanttToOrder(id) {
    $.ajax({
        url: "/VBP/GetCriticalRoadGanttToOrder/" + id,
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSON = JSON.parse(JSON.stringify(result));
            var lenghtElements = Object.keys(myJSON).length;
            var heightDiagramm = lenghtElements * 14 + 80;
            heightDiagramm += 'px';
            var minDate = converDateJSON(myJSON[0].DataOtgruzkiBP);
            var maxDate = converDateJSON(myJSON[0].DataOtgruzkiBP);
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
                myJSON[i].TaskName = myJSON[i].TaskName;
                if (myJSON[i].DataOtgruzkiBP < minDate) {
                    minDate = myJSON[i].DataOtgruzkiBP;
                }
                if (myJSON[i].ContractDateComplited > maxDate) {
                    maxDate = myJSON[i].ContractDateComplited;
                }
            }
            var minCorrectDate = new Date();
            minCorrectDate = minCorrectDate.getTime();
            if (minDate < minCorrectDate) {
                minDate = minCorrectDate;
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
                        rentedTo: deal.User,
                        start: deal.From,
                        end: deal.To,
                        color: "#910000",
                        dependency: 'prototype',
                        name: renderToNullString(deal.User, deal.Milestone),
                        pointWidth: pointWidthForGantt,
                        milestone: deal.Milestone,
                        taskName: myJSON.TaskName,
                        nameN: myJSON.UserName,
                        percentComplited: myJSON.PercentComplited,
                        y: i
                    };
                });
                return {
                    dataOtgruzkiBP: myJSON.DataOtgruzkiBP,
                    remainingDuration: myJSON.RemainingDuration,
                    taskName: myJSON.TaskName,
                    percentComplited: myJSON.PercentComplited,
                    contractDateComplited: myJSON.ContractDateComplited,
                    name: myJSON.OrderNumber,
                    nameN: myJSON.UserName,
                    color: myJSON.Color,
                    data: data,
                    current: myJSON.Deals[myJSON.Current]
                };
            });
            Highcharts.setOptions({
                lang: {
                    loading: 'Загрузка...',
                    months: ['01', '02', 'Мар', 'Апр', 'Май', 'Июн', 'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
                    weekdays: ['Вc', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
                    shortMonths: ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
                    rangeSelectorFrom: "С",
                    rangeSelectorTo: "По",
                    rangeSelectorZoom: "Период",
                    week: 'Нед',
                    Start: 'Начало'
                },
                credits: {
                    enabled: false
                }
            });
            Highcharts.ganttChart('criticalRoad', {
                series: series,
                plotOptions: {
                    series: {
                        animation: false,
                        dataLabels: {
                            enabled: true,
                            format: '{point.percentComplited}',
                            style: {
                                color: "contrast",
                                fontSize: pointWidthForGantt - minusPxTextForGantt,
                                fontWeight: "bold",
                                textOutline: "1px contrast"
                            }
                        },
                        allowPointSelect: true,
                        events: {
                            click: function (event) {
                                click: GetUserGantt(this.userOptions.nameN);
                            }
                        }
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
                    pointFormat: '<span>Задача: {point.taskName}</span><br/><span>Начало: {point.start:%e. %b}</span><br/><span>Окончание: {point.end:%e. %b}</span>'
                },
                xAxis: [{
                    min: minDate,
                    max: maxDate,
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - minusPxTextForGantt
                        }
                    },
                    endOnTick: true,
                    dateTimeLabelFormats: {
                        day: '%e.%b',
                        hour: '%e.%b',
                        millisecond: '%e.%b',
                        minute: '%e.%b',
                        month: 'Нед. %W',
                        second: '%e.%b',
                        week: 'Нед. %W',
                        year: 'Нед. %W'
                    }
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
                        columns: [
                            {
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
                                    text: 'Ост.тр',
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
                                    text: 'Начало',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return dateFormat('%e. %b', s.dataOtgruzkiBP);
                                })
                            },
                            {
                                title: {
                                    text: 'Окончание',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return dateFormat('%e. %b', s.contractDateComplited);
                                })
                            },
                            {
                                title: {
                                    text: 'Исполнитель',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return s.name;
                                })
                            },
                            {
                                title: {
                                    text: 'Задача',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return s.taskName;
                                })
                            }]
                    }
                },
                chart: {
                    height: heightDiagramm
                }
            });
        }
    });
}

function GetHSSPO() {
    $.ajax({
        url: "/VBP/GetHSSPO/",
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
            Highcharts.chart('hssPO', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'line'
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    name: 'ХСС',
                    data: dataArray,
                    color: colorDiagramm
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
                            style: {
                                color: "#0d233a" 
                            }
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

var cmoTableData = [
    { "title": "Позиция", "data": "position", "autowidth": true, "bSortable": true },
    { "title": "Подрядчик", "data": "customer", "autowidth": true, "bSortable": true },
    { "title": "Размещен", "data": "dateOpen", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Ожидается", "data": "criticalDate", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "Поступил", "data": "dateComplited", "autowidth": true, "bSortable": true, "className": 'text-center', "defaultContent": "", "render": processNull },
    { "title": "№ заявки", "data": "orderNumber", "autowidth": true, "bSortable": true, "className": 'text-center' }
];

function GetOrderCMOTable(id) {
    var table = $('#tableCMOOrder').DataTable();
    table.destroy();
    $('#tableCMOOrder').empty();
    $("#tableCMOOrder").DataTable({
        "dom": '<"toolbar1">frtip',
        "ajax": {
            "cache": false,
            "url": "/VBP/GetOrderCMOTable/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "asc"]],
        "processing": true,
        "columns": cmoTableData,
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
}

function GetBurndownDiagramM(id) {
    $.ajax({
        url: "/VBP/GetBurndownDiagramM/" + id,
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArrayBP = new Array();
            var dataArrayP = new Array();
            var dataArrayI = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].Week;
                dataArrayBP[i] = result[i].ValueBP;
                dataArrayP[i] = result[i].ValueP;
                dataArrayI[i] = result[i].ValueI;
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('burndownDiagramM', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'line'
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    name: 'БПлан',
                    data: dataArrayBP,
                    color: colorDiagramm
                },
                {
                    name: 'План',
                    data: dataArrayP,
                    color: "#008000"
                },
                {
                    name: 'Оптимум',
                    data: dataArrayI,
                    color: "#191970"
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
                            style: {
                                color: "#0d233a"
                            }
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

function GetBurndownDiagramE(id) {
    $.ajax({
        url: "/VBP/GetBurndownDiagramE/" + id,
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArrayBP = new Array();
            var dataArrayP = new Array();
            var dataArrayI = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].Week;
                dataArrayBP[i] = result[i].ValueBP;
                dataArrayP[i] = result[i].ValueP;
                dataArrayI[i] = result[i].ValueI;
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('burndownDiagramE', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'line'
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    name: 'БПлан',
                    data: dataArrayBP,
                    color: colorDiagramm
                },
                {
                    name: 'План',
                    data: dataArrayP,
                    color: "#008000"
                },
                {
                    name: 'Оптимум',
                    data: dataArrayI,
                    color: "#191970"
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
                            style: {
                                color: "#0d233a"
                            }
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

function GetBurndownDiagramP(id) {
    $.ajax({
        url: "/VBP/GetBurndownDiagramP/" + id,
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArrayBP = new Array();
            var dataArrayP = new Array();
            var dataArrayI = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].Week;
                dataArrayBP[i] = result[i].ValueBP;
                dataArrayP[i] = result[i].ValueP;
                dataArrayI[i] = result[i].ValueI;
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('burndownDiagramP', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'line'
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: catigoriesArray,
                    style: {
                        width: '100px'
                    }
                },
                series: [{
                    name: 'БПлан',
                    data: dataArrayBP,
                    color: colorDiagramm
                },
                {
                    name: 'План',
                    data: dataArrayP,
                    color: "#008000"
                },
                {
                    name: 'Оптимум',
                    data: dataArrayI,
                    color: "#191970"
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
                            style: {
                                color: "#0d233a"
                            }
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

function GetTableApproveCD(id) {
    var table = $('#tableApproveCD').DataTable();
    table.destroy();
    $('#tableApproveCD').empty();
    $("#tableApproveCD").DataTable({
        "dom": '<"toolbar1">frtip',
        "ajax": {
            "cache": false,
            "url": "/VBP/GetNoApproveTable/" + id,
            "type": "POST",
            "datatype": "json"
        },
        "order": [[0, "asc"]],
        "processing": true,
        "columns": objOrders,
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
}

function GetPerfomance() {
    $.ajax({
        url: "/VBP/GetPerfomance/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSONRemainingPlan = new Array();
            myJSONRemainingPlan[0] = result[0];
            var colorLen = '#2b908f';
            if (result[0] > 60) {
                colorLen = '#2b908f';
            }
            else if (result[0] > 56) {
                colorLen = '#faffb3';
            }
            else {
                colorLen = '#910000';
            }
            Highcharts.setOptions({
                credits: {
                    enabled: false
                }
            });
            Highcharts.chart('perfomancePO', {
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
                    text: 'Производительность',
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
                    max: 100,
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
                    name: 'Производительность',
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

function GetUserGantt(id) {
    $.ajax({
        url: "/VBP/GetUserGantt/" + id,
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var myJSON = JSON.parse(JSON.stringify(result));
            var lenghtElements = Object.keys(myJSON).length;
            var heightDiagramm = lenghtElements * 14 + 80;
            heightDiagramm += 'px';
            var minDate = converDateJSON(myJSON[0].DataOtgruzkiBP);
            var maxDate = converDateJSON(myJSON[0].DataOtgruzkiBP);
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
                myJSON[i].TaskName = myJSON[i].TaskName;
                if (myJSON[i].DataOtgruzkiBP < minDate) {
                    minDate = myJSON[i].DataOtgruzkiBP;
                }
                if (myJSON[i].ContractDateComplited > maxDate) {
                    maxDate = myJSON[i].ContractDateComplited;
                }
            }
            var minCorrectDate = new Date();
            minCorrectDate = minCorrectDate.getTime();
            if (minDate < minCorrectDate) {
                minDate = minCorrectDate;
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
                        rentedTo: deal.User,
                        start: deal.From,
                        end: deal.To,
                        color: "#910000",
                        dependency: 'prototype',
                        name: renderToNullString(deal.User, deal.Milestone),
                        pointWidth: pointWidthForGantt,
                        milestone: deal.Milestone,
                        taskName: myJSON.TaskName,
                        percentComplited: myJSON.PercentComplited,
                        y: i
                    };
                });
                return {
                    dataOtgruzkiBP: myJSON.DataOtgruzkiBP,
                    remainingDuration: myJSON.RemainingDuration,
                    taskName: myJSON.TaskName,
                    percentComplited: myJSON.PercentComplited,
                    contractDateComplited: myJSON.ContractDateComplited,
                    name: myJSON.OrderNumber,
                    color: myJSON.Color,
                    data: data,
                    current: myJSON.Deals[myJSON.Current]
                };
            });
            Highcharts.setOptions({
                lang: {
                    loading: 'Загрузка...',
                    months: ['01', '02', 'Мар', 'Апр', 'Май', 'Июн', 'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
                    weekdays: ['Вc', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
                    shortMonths: ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
                    rangeSelectorFrom: "С",
                    rangeSelectorTo: "По",
                    rangeSelectorZoom: "Период",
                    week: 'Нед',
                    Start: 'Начало'
                },
                credits: {
                    enabled: false
                }
            });
            Highcharts.ganttChart('ganttUser', {
                series: series,
                plotOptions: {
                    series: {
                        animation: false,
                        dataLabels: {
                            enabled: true,
                            format: '{point.percentComplited}',
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
                    pointFormat: '<span>Задача: {point.taskName}</span><br/><span>Начало: {point.start:%e. %b}</span><br/><span>Окончание: {point.end:%e. %b}</span>'
                },
                xAxis: [{
                    min: minDate,
                    max: maxDate,
                    labels: {
                        style: {
                            "color": "#0d233a",
                            "fontSize": pointWidthForGantt - minusPxTextForGantt
                        }
                    },
                    endOnTick: true,
                    dateTimeLabelFormats: {
                        day: '%e.%b',
                        hour: '%e.%b',
                        millisecond: '%e.%b',
                        minute: '%e.%b',
                        month: 'Нед. %W',
                        second: '%e.%b',
                        week: 'Нед. %W',
                        year: 'Нед. %W'
                    }
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
                        columns: [
                            {
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
                                    text: 'Ост.тр',
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
                                    text: 'Начало',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return dateFormat('%e. %b', s.dataOtgruzkiBP);
                                })
                            },
                            {
                                title: {
                                    text: 'Окончание',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return dateFormat('%e. %b', s.contractDateComplited);
                                })
                            },
                            {
                                title: {
                                    text: 'Исполнитель',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return s.name;
                                })
                            },
                            {
                                title: {
                                    text: 'Задача',
                                    style: {
                                        "color": "#0d233a",
                                        "fontSize": pointWidthForGantt - minusPxTextForGantt
                                    }
                                },
                                categories: map(series, function (s) {
                                    return s.taskName;
                                })
                            }]
                    }
                },
                chart: {
                    height: heightDiagramm
                }
            });
        }
    });
    $('#ganttUserModal').modal('show');
}