# Clothing Store Web Application

This is a web application for a clothing store built with ASP.NET Razor Pages and SQL Entity Framework. The application allows customers to browse products, add items to their cart, and place orders. It also includes an admin portal for managing products, orders, and customers.

## Features

- User authentication and authorization for customers and admins
- Product catalog with search and filtering options
- Product details page with images and descriptions
- Shopping cart for managing items
- Checkout process with shipping and payment options
- Admin portal for managing products, orders, and customers
- SQL Entity Framework for data storage and retrieval

## Getting Started

To get started, you can clone the repository and open the solution file in Visual Studio.

### Prerequisites

- Visual Studio
- .NET 6.0
- Microsoft SQL Server

### Installing

1. Clone the repository
2. Open the solution file in Visual Studio
3. Go to 'appsettings.json' and configure your preferred SQL connection string under 'DefaultConnection'
3. Run the following command in the Package Manager Console to create the database:
```
Update-Database
```
4. Build the solution
5. Run the application

## Usage

### User

As a normal user (or customer), you can browse the product catalog and add items to your shopping cart. When you're ready to checkout, you'll need to create an account or log in if you already have one. You'll then be prompted to enter your shipping and payment information before submitting your order.

### Admin

As an admin, you can manage the product catalog, view and edit customer orders, and manage customer accounts. To access the admin portal, you'll need to log in with an admin account.

## Contributing

If you would like to contribute, feel free to open an issue or pull request on the repository.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
