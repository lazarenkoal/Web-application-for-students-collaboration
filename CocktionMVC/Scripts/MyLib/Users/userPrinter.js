//Функция добавляет пользователя 
//name - имя пользователя
//surname - фамилия пользователя
//photoPath - путь к фотке
//rating - рэйтинг пользователя
//eggs - яйца, которые пользователь заработал
//auctionsAmount - количество аукционов
function addUser(name, surname, id ,photoPath, rating, eggs, auctionsAmount, isFirst, isSubscribed, university) {
    if (!isFirst) {
        $("#usersHere").append(
            "<div class=\"userInfo\"><div class=\"row\"><div class=\"col-md-4\"> <img src=\"" + photoPath + "\"class=\"img-thumbnail\">" +
            "</div><div class=\"col-md-8\"> <p><b><a href=\"http://cocktion.com/Users/GetUser/"+id+"\">" + name + ' ' + surname + "</a></b></p><p><b>Рейтинг:</b>" + rating +
            "</p> <p><b>Монетки:</b>" + eggs + "</p>" + "<p><b>Аукционов: </b>" + auctionsAmount + "</p>" + "<p><b>Университет: </b>" + university + 
            "</div> </div>"+displaySubscriptionStatus(id, isSubscribed)+" </div><br/>" //добавить кнопку дисплея кнопки
        );
    } else {
        $("#usersHere").append(
            "<div class=\"userInfo\"><div class=\"row\"><div class=\"col-md-4\"> <img src=\"" + photoPath + "\"class=\"img-thumbnail\">" +
            "</div><div class=\"col-md-8\"> <p><b><a href=\"http://cocktion.com/Users/GetUser/" + id + "\">" + name + ' ' + surname + "</a></b></p><p><b>Рейтинг:</b>" + rating +
            "</p> <p><b>Монетки:</b>" + eggs + "</p>" + "<p><b>Аукционов: </b>" + auctionsAmount + "</p>" + "<p><b>Университет: </b>" + university +
            "</div> </div>" + displaySubscriptionStatus(id, isSubscribed) + " </div>"
        );
    }
}
//<div class=\"col-md-4\"><p><button class=\"btn btn-default\"></button></p></div>

function displaySubscriptionStatus(id, isSubscribed) {
    if (isSubscribed == 'True') {
        //если подписан - паказываем ему, что у него в колхозе все в поряде
        return "<p>У вас в информаторах! <span class=\"glyphicon glyphicon-ok\"></span></p>";
    } else {
        //если не подписан - показываем кнопку
        return "<br/><p><button class=\"btn btn-default\" onclick=\"subscribeOnUser('" +
            id+"')\" id=\""+id+"\">Подписаться</button></p>";
    }
}

function addUnauthUser(name, surname, id, photoPath, rating, eggs, auctionsAmount, isFirst, isSubscribed, university) {
    if (!isFirst) {
        $("#usersHere").append(
            "<div class=\"userInfo\"><div class=\"row\"><div class=\"col-md-4\"> <img src=\"" + photoPath + "\"class=\"img-thumbnail\">" +
            "</div><div class=\"col-md-8\"> <p><b><a href=\"http://cocktion.com/Users/GetUser/" + id + "\">" + name + ' ' + surname + "</a></b></p><p><b>Рейтинг:</b>" + rating +
            "</p> <p><b>Монетки:</b>" + eggs + "</p>" + "<p><b>Аукционов: </b>" + auctionsAmount + "</p>" + "<p><b>Университет: </b>" + university +
            "</div> </div></div><br/>" //добавить кнопку дисплея кнопки
        );
    } else {
        $("#usersHere").append(
            "<div class=\"userInfo\"><div class=\"row\"><div class=\"col-md-4\"> <img src=\"" + photoPath + "\"class=\"img-thumbnail\">" +
            "</div><div class=\"col-md-8\"> <p><b><a href=\"http://cocktion.com/Users/GetUser/" + id + "\">" + name + ' ' + surname + "</a></b></p><p><b>Рейтинг:</b>" + rating +
            "</p> <p><b>Монетки:</b>" + eggs + "</p>" + "<p><b>Аукционов: </b>" + auctionsAmount + "</p>" + "<p><b>Университет: </b>" + university +
            "</div> </div></div>"
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