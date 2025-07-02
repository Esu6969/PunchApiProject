Employee Punch In/Out REST API (ASP.NET Core with SQLite)

Project Description

This is a production-style REST API designed to manage employee attendance records, specifically tracking punch-in and punch-out times. The backend is built with ASP.NET Core Web API and uses Entity Framework Core with SQLite as the local database provider. The project follows industry-standard practices for API design and data persistence.

Core Features

Record employee punch-in times with the current server timestamp.
Record punch-out times for a specific punch record by its unique ID.
Retrieve a complete list of all punch-in and punch-out records.
Persistent data storage using a lightweight SQLite database.
Full API tested with Postman, ensuring real-time CRUD operation handling.
Technologies Used

ASP.NET Core Web API
Entity Framework Core (Code First Approach with Migrations)
SQLite (Local Database)
Postman for API testing
.NET CLI for project build and migration management
API Endpoints Overview

The API exposes three primary endpoints:

Punch In:
Accepts an employee name and records their punch-in time.
Punch Out:
Accepts a punch record ID and updates the punch-out time for that specific record.
Get All Punch Records:
Returns all punch-in and punch-out entries stored in the database.
API Testing Workflow (Postman)

To test this API locally:

Start the API server by running:
dotnet run
For Punch In, send a POST request to: http://localhost:5031/api/punch/punchin
With the body containing the employee name as plain text.

For Punch Out, send a POST request to: http://localhost:5031/api/punch/punchout/{id}
Where {id} is the unique ID of the punch record.

To fetch all records, send a GET request to: http://localhost:5031/api/punch
Each API response returns JSON with timestamped punch data for easy verification.

Database Management (EF Core Migrations)

This project uses Entity Framework Core Code First Migrations to manage database schema updates.

After creating or modifying models, the following commands were used:

- dotnet ef migrations add InitialCreate

- dotnet ef database update

The SQLite database file punch_data.db is automatically created and updated in the project root.

Notes :- This project reflects real-world backend development workflows, including:

- API versioning and routing best practices
- Proper usage of dependency injection for database context
- Error handling for invalid or missing records
- Clean separation between Models, Data Context, and Controllers
- Consistent naming conventions and coding standards used in enterprise projects.
