//Проверяет значение времени 
//Т.е. часов и минут
function verifyTime() {
    var hours = document.getElementById('Hours').value;
    var minutes = document.getElementById('Minutes').value;
    if (hours.length == 0) {
        $('#hoursErrorField').empty();
        $("#infoErrorField").empty();
        $("#hoursErrorField").append('Неверное значение часов');
        return false;
    }
    else if (minutes.length == 0) {
        $("#minutesErrorField").append('Неверное значение минут');
        return false;
    }
    else
        return true;
}

/*Функция, которая проверяет все значения
    в формах. Нужно последовательно удалять ненужные
    сообщения по мере того, как пользователь что-то туда зафигачивает
    нужно сделать по-нормально, т.е. зафигачить тут поверку по событиями
    настроить события на ищменениях на то, чтобы они удалялили ненужные поля*/
function verifyFormData() {
    var name = document.getElementById('Name').value;
    var description = document.getElementById("Description").value;
    var category = document.getElementById("info").innerText;
    if (name.length == 0) {
        $("#nameErrorField").append('Нужно ввести название товара:)');
        return false;
    }
    else if (description.length == 0) {
        $('#nameErrorField').empty();
        $("#descriptionErrorField").append("Нужно ввести описание товара");
        return false;
    }
    else if (category.length == 0) {
        $('#descriptionErrorField').empty();
        $("#infoErrorField").append("Нужно выбрать категорию товара!!!");
        return false
    }
    else
        return true;
}

//Валидация файла)
var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png"];
function validateFile() {
    var inputFile = document.getElementById("file");
    var sFileName = inputFile.value;
    var blnValid = false;
    if (sFileName.length > 0) {
        for (var j = 0; j < _validFileExtensions.length; j++) {
            var sCurExtension = _validFileExtensions[j];
            if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                blnValid = true;
                break;
            }
            else {
                $("#fileErrorField").append("<p>" + 'Это либо не файл, либо не картинка!' + "</p>");
            }
        }
    }
    return blnValid;
}