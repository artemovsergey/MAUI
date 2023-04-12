# CHAPTER 14

# Releasing Our Application

Once you have built your application, you need to get it to your users.
There are many ways to achieve this. You can publish a release build and
ship it directly to your customers or you can make use of the stores that
each platform provider offers.
Shipping directly to an end customer can sometimes be the best
option, such as when you are building an internal application and you
don’t want it to be publicly accessible.
Most often the recommended way to ship applications to users is to go
through the stores provided by each platform provider (e.g., App Store from
Apple, Play Store from Google, and Microsoft Store from Microsoft). This
does involve agreeing to terms and conditions, and these providers take a
percentage of any income you make. There are many benefits that justify
paying the fees. They provide trusted platforms for users to find and download
your applications. The store will provide a much wider reach for your intended
audience. The store also manages the ability to provide updates seamlessly.

# Distributing Your Application

The aim of this section is not to give a step-by-step guide on distributing
to each of the stores mentioned above. Initially, I wanted to provide this
information, but the details around doing so have changed numerous 
times during the time it has taken to write this book. For this reason alone,
I will defer to the platform providers and Microsoft’s documentation on
how to achieve distribution. What this chapter will cover is details around
distributing applications, why you need to do it, and some of the common
issues that crop up during the process.

---
One very important thing to note is that apps built with .NET MAUI
follow the same rules and common issues that native applications
follow. Therefore, when encountering issues with each specific store,
sometimes a search engine will return better results if you omit the
.NET MAUI part.
---

# Android

Android has the biggest mobile user base. However, given the model it
follows of allowing manufacturers to customize the Android operating
system as well as providing varying sets of hardware, it can be the most
problematic.
An Android Package, or APK for short, is the resulting application file
that runs on an Android device. If you wish to provide a mechanism to
download this file (e.g., a website or file share), users can side load the
application onto their Android device. This is not recommended in the
public domain because it can be very difficult to trust the packages that are
freely downloaded from the Internet.
When you wish to distribute using the Google Play Store, you are
required to build an Android App Bundle, or AAB for short. It contains all
of the relevant files needed to compile an APK ready for installation on a
user’s device.
Essentially you build an Android App Bundle, sign it with a specific
signing key that you own, and upload the bundle to Google Play. Google
uses this bundle when a user comes to download your application and
compiles a specific APK for that device. This is the way to do things now.
If you have worked with Android apps in the past, you may recall building
the APK yourself. This runs into the issue that the APK is architecturespecific, and in the current market where there are multiple architectures
supported by the various Android devices, you can end up with an
application size that is the sum of the number of architectures multiplied
by the actual size (for example, if there are four architectures and the
application size is 25MB, the resulting APK is 100MB).

# Additional Resources

Both Microsoft and Google provide documentation on how to distribute
applications via the Google Play Store. See the following links.
• Microsoft: How to publish an application ready for the
Play Store, https://learn.microsoft.com/dotnet/
maui/android/deployment/overview
• Google: How to upload your application to the Play
Store, https://developer.android.com/studio/
publish/upload-bundle
• It is also worth noting that other stores/platforms
provide the ability to distribute, install, and run
Android applications. Amazon devices such as the
Kindle Fire are built on top of Android and allow the
running of Android applications. Amazon provides its
own store, details of which can be found at https://
developer.amazon.com.

# iOS

iOS and macOS are considered really painful when dealing with
distributing and signing. Having spent several years going through this
pain, I want to break down the key concepts to hopefully reduce the pain
that you might experience. Thankfully the Apple tooling has come a long
way since I started building mobile apps in 2007, so you don’t have to
relive all those painful memories.
The following sections cover the development and application settings
you will need to create on the Apple developer website at https://
developer.apple.com.

# Certificate

You need to generate a certificate on the machine that will build your
application. Most documentation takes you through the complex scenarios
of creating a Certificate Signing Request and then uploading to Apple.
There is actually a far simpler way by using Xcode. The following steps can
help to achieve this:
• Click the Xcode main menu.
• Click on Settings.
• Click on the Accounts tab.
• Click the Manage Certificates button, shown in
Figure 14-1.

