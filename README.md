# AdExcellence - Advertising Hoarding Management System

AdExcellence is a comprehensive web application designed to streamline the process of managing and booking advertising hoardings. This platform serves three types of users: Admins, Hoarding Owners, and Advertisers, each with tailored features to meet their specific needs.

## Features

### For Advertisers
- Browse available hoardings with detailed information
- Book hoardings for specific time periods
- Manage bookings and payments
- View booking history and status
- Update profile and preferences

### For Hoarding Owners
- List and manage hoarding properties
- Set pricing and availability
- Track bookings and revenue
- Manage property details and images
- View booking requests and history

### For Admins
- Manage users (Advertisers and Hoarding Owners)
- Monitor and approve listings
- Generate reports and analytics
- Handle user verification
- System configuration and maintenance

## Technology Stack

- **Framework**: ASP.NET Web Forms
- **Database**: Microsoft SQL Server
- **Frontend**: HTML5, CSS3, Tailwind CSS
- **Backend**: C#
- **Authentication**: Custom user authentication system
- **Other Tools**: Visual Studio 2022

## Installation Requirements

1. **Development Environment**:
   - Visual Studio 2022 or later
   - .NET Framework 4.7.2 or later
   - SQL Server 2019 or later
   - SQL Server Management Studio (SSMS)

2. **Database Setup**:
   - Create a new database named `adexcel`
   - Restore the database backup file (if provided)
   - Update the connection string in `Web.config`

3. **Configuration**:
   ```xml
   <connectionStrings>
     <add name="ConnectionString" 
          connectionString="Data Source=localhost;Initial Catalog=adexcel;Integrated Security=True;TrustServerCertificate=True"
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

## Running the Application

1. Clone the repository:
   ```bash
   git clone https://github.com/laveshparyani/AdExcellence.git
   ```

2. Open the solution file `AdExcellence.sln` in Visual Studio

3. Build the solution to restore NuGet packages

4. Update the database connection string in `Web.config`

5. Run the application using IIS Express from Visual Studio

## Project Structure

- `/AdExcellence` - Main project directory
  - `/App_Code` - Common classes and utilities
  - `/css` - Stylesheets and Tailwind configuration
  - `/images` - Image assets
  - `/bin` - Compiled binaries
  - `*.aspx` - Web Forms pages
  - `*.Master` - Master pages for layout

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Authors

- Lavesh Paryani
- Kapil

## License

This project is proprietary and confidential. All rights reserved.
