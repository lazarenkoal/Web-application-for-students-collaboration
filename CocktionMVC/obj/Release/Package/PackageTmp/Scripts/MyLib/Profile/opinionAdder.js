//Отправляет мнение пользователя на сервер
function addOpinion(userId) {
    var post = $('#writeFeedbackTextarea').val();//получаем текст в textarea

    //TODO здесь сделать код, который будет отправлять на сервак всю необходимую информацию
    var xhr = new XMLHttpRequest();
    var formData = new FormData();
    formData.append('message', post);
    formData.append('userId', userId);
    xhr.open('POST', '/Profile/AddUsersFeedback');
    xhr.send(formData);
    xhr.onreadystatechange = function() {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var obj = JSON.parse(xhr.responseText);
            if (obj.Status == 'success') {
                var name = obj.Name;
                var surname = obj.Surname;
                //Обновляем формочку на клиенте
                $('#feedbackPosts').append(
                    "<div class=\"feedbackPost\">" + "<p class=\"postAuthor\"><b>" + name + ' ' + surname + "</b> сказал: </p>" +
                    "<p>" + post + "<p/>" + "</div>" + "</br>"
                );

                //очищаем поле для ввода текста
                $('#writeFeedbackTextarea').val('');
            } else {
                alert('К сожалению, произошла какая-то ошибка');
            }
        }
    }
}