# Implementing .NET MAUI Blazor

In the second part of this book, we will learn how to build a .NET MAUI Blazor Hybrid app. Blazor
is a single-page app (SPA) framework that uses Razor components as building blocks. You might
have heard of other SPA frameworks, such as React, Angular, Vue, and so on. Most SPA frameworks
use JavaScript, but Blazor uses C# instead of JavaScript. Blazor can also be used to build the so-called
Blazor Hybrid application. In a Blazor Hybrid app, Razor components run natively on the device
using an embedded Web View control so that the Blazor Hybrid app can access device features in
the same way as a native app. We will re-build our app as a Blazor Hybrid app in Part 2. There are
four chapters in Part 2.

This section comprises the following chapters:

* Chapter 7, Introducing Blazor Hybrid App Development

* Chapter 8, Understanding the Blazor Layout and Routing

* Chapter 9, Implementing Blazor Components

* Chapter 10, Advanced Topics in Creating Razor Components

In .NET MAUI, there is another way to build the user interface, which is using Blazor. Blazor can be
used as a single-page application (SPA) framework to develop web applications. It can also be used
to develop .NET MAUI apps in the form of a Blazor Hybrid app. The building block of Blazor is the
Razor component. Using Blazor and Blazor Hybrid, we can reuse Razor components between native
apps and web apps. Compared to the XAML UI, the Blazor UI can achieve higher reusability, which
includes both native apps and web apps. In this chapter, we will introduce what Blazor is and explain
its usage in different scenarios. We will also introduce the Razor component and how to develop a
Blazor Hybrid app using Razor components.

We will cover the following topics in this chapter:

* What is Blazor?

* How to create a .NET MAUI Blazor project

* How to create a new Razor component

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC or Mac. Please refer to the Development environment setup section in Chapter 1, Getting
Started with .NET MAUI, for the details.
The source code for this chapter is available in the following GitHub repository: https://github.
com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development/
tree/main/Chapter07.
The source code can be downloaded using the following git command:

```
git clone -b chapter07 https://github.com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development PassXYZ.Vault2
```

# What is Blazor?

Blazor is a framework for building web applications using HTML, CSS, and C#. To build web
applications using Blazor in ASP.NET Core, there are two options to choose from, which are Blazor
Server and Blazor WebAssembly (Wasm). With .NET MAUI, Blazor can also be used to build native
applications, and this is the third form – a Blazor Hybrid app.
In web application development, the tasks usually include creating a frontend user interface and
backend service. Backend services are accessed through the RESTful API or remote procedure calls
(RPCs). The components of user interfaces consist of HTML, CSS, and JavaScript. They are loaded
in a browser and displayed as web pages. We can render components related to the user interaction
on the server in the ASP.NET Core architecture. This hosting model is Blazor Server. We can also
choose to execute most of the user interface components in the browser; this hosting model is called
Blazor Wasm. In some applications, we have requirements for the application to access device-specific
features, such as sensors or cameras. In that case, we usually need to develop native applications to
meet such requirements. With Blazor, we have another choice, which is the Blazor Hybrid app. First,
let’s talk about Blazor Server.

# Learning about Blazor Server

In traditional web application development, user interaction logic is performed on the server side.
In the MVC design pattern, processing user interaction is part of the application architecture. If
there is a user interaction in the browser, it is sent back to the server to be processed. In response to
the user’s request, the entire page may be reloaded. To improve the performance, Blazor Server uses
a design similar to an SPA framework. To respond to the user request, Blazor Server processes the
request, and it only sends the Document Object Model (DOM) changes related to the user action to
the browser. In Blazor Server, as we can see in Figure 7.1, the processing logic is the same as an SPA,
with the difference being that Razor components are rendered on the server instead of the browser.
SignalR, an open source library that can be used for real-time communication between the web client
and the server, is used as the connection between the server and the browser:

