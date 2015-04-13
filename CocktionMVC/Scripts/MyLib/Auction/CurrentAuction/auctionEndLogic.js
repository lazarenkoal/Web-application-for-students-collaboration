/*Печатает на экране пользователя результаты аукциона,
в зависимости от типа пользователя.
*/
function printAuctionResults(obj) {
    if (obj.Type == 'Owner') {
        $('#auctionInfoContainer').empty();
        $('#auctionInfoContainer').append(obj.Message);
        $("#showAuctionInfoBtn").click();
    }//end of if
    else if (obj.Type == 'Winner') {
        $('#auctionInfoContainer').empty();
        $('#auctionInfoContainer').append(obj.Message);
        $("#showAuctionInfoBtn").click();
    }//end of if
    else if (obj.Type == 'Info') {
        $('#timer').append(obj.Message);
    }
    else if (obj.Type == 'Looser') {
        $('#timer').append(obj.Message);
    }
    else if (obj.Type == 'Owner_undfnd') {
        $('#timer').append(obj.Message);
    }
}

/*Запрашивает с сервера результаты аукциона
*/
function getAuctionResults() {
    var winnerNameRequest = new XMLHttpRequest();
    var idSender = new FormData();
    idSender.append('auctionId', auctionId);
    winnerNameRequest.open('POST', '/AuctionRealTime/SendAuctionResults');
    winnerNameRequest.send(idSender);
    winnerNameRequest.onreadystatechange = function () {
        if (winnerNameRequest.readyState == 4 && winnerNameRequest.status == 200) {
            var info = JSON.parse(winnerNameRequest.responseText);
            $('#timer').empty();
            printAuctionResults(info);
        };
    };
    return false;
};

/*Запускается, когда таймер заканчивает работу*/
function sayThatFinished() {
    chat.server.finishAuction(auctionId);
};

