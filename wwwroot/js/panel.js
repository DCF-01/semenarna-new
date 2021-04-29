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
    let spec_row = document.createElement('div');
    spec_row.className = 'form-group row';
    spec_row.innerHTML = `
        <div class="col-3">
            <input type="text" class="form-control" name="First[]">
        </div>
        <div class="col-3">
            <input type="text" class="form-control" name="Second[]">
        </div>
        <div class="col-3">
            <input type="text" class="form-control" name="Third[]">
        </div>
        <div class="col-3">
            <input type="text" class="form-control" name="Fourth[]">
        </div>
`;

    spec_form.insertBefore(spec_row, spec_add_item_group);
}

add_row_btn.addEventListener('click', (e) => {
    console.log('clicked')
    addSpecRow();
})