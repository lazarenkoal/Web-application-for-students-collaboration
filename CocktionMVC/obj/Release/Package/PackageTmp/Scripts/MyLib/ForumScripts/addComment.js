function addComment(modelId) {
    if ($("#message").val().length != 0)
    {
        var formData = new FormData();
        formData.append("message", $("#message").val());
        formData.append("houseId", modelId);
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/AuctionHouse/AddComment");
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var respond = JSON.parse(xhr.responseText);
                if (respond.Status == "Success") {
                    $("#messageContainer").prepend("<p>" + $("#message").val() + "</p>");
                    $("#messageContainer").prepend("<p>" + respond.Author + "</p>");
                    $("#message").val("");
                }
                else
                {
                    $("#messageContainer").prepend("<p>" + "Не удалось!" + "</p>");
                }
            }
        }
    }
    else
    {
        alert("Необходимо ввести сообщение свое!");
    }
}