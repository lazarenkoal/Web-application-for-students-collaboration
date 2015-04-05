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
            "</div> </div> </div><br/>"
        );
    } else {
        $("#usersHere").append(
            "<div class=\"userInfo\"><div class=\"row\"><div class=\"col-md-4\"> <img src=\"" + photoPath + "\"class=\"img-thumbnail\">" +
            "</div><div class=\"col-md-8\"> <p><b><a href=\"http://cocktion.com/Users/User/" + id + "\">" + name + ' ' + surname + "</a></b></p><p><b>Рейтинг:</b>" + rating +
            "</p> <p><b>Яйца:</b>" + eggs + "</p>" + "<p><b>Аукционов: </b>" + auctionsAmount + "</p>" +
            "</div> </div> </div>"
        );
    }
}