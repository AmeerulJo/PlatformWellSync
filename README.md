# Platform Well Synchronization Application

## 1. Overview

### Objective

Develop a .NET application that synchronizes Platform and Well data from a REST API into a local SQL Server database.

### Requirements

* Authenticate using Login API.
* Retrieve bearer token.
* Call GetPlatformWellActual endpoint.
* Store Platform data into Platform table.
* Store Well data into Well table.
* Perform Insert or Update based on record Id.
* Handle missing fields and additional fields without application failure.
* Use SQL Server LocalDB.
* Use Entity Framework Core Code First approach.

---

## 2. Technology Stack

| Component            | Technology                               |
| -------------------- | ---------------------------------------- |
| Language             | C#                                       |
| Framework            | .NET 8                                   |
| Database             | SQL Server LocalDB                       |
| ORM                  | Entity Framework Core 8                  |
| HTTP Client          | HttpClient                               |
| Dependency Injection | Microsoft.Extensions.DependencyInjection |
| Configuration        | appsettings.json                         |

---

## 3. Application Architecture

### Flow

1. Login to API
2. Retrieve JWT Bearer Token
3. Call GetPlatformWellActual
4. Deserialize API Response
5. Upsert Platform Records
6. Upsert Well Records
7. Save Changes to Database

### Architecture Diagram

API
↓
Authentication
↓
Bearer Token
↓
GetPlatformWellActual
↓
DTO Mapping
↓
Business Logic
↓
Entity Framework Core
↓
SQL Server LocalDB

---

## 4. Database Design

### Platform Table

| Column       | Type          |
| ------------ | ------------- |
| Id           | int (PK)      |
| PlatformName | nvarchar(200) |
| CreatedDate  | datetime      |
| UpdatedDate  | datetime      |

### Well Table

| Column      | Type          |
| ----------- | ------------- |
| Id          | int (PK)      |
| PlatformId  | int (FK)      |
| WellName    | nvarchar(200) |
| CreatedDate | datetime      |
| UpdatedDate | datetime      |

### Relationship

Platform (1) → (Many) Wells

---

## 5. Entity Framework Code First

The database schema is generated using Entity Framework Core migrations.

Migration Command:

dotnet ef migrations add InitialCreate

Database Creation:

dotnet ef database update

Automatic migration execution is performed during application startup.

---

## 6. Synchronization Logic

### Insert Logic

If record Id does not exist:

* Create new Platform record.
* Create new Well record.

### Update Logic

If record Id exists:

* Update Platform properties.
* Update Well properties.

### Pseudocode

For each Platform:
If Platform exists:
Update Platform
Else:
Insert Platform

```
For each Well:
    If Well exists:
        Update Well
    Else:
        Insert Well
```

SaveChanges()

---

## 7. Error Handling

### API Errors

* HTTP status validation
* Unauthorized access handling
* Retry-ready architecture

### Database Errors

* Entity validation
* Foreign key integrity

### Logging

Application logs:

* Login Success
* API Call Success
* Platform Records Processed
* Well Records Processed
* Synchronization Completion

---

## 8. Dynamic JSON Handling

Requirement 7 states that the application must tolerate missing fields and additional fields.

Implementation:

* Nullable DTO properties.
* Case-insensitive JSON deserialization.
* Unknown JSON properties ignored automatically.

Example:

Original JSON

{
"id": 1,
"platformName": "Platform A"
}

Modified JSON

{
"id": 1,
"platformName": "Platform A",
"newProperty": "ignored"
}

Result:

Application continues processing successfully.

---

## 9. Project Structure

PlatformWellSync
│
├── Data
├── Models
├── Services
├── appsettings.json
├── Program.cs
└── PlatformWellSync.Console.csproj

---

## 10. Build Instructions

Restore Packages

dotnet restore

Create Migration

dotnet ef migrations add InitialCreate

Update Database

dotnet ef database update

Run Application

dotnet run

---

## 11. Assumptions

* API credentials remain valid.
* Platform Id is unique.
* Well Id is unique.
* SQL Server LocalDB is installed.
* API endpoint availability is outside application control.

---

## 12. Conclusion

The solution successfully fulfills all assessment requirements by:

* Authenticating against the API.
* Synchronizing Platform and Well data.
* Supporting insert and update operations.
* Using Entity Framework Core Code First.
* Handling missing and additional JSON fields safely.
* Persisting data in SQL Server LocalDB.
