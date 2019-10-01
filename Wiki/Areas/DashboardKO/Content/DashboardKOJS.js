$(document).ready(function () {
    getQuartalUsersResult();
    getUsersM1();
    getUsersM2();
    getUsersM3();
    getPeriodReport();
    getDevisionQuaResult();
    getDevisionM1();
    getDevisionM2();
    getDevisionM3();
    getRemainingWorkAll();
    getRemainingWork();
    getRemainingWorkDevisionAll();
    getRemainingDevisionWork();
    getRemainingWorkAllE();
    getRemainingWorkE();
    getHSSPO();
    getHSSKBM();
    getHSSKBE();
    getTimeSheet();
    manpowerUsersInMonth();
});

function getQuartalUsersResult() {
    $.ajax({
        url: "/ReportPage/GetUsersQuaResult/",
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
            Highcharts.chart('quartalUsersResult', {
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
                    text: 'Квартальные НЧ',
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

function getUsersM1() {
    $.ajax({
        url: "/ReportPage/GetUsersM1/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].month;
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
            Highcharts.chart('usersM1', {
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
                    text: labelName,
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
                    color: '#2b908f',
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

function getUsersM2() {
    $.ajax({
        url: "/ReportPage/GetUsersM2/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].month;
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
            Highcharts.chart('usersM2', {
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
                    text: labelName,
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
                    color: '#2b908f',
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

function getUsersM3() {
    $.ajax({
        url: "/ReportPage/GetUsersM3/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].month;
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
            Highcharts.chart('usersM3', {
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
                    text: labelName,
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
                    color: '#2b908f',
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

function getDevisionQuaResult() {
    $.ajax({
        url: "/ReportPage/GetDevisionQuaResult/",
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
            Highcharts.chart('quartalDevisionResult', {
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
                    text: 'Квартальные НЧ',
                    style: {
                        "font-size": "13px"
                    },
                    margin: 0
                },
                xAxis: {
                    categories: catigoriesArray
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

function getDevisionM1() {
    $.ajax({
        url: "/ReportPage/GetDevisionM1Result/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].month;
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
            Highcharts.chart('DevisionResultM1', {
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
                    text: labelName,
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
                    color: '#2b908f',
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

function getDevisionM2() {
    $.ajax({
        url: "/ReportPage/GetDevisionsM2Result/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].month;
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
            Highcharts.chart('DevisionResultM2', {
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
                    text: labelName,
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
                    color: '#2b908f',
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

function getDevisionM3() {
    $.ajax({
        url: "/ReportPage/GetDevisionsM3Result/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].month;
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
            Highcharts.chart('DevisionResultM3', {
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
                    text: labelName,
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
                    color: '#2b908f',
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

function getRemainingWorkAll() {
    $.ajax({
        url: "/ReportPage/GetRemainingWorkAll/",
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
            Highcharts.chart('remainingWorkAll', {
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
                    text: 'Оставшиеся тр-ты с учетом НИОКРов (сотрудник)',
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
                    text: 'Оставшиеся тр-ты по заказам (сотрудник)',
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

function getRemainingWorkDevisionAll() {
    $.ajax({
        url: "/ReportPage/GetRemainingDevisionWorkAll/",
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
            Highcharts.chart('remainingWorkDevisionAll', {
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
                    text: 'Оставшиеся тр-ты с учетом НИОКРов (бюро)',
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

function getRemainingDevisionWork() {
    $.ajax({
        url: "/ReportPage/GetRemainingDevisionWork/",
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
            Highcharts.chart('remainingWorkDevision', {
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
                    text: 'Оставшиеся тр-ты по заказам (бюро)',
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

function getRemainingWorkAllE() {
    $.ajax({
        url: "/ReportPage/GetRemainingWorkAllE/",
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
            Highcharts.chart('remainingWorkAllE', {
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
                    text: 'Оставшиеся тр-ты с учетом НИОКРов (сотрудник)',
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
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
                    text: 'Оставшиеся тр-ты по заказам (сотрудник)',
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

function getHSSPO() {
    $.ajax({
        url: "/ReportPage/GetHSSPO/",
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
                    text: 'ХСС производства',
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
                    name: 'ХСС',
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

function getHSSKBM() {
    $.ajax({
        url: "/ReportPage/GetHSSKBM/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            var dataRealArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
                dataRealArray[i] = result[i].realHss;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
            Highcharts.chart('hssKBM', {
                legend: {
                    enabled: true
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
                    text: 'ХСС КБМ',
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
                    name: 'ХСС',
                    data: dataArray
                }, {
                    color: '#910000',
                    name: 'ХСС (с перетоками)',
                    data: dataRealArray
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

function getHSSKBE() {
    $.ajax({
        url: "/ReportPage/GetHSSKBE/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = Object.keys(result).length;
            var catigoriesArray = new Array();
            var dataArray = new Array();
            var dataRealArray = new Array();
            for (var i = 0; i < lenghtArrayResult; i++) {
                catigoriesArray[i] = result[i].userName;
                dataArray[i] = result[i].count;
                dataRealArray[i] = result[i].realHss;
            }
            var catigoriesJSON = JSON.stringify(catigoriesArray);
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
            Highcharts.chart('hssKBE', {
                legend: {
                    enabled: true
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
                    text: 'ХСС КБЭ',
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
                    name: 'ХСС',
                    data: dataArray
                }, {
                    color: '#910000',
                    name: 'ХСС (с перетоками)',
                    data: dataRealArray
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

function getTimeSheet() {
    $.ajax({
        url: "/ReportPage/GetTimesheet/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var lenghtArrayResult = result[0].stepDate;
            var dateArray = new Array();
            for (var i = 1; i < lenghtArrayResult + 1; i++) {
                dateArray.push(result[i].date);
            }
            var usersArray = new Array();
            lenghtArrayResult = Object.keys(result).length;
            for (i = 1; i < lenghtArrayResult; i = i + result[0].stepDate) {
                usersArray.push(result[i].user);
            }
            var dataArray = new Array();
            var dataForArray = new Array();
            for (i = 1; i < lenghtArrayResult; i++) {
                dataForArray = [result[i].stepDate, result[i].stepUser, result[i].data];
                dataArray.push(dataForArray);
            }
            Highcharts.chart('timesheetContainer', {
                legend: {
                    enabled: false
                },
                navigation: {
                    buttonOptions: {
                        enabled: false
                    }
                },
                chart: {
                    type: 'heatmap'
                },
                title: {
                    text: '* согласно проставленным часам в timesheet',
                    style: {
                        "font-size": "13px"
                    },
                    margin: 0
                },
                xAxis: {
                    categories: dateArray
                },
                yAxis: {
                    categories: usersArray,
                    title: null
                },
                colorAxis: {
                    min: 0,
                    minColor: '#ffffff',
                    maxColor: '#90ed7d'
                },
                series: [{
                    name: 'ч.',
                    borderWidth: 1,
                    data: dataArray,
                    dataLabels: {
                        enabled: true,
                        color: '#000000'
                    }
                }]
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function manpowerUsersInMonth() {
    //01
    $.ajax({
        url: "/ReportPage/GetUsersMMP1/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            document.getElementById("periodReportUsersKBMString").textContent = 'Выработка НЧ КБМ за ' + labelName;
            document.getElementById("periodReportUsersKBEString").textContent = 'Выработка НЧ КБЭ за ' + labelName;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container1', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Васюхневич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                            color: '#FFED66',
                            label: {
                                "text": dataArray10,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                            color: '#D8D8D8',
                            label: {
                                "text": dataArray20,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                            color: '#00CECB',
                            label: {
                                "text": dataArray30,
                                align: 'right',
                                x: 10,
                                y: -10
                            }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //02
    $.ajax({
        url: "/ReportPage/GetUsersMMP2/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container2', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Волкова']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //03
    $.ajax({
        url: "/ReportPage/GetUsersMMP3/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container3', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Глебик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //04
    $.ajax({
        url: "/ReportPage/GetUsersMMP4/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container4', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Кальчинский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //05
    $.ajax({
        url: "/ReportPage/GetUsersMMP5/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container5', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Маляревич']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //06
    $.ajax({
        url: "/ReportPage/GetUsersMMP6/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container6', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Носик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //07
    $.ajax({
        url: "/ReportPage/GetUsersMMP7/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container7', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Фейгина']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //08
    $.ajax({
        url: "/ReportPage/GetUsersMMP8/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container8', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Добыш']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //09
    $.ajax({
        url: "/ReportPage/GetUsersMMP9/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container9', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Жибуль']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //10
    $.ajax({
        url: "/ReportPage/GetUsersMMP10/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container10', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Жук']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //11
    $.ajax({
        url: "/ReportPage/GetUsersMMP11/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container11', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Климович']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //12
    $.ajax({
        url: "/ReportPage/GetUsersMMP12/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container12', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Кучинский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //13
    $.ajax({
        url: "/ReportPage/GetUsersMMP13/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container13', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Тимашкова']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //14
    $.ajax({
        url: "/ReportPage/GetUsersMMP14/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container14', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Тиханский']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
                        }
                    }
                }
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //15
    $.ajax({
        url: "/ReportPage/GetUsersMMP15/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var labelName = result[0].period;
            var lenghtArrayResult = Object.keys(result).length;
            var normHoure = 0;
            var normHoureFact = 0;
            var dataArrayPlan = 0;
            var dataArray10 = 0;
            var dataArray20 = 0;
            var dataArray30 = 0;
            for (var i = 0; i < 1; i++) {
                normHoure = result[i].normHoure;
                normHoureFact = result[i].normHoureFact;
                dataArrayPlan = result[i].plan;
                dataArray10 = result[i].plan10;
                dataArray20 = result[i].plan20;
                dataArray30 = result[i].plan30;
            }
            Highcharts.chart('container15', {
                chart: {
                    marginTop: 40,
                    inverted: true,
                    marginLeft: 135,
                    type: 'bullet'
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    enabled: false
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: ['Филончик']
                },
                yAxis: {
                    min: 0,
                    max: dataArray30,
                    plotBands: [{
                        from: 0,
                        to: dataArrayPlan,
                        color: '#FF5E5B',
                        label: {
                            "text": dataArrayPlan,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArrayPlan,
                        to: dataArray10,
                        color: '#FFED66',
                        label: {
                            "text": dataArray10,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray10,
                        to: dataArray20,
                        color: '#D8D8D8',
                        label: {
                            "text": dataArray20,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }, {
                        from: dataArray20,
                        to: dataArray30,
                        color: '#00CECB',
                        label: {
                            "text": dataArray30,
                            align: 'right',
                            x: 10,
                            y: -10
                        }
                    }],
                    title: null,
                    gridLineWidth: 0
                },
                series: [{
                    data: [{
                        y: normHoureFact,
                        target: normHoure
                    }]
                }],
                tooltip: {
                    pointFormat: '<b>{point.y}</b> (Фактические НЧ: {point.target})'
                },
                plotOptions: {
                    series: {
                        pointPadding: 0.25,
                        borderWidth: 0,
                        color: '#000',
                        targetOptions: {
                            width: '200%'
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