//Скрипт, добавляющий ставку к аукциону.
function addBid(auctionId) {
        var formData = new FormData();
        $("#progressBar").show();

        //добавляю файл
        var fileInput = document.getElementById('fileInput');
        formData.append(fileInput.files[0].name, fileInput.files[0]);
        //добавляю поле с именем
        var bidName = document.getElementById('bidName').value;
        formData.append("name", bidName);

        //добавляю поле с опмсание
        var bidDescription = document.getElementById('bidDescription').value;
        formData.append("description", bidDescription);

        //добавляю поле с категорией
        var bidCategory = document.getElementById('bidCategory').value;
        formData.append('category', bidCategory);

        //adding a field with auctionId
        formData.append('auctionId', auctionId);

        //создаю запрос
        var xhr = new XMLHttpRequest();
        
    //если все хорошо
        xhr.upload.onprogress = function (e) {
            $('#bar').css('width', (e.loaded / e.total) * 100 + '%');
        }
        
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                //получение статуса с сервера при отправке
                // document.getElementById('updater').outerText = 'успешно добавлено';
                //TODO добавить код взаимодействия с пользователем
                
                //Зачистка полей
                $('#bidName').val("");
                $('#bidDescription').val("");
                $('#bidCategory').val("");
                $("#progressBar").hide();
                $("#fileInput").val("");
                // $("#addExtraBid").show();
                //добавление нодика ко всем остальным клиентам
                // chat.server.addNodesToClients(name1, filename1, auctionId, xhr.responseText);
            };
        }

        xhr.open('POST', '/BidAuctionCreator/AddProductBet');
        xhr.send(formData); //отправка данных
        
        return false;
    }

function addExtraBid(auctionId) {
    var formData = new FormData();

    //добавляю файл
    var fileInput = document.getElementById('extraFileInput');
    filename1 = fileInput.files[0].name;
    formData.append(fileInput.files[0].name, fileInput.files[0]);

    //добавляю поле с именем
    var bidName = document.getElementById('extraBidName').value;
    name1 = bidName;
    formData.append("name", bidName);

    //добавляю поле с опмсание
    var bidDescription = document.getElementById('extraBidDescription').value;
    formData.append("description", bidDescription);

    //добавляю поле с категорией
    var bidCategory = document.getElementById('extraBidCategory').value;
    formData.append('category', bidCategory);

    //adding a field with auctionId
    formData.append('auctionId', auctionId);

    //создаю запрос
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/BidAuctionCreator/AddExtraBid');
    xhr.send(formData); //отправка данных
    //если все хорошо
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            //получение статуса с сервера при отправке
            document.getElementById('extraBidContainer').outerText = 'успешно добавлен довесок';
            //добавление нодика ко всем остальным клиентам
            //var response = JSON.parse(xhr.responseText);
            //var parentId = response.FirstBidId;
            //var childId = response.ThisBidId;
            //chat.server.addNodesToClients(name1, filename1, auctionId, xhr.responseText);
            //chat.server.addExtraNodeToClients(name1, filename1, auctionId, parentId, childId);
        };

    }
}
function showExtraBidAdder() {
    $("#extraBidContainer").show();
}

//Функция проверяет на то, что чувак сделал ставочку
function check(modelId) {
    var xhr = new XMLHttpRequest();
    var data = new FormData();
    data.append("auctionId", modelId);
    xhr.open("POST", "/AuctionRealTime/CheckIfUserBidded");
    xhr.send(data);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var info = JSON.parse(xhr.responseText);
            $("#nodeInfo").append(info.HaveBid);
        };
    };
};