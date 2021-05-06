const attr = document.createAttribute('disabled');
var check = function () {
    if (document.getElementById('confirm_password').value !== '') {
        if (document.getElementById('password').value ==
            document.getElementById('confirm_password').value) {
            document.getElementById('message').style.color = 'green';
            document.getElementById('message').innerHTML = 'Лозинката е валидна';
            document.getElementById('submit-btn-register').removeAttributeNode(attr);
        } else {
            document.getElementById('message').style.color = 'red';
            document.getElementById('message').innerHTML = 'Лозинката се разликува';
            document.getElementById('submit-btn-register').setAttributeNode(attr);
        }
    }
    else {
        document.getElementById('message').innerHTML = '';
    }
}

function createAlert(message_str = 'An error has occured.', alert_type = 'success') {

    

    let alert_exists = document.querySelector('.alert');
    let code = `<div class="alert alert-${alert_type} alert-dismissible fade show" role = "alert" >
            <strong>${message_str}</strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>`;

    if (alert_exists != null) {
        clearTimeout(alert_timeout);
        alert_timeout = setTimeout(closeAlert, 4000);
        alert_container.innerHTML = code;
    }
    else {
        alert_timeout = setTimeout(closeAlert, 4000);

        alert_container.innerHTML += code;
        let close = alert_container.querySelector('.close');

        close.addEventListener('click', (e) => {
            clearTimeout(alert_timeout);
        });
    }



}