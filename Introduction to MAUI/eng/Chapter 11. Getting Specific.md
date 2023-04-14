# CHAPTER 11

# Getting Specific

In this chapter, you will be learning about .NET MAUI Essentials and how
it enables you to access platform-specific APIs without having to worry
about any of the platform-specific complexities. Two concrete examples
of are requesting permissions on each platform and accessing the
device’s geolocation information. You will explore what is required if you
really do need to interact with platform-specific APIs that have not been
abstracted for you. Finally, you will cover multiple techniques, concepts,
and architectures that enable you to tweak the UI and behavior of your
applications based on the platforms they are running on.

# .NET MAUI Essentials

In the previous chapter, you created a Weather widget. You did not finish
the job, though, as it currently only loads the weather for Maui, Hawaii.
I don’t know about you, but I am not lucky enough to live there! In this
section, you will discover what the current device’s location is in terms of
longitude and latitude, and you will then send that information up to the
Open Weather API for a much more accurate weather summary of the
user’s current location.
In order to achieve this, you need an understanding of two key
concepts: the permissions system of each operating system, and how to
access the APIs specific to GPS coordinates. Thankfully .NET MAUI has 
you covered for both scenarios, but you do need to be aware of how they
work and any platform-specific differences. Let’s take a look at each to get
a better understanding.

# Permissions

A common theme I have been discussing in this book is how .NET MAUI
does a lot of the heavy lifting when it comes to dealing with each supported
platform. This continues with permissions because .NET MAUI abstracts a
large number of permissions.

---
It is worth noting that every operating system is different. Not all
require permissions for certain features. Refer to the Microsoft
documentation on what .NET MAUI supports and what is required
for each platform at https://learn.microsoft.com/dotnet/
maui/platform-integration/appmodel/permissionsavailable-permissions.
---

There are two key methods that enable you to interact with the
permission system in .NET MAUI.

# Checking the Status of a Permission

In order to check whether the user has already granted permission to
your application, you can use the CheckStatusAsync method on the class.
For your Weather widget, you need access to the devices geolocation
information. You have two options in terms of the permission to use:
• LocationWhenInUse: This only allows the application
to access the geolocation information while the app is
open in the foreground.

LocationAlways: This allows the application to also
access the geolocation information even when the app
is backgrounded. This can be particularly useful for
exercise tracking applications that need to monitor the
user’s movement.
You only need the LocationWhenInUse option for your application.

```Csharp
PermissionStatus status = await Permissions.
CheckStatusAsync<Permissions.LocationWhenInUse>();
```

It is recommended that you check the status of the permission
before requesting it to gain an understanding of whether the user has
been asked before. On iOS, you are only allowed to ask once and then
you are required to prompt the user to go to the Settings app and enable
permission if they wish to change their mind. Sadly, Android provides a
different approach and will return a status of Denied even if the user has
not been prompted before. In this scenario you are then recommended
to call ShouldShowRationale to check whether the user really has been
prompted.
The possible values for PermissionStatus are as follows:
• Unknown: The permission is in an unknown state, or on
iOS, the user has never been prompted.
• Denied: The user denied the permission request.
• Disabled: The feature is disabled on the device.
• Granted: The user granted permission or it is
automatically granted.
• Restricted: In a restricted state

# Requesting Permission

Once you have confirmed that the user has not been prompted with a
permission request, you can proceed to prompting them by using the
Permissions.RequestAsync method along with the specific permission to
request. In your example, this will be the LocationWhenInUse permission.
PermissionStatus status = await Permissions.
RequestAsync<Permissions.LocationWhenInUse>();
It is worth noting that the RequestAsync method needs to be run on
the main or UI thread. This is needed because it can result in presenting
the built-in system UI in order to ask the user if they wish to give
permission. Therefore, whenever you call Permissions.RequestAsync you
must make sure your code is already running on the main thread with the
MainThread.IsMainThread property, or you can dispatch out to the main
thread with the MainThread.InvokeOnMainThreadAsync method.

