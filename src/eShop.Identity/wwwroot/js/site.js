document.addEventListener('DOMContentLoaded', function() {
    // Toggle password visibility
    const togglePasswordButtons = document.querySelectorAll('.toggle-password');
    togglePasswordButtons.forEach(button => {
        button.addEventListener('click', function() {
            const input = this.closest('.input-group').querySelector('input');
            const icon = this.querySelector('i');
            
            if (input.type === 'password') {
                input.type = 'text';
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                input.type = 'password';
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        });
    });
    
    // Password strength meter
    const passwordInput = document.querySelector('input[name="Input.Password"]');
    if (passwordInput) {
        passwordInput.addEventListener('input', function() {
            const progressBar = document.querySelector('.progress .progress-bar');
            const feedback = document.querySelector('.password-feedback');
            
            if (!progressBar || !feedback) return;
            
            let strength = 0;
            let status = 'None';
            
            if (this.value.length > 0) strength += 20;
            if (this.value.length >= 8) strength += 20;
            if (/[A-Z]/.test(this.value)) strength += 20;
            if (/[0-9]/.test(this.value)) strength += 20;
            if (/[^A-Za-z0-9]/.test(this.value)) strength += 20;
            
            progressBar.style.width = strength + '%';
            
            if (strength <= 20) {
                progressBar.className = 'progress-bar bg-danger';
                status = 'Very Weak';
            } else if (strength <= 40) {
                progressBar.className = 'progress-bar bg-warning';
                status = 'Weak';
            } else if (strength <= 60) {
                progressBar.className = 'progress-bar bg-info';
                status = 'Medium';
            } else if (strength <= 80) {
                progressBar.className = 'progress-bar bg-primary';
                status = 'Strong';
            } else {
                progressBar.className = 'progress-bar bg-success';
                status = 'Very Strong';
            }
            
            feedback.textContent = status;
        });
    }
});