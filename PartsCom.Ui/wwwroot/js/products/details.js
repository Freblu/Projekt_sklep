// Product Details Page JavaScript

// Image Gallery
function changeImage(imageUrl, thumbnailElement) {
    const mainImage = document.getElementById('mainImage');
    if (mainImage) {
        mainImage.src = imageUrl;
    }

    // Update active thumbnail
    document.querySelectorAll('.thumbnail').forEach(thumb => {
        thumb.classList.remove('active');
    });
    if (thumbnailElement) {
        thumbnailElement.classList.add('active');
    }
}

let thumbnailScrollPosition = 0;
const thumbnailWidth = 88; // 80px width + 8px gap

function scrollThumbnails(direction) {
    const container = document.getElementById('thumbnailsContainer');
    if (!container) return;

    const thumbnails = container.querySelectorAll('.thumbnail');
    const maxScroll = Math.max(0, (thumbnails.length * thumbnailWidth) - container.parentElement.offsetWidth);

    thumbnailScrollPosition += direction * thumbnailWidth * 2;
    thumbnailScrollPosition = Math.max(0, Math.min(thumbnailScrollPosition, maxScroll));

    container.style.transform = `translateX(-${thumbnailScrollPosition}px)`;
}

// Quantity Selector
function changeQuantity(delta) {
    const input = document.getElementById('quantity');
    if (!input) return;

    const min = parseInt(input.min) || 1;
    const max = parseInt(input.max) || 999;
    let value = parseInt(input.value) || 1;

    value += delta;
    value = Math.max(min, Math.min(max, value));

    input.value = value;
}

// Add to Cart
async function addToCart(productId) {
    const quantity = parseInt(document.getElementById('quantity')?.value) || 1;

    try {
        const response = await fetch('/Cart/Add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            },
            body: JSON.stringify({
                productId: productId,
                quantity: quantity
            })
        });

        if (response.ok) {
            // Show success message or update cart icon
            alert('Product added to cart!');
            // Optionally reload or update cart count
        } else if (response.status === 401) {
            // Redirect to login
            window.location.href = '/Account/Login?returnUrl=' + encodeURIComponent(window.location.pathname);
        } else {
            alert('Failed to add product to cart. Please try again.');
        }
    } catch (error) {
        console.error('Error adding to cart:', error);
        alert('An error occurred. Please try again.');
    }
}

// Tabs
document.addEventListener('DOMContentLoaded', function() {
    const tabButtons = document.querySelectorAll('.tab-btn');
    const tabPanels = document.querySelectorAll('.tab-panel');

    tabButtons.forEach(button => {
        button.addEventListener('click', function() {
            const tabId = this.getAttribute('data-tab');

            // Update buttons
            tabButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');

            // Update panels
            tabPanels.forEach(panel => {
                panel.classList.remove('active');
                if (panel.id === tabId) {
                    panel.classList.add('active');
                }
            });
        });
    });

    // Handle quantity input
    const quantityInput = document.getElementById('quantity');
    if (quantityInput) {
        quantityInput.addEventListener('change', function() {
            const min = parseInt(this.min) || 1;
            const max = parseInt(this.max) || 999;
            let value = parseInt(this.value) || 1;

            value = Math.max(min, Math.min(max, value));
            this.value = value;
        });
    }

    // Color options
    const colorOptions = document.querySelectorAll('.color-option');
    colorOptions.forEach(option => {
        option.addEventListener('click', function() {
            colorOptions.forEach(opt => opt.classList.remove('active'));
            this.classList.add('active');
        });
    });
});
