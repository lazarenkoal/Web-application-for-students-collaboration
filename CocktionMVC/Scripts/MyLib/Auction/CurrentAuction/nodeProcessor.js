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
            $('#nodeInfo').empty();
            $('#nodeInfo').append('<p>' + info.Name + '</p>');
            $('#nodeInfo').append('<p>' + info.Description + '</p>');
            $('#nodeInfo').append('<img src = " ' + 'http://cocktion.com/Images/Thumbnails/' + info.FileName + '"/>');
        };
    }
    req.onprogress = function () {
        $('#nodeInfo').append('<p>Идет получение информации </p>');
    }
};

//если выбран какой-то из товаров
function nodeSelected(properties) {
    nodeId = properties.nodes[0];
    $('#nodeInfo').empty();
    //проверка того, зашел зарегистрированный пользователь
    //или нет
    if (nodeId != null) {
        getNodeInfo(nodeId);
        if (userStatus == 'True') {//если зарегистрирован
            $('#toteBoardContainer').show();
            if (userName == ownerName) {
                //проверяем выбрал ли или нет
                if (isChoosed == false) {
                    $('#chooseLeader').show();
                    $('#nodeInfo').append('<p>Вы хотите выбрать лидером элемент с айди ' + nodeId + '</p>');
                }
                else {
                    $('#chooseLeader').show();
                    $('#nodeInfo').append('<p>Можно сменить выбор!!! на ' + nodeId + '</p>');
                }
            }
            else {
                $('#nodeInfo').append('<p> информация о нодике с айди ' + nodeId + '</p>');
            }
        }
        else {
            $('#nodeInfo').append('<p> информация о нодике с айди яуй ' + nodeId + '</p>');
        }
    }
    else {
        $('#nodeInfo').append('<p> Если вы хотите посмотреть информацию о товаре - кликнете на него;) </p>');
        $('#toteBoardContainer').hide();
        $('#chooseLeader').hide();
    }

};

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
}