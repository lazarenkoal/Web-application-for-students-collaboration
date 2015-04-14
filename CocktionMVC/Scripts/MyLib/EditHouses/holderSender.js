//посылает на сервак holder
function sendHolderToServer() {

    if (verifyHolderForm()) {
        var confirmString = "Название: " + $("#holderName").val() + "\n" + "Все окей? Добавляю?";
        if (confirm(confirmString)) {

            var formData = new FormData();
            var xhr = new XMLHttpRequest();
            formData.append("holderName", $("#holderName").val());
            var fileInput = document.getElementById('holderPhoto');
            if (fileInput.files.length > 0) {
                formData.append(fileInput.files[0].name, fileInput.files[0]);
            }
            
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    alert("Ты все четко добавил;)");
                    $("#holderName").val("")
                    $("#holderPhoto").val("");
                    $('#holderBar').css('width', '0%');
                    var respond = JSON.parse(xhr.responseText);
                    alert(respond.Status);
                }
            }

            xhr.upload.onprogress = function (e) {
                $('#holderBar').css('width', (e.loaded / e.total) * 100 + '%');
            }

            xhr.open("POST", "/EditHouses/AddHolder");
            xhr.send(formData);
        }
    }
}
