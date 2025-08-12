# BeerBarBrewery API

A comprehensive RESTful API for managing beers, bars, and breweries with their relationships. Built with ASP.NET Core 8.0, Entity Framework Core, and following clean architecture principles.

## 🏗️ Architecture

- The solution provides:
- **Clean Architecture** with separation of concerns
- **Repository Pattern** for data access
- **Unit of Work Pattern** for transaction management
- **AutoMapper** for object mapping
- **Dependency Injection** throughout the application
- **Comprehensive Unit Testing** with NUnit and Moq

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run database migrations
4. Build and run the application

## 📚 API Endpoints

### 🍺 Beer Management

#### Get Beer by ID
- **GET** `/api/beer/{id}`
- **Description**: Retrieves a specific beer by its ID
- **Parameters**: `id` (integer) - Beer ID (must be > 0)
- **Success Response**: `200 OK` with `BeerResponse`
- **Error Responses**: 
  - `400 Bad Request` - Invalid ID
  - `404 Not Found` - Beer not found

#### Get Beers by Alcohol Volume Range
- **GET** `/api/beer?gtAlcoholByVolume={min}&ltAlcoholByVolume={max}`
- **Description**: Retrieves beers within specified ABV range (at least one parameter required)
- **Parameters**: 
  - `gtAlcoholByVolume` (decimal, optional) - Minimum ABV (exclusive, must be ≥ 0)
  - `ltAlcoholByVolume` (decimal, optional) - Maximum ABV (exclusive, must be ≥ 0)
- **Success Response**: `200 OK` with `BeerResponse[]`
- **Error Responses**:
  - `400 Bad Request` - Invalid ABV values or no parameters provided
  - `404 Not Found` - No beers found matching criteria

#### Create Beer
- **POST** `/api/beer`
- **Description**: Creates a new beer record
- **Request Body**: `CreateBeerRequest`
```json
{
  "name": "string",
  "percentageAlcoholByVolume": 0.0
}
```
- **Success Response**: `201 Created` with `BeerResponse`
- **Error Response**: `400 Bad Request` - Invalid data

#### Update Beer
- **PUT** `/api/beer/{id}`
- **Description**: Updates an existing beer record
- **Parameters**: `id` (integer) - Beer ID
- **Request Body**: `CreateBeerRequest`
- **Success Response**: `200 OK` with success message
- **Error Responses**:
  - `400 Bad Request` - Invalid data
  - `404 Not Found` - Beer not found

#### Delete Beer
- **DELETE** `/api/beer/{id}`
- **Description**: Deletes a beer record
- **Parameters**: `id` (integer) - Beer ID
- **Success Response**: `200 OK` with success message
- **Error Responses**:
  - `400 Bad Request` - Invalid ID
  - `404 Not Found` - Beer not found

### 🏭 Brewery Management

#### Get All Breweries
- **GET** `/api/brewery`
- **Description**: Retrieves all brewery records
- **Success Response**: `200 OK` with `BreweryResponse[]`
- **Error Response**: `404 Not Found` - No breweries found

#### Get All Breweries with Beers
- **GET** `/api/brewery/beer`
- **Description**: Retrieves all breweries with their associated beers
- **Success Response**: `200 OK` with `BreweryWithBeerResponse[]`
- **Error Response**: `404 Not Found` - No data found

#### Get Brewery by ID
- **GET** `/api/brewery/{id}`
- **Description**: Retrieves a specific brewery by ID
- **Parameters**: `id` (integer) - Brewery ID
- **Success Response**: `200 OK` with `BreweryResponse`
- **Error Responses**:
  - `400 Bad Request` - Invalid ID
  - `404 Not Found` - Brewery not found

#### Get Brewery with Beers by ID
- **GET** `/api/brewery/{breweryId}/beer`
- **Description**: Retrieves a brewery with its associated beers
- **Parameters**: `breweryId` (integer) - Brewery ID
- **Success Response**: `200 OK` with `BreweryWithBeerResponse`
- **Error Responses**:
  - `400 Bad Request` - Invalid ID
  - `404 Not Found` - Brewery not found

#### Create Brewery
- **POST** `/api/brewery`
- **Description**: Creates a new brewery record
- **Request Body**: `CreateBreweryRequest`
```json
{
  "name": "string"
}
```
- **Success Response**: `201 Created` with `BreweryResponse`
- **Error Response**: `400 Bad Request` - Invalid data

#### Update Brewery
- **PUT** `/api/brewery/{id}`
- **Description**: Updates an existing brewery record
- **Parameters**: `id` (integer) - Brewery ID
- **Request Body**: `CreateBreweryRequest`
- **Success Response**: `200 OK` with success message
- **Error Responses**:
  - `400 Bad Request` - Invalid data
  - `404 Not Found` - Brewery not found

#### Delete Brewery
- **DELETE** `/api/brewery/{id}`
- **Description**: Deletes a brewery record
- **Parameters**: `id` (integer) - Brewery ID
- **Success Response**: `200 OK` with success message
- **Error Responses**:
  - `400 Bad Request` - Invalid ID
  - `404 Not Found` - Brewery not found

