document.addEventListener("DOMContentLoaded", function() {
    const searchInput = document.getElementById("searchBar");
    const pageSizeSelect = document.getElementById("pageSizeSelect");
    const sortBySelect = document.getElementById("sortBySelect");
    
    const baseUrl = window.location.pathname;

    function getCurrentParams() {
        const params = new URLSearchParams(window.location.search);
        return params;
    }
    
    function updateWithParams(params) {
        params.set('PageNumber', '1');
        
        window.location.href = baseUrl + '?' + params.toString();
    }
    
    if (searchInput) {
        const params = getCurrentParams();
        searchInput.value = params.get('SearchTerm');

        searchInput.addEventListener('keypress', (event) => {
            if (event.key === "Enter") {
                const params = getCurrentParams();

                if (searchInput.value) {
                    params.set('SearchTerm', searchInput.value);
                } else {
                    params.delete('SearchTerm');
                }
                
                updateWithParams(params);
            }
        })
    }
    
    if (pageSizeSelect) {
        pageSizeSelect.addEventListener('change', function() {
            const params = getCurrentParams();
            
            if (pageSizeSelect.value) {
                params.set('PageSize', pageSizeSelect.value);
            } else {
                params.delete('PageSize');
            }
            
            updateWithParams(params);
        });
    }
    
    if (sortBySelect) {
        sortBySelect.addEventListener('change', function() {
            const params = getCurrentParams();
            
            if (sortBySelect.value) {
                params.set('SortBy', sortBySelect.value);
            } else {
                params.delete('SortBy');
            }
            
            updateWithParams(params);
        });
    }
})