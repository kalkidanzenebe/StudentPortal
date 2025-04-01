# Student Portal System 🎓

A secure ASP.NET Core MVC web application for managing student information with authentication and CRUD operations.

## Features ✨
- **User Authentication** 🔐
  - Registration & Login/Logout
  - Session management
- **Student Management** 📚
  - Add new students
  - Edit existing records
  - Delete students
  - Real-time validation
- **Modern UI** 💅
  - Responsive design
  - Font Awesome icons
  - Smooth animations
- **Security** 🛡️
  - Secure password hashing
  - Protected routes
  - Sensitive data protection

## Technologies 🛠️
- **Backend**: ASP.NET Core 7 MVC
- **Database**: Entity Framework Core (Code First)
- **Frontend**: Bootstrap 5, Font Awesome
- **Authentication**: Cookie-based
- **Validation**: ASP.NET Core Model Validation


## Installation and Setup ⚙️
### 1. Clone the Repository
```sh
git clone https://github.com/kalkidanzenebe/StudentPortal.git
cd StudentPortal
```

### 2. Configure Database
  ```
dotnet ef database update
  ```
### 3. Configure Database
  ```
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your_connection_string"
  ```
### 3. Run the Application
```sh
dotnet run
```
### Configuration ⚙️
#### appsettings.Template.json
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
### Environment Variables
```
#Development
$env:ASPNETCORE_ENVIRONMENT = "Development"

# Production
$env:ConnectionStrings__DefaultConnection = "Server=prod;Database=Students;..."
 ```
## Usage
1. **Register a User** - Open the `/Auth/Register` page and create an account.
2. **Login** - Use the registered credentials to log in.
3. **Manage Students** - Once logged in, add, edit, or delete students.
4. **Logout** - Click the logout button to end the session.

## Contributing
If you'd like to contribute:
- Fork the repository
- Create a new branch (`git checkout -b feature-name`)
- Make your changes and commit (`git commit -m "feat: added XYZ feature"`)
- Push to GitHub and create a Pull Request


