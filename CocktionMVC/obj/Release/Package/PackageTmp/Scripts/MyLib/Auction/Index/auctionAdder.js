//Добавляет ячейку с информацией об аукционе в таблицу
//containerName - название элемента, в котором содержится таблица
//name - наименование аукциона
//description - описание аукциона
//date - дата окончания аукциона
//imgSrc - путь к фотке аукциона
//link - ссылка на этот аукцион
function addCellToTheGrid(containerName, name, description, date, imgSrc, link) {
    if (auctionColCounter < 4) {
        if (auctionColCounter == 0) {
            addAuctionRow(containerName, auctionRowCounter);
            appendAuctionInfo(auctionRowCounter, name, description, date, imgSrc, link);
            auctionColCounter++;
        }
        else {
            appendAuctionInfo(auctionRowCounter, name, description, date, imgSrc, link);
            auctionColCounter++;
        }
    }
    else {
        auctionColCounter = 0;
        ++auctionRowCounter;
        addAuctionRow(containerName, auctionRowCounter);
        appendAuctionInfo(auctionRowCounter, name, description, date, imgSrc, link);
        auctionColCounter++;
    }
}

//Добавляет col с аукционом в нужный row
//nameOfRow - название row, в который нужно вставить
//description - описание аукциона
//date - дата окончания
//imgSrc - путь к фотке
//link - ссылка на аукцион
function appendAuctionInfo(nameOfRow, name, description, date, imgSrc, link) {
    document.getElementById('auction'+nameOfRow).innerHTML += ("<div class=\"col-md-3\"><div class=\"auction\"> " +
        "<img class=\"img-circle\" src=\"" + imgSrc + "\" >" +
    "<p><b>Продается: </b>" + name + "</p>" +
    "<p><b>Описание: </b>" + description.trim() + "</p>" +
    "<p><b>Кончится: </b>" + date + "</p>" +
    "<p id=\"auctionLink\"><a href=\"" + link + "\">Заглянуть</a></p>" +
    "</div></div>");
}

//Добавляет новый div (class=row) на страничку, для того, чтобы в нее потом запихивать колонки
function addAuctionRow(containerName, nameOfRow) {
    //$("#panelContainer").append("<div class=\"row\" id=\"" + nameOfRow+"\">добавил!</div>");
    document.getElementById(containerName).innerHTML += "<div class=\"row\" id=\"auction" + nameOfRow + "\"></div></br>";
}

var auctionRowCounter = 0;
var auctionColCounter = 0;

//Вписывает в указанынй элемент информацию о том, что никто ничего на аукционе не продает
//10.04.2015
function sayThatEmpty(elementName) {
    document.getElementById(elementName).innerHTML = "<p>К сожалению, в данный момент ничего не продается ;(</p>";
}