//Проверяет форму для ввода нового дома
function verifyHouseForm() {
    if ($("#description").val().length == 0) {
        alert("Не введено описание дома!");
        return false;
    } else if ($("#faculty").val().length == 0) {
        alert("Не введен факультет/ общага!");
        return false;
    } else if ($("#adress").val().length == 0) {
        alert("Не введен адрес!");
        return false;
    }  else if ($("#housePhoto").val().length == 0) {
        alert('Нет фотографии дома!!!');
        return false;
    }
    else if ($("#selector").val() == -1) {
        alert('Не выбран холдер!');
        return false;
    } 
    return true;
}

//првоеряет форму с холдером на корректность
function verifyHolderForm() {
    if ($("#holderName").val().length == 0) {
        alert("Не введено название холдера");
        return false;
    } else if ($("#holderPhoto").val().length == 0) {
        alert('Не введена фотография дома');
        return false;
    } else if ($("#holderCity").val().length == 0) {
        alert('Не введен город ');
        return false;
    }
    else
        return true;
}