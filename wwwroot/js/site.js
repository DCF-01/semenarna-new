//SITE JS

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


/* STORE JS */



const store_search_box = document.getElementById('store-search');
const search_suggestions_box = document.getElementById('search-suggestions');
let query_url = location.protocol + '//' + location.host + '/Store/Query/Find';
let query_string;
let table_body = document.getElementById('table-body');
let alert_container = document.getElementById('alert-container');
let add_to_cart_btn = document.querySelector('#add-to-cart');
let payment_checkmarks = document.querySelectorAll('.payment-method');
let delivery_checkmarks = document.querySelectorAll('.delivery-method');
let order_submit_btn = document.getElementById('checkout-submit-btn');
let select_lists = document.querySelector('.list');

let timeout_toast;

function displayQuerySearch(data, url_param) {
    clearSearchBox();
    query_string = store_search_box.value;

    /*let max = 5;
    if (data.length < 5) {
        max = data.length;
    }*/

    /*CREATE SEARCH HEADER*/
    //list item
    const new_node = document.createElement('li');
    new_node.className = 'list-group-item';
    /*new_node.textContent = data[i].name;*/

    //div row d-flex align-items-center
    let div = document.createElement('div');
    div.className = 'd-flex align-items-center justify-content-between';

    //p element (name)
    let name = document.createElement('p');
    name.textContent = 'Име';
    name.className = 'm-0 p-2';

    //p element (description)
    let price = document.createElement('p');
    price.textContent = 'Цена';
    price.className = 'm-0 p-2';

    //img element (image)
    let image = document.createElement('p');
    image.textContent = 'Производ';
    image.className = 'm-0 p-2';

    div.appendChild(name);
    div.appendChild(price);
    div.appendChild(image);

    new_node.appendChild(div);

    search_suggestions_box.appendChild(new_node);


    console.log(data);
    data.forEach(el => {
        //product view link
        const product_link = document.createElement('a');
        product_link.href = location.protocol + '//' + location.host + '/Store/Product/Single/' + el.productId;

        //list item
        const new_node = document.createElement('li');
        new_node.className = 'list-group-item search-box-list-item';
        /*new_node.textContent = el.name;*/

        //div row d-flex align-items-center
        let div = document.createElement('div');
        div.className = 'd-flex align-items-center justify-content-between';

        //p element (name)
        let name = document.createElement('p');
        name.textContent = el.name
        name.className = 'm-0 p-2';

        //p element (price)
        let price = document.createElement('p');
        price.textContent = `${el.price} ден`;
        price.className = 'm-0 p-2';

        //img element (image)
        let image = document.createElement('img');
        image.src = 'data:image/png;base64, ' + el.img;
        image.width = 48;

        div.appendChild(name);
        div.appendChild(price);
        div.appendChild(image);

        product_link.appendChild(div);
        new_node.appendChild(product_link);

        search_suggestions_box.appendChild(new_node);
    });


    /*if (max >= 1) {
        const new_node = document.createElement('li');
        new_node.className = 'list-group-item';

        const link_all = document.createElement('a');
        link_all.textContent = 'View All';
        link_all.href = location.protocol + '//' + location.host + '/Store/Base/Find' + url_param + query_string;
        new_node.appendChild(link_all);

        search_suggestions_box.appendChild(new_node);
    }*/

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

if (search_suggestions_box !== null) {
    document.addEventListener('click', (e) => {
        let li_element = search_suggestions_box.querySelector('li');
        if (!search_suggestions_box.contains(e.target) && li_element !== null) {
            clearSearchBox();
        }
    });
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
                url_param = '?SKU=';
                query = query_url + url_param + parseInt(query_string);
            }
            else {
                url_param = '?name=';
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

function compareArray(arr_one, arr_two) {
    return arr_one.length === arr_two.length && arr_one.every((value, index) => value === arr_two[index]);
}


/* CART */
// cart objects
let cart;
let cart_str;
let cart_obj;


//helper var / check if element exists
let cart_table_exists;
let order_table_exists;


class Product {
    constructor(id, name, price, quantity, img, variations) {
        this.id = parseInt(id);
        this.name = name;
        this.price = price.toString();
        this.quantity = parseInt(quantity);
        this.img = img;
        this.variations = variations;
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
                if (parseInt(this.items[i].id) === parseInt(item.id) && compareArray(this.items[i].variations, item.variations)) {
                    this.items[i].quantity += parseInt(item.quantity);
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

    remove(id, variations) {
        let temp = this.items;
        this.items = [];
        temp.forEach(item => {
            if (!(parseInt(id) === parseInt(item.id) && compareArray(item.variations, variations))) {
                this.items.push(item);
            }
        })
    }

}

class Order {
    constructor(first_name, last_name, company_name, country, address, zip_code, city, email, phone, payment_method, delivery_method, cart) {
        this.FirstName = first_name;
        this.LastName = last_name;
        this.CompanyName = company_name;
        this.Country = country;
        this.Address = address;
        this.ZipCode = zip_code;
        this.City = city;
        this.Email = email;
        this.Phone = phone;
        this.PaymentMethod = payment_method;
        this.DeliveryMethod = delivery_method;
        this.Cart = cart;
    }
}

function submitOrder(order) {
    let first_name = document.getElementById('first').value;
    let last_name = document.getElementById('last').value;
    let company_name = document.getElementById('co-name').value;
    let country = document.getElementById('country').value;
    let address = document.getElementById('address').value;
    let zip_code = document.getElementById('zip').value;
    let city = document.getElementById('city').value;
    let email = document.getElementById('email').value;
    let phone = document.getElementById('phone').value;

    let payment_method = null;
    let delivery_method = null;

    let all_delivery_methods = document.querySelectorAll('.delivery-method');
    let all_payment_methods = document.querySelectorAll('.payment-method');

    all_delivery_methods.forEach(el => {
        if (el.checked === true) {
            delivery_method = el.value;
        }
    });

    all_payment_methods.forEach(el => {
        if (el.checked === true) {
            payment_method = el.value;
        }
    });


    if (payment_method === null || delivery_method === null) {
        createAlert('Please fill out the checkbox', 'danger');
        return;
    }

    let cart_str = localStorage.getItem('cart');
    let cart = JSON.parse(cart_str);

    cart.items.forEach(el => {
        el.img = null;
    });



    let new_order = new Order(first_name, last_name, company_name, country, address, zip_code, city, email, phone, payment_method, delivery_method, cart);

    let url = location.protocol + '//' + location.host + '/Cart/Order/Process';
    console.log(new_order);
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json charset=utf-8'
        },
        redirect: 'follow',
        body: JSON.stringify(new_order)
    }).then(res => {
        if (res.redirected) {
            cart = new Cart([], null);
            localStorage.setItem('cart', JSON.stringify(cart));
            window.location.href = res.url;
        }
        else {
            createAlert('Your order has NOT been placed. Please check your billing details and try again.', 'danger');
        }
    })
        .catch((err) => {
            console.info(err);
        });

}

function checkUserLogin(callback) {
    let query = location.protocol + '//' + location.host + '/Auth/CheckLogin';
    return fetch(query, {
        method: 'GET', // POST, PUT, DELETE, etc.
        headers: {
            'Content-Type': 'text/plain;charset=UTF-8'
        },
        credentials: 'include', // omit, include
        redirect: 'follow', // manual, error
    }).then((response) => {
        if (response.ok || response.redirected) {
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
    let query = location.protocol + '//' + location.host + '/Cart/Base/Get';
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
                        updateCartTotalDOM();
                        callback(cart);
                    });
            }
        })

    return userCart;
}

