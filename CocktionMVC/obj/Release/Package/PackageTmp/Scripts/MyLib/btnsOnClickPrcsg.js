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

});