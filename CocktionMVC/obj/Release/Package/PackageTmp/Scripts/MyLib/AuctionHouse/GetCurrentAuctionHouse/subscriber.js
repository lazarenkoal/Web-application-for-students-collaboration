
function  checkSubscription(modelId) {
    var formData = new FormData();
    formData.append('modelId', modelId);
    var xhr = new XMLHttpRequest();
    var response;
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            response = JSON.parse(xhr.responseText);
            if (response.Status == "Success") {
                document.getElementById('subscribe').innerHTML = "<p>Вы подписаны!  <span class=\"glyphicon glyphicon-ok\"></span></p>" +
                    "<p>Удалить из подписки<button class=\"btn btn-default\" onclick=\"unsubscribeFromHouse()\">" +
                    "<span class=\"glyphicon glyphicon-remove\"></span></button></p>"
                
            } else {
                document.getElementById('subscribe').innerHTML = "<p>Добавить в подписки " + "<button class=\"btn btn-default\" onclick=\"subscribeOnHouse()\">" +
                "<span class=\"glyphicon glyphicon-plus\"></span></button></p>";
            }
        };
    }

    xhr.open('POST', '/Subscription/CheckHouseSubscription');
    xhr.send(formData); //отправка данных
}

function unsubscribeFromHouse() {
    var formData = new FormData();
    formData.append('houseId', houseId);
    
    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var response = JSON.parse(xhr.responseText);
            if (response.Status) {
                document.getElementById('subscribe').innerHTML = "<p>Удален из подписок! <span class=\"glyphicon glyphicon-ok\"></span></p>";
            } else {
                document.getElementById('subscribe').innerHTML = "<p>Попробуйте еще раз ;(</p>";
            }
        };
    }

    xhr.open('POST', '/Subscription/UsubscribeFromHouse');
    xhr.send(formData); //отправка данных
}


var houseId;
function subscribeOnHouse() {
    var formData = new FormData();
    formData.append('houseId', houseId);
    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var response = JSON.parse(xhr.responseText);
            if (response.Status) {
                document.getElementById('subscribe').innerHTML = "<p>В ваших подписках  <span class=\"glyphicon glyphicon-ok\"></span></p>";
            } else {
                document.getElementById('subscribe').innerHTML = "<p>Попробуйте еще раз ;(</p>";
            }
        };
    }

    xhr.open('POST', '/Subscription/SubscribeOnHouse');
    xhr.send(formData); //отправка данных
}