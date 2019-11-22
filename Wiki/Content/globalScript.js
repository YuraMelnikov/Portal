function processNull(data) {
    if (data === 'null') {
        return '';
    } else {
        return data;
    }
}

function convertRuDateToDateISO(st) {
    var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
    var dt = new Date(st.replace(pattern, '$3-$2-$1'));
    return dt;
}

function ConvertDateToGlobalShortString(dateTime) {
    var yearString = dateTime.getFullYear() + '.';
    var monthString = dateTime.getMonth() + 1 + '.';
    var dayString = dateTime.getDate();
    if (monthString.length < 3) {
        monthString = '0' + monthString;
    }
    if (dayString.length < 3) {
        dayString = '0' + dayString;
    }

    var dateString = yearString + monthString + dayString;

    return dateString;
}