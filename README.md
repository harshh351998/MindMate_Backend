# MindMate Backend

MindMate is a journaling application backend built with .NET 9.0, following a clean architecture pattern. This backend provides RESTful APIs for user authentication, journal entry management, and health tracking.

## Project Structure

The solution follows a clean architecture pattern with the following projects:

- **MindMate.Api**: The main API project that handles HTTP requests and responses
- **MindMate.Core**: Contains domain models and interfaces
- **MindMate.Application**: Contains application business logic and services
- **MindMate.Infrastructure**: Implements data access and external service integrations

## Features

- User Authentication and Authorization using JWT
- Journal Entry Management
- Health Tracking
- Swagger API Documentation
- CORS support for Angular frontend
- SQLite Database

## Prerequisites

- .NET 9.0 SDK
- SQLite (for local development)

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the following commands:

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the project
cd src/MindMate.Api
dotnet run
```

The API will be available at `https://localhost:5001` and Swagger documentation at `https://localhost:5001/swagger`.

## API Endpoints

### Authentication
- POST `/api/auth/register` - Register a new user
- POST `/api/auth/login` - Login user and get JWT token

### Users
- GET `/api/users/me` - Get current user profile
- PUT `/api/users/me` - Update user profile

### Journal Entries
- GET `/api/journal-entries` - Get all journal entries
- GET `/api/journal-entries/{id}` - Get specific journal entry
- POST `/api/journal-entries` - Create new journal entry
- PUT `/api/journal-entries/{id}` - Update journal entry
- DELETE `/api/journal-entries/{id}` - Delete journal entry

### Health
- GET `/api/health` - Health check endpoint

## Configuration

The application uses the following configuration files:
- `appsettings.json` - Main configuration file
- `appsettings.Development.json` - Development-specific settings

Key configuration sections:
- JWT settings
- Database connection string
- CORS settings

## Database

The application uses SQLite as its database. The database file is located at `src/MindMate.Api/mindmate.db`. The database is automatically created when the application starts if it doesn't exist.

## Security

- JWT-based authentication
- CORS policy configured for specific origins
- HTTPS enabled in production
- Password hashing for user credentials

## Development

To run the project in development mode:

```bash
cd src/MindMate.Api
dotnet run --environment Development
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.
