//Коннектится к хабу и добавляет свеженькие аукциончики в список
$(function () {
    var adder = $.connection.auctionListHub;
    adder.client.addAuctionToTheList = function (name, description, category, photoPath) {
        $('.AuctionContainer').prepend('<div>' + '<p>' + name + '</p>' + '<p>' + description + '</p>' + '<p>' + category + '</p>' +
            '<img src="' + 'http://cocktion.com/Images/Photos/' + photoPath + '" width="150" height="150" />' + '</div>');
    }
    $.connection.hub.start().done(function () {
    });
});