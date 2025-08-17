# Vanfist - Role-Based Access Control (RBAC) System

A comprehensive ASP.NET Core MVC application implementing Role-Based Access Control (RBAC) with Entity Framework Core.

## 🏗️ Project Architecture

```
Vanfist/
├── Configuration/         # Database and application configuration
├── Constants/             # Application constants and enums
├── Controllers/           # MVC Controllers
├── DTOs/                  # Data Transfer Objects
│   ├── Requests/          # Request DTOs
│   └── Responses/         # Response DTOs
├── Entities/              # Entity Framework entities
├── Repositories/          # Data access layer
├── Services/              # Business logic services
└── Views/                 # Razor views
```

## 📁 Folder Structure & Documentation

### 🔧 Configuration/
Contains application configuration classes and database setup.

#### `ApplicationDbContext.cs`
- **Purpose**: Entity Framework Core DbContext configuration
- **Features**:
  - Database entity configurations
  - Relationship mappings
  - Constraint definitions
  - Many-to-many relationship configurations

**Key Configurations:**
```csharp
// Entity configurations with constraints
entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
entity.HasIndex(e => e.Username).IsUnique();

// Many-to-many relationships
Account ↔ Role (via AccountRoles table)
Role ↔ Permission (via RolePermissions table)
```

### 📊 Constants/
**Purpose**: Application-wide constants, enums, and static values.

**Planned Contents:**
- Role types
- Permission constants
- Status enums
- Error codes
- Configuration keys

### 🎮 Controllers/
MVC Controllers handling HTTP requests and responses.

#### `HomeController.cs`
- **Purpose**: Default application controller
- **Actions**:
  - `Index()` - Home page
  - `Privacy()` - Privacy policy page
  - `Error()` - Error handling page

**Planned Controllers:**
- `AccountController` - User management
- `RoleController` - Role management
- `PermissionController` - Permission management
- `AuthController` - Authentication

### 📦 DTOs/
Data Transfer Objects for API communication and data validation.

#### Requests/
**Purpose**: Incoming data models for API endpoints.

**Planned DTOs:**
- `CreateAccountRequest`
- `UpdateAccountRequest`
- `AssignRoleRequest`
- `CreatePermissionRequest`

#### Responses/
**Purpose**: Outgoing data models for API responses.

#### `ErrorViewModel.cs`
- **Purpose**: Error response model
- **Properties**:
  - `RequestId` - Unique request identifier
  - `ShowRequestId` - Computed property for display logic

### 🏛️ Entities/
Entity Framework Core domain models representing database tables.

#### `Account.cs`
- **Purpose**: User account entity
- **Properties**:
  - `Id` - Primary key
  - `Username` - Unique username (50 chars)
  - `Email` - Unique email (100 chars)
  - `PasswordHash` - Hashed password (255 chars)
  - `FirstName`, `LastName` - User names (50 chars each)
  - `Number` - Contact number (20 chars)
- **Relationships**: Many-to-many with `Role`

#### `Role.cs`
- **Purpose**: RBAC role entity
- **Properties**:
  - `Id` - Primary key
  - `Name` - Unique role name (50 chars)
  - `Description` - Role description (200 chars)
- **Relationships**: 
  - Many-to-many with `Account`
  - Many-to-many with `Permission`

#### `Permission.cs`
- **Purpose**: Permission entity linking actions to resources
- **Properties**:
  - `Id` - Primary key
  - `Name` - Unique permission name (50 chars)
  - `Description` - Permission description (200 chars)
  - `ActionId` - Foreign key to ActionEntity
  - `ResourceId` - Foreign key to Resource
- **Relationships**:
  - Many-to-one with `ActionEntity`
  - Many-to-one with `Resource`
  - Many-to-many with `Role`

#### `ActionEntity.cs`
- **Purpose**: Available actions (CRUD operations)
- **Properties**:
  - `Id` - Primary key
  - `Name` - Unique action name (50 chars)
  - `Description` - Action description (200 chars)
- **Relationships**: One-to-many with `Permission`

#### `Resource.cs`
- **Purpose**: System resources (entities, controllers)
- **Properties**:
  - `Id` - Primary key
  - `Name` - Unique resource name (50 chars)
  - `Description` - Resource description (200 chars)
- **Relationships**: One-to-many with `Permission`

### 🗄️ Repositories/
Data access layer implementing the Repository pattern.

#### `IRepository.cs`
- **Purpose**: Generic repository interface
- **Methods**:
  - `GetByIdAsync(int id)` - Retrieve by ID
  - `GetAllAsync()` - Retrieve all records
  - `AddAsync(T entity)` - Add new entity
  - `UpdateAsync(T entity)` - Update existing entity
  - `DeleteAsync(T entity)` - Delete entity
  - `SaveChangesAsync()` - Persist changes

