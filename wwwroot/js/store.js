const store_search_box = document.getElementById('store-search');
const search_suggestions_box = document.getElementById('search-suggestions');
let query_url = 'https://localhost:44380/Store/Query/Find';
let query_string;
let table_body = document.getElementById('table-body');
let alert_container = document.getElementById('alert-container');

let add_to_cart_btns = document.querySelectorAll('.add-to-cart-btn');

let timeout_toast;

function displayQuerySearch(data, url_param) {
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
        div.className = 'd-flex align-items-center justify-content-between';

        //p element (name)
        let name = document.createElement('p');
        name.textContent = data[i].name
        name.className = 'm-0 p-2';

        //p element (description)
        let price = document.createElement('p');
        price.textContent = data[i].price;
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
        link_all.href = 'https://localhost:44380/Store/Base/Find' + url_param + query_string;
        new_node.appendChild(link_all);

        search_suggestions_box.appendChild(new_node);
    }

}

function isNumeric(str) {
    if (typeof (str) != 'string') {
        return false;
    }
    return !isNaN(str) && !isNaN(parseFloat(str));
}

function clearSearchBox() {
    search_suggestions_box.innerHTML = '';
}

if (store_search_box !== null) {
    store_search_box.addEventListener('keyup', () => {
        clearSearchBox();

        query_string = store_search_box.value;
        if (query_string.length >= 1) {
            let url_param;
            let query;
            let check_active_category = document.querySelector('#active-category');

            if (check_active_category != null) {
                query_url
            }

            if (isNumeric(query_string)) {
                url_param = '?product_id='
                query = query_url + url_param + parseInt(query_string);
            }
            else {
                url_param = '?name='
                query = query_url + url_param + query_string;
            }


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
                        displayQuerySearch(e, url_param);
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
// cart objects
let cart;
let cart_str;
let cart_obj;


//helper var / check if element exists
let cart_table_exists


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


function checkUserLogin(callback) {
    let query = 'https://localhost:44380/Auth/CheckLogin';
    return fetch(query, {
        method: 'GET', // POST, PUT, DELETE, etc.
        headers: {
            'Content-Type': 'text/plain;charset=UTF-8'
        },
        credentials: 'include', // omit, include
        redirect: 'follow', // manual, error
    }).then((response) => {
        if (response.ok) {
            response.json().then((data) => {
                //true or false (login status)
                let result = JSON.parse(data);
                callback(result);
            });
        }
        else {
            console.log(response.status);
        }
    })

}


function getUserCart(callback) {
    let query = 'https://localhost:44380/Cart/Base/Get';
    cart = new Cart([], null);

    let userCart = fetch(query, {
        method: 'GET',
        headers: {
            'Content-Type': 'text/plain;charset=UTF-8'
        },
        credentials: 'include',
        redirect: 'follow',
    })
        .then(res => {
            if (res.ok) {
                res.json()
                    .then(data => {
                        /*let product = new Product(data.id, data.name, data.price, 1, data.img);*/

                        if (data !== null) {
                            cart = new Cart(data.items, null);
                        }
                        localStorage.setItem('cart', JSON.stringify(cart));
                        callback(cart);
                    });
            }
        })

    return userCart;
}

function displayProducts(loggedStatus) {
    if (loggedStatus === true) {
        getUserCart(listCartItems);
    }
    else {
        initCart()
        if (cart_table_exists !== null) {
            listCartItems(cart);
        }
    }

}

//init cart on window load
window.onload = (event) => {
    // cart objects
    cart = null;
    cart_str = localStorage.getItem('cart');
    cart_obj = JSON.parse(cart_str);

    //helper variable
    cart_table_exists = document.querySelector('#cart-table');

    //if logged in, executes the callback (1st arg);
    checkUserLogin(displayProducts)

}


//use existing or create new cart
function initCart() {
    if (cart_str !== null) {
        cart = new Cart(cart_obj.items, cart_obj.userId);
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
            method: 'GET',
            headers: {
                'Content-Type': 'text/plain;charset=UTF-8'
            },
            credentials: 'include',
            redirect: 'follow',
        }).then(res => {
            if (res.ok) {
                res.json()
                    .then(data => {
                        let product = new Product(data.productId, data.name, data.price, 1, data.img);

                        cart.add(product);
                        console.log(cart);

                        window.localStorage.setItem('cart', JSON.stringify(cart));

                        checkUserLogin(addCartToDb);

                        //create alert product added
                        createAlert();

                        timeout_toast = setTimeout(closeAlert, 4000);


                    });
            }
        });
    });
});
//update client cart then check login and update back-end if logged in
function updateCartQuantity(id, quantity) {
    if (cart.items !== null) {
        cart.items.forEach(item => {
            if (item.id === parseInt(id)) {
                let int_quantity = parseInt(quantity);
                //only update if !== 
                if (int_quantity !== item.quantity) {
                    item.quantity = int_quantity;
                    //add to localstorage and update backend cart
                    localStorage.setItem('cart', JSON.stringify(cart));
                    checkUserLogin(addCartToDb);
                }
            }
        });

    }
}

