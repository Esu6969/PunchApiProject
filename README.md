**Employee Punch API Project**
A full-stack employee attendance system built with ASP.NET Core, React, and PostgreSQL. Includes secure login, employee registration, punch-in/punch-out workflow, modern UI, automated database setup, and complete REST API with Swagger documentation. Production-ready architecture with DTOs, CORS, session support, and health checks.

🚀 Features
## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- PostgreSQL 14+ or SQL Server / LocalDB (default in `appsettings.json`)
- Node.js 18+ & npm
- Visual Studio Code or Visual Studio 2022 (optional)

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/Esu6969/PunchApiProject.git
   cd PunchApiProject
   ```

2. Configure Database
   - Create a PostgreSQL or SQL Server database
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
| Database   | PostgreSQL or SQL Server     |
| Tools      | Swagger, Session, CORS, Logging |

## Development Setup and Troubleshooting

This section explains how to run the project locally, configure the database connection, enable EF Core SQL logging for debugging, apply migrations, and useful API endpoints.

### Connection Strings

The application reads connection strings from `appsettings.json`. By default:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PunchInOut;Trusted_Connection=True;TrustServerCertificate=True"
}
```

You can also add a second connection string named `PunchInOutDataContextConnectionString` if you need a separate database.

### Apply Database Migrations

Recommended: use EF Core migrations so the schema is created and updated reliably.

- Create a migration (if required):
  - `dotnet ef migrations add InitialCreate`
- Apply migrations:
  - `dotnet ef database update`

Alternatively, the application may call `Database.EnsureCreated()` or `Database.Migrate()` at startup to create or migrate the database automatically.

### Enable EF Core SQL Logging (Development Only)

To see SQL statements EF Core executes and (optionally) parameter values, enable logging in `Program.cs`:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PunchDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
           .LogTo(Console.WriteLine, LogLevel.Information));
```

- `EnableSensitiveDataLogging()` prints parameter values — enable only in development.
- `LogTo(Console.WriteLine, LogLevel.Information)` writes EF SQL to console and Visual Studio Output window when running the app.
- Optionally set `Microsoft.EntityFrameworkCore.Database.Command` to `Information` in `appsettings.json` to show DB commands.

### Troubleshooting Tips

- If you don’t see rows in the database, verify the runtime connection string (log it at startup) and check API responses for errors.
- The `PunchService` methods expect the integer primary key `Id`. The frontend may send the string `EmployeeId` (e.g. `"EMP001"`). The project includes convenience endpoints `POST /api/punch/in/by-employeeid` and `/api/punch/out/by-employeeid` that resolve the string to the integer `Id` server-side.
- Check browser developer tools (Network tab) to inspect request payloads and API responses.

### Useful Endpoints

- `POST /api/auth/login` — returns `id` (integer) and `employeeId` (string)
- `POST /api/punch/in` — accepts JSON `{ "employeeId": <integer> }`
- `POST /api/punch/in/by-employeeid` — accepts JSON `{ "employeeId": "EMP001" }`
- `POST /api/punch/out` and `POST /api/punch/out/by-employeeid` — analogous to punch in
- `GET /api/punch/records/{employeeId}` — returns records for integer `employeeId`
- `GET /api/employee` — list employees

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