![image](https://user-images.githubusercontent.com/26972859/231512532-0223268d-e52a-4ede-81dd-900520790fa4.png)
Figure 14-1. Apple settings screen showing how to manage
certificates

# Identifier

This represents your application. It requires you to define unique details to
identify the application that will be exposed to the public store as well as
defining what capabilities your application requires.

# Capabilities

iOS applications run under a sandboxed environment. Apple provides a
set of App Services that can be utilized by your applications and enhance
its capabilities. Capabilities include services like in-app purchasing, push
notifications, Apple Pay, and such. The use of these services needs to be
defined at compile time and the usage of them will be reviewed when you
upload your application to Apple for review. Therefore, it’s important that
you make sure you only have the ones you need. Don’t worry, though; a failure here will give a fairly useful error message and can be a relatively
easy fix. More information can be found at https://developer.apple.
com/documentation/xcode/capabilities.

---
Changes to the capabilities of your application will invalidate your
provisioning profiles so they will need to be edited in the https://
developer.apple.com portal to update them with the newer
capabilities.
---

# Entitlements

Entitlements tie in closely with capabilities and allow you to configure
settings during compilation. You need to add an Entitlements.plist file
to your application and then add the relevant entries for the configuration.
Information on how to configure this can be found at https://learn.
microsoft.com/dotnet/maui/ios/deployment/entitlements

# Provisioning Profiles

Provisioning profiles determine how your application will be provisioned
for deployment. There are two main types:

• Development: This is what you need when running a
debug build of your application on your own device.

• Distribution: This is required for the release builds
when you ship to the App Store.

A common issue around provisioning profiles is when trying to run the
application on your device and the tooling reports back Unable to deploy
app to this device, no provisioning profiles were found. When observing
this a good starting point is to double check that you have the provisioning
profile installed and whether the profile has been invalidated by changing
any Capabilities.

# Additional Resources

Both Microsoft and Apple provide documentation on how to distribute
applications via the Apple App Store.
• Microsoft: How to publish an application ready for the
App Store, https://learn.microsoft.com/dotnet/
maui/ios/deployment/overview
• Apple: How to upload your application to the App
Store, https://developer.apple.com/app-store/

# macOS

When distributing your .NET MAUI application for macOS, you can
generate an .app or a .pkg file. An .app file is a self-contained app that
can be run without installation, whereas a .pkg is an app packaged in an
installer.

# Additional Resources

Both Microsoft and Apple provide documentation on how to distribute
applications via the Apple App Store.
• Microsoft: How to publish an application ready for the
App Store, https://learn.microsoft.com/dotnet/
maui/macos/deployment/overview
• Apple: How to upload your application to the App
Store, https://developer.apple.com/macos/
distribution/

# Windows

When distributing your .NET MAUI app for Windows, you can publish the
app and its dependencies to a folder for deployment to another system.
Publishing a .NET MAUI app for Windows creates an MSIX app package,
which has numerous benefits for the users installing your app.
MSIX is a Windows app package format that provides a modern
packaging experience to all Windows apps.

# Additional Resources

Microsoft provides documentation on how to distribute applications via
the Microsoft Store.
• Microsoft: How to publish an application ready for the
App Store, https://learn.microsoft.com/dotnet/
maui/windows/deployment/overview
• Microsoft: How to upload your application to the
Microsoft Store, https://developer.microsoft.com/
microsoft-store/

# Things to Consider

Many issues can crop up when you make the jump from a debug build
running on a simulator, emulator, or physical device to building a release
build ready to run on an end user’s machine.

# Following Good Practices

Each of the platform-specific sections prior to this one contained
information or links to resources that show how to deploy your
applications to each platform provider’s public store. This is all great but 
one key detail that is lacking is the use of continuous integration and
continuous delivery (CI/CD) in order to provide a clean environment that
can reliably produce a build that can be deployed.
Continuous integration (CI) is the practice of merging all developers'
working copies to a shared mainline.
Continuous delivery (CD) is a software engineering approach in which
teams produce software in short cycles, ensuring that the software can be
reliably released at any time and, when releasing the software, without
doing so manually. It aims at building, testing, and releasing software with
greater speed and frequency. The approach helps reduce the cost, time,
and risk of delivering changes by allowing for more incremental updates to
applications in production. A straightforward and repeatable deployment
process is important for continuous delivery.
Both concepts are usually considered together as they help to make
it a far smoother experience when working in a team. I was there in the
early stages of learning and building apps and I neglected this part. If I
could go back and tell a much younger Shaun some advice, it would be
to get this part set up and early in the development process. Thanks to
the dotnet CLI that is available to us, the setup to provide the necessary
steps is straightforward. On top of that, tools like GitHub, Azure DevOps,
TeamCity, and others will likely provide some level of out of the box
support for this.
If you imagine that each of the applications can be built with the
dotnet CLI, for example

```dotnet publish -f:net7.0-android -c:Release```

---
I am using net7.0 here because my application is built against .NET
7.0. If you are working against a different version of .NET, replace
net7.0 with your chosen version. If you are unsure what version
you are using, open your csproj file and look at the value inside the
<TargetFrameworks></TargetFrameworks> tags.
---
There are more required arguments to pass to the build, which involve
signing key passwords and more, but this shows how easily this can be
added to a set of automated steps that run each time code is committed or
a merge request is opened.
You should also consider the testing that you added in Chapter 12 and
see how this can also be incorporated into a CI environment.

```dotnet test```

This is far simpler than the publishing step. Running the tests in a CI
environment really should be considered a critical set of criteria when
building any application. The safety net that this provides in making sure
your changes do not unintentionally break other bits of functionality alone
makes it worthwhile.

# Performance

Android has always been one of the slower platforms when building
mobile applications. Don’t get me wrong; the applications can perform
well on the higher-end devices, but Android devices come in a wide
range of specifications, and typically in the business environment it is
the cheaper devices that get bought in bulk and are expected to perform
well. There are some concepts that you should consider when publishing
your Android applications in order to boost the performance of your
applications.

# Startup Tracing

There are some extra steps that you can do in order to boost the startup
times of your Android applications. Startup tracing essentially profiles
an application when it starts to determine what libraries and other
initializations are required so when you release the application it will
benefit from a faster startup time. It is worth noting that boosting the
startup time can result in an increase in application size so I recommend
playing around with the settings to find the right balance for your
application.
Microsoft has published two great blog posts on how startup tracing
can be configured, the improvements it makes, and how the application
can be affected:

• https://devblogs.microsoft.com/dotnet/dotnet-7-
performance-improvements-in-dotnet-maui/
• https://devblogs.microsoft.com/xamarin/fasterstartup-times-with-startup-tracing-on-android/

# Image Sizes

One thing that can perform really poorly is the use of images that do not
match the dimensions in which they need to be rendered on screen.
For example, an image that displays at 100x100 pixels in the application
really should be that size when supplied. If you were to render an image
that was actually 300x300 pixels, it will not only look poor on the device
due to scaling, but it will slow the application down. Plus, it involves
storing an image that is bigger than really needed. Therefore, make sure
that your images are correctly sized to gain the best experience when
rendering them.

# Use of ObservableCollection

A lot of common coding examples show how to bind an
ObservableCollection to the ItemsSource property of a control. This
can have its uses, but it can have a big performance overhead. The reason
is that each time an element is added to the collection, a UI update will
be triggered because the control is monitoring for changes against the
ObservableCollection. If you do not need live updating items in a
collection, it is typically much faster to use a List and simple raise the
PropertyChanged event from INotifyPropertyChanged instead.
Let’s take a look at the code you added in Chapter 9 and see how it can
be improved:

```Csharp
public ObservableCollection<Board> Boards { get; } = new
ObservableCollection<Board>();
public void LoadBoards()
{
 var boards = this.boardRepository.ListBoards();
 foreach (var board in boards)
 {
 Boards.Add(board);
 }
}
```
You can improve the performance of the above code by implementing
it with a List as follows:

```Csharp
private IList<Board> boards;
public IList<Board> Boards
{
 get => this.boards;
 private set => SetProperty(ref this.boards, value);
}
public void LoadBoards()
{
 Boards = this.boardRepository.ListBoards();
}
```
This new code will result in the UI only being updated once rather than
once per each board that is added to the Boards collection.

# Linking

While devices these days do tend to offer generous amounts of storage
space, it is still considered a very good practice to minimize the amount of
memory your apps really consume, especially when considering mobile
devices that have limited data networks in order to download the apps.

# What Is Linking?

Linking is performed by the Linker to remove unused code from compiled
assemblies. This helps to reduce the size of your applications by trimming
out any unused parts of libraries that you use.
Linking is a highly complex topic and I am only really scratching the
surface. For further reference, I recommend checking out the Microsoft
documentation at https://learn.microsoft.com/xamarin/ios/deploytest/linker.
The Linker provides the fantastic ability to reference the full .NET base
class library so when you compile your application ready for distributing,
it will only include the parts of that BCL that you actually reference and use
within your application.

# Issues That Crop Up

As you can imagine, if the Linker is unable to detect that something is
really used in your application and it is removed, things can go very wrong
at runtime. Your application will most likely crash when it tries to use a
type that isn’t included in your build.
This can quite often happen when only referring to types in XAML. As
I covered in Chapter 5, the XAML compiler isn’t as powerful as the C#
compiler and it can miss scenarios.
Reflection is another option to really avoid. Not only can it trick the
compiler into not realizing APIs are used but it can also not perform well.
It is worth considering that some third-party packages that you end
up using in your applications may not be Linker safe. For this reason, the
default setting of Link SDK assemblies only is set. This means that only
the assemblies provided by Microsoft will be linked because they are built
to be Linker safe. In an ideal world, the third-party libraries would also
be Linker safe, but I can safely say that the people building these fantastic
packages are already spread thin building them, so if it is something that
you really require, I strongly urge you to investigate helping them provide it
or sponsoring the people that build it to help them.

# Crashes/Analytics

Given that I have covered how things can go wrong, I would like to cover
a way in which you can gain insight to when that happens. Each of the
platform providers do offer a way to collect crash information and report
them it to you in order to make sure that you can prevent things like
crashes from ruining the experience your applications provide.
There are frameworks/packages that aim to make this process easier by
collecting and collating information from each platform into a centralized
site. Further to this, you can enable the collection of analytic information
to aid your understanding of how your users like to interact with your
application and identify areas that you can improve upon.
In fact, a lot of the effort in my day job goes into finding ways to
improve products. This only truly comes to light when you learn how your
users interact with your applications. Capturing analytic information
isn’t the sole route I recommend taking. End user engagement can also
be a fantastic thing to do if you have the opportunity. I would also like
to highlight things like App Tracking Transparency by Apple and the
Google equivalent as you want to make sure that when collecting analytic
information you are not passing on information that can be used to track
your users, or you at least make them aware of it. Further to this, it is
considered good practice to allow users to opt in to enable the collection of
analytical information rather than just capturing it or making them opt out.
There are some companies that provide solutions for this already. They
are fee-based but do offer a free tier with fewer features.

# Sentry

Sentry offers a .NET MAUI package that will make it easier to collect crash
and analytical information. The website contains details on its usage and
pricing: https://sentry.io/for/dot-net/.
Sentry also has the source code open sourced on GitHub and provides
usage examples as well as assisting in understanding what the code does:
https://github.com/getsentry/sentry-dotnet/tree/main/src/
Sentry.Maui

# App Center

App Center offers a wide range of features but the main one to focus on
for here is the concept of collecting crash and analytical information. The
website contains details on its usage and pricing: https://appcenter.ms.
Andreas Nesheim has written a great article on how you can get started
with using App Center diagnostics in .NET MAUI: www.andreasnesheim.
no/using-app-center-diagnostics-analytics-with-net-maui/.

# Obfuscation

It is a very safe assumption that if you are providing a compiled
application to user’s devices that any of the code in the application can be
compromised, intellectual property (IP) can be stolen, or an attacker can
learn about vulnerabilities in your application. If you really wish to retain
your IP, then you likely want to keep it on a server-side component and
have your application call it via a web API. That being said, there is still
serious value in making use of tools that obfuscate the compiled codebase
to make it more difficult for an attacker to decipher what the application
is doing.
Let’s take a look at a simple class and how it will look when decompiled
after obfuscation.

```Csharp
public class SomethingSecure
{
 private string PrivateSecret { get; } = "abc";
 internal string InternalSecret { get; } = "def";
 public string PublicSecret { get; } = "ghi";
}
```
The code decompiled using ILSpy without being obfuscated first looks
as follows:

```Csharp
using System;
public class SomethingSecure
{
 private string PrivateSecret { get; } = "abc";
 internal string InternalSecret { get; } = "def";
 public string PublicSecret { get; } = "ghi";
}
```
If you run the original code through an obfuscation tool and then
decompile the source, you will end up with something like the following:

```Csharp
// \u0008\u0002
using System;
[\u000f\u0002(1)]
[\u000e\u0002(0)]
public sealed class \u0008\u0002
{
 private readonly string m_\u0002 = \u0002\u0003.\
u0002(-815072442);
 private readonly string m_\u0003 = \u0002\u0003.\
u0002(-815072424);
 private readonly string m_\u0005 = \u0002\u0003.\
u0002(-815072430);
 private string \u0002()
 {
 return this.m_\u0002;
 }
 internal string \u0003()
 {
 return this.m_\u0003;
 }
 public string \u0005()
 {
 return this.m_\u0005;
 }
}
```
It is clear from the above that it is much more difficult now to follow
what this code is doing.

---
Obfuscation doesn’t make it impossible for attackers to gain an
understanding of what the code does. It does, however, make that
task much more difficult.
---

# Distributing Test Versions

There are a lot of different tools and websites that help you ship test builds
out to people who can test your application. I have become most fond of
using the deployment options provided by Apple and Google. The main
reason I prefer to do it this way is that you do not need to change any of
your deployment processes. You can continue to publish applications
ready for releasing to the public via each store. In fact, these processes
even upload the builds to the store portals. They simply allow you to
release the application to a subset of users.

As is in keeping with this chapter, I won’t walk you through each of
these portals because the details can change from time to time. I refer you
to the documentation provided by each platform provider and strongly
urge you to investigate.
• Apple TestFlight, https://testflight.apple.com
• Google Play Internal Testing, https://play.google.
com/console/about/internal-testing/

# Summary

In this chapter, you have

• Explored the concepts of distributing your application

• Learned about continuous integration and continuous
delivery to improve your development processes

• Learned about linking, what it is, and how it can
benefit/hinder you

• Covered why it is important to collect analytical and
crash information

• Explored why you may want to consider obfuscating
your code

• Reached the end of our application-building journey
together



























