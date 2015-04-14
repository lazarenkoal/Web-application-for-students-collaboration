function addHolderCell(containerName, holderName, link, imgSrc) {
    if (colCounter < 4) {
        if (colCounter == 0) {
            addRow(containerName, rowCounter);
            appendHolderInfo(rowCounter, holderName, link, imgSrc);
            colCounter++;
        }
        else {
            appendHolderInfo(rowCounter, holderName, link, imgSrc);
            colCounter++;
        }
    }
    else {
        colCounter = 0;
        ++rowCounter;
        addRow(containerName, rowCounter);
        appendHolderInfo(rowCounter, holderName, link, imgSrc);
        colCounter++;
    }
}

//Добавляет непосредственно саму информацию о доме в ячейку
//nameOfRow - название строки, в которую надо вставить все
//nameOfFaculty - название факультета
//adress - адрес факультета
//link - ссылочка на факультет
//imgSrc - путь к фоточке
function appendHolderInfo(nameOfRow, nameOfHolder, link, imgSrc) {
    document.getElementById(nameOfRow).innerHTML += ("<div class=\"col-md-3\"><div class=\"holder\"> " +
    "<img src=\"" + imgSrc + "\" >" + //вставляем фотографию
    "<p><b>Название: </b>" + nameOfHolder + "</p>" +    //название факультета
    "<p><a href=\"" + link + "\">Заглянуть</a></p></div></div>"); //ссылочка
}

//Добавляет новый div (class=row) на страничку, для того, чтобы в нее потом запихивать колонки
//containerName - название контейнера, в который надо вставить
//nameOfRow - название строчки, в которую надо вставить что-то
function addRow(containerName, nameOfRow) {
    document.getElementById(containerName).innerHTML += "<div class=\"row\" id=\"" + nameOfRow + "\"></div></br>";
}

var rowCounter = 0;
var colCounter = 0;
