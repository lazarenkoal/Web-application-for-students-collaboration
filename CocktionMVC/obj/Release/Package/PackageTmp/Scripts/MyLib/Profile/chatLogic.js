function highlightUser(divelem) {
    $("#" + divelem.id).css('background-color', 'aliceblue');
}

function dehighlightUser(divelem) {
    $("#" + divelem.id).css('background-color', 'white');
}


function showHisMessages(divelem) {
    $("#messages").empty();
    $("#userHeader").empty();
    $("#userHeader").append($("#"+divelem.id).attr('value'));
    responderName = $("#" + divelem.id).attr('value');
    chat.server.getMessages(responderName, thisUserName);
}

function generateMessageString(author, message) {
    return "<p><b>" + author + ": </b>" + message + "</p>";
}

function addUserToList(username,id, photoPath) {
    var textToAppend = "<div class=\"userToSpeak row\" id=\"" + id + "\" "+"value=\""+username+"\""+" onclick=\"showHisMessages(this)\"" +
            "onmouseout=\"dehighlightUser(this)\" onmouseover=\"highlightUser(this)\">" +
            "<div class=\"col-md-3\"><img class=\"img-circle\" src=\"" + photoPath + "\"></p></div>" +
            "<div class=\"col-md-9\"><p>" + username + "</p></div></div>";
    $("#usersList").append(textToAppend);
}


