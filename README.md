# 👨‍💼 Employee Punch In/Out API (ASP.NET Core with PostgreSQL)

This is a production-ready REST API developed using ASP.NET Core Web API, designed to track employee attendance through punch-in and punch-out timestamps. The system leverages Entity Framework Core with PostgreSQL for efficient and scalable data persistence — making it suitable for both development and real-world deployments.

The architecture follows clean code principles, with a clear separation of concerns using:

- ✅ Models for representing the database structure
- ✅ DTOs for secure and clean data transfer
- ✅ Services for encapsulating business logic
- ✅ Controllers for handling API routes and responses
  
This API is fully tested using Postman, includes proper error handling, input validation, Swagger UI, and is structured to be maintainable, extensible, and deployable.

---

🚀 Core Features

- ✅ Punch In (records employee name and punch-in time)
- ✅ Punch Out (updates punch-out time based on ID)
- ✅ Get All Punch Records
- ✅ Filter Records by Date
- ✅ Calculate Total Hours Worked
- ✅ PostgreSQL database for persistent and scalable storage
- ✅ Swagger documentation for easy testing
- ✅ DTOs and Clean Architecture for security and clarity

---
**UPDATE**

- ✅ Switched Database: Migrated from SQLite to PostgreSQL using Entity Framework Core.Connection configured via appsettings.json.

- ✅ Verified End-to-End Flow: Successfully tested all API routes (/punchin, /punchout, /filter, /totalhours) using Postman.Confirmed that data is being stored in PostgreSQL.

- ✅ Verified Data in PostgreSQL:

     Connected to the PostgreSQL database using: **psql -U postgres -d punchdb**

     Viewed punch records directly from the database using SQL: **SELECT * FROM "PunchRecords";**

---

**🧠 Project Structure Overview**

This project follows a clean and modular folder structure inspired by industry best practices in backend development using ASP.NET Core. Here's what each folder and file is responsible for:

- ✅ **Controllers/**
     PunchController.cs: Routes HTTP requests like PunchIn, PunchOut, GetAllPunches to the Service Layer.
  
- ✅ **Services/**
     IPunchServices.cs: Interface defining service contracts.
     PunchServices.cs: Implements methods like PunchIn, PunchOut, filtering, and total hours logic.
  
- ✅ **Models/**
     PunchRecord.cs: Entity class representing a punch record used by EF Core to generate tables.
  
- ✅ **DTOs/**
     PunchRequestDTO.cs: Accepts clean input from users to ensure secure and clean operations.
  
- ✅ **Data/**
     AppDbContext.cs: Connects the PunchRecord model to a PostgreSQL database using EF Core.
  
- ✅ **Migrations/**
     Contains schema change history using EF Core dotnet ef tools.
  
- ✅ **Properties/**
     Contains metadata like Swagger and launch settings for development tools.
  
- ✅ **Project Root Files**
  

**File Name	Description**

| File Name                | Description                                                  |
|--------------------------|--------------------------------------------------------------|
| `Program.cs`             | Registers services, DB context, and launches the app.        |
| `appsettings.json`       | Contains PostgreSQL connection string and config settings.   |
| `PunchApiProject.csproj` | Lists all dependencies and project settings.                 |
| `PunchApiProject.sln`    | Solution file for organizing the project.                    |
| `PunchApiProject.http`   | Enables direct API testing inside Visual Studio Code.        |

**⚙️ Technology Stack**

- ASP.NET Core Web API
- Entity Framework Core (Code First)
- PostgreSQL (replaces SQLite)
- Postman (API testing)
- Swagger (API documentation)
- .NET CLI (dotnet ef, dotnet run, dotnet build)
  
**🔗 API Endpoints**

| Method | Endpoint                                | Description                     |
|--------|-----------------------------------------|---------------------------------|
| POST   | `/api/punch/punchin`                    | Punch in employee               |
| POST   | `/api/punch/punchout/{id}`              | Punch out employee by ID        |
| GET    | `/api/punch`                            | Get all punch records           |
| GET    | `/api/punch/filter?date=YYYY-MM-DD`     | Filter records by specific date |
| GET    | `/api/punch/totalhours/{id}`            | Calculate total hours worked    |

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
