function printHouses(btn) {
    var text = btn.innerHTML;
    btn.focus();
    $("#houses").empty();
    $("#houses").append("</br>");
    for (var i = 0; i < a.length; i++) {
        if (a[i].university == text) {
            if (a[i].adress.length > 0) {
                $("#houses").append('<p><input type=\"checkbox\" name=\"houseCheckbox\" id=\" ' + a[i].id + '\" />' + ' фак-т ' + a[i].faculty + ' ,' + a[i].adress + '</p>');

            } else {
                $("#houses").append('<p><input type=\"checkbox\" name=\"houseCheckbox\" id=\" ' + a[i].id + '\" />' + ' фак-т ' + a[i].faculty + ' ,' + ' где-то в Москве...' + '</p>');

            }
        }
    }
}
var a = new Array();