#### `Repository.cs`
- **Purpose**: Generic repository implementation
- **Features**:
  - Entity Framework Core integration
  - Async operations
  - Generic type support
  - Unit of work pattern

### 🔧 Services/
Business logic layer for application operations.

**Planned Services:**
- `IRbacService` / `RbacService` - Role-based access control operations
- `IAccountService` / `AccountService` - User management
- `IPermissionService` / `PermissionService` - Permission management
- `IAuthService` / `AuthService` - Authentication and authorization

### 🎨 Views/
Razor views for the MVC application.

#### Structure:
```
Views/
├── Home/                  # Home controller views
│   ├── Index.cshtml      # Home page
│   └── Privacy.cshtml    # Privacy policy
├── Shared/               # Shared layouts and partials
│   ├── _Layout.cshtml    # Main layout
│   └── Error.cshtml      # Error page
├── _ViewImports.cshtml   # Global view imports
└── _ViewStart.cshtml     # View initialization
```

## 🗄️ Database Schema

### Tables:
1. **Accounts** - User accounts
2. **Roles** - RBAC roles
3. **Permissions** - Action-Resource permissions
4. **Actions** - Available actions
5. **Resources** - System resources
6. **AccountRoles** - User-Role assignments (junction table)
7. **RolePermissions** - Role-Permission assignments (junction table)

### Relationships:
```
Account ←→ Role (Many-to-Many)
Role ←→ Permission (Many-to-Many)
Permission → ActionEntity (Many-to-One)
Permission → Resource (Many-to-One)
```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server or LocalDB
- Entity Framework Core Tools

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Vanfist
   ```

2. **Install Entity Framework Tools**
   ```bash
   dotnet tool install --global dotnet-ef
   ```

3. **Create and apply migrations**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

### Configuration

Update `appsettings.json` with your database connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VanfistDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## 🔐 RBAC Implementation

### RBAC Levels Supported:
- **RBAC0**: User-Role assignments
- **RBAC1**: Role-Permission assignments (Action + Resource)

### Key Features:
- ✅ User account management
- ✅ Role-based access control
- ✅ Permission-based authorization
- ✅ Action-Resource permission model
- ✅ Many-to-many relationships
- ✅ Data integrity constraints
- ✅ Repository pattern
- ✅ Async operations

### Usage Examples:

#### Creating a User
```csharp
var account = new Account
{
    Username = "john.doe",
    Email = "john@example.com",
    PasswordHash = "hashed_password",
    FirstName = "John",
    LastName = "Doe"
};
```

#### Assigning Roles
```csharp
// Many-to-many relationship
account.Roles.Add(adminRole);
```

#### Creating Permissions
```csharp
var permission = new Permission
{
    Name = "Create User",
    ActionId = createAction.Id,
    ResourceId = userResource.Id
};
```

## 🛠️ Development

### Adding New Entities:
1. Create entity in `Entities/` folder
2. Add DbSet to `ApplicationDbContext`
3. Configure relationships in `OnModelCreating`
4. Create migration: `dotnet ef migrations add AddNewEntity`
5. Update database: `dotnet ef database update`

### Adding New Controllers:
1. Create controller in `Controllers/` folder
2. Implement CRUD operations
3. Add corresponding views in `Views/` folder
4. Update routing if needed

### Adding New Services:
1. Create interface in `Services/` folder
2. Implement service class
3. Register in `Program.cs` dependency injection

## 📝 API Documentation

### Planned Endpoints:
- `GET /api/accounts` - List all accounts
- `POST /api/accounts` - Create new account
- `GET /api/accounts/{id}` - Get account details
- `PUT /api/accounts/{id}` - Update account
- `DELETE /api/accounts/{id}` - Delete account
- `POST /api/accounts/{id}/roles` - Assign role to user
- `DELETE /api/accounts/{id}/roles/{roleId}` - Remove role from user

## 🔒 Security Considerations

1. **Password Hashing**: Always hash passwords before storing
2. **Input Validation**: Validate all user inputs
3. **Authorization**: Implement proper authorization checks
4. **Audit Trail**: Log important operations
5. **Data Integrity**: Use database constraints

## 🧪 Testing

### Planned Test Structure:
```
Tests/
├── Unit/                  # Unit tests
├── Integration/           # Integration tests
└── E2E/                  # End-to-end tests
```

## 📈 Future Enhancements

1. **RBAC2**: Role hierarchies
2. **RBAC3**: Role constraints
3. **Session Management**: User sessions
4. **API Authentication**: JWT tokens
5. **Audit Logging**: Operation tracking
6. **Caching**: Performance optimization
7. **Multi-tenancy**: Multi-tenant support

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 📞 Support

For questions and support, please open an issue in the repository.
