/*Ищет университет в базе данных
  Динамически выводит результат на экран
*/
function searchUniversity() {
    $("#universitySearchResults").empty();
    $('#chooseFacultyRadio').prop('checked', false);
    $('#allFacultyRadio').prop('checked', false);
    $("#facultyToChoose").empty();
    if ($("#universitySearch").val().length > 0) {
        var xhr = new XMLHttpRequest();


        var formData = new FormData();
        formData.append('university', $("#universitySearch").val().trim());

        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var searchResults = JSON.parse(xhr.responseText);
                if (searchResults.IsSearchEmpty == true) {
                    if (!document.getElementById('emptySearch')) {
                        $("#universitySearchResults").append(generateSearchRespond('Тю-тю...', false));
                    }
                } else {
                    var univLength = searchResults.Names.length;
                    for (var i = 0; i < univLength; i++) {
                        if (!document.getElementById(searchResults.Names[i])) {
                            $("#universitySearchResults").append(generateSearchRespond(searchResults.Names[i], true));
                        }
                    }
                }
            };
        }

        xhr.open('POST', '/Search/SearchUniversity');
        xhr.send(formData); //отправка данных
    }
}

function generateSearchRespond(respondString, status) {
    if (status) {
        return "<div onclick=\"onClickHandler(this.id)\" onmouseout=\"onMouseOutHandler(this.id)\" onmouseover=\"onMouseOverHandler(this.id)\" class =\"searchRespond\" id=\"" + respondString + "\">" +
        respondString + "</div>";
    }
    return "<div  class =\"searchRespond\" id=\"emptySearch\">" +
        respondString + "</div>";
}

function onMouseOverHandler(searchResult) {
    $("#" + searchResult).css('background-color', 'blue');
}

function onMouseOutHandler(param) {
    $("#" + param).css('background-color', 'whitesmoke');
}

function onClickHandler(param) {
    $("#universitySearch").val(param);
    $("#universitySearchResults").empty();
}

function constructFacultyToAppend(facultyName, facultyId) {
    return "<p><input type=\"checkbox\" onclick=\"addCheckedId(this)\" id=\"" + facultyId + "\"> " + facultyName + "</p>";
}

function getFacultyList() {
    var xhr = new XMLHttpRequest();
    $("#facultyToChoose").empty();
    var formData = new FormData();
    formData.append('university', $("#universitySearch").val().trim());


    xhr.onreadystatechange = function() {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var faculties = JSON.parse(xhr.responseText);
            if (faculties.IsEmpty == false) {
                $("#universitySearch").css('border-color', 'rgb(204, 204, 204)');
                for (var i = 0; i < faculties.Names.length; i++) {
                    $("#facultyToChoose").append(constructFacultyToAppend(faculties.Names[i], faculties.Ids[i]));
                }
            } else {
                $("#universitySearch").css('border-color', 'red');
            }
        }
    }

    xhr.open('POST', '/Search/GetFacultyList');
    xhr.send(formData); //отправка данных
}

function facultyRadioCheckHandler(btn) {
    $("#universitySearchResults").empty();
    switch (btn.id) {
        case 'allFacultyRadio':
        housesIds = new Array();
        $('#chooseFacultyRadio').prop('checked', false);
        $("#facultyToChoose").empty();
        break;
    case 'chooseFacultyRadio':
        $('#allFacultyRadio').prop('checked', false);
        getFacultyList();
        break;
    }
}

var housesIds = new Array();

function addCheckedId(btn) {
    if (btn.checked) {
        housesIds.push(btn.id);
    }
}

var ids = "?";

function createStringOfHouses() {
    if (housesIds.length == 0) {
        ids = $("#universitySearch").val();
    } else {
        for (var i = 0; i < housesIds.length; i++) {
            ids += (housesIds[i] + '!');
        }
    }
    return ids;
}
