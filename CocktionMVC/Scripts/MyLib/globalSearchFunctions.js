function sendSearchString() {
    $('#progressBarContainer').show();
        if (canSendSearchString()) {
        $("#auctionsContainer").empty();
        auctionRowCounter = 0;
        auctionColCounter = 0;
        var formData = new FormData();
        formData.append('searchString', $("#searchField").val());

        var xhr = new XMLHttpRequest();
        xhr.upload.onprogress = function (e) {
            $('#bar').css('width', (e.loaded / e.total) * 100 + '%');
        }

        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var respond = JSON.parse(xhr.responseText);
                if (!respond.IsEmpty) {
                    //если в коллекции что-то есть - выводим все пользователям на страничку
                    for (var i = 0; i < respond.Auctions.length; i++) {
                        addCellToTheGrid('auctionsContainer', respond.Auctions[i].Name,
                            respond.Auctions[i].Description, 'через 3 дня', 'http://cocktion.com/Images/Thumbnails/' +
                            respond.Auctions[i].Photo, 'http://cocktion.com/Auction/CurrentAuction/' +
                            respond.Auctions[i].Id);
                    }
                } else {
                    //если в коллекции ничего нет - выводим сообщенеи
                    $("#auctionsContainer").append("<p>Такого еще никто не продавал...</p>");
                }
                $('#bar').css('width', 0 + '%');
                $('#progressBarContainer').hide();
            };
        }

        xhr.open('POST', '/Search/SearchEverywhere');
        xhr.send(formData); //отправка данных
    }
}




function canSendSearchString() {
    var searchString = $("#searchField").val();
    if (searchString.length == 0) {
        $("#searchField").css('border-color', 'red');
        return false;
    } else {
        $("#searchField").css('border-color', 'rgb(204, 204, 204)');
        return true;
    }
}