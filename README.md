# ═══════════════════════════════════════
# 🇬🇧 ENGLISH SECTION — TECHNICAL SPECIFICATION
# ═══════════════════════════════════════

## 1. Project Overview

A real estate web API platform similar to Somon.tj but focused exclusively on the **housing/property section**. Three user roles: Admin, Seller, and Buyer. Three property types: Private House (Havli), Building Apartment (Dom), and Rental Apartment (Kvartira — rent only, no sale).

## 2. Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Runtime | .NET / ASP.NET Core | 9.0 |
| Language | C# | 13.0 |
| Database | PostgreSQL | 16+ |
| ORM / Migration | Entity Framework Core | 9.x |
| Authentication | ASP.NET Core Identity + JWT Bearer | — |
| Caching | In-Memory Cache (IMemoryCache) | Built-in |
| API Docs | Swashbuckle (Swagger / OpenAPI) | 7.x |
| DI Container | Built-in ASP.NET Core DI | Built-in |
| Architecture | Clean Architecture | — |
| Background Jobs | BackgroundService (IHostedService) | Built-in |

## 4. Full Folder & File Structure

```
RealEstate.sln
│
├── src/
│   │
│   ├── RealEstate.Domain/
│   │   ├── RealEstate.Domain.csproj
│   │   ├── Entities/
│   │   │   ├── BaseEntity.cs
│   │   │   ├── Property/
│   │   │   │   ├── Property.cs            ← Abstract base for all property types
│   │   │   │   ├── Havli.cs               ← Private House entity
│   │   │   │   ├── DomApartment.cs        ← Building apartment entity
│   │   │   │   └── RentalApartment.cs     ← Rent-only apartment entity
│   │   │   └── User/
│   │   │       └── AppUser.cs             ← Extends IdentityUser
│   │   ├── Enums/
│   │   │   ├── PropertyType.cs            ← Havli | DomApartment | Rental
│   │   │   ├── PropertyStatus.cs          ← Available | Sold | Rented | Inactive
│   │   │   ├── ListingType.cs             ← ForSale | ForRent
│   │   │   └── UserRole.cs                ← Admin | Seller | Buyer
│   │   ├── Interfaces/
│   │   │   ├── Repositories/
│   │   │   │   ├── IGenericRepository.cs
│   │   │   │   ├── IPropertyRepository.cs
│   │   │   │   ├── IHavliRepository.cs
│   │   │   │   ├── IDomApartmentRepository.cs
│   │   │   │   └── IRentalApartmentRepository.cs
│   │   │   └── Services/
│   │   │       ├── IPropertyService.cs
│   │   │       ├── IHavliService.cs
│   │   │       ├── IDomApartmentService.cs
│   │   │       ├── IRentalApartmentService.cs
│   │   │       ├── IAuthService.cs
│   │   │       └── IAdminService.cs
│   │   └── Common/
│   │       ├── GenericResponse.cs         ← Generic API response wrapper
│   │       └── PagedResult.cs             ← Pagination wrapper
│   │
│   ├── RealEstate.Application/
│   │   ├── RealEstate.Application.csproj
│   │   ├── DTOs/
│   │   │   ├── Auth/
│   │   │   │   ├── RegisterDto.cs
│   │   │   │   ├── LoginDto.cs
│   │   │   │   └── TokenResponseDto.cs
│   │   │   ├── Property/
│   │   │   │   ├── PropertyBaseDto.cs
│   │   │   │   ├── HavliDto.cs
│   │   │   │   ├── HavliCreateDto.cs
│   │   │   │   ├── HavliUpdateDto.cs
│   │   │   │   ├── DomApartmentDto.cs
│   │   │   │   ├── DomApartmentCreateDto.cs
│   │   │   │   ├── DomApartmentUpdateDto.cs
│   │   │   │   ├── RentalApartmentDto.cs
│   │   │   │   ├── RentalApartmentCreateDto.cs
│   │   │   │   └── RentalApartmentUpdateDto.cs
│   │   │   ├── Filter/
│   │   │   │   ├── PropertyFilterDto.cs
│   │   │   │   ├── HavliFilterDto.cs
│   │   │   │   ├── DomFilterDto.cs
│   │   │   │   └── RentalFilterDto.cs
│   │   │   ├── Pagination/
│   │   │   │   └── PaginationDto.cs
│   │   │   └── Admin/
│   │   │       ├── AdminDashboardDto.cs
│   │   │       └── UserManageDto.cs
│   │   ├── Services/
│   │   │   ├── PropertyService.cs
│   │   │   ├── HavliService.cs
│   │   │   ├── DomApartmentService.cs
│   │   │   ├── RentalApartmentService.cs
│   │   │   ├── AuthService.cs
│   │   │   └── AdminService.cs
│   │   ├── Validators/
│   │   │   ├── HavliCreateValidator.cs
│   │   │   ├── DomApartmentCreateValidator.cs
│   │   │   └── RentalApartmentCreateValidator.cs
│   │   └── Mappings/
│   │       └── MappingProfile.cs          ← AutoMapper profiles
│   │
│   ├── RealEstate.Infrastructure/
│   │   ├── RealEstate.Infrastructure.csproj
│   │   ├── Data/
│   │   │   ├── AppDbContext.cs            ← EF Core DbContext
│   │   │   └── Migrations/               ← EF Core auto-generated migrations
│   │   ├── Repositories/
│   │   │   ├── GenericRepository.cs
│   │   │   ├── PropertyRepository.cs
│   │   │   ├── HavliRepository.cs
│   │   │   ├── DomApartmentRepository.cs
│   │   │   └── RentalApartmentRepository.cs
│   │   ├── Identity/
│   │   │   └── IdentitySeeder.cs          ← Seeds roles & default admin
│   │   ├── JWT/
│   │   │   ├── JwtSettings.cs
│   │   │   └── JwtTokenGenerator.cs
│   │   ├── Cache/
│   │   │   └── CacheService.cs            ← IMemoryCache wrapper
│   │   ├── BackgroundServices/
│   │   │   └── PropertyStatusUpdaterService.cs ← Updates expired listings
│   │   └── DependencyInjection/
│   │       └── InfrastructureServiceExtensions.cs
│   │
│   └── RealEstate.API/
│       ├── RealEstate.API.csproj
│       ├── Program.cs                     ← Entry point, all DI registrations
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       ├── Controllers/
│       │   ├── AuthController.cs          ← Register, Login, Refresh
│       │   ├── HavliController.cs         ← CRUD + Filter + Pagination
│       │   ├── DomApartmentController.cs  ← CRUD + Filter + Pagination
│       │   ├── RentalController.cs        ← CRUD + Filter + Pagination
│       │   └── AdminController.cs         ← Admin-only operations
│       ├── Middleware/
│       │   ├── ExceptionHandlingMiddleware.cs
│       │   └── RequestLoggingMiddleware.cs
│       └── Extensions/
│           ├── SwaggerExtensions.cs
│           ├── AuthExtensions.cs
│           └── CorsExtensions.cs
│

## 5. NuGet Packages — Per Project

### 5.1 RealEstate.Domain
```
No external NuGet packages required.
(Pure C# — only .NET 9 BCL)
```

### 5.2 RealEstate.Application
```
Install via: dotnet add package <PackageName> --version <Version>

AutoMapper                            → 12.0.1
AutoMapper.Extensions.Microsoft.DependencyInjection → 12.0.1
FluentValidation                      → 11.9.2
FluentValidation.DependencyInjectionExtensions → 11.9.2
Microsoft.Extensions.Logging.Abstractions → 9.0.0
```

### 5.3 RealEstate.Infrastructure
```
Microsoft.EntityFrameworkCore                      → 9.0.x
Microsoft.EntityFrameworkCore.Tools               → 9.0.x   (CLI migration)
Npgsql.EntityFrameworkCore.PostgreSQL             → 9.0.x
Microsoft.AspNetCore.Identity.EntityFrameworkCore → 9.0.x
Microsoft.AspNetCore.Authentication.JwtBearer     → 9.0.x
System.IdentityModel.Tokens.Jwt                   → 8.x
Microsoft.Extensions.Caching.Memory              → 9.0.x   (In-Memory Cache)
Microsoft.Extensions.Configuration.Abstractions   → 9.0.x
```

### 5.4 RealEstate.API
```
Microsoft.AspNetCore.OpenApi                      → 9.0.x
Swashbuckle.AspNetCore                           → 7.x
Swashbuckle.AspNetCore.Filters                   → 8.x     (JWT auth in Swagger UI)
Microsoft.AspNetCore.Authentication.JwtBearer     → 9.0.x
Serilog.AspNetCore                               → 8.x     (Logging)
Serilog.Sinks.Console                            → 6.x
```

### 5.5 Test Projects
```
xunit                                            → 2.9.x
xunit.runner.visualstudio                        → 2.8.x
Moq                                              → 4.20.x
Microsoft.EntityFrameworkCore.InMemory           → 9.0.x
Microsoft.AspNetCore.Mvc.Testing                 → 9.0.x
FluentAssertions                                 → 6.12.x
```

## 6. Step-by-Step Setup Guide

### Step 1 — Prerequisites
```
Install:
  .NET 9 SDK          → https://dotnet.microsoft.com/download/dotnet/9
  PostgreSQL 16       → https://www.postgresql.org/download/
  pgAdmin 4           → https://www.pgadmin.org/download/
  Visual Studio 2022+ (or VS Code + C# Dev Kit extension)
  EF Core CLI tools   → dotnet tool install --global dotnet-ef --version 9.*
```

### Step 2 — Create Solution & Projects
```bash
mkdir RealEstate && cd RealEstate
dotnet new sln -n RealEstate

dotnet new classlib -n RealEstate.Domain        -o src/RealEstate.Domain
dotnet new classlib -n RealEstate.Application   -o src/RealEstate.Application
dotnet new classlib -n RealEstate.Infrastructure -o src/RealEstate.Infrastructure
dotnet new webapi   -n RealEstate.API           -o src/RealEstate.API

dotnet new xunit    -n RealEstate.UnitTests     -o tests/RealEstate.UnitTests
dotnet new xunit    -n RealEstate.IntegrationTests -o tests/RealEstate.IntegrationTests

dotnet sln add src/RealEstate.Domain
dotnet sln add src/RealEstate.Application
dotnet sln add src/RealEstate.Infrastructure
dotnet sln add src/RealEstate.API
dotnet sln add tests/RealEstate.UnitTests
dotnet sln add tests/RealEstate.IntegrationTests
```

### Step 3 — Add Project References
```bash
# Application depends on Domain
dotnet add src/RealEstate.Application reference src/RealEstate.Domain

# Infrastructure depends on Application + Domain
dotnet add src/RealEstate.Infrastructure reference src/RealEstate.Application
dotnet add src/RealEstate.Infrastructure reference src/RealEstate.Domain

# API depends on all layers
dotnet add src/RealEstate.API reference src/RealEstate.Application
dotnet add src/RealEstate.API reference src/RealEstate.Infrastructure
dotnet add src/RealEstate.API reference src/RealEstate.Domain

# Tests
dotnet add tests/RealEstate.UnitTests reference src/RealEstate.Application
dotnet add tests/RealEstate.UnitTests reference src/RealEstate.Infrastructure
dotnet add tests/RealEstate.IntegrationTests reference src/RealEstate.API
```

### Step 4 — Install NuGet Packages

#### Application layer
```bash
cd src/RealEstate.Application
dotnet add package AutoMapper --version 13.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 13.0.1
dotnet add package FluentValidation --version 11.9.2
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.9.2
dotnet add package Microsoft.Extensions.Logging.Abstractions --version 9.0.0
```

#### Infrastructure layer
```bash
cd ../RealEstate.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.3.0
dotnet add package Microsoft.Extensions.Caching.Memory --version 9.0.0
dotnet add package Microsoft.Extensions.Configuration.Abstractions --version 9.0.0
```

#### API layer
```bash
cd ../RealEstate.API
dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.0
dotnet add package Swashbuckle.AspNetCore --version 7.2.0
dotnet add package Swashbuckle.AspNetCore.Filters --version 8.0.2
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.0.0
dotnet add package Serilog.AspNetCore --version 8.0.3
dotnet add package Serilog.Sinks.Console --version 6.0.0
```

#### Test projects
```bash
cd ../../tests/RealEstate.UnitTests
dotnet add package xunit --version 2.9.0
dotnet add package Moq --version 4.20.72
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 9.0.0
dotnet add package FluentAssertions --version 6.12.2

cd ../RealEstate.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 9.0.0
dotnet add package FluentAssertions --version 6.12.2
```

### Step 5 — Database Configuration
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=RealEstateDb;Username=postgres;Password=yourpassword"
  },
  "JwtSettings": {
    "SecretKey": "your-very-long-secret-key-min-256-bits",
    "Issuer": "RealEstateAPI",
    "Audience": "RealEstateClient",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  },
  "CacheSettings": {
    "DefaultExpirationMinutes": 10
  }
}
```

### Step 6 — EF Core Migrations
```bash
cd src/RealEstate.API

# Create initial migration
dotnet ef migrations add InitialCreate \
  --project ../RealEstate.Infrastructure \
  --startup-project . \
  --output-dir Data/Migrations

# Apply migration to database
dotnet ef database update \
  --project ../RealEstate.Infrastructure \
  --startup-project .
```

### Step 7 — Folder Creation Inside Each Project

#### Domain folders
```bash
cd src/RealEstate.Domain
mkdir -p Entities/Property Entities/User Enums Interfaces/Repositories Interfaces/Services Common
```

#### Application folders
```bash
cd ../RealEstate.Application
mkdir -p DTOs/Auth DTOs/Property DTOs/Filter DTOs/Pagination DTOs/Admin \
         Services Validators Mappings
```

#### Infrastructure folders
```bash
cd ../RealEstate.Infrastructure
mkdir -p Data/Migrations Repositories Identity JWT Cache BackgroundServices DependencyInjection
```

#### API folders
```bash
cd ../RealEstate.API
mkdir -p Controllers Middleware Extensions
```

### Step 8 — Key File Descriptions (No Code, Structure Only)

#### GenericResponse.cs (Domain/Common)
```
Fields:
  - bool IsSuccess
  - string Message
  - T? Data
  - List<string>? Errors
  - int StatusCode
Static factory methods: Success(), Failure(), NotFound(), Unauthorized()
```

#### PagedResult.cs (Domain/Common)
```
Fields:
  - List<T> Items
  - int TotalCount
  - int PageNumber
  - int PageSize
  - int TotalPages (calculated)
  - bool HasPreviousPage
  - bool HasNextPage
```

#### IGenericRepository.cs (Domain/Interfaces)
```
Methods:
  - Task<T?> GetByIdAsync(Guid id)
  - Task<IEnumerable<T>> GetAllAsync()
  - Task<PagedResult<T>> GetPagedAsync(int page, int pageSize)
  - Task AddAsync(T entity)
  - Task UpdateAsync(T entity)
  - Task DeleteAsync(Guid id)
  - Task<bool> ExistsAsync(Guid id)
```

#### Property.cs — Abstract base entity (Domain/Entities/Property)
```
Fields:
  - Guid Id
  - string Title
  - string Description
  - decimal Price
  - string Address
  - string District
  - string Region
  - string City
  - double Area (m²)
  - int Rooms
  - bool HasBathroom
  - bool HasToilet
  - List<string> ImageUrls
  - PropertyStatus Status
  - PropertyType Type
  - ListingType ListingType (ForSale / ForRent)
  - Guid SellerId
  - AppUser Seller
  - DateTime CreatedAt
  - DateTime UpdatedAt
```

#### Havli.cs (Domain/Entities/Property)
```
Inherits: Property
Additional fields:
  - double LandArea (sotka or m²)
  - bool HasGarage
  - bool HasPool
  - bool HasBasement
  - string FenceType
```

#### DomApartment.cs (Domain/Entities/Property)
```
Inherits: Property
Additional fields:
  - int Floor
  - int TotalFloors
  - string Entrance (A, B, C, D)
  - bool HasElevator
  - bool HasBalcony
```

#### RentalApartment.cs (Domain/Entities/Property)
```
Inherits: Property
Additional fields:
  - int Floor
  - int TotalFloors
  - string Entrance (A, B, C, D)
  - bool HasElevator
  - bool HasBalcony
  - decimal MonthlyRent
  - bool UtilitiesIncluded
  - int MinRentalMonths
  NOTE: ListingType is always ForRent — enforced at entity level
```

#### AppDbContext.cs (Infrastructure/Data)
```
Extends: IdentityDbContext<AppUser>
DbSets:
  - DbSet<Havli> Havlis
  - DbSet<DomApartment> DomApartments
  - DbSet<RentalApartment> RentalApartments
Configurations:
  - Table-per-hierarchy (TPH) or Table-per-type (TPT) for property inheritance
  - Owned types for address
  - Seeded roles: Admin, Seller, Buyer
```

#### JwtTokenGenerator.cs (Infrastructure/JWT)
```
Dependencies: IOptions<JwtSettings>, UserManager<AppUser>
Methods:
  - Task<TokenResponseDto> GenerateTokenAsync(AppUser user)
  - Task<string> GenerateRefreshTokenAsync()
  - ClaimsPrincipal? ValidateToken(string token)
Claims in token:
  - sub (userId)
  - email
  - role
  - jti (unique token id)
```

#### CacheService.cs (Infrastructure/Cache)
```
Wraps IMemoryCache
Methods:
  - T? Get<T>(string key)
  - void Set<T>(string key, T value, TimeSpan? expiry)
  - void Remove(string key)
  - bool TryGetValue<T>(string key, out T? value)
Cache keys pattern:
  - "havli:{id}"
  - "havli:list:{filter_hash}"
  - "rental:list:{filter_hash}"
  - "dom:list:{filter_hash}"
```

#### PropertyStatusUpdaterService.cs (Infrastructure/BackgroundServices)
```
Extends: BackgroundService
Purpose: Every 24 hours checks listings older than X days, marks them Inactive
ExecuteAsync: runs on timer, calls repository to update stale listings
Interval: configurable via appsettings
```

#### ExceptionHandlingMiddleware.cs (API/Middleware)
```
Catches all unhandled exceptions
Returns: GenericResponse with appropriate HTTP status code
Handles:
  - NotFoundException → 404
  - UnauthorizedException → 401
  - ValidationException → 422
  - General Exception → 500
Logs all exceptions via ILogger
```

#### RequestLoggingMiddleware.cs (API/Middleware)
```
Logs every incoming HTTP request:
  - Method, Path, QueryString
  - StatusCode
  - Duration (ms)
  - UserId (if authenticated)
```

#### SwaggerExtensions.cs (API/Extensions)
```
Configures:
  - OpenAPI document info (title, version, description)
  - JWT Bearer security definition
  - Swagger UI with Authorize button
  - XML comments from all projects
  - Operation filters for auth endpoints
```

#### Program.cs (API)
```
Registration order:
  1. DbContext with PostgreSQL connection string
  2. ASP.NET Core Identity (AppUser, roles)
  3. JWT Authentication + Authorization
  4. AutoMapper profiles
  5. FluentValidation validators
  6. All Repositories (scoped)
  7. All Services (scoped)
  8. CacheService (singleton)
  9. BackgroundService (singleton)
  10. Swagger / OpenAPI
  11. CORS policy
  12. Serilog logger
  13. Middleware pipeline:
      - Exception handling middleware
      - Request logging middleware
      - Authentication
      - Authorization
      - Swagger UI
      - Controllers
  14. IdentitySeeder (run at startup)
```

### Step 9 — API Endpoints Summary

#### Auth — /api/auth
```
POST   /register        → Register new user (Seller or Buyer)
POST   /login           → Login, returns JWT + RefreshToken
POST   /refresh-token   → Refresh expired JWT
POST   /logout          → Invalidate refresh token
GET    /me              → Get current user profile [Authorized]
```

#### Havli — /api/havli
```
GET    /               → Get all Havlis (filtered + paginated) [Public]
GET    /{id}           → Get single Havli [Public]
POST   /               → Create Havli listing [Seller]
PUT    /{id}           → Update Havli [Seller/Owner]
DELETE /{id}           → Delete Havli [Seller/Owner or Admin]
GET    /my-listings    → Get current seller's listings [Seller]
```

#### Dom Apartment — /api/dom
```
GET    /               → Get all Dom apartments (filtered + paginated) [Public]
GET    /{id}           → Get single apartment [Public]
POST   /               → Create listing [Seller]
PUT    /{id}           → Update [Seller/Owner]
DELETE /{id}           → Delete [Seller/Owner or Admin]
GET    /my-listings    → Seller's listings [Seller]
```

#### Rental — /api/rental
```
GET    /               → Get all rentals (filtered + paginated) [Public]
GET    /{id}           → Get single rental [Public]
POST   /               → Create rental listing [Seller]
PUT    /{id}           → Update [Seller/Owner]
DELETE /{id}           → Delete [Seller/Owner or Admin]
GET    /my-listings    → Seller's listings [Seller]
```

#### Admin — /api/admin
```
GET    /dashboard      → Stats overview [Admin]
GET    /properties     → All properties with status [Admin]
PUT    /properties/{id}/status → Change property status [Admin]
DELETE /properties/{id}        → Force delete [Admin]
GET    /users          → All users [Admin]
PUT    /users/{id}/block       → Block user [Admin]
DELETE /users/{id}             → Delete user [Admin]
```

### Step 10 — Filter & Pagination Parameters

#### Havli Filter
```
minPrice, maxPrice         (decimal)
minArea, maxArea           (double) — land area
minHouseArea, maxHouseArea (double)
minRooms, maxRooms         (int)
hasGarage                  (bool?)
hasPool                    (bool?)
region                     (string)
district                   (string)
status                     (PropertyStatus)
sortBy                     (price_asc | price_desc | newest | oldest)
pageNumber                 (int, default: 1)
pageSize                   (int, default: 10, max: 50)
```

#### Dom Apartment Filter
```
minPrice, maxPrice    (decimal)
minArea, maxArea      (double)
minRooms, maxRooms    (int)
minFloor, maxFloor    (int)
entrance              (A | B | C | D)
hasBalcony            (bool?)
hasElevator           (bool?)
region, district      (string)
sortBy
pageNumber, pageSize
```

#### Rental Filter
```
minMonthlyRent, maxMonthlyRent (decimal)
minArea, maxArea               (double)
minRooms, maxRooms             (int)
minFloor, maxFloor             (int)
entrance                       (A | B | C | D)
utilitiesIncluded              (bool?)
region, district               (string)
sortBy
pageNumber, pageSize
```

### Step 11 — Authorization Matrix

| Endpoint | Public | Buyer | Seller | Admin |
|----------|--------|-------|--------|-------|
| GET listings | ✅ | ✅ | ✅ | ✅ |
| GET single | ✅ | ✅ | ✅ | ✅ |
| POST listing | ❌ | ❌ | ✅ | ✅ |
| PUT own listing | ❌ | ❌ | ✅ | ✅ |
| DELETE own listing | ❌ | ❌ | ✅ | ✅ |
| DELETE any listing | ❌ | ❌ | ❌ | ✅ |
| Admin dashboard | ❌ | ❌ | ❌ | ✅ |
| Manage users | ❌ | ❌ | ❌ | ✅ |

---

---
