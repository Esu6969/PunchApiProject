# 👨‍💼 Employee Punch In/Out API (ASP.NET Core with SQLite)

This is a production-ready REST API developed using ASP.NET Core Web API, designed to track employee attendance through punch-in and punch-out timestamps. The system leverages Entity Framework Core with SQLite for efficient, lightweight, and persistent local data storage — ideal for development and small-scale deployments.

The architecture follows clean code principles, with a clear separation of concerns using:

Models for representing the database structure,
DTOs for secure and clean data transfer,
Services for encapsulating business logic, and
Controllers for handling API routes and responses.
This API is fully tested using Postman, includes proper error handling, input validation, and is structured to be easily maintainable, extensible, and deployable in real-world scenarios.

---

## 🚀 Core Features

- ✅ Punch In (records employee name and punch-in time)
- ✅ Punch Out (updates punch-out time based on ID)
- ✅ Get All Punch Records
- ✅ Filter Records by Date
- ✅ Calculate Total Hours Worked
- ✅ SQLite database for persistent storage
- ✅ Swagger documentation for easy testing

---

## 🧠 Project Structure Overview

This project follows a clean and modular folder structure inspired by industry best practices in backend development using ASP.NET Core. Here's what each folder and file is responsible for:

✅ Controllers/ :- Contains the API endpoints logic.
- PunchController.cs: Routes HTTP requests like PunchIn, PunchOut, GetAllPunches to the Service Layer. It’s the only class directly exposed to the outside world (like Postman, Swagger, etc.).

✅ Services/
Handles the core business logic of the application.
- IPunchServices.cs: Interface that defines the contract for the service.
- PunchServices.cs: Implements all methods like PunchIn, PunchOut, filter by date, and calculate total hours. Also handles interaction with the database context.

✅ Models/ :- Defines your application’s data structure.
- PunchRecord.cs: Entity class representing a punch entry. Used directly by Entity Framework Core to create database tables.

✅ DTOs/ :- Encapsulates data passed in HTTP requests and responses to avoid exposing internal models.
- PunchRequestDTO.cs: Used to accept only required fields (like EmployeeName) from users, keeping your Models secure and clean.

✅ Data/:- Holds the database context and configurations.
- AppDbContext.cs: Inherits from DbContext and connects your model (PunchRecord) to a local SQLite database using Entity Framework Core.

✅ Migrations/ :- Contains auto-generated database schema change scripts.
- Created using dotnet ef migrations add <name> command and used to update the SQLite database structure.

✅ Properties/:- Contains metadata like launch settings for development tools (e.g., IIS settings, Swagger, etc.). Mostly auto-managed.

✅ Project Root Files

| File Name               | Description                                                                 |
|-------------------------|-----------------------------------------------------------------------------|
| `Program.cs`            | Entry point of the application. Registers services, DB context, and starts the app. |
| `appsettings.json`      | Configuration file for environment settings (e.g., DB connection string).  |
| `PunchApiProject.csproj`| Project definition file with references to packages like EF Core, Swagger. |
| `PunchApiProject.sln`   | Solution file that binds together all projects (.csproj files).             |
| `punch_data.db`         | Actual SQLite database file used for storing punch records.                |
| `PunchApiProject.http`  | Lets you test API endpoints directly in VS Code.                           |

---

## ⚙️ Technology Stack

- ASP.NET Core Web API
- Entity Framework Core (Code First)
- SQLite (local database)
- Postman (API testing)
- Swagger (API documentation)
- .NET CLI (`dotnet ef`, `dotnet run`)

---

## 🔗 API Endpoints

| Method | Endpoint                            | Description                     |
|--------|-------------------------------------|---------------------------------|
| POST   | `/api/punch/punchin`                | Punch in employee               |
| POST   | `/api/punch/punchout/{id}`          | Punch out employee by ID        |
| GET    | `/api/punch`                        | Get all punch records           |
| GET    | `/api/punch/filter?date=YYYY-MM-DD` | Filter records by specific date |
| GET    | `/api/punch/totalhours/{id}`        | Calculate total hours worked    |

---
