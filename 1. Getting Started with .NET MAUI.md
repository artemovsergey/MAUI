# Getting Started with .NET MAUI

Since the release of .NET 5, Microsoft has been trying to unify different .NET implementations into
one .NET release. .NET Multi-platform App UI (or .NET MAUI) is an effort to provide a unified
cross-platform UI framework. We will learn how to use .NET MAUI to develop cross-platform
applications in this book.

The following is what we will learn in this chapter:

• An overview of cross-platform technologies

• A comparison of cross-platform technologies (.NET, Java, and JavaScript)

• The .NET landscape and the history of Xamarin

• .NET MAUI features

• .NET MAUI Blazor apps

• A development environment setup

If you’re new to .NET development, this chapter will help you to understand the .NET landscape. For
Xamarin developers, many topics in this book may sound familiar, and this chapter will give you an
overview of what we will discuss in this book.

# An overview of cross-platform technologies

Before discussing cross-platform technologies, let’s review the application development landscape
first to understand the different cross-platform technologies better.
.NET MAUI is a cross-platform development framework from Microsoft for building apps, targeting
both mobile and desktop form factors on Android, iOS, macOS, Windows, and Tizen.

Generally, software development can be divided into two categories – systems programming and
application programming. Application programming aims to produce software that provides services
to the user directly, whereas system programming aims to produce software and software platforms
that provide services to other software. In the .NET domain, the development of the .NET platform
itself belongs to systems programming, whereas the application development on top of the .NET
platform belongs to application programming.
The design or architecture in a modern system includes the client and server side of software, which
we can refer to as the frontend and backend.
For the software on the client side, we can further divide it into two categories – native applications
and web applications.

## Native applications

In native application development, we usually refer to application development for a particular operating
system. With desktop applications, this could be Windows applications, macOS applications, or Linux
applications. With mobile applications, this could be Android or iOS.
When we develop a native application, we have to develop it for each platform (Windows, Linux,
Android, or macOS/iOS). We need to use different programming languages, tools, and libraries to
develop each of them individually

## Web applications

Web application development has gone through several generations of evolution over the past few
decades, from a Netscape browser with static web pages to a modern single-page application (SPA)
using JavaScript frameworks (such as React or Angular). In web application development, JavaScript
and various JavaScript-based frameworks dominate the market. In the .NET ecosystem, Blazor is
trying to catch up in this area.

## Backend services

Both native applications and web applications usually need some backend services to access business
logic or a database. For backend development, many languages and frameworks can be used, such as
Java/Spring, .NET, Node.js, Ruby on Rails, or Python/Django. Usually, native applications and web
applications can share the same backend service. Java and .NET are the most popular choices for
backend service developments.

# Cross-platform technologies

Technologies used in web application development and backend services development are not
platform-specific and can be used on different platforms as they are. When we talk about cross-platform
development, we usually refer to native application development. In native application development, 
cross-platform development technologies can help to reduce costs and improve efficiency. The most
popular cross-platform development technologies in this category include Flutter, .NET MAUI/
Xamarin, and React Native. Table 1.1 provides an overview of available cross-platform technologies
and alternative solutions from Microsoft. The technologies listed here are not exhaustive. I just want
to give you a feeling of what kind of technologies exist in each category and what Microsoft solution
can be used as an alternative.

