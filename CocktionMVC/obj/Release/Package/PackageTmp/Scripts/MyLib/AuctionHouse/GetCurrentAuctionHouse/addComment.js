function addComment(modelId) {
    if ($("#message").val().length != 0)
    {
        var formData = new FormData();
        var messageData = $("#message").val();
        $("#message").val("");
        formData.append("message", messageData);
        formData.append("houseId", modelId);
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/AuctionHouse/AddComment");
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var respond = JSON.parse(xhr.responseText);
                if (respond.Status == "Success") {
                    $("#messageContainer").append("<div id=\"postContainer\"><div id=\"author\"><p><b>" + respond.Author + "</b> сказал сегодня:</p></div><div id=\"messageInstance\"><p>" + messageData + "</p></div></div><br /><br />");
                } else
                    $("#messageContainer").prepend("<p>" + "Не удалось!" + "</p>");

            }
        }
    }
    else
    {
        alert("Необходимо ввести сообщение свое!");
    }
}