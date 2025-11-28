**Employee Punch API Project**
A full-stack employee attendance system built with ASP.NET Core, React, and PostgreSQL. Includes secure login, employee registration, punch-in/punch-out workflow, modern UI, automated database setup, and complete REST API with Swagger documentation. Production-ready architecture with DTOs, CORS, session support, and health checks.

🚀 Features

🔹**Employee Management**

- Employee registration with name, email, department, position, join date, and more.
- Duplicate Employee ID & Email validation.
- Password hashing for secure storage.

🔹**Authentication**

- Login using Employee ID or Email (string-based ID support).
- JWT authentication setup for secure access (extensible for protected routes).
- Session support added for state management.

🔹 **Punch In / Punch Out System**

- Secure punch-in and punch-out functionality.
- Timer on dashboard to track working duration.
- Punch-out summary with total hours.
- Real-time feedback messages.

🔹 **Dashboard**

- View employee profile, company details, and work status.
- Clean, modern, and responsive design optimized for desktop/laptop.

🔹**Backend Enhancements**

- RESTful API endpoints for registration, login, punch actions, and record retrieval.
- Automatic table creation on startup using EnsureCreated().
- Improved error handling and logging.
- CORS enabled for multiple frontend origins.
- Swagger UI for API documentation (/swagger).
- Health check endpoint (/health).

🔹 **Frontend Enhancements**

- Redesigned Login & Registration pages with a professional UI.
- Form validation with clear error messaging.
- Routing fixes using React Router.
- API integration aligned with backend DTOs.
- Responsive layout for all screens.

## 🧰 Tech Stack

| Layer      | Technologies                 |
|------------|------------------------------|
| Frontend   | React, JavaScript, CSS       |
| Backend    | ASP.NET Core (C#)            |
| Database   | PostgreSQL                   |
| Tools      | Swagger, Session, CORS, Logging |


**📝 Development Notes**

- CORS supports multiple frontend URLs.
- Automatic database table creation with EnsureCreated().
- Session enabled for user state management.
- Logging added for startup, registration, login, and errors.
- SPA fallback route serves login.html for React routing support.
- DTOs for Login & Registration are fully aligned with frontend.

**🧑‍💻 Future Enhancements**

- Complete JWT-based role protection for routes.
- Admin dashboard for employee management.
- Export punch data (PDF/Excel).
- Analytics dashboard for HR (working hours, late logs, etc.).
- Email notifications for punch reminders.

**License**

- MIT License — free to use, modify, and distribute.





