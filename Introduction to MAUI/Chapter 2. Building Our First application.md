# Building Our First application

In this chapter, you will learn how to set up your development
environment across all of the required platforms. You will then use
that environment to create, build, and run your very first .NET MAUI
application. Finally, you will take a look at the application you will build as
you progress through this book.

## Setting Up Your Environment

Before you get into creating and building the application, you must make
sure you have an environment set up

## macOS

There are several tools that you must install on macOS to allow support for
building Mac Catalyst applications and to provide the ability to build iOS
applications from a Windows environment.

--- This is required if you wish to develop on macOS or deploy to a Mac
or iOS device (even from a Windows machine). If you are happy with
only deploying to Windows or Android from a Windows machine, then
you can skip this part or just read it for reference.

## Visual Studio for Mac

As mentioned in the previous chapter, you will be primarily focusing on
using Visual Studio on Windows. You will have a requirement to use Visual
Studio for Mac much later, but you will set it up now. Also note that while
the book does focus on using Visual Studio on Windows, quite a lot of the
concepts should still translate well enough to Visual Studio for Mac if that
is your preferred environment.
Download and install Visual Studio 2022 for Mac. This can be accessed
from https://visualstudio.microsoft.com/vs/mac/.

1. Open the downloaded
VisualStudioForMacInstaller.dmg file.
2. Double-click the Install Visual Studio for Mac
option. Figure 2-1 shows the Visual Studio for Mac
installer.


![images/image21.png]()
Figure 2-1. Visual Studio for Mac installer

3. You will likely be shown a security dialog making
sure that you wish to run this installer. Double check
the details and then proceed if all looks fine.
4. Accept the terms and conditions by clicking
Continue.
5. In the next window, select .NET, Android, iOS, and
macOS (Cocoa) and then click Install. Figure 2-2
shows the installer with the required options for
.NET MAUI checked.

![images/image22.png]()
Figure 2-2. Visual Studio for Mac installation options

Please refer to the Microsoft documentation page at https://learn.
microsoft.com/dotnet/maui/get-started/installation?tabs=vsmac if
any of the installation options have changed.

## Xcode

Xcode is Apple’s IDE for building applications for iOS and macOS. You
don’t need to use Xcode directly, but Visual Studio needs it in order to
compile your iOS and macOS applications.
Thankfully this install is straightforward despite it being a rather large
download.

1. Open the App Store application.
2. Enter Xcode into the Search box and press return.
3. Click Get. Figure 2-3 shows Xcode available on the
Apple App Store.

![images/image23.png]()
Figure 2-3. Xcode on the App Store

4. Once downloaded, open Xcode and wait for it to
install the command line tools. Note that this is
usually required to be performed after each major
update to Xcode, too.

--- I suggest using caution when applying updates to the whole suite
of applications that you are installing today. Typically, when a new,
big release of .NET MAUI comes out, it likely requires an update of
Xcode. I personally like to keep these expected versions in sync so I
recommend checking for the updates within Visual Studio first and
verifying that it expects a new version of Xcode before proceeding to
update that.

## Remote Access

The final step to set up the macOS environment is to enable remote login
so that Visual Studio (Windows) can communicate to the Mac to build and
run iOS and macOS applications.

1. Open System Settings (macOS Ventura 13.0+) or
System Preferences on older macOS versions.

2. Select General on the left-hand panel and then
Sharing, as highlighted in Figure 2-4. This images/image
shows the macOS System Settings dialog with the
Sharing menu option highlighted.

![images/image24.png]()
Figure 2-4. macOS system settings

3. Enable Remote Login. Figure 2-5 shows the Remote
Login option enabled.

![images/image25.png]()
Figure 2-5. macOS sharing options

4. Add your user to the list of allowed users for
Remote Login. My user is an Administrator so the
Administrators user group enables remote login
access for this user. Figure 2-6 shows the Remote
Login editor to enable access for users on macOS.

![images/image26.png]()
Figure 2-6. macOS remote login options

5. That’s it! Your mac should now be ready to use.

# Windows

## Visual Studio

