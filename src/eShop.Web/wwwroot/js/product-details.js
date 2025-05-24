document.addEventListener("DOMContentLoaded", function () {
  initProductCarousel();
  initVariantSelection();
  handleReviewTabActivation();
});

/*
    Initialize the product carousel
*/
function initProductCarousel() {
  const productCarousel = document.querySelector("#productCarousel");
  if (productCarousel) {
    new bootstrap.Carousel(productCarousel);
  }
}

/*
    Initialize the variant selection functionality
*/
function initVariantSelection() {
  const variantSelect = document.getElementById("variantId");
  const selectedVariantInput = document.getElementById("selectedVariantId");
  const addToCartBtn = document.getElementById("addToCartBtn");
  const productPriceElement = document.getElementById("productPrice");
  const basePrice = productPriceElement
    ? productPriceElement.textContent.replace(/[^0-9.]/g, "")
    : null;

  // Initialize form state
  function updateFormState() {
    if (addToCartBtn) {
      addToCartBtn.disabled = false;
    }

    if (selectedVariantInput) {
      if (variantSelect && variantSelect.value) {
        selectedVariantInput.value = variantSelect.value;
      } else {
        selectedVariantInput.value = "";
      }
    }

    // Update the displayed price based on selected variant
    updateDisplayedPrice();
  }

  /*
    Update the displayed price based on the selected variant
  */
  function updateDisplayedPrice() {
    if (!productPriceElement || !variantSelect) return;

    let priceToDisplay = basePrice;

    // If a variant is selected, use its price
    if (variantSelect.value) {
      const selectedOption = variantSelect.options[variantSelect.selectedIndex];
      if (selectedOption && selectedOption.dataset.price) {
        priceToDisplay = selectedOption.dataset.price;
      }
    }

    // Update the displayed price
    productPriceElement.textContent =
      "$" + parseFloat(priceToDisplay).toFixed(2);
  }

  // Set initial state
  updateFormState();

  // Listen for changes to variant select
  if (variantSelect) {
    variantSelect.addEventListener("change", function () {
      updateFormState();
    });
  }
}

function handleReviewTabActivation() {
  // only run when flag is set
  const productRow = document.querySelector(".row[data-review-failed]");
  const reviewFailed = productRow && productRow.dataset.reviewFailed === "true";
  if (!reviewFailed) return;

  // 1) Activate the Reviews tab
  var reviewsTabEl = document.querySelector("#reviews-tab");
  if (reviewsTabEl) {
    new bootstrap.Tab(reviewsTabEl).show();
  }

  // 2) Scroll to the first error message
  var err = document.querySelector(".text-danger, .validation-summary-errors");
  if (err) {
    err.scrollIntoView({ behavior: "smooth", block: "start" });
  }

  // 3) Reset URL to avoid repost on refresh
  var cleanUrl =
    window.location.pathname +
    window.location.search
      .replace(/(\?|&)handler=[^&]*/g, "")
      .replace(/&$/, "");
  history.replaceState(null, "", cleanUrl);
}
