function initialiseHub(modelId, isUseAuth, userIdentityName) {
    chat = $.connection.auctionHub;
    //Функция для добавления сообщения на страничку
    chat.client.addNewMessageToPage = function (name, message) {
        var textToAppend = '<p><b>' + name + ': </b>' + message + '</p>';
        $("#chatMessagesContainer").append(textToAppend);
    };

    //Функция для отображения информации о тотализаторе
    chat.client.updateToteBoard = function(data)
    {
        $('#coefficientsTable').empty();
        $('#coefficientsTable').append("<p>"+data + "</p>");
    };

    chat.client.addExtraNodesToPages = function (fileName, name, parentProductId, childProductId) {
        nodes.add([{ id: childProductId, label: name.trim(), shape: 'circularImage', image: 'http://cocktion.com/Images/Thumbnails/' + fileName }]);
        edges.add([{ from: parentProductId, to: childProductId }]);
    }

    //Функция для добавления товара на все страницы аукциона
    chat.client.addNodesToPages = function (fileName, name, productId){
        nodes.add([{ id: productId, label: name.trim(), shape: 'circularImage', image: 'http://cocktion.com/Images/Thumbnails/' + fileName }]);
        edges.add([{ from: i, to: productId }]);

    };

    //функция для объявления лидера на всех страничках
    //ВО ВРЕМЯ АУКЦИОНА!!!!!!!
    chat.client.showLeaderOnPage = function (leaderId, liderName){
        $('#leaderHolder').empty();
        $('#leaderHolder').append('<p><b>Лидер аукциона: </b>' + liderName+'</p>');
        isChosen = true;
    };

    chat.client.finishAuction = function()
    {
        $n.countdown('stop');
        getAuctionResults();
    };

    $.connection.hub.start().done(function () {
        chat.server.addNewRoom(modelId);
        chat.server.getTote(modelId);
        $('#sendMessageBtn').click(function() {
            if (isUseAuth == 'True') {
                chat.server.send(userIdentityName, $("#enterMessageTextBox").val(), modelId);
                $("#enterMessageTextBox").val("").focus();
            } else {
                alert("Я же предупредил, что вам нужно авторизоваться;)");
            }
        });
    });
}