function checkInterest() {
    if ($("#name").val().length == 0) {
        alert("Не введено название холдера");
        return false;
    } else if ($("#fileInput").val().length == 0) {
        alert('Не введена фотография интереса');
        return false;
    }
    else
        return true;
}

function sendInterest() {
    if (checkInterest()) {
        var confirmString = "Название: " + $("#name").val() + "\n" + "Все окей? Добавляю?";
        if (confirm(confirmString)) {
            var formData = new FormData();
            var xhr = new XMLHttpRequest();
            formData.append("name", $("#name").val());
            var fileInput = document.getElementById('fileInput');
            if (fileInput.files.length > 0) {
                formData.append(fileInput.files[0].name, fileInput.files[0]);
            }

            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    $("#name").val("");
                    $("#fileInput").val("");
                    $('#interestBar').css('width', '0%');
                    var respond = JSON.parse(xhr.responseText);
                    alert(respond.Status);
                }
            }

            xhr.upload.onprogress = function (e) {
                $('#interestBar').css('width', (e.loaded / e.total) * 100 + '%');
            }

            xhr.open("POST", "/EditInterests/AddInterest");
            xhr.send(formData);
        }
    }
}