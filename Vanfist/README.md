# Vanfist - ASP.NET Core Web Application

## Overview
Vanfist is a modern ASP.NET Core web application with a complete authentication system and Role-Based Access Control (RBAC) framework.

## Features

### ✅ Completed Features
- **User Authentication System**
  - User registration with email validation
  - Secure login with password hashing (SHA256)
  - Session management with 24-hour expiration
  - Logout functionality
  - Modern, responsive UI using Bootstrap

### 🔄 In Progress
- **Role-Based Access Control (RBAC)**
  - User roles and permissions system
  - Resource-based access control
  - Action-based permissions

## Technology Stack
- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server with Entity Framework Core
- **UI**: Bootstrap 5 with responsive design
- **Authentication**: Custom implementation with session management
- **Password Security**: SHA256 hashing

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)

### Installation
1. Clone the repository
2. Navigate to the project directory: `cd Vanfist`
3. Update the connection string in `appsettings.json` if needed
4. Run the application: `dotnet run`

### Database Setup
The application automatically creates the database and tables on first run. The database includes:
- **Accounts**: User account information
- **Sessions**: User session management
- **Roles**: User roles
- **Permissions**: System permissions
- **Actions**: Available actions
- **Resources**: System resources

### Usage
1. **Register**: Visit `/Auth/Register` to create a new account
2. **Login**: Visit `/Auth/Login` to sign in
3. **Home**: After login, you'll be redirected to the home page with user information
4. **Logout**: Use the dropdown menu in the navigation bar to logout

## Project Structure
```
Vanfist/
├── Controllers/
│   ├── AuthController.cs      # Authentication endpoints
│   └── HomeController.cs      # Home page controller
├── Services/
│   ├── IAuthService.cs        # Authentication service interface
│   └── Impl/
│       └── AuthService.cs     # Authentication service implementation
├── DTOs/
│   ├── Requests/              # Request models
│   └── Responses/             # Response models
├── Entities/                  # Database entities
├── Views/
│   ├── Auth/                  # Authentication views
│   └── Home/                  # Home page views
└── Configuration/
    └── ApplicationDbContext.cs # Database context
```

## Security Features
- **Password Hashing**: All passwords are hashed using SHA256
- **Session Management**: Secure session handling with expiration
- **Input Validation**: Comprehensive validation on all forms
- **SQL Injection Protection**: Entity Framework Core parameterized queries
- **XSS Protection**: ASP.NET Core built-in protection

## Development
The application is set up for development with:
- Hot reload enabled
- Detailed error pages in development mode
- Automatic database creation
- Comprehensive logging

## Next Steps
- Implement RBAC functionality
- Add user profile management
- Implement password reset functionality
- Add email verification
- Implement API endpoints for mobile applications
