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
let spec_form = document.getElementById('spec-form');
let spec_add_item_group = document.getElementById('spec-add-item-group');



function addSpecRow() {
    let last_row = spec_add_item_group.previousElementSibling;
    let spec_row = last_row.cloneNode(true);

    spec_form.insertBefore(spec_row, spec_add_item_group);
}

if (add_row_btn !== null) {
    add_row_btn.addEventListener('click', (e) => {
        console.log('clicked')
        addSpecRow();
    });
}

let first_column = document.querySelectorAll('.first-column');
let second_column = document.querySelectorAll('.second-column');
let third_column = document.querySelectorAll('.third-column');
let fourth_column = document.querySelectorAll('.fourth-column');





first_column.forEach(el => {
    el.addEventListener('keyup', (e) => {
        let first_column_toggle = document.querySelectorAll('.first-column-toggle');
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
        let second_column_toggle = document.querySelectorAll('.second-column-toggle');
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
        let third_column_toggle = document.querySelectorAll('.third-column-toggle');
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
        let fourth_column_toggle = document.querySelectorAll('.fourth-column-toggle');
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