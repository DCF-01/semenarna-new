const delete_user_btn = document.querySelectorAll('.delete-item-btn');
const current_url = window.location.href;

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
let spec_form = document.getElementById('spec-form');
let spec_add_item_group = document.getElementById('spec-add-item-group');
let col_count = 4;


function addSpecRow() {
    let last_row = spec_add_item_group.previousElementSibling;
    let spec_row = last_row.cloneNode(true);

    spec_form.insertBefore(spec_row, spec_add_item_group);
}

function addSpecColumn() {


    let all_rows = document.querySelectorAll('.row-parent');

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
    /*let col_string = `.column-${col_count}`;*/
    let master_node = document.querySelector(`.column-${col_count}`);


    master_node.addEventListener('keyup', (e) => {
        let slave_nodes = document.querySelectorAll('.column-toggle-' + col_count);
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

if (add_row_btn !== null) {
    add_row_btn.addEventListener('click', (e) => {
        console.log('clicked')
        addSpecRow();
    });
}

if (add_col_btn !== null) {
    add_col_btn.addEventListener('click', (e) => {
        console.log('clicked')
        addSpecColumn();
    });
}

let first_column = document.querySelectorAll('.column-1');
let second_column = document.querySelectorAll('.column-2');
let third_column = document.querySelectorAll('.column-3');
let fourth_column = document.querySelectorAll('.column-4');


first_column.forEach(el => {
    el.addEventListener('keyup', (e) => {
        let first_column_toggle = document.querySelectorAll('.column-toggle-1');
        if (e.target.value === '') {
            first_column_toggle.forEach(el => {
                el.disabled = true;
                el.style.opacity = 0.4;
            });
        }
        else {
            first_column_toggle.forEach(el => {
                el.disabled = false;
                el.style.opacity = 1
            });
        }
    })
})
second_column.forEach(el => {
    el.addEventListener('keyup', (e) => {
        let second_column_toggle = document.querySelectorAll('.column-toggle-2');
        if (e.target.value === '') {
            second_column_toggle.forEach(el => {
                el.disabled = true;
                el.style.opacity = 0.4;
            });
        }
        else {
            second_column_toggle.forEach(el => {
                el.disabled = false;
                el.style.opacity = 1
            });
        }
    })
})
third_column.forEach(el => {
    el.addEventListener('keyup', (e) => {
        let third_column_toggle = document.querySelectorAll('.column-toggle-3');
        if (e.target.value === '') {
            third_column_toggle.forEach(el => {
                el.disabled = true;
                el.style.opacity = 0.4;
            });
        }
        else {
            third_column_toggle.forEach(el => {
                el.disabled = false;
                el.style.opacity = 1;
            });
        }
    })
})
fourth_column.forEach(el => {
    el.addEventListener('keyup', (e) => {
        let fourth_column_toggle = document.querySelectorAll('.column-toggle-4');
        if (e.target.value === '') {
            fourth_column_toggle.forEach(el => {
                el.disabled = true;
                el.style.opacity = 0.4;
            });
        }
        else {
            fourth_column_toggle.forEach(el => {
                el.disabled = false;
                el.style.opacity = 1;
            });
        }
    })
})