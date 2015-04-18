function showEditInfoForm() {
    $("#editInformationContainer").show();
}

function sendInfo(){
    if (checkInputs()) {
        var formData = new FormData();
        var xhr = new XMLHttpRequest();
        $("#infoBarContainer").show();
        formData.append('name', $("#name").val());
        formData.append('surname', $("#surname").val());
        formData.append('school', $("#school").val());

        xhr.upload.onprogress = function (e) {
            $('#infoBar').css('width', (e.loaded / e.total) * 100 + '%');
        }

        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var respond = JSON.parse(xhr.responseText);
                if (respond.Status == "Success") {
                    $("#userRates").append("<p>Информация обновлена</p>");
                    $("#infoBarContainer").hide();
                } else {
                    $("#infoBar").css('background-color', 'red');
                    $("#infoBar").css('width', '100%');
                }
            };
        }

        xhr.open('POST', '/Profile/EditProfileInformation');
        xhr.send(formData); //отправка данных
    }
}

function showInterestsForm() {
    $("#showInterestsFormBtn").hide();
    $("#interestsToChoose").show();
}

var interestsIds = "";
function addId(check) {
    $("#sendInterestsBtn").show();
    interestsIds += check.id;
}

function sendInt() {
    var formData = new FormData();
    var xhr = new XMLHttpRequest();
   
    formData.append('ids', interestsIds);

    xhr.upload.onprogress = function (e) {
        $('#interestsBar').css('width', (e.loaded / e.total) * 100 + '%');
    }

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var respond = JSON.parse(xhr.responseText);
            if (respond.Status == "Success") {
                $("#userRates").append("<p>Информация обновлена</p>");
                $("#interestsBarContainer").hide();
            } else {
                $("#interestsBar").css('background-color', 'red');
                $("#interestsBar").css('width', '100%');
            }
        };
    }

    xhr.open('POST', '/Profile/AddInterests');
    xhr.send(formData); //отправка данных
}


function checkInputs() {
    var school = $("#school").val();
    var name = $("name").val();
    var surname = $("surname").val();
    if (school || name || surname) {
        return true;
    } else {
        $("#errorField").show();
        $("#errorField").empty();
        $("#errorField").append("<p>Нужно добавить хотьк какую-нибудь информацию </p>");
    }
}

function handleInfoChange() {
    $("#errorField").empty();
    $("#errorField").hide();
}