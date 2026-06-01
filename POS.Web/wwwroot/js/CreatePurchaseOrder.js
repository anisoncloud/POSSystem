// ════════════════════════════════════════════════════════════════════
// PURCHASE ORDER ITEMS ENGINE
// ════════════════════════════════════════════════════════════════════

// All available products from ViewBag
const allProducts = @Html.Raw(System.Text.Json.JsonSerializer.Serialize(
    products.Select(p => new
        {
            id = p.Id,
            name = p.Name,
            sku = p.SKU,
            purchasePrice = p.PurchasePrice,
            stock = p.StockQuantity,
            unit = p.UnitType.ToString(),
        }),
    new System.Text.Json.JsonSerializerOptions
                    {
        PropertyNamingPolicy =
        System.Text.Json.JsonNamingPolicy.CamelCase
    }));

let rowIndex = 0;

// ── Add a new item row ─────────────────────────────────────────────
function addItemRow() {
    const tbody = document.getElementById('itemsBody');
    const emptyRow = document.getElementById('emptyRow');

    // Hide empty state row
    emptyRow.style.display = 'none';

    const idx = rowIndex++;
    const tr = document.createElement('tr');
    tr.id = `item-row-${idx}`;
    tr.dataset.index = idx;

    tr.innerHTML = `
                <!-- Hidden inputs submitted with form -->
                <input type="hidden"
                       name="Items[${idx}].ProductId"
                       id="productId_${idx}"
                       value="">

                <!-- Product selector -->
                <td class="ps-3">
                    <select class="form-select form-select-sm"
                            id="productSelect_${idx}"
                            onchange="onProductSelected(${idx})"
                            required>
                        <option value="">-- Select Product --</option>
                        ${allProducts.map(p => `
                            <option value="${p.id}"
                                    data-price="${p.purchasePrice}"
                                    data-unit="${p.unit}"
                                    data-stock="${p.stock}">
                                ${p.name}
                                (${p.sku})
                                — Stock: ${p.stock}
                            </option>`).join('')}
                    </select>
                    <div class="mt-1" id="productMeta_${idx}"></div>
                </td>

                <!-- Quantity -->
                <td>
                    <input type="number"
                           class="form-control form-control-sm"
                           name="Items[${idx}].Quantity"
                           id="qty_${idx}"
                           value="1"
                           min="1"
                           onchange="recalculateRow(${idx})"
                           oninput="recalculateRow(${idx})"
                           required>
                </td>

                <!-- Unit cost -->
                <td>
                    <div class="input-group input-group-sm">
                        <span class="input-group-text">$</span>
                        <input type="number"
                               class="form-control"
                               name="Items[${idx}].UnitCost"
                               id="cost_${idx}"
                               value="0.00"
                               min="0.01"
                               step="0.01"
                               onchange="recalculateRow(${idx})"
                               oninput="recalculateRow(${idx})"
                               required>
                    </div>
                </td>

                <!-- Row total -->
                <td class="text-end">
                    <span class="fw-semibold text-primary"
                          id="rowTotal_${idx}">
                        $0.00
                    </span>
                </td>

                <!-- Remove button -->
                <td class="text-center">
                    <button type="button"
                            class="btn btn-sm btn-outline-danger"
                            onclick="removeItemRow(${idx})"
                            title="Remove">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>`;

    tbody.appendChild(tr);
    recalculateSummary();

    // Focus the new product select
    document.getElementById(`productSelect_${idx}`).focus();
}

// ── When product is selected auto-fill cost ───────────────────────
function onProductSelected(idx) {
    const select = document.getElementById(`productSelect_${idx}`);
    const costEl = document.getElementById(`cost_${idx}`);
    const metaEl = document.getElementById(`productMeta_${idx}`);
    const hiddenId = document.getElementById(`productId_${idx}`);
    const selected = select.options[select.selectedIndex];

    if (!select.value) {
        hiddenId.value = '';
        metaEl.innerHTML = '';
        recalculateRow(idx);
        return;
    }

    const price = parseFloat(selected.dataset.price || 0);
    const stock = parseInt(selected.dataset.stock || 0);
    const unit = selected.dataset.unit || '';

    // Set the hidden productId input
    hiddenId.value = select.value;

    // Auto-fill purchase price
    costEl.value = price.toFixed(2);

    // Show product meta below select
    metaEl.innerHTML = `
                <small class="text-muted">
                    <i class="bi bi-box me-1"></i>
                    Current stock: <strong>${stock}</strong>
                    ${unit ? `&nbsp;·&nbsp; Unit: ${unit}` : ''}
                </small>`;

    recalculateRow(idx);
}

