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
        xhr.open('POST', '/FileSaver/UploadFile');
        xhr.send(formData); //отправка данных
        //если все хорошо
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                //получение статуса с сервера при отправке
                document.getElementById('updater').outerText = 'успешно добавлено';
                //добавление нодика ко всем остальным клиентам
                chat.server.addNodesToClients(name1, filename1, auctionId, xhr.responseText);
            };

        }
        return false;
    }
}