function displayProducts(loggedStatus) {
    if (loggedStatus === true) {
        getUserCart(listCartItems);
        getUserCart(listCheckoutItems);
    }
    else {
        initCart();
        if (cart_table_exists !== null) {
            listCartItems(cart);
        }
        else if (checkout_table_exists !== null) {
            listCheckoutItems(cart);
        }
        updateCartTotalDOM();
    }
}

//init cart on window load
window.onload = (event) => {
    // cart objects
    cart = null;
    cart_str = localStorage.getItem('cart');
    cart_obj = JSON.parse(cart_str);

    //helper variable
    cart_table_exists = document.querySelector('.cart-table');
    checkout_table_exists = document.querySelector('.order-table');

    //if logged in, executes the callback (1st arg);
    checkUserLogin(displayProducts)


    //get carousels
    let sliders = document.querySelectorAll('.product-slider');
    if (sliders[0] !== null) {
        for (let item of sliders) {
            GetCarousel(item, getCarouselDefaultValue(item.parentElement))
        }
    }
}

function getCarouselDefaultValue(parent_element) {
    let parent = parent_element.querySelector('.filter-control');
    let value = parent.firstElementChild.firstElementChild.dataset.value;

    return value;
}

function getCartQuantity() {
    let cart_str = localStorage.getItem('cart');
    let cart = JSON.parse(cart_str);
    let quantity = 0;

    if (cart.items !== null) {
        cart.items.forEach(el => {
            quantity += 1;
        });
    }

    return quantity;
}