function addCartToDb(loggedStatus) {
    if (loggedStatus === true) {

        //add all product ID's to str array
        let array_products = [];
        let temp = JSON.parse(localStorage.getItem('cart'));
        /*let product = new Product(data.id, data.name, data.price, 1, data.img);*/
        //only id and quantity


        temp.items.forEach(i => {
            let p = new Product(i.id, null, null, i.quantity, null);
            array_products.push(p);
        });

        let cart_to_server = new Cart(array_products, null);

        let query = 'https://localhost:44380/Cart/Base/Update'
        fetch(query, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            redirect: 'follow',
            body: JSON.stringify(cart_to_server)
        }).then((response) => {
            if (response.ok) {
                console.log('CartDb updated');
            }
            else {
                console.log(response.status, 'error');
            }
        });
    }
}

function listCartItems(cart) {
    if (checkCart() && cart_table_exists !== null) {
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
            let current_quantity = item.quantity;
            td_4.innerHTML = `<span class='quantity-text'>${current_quantity}</span> <a href='#' class='cart-edit-quantity'>Edit</a>`;

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





        });
        let cart_edit_nodes = document.querySelectorAll('.cart-edit-quantity');
        cart_edit_nodes.forEach(i => {
            i.addEventListener('click', (e) => {
                openInputBox(e);
            })
        })
    }
}

function openInputBox(event) {
    let parent = event.target.parentNode;
    console.log(parent);

    let current_quantity = parent.querySelector('.quantity-text').textContent;

    parent.innerHTML = `<input type="number" class="quantity-input" name="quantity" min="1" value=${current_quantity}> <a href='#' class='cart-update-quantity'>Update</a>`;

    //input box element
    let input_value_el = parent.querySelector('.quantity-input');

    //update button (a element)
    let cart_update_quantity = document.querySelector('.cart-update-quantity');

    cart_update_quantity.addEventListener('click', (e) => {
        //parent node (td)
        let parent = e.target.parentNode;
        console.log(e.target.parentNode);

        //update current html quantity from input box value
        current_quantity = input_value_el.value;
        parent.innerHTML = `<span class='quantity-text'>${current_quantity}</span> <a href='#' class='cart-edit-quantity'>Edit</a>`;
        let quantity_text = parent.querySelector('.quantity-text');
        quantity_text.textContent = current_quantity;

        //update js cart quantity
        let td_id = parent.parentNode.querySelector('td');
        let item_id = td_id.textContent;

        updateCartQuantity(item_id, current_quantity);

        let cart_edit_quantity = parent.querySelector('.cart-edit-quantity');
        cart_edit_quantity.addEventListener('click', (e) => {
            openInputBox(e);
        });
    });
}

let custom_input_clear = document.querySelector('#custom-input-clear');

function clearInput() {
    let custom_input = document.querySelector('.custom-input');
    custom_input.value = '';
}

if (custom_input_clear != null) {
    custom_input_clear.addEventListener('click', (e) => {
        clearInput();
    })
}

//single product gallery images
let gallery_images = document.querySelectorAll('.gallery-image');

if (gallery_images != null) {
    gallery_images.forEach(img => {
        console.log('clicked');
        img.addEventListener('click', (e) => {
            changeProductImage(e.target.src);
        });
    })
}

function changeProductImage(value) {
    let product_image = document.getElementById('product-image');
    product_image.src = value;
}