#### Assign Beer to Brewery
- **POST** `/api/brewery/beer`
- **Description**: Associates a beer with a brewery using many-to-many relationship
- **Request Body**: `BreweryBeerRequest`
```json
{
  "breweryId": 0,
  "beerId": 0
}
```
- **Success Responses**: 
  - `200 OK` - "Beer assigned to brewery successfully." (new relationship)
  - `200 OK` - "Beer already assigned to brewery." (existing relationship)
- **Error Responses**:
  - `400 Bad Request` - Invalid data
  - `404 Not Found` - Brewery or beer not found

### 🍻 Bar Management

#### Get All Bars
- **GET** `/api/bar`
- **Description**: Retrieves all bar records
- **Success Response**: `200 OK` with `BarResponse[]`
- **Error Response**: `404 Not Found` - No bars found

#### Get Bar by ID
- **GET** `/api/bar/{id}`
- **Description**: Retrieves a specific bar by ID
- **Parameters**: `id` (integer) - Bar ID
- **Success Response**: `200 OK` with `BarResponse`
- **Error Responses**:
  - `400 Bad Request` - Invalid ID
  - `404 Not Found` - Bar not found

#### Get All Bars with Beers
- **GET** `/api/bar/beer`
- **Description**: Retrieves all bars with their served beers
- **Success Response**: `200 OK` with `BarWithBeerResponse[]`
- **Error Response**: `404 Not Found` - No data found

#### Get Beers Served at Bar
- **GET** `/api/bar/{barId}/beer`
- **Description**: Retrieves all beers served at a specific bar
- **Parameters**: `barId` (integer) - Bar ID
- **Success Response**: `200 OK` with `BeerResponse[]`
- **Error Responses**:
  - `400 Bad Request` - Invalid ID
  - `404 Not Found` - No beers found for bar

#### Create Bar
- **POST** `/api/bar`
- **Description**: Creates a new bar record
- **Request Body**: `CreateBarRequest`
```json
{
  "name": "string",
  "address": "string"
}
```
- **Success Response**: `201 Created` with `BarResponse`
- **Error Response**: `400 Bad Request` - Invalid data

#### Update Bar
- **PUT** `/api/bar/{id}`
- **Description**: Updates an existing bar record
- **Parameters**: `id` (integer) - Bar ID
- **Request Body**: `CreateBarRequest`
- **Success Response**: `200 OK` with success message
- **Error Responses**:
  - `400 Bad Request` - Invalid data
  - `404 Not Found` - Bar not found

#### Delete Bar
- **DELETE** `/api/bar/{id}`
- **Description**: Deletes a bar record
- **Parameters**: `id` (integer) - Bar ID
- **Success Response**: `200 OK` with success message
- **Error Responses**:
  - `400 Bad Request` - Invalid ID
  - `404 Not Found` - Bar not found

#### Assign Beer to Bar
- **POST** `/api/bar/beer`
- **Description**: Associates a beer with a bar
- **Request Body**: `BarBeerRequest`
```json
{
  "barId": 0,
  "beerId": 0
}
```
- **Success Responses**: 
  - `200 OK` - "Beer assigned to bar successfully." (new relationship)
  - `200 OK` - "Beer already assigned to bar." (existing relationship)
- **Error Responses**:
  - `400 Bad Request` - Invalid data
  - `404 Not Found` - Bar or beer not found

## 📋 Data Models

### Request Models
- **CreateBeerRequest**: `{ name, percentageAlcoholByVolume }`
- **CreateBreweryRequest**: `{ name }`
- **CreateBarRequest**: `{ name, address }`
- **BarBeerRequest**: `{ barId, beerId }`
- **BreweryBeerRequest**: `{ breweryId, beerId }`

### Response Models
- **BeerResponse**: `{ id, name, percentageAlcoholByVolume }`
- **BreweryResponse**: `{ id, name }`
- **BarResponse**: `{ id, name, address }`
- **BreweryWithBeerResponse**: `{ id, name, beers[] }`
- **BarWithBeerResponse**: `{ id, name, address, beers[] }`
- **ErrorDetails**: `{ message, statusCode }`

## 🔧 Technical Features

- **Global Exception Handling** middleware
- **Input Validation** with model state validation
- **Swagger/OpenAPI** documentation
- **AutoMapper** for object mapping
- **Repository Pattern** with Unit of Work
- **Many-to-Many Relationships** between Beer-Bar and Beer-Brewery entities
- **Comprehensive Error Handling** with consistent error responses
- **Clean Architecture** with separation of concerns

## 🧪 Testing

The solution includes comprehensive unit tests covering:
- **Repository Layer** tests with in-memory database
- **Business Logic** tests with mocked dependencies
- **Controller** tests with mocked services
- **Unit of Work** pattern tests

Run tests using:
```bash
dotnet test
```

## 🏃‍♂️ Running the Application

1. **Development**: `dotnet run --project BeerBarBrewery`
2. **Swagger UI**: Navigate to `/swagger` when running in development
3. **API Base URL**: `https://localhost:7xxx/api`

## 📝 Notes

- All endpoints return JSON responses
- IDs must be positive integers (> 0)
- Comprehensive error handling with meaningful error messages
- Supports relationships between beers, bars, and breweries
- **Assignment operations are idempotent** - attempting to assign an existing relationship returns success with appropriate message
- Built following REST API best practices