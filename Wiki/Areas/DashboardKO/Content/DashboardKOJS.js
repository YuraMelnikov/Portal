$(document).ready(function () {
    getQuartalUsersResult();
    getUsersM1();
    getUsersM2();
    getUsersM3();
    getPeriodReport();
    getDevisionQuaResult();
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