function getCartTotal() {
    let cart_str = localStorage.getItem('cart');
    let cart = JSON.parse(cart_str);
    let total = 0;

    if (cart.items !== null) {
        cart.items.forEach(el => {
            let item_price = parseInt(el.price) * parseInt(el.quantity);
            total += item_price;
        });
    }

    return total;


}
//update cart total on page, total on hover, quantity on hover'
function updateCartTotalDOM() {
    let total = getCartTotal();

    let total_element = document.querySelector('.cart-total');
    if (total_element !== null) {
        total_element.innerHTML = `Total <span>${total} ден</span>`;
    }

    let cart_quantity_hover = document.getElementById('cart-quantity-hover');

    if (cart_quantity_hover !== null) {
        cart_quantity_hover.textContent = getCartQuantity();
        cart_quantity_hover.style.opacity = '1';
    }
    else {

    }
}


//use existing or create new cart
function initCart() {
    if (cart_str !== null) {
        cart = new Cart(cart_obj.items, cart_obj.userId);
    }
    else {
        cart = new Cart([], null);
        localStorage.setItem('cart', JSON.stringify(cart));
    }
}

function checkCart() {
    if (cart.items.length > 0) {
        return true;
    }
    return false;
}

function closeAlert() {
    $('.alert').alert('close');
}

/* SINGLE PRODUCT ADD TO CART*/


if (add_to_cart_btn !== null) {

    add_to_cart_btn.addEventListener('click', (e) => {
        console.log('clicked');
        let item_id = document.getElementById('item-id').textContent;
        let item_name = document.getElementById('item-name').textContent;
        let item_price = document.getElementById('item-price').textContent
        let item_quantity = document.getElementById('item-quantity').value;
        let item_image = document.getElementById('item-image').src;


        var all_variations = document.querySelectorAll('.variation-select');
        let item_variations = [];


        all_variations.forEach(el => {
            item_variations.push(el.value);
        });

        let regex = /\d{1,}/gm;
        let price_str = regex.exec(item_price);

        let product = new Product(item_id, item_name, parseInt(price_str[0]), parseInt(item_quantity), item_image, item_variations);

        //get current cart from localstorage
        let cart_str = localStorage.getItem('cart');
        let current_cart = JSON.parse(cart_str);

        //set cart.items to current items
        let cart = new Cart();
        cart.items = current_cart.items;



        cart.add(product);
        console.log(cart);

        localStorage.setItem('cart', JSON.stringify(cart));
        //update total and quantity top bar + total on page if on cart
        updateCartTotalDOM();
        checkUserLogin(addCartToDb);

        //create alert product added
        createAlert('Item added to cart.');

    });

}
function removeFromCart(id, variations) {
    if (cart.items !== null) {

        cart.remove(id, variations);

        localStorage.setItem('cart', JSON.stringify(cart));
        checkUserLogin(addCartToDb);
    }
}


