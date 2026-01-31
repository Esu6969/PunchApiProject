// Create floating particles
function createParticles() {
    const container = document.querySelector('.container');
    for (let i = 0; i < 15; i++) {
        const particle = document.createElement('div');
        particle.className = 'particle';
        particle.style.cssText = `
            position: absolute;
            width: ${Math.random() * 30 + 10}px;
            height: ${Math.random() * 30 + 10}px;
            left: ${Math.random() * 100}%;
            top: ${Math.random() * 100}%;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.3);
            animation: floatCircle ${Math.random() * 10 + 15}s ease-in-out infinite;
            animation-delay: ${Math.random() * 5}s;
            pointer-events: none;
        `;
        container.appendChild(particle);
    }
}

// Set today's date as default for join date
document.addEventListener('DOMContentLoaded', function() {
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('joinDate').value = today;
    
    createParticles();
});

// Handle form submission
document.getElementById('registerForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const submitBtn = document.getElementById('submitBtn');
    const btnText = document.getElementById('btnText');
    const btnArrow = document.getElementById('btnArrow');
    const successMessage = document.getElementById('successMessage');
    const errorMessage = document.getElementById('errorMessage');

    // Hide messages
    successMessage.classList.remove('show');
    errorMessage.classList.remove('show');

    // Get form data
    const formData = {
        firstName: document.getElementById('firstName').value.trim(),
        lastName: document.getElementById('lastName').value.trim(),
        email: document.getElementById('email').value.trim(),
        phone: document.getElementById('phone').value.trim(),
        department: document.getElementById('department').value,
        position: document.getElementById('position').value.trim(),
        employeeId: document.getElementById('employeeId').value.trim(),
        joinDate: document.getElementById('joinDate').value,
        password: document.getElementById('password').value
    };

    const confirmPassword = document.getElementById('confirmPassword').value;

    // Validate passwords match
    if (formData.password !== confirmPassword) {
        errorMessage.textContent = '⚠️ Passwords do not match!';
        errorMessage.classList.add('show');
        return;
    }

    // Validate password length
    if (formData.password.length < 6) {
        errorMessage.textContent = '⚠️ Password must be at least 6 characters!';
        errorMessage.classList.add('show');
        return;
    }

    // Show loading state
    submitBtn.disabled = true;
    btnText.textContent = 'Creating Account...';
    btnArrow.innerHTML = '<div class="loading-spinner"></div>';

    console.log('Sending registration data:', formData);

    try {
        // UPDATED: Use correct API endpoint
        const response = await fetch('http://localhost:5031/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData)
        });

        console.log('Response status:', response.status);
        const data = await response.json();
        console.log('Response data:', data);

        if (!response.ok) {
            throw new Error(data.message || 'Registration failed');
        }

        // Show success message
        successMessage.textContent = '✅ ' + (data.message || 'Registration successful! Redirecting to login...');
        successMessage.classList.add('show');
        
        // Reset form
        document.getElementById('registerForm').reset();
        
        // Redirect to login after 2 seconds
        setTimeout(() => {
            window.location.href = 'login.html';
        }, 2000);
        
    } catch (error) {
        console.error('Registration error:', error);
        errorMessage.textContent = `⚠️ ${error.message}`;
        errorMessage.classList.add('show');
        
        // Reset button
        submitBtn.disabled = false;
        btnText.textContent = 'Create Account';
        btnArrow.textContent = '→';
    }
});

// Real-time password validation
document.getElementById('password').addEventListener('input', function() {
    const password = this.value;
    const confirmPassword = document.getElementById('confirmPassword');
    
    if (password.length < 6 && password.length > 0) {
        this.style.borderColor = '#ef4444';
    } else if (password.length >= 6) {
        this.style.borderColor = '#10b981';
    } else {
        this.style.borderColor = '#e5e7eb';
    }
    
    // Check confirm password if it has value
    if (confirmPassword.value) {
        validatePasswordMatch();
    }
});

document.getElementById('confirmPassword').addEventListener('input', validatePasswordMatch);

function validatePasswordMatch() {
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword');
    
    if (confirmPassword.value === password && password.length >= 6) {
        confirmPassword.style.borderColor = '#10b981';
    } else if (confirmPassword.value.length > 0) {
        confirmPassword.style.borderColor = '#ef4444';
    } else {
        confirmPassword.style.borderColor = '#e5e7eb';
    }
}

// Add input focus effects
document.querySelectorAll('input, select').forEach(input => {
    input.addEventListener('focus', function() {
        this.parentElement.style.transform = 'scale(1.01)';
        this.parentElement.style.transition = 'transform 0.3s ease';
    });

    input.addEventListener('blur', function() {
        this.parentElement.style.transform = 'scale(1)';
    });
});

// Add parallax mouse effect
document.addEventListener('mousemove', function(e) {
    const card = document.querySelector('.register-card');
    if (card) {
        const x = (e.clientX / window.innerWidth - 0.5) * 10;
        const y = (e.clientY / window.innerHeight - 0.5) * 10;
        
        card.style.transform = `translate(${-x}px, ${-y}px)`;
        card.style.transition = 'transform 0.3s ease';
    }
});

// Format phone number as user types
document.getElementById('phone').addEventListener('input', function(e) {
    let value = e.target.value.replace(/\D/g, '');
    if (value.length > 0) {
        if (value.length <= 3) {
            value = `+1 (${value}`;
        } else if (value.length <= 6) {
            value = `+1 (${value.slice(0, 3)}) ${value.slice(3)}`;
        } else {
            value = `+1 (${value.slice(0, 3)}) ${value.slice(3, 6)}-${value.slice(6, 10)}`;
        }
    }
    e.target.value = value;
});