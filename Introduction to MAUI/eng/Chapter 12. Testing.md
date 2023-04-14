# CHAPTER 12

# Testing

Testing is such an important part of the software development process;
it enables you to verify that what you have delivered is what was required
and also validate that the software behaves correctly. It also provides the
safety net of catching regressions in the products that you build.
There are many different approaches for designing and writing tests
and where they fit into the software development process. This chapter
is not intended to provide full insight into those approaches, but it will
expose you to various methods of testing a .NET MAUI application, why
they can be beneficial, and pique your interest in learning to use them in
more depth

# Unit Testing

Unit testing is the process of ensuring that small units, typically a method
or class, of an application meet their design and behave as intended. One
big benefit of testing such a small unit of the code is that it makes it easier
for you to identify where issues may lie or creep in as part of regression.
I have worked on many legacy systems throughout my career where the
teams neglected to apply unit testing and the experience when trying to
identify the cause of a bug in a large system really can be costly in terms of
time and money.
Despite unit testing featuring near the end of this book, it is a concept
that should be adopted early in the development process. Unit testing can
aid in the design and building of code that is easier to read and maintain
because it forces you to expose these small units of functionality and
ultimately follow SOLID principles.
Unit testing itself will not catch all bugs in the system and should not
be relied upon as a sole means of testing your applications. When used in
combination with other forms of testing such as integration, functional, or
end-to-end testing, you can build up confidence that your application is
stable and delivers what is required.
Let’s see how to implement unit testing with .NET MAUI.

# Unit Testing in .NET MAUI

.NET MAUI applications are, as the name suggests, .NET-based projects,
meaning that any of the existing .NET-based unit testing frameworks can
be used.

```xml
As it currently stands, the default .NET MAUI project is not compatible
with a unit test project. I will cover how to solve this in the “Adding a
Unit Test Project to Your Solution” section.
```

There are three well-known frameworks that come with template
support in Visual Studio, meaning you can create them with File ➤ Add
New Project option. The three frameworks are listed below.

# xUnit

xUnit appears to be the choice of the .NET MAUI team. One main reason
for this is likely the support around being able to run xUnit-based unit tests
on actual devices, meaning you can test device-specific implementations.
https://xunit.net

# NUnit

NUnit is an old favorite of mine. I have used it on so many projects in the
past! It has some great features like being able to run the same test case
with multiple sets of data to reduce the amount of testing code you need to
write and ultimately maintain.
https://nunit.org

# MSTest

MSTest is a testing framework that is built and supplied by Microsoft. It
doesn’t appear as feature rich as NUnit or xUnit but it still does a great job.
https://learn.microsoft.com/dotnet/core/testing/unittesting-with-mstest

# Your Chosen Testing Framework

We will be using xUnit for this book mainly due to the benefits it brings
with being able to also run the unit tests on devices.
Tests in xUnit are decorated with the [Fact] attribute with the
expectation that as the author of the test methods you will name them in a
way that defines a fact which the test will prove to be true.
Most of the test frameworks are quite similar and tend to differ in terms
of keywords when identifying tests. Go with whatever testing framework
you are most comfortable with. If you do not have much experience
with any, perhaps experiment with each to see which gives you the best
experience. At the end of the day, you will be building and maintaining
these tests so it needs to benefit you and your team.

# Adding Your Own Unit Tests

There are some steps that you need to follow in order to make sure that you
can unit test your .NET MAUI application. Let’s add a test project to the
solution and then make the necessary changes.

# Adding a Unit Test Project to Your Solution

1. Click the File menu.
2. Click Add.
3. Click New Project.
4. Enter Test in the Search for templates box.
Figure 12-1 shows the Add a new project dialog in
Visual Studio.

