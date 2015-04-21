//Подсвечивает пользователя, при наведении на него мыши
function highlightUser(divelem) {
    divelem.style.backgroundColor = 'aliceblue';
}

//убирает подсветку при отведении мыши
function dehighlightUser(divelem) {
    divelem.style.backgroundColor = 'white';
}

//подгружает с сервера сообщения пользователя
//надо бы эту функцию умнее сделать!!!
function showHisMessages(divelem) {
    var name = divelem.getAttribute('value');
    responderName = name;
    $("#messages").empty();
    $("#userHeader").empty();
    $("#userHeader").append(name);
    chat.server.getMessages(responderName, thisUserName);
    $("#messages").animate({ scrollTop: $("#messages")[0].scrollHeight }, 100);
}

//генерирует строку для вывода в чате
function generateMessageString(author, message, date) {
    return "<p><b>" + author + ": </b>" + message +" " + date + "</p>";
}

//добавляет пользователя в список людей, с которыми ты счас общаешься
function addUserToList(username, photoPath) {
    var textToAppend = "<div class=\"userToSpeak row\"" +"value=\""+username+"\""+" onclick=\"showHisMessages(this)\"" +
            "onmouseout=\"dehighlightUser(this)\" onmouseover=\"highlightUser(this)\">" +
            "<div class=\"col-md-3\"><img class=\"img-circle\" src=\"" + photoPath + "\"></p></div>" +
            "<div class=\"col-md-9\"><p>" + username + "</p></div></div>";
    $("#usersList").append(textToAppend);
    return textToAppend;
}


//отпавка по кнопке энтер
$(document).ready(function () {
    $('#messageContainer').keypress(function (e) {
        if (e.keyCode == 13)
            $('#sendMessageBtn').click();
    });
});