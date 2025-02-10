# ToDo List Application

A full-stack ToDo List application built with .NET 8 Web API and React, following Clean Architecture principles.

## Features

### Backend (ASP.NET Core Web API)
- RESTful API endpoints for CRUD operations
- SQLite database with Entity Framework Core
- Clean Architecture layers (Core, Application, Infrastructure, API)
- Swagger/OpenAPI documentation
- Input validation and error handling
- Unit tests with xUnit and Moq

### Frontend (React)
- Interactive UI with React Hooks
- Create, Read, Update, Delete todos
- Real-time status updates
- Edit-in-place functionality
- Error handling with toast notifications
- Responsive design

## Tech Stack

**Backend**
- .NET 8
- Entity Framework Core
- SQLite
- AutoMapper
- xUnit (Testing)
- Swagger

**Frontend**
- React 18
- React Icons
- Axios
- React Toastify
- Bootstrap 5

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- SQLite

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/yourusername/todoapp.git
   cd todoapp

2. Backend Setup
   ```bash
    cd ToDoApp.API
    dotnet restore
    dotnet ef database update
    dotnet run

API will be available at http://localhost:5223
Swagger Docs: http://localhost:5223/swagger

3. FrontEnd Setup
   ```bash
    cd ToDoApp.Client
    npm install
    npm start
   
App will open at http://localhost:3000

Backend Tests
   ```bash
    cd ToDoApp.Tests
    dotnet test
