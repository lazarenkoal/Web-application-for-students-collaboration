/**
 * Created by aleksandrlazarenko on 10.04.15.
 */

//Распределяет новую ячейку по сетки со столбцами
//nameOfRow - название строки, в которую надо вставить все
//nameOfFaculty - название факультета
//adress - адрес факультета
//link - ссылочка на факультет
//imgSrc - путь к фоточке
//containerName - название контейнера, в который надо вставить
function addHouseCell(containerName, nameOfFaculty, adress, link, imgSrc) {
    if (colCounter < 4) {
        if (colCounter == 0) {
            addRow(containerName, rowCounter);
            appendHouseInfo(rowCounter, nameOfFaculty, adress, link, imgSrc);
            colCounter++;
        }
        else {
            appendHouseInfo(rowCounter, nameOfFaculty, adress, link, imgSrc);
            colCounter++;
        }
    }
    else {
        colCounter = 0;
        ++rowCounter;
        addRow(containerName, rowCounter);
        appendHouseInfo(rowCounter, nameOfFaculty, adress, link, imgSrc);
        colCounter++;
    }
}

//Добавляет непосредственно саму информацию о доме в ячейку
//nameOfRow - название строки, в которую надо вставить все
//nameOfFaculty - название факультета
//adress - адрес факультета
//link - ссылочка на факультет
//imgSrc - путь к фоточке
function appendHouseInfo(nameOfRow, nameOfFaculty, adress, link, imgSrc) {
    document.getElementById(nameOfRow).innerHTML += ("<div class=\"col-md-3\"><div class=\"house\"> " +
    "<img src=\"" + imgSrc + "\" class= \"img-circle\">" + //вставляем фотографию
    "<p><b>Факультет: </b>" + nameOfFaculty + "</p>" +    //название факультета
    "<p><b>Адрес: </b>" + adress + "</p>" + //адрес факультета
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