---
It is considered best practice to only prompt the user for permission
to use a specific feature when they first try to use that feature. This
helps to provide context to the user around why the permission
is being requested. You may also find that the different platform
providers (e.g., Apple, Google, and Microsoft) have different rules they
apply when reviewing and approving the applications you submit to
their stores. For this, I recommend working with the most restrictive
rules to save yourself pain and effort.
---

# Handling Permissions in Your Application

The following section of code comes recommended from the Microsoft
documentation site at https://learn.microsoft.com/dotnet/maui/
platform-integration/appmodel/permissions?#example. It has been
included and left unchanged as it helps to really highlight the differences
between platforms.
First, create the new folder and class for this new piece of
functionality. Call the folder Services. Add a new interface file and call it
ILocationService.cs under the Services folder. The contents of this new
interface should be updated to the following

```Csharp
namespace WidgetBoard.Services;
public interface ILocationService
{
 Task<Location> GetLocationAsync();
}
```
This provides a definition of what a location service implementation
will provide: an asynchronous method that will ultimately return a
Location object.
Next, create an implementation. Add a new class file under the
Services folder and call it LocationService.cs. Modify the initial
contents to the following:

```Csharp
namespace WidgetBoard.Services;
public class LocationService : ILocationService
{
}
```
Now that you have a blank class, you can add the method for handling
permission requests ready for use.

```Csharp
private async Task<PermissionStatus>
CheckAndRequestLocationPermission()
{
 PermissionStatus status = await Permissions.
CheckStatusAsync<Permissions.LocationWhenInUse>();
 if (status == PermissionStatus.Granted)
 {
 return status;
 }
 if (status == PermissionStatus.Denied && DeviceInfo.
Platform == DevicePlatform.iOS)
 {
 // Prompt the user to turn on in settings
 // On iOS once a permission has been denied it may not
be requested again from the application
 return status;
 }
 if (Permissions.ShouldShowRationale<Permissions.
LocationWhenInUse>())
 {
 // Prompt the user with additional information as to
why the permission is needed
 }
 status = await Permissions.RequestAsync<Permissions.
LocationWhenInUse>();
 return status;
}
```
Now that you have added the ability to request the user’s permission to
use the geolocation APIs on the device, you can proceed to using it.

# Using the Geolocation API

.NET MAUI provides the ability to access each platform’s geolocation APIs
in order to retrieve a longitude and latitude representing where in the
world the device running the application is currently located. Full details
of what the API provides can be found at https://learn.microsoft.com/
dotnet/maui/platform-integration/device/geolocation.

# Registering the Geolocation Service

Open the MauiProgram.cs file and register the geolocation
implementation so that you can use it via the dependency injection layer.
You need to add the following line into the CreateMauiApp method:

```Csharp
builder.Services.AddSingleton(Geolocation.Default);
```

# Using the Geolocation Service

This now means that you can add a dependency on the IGeolocation
interface and wherever .NET MAUI provides you with an instance. Let’s
use the IGeolocation implementation in your LocationService.cs file.
There are a few modifications you need to make, so I will walk through
each one.
Add a field for the IGeolocation implementation in the root of
the class.

```Csharp
private readonly IGeolocation geolocation;
Assign the IGeolocation implementation in the constructor.
public LocationService(IGeolocation geolocation)
{
 this.geolocation = geolocation;
}
Provide the method to return a Location object.
public async Task<Location> GetLocationAsync()
{
 return await MainThread.InvokeOnMainThreadAsync(async () =>
 {
 var status = await CheckAndRequestLocationPermission();
 if (status != PermissionStatus.Granted)
 {
 return null;
 }
 return await this.geolocation.GetLocationAsync();
 });
}
```
This implementation first makes sure that you are running on the main
thread, which is required for location-based access. Then it calls your
permission handling method and, if the app has permission, it calls the
IGeolocation implementation and returns the resulting Location object.
Now you are ready to make use of the LocationService.

# Registering the LocationService

Open the MauiProgram.cs file and register the LocationService
implementation so that you can use it via the dependency injection layer.
You need to add the following line into the CreateMauiApp method:

```Csharp
builder.Services.AddSingleton<ILocationService,
LocationService>();
```

