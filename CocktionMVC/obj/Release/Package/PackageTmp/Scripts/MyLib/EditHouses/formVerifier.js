function verifyForm() {
    if ($("#university").val().length == 0) {
        alert("Ты не ввела университет!");
        return false;
    } else if ($("#faculty").val().length == 0) {
        alert("Ты не ввела факультет!");
        return false;
    } else if ($("#adress").val().length == 0) {
        alert("Ты не ввела адрес!");
        return false;
    }
    else
        return true;
}