First, you must install Visual Studio 2022. These steps will guide you
through getting it ready to build .NET MAUI applications:

1. Download and install Visual Studio 2022. This
can be accessed from https://visualstudio.
microsoft.com/downloads/.
2. Run the installer and you will see the workload
selection screen. Select the Mobile development
with .NET workload. Figure 2-7 shows the Visual
Studio Windows installer with the required .NET
MAUI workloads checked.

![images/image27]()
Figure 2-7. Visual Studio Windows installation options

Please refer to the Microsoft documentation page at https://learn.
microsoft.com/dotnet/maui/get-started/installation?tabs=vswin if
any of the installation options have changed.

## Visual Studio to macOS

The final item to configure in your Windows environment is to set up the
connection between Visual Studio and your macOS so that iOS and macOS
builds can be compiled.

1. Inside Visual Studio select the Tools menu item.
2. Select iOS ➤ Pair to Mac
3. Check and confirm the firewall access. Figure 2-8
shows the firewall request dialog that is presented
when first running Visual Studio on Windows.

![images/image28.png]()
Figure 2-8. Windows firewall request

4. Note that you may also see a second firewall popup
for the Xamarin Broker.
5. Select your Mac from the list.
6. Click Connect. Figure 2-9 shows the Pair to Mac
dialog that allows you to connect your Visual Studio
running on Windows to connect to your macOS
machine.

![images/image29.png]()
Figure 2-9. Pair to Mac screen

7. Enter the username and password that you use to
log into your Mac
8. Wait for the tooling to connect and make sure that
everything is configured on the Mac.
9. When you see the symbol shown in Figure 2-10,
your setup is complete. Figure 2-10 shows the Pair to
Mac dialog with the connected symbol against your
macOS machine.

![images/image2-10.png]()
Figure 2-10. Pair to Mac screen with confirmation

10. Visual Studio should now connect automatically
when you open a .NET MAUI solution. Figure 2-11
shows the Pair to Mac button in Visual Studio on
Windows.

1[images/image2-11.png]

## Troubleshooting Installation Issues

Given that there are several moving parts in the development ecosystem
when building .NET MAUI applications, there is room for things to go
wrong. In this section, I will go over a few common issues and how to
check that things are correctly set up.

## .NET MAUI Workload Is Missing

In order to check whether the .NET MAUI workload has been installed, you
can check either in Visual Studio Installer or through the command line.

## Visual Studio Installer

This currently only works on Windows, but you can follow these steps.
1. Open the Start menu.
2. Type in Visual Studio Installer.
3. Open the installer.
4. Select Modify on the Visual Studio 2022 installation.
5. View the workloads and check that the Mobile
development with .NET workload is ticked.

## Command Line

This has the benefit of working on both Windows and macOS.
1. Open a Terminal session.
2. Run the following command:
dotnet workload list
3. Verify that the results include maui.

```
Installed Workload Id Manifest Version Installation Source
------------------------------------------------------
maui 7.0.49/7.0.100 SDK 7.0.100
```

## Creating Your First Application

You will be using the user interface in order to create your application,
build, and run it. I will also be including the dotnet command line
commands because I find they can be quite helpful when building and
debugging.

## Creating in Visual Studio

1. Launch Visual Studio 2022. In the window that
opens, select the Create a new project option.
Figure 2-12 shows the initial starting screen in Visual
Studio running on Windows with the Create a new
project option highlighted.

![images/image2-12.png]()
Figure 2-12. Creating a project in Visual Studio

2. In the window that follows, type .NET MAUI in the
Search for templates box. Then select the .NET
MAUI App option and click Next. Figure 2-13 shows
the project creation screen with the .NET MAUI App
project selected.

![images/image2.13.png]()
Figure 2-13. Selecting a .NET MAUI App project type

3. In the next window, enter a name for your project. I
chose WidgetBoard. Choose a location if you would
like to store it somewhere different from the default
location, and click Create. Figure 2-14 shows the
Configure your new project screen in Visual Studio.

![images/image2-14.png]()
Figure 2-14. The Configure your new project dialog

