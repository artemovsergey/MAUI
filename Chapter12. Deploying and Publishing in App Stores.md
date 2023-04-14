# Deploying and Publishing in App Stores

After we complete the development work, we want to publish our app in various app stores. Since
.NET MAUI is a cross-platform framework, we can build the same source code for Android, iOS,
macOS, and Windows. It is possible to deploy our app to a repository such as GitHub, but most users
of these platforms use app stores instead. We need to know how to prepare our app for different app
stores. This is the topic of this chapter. In this chapter, we will cover the preparation of the application
packages before publishing.

We will cover the following topics in this chapter:

* Preparing application packages for publishing

* Automating the build process using GitHub Actions

# Technical requirements

To test and debug the source code in this chapter, we need to install Visual Studio 2022 in both Windows
and macOS environments. Please refer to the Development environment setup section in Chapter 1,

Getting Started with .NET MAUI for the full details about environment setup.

We will build Windows and Android packages using Windows and build iOS and macOS packages
using macOS.

The source code of this chapter is available in the following GitHub repository:

https://github.com/PacktPublishing/.NET-MAUI-Cross-PlatformApplication-Development/tree/main/Chapter12

# Preparing application packages for publishing

In previous chapters, there is very little platform-specific knowledge required in the .NET MAUI
development. However, we cannot avoid platform-specific information when we prepare to publish
our app to individual app stores. In this chapter, we will introduce what we need to prepare the app
for publishing and then we will introduce how to automate the process using GitHub Actions.

# What to prepare for publishing

To prepare for publishing, we will focus on the work that we need to do before submitting the package
to an app store. After the packages are loaded into your chosen app store, please refer to the documents
in each app store about the actual publishing process.

In the preparation for publishing, we are trying to answer these questions:

* How to identify an app in app store

* How to identify the app developers

* Which devices can the app support

To build and sign application packages on different platforms, there is platform-specific configuration
involved. In .NET MAUI, the platform-specific information is included in the Visual Studio project file
and the platform-specific configuration file. In the Visual Studio project file, conditional compilation
is used to specify platform-specific information. We can refer to Table 12.1 for an overview of what
we need to change for each platform.

