# EventEase Venue Booking System

An ASP.NET Core MVC web application for managing event venue bookings. Built for EventEase event management company to streamline venue management, prevent double bookings, and provide a clear view of scheduled events.

## Features

- **Venue Management**: Create, view, update, and delete venues with details including capacity, location, and images
- **Event Management**: Manage events that can be booked at various venues
- **Booking System**: Create and manage venue bookings with conflict detection to prevent double bookings
- **Calendar View**: Visual timeline of all bookings organized by month
- **Dashboard**: Quick overview of system statistics and upcoming bookings
- **Responsive Design**: Modern, mobile-friendly Bootstrap UI

## Technology Stack

- **Framework**: ASP.NET Core MVC (.NET 8)
- **Database**: SQL Server (with Azure SQL support)
- **ORM**: Entity Framework Core 8
- **UI**: Bootstrap 5, Bootstrap Icons
- **Architecture**: Model-View-Controller (MVC)

## Database Structure

The application uses three main entities:

1. **Venue** - Stores venue information (name, location, capacity, image)
2. **Event** - Stores event details (name, date, description, optional venue)
3. **Booking** - Links events to venues with booking dates and customer info



## Getting Started

### Prerequisites

- .NET 8 SDK or later
- SQL Server (LocalDB or full instance) or Azure SQL Database
- Visual Studio 2022 or Visual Studio Code

### Local Development Setup

1. **Clone or extract the project**
   ```bash
   cd EventEase
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Update database (migrations are already created)**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Open browser to `https://localhost:7001` or `http://localhost:5001`
   - The dashboard will display system overview

### Default Connection String (Local Development)

```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EventEaseDB;Trusted_Connection=True;MultipleActiveResultSets=true"
```

## Azure Deployment Guide

### Part 1: Deploy Web App to Azure App Service

#### Option A: Using Visual Studio (Recommended)

1. **Publish from Visual Studio**
   - Right-click on the project in Solution Explorer
   - Select **Publish**
   - Choose **Azure** as target
   - Select **Azure App Service (Windows)**
   - Click **Create New** or select existing App Service
   - Configure:
     - **Name**: `eventease-booking-system` (or your preferred name)
     - **Subscription**: Select your Azure subscription
     - **Resource Group**: Create new or select existing
     - **Hosting Plan**: Select or create (S1 Standard recommended)
   - Click **Create** and then **Finish**
   - Click **Publish**

#### Option B: Using Azure CLI

1. **Login to Azure**
   ```bash
   az login
   ```

2. **Create Resource Group**
   ```bash
   az group create --name EventEaseResourceGroup --location "South Africa North"
   ```

3. **Create App Service Plan**
   ```bash
   az appservice plan create \
     --name EventEaseServicePlan \
     --resource-group EventEaseResourceGroup \
     --sku S1 \
     --is-linux false
   ```

4. **Create Web App**
   ```bash
   az webapp create \
     --name eventease-booking-system \
     --resource-group EventEaseResourceGroup \
     --plan EventEaseServicePlan \
     --runtime "dotnet:8"
   ```

5. **Publish Application**
   ```bash
   dotnet publish -c Release -o ./publish
   
   # Zip the publish folder and deploy
   Compress-Archive -Path .\publish\* -DestinationPath .\publish.zip
   
   az webapp deployment source config-zip \
     --resource-group EventEaseResourceGroup \
     --name eventease-booking-system \
     --src publish.zip
   ```

### Part 2: Set Up Azure SQL Database

#### Create Azure SQL Server and Database

1. **Create SQL Server**
   ```bash
   az sql server create \
     --name eventease-sql-server \
     --resource-group EventEaseResourceGroup \
     --location "South Africa North" \
     --admin-user eventeaseadmin \
     --admin-password YourStrongPassword123!
   ```

2. **Create SQL Database**
   ```bash
   az sql db create \
     --name EventEaseDB \
     --server eventease-sql-server \
     --resource-group EventEaseResourceGroup \
     --service-objective S0
   ```

3. **Configure Firewall Rules**
   ```bash
   # Allow Azure services access
   az sql server firewall-rule create \
     --resource-group EventEaseResourceGroup \
     --server eventease-sql-server \
     --name AllowAzureServices \
     --start-ip-address 0.0.0.0 \
     --end-ip-address 0.0.0.0
   
   # Allow your local machine (optional - for testing)
   az sql server firewall-rule create \
     --resource-group EventEaseResourceGroup \
     --server eventease-sql-server \
     --name AllowLocalMachine \
     --start-ip-address YOUR_PUBLIC_IP \
     --end-ip-address YOUR_PUBLIC_IP
   ```

#### Update Application Connection String

1. **Get Connection String from Azure**
   ```bash
   az sql db show-connection-string \
     --client ado.net \
     --name EventEaseDB \
     --server eventease-sql-server
   ```