---Please bear in mind that Windows has a limitation on the length of
the location path. If the path is longer than 255 characters, then
strange behavior will follow. Visual Studio will fail to build perfectly
valid code and so on. This can be rectified by disabling the path limit
(https://learn.microsoft.com/windows/win32/fileio/
maximum-file-path-limitation?tabs=cmd#enable-longpaths-in-windows-10-version-1607-and-later).


4. Select the version of .NET you wish to use. At the time
of writing this book .NET 7.0 is the current version
so I am using this version. Figure 2-15 shows the
Additional information dialog where you can choose
the .NET Framework version for your application.

![images/image2-15.png]()
Figure 2-15. The .NET Framework selection dialog

5. Wait for the project to be created and any background
restore and build tasks to be completed.

Now admire the very first .NET MAUI application that we have created
together.

## Creating in the Command Line

While the command line might feel more complicated, at times there are
actually fewer steps required than when using Visual Studio.

1. Open a Terminal/command line session.
2. cd to the location you want to create your
application:

```cd c:\work\```

3. Create the application, giving the project a name:
```dotnet new maui --name WidgetBoard```

4. cd to the new folder, WidgetBoard:
```cd WidgetBoard```

5. Pull in all dependencies for the application:
```dotnet restore```

You now have a .NET MAUI application. Let’s proceed to learning how
to build and ultimately run it.

## Building and Running Your First Application

Now that you have your project created, let’s go ahead and build and run
it in order to get familiar with the tooling. The introduction of the single
project approach for .NET MAUI applications may bend your way of
thinking when it comes to building applications. In the past, a solution
containing .NET projects would typically have a single start-up project,
but these projects would have a single output. Now that a single project
actually has multiple outputs, you need to learn how to configure that for
your builds. In fact, this is done by clicking the down arrow, which can be
seen in Figure 2-16.

![images/image2-16.png]()
Figure 2-16. Build target selection dropdown in Visual Studio


You may also notice the dropdown in the above images/image that currently
says WidgetBoard (net7.0-android). This allows you to show in the visible
file what applies to that specific target, but it does not affect what you are
currently compiling. Figure 2-17 shows this a little clearer.
1. This is where you set the current target to compile
for and run.
2. This is highlighting in the code file what will compile
for the target chosen in the dropdown. Notice here
that you are compiling for Windows but showing
what would compile for Android.

Figure 2-17 highlights items 1 and 2 from the above list to highlight
what is compiled vs. what is targeted in Visual Studio.

![images/image2-17.png]()
Figure 2-17. Showing the differences between what target is being
compiled and what target is being shown in the current editor

## Getting to Know Your Application

Together we will be building an application from the very initial stages
through to deploying it to stores for public consumption. Given that the
application will play such a pivotal role in this book, I want to introduce
you to the concept first.
I want to try something a little bit different from the normal types of
apps that are built as part of a book or course. Something that requires a
fair amount of functionality that a lot of real-world applications also need.
Something that can help to make use of potentially older hardware so we
can give them a new lease on life.

## WidgetBoard

The application that we will be building together will allow users to turn
old tablets or computers into their own unique digital board. Figure 2-18
shows a sketch of how it could look once a user has configured it.

![images/image2-18.png]()
Figure 2-18. Sketch prototype of the application we will be building

We will build “widgets” that can be positioned on the screen. These
widgets will range from showing the current time to pulling weather
information from a web API to displaying images/images from your library. The
user will also be able to customize the color, among other options, and
ultimately save these changes so that they will be remembered when the
user next opens the application.
I am planning for this to provide a digital calendar/photo frame for our
home. I would love to hear or see what you are able to build.

# Summary

In this chapter, you have
• Set up your development environment so that you are
capable of creating, building, and ultimately running/
deploying the application.
• Created, built, and run your very first .NET MAUI
application
• Met the application that we will be building together
In the next chapter, you will
• Dissect the application you just created.
• Gain an understanding of the key components of a
.NET MAUI application.
• Learn about the lifecycle of a .NET MAUI application.

# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch02.















