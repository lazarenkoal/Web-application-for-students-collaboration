//Функция добавляет пользователя 
//name - имя пользователя
//surname - фамилия пользователя
//photoPath - путь к фотке
//rating - рэйтинг пользователя
//eggs - яйца, которые пользователь заработал
//auctionsAmount - количество аукционов
function addUser(name, surname, id ,photoPath, rating, eggs, auctionsAmount, isFirst) {
    if (!isFirst) {
        $("#usersHere").append(
            "<div class=\"userInfo\"><div class=\"row\"><div class=\"col-md-4\"> <img src=\"" + photoPath + "\"class=\"img-thumbnail\">" +
            "</div><div class=\"col-md-8\"> <p><b><a href=\"http://cocktion.com/Users/User/"+id+"\">" + name + ' ' + surname + "</a></b></p><p><b>Рейтинг:</b>" + rating +
            "</p> <p><b>Яйца:</b>" + eggs + "</p>" + "<p><b>Аукционов: </b>" + auctionsAmount + "</p>" +
            "<div class=\"col-md-4\"><p><button class=\"btn btn-default\">В друзья</button></p><p><button class=\"btn btn-default\">Предложить аукцион</button></p></div></div> </div> </div><br/>"
        );
    } else {
        $("#usersHere").append(
            "<div class=\"userInfo\"><div class=\"row\"><div class=\"col-md-4\"> <img src=\"" + photoPath + "\"class=\"img-thumbnail\">" +
            "</div><div class=\"col-md-8\"> <p><b><a href=\"http://cocktion.com/Users/User/" + id + "\">" + name + ' ' + surname + "</a></b></p><p><b>Рейтинг:</b>" + rating +
            "</p> <p><b>Яйца:</b>" + eggs + "</p>" + "<p><b>Аукционов: </b>" + auctionsAmount + "</p>" +
            "<div class=\"col-md-4\"><p><button class=\"btn btn-default\">В друзья</button></p><p><button class=\"btn btn-default\">Предложить аукцион</button></p></div></div> </div> </div>"
        );
    }
}


//проверяет чекбоксы ф чильтрах
function sexCheckboxCheckedHandler(checkId) {
    switch (checkId.id) {
        case 'male':
            $("#female").prop('checked', false);
            break;
        case 'female':
            $("#male").prop('checked', false);
            break;
    }
}