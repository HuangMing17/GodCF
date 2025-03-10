document.addEventListener('DOMContentLoaded', function () {
    // DOM Elements
    const bookingDate = document.getElementById('bookingDate');
    const bookingTime = document.getElementById('bookingTime');
    const tables = document.querySelectorAll('.table');
    const selectedTableNumber = document.getElementById('selectedTableNumber');
    const selectedDateTime = document.getElementById('selectedDateTime');
    const reservationForm = document.querySelector('.booking-form form');

    // Set minimum date to today
    const today = new Date();
    const maxDate = new Date();
    maxDate.setDate(today.getDate() + 14); // Allow booking up to 14 days in advance

    bookingDate.min = today.toISOString().split('T')[0];
    bookingDate.max = maxDate.toISOString().split('T')[0];
    bookingDate.value = today.toISOString().split('T')[0];

    // Load occupied tables from localStorage or initialize with mock data
    let occupiedTables = JSON.parse(localStorage.getItem('occupiedTables')) || {
        [getDateTimeKey(today)]: [2, 5] // Example: Tables 2 and 5 are occupied now
    };

    // Table selection
    let selectedTable = null;

    tables.forEach(table => {
        table.addEventListener('click', function () {
            const tableNumber = this.dataset.table;
            const dateTimeKey = getDateTimeKey(new Date(bookingDate.value + ' ' + bookingTime.value));

            // Check if table is occupied
            if (isTableOccupied(tableNumber, dateTimeKey)) {
                alert('Bàn này đã được đặt trong khung giờ đã chọn');
                return;
            }

            // Remove selection from previously selected table
            if (selectedTable) {
                selectedTable.classList.remove('selected');
            }

            // Select new table
            this.classList.add('selected');
            selectedTable = this;
            selectedTableNumber.textContent = `Bàn ${tableNumber}`;
            updateSelectedDateTime();
        });
    });

    // Update table availability when date/time changes
    function updateTableAvailability() {
        if (!bookingTime.value) {
            return;
        }

        const dateTimeKey = getDateTimeKey(new Date(bookingDate.value + ' ' + bookingTime.value));

        tables.forEach(table => {
            const tableNumber = parseInt(table.dataset.table);

            // Reset classes
            table.classList.remove('occupied', 'available');

            // Update status
            if (isTableOccupied(tableNumber, dateTimeKey)) {
                table.classList.add('occupied');
                // Deselect if this was the selected table
                if (selectedTable === table) {
                    selectedTable.classList.remove('selected');
                    selectedTable = null;
                    selectedTableNumber.textContent = 'Chưa chọn';
                }
            } else {
                table.classList.add('available');
            }
        });

        updateSelectedDateTime();
    }

    // Date/Time change handlers
    bookingDate.addEventListener('change', updateTableAvailability);
    bookingTime.addEventListener('change', updateTableAvailability);

    // Update selected date/time display
    function updateSelectedDateTime() {
        if (bookingDate.value && bookingTime.value) {
            const date = new Date(bookingDate.value + ' ' + bookingTime.value);
            selectedDateTime.textContent = formatDateTime(date);
        } else {
            selectedDateTime.textContent = 'Chưa chọn';
        }
    }

    // Helper function to check if table is occupied
    function isTableOccupied(tableNumber, dateTimeKey) {
        return occupiedTables[dateTimeKey] &&
            occupiedTables[dateTimeKey].includes(parseInt(tableNumber));
    }

    // Helper function to get datetime key
    function getDateTimeKey(date) {
        return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}-${String(date.getHours()).padStart(2, '0')}`;
    }

    // Helper function to format date and time
    function formatDateTime(date) {
        return new Intl.DateTimeFormat('vi-VN', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: 'numeric',
            minute: 'numeric'
        }).format(date);
    }

    // Form submission
    if (reservationForm) {
        reservationForm.addEventListener('submit', function (e) {
            if (!selectedTable) {
                e.preventDefault();
                alert('Vui lòng chọn bàn trước khi đặt');
                return;
            }

            if (!bookingTime.value) {
                e.preventDefault();
                alert('Vui lòng chọn giờ đặt bàn');
                return;
            }

            // Add reservation data to form
            const reservationData = {
                table: selectedTable.dataset.table,
                date: bookingDate.value,
                time: bookingTime.value,
                datetime: formatDateTime(new Date(bookingDate.value + ' ' + bookingTime.value))
            };

            // Add reservation data to form as hidden field
            const reservationDataInput = document.createElement('input');
            reservationDataInput.type = 'hidden';
            reservationDataInput.name = 'reservationData';
            reservationDataInput.value = JSON.stringify(reservationData);
            this.appendChild(reservationDataInput);

            // Update occupied tables
            const dateTimeKey = getDateTimeKey(new Date(bookingDate.value + ' ' + bookingTime.value));
            if (!occupiedTables[dateTimeKey]) {
                occupiedTables[dateTimeKey] = [];
            }
            occupiedTables[dateTimeKey].push(parseInt(reservationData.table));

            // Save to localStorage
            localStorage.setItem('occupiedTables', JSON.stringify(occupiedTables));
        });
    }

    // Phone number validation
    const phoneInput = document.getElementById('customerPhone');
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

    // Clean up old occupied tables data (older than 14 days)
    function cleanupOldData() {
        const cutoffDate = new Date();
        cutoffDate.setDate(cutoffDate.getDate() - 14);

        for (const dateTimeKey in occupiedTables) {
            const [year, month, day] = dateTimeKey.split('-');
            const date = new Date(year, month - 1, day);
            if (date < cutoffDate) {
                delete occupiedTables[dateTimeKey];
            }
        }
        localStorage.setItem('occupiedTables', JSON.stringify(occupiedTables));
    }

    // Initial setup
    cleanupOldData();
    updateTableAvailability();
});