// ── Recalculate a single row total ────────────────────────────────
function recalculateRow(idx) {
    const qty = parseFloat(
        document.getElementById(`qty_${idx}`)?.value || 0);
    const cost = parseFloat(
        document.getElementById(`cost_${idx}`)?.value || 0);
    const rowTotal = qty * cost;
    const el = document.getElementById(`rowTotal_${idx}`);
    if (el) el.textContent = '$' + rowTotal.toFixed(2);

    recalculateSummary();
}

// ── Recalculate the order summary totals ──────────────────────────
function recalculateSummary() {
    const tbody = document.getElementById('itemsBody');
    const rows = tbody.querySelectorAll('tr[data-index]');
    let totalAmt = 0;
    let totalQty = 0;
    let itemCount = 0;

    rows.forEach(row => {
        const idx = row.dataset.index;
        const qty = parseFloat(
            document.getElementById(`qty_${idx}`)?.value || 0);
        const cost = parseFloat(
            document.getElementById(`cost_${idx}`)?.value || 0);

        if (document.getElementById(`productSelect_${idx}`)?.value) {
            totalAmt += qty * cost;
            totalQty += qty;
            itemCount++;
        }
    });

    document.getElementById('summaryItemCount').textContent =
        itemCount;
    document.getElementById('summaryTotalQty').textContent =
        totalQty;
    document.getElementById('summaryTotal').textContent =
        '$' + totalAmt.toFixed(2);
}

// ── Remove a row ──────────────────────────────────────────────────
function removeItemRow(idx) {
    const row = document.getElementById(`item-row-${idx}`);
    const tbody = document.getElementById('itemsBody');
    const emptyRow = document.getElementById('emptyRow');

    if (row) row.remove();

    // Show empty state if no rows left
    if (tbody.querySelectorAll('tr[data-index]').length === 0)
        emptyRow.style.display = '';

    recalculateSummary();
}

// ── Clear all items ───────────────────────────────────────────────
function clearAllItems() {
    if (!confirm('Remove all items from this order?')) return;

    const tbody = document.getElementById('itemsBody');
    const emptyRow = document.getElementById('emptyRow');

    // Remove all data rows
    tbody.querySelectorAll('tr[data-index]')
        .forEach(r => r.remove());

    emptyRow.style.display = '';
    rowIndex = 0;
    recalculateSummary();
}

// ── Validate before submit ────────────────────────────────────────
function validateForm() {
    const supplierVal = document
        .getElementById('supplierSelect').value;

    if (!supplierVal) {
        alert('Please select a supplier.');
        document.getElementById('supplierSelect').focus();
        return false;
    }

    const tbody = document.getElementById('itemsBody');
    const rows = tbody.querySelectorAll('tr[data-index]');

    if (rows.length === 0) {
        alert('Please add at least one product to the order.');
        return false;
    }

    let valid = true;

    rows.forEach(row => {
        const idx = row.dataset.index;
        const prod = document
            .getElementById(`productSelect_${idx}`)?.value;
        const qty = parseFloat(
            document.getElementById(`qty_${idx}`)?.value || 0);
        const cost = parseFloat(
            document.getElementById(`cost_${idx}`)?.value || 0);

        if (!prod) {
            alert('Please select a product for all rows.');
            document.getElementById(
                `productSelect_${idx}`).focus();
            valid = false;
            return;
        }

        if (qty <= 0) {
            alert('Quantity must be greater than 0.');
            document.getElementById(`qty_${idx}`).focus();
            valid = false;
            return;
        }

        if (cost <= 0) {
            alert('Unit cost must be greater than 0.');
            document.getElementById(`cost_${idx}`).focus();
            valid = false;
            return;
        }
    });

    if (valid) {
        const btn = document.getElementById('submitBtn');
        btn.disabled = true;
        btn.innerHTML = `
                    <span class="spinner-border
                                 spinner-border-sm me-2"></span>
                    Creating...`;
    }

    return valid;
}

// ── Init: add first empty row on page load ────────────────────────
document.addEventListener('DOMContentLoaded', () => {
    addItemRow();
});

 