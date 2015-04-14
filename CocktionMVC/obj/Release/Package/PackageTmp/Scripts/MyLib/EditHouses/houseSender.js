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