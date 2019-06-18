$(document).ready(function () {
    startMenu();
});

function loadData(listId) {
    clearTextBox();
    document.getElementById('pageData').innerHTML = listId;
    if (listId === 1 || listId === "1") {
        activeReclamation();
    }
    else if (listId === 2 || listId === "2") {
        closeReclamation();
    }
    else if (listId === 3 || listId === "3") {
        allReclamation();
    }
}