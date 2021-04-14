const store_search_box = document.getElementById('store-search');
const search_suggestions_box = document.getElementById('search-suggestions');
let query_url = 'https://localhost:44380/Store/Query/Find';
let query_string;
let table_body = document.getElementById('table-body');
let alert_container = document.getElementById('alert-container');

let add_to_cart_btns = document.querySelectorAll('.add-to-cart-btn');

let timeout_toast;

function displayQuerySearch(data) {
    query_string = store_search_box.value;

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

if (store_search_box !== null) {
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
}


/* CART */

class Product {
    constructor(id, name, price, quantity, img) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.quantity = quantity;
        this.img = img;
    }
}

class Cart {
    constructor(items, userId) {
        this.items = items;
        this.userId = userId;
    }


    add(item) {
        let length = this.items.length;
        if (length > 0) {
            for (let i = 0; i < length; i++) {
                if (this.items[i].id === item.id) {
                    this.items[i].quantity += 1;
                    return true;
                }
                /*if (i === length - 1){
                    this.items.push(item);
                    return true;
                }*/
            }
            this.items.push(item);
            return true
        }
        else {
            this.items.push(item);
            return true
        }
    }

    getById(id) {
        this.items.forEach(e => {
            if (e.id === id) {
                return e;
            }
            return null;
        });
    }

    /* get userId() {
         return this.userId
     }
 
     set userId(id) {
         this.userId = id;
     }*/

    /*get items() {
        return this.items;
    }*/

    /*set items(updated_items) {
        this.items = updated_items;
    }*/

    remove(id) {
        let temp = this.items;
        this.items = [];
        this.temp.foreach(item => {
            if (item.id !== id) {
                this.items.push(item);
            }
        })
    }

}
// cart objects
let cart = null;
let cart_str = window.localStorage.getItem('cart');
let cart_obj = JSON.parse(cart_str);

async function checkUserLogin() {
    let query = 'https://localhost:44380/Auth/CheckLogin';
    fetch(query, {
        method: 'GET', // POST, PUT, DELETE, etc.
        headers: {
            'Content-Type': 'text/plain;charset=UTF-8'
        },
        credentials: 'include', // omit, include
        redirect: 'follow', // manual, error
    }).then(res => {
        if (res.ok) {
            res.json()
                .then(data => {
                    if (JSON.parse(data) === true) {
                        console.log(true);
                        return true;
                    }
                    else {
                        console.log(false);
                        return false;
                    }
                });
        }
    })
}

async function getUserCart() {
    let query = 

    let userCart = fetch(query, {
        method: 'GET', // POST, PUT, DELETE, etc.
        headers: {
            'Content-Type': 'text/plain;charset=UTF-8'
        },
        credentials: 'include', // omit, include
        redirect: 'follow', // manual, error
    })
        .then(res => {
            if (res.ok) {
                res.json()
                    .then(data => {
                        return JSON.parse(data);
                    })
            }
        })

    return userCart;
}



//init cart on window load
window.onload = (event) => {
    checkUserLogin().then(res => {
        res.json()
            .then(value => {
                if (JSON.parse(value) === true) {
                    cart = getUserCart().then(data => {
                        return JSON.parse(data);
                    })
                }
                else {
                    initCart();
                }
            })
    });
    //list items
    if (window.location.href === 'https://localhost:44380/Cart') {
        listCartItems();
    }
}



function initCart() {
    if (cart_str !== null) {
        cart = new Cart(cart_obj.items, cart_obj.userId);
        return true;
    }
    else {
        cart = new Cart([], null);
    }
}

function checkCart() {
    if (cart.items.length > 0) {
        return true;
    }
    return false;
}

function createAlert() {
    let code = `<div class="alert alert-success alert-dismissible fade show" role = "alert" >
            <strong>Производот е додаден во вашата кошничка</strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>`;

    alert_container.innerHTML += code;

}

function closeAlert() {
    $('.alert').alert('close');
}

add_to_cart_btns.forEach(el => {
    el.addEventListener('click', (e) => {

        let element = e.target;
        let id = element.querySelector('.item-id').textContent;

        let query = 'https://localhost:44380/Store/Query/Single/' + id;

        fetch(query, {
            method: 'GET', // POST, PUT, DELETE, etc.
            headers: {
                'Content-Type': 'text/plain;charset=UTF-8'
            },
            credentials: 'include', // omit, include
            redirect: 'follow', // manual, error
        }).then(res => {
            if (res.ok) {
                res.json()
                    .then(data => {
                        let product = new Product(data.id, data.name, data.price, 1, data.img);

                        cart.add(product);
                        console.log(cart);

                        window.localStorage.setItem('cart', JSON.stringify(cart));

                        //create alert product added
                        createAlert();

                        timeout_toast = setTimeout(closeAlert, 4000);



                        /*alert_container.*/
                    });
            }
        })
        /*console.log(id);*/
    })
})



function listCartItems() {
    if (checkCart()) {
        let count = 0;
        cart.items.forEach(item => {
            let tr = document.createElement('tr');

            //table heading
            let th = document.createElement('th');
            count += 1;
            th.textContent = count;
            th.scope = 'row';

            let td_1 = document.createElement('td');
            td_1.textContent = item.id
            td_1.style.display = 'none';

            let td_2 = document.createElement('td');
            td_2.textContent = item.name

            let td_3 = document.createElement('td');
            td_3.textContent = item.price

            let td_4 = document.createElement('td');
            td_4.textContent = item.quantity

            let td_5 = document.createElement('td');
            let img = document.createElement('img');
            img.width = 48;
            img.src = 'data:image/jpeg;base64, ' + item.img;
            td_5.appendChild(img);



            tr.appendChild(th);
            tr.appendChild(td_1);
            tr.appendChild(td_2);
            tr.appendChild(td_3);
            tr.appendChild(td_4);
            tr.appendChild(td_5);

            table_body.appendChild(tr);

        })
    }
}



/*https://localhost:44380/Store/Query/Single?id=47*/