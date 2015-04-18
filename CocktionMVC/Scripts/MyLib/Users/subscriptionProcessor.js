function subscribeOnUser(userId) {
    var formData = new FormData();
    formData.append('userId', userId);
    var xhr = new XMLHttpRequest();
    var response;
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            response = JSON.parse(xhr.responseText);
            if (response.Status == "Success") {
                document.getElementById(userId).parentElement.innerHTML = "<p>У вас в информаторах! <span class=\"glyphicon glyphicon-ok\"></span></p>";
            } else {
                document.getElementById(userId).innerHTML = "<p>Попробуйте еще раз, пожалуйста... <span class=\"glyphicon glyphicon-ok\"></span></p>";

            }
        };
    }

    xhr.open('POST', '/Subscription/SubscribeOnUser');
    xhr.send(formData); //отправка данных
}