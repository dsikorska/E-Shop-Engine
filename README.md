# E-Shop Engine
The application was created for educational purposes. 
The project implements Repository pattern, Dependency Injection and Separation of Concerns.
There is also Unit of Work implemented, but not used at all.

**It shouldn't be use for any real business. I don't take any reposibility if you decide to use it for real transactions. Before you do it, make sure the application fulfill security standards.**

## Getting Started
### Prerequisites
* .NET Framework 4.7+
* SQL Server
* IIS 8.0+
* Microsoft Visual Studio 2013+

### Installing
* Clone / download repo.
* Run E-Shop-Engine.sln file in VS.
* In folder E-Shop-Engine.Website/App_Data/app.config set your SMTP credentials.
* In folder E-Shop-Engine.Website/App_Data/connectionString.config set your connection string.
* In Package Manager Console run at E-Shop.Engine.Services command:
```update-database```
* Build and run the solution.

Application errors are logging in E-Shop-Engine.Website/logs.

## Built With
* .NET Framework 4.7
* ASP.NET MVC 5
* Entity Framework 6
* ASP.NET Identity 2
* AutoMapper
* AutoFac
* NLog
* X.PagedList
* Bootstrap 4
* jQuery
* AJAX
* There is example DotPay payment implemented.

## License
This project is licensed under MIT License.
