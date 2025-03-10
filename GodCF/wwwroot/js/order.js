document.addEventListener('DOMContentLoaded', function () {
    // Constants
    const SHIPPING_FEE = 15000;

    // DOM Elements
    const orderItems = document.querySelector('.order-items');
    const subtotalAmount = document.querySelector('.subtotal-amount');
    const totalAmount = document.querySelector('.total-amount');
    const orderForm = document.querySelector('.order-form');
    const paymentMethods = document.querySelectorAll('input[name="payment"]');
    const bankDetails = document.querySelector('.bank-details');

    // Get cart data from localStorage
    let cart = [];
    const cartData = localStorage.getItem('cart');

    if (cartData) {
        try {
            cart = JSON.parse(cartData);
            displayOrderItems();
        } catch (error) {
            console.error('Error parsing cart data:', error);
            showError('Đã xảy ra lỗi khi tải thông tin giỏ hàng');
        }
    } else {
        showError('Không tìm thấy thông tin giỏ hàng');
    }

    // Display order items
    function displayOrderItems() {
        if (cart.length === 0) {
            orderItems.innerHTML = '<p>Giỏ hàng trống</p>';
            return;
        }

        let subtotal = 0;
        orderItems.innerHTML = '';

        cart.forEach(item => {
            const itemTotal = item.price * item.quantity;
            subtotal += itemTotal;

            orderItems.innerHTML += `
                <div class="order-item">
                    <img src="${item.image}" alt="${item.name}">
                    <div class="order-item-info">
                        <div class="order-item-title">${item.name}</div>
                        <div class="order-item-price">${formatPrice(item.price)}</div>
                        <div class="order-item-quantity">Số lượng: ${item.quantity}</div>
                    </div>
                </div>
            `;
        });

        // Update totals
        subtotalAmount.textContent = formatPrice(subtotal);
        updateTotal(subtotal);
    }

    // Update total amount
    function updateTotal(subtotal) {
        const total = subtotal + SHIPPING_FEE;
        totalAmount.textContent = formatPrice(total);
    }

    // Format price
    function formatPrice(price) {
        return price.toLocaleString('vi-VN') + 'đ';
    }

    // Show error message
    function showError(message) {
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-message';
        errorDiv.style.cssText = `
            background-color: #fff3f3;
            color: #dc3545;
            padding: 1rem;
            border-radius: 5px;
            margin-bottom: 1rem;
            text-align: center;
        `;
        errorDiv.textContent = message;

        const container = document.querySelector('.order-container');
        container.insertBefore(errorDiv, container.firstChild);
    }

    // Handle payment method change
    paymentMethods.forEach(method => {
        method.addEventListener('change', function () {
            if (this.value === 'bank') {
                bankDetails.style.display = 'block';
            } else {
                bankDetails.style.display = 'none';
            }
        });
    });

    // Form submission
    if (orderForm) {
        orderForm.addEventListener('submit', function (e) {
            if (cart.length === 0) {
                e.preventDefault();
                showError('Không thể đặt hàng với giỏ hàng trống');
                return;
            }

            // Add order data to form
            const orderData = {
                items: cart,
                subtotal: convertPriceToNumber(subtotalAmount.textContent),
                shipping: SHIPPING_FEE,
                total: convertPriceToNumber(totalAmount.textContent),
                orderDate: new Date().toISOString()
            };

            // Add order data to form as hidden field
            const orderDataInput = document.createElement('input');
            orderDataInput.type = 'hidden';
            orderDataInput.name = 'orderData';
            orderDataInput.value = JSON.stringify(orderData);
            this.appendChild(orderDataInput);

            // Clear cart after successful submission
            localStorage.removeItem('cart');
        });
    }

    // Helper function to convert price string to number
    function convertPriceToNumber(priceStr) {
        return parseInt(priceStr.replace(/\D/g, ''));
    }

    // Form validation
    const phoneInput = document.getElementById('phone');
    if (phoneInput) {
        phoneInput.addEventListener('input', function () {
            const phonePattern = /^[0-9]{10}$/;
            if (!phonePattern.test(this.value)) {
                this.setCustomValidity('Vui lòng nhập số điện thoại hợp lệ (10 số)');
            } else {
                this.setCustomValidity('');
            }
        });
    }
});