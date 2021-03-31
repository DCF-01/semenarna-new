const delete_user_btn = document.querySelectorAll('.delete-user-btn');
const current_url = window.location.href;

delete_user_btn.forEach(element => {
    element.addEventListener('click', (e) => {
        e = e || window.event;
        let target = e.target || e.srcElement;
        let id = target.parentNode.childNodes[1].textContent;
        console.log(id);

        fetch(current_url, {
            method: 'DELETE', // *GET, POST, PUT, DELETE, etc.
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `id=${id}` // body data type must match "Content-Type" header
        }).then((res) => {
            if (res.status === 200) {
                console.log('user deleted');
                location.reload();
            }
            else {
                console.log(res.status);
            }
        });

    });
});


    /*
    });*/
