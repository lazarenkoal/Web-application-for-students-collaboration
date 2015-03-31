//Айдишник нового ряда
var idOfNewRow = -1;

//Функция показывает аукционные дома плиточкой
function printHousesInCells(house, parentId) {
    var parent = document.getElementById(parentId);
    //проверяем количество элементов на больше > 5
    if (numberOfElementsInRow >= 5) {
        //обнулить счетчик
        numberOfElementsInRow = 0;
        ++idOfNewRow;
        //если больше - нужно создать новый ряд
        parent.append("<div class = \"row\" id = \"" +idOfNewRow.toString() + "\"> </div>");

        //добавить в новый ряд колонку
        addColumn(house, idOfNewRow, numberOfElementsInRow);
        
    } else {
        //если все ок - добавляем в текущий row
        var newRow = document.getElementById(idOfNewRow);
        addColumn(house, idOfNewRow, numberOfElementsInRow);
    }
};

/*
    Добавляет новую колонку в ряд.
    house - дом, который надо добавить
    idOfRow - айдишник ряда, в который надо засунуть
    numsInRow - количество элементов в ряду
*/
function addColumn(house, idOfRow, numsInRow) {
    var row = document.getElementById(idOfRow);
    row.append(
        "<div class=\"col-md-2\">" + 
        "<p style=\"font-weight: bold\" >" + house.university + "</p>" + 
        "<p>" + house.faculty + "</p>" + 
        "<p>" + "<a href = \""+ house.link + "\"> Хочу посмотреть! </a>" + "</p>" + "</div>"
        );
    numsInRow ++;
}

//Количество элементов в ряду 
var numberOfElementsInRow = 5;

//инициализация объекта дом
var house =
{
    university : null,
    faculty : null, 
    link: null
};

//Функция инициализирует новый дом, используя текущие параметры
function initializeHouse(university, faculty, link) {
    house.university = university;
    house.faculty = faculty;
    house.link = link;
    return house;
}

