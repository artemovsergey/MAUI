# Introduction to MAUI

Welcome to Introducing .NET MAUI.
This book is for developers who are new to .NET MAUI and cross-platform
development. You should have basic knowledge of C# but require no prior
knowledge of using .NET MAUI. The content ranges from beginner through
to more advanced topics and is therefore tailored to meet a wide range of
experiences.
This book provides an in-depth explanation of each key concept in
.NET MAUI, and you will use these concepts in practical examples while
building a cross-platform application. The content has been designed
to primarily flow with the building of this application; however, there
is a secondary theme that involves grouping as many related concepts
as possible. The idea behind this is to both learn as you go and also to
have content that closely resembles reference information, which makes
returning to this book as easy as possible.
All code examples in this book, unless otherwise stated, are applied
directly to the application you are building. Once key concepts have been
established, the book will offer improvements or alternatives to simplify
your life as you build production-worthy applications. The book does not
rely upon these simplifications as part of the practical examples and the
reason for this is simple: I strongly believe that you need to understand the
concepts before you start to use them.
Finally, all chapters that involve adding code into the application
project contain a link to the resulting source code. This is to show the final
product and for you to use as a comparison if anything goes wrong during
your building of the application.

# CHAPTER 1

# Introduction to .NET MAUI

In this chapter, you will gain an understanding of what exactly .NET
MAUI is, how it differs from other frameworks, and what it offers you as a
developer wishing to build a cross-platform application that can run on
both mobile and desktop environments. I will also cover the reasons why
you should consider it for your next project by weighing the possibilities
and limitations of the framework as well as the rich array of tooling
options.

# What is .NET MAUI?

.NET Multi-platform App UI, or .NET MAUI for short, is a crossplatform framework that allows developers to build mobile and desktop
applications written in C# and XAML. It allows developers to target both
mobile (Android and iOS) and desktop (macOS and Windows) platforms
from a single codebase. Figure 1-1 shows the platforms officially supported
by .NET MAUI and Microsoft.

![image1.png]()
Figure 1-1. .NET MAUI platform support

NET MAUI provides a single API that allows developers to write once
and run anywhere. When building a .NET MAUI application, you write
code that interacts with this single cross-platform API and .NET MAUI
provides the bridge between your code and the platform-specific layer.
If you take a look inside the prism in Figure 1-1, you can start to
understand the components that .NET MAUI both uses and offers.
Figure 1-2 shows how an Android application is compiled.

![image2.png]()
Figure 1-2. Interacting with .NET MAUI APIs

Of course, there will be times when you need to directly access a
platform feature. .NET MAUI also provides enough flexibility that you can
achieve this by interacting directly with the platform-specific APIs:

- .NET for Android
- .NET for iOS
- .NET for macOS
- Windows UI Library (WinUI) 3

Figure 1-3 shows how the code bypasses the .NET MAUI APIs and
interacts directly with the .NET for Android APIs.

![image3.png]()
Figure 1-3. Interacting with platform-specific APIs

# Digging a Bit Deeper

There are some extra steps that the tooling will perform under the hood
to get your application built and ultimately ready for use on each of the
possible platforms.
When building a .NET application, even if it is not using .NET MAUI,
you will very likely hear the term BCL, which is short for the base class
library. This is the foundation of all .NET applications, and in the same way
that .NET MAUI abstracts away the platforms you wish to build for, the BCL
abstracts away what that platform implements when your application runs.
To run your application on your desired platform, you need a .NET
runtime. For Android, iOS, and macOS, this is the Mono runtime. The
Mono runtime provides the ability to run .NET code on many different
platforms. For Windows, this is Win32. Each of these runtimes provide
the functionality required for the BCL and therefore a consistent working
environment across all supported platforms.

I like to think of the BCL as the contract between what we are
compiling against and what we are running that compiled code with.
Figure 1-4 shows all of the layers involved in compiling and running a
.NET MAUI application.

![image4.png]()
Figure 1-4. The full breakdown

To continue with the example of building for Android in the previous
diagrams and taking note of the diagram in Figure 1-4, the following can
be said:
Your code is compiled against .NET MAUI, .NET for Android and the
base class library. It then runs on the Mono runtime, which provides a
full implementation of the base class library on the Android platform.
Looking at the above statement, you can replace the parts that are
platform specific with another platform (e.g., swapping Android for iOS)
and the statement will still be true.

# Where Did It Come From?

