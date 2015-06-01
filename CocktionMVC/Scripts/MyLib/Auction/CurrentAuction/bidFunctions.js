function checkBidForm() {
    if ($("#bidName").val().length == 0) {
        $("#bidName").css('border-color', 'red');
        return false;
    } else {
        $("#bidName").css('border-color', 'rgb(204, 204, 204)');
        return true;
    }
}

function checkCategory() {
    if (category.length == 0) {
        $("#typeOfAuctionContainer").css('border', 'solid 1px red');
        return false;
    } else {
        $("#typeOfAuctionContainer").css('border', 'solid 1px rgb(204, 204, 204)');
        return true;
    }
}

var category = "";

function radioProcessor(btn) {
    switch (btn.id) {
    case 'bookRadio':
        $('#productRadio').prop('checked', false);
        $('#serviceRadio').prop('checked', false);
        category = 'Книга';
        break;
    case 'productRadio':
        $('#bookRadio').prop('checked', false);
        $('#serviceRadio').prop('checked', false);
        category = 'Вещь';
        break;
    case 'serviceRadio':
        $("#bookRadio").prop('checked', false);
        $("#productRadio").prop('checked', false);
        category = 'Услуга';
        break;
    }
}
var fileInput;
function checkPhoto() {
    fileInput = document.getElementById('fileInput');
    if (fileInput.files.length > 0)
    {
        var ext = (/[.]/.exec(fileInput.files[0].name)) ? /[^.]+$/.exec(fileInput.files[0].name) : undefined;
        if ((ext == "jpeg") || (ext == "jpg") || (ext == "png")) {
            return true;
        }
        else {
            alert("Формат является недопустимым при загрузке фотографий. Пожалуйста, попробуйте все-таки с фотографией");
            return false;
        }
    }
    else
    {
        return true;
    }
}
//Скрипт, добавляющий ставку к аукциону.
function addBid(auctionId) {
    if ((checkBidForm() & checkCategory()) & checkPhoto()) {
        var formData = new FormData();
        $("#progressBar").show();

        //добавляю файл
        //добавляю файл
        var fileInput = document.getElementById('fileInput');
        if (fileInput.files.length > 0) {
            formData.append(fileInput.files[0].name, fileInput.files[0]);
        }

        //добавляю поле с именем
        var bidName = document.getElementById('bidName').value;
        formData.append("name", bidName);

        //добавляю поле с опмсание
        var bidDescription = document.getElementById('bidDescription').value;
        formData.append("description", bidDescription);

        formData.append('category', category);

        //adding a field with auctionId
        formData.append('auctionId', auctionId);

        //создаю запрос
        var xhr = new XMLHttpRequest();

        //если все хорошо
        xhr.upload.onprogress = function(e) {
            $('#bar').css('width', (e.loaded / e.total) * 100 + '%');
        }

        xhr.onreadystatechange = function() {
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