//update client cart then check login and update back-end if logged in
function updateCartQuantity(id, quantity, variations) {
    if (cart.items !== null) {
        cart.items.forEach(item => {
            if (parseInt(item.id) === parseInt(id) && compareArray(item.variations, variations)) {
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
            let p = new Product(i.id, i.name, i.price, parseInt(i.quantity), null, i.variations);
            array_products.push(p);
        });

        let cart_to_server = new Cart(array_products, null);

        let query = location.protocol + '//' + location.host + '/Cart/Base/Update';
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
                /*console.log('CartDb updated');*/
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
            /*let th = document.createElement('th');
            count += 1;
            th.inn
            th.scope = 'row';*/

            let td_1 = document.createElement('td');
            td_1.textContent = item.id
            td_1.style.display = 'none';

            let td_2 = document.createElement('td');
            td_2.innerHTML = `<img src="${item.img}" alt="" style="max-width: 48px;">`;
            td_2.className = 'cart-pic';

            let td_3 = document.createElement('td');
            td_3.innerHTML = `<h5>${item.name}</h5>`;
            td_3.className = 'cart-title';


            let td_4 = document.createElement('td');
            td_4.textContent = `${item.price} ден`;
            td_4.className = 'p-price';

            let td_5 = document.createElement('td');
            let current_quantity = item.quantity;
            td_5.innerHTML = `<span class='quantity-text'>${current_quantity}</span> <a href='#' class='cart-edit-quantity'>Edit</a>`;

            let td_6 = document.createElement('td');
            for (let i = 0; i < item.variations.length; i++) {
                if (i === item.variations.length - 1) {
                    td_6.innerHTML += `<span class="cart-variation">${item.variations[i]}</span>`;
                }
                else {
                    td_6.innerHTML += `<span class="cart-variation">${item.variations[i]},&nbsp</span>`;
                }
            }


            let td_7 = document.createElement('td');
            let price_total = parseInt(item.price) * parseInt(item.quantity);
            td_7.textContent = `${price_total} ден`;
            td_7.className = 'total-price';


            let td_8 = document.createElement('td');
            td_8.innerHTML = ` <i class="ti-close cart-remove-item"></i>`;
            td_8.className = 'close-td';


            /*
                        let td_7 = document.createElement('td');
                        let img = document.createElement('img');
                        img.width = 48;
                        img.src = 'data:image/jpeg;base64, ' + item.img;
                        td_7.appendChild(img);*/



            tr.appendChild(td_1);
            tr.appendChild(td_2);
            tr.appendChild(td_3);
            tr.appendChild(td_4);
            tr.appendChild(td_5);
            tr.appendChild(td_6);
            tr.appendChild(td_7);
            tr.appendChild(td_8);

            table_body.appendChild(tr);





        });
        let cart_edit_nodes = document.querySelectorAll('.cart-edit-quantity');
        cart_edit_nodes.forEach(i => {
            i.addEventListener('click', (e) => {
                openInputBox(e);
            });
        });
        //update total after listing
        updateCartTotalDOM();

        let cart_remove_nodes = document.querySelectorAll('.cart-remove-item');
        cart_remove_nodes.forEach(el => {
            el.addEventListener('click', (e) => {
                console.log('removed');
                let parent_container = e.target.parentNode.parentNode;
                let item_id = parent_container.querySelector('td').textContent;

                let item_variation_el = parent_container.querySelectorAll('.cart-variation');
                let item_variations = [];

                item_variation_el.forEach(el => {
                    let regex = /[\wа-я]+/ig;
                    let variation_string = regex.exec(el.textContent);

                    item_variations.push(variation_string[0]);
                });

                removeFromCart(item_id, item_variations);
                //update dom if exists
                updateCartTotalDOM();

                parent_container.remove();
            });
        });
    }
}

function listCheckoutItems(cart) {
    if (checkCart() && checkout_table_exists !== null) {
        let table = checkout_table_exists;

        let total = 0;
        cart.items.forEach(item => {

            //table heading
            /*let th = document.createElement('th');
            count += 1;
            th.inn
            th.scope = 'row';*/

            let li_1 = document.createElement('li');
            li_1.className = 'fw-normal';

            li_1.innerHTML += `${item.name}:&nbsp`;
            item.variations.forEach(v => {
                li_1.innerHTML += `${v}&nbsp`;
            });
            let product_total = parseInt(item.quantity) * parseInt(item.price);
            li_1.innerHTML += `x ${item.quantity}&nbsp <span>${product_total} ден</span>`;

            total += product_total;

            table.appendChild(li_1);
        });

        let li_total = document.createElement('li');
        li_total.className = 'total-price';
        li_total.innerHTML = `Total <span>${total} ден</span>`;

        table.appendChild(li_total);


    }
}





