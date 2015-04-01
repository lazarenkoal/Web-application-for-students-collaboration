//число после оффсета - время в милисекундах
//скроллит при нажатии на кнопочку к верхнему элементу
function scrollToTheTop() {
    $('html, body').animate({
        scrollTop: $("#top").offset().top
    }, 1000);
};

function scrollToTheForum() {
    $('html, body').animate({
        scrollTop: $("#messageEnter").offset().top
    }, 1000);
}