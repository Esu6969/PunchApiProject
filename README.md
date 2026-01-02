**Employee Punch API Project**
A full-stack employee attendance system built with ASP.NET Core, React, and PostgreSQL. Includes secure login, employee registration, punch-in/punch-out workflow, modern UI, automated database setup, and complete REST API with Swagger documentation. Production-ready architecture with DTOs, CORS, session support, and health checks.

🚀 Features
## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL 14+
- Node.js 18+ & npm
- Visual Studio Code or Visual Studio 2022

### Installation

1. Clone the repository
```bash
   git clone https://github.com/Esu6969/PunchApiProject.git
   cd PunchApiProject
```

2. Configure Database
   - Create a PostgreSQL database
   - Copy `appsettings.example.json` to `appsettings.json`
   - Update connection string with your credentials

3. Install Backend Dependencies
```bash
   dotnet restore
   dotnet ef database update
```

4. Install Frontend Dependencies
```bash
   cd frontend
   npm install
```

5. Run the Application
```bash
   # Terminal 1 - Backend
   dotnet run

   # Terminal 2 - Frontend
   cd frontend
   npm start
```

6. Access the Application
   - Frontend: http://localhost:3000
   - API: http://localhost:5000
   - Swagger: http://localhost:5000/swagger
     
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


**🔌 API Endpoints**

### Authentication
- `POST /api/auth/register` - Register new employee
- `POST /api/auth/login` - Employee login

### Punch Operations
- `POST /api/punch/in` - Punch in
- `POST /api/punch/out` - Punch out
- `GET /api/punch/records` - Get punch records

### Employee
- `GET /api/employee/{id}` - Get employee details
- `PUT /api/employee/{id}` - Update employee info

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





