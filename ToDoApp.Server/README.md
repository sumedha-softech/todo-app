# ToDoApp.Server

**Note:**  
This README is for the **ToDoApp.Server** (.NET 8 Web API) project only.  
If you have a frontend (SPA or otherwise), it should be managed separately and is not included in this folder.

ToDoApp.Server is a .NET 8 Web API for managing to-do items. It exposes endpoints for creating, reading, updating, and deleting tasks, and is designed for integration with any frontend or client.

## Features

- **CRUD To-Do Items:**  
  Create, retrieve, update, and delete to-do items.

- **RESTful Endpoints:**  
  Follows REST conventions for easy integration.

- **Input Validation:**  
  Ensures to-do items meet required criteria.

- **Flexible Data Storage:**  
  Supports in-memory storage by default; can be extended for persistent storage.

- **Swagger/OpenAPI:**  
  Built-in API documentation for testing and exploration.

## Project Structure

```
ToDoApp.Server/
├── Controllers/
│   └── ToDoController.cs
├── Models/
│   └── ToDoItem.cs
├── Services/
│   └── ToDoService.cs
├── Program.cs
├── appsettings.json
└── README.md
```

- **Controllers:**  
  - `ToDoController`: Handles all to-do item API endpoints.

- **Services:**  
  - `ToDoService`: Business logic for managing to-do items.

- **Data Access:**  
  - `AppDbContext`: Entity Framework Core context for to-do items.

- **Models:**  
  - `ToDoItem`: Entity and DTO for to-do items.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### API Documentation

- Swagger UI is available at `/swagger` when running the application.

## Example Endpoints

- `GET /api/todo`  
  Retrieve all to-do items.

- `POST /api/todo`  
  Create a new to-do item.

- `PUT /api/todo/{id}`  
  Update an existing to-do item.

- `DELETE /api/todo/{id}`  
  Delete a to-do item.

## Customization

- To switch from in-memory to persistent storage, implement a data provider (e.g., using Entity Framework Core) and update the service registration in `Program.cs`.

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Swagger/OpenAPI

---

## License

This project is for educational and demonstration purposes.