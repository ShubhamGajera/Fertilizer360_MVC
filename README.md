
# 🌱  Fertilizer360  –  Smart Fertilizer Booking System

![Made with ASP.NET Core](https://img.shields.io/badge/Made%20with-ASP.NET%20Core-blue?logo=dotnet)
![Responsive UI](https://img.shields.io/badge/Design-Figma-purple?logo=figma)
![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)

**Fertilizer360** is a modern, user-friendly **ASP.NET Core MVC** web application designed to simplify fertilizer booking and management for both users and admins. With clean Razor Pages, MVC structure, responsive design, and integrated admin controls, Fertilizer360 offers a robust and secure platform to manage fertilizer pre-bookings and user data.

---

## ✨ Features

- 🔐 Secure Login & Registration (User/Admin)
- 📋 Advance Fertilizer Booking Form (CRUD)
- 🧾 Booking History Viewer for Users
- 👤 User Profile Update with Image Upload
- 🛠️ Admin Panel (Add, Edit, Delete Fertilizer Records)
- 🌐 Fully Responsive Razor Views (Mobile, Tablet, Desktop)
- 💬 TempData-based Success & Error Notifications
- 🔎 Optional Search & Filter Capabilities

---

## 🎨  UI/UX Design (Figma)

The UI of Fertilizer360 is carefully designed in Figma for a seamless and intuitive user experience.

[![View on Figma](https://img.shields.io/badge/View%20Design-Figma-blue?logo=figma)](https://www.figma.com/design/QucTY5eVOAccKhq17niAMC/Fertilizer360?node-id=0-1&p=f&t=lZBNwc16r7aVgKRC-0)

---

## 📦 Tech Stack

| Technology                  | Purpose                                |
|-----------------------------|----------------------------------------|
| **ASP.NET Core MVC**        | Backend framework & web application    |
| **Entity Framework Core**   | ORM for database operations            |
| **SQL Server**              | Relational database                    |
| **Razor Views**             | Frontend templating                    |
| **Bootstrap 5**             | Responsive UI styling                  |
| **Identity**                | User authentication & authorization    |
| **TempData / ViewBag**      | Message passing & alerts               |
| **Figma**                   | UI/UX Design Tool                      |

---

## 🔧 Setup Instructions

### 1️⃣ Prerequisites

- Visual Studio 2022 or later
- .NET 6 or .NET 7 SDK
- SQL Server or SQL Express

### 2️⃣ Clone the Repo

```bash
git clone https://github.com/your-username/Fertilizer360.git
cd Fertilizer360
````

### 3️⃣ Restore & Build

Open the solution in Visual Studio and run:

```bash
Update-Package -reinstall
Build Solution (Ctrl + Shift + B)
```

### 4️⃣ Apply Migrations

```bash
Add-Migration InitialCreate
Update-Database
```

### 5️⃣ Run the App

Press `F5` or `Ctrl + F5` in Visual Studio to launch the app.

> App will run on `https://localhost:portnumber` or `http://localhost:portnumber`

---

## 🔐 Auth & Roles

* **Authentication**: ASP.NET Identity (Email & Password)
* **Authorization**: Role-based (`User`, `Admin`)
* **User Table**: Extended IdentityUser with profile fields

---

## 🚀 Deployment

You can deploy the app using:

* **IIS**
* **Azure App Service**
* **Docker Container**
* **Any .NET-supported hosting**

Example for production:

* Publish via **Right-click > Publish**
* Choose your target: Folder, FTP, Azure, etc.

---

## 👥 Team

* 👨‍💻 **Backend & UI Developer**: [Shubham Gajera](https://www.linkedin.com/in/shubham-gajera-2135b8268)

📧 Email: **[shubhamgajera122@gmail.com](mailto:shubhamgajera122@gmail.com)**

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙌 Acknowledgements

* ASP.NET Core Team
* Entity Framework Core Docs
* Bootstrap 5
* Figma for design collaboration

> *“Fertilizer360 brings a smart, modern touch to fertilizer management for digital-first agriculture.”*

