// POS Cart Engine
// ────────────────────────────────────────────────────────────────────────────
let cart = [];
let selectedPaymentMethod = 0;
let selectedPaymentRef = '';

// Barcode scanner: fires on Enter key (USB scanner ends with \n)
document.getElementById('barcodeInput').addEventListener('keydown', e => {
    if (e.key === 'Enter') { e.preventDefault(); searchProduct(); }
});

// Order type toggle
document.querySelectorAll('input[name="orderType"]').forEach(r => {
    r.addEventListener('change', () => {
        const isRestaurant = r.value === '1';
        document.getElementById('tableSelect').classList.toggle('d-none', !isRestaurant);
        loadProductGrid(null);
    });
});

async function searchProduct() {
    const query = document.getElementById('barcodeInput').value.trim();
    if (!query) return;
    const res = await fetch(`/POS/GetProduct?query=${encodeURIComponent(query)}`);
    const data = await res.json();
    if (data.success) addToCart(data.product);
    else showToast(data.message, 'danger');
    document.getElementById('barcodeInput').value = '';
}

function handleProductCardClick(cardEl) {
    if (cardEl.classList.contains('product-card-disabled')) {
        showToast('This product is out of stock.', 'warning');
    }
    let product = cardEl.dataset.product;
    product = JSON.parse(product);
    console.log(product);
    addToCart(product)
    // Flash the card so user sees it was registered
    cardEl.classList.add('product-card--flash');
    setTimeout(() => cardEl.classList.remove('product-card--flash'), 400);
}
function addToCart(product) {
    const existing = cart.find(i => i.id === product.id);    
    if (existing) {
        if (existing.qty >= product.stock) {
            showToast('Insufficient stock', 'warning'); return;
        }
        existing.qty++;
    } else {
        cart.push({ ...product, qty: 1 });
    }
    renderCart();
}

function updateQty(productId, delta) {
    const item = cart.find(i => i.id === productId);
    if (!item) return;
    item.qty += delta;
    if (item.qty <= 0) removeFromCart(productId);
    else renderCart();
}

function removeFromCart(productId) {
    cart = cart.filter(i => i.id !== productId);
    renderCart();
}

function clearCart() {
    cart = [];
    renderCart();
}

function renderCart() {
    const container = document.getElementById('cartItems');
    const emptyMsg = document.getElementById('emptyCartMsg');

    if (cart.length === 0) {
        container.innerHTML = '';
        emptyMsg.style.display = 'block';
        document.getElementById('checkoutBtn').disabled = true;
        updateTotals(0, 0, 0);
        return;
    }

    //emptyMsg.style.display = 'none';
    document.getElementById('checkoutBtn').disabled = false;

    container.innerHTML = cart.map(item => `
        <div class="card mb-2 shadow-sm border-0">
        <div class="card-body p-2">
        <div class="d-flex justify-content-between align-items-start">
        <div class="flex-grow-1">
        <div class="fw-semibold small">${item.name}</div>
        <div class="text-muted x-small">৳${item.price.toFixed(2)} × ${item.qty}</div>
        </div>
        <div class="d-flex align-items-center gap-1">
        <button class="btn btn-sm btn-outline-secondary px-2 py-0"
        onclick="updateQty(${item.id},-1)">−</button>
        <span class="fw-bold px-1">${item.qty}</span>
        <button class="btn btn-sm btn-outline-secondary px-2 py-0"
        onclick="updateQty(${item.id},1)">+</button>
        <button class="btn btn-sm btn-outline-danger px-2 py-0 ms-1"
        onclick="removeFromCart(${item.id})">×</button>
        </div>
        </div>
        <div class="text-end fw-bold text-success small mt-1">
        ৳${(item.price * item.qty).toFixed(2)}
        </div>
        </div>
        </div>`).join('');

    recalculate();
}

function recalculate() {
    const subTotal = cart.reduce((s, i) => s + i.price * i.qty, 0);
    const tax = cart.reduce((s, i) => s + i.price * i.qty * (i.taxRate || 0), 0);
    const discountType = parseInt(document.getElementById('discountType').value);
    const discountVal = parseFloat(document.getElementById('discountValue').value) || 0;

    let discount = 0;
    if (discountType === 1) discount = Math.min(discountVal, subTotal);
    else if (discountType === 2) discount = subTotal * discountVal / 100;

    updateTotals(subTotal, tax, discount);
}

function updateTotals(sub, tax, discount) {
    const total = sub + tax - discount;
    document.getElementById('cartSubtotal').textContent = '৳' + sub.toFixed(2);
    document.getElementById('cartTax').textContent = '৳' + tax.toFixed(2);
    document.getElementById('discountDisplay').textContent = '-৳' + discount.toFixed(2);
    document.getElementById('cartTotal').textContent = '৳' + total.toFixed(2);
    document.getElementById('checkoutTotal').textContent = '৳' + total.toFixed(2);
}

function selectPayment(method, btn) {
    selectedPaymentMethod = method;
    document.querySelectorAll('.payment-btn').forEach(b => b.classList.remove('active'));
    btn.classList.add('active');
    const showRef = method === 1 || method === 2;
    document.getElementById('paymentRefRow').style.setProperty('display',
        showRef ? 'flex' : 'none', 'important');
}

