//Коннектится к хабу и добавляет свеженькие аукциончики в список
$(function () {
    var adder = $.connection.auctionListHub;
    adder.client.addAuctionToTheList = function (name, description, category, time, photoPath, id) {
        addCellToTheGrid('auctionPanel', name, description,
                   time, 'http://cocktion.com/Images/Thumbnails/' + photoPath,
                   'http://cocktion.com/Auction/CurrentAuction/' + id);
    }
    $.connection.hub.start().done(function () {
    });
});

