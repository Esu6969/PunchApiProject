# Employee Punch API Project

A full-stack employee punch-in/punch-out and registration system built with React (frontend), ASP.NET Core (backend), and PostgreSQL (database).

## Features

- Employee registration with name and email
- Secure punch-in and punch-out functionality
- Real-time feedback and greeting messages after registration and punch actions
- Dashboard for employees to view their profile, company details, and work status
- Timer on the dashboard to track work duration, with punch-out summary
- Responsive, modern UI with professional design and color scheme
- RESTful API endpoints for registration, punch actions, and record retrieval
- CORS enabled for seamless frontend-backend communication
- Error handling and validation for all user actions
- JWT authentication setup for secure login and protected routes (extensible)
- Swagger UI for API documentation (`/swagger`)
- Health check endpoint (`/health`)
- Automatic database migrations on startup

## Tech Stack

- **Frontend:** React, JavaScript, CSS
- **Backend:** ASP.NET Core (C#)
- **Database:** PostgreSQL

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/punch-api-project.git
   cd punch-api-project
   ```

2. Update your database connection strings in `appsettings.json`.

3. Run the backend:
   ```
   dotnet run
   ```

4. Run the frontend:
   ```
   cd frontend
   npm install
   npm start
   ```

5. Access the app:
   - API Swagger: [http://localhost:5031/swagger](http://localhost:5031/swagger)
   - Frontend: [http://localhost:3000](http://localhost:3000)

## API Endpoints

- `POST /api/punch/register` — Register a new employee
- `POST /api/punch/in` — Punch in
- `POST /api/punch/out` — Punch out
- `GET /api/punch/records` — Get punch records
- `GET /health` — Health check

## Development Notes

- CORS is enabled for local frontend development.
- Automatic database migrations are applied on backend startup.
- Session and static file support are enabled.
- Fallback route serves `login.html` for SPA support.

##
