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
            $('#nodeInfo').append('<img src = " ' + 'http://cocktion.azurewebsites.net/Images/Thumbnails/' + info.FileName + ' "/>');
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
        $('#chooseLeader').hide();
    }
};

function showUploaderOrStopper(userStatus, userName) {
    //если пользователь авторищован
    if (userStatus == 'True') {
        //если это создатель, показываем ему кнопку
        if (userName == ownerName) {
            $('#stopAuction').show();

        }
            //если это не создатель, то показываем форму добавления ставки
        else {
            $('#uploader').show();
        }
    }
    else {
        $('#updater').append('<p>Для того, чтобы принять участие в аукционе, пожалуйста авторизуйтесь или зарегистрируйтесь:) </p>');
    }
}

function printLider(nodeId)
{
    if (nodeId != null) {
        $('#leaderInfo').empty();
        $('#leaderInfo').append('В данный момент id лидера = ' + nodeId);
    }
}