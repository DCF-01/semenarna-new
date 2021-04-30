const attr = document.createAttribute('disabled');
var check = function () {
    if (document.getElementById('password').value !== '') {
        if (document.getElementById('password').value ==
            document.getElementById('confirm_password').value) {
            document.getElementById('message').style.color = 'green';
            document.getElementById('message').innerHTML = 'Пасвордот е валиден';
            document.getElementById('submit-btn-register').removeAttributeNode(attr);
        } else {
            document.getElementById('message').style.color = 'red';
            document.getElementById('message').innerHTML = 'Пасвордот се разликува';
            document.getElementById('submit-btn-register').setAttributeNode(attr);
        }
    }
    else {
        document.getElementById('message').innerHTML = '';
    }
}
