var checkboxes = document.getElementsByName("houseCheckbox"); //получение чекбоксиковё

//Функция кодирует айдишники домов в формате
//!1!2!34!43!38! в зависимости от кликнутого чекбокса
function generateStringWithHouses() {
    var houses = "!";
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) { //проверяем, что чекбокс кликнут
            houses += checkboxes[i].id + "!";
        }
    }
    return houses;
}

//обработчик клика на кнопку creator
//шлет информацию о новом аукциончике на сервачок
function creatorOnClick() {
    if ((verifyTime() & verifyFormData())) {
        if (validateFile()) {

            //зашиваем домишки
            var houses = generateStringWithHouses().toString(); 
            if (houses == "!") {
                //в этом случае человек вообще не отмечал никаких домов
                $("#housesErrorField").empty();
                $("#housesErrorField").append("<p>" + "Вы не отметили вообще никаких домов!" + "</p>");
                return false;
            }
            else {
                //готовим данные к отправке
                var formData = new FormData();
                var req = new XMLHttpRequest;
                
                //запихиваем текстовые данные об аукционе
                formData.append("name", $('#Name').val());
                formData.append("description", $('#Description').val());
                formData.append("category", $('#info').text());
                formData.append("hours", $('#Hours').val());
                formData.append("minutes", $('#Minutes').val());
                formData.append("housesId", houses);

                //запихиваем файлик
                var fileInput = document.getElementById('file');
                formData.append(fileInput.files[0].name, fileInput.files[0]);

                //очищаем старые контейнеры
                $('#formContainer').empty();
                $('#formContainer').append('<p>загрузка в процессе</p>');

                //отправляем
                req.open("POST", "/BidAuctionCreator/CreateAuction");
                req.send(formData);
                req.onreadystatechange = function () {
                    //по готовности пишем статус
                    if (req.readyState == 4 && req.status == 200) {
                        $('#formContainer').empty();
                        $('#formContainer').append('<p>' + req.responseText + '</p>');
                    }
                }
                return true;
            }
        }
        return false;
    }
    else
        return false;
}