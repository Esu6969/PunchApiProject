📌 Project Description

This is a production-style REST API designed to manage employee attendance records by tracking punch-in and punch-out times. The backend is developed using ASP.NET Core Web API and leverages Entity Framework Core with SQLite as the local database provider.The project reflects clean code practices and real-world backend workflows suitable for professional environments.

✅ Core Features

- Record employee punch-in times with the current server timestamp
- Record punch-out times using a specific punch record ID
- Retrieve a complete list of punch-in and punch-out records
- Persistent data storage using a lightweight SQLite database
- Fully tested using Postman with real-time CRUD operation handling
- Clean separation of Controllers, Services, DTOs, and Models

🛠 Technologies Used

- ASP.NET Core Web API
- Entity Framework Core (Code First with Migrations)
- SQLite (Lightweight local database)
- Postman for API testing
- .NET CLI for build, run, and migration commands
  
🔗 API Endpoints Overview

1. Punch In - POST http://localhost:5031/api/punch/punchin
   
2. Punch Out
POST http://localhost:5031/api/punch/punchout/{id}
Updates punch-out time by record ID

3. Get All Records
GET http://localhost:5031/api/punch
Returns all stored punch-in/out entries

4. Filter by Date
GET http://localhost:5031/api/punch/filter?date=2025-07-07

5. Get Total Hours Worked
GET http://localhost:5031/api/punch/totalhours/{id}



🧪 API Testing Workflow (Postman)

To test the API locally:

- Start the API server:
- dotnet run
- For Punch In, Send a POST request to :-  http://localhost:5031/api/punch/punchin

- For Punch Out:
  Send a POST request to :- http://localhost:5031/api/punch/punchout/{id}

- For All Records:
  Send a GET request to :- http://localhost:5031/api/punch

All responses are returned in clean JSON format with real-time timestamps.

🗃️ Database Management (EF Core Migrations)

This project uses Entity Framework Core Code First for managing schema changes.

To apply migrations and update the database, use:

dotnet ef migrations add InitialCreate
dotnet ef database update
SQLite will automatically create and update the file: punch_data.db in the root directory.

🧠 Notes

This project demonstrates real-world backend development practices, including:

- Proper usage of dependency injection for the database context
- Error handling for invalid or missing punch records
- Clean separation of concerns between Models, DTOs, Services, and Controllers
- Well-organized folder structure and naming conventions
- Designed to be easily extended with authentication, logging, data export, etc.

