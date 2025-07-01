# Punch API Project (Backend using C# .NET Web API)

---

## 📌 Project Overview

This is a **backend REST API project** developed with **C#** and **ASP.NET Core Web API**.
The project allows employees to **Punch In** and **Punch Out**, storing punch records in a **local JSON file (`punch_data.json`)**.

This project is ideal for **beginners** learning:

* REST API Development
* File I/O in C#
* ASP.NET Core Web API
* Testing APIs using Postman

---

## ✅ Features

| Feature              | Description                                                |
| -------------------- | ---------------------------------------------------------- |
| 🕒 Punch In API      | Records employee punch-in time                             |
| 🕔 Punch Out API     | Records employee punch-out time                            |
| 📄 JSON File Storage | Stores all punch data locally in a JSON file               |
| ✅ Input Validation   | Basic input validation for API requests                    |
| 🧪 Testable          | Fully testable using Postman or any other API testing tool |
| 📂 Simple Structure  | Beginner-friendly project folder and file structure        |

---

## ✅ Technologies Used

| Technology           | Purpose                   |
| -------------------- | ------------------------- |
| C#                   | Backend business logic    |
| ASP.NET Core Web API | API development framework |
| JSON                 | Data storage              |
| Visual Studio Code   | Development IDE           |
| Postman              | API testing tool          |

---

## ✅ API Endpoints

| HTTP Method | Endpoint         | Description        |
| ----------- | ---------------- | ------------------ |
| POST        | `/api/punch/in`  | Employee Punch In  |
| POST        | `/api/punch/out` | Employee Punch Out |
| GET         | `http://localhost:5000/api/punch`     | Retrieve all punch history |

---

## ✅ Running the Project Locally

| Step | Description                                                                 |
| ---- | --------------------------------------------------------------------------- |
| 1    | Open the project in **Visual Studio Code**                                  |
| 2    | Build and run the API using **dotnet run** or from the **VS Code Terminal** |
| 3    | Use **Postman** to test the API endpoints                                   |

---

## ✅ Sample API Endpoints for Postman Testing

| HTTP Method | Endpoint URL                          | Description        |
| ----------- | ------------------------------------- | ------------------ |
| POST        | `http://localhost:5000/api/punch/in`  | Punch In an employee |
| POST        | `http://localhost:5000/api/punch/out` | Punch Out an employee |
| GET         | `http://localhost:5000/api/punch`     | Retrieve all punch history |


> ℹ️ *Make sure the API server is running before testing with Postman.*

---

## ✅ Example JSON Output (`punch_data.json`)

```json
[
  {
    "PunchId": 1,
    "EmployeeId": 101,
    "PunchType": "In",
    "PunchTime": "2025-06-30T10:00:00"
  },
  {
    "PunchId": 2,
    "EmployeeId": 101,
    "PunchType": "Out",
    "PunchTime": "2025-06-30T18:00:00"
  }
]
```

---

