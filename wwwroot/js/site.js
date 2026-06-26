// ===== ShopEase - Main JavaScript =====

// Add to cart function (used globally)
async function addToCart(productId, quantity = 1) {
    try {
        const response = await fetch('/Cart/AddToCart', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `productId=${productId}&quantity=${quantity}`
        });

        const data = await response.json();

        if (data.success) {
            // Update cart count badge
            const badge = document.getElementById('cartCount');
            if (badge) badge.textContent = data.cartCount;

            // Show toast
            showToast(data.message || 'Added to cart!', 'success');
        } else {
            showToast(data.message || 'Could not add to cart.', 'danger');
        }
    } catch (error) {
        showToast('Something went wrong. Please try again.', 'danger');
    }
}

// Toast notification
function showToast(message, type = 'success') {
    const toastEl = document.getElementById('cartToast');
    const toastMsg = document.getElementById('toastMessage');

    if (toastEl && toastMsg) {
        toastMsg.textContent = message;
        toastEl.className = `toast align-items-center text-bg-${type} border-0`;

        const toast = new bootstrap.Toast(toastEl, { delay: 2500 });
        toast.show();
    }
}

// Bind all "Add to Cart" buttons
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.add-to-cart-btn').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            const productId = this.dataset.productId;
            if (!productId) return;

            // Add animation
            this.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Adding...';
            this.disabled = true;

            addToCart(parseInt(productId), 1).then(() => {
                setTimeout(() => {
                    this.innerHTML = '<i class="bi bi-cart-check me-1"></i> Added!';
                    setTimeout(() => {
                        this.innerHTML = '<i class="bi bi-cart-plus me-1"></i> Add to Cart';
                        this.disabled = false;
                    }, 1200);
                }, 300);
            });
        });
    });

    // Animate cards on scroll
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, { threshold: 0.1 });

    document.querySelectorAll('.product-card, .category-card, .card').forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        card.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
        observer.observe(card);
    });
});
