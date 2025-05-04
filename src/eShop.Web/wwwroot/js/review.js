// Review form handling
document.addEventListener('DOMContentLoaded', function() {
    // Handle star rating selection
    const ratingInputs = document.querySelectorAll('.rating input');
    
    ratingInputs.forEach(input => {
        input.addEventListener('change', function() {
            const labels = this.parentNode.querySelectorAll('label');
            
            // Reset all stars
            labels.forEach(label => {
                label.querySelector('i').classList.remove('text-warning');
                label.querySelector('i').style.color = '#ddd';
            });
            
            // Fill stars up to selected rating
            const selectedIndex = Array.from(ratingInputs).indexOf(this);
            for (let i = 0; i <= selectedIndex; i++) {
                const starIcon = ratingInputs[i].nextElementSibling.querySelector('i');
                starIcon.classList.add('text-warning');
            }
        });
    });
    
    // Review form validation
    const reviewForm = document.querySelector('form[asp-page-handler="SubmitReview"]');
    if (reviewForm) {
        reviewForm.addEventListener('submit', function(e) {
            let valid = true;
            
            // Check if a rating is selected
            const ratingSelected = Array.from(ratingInputs).some(input => input.checked);
            if (!ratingSelected) {
                const ratingErrorSpan = document.querySelector('span[data-valmsg-for="NewReview.Rating"]');
                if (ratingErrorSpan) {
                    ratingErrorSpan.textContent = 'Please select a rating';
                    ratingErrorSpan.style.display = 'block';
                }
                valid = false;
            }
            
            // Check if title field is filled
            const titleInput = document.getElementById('reviewTitle');
            if (titleInput && !titleInput.value.trim()) {
                const titleErrorSpan = document.querySelector('span[data-valmsg-for="NewReview.Title"]');
                if (titleErrorSpan) {
                    titleErrorSpan.textContent = 'Title is required';
                    titleErrorSpan.style.display = 'block';
                }
                valid = false;
            }
            
            // Check if comment field is filled
            const commentInput = document.getElementById('reviewComment');
            if (commentInput && !commentInput.value.trim()) {
                const commentErrorSpan = document.querySelector('span[data-valmsg-for="NewReview.Body"]');
                if (commentErrorSpan) {
                    commentErrorSpan.textContent = 'Review content is required';
                    commentErrorSpan.style.display = 'block';
                }
                valid = false;
            }
            
            if (!valid) {
                e.preventDefault();
            }
        });
    }
    
    // Handle review tab display
    const reviewTab = document.getElementById('reviews-tab');
    if (reviewTab) {
        reviewTab.addEventListener('click', function() {
            // Scroll to review form if coming from a review CTA elsewhere on page
            if (window.location.hash === '#write-review') {
                setTimeout(() => {
                    const reviewForm = document.querySelector('.product-reviews form');
                    if (reviewForm) {
                        reviewForm.scrollIntoView({ behavior: 'smooth' });
                    }
                }, 300);
            }
        });
    }
    
    // Auto-open review tab if hash is present
    if (window.location.hash === '#reviews' || window.location.hash === '#write-review') {
        const reviewTab = document.getElementById('reviews-tab');
        if (reviewTab) {
            const tab = new bootstrap.Tab(reviewTab);
            tab.show();
        }
    }
});