function openInputBox(event) {
    let parent = event.target.parentNode;

    let current_quantity = parent.querySelector('.quantity-text').textContent;

    parent.innerHTML = `<input type="number" class="quantity-input" name="quantity" min="1" value=${current_quantity} style="width: 80px;"> <a href='#' class='cart-update-quantity'>Update</a>`;

    //input box element
    let input_value_el = parent.querySelector('.quantity-input');

    //update button (a element)
    let cart_update_quantity = document.querySelector('.cart-update-quantity');

    cart_update_quantity.addEventListener('click', (e) => {
        //get update btn parent-parent
        let parent_container = e.target.parentNode.parentNode;

        //parent node (td)
        let parent = e.target.parentNode;

        //update current html quantity from input box value
        current_quantity = input_value_el.value;
        parent.innerHTML = `<span class='quantity-text'>${current_quantity}</span> <a href='#' class='cart-edit-quantity'>Edit</a>`;
        let quantity_text = parent.querySelector('.quantity-text');
        quantity_text.textContent = current_quantity;


        //change product total field
        let price = parent_container.querySelector('.p-price').textContent;
        let quantity = current_quantity;

        let total_field = parent_container.querySelector('.total-price');
        total_field.textContent = parseInt(quantity) * parseInt(price);


        //update js cart quantity
        let td_id = parent.parentNode.querySelector('td');
        let item_id = td_id.textContent;

        let item_variation_el = parent.parentNode.querySelectorAll('.cart-variation');
        let item_variations = [];

        item_variation_el.forEach(el => {
            let regex = /[\wа-я]+/ig;
            /*let regex = /\w{1,}/gm;*/
            let variation_string = regex.exec(el.textContent);
            item_variations.push(variation_string[0]);
        });


        updateCartQuantity(item_id, current_quantity, item_variations);
        //update for total on cart page
        updateCartTotalDOM();

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
/*let gallery_images = document.querySelectorAll('.gallery-image');

if (gallery_images != null) {
    gallery_images.forEach(img => {
        console.log('clicked');
        img.addEventListener('click', (e) => {
            changeProductImage(e.target.src);
        });
    })
}*/

function changeProductImage(value) {
    let product_image = document.getElementById('product-image');
    product_image.src = value;
}

var prev_checkbox_payment = null;
var prev_checkbox_delivery = null;

if (payment_checkmarks !== null) {
    payment_checkmarks.forEach(el => {
        el.addEventListener('click', (e) => {
            if (prev_checkbox_payment === null) {
                prev_checkbox_payment = e.target;
            }
            else if (prev_checkbox_payment !== e.target) {
                prev_checkbox_payment.checked = !e.target.checked;
                prev_checkbox_payment.dispatchEvent(new Event('change'));
                prev_checkbox_payment = e.target;
                /*prev_checkbox.click();*/
            }
        });
    });
}

if (delivery_checkmarks !== null) {
    delivery_checkmarks.forEach(el => {
        el.addEventListener('click', (e) => {
            if (prev_checkbox_delivery === null) {
                prev_checkbox_delivery = e.target;
            }
            else if (prev_checkbox_delivery !== e.target) {
                prev_checkbox_delivery.checked = !e.target.checked;
                prev_checkbox_delivery.dispatchEvent(new Event('change'));
                prev_checkbox_delivery = e.target;
                /*prev_checkbox.click();*/
            }
        });
    });
}
if (order_submit_btn !== null) {
    order_submit_btn.addEventListener('click', (e) => {
        submitOrder(e);
    });
}

let page_btns = document.querySelectorAll('.page-btn');

if (page_btns !== null) {
    page_btns.forEach(b => {
        b.addEventListener('click', e => {
            document.getElementById('current-page-input').value = e.target.dataset.page;
            document.getElementById('filter-form').submit();
        });
    });
}

let category_btns = document.querySelectorAll('.category-btn');
if (category_btns) {
    category_btns.forEach(b => {
        b.addEventListener('click', e => {
            document.getElementById('current-category-input').value = e.target.dataset.id;
            let activeCategory = document.getElementById('active-category');

            if (activeCategory) {
                activeCategory.id = "";
            }
            e.target.id = "active-category";
        });
    });
}

let search_btn = document.getElementById('submit-search-btn');
let search_field = document.getElementById('store-search');
let filter_btn = document.getElementById('filter-btn');

if (filter_btn) {
    search_field.setAttribute('form', 'filter-form');
    search_btn.setAttribute('form', 'filter-form');

    search_btn.addEventListener('click', e => {
        document.getElementById('filter-form').submit();
    });
}


/* home page carousels*/
function GetCarousel(parent_element, category, clicked_element = null) {
    let url = location.protocol + '//' + location.host + '/Store/Query/GetCarousel?category=' + category;

    fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'text/plain;charset=UTF-8'
        },
        credentials: 'include',
        redirect: 'follow',
    }).then(res => {
        if (res.ok) {
            res.json().then(result => {
                if (clicked_element !== null) {
                    toggleActiveTab(clicked_element);
                }
                populateCarousel(parent_element, result);
            });
        }
        else {
            console.log(res.status);
        }
    });
}

