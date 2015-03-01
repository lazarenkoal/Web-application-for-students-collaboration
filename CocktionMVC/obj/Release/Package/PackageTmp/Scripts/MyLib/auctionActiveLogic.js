//Скрипт, добавляющий ставку к аукциону.
function addBid(auctionId) {
    var name1, filename1;
    document.getElementById('uploader').onsubmit = function () {
        var formData = new FormData();

        //добавляю файл
        var fileInput = document.getElementById('fileInput');
        filename1 = fileInput.files[0].name;
        formData.append(fileInput.files[0].name, fileInput.files[0]);

        //добавляю поле с именем
        var bidName = document.getElementById('bidName').value;
        name1 = bidName;
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
        xhr.open('POST', '/BidAuctionCreator/UploadFile');
        xhr.send(formData); //отправка данных
        //если все хорошо
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                //получение статуса с сервера при отправке
                document.getElementById('updater').outerText = 'успешно добавлено';
                $("#addExtraBid").show();
                //добавление нодика ко всем остальным клиентам
                chat.server.addNodesToClients(name1, filename1, auctionId, xhr.responseText);
            };

        }
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
                var response = JSON.parse(xhr.responseText);
                var parentId = response.FirstBidId;
                var childId = response.ThisBidId;

                chat.server.addNodesToClients(name1, filename1, auctionId, xhr.responseText);
                chat.server.addExtraNodeToClients(name1, filename1, auctionId, parentId, childId);
            };

        }
}
function showExtraBidAdder() {
    $("#extraBidContainer").show();
}
