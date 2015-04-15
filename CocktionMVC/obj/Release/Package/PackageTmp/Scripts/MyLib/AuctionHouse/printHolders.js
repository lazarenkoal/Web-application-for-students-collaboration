function addHolderCell(containerName, holderName, link, imgSrc) {
    if (holderColCounter < 4) {
        if (holderColCounter == 0) {
            addHolderRow(containerName, holderRowCounter);
            appendHolderInfo(holderRowCounter, holderName, link, imgSrc);
            holderColCounter++;
        }
        else {
            appendHolderInfo(holderRowCounter, holderName, link, imgSrc);
            holderColCounter++;
        }
    }
    else {
        holderColCounter = 0;
        ++holderRowCounter;
        addHolderRow(containerName, holderRowCounter);
        appendHolderInfo(holderRowCounter, holderName, link, imgSrc);
        holderColCounter++;
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
function addHolderRow(containerName, nameOfRow) {
    document.getElementById(containerName).innerHTML += "<div class=\"row\" id=\"" + nameOfRow + "\"></div></br>";
}

var holderRowCounter = 0;
var holderColCounter = 0;
