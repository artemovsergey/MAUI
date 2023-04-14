# Building Our First .NET MAUI App

In this chapter, we will create a new .NET MAUI project and make the necessary changes so that we
can use it in the subsequent development. The app that we will develop is a password manager app.
We will add features to it gradually in the coming chapters. When we complete Part 1, we will have
a functional password manager app. In this chapter, we will create the app using the Visual Studio
template, and initialize the resources of the application. After that, we will build and test it on supported
platforms. To use Shell in our app, we will create a new Xamarin.Forms project with Shell support
and then migrate it to our .NET MAUI project.
The following topics will be covered in this chapter:

* Setting up a new .NET MAUI project

* App startup and lifecycle management

* Configuring resources

* Creating a new Xamarin.Forms project with Shell

* Migrating this Xamarin.Forms project to .NET MAUI

# Technical requirements

To test and debug the source code in this chapter, you need Visual Studio 2022 installed on both
Windows and macOS. Please refer to the Development environment setup section in Chapter 1, Getting
Started with .NET MAUI, for the details.

# Managing the source code in this book

Since we develop a password manager app incrementally in this book, the source code of each chapter
is built on top of the previous chapters. To keep working on continuous improvement, we will have separate branches to keep the source code of each chapter. If you want to clone the source code of
all chapters in one command, you can clone from the main branch. In the main branch, we have all
chapters in separate folders. If you don’t want to use Git, you can also download the source code as a
compressed file from the release area, as shown in the following diagram (Figure 2.1):

