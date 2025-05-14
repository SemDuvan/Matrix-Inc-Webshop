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