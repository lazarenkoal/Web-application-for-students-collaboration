function sendHouseToServer() {
    if (verifyForm()) {
        var confirmString = "ВУЗ: " + $("#university").val() + "\n" + "Факультет: " + $("#faculty").val() +
           "\n" + "Aдрес: " + $("#adress").val() + "\n" + "Все окей? Добавляю?";
        if (confirm(confirmString)) {
            var formData = new FormData();
            var xhr = new XMLHttpRequest();
            formData.append("university", $("#university").val());
            formData.append("faculty", $("#faculty").val());
            formData.append("adress", $("#adress").val());
            xhr.open("POST", "/EditHouses/AddHouse");
            xhr.send(formData);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    alert("Ты все четко добавила;)");
                    $("#university").val("");
                    $("#faculty").val("");
                    $("#adress").val("");
                    var respond = JSON.parse(xhr.responseText);
                    $("#houseList").prepend("<p>" + respond.House.toString() + "</p>");
                }
            }
        }
    }
}