# Using the ILocationService

Let’s use the ILocationService implementation in your
WeatherWidgetViewModel.cs file. There are a few modifications you need
to make, so I will walk through each one
Add a field for the ILocationService implementation in the root of
the class.

```Csharp
private readonly ILocationService locationService;
```
Assign the ILocationService implementation in the constructor;
changes are in bold.

```Csharp
public WeatherWidgetViewModel(
 IWeatherForecastService weatherForecastService,
 ILocationService locationService)
{
 this.weatherForecastService = weatherForecastService;
 this.locationService = locationService;
 LoadWeatherCommand = new Command(async () => await
LoadWeatherForecast());
}
```
Modify your State enum to include a new value so that you can
handle when something goes wrong with permission access. Add a
PermissionError value, as can be seen below in bold.

```Csharp
public enum State
{
 None = 0,
 Loading = 1,
 Loaded = 2,
 Error = 3,
 PermissionError = 4
}
```
Modify your LoadWeatherForecast method to call your new
ILocationService implementation in order to find out the device’s
location and then use that to call the Open Weather API to find out the
weather at the device’s location.

```Csharp
private async Task LoadWeatherForecast()
{
 State = State.Loading;
 try
 {
 var location = await this.locationService.
GetLocationAsync();
 if (location is null)
 {
 State = State.PermissionError;
 return;
 }
 var forecast = await weatherForecastService.
GetForecast(location.Latitude, location.Longitude);
 Temperature = forecast.Current.Temperature;
 Weather = forecast.Current.Weather.First().Main;
 IconUrl = forecast.Current.Weather.First().IconUrl;
 State = State.Loaded;
 }
 catch (Exception ex)
 {
 State = State.Error;
 }
}
```
You have introduced a few changes here so let’s break them down.
First, you are calling the locationService to get the device’s location.
If it returns null, it means the application does not have permission and
you set the State to PermissionError.
If you have permission, you pass the device’s current location into the
weatherForecastService.GetForecast method.

# Displaying Permission Errors to Your User

You have added the new state value and also assigned it in your view
model when you either fail to retrieve the permission setting or the user
has denied permission to the LocationWhenInUse feature. Now you can
add in support into your UI to respond to this value and show something
appropriate to the user. Open the WeatherWidgetView.xaml file and make
the following modifications.
Add in the converter instance inside the <ContentView.Resources> tag

```xml
<converters:IsEqualToStateConverter
 x:Key="HasPermissionErrorConverter"
 State="PermissionError" />
```
Then you can add a section that will render when the State
property is equal to PermissionError. You should add this into the
WeatherWidgetView.xaml file after the following section:

```xml
<!-- Error -->
<VerticalStackLayout
 IsVisible="{Binding State,
Converter={StaticResource HasErrorConverter}}">
 ...
</VerticalStackLayout>
```
The section you want to add is as follows:

```xml
<!-- PermissionError -->
<VerticalStackLayout
 IsVisible="{Binding State, Converter={StaticResource
HasPermissionErrorConverter}}">
 <Label
 Text="Unable to retrieve location data" />
 <Button
 Text="Retry"
 Command="{Binding LoadWeatherCommand}" />
</VerticalStackLayout>
```
Now that you have added all of the required bits of code to call into
the Permissions and Geolocation APIs, you need to configure each of your
supported platforms to enable the location permission.

# Configuring Platform-Specific Components

This is where .NET MAUI stops holding your hand and requires you to
do some work in the platform-specific folders. Many of the APIs that are
provided by .NET MAUI, as detailed in this section of the documentation
site at https://learn.microsoft.com/dotnet/maui/platformintegration/, have the potential to require some level of platformspecific setup. This will vary per platform. For example, for haptic support,
only Android requires some setup, whereas for the Geolocation API, all
platforms require some setup.
Thankfully .NET MAUI provides helpful exceptions and error messages
if you miss any of the platform-specific setup and they usually indicate
the action required to fix the issue. Topics like this do make it imperative
that you really test your application on each of the platforms you wish to
support to verify that it behaves as expected. Let’s set up each platform so that your app can fully support accessing
the devices’ current location.

