//TODO Засунуть функцию такую в другое место...
//при нажатии на кнопку выбрать лидера
document.getElementById('chooseLeader').onclick = function () {
    var data = new FormData();
    data.append('AuctionId', auctionId);
    data.append('ProductId', nodeId);
    winnerId = nodeId;
    var request = new XMLHttpRequest();
    request.open('POST', '/AuctionRealTime/AddLider');
    request.send(data);
    request.onreadystatechange = function () {
        $('#updater').empty();
        $('#updater').append('<p>' + request.responseText.substring(1, request.responseText.length - 1) + '</p>');
        chat.server.setLider(nodeId, auctionId);
        isChoosed = true;
    };
    return false;
};

//кнопка для заканчивания аукциона.
$('#stopAuction').click(function () {
    sayThatFinished();
});

$('#addEggsBtn').click(function () {
        var formData = new FormData();
        var eggsAmnt = document.getElementById('eggsAmount').value;
        formData.append('eggsAmount', eggsAmnt);
        formData.append('auctionId', auctionId);
        formData.append('productId', nodeId);
        var req = new XMLHttpRequest();
        req.open('POST', '/AuctionRealTime/AddRate');
        req.send(formData);
        req.onreadystatechange = function () {
            if (req.readyState == 4 && req.status == 200) {
                var info = JSON.parse(req.responseText);
                $('#eggInfo').append('<p>' + info.Status + '</p>');
                $('#eggInfo').append('<p>' + info.UsersAmountOfEggs + '</p>');
            };
        };
        return false;

    
});