![image](https://user-images.githubusercontent.com/26972859/231839550-5e6ea496-5989-404d-ac65-058ad47db77f.png)

Figure 7.1: Blazor Server

```Csharp
Razor components versus Blazor components
People may get confused between Blazor and Razor. Razor was introduced as a template engine
of ASP.NET in 2010. Razor syntax is a markup syntax in which developers can embed C# code
into an HTML page. Blazor is an SPA framework that uses Razor syntax as the programming
language. It was introduced around 2018. Blazor is a component-based framework, and a
Blazor app consists of Razor components. In other words, Blazor is a hosting model for Razor
components. Blazor components and Razor components are widely used interchangeably, but
the correct terminology is Razor component.
```
A Razor component resides in a file with the .razor extension, and it is compiled as a .NET class at
runtime. This .razor file can also be split into two files with .razor and .razor.cs extensions.
The idea is quite similar to XAML and code-behind, which we learned about in Part 1 of this book.
We will learn how to build Razor components using Razor syntax in this chapter. Next, we will talk
about Blazor Wasm.

# Understanding Blazor Wasm

Blazor Wasm is a hosting model that renders Razor components in a web browser. Razor components
loaded in the browser are compiled in Wasm using the .NET runtime, as we can see in Figure 7.2:

![image](https://user-images.githubusercontent.com/26972859/231839982-485fe51d-2c94-41e1-ac59-6b31f29c122e.png)
 
Figure 7.2: Blazor Wasm

In the browser, the startup page will load the .NET environment and Razor components. Razor
components are compiled to Wasm using a .NET Intermediate Language (IL) interpreter at runtime
to handle DOM changes. This is so-called just-in-time (JIT) compilation. Using JIT, the compilation
happens at runtime, so the performance is slower than ahead-of-time (AOT) compilation. Using AOT,
the compilation is done at development time, so the runtime performance can be improved. Blazor Wasm
supports mixed-mode AOT compilation in .NET 7, which means part of .NET code can be compiled
in Wasm at the development stage to gain a significant runtime performance improvement.
Mixedmode AOT means that only part of the IL code can be compiled into Wasm due to technical reasons.

## Wasm

Wasm is a binary instruction format for a stack-based virtual machine. Wasm is supported by
most modern web browsers. With Wasm, we can use many programming languages to develop
client-side components.

As an SPA framework, we can compare Blazor to other JavaScript-based SPA frameworks. There are
many JavaScript SPA frameworks, such as React, Angular, and Vue. In Table 7.1, we have compared
Blazor with React. Of course, we can use other JavaScript frameworks for the comparison as well. The
reason that I chose React is that React Native can be used to develop Hybrid apps, which has some
similarities to .NET MAUI Blazor, which we will discuss in the next section:

![image](https://user-images.githubusercontent.com/26972859/231840337-9be5c1ac-a025-4297-adb6-0175ab5814c4.png)

Table 7.1: Comparison of Blazor and React

Both JavaScript and Wasm are built-in features of modern browsers. The SPA frameworks using either
JavaScript or Wasm do not have any extra dependencies to run in a browser.
Blazor Wasm supports JavaScript Interop, so JavaScript components can be used by Blazor as well.
Many JavaScript libraries have been ported to Blazor.
Both Blazor and React support PWA development, which makes SPAs accessible in offline mode.
The first-time loading performance of Blazor is slightly heavier than React due to the extra download
time of .NET runtimes.

# Exploring Blazor Hybrid

We can use Blazor as the user interface layer in desktop or mobile native frameworks, known as Blazor
Hybrid apps. In a Blazor Hybrid app, Razor components are rendered natively on the device using an
embedded WebView control. Wasm isn’t involved, so the app has the same capability as a native app.
As we can see in Figure 7.3, in a Hybrid app, we can use the BlazorWebView control to build and
run Razor components inside an embedded WebView. The BlazorWebView control is available in
.NET MAUI and Windows desktop environments. We can build Blazor Hybrid applications in .NET
MAUI, WPF, or Windows Forms:

![image](https://user-images.githubusercontent.com/26972859/231840499-5fc356a4-6a9f-416d-a52a-7d066c8eceef.png)

Figure 7.3: BlazorWebView

Blazor Server, Blazor Wasm, and Blazor Hybrid run in different runtime environments, so they have
different capabilities. We can create different project types using either the command line or Visual Studio.
To list the installed project templates, we can use the following command:

```
dotnet new --list
These templates matched your input:
Template Name Short Name Language
--------------------------- ------------------- ----------
Blazor Server App blazorserver [C#]
Blazor WebAssembly App blazorwasm [C#]
Razor Class Library razorclasslib [C#]
.NET MAUI App maui [C#]
.NET MAUI Blazor App maui-blazor [C#]
.NET MAUI Class Library mauilib [C#]
Class Library classlib [C#],F#,VB
```
In the preceding list, we filtered out irrelevant project types. To understand the different project types
better, we can review the summary depicted in Table 7.2:

![image](https://user-images.githubusercontent.com/26972859/231840760-8f376c8f-aafe-4a31-97ea-64bbfc4d4928.png)

Table 7.2: .NET MAUI and Blazor-related project types

There are seven project types listed in Table 7.2. They can be divided into two groups – Blazor app
and .NET MAUI app. Let’s take a look at them.

# Blazor apps

For both Blazor Server and Blazor Wasm, the target framework is net6.0, but they use different SDKs.
A Blazor Server app can take full advantage of the server’s capability with Microsoft.NET.Sdk.Web,
while Blazor Wasm can only access a limited set of .NET API using Microsoft.NET.Sdk.BlazorWasm.
To share Razor components between Blazor Server and Blazor Wasm, we can use a Razor Class Library,
which uses Microsoft.NET.Sdk.Razor. The standard .NET class library, which can be shared by all
.NET 6.0 apps, uses Microsoft.NET.Sdk.

# .NET MAUI apps

For a .NET MAUI App, we can build XAML-based .NET MAUI apps using Microsoft.NET.Sdk;
.NET MAUI Blazor apps use Microsoft.NET.Sdk.Razor. Both project types target the same set of
target frameworks.
To share components, the standard .NET class library can be used. If .NET MAUI features have to be
used in the shared components, the .NET MAUI class library can be used. For example, PassXYZLib
is a .NET MAUI class library. Both the .NET class library and the .NET MAUI class library use the
same Microsoft.NET.Sdk, but they target different frameworks.
The Razor components in the .NET MAUI Blazor app have full access to the native capabilities of the
device through the underlying platform SDK. The relationship between Blazor and Blazor Hybrid is
similar to React and React Native. In a .NET MAUI Blazor app and React Native, both may have to
implement some platform-specific code. We can use Table 7.3 to compare how to access platformspecific APIs in these two frameworks.

# Platform-specific implementation in .NET MAUI Blazor and React Native

In .NET MAUI, a full .NET implementation is available on the target operating system. For example,
we have all .NET 6.0 APIs and almost a full .NET version of the Android APIs in net6.0-android.
To implement a platform-specific feature in Android, we don’t have to use Android Studio and Java/
Kotlin. We can implement it in the .NET environment entirely. The recommended solution is to
combine multi-targeting with partial classes and partial methods to invoke platform code from crossplatform code. This approach can improve the reusability of code and limit the platform-specific code
to the minimum scope.

In React Native, native modules are used to implement platform-specific code. To implement native
modules, platform-specific tools have to be used. For example, Android Studio is used to implement
Android native modules and Xcode is used for iOS ones:

|Platforms|NET MAUI Blazor|React Native|
|:--------|:--------------|:-----------|
|Android|net6.0-android31.0/C#|Android Studio/Java or Kotlin|
|iOS|net6.0-ios15.2/C#|Xcode/Object-C or Swift|
|macOS|net6.0-maccatalyst15.2/C#|N/A|
|Windows|net6.0-windows10.0.19041/C#|N/A|

Table 7.3: Comparison between .NET MAUI Blazor and React Native

There are pros and cons to both approaches. In the .NET MAUI implementation, both .NET- and
platform-specific APIs are provided as .NET APIs in C#. We can maximize the reuse of the code to
expose platform-specific implementations as a cross-platform API. However, the effort to maintain
the target frameworks is huge.
In React Native, there is no standard platform-specific implementation available. The developers need
to develop them on their own when they need to access platform-specific features. It looks flexible,
but it is difficult to reuse code in the framework. Different developers may invent different libraries
to access the same set of platform features. Native module developers must acquire different skills to
develop cross-platform apps.
If we create a cross-platform solution using Blazor, it is possible to develop a solution that includes
web, mobile, desktop, and backend services using one architecture and programming language. The
shared components can be reused in all target applications.
The paradigm of React Native is to learn once, write anywhere. React and React Native both use the
JavaScript language and similar frameworks, but they cannot reuse components as we can do in Blazor.
React Native uses a native user interface, which is similar to the XAML-based .NET MAUI app, while
Blazor Hybrid apps use HTML-based web user interfaces.
To summarize, Blazor Hybrid and React Native have quite different design goals, so their features and
capabilities are different. You must understand your requirements first before choosing a framework.

# Creating a new .NET MAUI Blazor project

To learn how to develop a Blazor Hybrid app, we must upgrade our PassXYZ.Vault project to
support the Blazor-based UI. We don’t have to do this from scratch – we can convert our current
project so that it supports the Blazor UI. In this way, we can build both a XAML-based app and a
Hybrid app using the same project. Before we add the Blazor UI to our app, we can create a new .NET
MAUI Blazor project with the same app name first so that we can refer to this new project to convert
our project into a .NET MAUI Blazor project. We can create a new .NET MAUI Blazor project from the command line or Visual Studio. We will
demonstrate both in this section.

# Generating a .NET MAUI Blazor project with the dotnet command line

Let’s create a new project using the .NET command line first. This can be done on either Windows
or macOS.

We can create a new project using the shortname maui-blazor listed in Table 7.2:

```
dotnet new maui-blazor -o PassXYZ.Vault
Welcome to .NET 6.0!
---------------------
SDK Version: 6.0.402
Telemetry
---------
The .NET tools collect usage data in order to help us improve
your experience. It is collected by Microsoft and shared with
the community. You can opt-out of telemetry by setting the
DOTNET_CLI_TELEMETRY_OPTOUT environment variable to '1' or
'true' using your favorite shell.
Read more about .NET CLI Tools telemetry: https://aka.ms/
dotnet-cli-telemetry
----------------
Installed an ASP.NET Core HTTPS development certificate.
To trust the certificate run 'dotnet dev-certs https --trust'
(Windows and macOS only).
Learn about HTTPS: https://aka.ms/dotnet-https
----------------
Write your first app: https://aka.ms/dotnet-hello-world
Find out what's new: https://aka.ms/dotnet-whats-new
Explore documentation: https://aka.ms/dotnet-docs
Report issues and find source on GitHub: https://github.com/
dotnet/core
Use 'dotnet --help' to see available commands or visit:
https://aka.ms/dotnet-cli
--------------------------------------------------------------
------
The template ".NET MAUI Blazor App" was created successfully.
```

In the preceding command, we specified the short name of maui-blazor to choose the project
template and we used PassXYZ.Vault as the project name. Once we have created the project, we
can build and run it:

```
C:\ > dotnet build -t:Run -f net6.0-android
MSBuild version 17.3.2+561848881 for .NET
 Determining projects to restore...
 All projects are up-to-date for restore.
 PassXYZ.Vault -> C:\PassXYZ.Vault\Hybrid\bin\Debug\net6.0-
android\PassXYZ.Vault.dll
Build succeeded.
 0 Warning(s)
 0 Error(s)
Time Elapsed 00:01:43.79
```

If you are using a later version of Visual Studio, you may specify a different framework, such
as net7.0-android.
In the build command, we specify net6.0-android as the target framework to test our new
app. We can refer to Figure 7.5 to see a screenshot of this new app and project structure. With that, we
have created the new project using a command line. Now, let’s learn how to do the same using Visual
Studio on Windows. If you are using Visual Studio on a Mac, the process is very similar.

# Creating a .NET MAUI Blazor project using Visual Studio on Windows

To create a.NET MAUI Blazor project using Visual Studio, we can start Visual Studio and select Create
a new project, and then type MAUI in the search box. As we can see in Figure 7.4, we can select .NET
MAUI Blazor App from the list of project templates:

![image](https://user-images.githubusercontent.com/26972859/231842167-e2159519-8792-4b49-b073-c4794ffb90c8.png)

Figure 7.4: Creating a new .NET MAUI Blazor project

After following the wizard to create the project, we can select net6.0-android as the target
framework to build and run it. To save space, we will use the Android platform as an example here;
you can select other target frameworks to try and test if you like.

# Running the new project

To run the project, we can press F5 or Ctrl + F5 in Visual Studio, or we can execute the dotnet
command from the command line. We can see screenshots of this and the project structure in Figure 7.5:

![image](https://user-images.githubusercontent.com/26972859/231842522-e761f082-0f02-49ce-82a2-a2fef1e896d6.png)

Figure 7.5: Screenshots and project structure

The user interface of the app created from the template looks like an SPA with a navigation menu at
the top on Android. If we run it on Windows with a bigger screen, the navigation menu will be shown
side by side on the left of the screen. The project structure is similar to a standard .NET MAUI app
with the following differences:

* wwwroot/: This folder is the root of static files for web pages

* Pages/: This folder contains Razor pages in the app

* Shared/: This folder contains Razor components that can be shared

* Main.razor: This is the main page of the Blazor app

* _Imports.razor: This is a helper to import Razor components at the folder or project level

To understand the difference between the .NET MAUI app and the .NET MAUI Blazor app, we must
review the startup code.

# The startup code of the .NET MAUI Blazor app

All .NET MAUI apps include a file called MauiProgram.cs for their startup and configuration.
Let’s review the startup code of the .NET MAUI Blazor app:

```Csharp
namespace PassXYZ.Vault;
public static class MauiProgram {
 public static MauiApp CreateMauiApp() {
 var builder = MauiApp.CreateBuilder();
 builder.UseMauiApp<App>()
 .ConfigureFonts(fonts => {
 fonts.AddFont("OpenSans-Regular.ttf",
 "OpenSansRegular");
 });
 builder.Services.AddMauiBlazorWebView(); ❶
#if DEBUG
 builder.Services.AddBlazorWebViewDeveloperTools(); ❷
#endif
 builder.Services.AddSingleton<WeatherForecastService>();
 return builder.Build();
 }
}
```

In the .NET MAUI Blazor version, we can see that the following Blazor configurations are added:
❶ BlazorWebView is added by calling AddMauiBlazorWebView()
❷ Developer tools are added by calling AddBlazorWebViewDeveloperTools() for debugging
The rest of the startup process is the same as that for a XAML-based .NET MAUI app. In App.xaml.
cs, the inherited MainPage property of the App class is set to MainPage.xaml:

```Csharp
namespace PassXYZ.Vault;
public partial class App : Application {
 public App() {
 InitializeComponent();
 MainPage = new MainPage();
 }
}
```

The major difference between XAML-based apps and Blazor Hybrid apps is in MainPage.xaml.
Let’s review MainPage.xaml:

```Csharp
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com
 /dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:local="clr-namespace:PassXYZ.Vault"
 x:Class="PassXYZ.Vault.MainPage"
 BackgroundColor="{DynamicResource PageBackgroundColor}">
 <BlazorWebView HostPage="wwwroot/index.html"> ①
 <BlazorWebView.RootComponents> ②
 <RootComponent Selector③="#app" ComponentType④=
 "{x:Type local:Main}" />
 </BlazorWebView.RootComponents>
 </BlazorWebView>
</ContentPage>
```
There is only one UI element, called BlazorWebView, defined in MainPage.xaml. In
BlazorWebView, we can use the HostPage property and the RootComponent nested component
to customize BlazorWebView.
We can treat BlazorWebView as a browser. The user interface of a browser is loaded from an HTML
file. The HostPage property ① is used to specify the static HTML page to be loaded in the web view
control. In our case, it is wwwroot/index.html; we will review it in Listing 7.1.
In this static HTML file, we need to specify where the Razor component should be placed, and which
Razor component should be the root component. We can specify both using the RootComponent
nested component ②.
In the previous chapter, we learned that a tag of XAML maps to a C# class eventually. Here, both
BlazorWebView and RootComponent are C# classes as well.
In RootComponent, we use the Selector property ③ to define a CSS selector that specifies
where the root Razor component in our app should be placed. In our case, it is the #app CSS selector
defined in index.html. The ComponentType property ④ defines the type of the root component.
In our case, it is Main.

Finally, let’s review the HTML file (index.html) that we mentioned previously:

**Listing 7.1: index.html (https://epa.ms/index7-1)**

```xml
<!DOCTYPE html>
<html lang="en">
<head>
 <meta charset="utf-8" />
 <meta name="viewport" content="width=device-width,
 initial-scale=1.0, maximum-scale=1.0, user-
 scalable=no, viewport-fit=cover" />
 <title>PassXYZ.Vault</title>
 <base href="/" />
 <link rel="stylesheet" href="css/bootstrap/
 bootstrap.min.css" /> ❶
 <link href="css/app.css" rel="stylesheet" />
 <link href="PassXYZ.Vault.styles.css" rel="stylesheet" />
</head>
<body>
 <div class="status-bar-safe-area"></div>
 <div id="app">Loading...</div> ❷
 <div id="blazor-error-ui">
 An unhandled error has occurred.
 <a href="" class="reload">Reload</a>
 <a class="dismiss">✕</a>
 </div>
 <script src="_framework/blazor.webview.js"
 autostart="false"> ❸
 </script>
</body>
</html>
```

We can see that index.html is a simple HTML file:
❶ It uses the CSS stylesheet from the Bootstrap framework.
❷ The id selector is defined as "app", which we pass to the Selector attribute of RootComponent
in MainPage.xaml.
❸ A JavaScript file called blazor.webview.js is loaded at the end of index.html. It initializes
the runtime environment of BlazorWebView.
With that, we have an overview of the .NET MAUI Blazor app. In the next section, we will replace
the XAML-based user interface with a Blazor user interface.

# Migrating to a .NET MAUI Blazor app

Instead of doing everything from scratch, we can use both the XAML and Blazor UIs in our app by
changing the project configuration referring to the project that we created in the previous section.
We will use a mixed XAML and Blazor user interface in one app for a while until we move everything
to Blazor.
To convert our app into a .NET MAUI Blazor app, we need to make the following changes:
1. Change the SDK from Microsoft.NET.Sdk to Microsoft.NET.Sdk.Razor in the
project file since the .NET MAUI Blazor app uses a different SDK.
In the PassXYZ.Vault.csproj project file, we have the following line:
```<Project Sdk="Microsoft.NET.Sdk">```
We need to replace it with the following line:
```<Project Sdk="Microsoft.NET.Sdk.Razor">```

  2. Copy the following folders from the new project that we just created into our app:

  * wwwroot
  
  * Shared

  3. Copy the following files from the new project into our app:
  
  * _Imports.razor
  
  * MainPage.xaml
  
  * MainPage.xaml.cs
  
  * Main.razor
4. Change MauiProgram.cs by adding the following code:
 Builder.Services.AddMauiBlazorWebView();
#if DEBUG
 builder.Services.AddBlazorWebViewDeveloperTools();
#endif

To review the commit history of these changes, go to https://epa.ms/Blazor7-1.
With these changes, we have made all the modifications we need to the configuration and are ready
to move on to the next step. However, before we start to work on these changes, let’s get familiar with
the basic Razor syntax.

# Understanding Razor syntax

Blazor apps consist of Razor components. As we learned in Chapter 3, User Interface Design with
XAML, XAML is a type of language derived from the XML language. XAML-based UI elements
consist of XAML pages and code-behind C# files. Razor components look very similar to this pattern.
The difference is that Razor uses HTML as its markup language and C# code can be embedded into
HTML directly. Optionally, we can also choose to separate C# code in a code-behind file to separate
the UI and logic.

# Code blocks in Razor

If we add a new Razor component to the project, it will look like this:
  
```  
<h3>Hello World!</h3>
@code {
 // Put your C# code here
}
```
In the preceding example, we can design our page just like an HTML page and put programming logic
into a code block. Razor pages or Razor components will be generated as C# classes. The filename is
used as the class name. They can be used as HTML tags on another Razor page.

# Implicit Razor expressions

In Razor syntax, we can transit from HTML to C# using the @ symbol. These are called implicit Razor
expressions. For example, we can use the following implicit expression to set the text of the label
tag with the currentUser.Username C# variable:
  
```xml
<label>@currentUser.Username</label>  
```
There must not be any spaces between implicit expressions. We cannot use C# generics in implicit
expressions since the characters inside the brackets (<>) are interpreted as an HTML tag.  
  
# Explicit Razor expressions  
  
To resolve the issues of implicit expressions (such as white space or using generics), we can use explicit
Razor expressions. Explicit Razor expressions consist of an @ symbol with parentheses. We can call
a generic method like so:  
  
```Csharp
<p>@(GenericMethod<int>())</p>  
```
When we want to concatenate text with an expression, we also need to use explicit expressions, like so:  
  
```Csharp
<p>@(currentUser.FirstName)_@(currentUser.LastName)</p> 
```  
We can use explicit Razor expressions in more complicated cases, such as passing a lambda expression
to an event handler. Let’s review another case of using an explicit Razor expression when we embed
HTML inside C# code.

# Expression encoding  
  
Sometimes, we may want to embed HTML as a string inside C# code, but the result may not be what
we expect.
  
Let’s say we write the following C# expression:
  
```@("<span>Hello World!</span>")```
  
The result will look like this after rendering:
  
```&lt;span&gt;Hello World&lt;/span&gt;```
  
To preserve the HTML string, we need to use the MarkupString keyword, like this:
  
```@((MarkupString)"<span>Hello World</span>")```
  
The result of the preceding C# expression is as follows:
  
```<span>Hello World!</span>```
  
This is the output that we want. We will learn more about explicit Razor expressions when we create
Razor components.  
  
# Directives 
  
Besides HTML code and C# code blocks, there are a set of reserved keywords to be used as Razor
directives. Razor directives are represented by implicit expressions with reserved keywords after the
@ symbol. We saw the code block as @code in the previous section. Here, @code is a directive with
a reserved keyword of code. The following are the directives that we can use in this book:  
  

  * @attribute: This is used to add the given attribute to the class

  * @code: This is used to define a code block

  * @implements: This is used to implement an interface for the generated class

  * @inherits: This is used to specify the parent class for the generated class

  * @inject: This is used to inject a service using dependency injection

  * @layout: This is used to specify a layout for routable Razor components

  * @namespace: This is used to define the namespace for the generated class

  * @page: This is used to define a route for the page

  * @using: This is similar to the using keyword in C#, which imports a namespace  
  
  
  # Directive attributes
  
  In a Razor page, HTML tags can be classes and attributes can be members of the class. Let’s review
the following example:
  
```xml
<input type="text" @bind="currentUser.Username">  
```
Here, input is an HTML tag, which is a class. The type attribute is the property of the input tag,
which is assigned to the "text" string. You may have noticed that another attribute called @bind
looks a little different from the normal attribute. It looks like a Razor implicit expression. Yes, it is an
implicit expression and bind is a reserved keyword. It is a directive attribute. The difference between
a Razor directive and a Razor directive attribute is that the latter is used as the attribute of an HTML
tag. The following are the directive attributes that we will use in this book:

  * @bind: This is used in data binding

  * @on{EVENT}: This is used in event handling

  * @on{EVENT}:preventDefault: This is used to prevent the default action for the event

  * @on{EVENT}:stopPropagation: This is used to stop event propagation

  * @ref: This is used to provide a way to reference a component instance

  * @typeparam: This is used to declare a generic type parameter  
  
Now that we’ve learned about the basic syntax of the Razor markup language, let’s move on to the
real work.  
  
# Creating a Razor component  
  
To develop a .NET MAUI Blazor app, we can choose to build all user interfaces using Blazor or a mix
of Razor components with XAML components. We will start with the second option first since we
already have a completed password manager app from Chapter 6, Introducing Dependency Injection
and Platform-Specific Services.  
  
# Redesigning the login page using a Razor component  
  
The user interface that we want to replace first is the login page. We can use a Razor page to replace
the XAML page to perform the same function.
In the Blazor Hybrid app, BlazorWebView is the control that hosts Razor components. We can
change LoginPage.xaml to the following  
  
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com
/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:b="clr-namespace:Microsoft.AspNetCore.Components.
 WebView.Maui;
 assembly=Microsoft.AspNetCore.Components.WebView.Maui"
 xmlns:local="clr-namespace:PassXYZ.Vault.Pages"
 x:Class="PassXYZ.Vault.Views.LoginPage"
 Shell.NavBarIsVisible="False">
 <b:BlazorWebView HostPage="wwwroot/login.html"> ❶
 <b:BlazorWebView.RootComponents>
 <b:RootComponent Selector="#login-app" ❸
 ComponentType="{x:Type local:Login}" /> ❷
 </b:BlazorWebView.RootComponents>
 </b:BlazorWebView>
</ContentPage>  
```
The preceding page only contains a BlazorWebView control. Here, we can pay attention to the
following items:

  * The HostPage attribute is used to specify the HTML page to load in BlazorWebView. It is
login.html (Listing 7.2) in this case.
The attributes of RootComponent define the Razor component and CSS selector to be used:

  * The ComponentType attribute specifies the Razor Login component; we will work on it soon.

  * The Selector attribute specifies the CSS selector on which our web UI will be loaded. We defined
the CSS #login-app ID in login.html. The login.html HTML page is created and stored
in the wwwroot folder. Let’s review it in Listing 7.2:  
  
**Listing 7.2: login.html (https://epa.ms/Login7-2)**  
  
```xml
<!DOCTYPE html>
<html lang="en">
<head>
 <meta charset="utf-8" />
 <meta name="viewport" content="width=device-width,
 initial-scale=1.0, maximum-scale=1.0, user-scalable=no,
 viewport-fit=cover" />
 <title>PassXYZ.Vault Login</title>
 <base href="/" />
 <link rel="stylesheet" href="css/bootstrap/
 bootstrap.min.css" />
 <link href="css/app.css" rel="stylesheet" />
 <link href="PassXYZ.Vault.styles.css" rel="stylesheet" />
</head>
<body class="text-center">
 <div id="login-app">Loading...</div> ①
 <div id="blazor-error-ui">
 An unhandled error has occurred.
 <a href="" class="reload">Reload</a>
 <a class="dismiss">✕</a>
 </div>
<script src="_framework/blazor.webview.js"
 autostart="false">
 </script>
</body>
</html>
```
As we can see in Listing 7.2, it is very similar to index.html, which we discussed earlier. It defines
the following CSS "login-app" ID ①, which is used to load our Razor component:  
  
```<div id="login-app">Loading…</div>```
  
In the .NET MAUI Blazor template, the CSS framework, Bootstrap (bootstrap.min.css), is
loaded by default. The embedded Bootstrap version was 5.1 at the time of writing. You may find a
newer version in your project.
Bootstrap is a well-known framework for web development. It comes with many examples of how to
use it. For the login page, there is a sign-in example on the Bootstrap website, as shown in Figure 7.6.
We will use it to build our Login component:  
  
![image](https://user-images.githubusercontent.com/26972859/231845068-0bcfaa88-aca2-4e2d-be2a-ba24dacbaee5.png
 
Figure 7.6: Bootstrap sign-in example  
  
You can find this sign-in example at https://getbootstrap.com/docs/5.1/examples/sign-in/.  
  
This sign-in example includes two files:

  * index.html (Listing 7.3) is the user interface of the sign-in page. It defines the following:

  * Two ```<input>``` tags for the username ❶ and password ❷

  * An ```<input>``` tag ❸ for a checkbox to remember the username

  * A ```<button>``` tag ❹ to process the login activity

  * It uses Bootstrap CSS styles and its own styles defined in signin.css

  * signin.css (Listing 7.4) defines the CSS styles specific to the sign-in page:  
  
  **Listing 7.3: index.html (Bootstrap sign-in example)**
  
 ```xml
 <!doctype html>
<html lang="en">
 <head> ... </head>
 <body class="text-center">
<main class="form-signin"> 
 <form>
<img class="mb-4" src=".../bootstrap-logo.svg"
 alt="" width="72" height="57">
 <h1 class="h3 mb-3 fw-normal">Please sign in</h1>
 <div class="form-floating">
 <input type="email" class="form-control"
 id="floatingInput" placeholder="name@example.com">❶
 <label for="floatingInput">Email address</label>
 </div>
 <div class="form-floating">
 <input type="password" class="form-control"
 id="floatingPassword" placeholder="Password"> ❷
 <label for="floatingPassword">Password</label>
 </div>
 <div class="checkbox mb-3">
 <label>
 <input type="checkbox" value="remember-me"> ❸
 Remember me
 </label>
 </div>
<button class="w-100 btn btn-lg btn-primary"
 type="submit">Sign in</button> ❹
 <p class="mt-5 mb-3 text-muted">&copy; 2017–2021</p>
 </form>
</main>
 </body>
</html>
```
In signin.css (Listing 7.4), we customize the form-signin CSS class , which is used in the
sign-in page of index.html:
  
**Listing 7.4: signin.css (Bootstrap sign-in example)**  
  
```css
html,
body {
 height: 100%;
}
body {
 display: flex;
 align-items: center;
 padding-top: 40px;
 padding-bottom: 40px;
 background-color: #f5f5f5;
}
.form-signin {
 width: 100%;
 max-width: 330px;
 padding: 15px;
 margin: auto;
}
.form-signin .checkbox {
 font-weight: 400;
}  
.form-signin .form-floating:focus-within {
 z-index: 2;
}
.form-signin input[type="email"] {
 margin-bottom: -1px;
 border-bottom-right-radius: 0;
 border-bottom-left-radius: 0;
}
.form-signin input[type="password"] {
 margin-bottom: 10px;
 border-top-left-radius: 0;
 border-top-right-radius: 0;
}
```
To create a new Razor component, we must create a folder called Pages in the project. After that,
we can right-click on the Pages folder that we just created in Visual Studio and select ```Add -> Razor Component```.
We can name this component Login.razor and create it. After creating the file, we
can copy the portion between the ```<main>``` tag from Listing 7.3 into the Razor page inside a ```<div>```
tag, as shown in Listing 7.5:  
  
**Listing 7.5: Login.razor (https://epa.ms/Login7-5)**
  
```razor
@using System.Diagnostics
@using PassXYZ.Vault.Services
@using PassXYZ.Vault.ViewModels
@inject LoginViewModel viewModel ❶
<div>
 <main class="form-signin">
 <form>
 <img class="mb-4"...>
 <h1 class="h3 mb-3 fw-normal">Please sign in</h1>
 <div class="form-floating">
 <label for="floatingInput">Username</label>
 <input type="text" @bind="@currentUser.Username" ❹
 class="form-control" id="floatingInput"
 placeholder="Username">
</div>
 <div class="form-floating">
 <label for="floatingPassword">Password</label>
 <input type="password" @bind="@currentUser
 .Password" class="form-control"
 id="floatingPassword" placeholder=
 "Password">
 </div>
 <div class="checkbox mb-3">
 <label>
 <input type="checkbox" value="remember-me">
 Remember me
 </label>
 </div>
 <button class="w-100 btn btn-lg btn-primary"
 type="submit" @onclick="OnLogin">Sign in</button>
 <p class="mt-5 mb-3 text-muted">&copy; 2017–2021</p>
 </form>
 </main>
</div>
@code {
 private LoginUser currentUser { get; set; } = default!; ❷
 protected override void OnInitialized() { ❸
 if (currentUser == null) {
 currentUser = viewModel.CurrentUser;
 }
 }
 private void OnLogin(MouseEventArgs e) {
 viewModel.OnLoginClicked(); 
 }
}
```
  
As we can see in Login.razor, Razor is a markup language that can mix HTML pages with C#
code. We can embed C# code after the @ symbol in HTML or put C# code inside the @code block.  
  
In the @code block, we define a private variable called currentUser ❷ of the LoginUser type.
We initialize currentUser in the override method, OnInitialized() ❸, to the property of
the view model. We can refer to currentUser in HTML after the @ symbol ❹. In the same way,
we can define the OnLogin() event handler and refer to it in the onclick event.
Once the user enters their username and password, the properties of currentUser are filled in. When
the user clicks the login button, OnLogin() will be invoked. The view model’s OnLoginClicked()
method  is used to perform the login action.  
  
# The Model-View-ViewModel (MVVM) pattern in Blazor

  The advantage of using Blazor for the user interface design is that we can do most of the UI design
using HTML first. Once we are satisfied with the UI design, we can add our programming logic to the
design. To separate the responsibilities in design, we can use the MVVM pattern, which we learned
about in Chapter 3, User Interface Design with XAML, in Razor component development. In Blazor,
we can treat the HTML markup as a view and the code block as a view model. If the logic in the code
block is too complex, we can separate it into a C# code-behind file.
On the login page, we can still use LoginViewModel from the XAML world. This is because
we will transit from Blazor back to the XAML UI inside LoginViewModel. This is done just to
demonstrate how we can mix the Blazor and XAML UIs in one app. We will replace the XAML UI
with the Blazor UI in the next chapter.
In a Razor component, we can either put both HTML and C# in one file, or we can split it into a Razor
file and a C# code-behind file, just like XAML.
Let’s do this for Login.razor. If we split it into two files, the component will be split into two partial
classes residing in Login.razor and Login.razor.cs, as we can see in Listing 7.6 and Listing 7.7:
  
**Listing 7.6 Login.razor (https://epa.ms/Login7-6)**  
  
```
@namespace PassXYZ.Vault.Pages
<div>
 <main class="form-signin">
 <form>
 <img class="mb-4"...>
 <h1 class="h3 mb-3 fw-normal">Please sign in</h1>
 <div class="form-floating">
 <label for="floatingInput">Username</label>
<input type="text" @bind="@currentUser.Username"
 class="form-control" id="floatingInput"
 placeholder="Username">
 </div>
 <div class="form-floating">
 <label for="floatingPassword">Password</label>
 <input type="password" @bind="@currentUser.
 Password" class="form-control"
 id="floatingPassword" placeholder="Password">
 </div>
 <div class="checkbox mb-3">
 <label>
 <input type="checkbox" value="remember-me"> Remember
me
 </label>
 </div>
 <button class="w-100 btn btn-lg btn-primary"
 type="submit" @onclick="OnLogin">Sign in</button>
 <p class="mt-5 mb-3 text-muted">&copy; 2021–2022</p>
 </form>
 </main>
</div>
```
In Listing 7.6, there is HTML markup only in Login.razor. This makes it looks better in terms of
separating the UI and logic. Let’s review the corresponding C# code in Listing 7.7:  
  
**Listing 7.7: Login.razor.cs (https://epa.ms/Login7-7)**  
  
```Csharp
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using PassXYZ.Vault.Services;
using PassXYZ.Vault.ViewModels;
using Microsoft.AspNetCore.Components.Web;
namespace PassXYZ.Vault.Pages;
public partial class Login : ComponentBase {
[Inject]
LoginViewModel viewModel { get; set; } = default!;
 private LoginUser currentUser { get; set; } = default!;
 protected override void OnInitialized() {
 if (currentUser == null) {
 currentUser = viewModel.CurrentUser;
 }
 }
 private void OnLogin(MouseEventArgs e) {
 viewModel.OnLoginClicked();
 }
}
```
In Listing 7.7, we moved all the code from the @code block to the C# file inside the Login class, which
is derived from the ComponentBase class. All Razor components are derived from ComponentBase.
You may have noticed that there is an attribute called Inject in the declaration of the viewModel
property. This is used for dependency injection.  
  
# Dependency injection in Blazor  
  
We introduced dependency injection in Chapter 6, Introducing Dependency Injection and PlatformSpecific Services. All the knowledge in that chapter applies equally here, but Blazor provides more.
With Blazor, we can use dependency injection in both HTML and C#.
As shown in Listing 7.5, the following declaration is defined at the beginning of Login.razor:  
  
```@inject LoginViewModel viewModel```  
  
Here, we inject the LoginViewModel class into the viewModel variable. This is property injection,
and we can use property injection much easier than before in Blazor.
To use dependency injection, we need to register LoginViewModel in MauiProgram.cs, as we
did in Chapter 6, Introducing Dependency Injection and Platform-Specific Services:  
  
```Csharp
builder.Services.AddSingleton<LoginViewModel,LoginViewModel>();
```
When we move it to the C# code-behind file, we can use the Inject attribute to do the same:  
  
```Csharp
[Inject]
LoginViewModel viewModel { get; set; } = default!;
```
In web development, we usually use both HTML and CSS styles together to design web user interfaces.
In the Bootstrap example, there is a signin.css file. Where do we keep our CSS styles? Let’s look
at this topic in the next section.  
  
# CSS isolation

When we introduced the Bootstrap sign-in example, we mentioned that there’s an HTML file and a
CSS file. Where do we put that CSS file so that we can reuse the sign-in CSS styles on our login page?
In HTML design, when we use a CSS framework such as Bootstrap, sometimes, we also need to
customize styles at the page level. To support this in Blazor, there is a technique called CSS isolation
for Razor components. For component- or page-specific CSS styles, we can keep it in a file with the
.razor.css extension. The filename should match the name of the .razor file in the same folder.
For our login page, we can copy sign-in.css from the Bootstrap example to Login.razor.
css and make minor modifications, as shown in Listing 7.8:  
  
# Listing 7.8: Login.razor.css (https://epa.ms/Login7-8)  
  
```css
div {
 display: flex;
 align-items: center;
 background-color: #f5f5f5;
}
.form-signin {
 width: 100%;
 max-width: 330px;
 padding: 15px;
 margin: auto;
}
.form-signin .checkbox {
 font-weight: 400;
}
.form-signin .form-floating:focus-within {
 z-index: 2;
}
.form-signin input[type="email"] {
 margin-bottom: -1px;
 border-bottom-right-radius: 0;
 border-bottom-left-radius: 0;
}
.form-signin input[type="password"] {
 margin-bottom: 10px;
 border-top-left-radius: 0;
 border-top-right-radius: 0;
}
```
The styles defined in Login.razor.css are only applied to the rendered output of the Login
component. Finally, let’s look at this new login UI in Blazor:  
  
 ![image](https://user-images.githubusercontent.com/26972859/231849849-fe4f7cbd-c896-409a-9bda-65d308f18af5.png)
  
 Figure 7.7: Login page 
  
We can see that the look and feel of this new UI in Figure 7.7 (shown in iOS) are the same as those
from the Bootstrap sign-in example, except we changed the icon. The functionality of logging in
hasn’t changed, except we used Blazor to create a new UI. After we log in using this Razor page, the
remaining programming logic is still the same as what was shown in Chapter 6, Introducing Dependency
Injection and Platform-Specific Services. The UI framework is switched back to XAML after login since
we haven’t changed anything after that yet.
  
# Summary

In this chapter, we learned about Blazor and how to develop a Blazor Hybrid app. Blazor is an
alternative solution for UI design for .NET MAUI. The difference between Blazor and XAML is that
the look and feel of the XAML user interface are the same as the native user interface, but the Blazor
UI has the look and feel of a web app. In terms of the functionalities that they can provide, there is
not much difference. We can mix Blazor and XAML in one app. We can use the MVVM pattern in
both XAML and Blazor. With Blazor, the UI code can be shared between the Blazor Hybrid app and
the web app. If you are looking for a solution that supports both native and web apps, .NET MAUI
Blazor could be a good choice.
In the next chapter, we will replace all the user interfaces in our app using Blazor. We will also introduce
how to do the initial UI design using layout and routing.  


