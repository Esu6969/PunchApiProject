// Check if user is logged in
window.onload = function() {
    // Check for session data
    const employeeData = sessionStorage.getItem('employeeData');
    
    if (!employeeData) {
        // Redirect to login if not logged in
        alert('Please login first');
        window.location.href = 'login.html';
        return;
    }

    // Parse employee data
    const userData = JSON.parse(employeeData);
    console.log('Logged in user:', userData);
    
    // Update user interface
    document.getElementById('userName').textContent = userData.name || 'Employee';
    document.getElementById('welcomeName').textContent = (userData.name || 'Employee').split(' ')[0];
    
    // Load user stats
    loadStats(userData);
    updateClock();
    setInterval(updateClock, 1000);
    
    // Attach event listeners
    document.getElementById('punchInBtn').addEventListener('click', () => punchIn(userData));
    document.getElementById('punchOutBtn').addEventListener('click', () => punchOut(userData));
    document.getElementById('logoutBtn').addEventListener('click', logout);
};

// Punch In function
async function punchIn(userData) {
    try {
        console.log('Punching in for employee:', userData.employeeId);
        
        // UPDATED: Use correct API endpoint and send integer ID
        const response = await fetch('http://localhost:5031/api/punch/in', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                employeeId: parseInt(userData.employeeId) || userData.id || 1, // Convert to integer
                timestamp: new Date().toISOString()
            })
        });

        console.log('Punch in response status:', response.status);
        const data = await response.json();
        console.log('Punch in response:', data);

        if (response.ok && data.success) {
            updateStatus('Clocked In', '#10b981');
            addActivityItem('🟢', 'Punched In', 'Started work shift', 'Just now');
            alert('✅ ' + data.message);
        } else {
            throw new Error(data.message || 'Punch in failed');
        }
    } catch (error) {
        console.error('Punch in error:', error);
        alert('❌ ' + error.message);
    }
}

// Punch Out function
async function punchOut(userData) {
    try {
        console.log('Punching out for employee:', userData.employeeId);
        
        // UPDATED: Use correct API endpoint and send integer ID
        const response = await fetch('http://localhost:5031/api/punch/out', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                employeeId: parseInt(userData.employeeId) || userData.id || 1, // Convert to integer
                timestamp: new Date().toISOString()
            })
        });

        console.log('Punch out response status:', response.status);
        const data = await response.json();
        console.log('Punch out response:', data);

        if (response.ok && data.success) {
            updateStatus('Clocked Out', '#ef4444');
            addActivityItem('🔴', 'Punched Out', 'Ended work shift', 'Just now');
            alert('✅ ' + data.message);
        } else {
            throw new Error(data.message || 'Punch out failed');
        }
    } catch (error) {
        console.error('Punch out error:', error);
        alert('❌ ' + error.message);
    }
}

// Update status display
function updateStatus(text, color) {
    const statusElement = document.getElementById('currentStatus');
    statusElement.textContent = text;
    statusElement.style.color = color;
}

// Add activity item to the list
function addActivityItem(icon, title, description, time) {
    const activityList = document.getElementById('activityList');
    const item = document.createElement('div');
    item.className = 'activity-item';
    item.style.animation = 'slideUp 0.5s ease';
    item.innerHTML = `
        <div class="activity-info">
            <div class="activity-icon">${icon}</div>
            <div class="activity-details">
                <h4>${title}</h4>
                <p>${description}</p>
            </div>
        </div>
        <div class="activity-time">${time}</div>
    `;
    activityList.insertBefore(item, activityList.firstChild);
    
    // Keep only last 10 activities
    if (activityList.children.length > 10) {
        activityList.removeChild(activityList.lastChild);
    }
}

// Load stats from API or use demo data
async function loadStats(userData) {
    try {
        const employeeId = parseInt(userData.employeeId) || userData.id || 1;
        console.log('Loading stats for employee:', employeeId);
        
        // UPDATED: Use correct API endpoint
        const response = await fetch(`http://localhost:5031/api/punch/stats/${employeeId}`);
        
        console.log('Stats response status:', response.status);
        
        if (response.ok) {
            const stats = await response.json();
            console.log('Stats data:', stats);
            
            document.getElementById('hoursToday').textContent = stats.hoursToday || '0.0';
            document.getElementById('hoursWeek').textContent = stats.hoursWeek || '0.0';
            document.getElementById('hoursMonth').textContent = stats.hoursMonth || '0.0';
            document.getElementById('attendanceRate').textContent = stats.attendanceRate || '100%';
        } else {
            throw new Error('Failed to load stats');
        }
    } catch (error) {
        console.error('Stats loading error:', error);
        // Use demo data
        document.getElementById('hoursToday').textContent = '0.0';
        document.getElementById('hoursWeek').textContent = '0.0';
        document.getElementById('hoursMonth').textContent = '0.0';
        document.getElementById('attendanceRate').textContent = '100%';
    }
}

// Update clock display
function updateClock() {
    const now = new Date();
    const timeString = now.toLocaleTimeString('en-US', { 
        hour: '2-digit', 
        minute: '2-digit', 
        second: '2-digit' 
    });
    const statusTime = document.getElementById('statusTime');
    if (statusTime) {
        statusTime.textContent = timeString;
    }
}

// Logout function
function logout() {
    if (confirm('Are you sure you want to logout?')) {
        // Clear session data
        sessionStorage.removeItem('employeeData');
        
        // Redirect to login page
        window.location.href = 'login.html';
    }
}

// Add animation to stats on scroll
const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.animation = 'slideUp 0.6s ease';
        }
    });
});

document.querySelectorAll('.stat-card').forEach(card => {
    observer.observe(card);
});