const store_search_box = document.getElementById('store-search');
const search_suggestions_box = document.getElementById('search-suggestions');
let query_url = 'https://localhost:44380/Store/Query/Find';
let query_string = store_search_box.value;

function displayQuerySearch(data) {
    let max = 5;
    if (data.length < 5) {
        max = data.length;
    }

    console.log(data);
    for (let i = 0; i < max; i++) {
        //list item
        const new_node = document.createElement('li');
        new_node.className = 'list-group-item';
        /*new_node.textContent = data[i].name;*/

        //div row d-flex align-items-center
        let div = document.createElement('div');
        div.className = 'd-flex align-items-center';

        //p element (name)
        let name = document.createElement('p');
        name.textContent = data[i].name
        name.className = 'm-0 p-2';

        //p element (description)
        let price = document.createElement('p');
        price.textContent = data[i].description
        price.className = 'm-0 p-2';

        //img element (image)
        let image = document.createElement('img');
        image.src = 'data:image/png;base64, ' + data[i].img;
        image.width = 48;

        div.appendChild(name);
        div.appendChild(price);
        div.appendChild(image);

        new_node.appendChild(div);

        search_suggestions_box.appendChild(new_node);
    }

    if (max >= 1) {
        const new_node = document.createElement('li');
        new_node.className = 'list-group-item';

        const link_all = document.createElement('a');
        link_all.textContent = 'View All';
        link_all.href = 'https://localhost:44380/Store/Base/Find?name=' + query_string;
        new_node.appendChild(link_all);

        search_suggestions_box.appendChild(new_node);
    }

}

function clearSearchBox() {
    search_suggestions_box.innerHTML = '';
}


store_search_box.addEventListener('keyup', () => {
    clearSearchBox();

    query_string = store_search_box.value;
    if (query_string.length >= 1) {

        console.log(query_string);

        const query = query_url + '?name=' + query_string;

        fetch(query, {
            method: 'GET', // POST, PUT, DELETE, etc.
            headers: {
                'Content-Type': 'text/plain;charset=UTF-8'
            },
            credentials: 'include', // omit, include
            redirect: 'follow', // manual, error
        }).then(res => {
            if (res.ok) {
                res.json().then(e => {
                    displayQuerySearch(e);
                });
            }
            else {
                console.log(res.status, 'error');
            }
        });
    }

})