![image](https://user-images.githubusercontent.com/26972859/231735500-c2c81f97-1278-4c04-90fc-012f3e9d3b29.png)
Figure 2.1: Source code in GitHub

Since new .NET MAUI releases may be available from time to time, the Git tags and versions in the
release area will be updated according to the new .NET MAUI releases and bug fixes.

The source code of this book can be found in the following GitHub repository:
https://github.com/PacktPublishing/Modern-Cross-Platform-ApplicationDevelopment-with-.NET-MAUI

There are three ways to download the source code in this book:

* Download the source code in a compressed file.

The source code can be downloaded in the release area, or use the following URL:
https://github.com/PacktPublishing/.NET-MAUI-Cross-PlatformApplication-Development/releases/tag/V1.0.0
The release tag may be changed when a new release is available.

* Clone the source code of one chapter.
To check out the source code of a chapter, you can use the following command as an example:
```$ git clone https://github.com/PacktPublishing/.NET-MAUICross-Platform-Application-Development.git -b chapter02```

* Clone the source code from the main branch.
To check out the source code of all chapters from the main branch, you can use the
following command:

```$ git clone https://github.com/PacktPublishing/.NET-MAUICross-Platform-Application-Development.git```

# Setting up a new .NET MAUI project

We can create a new .NET MAUI project using Visual Studio or the command line.

# Creating a new project using Visual Studio

To create a new .NET MAUI project, follow these steps:

1. Launch Visual Studio 2022 and select Create a new project on the startup screen. This will
open the Create a new project wizard.
In the top-middle section of the screen, there is a search box. We can type Maui in the search
box, and .NET MAUI-related project templates will be shown (see Figure 2.2):

![image](https://user-images.githubusercontent.com/26972859/231735973-12c9a704-3dd1-451e-9987-5de35931ec9d.png)
Figure 2.2: New project setup – Create a new project

There are three templates for the .NET MAUI app or library:
* .NET MAUI App – This is for a XAML-based .NET MAUI app.
* .NET MAUI Blazor App – This template can be used to create a .NET MAUI Blazor app.
* .NET MAUI Class Library – This is the option to build a .NET MAUI class library. We can
build shared components as a .NET MAUI class library when we develop a .NET MAUI app.

2. Let’s select .NET MAUI App and click the Next button; it goes to the next step to configure
your new project, as shown in Figure 2.3:

![image](https://user-images.githubusercontent.com/26972859/231736227-91d9f7c2-2cdb-4391-837d-423dbec12fa5.png)
Figure 2.3: New project setup – Configure your new project

3. Enter the project name and solution name as PassXYZ.Vault and click the Next button. After
the project is created, the project structure will look like Figure 2.4, displaying the following:
* Common files – In a new project, there are three files included in the template – App.xaml,
MainPage.xaml, and MauiProgram.cs. This is the group of files that we will work
on throughout the book. They are platform agnostic. Both business logic and UI can be
developed here and shared on all platforms.
* Platform-specific files – There are five subfolders (Android, iOS, MacCatalyst, Windows,
and Tizen) in the Platforms folder. Since we won’t support Tizen, we can remove it
from our project.

- Resources – A variety of resources ranging from images, fonts, splash screens, styles, and raw
assets are in the Resources folder. These resources can be used in all supported platforms.

![image](https://user-images.githubusercontent.com/26972859/231736451-eba6531b-f282-43db-9ca5-fce5ed025a9e.png)
Figure 2.4: .NET MAUI project structure

In the .NET MAUI project, there is only one project structure. Later, we will see that the development
of Xamarin.Forms involves multiple projects.

# Creating a new project using the dotnet command

Even though we installed .NET MAUI as part of the Visual Studio installation, .NET MAUI can be
installed separately using the command line as well. In this way, we can choose other development
tools than Visual Studio. We can create and build a .NET MAUI app using the dotnet command
line. To find out about the installed project templates, we can use the following command:

```C:\ > dotnet new --list```

To create a new project using the command line, we can execute the following command:

```C:\ > dotnet new maui -n "PassXYZ.Vault"```

The new .NET MAUI project has been created, and we can build and test it now. Before we move to
that, let’s spend some time having a look at the .NET MAUI app startup code and lifecycle

# App startup and lifecycle

In the .NET MAUI project, the app startup and lifecycle management are handled in the following
two files:

-  MauiProgram.cs
-  App.xaml/App.xaml.cs

For the app startup and configuration, .NET Generic Host is used. When the application starts, a
.NET Generic Host object is created to encapsulate an app’s resources and lifetime functionality, such
as the following:

- Dependency injection (DI)

- Logging

- Configuration

- App shutdown

This enables apps to be initialized in a single location and provides the ability to configure fonts,
services, and third-party libraries.

---
NET Generic Host
If you are a Xamarin developer, you may not be familiar with .NET Generic Host. In ASP.NET
Core, .NET Generic Host is used to encapsulate the resources in an app. In .NET MAUI, the
same pattern is borrowed and used for startup and configuration management.
---

Let’s examine the app startup code in Listing 2.1 (MauiProgram.cs):

**Listing 2.1: MauiProgram.cs (https://epa.ms/MauiProgram2-1)**

```Csharp
namespace PassXYZ.Vault;
public static class MauiProgram
{
 public static MauiApp CreateMauiApp() ➊
 {
 var builder = MauiApp.CreateBuilder(); ➋
 builder
 .UseMauiApp<App>() ❹
 .ConfigureFonts(fonts =>
 {
 fonts.AddFont("OpenSans-Regular.ttf",
 "OpenSansRegular");
 });
 return builder.Build(); ➌
 }
}
```
We can see the following in Listing 2.1:

➊ In each platform, the entry point is in platform-specific code. The entry point calls the
CreateMauiApp function, which is a method of the MauiProgram static class.

➋ Inside CreateMauiApp, it calls the CreateBuilder function, which is a method of the
MauiApp static class, and returns a MauiAppBuilder instance, which provides a .NET Generic
Host interface.

➌ The return value of CreateMauiApp is a MauiApp instance, which is the entry point of your app.

❹ The App class referenced in the UseMauiApp method is the root object of our application. Let’s
review the definition of the App class in Listing 2.2:

**Listing 2.2: App.xaml.cs (https://epa.ms/App2-2)**

```Csharp
namespace PassXYZ.Vault;
public partial class App : Application ①
{
public App()
{
 InitializeComponent();
 MainPage = new AppShell(); ②
}
}
```
In Listing 2.2, ① the App class is derived from the Application class, and the Application
class is defined in the Microsoft.Maui.Controls namespace.
② AppShell is an instance of Shell, and it defines the UI of the initial page of the app. Application
creates an instance of the Window class within which the application will run and views will be
displayed. In the App class, we can overwrite the CreateWindow method to manage the lifecycle,
which we will see soon.

# Lifecycle management

In the .NET MAUI app, there are the following four lifecycle states:

* Running

* Not running

* Deactivated

* Stopped

During the state transitions, the predefined lifecycle events will be triggered. There are six crossplatform lifecycle events defined, as we can see in Table 2.1:

|Event|Description|State transition|Override method|
|:----|:----------|:---------------|:--------------|
|Created|This event is raised after the native window has been created.|Not running -> Running|OnCreated|
|Activated|This event is raised when the window has been activated and is, or will become, the focused window.|Not running -> |Running|OnActivaded|
|Deactivated|This event is raised when the window is no longer the focused window. However, the window might still be visible.|Running -> Deactivated|OnDeactivated|
|Stopped|This event is raised when the window is no longer visible.|Deactivated -> Stopped|OnStopped|
|Resumed|This event is raised when an app resumes after being stopped.|Stopped -> Running|OnResumed|
|Destroying|This event is raised when the native window is being destroyed and deallocated.|Stopped -> Not running|OnDestroying|

Table 2.1: Lifecycle events and override methods

These lifecycle events are associated with the instance of the Window class created by Application.
For each event, there is a corresponding override method defined. We can either subscribe to the
lifecycle events or create override functions to handle lifecycle management.

## Subscribing to the Window lifecycle events

To subscribe to the lifecycle events, as we can see in Listing 2.3, at ➊ we can override the CreateWindow
method in the App class to create a Window instance on which we can subscribe to events:

**Listing 2.3: App.xaml.cs with lifecycle events (https://epa.ms/App2-3)**

```Csharp
using System.Diagnostics;
namespace PassXYZ.Vault;
public partial class App : Application {
 public App() {
 InitializeComponent();
 MainPage = new MainPage();
 }
 protected override Window CreateWindow(IActivationState
 activationState) ➊
 {
 Window window = base.CreateWindow(activationState);
 window.Created += (s, e) => {
 Debug.WriteLine("PassXYZ.Vault.App: 1. Created event");
 };
 window.Activated += (s, e) => {
 Debug.WriteLine("PassXYZ.Vault.App: 2. Activated event");
 };
 window.Deactivated += (s, e) => {
 Debug.WriteLine("PassXYZ.Vault.App: 3. Deactivated
 event");
 };
 window.Stopped += (s, e) => {
 Debug.WriteLine("PassXYZ.Vault.App: 4. Stopped event");
 };
 window.Resumed += (s, e) => {
 Debug.WriteLine("PassXYZ.Vault.App: 5. Resumed event");
 };
 window.Destroying += (s, e) => {
 Debug.WriteLine("PassXYZ.Vault.App: 6. Destroying
 event");
 };
 return window;
 }
}
```
In Listing 2.3, we revised the code of App.xaml.cs and we subscribed to all six events so that we
can run a test and observe the state from the Visual Studio Output window, as shown next. After we
launch our app, we can see that Created and Activated events are fired. Then, we minimize our app. We can see that Deactivated and Stopped events are fired. When we resume our app,
Resumed and Activated events are fired. Finally, we close our app, and a Destroying event
is fired:

```
PassXYZ.Vault.App: 1. Created event
PassXYZ.Vault.App: 2. Activated event
PassXYZ.Vault.App: 4. Stopped event
PassXYZ.Vault.App: 3. Deactivated event
PassXYZ.Vault.App: 5. Resumed event
PassXYZ.Vault.App: 2. Activated event
PassXYZ.Vault.App: 5. Resumed event
PassXYZ.Vault.App: 2. Activated event
The thread 0x6f94 has exited with code 0 (0x0).
PassXYZ.Vault.App: 6. Destroying event
The program '[30628] PassXYZ.Vault.exe' has exited with code 0
(0x0).

```
# Consuming the lifecycle override methods

Alternatively, we can consume the lifecycle override methods. We can create our own derived class
from the Window class:
1. In Visual Studio, right-click on the project node and select Add and then New Item….
2. In the Add New Item window, select C# Class from the template and name it PxWindow. We
created a new class, as shown next in Listing 2.4:

**Listing 2.4: PxWindow.cs (https://epa.ms/PxWindow2-4)**

```Csharp
using System.Diagnostics;
namespace PassXYZ.Vault;
public class PxWindow : Window
{
 public PxWindow() : base() {}
 public PxWindow(Page page) : base(page) {}
 protected override void OnCreated() {
 Debug.WriteLine("PassXYZ.Vault.App: 1. OnCreated");
 }
 protected override void OnActivated() {
 Debug.WriteLine("PassXYZ.Vault.App: 2. OnActivated");
 }
 protected override void OnDeactivated() {
 Debug.WriteLine("PassXYZ.Vault.App: 3. OnDeactivated");
 }
 protected override void OnStopped() {
 Debug.WriteLine("PassXYZ.Vault.App: 4. OnStopped");
 }
 protected override void OnResumed() {
 Debug.WriteLine("PassXYZ.Vault.App: 5. OnResumed");
 }
 protected override void OnDestroying() {
 Debug.WriteLine("PassXYZ.Vault.App: 6. OnDestroying");
 }
}
```
In Listing 2.4, we created a new class, PxWindow. In this class, we define our lifecycle override
methods. We can use this new class in App.xaml.cs.
Next, let’s look at the modified version of App.xaml.cs (Listing 2.5):

** Listing 2.5 Modified App.xaml.cs with PxWindow (https://epa.ms/App2-5) **

```Csharp
namespace PassXYZ.Vault;
public partial class App : Application
{
public App()
{
 InitializeComponent();
}
protected override Window CreateWindow(IActivationState
 activationState) ➊
 {
 return new PxWindow(new MainPage());
 }
}
```
When we repeat the test steps, we can see the following output from Visual Studio Output windows.
The output looks very similar to the previous one. Basically, both approaches have the same effect on
lifecycle management:

```
PassXYZ.Vault.App: 1. OnCreated
PassXYZ.Vault.App: 2. OnActivated
PassXYZ.Vault.App: 4. OnStopped
PassXYZ.Vault.App: 3. OnDeactivated
PassXYZ.Vault.App: 5. OnResumed
PassXYZ.Vault.App: 2. OnActivated
PassXYZ.Vault.App: 5. OnResumed
PassXYZ.Vault.App: 2. OnActivated
PassXYZ.Vault.App: 6. OnDestroying
The program '[25996] PassXYZ.Vault.exe' has exited with code 0
(0x0).
```

We have learned about app lifecycle management in .NET MAUI through the Window class. We can
either subscribe to lifecycle events or override the overridable methods to manage the app lifecycle.
Table 2.1 shows the comparison of these two approaches.
If you were a Xamarin.Forms developer, you might know that there were lifecycle methods defined
in the Application class as well. In .NET MAUI, the following virtual methods are still available:

* OnStart – Called when the application starts

* OnSleep – Called each time the application goes to the background

* OnResume – Called when the application is resumed, after being sent to the background

To observe the behavior of these methods, we can override the following methods in our App class,
as shown in Listing 2.6:

**Listing 2.6: App.xaml.cs (https://epa.ms/App2-6)**

```Csharp
using System.Diagnostics;
namespace PassXYZ.Vault;
public partial class App : Application
{
public App()
{
InitializeComponent();
 MainPage = new MainPage();
}
protected override void OnStart() { ➊
 Debug.WriteLine("PassXYZ.Vault.App: OnStart");
}
protected override void OnSleep() { ➋
 Debug.WriteLine("PassXYZ.Vault.App: OnSleep");
}
protected override void OnResume() { ➌
 Debug.WriteLine("PassXYZ.Vault.App: OnResume");
}
}
```
When we test the preceding code on Windows, we can see the following debug message in the Visual
Studio Output window:

```
PassXYZ.Vault.App: OnStart
PassXYZ.Vault.App: OnSleep
The thread 0x6844 has exited with code 0 (0x0).
The thread 0x6828 has exited with code 0 (0x0).
The thread 0x683c has exited with code 0 (0x0).
PassXYZ.Vault.App: OnResume
```
As seen in Listing 2.6,
➊ When the app starts, we can see the OnStart method is invoked.
➋ When we minimize our app, we can see the OnSleep method is invoked.
➌ When we resume the app from the taskbar, the OnResume method is invoked.
We have learned about the lifecycle states of the .NET MAUI app. We also learned that we could
subscribe to the lifecycle events or use override methods to manage the lifecycle. Next, let’s explore
the configuration of resources during the app startup.

# Configuring the resources

Resource management is one of the major differences between .NET MAUI and Xamarin.
When it comes to cross-platform development, each platform has its own way of managing resources,
and it’s a daunting task for the development team to know and manage all those things. For example,
we must include images in multiple sizes to support different resolutions.
In Xamarin, most of the resources are managed separately in platform-specific projects. If we want
to add an image, we must add the image files with different sizes to all platform projects separately.
.NET MAUI provides an elegant solution to manage resources effectively. The design goal of one single
project for all supported platforms helps to manage resources in one place.
In .NET MAUI, resource files can be tagged into different categories using a build action based on
the role they play in the project, as we can see in Table 2.2:

|Resource Type|Build Action|Example|
|:------------|:-----------|:----- |
|Images|MauiImage|dotnet_bot.svg|
|Icons |MauiIcon|appicon.svg|
|Splash screen image|MauiSplashScreen|appiconfg.svg|
|Fonts|MauiFont|OpenSans-Regular.ttf|
|Style definition using external CSS|MauiCss||
|Raw assets|MauiAsset||
|XAML UI definition|MauiXaml||

Table 2.2: .NET MAUI resource types

After adding a resource file, the build action can be set in the Properties window in Visual Studio.
If we look at the project file, we can see the following ItemGroup. If we put resources according to
the convention of default folder setup, the resources will be treated as the respective category and the
build action will be set automatically:

```xml
<ItemGroup>
 <!-- App Icon -->
 <MauiIcon Include="Resources\AppIcon\appicon.svg"
 ForegroundFile="Resources\AppIcon\appiconfg.svg"
Color="#512BD4" />
 <!—Splash Screen 
 <MauiSplashScreen Include= "Resources\Splash\splash.svg" Color="#512BD4" BaseSize="128,128" />
 <!-- Images -->
 <MauiImage Include="Resources\Images\*" />
 <!-- Custom Fonts -->
 <MauiFont Include="Resources\Fonts\*" />
</ItemGroup>
```

# App icon

In our app setup, we have an SVG image file, appicon.svg, under the Resources\AppIcon
folder, with the build action set to MauiIcon. At build time, this file is used to generate the icon
images on the target platform for various purposes, such as on the device, or in the app store.
It is possible to move this SVG file together with other images to the Resources\Images folder.
In that case, we should use the following entry in the project file:

```xml
<MauiIcon Include="Resources\Images\appicon.png"
ForegroundFile="Resources\Images\appiconfg.svg" Color="#512BD4"
/>
```
However, in this case, the build action is inconsistent for the files under the same folder, so appicon.
svg resides in the Resources\AppIcon folder instead of Resources\Images in our project.

# Splash screen

This is like an app icon. We have an SVG image file, splash.svg, in the Resources/Splash
folder, with the build action set to MauiSplashScreen:

```xml
<MauiSplashScreen Include="Resources\Splash\splash.svg"
Color="#512BD4" />
```
App icons, splash screens, and other images are simple resources, and we configure them in the
project file directly.
Some frequently used resources, such as custom font and DI, may have to be configured in code, or
both code and project files. We will discuss custom fonts here, and leave DI in w, Dependency Injection
and Refining Design.

# Setting custom font icons

Custom fonts can be managed as part of resources. In a mobile app, the visual representation is generally
delivered through images. We use images in all kinds of navigation activities. In Android and iOS
development, we need to manage image resources for different screen resolutions. In both Xamarin.
Forms and .NET MAUI, we can use a custom font (icon font) instead of images for application icons.
In .NET MAUI, if controls can display text, these controls usually define properties that we can use
to configure font settings for the text. The following properties are configurable:

* FontAttributes, which is an enumeration with three members: None, Bold, and Italic.
The default value of this property is None.

* FontSize, which is the property of the font size, and the type is double.

* FontFamily, which is the property of the font family, and the type is string

# What is the advantage of using custom fonts for icons?

There are many advantages to using custom fonts as icons instead of images.
Font icons are vector icons instead of bitmap icons. Vector icons are scalable, meaning you don’t need
different images with different sizes and different resolutions based on the device. Icon font scaling
can be handled through the FontSize property. The font file size is also much smaller than the
images. A font file with hundreds of icons in it can be only a few KB in size.
Besides font size, the icon color can be changed with the TextColor property. With static images,
we are not able to change the icon color.
Finally, font files can be managed in the shared project, so we don’t have to manage fonts separately
on different platforms.

# Custom font setup

Custom font setup includes two parts – font files and configuration.

Custom font files can be added to the .NET MAUI or Xamarin.Forms shared project. The configuration
of custom fonts in .NET MAUI is different than Xamarin.Forms. In Xamarin.Forms, we must configure
it in AssemblyInfo.cs, while we can manage the configuration through .NET Generic Host in
.NET MAUI.

In Xamarin.Forms, the process for accomplishing this is as follows:

1. Add the font file to the Xamarin.Forms shared project as an embedded resource (build
action: EmbeddedResource).
2. Register the font file with the assembly in a file such as AssemblyInfo.cs, using the
ExportFont attribute. An optional alias can also be specified.

In .NET MAUI, the process is changed and simplified as follows:
1. Add the font files in the Resources->Fonts folder. The build action is set to MauiFont,
as we can see in Figure 2.5:

![image](https://user-images.githubusercontent.com/26972859/231742991-158f48fa-f515-4329-9c98-8168c4233825.png)
Figure 2.5: .NET MAUI Resources

Instead of registering the font file with the assembly, .NET MAUI initializes most of the resources
through .NET Generic Host in the startup code, as shown in the following Listing 2.7 at ➊.
Font files are added using the ConfigureFonts() method, which is an extension method
of the MauiAppBuilder class.

In our project, we use the Font Awesome icon library from the following open source project:
https://github.com/FortAwesome/Font-Awesome
The fa-brands-400.ttf, fa-regular-400.ttf, and fa-solid-900.ttf font files can
be downloaded from the preceding website.
Let’s review the source code Listing 2.7 and see how to add these fonts to the app configuration.

**Listing 2.7: MauiProgram.cs (https://epa.ms/MauiProgram2-7)**

```Csharp
namespace PassXYZ.Vault;
public static class MauiProgram
{
 public static MauiApp CreateMauiApp()
  {
 var builder = MauiApp.CreateBuilder();
 builder
 .UseMauiApp<App>()
 .ConfigureFonts(fonts => ➊
 {
 fonts.AddFont("fa-regular-400.ttf",
 "FontAwesomeRegular");
 fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
 fonts.AddFont("fa-brands-400.ttf",
 "FontAwesomeBrands");
 fonts.AddFont("OpenSans-Regular.ttf",
 "OpenSansRegular");
 });
 return builder.Build();
 }
}
```
In the above code, we can add fonts by invoking the ConfigureFonts ➊ method on the
MauiAppBuilder object. To pass arguments to ConfigureFonts, we call AddFont method
to add font to IFontCollection object.

# Displaying font icons

To display font icons in .NET MAUI applications, we can specify the font icon data in a
FontImageSource object. This class, which derives from the ImageSource class, has the
following properties, as shown in Table 2.3:

|Property name|Type|Description|
|:------------|:---|:----------|
|Glyph|string|Unicode character value, such as "&#xf007;"|
|Size|double|The size of the font in device-independent units|
|FontFamily|string|A string representing the font family, such as FontAwesomeRegular|
|Color|Color|Font icon color in Microsoft.Maui.Graphics.Color|

Table 2.3: Properties of FontImageSource

The following XAML example has a single font icon being displayed by an Image view:

```xml
<Image BackgroundColor="#D1D1D1">
 <Image.Source>
 <FontImageSource Glyph="&#xf007;"
 FontFamily="FontAwesomeRegular"
Size="32" />
 </Image.Source>
</Image>
```
If you are not familiar with the XAML syntax in the preceding example, don’t worry. We will learn
about it in the next chapter. In the preceding code, a User icon is displayed in an Image control,
which is from the FontAwesomeRegular font family that we just added in the configuration. The
Glyph of the User icon is \uf007 in hex format and this is the C# escaped format. To use it in
XML, we have to use the escaped format of XML, which is &#xf007;.

The equivalent C# code is as follows:

```Csharp
Image image = new Image {
 BackgroundColor = Color.FromHex("#D1D1D1")
};
image.Source = new FontImageSource {
 Glyph = "\uf007",
 FontFamily = "FontAwesomeRegular",
 Size = 32
};
```
In the preceding example, we refer to a font icon using Glyph in a hex number as a string. This is
not convenient to use practically. We can define font glyphs as C# string constants so that we can
refer to something more meaningful. There are many ways to do this. Here, we use the open source
tool IconFont2Code to generate string constants. IconFont2Code can be found at the following
URL in GitHub:

https://github.com/andreinitescu/IconFont2Code

We use Font Awesome in our project. On the website of IconFont2Code, we can upload our font
library from the Resources\Fonts folder, and IconFont2Code will generate the code for us, as
in the following example:

```Csharp
namespace PassXYZ.Vault.Resources.Styles;
static class FontAwesomeRegular
{
 public const string Heart = "\uf004";
 public const string Star = "\uf005";
 public const string Scan = "\uf006";
 public const string User = "\uf007";
 public const string Qrcode = "\uf008";
 public const string Fingerprint = "\uf009";
 public const string Clock = "\uf017";
 public const string ListAlt = "\uf022";
 public const string Flag = "\uf024";
 public const string Bookmark = "\uf02e";
 ...
public const string SmileBeam = "\uf5b8";
public const string Surprise = "\uf5c2";
public const string Tired = "\uf5c8";
}
```
We can save the generated C# files in the Resources\Styles folder. The preceding file can be
found here:

```Resources\Styles\FontAwesomeRegular.cs```

With the preceding FontAwesomeRegular static class, a font icon can be used just like the normal
text in a XAML file:

```xml
<Button
 Text="Click me"
 FontAttributes="Bold"
 Grid.Row="3"
 SemanticProperties.Hint="Counts the number of times you
 click"
 Clicked="OnCounterClicked"
 HorizontalOptions="Center">
 <Button.ImageSource>
 <FontImageSource FontFamily="FontAwesomeSolid"
 Glyph="{x:Static app:FontAwesomeSolid.
 PlusCircle}"
 Color="{DynamicResource SecondaryColor}"
 Size="16" />
 </Button.ImageSource>
</Button>
```
In the preceding code, we add a circle plus icon to the Button control in front of the text "Click
me". To refer to the icon name in the generated C# class, we added an app namespace, as defined here:

```xmlns:app="clr-namespace:PassXYZ.Vault.Resources.Styles"````

So far, we have created our project and configured the necessary resources that we need. It’s time to
build and test our app.

# Building and debugging

As we recall in Chapter 1, Getting Started with .NET MAUI, regarding the development environment
setup, we cannot build and test all targets using one platform. Please refer to Table 1.8 about the
available build targets on Windows and macOS platforms. To make it simple, we will build and test
Windows and Android on the Windows platform. For iOS and macOS builds, we will do it on the
macOS platform.
Once we are ready, we can build and debug our app.
Let’s build and test on the Windows platform first. We can choose a framework that we want to run
or debug, as shown in Figure 2.6:

![image](https://user-images.githubusercontent.com/26972859/231744837-e1768581-19d8-4221-9066-df85791f85e0.png)
Figure 2.6: Building and debugging

# Windows

We can run or debug a Windows build on a local machine by selecting net6.0-windows10.0.19041
as the framework. To do this, we must enable Developer Mode on Windows, if it is not enabled yet.
Please refer to Figure 2.7 to enable Developer Mode on Windows 10 or 11:
1. Open the Start menu.
2. Search for Developer settings and select it.
3. Turn on Developer Mode.
4. If you receive a warning message about Developer Mode, read it, and select Yes.

![image](https://user-images.githubusercontent.com/26972859/231745011-414b8ffb-6b51-4f64-9d10-2d6b44b25bbb.png)
Figure 2.7: Developer Mode

# Android

For the Android build, we can test it using an Android emulator or device. Before building or debugging,
we need to connect a device or set up an instance of an emulator. Please refer to the following Microsoft
documentation about how to set up a device or an emulator instance:
https://learn.microsoft.com/en-us/dotnet/maui/
We can run or debug from Visual Studio (Figure 2.6) by selecting net6.0-android as the framework.
Alternatively, we can also build and run from the command line using the following command:

```dotnet build -t:Run -f net6.0-android```

![image](https://user-images.githubusercontent.com/26972859/231745362-0caad7fe-5bdc-44d5-8d5c-72a88a600a70.png)
Figure 2.8: Running on Android and Windows

After we run the app on Android and Windows targets, we can see the preceding screen (Figure 2.8).

# iOS and macOS

We can build and test iOS and macOS targets on a Mac computer. The steps to build and test using
Microsoft Visual 2022 for Mac are similar to what we have done in Windows and Android. Let’s look
at how to do it using a command line.
To build and test the iOS target, we can use the following command in the project folder:

```dotnet build -t:Run -f net6.0-ios -p:_DeviceName=:v2:udid=02C556DA-64B8-440B-8F06-F8C56BB7CC22```

To select a target iOS emulator, we need to provide the device ID using the following parameter:

```-p:_DeviceName=:v2:udid=```

To find the device ID, we can launch Xcode on a Mac computer and go to Windows -> Devices and
Simulators, as shown in Figure 2.9:

![image](https://user-images.githubusercontent.com/26972859/231745717-5a67c7dc-7790-48c8-a874-167d0743c1ef.png)
Figure 2.9: Devices and simulators in Xcode

For the macOS target, we can use the following command to build and test:

```dotnet build -t:Run -f net6.0-maccatalyst```

The screenshot for iOS and macOS is shown in Figure 2.10 and we can see that the look and feel are
similar to Android and Windows.

![image](https://user-images.githubusercontent.com/26972859/231745910-85ddfa94-8518-46d8-a2bf-e3ae9de4b81b.png)

Figure 2.10: Running on iOS and macOS

The environment setup for Android, iOS, and macOS involves platform-specific details. Please refer
to the Microsoft documentation for detailed instructions.
Even though this app works well, you can see that the app we just built is a simple one with only one
window. To lay a better foundation for our subsequent development, we will use Shell as a navigation
framework. There is a good Shell-based template in Xamarin.Forms, and we can use that to create
the initial code for our app.

# Scaffolding a Model-View-ViewModel project

We can run the app that we just created successfully now. We are going to develop a password manager
app named PassXYZ.Vault in the rest of this book. Version 1.x.x of this app is implemented in Xamarin.
Forms, and you can find it in GitHub here:
https://github.com/passxyz/Vault
Version 1.x.x is built using Xamarin.Forms 5.0.0. We will recreate it using .NET MAUI and share the
experience in this book. The new release will be version 2.x.x, and the source code is located here:
https://github.com/passxyz/Vault2

Shell is supported by Microsoft.Maui.Controls.Shell and Xamarin.Forms.Shell in
.NET MAUI and Xamarin.Forms. It provides a common navigation user experience that can be used
on all platforms. We will explain more about Shell in Chapter 5, Introducing Shell and Navigation. We
will use .NET MAUI Shell as the UI framework and navigation method in this book.
The projects created from the Visual Studio template of both .NET MAUI and Xamarin.Forms use
Shell. However, the default .NET MAUI project template contains only the simplest form of Shell, as
we can see in Listing 2.8:

**Listing 2.8: AppShell.xaml (https://epa.ms/AppShell2-8)**

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
 x:Class="PassXYZ.Vault.AppShell"
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:local="clr-namespace:PassXYZ.Vault"
 Shell.FlyoutBehavior="Disabled">
 <ShellContent
 Title="Home"
 ContentTemplate="{DataTemplate local:MainPage}"
 Route="MainPage" />
</Shell>
```
In Listing 2.8, if you are not familiar with XAML, don’t worry. We will introduce XAML syntax in Chapter 3,
User Interface Design with XAML. We can see that MainPage is displayed in ShellContent. It is
the simplest UI without too much content in it. In our app, we will use the Model-View-ViewModel
(MVVM) pattern and UI based on Shell. The MVVM pattern is a commonly used UI design pattern
in .NET MAUI app development. We need to create the boilerplate code to include both the MVVM
pattern and the Shell navigation structure. We could do it from scratch. However, the Xamarin.Forms
template includes such boilerplate codes that I used in version 1.x.x of PassXYZ.Vault. We can create
the same project template for .NET MAUI. By doing this, we can also have an overview of how to
migrate or reuse existing code from Xamarin.Forms.

# Migrating and reusing a Shell template from Xamarin.Forms

Xamarin.Forms include a more complex Shell template that can be configured to generate boilerplate
code for flyout or tabbed Shell navigation. We can create a new Xamarin.Forms project using this
template. After that, we can use this boilerplate code in the .NET MAUI app that we just created.
To create a new Xamarin.Forms project, follow these steps:

1. Launch Visual Studio 2022 and select Create a new project. This opens the Create a new project
wizard. In the search box, we can type Xamarin, and all Xamarin-related project templates
will be shown (see Figure 2.11):

![image](https://user-images.githubusercontent.com/26972859/231746399-15710564-19f8-43cb-a71e-a138a477ac28.png)

Figure 2.11: New Xamarin project

2. Select Mobile App (Xamarin.Forms) from the list and click Next. On the next screen, as
shown in Figure 2.12, we should choose a different location, but use the same project name,
PassXYZ.Vault, and click the Create button:

![image](https://user-images.githubusercontent.com/26972859/231746490-037ab6d0-7ca5-4b29-8c54-4f52615e738e.png)

Figure 2.12: Configure the Xamarin project

3. We have one more step, as shown in Figure 2.13. Let’s select the Flyout template and click Create:

![image](https://user-images.githubusercontent.com/26972859/231746597-a93d9c2b-681f-40f2-a2e0-673b524468b2.png)

Figure 2.13: Configure the Xamarin project – Flyout

After the new solution is created, we can see that there are four projects in the solution, as shown in
Figure 2.14:

* PassXYZ.Vault – This is a .NET Standard project that is shared by other projects, and all
platform-independent code should be here

* PassXYZ.Vault.Android – This is the Android platform-specific project

* PassXYZ.Vault.iOS – This is the iOS platform-specific project

* PassXYZ.Vault.UWP – This is the UWP-specific project

We can see that the project structure of Xamarin.Forms is quite different from .NET MAUI. There are
multiple projects in the solution. All resources are managed separately in platform-specific projects.
Most of the development work should be done in the .NET Standard project, PassXYZ.Vault, and we
will migrate and reuse the code in this project.

![image](https://user-images.githubusercontent.com/26972859/231746771-11edb7c5-344a-4a75-9e2c-59e6f299144b.png)

Figure 2.14: Xamarin.Forms project structure

The source code of this Xamarin.Forms project can be found here:
https://github.com/shugaoye/PassXYZ.Vault2/tree/xamarin

If platform-specific code is not involved, the migration process is relatively simple. We are handling the
simplest case here. The production code is usually much more complicated than this so any migration
should be planned after a detailed analysis.

Let’s focus on the .NET Standard project. The content in the .NET Standard project includes the
boilerplate code of the MVVM pattern and Shell UI, which is what we need. We can copy files in the
previous list to the .NET MAUI project and change namespaces in the source code.

The following are the steps to be taken in the migration process:

1. Please refer to Table 2.4, which shows a list of actions corresponding to the list of files and
folders in the .NET Standard project:

|Xamarin.Forms|Actions|.NET MAUI|
|:------------|:------|:--------|
|App.xaml |No |Keep the .NET MAUI version. It defines the instance of the Application class.|
|AppShell.xaml|Replace|Overwrite the .NET MAUI version and change namespaces to .NET MAUI. This file defines the Shell navigation hierarchy.|
|Views/|Copy|New folder in .NET MAUI project. Need to change namespaces.|
|ViewModels/|Copy|New folder in .NET MAUI project. Need to change namespaces.|
|Services/|Copy|Interface to export models. New folder in .NET MAUI project. Need to change namespaces.|
|Models/|Copy|New folder in .NET MAUI project. Need to change namespaces.|

Table 2.4: Actions in the .NET standard project

2. In the .NET MAUI project, please refer to Table 2.4 to replace the following namespaces:

|Old namespace|New namespace|
|:------------|:------------|
|xmlns="http://xamarin.com/schemas/2014/forms"|xmlns="http://schemas.microsoft.com/dotnet/2021/maui"|
|using Xamarin.Forms|using Microsoft.Maui AND using Microsoft.Maui.Controls|
|using Xamarin.Forms.Xaml|using Microsoft.Maui.Controls.Xaml|

Table 2.5: Namespaces in .NET MAUI and Xamarin.Forms

3. Test and fix any errors.

In Figure 2.15, we can see that the list of files changed is views and view models:

![image](https://user-images.githubusercontent.com/26972859/231748161-e082af77-4069-4e26-a913-f86d60e149fa.png)

Figure 2.15: Changed files in migration (https://bit.ly/3NlfqvO)

For this simple case, all changes are related to the namespace. This is not true in real-world situations.
Even though the process looks simple, it is still relatively complicated for people who are new to .NET
MAUI. I don’t recommend you to repeat this. Instead, I created a new Visual Studio project template
to include the desired outcome.

Let’s build and test this updated app.

![image](https://user-images.githubusercontent.com/26972859/231748309-fe99055a-8290-4c83-84c9-953131a2be56.png)

Figure 2.16: PassXYZ.Vault with .NET MAUI Shell

In Figure 2.16, we can see that there are three pages included in the default Shell menu:

• About – This is an about page

• Browse – This is the entry point of a list of items

• Logout – This is the link to the login page where you can log in or log out

We will use this as the boilerplate code for further development in this book. To summarize the work
that we have done in this section, I created a Visual Studio project template for it. By using this project
template, we can generate the project structure we want.

# Visual Studio project template

The project template can be downloaded as a Visual Studio extension package from the Visual Studio
Marketplace, as shown in Figure 2.17:

![image](https://user-images.githubusercontent.com/26972859/231748648-354aba08-f03e-4562-8a8a-4a8ad6060c80.png)

Figure 2.17: Project template in Visual Studio Marketplace

After the installation of this project template, we can create a new .NET MAUI project, as shown in Figure 2.18:

![image](https://user-images.githubusercontent.com/26972859/231748829-4ffa1198-e32e-4f77-8b8d-e3d6810ab1cb.png)

Figure 2.18: Creating a new .NET MAUI project using the project template

In the project created using this template, the project structure is the same as the one in this chapter.
The source code of this project template can be found here:
https://github.com/passxyz/MauiTemplate

# Summary

We created a new .NET MAUI project in this chapter. We learned how to configure the .NET MAUI
app using .NET Generic Host, and we can use a custom font (Font Awesome) after updating the
configuration of the resources. We also learned about the .NET MAUI application lifecycle. We tested
how to subscribe to lifecycle events by overriding the CreateWindow method and by creating a
derived class of the Window class. To create the boilerplate code with the MVVM pattern and Shell
support, we created a new .NET MAUI project template. Throughout the process, we demonstrated
how to migrate Xamarin.Forms code to .NET MAUI.
In the next chapter, we will learn how to create a user interface using XAML. XAML can be used to
build user interfaces for WPF, UWP, Xamarin.Forms, and .NET MAUI. We will create and improve
the user interfaces of our password manager app using XAML.










