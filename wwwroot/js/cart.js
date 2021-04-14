class Product {
    constructor(id, name, price, quantity, img) {
        this.id = id,
        this.name = name;
        this.price = price;
        this.quantity = quantity;
        this.img = img
    }

    set quantity(value) {
        if (parseInt(value) < 1) {
            throw Error('Quantity < 1, error');
        }
        this.quantity = value;
    }



    update(value) {
        if (parseInt(value) < 1) {
            alert('Quantity cannot be less than 1.');
            return;
        }
        this.quantity = value;
    }
}

class Cart {
    constructor(items, userId) {
        this.items = items;
        this.userId = userId;
    }

    get userId() {
        return this.userId
    }

    set userId(id) {
        this.userId = id;
    }

    get items() {
        return this.items;
    }

    set items(updated_items) {
        this.items = updated_items;
    }

    remove(id) {
        let temp = this.items;
        this.items = [];
        this.temp.foreach(item => {
            if (item.id === id) {
                continue;
            }
            else {
                this.items.push(item);
            }
        })
    }

}

let cart = window.localStorage.getItem('cart');

if (cart !== null) {
    return;
}
else {
    cart = new Cart([], null);
}