//Функция для получения информации о нодике
function getNodeInfo(nodeId) {
    var req = new XMLHttpRequest();
    var data = new FormData();
    data.append("Id", nodeId);
    req.open('POST', '/AuctionRealTime/SendInfoAboutProduct');
    req.send(data);
    req.onreadystatechange = function () {
        if (req.readyState == 4 && req.status == 200) {
            var info = JSON.parse(req.responseText);
            $('#productPhotoContainer').empty();
            $('#productPhotoContainer').append(
                '<img class=\"img-thumbnail\" src=\"' + 'http://cocktion.com/Images/Photos/' + info.FileName + "\">"
            );

            $("#productInfoContainer").empty();
            $("#productInfoContainer").append('<p><b>Что: </b>' + info.Name + '</p>');
            $("#productInfoContainer").append('<p><b>Конкретно: </b>' + info.Description + '</p>');
            $("#productInfoContainer").append('<p><b>Категория: </b>' + info.Category + '</p>');

            $("#showNodeInfoBtn").click();
        };
    }
    req.onprogress = function () {
        $('#nodeInfo').append('<p>Идет получение информации </p>');
    }
};

//если выбран какой-то из товаров
function nodeSelected(properties) {
    nodeId = properties.nodes[0];
    //проверка того, зашел зарегистрированный пользователь
    //или нет
    if (nodeId != null) {
        getNodeInfo(nodeId);
        if (userStatus == 'True') { //если зарегистрирован
            if (userName == ownerName) {
                //проверяем выбрал ли или нет
                if (isChoosed == false) {
                    $('#chooseLiderForm').show();
                    $('#chooseLiderInfo').empty();
                    $('#chooseLiderInfo').append('<p>Выбрать этот товар лидером?</p>');
                } else {
                    $('#chooseLiderForm').show();
                    $('#chooseLiderInfo').empty();
                    $('#chooseLiderInfo').append('<p>Cменить выбор на этот?</p>');
                }
            }
        }
    }
}

//Функция добавляет кружочек на "карту" аукциона.
function addNode(fileName, name, productId) {
    nodes.add([{ id: productId, label: name.trim(), shape: 'circularImage', image: 'http://cocktion.com/Images/Thumbnails/' + fileName }]);
    edges.add([{ from: i, to: productId }]);
};

/*Функция добавляет дополнительную связь между довеском
        и тем товаром, к которому он приклеен
        fileName - название файла на сервер
        name - имя товара, которое надо отобразить
        parentId - айдишник товара, к которому надо приклеить
        childId - айдишник товара, который надо приклеить к 
        parent'у
        */
function addExtraNode(fileName, name, parentId, childId) {
    nodes.add([{ id: childId, label: name.trim(), shape: 'circularImage', image: 'http://cocktion.com/Images/Thumbnails/' + fileName }]);
    edges.add([{ from: parentId, to: childId }]);
};