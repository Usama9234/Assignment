# Application Setup Instructions

## Prerequisites

1. **SQL Server Management Studio (SSMS) 19**
   - Download and install from the [official Microsoft website](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).
2. **Microsoft Visual Studio**
   - Download and install from the [official Visual Studio website](https://visualstudio.microsoft.com/).

## Configuration

1. **Update Connection String**

   - Open `appsettings.json`.
   - Update the `ConnectionStrings` section with your SQL Server instance name.

2. **Entity Framework Core Setup**

   - Open NuGet Package Manager Console in Visual Studio.
   - Install Entity Framework Core:
     ```bash
     Install-Package Microsoft.EntityFrameworkCore
     ```
   - Run the following commands to set up the database:
     ```bash
     Add-Migration InitialCreate
     Update-Database
     ```

3. **Launch Settings**
   - Verify the `launchSettings.json` file for correct configuration.

## Running the Application

1. Start the application in Visual Studio.

## Initial Setup

1. **User Registration**

   - Register with the email `admin@gmail.com` for full access to manage products.
   - Other users will have read-only access.

2. **Product Setup**
   - Create at least 6 products to test pagination functionality.
