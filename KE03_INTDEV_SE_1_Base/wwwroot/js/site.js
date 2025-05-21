document.addEventListener('DOMContentLoaded', function () {
    var productModal = document.getElementById('productModal');
    if (productModal) {
        productModal.addEventListener('show.bs.modal', function (event) {
            var button = event.relatedTarget;
            document.getElementById('modalTitle').textContent = button.getAttribute('data-name');
            document.getElementById('modalPrice').textContent = button.getAttribute('data-price');
            //document.getElementById('modalDescription').textContent = button.getAttribute('data-description');
            //document.getElementById('modalImage').src = button.getAttribute('data-image');
        });
    }
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
        cart.forEach((item, index) => {
            dropdown.innerHTML += `
        <li class="dropdown-item d-flex align-items-center">
            <img src="${item.imgSrc}" style="width:40px;height:30px;object-fit:cover;margin-right:8px;" />
            <span class="me-2">${item.name}</span>
            <button type="button" class="btn btn-sm btn-light ms-2 px-2 py-0" onclick="decrementCartItem(${item.id}); event.stopPropagation(); return false;">-</button>
            <span class="mx-2">${item.quantity}</span>
            <button type="button" class="btn btn-sm btn-light px-2 py-0" onclick="incrementCartItem(${item.id}); event.stopPropagation(); return false;">+</button>
            <span class="ms-2">€${item.price.toFixed(2)}</span>
            <button type="button" class="btn btn-sm btn-danger ms-2 px-2 py-0" onclick="removeCartItem(${item.id}); event.stopPropagation(); return false;">&times;</button>
        </li>`;
        });
    }
    dropdown.innerHTML += '<li><hr class="dropdown-divider" /></li>';

    const total = cart.reduce((sum, item) => sum + item.price * item.quantity, 0);

    // Haal het anti-forgery token op uit het verborgen formulier
    const tokenInput = document.querySelector('#antiForgeryTokenForm input[name="__RequestVerificationToken"]');
    const tokenHtml = tokenInput
        ? `<input type="hidden" name="__RequestVerificationToken" value="${tokenInput.value}" />`
        : '';

    dropdown.innerHTML += `<li class="dropdown-item fw-bold" id="cart-total">Totaal: €${total.toFixed(2)}</li>
    <li class="dropdown-item">
        <form method="post" action="/Bestellen?handler=PlaatsBestelling" id="cartFormDropdown">
            <input type="hidden" name="CartJson" id="CartJsonDropdown" />
            ${tokenHtml}
            <button type="submit" class="btn btn-primary w-100">Bestellen</button>
        </form>
    </li>`;
}

document.addEventListener('DOMContentLoaded', updateCartUI);
window.updateCartUI = updateCartUI;

function incrementCartItem(id) {
    let cart = getCart();
    let item = cart.find(i => i.id === id);
    if (item) {
        item.quantity += 1;
        sessionStorage.setItem('cart', JSON.stringify(cart));
        if (window.updateCartUI) window.updateCartUI();
    }
}

function decrementCartItem(id) {
    let cart = getCart();
    let item = cart.find(i => i.id === id);
    if (item) {
        item.quantity -= 1;
        if (item.quantity <= 0) {
            cart = cart.filter(i => i.id !== id);
        }
        sessionStorage.setItem('cart', JSON.stringify(cart));
        if (window.updateCartUI) window.updateCartUI();
    }
}

function removeCartItem(id) {
    let cart = getCart();
    cart = cart.filter(i => i.id !== id);
    sessionStorage.setItem('cart', JSON.stringify(cart));
    if (window.updateCartUI) window.updateCartUI();
}


document.body.addEventListener('submit', function (e) {
    if (e.target && e.target.id === 'cartFormDropdown') {
        e.preventDefault();
        // Filter imgSrc eruit:
        let cart = getCart().map(({ id, name, price, quantity, ProductId }) => ({
            id: id ?? ProductId, // afhankelijk van je cart-structuur
            name,
            price,
            //quantity
        }));
        document.getElementById('CartJsonDropdown').value = JSON.stringify(cart);

        const form = e.target;
        const formData = new FormData(form);
        for (let pair of formData.entries()) {
            console.log(pair[0] + ': ' + pair[1]);
        }

        fetch('/Bestellen?handler=PlaatsBestelling', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) throw new Error("Server error");
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    updateCartUI();
                } else {
                    alert("Er ging iets mis met je bestelling.");
                }
            })
            .catch(() => alert("Er ging iets mis met de verbinding of server."));
    }
});


