# eShop
**eShop** is a sample full-stack e-commerce project built with ASP.NET Core and React, following a layered architecture.

## Project Overview
eShop features a customer-facing site, an admin interface, a backend API, and an identity server. It leverages ASP.NET Core, React, and OpenIddict to implement essential ecommerce functionality. Authentication is managed through a dedicated identity server (eShop.Identity) using OpenIddict, which handles user authentication and authorization for both the customer and admin interfaces.

## Architecture
```
├── eShop.API          # ASP.NET Core Web API
├── eShop.Business     # Business Logic Layer
├── eShop.Data         # Data Access Layer (EF Core)
├── eShop.Admin        # React TypeScript Admin Panel
├── eShop.Web          # ASP.NET Core MVC Customer Site
├── eShop.Identity     # OpenID Connect Identity Server
└── eShop.Shared       # Common DTOs & Contracts
```

## Features
### Customer Features
- Product catalog with search, filtering, and pagination
- Category-based product organization
- Shopping cart (session-based for guests, persistent for users)
- Product reviews and ratings
### Admin Features
- Dashboard with analytics overview
- Complete product management (CRUD, images, variants, attributes)
- Category management with custom attributes
- User management
- Review monitoring

## Technologies Used
- **Backend**: ASP.NET Core, EF Core, AutoMapper, OpenIddict
- **Database**: MS SQL Server
- **Admin Front-end**: React, TypeScript, Vite, Redux, RTK Query, React Hook Form, Zod, React Router DOM.
- **Customer Front-end**: ASP.NET Core MVC, Razor Pages (Tag Helpers, View Components, Partial Views)
- **Authentication**: OpenIddict
- **Image Storage**: Cloudinary DotNet

## Setup and Installation
1. Clone the repository:
```bash
git clone https://github.com/yourusername/eShop.git
```
2. Install dependencies:
- Install .NET Core SDK and Node.js.
- For `eShop.Admin`: `cd eShop.Admin && npm install`.
3. Set up the database:
- Configure the connection string in `appsettings.json` or use `dotnet user-secrets`.
- Run migrations: `dotnet ef database update --project .\src\eShop.Data\ --startup-project .\src\eShop.API\`.
4. Run the applications:
- Start `eShop.API`, `eShop.Identity`, and `eShop.Web` via `dotnet run`.
- For `eShop.Admin`: `npm run dev`.