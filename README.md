# 🛒 Cartify E-commerce Website

Cartify is a fully functional **E-commerce web application** built with **ASP.NET MVC** using a **3-Tier Architecture** pattern.
It demonstrates clean separation of concerns with **DAL (Data Access Layer)**, **BLL (Business Logic Layer)**, and **PLL (Presentation Layer)**.

The project includes core e-commerce features such as user authentication, product management, shopping cart, checkout, and order tracking.

## Demo :
[![Watch the video](https://img.icons8.com/fluency/48/play-button-circled.png)](https://drive.google.com/file/d/1PKYsnWbrXGki5DiRdvnieB6WCovmY_p6/view?usp=sharing)


### **1. Data Access Layer (DAL)**

- **CartifyDbContext.cs** – EF Core DbContext
- **Entities/** – Database entities (Users, Products, Orders, Cart, Wishlist, Payments, etc.)
- **Repositories/** – Repository pattern (abstractions & implementations)

### **2. Business Logic Layer (BLL)**

- **Services/** – Core services (Cart, Checkout, Product, Order, Search, Wishlist, User, etc.)
- **ViewModels/** – Strongly typed models for Views
- **Helper/** – Utilities (File upload, Identity extensions, etc.)

### **3. Presentation Layer (PLL)**

- **Controllers/** – MVC Controllers (Account, Cart, Checkout, Orders, Store, Wishlist, Admin)
- **Views/** – Razor views for User and Admin dashboards
- **wwwroot/** – Static assets (CSS, JS, images, fonts)

---

## Features

- **User Authentication & Profile Management**
- **Product Management** (CRUD for Admins)
- **Shopping Cart & Wishlist**
- **Checkout & Payment Integration**
- **Order History & Tracking**
- **Search & Filtering**
- **AJAX-based dynamic content loading for smooth and seamless interactions**
- **Admin Dashboard** (Manage users, categories, orders, products)
- **Email Verification & Notifications**
- **Payment Integration with Stripe** 💳  
  - Secure online payments using Stripe.  
  - Supports credit/debit cards.  
  - Transactions are stored in the **Payments** table and linked to orders.


---

## Tech Stack

- **Backend:** ASP.NET Core MVC, C#, Entity Framework Core
- **Frontend:** Razor Views, Bootstrap, jQuery, AJAX
- **Database:** SQL Server (Entity Framework Code First)
- **Architecture:** 3-Tier Architecture (DAL, BLL, PLL)

---

## Setup Instructions

1. Clone the repository:

   ```bash
   git clone https://github.com/Hassan1Abdallah/Cartify.git
   cd Cartify
   ```

2. Open `Cartify.sln` in **Visual Studio**.
3. Update `appsettings.json` with your **SQL Server connection string**.
4. Run migrations (if not seeded automatically):

   ```powershell
   Update-Database
   ```
5. Run AddProductsQuery.sql in SQL Server to load products

6. Build and run the project.

---


