# CHAPTER 3

# The Fundamentals of .NET MAUI

In this chapter, you will dissect the project you created in Chapter 2
and dive into the details of each key area. The focus is to provide a good
overview of what a .NET MAUI single project looks like, where each of the
key components are located, and some common ways of enhancing them.

## Project Structure

.NET MAUI provides support for multiple platforms from within a single
project. The focus is to allow us as developers to share as much code and
as many resources as possible.
You will likely hear the term single project a lot during your time
working with .NET MAUI. It is a concept that is new to the .NET world as
part of .NET MAUI. Its key feature is that you can build applications for
multiple different targets from, you guessed it, a single project. If you have
ever built .NET applications that aim to share code, you will have noticed
that each application you wanted to build and deploy required its own
project. The same was true with Xamarin.Forms in that you would have
at least one project with your common code and then one project per
platform. The single project now houses both the shared code and the
platform-specific bits of code.

Figure 3-1 shows a comparison between the old separate project
approach in Xamarin.Forms and the new .NET MAUI project format.

![image](https://user-images.githubusercontent.com/26972859/231077346-7c1360b9-d65c-4993-9e7e-b4761b1069a2.png)

Figure 3-1. Comparison of Xamarin.Forms projects to a .NET
MAUI project

Let’s inspect the project you created in Chapter 2 so that you can
start to get an understanding of how .NET MAUI supports the multiple
platforms and how they relate to shared code.
The new project has the following structure:

- Platforms/: This folder contains all the platformspecific code. Inside this folder is a set of folders, each
with a name that relates to the platform that it supports.
Thus Platforms/Android supports the Android
platform.

- Resources/: This folder is where you store all your
resources for the application. A resource is typically
anything you wish to embed in the application that
isn’t strictly code, such as an image, a font file, or even
an audio or video file.

In the past, resource management was always a pain point when
building cross-platform applications. For example, building an application for both Android and iOS with Xamarin.Forms could result in needing four
or five different sizes of each image rendered in the application.
MauiProgram.cs: This class is where you initialize your
.NET MAUI application. It makes use of the Generic
Host Builder, which is the Microsoft approach to
encapsulating the requirements of an application.
These requirements include but are not limited to
dependency injection, logging, and configuration.

  • App.xaml.cs: This is the main entry point to the crossplatform application. Note this line of code from the
MauiProgram.cs file includes our App class:
builder.UseMauiApp<App>();

  • App.xaml: This file includes common UI resources
that can be used throughout the application. I will
cover these types of resources in much more detail in
Chapters 5 and 8.

  • MainPage.xaml and MainPage.xaml.cs: These two files
combine to make up your application’s first page.

  • AppShell.xaml and AppShell.xaml.cs: These two files
enable you to define how your application will be laid
out through the use of the .NET MAUI concept called
Shell. I will cover Shell extensively in Chapter 5.
Note that wherever you see a .xaml file, there will typically be an
associated .xaml.cs file. This is due to limitations in what XAML can
provide; it requires an associated C# file to cover the parts that XAML does
not support. I will cover XAML much more extensively in Chapter 5.
It is also worth noting that you do not have to write any XAML. Sure,
.NET MAUI and its predecessor, Xamarin.Forms, have a deep connection
to XAML but because the XAML is ultimately compiled down to C#, anything that is possible to create in XAML is also possible in C#. You will
look through the different possibilities for architecting your applications in
the next chapter (Chapter 4).

## /Platforms/ Folder

I mentioned that the platform-specific code lives in the Platforms folder.
While cross-platform applications provide a nice abstraction from the
platforms we wish to support, I still believe it is extremely valuable to know
how these platforms behave. Let’s dive in and look at each of the platform
folders to understand what is happening.

### Android

Inside the Android platform folder you will see the following files:
  
  • MainApplication.cs: This is the main entry point for
the Android platform. Initially you should note that
it does very little. The bit it does is rather important,
though; it is responsible for creating the MauiApp using
the MauiProgram class. This is the bridge between the
Android application and your cross-platform .NET
MAUI code.
  
• MainActivity.cs: An activity in Android development
is a type of app component that provides a user
interface. The MainActivity starts when your app is
loaded. This is typically done by tapping the app icon;
however, it can also be triggered by a notification or
other source.
  
• AndroidManifest.xml: This file is extremely important.
It is how you define the components that make up your
application, any permissions it requires, the application version information, the minimum and target SDK
versions, and any hardware or software features that it
requires.

### iOS

Inside the iOS platform folder, you will see the following files:
• AppDelegate.cs: This class allows you to respond to all
platform-specific parts of the application lifecycle.
• Info.plist: This file contains configuration about
the application. It is like the AndroidManifest.xml file
discussed in the Android section. You can change the
application’s version and include reasons why your
application requires permission to use certain features.
• Program.cs: This is the main entry point.

### MacCatalyst

Inside the MacCatalyst platform folder, you will see the following files.
It is worth noting that this section is nearly identical to the previous iOS
section. It’s been kept separate to provide an easy reference to what the
platform folder consists of for MacCatalyst.
• AppDelegate.cs: This class allows you to respond to all
platform-specific parts of the application lifecycle.
• Info.plist: This file contains configuration about
the application. It is like the AndroidManifest.xml
file discussed in the Android section: you can change
the application version and include reasons why your
application requires permission to use certain features.
• Program.cs: This is the main entry point.

### Tizen

Inside the Tizen platform folder, you will see the following files:
• Main.cs: This is the main entry point for your Tizen
application.
• tizen-manifest.xml: This file is very similar to the
AndroidManifest.xml file. It is how you define the
components that make up your application, any
permissions it requires, the application version
information, the Tizen API version, and any hardware
or software features it requires.

### Windows

Inside the Windows platform folder, you will see the following files:
• app.manifest: The package manifest is an XML
document that contains the info the system needs to
deploy, display, or update a Windows app. This info
includes package identity, package dependencies,
required capabilities, visual elements, and extensibility
points. Every app package must include one package
manifest.
• App.xaml and App.xaml.cs: The main entry points for
your Windows application
• Package.appxmanifest: An application manifest is an
XML file that describes and identifies the shared and
private side-by-side assemblies that an application
should bind to at run time. They should be the
same assembly versions that were used to test the
application. Application manifests may also describe
metadata for files that are private to the application.

# Summary

Phew! That felt like a lot to take in! I think I need to take a tea break! Don’t
worry, though; while this gives an overview of what each of the files are
responsible for, you will be modifying most of them throughout this book
with some practical examples so if there are any points that aren’t clear, or
you feel you will need to revisit them, you certainly will be.

## /Resources/ Folder
  
The Resources folder is where you store anything you want to include in
your application that is not strictly code. Let’s look through each of the
sub-folders and key types of resource. 
  
## Fonts  
  
.NET MAUI allows you to embed your own custom fonts. This is especially
handy if you are building an app for a specific brand, or you want to make
sure that you render the same font on each platform. You can embed either
True Type Fonts (.ttf files) or Open Type Fonts (.otf files).
  
---
  A word of warning around fonts. I strongly recommend that you
check the licensing rules around fonts before including them in your
application. While there are sites that make it possible to download
fonts freely, a very large percentage of those fonts usually require
paying to use them.
---
  
There are two parts to embedding a font so that it can be used within
your application.
  
 1. The font file should be place in this folder
(Resources/Fonts).
  
  By default, the font will be automatically included
as a font file based on the following line that can be
found inside the project file (WidgetBoard.csproj):
<MauiFont Include="Resources\Fonts\*" />
What the above line does is set the ```build action``` of
the file you just included to be of type MauiFont.
If you want to perform this manually, you can rightclick the file inside Visual Studio, click **Properties**,
and inside the Properties panel set the **Build Action**
to MauiFont.
  
2. Configure the font.
When bootstrapping your application, you need
to specify which fonts you wish to load. This is
performed with the following lines inside your
MauiProgram.cs file:
  
```Csharp
.ConfigureFonts(fonts =>
{
 fonts.AddFont("Lobster-Regular.ttf", "Lobster");
});  
```
In the above example, you add the font file LobsterRegular.ttf to the collection of fonts and give it an
alias of Lobster. This means you can just use the
name of Lobster when referring to the file in your
application. 
  
### Images  
  
Practically every application you build will include some images. Each
platform that you wish to support has its own rules on the image sizes that
you need to supply to make the image render as sharp and clear on the many devices they run. Take iOS, for example. In order to supply a 24x24
pixel image in your app, you must provide three different image sizes:
24x24, 48x48, and 72x72. This is due to the different DPIs for the devices
Apple builds. Android devices follow a similar pattern but the DPIs are not
the same. This is similar for Windows.
Figure 3-2 shows an example image that would be rendered at 24x24
pixels. Note that while Windows shows the three sizes, this is just based off
recommendations for trying to cover the most common settings. In truth,
Windows devices can have their DPIs vary much more. Figure 3-2 shows
the required image sizes needed for all supported platforms in order to
render a 24x24 pixel image.  
  
 ![image](https://user-images.githubusercontent.com/26972859/231078950-26519e2f-b6b6-4c12-90cb-73bc51252bc8.png)
Figure 3-2. Required image sizes across the various platforms
 
You can see from the figure above that it can become painful very
quickly if you have lots of images in your application each requiring at least
five different sizes to be maintained. Thankfully .NET MAUI gives us the
ability to provide a single Scalable Vector Graphic (SVG) image and it will
generate the required images for all the platforms when the application is
compiled. I cannot tell you how happy all of us Xamarin.Forms old timers
are at this new piece of functionality!  
  
As it currently stands, if the SVG image is of the correct original size,
you can simply drop the image into the /Resources/Images/ folder and it
will just begin to work in your application. In a similar way to how the fonts
are automatically picked up, you can see how the images are also handled
by looking inside your project file and observing the line <MauiImage
Include="Resources\Images\*" />
  
---
  .NET MAUI doesn’t render SVGs directly but generates PNG images
from the SVGs at compile time. This means that when you are
referring to the image you wish, it needs to have the .png extension.
For example, when embedding an image called image.svg, in code
you refer to it as image.png.
---
  
If the contents of the SVG are not of the desired size, then you can add
some configuration to tell the tooling what size the image should be. For
this the image should not be added to the /Resources/Images/ folder as
the tooling will end up generating duplicates and there is no telling which
one will win. Instead, you can simply add the image to the /Resources/
folder and then add the following line to your project file:  
  
```xml
<MauiImage Include="Resources\image.svg" BaseSize="24,24" />  
```
 The above code will treat the contents of the image.svg file as being
24x24 pixels and then scale for each platform based on that size. 
  
### Raw  
  
Your final type of resource to embed is raw files. This essentially means
that what is embedded can be loaded at runtime. A typical example of this
is to provide some data to preload into the application when first starting  
  
## Where To Begin?
  
.NET MAUI applications have a single main entry point that is common
across all platforms. This provides us with a way to centralize much of the
initialization process for our applications and therefore only write it once.
You will have noticed that in each of the platform-specific main
entry points covered in the previous section they all called MauiProgram.
CreateMauiApp();. This is the main entry point into your .NET MAUI and
shared application.
The CreateMauiApp method allows you to bootstrap your application.
Bootstrapping refers to a self-starting process that is supposed to continue
or grow without external input (Wikipedia quote). This means that
your implementation in this method is responsible for configuring the
application from setting up logging, general application configuration, and
registering implementations to be handled with dependency injection.
This is one of the big improvements in .NET MAUI over Xamarin.Forms.
This is done through the Generic Host Builder.  
  
 ## Generic Host Builder 
  
 I mentioned back in Chapter 1 that one of the benefits that comes with the
evolution to .NET MAUI is powerful dependency injection. The Generic
Host Builder is tried and tested through other .NET frameworks such as
ASP .NET Core and it has thankfully become available to all application
types now.
Before we jump in to how the Generic Host Builder works, let’s look at
what exactly dependency injection is and why you should use it.

# What Is Dependency Injection?
 
**Dependency injection** (DI) is a software design pattern aimed at reducing
hard-coded dependencies in a software application. A dependency is
an object that another object depends on. This hard-coded dependency
approach is referred to as being tightly coupled. Let’s work through an
example to show how and why it’s named so and how you can remove the
need for the hard-coded dependencies thus making your design loosely
coupled.
So, my wife is a fantastic baker. She bakes these beautiful, delicious
cakes and this is the main reason I have gained so much weight recently.
I am going to use the process of her baking a cake to show this concept of
dependencies.  
  
```Csharp
public class Baker
{
 public Cake Bake()
 {
 }
}  
```
  
The above code looks relatively straightforward, right? She bakes a
cake. Now let’s consider how she might go about making the cake. She
needs a way of sourcing the ingredients, weighing them, mixing them, and
finally baking them. We end up with something like  
  
```Csharp
public class Baker
{
 private readonly WeighingScale weighingScale = new WeighingScale();
 private readonly Oven oven = new Oven();
 private readonly MixingMachine mixingMachine = new MixingMachine();
 private readonly IngredientsProvider ingredientsProvider = new IngredientsProvider();
 public Cake Bake()
 {
   Ingredient ingredient = ingredientsProvider.Provide();
   weighingScale.Weigh(ingredient);
 }
}
```
 We can see that for the Baker to do their job, they need to know
about all these different pieces of equipment. Now imagine that the
WeighingScale breaks, and a replacement is provided. Baker will still need
to weigh the ingredients but won’t care how that weighing is performed.
Imagine that the new WeighingScale is digital and now requires batteries.
There are a few reasons why we want to move away from having hardcoded dependencies as in our Baker example. 
  
 If we did replace the WeighingScale with a different
implementation, we would have to modify the
Baker class.
• If the WeighingScale has dependencies (e.g., batteries
in our new digital scale), they must also be configured
in the Baker class.
• This becomes more difficult to unit test because the
Baker is creating dependencies and therefore a unit test
would result in having to test far more than a unit test is
designed to.
Dependency injection can help us to address the above issues by
allowing us to achieve Inversion of Control (IoC). Inversion of Control
essentially means that we are inverting the knowledge of the dependency 
  
from the Baker knowing about a WeighingScale to them knowing about
something that can weigh ingredients but not an actual implementation.
This is done through the introduction of an interface which we will call
IWeighingScale.  
  
```Csharp
public class Baker
{
 private readonly IWeighingScale weighingScale;
 private readonly Oven oven = new Oven();
 private readonly MixingMachine mixingMachine = new MixingMachine();
 private readonly IngredientsProvider ingredientsProvider = new IngredientsProvider();
 public Baker(
 IWeighingScale weighingScale)
 {
    this.weighingScale = weighingScale;
 }
 public Cake Bake()
 {
   Ingredient ingredient = ingredientsProvider.Provide();
   this.weighingScale.Weigh(ingredient);
 }
}
```
Now our Baker knows about an interface for something that can weigh
their ingredients but not the actual thing that does the weighing. This
means that in the scenario where the weighing scale breaks and a new
one is supplied, there is no change to the Baker class in order to handle
this new scale. Instead, it is registered as part of the application start-up  
  
or bootstrapping process. Of course, we could and should follow the same
approach for our other dependencies.
One additional concept I have introduced here is the use of constructor
injection. Constructor injection is the process of providing the registered
dependencies when creating an instance of our Baker. So, when our Baker
is created, it is passed an instance of WeighingScale.
If you have a background with Xamarin.Forms, you will have come
across the DependencyService. This provided a mechanism for managing
dependency injection within an application; however, it received criticism
in the past for not supporting constructor injection. This doesn’t mean
it wasn’t possible to achieve constructor injection in Xamarin.Forms
applications but it required the use of a third-party package and there are a
lot of great packages out there! Now it is all baked into .NET MAUI.  
  
## Registering Dependencies  
  
In the previous section, I discussed how to minimize concrete
dependencies in your code base. Now let’s look through how to configure
those dependencies so that the dependents are given the correct
implementations.
Implementations that you register in the generic host builder are
referred to as services and the work of providing the implementations out
to dependents is referred to as the ServiceProvider. You can register your
services using the following.
  
## AddSingleton

A singleton registration means that there will only ever be one instance
of the object. So, based on the example of our Baker needing to use an
IWeighingScale, we register it as follows:  
  
```Csharp
builder.Services.AddSingleton<IWeighingScale, WeighingScale>();
```
Then every time that an IWeighingScale is resolved, we will be
provided with the same instance. This suits the weighing scale example
because we use the same one throughout our baking process.  
  
---
It is extremely unlikely that you will ever need to register a view
model as a singleton. Doing so can introduce bits of behavior that you
are most likely not expecting on top of the fact that you can run the
risk of leaking memory.
---
  
## AddTransient  
  
 A transient registration is the opposite of a singleton. Every time an
implementation is resolved, a new instance is created and provided. So
based on the example of our Baker needing to use an IWeighingScale, we
register it as follows: 
  
```Csharp
builder.Services.AddTransient<IWeighingScale, WeighingScale>();
```
As mentioned, every time an IWeighingScale is resolved, we will
be provided with a new instance. A better example here might be the
greaseproof paper that lines the cake tins. They are used once and
thrown away.  
  
## AddScoped  
  
A scoped registration is somewhere in the middle of a singleton and
transient. A single instance will be provided for a “scope,” and then when
a new scope is created, a new instance will be provided for the life of
that scope.  
  
```Csharp
builder.Services.AddScoped<IWeighingScale, WeighingScale>();
```
  
This type of registration feels much better suited to a web application
where requests come in and a scope will represent a single request. In the
mobile and desktop world, your application typically has a single state and
therefore is less likely to need scoped registrations. Currently .NET MAUI
does not provide any automatic creations of scopes, but you have the
power to create your own using the IServiceScopeFactory interface and
ultimately its implementation.  
  
# Application Lifecycle  
  
Sadly, no two platforms provide the same set of behaviors or lifecycle
events such as when an application is started, backgrounded, or closed.
This is where cross-platform frameworks provide us with a solid set
of encapsulated events to cover most scenarios. There are four main
application states in a .NET MAUI application.  
  
## Application States
  
These are the application states:
• Not running: This means that the application has
not been started and is not loaded into memory. This
is typically when the application has been installed,
the device has been powered on, the application
was closed by the user, or the operating system has
terminated the application to free up some resources.
• Running: This means that the application is visible and
is focused.
• Deactivated: This means that the application is no
longer focused but may still be visible. On mobile, this
could mean that the operating system is showing a permission request alert (e.g., an application asking for
permission to use the camera) or similar.
• Stopped: This means that the application is no longer
visible.  
  
You can now see how a .NET MAUI application moves between the
above four states and the events that are triggered to an application.
Figure 3-3 shows the possible states that a .NET MAUI application can take
during its lifetime and how it transitions between those states.  
  
![image](https://user-images.githubusercontent.com/26972859/231094374-72c17a79-ea47-424c-b2c3-5ddfd20471e9.png)
Figure 3-3. Application state lifecycle chart  
  
Before we dive into the details of each of the events that are fired
between the state transitions, I need to give you some background on
how they can be accessed and why. In order to access these events, you
must access the Window class. It certainly isn’t a common concept to have
a window in a mobile application, but you must appreciate that you are
dealing with a cross-platform framework and therefore an approach that
fits desktop as well as mobile. I see it as follows: a mobile application is a
single window application, and a desktop is likely to be multi-window.  
  
 # Lifecycle Events
  
 Now on to the events that move an application between states.
• Created: This event is raised after the platform window
has been created. Note that the window may not be
visible yet.
• Activated: This event is raised when the window is the
focused window.
• Deactivated: This event is raised when the window is
no longer the focused window. Note that the window
may still be visible.
• Stopped: This event is raised when the window is no
longer visible. The application may resume from this
state but it is not guaranteed, so it is recommended that
you cancel any long-running processes or anything
that may consume resources on the device. Mobile
operating systems are much stricter on what can
happen in the background.
• Resumed: This event is raised when an application
resumes from the Stopped state. It is recommended
to prepare your application for full use again (e.g.,
subscribe to events or messages, refresh any visible
content).
• Destroying: This event is raised when the platform
window is being destroyed and removed from memory.
It is recommended that you unsubscribe from events or
messages. 
  
 # Handling Lifecycle Events 
  
 By default, a .NET MAUI application won’t give you access to the lifecycle
events; this is something you must opt in for. In order to opt in, you must
modify your App class.
Open Visual Studio. You need to add a new class to your project and
call it StateAwareWindow. Your new class will need to be modified so it
looks as follows: 
  
```Csharp
public class StateAwareWindow: Window
{
 public StateAwareWindow() : base()
 {
 }
 public StateAwareWindow(Page page) : base(page)
 {
 }
 protected override void OnCreated()
 {
 // Initialise our application
 }
}
```
Inside of your application, you can override all methods that will
be executed when the specific event occurs. Each override method
follows the naming of the events, as described previously, with a prefix
of On. Therefore, to handle the Activated event, you override the
OnActivated method.
The final step is to make use of the new class, so inside your App.xaml.
cs file, add the following:
  
```Csharp
protected override Window CreateWindow(IActivationState
activationState)
{
 return new StateAwareWindow(MainPage);
}
```
This will create a new instance of StateAwareWindow and pass it a
reference to the application’s MainPage. If you do not pass in a reference
to a Page to the Window implementation, you will experience exceptions
being thrown.  
  
# Cross-Platform Mappings to Platform Lifecycle Events  
  
I strongly believe that despite the fact that .NET MAUI provides us with
these events, you should understand how they map to the underlying
platforms. If you understand what is being called on the platform-specific
side, it can really help to diagnose things when they go wrong or perhaps
point you in the direction of a better approach for your scenarios.
Let’s break down how the .NET MAUI lifecycle events map to the
platform-specific events and then show off the bits that are not mapped if
you ever need to use them. See Table 3-1.  
  
 **Table 3-1**. Cross-Platform Lifecycle Events Mapped to the PlatformSpecific Events
  
  |Event|Android|iOS/MacCatalyst|Windows|
  |:---|:----|:---|:---|
  |Created|OnPostCreate|FinishedLaunching|Created|
  |Activated|OnResume|OnActivated|Activated(Code Activated and Pointer Activated)|
  |Deactivated|OnPause|OnResignActivation|Activated|(Deactivated)|
  |Stopped|OnStop|DidEnterBackground|Visibility Changed|
  |Resumed|OnRestart|WillEnterForeground|Resumed|
  |Destroying|OnDestroy|WillTerminate|Closed|
  
  This list may not provide too much meaning right now and I wouldn’t
worry yourself with needing to know this. The aim here is to provide you
with a quick look-up to be able to then research if any lifecycle events are
going wrong or possibly not the right fit for your solution.
  
## Platform-Specific Lifecycle Events

There are actually many platform-specific lifecycle events that .NET MAUI
does not map to. What .NET MAUI does provide is a set of lifecycle events
that map consistently across all platforms. The rest in this section are really
specific to each individual platform. I won’t be covering all of the details of
each individual event; however, I will cover how to make use of one so that
you will know how to make use of an event that better suits your use case.
---
When searching for information around a platform-specific
event, don’t feel constrained to searching for .NET MAUI-specific
documentation. You have the power to leverage the platform APIs. You
should be able to search for information in the context of Android or
iOS and the code should be relatively easy to translate into C#.
---
In order to register for a platform-specific event, you need to make use
of the ConfigureLifecycleEvents method on the MauiAppBuilder class.
Let’s look at a concrete example for each platform. The code in each of the
following examples is largely the same but the duplication has been kept to
show the bigger picture. I have highlighted the differences in bold to show
the key differences.
  
# Android  
  
To receive a notification for an Android lifecycle event, you call the
ConfigureLifecycleEvents method on the MauiAppBuilder object. You
can then make use of the AddAndroid method and specify the events you
wish to handle and how you wish to handle them.  
  
```Csharp
using Microsoft.Maui.LifecycleEvents;
namespace WidgetBoard;
public static class MauiProgram
{
 public static MauiApp CreateMauiApp()
 {
 var builder = MauiApp.CreateBuilder();
 builder
 .UseMauiApp<App>()
 .ConfigureLifecycleEvents(events =>
  {
#if ANDROID
 events.AddAndroid(lifecycle=>
 lifecycle.OnStart((activity) =>
OnStart(activity)));
 static void OnStart(Activity activity)
 {
 // Perform your OnStart logic
 }
#endif
 });
 return builder.Build();
 }
}
   
```
For more information on the available lifecycle events, I recommend
checking out the following documentation pages:
Microsoft: https://learn.microsoft.com/dotnet/maui/
fundamentals/app-lifecycle#android
Android: https://developer.android.com/guide/components/
activities/activity-lifecycle
  
 # iOS and MacCatalyst 
  
 To receive a notification for an iOS lifecycle event, you call the
ConfigureLifecycleEvents method on the MauiAppBuilder object. You
can then make use of the AddiOS method and specify the events you wish
to handle and how you wish to handle them.
  
using Microsoft.Maui.LifecycleEvents;
namespace WidgetBoard;
public static class MauiProgram
  
{
 public static MauiApp CreateMauiApp()
 {
 var builder = MauiApp.CreateBuilder();
 builder
 .UseMauiApp<App>()
 .ConfigureLifecycleEvents(events =>
 {
#if IOS || MACCATALYST
 events.AddiOS(lifecycle =>
 lifecycle.OnActivated((app) =>
OnActivated(app)));
 static void OnActivated(UIKit.UIApplication
application)
 {
 // Perform your OnActivated logic
 }
#endif
 });
 return builder.Build();
 }
}  
  
For more information on the available lifecycle events, I recommend
checking out the following documentation pages:
Microsoft: https://learn.microsoft.com/dotnet/maui/
fundamentals/app-lifecycle#ios
iOS: https://developer.apple.com/documentation/uikit/app_and_
environment/managing_your_app_s_life_cycle?language=objc  
  
# Windows  
  
To receive a notification for a Windows lifecycle event, you call the
ConfigureLifecycleEvents method on the MauiAppBuilder object. You
can then make use of the AddWindows method and specify the events you
wish to handle and how you wish to handle them.
  
```Csharp
using Microsoft.Maui.LifecycleEvents;
namespace WidgetBoard;
public static class MauiProgram
{
 public static MauiApp CreateMauiApp()
 {
 var builder = MauiApp.CreateBuilder();
 builder
 .UseMauiApp<App>()
 .ConfigureLifecycleEvents(events =>
 {
#if WINDOWS
 events.AddWindows(lifecycle =>
 lifecycle.OnActivated((window, args) =>
OnActivated(window, args)));
 static void OnActivated(Microsoft.
UI.Xaml.Window window, Microsoft.UI.Xaml.
WindowActivatedEventArgs args)
 {
 // Perform your OnActivated logic
 }
#endif
 });
  
  return builder.Build();
 }
}
  
```
  
For more information on the available lifecycle events, I recommend
checking out the following documentation page:
Microsoft: https://learn.microsoft.com/dotnet/maui/
fundamentals/app-lifecycle#windows  
  
---
You may have noticed the usage of #if statements. Due to the
nature of compiling for multiple platforms in a single project, you
will need to write platform-specific code. If, like me, you do not like
the #if statement or at would like to keep its usage to a minimum,
then fear not: we will be taking a closer look at minimizing it in
Chapter 13.
---
  
# Summary  
  
In this chapter, you have
• Walked through the main components of a .NET MAUI
application
• Earned a tea break
• Learned about the start-up process
• Learned about the life of a .NET MAUI application
  
In the next chapter, you will
• Learn about the different possibilities you have to
architect your applications
• Decide on what architecture to use
• Walk through a concrete example by creating your
ClockWidget
• Learn how to further optimize your implementation
using NuGet packages  
  
  
  
  
  
  
  
  