# Android

Android requires several permissions and features to be configured in
order for your application to use the LocationWhenInUse permission. You
can configure them inside the Platforms/Android/MainApplication.cs
file so open it and make the following additions in bold:

```Csharp
using Android.App;
using Android.Runtime;
[assembly: UsesPermission(Android.Manifest.Permission.
AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.
AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location",
Required = false)]
[assembly: UsesFeature("android.hardware.location.gps",
Required = false)]
[assembly: UsesFeature("android.hardware.location.network",
Required = false)]
namespace WidgetBoard;
```
Note that the use of the assembly keyword requires that the attributes
are applied at the assembly level and not on the class like the current
[Application] attribute usage. For further reference on how to get started
with geolocation, refer to the Microsoft documentation at https://
learn.microsoft.com/dotnet/maui/platform-integration/device/
geolocation?tabs=android-get-started.

If you run the application on Android now, you will see that the first
time you add a Weather widget onto a board, the system will present the
following popup to the user asking them to allow permission for your
application to use the location feature. Figure 11-1 shows the result of
running your application on Android.

![image](https://user-images.githubusercontent.com/26972859/231486435-b082c9f9-79af-4c58-bc42-eb815a1cb535.png)
Figure 11-1. The application running on Android showing the
permission prompt when a weather widget is first added to a board

# iOS/Mac

Apple requires that you specify the reason your application wants to use
the Geolocation feature in the process of defining that your application
uses the feature. You can configure this by modifying the Platforms/iOS/
Info.plist and Platforms/MacCatalyst/Info.plist files for iOS and
Mac Catalyst, respectively. Both files require the same change, so let’s open
them and add the following lines in. Note that I am opting to edit the files
inside Visual Studio Code as I find it provides a better editing experience.
There is a built-in editor inside Visual Studio but I personally prefer to edit
the XML directly. Add the following lines inside the <dict> element:

```xml
<key>NSLocationWhenInUseUsageDescription</key>
<string>In order to provide accurate weather
information.</string>  
```
For further reference on how to get started with Geolocation refer
to the Microsoft documentation at https://learn.microsoft.com/
dotnet/maui/platform-integration/device/geolocation?tabs=ios -
get-started
If you run the application on iOS and macOS now, you will see that the
first time you add a Weather widget onto a board, the system will present
the following popup to the user asking them to allow permission for your
application to use the location feature. Figure 11-2 shows the result of
running the application on iOS.

![image](https://user-images.githubusercontent.com/26972859/231486775-9169a4ff-9726-4bd8-bd2b-0a3065aff5c9.png)
Figure 11-2. The application running on iOS (left) and macOS (right)
showing the permission prompt when a weather widget is first added
to a board

# Windows

Windows applications have the concept of capabilities and it is up to
developers to declare which capabilities are required in their applications.
In order to do so for your application, you need to modify the Platforms/Windows/Package.appxmanifest file. Note that I am opting to
edit the files inside Visual Studio Code as I find it provides a better editing
experience. Add the following line inside the <Capabilities> element:

```xml
<DeviceCapability Name="location"/>
```
For further reference on how to get started with Geolocation refer to
the Microsoft documentation at https://learn.microsoft.com/dotnet/
maui/platform-integration/device/geolocation?tabs=windowsget-started.
If you run the application on Windows now, you don’t see a permission
request popup. Figure 11-3 shows the result of running the application on
Windows.

![image](https://user-images.githubusercontent.com/26972859/231487071-a404f50a-2c09-45be-bfce-7e71c026bfd8.png)
Figure 11-3. The application running on Windows showing the
permission prompt when a weather widget is first added to a board
  
# Platform-Specific API Access  
  
While .NET MAUI does provide you with a lot of functionality out of the
box, there can be times when you need to write your own interaction with
the platform-specific layer to achieve your goals. Whatever functionality
can be achieved on a specific platform can also be achieved within a .NET
MAUI application. You just might have to do the heavy lifting yourself. If
your implementation is considered useful enough to other developers, you
should propose the changes back to the .NET MAUI team.
There are two main concepts you can utilize when building platformspecific code in .NET MAUI. Let’s take a look at each one through the
simple example of building a LocationService that returns the longitude
and latitude of the headquarters for each platform provider (e.g., Google,
Apple, and Microsoft).  
  
# Platform-Specific Code with Compiler Directives 
  
You will most likely come across a usage of the #if compiler directive
when working on a .NET MAUI application. I am not a big fan of them but I
do accept that in some scenarios they do provide value.  
  
```Csharp
namespace WidgetBoard.Services;
public class PlatformLocationService : ILocationService
{
 public Task<Location> GetLocationAsync()
 {
 Location location;
#if ANDROID
 location = new Location(37.419857, -122.078827);
#elif WINDOWS
 location = new Location(47.639722, -122.128333);
#else
 location = new Location(37.334722, -122.008889);
#endif
 return Task.FromResult(location);
 }
}
```
The above code will be compiled in different ways based on the target
platform. The resulting compiled code for the Android platform looks as
follows:
  
```Csharp
namespace WidgetBoard.Services;
public class PlatformLocationService : ILocationService
{
 public Task<Location> GetLocationAsync()
 {
 Location location;
 location = new Location(37.419857, -122.078827);
 return Task.FromResult(location);
 }
}  
```

This means that only the code specific to the platform will be compiled
and shipped to that platform.
This approach can work well in this scenario, but as soon as you need
to use multiple classes or other platform-specific libraries, the code will
become complex very quickly. In more complex scenarios, you can use the
platform-specific folders created in your project for you.  
  
# Platform-Specific Code in Platform Folders  
  
I briefly covered these folders in Chapter 2. Each platform has a folder and
the files inside each folder (e.g., /Platforms/Android/) will only be compiled
for that platform when you are targeting it. In order to create the same
PlatformLocationService from the previous section, you first need to
create a partial class under the Services folder with the following contents:  
  
```Csharp
namespace WidgetBoard.Services;
public partial class MultiPlatformLocationService :
ILocationService
{
}  
```

The above code will not compile now because you haven’t implemented
ILocationService. This is expected until you add in your platform-specific
implementations, so don’t worry. You add the partial keyword because
this is only a partial implementation. The platform-specific files and classes
you will add shortly will complete this partial implementation.
Next, you need to create your Android platform-specific
implementation. To do this, you add a new class file under the
/Platforms/Android/ folder and call it PlatformLocationService.cs,
just like the one above. You want to modify its contents to the following:  
  
```Csharp
namespace WidgetBoard.Services;
public partial class MultiPlatformLocationService
{
 public Task<Location> GetLocationAsync()
 {
 return Task.FromResult(new Location(37.419857,
-122.078827));
 }
}
``` 
This class will only be compiled when the Android platform is being
targeted and therefore you get a very similar compiled output to the one
in the “Platform-Specific Code with Compiler Directives” section. The
key difference is that you don’t need to add any of those unpleasant #if
directives.  
  
```Csharp
When building platform-specific implementations this way, the
namespace of your partial classes must match! Otherwise, the
compiler won’t be able to build a single class.  
```
  
# Overriding the Platform-Specific UI
 
One fundamental part of .NET MAUI is in the fact that it utilizes the
underlying platform controls to handle the rendering of our applications.
This will result in our applications looking different on each of the
platforms. In the majority of scenarios, this is considered a good thing
because the application is in keeping with the platform’s look and feel.
At times, though, you will need to override some of the platform-specific
rendering or even just to tweak how controls render in your application on
a specific platform.  
  
# OnPlatform  
  
A common example of needing to change control properties are around
the sizing of text or spacing around controls (Margin or Padding). I always
find that the final finishing touches to get an application feeling really slick
and polished can result in needing to tweak details like this per platform.
There are two main ways to achieve this, and they depend on whether
you are a XAML or C# oriented UI builder. Let’s look over both with an
example.  
  
# OnPlatform Markup Extension  
  
XAML, as mentioned, is not as feature-rich in terms of what can be written
and achieved. Therefore, additional functionality is provided by .NET
MAUI to overcome these limitations. One such example is the OnPlatform
markup extension. XAML markup extensions help enhance the power and
flexibility of XAML by allowing element attributes to be set from a variety
of sources.
You might decide that in your ClockWidgetView.xaml file the FontSize
property is too large for iOS and Android and opt to change it only for
those platforms. Let’s take a look at the code and see how you can modify
the property based on the platform the application is running on. 
  
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Label
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:viewmodels="clr-namespace:WidgetBoard.ViewModels"
 x:Class="WidgetBoard.Views.ClockWidgetView"
 FontSize="60"
 VerticalOptions="Center"
 HorizontalOptions="Center"
 x:DataType="viewmodels:ClockWidgetViewModel"
 Text="{Binding Time}">
 <Label.BindingContext>
 <viewmodels:ClockWidgetViewModel />
 </Label.BindingContext>
</Label>  
```
The code above shows that the FontSize property is currently fixed to
a value of 60. With the OnPlatform markup extension, you can change this
value based on the platform the application is running on. The following
code example shows how you can retain the default value of 60 and then
override for the platforms that you wish:  

```xml
FontSize="{OnPlatform Default=60, Android=25, iOS=30}"  
```
The code example above states that all platforms will default to using a
FontSize of 60 unless the application is running on Android and a value of
25 will be used or if the application is running on iOS and a value of 30 will
be used. 
  
# Conditional Statements  
  
If you had built your UI in C# or wanted to at least modify the FontSize
property of a Label control in a similar way you could write the following
conditional C# statement:  
  
```Csharp
public ClockWidgetView()
{
 if (DeviceInfo.Platform == DevicePlatform.Android)
 {
 FontSize = 25;
 }
 else if (DeviceInfo.Platform == DevicePlatform.iOS)
 {
 FontSize = 30;
 }
 else
 {
 FontSize = 60;
 }
}  
```
For further information on using the OnPlatform markup extension
and other possible markup extensions that enable the customization
of your application, please refer to the Microsoft documentation at
https://learn.microsoft.com/dotnet/maui/xaml/markup-extensions/
consume#onplatform-markup-extension.
There will be times when just overriding values like this is not enough.
For the more complex scenarios, you need to consider an architecture that
is completely new to .NET MAUI and that is the handler architecture.  
  
# Handlers

Handlers are an area where .NET MAUI really shines! If you have
come from a Xamarin.Forms background, you will appreciate the pain
that custom renderers brought. If you don’t have any Xamarin.Forms
experience, you are very lucky! I won’t dig down too deep into the details
of the old approach as this is a book on .NET MAUI and not the past;
however, I feel there is value in talking about the old issues and how they
have been overcome by the new handler architecture.
In both Xamarin.Forms and .NET MAUI, we predominantly build our
user interfaces with abstract controls: controls defined in the Microsoft
namespace and not specifically any platform controls. These controls
eventually need to be mapped down to the platform-specific layer. In the
Xamarin.Forms days, you would have a custom renderer. The renderer
would be responsible for knowing about the abstract control and also the
platform-specific control and mapping property values and event handlers
and such between the two. This is considered a tightly coupled design,
meaning that it becomes really quite difficult to enhance the controls and
their rendering. If you wanted to override a small amount of behavior,
you would have to implement a full renderer responsible for mapping all
properties/events. This was very painful!  
In .NET MAUI, this concept of renderers has been entirely replaced
with handlers. This new architecture provides some extra layers between
the abstract controls in the .NET MAUI namespace and the underlying
platform-specific controls being rendered in our applications. This is
considered much more loosely coupled, mainly due to the fact that each
control will implement a number of interfaces and it is the handler’s
responsibility to interact with the interface rather than the specific control.
This has many benefits including the fact that multiple controls can all
implement the same interface and ultimately rely on the same single
handler. It also provides the ability to define smaller chunks of common
functionality and, as you all know, smaller classes and files are much
easier to read, follow, and ultimately maintain. Figure 11-4 shows how the
abstract Button class in .NET MAUI is mapped to the specific controls on
each platform.  
  
![image](https://user-images.githubusercontent.com/26972859/231488966-8cfc6c41-57cb-4671-96cd-82085ebe13d1.png)
Figure 11-4. The handler architecture in .NET MAUI  
  
If you wish to create a new control that needs to map to platformspecific implementations, you should follow the pattern shown in
Figure 11-4. For example, if you made your FixedWidgetBoard a control
in this manner, you would also create an IFixedWidgetBoard interface
and then a FixedWidgetBoardHandler and then map from the virtual view
across to a platform view. You didn’t take this approach in your scenario
because there was no benefit. In fact, it would result in more code because  
you would need to map to each platform individually. This concept may
sound like it will always cause more effort; however, in the situation of a
Button, it makes sense because each platform already has a definition of
what a button is and how it behaves.
Quite often as application developers you will be using existing
controls rather than building your own controls, so rather than needing
to build everything you see in Figure 11-4, you can customize controls
through the use of handlers.
  
# Customizing Controls with Mappers  
  
Mappers are key to the handler architecture. They define the actions that
will be performed when either a property is changed or a command is sent
between cross-platform controls and platform-specific views. This piece
of information in itself might not be that helpful, but once you gain an
understanding of how to modify these actions or provide new ones, you
can start to understand just how powerful this can be. The majority of the
.NET MAUI handlers are in the Microsoft.Maui.Handlers namespace,
which makes them relatively easy to discover. There are a few exceptions
to that rule, which are defined in their documentation at https://learn.
microsoft.com/dotnet/maui/user-interface/handlers/#handlerbased-views.

---
It is important to note that by modifying the mappers for handlers,
you will be overriding the behavior for all implementations of the
control it handles. You can overcome this by creating a class (e.g.
MyButton) that inherits from the control you wish to enhance
(e.g. Button) and then having your handler target the new class
(MyButton).
---
  
# Scoping of Mapper Customization
  
All controls in .NET MAUI that utilize the handler architecture also provide
HandlerChanging and HandlerChanged events, or OnHandlerChanging
and OnHandlerChanged methods, meaning you can subscribe to them and
customize the look and feel of a specific control instance.
  
# Further Reading

One great example of overriding controls in such a way is a talk by Peter
Marchev, a Telerik developer, showing how you can customize individual
components in their charting control with very limited amounts of code.
The talk can be viewed at www.youtube.com/watch?v=s7WfTT-MVSg.
  
# Summary  
  
In this chapter, you

  • Learned about permissions on the various platforms
and how to request them

  • Learned how to use the Geolocation API

  • Wrote your own platform-specific interaction when
necessary

  • Discovered how to tweak the UI based on the platform
upon which your application is running

  • Further tweaked the UI through the use of the handler
architecture

  In the next chapter, you will

  • Learn what testing is and why it is important. 
  
Cover what unit testing is and how you can apply it to a
.NET MAUI application.
  
• Learn what snapshot testing is and how you can
implement it.

  • Gain an understanding of device tests and how you can
apply them to your applications.

  • Look to the future for yet more testing goodness.  
  
# Source Code  

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch11.  
  
# Extra Assignment  
  
You have only scratched the surface on the platform integration APIs that
.NET MAUI offers you. I would love for you to look over the other possible
APIs and build your own widgets that would benefit from them. The
documentation for the platform integration APIs can be found at https://
learn.microsoft.com/dotnet/maui/platform-integration/.  
  
# Barometer Widget
  
You can make use of the Barometer API in order to report the ambient
air pressure back to the user. In fact, this might be a good addition to the
Weather widget rather than a whole new widget. The documentation for
this API can be found at https://learn.microsoft.com/dotnet/maui/
platform-integration/device/sensors?#barometer.
  
# Geocoding Lookup
  
I am reluctant to enable permissions like location access to apps I don’t
believe really need them. Perhaps you can enhance your Weather widget
to allow the user to supply their nearest city, town, or postal code and
then use the Geocoding API to reverse lookup the longitude and latitude
information required for the Open Weather API. The documentation
for the Geocoding API can be found at https://learn.microsoft.com/
dotnet/maui/platform-integration/device/geocoding.  
  
  

