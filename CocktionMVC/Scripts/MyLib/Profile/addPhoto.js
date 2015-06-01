//Добавляет пользователю аватарку
function showPhotoForm() {
    $("#addPhotoContainer").show();
}

function checkPhoto() {
    var ext = (/[.]/.exec(fileInput.files[0].name)) ? /[^.]+$/.exec(fileInput.files[0].name) : undefined;
    if ((ext == "jpeg") || (ext == "jpg") || (ext == "png")) {
        return true;
    }
    else {
        alert("Формат является недопустимым при загрузке фотографий. Пожалуйста, попробуйте все-таки с фотографией");
        return false;
    }
} 

function addPhoto() {
    if (fileInput.files.length > 0) {
        if (checkPhoto()) {
            $("#progressBarContainer").show();
            var formData = new FormData();
            formData.append(fileInput.files[0].name, fileInput.files[0]);
            var xhr = new XMLHttpRequest();

            xhr.upload.onprogress = function (e) {
                $('#bar').css('width', (e.loaded / e.total) * 100 + '%');
            }

            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    var respond = JSON.parse(xhr.responseText);
                    if (respond.Status == "Success") {
                        $("#avatar").attr('src', 'http://cocktion.com/Images/Thumbnails/' + respond.FileName);
                        $('#bar').css('width', 0 + '%');
                        $("#progressBarContainer").hide();
                    } else {
                        $('#bar').css('background-color', 'red');
                        $('#bar').css('width', '100%');
                    }
                };
            }

            xhr.open('POST', '/Profile/AddPhotoToUser');
            xhr.send(formData); //отправка данных
        }
    } else {
        $("#addPhotoBtn").attr('disabled', 'disabled');
    }
}

function showAddPhotoButton() {
    fileInput = document.getElementById('usersPhoto');
    if (fileInput.files.length > 0) {
        $("#addPhotoBtn").removeAttr('disabled');
    }
}

var fileInput;

