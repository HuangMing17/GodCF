document.addEventListener('DOMContentLoaded', function () {
    // Initialize cart state
    let cart = [];
    let isCartVisible = false;

    // DOM Elements
    const filterBtns = document.querySelectorAll('.filter-btn');
    const menuItems = document.querySelectorAll('.menu-item');
    const cartPreview = document.querySelector('.shopping-cart-preview');
    const cartItems = document.querySelector('.cart-items');
    const cartCount = document.querySelector('.cart-count');
    const totalAmount = document.querySelector('.total-amount');
    const addToCartBtns = document.querySelectorAll('.add-to-cart');

    // Filter menu items
    filterBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            // Remove active class from all buttons
            filterBtns.forEach(b => b.classList.remove('active'));
            // Add active class to clicked button
            btn.classList.add('active');

            const category = btn.dataset.category;

            menuItems.forEach(item => {
                if (category === 'all' || item.dataset.category === category) {
                    item.style.display = 'block';
                } else {
                    item.style.display = 'none';
                }
            });
        });
    });

    // Toggle cart visibility
    function toggleCart() {
        isCartVisible = !isCartVisible;
        cartPreview.classList.toggle('active');
    }

    // Add click event for all add to cart buttons
    addToCartBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            const menuItem = btn.closest('.menu-item');
            const itemId = btn.dataset.id;
            const itemName = menuItem.querySelector('h3').textContent;
            const itemPrice = menuItem.querySelector('.price').textContent;
            const itemImage = menuItem.querySelector('img').src;

            addToCart({
                id: itemId,
                name: itemName,
                price: convertPriceToNumber(itemPrice),
                image: itemImage,
                quantity: 1
            });

            // Show cart preview
            if (!isCartVisible) {
                toggleCart();
            }
        });
    });

    // Add item to cart
    function addToCart(item) {
        const existingItem = cart.find(i => i.id === item.id);

        if (existingItem) {
            existingItem.quantity += 1;
        } else {
            cart.push(item);
        }

        updateCartDisplay();
    }

    // Update cart quantity
    function updateItemQuantity(itemId, change) {
        const item = cart.find(i => i.id === itemId);
        if (item) {
            item.quantity += change;
            if (item.quantity <= 0) {
                cart = cart.filter(i => i.id !== itemId);
            }
            updateCartDisplay();
        }
    }

    // Update cart display
    function updateCartDisplay() {
        cartItems.innerHTML = '';
        let total = 0;

        cart.forEach(item => {
            total += item.price * item.quantity;
            const itemElement = document.createElement('div');
            itemElement.className = 'cart-item';
            itemElement.innerHTML = `
                <img src="${item.image}" alt="${item.name}">
                <div class="cart-item-info">
                    <div class="cart-item-title">${item.name}</div>
                    <div class="cart-item-price">${formatPrice(item.price)}</div>
                    <div class="cart-item-quantity">
                        <button class="quantity-btn minus" data-id="${item.id}">-</button>
                        <span>${item.quantity}</span>
                        <button class="quantity-btn plus" data-id="${item.id}">+</button>
                    </div>
                </div>
            `;

            // Add event listeners to quantity buttons
            const minusBtn = itemElement.querySelector('.minus');
            const plusBtn = itemElement.querySelector('.plus');

            minusBtn.addEventListener('click', () => updateItemQuantity(item.id, -1));
            plusBtn.addEventListener('click', () => updateItemQuantity(item.id, 1));

            cartItems.appendChild(itemElement);
        });

        cartCount.textContent = cart.reduce((acc, item) => acc + item.quantity, 0);
        totalAmount.textContent = formatPrice(total);

        // Save cart to localStorage
        localStorage.setItem('cart', JSON.stringify(cart));
    }

    // Helper function to convert price string to number
    function convertPriceToNumber(priceStr) {
        return parseInt(priceStr.replace(/\D/g, ''));
    }

    // Helper function to format price
    function formatPrice(price) {
        return price.toLocaleString('vi-VN') + 'đ';
    }

    // Checkout button click handler
    document.querySelector('.checkout-btn').addEventListener('click', () => {
        if (cart.length === 0) {
            alert('Giỏ hàng của bạn đang trống!');
            return;
        }
        // Redirect to order page
        window.location.href = '/Home/Order';
    });

    // Close cart when clicking outside
    document.addEventListener('click', (e) => {
        if (isCartVisible &&
            !cartPreview.contains(e.target) &&
            !e.target.closest('.add-to-cart')) {
            toggleCart();
        }
    });

    // Load cart from localStorage if exists
    const savedCart = localStorage.getItem('cart');
    if (savedCart) {
        cart = JSON.parse(savedCart);
        updateCartDisplay();
    }
});