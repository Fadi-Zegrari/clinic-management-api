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

This is the first version of the project. Further improvements and features may be added in future versions.