NET MAUI is the evolution of Xamarin.Forms, which itself has a rich
history of providing developers with a great way to build cross-platform
applications. Of course, no framework is perfect, and Xamarin.Forms
certainly had its limitations. Thankfully the team at Microsoft decided 

to take the pragmatic approach of taking a step back and evaluating all
the pain points that existed for themselves as maintainers and (more
importantly) for us as developers using the framework.
Not only do we therefore gain improvements from the Xamarin
framework as part of this evolution, but we also get all the goodies that
come with .NET such as powerful built-in dependency injection, better
performance, and other topics that I will touch on throughout this book.
This makes me believe that this mature cross-platform framework has
finally become a first-class citizen of the .NET and Microsoft ecosystems. I
guess the clue is in the first part of its new name.
On the topic of its name, .NET MAUI implies that it is a UI framework,
and while this is true, this is not all that the framework has to offer.
Through the .NET and the .NET MAUI platform APIs, we are provided with
ways of achieving common application tasks such as file access, accessing
media from the device gallery, using the accelerometer, and more. The
.NET MAUI platform APIs were previously known as Xamarin Essentials,
so if you are coming in with some Xamarin Forms experience, they
should feel familiar. I will touch on much more of this functionality as you
progress through this book

# How It Differs From the Competition

.NET MAUI provides its own controls (for example, a Button) and then
maps them to the relevant implementation on each platform. To continue
with the example of a button, this is a UIButton from UIKit on iOS and
macOS, an AppCompatButton from AndroidX.AppCompat.Widget on
Android, and a Button from Microsoft.UI.Xaml.Controls on Windows.
This gives a great level of coverage in terms of providing a common
implementation that works across all platforms. With the introduction of
the .NET MAUI handler architecture (which will we be looking at in more
detail in Chapter 11) we truly gain the power to tweak the smallest of
implementation details on a per-platform basis. This is especially useful
when the common API provided by .NET MAUI may be limited down to
the least amount of crossover between each platform and doesn’t provide
everything we need. It is worth noting that your application will render
differently on each platform as it utilizes the platform specific controls and
therefore their look and feel.
Other frameworks such as Flutter opt to render their own types directly
rather than mapping across to the implementations provided by each
platform. These frameworks provide a common look and feel across each
platform. This is a hotly contested topic but I personally believe that making
applications fit in with the platform they are running on is a big benefit.

# Why Use .NET MAUI?

There are several reasons why you should consider using .NET MAUI for
your next application: a large number of supported platforms, increased
code sharing capabilities, an emphasis on allowing developers to build
applications that fit their style, great performance, and many more. Let’s
take a look at them.

# Supported Platforms

.NET MAUI provides official support for all of the following platforms:

- Android 5.0 (API level 21) and above
- iOS 11.0 and above
- macOS 10.15 and above (using Mac Catalyst) **
- Windows desktop

** MacCatalyst allows native Mac apps to be built and share code with
iPad apps. This is an Apple technology that allows developers to shared code
between Mac and iPad. For further reference, go to the Apple documentation
at https://developer.apple.com/mac-catalyst/.

.NET MAUI provides community-driven support for
- Tizen: The implementation is provided by Samsung

I thoroughly recommend checking out the documented list of supported
platforms in case it has changed since the time of writing. The list can be found
at https://learn.microsoft.com/dotnet/maui/supported-platforms.

# Code Sharing

A fundamental goal of all cross-platform frameworks is to enable
developers to focus on achieving their main goals by reducing the effort
required to support multiple platforms. This is achieved by sharing
common code across all platforms. Where I believe .NET MAUI excels over
alternative frameworks is in the first four characters of its name; Microsoft
has pushed hard to produce a single .NET that can run anywhere.
Being a full stack developer myself, I typically need to work on webbased back ends as well as mobile applications, .NET allows me to write
code that can be compiled into a single library. This library can then be
shared between the web and client applications, further increasing the
code sharing possibilities and ultimately reducing the maintenance effort.
I have given talks based on a mobile game (www.superwordsearch.
com) I built using Xamarin.Forms, where I boasted that we were able to
write 96% of our code in our shared project. I have not yet converted this
across to .NET MAUI; however, initial investigations show that this number
will only increase.
There are further possibilities for sharing code between web and client,
such as the use of .NET MAUI Blazor, which provides the use of web-based
technologies inside a .NET MAUI application. While I won’t be covering
.NET MAUI Blazor in detail in this book, Microsoft does provide some
really great documentation and guides on what it is and how to build your
first application with the technology at https://learn.microsoft.com/
aspnet/core/blazor/hybrid/tutorials/maui.

