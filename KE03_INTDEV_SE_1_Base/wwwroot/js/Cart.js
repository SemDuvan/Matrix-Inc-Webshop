function addProductFromButton(btn) {
    addToCart({
        id: parseInt(btn.getAttribute('data-id')),
        name: btn.getAttribute('data-name'),
        price: parseFloat(btn.getAttribute('data-price')),
        imgSrc: btn.getAttribute('data-imgsrc')
    });
}

function addToCart(product) {
    let cart = JSON.parse(sessionStorage.getItem('cart') || '[]');
    let existing = cart.find(i => i.id === product.id);
    if (existing) {
        existing.quantity += 1;
    } else {
        product.quantity = 1;
        cart.push(product);
    }
    sessionStorage.setItem('cart', JSON.stringify(cart));
    if (window.updateCartUI) window.updateCartUI();
    showCartToast(product.name + " toegevoegd aan winkelkar!");
}

function showCartToast(message) {
    let toast = document.createElement('div');
    toast.textContent = message;
    toast.style.position = 'fixed';
    toast.style.bottom = '30px';
    toast.style.right = '30px';
    toast.style.background = '#212121';
    toast.style.color = '#fff';
    toast.style.padding = '12px 24px';
    toast.style.borderRadius = '8px';
    toast.style.zIndex = 9999;
    document.body.appendChild(toast);
    setTimeout(() => document.body.removeChild(toast), 2000);
}
