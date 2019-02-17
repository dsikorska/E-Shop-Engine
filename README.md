# E-Shop Engine
The application was created for educational purposes. 
The project implements Repository and Dependency Injection patterns.


**It shouldn't be use for any real business. I don't take any reposibility if you decide to use it for real transactions. Before you do it, make sure the application fulfill security standards.**

## Features
* Searching / paging / sorting.
* Category and Subcategory system for Products.
* Shopping Cart
* Ordering system
* Payment system (only dotPay available at the moment)
* Individual profile creation
* Profile confirmation system (email unique code)
* Administrators panel
* CRUD operations
* Logging Errors system

## Getting Started
### Prerequisites
* .NET Framework 4.7+
* SQL Server
* IIS 8.0+

### Installation guide
#### Development
* Clone / download repo.
* Run E-Shop-Engine.sln file in VS.
* In folder E-Shop-Engine.Website/App_Data/app.config set your SMTP credentials.
* In folder E-Shop-Engine.Website/App_Data/connectionString.config set your connection string.
* In Package Manager Console run at E-Shop.Engine.Services command:
```update-database```
* Build and run the solution.
* Login at default admin account: 
```login: my@email.com```
```password: Qwerty1!```
**You can change it at "Your Account" tab. Do it after you set up your SMTP settings at "Admin Panel" - the step below.**
* Go to "Admin Panel" and at tab Settings update data for your needs.

Application errors are logging in App_Data/logs.

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

## How to implement more payment methods:
* Go to E-Shop-Engine.Services -> Services -> Payment and create new folder.
* There are 2 model classes that payment can inherit from: PaymentDetails (intended to send data to external server) and PaymentResponse (intended to receive data from external server).
* Create interface for service that inherit from IPaymentService. Then create the implementation.

        public class DotPayPaymentService : IDotPayPaymentService
        {
            private static Settings settings;
    
            public DotPayPaymentService(ISettingsRepository settingsRepository)
            {
                settings = settingsRepository.Get();
            } 
		
		    // The implementation...
	    }
	
* Now go to E-Shop-Engine.Website -> Controllers -> Payment and create new Controller.
* The controller should inherit from BasePaymentController.

        public class DotPayController : BasePaymentController
        {
            public DotPayController(
                IOrderRepository orderRepository,
                ICartRepository cartRepository,
                ISettingsRepository settingsRepository,
                IMailingService mailingService,
                IDotPayPaymentService paymentService,
                IAppUserManager userManager,
                IUnitOfWork unitOfWork)
                : base(
                      orderRepository,
                      cartRepository,
                      settingsRepository,
                      mailingService,
                      paymentService,
                      userManager,
                      unitOfWork)
            {
                //
            }
	
* Register new payment method in AutoFac (E-Shop-Engine.Website -> App_Start -> AutoFacConfig)
```builder.RegisterType<DotPayPaymentService>().As<IDotPayPaymentService>().InstancePerRequest();```
* Go to Views -> Payment -> Select and add new payment. Remember to set input element value the same as Controller name
```<div class="custom-control custom-checkbox form-control-lg">```
    ```<input type="radio" name="paymentMethod" class="custom-control-input" id="dotpay" value="DotPay">```
    ```<label class="custom-control-label" for="dotpay">```
        ```<img src="~/Content/payment/dotpay_logo.jpg" class="payment-img" />```
    ```</label>```
``` </div> ```
		
## License
This project is licensed under MIT License.
