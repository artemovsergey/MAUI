# Developing Unit Tests

Testing is an important way to ensure software quality in modern software development. There are
different types of testing involved in the software development lifecycle, such as unit testing, integration
testing, and system testing. Unit testing is used to test software modules or components in an isolated
environment. It is usually done by developers. With a well-planned unit test strategy, programming
issues can be found at the earliest stage in the software development lifecycle, so unit testing is the
most efficient and economical approach to ensuring the quality of your software. In .NET MAUI app
development, we can reuse existing unit test frameworks or libraries in the .NET ecosystem. By using
a test framework or library, we can speed up the unit test development. A good test framework is
usually designed to easily integrate with a continuous integration (CI) and continuous deployment
(CD) environment. In this chapter, we will introduce how to set up unit testing and run unit test cases
as part of the .NET MAUI app development lifecycle.
We will cover the following topics in this chapter:


• Unit testing in .NET

• Razor component testing using bUnit

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC or Mac. Please refer to the Development environment setup section in Chapter 1, Getting
Started with .NET MAUI, for the details.

The source code for this chapter is available in the following GitHub repository:

https://github.com/PacktPublishing/.NET-MAUI-Cross-PlatformApplication-Development/tree/main/Chapter11

The source code can be downloaded using the following Git command:

```
git clone -b chapter11 https://github.com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development PassXYZ.Vault2
```

# Unit testing in .NET

To develop unit test cases, we usually use a unit test framework to improve efficiency. There are several
unit test frameworks available in a .NET environment as follows:

* Microsoft Test Framework (MSTest) is shipped together with Visual Studio. The initial
version of MSTest (V1) was not an open source product. The first release was shipped with
Visual Studio 2005. Please refer to the book Microsoft Visual Studio 2005 Unleashed by Lars
Powers and Mike Snell to find more information about MSTest (V1). Later, Microsoft made the
new-release MSTest (V2) open source and hosted it on GitHub. The first MSTest (V2) release
was available around 2017.

* NUnit is an open source testing framework ported from JUnit. It was the first unit test
framework for .NET. The earliest releases were hosted at SourceForge in 2004. Please refer to
the version 2.0 release note in the Further reading section. The most recent releases have been
moved to GitHub.

* xUnit is a more modern and extensible framework developed by Jim Newkirk and Brad Wilson.
They were the creators of NUnit, and they made many improvements to this new test framework
compared to NUnit. Please refer to Jim’s blog Why Did we Build xUnit 1.0? to find out more
information about the improvements. The first stable release of xUnit was available around 2015.
They are all quite popular and can be chosen based on the project requirements. In this chapter,
we will use xUnit to develop unit test cases since it is a newer framework with many improvements
compared to NUnit.

No matter which unit test framework you choose, the process of unit test development is quite similar.
The content in this chapter can still help you if you use a different framework in your project. Unit
test cases can only run against a cross-platform target framework rather than platform-specific target
frameworks. In this book, we use .NET 6.0, so the target framework of unit testing is net6.0 instead
of net6.0-android or net6.0-ios.
To develop a unit test for .NET MAUI, we will introduce the test case development for both XAMLbased and Blazor-based apps. In both cases, we will use the MVVM pattern in the design. The unit
test cases at the model layer are the same for both, but the testing in the view and the view model is
quite different. For a XAML-based app, it is quite complicated to develop unit test cases for the view
and the view model. In order to test the view model, we have to resolve the dependencies of XAML
components. For example, in the XAML version of our app, we need to call Shell navigation methods
in the view model as shown in the following code:

```
await Shell.Current.GoToAsync(
 $"{nameof(ItemsPage)}?{nameof(ItemsViewModel.ItemId)}={item.Id}");
```
To resolve dependencies, in Xamarin.Forms, there is an open source project, Xamarin.Forms.
Mocks, which can help mock Xamarin.Forms components. We also need something similar to
develop unit test cases for the view model in .NET MAUI XAML apps, but I cannot find any equivalent
at the moment. There is also a native user interface test framework, Xamarin.UITest, which is
for Android and iOS, but this framework cannot be used in .NET MAUI yet. Regardless, Xamarin.
UITest is not a cross-platform solution so we won’t discuss it in this book.
For a Blazor Hybrid app, we have a good test library, bUnit, which can be used to test Razor components.
We can develop unit test cases for the view, view model, and model layers for Blazor apps.
In this chapter, we will develop unit tests for the model layer first, which is common for both XAML
and Blazor. After that, we will introduce unit test development for Blazor apps using bUnit. bUnit is
a testing library that can be used with all three test frameworks (xUnit, NUnit, and MSTest).

# Setting up the unit test project

To get our hands dirty, let us create a unit test project. We can create a xUnit project using either
Visual Studio or the .NET command line:

1. To start with Visual Studio, we can add a new project to our current solution as shown in
Figure 11.1:

![image](https://user-images.githubusercontent.com/26972859/232018770-8c5900cb-f1de-4623-aa29-2c01cc7b34cb.png)

Figure 11.1 – Creating a xUnit project

2. We can type xunit into the search box and select xUnit Test Project for C#.

3. On the next screen, we can name the project PassXYZ.Vault.Tests and click on Next.

4. After that, please select the framework as .NET 6.0 and click Create.

To create the project using a command line, we can create the folder first and use a .NET command to create the project as follows:

```
mkdir PassXYZ.Vault.Tests
cd PassXYZ.Vault.Tests
dotnet new xunit
dotnet test
```

Once we have created the project, we can use the dotnet test command to run the test cases.
The default test case in the template will be executed. We will add test cases to this test project. The
test targets are the components of the PassXYZ.Vault and PassXYZ.BlazorUI projects,
so we need to add these two projects as reference projects. The target framework of PassXYZ.
BlazorUI is net6.0, so we can add it directly. However, the target frameworks of PassXYZ.
Vault are platform-specific, so we need to make some changes before we can add them as a reference
in PassXYZ.Vault.Tests.
The project file of our password manager app is PassXYZ.Vault.csproj. We need to add
net6.0 as one of the target frameworks to this project file:

```
<TargetFrameworks>net6.0;net6.0-android;net6.0-ios;net6.0-maccatalyst</TargetFrameworks>
```
When we build project PassXYZ.Vault on the supported platforms, we expect an executable since
it is an app. However, when we build PassXYZ.Vault for the net6.0 target framework, we want
to test it. PassXYZ.Vault should be generated as a library so that the test framework can use it
to run test cases. In this case, we expect to build a file with a .dll extension instead of .exe, so we
need to make the following change:

```
<OutputType Condition="'$(TargetFramework)'!='net6.0'">Exe</OutputType>
```
In the preceding build setup, a condition is added to check the target framework for the output type. If the target framework is not net6.0, we will build output as an executable.

With these changes, we can add reference projects to PassXYZ.Vault.Tests by right-clicking on
the solution node and selecting Add -> Project Reference … or editing the project file for PassXYZ.
Vault.Tests to add these lines:

```
<ItemGroup>
 <ProjectReference Include="..\PassXYZ.BlazorUI\PassXYZ.
 BlazorUI.csproj" />
 <ProjectReference Include="..\PassXYZ.Vault\PassXYZ.Vault.
 csproj" />
</ItemGroup>
```
To test the MAUI project, we also need to add the following configuration to the PassXYZ.Vault.
Tests project:

```
<UseMaui>true</UseMaui>
```
Now, we have set up the xUnit project. Let us add our test cases.

# Creating test cases to test the IDataStore interface

We will start to add test cases at the model layer first since the test case setup at the model layer is the
same for both the XAML and Blazor versions of our app.

At the model layer, the major implementation is in the PassXYZLib library – you may refer to the
source code of PassXYZLib to find more about the unit test cases at the model layer:
https://github.com/shugaoye/PassXYZLib

In our app, IDataStore is the interface to export PassXYZLib, so let’s add test cases to our
test interface, IDataStore. To test the IDataStore interface, we can create a new test class,
DataStoreTests, in the PassXYZ.Vault.Tests project. We can start to add a new test case
to test the case by adding an item as follows:

```
public class DataStoreTests
{
 [Fact] ❶
 public async void Add_Item()
 {
 // Arrange ❷
 IDataStore<Item> datastore = new MockDataStore();
 ItemSubType itemSubType = ItemSubType.Entry;
 // Act ❸
 var newItem = datastore.CreateNewItem(itemSubType);
 newItem.Name = $"{itemSubType.ToString()}01";
 await datastore.AddItemAsync(newItem);
 var item = datastore.GetItem(newItem.Id);
 // Assert ❹
 Assert.Equal(newItem.Id, item.Id);
 }
}
```
xUnit uses attributes to inform the framework about test case setup. In this test case, we use the
[Fact] attribute, ❶, to mark this method as a test case. To define a test case, we can use a common
pattern – Arrange, Act, and Assert:
• Arrange ❷ – We will prepare all necessary setup for the test. To add an item, we need to initialize
the IDataStore interface first, and then we will define a variable to hold the item type.
• Act ❸ – We execute the methods that we want to test, which are CreateNewItem()
and AddItemAsync().
• Assert ❹ – We check the result that we expected. In our case, we try to retrieve the new item
using item.Id. After that, we check to ensure that the item ID retrieved is the same as what
we expected.
As you may have noticed, we tested the Entry type in the previous test case. The Entry type is only
one of the item types – we have many. To test all of them, we need to create many test cases. xUnit
supports another test case type, [Theory], which helps us to test different scenarios with one test case.
We can use the “delete an item” test case to demonstrate how to test different scenarios in one test
case with the [Theory] attribute. In this test case, we can delete an item in different item types in
one test case:

```Csharp
public class DataStoreTests
{
 ...
 [Theory] ①
 [InlineData(ItemSubType.Entry)] ②
 [InlineData(ItemSubType.Group)]
 [InlineData(ItemSubType.Notes)]
 [InlineData(ItemSubType.PxEntry)]
 public async void Delete_Item(ItemSubType itemSubType)
 
 // Arrange
 IDataStore<Item> datastore = new MockDataStore();
 var newItem = datastore.CreateNewItem(itemSubType); ③
 newItem.Name = $"{itemSubType.ToString()}01";
 await datastore.AddItemAsync(newItem);
 // Act
 bool result = await
 datastore.DeleteItemAsync(newItem.Id); ④
 Debug.WriteLine($"Delete_Item: {newItem.Name}");
 // Assert
 Assert.True(result); ⑤
 }
 ...
}
```
When we create a test case using the [Theory] attribute, ①, we can pass different item types using
the itemSubType parameter. The value of the itemSubType argument is defined using the
[InlineData] attribute, ②.
To arrange test data, we create a new item using the itemSubType argument, ③. Then, we execute
the DeleteItemAsync() method, ④, which is the one that we want to test.
Finally, we check the return value, ⑤. If the item is deleted successfully, the result is true. Otherwise,
the result is false.
We have learned how to create a test case using the [Fact] attribute and how to cover different
scenarios using the [Theory] attribute. Let’s discuss more topics in test case development in the
next section.

# Sharing context between tests

In our previous test cases, we created a new IDataStore instance for each test case. Can we share one
IDataStore instance instead of creating the same instance every time? We can reduce duplication
by sharing the test setup among a group of test cases in xUnit.

There are three ways to share the setup and cleanup code between tests in xUnit:

* Constructor and Dispose – we can use a class constructor to share the setup and cleanup code
without sharing instances

* Class Fixture – we can use a fixture to share object instances in a single class

* Collection Fixtures – we can use collection fixtures to share object instances in multiple
test classes

Sharing using a constructor

To remove the duplicated setup code from the previous tests, we can move the creation of the
IDataStore instance to the constructor of the DataStoreTests test class as follows:

```Csharp
public class DataStoreTests
{
 IDataStore<Item> datastore;
 public DataStoreTests()
 {
 datastore = new MockDataStore();
 Debug.WriteLine("DataStoreTests: Created");
 }
 ...
}

```

In this code, we added a private member variable, datastore, and created an instance of IDataStore
in the constructor of DataStoreTests. We also added a debug output so we can monitor the
creation of the IDataStore interface. Let us debug the execution of the DataStoreTests class
so we can see the debug output here:

```
DataStoreTests: Created
Delete_Item: Entry01
DataStoreTests: Created
Delete_Item: Group01
DataStoreTests: Created
Delete_Item: PxEntry01
DataStoreTests: Created
Delete_Item: Notes01
DataStoreTests: Created
Create_Item: PxEntry
DataStoreTests: Created
Create_Item: Group
DataStoreTests: Created
Create_Item: Entry
DataStoreTests: Created
Create_Item: Notes
DataStoreTests: Created
Add_Item: Done
```
We can see from the debug output that a DataStoreTests class is created for each test case. There
is no difference whether we create the instance of IDataStore inside the test method or in the
constructor. All the test cases are still isolated from each other. When we use the [Theory] attribute
to test different scenarios with one method, each of them looks like a separate test case at runtime. To
understand this better, we can use dotnet command to list all the tests defined:

```
dotnet test -t
 Determining projects to restore...
 All projects are up-to-date for restore.
Microsoft (R) Test Execution Command Line Tool Version 17.3.0
 (x64)
Copyright (c) Microsoft Corporation. All rights reserved.
The following Tests are available:
 PassXYZ.Vault.Tests.DataStoreTests.Add_Item
 PassXYZ.Vault.Tests.DataStoreTests.Delete_Item(itemSubType:
 Entry)
 PassXYZ.Vault.Tests.DataStoreTests.Delete_Item(itemSubType:
 Group)
 PassXYZ.Vault.Tests.DataStoreTests.Delete_Item(itemSubType:
 Notes)
 PassXYZ.Vault.Tests.DataStoreTests.Delete_Item(itemSubType:
 PxEntry)
 PassXYZ.Vault.Tests.DataStoreTests.Create_Item(itemSubType:
 Entry)
 PassXYZ.Vault.Tests.DataStoreTests.Create_Item(itemSubType:
 Group)
 PassXYZ.Vault.Tests.DataStoreTests.Create_Item(itemSubType:
 Notes)
 PassXYZ.Vault.Tests.DataStoreTests.Create_Item(itemSubType:
 PxEntry)
```
We can see that each parameter defined by the [InlineData] attribute is shown as a separate test
case. They are all isolated test cases at runtime.
After we list all tests, we can selectively execute them using the dotnet command.
If we want to run all the tests in the DataStoreTests class, we can use this command:

```dotnet test --filter DataStoreTests```

If we want to run Add_Item tests only, we can use this command:

```dotnet test --filter DataStoreTests.Add_Item```

As we can see from the debug output, even though we created an instance of IDataStore in the
constructor, the instance is re-created for each test. The instances created in the test class constructor
won’t be shared across tests. Even though the effect is still the same, the code looks more concise.
However, in some cases, we do want to share instances across tests. We can do so using class fixtures.
Let’s look at these cases in the next section.

Sharing using class fixtures

When we use a tool in all test cases, we may want to share the setup in all test cases instead of creating
the same one every time. Let’s use a logging function as an example to explain this.
To have a test report, we want to create a test log to monitor the execution of unit tests. There is a
library called Serilog that can be used for this purpose. We can log messages to different channels
using Serilog. To use Serilog, we need to set it up first and clean up it after all the tests have
been executed. In this case, we want to share an instance of Serilog between all the tests instead
of creating one for each test. With this setup, we can generate one log file for all the tests instead of
multiple log files for each test.

To use Serilog, we need to add the Serilog package to the project first. To do that, we can run
the following dotnet commands in the project’s PassXYZ.Vault.Tests folder:

```
dotnet add package Serilog
dotnet add package Serilog.Sinks.File
```
After adding the Serilog libraries to the project, we can create a class fixture, SerilogFixture,
for demonstration purposes:

```Csharp
public class SerilogFixture : IDisposable { ❶
 public ILogger Logger { get; private set; }
 public SerilogFixture() {
 Logger = new LoggerConfiguration() ❷
 .MinimumLevel.Debug()
 .WriteTo.File(@"logs\xunit_log.txt")
 .CreateLogger();
 Logger.Debug("SerilogFixture: initialized");
 }
 public void Dispose() {
 Logger.Debug("SerilogFixture: closed");
 Log.CloseAndFlush(); ❸
 }
}
public class DataStoreTests : IClassFixture<SerilogFixture>
{ ❹
 IDataStore<Item> datastore;
 SerilogFixture serilogFixture;
 public DataStoreTests(SerilogFixture fixture) { ❺
 serilogFixture = fixture; ❻
 datastore = new MockDataStore();
 serilogFixture.Logger.Debug("DataStoreTests: Created");
 }
 [Fact]
 public async void Add_Item() ...
 ...
}
```

If we want to use class fixtures, we can create them using the following steps:
1. We can create a new class as the fixture class and add the setup code to the constructor. Here,
we created a fixture class, SerilogFixture, ❶, and initialized the ILogger interface,
❷, in the constructor.
2. Because we need to clean up the setup after the test case execution, we need to implement the
IDisposable interface for the fixture class and put the cleanup code in the Dispose()
method. We implemented IDisposable in SerilogFixture and called the Serilog
function, Log.CloseAndFlush(), ❸, in the Dispose() method.
3. To use the fixture, the test case needs to implement the ```IClassFixture<T>``` interface. We
implemented this in the DataStoreTests test class, ❹.
4. To access the fixture instance, we can add it as a constructor argument and it will be provided
automatically. In the constructor of DataStoreTests, ❺, we assign the argument to the
private member variable, serilogFixture, ❻. In test cases, we can access Serilog
using this variable.
To verify this setup, we replaced all our debug output with Serilog Debug. After executing the
tests in DataStoreTests, we can see the log messages here in the xunit_log.txt log file:

```
2022-08-28 10:25:39.273 +08:00 [DBG] SerilogFixture:
initialized
2022-08-28 10:25:39.332 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.350 +08:00 [DBG] Delete_Item: Entry01
2022-08-28 10:25:39.355 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.355 +08:00 [DBG] Delete_Item: Group01
2022-08-28 10:25:39.356 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.357 +08:00 [DBG] Delete_Item: PxEntry01
2022-08-28 10:25:39.358 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.358 +08:00 [DBG] Delete_Item: Notes01
2022-08-28 10:25:39.359 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.359 +08:00 [DBG] Create_Item: PxEntry
2022-08-28 10:25:39.360 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.360 +08:00 [DBG] Create_Item: Group
2022-08-28 10:25:39.361 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.361 +08:00 [DBG] Create_Item: Entry
2022-08-28 10:25:39.362 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.362 +08:00 [DBG] Create_Item: Notes
2022-08-28 10:25:39.362 +08:00 [DBG] DataStoreTests: Created
2022-08-28 10:25:39.364 +08:00 [DBG] Add_Item: Done
2022-08-28 10:25:39.367 +08:00 [DBG] SerilogFixture: closed  
```
As we expected, the SerilogFixture class is initialized just once, and the instance can be used
in all the tests in DataStoreTests, compared to the IDataStore interface being initialized
for each test.

# Sharing using collection fixtures

Using class fixtures, as shown in the previous section, we can share the test setup context in one test
class. There are also cases in which we might want to share the test setup in multiple test classes. We
can do so using collection fixtures.
In our case of Serilog, we can use it in many test classes as well so that we can see all the log
messages in one log file. To use one Serilog setup for all the test classes, we can implement collection
fixtures in our project. To use collection fixtures, we can create two new classes, SerilogFixture
and SerilogCollection, in the PassXYZ.Vault.Tests project as shown in Listing 11.1:

**Listing 11.1: SerilogFixture.cs (https://epa.ms/SerilogFixture11-1)**

```public
namespace PassXYZ.Vault.Tests;
public class SerilogFixture : IDisposable {
 public ILogger Logger { get; private set; }
 public SerilogFixture() {
 Logger = new LoggerConfiguration()
 .MinimumLevel.Debug()
 .WriteTo.File(@"logs\xunit_log.txt")
 .CreateLogger();
 Logger.Debug("SerilogFixture: initialized");
 }
 public void Dispose() {
 Logger.Debug("SerilogFixture: closed");
 Log.CloseAndFlush();
 }
}
[CollectionDefinition("Serilog collection")] ❶
public class
SerilogCollection:ICollectionFixture<SerilogFixture>
{
} ❷
```

We can follow the steps below to implement collection fixtures:
1. We create a new class file, SerilogFixture.cs, and put both SerilogFixture and
SerilogCollection into this file.
2. We decorate the collection definition class, SerilogCollection, with the
[CollectionDefinition] attribute, ❶. Then, we give it a unique name that can be
used to identify the test collection.
3. The collection definition class, SerilogCollection, needs to implement the
```ICollectionFixture<T>``` interface, ❷.
To use a collection fixture, we can make the following changes to our test classes:
1. We can add the [Collection] attribute to all the test classes that will be part of the
collection. We assign Serilog collection as a name to the test collection definition in
the attribute. In our case, as we can see in Listing 11.2, we add a [Collection("Serilog
collection")] attribute, ①, to the DataStoreTests class.
2. To access the fixture instance, we can do the same as in the previous section with class fixtures
and add it as a constructor argument. Then, it will be provided automatically. In the constructor
of DataStoreTests, we assign a fixture argument to the serilogFixture variable, ②:

**Listing 11.2: DataStoreTests.cs (https://epa.ms/DataStoreTests11-2)**

```Csharp
namespace PassXYZ.Vault.Tests;
[Collection("Serilog collection")] ①
public class DataStoreTests {
 IDataStore<Item> datastore;
 SerilogFixture serilogFixture;
 public DataStoreTests(SerilogFixture fixture) {
 datastore = new MockDataStore();
 serilogFixture = fixture; ②
 serilogFixture.Logger.Debug("DataStoreTests
 initialized");
 }
 [Fact]
 public async void Add_Item() {
 // Arrange
 ItemSubType itemSubType = ItemSubType.Entry;
// Act
 var newItem = datastore.CreateNewItem(itemSubType);
 newItem.Name = $"{itemSubType.ToString()}01";
 await datastore.AddItemAsync(newItem);
 var item = datastore.GetItem(newItem.Id);
 // Assert
 Assert.Equal(newItem.Id, item.Id);
 serilogFixture.Logger.Debug("Add_Item done");
 }
 [Theory]
 [InlineData(ItemSubType.Entry)]
 [InlineData(ItemSubType.Group)]
 [InlineData(ItemSubType.Notes)]
 [InlineData(ItemSubType.PxEntry)]
 public async void Delete_Item(ItemSubType itemSubType)...
 [Theory]
 [InlineData(ItemSubType.Entry)]
 [InlineData(ItemSubType.Group)]
 [InlineData(ItemSubType.Notes)]
 [InlineData(ItemSubType.PxEntry)]
 public void Create_Item(ItemSubType itemSubType) ...
}
```
With these examples, we have introduced how to create unit tests in the model layer. The knowledge
that we have gained so far can be used in unit testing for other .NET applications as well.
So far, we have concluded the introduction of the model layer unit test. In the next part of this chapter,
we will explore the Razor component unit test using the bUnit library.

# Razor component testing using bUnit

In .NET MAUI development, we don’t really have a good unit test framework for XAML-based UI
components, but we do have one for Blazor. bUnit is an excellent test library that can be used for the
unit test development of Razor components. With the bUnit library, we can develop unit test cases
for Razor components using xUnit, NUnit, or MSTest. We will use xUnit with bUnit for the rest
of the chapter. The structure of unit test cases using bUnit is similar to the xUnit test cases that we
introduced in the previous section.

The test targets in the rest of this chapter are the following Razor components that we created in the
second part of this book:

  * Razor components in the PassXYZ.BlazorUI project

  * Razor components in the PassXYZ.Vault project

  To test Razor components using bUnit, we need to change the project configuration of
PassXYZ.Vault.Tests.

# Changing project configuration for bUnit

To set up the test environment, we need to add the bUnit and Moq packages and, update the SDK
type. We can make the following changes to the xUnit PassXYZ.Vault.Tests testing project:

1. Add bUnit to the project.

  To add the bUnit library to the project, we can change to the project folder first and execute
the following command from a console:
  
```
cd PassXYZ.Vault.Tests
dotnet add package bunit
```

  2. We also need to add the Moq package, which is a mocking library that we will use in the test setup:
dotnet add package Moq

  3. Change the project configuration. To test the Razor components, we also need to change the project’s SDK to Microsoft.NET.Sdk.Razor.

  In the PassXYZ.Vault.Tests.csproj project file, we need to replace the following line: ```<Project Sdk="Microsoft.NET.Sdk">```

  We will do so with this: ```<Project Sdk="Microsoft.NET.Sdk.Razor">```

  Once we have the project configuration ready, we can create a simple unit test case using bUnit to
test our Razor components.

# Creating a bUnit test case

In our PassXYZ.Vault app, we have two kinds of Razor components that can be tested. The shared
Razor components reside in the PassXYZ.BlazorUI project. They are generic Razor components
that can be used in different projects. The other set of Razor components is the one in the Pages
folder of the PassXYZ.Vault project. They are specific to the PassXYZ.Vault app and they
use shared components from the PassXYZ.BlazorUI project.
To test the Razor components in the PassXYZ.BlazorUI project, we can test each component
separately. These test cases are unit test cases. The Razor components in the Pages folder of the
PassXYZ.Vault project are UI pages. These pages use UI components from other packages, so
they have more dependencies. These test cases can be considered integration test cases.
We can create a test case for the ModalDialog Razor component in the PassXYZ.BlazorUI
project first. To test ModalDialog, we can create a xUnit test class, ModalDialogTests, as
shown in Listing 11.3:

**Listing 11.3: ModalDialogTests.cs (https://epa.ms/ModalDialogTests11-3)**

```Csharp
namespace PassXYZ.Vault.Tests {
 [Collection("Serilog collection")]
 public class ModalDialogTests : TestContext { ❶
 SerilogFixture serilogFixture;
 public ModalDialogTests(SerilogFixture serilogFixture) {
 this.serilogFixture = serilogFixture;
 }
 [Fact]
 public void ModalDialogInitTest() {
 string title = "ModalDialog Test"; ❷
 var cut = RenderComponent<ModalDialog>( ❸
 parameters => parameters.Add(p => p.Title, title) ❹
 .Add(p => p.CloseButtonText, "Close")
 .Add(p => p.SaveButtonText, "Save"));
 cut.Find("h5").TextContent.MarkupMatches(title); ❺
 serilogFixture.Logger.Debug("ModalDialogInitTest:
 done");
 }
 ...
 }
}
```

As we can see in the ModalDialogTests unit test class, it is very similar to the unit test class that
we created for the model layer. We reuse the collection fixture that we created before and initialized
it in the constructor. In the ModalDialogInitTest test case, we still use the Arrange, Act, and
Assert pattern to implement the test case.
All bUnit test classes inherit from TestContext ❶. In the Arrange phase, we initialize a local title
variable, ❷, with a defined string. In the Act phase, we call a generic method, ```RenderComponent<T>```,
❸, and use the ModalDialog type as the type parameter. We pass the title variable, ❹, as the
component parameter. The result of ```RenderComponent<T>``` is stored in the cut variable. In the
Assert phase, we verify that the title text after rendering is the same as the argument that we pass to
it using the Find() bUnit method, ❺. The Find() bUnit method can be used to find any HTML
tag. In ModalDialog, the title is rendered as a ```<h5>``` HTML tag.
In the ModalDialogInitTest test case, we can see the structure of the bUnit tests. In bUnit tests,
we render the component under test first. The result of the rendering is stored in the cut variable,
❸. It is an instance of the IRenderedComponent interface. We can verify the result by referring
to the properties or calling the methods of the IRenderedComponent instance.
When Razor components are rendered in TestContext, they have the same lifecycle as any other
Razor component. We can pass parameters to components under test, and they can produce output,
similar to what happens in a browser.
When we render the ModalDialog component in the preceding example, we can
pass component parameters to it using the Add() method of a parameter builder of the
```ComponentParameterCollectionBuilder<TComponent>``` type.
We may not have a problem rendering simple components using C# code. However, we usually need to
pass multiple parameters to a component, and it is not convenient to do so in C# code. With bUnit, we
can develop test cases in Razor files, which can bring a much better experience in unit test development.

# Creating test cases in Razor files

To create tests in Razor markup files directly, we can declare components using Razor markup as
we use them in a Razor page. In this way, we don’t have to call Razor components in the C# code
and pass parameters using function calls. For a Razor page, we can render Razor components using
Razor templates.
We can demonstrate how to create tests in Razor markup files by creating test cases for a more
complicated EditorDialog component. We created the EditorDialog component in Chapter 9,
Razor Components and Data Binding. In Listing 11.4, let’s review the unit tests for it:

**Listing 11.4: EditorDialogTests.razor (https://epa.ms/EditorDialogTests11-4)**

```Csharp
@inherits TestContext ❶
<h3>EditorDialogTests</h3>
@code {
 bool _isOnCloseClicked = false;
 string _key = string.Empty;
 string _value = string.Empty;
 string updated_key = "key updated";
 string updated_value = "value udpated";
 void OnSaveClicked(string key, string value) {
 _key = key; _value = value;
 }
 void OnCloseHandler() {
 _isOnCloseClicked = true;
 }
 [Fact]
 public void EditorDialog_Init_WithoutArgument() ...
 [Fact]
 public void Edit_OnClose_Clicked() {
var cut = Render(@<EditorDialog Key="@_key"
 Value="@_value"
 OnSave=@OnSaveClicked OnClose=@OnCloseHandler>
 </EditorDialog>); ❷
 cut.Find("button[class='btn btn-secondary']").Click();❸
 Assert.True(_isOnCloseClicked); ❹
 }
 [Fact]
 public void Edit_With_KeyEditingEnabled() { ❺
var cut = Render(@<EditorDialog Key="@_key"
 Value="@_value"
 IsKeyEditingEnable="true" OnSave=@OnSaveClicked>
 </EditorDialog>);
 cut.Find("input").Change(updated_key);
 cut.Find("textarea").Change(updated_value);
 cut.Find("button[type=submit]").Click();
 Assert.Equal(_key, updated_key);
 Assert.Equal(_value, updated_value);
 }
 [Fact]
 public void Edit_With_KeyEditingDisabled() ...
}
```
We can create a new Razor component, EditorDialogTests, in the PassXYZ.Vault.Tests
project. Since it is a bUnit test class, it is a child class of TestContext, ❶. In this class, we create
test cases in a code block using Razor templates.
  
We can review the Edit_OnClose_Clicked test case first. In this test case, we render the
EditorDialog component first and after that, we test the close button.
To render the EditorDialog component, we call the Render() method, ❷, of TestContext.
Compared to the previous example, here, we can render the Razor markup directly instead of calling
the C# function. The Razor markup that we use here is called Razor templates and you can find more
information about it in this Microsoft document:
  
https://learn.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0#razor-templates-1

Razor templates can be defined in the following format:

```@<{HTML tag}>…</{HTML tag}>```

It consists of an @ symbol and a pair of open and closed HTML tags. Razor templates can be used in
the code block of the Razor file. It cannot be used in a C# or C# code-behind file.
Using this format, we can specify a snippet of the Razor markup as the parameter of a C# function.
The snippet of the Razor markup is a Razor template and its data type is RenderFragment or
```RenderFragment<TValue>```. In Listing 11.4, we pass parameters to EditorDialog using
Razor templates, as we can see in the following code:

```
var cut = Render(@<EditorDialog Key="@_key" Value="@_value" OnSave=@OnSaveClicked OnClose=@OnCloseHandler>
</EditorDialog>);
```
After EditorDialog is rendered, we can find the close button and simulate the click action, ❸:

```
cut.Find("button[class='btn btn-secondary']").Click();  
```
In the OnCloseHandler event handler, the _isOnCloseClicked variable, ❹, is set to true
so that we can assert the result.
  
In the Edit_With_KeyEditingEnabled test case, ❺, after the component is rendered, we can
simulate user interactions to set the key and value fields in the component. After that, we can simulate
clicking on the save button as we can see here:
  
```
 cut.Find("input").Change(updated_key);
 cut.Find("textarea").Change(updated_value);
 cut.Find("button[type=submit]").Click();
```

When the button is clicked, the event handler is invoked. In the OnSaveClicked event handler,
we save the changed key and value so we can assert the result: 

```
 Assert.Equal(_key, updated_key);
 Assert.Equal(_value, updated_value);
```

As we can see from these two test cases, we can design bUnit tests much more easily by creating tests
in a Razor file. We can render components using Razor templates and we can also trigger various user
interactions to test the components interactively.
Razor templates are a great tool to help us combine Razor markup and C# code so that we can leverage
the best features from both worlds. However, we have a limitation when we use Razor templates. Let’s
see how to overcome it in the next section.  
  
# Using the RenderFragment delegate

  Even though Razor templates can help simplify the test setup, there is a limitation, particularly in a
complicated test case setup. In a complicated test case, the Razor templates can be very long. If we
want to reuse the same Razor templates in another test case, we need to copy them to the new test
case. We may have to create a lot of duplicated code, and this is the limitation of Razor templates.
In this case, we can use a RenderFragment delegate. As its name indicates, it is the delegate type
of RenderFragment or ```RenderFragment<TValue>```. The data type of Razor templates is
RenderFragment or ```RenderFragment<TValue>```. A RenderFragment delegate is the
delegate type for Razor templates.  
  
 You can find more information about the RenderFragment delegate in the following
Microsoft document: 
  
 https://learn.microsoft.com/en-us/aspnet/core/blazor/performance?view=aspnetcore-3.1#define-reusable-renderfragmentsin-code-2
To demonstrate how to use the RenderFragment delegate, let’s set up a more complex test for the

EditorDialog component. EditorDialog can be used to edit either Item or Field. We can
use an item-editing case to show how to use the RenderFragment delegate.

We can create a new test class, ItemEditTests, in the PassXYZ.Vault.Tests project. To
separate the Razor markup and C# code, we can split the ItemEditTests test class into a Razor
file (ItemEditTests.razor) and a C# code-behind file (ItemEditTests.razor.cs). We
can declare the markup for testing in the Razor file, as shown in Listing 11.5: 
  
**Listing 11.5: ItemEditTests.razor (https://epa.ms/ItemEditTests11-5)**  
  
```
@inherits TestContext
@namespace PassXYZ.Vault.Tests
<h3>ItemEditTests</h3>
@code {
 private RenderFragment _editorDialog => __builder =>
 {
 <CascadingValue Value="@_dialogId" Name="Id">
<EditorDialog IsKeyEditingEnable=@isNewItem
 OnSave=@OnSaveClicked Key=@testItem.Name
 Value=@testItem.Notes>
 @if (isNewItem) {
 <select id="itemType" @bind="testItem.ItemType"
 class="form-select" aria-label="Group">
 <option selected value="Group">Group</option>
 <option value="Entry">Entry</option>
 <option value="PxEntry">PxEntry</option>
 <option value="Notes">Notes</option>
 </select>
 }
 </EditorDialog>
 </CascadingValue>
 };
}
```
We define a RenderFragment delegate, _editorDialog, in the @code block of ItemEditTests.
razor. The RenderFragment delegate must accept a parameter called ```__builder``` of the
RenderTreeBuilder type. In the markup code, we can access the variables defined in the test class.
Now let’s look at the usage of _editorDialog in the C# code-behind file in Listing 11.6:  
  
**Listing 11.6: ItemEditTests.razor.cs (https://epa.ms/ItemEditTests11-6)** 
  
```Csharp
namespace PassXYZ.Vault.Tests;
[Collection("Serilog collection")]
public partial class ItemEditTests : TestContext {
 readonly SerilogFixture serilogFixture;
 bool isNewItem { get; set; } = false;
 NewItem testItem { get; set; }
 string _dialogId = "editItem";
 string updated_key = "Updated item";
 string updated_value = "This item is updated.";
 public ItemEditTests(SerilogFixture fixture) {
 testItem = new() {
 Name = "New item",
 Notes = "This is a new item."
 };
 serilogFixture = fixture;
 }
 void OnSaveClicked(string key, string value) {
 testItem.Name = key; testItem.Notes = value;
 }
 [Fact]
 public void Edit_New_Item() {
 isNewItem = true;
 var cut = Render(_editorDialog); ❶
 cut.Find("#itemType").Change("Entry");
 cut.Find("input").Change(updated_key);
 cut.Find("textarea").Change(updated_value);
 cut.Find("button[type=submit]").Click();
 Assert.Equal(updated_key, testItem.Name);
 Assert.Equal(updated_value, testItem.Notes);
 }
 [Fact]
 public void Edit_Existing_Item() {
 isNewItem = false; ❸
 var cut = Render(_editorDialog); ❶
var ex = Assert.Throws<ElementNotFoundException>(() =>
 cut.Find("#itemType").Change("Entry")); ❷
 Assert.Equal("No elements were found that matches the
selector '#itemType'", ex.Message); ❹
 cut.Find("textarea").Change(updated_value);
 cut.Find("button[type=submit]").Click();
 Assert.Equal(updated_value, testItem.Notes);
 }
}
```
 Since _editorDialog defines the Item editing, we can implement multiple test cases against
_editorDialog. We can see that we render _editorDialog, ❶, for multiple test cases, such
as Edit_New_Item and Edit_Existing_Item. Using the RenderFragment delegate, our
testing code looks much more elegant and cleaner. If we did not go this way, we would need to repeat
long markup code in multiple places. Using C# code directly may have even led to more duplicated code.
In both test cases, we follow a similar process to testing EditorDialog by setting values and then
clicking on the Save button. In the markup code, we have a ```<select>``` tag defined. We can change
the option, ❷, of the ```<select>``` tag in the test code. This ```<select>``` tag is rendered conditionally
referring to the value of the isNewItem variable. In the Edit_Existing_Item test, we can also
test the negative case when the isNewItem variable,❸, is set to false. In this case, an exception
is thrown since the ```<select>``` tag is not rendered. We can see that bUnit can also be used to test
negative cases by verifying the content of exception, ❹.
We created bUnit tests for the shared components in the PassXYZ.BlazorUI project in the previous
examples. Since these shared components are reusable building blocks for a high-level UI, most of
them declare many component parameters. The RenderFragment delegate or Razor templates
can help to simplify the test setup.
If we move to the Razor pages in the Pages folder of the PassXYZ.Vault project, Items,
ItemDetail, or Login are Razor components as well, but they are not designed for reuse. They are
Razor pages with route templates defined and they don’t have many component parameters defined.  
The component parameters declared in these Razor pages are used for routing purposes. When we
design test cases for these Razor pages, we can implement tests in a C# class rather than Razor files.  
  
# Testing Razor pages  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  