let carousel_control_items = document.querySelectorAll('.control-item');
if (carousel_control_items[0] !== null) {
    carousel_control_items.forEach(el => {
        el.addEventListener('click', (e) => {
            let parent = e.target.parentNode.parentNode.parentNode;
            let product_slider = parent.querySelector('.owl-carousel');



            GetCarousel(product_slider, e.target.dataset.value, e.target);
        });
    })
}
// 2/3 width carousel
function populateCarousel(parent_element, data) {
    let isFullWidth = false;
    if (parent_element.classList.contains('full-width-carousel')) {
        isFullWidth = true;
        $(parent_element).owlCarousel('destroy');
        parent_element.className = 'product-slider owl-carousel full-width-carousel';
    }
    else {
        $(parent_element).owlCarousel('destroy');
        parent_element.className = 'product-slider owl-carousel';

    }




    while (parent_element.firstChild) {
        parent_element.removeChild(parent_element.lastChild)
    }


    for (let item of data) {
        let product_item = document.createElement('div');
        product_item.className = 'product-item';

        if (item.onSale === true) {
            product_item.innerHTML =
                `
                    <div class="pi-pic">
                        <a href="/Store/Product/Single/${item.id}"><img src="data:image/png;base64, ${item.img}" alt=""></a>
                        <div class="sale">Sale</div>
                        <div class="icon">
                            <i class="icon_heart_alt"></i>
                        </div>
                        <ul>
                            <li class="quick-view"><a href="/Store/Product/Single/${item.id}">Види <i class="fas fa-arrow-right"></i></a></li>
                        </ul>
                    </div>
                    <div class="pi-text">
                        <div class="catagory-name">${item.category}</div>
                        <a href="/Store/Product/Single/${item.id}">
                            <h5>${item.name}</h5>
                        </a>
                        <div class="product-price">
                            ${item.price} ден
                            <span>${item.salePrice} ден</span>
                        </div>
                    </div>
                `;
            parent_element.appendChild(product_item);
        }
        else {
            product_item.innerHTML =
                `
                    <div class="pi-pic">
                        <a href="/Store/Product/Single/${item.id}"><img src="data:image/png;base64, ${item.img}" alt=""></a>
                        <div class="icon">
                            <i class="icon_heart_alt"></i>
                        </div>
                        <ul>
                            <li class="quick-view"><a href="/Store/Product/Single/${item.id}">Види <i class="fas fa-arrow-right"></i></a></li>
                        </ul>
                    </div>
                    <div class="pi-text">
                        <div class="catagory-name">${item.category}</div>
                        <a href="/Store/Product/Single/${item.id}">
                            <h5>${item.name}</h5>
                        </a>
                        <div class="product-price">
                            ${item.price} ден
                        </div>
                    </div>
                `;
            parent_element.appendChild(product_item);
        }
    };


    if (isFullWidth) {
        $(parent_element).owlCarousel({
            loop: true,
            margin: 25,
            nav: true,
            items: 4,
            dots: true,
            navText: ['<i class="fas fa-chevron-left"></i>', '<i class="fas fa-chevron-right"></i>'],
            smartSpeed: 1200,
            autoHeight: false,
            autoplay: true,
            responsive: {
                0: {
                    items: 1,
                },
                576: {
                    items: 3,
                },
                992: {
                    items: 5,
                },
                1200: {
                    items: 6,
                }
            }
        });
    }
    else {
        $(parent_element).owlCarousel({
            loop: true,
            margin: 25,
            nav: true,
            items: 3,
            dots: true,
            navText: ['<i class="fas fa-chevron-left"></i>', '<i class="fas fa-chevron-right"></i>'],
            smartSpeed: 1200,
            autoHeight: false,
            autoplay: true,
            responsive: {
                0: {
                    items: 1,
                },
                576: {
                    items: 2,
                },
                992: {
                    items: 2,
                },
                1200: {
                    items: 3,
                }
            }
        });
    }



}

//toggle active tab slider
function toggleActiveTab(clicked_element) {

    let child_elements = clicked_element.parentNode.children;

    for (let item of child_elements) {
        item.className = 'control-item';
    }

    clicked_element.className = 'active control-item';
}