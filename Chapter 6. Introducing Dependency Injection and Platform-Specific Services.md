# Introducing Dependency Injection and Platform-Specific Services

In the last chapter, we introduced navigation and Shell in .NET Multi-platform App UI (.NET MAUI),
and we completed the navigation design of our app. We improved two interfaces, IDataStore
and IUserService, to separate the model from the view and view model. In the current code, we
used the DependencyService class to decouple the interface implementations. In this chapter,
we will refine our design using dependency injection (DI) to replace the DependencyService
class. There is a built-in service to support DI in .NET MAUI. With the help of DI, we can refine our
design and decouple the dependencies in a more elegant way.

We will cover the following topics in this chapter:

* A quick review of design principles

* Using DI

* Connecting to the database

DI is a technique to realize the design principle of dependency inversion or Dependency Inversion
Principle (DIP). DIP is one of the SOLID design principles, and we will learn how to use SOLID
design principles in our design. We will have a quick overview of SOLID design principles at the
beginning of this chapter before we dive into DI.

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC. Please refer to the Development environment setup section in Chapter 1, Getting Started with
.NET MAUI, for further details.

The source code for this chapter is available in the following GitHub repository:
https://github.com/PacktPublishing/.NET-MAUI-Cross-PlatformApplication-Development/tree/main/Chapter06
The source code can be downloaded using the following Git command:

```
git clone -b chapter06 https://github.com/PacktPublishing/.
NET-MAUI-Cross-Platform-Application-Development PassXYZ.Vault2
```
# A quick review of design principles

Design principles are high-level guidelines about design considerations. They can give fundamental
pieces of advice for you to make a better design decision. There are general design principles that can
be used not only for software design but are also applicable to other design works.
Let’s review some general design principles before we move to commonly used design principles
(SOLID) in software development.

# Exploring types of design principles

Design principles can be a huge topic. So, instead of a detailed description, here, I will share my
experience of applying design principles in development by giving you a quick review of the design
principles used in this book. We will start with high-level principles such as DRY, KISS, YAGNI, and
so on first, and then we move to the ones that are mostly used in software development. The most
commonly used ones in object-oriented programming (OOP) are SOLID design principles.

# Don’t Repeat Yourself (DRY)

As people often say, don’t reinvent the wheel; we should try to reuse existing components rather than
redevelop something that already exists.

# Keep It Simple, Stupid (KISS)

We should choose a simple and straightforward approach rather than involve unnecessary complexity
in a design.

# You Aren’t Gonna Need It (YAGNI)

We should implement functionality when it is required. In software development, there is a tendency
to futureproof a design. This may create something that is actually not needed and increase the
complexity of the solution.

# SOLID design principles

SOLID design principles are the ones used in software development. They are high-level guidelines
for many design patterns. SOLID is an acronym for the following five principles:

* Single Responsibility Principle (SRP)—A class should only have one responsibility. As a developer,
you have one and only one reason to change a class. With this design principle in mind, the
implementation is easier to understand and more efficient to deal with as requirements change.

* Open/Closed Principle (OCP)—Classes should be open for extension but closed for modification.
The main idea of this principle is to keep the existing code from breaking when you implement
new features.

* Liskov Substitution Principle (LSP)—If the object of the parent type can be used in a context,
the object of the child type should be able to be used the same way without anything breaking.

* Interface Segregation Principle (ISP)—A design should not implement an interface that
it doesn’t use, and a class should not be forced to depend on methods it doesn’t intend to
implement. We should design concise and simple interfaces rather than large and complex ones.

* Dependency Inversion Principle (DIP)—This principle emphasizes the decoupling of software
modules. High-level modules should not depend on low-level modules directly. Both should
depend on abstractions. Abstractions should not depend on details. Details should depend
on abstractions.

Design principles are guidelines to help us to make better design decisions. However, it is up to us to
decide what to do in the actual implementation, not the design principles.
Using design principles

Now that we’ve talked about the different design principles, let me share my lessons learned when
using design principles.

In the model of our app, I reused KeePassLib from Dominik Reichl. When I ported it to .NET
Standard, I changed the inheritance hierarchy, as shown in Figure 6.1:

![image](https://user-images.githubusercontent.com/26972859/231832205-e68ca92c-915b-4871-9bb8-6bb08aaa91e1.png)

Figure 6.1: Class diagram of Item, PwEntry, and PwGroup

I created an abstract parent class, Item, for the group (PwGroup) and entry (PwEntry). It looks
like this change breaks OCP in SOLID. The reason that I did it this way is influenced by a lesson I
learned in the previous implementation.
So, previously I did not implement KPCLib this way before version 1.2.3. At that time, I used PwGroup
and PwEntry directly, so I had to handle groups and entries separately. You can imagine the increased
complexity in ItemsPage and ItemsViewModel. The most important side effect is that I couldn’t
separate the model and view model clearly. In the view model, I had to handle a lot of details using
KeePassLib directly. After the Item abstract parent class is introduced, I can hide most of the
detailed implementation in services (IDataStore and IUserService) and PassXYZLib. No
code that is dependent on KeePassLib is present in the view and view model anymore. The thought
behind this change is influenced more by KISS rather than just sticking to OCP. The result is that the
overall architecture looks much better if we consider other SOLID principles, such as LSP and SRP.
The point that I want to share here is that we may find conflicts among various design principles in
the actual work. It’s up to us to make decisions rather than just sticking to design principles. The best
design decision usually comes from the lessons learned in previous failure cases.
Now, coming back to our primary topic, we’ll talk about refining design using one of the SOLID
principles—dependency inversion. In SOLID design principles, dependency inversion emphasizes the
decoupling of software modules, and it also gives guidelines about how to do it. The key idea behind
it is we should try to depend on abstractions whenever possible. In the actual implementation, DI is
the technique that we use frequently to apply the idea of dependency inversion.

# Using DI

DI is one of the tools that we can use in .NET MAUI. It is actually not something new, and it has been
used heavily in backend frameworks such as ASP.NET Core or the Java Spring Framework. DI is a
technique for achieving dependency inversion (DIP). It can help to decouple the usage of an object
from its creation so that we don’t have to depend on the object we use directly. In our app, after we
decouple the implementation of the IDataStore interface, we can start with a mock implementation
first and replace it with the actual implementation later.
In .NET MAUI, Microsoft.Extensions.DependencyInjection—or MS.DI, as we shorten
the name in this chapter—is a built-in service that we can use directly.
In the .NET world, there are many DI containers available other than MS.DI. They may be more powerful
and flexible than MS.DI, such as the Autofac DI container or the Simple Injector DI container, and
so on. Then, why do we choose MS.DI instead of other powerful and flexible DI containers? Here, we
may want to think about KISS and YAGNI principles again. We should not choose a more powerful
solution by assuming that we may use some features in the future. The simplest and easiest solution is
to use what we already have without any extra effect. With MS.DI, we don’t have to involve any extra
dependencies. Irrespective of whether we want to use it or not, it is already there in the configuration
of .NET MAUI. We can just add a few lines of code to make our design better. For other DI containers,
it may be better at imagining the future, but we have to introduce additional dependencies and do the
necessary configuration in our code before we can really use them. If you are working on a complex
system design, you may want to evaluate the available DI containers and choose the right one for
your system. For our case, PassXYZ.Vault is a relatively simple app, and we won’t benefit directly
from the advanced DI features provided by Autofac or Simple Injector. The functionalities provided
by MS.DI are sufficient for our implementation.
Before we move to the topic of DI, let’s look at our current implementation first. Instead of using DI,
we are using DependencyService from Xamarin.Forms in our app to decouple the interface and its
implementation. We will refine our code to replace DependencyService with DI in this chapter.

# Dependency Service

In Chapter 2, Building Our First .NET MAUI App, we created our app from a Xamarin.Forms template
and migrated it to .NET MAUI. Whatever works in Xamarin.Forms can still work in .NET MAUI,
including Dependency Service.
DependencyService is a service locator that enables Xamarin.Forms applications to invoke native
functionality from shared code, but it can also be used to play a DI container role for simple use cases.
The module that we want to decouple in our app is the model layer, which is a third-party library
from KeePass. As we can see in the package diagram in Figure 6.2, our system includes three separate
assemblies: KPCLib, PassXYZLib, and PassXYZ.Vault:

![image](https://user-images.githubusercontent.com/26972859/231832524-56f7ccd7-7d65-404e-91a3-da69a899f42d.png)

Figure 6.2: Package diagram

The KPCLib package includes two namespaces, KeePassLib and KPCLib. PassXYZLib is a
package to extend the functionality of the KPCLib package with .NET MAUI-specific implementation.
PassXYZ.Vault is our app that depends on the PassXYZLib package directly and depends
on the KPCLib package indirectly. According to the DI principle, we want to create dependencies
on abstractions rather than actual implementations. We created two interfaces, IDataStore and
IUserService, which we can use to decouple from the actual implementations.
Using DependencyService includes two steps—registration and resolution. Let’s look at this in
more detail:
1. Registering a service
We need to register the IDataStore and IUserService interfaces first before can we use
them. In the current code, they are registered in the constructor of the App class, as shown here:

```Csharp
DependencyService.Register<MockDataStore>();
DependencyService.Register<UserService>();
```
We can use the Register() method of Dependency Service to register the implementation
of services. The MockDataStore and UserService classes implement IDataStore and
IUserService interfaces. MockDataStore is not the actual implementation, and it is used
for testing purposes only. We will replace it with the actual implementation later in this chapter.
This is one of the benefits that we can see after we decouple from the actual implementation.

2. Resolving a service
To resolve the dependency in our code, we defined a DataStore public property of type
IDataStore in the BaseViewModel class, as shown here:

```Csharp
public static IDataStore<Item> DataStore =>
 DependencyService.Get<IDataStore<Item>>();
```
We can use the Get() method of DependencyService to resolve the dependency. Since
BaseViewModel is the parent class of all other view models, we can access the IDataStore
interface in all view models with this setup.
For the IUserService interface, we created a LoginUser singleton class and defined a
UserService public property in the LoginUser class, as shown here:
```Csharp
public static IUserService<User> UserService =>
 DependencyService.Get<IUserService<User>>();
```

This is the current implementation of DependencyService in our app. Let’s replace it with DI
supported in MS.DI.

# Using built-in MS.DI DI service

To use MS.DI as a DI service, the usage is similar to DependencyService, which includes two
steps—registration and resolution. We can use the ServiceCollection class for the registration
and the ServiceProvider class for the resolution, as shown in Figure 6.3:

![image](https://user-images.githubusercontent.com/26972859/231832981-8c00d2f4-4332-4648-8e15-d6f6b592ad72.png)

Figure 6.3: Usage of MS.DI

If we want to use DI to resolve the IDataStore service, we can use the steps in the following
code block:

```Csharp
// Registration
var services = new ServiceCollection(); ❶
services.AddSingleton <IDataStore<Item>, MockDataStore>();❷
// Resolution
ServiceProvider provider =
 services.BuildServiceProvider(validateScopes: true); ❸
IDataStore<Item> dataStore =
 provider.GetRequiredService<IDataStore<Item>>(); ❹
```
❶We need to create an instance of the ServiceCollection class first. ServiceCollection
implements the IServiceCollection interface.
❷ The IServiceCollection interface itself does not define any method directly. There are a set of
extension methods defined in MS.DI. We can use the AddSingleton() extension method to register
the concrete MockDataStore class for the IDataStore abstraction. The AddSingleton()
method can use a generic type to define the interface and the implementation. There are multiple
overloaded versions of the AddSingleton() extension method available.
❸ To resolve objects, we can get an instance of ServiceProvider by calling the
BuildServiceProvider() extension method of IServiceCollection. The
ServiceProvider class implements the IServiceProvider interface. The IServiceProvider
interface is defined in the System namespace and it defines only one method, GetService().
The rest of the methods are extension methods defined in the Microsoft.Extensions.
DependencyInjection namespace, as we can see in Figure 6.3.
❹ Once we have an instance of ServiceProvider, we can resolve the IDataStore interface
using the GetRequiredService() extension method.

Even though MS.DI is a lightweight DI service, it provides enough features for .NET MAUI applications,
as set out here:

  * Lifetime management of instances

  * Constructor, method, and property injections

  Let’s explore these features in the next sections.

## Lifetime management

  We can manage the lifetime of instances by configuring ServiceCollection.

  To configure ServiceCollection, we can use the following three extension methods:

  * AddSingleton—This method creates a single instance throughout the life of the application.
It creates an instance for the first time and reuses it in the following calls.

  * AddTransient—This method creates an instance for each call. The lifetime of the instance
depends on the scope of the programming logic.
 
  * AddScoped—The lifetime of the instance resolved by this method is within the scope defined
by the design. It creates one instance and reuses the same instance within the defined scope.
In ASP.NET, we can define the scope as the session of each HTTP request.
To explain the lifetime management of MS.DI, we can review the following code snippet, together
with Figure 6.4:

```Csharp
var services = new ServiceCollection();
services.AddSingleton< IUserService<User>, UserService>();❶
services.AddScoped<IDataStore<Item>, DataStore>(); ❶
services.AddTransient<ItemsViewModel>(); ❶
ServiceProvider rootContainer =
 services.BuildServiceProvider(validateScopes: true); ❷
var userService =
 rootContainer.GetRequiredService<IUserService<User>>();
IServiceScope scope1 = rootContainer.CreateScope(); ❸
 IDataStore<Item> dataStore1 =
 scope1.ServiceProvider.GetRequiredService<IDataStore<Item>>
 ();
IServiceScope scope2 = rootContainer.CreateScope(); ❸
IDataStore<Item> dataStore2 = Scope2.ServiceProvider.
 GetRequiredService<IDataStore<Item>>
 ();
```
In the preceding code, ❶ we registered IUserService as a Singleton object, IDataStore
as a Scoped object, and ItemsViewModel as a Transient object.

After the registration, ❷ we created a ServiceProvider instance and stored it in a rootContainer
variable. ❸ We created two scopes, scope1 and scope2, using the rootContainer. We can
review the lifetime management of the created objects in Figure 6.4:

![image](https://user-images.githubusercontent.com/26972859/231833570-391b041a-d9ba-4f91-84d2-9d808907326c.png)

Figure 6.4: Lifetime management in MS.DI

userService is created as a Singleton object, so there is only one instance, and the instance has
the same lifetime as the application itself. The two scopes that are scope1 and scope2 have their
own lifetimes that are decided by our design. The Scoped objects, dataStore1 and dataStore2,
have the same lifetime as the scope to which they belong. The instances of ItemViewModel are
Transient objects.
For each of these three methods, AddSingleton(), AddScoped(), and AddTransient(), multiple
overloaded versions are defined to meet various requirements in the ServiceCollection configuration.
In our application, we have two versions of the IDataStore interface implementation:
1. DataStore—This is the actual implementation
2. MockDataStore—This is the one used for testing purposes

Using MS.DI, we can use MockDataStore in the Debug build and use DataStore in the
Release build. This configuration can be implemented as shown in the following code snippet:

```Csharp
bool isDebug = false;
var services = new ServiceCollection();
services.AddSingleton<DataStore, DataStore>();
services.AddSingleton<MockDataStore, MockDataStore>();
services.AddSingleton<IDataStore<Item>>(c => {
 if (isDebug)
 {
 return c.GetRequiredService<MockDataStore>();
 }
 else
 {
 return c.GetRequiredService<DataStore>();
 }
});
```

In the preceding code snippet, we can configure concrete classes and DataStore, MockDataStore,
and IDataStore interfaces for different build configurations. In the configuration of IDataStore,
we can use a delegate to resolve the object. The isDebug variable can be set using the build configuration
so that it can be set to true/false according to whether it is a debug or release build.
Configuring DI in .NET MAUI
MS.DI is implemented as part of the .NET release, so it is available for all kinds of applications in .NET
5 or later releases. We can implement DI using ServiceCollection and ServiceProvider,
as we introduced in the previous section. However, there is a much simpler way to use MS.DI in .NET
MAUI. DI is integrated as part of the .NET Generic Host configuration, so we don’t need to create an
instance of ServiceCollection by ourselves. We can use the preconfigured DI service directly
without any extra work.
To understand the preconfigured DI service in .NET MAUI, we can review the .NET MAUI application
startup process again using Figure 6.5. Figure 6.5 includes both a class diagram and a sequence diagram
of the involved classes:

![image](https://user-images.githubusercontent.com/26972859/231833825-7bfccde8-0f92-4377-9d36-93c3d2e41faf.png)

At the top of Figure 6.5, we can see that there are four classes involved in the .NET MAUI application startup:
1. Platform entry point—The entry point of the .NET MAUI application is in platform-specific code.
For the .NET MAUI project, it is in the Platforms folder. There are different classes defined
for different platforms, as we can see in Table 6.1. In Figure 6.5, we use the MauiApplication
Android version as an example:

|Platform|Entry point class|
|:-------|:----------------|
|Android|MauiApplication|
|iOS/macOS|MauiUIApplicationDelegate|
|Windows|MauiWinUIApplication|

Table 6.1: Entry points in different platforms

All entry-point classes implement the IPlatformApplication interface, as we can see
in the following code snippet:

```Csharp
public interface IPlatformApplication
{
 static IPlatformApplication? Current { get; set; }
 IServiceProvider Services { get; }
 IApplication Application { get; }
}
```
IServiceProvider is defined as part of this interface, so we can use it directly to resolve
DI objects once the application is initialized.

MauiProgram ❶—As we can see in the code of the MauiProgram implementation
shown next, each .NET MAUI app needs to define a static MauiProgram class and create
a CreateMauiApp() method. The CreateMauiApp() method is invoked by the
following override function, which is defined in all platform entry points. The return value is
a MauiApp instance:

```Csharp
protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
```
MauiApp ❷—Inside CreateMauiApp(), it creates a MauiAppBuilder instance by
calling MauiApp.CreateBuilder().

MauiAppBuilder ❸—MauiAppBuilder includes a Services attribute of the
IServiceCollection interface type. We can use it to configure DI for the .NET MAUI app

From the previous analysis of the .NET MAUI app startup process, we can see that both
IServiceCollection and IServiceProvider have been initialized during the startup
process, so we can use them directly without further configuration.

We can refer to the following code snippet when we analyze the startup process. We registered two
abstractions, IDataStore and IUserService, and a LoginUser class. They are all singleton objects:

```Csharp
public static class MauiProgram { ❶
 public static MauiApp CreateMauiApp() { ❷
 var builder = MauiApp.CreateBuilder(); ❸
 builder
 .UseMauiApp<App>()
 .ConfigureFonts(fonts => {
 fonts.AddFont("fa-regular-400.ttf",
 "FontAwesomeRegular");
fonts.AddFont("fa-solid-900.ttf",
 "FontAwesomeSolid");
 fonts.AddFont("fa-brands-400.ttf",
 "FontAwesomeBrands");
 fonts.AddFont("OpenSans-Regular.ttf",
 "OpenSansRegular");
 fonts.AddFont("OpenSans-SemiBold.ttf",
 "OpenSansSemiBold");
 });
 builder.Services.AddSingleton<IDataStore<Item>,
 DataStore>();
builder.Services.AddSingleton<IUserService<User>,
 UserService>();
 builder.Services.AddSingleton<LoginUser, LoginUser>();
 return builder.Build();
 }
}
```
Once we have configured the interfaces and class, we can use them in our implementation. We can use
the IServiceProvider interface to resolve objects. When we implement DI, there are three ways
of injecting dependencies. We can use constructor injection, method injection, or property injection.
Let’s see how we can do it using the most common methods in the next two sections.

# Constructor injection

In constructor injection, the dependencies required for the class are provided as arguments to
the constructor. We can resolve dependencies using the constructor. This requires the registered
dependency to have a parameterless constructor. In our code, the LoginUser class depends on the
IuserService interface. The concrete class implementing IUserService is UserService,
which defines a parameterless constructor. We can define the constructor of LoginUser as follows:

```Csharp
private IUserService<User> _userService;
public LoginUser(IUserService<User> userService)
{
_userService = userService ?? throw new
 ArgumentNullException(nameof(userService));
 _userService.CurrentUser = this;
}
```
In the constructor of LoginUser, we list the dependency as a userService parameter. In this
case, MS.DI will resolve IUserService as the configured UserService concrete class for us.

# Property injection

There are many cases in which we won’t be able to use constructor injection. In these cases, we can
resolve the dependencies through IServiceProvider.
In the .NET MAUI application, the hosting environment creates an IServiceProvider interface
for us, as we can see in Figure 6.5. We can use the IPlatformApplication interface defined in
the platform-specific entry points to get the IServiceProvider interface, as shown in Listing 6.1:

**Listing 6.1: ServiceHelper.cs (https://epa.ms/ServiceHelper6-1)**

```Csharp
namespace PassXYZ.Vault.Services;
public static class ServiceHelper
{
 public static TService GetService<TService>()
 => Current.GetService<TService>(); ❷
 public static IServiceProvider Current => ❶
#if WINDOWS10_0_17763_0_OR_GREATER
 MauiWinUIApplication.Current.Services;
#elif ANDROID
 MauiApplication.Current.Services;
#elif IOS || MACCATALYST
 MauiUIApplicationDelegate.Current.Services;
#else
 null;
#endif
}
```
❶ In the ServiceHelper class, we define a Current static variable to keep the reference of
IServiceProvider, which is from the IPlatformApplication interface in platform
entry points.
❷ A GetService() static method is defined that calls the GetService() method
of IServiceProvider.

```Csharp
ServiceHelper
For the ServiceHelper implementation, I referred to the MauiApp-DI GitHub project.
Thanks for James Montemagno’s sample code in GitHub!
https://github.com/jamesmontemagno/MauiApp-DI
```
We can update our source code to replace Dependency Service with DI with the help of
ServiceHelper. In BaseViewModel.cs, we replaced DependencyService with DI, as
shown next.
So, we replaced the following code:
  
```Csharp
public static IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
```
This is what we replaced it with:

```Csharp
public static IDataStore<Item> DataStore => ServiceHelper.GetService<IDataStore<Item>>();
```

You may feel that the preceding code of property injection doesn’t look elegant compared to constructor
injection. I haven’t figured out a better way to do this in .NET MAUI. However, in the next part of
this book, when we introduce the Blazor Hybrid app, we can resolve property injection using the C#
attribute. To resolve the IDataStore interface in Blazor, we can do it in a much simpler way, as
shown here:

```Csharp
[Inject]
public IDataStore<Item> DataStore { get; set; } = default!;
```
We can use the [Inject] C# attribute to resolve the dependency implicitly without calling the
GetService() method of ServiceHelper explicitly.
When we move from Dependency Service to DI, we will create another concrete class for the
IDataStore interface. This class will handle the CRUD operations of the password database.

# Connecting to the database

The password database is a local database in KeePass 2.x format. Inside the database, password data
is stored as groups and entries. In the KeePassLib namespace, a PwDatabase class is defined
to manage database operations. We can refer to the class diagram in Figure 6.6 to understand the
relationship between PwDatabase, PwGroup, and PwEntry:

![image](https://user-images.githubusercontent.com/26972859/231835206-6017c155-681c-49b7-bce1-4a5f89e6fba0.png)

Figure 6.6: Class diagram of KeePass database

In PwDatabase, a RootGroup property of type PwGroup is defined. It contains all groups and entries
stored in the database. We can navigate the data structure of the KeePass database from RootGroup
to a particular entry. In PwEntry, a set of standard fields is defined, as shown in Figure 6.7:

![image](https://user-images.githubusercontent.com/26972859/231835346-b1a3cf68-d9c9-4fb3-ae9a-618dd4e615f2.png)

Figure 6.7: Group, entry, and field

If we have a list of entries that include only standard fields, this list looks like a table. In Figure 6.7, the
current group includes five entries (GitHub, Google, Facebook, Instagram, and Chase Bank) and a
sub-group (Cloud). On the left, there is a screenshot of ItemsPage, which shows the items in the
current group. If the Google item was selected, it would be displayed as an entry in the screenshot
on the right-hand side. Users may add additional fields to the entry, so the KeePass database is not
a relational database—it is more like a key-value database. Each key-value pair is a field, such as a
URL field.
To use PwDatabase in our app, a derived class, PxDatabase, is defined. PxDatabase added
additional properties and methods such as CurrentGroup, DeleteGroup(), DeleteEntry(),
and so on.
To access a database, we can open the database file and perform CRUD operations on it. Since we
are building a cross-platform app, it is not convenient to handle the database file directly for the end
users. In PassXYZ.Vault, the user concept is used instead of a data file. In PassXYZLib, a User
class is defined to encapsulate the underlying file operations.
To access the database, we defined database initialization and CRUDL operations in the IDataStore
and IUserService interfaces. The DataStore and UserService concrete classes are used
to implement these two interfaces.

# Initializing the database
  
The initialization of the database is part of the login process, so the following login method is defined
in the IUserService interface:

# ```Task<bool> LoginAsync(T user)```;
  
In the UserService class, LoginAsync() is defined as an async method, as we can see here:

```Csharp
public async Task<bool> LoginAsync(User user) {
 if (user == null) {
Debug.Assert(false); throw new ArgumentNullException("user");
 }
 return await Task.Run(() => { ❶
 if (string.IsNullOrEmpty(user.Password)) { return false; }
 db.Open(user); ❷
 if (db.IsOpen) {
 db.CurrentGroup = db.RootGroup;
 }
 return db.IsOpen;
 });
}
```
In LoginAsync(), ❶ a separate task is used to handle the open operation of the database. The
Open() ❷ method of PxDatabase is called, and an instance of the User class is passed to the
Open() method as an argument.

# Performing CRUD operations

The data operation of the KeePass database is similar to the CRUD operations in a relational database.
Once we log in and connect to a database, we can access our password data. The first step is to retrieve
a list of items. After login, the first list is retrieved from the root group. There is a read-only property,
RootGroup ①, which is defined in the IDataStore interface, as we can see in the following code
snippet. Later, when the user navigates to a different group, the CurrentGroup ② property is used
to keep the current location in the navigation:

**Listing 6.2: IDataStore.cs (https://epa.ms/IDataStore6-2)**

```Csharp
public interface IDataStore<T> {
 #region DS_misc
 T CurrentGroup { get; set; } ②
 string CurrentPath { get; }
 T RootGroup { get; } ①
 bool IsOpen { get; }
 string GetStoreName();
 DateTime GetStoreModifiedTime();
 Task<bool> MergeAsync(string path);
 ObservableCollection<PxIcon> GetCustomIcons(...);
 Task<bool> DeleteCustomIconAsync(PxIcon icon);
 ImageSource GetBuiltInImage(PxIcon icon);
 #endregion
 DS_Item ...
}
```

# Adding an item

The first operation in CRUD is a create or add operation. We can add an item that can be an entry or a
group to the current group. The user interface for an add operation is a toolbar item in ItemsPage,
as shown here:

```Csharp
<ContentPage.ToolbarItems>
 <ToolbarItem Text="{x:Static resources:Resources.action_id_add}" Command="{Binding AddItemCommand}">
 <ToolbarItem.IconImageSource>
 <FontImageSource FontFamily="FontAwesomeSolid"
 Glyph="{x:Static styles:FontAwesomeSolid.Plus}"
 Color="{DynamicResource SecondaryColor}"
 Size="16" />
 </ToolbarItem.IconImageSource>
 </ToolbarItem>
</ContentPage.ToolbarItems>
```
We can see a toolbar item icon is shown in the top-right corner of ItemsPage in Figure 6.8:

![image](https://user-images.githubusercontent.com/26972859/231835897-4c0bcfd6-f1fa-4bc4-acd1-e8f88a6351f3.png)

 Figure 6.8: Adding an item

---
When the Add button is clicked, it invokes the AddItemCommand command defined in
ItemsViewModel through data binding.
The AddItemCommand command invokes the following OnAddItem() method in the view model:
---

**Listing 6.3: ItemsViewModel.cs (https://epa.ms/ItemsViewModel6-3)**

```Csharp
private async void OnAddItem(object obj) {
 string[] templates = {
Properties.Resources.item_subtype_group,
Properties.Resources.item_subtype_entry,
Properties.Resources.item_subtype_notes,
Properties.Resources.item_subtype_pxentry
 };
 var template = await Shell.Current.DisplayActionSheet(
Properties.Resources.pt_id_choosetemplate,
Properties.Resources.action_id_cancel, null,
 templates); ❶
 ItemSubType type = ItemSubType.None;
 if (template == Properties.Resources.item_subtype_entry) {
type = ItemSubType.Entry;
 } else if (template == Properties.Resources.item
 _subtype_pxentry){
type = ItemSubType.PxEntry;
 } else if (template == Properties.Resources.
 item_subtype_group) {
type = ItemSubType.Group;
 } else if (template == Properties.Resources.item
 _subtype_notes) {
type = ItemSubType.Notes;
 } else if (template == Properties.Resources.action
 _id_cancel) {
type = ItemSubType.None;
 } else {
type = ItemSubType.None;
 }
if (type != ItemSubType.None) {
var itemType = new Dictionary<string, object> ❷
{
 { "Type", type }
};
await Shell.Current.GoToAsync(nameof(NewItemPage),
 itemType); ❸
 }
}
```
❶ In this OnAddItem() function, ActionSheet is displayed to let the user choose an item type.
The item type can be a group or an entry.
❷ Once we get the item type, we can build a dictionary with the item type and the name of the query
parameter. We store this object of the dictionary in an itemType variable.
❸ This itemType variable can be passed to NewItemPage as a query parameter. In Chapter 5,
Introducing Shell and Navigation, we learned how to pass a string value as a query parameter to a
page in Shell navigation. Here, we can pass an object as a query parameter to a page after we wrap it
in a dictionary.
To add a new item, the user interface is defined in NewItemPage, and the logic is processed in
NewItemViewModel. Let’s review the implementation of NewItemViewModel in Listing 6.4:

**Listing 6.4: NewItemViewModel.cs (https://epa.ms/NewItemViewModel6-4)**

```Csharp
using KPCLib;
using PassXYZLib;
namespace PassXYZ.Vault.ViewModels;
[QueryProperty(nameof(Type), nameof(Type))] ①
public class NewItemViewModel : BaseViewModel {
private string text;
private string description;
private ItemSubType _type = ItemSubType.Group;
public NewItemViewModel() {
 SaveCommand = new Command(OnSave, ValidateSave);
CancelCommand = new Command(OnCancel);
 this.PropertyChanged +=
 (_, __) => SaveCommand.ChangeCanExecute();
 Title = "New Item";
}
private void SetTitle(ItemSubType type)...
private bool ValidateSave()...
public ItemSubType Type { ②
 get => _type;
 set {
 _ = SetProperty(ref _type, value);
 SetTitle(_type);
 }
 }
public string Text...
public string Description...
public Command SaveCommand { get; }
public Command CancelCommand { get; }
private async void OnCancel() {
 await Shell.Current.GoToAsyc(".."); }
private async void OnSave() {
 Item? newItem = DataStore.CreateNewItem(_type); ③
 if (newItem != null) {
 newItem.Name = Text;
 newItem.Notes = Description;
 await DataStore.AddItemAsync(newItem); ④
 }
 await Shell.Current.GoToAsyc("..");
}
}
```
The design of NewItemPage is very simple. It includes two controls, Entry and Editor (used to
edit the name and notes of an item). Entry is used to enter or edit a single line of text, and Editor
is used to edit multiple lines of text. In the view model NewItemViewModel view model, we can
see how to add a new item, as follows:
① The query parameter is defined with QueryPropertyAttribute. ② The Type property
declared as ItemSubType is used to receive the query parameter. The received item type is stored
in the _type backing variable. In NewItemPage, two toolbar items are defined, and the actions
are bound to the OnSave and OnCancel methods in the view model.
Once the user enters a name and notes in the user interface and clicks the Save button, ③ a new
item instance is created using the CreateNewItem() factory method, which is defined in the
IDataStore interface. ④ After filling in the new item instance from the user input, we can add
this new item to the database by invoking the AddItemAsync() method.
Now we’ve implemented the add operation, let’s implement the rest of the data operations in the
next section.
Editing or deleting an item
In CRUD operations, we don’t need an existing item to perform a create operation, but we need to
have an instance of the existing item to perform update and delete operations.
For a read operation, if the item is a group, we implement it by sending an ItemId query parameter
to ItemsPage and finding the group in the setter of ItemId in the ItemsViewModel view
model. If the item is an entry, we send an ItemId query parameter to ItemDetailPage and find
the entry in the setter of ItemId in ItemDetailViewModel.
For update/edit and delete operations, we can use context actions to do it. With context actions, we
can act on an item in ListView. The context actions look quite different on iOS, Android, and
Windows, as we can see in Figure 6.9:

![image](https://user-images.githubusercontent.com/26972859/231836373-2ab7120b-a569-4f35-b13d-3458d0b87fe4.png)

Figure 6.9: Context actions

On the iOS platform, you can take action on an item by sliding it to the left. On an Android system,
you can long-press an item and the context actions menu is shown in the top-right corner of the
screen. On Windows, you may be familiar with the right mouse click to see the context actions menu.
In our app, we implement a context actions menu in ItemsPage. In ItemsPage, we define context
actions in ViewCell, as follows:

```xml
<ListView.ItemTemplate>
 <DataTemplate>
 <ViewCell>
 <ViewCell.ContextActions>
 <MenuItem Clicked="OnMenuEdit" ❶
 CommandParameter="{Binding .}"
 Text="{x:Static resources:Resources.
 action_id_edit}" />
 <MenuItem Clicked="OnMenuDeleteAsync" ❷
 CommandParameter="{Binding .}"
 Text="{x:Static resources:Resources.
 action_id_delete}"
 IsDestructive="True" />
 </ViewCell.ContextActions>
 <Grid Padding="10" x:DataType="model:Item" ...>
 </ViewCell>
 </DataTemplate>
</ListView.ItemTemplate>
```
We define two menu items for edit and delete context actions. Two event handlers, ❶ OnMenuEdit
and ❷ OnMenuDeleteAsync, are assigned to the Clicked event. We can review the source code
of the event handlers here:

```Csharp
private void OnMenuEdit(object sender, EventArgs e) {
 var mi = (MenuItem)sender;
 if (mi.CommandParameter is Item item) {
 _viewModel.Update(item); ❶
 }
}
private async void OnMenuDeleteAsync(object sender,
 EventArgs e) {
 var mi = (MenuItem)sender;
 if (mi.CommandParameter is Item item) {
 await _viewModel.DeletedAsync(item); ❷
 }
}
```
The OnMenuEdit and OnMenuDeleteAsync event handlers call the ❶ Update() and ❷
DeleteAsync() methods in the view model. Let’s review the source code of these functions in
the ItemsViewModel view model, as follows:

```Csharp
public async void Update(Item item) {
 if (item == null) { return; }
 await Shell.Current.Navigation.PushAsync(new
 FieldEditPage(async (string k, string v, bool
 isProtected) => { ❶
 item.Name = k;
 item.Notes = v;
 await DataStore.UpdateItemAsync(item); ❷
 }, item.Name, item.Notes, true));
}
public async Task DeletedAsync(Item item) {
 if (item == null) { return; }
 if (Items.Remove(item)) {
 _ = await DataStore.DeleteItemAsync(item.Id); ❸
 }
 else { return; }
}
```
In ItemsViewModel, to edit or update an item, ❶ we use a FieldEditPage content page to
perform the editing. When we invoke the constructor of FieldEditPage, an anonymous function
is passed as a parameter. When the user completes the editing in FieldEditPage, this function will
be invoked. In this function, ❷ we call the UpdateItemAsync() method of the IDataStore
interface to update the item.
The delete operation is quite simple. We can just call the ❸ DeleteItemAsync() method of the
IDataStore interface to remove the item.
After we implement CRUD operations, our app has most of the desired features of a password manager
app. We can create a new database by signing up a new user. After we have a new database, we can log
in to access our data. After we create entries and groups, we can also edit or delete them.
For a password manager app, there are more desired features, such as fingerprint scanning, one-time
password, and so on. Most of these desired features are already included in the PassXYZ.Vault
1.x.x releases. I will continue migrating features from 1.x.x to the .NET MAUI (2.x.x) releases when
the dependencies are available for .NET MAUI.

---
Device features
We can access device features using the class in the Microsoft.Maui.Devices namespace.
The device features implementation originated from Xamarin.Essentials and then changed
to Maui.Essentials in .NET MAUI preview versions. It finally became Microsoft.
Maui.Devices in GA releases. Not all device features can be found in Microsoft.Maui.
Devices, such as fingerprint scanning. To support fingerprint scanning in .NET MAUI, I
need to wait for a library such as Plugin.Fingerprint, available in .NET MAUI, to enable
it in PassXYZ.Vault.
---

# Summary

In this chapter, we started with the introduction of design principles. After that, we introduced SOLID
design principles and I shared lessons learned in the design of our app. One of the most important SOLID
principles is Dependency Inversion Principle (DIP). Dependency Injection (DI) is the technique
to apply DIP in the actual implementation. In our app, we use the .NET MAUI built-in DI service to
decouple dependencies so that we can separate the implementation of the service from the interface.
With all the knowledge that we gathered about .NET MAUI, we completed our app implementation
by replacing MockDataStore with the actual implementation. We implemented CRUD operations
on top of this new IDataStore service. We have a fully functional password manager app now.
With the current version of the password manager app, we conclude Part 1 of this book.
In Part 2 of the book, we will explore the Blazor Hybrid app in .NET MAUI. This is a new capability
that does not exist in Xamarin.Forms. With Blazor support, we can bring some modern frontend
development techniques to .NET MAUI development.

# Further reading

  * Autofac is an inversion of control (IoC) container for .NET Core, ASP.NET Core, .NET4.5.1+, and more:
https://autofac.org/

  * Simple Injector is a DI container that can support .NET 4.5 and .NET Standard:
https://simpleinjector.org/










