function checkSubscription(userId) {
    var formData = new FormData();
    formData.append('userId', userId);
    var xhr = new XMLHttpRequest();
    var response;
    xhr.onreadystatechange = function() {
        if (xhr.readyState == 4 && xhr.status == 200) {
            response = JSON.parse(xhr.responseText);
            if (response.Status == "Success") {
                document.getElementById('subscribe').innerHTML = "<p>У вас в информаторах! <span class=\"glyphicon glyphicon-ok\"></span></p>";
            } else {
                document.getElementById('subscribe').innerHTML = "<p>Добавить в информаторы " + "<button class=\"btn btn-default\" onclick=\"subscribeOnUser('" + userId.toString() + "')\">" +
                    "<span class=\"glyphicon glyphicon-plus\"></span></button></p>";
            }
        };
    }

    xhr.open('POST', '/Subscription/CheckUsersSubscription');
    xhr.send(formData); //отправка данных
}



function subscribeOnUser(userId) {
    var formData = new FormData();
    formData.append('userId', userId);
    var xhr = new XMLHttpRequest();
    var response;
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            response = JSON.parse(xhr.responseText);
            if (response.Status == "Success") {
                document.getElementById('subscribe').innerHTML = "<p>У вас в информаторах! <span class=\"glyphicon glyphicon-ok\"></span></p>";
            } else {
                document.getElementById('subscribe').innerHTML = "<p>Попробуйте еще раз, пожалуйста... <span class=\"glyphicon glyphicon-ok\"></span></p>";

            }
        };
    }

    xhr.open('POST', '/Subscription/SubscribeOnUser');
    xhr.send(formData); //отправка данных
}