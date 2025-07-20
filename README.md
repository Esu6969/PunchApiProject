# 👨‍💼 Employee Punch In/Out API (ASP.NET Core with PostgreSQL)

This is a production-ready REST API built with ASP.NET Core Web API that allows employees to punch in and punch out, while tracking their working hours with accurate timestamps. The backend uses Entity Framework Core with a PostgreSQL database, designed for scalable, secure, and maintainable deployments.

The architecture follows clean code principles, with a clear separation of concerns using:

- ✅ Models for representing the database structure
- ✅ DTOs for secure and clean data transfer
- ✅ Services for encapsulating business logic
- ✅ Controllers for handling API routes and responses
  
This API is fully tested using Postman, includes proper error handling, input validation, Swagger UI, and is structured to be maintainable, extensible, and deployable.

---

🚀 Core Features

- ✅ Register/Login (JWT)
- ✅ Punch In / Punch Out
- ✅ View All Punch Records
- ✅ Filter Records by Date
- ✅ Calculate Total Hours Worked
- ✅ Get Full Activity (Authenticated)
- ✅ Secure JWT-based Authorization
- ✅ Swagger UI for documentation
- ✅ PostgreSQL for persistent storage
  
---
**UPDATES**
## ✅ Project Features & Fixes

| Feature / Fix                                  | Description                                                                 |
|------------------------------------------------|-----------------------------------------------------------------------------|
| 🔄 Switched from SQLite to PostgreSQL          | Fully migrated using EF Core                                                |
| 🔐 PostgreSQL Permission Fixes                 | Created custom user `Dhruvil` and granted full privileges to run EF migrations |
| 🛠️ EF Core Migration Working with PostgreSQL  | Enabled `dotnet ef database update` using correct ownership and sequence grants |
| 🔑 JWT Authentication Added                    | Secured routes using `[Authorize]` attribute and token-based login         |
| 🔐 Secure Endpoints                            | Punching and employee activity endpoints now require Bearer token          |
| 🧪 Postman Testing Verified                    | All endpoints tested using Postman with appropriate headers and tokens     |
| 📤 Added Endpoint for `GET /api/employee/all-activity` | Returns full employee punch-in/out activity                          |
| 🗂️ Refactored Controllers                      | Punch logic is separated from authentication logic                         |


---
**🔐 Authentication Flow (JWT)**

	1.	Register via POST /api/employee/register
	2.	Login via POST /api/employee/login
	3.	Receive JWT Token
	4.	Use the token in Authorization header: Authorization: Bearer <your_token_here>

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
  
**## 📌 API Endpoints**

| Method | Endpoint                                | Description                          | Auth Required |
|--------|-----------------------------------------|--------------------------------------|---------------|
| POST   | `/api/employee/register`                | Register a new user                  | ❌            |
| POST   | `/api/employee/login`                   | Login and receive JWT token          | ❌            |
| POST   | `/api/punch/punchin`                    | Punch in                             | ✅            |
| POST   | `/api/punch/punchout/{id}`              | Punch out using punch record ID      | ✅            |
| GET    | `/api/punch`                            | Get all punch records                | ✅            |
| GET    | `/api/punch/filter?date=YYYY-MM-DD`     | Filter records by date               | ✅            |
| GET    | `/api/punch/totalhours/{id}`            | Calculate total hours worked         | ✅            |
| GET    | `/api/employee/all-activity`            | View all activity by all employees   | ✅            |

---

**🛠️ Tools & Technologies**

-	ASP.NET Core Web API (.NET 7+)
-	Entity Framework Core (Code-First)
-	PostgreSQL
-	Postman (Testing)
-	Swagger (Auto-generated Docs)
-	JWT Authentication
-	.NET CLI (dotnet ef, dotnet run, dotnet build)

---
