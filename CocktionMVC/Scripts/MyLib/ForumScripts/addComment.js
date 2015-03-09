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
                $("#messageContainer").append(respond.AuthorName);
                $("#messageContainer").append(respond.Message);
            }
        }
    }
    else
    {
        alert("Необходимо ввести сообщение свое!");
    }
}