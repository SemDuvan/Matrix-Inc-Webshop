// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    var productModal = document.getElementById('productModal');
    productModal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        document.getElementById('modalTitle').textContent = button.getAttribute('data-name');
        document.getElementById('modalPrice').textContent = button.getAttribute('data-price');
        document.getElementById('modalDescription').textContent = button.getAttribute('data-description');
        document.getElementById('modalImage').src = button.getAttribute('data-image');
    });
});

function getCart() {
    return JSON.parse(sessionStorage.getItem('cart') || '[]');
}


function updateCartUI() {
    const cart = getCart();
    const count = cart.reduce((sum, item) => sum + item.quantity, 0);
    document.getElementById('cart-count').textContent = count;

    const dropdown = document.getElementById('cart-dropdown-list');
    dropdown.innerHTML = '';

    if (cart.length === 0) {
        dropdown.innerHTML = '<li class="dropdown-item text-muted">Winkelkar is leeg</li>';
    } else {
        cart.forEach(item => {
            dropdown.innerHTML += `
                        <li class="dropdown-item d-flex align-items-center">
                            <img src="${item.imgSrc}" style="width:40px;height:30px;object-fit:cover;margin-right:8px;" />
                            <span>${item.name}</span>
                            <span class="ms-auto">x ${item.quantity}</span>
                            <span class="ms-2">€${item.price.toFixed(2)}</span>
                        </li>`;
        });
    }
    dropdown.innerHTML += '<li><hr class="dropdown-divider" /></li>';

    const total = cart.reduce((sum, item) => sum + item.price * item.quantity, 0);
    dropdown.innerHTML += `<li class="dropdown-item fw-bold" id="cart-total">Totaal: €${total.toFixed(2)}</li>
                <li class="dropdown-item"><a href="/Winkelkar" class="btn btn-primary w-100">Bestellen</a></li>`;
}

document.addEventListener('DOMContentLoaded', updateCartUI);
window.updateCartUI = updateCartUI;