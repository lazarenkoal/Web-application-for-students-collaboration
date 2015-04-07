//функция будет посылать в таймер информацию о
//результатах аукциона
function printAuctionResults(obj) {
    if (obj.Type == 'Owner') {
        //необходимо связаться с продавцом
        //печатаем в таймер сообщение
        $('#auctionInfoContainer').empty();
        $('#auctionInfoContainer').append(obj.Message);
        //открываем окно для чата
        //вызываем метод, кот будет рассылать
        //сообщения
    }//end of if
    else if (obj.Type == 'Winner') {
        //необходимо связаться с победителем

        //Печатаем в таймер сообщение, затем
        $('#auctionInfoContainer').empty();
        $('#auctionInfoContainer').append(obj.Message);

        //открываем окно для чата
        //вызываем метод, который будет рассылать
        //собшения
    }//end of if
    else if (obj.Type == 'Info') {
        //вставляем в таймер информацию
        $('#timer').append(obj.Message);
    }
    else if (obj.Type == 'Looser') {
        //пока что только выводим информацию
        //из сообщения в таймер
        $('#timer').append(obj.Message);
    }
    else if (obj.Type == 'Owner_undfnd') {
        //случай, в котором хозяин не выбрал лидера
        //пока что только выводим сообщения в таймер.
        $('#timer').append(obj.Message);
    }
}

//функция вовзвращает название победившего продукта
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

//функция, которая включается, когда таймер заканчивает
//работу
function sayThatFinished() {
    chat.server.finishAuction(auctionId);
};

