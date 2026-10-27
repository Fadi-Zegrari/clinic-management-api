# Clinic System API

A RESTful Web API for managing clinic operations, developed using ASP.NET Core Web API and SQL Server.

## Project Overview

The Clinic System API is a project designed to manage clinic-related data and operations. It provides endpoints for working with different clinic entities through a structured three-tier architecture.

## Technologies Used

- **C#**
- **ASP.NET Core Web API (.NET 8)**
- **Entity Framework Core 8**
- **SQL Server Express**
- **Swagger / OpenAPI**

## Architecture

The project follows a Three-Tier Architecture to separate responsibilities into distinct layers:

- **Presentation Layer:** Handles HTTP requests through API controllers.
- **Business Layer:** Contains the business logic and service classes.
- **Data Access Layer:** Handles database interactions using Entity Framework Core.

## Database

The project uses SQL Server Express as its database management system.

Entity Framework Core is used with the **Database First** approach, where the database schema is created first and the application code is designed to work with it.

## Project Status

**Version 1 — Initial Implementation**

This is the initial version of the project. Future updates will focus on improving code quality, implementing security features such as authentication and authorization, adding automated tests, and expanding the project's functionality.

## Database Setup

A database backup is available in this repository as `ClinicDB.bak`.

### Prerequisites

- SQL Server Express
- SQL Server Management Studio (SSMS)

### Restore the Database

1. Open SQL Server Management Studio.
2. Connect to your SQL Server instance.
3. Right-click `Databases` and select `Restore Database`.
4. Select `Device` as the source and choose `ClinicDB.bak`.
5. Complete the restore process.

### Connection String

Update the connection string in `appsettings.json` to match your SQL Server instance and database name.
