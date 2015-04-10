//Добавляет ячейку с информацией об аукционе в таблицу
//containerName - название элемента, в котором содержится таблица
//name - наименование аукциона
//description - описание аукциона
//date - дата окончания аукциона
//imgSrc - путь к фотке аукциона
//link - ссылка на этот аукцион
function addCellToTheGrid(containerName, name, description, date, imgSrc, link) {
    if (colCounter < 4) {
        if (colCounter == 0) {
            addRow(containerName, rowCounter);
            appendAuctionInfo(rowCounter, name, description, date, imgSrc, link);
            colCounter++;
        }
        else {
            appendAuctionInfo(rowCounter, name, description, date, imgSrc, link);
            colCounter++;
        }
    }
    else {
        colCounter = 0;
        ++rowCounter;
        addRow(containerName, rowCounter);
        appendAuctionInfo(rowCounter, name, description, date, imgSrc, link);
        colCounter++;
    }
}

//Добавляет col с аукционом в нужный row
//nameOfRow - название row, в который нужно вставить
//description - описание аукциона
//date - дата окончания
//imgSrc - путь к фотке
//link - ссылка на аукцион
function appendAuctionInfo(nameOfRow, name, description, date, imgSrc, link) {
    document.getElementById(nameOfRow).innerHTML += ("<div class=\"col-md-3\"><div class=\"auction\"> " +
        "<img class=\"img-circle\" src=\"" + imgSrc + "\" >" +
    "<p><b>Продается: </b>" + name + "</p>" +
    "<p><b>Описание: </b>" + description + "</p>" +
    "<p><b>Кончится: </b>" + date + "</p>" +
    "<p id=\"auctionLink\"><a href=\"" + link + "\">Заглянуть</a></p>" +
    "</div></div>");
}

//Добавляет новый div (class=row) на страничку, для того, чтобы в нее потом запихивать колонки
function addRow(containerName, nameOfRow) {
    //$("#panelContainer").append("<div class=\"row\" id=\"" + nameOfRow+"\">добавил!</div>");
    document.getElementById(containerName).innerHTML += "<div class=\"row\" id=\"" + nameOfRow + "\"></div></br>";
}

var rowCounter = 0;
var colCounter = 0;

//Вписывает в указанынй элемент информацию о том, что никто ничего на аукционе не продает
//10.04.2015
function sayThatEmpty(elementName) {
    document.getElementById(elementName).innerHTML = "<p>К сожалению, в данный момент ничего не продается ;(</p>";
}