# Developer Freedom

.NET MAUI offers many ways to build the same thing. Where Xamarin.
Forms was largely designed to support a specific application architecture
(such as MVVM, which I will talk all about in Chapter 4), .NET MAUI is
different. One key benefit of the rewrite by the team at Microsoft is it now
allows the use of other architectures such as MVU (Chapter 4). This allows
us as developers to build applications that suit our preferences, from
different architectural styles to different ways of building UIs and even
different ways of styling an application.

# Community

Xamarin has always had a wonderful community. From bloggers to opensource maintainers, there is a vast amount of information and useful
packages available to help any developer build a great mobile application.
One thing that has really struck me is the number of Microsoft employees
who are part of this community; they are clearly passionate about the
technology and dedicate their own free time to contributing to this
community. The evolution to .NET MAUI brings this community with it

# Fast Development Cycle

.NET MAUI offers two great ways to boost a developer’s productivity.

# .NET Hot Reload

.NET Hot Reload allows you to modify your managed source code while
the application is running, without the need to manually pause or hit a
breakpoint. Then, your code edits can be applied to your running app
without the need to recompile. It is worth noting that this feature is not
specific to .NET MAUI but is yet another great example of all the goodness
that comes with the framework being part of the .NET ecosystem.

# XAML Hot Reload

XAML Hot Reload allows you to edit the UI in your XAML files, save the
changes, and observe those changes in your running application without
the need to recompile. This is a fantastic feature that really shines when
you need to tweak some controls.

# Performance

NET MAUI applications are compiled into native packages for each of the
supported platforms, which means that they can be built to perform well.
Android has always been the slowest platform when dealing with
Xamarin.Forms and the team at Microsoft has been working hard and
showing off the improvements. The team has provided some really great
resources in the form of blog posts covering the progress that has been
made to bring the start-up times of Android applications to well below
one second. These posts cover metrics plus tips on how to make your
applications really fly.

- • https://devblogs.microsoft.com/dotnet/
performance-improvements-in-dotnet-maui/
-• https://devblogs.microsoft.com/dotnet/dotnet-7-
performance-improvements-in-dotnet-maui/

Android apps built using .NET MAUI compile from C# into
intermediate language (IL), which is then just-in-time (JIT) compiled to a
native assembly when the app launches.
iOS and macOS apps built using .NET MAUI are fully ahead-of-time
(AOT) compiled from C# into native ARM assembly code.
Windows apps built using .NET MAUI use Windows UI Library
(WinUI) 3 to create native apps that target the Windows desktop.

# Strong Commercial Offerings

There are several commercial options that provide additional UI elements
and other integrations such as Office document editing or PDF viewing in
your .NET MAUI applications. Some options (at the time of writing) are

- SyncFusion
“The feature-rich/flexible/fast .NET MAUI controls for
building cross-platform mobile and desktop apps with C#
and XAML”
www.syncfusion.com/maui-controls


- Telerik UI for .NET MAUI
“Kickstart your multiplatform application development with a
Preview version of Telerik UI for .NET MAUI controls!”
www.telerik.com/maui-ui

- DevExpress
“Our .NET Multi-platform App UI Component Library
ships with high-performance UI components for Android
and iOS mobile development (WinUI desktop support is
coming in 2022). The library includes a Data Grid, Chart,
Scheduler, Data Editors, CollectionView, Tabs, and Drawer
components.”
www.devexpress.com/maui/

Note that while these are commercial products, several of them
provide free licenses for smaller companies or independent developers so
I recommend checking out their products.

# Limitations of .NET MAUI

I hope this doesn’t get me in too much trouble with the wonderful team
over at Microsoft ☺. This section is not aimed at slating the technology (I
wouldn’t be writing a book about something I didn’t believe in); it is purely
aimed at making clear what cannot be achieved or at least what is not
provided out of the box, to help you as a reader best decide whether this is
the right technology for your next project. Of course, I hope it is, but let’s
look at what I feel are its main limitations.

# No Web Assembly (WASM) Support

NET MAUI does not provide support for targeting Web Assembly. This
means that you cannot target the web directly from a .NET MAUI project,
but you can still run Blazor inside your .NET MAUI application. This opens
the door for further code sharing; as discussed earlier, it is entirely possible
to build Blazor components that can be shared between .NET MAUI Blazor
and .NET Blazor applications.
If you do require direct WASM support, then a good alternative to .NET
MAUI is the Uno Platform.

