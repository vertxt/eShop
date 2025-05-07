// reviews-pagination.js
class ReviewsPagination {
    constructor() {
        this.init();
    }

    init() {
        const loadMoreBtn = document.getElementById('loadMoreReviews');
        
        if (loadMoreBtn) {
            loadMoreBtn.addEventListener('click', this.handleLoadMore.bind(this));
        }
    }

    handleLoadMore(event) {
        const btn = event.currentTarget;
        const productId = btn.getAttribute('data-product-id');
        const currentPage = parseInt(btn.getAttribute('data-current-page'));
        const pageSize = parseInt(btn.getAttribute('data-page-size'));
        const nextPage = currentPage + 1;
        
        // Show loading indicator
        btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...';
        btn.disabled = true;
        
        // Get both the JSON data (for pagination info) and HTML (for rendered reviews)
        Promise.all([
            fetch(`/api/products/${productId}/reviews?PageNumber=${nextPage}&PageSize=${pageSize}`).then(resp => resp.json()),
            fetch(`/api/products/${productId}/reviews/html?PageNumber=${nextPage}&PageSize=${pageSize}`).then(resp => resp.text())
        ])
        .then(([data, html]) => {
            console.log(data);
            // Add new reviews to the list
            const reviewsList = document.getElementById('reviewsList');
            reviewsList.insertAdjacentHTML('beforeend', html);
            
            // Update button state
            btn.innerHTML = 'Load More';
            btn.disabled = false;
            
            // Update page number
            btn.setAttribute('data-current-page', nextPage);
            
            // Hide button if no more pages
            if (!data.metadata.hasNextPage) {
                btn.style.display = 'none';
            }
        })
        .catch(error => {
            console.error('Error loading more reviews:', error);
            btn.innerHTML = 'Load More';
            btn.disabled = false;
        });
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    new ReviewsPagination();
});