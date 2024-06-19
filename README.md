## About the project

This API, developed using .NET 8, adopts Domain-Driven Design (DDD) principles to provide a structured and effective solution for personal expense management. The primary objective is to allow users to register their expenses, detailing information such as title, date and time, description, amount, and payment type, with data securely stored in a MySQL database.

The API architecture is based on REST, utilizing standard HTTP methods for efficient and simplified communication. Additionally, it is complemented by Swagger documentation, which provides an interactive graphical interface for developers to explore and test endpoints easily.

### Features

- **Domain-Driven Design (DDD)**: Modular structure that enhances understanding and maintenance of the application domain.
- **Unit testing**: Comprehensive tests with FluentAssertions to ensure functionality and quality.
- **Reporting generation**: Capability to export detailed reports to PDF and Excel, offering visual and effective expense analysis.
- **RESTful API and Swagger documentation**: Documented interface facilitating integration and testing by developers.

### Built with

![badge-dot-net]
![badge-swagger]

## Getting Started

To get a local copy up and running, follow these simple steps.

### Requirements

* Visual Studio 2022+ or Visual Studio Code
* Windows 10+ or Linux/MacOS with .NET SDK installed
* PostgreSQL

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/vitor2233/CashFlow.git
    ```

2. Fill in the required information in the `appsettings.Development.json` file.
3. Run the API



<!-- Links -->
[dot-net-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

<!-- Badges -->
[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge