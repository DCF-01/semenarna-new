const delete_user_btn = document.querySelectorAll('.delete-user-btn');
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
            credentials: 'include'
        }).then((res) => {
            if (res.ok) {
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
