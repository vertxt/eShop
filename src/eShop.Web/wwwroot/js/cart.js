$(document).ready(function() {
    $('.quantity-minus').click(function() {
        var input = $(this).closest('.input-group').find('.quantity-input');
        var value = parseInt(input.val());
        if (value > 1) {
            input.val(value - 1);
            showUpdateButton(input);
        }
    });

    $('.quantity-plus').click(function() {
        var input = $(this).closest('.input-group').find('.quantity-input');
        var value = parseInt(input.val());
        var max = parseInt(input.attr('max'));
        if (value < max) {
            input.val(value + 1);
            showUpdateButton(input);
        }
    });

    $('.quantity-input').change(function() {
        showUpdateButton($(this));
    });

    function showUpdateButton(input) {
        input.closest('.quantity-form').find('.update-quantity').removeClass('d-none');
    }
});