async function checkout() {
    /*if (cart.length === 0) return;

    const orderType = parseInt(document.querySelector('input[name="orderType"]:checked').value);
    const tableId = orderType === 1
        ? (document.getElementById('tableSelect').value || null)
        : null;

    const discountType = parseInt(document.getElementById('discountType').value);
    const discountValue = parseFloat(document.getElementById('discountValue').value) || 0;

    const payload = {
        orderType: orderType,
        tableId: tableId ? parseInt(tableId) : null,
        customerName: document.getElementById('customerName').value,
        customerPhone: document.getElementById('customerPhone').value,
        discountType,
        discountValue,
        items: cart.map(i => ({ productId: i.id, quantity: i.qty, notes: '' })),
        payments: [{
            method: selectedPaymentMethod,
            amount: parseFloat(document.getElementById('cartTotal').textContent.replace('৳', '')),
            reference: document.getElementById('paymentRef')?.value || ''
        }]
    };*/    
    // ── Validate cart is not empty ────────────────────────────────
    if (cart.length === 0) {
        showToast('Cart is empty.', 'warning');
        return;
    }

    // ── Read order type ───────────────────────────────────────────
    const orderTypeInput = document.querySelector(
        'input[name="orderType"]:checked');
    const orderType = parseInt(orderTypeInput?.value ?? '0');

    // ── Read table for restaurant orders ──────────────────────────
    const tableSelect = document.getElementById('tableSelect');
    const tableId = (orderType === 1 && tableSelect?.value)
        ? parseInt(tableSelect.value)
        : null;

    if (orderType === 1 && !tableId) {
        showToast('Please select a table for restaurant order.',
            'warning');
        return;
    }

    // ── Read discount ─────────────────────────────────────────────
    const discountType = parseInt(
        document.getElementById('discountType').value ?? '0');
    const discountValue = parseFloat(
        document.getElementById('discountValue').value ?? '0') || 0;

    // ── Calculate total ───────────────────────────────────────────
    const totalText = document.getElementById('cartTotal')
        .textContent.replace('৳', '').trim();
    const total = parseFloat(totalText) || 0;

    if (total <= 0) {
        showToast('Order total must be greater than zero.',
            'warning');
        return;
    }

    // ── Read payment reference ────────────────────────────────────
    const paymentRef = document
        .getElementById('paymentRef')?.value?.trim() ?? '';

    // ── Build payload — match CreateOrderDto exactly ──────────────
    const payload = {
        orderType: orderType,
        tableId: tableId,
        customerName: document.getElementById('customerName').value.trim() || null,
        customerPhone: document.getElementById('customerPhone').value.trim() || null,
        discountType: discountType,
        discountValue: discountValue,
        notes: null,
        items: cart.map(i => ({
            productId: i.id,
            quantity: i.qty,
            notes: null,
        })),
        payments: [{
            method: selectedPaymentMethod,
            amount: total,
            reference: paymentRef || null,
        }],
    };


    // ── Log payload for debugging ─────────────────────────────────
    console.log('Checkout payload:', JSON.stringify(payload, null, 2));

    // ── Get CSRF token ────────────────────────────────────────────
    const tokenInput = document.querySelector(
        'input[name="__RequestVerificationToken"]');

    if (!tokenInput) {
        showToast('Security token missing. Refresh the page.',
            'danger');
        console.error('CSRF token input not found');
        return;
    }


    const btn = document.getElementById('checkoutBtn');
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm"></span> Processing...';

    try {
        const token = document.querySelector(
            'input[name="__RequestVerificationToken"]').value;
        const response = await fetch('/POS/Checkout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(payload)
        });
        const rawText = await response.text();
        console.log('Response status:', response.status);
        console.log('Response body:', rawText);
        //const data = await res.json();
        const data = JSON.parse(rawText);
        if (data.success) {
            showToast(`✓ Order ${data.invoiceNumber} completed!`, 'success');
            clearCart();
            window.open(`/POS/Invoice/${data.orderId}`, '_blank');
        } else {
            showToast(data.message, 'danger');
        }
    } finally {
        btn.disabled = false;
        btn.innerHTML = '<i class="bi bi-check-circle"></i> Checkout';
    }
}

function showToast(msg, type = 'info') {
    const t = document.createElement('div');
    t.className = `toast align-items-center text-bg-${type} border-0 show position-fixed top-0 end-0 m-3`;
    t.style.zIndex = 9999;
    t.innerHTML = `<div class="d-flex"><div class="toast-body">${msg}</div>
        <button class="btn-close btn-close-white me-2 m-auto" onclick="this.parentElement.parentElement.remove()"></button></div>`;
    document.body.appendChild(t);
    setTimeout(() => t.remove(), 3500);
}

// Load product grid
async function loadProductGrid(categoryId) {
    const url = `/POS/ProductGrid?categoryId=${categoryId ?? ''}&branchId=${window.branchId}`;
    const res = await fetch(url);
    document.getElementById('productCards').innerHTML = await res.text();
}

function filterCategory(catId, btn) {
    document.querySelectorAll('#categoryFilter button').forEach(b => {
        b.classList.remove('active', 'btn-primary');
        b.classList.add('btn-outline-secondary');
    });
    btn.classList.add('active', 'btn-primary');
    btn.classList.remove('btn-outline-secondary');
    loadProductGrid(catId);
}

// Init
loadProductGrid(null);