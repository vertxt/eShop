document.getElementById("clearFilterBtn").addEventListener('click', function(e) {
    e.preventDefault();
    console.log("Clicked");
    document.querySelectorAll('#filterForm input[type="checkbox"]').forEach(checkbox => {
        checkbox.checked = false;
    });
    
    document.getElementById("filterForm").submit();
})