![image](https://user-images.githubusercontent.com/26972859/231533858-3d8929c7-6534-44d9-9691-30920ed3a466.png)
Table 1.1: A comparison of languages and frameworks with Microsoft solutions

There is no best choice of cross-platform tool or framework. The final choice is usually decided
according to business requirements. However, from the preceding table, we can see that the .NET
ecosystem provides a full spectrum of tools for your requirements. The development team for a large
system usually requires people with experience in different programming languages and frameworks.
With .NET, the complexity of programming languages and frameworks can be dramatically simplified.

## A comparison of .NET, Java, and JavaScript 

We had an overview of the tools and frameworks used in web apps, native apps, and backend services
development. If we look at a higher level, that is, at the .NET ecosystem level, the ecosystem of Java or
JavaScript can match almost what we have in a .NET solution. Java, JavaScript, or .NET solutions can
provide tools or frameworks at nearly all layers. It would be interesting to compare Java, JavaScript,
and .NET at a higher level.
Java is developed as a language with the goal to write once and run anywhere. It is built around the Java
programming language and the Java Virtual Machine (JVM). The JVM is a mechanism to run on
supported platforms that helps to remove platform dependency for developers. With this cross-platform
capability, Java becomes a common choice for cross-platform applications and services development.
JavaScript is a language created for web browsers, and its capability is extensive due to the demands of
web development. The limitation of JavaScript is that it is a scripting language, so it lacks the language
features that can be found in Java or C#. However, this limitation doesn’t limit its usage and popularity.
Table 1.2 offers a comparison of three technologies:

![image](https://user-images.githubusercontent.com/26972859/231534265-50567661-7e0c-483f-b0b1-769733ff263f.png)
Table 1.2: A comparison of Java, JavaScript, and .NET

From Table 1.2, we can see that both .NET and Java have a good infrastructure to support multiple
languages. JavaScript has its limitation as a scripting language, so TypeScript and CoffeeScript were
invented to enhance it. TypeScript was developed by Microsoft to bring modern object-oriented
language features to JavaScript. TypeScript is compiled into JavaScript for execution, so it can work
well with existing JavaScript libraries.
Java is built around the JVM while .NET is built around the Common Language Runtime (CLR) and
the Common Type System (CTS). With the CTS and CLR as the core of a .NET implementation, it
supports multiple languages naturally with the capability to share a Base Class Library (BCL) in all
supported languages.
While there are multiple languages that use the JVM as the abstraction layer for cross-platform capability,
the interoperation between Java-derived languages is not at the same level as .NET languages. All
.NET languages are built on one architecture and share the same BCL, while Java languages, such as
Java, Kotlin, or Scala, are developed separately for very different purposes.

This comparison helps us to choose or evaluate a tech stack for cross-platform development. As a
.NET MAUI developer, this analysis can help you understand your choice better. To understand where
.NET MAUI is located in the .NET ecosystem, let’s have a quick overview of the history of the .NET
landscape in the next section.

##  Exploring the .NET landscape

Before we dive into the details of .NET MAUI, let’s have an overview of the .NET landscape. This
section is relevant if you are new to .NET. If you are a .NET developer, you can skip this section.
Since Microsoft introduced the .NET platform, it has evolved from a proprietary software framework
for Windows to a cross-platform and open source platform.

There are many ways to look at the .NET technology stack. Basically, it contains the following components:

* Common infrastructure (Compiler and tools suite)
* BCLs
* Runtime (Windows Runtime (WinRT) or Mono)

## .NET Framework

The history of .NET history begins with .NET Framework. It is a proprietary software framework
developed by Microsoft that runs primarily on Microsoft Windows. .NET Framework started as a
future-oriented application framework to standardize the software stack in the Windows ecosystem.
It is built around a Common Language Infrastructure (CLI) and C#. Even though the primary
programming language is C#, it is designed to be a language-agnostic framework. Supported languages
can share the same CTS and CLR. Most Windows desktop applications are developed using .NET
Framework, and it is shipped as a part of the Windows operating system.

## Mono

The first attempt to make .NET an open source framework was made by a company called Ximian.
When the CLI and C# were ratified by ECMA in 2001 and ISO in 2003, it provided a potential
opportunity for independent implementations.
In 2001, the open source project Mono was launched, aimed at implementing .NET Framework on
Linux desktop software.
Since .NET Framework was a proprietary technology at that time, .NET Framework and Mono had
their own compiler, BCL, and runtime.
Over time, Microsoft moved toward open source, and .NET source code became open source. The
Mono project adopted some source code and tools from the .NET code base.

At the same time, Mono projects went through many changes as well. At the time that Mono was
owned by the Xamarin company, Xamarin developed the Xamarin platform based on Mono to
support the .NET platform on Android, iOS, Universal Windows Platform (UWP), and macOS. In
2016, Microsoft acquired Xamarin, which became the cross-platform solution in the .NET ecosystem.

## .NET Core

Before the acquisition of Xamarin, Microsoft has already started work to make .NET a cross-platform
framework. The first attempt was the release of .NET Core 1.0 in 2016. .NET Core is a free and open
source framework, available for Windows, Linux, and macOS. It can be used to create modern web
apps, microservices, libraries, and console applications. Since .NET Core applications can run on
Linux, we can build microservices using containers and cloud infrastructure.
After .NET Core 3.x was released, Microsoft worked toward integrating and unifying .NET technology
on various platforms. This unified version was to supersede both .NET Core and .NET Framework. To
avoid confusion with .NET Framework 4.x, this unified framework was named .NET 5. Since .NET
5, a common BCL can be used on all platforms. In .NET 5, there are still two runtimes, which are
WinRT (used for Windows) and the Mono runtime (used for mobile and macOS).
In this book, we use will the .NET 6 release.

## .NET Standard and portable class libraries

Before the .NET 5 releases, with .NET Framework, Mono, and .NET Core, we had a different subset
of BCLs on different platforms. In order to share code between different runtimes or platforms, a
technique called Portable Class Libraries (PCLs) was used. When you create a PCL, you have to
choose a combination of platforms that you want to support. The level of compatibility choices is
decided by the developers. If you want to reuse any PCL, you must carefully study the list of platforms
that can be supported.
Even though a PCL provides a way to share code, it cannot resolve compatibility issues nicely. To
overcome the compatibility issues, Microsoft introduced .NET Standard.
.NET Standard is not a separate .NET release but instead a specification of a set of .NET APIs that
must be supported by most .NET implementations (.NET Framework, Mono, .NET Core, .NET 5 or
6, and so on).
After .NET 5 and later versions, a unified BCL is available, but .NET Standard will be still part of
this unified BCL. If your applications only need to support .NET 5 or later, you don’t really need to
care too much about .NET Standard. However, if you want to be compatible with old .NET releases,
.NET Standard is still the best choice for you. We will use .NET Standard 2.0 in this book to build
our data model, since this is a version that can support most existing .NET implementations and all
future .NET releases.
There will be no new versions of .NET Standard from Microsoft, but .NET 5, .NET 6, and all future
versions will continue to support .NET Standard 2.1 and earlier. Table 1.3 shows the platforms and
versions that .NET Standard 2.0 can support, and this is also the compatible list for our data model
in this book.

|.NET implementation|Version support|
|:------------------|:--------------|
|.NET and .NET Core |2.0, 2.1, 2.2, 3.0, 3.1, 5.0, and 6.0|
|.NET Framework 1|4.6.1 2, 4.6.2, 4.7, 4.7.1, 4.7.2, and 4.8|
|Mono|5.4 and 6.4|
|Xamarin.iOS|10.14 and 12.16|
|Xamarin.Mac|3.8 and 5.16|
|Xamarin.Android|8.0 and 10.0|
|UWP|10.0.16299, TBD|
|Unity|2018.1|
Table 1.3: .NET Standard 2.0-compatible implementations

The open-source project KPCLib is a .NET Standard 2.0 library, and we will use it in our app. In
Table 1.3, we can see that .NET Standard libraries can be used in both Xamarin and .NET MAUI apps.

## Using Xamarin for mobile development 

As we mentioned in an earlier section, Xamarin was part of the Mono project and was an effort to
support .NET on Android, iOS, and macOS. Xamarin.Forms is a cross-platform UI framework from
Xamarin. .NET MAUI is an evolution of Xamarin.Forms. Before we discuss .NET MAUI and Xamarin.
Forms, let us review the following diagram of Xamarin implementation on various platforms.

![image](https://user-images.githubusercontent.com/26972859/231535927-4e39cd8a-ae58-48ed-ab4b-4eae86d6b332.png)
Figure 1.1: Xamarin implementations

Figure 1.1 shows the overall architecture of Xamarin. Xamarin allows developers to create native UIs
on each platform and write business logic in C# that can be shared across platforms.

On supported platforms, Xamarin contains bindings for nearly the entire underlying platform SDKs.
Xamarin also provides facilities for directly invoking the Objective-C, Java, C, and C++ libraries,
giving you the power to use a wide array of third-party code. You can use existing Android, iOS, or
macOS libraries written in Objective-C, Swift, Java, or C/C++.
The Mono runtime is used as the .NET runtime on these platforms. It has two modes of operation –
Just-in-Time (JIT) and Ahead-of-Time (AOT). JIT compilation generates code dynamically as it is
executed. In AOT compilation mode, Mono precompiles everything, so it can be used on operating
systems where dynamic code generation is not possible.
As we can see in Figure 1.1, JIT can be used on Android and macOS, while AOT is used for iOS where
dynamic code generation is not allowed.
There are two ways to develop native applications using Xamarin.
You can develop native applications just like Android, iOS, or macOS developers, using native APIs
on each platform. The difference is that you use .NET libraries and C# instead of the platform-specific
language and libraries directly. The advantage of this approach is you can use one language and share
a lot of components through the .NET BCL, even if you work on different platforms. You can also
leverage the power of underlying platforms like native application developers.
If you want to reuse code on the user interface layer, you can use Xamarin.Forms instead of the native UI.

## Xamarin.Forms

Xamarin.Android, Xamarin.iOS, and Xamarin.Mac provide a .NET environment that exposes almost
the entire original SDK capability on their respective platforms. For example, as a developer, you
have almost the same capability with Xamarin.Android as you do with the original Android SDK. To
further improve code sharing, an open source UI framework, Xamarin.Forms, was created. Xamarin.
Forms includes a collection of cross-platform user interface components. The user interface design
can be implemented using the XAML markup language, which is similar to Windows user interface
design in WinUI or WPF.

## Xamarin.Essentials

Since Xamarin exposes the capability of the underlying platform SDKs, you can access device features
using the .NET API. However, the implementation is platform-specific. For example, when you use a
location service on Android or iOS, the .NET API can be different. To further improve code sharing
across platforms, Xamarin.Essentials can be used to access native device features. Xamarin.Essentials
provides a unified .NET interface for native device features. If you use Xamarin.Essentials instead of
native APIs, your code can be reused across platforms.

Some examples of functionalities provided by Xamarin.Essentials include the following:

• Device info

• The filesystem

• An accelerometer

• A phone dialer

• Text-to-speech

• Screen lock

Using Xamarin.Forms together with Xamarin.Essentials, most implementations, including business
logic, user interface design, and some level of device-specific features, can be shared across platforms.

## Comparing user interface design on different platforms 

Most modern application development on various platforms uses the Model-View-Controller
(MVC) design pattern. To separate the business logic and user interface design, there are different
approaches used on Android, iOS/macOS, and Windows. On all the platforms involved, even though
the programming languages used are different, they all use XML-based markup language to design
user interfaces.
On an iOS/macOS platform, developers can use Interface Builder in XCode to generate .storyboard
or .xib files. Both are XML-based script files used to keep user interface information, and this script
is interpreted at runtime together with Swift or Objective-C code to create the user interface. In 2019,
Apple announced a new framework, SwiftUI. Using SwiftUI, developers can build user interfaces using
the Swift language in a declarative way directly.
On the Android platform, developers can use Layout Editor in Android Studio to create a user
interface graphically and store the result in layout files. The layout files are in the XML format as well
and can be loaded at runtime to create the user interface.
On the Windows platform, Extensible Application Markup Language (XAML) is used in user
interface design. XAML is an XML-based language used for user interface design on the Windows
platform. For a WPF or UWP application, the XAML Designer can be used for user interface design.
In .NET MAUI, the XAML-based UI is the default application UI. Another pattern, the Model View
Update (MVU) pattern, can also be used. In the MVU pattern, the user interface is implemented in
C# directly without XAML. The coding style of MVU is similar to SwiftUI.
Even though SwiftUI on Apple platforms or MVU in .NET MAUI can be used, but the classic user
interface implementation is the XML-based markup language. Let us do a comparison in Table 1.4.

|Platform|IDE|Editor|Language|File extension|
|:-------|:--|:-----|:-------|:-------------|
|Windows|Visual Studio|XAML Designer|XAML/C# |.xaml|
|iOS/macOS |Xcode|Interface Builder|XML/Swift/Objective C|.layout|
|Android|Android Studio|Layout Editor|XML/Java/Kotlin|.storyboard or .xib|
|.NET MAUI/Xamarin.Forms |Visual Studio|N.A.|XAML/C#|.xaml|
|.NET MAUI Blazor |Visual Studio|N.A.|Razor/C#|.razor|

Table 1.4: A comparison of user interface design

In Table 1.4, we can see a comparison of user interface design on different platforms.
.NET MAUI and Xamarin.Forms use a dialect of XAML to design user interfaces on all supported
platforms. For .NET MAUI, we have another choice for user interface design, which is Blazor. We
will discuss Blazor later in this chapter.
In Xamarin.Forms, we create user interfaces in XAML and code-behind in C#. The underlying
implementation is still the native controls on each platform, so the look and feel of Xamarin.Forms
applications are the same as native ones.

Some examples of features provided by Xamarin.Forms include the following:

* XAML user interface language

* Data binding

* Gestures

* Effects

* Styling

Even though we can share almost all UI code with Xamarin.Forms, we still need to handle most of
the resources used by an application in each platform individually. These resources could be images,
fonts, or strings. In the project structure of Xamarin.Forms, we have a common .NET standard project
and multiple platform-specific projects. Most of the development work will be done in the common
project, but the resources are still handled in the platform-specific projects separately

## Moving to .NET MAUI

With the .NET unification, Xamarin has become a part of the .NET platform, and Xamarin.Forms
integrates with .NET in the form of .NET MAUI.
.NET MAUI is a first-class .NET citizen with the Microsoft.Maui namespace.

Making the move to .NET MAUI is also an opportunity for Microsoft to redesign and rebuild Xamarin.
Forms from the ground up and tackle some of the issues that have been lingering at a lower level.
Compared to Xamarin.Forms, .NET MAUI uses a single project structure, supports hot reloads better,
and supports MVU and Blazor development patterns.
From Figure 1.2, we can see that there is a common BCL for all supported operating systems. Under the
BCL, there are two runtimes, WinRT and the Mono Runtime, according to the platform. For each platform,
there is a dedicated .NET implementation to provide full support for native application development.

![image](https://user-images.githubusercontent.com/26972859/231538129-9f10d6e0-c39f-47d1-83a6-0fad8e868e21.png)
Figure 1.2: .NET MAUI architecture

Comparing to Xamarin.Forms, we can see from Table 1.5, there are many improvements in .NET MAUI.
.NET MAUI uses a single project structure to simplify project management. We can manage resources,
dependency injection, and configurations in one location instead of managing them separately
per platform.
.NET MAUI is fully integrated as part of .NET, so we can create and build projects using the .NET
SDK command line. In this case, we have more choices in terms of development environments.

|          |.NET MAUI|Xamarin.Forms|
|:---------|:--------|:------------|
|Project structure|Single project|Multiple projects|
|Resource management|One location for all platforms |Managed per platform|
|Fully integrated with .NET| Namespace in Microsoft.Maui and other IDEs can be chosen beside Visual Studio. Command-line support. We can create, build, and run in a console: dotnet new maui, dotnet build -t:Run -f net6.0-android, dotnet build -t:Run -f net6.0-ios, dotnet build -t:Run -f net6.0-maccatalyst|Namespace in Xamarin.Forms and it uses Visual Studio as an IDE       |
|Design improvement|1. Configuration through .NET Generic Host 2. Dependency injection support|Configuration scattered in different locations|
|MVU pattern|A modern type of UI implementation|No|
|Blazor Hybrid|Support through BlazorWebView|No|

Table 1.5: .NET MAUI improvement

In Table 1.5, we can see that .NET MAUI supports application configuration using .NET generic host,
can work with multiple IDE environments, supports dependency injection, and can use the MVVM
toolkit, etc. It also supports the MVU pattern and Blazor Hybrid UI. Next, we will look at the Blazor
Hybrid app.

## .NET MAUI Blazor apps 

In Table 1.4, where we compared the user interface design options on different platforms, we mentioned
that there is another way to design cross-platform user interfaces in .NET MAUI, which is Blazor.
Released in ASP.NET Core 3.0, Blazor is a framework for building an interactive client-side web UI with
.NET. With .NET MAUI and Blazor, we can build cross-platform apps in the form of Blazor Hybrid
apps. This way, the boundary between a native application and a web application becomes blurred.
.NET MAUI Blazor Hybrid apps enable Blazor components to be integrated with native platform
features and UI controls. The Blazor components have full access to the native capabilities of a device.
The way to use the Blazor web framework in .NET MAUI is through a BlazorWebView component.
.NET MAUI Blazor enables both native and web UIs in a single application, and they can co-exist in a
single view. With .NET MAUI Blazor, applications can leverage the Blazor component model (Razor
components), which uses HTML, CSS, and the Razor syntax. The Blazor part of an app can reuse
components, layouts, and styles that are used in an existing regular web app. BlazorWebView can
be composed alongside native elements; additionally, they leverage platform features and share states
with their native counterparts.

## Choosing XAML versus Razor in .NET MAUI

To design the user interface of your .NET MAUI application, you have a few choices for implementation:
• XAML: Implement user interface in XAML that is only similar to Xamarin.Forms. We can also
choose the MVU pattern to use C# code to create and style UI elements directly. No matter
whether you choose XAML or C# code directly, the underlying implementation is the same.
• Blazor: Implement a user interface in Razor Pages, which is similar to web application development.
• Blazor Hybrid app: Use both XAML and Razor Pages in your application.
It’s your decision how you want to design your application. You can choose one of the preceding
options or mix XAML and the Blazor UI according to the best fit. To develop Blazor Hybrid apps, you
should be able to use most of the existing Blazor libraries directly. Blazor provides good JavaScript
interoperability, and you can use your favorite JavaScript library in your development.

## Development environment setup

Both Windows and macOS can be used for .NET MAUI development, but you won’t be able to build
all targets with only one of them. You will need both Windows and Mac computers to build all targets.
In this book, the Windows environment is used to build and test Android and Windows targets. iOS
and macOS targets are built on a Mac computer.
.NET MAUI app can target the following platforms:
• Android 5.0 (API 21) or higher
• iOS 10 or higher
• macOS 10.13 or higher, using Mac Catalyst
• Windows 11 and Windows 10 version 1809 or higher, using Windows UI Library (WinUI) 3
.NET MAUI Blazor apps use the platform-specific WebView control, so they have the following
additional requirements:
• Android 7.0 (API 24) or higher
• iOS 14 or higher
• macOS 11 or higher, using Mac Catalyst
.NET MAUI build targets of Android, iOS, macOS, and Windows can be built using Visual Studio
on a Windows computer. In this environment, a networked Mac is required to build iOS and macOS
targets. Xcode must be installed on the paired Mac to debug and test an iOS MAUI app in a Windows
development environment.

.NET MAUI targets of Android, iOS, and macOS can be built and tested on macOS. 

|Target platform|Windows|macOS|
|:---------|:-------|:-------|
|Windows|Yes|No|
|Android|Yes|Yes|
|iOS|Yes (pair to Mac)|Yes|
|macOS|Build only (pair to Mac)|Yes|

Table 1.6: The development environment of .NET MAUI

Please refer to Table 1.6 to find out the build configurations on Windows and macOS.

# Installing .NET MAUI on Windows

.NET MAUI can be installed as part of Visual Studio 2022. The Visual Studio Community edition is free,
and we can download it from the Microsoft website at https://visualstudio.microsoft.
com/vs/community/.
After launching Visual Studio Installer, we will see a screen similar to the one shown in Figure 1.3.
Please select .NET Multi-platform App UI development and .NET desktop development in the list
of options. We also need to select ASP.NET and web development for the .NET MAUI Blazor app,
which will be covered in Part 2 of this book.

![image](https://user-images.githubusercontent.com/26972859/231541579-2fcedff7-54c6-444f-aa94-39c5c5d05f31.png)
Figure 1.3: Visual Studio 2022 installation

After the installation is completed, we can check the installation from the command line using the
following dotnet command:

```
C:\>dotnet workload list
Installed Workload Ids Manifest Version Installation Source
--------------------------------------------------------------
------
maui-windows 6.0.486/6.0.400 VS 17.3.32811.315
maui-maccatalyst 6.0.486/6.0.400 VS 17.3.32811.315
maccatalyst 15.4.446-ci.-release-6-0-
4xx.446/6.0.400 VS 17.3.32811.315
maui-ios 6.0.486/6.0.400 VS 17.3.32811.315
ios 15.4.446-ci.-release-6-0-
4xx.446/6.0.400 VS 17.3.32811.315
maui-android 6.0.486/6.0.400 VS 17.3.32811.315
android 32.0.448/6.0.400 VS 17.3.32811.315
Use `dotnet workload search` to find additional workloads to
install.
```
We are ready to create, build, and run a .NET MAUI app on Windows.

# Installing .NET MAUI on macOS

The installation of the Visual Studio Community edition is similar to what we have done on Windows.
The installation package can be downloaded from the same link.

![image](https://user-images.githubusercontent.com/26972859/231541800-7dc6c71a-8b14-4b90-b349-054ab535d6af.png)
Figure 1.4: Visual Studio for Mac 2022 installation

Please select .NET and .NET MAUI from the list of options in Figure 1.4.
After the installation is completed, we can check the installation from the command line using the
following dotnet command:

```
% dotnet workload list
Installed Workload Ids Manifest Version
Installation Source
--------------------------------------------------------------
------
wasm-tools 6.0.9/6.0.400 SDK 6.0.400
macos 12.3.453/6.0.400 SDK 6.0.400
maui-maccatalyst 6.0.536/6.0.400 SDK 6.0.400
maui-ios 6.0.536/6.0.400 SDK 6.0.400
maui-android 6.0.536/6.0.400 SDK 6.0.400
ios 15.4.453/6.0.400 SDK 6.0.400
maccatalyst 15.4.453/6.0.400 SDK 6.0.400
maui 6.0.536/6.0.400 SDK 6.0.400
tvos 15.4.453/6.0.400 SDK 6.0.400
android 32.0.465/6.0.400 SDK 6.0.400
Use `dotnet workload search` to find additional workloads to
install.
```
We are ready to create, build, and run a .NET MAUI app on macOS.

## What you will learn in this book

In this book, you will learn how to develop cross-platform applications using .NET MAUI. The
following are the topics covered in this book:
* The .NET ecosystem and .NET MAUI
* User interface design using XAML
* Data binding using the MVVM design pattern
* .NET MAUI Shell navigation and design navigations using routes
* Dependency injection
* Exploring Blazor to design a user interface using the .NET MAUI Blazor Hybrid app
* Using xUnit.net to write unit test cases for .NET classes and bUnit to implement test
cases for Razor Pages
* Publishing an application to Google Play, Apple Store, and Microsoft Store

# The app that we will build in this book

Throughout the book, we will learn about .NET MAUI programming by building a password manager
app. KeePass is an open source password manager used by many people. However, it is an application
built for .NET Framework, so it can run on Windows only. KeePass has a library, KeePassLib, which
implements most of the logic for encrypted database management. This is a popular database format
for password management. For example, IntelliJ IDEA uses the KeePass database to store passwords
used during development phases.
To make KeePassLib a cross-platform library, I ported KeePassLib to .NET Standard 2.0 as
the open source KPCLib project. The source code can be found at https://github.com/
passxyz/KPCLib.
I published an app, PassXYZ.Vault, developed using Xamarin.Forms and KPCLib in app stores.
In this book, we will rewrite PassXYZ.Vault together, using .NET MAUI. You can learn about
cross-platform programming through the progressive development of this app until it is published
in app stores.

# Summary

In this chapter, we started with an overview of cross-platform technologies. We compared a .NET
solution with other cross-platform technologies. After that, we went through the .NET landscape.
I explained the relationship between .NET Framework, Mono, and .NET Core. Then, we discussed
Xamarin and .NET MAUI. We reviewed the difference between Xamarin.Forms and .NET MAUI.
One important feature in .NET MAUI is that we can use Blazor together with XAML user interface
design. Then, we had an overview of .NET MAUI Blazor. Finally, we set up a development environment
for the rest of the chapters.
In the next chapter, we will explore how to build a .NET MAUI application from scratch.

# Further reading

* .NET MAUI: You can find more information about .NET MAUI in Microsoft’s official
documentation: https://docs.microsoft.com/en-us/dotnet/maui/
* KeePass: The official website for KeePass can be found at https://keepass.info/





