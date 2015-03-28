//достает то, на что пользователь кликнул при выборе категорий
$(document).on('click', '.dropdown-menu li a', function () {
    $('#info').empty();
    $('#info').append($(this).text());
});