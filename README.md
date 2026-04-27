# Library Book Borrowing System

A RESTful API for managing a library's book inventory, members, and borrowing records built with ASP.NET Core 10 and SQLite.

## Features

- Full CRUD for Books and Members
- Borrow and return books with availability tracking
- Optimistic concurrency control on book copies (prevents double-borrowing)
- In-memory caching for read-heavy endpoints
- Global error handling middleware with structured JSON error responses
- OpenAPI (Swagger) support in development

## Tech Used

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 (Minimal Hosting) |
| ORM | Entity Framework Core 10 |
| Database | SQLite |
| Caching | IMemoryCache (in-process) |
| API Docs | Microsoft.AspNetCore.OpenApi |

## Project Structure

```
LibrarySystem/
├── Controllers/         # HTTP endpoints (Books, Members, Borrows)
├── Services/            # Business logic
├── Repositories/        # Data access layer
├── Models/              # EF Core entity models
├── DTOs/                # Request/response shapes
├── Data/                # AppDbContext
├── Middleware/          # Global error handling
└── Migrations/          # EF Core migrations
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run locally

```bash
git clone https://github.com/[YOUR USERNAME]/Library_Book_Borrowing_System.git
cd Library_Book_Borrowing_System/LibrarySystem

dotnet ef database update
dotnet run
```

The API starts at `https://localhost:{port}`.

## API Reference

### Books — `/api/books`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/books` | Get all books |
| GET | `/api/books/{id}` | Get a book by ID |
| POST | `/api/books` | Add a new book |
| PUT | `/api/books/{id}` | Update a book |
| DELETE | `/api/books/{id}` | Delete a book |

**Book request body**
```json
{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "9780132350884",
  "totalCopies": 3,
  "availableCopies": 3
}
```

### Members — `/api/members`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/members` | Get all members |
| GET | `/api/members/{id}` | Get a member by ID |
| POST | `/api/members` | Register a new member |
| PUT | `/api/members/{id}` | Update a member |
| DELETE | `/api/members/{id}` | Remove a member |

**Member request body**
```json
{
  "fullName": "Jane Doe",
  "email": "jane@example.com"
}
```

### Borrows — `/api/borrows`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/borrows` | Get all borrow records |
| GET | `/api/borrows/{id}` | Get a borrow record by ID |
| POST | `/api/borrows` | Borrow a book |
| PUT | `/api/borrows/{id}/return` | Return a borrowed book |

**Borrow request body**
```json
{
  "bookId": 1,
  "memberId": 2
}
```

**Borrow response**
```json
{
  "id": 5,
  "bookId": 1,
  "bookTitle": "Clean Code",
  "memberId": 2,
  "memberName": "Jane Doe",
  "borrowDate": "2026-04-26T14:00:00Z",
  "returnDate": null,
  "status": "Borrowed"
}
```

## Error Handling

All errors return a consistent JSON shape:

```json
{ "error": "No available copies of this book." }
```

| Scenario | Status Code |
|---|---|
| Invalid input / business rule violation | `400 Bad Request` |
| Resource not found | `404 Not Found` |
| Unhandled server error | `500 Internal Server Error` |

## Concurrency

Book availability is protected with an optimistic concurrency token (`RowVersion`). If two requests attempt to borrow the last copy simultaneously, one will receive a `400` with the message:

> "Another request just borrowed the last copy. Please try again."