![image](https://user-images.githubusercontent.com/26972859/232075525-a24a9192-7dd4-41cc-a04b-20ae336af32d.png)

Table 12.1: The build configuration

In Table 12.1, to identify an app, the ApplicationId and ApplicationVersion variables
are defined in the Visual Studio project file. For each platform, there is a platform configuration file.

When distributing our app for Android, we generate a .apk file or a .aab file. The .apk file is
the original Android package format, which can be used to install an app package on the device or
emulator. The .aab file is used to submit the app to the Google Play Store. Before submission, we need
to sign the package using Keystore. ApplicationId and ApplicationVersion are mapped
to the package ID and version code in the Android configuration AndroidManifest.xml file.
When distributing our app for iOS or macOS, a .ipa file is generated for iOS. A .app or .pkg file
is generated for macOS. To sign an iOS or a macOS package, we need a distribution certificate and a
distribution profile. ApplicationId is mapped to the bundle id and ApplicationVersion
is mapped to the bundle version in Info.plist.
When distributing our app for Windows, the MSIX package format is used. We will build the package
in a .msix file extension. In Windows, a universally unique identifier (UUID) is used instead of
ApplicationId. This UUID is generated as ApplicationGuid as an application identifier.
ApplicationVersion is mapped to the Version attribute of the Identity tag in Package.
appxmanifest.

---
**What is MSIX?**

MSIX is a new Windows app package format that can be used for all Windows apps. Please
refer to the Microsoft documentation to find out more information:
https://learn.microsoft.com/en-us/windows/msix/overview
---

In the subsequent sections, we will introduce how to generate release packages on each platform. We
will build Windows and Android packages on Windows and, iOS and macOS packages on macOS.
We will try to do it using both Visual Studio and the command line.

# Publishing to Microsoft Store

We can build a .msix package for Microsoft Store using either Visual Studio or the command line
on Windows.

In Visual Studio, we need to set the target framework as net6.0-windows10.0.19041.0 and set the
build type to Release.

After that, we can right-click on the project node and select the Publish… menu item.

As we can see in Figure 12.1, a window will appear with Select distribution method on it. We can
select Microsoft Store under a new app name and click on the Next button.

![image](https://user-images.githubusercontent.com/26972859/232075939-2283e970-ffdc-419e-a7e8-71e68af1c49e.png)

Figure 12.1: Select distribution method

Before we move to the next step in Figure 12.2, we need an app name ready.

To create a new app name, we need to do it in Microsoft Store at the following URL:

http://developer.microsoft.com/dashboard

Once we have an app name, we can associate it with our app. You can see how to do this in Figure 12.2.

![image](https://user-images.githubusercontent.com/26972859/232076167-675bd80e-85ca-4b61-8893-1329717e337c.png)

Figure 12.2: Associating your app with Microsoft Store

After we click the Next button, Visual Studio will search for the app name in Microsoft Store for us.

The app name created in Microsoft Store is shown in Figure 12.3.

After we click the Next button, Visual Studio will search for the app name in Microsoft Store for us.
The app name created in Microsoft Store is shown in Figure 12.3.

![image](https://user-images.githubusercontent.com/26972859/232076381-9a4c8ed4-5a0e-4a83-9277-08fbf22ca981.png)

Figure 12.3: Selecting an app name

After selecting the app name, click on the Next button. A screen to select and configure packages will
appear, as shown in Figure 12.4.

![image](https://user-images.githubusercontent.com/26972859/232076523-c8a89b61-97a5-4c78-8531-6cfe8199928b.png)

Figure 12.4: Selecting and configuring packages

We can click on the drop-down menu under Publishing profile. A dialog will be shown, as we can
see in Figure 12.4. After clicking the OK button in the dialog box, it will create a new MSIX publish
profile. Once we have a publish profile, we can click on the Create button (which will now become
active) to create the package. It will take a while to finish the build and package creation. Once it
completes, we can see the following screen, as shown in Figure 12.5, and an MSIX package is now
ready to be submitted.

![image](https://user-images.githubusercontent.com/26972859/232076641-61d6026b-c27d-43c7-9f34-394cbd7ece5e.png)

Figure 12.5: An MSIX package

The location of the new package is shown in Figure 12.5. There is an option with which we can verify
the package by running Windows App Certification Kit.
In the preceding steps, two files (as shown here) relating to the publication of the app will have been
created in the project folder:

* Package.StoreAssociation.xml – this is a file to associate the app with Microsoft Store
* Properties\PublishProfiles\MSIX-win10-x86.pubxml – this is the publish profile

Both files may contain sensitive information so they should not be checked into the Git repository.
To integrate the build process in the CI/CD environment, we need to execute the build process using
the command line. To build a .msix package using the command line, we can execute the following
command from the project folder:

```
dotnet publish PassXYZ.Vault/PassXYZ.Vault.csproj -c Release -f
net6.0-windows10.0.19041.0
```
Once we build the .msix package, we can upload it to Microsoft Store in the Packages section of
the app submission.

# Publishing to the Google Play Store

To prepare for the submission in the Google Play Store, you need to create a new app in the Google
Play Console. To create a new app in the Google Play Console, you need a Google account.
To identify an app, every Android app has a unique application ID or package ID defined in the
configuration file AndroidManifest.xml. This configuration file is generated by Visual Studio
from the project file, and it can be found at Platforms/Android/AndroidManifest.xml.
Let’s review AndroidManifest.xml of our app in Listing 12.1:

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest
 xmlns:android="http://schemas.android.com/apk/res/android"
 package="com.passxyz.vault2" ❶
 android:installLocation="auto"
 android:versionCode="1"> ❷
 <application
android:allowBackup="true"
android:icon="@mipmap/appicon"
android:roundIcon="@mipmap/appicon_round"
android:supportsRtl="true"></application>
 <uses-permission
 android:name="android.permission.ACCESS_NETWORK_STATE" />
 <uses-permission android:name=
 "android.permission.INTERNET" />
</manifest>
```
**Listing 12.1: AndroidManifest.xml (https://epa.ms/AndroidManifest12-1)**

In our app, the application ID is "com.passxyz.vault2" ❶, which is generated from
ApplicationId, and the version is the value of android:versionCode ❷, which is generated
from ApplicationVersion.

Here is the declaration of the app identifier and version in the PassXYZ.Vault.csproj project file:

```xml
<!-- App Identifier -->
<ApplicationId>com.passxyz.vault2</ApplicationId>
<ApplicationIdGuid>8606B3B5-C03C-41D7-825F-B33718CF791C
 </ApplicationIdGuid>
<!-- Versions -->
<ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
<ApplicationVersion>1</ApplicationVersion>
```
To sign an Android package, we need to create a Keystore file. Please refer to the following Android
document for information on how to create a Keystore file and sign an Android app:
https://developer.android.com/studio/publish/app-signing
Once we have a Keystore file and have prepared the preceding configuration, we need to set the target
framework as net6.0-android and set the build type as Release in Visual Studio.
We can right-click now the project node and select Publish…. After that, the build will start and we
can see an archive has been created, as shown in Figure 12.6.

![image](https://user-images.githubusercontent.com/26972859/232077354-88dca61e-9c91-478c-9813-11eab963db6f.png)

Figure 12.6: Creating an archive for Android

Once the package is created, we can sign it by clicking the Distribute … button, as shown in Figure 12.6.

![image](https://user-images.githubusercontent.com/26972859/232077544-6ae2eca9-b6ec-4da6-90fc-338942ad36be.png)

Figure 12.7: Selecting a channel

Once we click the Distribute … button, we need to select a distribution channel, as shown in Figure 12.7.
It is possible to sign and submit the package by selecting Google Play, but we will select Ad Hoc to
sign it. We will submit the signed package to Google Play manually later.
Once we select Ad Hoc, we can see a different screen as shown in Figure 12.8.

![image](https://user-images.githubusercontent.com/26972859/232077646-ceddf1e5-cffd-48bf-92e6-3e17ca465c8a.png)

Figure 12.8: Signing Identity using a Keystore file

As shown in Figure 12.8, we can click the + button to add a Keystore file. After that, we can click the
Save As button to sign the package.
The signed .aab file can be submitted to the Google Play Store in the Google Play Console.
If you don’t have an existing Keystore file, you may want to follow the guide to creating a new one.
The default location of Keystore files is at %USERPROFILE%\AppData\Local\Xamarin\Mono for Android\Keystore\.
To create the package from the command line, we can execute the following command in the project folder:

```dotnet publish PassXYZ.Vault/PassXYZ.Vault.csproj -c Release -f net6.0-android```

To learn how to upload a signed Android App Bundle to the Google Play Store, please refer to the
following Android document:

https://developer.android.com/studio/publish/upload-bundle

# Publishing to Apple’s App Store

We can introduce the submission of an iOS or macOS app in the App Store together since they have
many similarities.

In iOS or macOS apps, bundler identifier and bundler version are used to identify an app. This
information is stored in the Info.plist configuration file. The bundler identifier is generated
from ApplicationId and the bundler version is generated from ApplicationVersion in
the Visual Studio project file.

To sign the package, we need a signing certificate and a provisioning profile. To create a signing
certificate and a provisioning profile, we can refer to the following document:
https://learn.microsoft.com/en-us/dotnet/maui/ios/deployment/provision
iOS apps can be distributed through the App Store only. The package for the submission is a file with
the .ipa extension. macOS apps can also be distributed through the App Store, but the package
itself can be installed directly.

Even though we can do some of the publishing steps in the Windows environment, we still need
to connect to a network-accessible macOS computer. To reduce the complexity, we use a macOS
environment for the build of both iOS and macOS apps. Before we build the packages, we need to
update the Visual Studio project file to configure our own signing certificate and distribution profile:

```
<PropertyGroup Condition="$(TargetFramework.Contains('-ios'))
and '$(Configuration)' == 'Release'">
 <RuntimeIdentifier>ios-arm64</RuntimeIdentifier>
 <CodesignKey>iPhone Distribution: Shugao Ye (W9WL9WPD24)
 </CodesignKey>
 <CodesignProvision>passxyz_2022</CodesignProvision>
</PropertyGroup>
<PropertyGroup Condition="$(TargetFramework.Contains('-
 maccatalyst')) and '$(Configuration)' == 'Release'">
 <CodesignEntitlement>Entitlements.plist
 </CodesignEntitlement>
 <CodesignKey>
 3rd Party Mac Developer Application: Shugao Ye
 (W9WL9WPD24)
 </CodesignKey>
 <CodesignProvision>passxyz.maccatalyst</CodesignProvision>
</PropertyGroup>
```
As we can see, we can use conditional configuration for both iOS and macOS builds. Different signing
certificates and distribution profiles are used for iOS and macOS.

In Visual Studio for macOS, we can also configure signing certificates and distribution profiles in
project settings for iOS, as shown in Figure 12.9. This setting is not supported for macOS yet at the
moment, but you may find it is already available when you read this book.

![image](https://user-images.githubusercontent.com/26972859/232078124-e30243ac-9c79-4abf-85f3-3ac4a335bd5f.png)

Figure 12.9: The configuration bundle signing

If we are not sure whether the setting is correct or not, we can verify it using Xcode. We can create
an app in Xcode using the same "com.passxyz.vault2" bundler ID as our app. After that, we
can check the configuration of Signing, as shown in Figure 12.10.

![image](https://user-images.githubusercontent.com/26972859/232079686-0735b208-9a3d-488f-8f97-a22d158f11b5.png)

Figure 12.10: The iOS signing settings in Xcode

If there is any issue with the signing certificate or provisioning profile, we can see error messages
reported by Xcode. Once the setting is correct in Xcode, the same setting can be used in Visual Studio
project without any issues.
With all the configurations ready, we can build the .ipa file in the project folder using the
following command:

```
dotnet publish PassXYZ.Vault/PassXYZ.Vault.csproj -c Release -f net6.0-ios /p:CreatePackage=true /p:ArchiveOnBuild=True
```
Once the preceding command executes successfully, an .ipa file is generated. We can submit this
file to the App Store. There are three methods that can be used to upload a package to the App Store.
Please refer to the following document to find out more details:

https://help.apple.com/app-store-connect/#/devb1c185036

From the preceding document, we know that we can use Xcode, altool, or Transporter to upload
a package.

We will use the Transporter app here. After we sign in using the Transporter app, we can upload the package to the App Store, as shown in Figure 12.11.

![image](https://user-images.githubusercontent.com/26972859/232079996-39266ced-4bba-464d-95a4-48189315f80f.png)

Figure 12.11: Uploading a package using the Transporter app

The building and uploading processes of a macOS package are similar to those for an iOS app. There
are three different frameworks (AppKit, MacCatalyst, and SwiftUI) that can be used to build macOS
apps. In .NET MAUI, MacCatalyst is used in the platform-specific implementation.
By default, App Sandbox is not enabled in MacCatalyst apps, so we need to enable it. To enable it in
the macOS app, we need to add an Entitlements.plist file in the build configuration. We can
review the Entitlements.plist file in Listing 12.2:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN"
 "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
 <key>com.apple.security.app-sandbox</key>
 <true/>
 <key>com.apple.security.files.user-selected.read-only
 </key>
 <true/>
 <key>com.apple.security.network.client</key>
 <true/>
</dict>
</plist>
```
**Listing 12.2: Entitlements.plist (https://epa.ms/Entitlements12-2)**

We cannot verify the configuration of the signing certificate and provisioning profile in Visual Studio
for macOS at the moment, but we can verify it in Xcode, as shown in Figure 12.12.

![image](https://user-images.githubusercontent.com/26972859/232080280-13dcd9ef-309a-40c2-a71c-3ecfabd92008.png)

Figure 12.12: The macOS app’s Signing settings in Xcode

With all configurations ready, we can build the package in our project folder using the following command:

```
dotnet publish PassXYZ.Vault/PassXYZ.Vault.csproj -c Release -f net6.0-maccatalyst /p:CreatePackage=true /p:EnablePackageSigning=true"
```
After we build the package successfully, we can upload the .pkg file to the App Store using the
Transporter app, as shown in Figure 12.13. We can see that we have uploaded both iOS and macOS
packages to the App Store successfully.

![image](https://user-images.githubusercontent.com/26972859/232080500-432397c4-7f71-4ad4-810d-090fec25b0c3.png)

Figure 12.13: Uploading the macOS app using the Transporter app

After we have uploaded packages to Microsoft Store, the Google Play Store, and the App Store, we
can test the uploaded packages before the final release using the testing tools provided by the stores:

* The App Store – TestFlight can be used to test iOS/macOS apps before the production release

* The Google Play Store – Alpha or Beta testing can be set up before the production release

* Microsoft Store – Package flights can be used on Microsoft Store to test uploaded packages

We have learned the basic steps of how to prepare application packages for supported platforms. With
all this in mind, we can explore how to set up the automated build of .NET MAUI app in a continuous
integration and continuous delivery (CI/CD) environment such as GitHub Actions or Azure DevOps.

# GitHub Actions

Since our source code is hosted in GitHub, we will use GitHub Actions as an example to introduce
how to set up CI workflows for .NET MAUI development.

# Understanding GitHub Actions

GitHub Actions is a CI/CD platform that can be used to support the automation of deployment. For
.NET MAUI app development, our target is to build, test, and deploy our apps to app stores or specified
publishing channels. In this section, we will focus on CI using GitHub Actions rather than both CI
and CD. To deploy apps to various stores, there are many account-specific setup steps, please refer to
the .NET MAUI document for the details:

https://learn.microsoft.com/en-us/dotnet/maui/deployment/

The GitHub Actions workflow is a process to automatically build and deploy the deliverables from a
project. The workflow usually starts from an event such as a push or pull_request event or when
an issue is submitted. Once a workflow is triggered, the defined jobs will start to perform certain tasks
inside a runner. Each job consists of one or more steps that either run a script or an action.
In summary, GitHub Actions include events, runners, jobs/steps, actions, and runners.

# Workflows

GitHub Actions workflows are defined by a YAML file in the .github/workflows directory.
YAML is a superset of JSON and it is a better human-readable language. A repository can have one or
multiple workflows to perform different jobs. Take a look at Figure 12.14 to understand the workflow
defined in the PassXYZ.Vault project.

![image](https://user-images.githubusercontent.com/26972859/232080767-09b3eda2-f7de-4987-8ef2-58012e38f4f2.png)

Figure 12.14: The workflow of Windows runner

As we can see in Figure 12.14, this is an example of how workflow performs Android and Windows
builds. A workflow is triggered by a push or pull_request event, or manually. It runs inside a
Windows runner to perform the builds. When the workflow is triggered, two jobs, Android Build
and Windows Build, will be executed. Each job includes four steps to perform the build, as seen in
Figure 12.14.

In our project, we defined the two following workflows:

* passxyz-ci-macos.yml – This is a workflow to build iOS and macOS on a macOS runner

* passxyz-ci-windows.yml – This is a workflow to build Android and Windows on a
Windows runner

We can see the YAML files in Listing 12.3 and Listing 12.4:

```yaml
name: PassXYZ.Vault CI Build (Windows)
on: ①
 push: ②
 branches: [ master ]
 paths-ignore:
 - '**/*.md'
 - '**/*.gitignore'
 - '**/*.gitattributes'
 pull_request: ③
 branches: [ master ]
 workflow_dispatch: ④
permissions:
 contents: read
env:
 DOTNET_NOLOGO: true
 DOTNET_SKIP_FIRST_TIME_EXPERIENCE: true
 DOTNET_CLI_TELEMETRY_OPTOUT: true
 DOTNETVERSION: 6.0.400
 PROJECT_NAME: PassXYZ.Vault
jobs: ⑤
# MAUI Android Build
 build-android: ⑥
 runs-on: windows-2022 ⑦
 name: Android Build
 steps: ⑧
 - name: Checkout
 uses: actions/checkout@v3 ⑨
 - name: Restore Dependencies
 run: dotnet restore ${{env.PROJECT_NAME}}/$
 {{env.PROJECT_NAME}}.csproj
 - name: Build MAUI Android
 run: dotnet publish ${{env.PROJECT_NAME}}/$
 {{env.PROJECT_NAME}}.csproj -c Release -f net6.0-
 android --no-restore
 - name: Upload Android Artifact
 uses: actions/upload-artifact@v3
 with:
 name: passxyz-android-ci-build
 path: ${{env.PROJECT_NAME}}/bin/Release/net6.0-
 android/*Signed.a*
# MAUI Windows Build
 build-windows:
 runs-on: windows-2022
 name: Windows Build
 steps:
 - name: Checkout
 uses: actions/checkout@v3
 - name: Restore Dependencies
 run: dotnet restore ${{env.PROJECT_NAME}}
 /${{env.PROJECT_NAME}}.csproj
 - name: Build MAUI Windows
 run: dotnet publish ${{env.PROJECT_NAME}}/$
 {{env.PROJECT_NAME}}.csproj -c Release -f net6.0-
 windows10.0.19041.0 --no-restore
 - name: Upload Windows Artifact
 uses: actions/upload-artifact@v3
 with:
 name: passxyz-windows-ci-build
 path: ${{env.PROJECT_NAME}}/...

```
**Listing 12.3: passxyz-ci-windows.yml (https://epa.ms/passxyz-ci-windows12-3)**

The YAML file in Listing 12.3 is a little long, but it explains what needs to be set up in a workflow. We
will now analyze it step by step.

# Events

The workflow is triggered by events that are defined after the on: keyword ①. In the preceding
workflow, we defined the push ②, pull_request ③ and workflow_dispatch ④ events.
For both push and pull_request, we monitor the events on the master branch. We also ignore
no build related commits such as markdown files or configuration files. Please refer to the following
GitHub documentation about events that can be used to trigger workflows for more information:
https://docs.github.com/en/actions/using-workflows/events-thattrigger-workflows

# Jobs

When a workflow is triggered, it starts to execute the defined jobs. Jobs are defined after the jobs:
⑤ keyword. One or more jobs can be defined in a workflow. They are identified by a job ID, such as
build-android ⑥. There are two jobs, build-android and build-windows, defined in
Listing 12.3. Each job can define a name, a runner, and multiple steps.

# Runners

A runner is the type of platform that runs the job. In our configuration, both Android and Windows
jobs are executed using Windows runners. The runner is defined after the runs-on: ⑦ keyword.
Please refer to GitHub Actions documentation about the configuration of runners. The runner that we
use is windows-2022, which is the label of the runner image. In the configuration of the windows2022 image, Visual Studio 2022 and .NET MAUI are pre-installed so we can run the build without
the installation of any dependencies. However, in the passxyz-ci-macos.yml workflow, we
need to install .NET MAUI first before we can start the build.

# Steps

Multiple steps can be defined in a job and they are defined after the steps: ⑧ keyword. In both
Android and Windows builds, there are four steps: checkout, restore dependencies, build, and upload.
Each step can run a script or an action. In the checkout step, a checkout action is used after the
uses: ⑨ keyword. An action is a custom application in the GitHub Actions platform to perform a
complex but frequent repeated task. Using actions, we can reuse code such as a component in objectoriented programming. To use it, we can just specify the action name with an optional version number.
In our script, we can specify the checkout action as actions/checkout@v3.

The source code of the checkout action is hosted on GitHub and can be found at the following site:

https://github.com/actions/checkout

In the restore and build steps, we can just run the following dotnet command directly after the
run: syntax:

```dotnet restore ${{env.PROJECT_NAME}}/${{env.PROJECT_NAME}}.csproj```

After the build is completed, we can upload the artifact using another upload-artifact action.
We have introduced the passxyz-ci-windows.yml workflow, which performs Android and
Windows builds. Let us review the passxyz-ci-macos.yml workflow, which performs iOS and
macOS builds, in Listing 12.4:

```yaml
name: PassXYZ.Vault CI Build (MacOS)
on:
 push:
 branches: [ master ]
 paths-ignore:
 - '**/*.md'
 - '**/*.gitignore'
 - '**/*.gitattributes'
 pull_request:
 branches: [ master ]
 workflow_dispatch:
permissions:
 contents: read
env:
 DOTNET_NOLOGO: true
 DOTNET_SKIP_FIRST_TIME_EXPERIENCE: true
 DOTNET_CLI_TELEMETRY_OPTOUT: true
 DOTNETVERSION: 6.0.400
 PROJECT_NAME: PassXYZ.Vault
jobs:
# MAUI iOS Build
 build-ios:
 runs-on: macos-12 ❶
 name: iOS Build
steps:
 - name: Checkout
 uses: actions/checkout@v3
 - name: Install .NET MAUI ❷
 shell: pwsh
 run: |
 & dotnet nuget locals all --clear
 & dotnet workload install maui --source
 https://aka.ms/dotnet6/nuget/index.json --source
 https://api.nuget.org/v3/index.json
 & dotnet workload install android ios maccatalyst
tvos macos maui wasm-tools maui-maccatalyst --source
 https://aka.ms/dotnet6/nuget/index.json --source
 https://api.nuget.org/v3/index.json
 - name: Restore Dependencies
 run: dotnet restore ${{env.PROJECT_NAME}}
 /${{env.PROJECT_NAME}}.csproj
 - name: Build MAUI iOS
 run: dotnet build ${{env.PROJECT_NAME}}
 /${{env.PROJECT_NAME}}.csproj -c Release -f
 net6.0-ios --no-restore /p:buildForSimulator=True
 /p:packageApp=True /p:ArchiveOnBuild=False
 - name: Upload iOS Artifact
 uses: actions/upload-artifact@v3
 with:
 name: passxyz-ios-ci-build
 path: ${{env.PROJECT_NAME}}/bin/Release/net6.0-
 ios/iossimulator-x64/**/*.app
# MAUI MacCatalyst Build
 build-mac:
 runs-on: macos-12
 name: MacCatalyst Build
 steps:
 - name: Checkout
uses: actions/checkout@v3
 - name: Install .NET MAUI
 shell: pwsh
 run: |
 & dotnet nuget locals all --clear
 & dotnet workload install maui --source
 https://aka.ms/dotnet6/nuget/index.json --source
 https://api.nuget.org/v3/index.json
 & dotnet workload install android ios maccatalyst
 tvos macos maui wasm-tools maui-maccatalyst --source
 https://aka.ms/dotnet6/nuget/index.json --source
 https://api.nuget.org/v3/index.json
 - name: Restore Dependencies
 run: dotnet restore ${{env.PROJECT_NAME}}
 /${{env.PROJECT_NAME}}.csproj
 - name: Build MAUI MacCatalyst
 run: dotnet publish ${{env.PROJECT_NAME}}
 /${{env.PROJECT_NAME}}.csproj -c Release -f
 net6.0-maccatalyst --no-restore -p:BuildIpa=True
 - name: Upload MacCatalyst Artifact
 uses: actions/upload-artifact@v3
 with:
 name: passxyz-macos-ci-build
 path: ${{env.PROJECT_NAME}}/bin/Release/net6.0-
 maccatalyst/maccatalyst-x64/publish/*.pkg
```
Listing 12.4: passxyz-ci-macos.yml (https://epa.ms/passxyz-ci-macos12-4)

The workflow of the iOS and macOS build is similar to the workflow of the Android and Windows build.
The difference is that the macos-12 ❶ runner is used here. Visual Studio for macOS is pre-installed
in this runner, but .NET MAUI is not installed at the moment. We need to add an extra step to install
.NET MAUI ❷ before the build. The rest of the steps are similar to a Windows or Android build.

We introduced the configuration of all builds in GitHub Actions. Let us check the build status on GitHub.

![image](https://user-images.githubusercontent.com/26972859/232082024-ce2a0909-c6cf-43d0-85db-00bf2d328704.png)

Figure 12.15: The Android and Windows build status

From Figure 12.15, we can see that both Android and Windows builds are completed successfully.
The build artifacts can be downloaded from GitHub after the build.

![image](https://user-images.githubusercontent.com/26972859/232082412-b59843fb-7b74-4518-a8bd-0f29684c4b74.png)

Figure 12.16: The iOS and MacCatalyst build status

In Figure 12.16, we can see that both iOS and MacCatalyst builds are completed successfully.
With the successful builds in GitHub Actions, we have concluded the introduction of packaging our
app for app store submission and automating build using GitHub Actions.

# Summary

CI/CD are common practices in today’s development process. In this chapter, we introduced how
to prepare the build and submit packages for various app stores. The process after the submission of
build packages is not covered since they are platform and account-specific topics.

After we introduce the build process of each platform, we can automate the process in GitHub Actions.
In the second part of this chapter, we introduced how to set up the build process in GitHub Actions.

With all the skills that you learned from this book, you should be able to develop your own .NET
MAUI applications and be ready to submit your apps to supported app stores now.

































































