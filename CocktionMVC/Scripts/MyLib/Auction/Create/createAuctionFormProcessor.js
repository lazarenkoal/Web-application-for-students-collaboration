/**
 * Обрабатывает клики на радио батоны.
 * Функция нужна для того, чтобы снимать клики с других радио-батонов,
 * чтобы в своей области была только одна кликнута.
 *
 * Объединены по принципу:
 * ProductType: book, product, service;
 * TimeBound: 1day, 1dayTime, 1weekTime;
 *
 * @param btn кнопка, которая кликается
 * 11.04.2015
 */
function auctionRadioCheckHandler(btn) {
    switch (btn.id) {
        case 'bookRadio':
            productType = btn.id;
            $('#productRadio').prop('checked', false);
            $('#serviceRadio').prop('checked', false);
            break;
        case 'productRadio':
            productType = btn.id;
            $('#bookRadio').prop('checked', false);
            $('#serviceRadio').prop('checked', false);
            break;
        case 'serviceRadio':
            productType = btn.id;
            $("#bookRadio").prop('checked', false);
            $("#productRadio").prop('checked', false);
            break;
        case '4hoursTime':
            timeBound = btn.id;
            $('#1dayTime').prop('checked', false);
            $('#1weekTime').prop('checked', false);
            break;
        case '1dayTime':
            timeBound = btn.id;
            $("#4hoursTime").prop('checked', false);
            $("#1weekTime").prop('checked', false);
            break;
        case '1weekTime':
            timeBound = btn.id;
            $("#4hoursTime").prop('checked', false);
            $('#1dayTime').prop('checked', false);
            break;
    }
}
var timeBound = ''; //хранит данные о кликнутом чекбоксе о времени
var productType = ''; //хранит данные о типе продукта  чекнутого

/**
 * Проверяет, что указаны границы по времени и тип продаваемой вещи.
 * Выделяет красным цветом форму, в которую ничего не введено.
 * Возвращает старый цвет, если все введено корректно
 *
 * Возвращает true, если все правильно отмечено
 *            false, если нет.
 * @returns {boolean}
 */
function checkRadio() {
    var timeBoundIsEmpty;
    var productTypeIsEmpty;
    var canSend = true;
    //проверка timeBound
    if (timeBound == "") {
        timeBoundIsEmpty = true;
        $("#timeContainer").css('border-color', 'red');
        canSend = false;
    } else {
        timeBoundIsEmpty = false;
        $("#timeContainer").css('border-color', 'lightgrey');
    }

    //проверка productType
    if (productType == '') {
        productTypeIsEmpty = true;
        canSend = false;
        $("#typeOfAuctionContainer").css('border-color', 'red');
    } else {
        productTypeIsEmpty = false;
        $("#typeOfAuctionContainer").css('border-color', 'lightgrey');
    }

    return canSend;
}

/**
 * Обрабатывает событие onchange поля productName.
 * Если она не пустая -> устанавливает ей старый цвет.
 */
function changeInputColor() {
    if ($("#productName").val().length != 0) {
        $("#productName").css('border-color', 'rgb(204, 204, 204)');
    }
}

/**
 * Возвращает тип товара в зависимости от названия кнопки
 * @param productType
 * @returns {string}
 */
function getProductType(productType) {
    switch (productType) {
        case 'bookRadio':
            return 'Книга';
            break;
        case 'productRadio':
            return 'Вещь';
            break;
        case 'serviceRadio':
            return 'Услуга';
            break;
    }
}

/**
 * Проверяет поле для ввода названия продукта.
 * Если оно непустое - меняет его цвет на красный.
 * Возвращает true, если все введено корректно
 *            false, если нет.
 * @returns {boolean}
 */
function checkInput() {
    var productName = $("#productName").val();
    if (productName.length == 0) {
        $("#productName").css('border-color', 'red');
        return false;
    } else {
        return true;
    }
}

/**
 * Отправляет данные на севрер. Проверяет все поля на корректность.
 */
function sendData() {
    var canSend = checkRadio() & checkInput();
    if (canSend) {
        //TODO проверить дома, когда будут
        //отправить все на сервер
        $("#progressBar").show();
        $("#createAuctionBtn").hide();

        var formData = new FormData();

        //добавляю файл
        var fileInput = document.getElementById('fileInput');
        if (fileInput.files.length > 0) {
            formData.append(fileInput.files[0].name, fileInput.files[0]);
        }

        //добавляем временные границы
        formData.append("timeBound", timeBound);

        //добавляю поле с именем
        var name = $("#productName").val();
        formData.append("name", name);

        //добавляю поле с опмсание
        var description = $("#descriptionName").val();
        formData.append("description", description);

        //добавляю поле с категорией
        var typeOfProductToSend = getProductType(productType);
        formData.append('category', typeOfProductToSend);

        createStringOfHouses();
        formData.append('housesIds', ids);

        //создаю запрос
        var xhr = new XMLHttpRequest();
       
        //если все хорошо - 
        xhr.upload.onprogress = function(e) {
            $('#bar').css('width', (e.loaded / e.total) * 100 + '%');
        }

        xhr.onreadystatechange = function() {
            if (xhr.readyState == 4 && xhr.status == 200) {
                $('#progressBar').hide(); //прячем прогресс бар
                clearForm(); //очищаем форму
                var auction = JSON.parse(xhr.responseText); //парсим ответ от сервера
                link = 'http://cocktion.com/Auction/CurrentAuction/' + auction.Id; //фигачим ссылочку

                //показываем кнопки
                //$("#goToAuctionBtn").show();
                $("#readyInfo").show();
                $("#readyInfo").append("<a href=\"" + link + "\">" + 'Перейти на аукцион!' + "</a><br/>");
                $("#createAuctionBtn").show();
            };
        }

        xhr.open('POST', '/BidAuctionCreator/CreateAuction');
        xhr.send(formData); //отправка данных
    }
}

var link;

function goToAuction() {
    window.location = link;
}

/**
 * Очищает чекбокс по айдишнику
 * @param checkBoxId
 */
function clearCheckboxes(checkBoxId) {
    switch (checkBoxId) {
        case 'bookRadio':
            $('#bookRadio').prop('checked', false);
            break;
        case 'productRadio':
            $('#productRadio').prop('checked', false);
            break;
        case 'serviceRadio':
            $('#serviceRadio').prop('checked', false);
            break;
        case '4hoursTime':
            $('#4hoursTime').prop('checked', false);
            break;
        case '1dayTime':
            $("#1dayTime").prop('checked', false);
            break;
        case '1weekTime':
            $("#1weekTime").prop('checked', false);
            break;

    }
}

/**
 * Чистит всю форму после отправки
 */
function clearForm() {
    $("#productName").val("");
    $("#descriptionName").val("");
    $("#fileInput").val("");
    clearCheckboxes(timeBound);
    clearCheckboxes(productType);
}
