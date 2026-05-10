// POS.Web/wwwroot/js/multi-select.js
// Pure vanilla JS — no jQuery, no external library needed

function initMultiSelect(wrapper) {

    // ── Read config from data attributes ─────────────────────────────────
    const fieldName = wrapper.dataset.fieldName;
    const placeholder = wrapper.dataset.placeholder || 'Click to select...';
    //const allOptions = JSON.parse(wrapper.dataset.options || '[]');
    let allOptions = [];
    try {
        const raw = wrapper.dataset.options;
        if (raw) allOptions = JSON.parse(raw);
    } catch(e) {
        console.warn(`multi-select: failed to parse options for
            field "${fieldName}"`, e);
    }
    let selectedIds = [];
    try {
        const raw = wrapper.dataset.selected;
        if (raw) selectedIds = JSON.parse(raw);
    } catch (e) {
        console.warn(`multi-select: failed to parse selected for
            field "${fieldName}"`, e);
    }
    const preSelected = JSON.parse(wrapper.dataset.selected || '[]');
    allOptions = allOptions.filter(
        opt => opt && typeof opt.text === 'string' && opt.text.trim() !== ''
    );
    // ── State ─────────────────────────────────────────────────────────────
    // selectedIds is the live array of chosen option IDs
    //let selectedIds = [...preSelected];
    let isOpen = false;

    // ── Build DOM structure ───────────────────────────────────────────────
    wrapper.style.position = 'relative';
    wrapper.innerHTML = `
        <div class="ms-control" id="ms-control-${fieldName}">

            <!-- Tags + search input row -->
            <div class="ms-tags-input" id="ms-tags-${fieldName}">
                <div class="ms-tags"    id="ms-tag-list-${fieldName}"></div>
                <input  class="ms-search-input"
                        id="ms-search-${fieldName}"
                        type="text"
                        placeholder="${placeholder}"
                        autocomplete="off">
            </div>

            <!-- Clear all link — hidden until something selected -->
            <div class="ms-clear-row" id="ms-clear-row-${fieldName}"
                 style="display:none">
                <a href="#"
                   class="ms-clear-all"
                   id="ms-clear-all-${fieldName}">
                    Clear all selections
                </a>
            </div>
        </div>

        <!-- Dropdown panel -->
        <div class="ms-dropdown" id="ms-dropdown-${fieldName}"
             style="display:none">
            <ul class="ms-option-list"
                id="ms-option-list-${fieldName}"></ul>
            <div class="ms-no-results"
                 id="ms-no-results-${fieldName}"
                 style="display:none">
                No matching categories found
            </div>
        </div>

        <!-- Hidden inputs submitted with the form -->
        <div id="ms-hidden-inputs-${fieldName}"></div>
    `;

    // ── Element references ────────────────────────────────────────────────
    const control = document.getElementById(`ms-control-${fieldName}`);
    const tagsInput = document.getElementById(`ms-tags-${fieldName}`);
    const tagList = document.getElementById(`ms-tag-list-${fieldName}`);
    const searchInput = document.getElementById(`ms-search-${fieldName}`);
    const dropdown = document.getElementById(`ms-dropdown-${fieldName}`);
    const optionList = document.getElementById(`ms-option-list-${fieldName}`);
    const noResults = document.getElementById(`ms-no-results-${fieldName}`);
    const clearRow = document.getElementById(`ms-clear-row-${fieldName}`);
    const clearAllLink = document.getElementById(`ms-clear-all-${fieldName}`);
    const hiddenInputs = document.getElementById(`ms-hidden-inputs-${fieldName}`);

    // ── Render tags (selected items shown as pills) ───────────────────────
    function renderTags() {
        tagList.innerHTML = '';

        selectedIds.forEach(id => {
            const opt = allOptions.find(o => o.id === id);
            if (!opt) return;

            const tag = document.createElement('span');
            tag.className = 'ms-tag';
            tag.dataset.id = id;
            tag.innerHTML = `
                ${opt.text}
                <button type="button"
                        class="ms-tag-remove"
                        data-id="${id}"
                        title="Remove ${opt.text}">
                    &times;
                </button>`;
            tagList.appendChild(tag);
        });

        // Show or hide the placeholder text
        searchInput.placeholder = selectedIds.length > 0
            ? ''
            : placeholder;

        // Show or hide the clear-all link
        clearRow.style.display = selectedIds.length > 0
            ? 'block'
            : 'none';

        renderHiddenInputs();
    }

    // ── Render hidden inputs for form submission ──────────────────────────
    // Each selected ID needs its own
    // <input type="hidden" name="CategoryIds" value="X">
    function renderHiddenInputs() {
        hiddenInputs.innerHTML = '';
        selectedIds.forEach(id => {
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = fieldName;
            input.value = id;
            hiddenInputs.appendChild(input);
        });
    }

    // ── Render dropdown options filtered by search term ───────────────────
    function renderOptions(searchTerm = '') {
        const term = searchTerm.toLowerCase().trim();
        

        const filtered = allOptions.filter(opt => 
            opt.text.toLowerCase().includes(term) ||
                (opt.subText && opt.subText.toLowerCase().includes(term))
        );
        
        optionList.innerHTML = '';

        filtered.forEach(opt => {
            const isSelected = selectedIds.includes(opt.id);

            const li = document.createElement('li');
            li.className = `ms-option ${isSelected ? 'ms-selected' : ''}`;
            li.dataset.id = opt.id;

            // Highlight matching search text
            const highlightedText = term
                ? opt.text.replace(
                    new RegExp(`(${escapeRegex(term)})`, 'gi'),
                    '<mark>$1</mark>')
                : opt.text;

            li.innerHTML = `
                <span class="ms-option-check">
                    ${isSelected ? '&#10003;' : ''}
                </span>
                <span class="ms-option-text">
                    <span class="ms-option-name">
                        ${highlightedText}
                    </span>
                    ${opt.subText
                    ? `<span class="ms-option-sub">
                               ${opt.subText}
                           </span>`
                    : ''}
                </span>`;

            li.addEventListener('mousedown', e => {
                // mousedown fires before blur — prevent dropdown closing
                e.preventDefault();
                toggleOption(opt.id);
            });
            optionList.appendChild(li);
        });

        // Show "no results" message if nothing matches
        noResults.style.display =
            filtered.length === 0 ? 'block' : 'none';
    }

    // ── Toggle a single option selected/deselected ────────────────────────
    function toggleOption(id) {
        const index = selectedIds.indexOf(id);
        if (index === -1)
            selectedIds.push(id);
        else
            selectedIds.splice(index, 1);

        renderTags();
        renderOptions(searchInput.value);
        searchInput.focus();
    }

    // ── Open dropdown ─────────────────────────────────────────────────────
    function openDropdown() {
        if (isOpen) return;
        isOpen = true;
        dropdown.style.display = 'block';
        renderOptions('');
        searchInput.value = '';
        control.classList.add('ms-open');
    }

    // ── Close dropdown ────────────────────────────────────────────────────
    function closeDropdown() {
        if (!isOpen) return;
        isOpen = false;
        dropdown.style.display = 'none';
        searchInput.value = '';
        control.classList.remove('ms-open');
    }

    // ── Clear all selections ──────────────────────────────────────────────
    function clearAll() {
        selectedIds = [];
        renderTags();
        renderOptions('');
    }

    // ── Escape special regex chars in search term ─────────────────────────
    function escapeRegex(str) {
        return str.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    }

    // ── Event listeners ───────────────────────────────────────────────────

    // Open dropdown when clicking the control box
    tagsInput.addEventListener('click', e => {
        if (e.target.classList.contains('ms-tag-remove')) return;
        openDropdown();
        searchInput.focus();
    });

    // Live search — filter options as user types
    searchInput.addEventListener('input', () => {
        renderOptions(searchInput.value);
        if (!isOpen) openDropdown();
    });

    // Close when focus leaves the whole component
    searchInput.addEventListener('blur', () => {
        setTimeout(closeDropdown, 150);
    });

    // Remove individual tag by clicking × button
    tagList.addEventListener('click', e => {
        const btn = e.target.closest('.ms-tag-remove');
        if (!btn) return;
        e.stopPropagation();
        const id = parseInt(btn.dataset.id);
        toggleOption(id);
    });

    // Clear all link
    clearAllLink.addEventListener('click', e => {
        e.preventDefault();
        clearAll();
    });

    // Keyboard: Escape closes dropdown
    searchInput.addEventListener('keydown', e => {
        if (e.key === 'Escape') closeDropdown();

        // Backspace removes last tag when search is empty
        if (e.key === 'Backspace' &&
            searchInput.value === '' &&
            selectedIds.length > 0) {
            toggleOption(selectedIds[selectedIds.length - 1]);
        }
    });

    // Close if clicking outside the component
    document.addEventListener('click', e => {
        if (!wrapper.contains(e.target)) closeDropdown();
    });

    // ── Initial render ────────────────────────────────────────────────────
    renderTags();
}