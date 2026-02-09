**⏱️ Employee Punch API Project**

A full-stack employee attendance management system built using ASP.NET Core, React, and PostgreSQL.
This project provides secure authentication, employee management, punch-in/punch-out tracking, and a modern responsive UI with production-ready backend architecture.

📌 **Overview**

The Employee Punch System helps organizations track employee working hours with secure login, real-time punch tracking, and centralized record management.

It includes:

- Secure employee authentication

- Employee profile management

- Punch In / Punch Out workflow

- Real-time working timer

- RESTful API with Swagger documentation

- Automated database setup

- Production-ready backend architecture

  
| Layer    | Technology                                      |
| -------- | ----------------------------------------------- |
| Frontend | React, JavaScript, CSS                          |
| Backend  | ASP.NET Core (.NET 8, C#)                       |
| Database | PostgreSQL / Ms-Sql Server                                     |
| Tools    | Swagger, CORS, Sessions, Logging, Health Checks |




**🚀 Features**
**👨‍💼 Employee Management**

- Employee registration with:

- Name

- Email

- Department

- Position

- Join Date

- Duplicate Employee ID & Email validation

- Secure password hashing



**🔐 Authentication**

- Login using:

- Employee ID

- Email

- JWT authentication ready (extendable for protected routes)

- Session-based state management support



**⏱️ Punch In / Punch Out System**

- Secure punch-in and punch-out

- Live working duration timer

- Punch-out summary with total work hours

- Real-time dashboard feedback



**📊 Dashboard**

- Employee profile overview

- Company and work status display

- Modern responsive UI (Desktop optimized)



**⚙️ Backend Enhancements**

- RESTful API architecture

- DTO-based request/response handling

- Automatic database table creation using EnsureCreated()

- Centralized error handling & logging

- Multi-origin CORS support

- Swagger API documentation

- Health check endpoint




**🎨 Frontend Enhancements**

- Professional Login & Registration UI

- Form validation with clear error messages

- React Router navigation fixes

- DTO-aligned API integration

- Fully responsive layout


**🚀 Getting Started**
**✅ Prerequisites**

- .NET 8 SDK

- PostgreSQL 14+

- Node.js 18+

- npm

- Visual Studio 2022 or VS Code
  

| Service     | URL                                                            |
| ----------- | -------------------------------------------------------------- |
| Frontend    | [http://localhost:3000](http://localhost:3000)                 |
| Backend API | [http://localhost:5000](http://localhost:5000)                 |
| Swagger     | [http://localhost:5000/swagger](http://localhost:5000/swagger) |


**🔌 API Endpoints**

**Authentication**
- POST /api/auth/register
- POST /api/auth/login

**Punch Operations**
- POST /api/punch/in
- POST /api/punch/out
- GET  /api/punch/records

**Employee**
- GET /api/employee/{id}
- PUT /api/employee/{id}

**🧾 Development Notes**

- Supports multiple frontend URLs via CORS

- Automatic DB creation on startup

- Session enabled for user state

- Structured logging for:

- Startup

- Authentication

- Errors

- SPA fallback routing for React

- DTOs fully aligned with frontend models

**🔮 Future Enhancements**

- Role-based JWT authorization

- Admin dashboard

- Punch data export (PDF / Excel)

- HR Analytics dashboard

- Email notification system

**📄 License**

- MIT License — Free to use, modify, and distribute.

**👨‍💻 Author**

- Developed by Dhruvil
