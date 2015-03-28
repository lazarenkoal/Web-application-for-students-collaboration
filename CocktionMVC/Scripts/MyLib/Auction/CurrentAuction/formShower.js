function showUploaderOrStopper(userStatus, userName) {
    //если пользователь авторищован
    if (userStatus == 'True') {
        //если это создатель, показываем ему кнопку
        if (userName == ownerName) {
            $('#stopAuction').show();

        }
            //если это не создатель, то показываем форму добавления ставки
        else {
            $('#uploader').show();
        }
    }
    else {
        $('#updater').append('<p>Для того, чтобы принять участие в аукционе, пожалуйста авторизуйтесь или зарегистрируйтесь:) </p>');
    }
}
