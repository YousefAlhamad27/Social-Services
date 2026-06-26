# Social Services Platform API

A secure, multi-layered ASP.NET Core Web API engineered to handle user registration, authentication, and financial service payments. 

## Tech Stack
* Framework: ASP.NET Core Web API (C#)
* Database: SQL Server
* Architecture: Multi-layered Data Access and Business Logic separation
* Data Transfer: DTOs (Data Transfer Objects) implementation for secure payload handling

## Architecture Overview
This project enforces strict separation of concerns, ensuring scalability and maintainability:
* Presentation/API Layer: Exposes HTTP endpoints and manages routing.
* Business Logic Layer (clsSocialServicesBussinessDataAccess): Processes core business rules and payment validations.
* Data Access Layer (clsSocialDataAccess): Manages secure, optimized transactions with the SQL Server database.

## Key Features
* Secure User Authentication: Manages session state and secure access to endpoints.
* Service Payment Integration: Processes and records financial transactions reliably.
* Environment Configuration: Utilizes environment variables (probeForEnv.env) to secure sensitive connection strings and API keys. 

## Local Setup
1. Clone the repository.
2. Configure the SQL Server connection string in GlobalConfiguration.cs or the appropriate environment file.
3. Run Entity Framework migrations or execute the provided SQL scripts to build the database schema.
4. Build and run the solution via Visual Studio or the .NET CLI.
