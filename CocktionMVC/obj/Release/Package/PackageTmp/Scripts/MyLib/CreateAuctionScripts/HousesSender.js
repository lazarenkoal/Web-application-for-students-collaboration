var checkboxes = document.getElementsByName("houseCheckbox");
function generateStringWithHouses() {
    var houses = "!";
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            houses += checkboxes[i].id + "!";
        }
    }
    return houses;
}
function  creatorOnClick () {
    if ((verifyTime() & verifyFormData())) {
        if (validateFile()) {
            var houses = generateStringWithHouses().toString();
            if (houses == "!") {
                $("#housesErrorField").empty();
                $("#housesErrorField").append("<p>" + "Вы не отметили вообще никаких домов!" + "</p>");
                return false;
            }
            else {
                var formData = new FormData();
                var req = new XMLHttpRequest;
                formData.append("Name", $('#Name').val());
                formData.append("Description", $('#Description').val());
                formData.append("Category", $('#info').text());
                formData.append("Hours", $('#Hours').val());
                formData.append("Minutes", $('#Minutes').val());
                formData.append("HousesId", houses);
                var fileInput = document.getElementById('file');
                var filename1 = fileInput.files[0].name;
                formData.append(fileInput.files[0].name, fileInput.files[0]);
                $('#formContainer').empty();
                $('#formContainer').append('<p>загрузка в процессе</p>');
                req.open("POST", "/BidAuctionCreator/CreateAuction");
                req.send(formData);
                req.onreadystatechange = function () {
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