2. **Configure App Service Connection String**
   ```bash
   az webapp config connection-string set \
     --resource-group EventEaseResourceGroup \
     --name eventease-booking-system \
     --connection-string-type SQLAzure \
     --settings DefaultConnection="Server=tcp:eventease-sql-server.database.windows.net,1433;Initial Catalog=EventEaseDB;Persist Security Info=False;User ID=eventeaseadmin;Password=YourStrongPassword123!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
   ```

### Part 3: Migrate Data to Azure SQL

#### Option 1: Using EF Core Migrations (Recommended)

1. **Update local appsettings.json with Azure connection string temporarily**
2. **Run migrations against Azure SQL**
   ```bash
   dotnet ef database update --connection "Server=tcp:eventease-sql-server.database.windows.net,1433;Initial Catalog=EventEaseDB;Persist Security Info=False;User ID=eventeaseadmin;Password=YourStrongPassword123!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
   ```

#### Option 2: Using SQL Server Management Studio (SSMS)

1. **Export local database**
   - Open SSMS
   - Right-click EventEaseDB → Tasks → Export Data-tier Application
   - Save as `.bacpac` file

2. **Import to Azure SQL**
   - Connect to Azure SQL Server in SSMS
   - Right-click Databases → Import Data-tier Application
   - Select your `.bacpac` file
   - Complete the wizard

#### Option 3: Using SqlPackage.exe

```bash
# Export local database
SqlPackage.exe /Action:Export /SourceConnectionString:"Server=(localdb)\mssqllocaldb;Database=EventEaseDB;Trusted_Connection=True;" /TargetFile:EventEaseDB.bacpac

# Import to Azure SQL
SqlPackage.exe /Action:Import /SourceFile:EventEaseDB.bacpac /TargetConnectionString:"Server=tcp:eventease-sql-server.database.windows.net,1433;Initial Catalog=EventEaseDB;Persist Security Info=False;User ID=eventeaseadmin;Password=YourStrongPassword123!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## Project Structure

```
EventEase/
├── Controllers/           # MVC Controllers
│   ├── BookingsController.cs
│   ├── EventsController.cs
│   ├── HomeController.cs
│   └── VenuesController.cs
├── Data/                  # Database Context & Migrations
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Models/                # Entity Models
│   ├── Booking.cs
│   ├── Event.cs
│   └── Venue.cs
├── Views/                 # Razor Views
│   ├── Bookings/
│   ├── Events/
│   ├── Home/
│   ├── Shared/
│   └── Venues/
├── wwwroot/               # Static files (CSS, JS, Images)
├── appsettings.json       # Configuration
├── database_script.sql    # Database creation script
├── DATABASE_DOCUMENTATION.md
└── README.md
```

## Key Features Implementation

### Double Booking Prevention

The `BookingsController` checks for overlapping bookings before creating or updating:

```csharp
var hasConflict = await _context.Bookings
    .AnyAsync(b => b.VenueId == booking.VenueId &&
                   b.BookingId != booking.BookingId &&
                   ((booking.StartDate >= b.StartDate && booking.StartDate < b.EndDate) ||
                    (booking.EndDate > b.StartDate && booking.EndDate <= b.EndDate) ||
                    (booking.StartDate <= b.StartDate && booking.EndDate >= b.EndDate)));
```

### Deletion Protection

Venues and Events with existing bookings cannot be deleted. The controller checks for related bookings before allowing deletion:

```csharp
var hasBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);
if (hasBookings)
{
    ModelState.AddModelError(string.Empty, "Cannot delete venue with existing bookings.");
}
```

### Flexible Event-Venue Relationship

Events can be created without a venue assignment, allowing events to exist before venues are confirmed. Bookings are used to finalize the venue-event relationship.

## Security Considerations

- **Connection Strings**: Use Azure Key Vault or App Service configuration for production
- **SQL Injection Protection**: Entity Framework Core parameterizes all queries
- **XSS Protection**: Razor views automatically encode output
- **CSRF Protection**: Anti-forgery tokens enabled on all forms

## Troubleshooting

### Common Issues

1. **Database connection fails**
   - Verify SQL Server is running
   - Check connection string in `appsettings.json`
   - Ensure firewall rules allow connection

2. **Migrations fail**
   - Ensure `dotnet-ef` tool is installed: `dotnet tool install --global dotnet-ef`
   - Delete Migrations folder and run `dotnet ef migrations add InitialCreate`

3. **Azure deployment fails**
   - Ensure Azure CLI is logged in: `az login`
   - Check resource quotas in your subscription
   - Verify runtime stack matches (Windows/Linux)

## License

This project was created for educational purposes as part of a cloud computing course assignment.

## Contact

For support or inquiries, contact EventEase development team.
