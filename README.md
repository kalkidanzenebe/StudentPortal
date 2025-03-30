# Student Portal Web Application

## Overview
The **Student Portal** is a web application built using **ASP.NET Core MVC, Entity Framework Core, and SQL Server**. It allows users to register, log in, and manage student records.

## Features
- **User Authentication** (Register, Login, Logout)
- **Student Management** (Add, Edit, Delete, List)
- **Session-Based Authorization** (Only logged-in users can access student records)
- **Responsive UI** built with Bootstrap
- **Data Persistence** using SQL Server with Entity Framework Core

## Technologies Used
- **ASP.NET Core MVC** - Backend framework
- **Entity Framework Core** - ORM for database interactions
- **SQL Server** - Database
- **Bootstrap** - Frontend UI framework
- **Git & GitHub** - Version control

## Installation and Setup
### 1. Clone the Repository
```sh
git clone https://github.com/kalkidanzenebe/StudentPortal.git
cd StudentPortal
```

### 2. Configure Database
- Open `appsettings.json` and update your database connection string.
- Run the following command to apply migrations:
  ```sh
  dotnet ef database update
  ```

### 3. Run the Application
```sh
dotnet run
```

The application should now be running on `https://localhost:5001/` (or the configured port).

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

## License
This project is licensed under the MIT License.

