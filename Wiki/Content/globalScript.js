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