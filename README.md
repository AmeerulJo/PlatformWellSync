# Platform Well Synchronization Application

## Overview

This application synchronizes Platform and Well data from the Aemenersol REST API into a local SQL Server database.

The application performs the following tasks:

* Authenticates using the Login API.
* Retrieves a Bearer Token.
* Calls the GetPlatformWellActual API endpoint.
* Stores Platform data into the Platform table.
* Stores Well data into the Well table.
* Performs Insert or Update (Upsert) operations based on record Id.
* Handles missing JSON properties and additional JSON properties without breaking.
* Uses Entity Framework Core Code First approach.
* Uses SQL Server LocalDB as the database.

---

## Technology Stack

| Technology            | Version                                  |
| --------------------- | ---------------------------------------- |
| .NET                  | 8                                        |
| Entity Framework Core | 8                                        |
| SQL Server            | LocalDB                                  |
| C#                    | 12                                       |
| HttpClient            | Built-in                                 |
| Dependency Injection  | Microsoft.Extensions.DependencyInjection |

---

## Solution Structure

```text
PlatformWellSync
│
├── docs
│   └── TechnicalDesignDocument.docx
│
├── PlatformWellSync.Console
│   ├── Data
│   ├── Models
│   ├── Services
│   ├── Program.cs
│   ├── appsettings.json
│   └── PlatformWellSync.Console.csproj
│
├── PlatformWellSync.sln
├── README.md
└── .gitignore
```

---

## Prerequisites

Before running the application, ensure the following are installed:

* .NET 8 SDK
* SQL Server LocalDB
* Visual Studio 2022 (or later)

Verify installation:

```bash
dotnet --version
```

---

## API Information

### Login Credentials

```text
Username: user@aemenersol.com
Password: Test@123
```

### API Documentation

Swagger:

http://test-demo.aemenersol.com/index.html

Postman Collection:

https://www.getpostman.com/collections/ed81f3774c5e051c89a9

---

## Database Configuration

Connection string is configured in:

```json
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PlatformWellDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## Setup Instructions

### 1. Clone Repository

```bash
git clone <repository-url>
cd PlatformWellSync
```

### 2. Restore Packages

```bash
dotnet restore
```

### 3. Create Database Migration

```bash
dotnet ef migrations add InitialCreate
```

### 4. Update Database

```bash
dotnet ef database update
```

### 5. Run Application

```bash
dotnet run --project PlatformWellSync.Console
```

---

## Application Workflow

```text
Login API
    │
    ▼
Retrieve Bearer Token
    │
    ▼
GetPlatformWellActual
    │
    ▼
Deserialize Response
    │
    ▼
Upsert Platform Records
    │
    ▼
Upsert Well Records
    │
    ▼
Save To Database
```

---

## Upsert Logic

### Insert

If record Id does not exist:

* Insert Platform
* Insert Well

### Update

If record Id exists:

* Update Platform
* Update Well

---

## Handling Dynamic API Responses

The application is designed to tolerate API schema changes.

### Supported Scenarios

#### Missing Property

```json
{
  "id": 1,
  "platformName": "Platform A"
}
```

Application continues successfully.

#### Additional Property

```json
{
  "id": 1,
  "platformName": "Platform A",
  "newProperty": "ignored"
}
```

Application ignores unknown properties.

### Implementation

* Nullable DTO properties
* System.Text.Json deserialization
* Unknown properties ignored automatically

---

## Entity Framework Core

This project uses the Code First approach.

### Migration Command

```bash
dotnet ef migrations add InitialCreate
```

### Database Update

```bash
dotnet ef database update
```

### Automatic Migration

Application startup executes:

```csharp
await db.Database.MigrateAsync();
```

to ensure the database schema is up to date.

---

## Tables

### Platform

| Column       | Type     |
| ------------ | -------- |
| Id           | int      |
| PlatformName | nvarchar |
| CreatedDate  | datetime |
| UpdatedDate  | datetime |

### Well

| Column      | Type     |
| ----------- | -------- |
| Id          | int      |
| PlatformId  | int      |
| WellName    | nvarchar |
| CreatedDate | datetime |
| UpdatedDate | datetime |

---

## Assumptions

* Platform Id is unique.
* Well Id is unique.
* API credentials remain valid.
* LocalDB is installed on the target machine.
* API availability is outside application control.

---

## Future Improvements

Possible enhancements include:

* Serilog logging
* Retry policy using Polly
* Unit tests using xUnit
* Repository pattern
* Scheduled synchronization using Hangfire or Windows Task Scheduler
* Docker support

---

## Author

Technical Assessment Submission

Platform Well Synchronization Application

.NET 8 + Entity Framework Core + SQL Server LocalDB
