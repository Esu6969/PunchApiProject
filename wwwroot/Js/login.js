// Create floating particles
function createParticles() {
    const container = document.querySelector('.container');
    for (let i = 0; i < 15; i++) {
        const particle = document.createElement('div');
        particle.className = 'particle';
        particle.style.width = Math.random() * 30 + 10 + 'px';
        particle.style.height = particle.style.width;
        particle.style.left = Math.random() * 100 + '%';
        particle.style.top = Math.random() * 100 + '%';
        particle.style.animationDuration = Math.random() * 10 + 15 + 's';
        particle.style.animationDelay = Math.random() * 5 + 's';
        container.appendChild(particle);
    }
}

// Handle form submission
document.getElementById('loginForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const employeeId = document.getElementById('employeeId').value.trim();
    const submitBtn = document.getElementById('submitBtn');
    const btnText = document.getElementById('btnText');
    const btnArrow = document.getElementById('btnArrow');
    const errorMessage = document.getElementById('errorMessage');

    // Hide error message
    errorMessage.classList.remove('show');

    // Validate input
    if (!employeeId) {
        errorMessage.textContent = '⚠️ Please enter your Employee ID';
        errorMessage.classList.add('show');
        return;
    }

    // Show loading state
    submitBtn.disabled = true;
    btnText.textContent = 'Signing in...';
    btnArrow.innerHTML = '<div class="loading-spinner"></div>';

    try {
        console.log('Attempting login with Employee ID:', employeeId);
        
        // Replace with your actual API endpoint - UPDATED URL
        const response = await fetch('http://localhost:5031/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ 
                employeeId: employeeId  // Keep as string
            })
        });

        console.log('Response status:', response.status);

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Login failed');
        }

        const data = await response.json();
        console.log('Login successful:', data);
        
        // Store employee data in sessionStorage
        sessionStorage.setItem('employeeData', JSON.stringify(data));
        
        // Show success message
        btnText.textContent = 'Success!';
        btnArrow.textContent = '✓';
        
        // Redirect to dashboard after short delay
        setTimeout(() => {
            window.location.href = 'dashboard.html';
        }, 500);
        
    } catch (error) {
        console.error('Login error:', error);
        errorMessage.textContent = `⚠️ ${error.message}`;
        errorMessage.classList.add('show');
        
        // Reset button
        submitBtn.disabled = false;
        btnText.textContent = 'Punch In';
        btnArrow.textContent = '🔓';
    }
});

// Add input focus effects
document.querySelectorAll('input').forEach(input => {
    input.addEventListener('focus', function() {
        this.parentElement.style.transform = 'scale(1.02)';
    });

    input.addEventListener('blur', function() {
        this.parentElement.style.transform = 'scale(1)';
    });
});

// Initialize particles on page load
window.addEventListener('DOMContentLoaded', function() {
    createParticles();
});

// Add parallax mouse effect
document.addEventListener('mousemove', function(e) {
    const card = document.querySelector('.login-card');
    if (card) {
        const x = (e.clientX / window.innerWidth - 0.5) * 10;
        const y = (e.clientY / window.innerHeight - 0.5) * 10;
        
        card.style.transform = `translate(${-x}px, ${-y}px)`;
    }
});