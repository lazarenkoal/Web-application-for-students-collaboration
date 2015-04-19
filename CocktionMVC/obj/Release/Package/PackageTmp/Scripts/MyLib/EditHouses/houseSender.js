//Посылает новый дом на сервак
function sendHouseToServer() {
    if (verifyHouseForm()) {

        var confirmString = "Описание: " + $("#description").val() + "\n" + "Факультет: " + $("#faculty").val() +
           "\n" + "Aдрес: " + $("#adress").val() + "\n" + "Все окей? Добавляю?";
        if (confirm(confirmString)) {

            var formData = new FormData();
            var xhr = new XMLHttpRequest();

            formData.append("description", $("#description").val());
            formData.append("faculty", $("#faculty").val());
            formData.append("adress", $("#adress").val());
            formData.append("holderId", $("#selector").val())
            var fileInput = document.getElementById('housePhoto');
            if (fileInput.files.length > 0) {
                formData.append(fileInput.files[0].name, fileInput.files[0]);
            }

            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    alert("Ты все четко добавил;)");
                    $("#description").val("");
                    $("#faculty").val("");
                    $("#adress").val("");
                    $("#housePhoto").val("");
                    $('#houseBar').css('width', '0%');
                    var respond = JSON.parse(xhr.responseText);
                    alert(respond.Status);
                }//end of if
            }//end of onreadystatechange

            xhr.upload.onprogress = function (e) {
                $('#houseBar').css('width', (e.loaded / e.total) * 100 + '%');
            }

            xhr.open("POST", "/EditHouses/AddHouse");
            xhr.send(formData);
        }//end of if(confirm...
    }//enf of of(verify..
}//end of sendHouse...

function deleteHouse(id, houseName) {

    var confirmString = "Удалить дом " + houseName + "?";
    if (confirm(confirmString)) {
        var formData = new FormData();
        formData.append("houseId", id);
        var xhr = new XMLHttpRequest();

        xhr.onreadystatechange = function() {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var respond = JSON.parse(xhr.responseText);
                if (respond.Status == "Success") {
                    alert("Дом " + houseName + " успешно удален");
                } else {
                    alert("Произошла какая-то ошибка при удалении");
                }
            } //end of if
        } //end of onreadystatechange

        xhr.open("POST", "/EditHouses/DeleteHouse");
        xhr.send(formData);
    }
}

function deleteHolder(id, holderName) {
    var confirmString = "Удалить холдер " + holderName + "?";
    if (confirm(confirmString)) {
        var formData = new FormData();
        formData.append("holderId", id);
        var xhr = new XMLHttpRequest();

        xhr.onreadystatechange = function() {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var respond = JSON.parse(xhr.responseText);
                if (respond.Status == "Success") {
                    alert("Холдер " + holderName + " успешно удален!");
                } else {
                    alert("Произошла какая-то ошибка при удалении");
                }
            } //end of if
        } //end of onreadystatechange

        xhr.open("POST", "/EditHouses/DeleteHolder");
        xhr.send(formData);
    }
}

function showEditHouseForm(id) {
    $("#house_" + id).show();
}

function editHouse(id) {
    var houseName = $("#faculty_" + id).val();
    var houseAdress = $("#adress_" + id).val();
    var houseDescription = $("#description_" + id).val();
    var confirmString = "Измениить дом?";

    if (confirm(confirmString)) {
        var formData = new FormData();

        var fileInput = document.getElementById('housePhoto_' + id);
        if (fileInput.files.length > 0) {
            formData.append(fileInput.files[0].name, fileInput.files[0]);
        }

        formData.append("houseName", houseName);
        formData.append("houseAdress", houseAdress);
        formData.append("houseDescription", houseDescription);
        formData.append("houseId", id);
        var xhr = new XMLHttpRequest();

        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var respond = JSON.parse(xhr.responseText);
                if (respond.Status == "Success") {
                    alert("Дом успешно изменен!");
                } else {
                    alert("Произошла какая-то ошибка при изменении");
                }
            } //end of if
        } //end of onreadystatechange

        xhr.open("POST", "/EditHouses/EditHouse");
        xhr.send(formData);
    }
}

function showEditHolderPanel(id) {
    $("#holder_" + id).show();
}

function editHolder(id) {
    var holderName = $("#holderName_" + id).val();
    var holderCity = $("#holderCity_" + id).val();
    var confirmString = "Измениить холдер?";

    if (confirm(confirmString)) {
        var formData = new FormData();

        var fileInput = document.getElementById('holderPhoto_' + id);
        if (fileInput.files.length > 0) {
            formData.append(fileInput.files[0].name, fileInput.files[0]);
        }

        formData.append("holderName", holderName);
        formData.append("holderCity", holderCity);
        formData.append("holderId", id);
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var respond = JSON.parse(xhr.responseText);
                if (respond.Status == "Success") {
                    alert("Холдер успешно изменен!");
                } else {
                    alert("Произошла какая-то ошибка при изменении");
                }
            } //end of if
        } //end of onreadystatechange

        xhr.open("POST", "/EditHouses/EditHolder");
        xhr.send(formData);
    }
}