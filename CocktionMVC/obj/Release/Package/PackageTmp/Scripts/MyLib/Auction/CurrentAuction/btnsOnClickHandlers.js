//TODO Засунуть функцию такую в другое место...
//при нажатии на кнопку выбрать лидера
function chooseLider() {
    var data = new FormData();
    data.append('auctionId', auctionId);
    data.append('productId', nodeId);
    winnerId = nodeId;
    var request = new XMLHttpRequest();
    request.open('POST', '/AuctionRealTime/AddLider');
    request.send(data);
    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200) {
            var resp = JSON.parse(request.responseText);
            if (resp.Status == 'True') {
                $("#chooseLeaderInfo").empty();
                $("#chooseLeaderInfo").append("Лидер успешно выбран:)");
                isChoosed = true;
            } else {
                alert("Извини, мы облажались... Попробуй еще раз...")
                alert(resp.Status);
            }
        }
    };
    return false;
};

//Функция для завершения аукциона
function finishAuction() {
    if (isChoosed == true) {
        sayThatFinished();
    } else {
        alert("Необходимо выбрать победителя, прежде чем завершать аукцион:)");
    }
};

//посылает ставку яичную
function makeEggBet() {
    if (checkIfCanSendEggs()) {
        $("#toteProgress").show();
        $("#eggInfo").empty();
        $("#eggsAmountTextbox").hide();
        $("#sendEggsBtn").hide();
        var formData = new FormData();
        var eggsAmnt = $('#eggsAmountTextbox').val();
        formData.append('eggsAmount', eggsAmnt);
        formData.append('auctionId', auctionId);
        formData.append('productId', nodeId);
        var req = new XMLHttpRequest();
        req.onreadystatechange = function() {
            if (req.readyState == 4 && req.status == 200) {
                var info = JSON.parse(req.responseText);
                sayResultOfAddingToteBet(info.Status, info.UsersAmountOfEggs);
                $("#eggInfo").show();
                $("#eggsAmountTextbox").val("");
                $("#toteProgress").hide();
                $("#eggsAmountTextbox").show();
                $("#sendEggsBtn").show();
            };
        };

        req.upload.onprogress = function(e) {
            $('#toteBar').css('width', (e.loaded / e.total) * 100 + '%');
        }

        req.open('POST', '/AuctionRealTime/AddToteRate');
        req.send(formData);
    }
}

function eggsTextBoxOnChange() {
    if ($("#eggsAmountTextbox").val().length != 0) {
        $("#eggsAmountTextbox").css('border-color', 'rgb(204, 204, 204)');
    }
}

function checkIfCanSendEggs() {
    if ($("#eggsAmountTextbox").val().length == 0) {
        $("#eggsAmountTextbox").css('border-color', 'red');
        return false;
    } 
    return true;
}

function sayResultOfAddingToteBet(status, eggs) {
    if (status == true) {
        $("#eggInfo").append("<p>Ставка поставлена, осталось яиц: " + eggs + "</p>");
    } else {
        $("#eggInfo").append("<p>К сожалению, мне не хватило этого количества яиц для такой ставки (" + eggs + ")</p>");
    }
}

//Показывает контейнер с товарами на аукционе
function showProducts(btn, isOwner) {
    if (isOwner == 'True') {
        btn.focus();
        $("#productsContainer").show();

        $("#toteBetsContainer").hide();
        $("#auctionInfoContainer").hide();

    } else {
        btn.focus();
        $("#productsContainer").show();
        $("#toteBetsContainer").hide();
        $("#auctionInfoContainer").hide();
    }
}

//Показывает информацию о ставках на аукционе
function showToteBets(btn) {
    btn.focus();
    $("#toteBetsContainer").show();
    $("#productsContainer").hide();
    $("#auctionInfoContainer").hide();
}

//Показывает окошко с информацией об аукционе
function showAuctionInfo(btn) {
    btn.focus();
    $("#auctionInfoContainer").show();
    $("#productsContainer").hide();
    $("#toteBetsContainer").hide();
}

//Показывает контейнер для добавления своего товара на
//аукцион
function showProductBids(btn, userStatus, userName) {
    btn.focus();
    if (userStatus == 'True') {
        if (userName == ownerName) {
            $('#productBidErrorInfo').empty();
            $('#productBidErrorInfo').show();
            $('#productBidErrorInfo').append('<p>Вы же владелец;) Вы уже добавили)</p>');
            $("#productBidContainer").hide();
            $("#chatContainer").hide();
            $('#stopAuction').show();
        } else {
            $("#productBidContainer").show();
            $("#chatContainer").hide();
        }
    } else {
        $('#productBidErrorInfo').show();
        $('#productBidErrorInfo').empty();
        $("#productBidContainer").hide();
        $("#chatContainer").hide();
        $('#productBidErrorInfo').append('<p>Для того, чтобы принять участие в аукционе, пожалуйста, авторизуйтесь или зарегистрируйтесь:) </p>');
    }
}

//Показывает контейнер с чатом
function showChat(btn, userStatus) {
    btn.focus();
    if (userStatus == 'True') {
        $("#chatContainer").show();
        $("#productBidContainer").hide();
        $('#productBidErrorInfo').hide();
    } else {
        $('#productBidErrorInfo').empty();
        $('#productBidErrorInfo').show();
        $("#chatContainer").show();
        $("#productBidContainer").hide();
        $("#chatControlPanel").hide();
        $('#productBidErrorInfo').append('<p>Чтобы отправлять сообщения, нужно быть авторизованным!</p>')
    }
}