![image](https://user-images.githubusercontent.com/26972859/231493948-80015487-83e8-4bea-bbd9-735952115fd2.png)
Figure 12-1. Add a new project dialog in Visual Studio

5. Select xUnit Test Project.
6. Click Next.
7. Enter a name for the project. I opted for
WidgetBoard.Tests and find that appending .Tests or
.UnitTests provides a common way to distinguish
between application and test projects. This is also
a common naming convention that simplifies
searching for all unit test projects when running
in a CI pipeline. I will cover this in more detail in
Chapter 14.
8. Click Next.
9. Select the framework. The default should be fine;
just make sure it matches the target version of the
.NET MAUI application project.
10. Click Create.

# Modify Your Application Project to Target net7.0

Sadly, the current .NET MAUI project template does not include the
net7.0 target framework, meaning that it is not initially compatible with
a standard unit test project. In order to correct this, you can manually
add the net7.0 target framework. Open the WidgetBoard/WidgetBoard.
csproj file in Visual Studio Code or your favorite text editor and make the
following changes.
Modify the first TargetFrameworks element to include net7.0; changes
are in bold:

```xml
<TargetFrameworks>net7.0;net7.0-android;net7.0-ios;net7.0-maccatalyst</TargetFrameworks>
```
Add a Condition attribute to the OutputType element; changes are
in bold:

```xml
<OutputType Condition="'$(TargetFramework)' != 'net7.0'">Exe
</OutputType>
```
Without this second change you will see a compilation error reporting
that error CS5001: Program does not contain a static 'Main' method
suitable for an entry point. This is due to the fact that you are building an
application and .NET applications expect to have a static Main method
as the entry point to the application. The OutputType for .NET MAUI
applications must be Exe, which might feel slightly confusing as you rarely
end up with an exe file that will be delivered.

---
If you are building against a newer version of .NET MAUI, you can
replace net7.0 with the version you are using, such as net8.0.
---

# Adding a Reference to the Project to Test

Now you need to add a reference from your test project onto the main
application project.
1. Right-click WidgetBoard.Tests.
2. Click Add.
3. Click Project Reference.
4. Select WidgetBoard from the list. Figure 12-2 shows
the Reference Manager dialog in Visual Studio.

![image](https://user-images.githubusercontent.com/26972859/231494592-12c69608-6b15-4e68-a8f0-3c484d8b5179.png)
Figure 12-2. Reference Manager in Visual Studio

5. Click OK.

# Modify Your Test Project to Use MAUI Dependencies

The final step is to make your test project bring in the .NET MAUI
dependencies just like the main application project. Open up the
WidgetBoard.Tests/WidgetBoard.Tests.csproj file in Visual Studio
Code or your favorite text editor and make the following changes
Add <UseMaui>true</UseMaui> into the top-level PropertyGroup
element, which should now look like this; the changes are in bold:

```xml
<PropertyGroup>
 <TargetFramework>net7.0</TargetFramework>
 <ImplicitUsings>enable</ImplicitUsings>
 <Nullable>enable</Nullable>
 <UseMaui>true</UseMaui>
  <IsPackable>false</IsPackable>
</PropertyGroup>
```
Now you have set up everything ready to begin writing and running
your unit tests.

# Testing Your View Models

The MVVM architecture lends itself very well to unit testing each
individual component.
First, you need to create a ViewModels folder in your
WidgetBoard.Tests project and then add a new class file called
BoardDetailsPageViewModelTests.cs. It is good practice to keep folders
and tests named similarly to the code that they are testing to make it easier
to organize and locate.
Now you can add in your first set of tests.

# Testing BoardDetailsPageViewModel

Inside the class file that you just created, add the following:

```Csharp
[Fact]
public void SaveCommandCannotExecuteWithoutBoardName()
{
 var viewModel = new BoardDetailsPageViewModel(null, null);
 Assert.Null(viewModel.BoardName);
 Assert.False(viewModel.SaveCommand.CanExecute(null));
}
[Fact]
public void SaveCommandCanExecuteWithBoardName()
{
 var viewModel = new BoardDetailsPageViewModel(null, null);
 viewModel.BoardName = "Work";
 Assert.True(viewModel.SaveCommand.CanExecute(null));
}
```

# Testing INotifyPropertyChanged

I covered in Chapter 4 that INotifyPropertyChanged serves as the
mechanism to keep your views and view models in sync; therefore, it can
be really useful to verify that your view models are correctly implementing
INotifyPropertyChanged by ensuring that it raises the PropertyChanged
event when it should.
The following test shows how to create an instance of the
BoardDetailsPageViewModel, subscribe to the PropertyChanged event,
modify a property that you expect to fire the PropertyChanged event, and
then Assert that the event was invoked:

```Csharp
[Fact]
public void SettingBoardNameShouldRaisePropertyChanged()
{
 var invoked = false;
 var viewModel = new BoardDetailsPageViewModel(null, null);
 viewModel.PropertyChanged += (sender, e) =>
 {
 if (e.PropertyName.Equals(nameof(BoardDetailsPageView
Model.BoardName)))
 {
 invoked = true;
 }
 };
 viewModel.BoardName = "Work";
 Assert.True(invoked);
}
```
This provides you with the confidence to know that if the BoardName is
not showing in your user interface, it will probably not be an issue inside
the view model.

# Testing Asynchronous Operations

Many modern applications involve some level of asynchronous operation
and a perfect example is your use of the Open Weather API in order to load
the current location’s weather. The WeatherWidgetViewModel relies on an
implementation of the IWeatherForecastService interface you created
in Chapter 10. Unit tests against specific implementations like this can
be considered flaky. A flaky test is one that provides inconsistent results.
Web service access can exhibit this type of behavior when unit testing
given access limits on the API or other potential issues that could impact a
reliable test run.
In order to remove test flakiness, you can create a mock
implementation that will provide a set of consistent behavior

# Creating Your ILocationService Mock

Create a new folder in your WidgetBoard.Tests project and call it Mocks. I
covered this before but organizing your code in such a way really can make
it much easier to maintain. With this new folder, you can create a new class
file inside and call it MockLocationService.cs. Modify the contents to the
following:

```Csharp
using WidgetBoard.Services;
namespace WidgetBoard.Tests.Mocks;
internal class MockLocationService : ILocationService
{
 private readonly Location? location;
 private readonly TimeSpan delay;
 private MockLocationService(Location? mockLocation,
TimeSpan delay)
 {
 location = mockLocation;
 this.delay = delay;
 }
 internal static ILocationService ThatReturns(Location?
location, TimeSpan after) =>
 new MockLocationService(location, after);
 internal static ILocationService
ThatReturnsNoLocation(TimeSpan after) =>
 new MockLocationService(null, after);
 public async Task<Location?> GetLocationAsync()
 {
 await Task.Delay(this.delay);
 return this.location;
 }
}
```
The implementation you provided for the GetLocationAsync
method forces a delay based on the supplied TimeSpan parameter in
the constructor to mimic a network delay and then return the location
supplied in the constructor.

One key detail I really like to use when building mocks is to make the
usage of them in my tests as easy to read as possible. You can see that the
MockLocationService cannot be instantiated because it has a private
constructor. This means that to use it you must use the ThatReturns or
ThatReturnsNoLocation methods. Look at this and see how much more
readable it is:

```Csharp
MockLocationService.ThatReturns(new Location(0.0, 0.0), after:
TimeSpan.FromSeconds(2));
```
The above is much more readable than the following because it
includes the intent:

```Csharp
new MockLocationService(new Location(0.0, 0.0), TimeSpan.FromSeconds(2));
```

# Creating Your WeatherForecastService Mock 

You can add a second file into the Mocks folder and call this class file
MockWeatherForecastService.cs. Modify the contents to the following:

```Csharp
using WidgetBoard.Communications;
namespace WidgetBoard.Tests.Mocks;
internal class MockWeatherForecastService :
IWeatherForecastService
{
 private readonly Forecast? forecast;
 private readonly TimeSpan delay;
 private MockWeatherForecastService(Forecast? forecast,
TimeSpan delay)
 {
 this.forecast = forecast;
 this.delay = delay;
 }
 internal static IWeatherForecastService
ThatReturns(Forecast? forecast, TimeSpan after) =>
 new MockWeatherForecastService(forecast, after);
 internal static IWeatherForecastService
ThatReturnsNoForecast(TimeSpan after) =>
 new MockWeatherForecastService(null, after);
 public async Task<Forecast?> GetForecast(double latitude,
double longitude)
 {
 await Task.Delay(this.delay);
 return forecast;
 }
}
```
The implementation you provided for the GetForecast method forces
a delay based on the supplied TimeSpan parameter in the constructor
to mimic a network delay and then return the forecast supplied in the
constructor.

# Creating Your Asynchronous Tests

With your mocks in place, you can write tests that will verify the
behavior of your application when calling asynchronous and potentially
long running operations. You need to add a new class file to your
ViewModels folder in the WidgetBoard.Tests project and call is
WeatherWidgetViewModelTests.cs and then modify the contents to the
following:

```Csharp
using WidgetBoard.Tests.Mocks;
using WidgetBoard.ViewModels;
namespace WidgetBoard.Tests.ViewModels;
public class WeatherWidgetViewModelTests
{
}
```

Now you can proceed to adding three tests to cover a variety of
different scenarios.

```Csharp
[Fact]
public async Task NullLocationResultsInPermissionErrorState()
{
 var viewModel = new WeatherWidgetViewModel(
 MockWeatherForecastService.ThatReturnsNoForecast(after:
TimeSpan.FromSeconds(5)),
 MockLocationService.ThatReturnsNoLocation(after:
TimeSpan.FromSeconds(2)));
 await viewModel.InitializeAsync();
 Assert.Equal(State.PermissionError, viewModel.State);
 Assert.Null(viewModel.Weather);
}
```
This first test, as the name implies, verifies that if a null location is
returned from the ILocationService implementation, the view model
State will be set to PermissionError and no Weather will be set.

```Csharp
[Fact]
public async Task NullForecastResultsInErrorState()
{
 var viewModel = new WeatherWidgetViewModel(
 MockWeatherForecastService.ThatReturnsNoForecast(after:
TimeSpan.FromSeconds(5)),
MockLocationService.ThatReturns(new Location(0.0, 0.0),
after: TimeSpan.FromSeconds(2)));
 await viewModel.InitializeAsync();
 Assert.Equal(State.Error, viewModel.State);
 Assert.Null(viewModel.Weather);
}
```
This second test, as the name implies, verifies that if a null forecast is
returned from the IWeatherForecastService implementation, the view
model State will be set to Error and no Weather will be set.

```Csharp
[Fact]
public async Task ValidForecastResultsInSuccessfulLoad()
{
 var weatherForecastService =
 MockWeatherForecastService.ThatReturns(
 new Communications.Forecast
 {
 Current = new Communications.Current
 {
 Temperature = 18.0,
 Weather = new Communications.Weather[]
 {
 new Communications.Weather
 {
 Icon = "abc.png",
Main = "Sunshine"
 }
 }
 }
 },
 after: TimeSpan.FromSeconds(5));
 var locationService = MockLocationService.ThatReturns(
 new Location(0.0, 0.0),
 after: TimeSpan.FromSeconds(2));
 var viewModel = new WeatherWidgetViewModel(
 weatherForecastService,
 locationService);
 await viewModel.InitializeAsync();
 Assert.Equal(State.Loaded, viewModel.State);
 Assert.Equal("Sunshine", viewModel.Weather);
}
```
This final test, as the name implies, verifies that if a valid forecast is
returned from the IWeatherForecastService implementation, the view
model State will be set to Loaded and the Weather will be correctly set.

# Testing Your Views

It is possible to write unit tests that will verify the behavior of your views.

# Creating Your ClockWidgetViewModel Mock

In order to verify your ClockWidgetView, you need to provide it with a view
model. Your ClockWidgetViewModel currently has some complexities in it
that will make it difficult to use in the test. It displays the current date/time.
Let’s create a mock to remove this potential difficulty. Inside your Mocks
folder, add a new class file called MockClockWidgetViewModel.cs and
modify the contents to match the following:

```Csharp
using WidgetBoard.ViewModels;
namespace WidgetBoard.Tests.Mocks;
public class MockClockWidgetViewModel : IWidgetViewModel
{
 public int Position { get; set; }
 public string Type => "Mock";
 public MockClockWidgetViewModel(DateTime time)
 {
 Time = time;
 }
 public DateTime Time { get; }
 public Task InitializeAsync() => Task.CompletedTask;
}
```
Now you can use this in your unit tests to verify that your
ClockWidgetView binds correctly to its view model.

# Creating Your View Tests

First, create a Views folder in your WidgetBoard.Tests project and then add
a new class file called ClockWidgetView.cs.

```Csharp
using WidgetBoard.Tests.Mocks;
using WidgetBoard.Views;
namespace WidgetBoard.Tests.Views;
public class ClockWidgetViewTests
{
 [Fact]
 public void TextIsUpdatedByTimeProperty()
 {
 var time = new DateTime(2022, 01, 01);
 var clockWidget = new ClockWidgetView();
 Assert.Null(clockWidget.Text);
 clockWidget.WidgetViewModel = new MockClockWidgetView
Model(time);
 Assert.Equal(time.ToString(), clockWidget.Text);
 }
}
```
The test TextIsUpdatedByTimeProperty creates a new
ClockWidgetView, assigns a your new MockClockWidgetViewModel, and
then verifies that that the Text property of the widget is correctly updated
to reflect the value from the Time property on your view model through its
binding.

# Device Testing

Device testing is really a form of unit testing; however, it provides some
unique abilities so it deserves its own top-level section. It essentially
enables you to write unit tests that can be run on a device and therefore
truly test any platform-specific pieces of functionality. A perfect example
of this is to test the PlatformLocationService you implemented in the
previous chapter to return the longitude and latitude coordinates of each
platform provider’s headquarters.

# Creating a Device Test Project

You need to create another project in order to handle the running of the
device tests. The documentation on the GitHub repository covers all that
is needed, so go to https://github.com/shinyorg/xunit-maui. Check in
the code repository called WidgetBoard.DeviceTests if you get stuck; there
is an already created project to use as a template.

# Adding a Device-Specific Test

```Csharp
using WidgetBoard.Services;
using Xunit;
namespace WidgetBoard.DeviceTests.Services;
public class PlatformLocationServiceTests
{
 [Fact]
 public async Task GetLocationAsyncWillReturnPlatform
SpecificLocation()
 {
 var locationService = new PlatformLocationService();
 var location = await locationService.
GetLocationAsync();
#if ANDROID
Assert.Equal(37.419857, location.Latitude);
Assert.Equal(-122.078827, location.Longitude);
#elif WINDOWS
Assert.Equal(47.639722, location.Latitude);
Assert.Equal(-122.128333, location.Longitude);
#else
Assert.Equal(37.334722, location.Latitude);
Assert.Equal(-122.008889, location.Longitude);
#endif
 }
}
```
Now that you have written your tests, you can run them on your
devices.

# Running Device-Specific Tests

In order to run your tests on a device, you first need to set your
WidgetBoard.DeviceTests project as the startup project. You can do this as
follows:
• Right-click the WidgetBoard.DeviceTests project in
Solution Explorer.
• Select Set as Startup Project.
Now start the application from Visual Studio., Figure 12-3 shows the
device test runner screen running on Windows.

![image](https://user-images.githubusercontent.com/26972859/231498799-d25896cb-cb5c-4b3e-8022-059e0a5ef0fd.png)
Figure 12-3. Device test runner on the Windows platform

You can click on a specific test and choose to run it, or you can simply
Run All Tests. This part is entirely manual so it will require a human to
perform these tasks but it can be left to run for as long as the tests need.
Finally you will see the results of the test runs and you can click them
to see more information. Figure 12-4 shows the device test runner and a
set of test results.

![image](https://user-images.githubusercontent.com/26972859/231498917-7edeb287-4761-4f24-a496-808f885be965.png)
Figure 12-4. Test run result for the GetLocationAsyncWillReturn
PlatformSpecificLocation device test

You can run these tests on all the platforms that you support to make
sure that the code does what is expected.

# Snapshot Testing

Snapshot testing is similar to unit testing, but it avoids the need to write
Assert statements to manually define each expectation in the test. Instead
the result of a test is compared to a golden master. A golden master is a
snapshot of a previous test run that you as the test author accept as the
expected result for subsequent test runs. A snapshot can be anything
ranging from a screenshot of the application to a serialization of an object
in memory. If you take a look at the WeatherWidgetViewModel you unit
tested in the earlier section, you can see that a serialization of the state
of the ValidForecastResultsInSuccessfulLoad test will result in the
following golden master being created:

```Csharp
{
 LoadWeatherCommand: {},
 IconUrl: https://openweathermap.org/img/wn/abc.png@2x.png,
 State: Loaded,
 Temperature: 18.0,
 Weather: Sunshine,
 Type: Weather
}
```
When this test is run, each time the serialized output of the
WeatherWidgetViewModel will be compared to the above golden master.
If any of the values are different from those in the golden master, the test
will fail.

# Snapshot Testing Your Application

In order to snapshot test your application, you will make use of the
excellent library called VerifyTests. VerifyTests has some really great
documentation and examples to get you started over at https://github.
com/VerifyTests/Verify.
You will additionally need to consume the Verify.Xunit NuGet
package. I have opted to create a separate project just to keep things
clearly separated for the purpose of this example. You can repeat the steps
in sections “Adding a Unit Test Project to Your Solution” and “Adding a
Reference to the Project to Test,” except that you will name the project
WidgetBoard.SnapshotTests.
Using VerifyTests, you can take a copy of your
WeatherWidgetViewModelTests class in the WidgetBoard.Tests project
and modify it to the following. The limited changes are shown in bold to
highlight the differences from the original.

```Csharp
[UsesVerify]
public class WeatherWidgetViewModelTests
{
 [Fact]
 public async Task
NullLocationResultsInPermissionErrorState()
 {
 var viewModel = new WeatherWidgetViewModel(
 new MockWeatherForecastService(null),
 new MockLocationService(null));
 await viewModel.InitializeAsync();
 await Verify(viewModel);
 }
 [Fact]
 public async Task NullForecastResultsInErrorState()
 {
 var viewModel = new WeatherWidgetViewModel(
 new MockWeatherForecastService(null),
 new MockLocationService(new Location(0.0, 0.0)));
 await viewModel.InitializeAsync();
 await Verify(viewModel);
 }
 [Fact]
 public async Task ValidForecastResultsInSuccessfulLoad()
 {
 var viewModel = new WeatherWidgetViewModel(
 new MockWeatherForecastService(new Communications.
Forecast
{
 Current = new Communications.Current
 {
 Temperature = 18.0,
 Weather = new Communications.Weather[]
 {
 new Communications.Weather
 {
 Icon = "abc.png",
Main = "Sunshine"
 }
 }
 }
 }),
 new MockLocationService(new Location(0.0, 0.0)));
 await viewModel.InitializeAsync();
 await Verify(viewModel);
 }
}
```
You remove the Assert statements and replace them by calling the
Verify method. In your original scenario, you were only asserting a small
number of things, but you can imagine that if the number of Assert
statements were to grow, then this single method call to Verify really does
reduce the complexity of your tests.
Brand new tests will always fail until you accept the golden master.
There is tooling that can make this task easier, which is again provided by
the VerifyTests developers.

# Passing Thoughts

I end this snapshot testing section with the statement that it is not for
everyone. Some people really like the reduction in test case size, while
it verifies more than most typical unit tests by the sheer fact that it
verifies the whole object under test. As a counter argument, some people
dislike that the expected state or golden master is in a file separate to the
tests. I personally believe they provide great value, and I hope that this
introduction to snapshot testing will give you enough context to decide
whether it is going to be a good fit for you and your team, or at least give
you the desire to experiment with the concept.

# Looking to the Future

I really wished this chapter could cover how to write and build tests that
can test your UI via automation tests. Sadly, this is not quite ready yet. It is
certainly something that is being looked at, but there is nothing concrete
or ready.
If you are coming in with a background in Xamarin.Forms, you may
well be aware of Xamarin.UITesting. This proved to be a little difficult
to work with and it was inconsistent at times, but it did provide the
groundwork for writing automation tests for a Xamarin.Forms application.
Currently the .NET MAUI team is evaluating a number of options to enable
you to test your applications.
There is currently the ability to test through the use of Appium
(https://appium.io); however, it can be clunky and unreliable at times.
I am most excited by the work that Jonathan Dick (the .NET MAUI
lead) is doing with Maui.UITesting. This is very much in its infancy at the
time of writing but I am expecting good things to come from it. You should
check out the details over on the GitHub repository at https://github.
com/Redth/Maui.UITesting.

# Summary

Now you have an overview of different testing techniques and the benefits
they bring. You may prefer snapshot over writing your own asserts. I don’t
mind either way so long as you do test your code.

In this chapter, you

• Learned what testing is and why it is important

• Explored unit testing and how you can apply it to a
.NET MAUI application

• Learned about snapshot testing and how you can
implement it

• Explored what device tests are and how you can apply
them to your applications

• Looked to the future for yet more testing goodness
In the next chapter, you will

• Learn what .NET MAUI Graphics is

• Gain an insight into some of the power provided by
.NET MAUI Graphics

• Build your own sketch widget with the .NET MAUI
GraphicsView control

# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch12.


