//Скрипт, который плавненько листает страничку туды-сюды

//скроллит при нажатии на кнопочку к дереву
function scrollToTree() {
    $('html, body').animate({
        scrollTop: $("#globalContainer").offset().top
    }, 1000);
};

//число после оффсета - время в милисекундах
//скроллит при нажатии на кнопочку к дереву
function scrollToChat () {
    $('html, body').animate({
        scrollTop: $("#chatContainer").offset().top
    }, 1000);
};