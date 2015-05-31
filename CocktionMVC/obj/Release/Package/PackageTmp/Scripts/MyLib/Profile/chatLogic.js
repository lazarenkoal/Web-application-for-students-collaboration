//Подсвечивает пользователя, при наведении на него мыши
function highlightUser(divelem) {
    divelem.style.backgroundColor = 'aliceblue';
}

//убирает подсветку при отведении мыши
function dehighlightUser(divelem) {
    divelem.style.backgroundColor = 'white';
}



//генерирует строку для вывода в чате
function generateMessageString(author, message, date) {
    return "<p><b>" + author + ": </b>" + message +" " + date + "</p>";
}

//добавляет пользователя в список людей, с которыми ты счас общаешься
function addUserToList(username, photoPath) {
    var textToAppend = "<div class=\"userToSpeak row\"" +"value=\""+username+"\""+" onclick=\"showHisMessages(this, '@Model.Id')\"" +
            "onmouseout=\"dehighlightUser(this)\" onmouseover=\"highlightUser(this)\">" +
            "<div class=\"col-md-6\"><p>" + username + "</p></div></div>";
    $("#usersList").append(textToAppend);
    return textToAppend;
}


