const delete_user_btn = document.querySelectorAll('.delete-item-btn');
const current_url = window.location.href;
let alert_container = document.getElementById('alert-container-panel');
var alert_timeout;

delete_user_btn.forEach(element => {
    element.addEventListener('click', (e) => {

        console.log('clicked');
        e = e || window.event;
        let target = e.target || e.srcElement;
        let id = target.parentNode.childNodes[1].textContent;
        let delete_url = current_url + `/Delete/${id}`;
        console.log(delete_url);

        fetch(delete_url, {
            method: 'DELETE', // *GET, POST, PUT, DELETE, etc.
            credentials: 'include',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            redirect: 'follow'
        }).then((res) => {
            if (res.ok) {
                window.location.reload();
                console.log('item-deleted')
            }
            else {
                console.log(res.status);
            }

        });

    });
});
/* add new spec row on click*/
let add_row_btn = document.getElementById('add-row-btn');
let add_col_btn = document.getElementById('add-col-btn');

/* remove new spec row onclick*/
let remove_row_btn = document.getElementById('remove-row-btn');
let remove_col_btn = document.getElementById('remove-col-btn');



let spec_form = document.getElementById('spec-form');
let spec_add_item_group = document.getElementById('spec-add-item-group');



function removeSpecRow() {
    let all_rows = document.querySelectorAll('.row-parent');
    if (all_rows.length > 2) {
        let node_to_remove = all_rows[all_rows.length - 1];
        node_to_remove.remove();
    }
    else {
        //toast
        let alert_exists = document.querySelector('.alert');
        if (alert_exists != null) {
            clearTimeout(alert_timeout);
            createAlert('Action canceled: There must be at least 2 rows.');
            alert_timeout = setTimeout(closeAlert, 4000);
        }
        else {
            createAlert('Action canceled: There must be at least 2 rows.');
            alert_timeout = setTimeout(closeAlert, 4000);
        }
    }


}

function removeSpecCol() {
    let all_rows = document.querySelectorAll('.row-parent');
    if (all_rows[0].childElementCount > 2) {
        all_rows.forEach(row => {
            let node_to_remove = row.lastElementChild;
            node_to_remove.remove();
        });
    }
    else {
        //toast
        let alert_exists = document.querySelector('.alert');
        if (alert_exists != null) {
            clearTimeout(alert_timeout);
            createAlert('Action canceled: There must be at least 2 columns.');
            alert_timeout = setTimeout(closeAlert, 4000);
        }
        else {
            createAlert('Action canceled: There must be at least 2 columns.');
            alert_timeout = setTimeout(closeAlert, 4000);
        }
    }
}

function addSpecRow() {
    let row_count = document.querySelectorAll('row-parent').childElementCount;

    let last_row = spec_add_item_group.previousElementSibling;
    //new row
    let spec_row = last_row.cloneNode(true);
    spec_row.firstChild.className = `form-control column-toggle-${row_count + 1}`;
    spec_form.insertBefore(spec_row, spec_add_item_group);

    addListener();
}

function addSpecCol() {


    let all_rows = document.querySelectorAll('.row-parent');

    let first_row = document.querySelector('.row-parent');
    let col_count = first_row.childElementCount;

    col_count += 1;
    all_rows.forEach(row => {
        let last_node = row.lastElementChild;
        let new_node = last_node.cloneNode(true);

        let node_name = new_node.lastElementChild.getAttribute('name');
        if (node_name === 'First[]') {
            new_node.lastElementChild.className = 'form-control column-' + col_count;
        }
        else {
            new_node.lastElementChild.className = 'form-control column-toggle-' + col_count;
        }
        row.appendChild(new_node);

    });
    addListener();
}

if (add_row_btn !== null) {
    add_row_btn.addEventListener('click', (e) => {
        console.log('clicked')
        addSpecRow();
    });
}

if (add_col_btn !== null) {
    add_col_btn.addEventListener('click', (e) => {
        console.log('clicked')
        addSpecCol();
    });
}

if (remove_row_btn !== null) {
    remove_row_btn.addEventListener('click', (e) => {
        console.log('clicked')
        removeSpecRow();
    });
}

if (remove_col_btn !== null) {
    remove_col_btn.addEventListener('click', (e) => {
        console.log('clicked')
        removeSpecCol();
    });
}

//add eventListener to all master nodes
function addListener(element = null) {
    let parent_el = document.querySelector('.row-parent');
    let num_of_cols = parent_el.childElementCount;

    for (i = 1; i <= num_of_cols; i++) {
        let master_node = document.querySelector(`.column-${i}`);
        let slave_nodes = document.querySelectorAll(`.column-toggle-${i}`);

        master_node.addEventListener('keyup', (e) => {
            if (e.target.value === '') {
                slave_nodes.forEach(el => {
                    el.disabled = true;
                    el.style.opacity = 0.4;
                });
            }
            else {
                slave_nodes.forEach(el => {
                    el.disabled = false;
                    el.style.opacity = 1
                });
            }
        });
    }

}
function createAlert(message_str = 'An error has occured.') {
    let code = `<div class="alert alert-danger alert-dismissible fade show" role = "alert" >
            <strong>${message_str}</strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>`;

    alert_container.innerHTML += code;

}

function closeAlert() {
    $('.alert').alert('close');
}


//exec once on js load
addListener();