# No Camera API

This has been a pain point for a lot of developers throughout the life
of Xamarin.Forms and continues to be an initial pain point for .NET
MAUI. There are some good arguments as to why it hasn’t happened.
Building a camera API against the Android Camera offering has not been
an easy task, as I am sure most developers who have embarked on that
journey can attest to. The sheer fact that Google is rewriting the entire API
for a third time shows the inherent challenges.

# Apps Won’t Look Identical on Each Platform

Controls in .NET MAUI make use of the platform implementations,
therefore an entry control on iOS will render differently to one on Android.
There are investigations into providing a way to avoid this and have
controls render exactly the same on all platforms, but this is still at an
early stage.

# Lack of Media Playback Out of the Box

Playing media has become a very common task. So many apps these days
offer the ability to play video or audio. I suspect this is also due to the vast
differences between platforms in how they provide playback support.

---While this functionality is not officially provided by .NET MAUI, this
does not mean the functionality is not possible.

# The Glass Is Half Full, Though

I believe that limitations are not a bad thing. Doing some things well is
a far better achievement than doing everything badly! I expect the list of
limitations will reduce as .NET MAUI matures. Thanks to the fact that
.NET MAUI is open source, we as consumers have the ability to suggest
improvements and even provide them directly to further enhance this
framework. I must also add that the .NET MAUI Community Toolkit is
great (of course, I am biased because I currently help to maintain it). It
provides value for the .NET MAUI community, and it is also maintained by
that community. Another huge advantage is that concepts in this toolkit
can and have been promoted to .NET MAUI itself. This gives me hope that
one day there will be a solid choice for both camera and media playback
APIs in .NET MAUI.

# How to Build .NET MAUI Applications   

There are several different ways to build an application with .NET MAUI. I
will look at each in turn, covering some details that will hopefully help you
decide which is the best fit for you.

# Visual Studio

Visual Studio is available for both Windows and macOS, but while these
operating systems are both officially supported, it is worth noting that the
Visual Studio products are separate and themselves not cross-platform.
This means that functionality and levels of support differ between the two.
Note that the Windows and macOS versions come with three different
pricing options, but I would like to draw your attention to the Community
edition, which is free for use by small teams and for educational purposes.
In fact, everything in this book can be achieved using the free Community
edition.

# Visual Studio (Windows)

Visual Studio is a comprehensive integrated development environment or
IDE that provides a great development experience. I have been using this
tool for years and I can happily say that it continues to improve with each
new version.
To build .NET MAUI apps, you must use at least Visual Studio 2022.
In Visual Studio (Windows), it is possible to build applications
that target

- Android
- iOS *
- Windows

* A networked Mac with Xcode 13.0 or above is required for iOS
development and deployment. This is due to limitations in place by Apple.

# Visual Studio for Mac

Visual Studio for Mac has only been released recently and as of 2022 it
has undergone a significant rework to provide a better experience for
developers.
To build .NET MAUI apps, you must use at least Visual Studio 2022
for Mac.
In Visual Studio for Mac, it is possible to build applications that target

- Android
- iOS
- macOS

# Rider

JetBrains Rider is an impressive cross-platform IDE that can run on
Windows, macOS, and Linux. JetBrains has a history of producing great
tools to help developers achieve their goals. One highlight is ReSharper,
which assists with inspecting and analyzing code. With Rider the
functionality provided by ReSharper is built in.
JetBrains offers Rider for free but only for educational use and opensource projects.

# Visual Studio Code

Visual Studio Code is a very popular lightweight code editor also provided
by Microsoft. Using extensions and the dotnet CLI, it is entirely possible
to build .NET MAUI applications using no other tools. It is worth noting,
however, that the debugging experience is vastly inferior to the other
products we have discussed.

# Summary

Throughout the course of this book, you will primarily be using Visual
Studio (Windows) as the tool to build your application. I will refer to Visual
Studio for Mac in the later parts when I cover how to deploy and test
macOS applications.
In this chapter, you have learned the following:

- What .NET MAUI is
- What it offers and what it does not offer
- Reasons why you should consider using it
- The tooling options available to build a .NET MAUI
application

In the next chapter, you will

- Get to know the application we will be building
together
- Learn how to set up the environment to build the
application