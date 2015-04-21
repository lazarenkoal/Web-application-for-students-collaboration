function highlightUser(divelem) {
    //$("#" + divelem.id).css('background-color', 'aliceblue');
    divelem.style.backgroundColor = 'aliceblue';
}

function dehighlightUser(divelem) {
    //$("#" + divelem.id).css('background-color', 'white');
    divelem.style.backgroundColor = 'white';

}



function showHisMessages(divelem) {
    var name = divelem.getAttribute('value');
    responderName = name;
    $("#messages").empty();
    $("#userHeader").empty();
    $("#userHeader").append(name);
    chat.server.getMessages(responderName, thisUserName);
    $("#messages").animate({ scrollTop: $("#messages")[0].scrollHeight }, 100);
}

function generateMessageString(author, message, date) {
    return "<p><b>" + author + ": </b>" + message +" " + date + "</p>";
}

function addUserToList(username, photoPath) {
    var textToAppend = "<div class=\"userToSpeak row\"" +"value=\""+username+"\""+" onclick=\"showHisMessages(this)\"" +
            "onmouseout=\"dehighlightUser(this)\" onmouseover=\"highlightUser(this)\">" +
            "<div class=\"col-md-3\"><img class=\"img-circle\" src=\"" + photoPath + "\"></p></div>" +
            "<div class=\"col-md-9\"><p>" + username + "</p></div></div>";
    $("#usersList").append(textToAppend);
    return textToAppend;
}


