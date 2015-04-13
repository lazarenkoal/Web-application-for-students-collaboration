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
                '<img class=\"img-circle\" src=\"' + 'http://cocktion.com/Images/Thumbnails/' + info.FileName + "\">"
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
                    $('#chooseLeaderForm').show();
                    $('#chooseLeaderInfo').empty();
                    $('#chooseLeaderInfo').append('<p>Выбрать этот товар лидером?</p>');
                } else {
                    $('#chooseLeaderForm').show();
                    $('#chooseLeaderInfo').empty();
                    $('#chooseLeaderInfo').append('<p>Cменить выбор на этот?</p>');
                }
            }
        }
        if (nodeId == i) {
            $("#addToteBetContainer").hide();
        } else {
            $("#addToteBetContainer").show();
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


var edges = new vis.DataSet();
var nodes = new vis.DataSet();
//если выбран победитель аукциона
var nodeId;

function initialiseNetwork(sellProductName, sellProductPhotoName) {
    // create an array with nodes
   // var nodes = new vis.DataSet();
    nodes.add([
        { id: i, label: sellProductName.trim(), shape: 'circularImage', image: 'http://cocktion.com/Images/Thumbnails/' + sellProductPhotoName}
    ]);
    // create an array with edges
    //var edges = new vis.DataSet();
    // create a network
    var container = document.getElementById('mynetwork');
    var data = {
        nodes: nodes,
        edges: edges,
    };
    var options = {
        width: '100%',
        height: '600px'
    }
    var network = new vis.Network(container, data, options);
    network.on('select', nodeSelected);
}