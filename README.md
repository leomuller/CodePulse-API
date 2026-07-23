# CodePulse API

A .NET Core REST API for the CodePulse application, providing backend services for the Angular frontend.

## Prerequisites

- .NET 8.0 SDK or later
- SQL Server (local instance or Docker)
- Visual Studio 2022 or VS Code with C# extension

## Getting Started

### 1. Setup Environment

Create `appsettings.Development.json` in the `CodePulse.API` project with your local configuration:

```json
{
  "ConnectionStrings": {
    "CodePulseConnectionString": "Server=your-server; Database=CodePulse; User Id=your-user; Password=your-password; MultipleActiveResultSets=true; TrustServerCertificate=true;"
  }
}
```

**Note:** `appsettings.Development.json` is in `.gitignore` for security.

### 2. Database Setup

Ensure SQL Server is running and the CodePulse database exists with the `webuser` account configured.

### 3. Run the API

```bash
cd CodePulse.API
dotnet run
```

The API will start on `https://localhost:5001` or the configured port.

### 4. Testing

```bash
dotnet test
```

## Project Structure

```
CodePulse.API/
├── Controllers/       # API endpoints
├── Services/          # Business logic
├── Models/            # Data models
├── Data/              # Database context and migrations
└── appsettings.json   # Configuration
```

## Configuration

- **Development:** Uses `appsettings.Development.json` (local secrets, not in git)
- **Production:** Uses `appsettings.json` and environment variables

## API Documentation

API endpoints are documented via Swagger/OpenAPI. When running locally, visit:
- `https://localhost:5001/swagger`

## Related Projects

- **CodePulse UI:** [CodePulse-UI](https://github.com/YOUR_USERNAME/CodePulse-UI) — Angular frontend application
