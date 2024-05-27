# School-Webapp

A clone of the popular learning management system Moodle, built with ASP.NET.

## Table of Contents

- [Installation](#installation)
- [Features](#features)
- [Usage](#usage)

## Installation

### From source **Recommended**

1. Clone the repository:

```bash 
git clone https://github.com/unownisgod/school-webapp.git
```

2. Open the solution file (`school-webapp.sln`) in Visual Studio.

3. Restore the NuGet packages:

```powershell
dotnet restore
```

4. Build the solution:

```powershell
dotnet build
```

5. Run the application:

```powershell
dotnet run
```

6. Open your web browser and navigate to `http://localhost:7023` to access the school-webapp application.

### From release

1. Download the lastest release version and execute the executable

2. Open your web browser and navigate to `http://localhost:5000` to access the school-webapp application.

## Features

The School-webapp application provides a user-friendly interface for managing courses, assignments, grades, and user profiles. Here are some key features:

- User registration and login
- Course creation and management
- Assignment creation and submission
- Gradebook and grading system
- User profile management

## Usage

There are three different roles within the platform:

### Admin

-They can enter new teachers or students and delete them
-They have access to the info of any of them, and can edit it at any time
-They can create, check, delete or edit the courses, subjects and ativities
-The admin is automatically generated after execution if no admin exists.

You can log in as admin with the Admin username and password as the password

### Teachers

-They can create, check, delete or edit the ativities
-they can evaluate students activities

You can login as a teacher with Using DGar8 as the username and DGar8-Password as pasword

### Users

-They can check and submit activities

You can login as a teacher with Using JGar10 as the username and JGar10-Password as pasword

### Disclaimer

Its highly advised to delete all the existing mock accounts before creating new ones.
The password must be changed after the first login.
