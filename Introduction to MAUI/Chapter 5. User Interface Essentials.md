# CHAPTER 5

# User Interface Essentials

In this chapter, you are going to investigate the fundamental parts of
building a .NET MAUI application. You are going to apply an icon and
splash screen, add in some pages and their associated view models, and
configure some bindings between your page and the view model. You will
also gain an understanding of what XAML is and what it has to offer as you
build the pages and the Shell of your application. You will also learn how
Shell allows you to navigate between pages in your application.

# Prerequisites

You need to do some setup before you can jump into using Shell. If Shell is
still feeling like an unknown concept, fear not. I will be covering it a little
bit later in this chapter under the “Shell” section.
Let’s go ahead and add the following folders to your project.

# Models

This will house all of your Model classes. If you recall from Chapter 4, these
are where some of your business logic is located. In your Models folder, you
need to create three classes.

# BaseLayout.cs

This will serve as a base class for the layout options you provide. During
this book you will only be building fixed layout boards, but I wanted to
lay some groundwork so if you are feeling adventurous you can go off
and build alternative layout options without having to restructure the
application. In fact, I would love to hear where you take it!

```Csharp
namespace WidgetBoard.Models;
public abstract class BaseLayout
{
}
```

# FixedLayout.cs

This will represent the fixed layout, as I mentioned in the previous section.
Your fixed layout will offer the user of the app the ability to choose a
number of rows and columns and then position their widgets in them.

```Csharp
namespace WidgetBoard.Models;
public class FixedLayout : BaseLayout
{
 public int NumberOfColumns { get; init; }
 public int NumberOfRows { get; init; }
}
```

# Board.cs

Your final model represents the overall board.

```Csharp
namespace WidgetBoard.Models;
public class Board
{
 public string Name { get; init; }
 public BaseLayout Layout { get; init; }
}  
```

# Pages

This will house the pages in your application. I am distinguishing between
a page and a view because they do behave differently in .NET MAUI. You
can think of a page as a screen that you are seeing whereas a view is a
smaller component. A page can contain multiple views.
Let’s go ahead and create the following files under the Pages folder.
The following steps show how to add the new pages.

Right-click the Pages folder.
• Select Add ➤ New Item.
• Select the .NET MAUI tab.
• Select .NET MAUI ContentPage (XAML).
• Click Add.

## BoardDetailsPage

This is the page that lets you both create and edit your boards. For now,
you will not touch the contents of this file. Note that you should see
BoardDetailsPage.xaml and BoardDetailsPage.xaml.cs files created.
You also need to jump over to the MauiProgram.cs file and register this
page with the Services inside the CreateMauiApp method.

```Csharp
builder.Services.AddTransient<BoardDetailsPage>();
```
# FixedBoardPage

This is the page that will render the boards you create in the previous page.
For now, you will not touch the contents of this file. Note that you should
see FixedBoardPage.xaml and FixedBoardPage.xaml.cs files created.
You will also need to jump over to the MauiProgram.cs file and register
this page with the Services inside the CreateMauiApp method.

```builder.Services.AddTransient<FixedBoardPage>();```

# ViewModels

This houses your ViewModels that are the backing for both your Pages and
Views. You created this folder in the previous chapter, but you need to add a
number of classes. The following steps show how to add the new pages.
• Right-click the ViewModels folder.
• Select Add ➤ New Class.
• Click Add.

# AppShellViewModel

This serves as the view model for the AppShell file that is created for you
by the tooling.
namespace WidgetBoard.ViewModels;
public class AppShellViewModel : BaseViewModel
{
}

You also need to jump over to the MauiProgram.cs file and register this
page with the Services inside the CreateMauiApp method.

```Csharp
builder.Services.AddTransient<AppShellViewModel>();
```

# BoardDetailsPageViewModel

This serves as the view model for the BoardDetailsPage file you created.

```Csharp
namespace WidgetBoard.ViewModels;
public class BoardDetailsPageViewModel : BaseViewModel
{
}
```
You also need to jump over to the MauiProgram.cs file and register this
page with the Services inside the CreateMauiApp method.

```Csharp
builder.Services.AddTransient<BoardDetailsPageViewModel>();
```

# FixedBoardPageViewModel

This serves as the view model for the FixedBoardPage file you created.

```Csharp
namespace WidgetBoard.ViewModels;
public class FixedBoardPageViewModel : BaseViewModel
{
}
```

You also need to jump over to the MauiProgram.cs file and register this
page with the Services inside the CreateMauiApp method.

```builder.Services.AddTransient<FixedBoardPageViewModel>();```

---
You should have noticed a common pattern with the creation of these
files and the need to add them to the MauiProgram.cs file. This is
to allow you to fully utilize the dependency injection provided by the
framework, which you learned about in Chapter 3.
---

# App Icons

Every application needs an icon, and for many people this will be how they
obtain their first impression. Thankfully these days device screens allow
for bigger icon sizes and therefore more detail to be included in them.
As with general image resources, each platform requires different sizes
and many more combinations to be provided. For example, iOS expects
the following:
• Five different sizes of the app icon
• Three different sizes for the Spotlight feature
• Three different sizes for Notifications
• Three different sizes for Settings
That’s up to 14 different image sizes required just for your application
icon on iOS alone. See https://developer.apple.com/design/humaninterface-guidelines/ios/icons-and-images/app-icon/.
.NET MAUI manages the process of generating all the required images
for you. All you need to do is provide an SVG image file. Since SVGs are
vector-based, they can scale to each required size.

## Adding Your Own Icon

Figure 5-1 shows the icon that you will be using for your application. You
can grab a copy of the files that you will be using from https://github.
com/bijington/introducing-dotnet-maui/tree/main/chapter05 and
place them in the Resources/AppIcon folder. You should notice that they
replace two existing files.

![image](https://user-images.githubusercontent.com/26972859/231117066-af76bc3f-63d8-4fcd-84cb-0bed1613b9e7.png)
Figure 5-1. Your application icon

If you look in the contents of your project file, you will see the
following entry:

```<MauiIcon Include="Resources\AppIcon\appicon.svg" />```

This tells the tooling to use the file appicon.svg and convert it into all
the required sizes for each platform when building. Note you only want
one MauiIcon in your project file. If you have multiple, the first one will
be used.
You do not need to replace the above entry as the file you should
have downloaded should have the name appicon.svg. If the file name is
different, either rename it or update the name in the project file.

# Platform Differences

It is worth noting that some platforms apply different rules to app icons
and also can provide rather different outputs.

## Android

App icons on Android can take many different shapes due to the different
device manufacturers and their own flavor of the Android operating
system. To cater for this, Google introduced the adaptive icon. This allows
a developer to define two layers in their icon:
• The background: This is typically a single color or
consistent pattern.
• The foreground: This includes the main detail.
.NET MAUI allows you to support the adaptive icon using the
IncludeFile and the ForegroundFile properties on the MauiIcon
element. You can see the IncludeFile is already defined in your project.
This represents the background. You can split your application icon into
two parts and then provide the detail to the ForegroundFile. Note that this
can be applied to all platforms and is my recommended way to ship an
application icon.

## iOS and macOS

Apple does not allow for any transparency in an app icon. You can either
make sure that you supply an image with no transparent pixels or you can
use the Color property on the MauiIcon element, which will fill in any
transparent pixels with that defined color.

# Splash Screen

A splash screen is the first thing a user sees when they start your
application. It gives you as a developer a way of showing the user
something while the application is launching. Once everything has
finished loading, the splash screen will be hidden and your main page will
be shown.

In a similar manner to how the app icon is managed, the splash screen
also has an entry in the project file and can generate a screen based on an
SVG file. In fact, you will be using the same image to save effort.

```xml
<MauiSplashScreen Include="Resources\Splash\splash.svg"
Color="#512BD4" BaseSize="128,128" />
```
Note that splash screens built in this manner must be static. You can’t
have any animations running to show progress.
The Color property enables you to define a background color for the
splash screen.

# XAML

As a .NET MAUI developer, you will hear XAML be mentioned many times,
XAML stands for eXtensible Application Markup Language. It is an XMLbased language used for defining user interfaces. It originates from WPF
and Silverlight, but the .NET MAUI version has its differences.
There are two different types of XAML files that you will encounter
when building your application:
• A ResourceDictionary: This is a single file that
contains resources that can easily be used throughout
your application. Resources/Styles/Styles.xaml
is a perfect example of this. The Styles.xaml file is a
default set of styles that is provided when you create
a new .NET MAUI application. If you wish to modify
some in-built styling, this is a very good place to do so.
• A View-based file: This contains both a .xaml and
.xaml.cs file. They are paired together using the
partial class keyword.

---
When dealing with this second item, you have to make sure that
the InitializeComponent line is called inside the constructor;
otherwise the XAML will not be interpreted correctly, and you will see
an exception thrown.
---

It is worth noting that XAML does not provide a rich set of features like
C# does and for this reason there is almost always a xaml.cs file that goes
alongside the XAML file. This C# file provides the ability to use the richfeature set of the C# language when XAML does not. For example, handling
a button interaction event would have to be done within the C# code file.

## Dissecting a XAML File

In the prerequisites section of this chapter, you created the
BoardDetailsPage.xaml file. Now you are going to modify it and add some
meaningful content so you can start to see your application take shape.
The code you should see in this file is shown below.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/
dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 x:Class="WidgetBoard.Pages.BoardDetailsPage"
 Title="BoardDetailsPage">
 <VerticalStackLayout>
 <Label
 Text="Welcome to .NET MAUI!"
 VerticalOptions="Center"
 HorizontalOptions="Center" />
 </VerticalStackLayout>
</ContentPage>
```

If you break this down into small chunks, you can start to understand
not only what makes up the UI of your application but also some of the
fundamentals of how XAML represents it.
The root element is a ContentPage. As mentioned, a typical view in
.NET MAUI is either a ContentPage or ContentView. As the name implies,
it is a page that presents its content, and this will be a single view as its
content.
As mentioned, XAML is an XML-based language and there are the
following key parts to understanding XAML:
1. Properties are set by attributes on your element, so

```<Label Text="Welcome to .NET MAUI!" />```
is effectively the same as writing
```
new Label
{
 Text ="Welcome to .NET MAUI!"
};
```

2. XAML represents the visual hierarchy in the file
structure. You can work out that ContentPage has
a child of VerticalStackLayout and it has a child
of Label. This can be especially helpful. A complex
XAML file will result in a complex visual tree and
you want to try your best to avoid this.
3. The xmlns tag works like a using statement in C#. This
allows you to refer to other functionality that might not
be available out of the box. For example, you can add
the line xmlns:views="clr-namespace:WidgetBoard.
Views" and it is the equivalent of adding using
WidgetBoard.Views; in a C# file. This allows you to
refer to the views in your codebase.

The content of your ContentPage in your XAML is a
VerticalStackLayout. I will cover layouts a little bit later in this chapter
but as a very brief overview they allow you to have multiple child views as
content and therefore open up the possibilities of creating your UIs. It is
worth noting that a ContentPage can only have a single child, which makes
layouts really important controls for use when building user interfaces.
Now that you have covered some of the key concepts around XAML,
let’s go ahead and start building your application’s first page.

# Building Your First XAML Page

I always like to work with a clear definition of what needs to be achieved so
let’s define what your page needs to do. It needs to do the following:
• Allow the user to create a new board.
• Fit on a variety of screen sizes.
• Allow the user to provide a name for the board.
• Allow the user to choose the layout type.
• Apply any valid properties for the specific layout
type chosen.
Now that you know what needs to be achieved, let’s go ahead and do
it. You need to delete the existing contents of the page and replace them
with a Border. A Border is similar to a ContentView in that it can only have
a single child, but it offers you some extra properties that allow you to
provide a nice looking UI. In particular, you care about the StrokeShape
and Stroke properties. You may notice that you are not actually setting
these properties in the XAML and you would be correct! There are two
main reasons for this:

You have suitable defaults defined in the Resources/
Styles/Styles.xaml file that was created for you. Note
that if you want to override these, it’s perfectly fine. I
will be covering this a little bit later in this chapter in
the “Styling” section.
• It is considered good practice to only define the
properties that you need to supply, which is basically
anything that changes from the defaults. While the
XAML compiler does a decent job of generating a
UI that is defined at compile time, some bits are still
potentially interpreted at runtime and this has a
performance impact.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
 xmlns="http://schemas.microsoft.com/
dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/
winfx/2009/xaml"
 x:Class="WidgetBoard.Pages.BoardDetailsPage">
 <Border
 MinimumWidthRequest="300"
 HorizontalOptions="Center"
 VerticalOptions="Center"
 Padding="0">
 </Border>
</ContentPage>
```

The most important part of the properties that you are setting are the
HorizontalOptions and VerticalOptions. They allow you to define where
in the parent this view will be displayed. By default, a view will Fill its

parents content, but you are going to make it float in the Center. The main
reason is so it will stay there regardless of the screen size it is running on.
Of course, there are more in-depth ways of handling different screen sizes
and you will explore them in the coming chapters.
While you have much more content to add to this XAML, file you
are going to do so in the context of the following topics. Your next step is
to add multiple child views. For this, you are going to need to choose a
suitable Layout.

# Layouts

.NET MAUI provides you with a set of prebuilt layout classes that allow you
to group and arrange views in your application. The aim of this section is to
explore each layout control and how it might be used for your application.
I strongly recommend playing around with each of the layouts to see what
will fit best for each individual use case and always remember to keep the
visual tree as simple as possible.

## AbsoluteLayout

As the name suggests, the AbsoluteLayout allows the positioning of its
children with absolute values. The x, y, width, and height of a child is
controlled through the LayoutBounds attached property. This means you
use as follows:

```xml
<AbsoluteLayout>
 <Label
 AbsoluteLayout.LayoutBounds="0,0,600,200"/>
</AbsoluteLayout>
```

Figure 5-2 shows how a control is positioned inside an
AbsoluteLayout.

![image](https://user-images.githubusercontent.com/26972859/231118592-0177bb26-72bc-4ac1-a2f1-d0bee584bb69.png)
Figure 5-2. Absolute layout overview

There is also the option to define layout bounds that are
proportional to the AbsoluteLayout itself. You can control this with the
AbsoluteLayout.LayoutFlags attached property

```Csharp
<AbsoluteLayout>
 <Label
 AbsoluteLayout.LayoutBounds="0,0,0.5,0.2"
 AbsoluteLayout.LayoutFlags="All"/>
</AbsoluteLayout>
```

This will result in the Label being positioned at 0,0 but the width will
be 50% of the AbsoluteLayout and the height will be 20%,. This provides
a lot of power when defining a user interface that can grow as the size of a
device also increases.
The LayoutFlags option provides you with a lot of power. You can
choose which part of the LayoutBounds are applied absolutely and which
are applied proportionally. Here are the possible values for LayoutFlags
and what they impact:

|Value|Description|
|:--|:--|
|None|All values are absolute.|
|XProportional|The X property is proportional to the AbsoluteLayout dimensions.|
|YProportional|The Y property is proportional to the AbsoluteLayout dimensions|
|WidthProportional|The Width property is proportional to the AbsoluteLayout dimensions.|
|HeightProportional|The Height property is proportional to the AbsoluteLayout dimensions.|
|PositionProportional|The X and Y properties are proportional to the AbsoluteLayout dimensions.|
|SizeProportional|The Width and Height properties are proportional to the AbsoluteLayout dimensions.|
|All|All properties are proportional to the AbsoluteLayout dimensions|

The AbsoluteLayout can be an incredibly powerful layout when used
in the right scenario. For your scenario, it offers more complexities than I
really think you need to handle.

# FlexLayout

The FlexLayout comes with a large number of properties to configure
how its children are positioned. If you want your controls to wrap, this is
the control for you! A good example for using the FlexLayout is a media
gallery.
Figure 5-3 shows how controls can be positioned inside a FlexLayout.

![image](https://user-images.githubusercontent.com/26972859/231120286-f2d319da-8575-4241-8f2f-7a2f00618c60.png)
Figure 5-3. FlexLayout overview

The above layout can be achieved with the following code example:

```xml
<FlexLayout
 AlignItems="Start"
 Wrap="Wrap"
 Margin="30"
 JustifyContent="SpaceEvenly">
 <Border
 BackgroundColor="LightGray"
 WidthRequest="100"
 HeightRequest="100" />
 <Border
 BackgroundColor="LightGray"
 WidthRequest="100"
 HeightRequest="100" />
 <Border
 BackgroundColor="LightGray"
 WidthRequest="100"
 HeightRequest="100" />
  <Border
 BackgroundColor="LightGray"
 WidthRequest="100"
 HeightRequest="100" />
</FlexLayout>

```
Each of the properties you are using allows you to customize where
each item is positioned during the rendering process and how they will
move around in the application if it is resized. For further information
on the possible ways of configuring the FlexLayout, read the Microsoft
documentation at https://learn.microsoft.com/dotnet/maui/userinterface/layouts/flexlayout.
Your BoardDetailsPage only needs controls positioned vertically so a
FlexLayout feels like an overly complicated layout for this purpose.

# Grid

I love Grids. They are usually my go-to layout option, mainly because
I have become used to thinking about how they lay out controls and
because they tend to allow you to keep your visual tree depth shallow.
The layout essentially works by allowing you to define a set of rows and
columns and then define which control should be displayed in which row/
column combination.
Figure 5-4 shows how controls can be positioned inside a Grid.

![image](https://user-images.githubusercontent.com/26972859/231120630-3dfe23e1-5573-4d5f-b3f3-de708c5b260e.png)
Figure 5-4. Grid layout overview

Controls inside a Grid are allowed to overlay each other, which can
provide an extra tool in a developers toolbelt when needing to show/
hide controls. Controls in the Grid are arranged by first defining the
ColumnDefinitions and RowDefinitions. Let’s take a look at how to create
the above layout with a Grid.

```xml
<Grid
 ColumnDefinitions ="*,2*,250,Auto"
 ColumnSpacing="20"
 Margin="30"
 RowDefinitions="*,*"
 RowSpacing="20">
 <Border
 BackgroundColor="LightGray"
 Grid.Column="0"
 Grid.Row="0" />
 <Border
         BackgroundColor="LightGray"
 Grid.Column="1"
 Grid.Row="1" />
 <Border
 BackgroundColor="LightGray"
 Grid.Column="2"
 Grid.Row="0" />
 <Border
 BackgroundColor="LightGray"
 Grid.Column="3"
 Grid.Row="1"
 WidthRequest="30"
 HeightRequest="30" />
</Grid>
```

You can see that you have created columns using a variety of different
options:
• 250: This is a fixed width of 250
• Auto: This means that the column will grow in width
based on its contents. It is recommended to use this
option sparingly as it will result in the Grid control
having to measure its children and force a rerender of
itself and the other children
• *: This is proportional and will result in the leftover
space being allocated out. In this example, two
columns use the * notation. This results in those two
columns being allocated 1/3 and 2/3 of the remaining
width respectively. This is because * is actually
considered 1*.

In your scenario, you are going to need multiple groups of controls.
For this reason, I believe grids will just make it slightly more complicated
for you.

# HorizontalStackLayout

The name really gives this away. It positions its children horizontally.
The HorizontalStackLayout is not responsible for providing sizing
information to its children, so the children are responsible for calculating
their own size.
Figure 5-5 shows how controls can be positioned inside a
HorizontalStackLayout.

![image](https://user-images.githubusercontent.com/26972859/231121064-a7d256a7-0b2b-4c5b-b497-d979ecfe8b33.png)

Figure 5-5. HorizontalStackLayout overview

The above layout can be achieved with the following code example:

```
<HorizontalStackLayout
 Spacing="20"
 Margin="30">
 <Border
 BackgroundColor="LightGray
 WidthRequest="100" />
 <Border
 BackgroundColor="LightGray"
 WidthRequest="100" />
 <Border
 BackgroundColor="LightGray"
 WidthRequest="100" />
</HorizontalStackLayout>
```
You wish to layout your controls vertically so you can guess where this
is going, although you will actually use one to group some of your inner
controls.

# VerticalStackLayout

The name really gives this away. It positions its children vertically.
The VerticalStackLayout follows the same sizing rules as the
HorizontalStackLayout, so the children are responsible for calculating
their own size.
And there you have it: something that arranges its children vertically,
which is exactly what you need!
Figure 5-6 shows how controls can be positioned inside a
VerticalStackLayout.

![image](https://user-images.githubusercontent.com/26972859/231121531-a4a1b9a7-a08f-45dd-a39a-cd66a1952b7c.png)
Figure 5-6. VerticalStackLayout overview

The above layout can be achieved with the following code example:

```xml
<VerticalStackLayout
 Spacing="20"
 Margin="30">
 <Border
 BackgroundColor="LightGray"
 HeightRequest="100" />
 <Border
 BackgroundColor="LightGray"
 HeightRequest="100" />
 <Border
 BackgroundColor="LightGray"
 HeightRequest="100" />
</VerticalStackLayout>
```
Let’s go ahead and create it. Inside the Border you added earlier, add
the following to your BoardDetailsPage.xaml file.

```xml
<VerticalStackLayout>
 <VerticalStackLayout
 Padding="20">
 <Label
 Text="Name"
 FontAttributes="Bold" />
 <Entry />
 <Label
 Text="Layout"
 FontAttributes="Bold" />
 <HorizontalStackLayout>
 <RadioButton
 x:Name="FixedRadioButton"
 Content="Fixed" />
 <!--<RadioButton
 Content="Freeform" />-->
 </HorizontalStackLayout>
 <VerticalStackLayout>
 <Label
 Text="Number of Columns"
 FontAttributes="Bold" />
 <Entry Keyboard="Numeric" />
 <Label
 Text="Number of Rows"
        FontAttributes="Bold" />
 <Entry Keyboard="Numeric" />
 </VerticalStackLayout>
 </VerticalStackLayout>
 <Button
 Text="Save"
 HorizontalOptions="End" />
</VerticalStackLayout>
```
Yes, I know! I spoke about keeping the visual tree simple and here
you are nesting quite a few layouts. I find there is typically some level of
pragmatism that needs to be applied. This page is still relatively simple in
terms of what is being rendered on screen so I will argue that it is fine. If
you were to repeat this layout multiple times, you would need to be a little
more strict and find the best way to lay it all out. Quite often you will find
that there can be a balancing act between defining something to give the
best performance vs. making it easier to maintain as a developer.
So you have now built your UI but you will notice that it doesn’t do
anything other than let the user type in the entry fields. You need to bind
the view up to your view model.
This is not strictly part of layouts but it is worth noting how you apply
the Keyboard property to your Entry controls. This allows you to inform
the operating system what soft keyboard to display and therefore limit
the type of data the user can enter. Note that this only applies to mobile
applications.


# Data Binding

UI-based applications, as their name suggests, involve presenting
an interface to the users. This UI is rarely ever just a static view and
therefore needs to be updated, drive updates into the application, or 

both. This process is typically an event-driven one as either side of this
synchronization needs to be notified when the other side changes. .NET
MAUI wraps this process up for you through a concept called data binding.
Data binding provides the ability to link the properties from two objects so
that changes in one property are automatically updated in the second.

# Binding

The most common type of bindings that you create are between a single
value at source and a single value at the target. The target is the owner
of the bindable property. I use the terms target and source because you
do not have to solely bind between a view and a view model. There are
scenarios where you may wish to bind one control to another.
Before you jump in to creating your first binding, you need to first
create something to bind to. Open your BoardDetailsPageViewModel
class, which is the view model for your view, and add the following:

```Csharp
private string boardName;
public string BoardName
{
 get => boardName;
 set => SetProperty(ref boardName, value);
}
```
It is worth noting that a Binding must be created against a property
(e.g., the BoardName definition from the code above). Binding to a field
(e.g., boardName) will not work.

# BindingContext

And finally the crucial step is to set the BindingContext of your page to this
view model. In Chapter 4, you did this by setting it in the XAML directly, but because you have registered your view model with the DI layer,
you can make the most of that and have it create the view model and
whatever dependencies it has for you. Open your BoardDetailsPage.
xaml.cs file and change the constructor to

```Csharp
public BoardDetailsPage(BoardDetailsPageViewModel
boardDetailsPageViewModel)
{
 InitializeComponent();
 BindingContext = boardDetailsPageViewModel;
}
```

The above code allows you to rely on the constructor injection
functionality that .NET MAUI and Shell provides.
The act of setting the BindingContext property means that any
bindings created in the page/view and any child views will be by default
against this BindingContext.
Now if you jump into the BoardDetailsPage.xaml file, you can apply
the binding to your new BoardName property in your view model. You want
to modify the first Entry that you added to look like

```Csharp
<Entry
 Text="{Binding BoardName}" />
```

This is a relatively small change and will look like the bindings you
created back in Chapter 4 when exploring the MVVM pattern. There isn’t
much detail to this but there is a fair amount of implicit behavior that I feel
I must highlight. Let’s cover what it tells you first and then what it doesn’t.
You are creating a binding between the BoardName property (which
exists on your BoardDetailsPageViewModel) and the Text property on the
Entry control.

Now on to what this code doesn’t tell you.

## Path

The binding could also be written as

```Text="{Binding Path=BoardName}"```

The Path element of the binding is implied if you do not explicitly
provide it but only as the first part of the binding definition. Why am
I telling you this? There are times when you will need to supply the
Path= part.

# Mode

I mentioned that bindings keep two properties in sync with each other.
When you create a binding, you can define which direction the updates
flow. In your example, you have not provided one, which then relies on
the default Mode for the bindable property that you are binding to. In this
case, it is the Text property of the Entry, which has a default binding
mode of TwoWay. I strongly urge you to make sure you are aware of both
these defaults and your expectation when creating a binding. Choosing
the correct Mode can also boost performance. For example, the OneTime
binding mode means that no updates need to be monitored for. In your
scenario, you don’t currently need to allow the view model to update the
Entry Text property; however, as you progress, this page will also allow
for the editing of a board so you will leave it alone. If you didn’t need
to edit, you could in theory modify your binding to be Text="{Binding
Path=BoardName, Mode=OneWay}".
There are several variations for binding modes:
• **Default**: As the name suggests, it uses the default,
which is defined in the target property.

• **TwoWay**: It allows for updates to flow both ways
between source and target. A typical example is
binding to the Text property of an Entry where you 
want to both receive input from the user and update
the UI, such as your scenario that you just added with
the Entry and its Text property as Text="{Binding
Path=BoardName}".

• **OneWay**: It allows for updates to flow from the source
to the target. An example of this is your ClockWidget
where you only want updates to flow from your source
to your target.

• **OneWayToSource**: It allows for updates to flow from
the target to the source. An example of this is binding
the SelectedItem property on the ListView to a value
in your view model.

• **OneTime**: It only updates the target once when the
binding context changes.

## Source

As mentioned, a binding does not have to be created against something
defined in your code (e.g., a property on a view model). It can, in fact, be
created against another control. If you look back at the XAML you created
for this page, you will notice that you gave one of the RadioButtons a name
of FixedRadioButton. This was actually setting you up for this moment:
you can now bind your innermost VerticalStackLayouts visibility to the
value of this RadioButton.

---
If you just wanted to allow the user to optionally turn a setting on
in your UI, you could use a Switch control instead. I opted for the
RadioButton as this will play very well with your extra assignment
at the end of this chapter.
---

```xml
<VerticalStackLayout
 IsVisible="{Binding IsChecked, Source={x:Reference
FixedRadioButton}}">
```

---
Bindings can start to look complicated quickly and this is a good
example, but if you break it down, it can become much easier to follow.
You are binding the IsVisible property on your VerticalStack
Layout to the IsChecked property from the Source, which is a
Reference to the RadioButton called FixedRadioButton.
---

# Applying the Remaining Bindings

Let’s apply the remaining bindings to your page and view model so that all
fields now update your view model.
In your BoardDetailsPageViewModel class, you need to add the
backing fields and properties to bind to

```Csharp
private bool isFixed = true;
private int numberOfColumns = 3;
private int numberOfRows = 2;
public bool IsFixed
{
 get => isFixed;
 set => SetProperty(ref isFixed, value);
}
public int NumberOfColumns
{
 get => numberOfColumns;
 set => SetProperty(ref numberOfColumns, value);
}

public int NumberOfRows
{
 get => numberOfRows;
 set => SetProperty(ref numberOfRows, value);
}

```
Then in your BoardDetailsPage.xaml file you need to bind to those
new properties with the bold sections below highlighting your additions.
Change the first RadioButton to be


```xml
<RadioButton
 Content="Fixed"
 x:Name="FixedRadioButton"
 IsChecked="{Binding IsFixed}" />
```
Then change the Entry that follows after the RadioButton to be

```xml
<Entry
 Text="{Binding NumberOfColumns}"
 Keyboard="Numeric" />
```
And finally change the Entry that follows that to be

```xml
<Entry
 Text="{Binding NumberOfRows}"
 Keyboard="Numeric" />
```
# MultiBinding

There can be occasions when you wish to bind multiple source properties
to a single target property in a view. To take a minor detour, let’s rework
your ClockWidgetViewModel to have two properties: one with the date and
one with the time. You should end up with the following code (the bold
highlights the new parts):

```Csharp
namespace WidgetBoard.ViewModels;
public class ClockWidgetViewModel : ViewModelBase
{
 private readonly Scheduler scheduler = new();
 private DateOnly date;
 private TimeOnly time;
 public ClockWidgetViewModel()
 {
 SetTime(DateTime.Now);
 }
 public DateOnly Date
 {
 get => date;
 set => SetProperty(ref date, value);
 }
 public TimeOnly Time
 {
 get => time;
 set => SetProperty(ref time, value);
 }
 private void SetTime(DateTime dateTime)
 {
 Date = DateOnly.FromDateTime(dateTime);
 Time = TimeOnly.FromDateTime(dateTime);
 scheduler.ScheduleAction(
 TimeSpan.FromSeconds(1),
 () =>
 {
 SetTime(DateTime.Now);
 });
 }
}
```
The change in the view model actually opens up a number of
possibilities for you. You could
• Add separate Labels to render the information in
different locations.
• Make use of a MultiBinding and render both pieces of
information in a single Label.
It is the latter you will be using here. Open your ClockWidgetView.
xaml file and make the changes you see in bold.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Label
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:viewmodels="clr-namespace:WidgetBoard.ViewModels"
 x:Class="WidgetBoard.Views.ClockWidgetView"
 FontSize="80"
 VerticalOptions="Center"
 HorizontalOptions="Center">
 <Label.BindingContext>
 <viewmodels:ClockWidgetViewModel />
 </Label.BindingContext>
 <Label.Text>
 <MultiBinding StringFormat="{}{0} {1}">
 <Binding Path="Date" />
 <Binding Path="Time" />
   </MultiBinding>
 </Label.Text>
</Label>
```
To list what you have done here, you have
• Removed the Text="{Binding Time}" line
• Moved the above functionality into the
MultiBinding section
You should notice a slightly different syntax to the single binding
approach. In fact, you can write a single binding in a similar way, such as

```xml
<Label.Text>
 <Binding Path="Time" />
</Label.Text>
```

However, I am sure you can appreciate that the original
Text="{Binding Time}" is a lot more concise and easier to read. Each of
the properties that you covered under the “Binding” section apply to each
of the Binding elements under MultiBinding

---
You must supply either a StringFormat or a Converter in a
MultiBinding or an exception will be thrown. The reason for this
is to allow for the multiple values to be mapped down to the single
value on the target.
---

# Command

Very often you will need your applications to respond to user interaction.
This can be by tapping or clicking on a button or selecting something in
a list. This interaction is recorded in your view, but you usually require 
that the logic to handle this interaction to be performed in the view
model. This comes in the form of a Command and an optional associated
CommandParameter set of properties. The Command property itself can be
bound from the view to the view model and allows the view model to not
only handle the interaction but also to determine whether the interaction
can be performed in the first place. You already added a Button to your
BoardDetailsPage.xaml file but you didn’t hook it, so let’s do exactly that!
You just need to modify your button to be (changes in bold)

```xml
<Button
 Text="Save"
 HorizontalOptions="End"
 Command="{Binding SaveCommand}" />
```

Based on the binding content that you have explored, you can say that
this Buttons Command property is now bound to a property on your view
model called SaveCommand. You haven’t actually created this property
yet. If you are thinking it would be great if the tooling could know this
and report it to me, then the next section has got you covered. “Compiled
Bindings” will show you how to inform the tooling of how to report it to
you. First, though, open your BoardDetailsPageViewModel.cs file and add
your command implementation.
Your implementation comes in multiple parts.
1. You define the property itself:
public Command SaveCommand { get; }
You typically define a command as a read-only property
as you rarely want it to change. You will likely come across
commands being defined with the use of the ICommand
interface rather than the Command class. The reason you are
using the latter is so that you can make use of a specific
method (see part 3) to update some of your view.

2. You define what action will be performed when the
command is executed (basically when the Button is
tapped/clicked in this scenario).

```xml
public BoardDetailsPageViewModel()
{
 SaveCommand = new Command(
 () => Save(),
 () => !string.IsNullOrWhiteSpace(BoardName));
}
private void Save()
{
 var board = new Board
 {
 Name = BoardName,
 Layout = new FixedLayout
 {
 NumberOfColumns = NumberOfColumns,
 NumberOfRows = NumberOfRows
 }
 };
}
```

The Command class takes two parameters. The first
is the action to perform when the command is
executed and the second, which is optional, is
a way of defining whether the command can be
executed. A good use case for this is if you wish to
make sure that the user has entered all the required
information. In your scenario, you will make sure
that the user has entered a name for the board.

3. You notify the view when the status of whether the
command can be executed changes. To be clear, you
don’t have to know that the status has changed; you
can simply inform the view that it should requery
the status. This is where the Command class and
its ChangeCanExecute method come in. For this,
you need to tweak your BoardName property to the
following:

```Csharp
public string BoardName
{
 get => boardName;
 set
 {
 SetProperty(ref boardName, value);
 SaveCommand.ChangeCanExecute();
 }
}
```

This change means that every time the BoardName property changes
(and this will be done via the binding from the view), the Button that is
bound to the SaveCommand will requery to check whether the command
can be executed. If it can, the Button will be enabled and the user can
interact with it; if not, it will be disabled.

# Compiled Bindings

Compiled bindings are a great feature that you should in almost all cases
turn on! They help to speed up your applications because they help the
compiler know what the bindings will be set to and reduce the amount of
reflection that is required. Reflection is notoriously bad for performance
so wherever possible it is highly recommended to avoid using it. Bindings 
by default do use an amount of reflection in order to handle the value
changes between source and target. Compiled bindings, as just discussed,
help to reduce this so let’s learn how to turn them on.
Compiled bindings also provide design-time validation. If you set a
binding to a property on your view model that doesn’t exist (imagine you
made a typo, which I do a lot!), without compiled bindings the application
would still build but your binding won’t do anything. With a compiled
binding, the application will fail to build and the tooling will report that the
property you mistyped doesn’t exist.

```xml
<ContentPage
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:viewmodels="clr-namespace:WidgetBoard.ViewModels"
 x:Class="WidgetBoard.Pages.BoardDetailsPage"
 x:DataType="viewmodels:BoardDetailsPageViewModel">

```
Now that you have set up your BoardDetailsPage to allow user entry
and even perform an action when the Save button is interacted with, you
need to structure your application so that you can see this happen.

# Shell

Shell in .NET MAUI enables you to define how your application will be
laid out, not in terms of actual visuals but by defining things like whether
you want your pages viewed in tabs or just a single page at a time. It also
enables you to define a flyout, which is a side menu in your application.
You can choose to have it always visible or toggle it to slide in/out, and this
can also vary based on the type of device you are running on. Typically
a desktop has more visual real estate so you may wish to keep the flyout
always open then.

For your application, you are going to make use of the flyout to allow
you to define multiple boards that you can configure and load. I really
like the idea of having one board for when I work and then swapping to
something else when working on a side project or even for gaming.
To save having to return to this area and change bits, you are going to
jump straight into the more in-depth option and feature-rich outcome.
Don’t worry, though; as you discover each new concept, you will dive
into some detail to cover what it is and why you are using along with then
applying that concept to your application.

## ShellContent

If you take a look at your AppShell.xaml file, you should see very little
inside. Currently it has the following line:

```xml
<ShellContent
 Title="Home"
 ContentTemplate="{DataTemplate local:MainPage}"
 Route="MainPage" />

```
This code sets the main content on the application to be an instance of
the MainPage. In fact, you want to delete this line and replace it with

```xml
<ShellContent
 ContentTemplate="{DataTemplate pages:BoardDetailsPage}" />
```
There isn’t too much difference here but you should explore what
it means.
Your application’s main content will now be an instance of your
recently created BoardDetailsPage. You don’t need the Title or Route
options anymore as you will be controlling them in different ways.
The Title property will be set based on the page that is shown so you
will learn about this a little later on.

The Route property you will control as part of the next section,
“Navigation.”
Finally, you will need to add ```xmlns:pages="clrnamespace:WidgetBoard.Pages"``` to the top of the file.

# Navigation

I am personally a fan of simplifying the code I write so long as it continues
to make it easy to read. With this in mind I would like to suggest you
improve on the registration of your pages and their view models already.

## Registering Pages for Navigation

Therefore I suggest that you create a new method into your MauiProgram.
cs file.

```Csharp
private static IServiceCollection AddPage<TPage, TViewModel>(
 IServiceCollection services,
 string route)
 where TPage : Page
 where TViewModel : BaseViewModel
{
 services
 .AddTransient(typeof(TPage))
 .AddTransient(typeof(TViewModel));
 Routing.RegisterRoute(route, typeof(TPage));
 return services;
}
```
Notice the line Routing.RegisterRoute(route, typeof(TView));.
This serves as a very important part in this topic of navigation. It means
that when you tell Shell to navigate to a specific route, it will create a new

instance of the TPage type you passed in and navigate to it. Of course,
because you have registered these types with the dependency injection
layer, it means that any dependencies that are defined as parameters to the
constructor will be created and passed in for you.
The above then means that rather than writing

```Csharp
services.AddTransient<BoardDetailsPage>()
services.AddTransient<BoardDetailsPageViewModel>()
Routing.RegisterRoute(route, typeof(TPage));

```
you can now write

```Csharp
AddPage<BoardDetailsPage, BoardDetailsViewModel>(builder.
Services, "boarddetails");
```
with the added change that you now define this route. So let’s go and
delete your old registrations and replace with

```Csharp
AddPage<BoardDetailsPage, BoardDetailsPageViewModel>(builder.
Services, "boarddetails");
AddPage<FixedBoardPage, FixedBoardPageViewModel>(builder.
Services, "fixedboard");

```
I also recommend defining the routes as constant strings somewhere
in your codebase to avoid typos when wanting to navigate to them.
This means you can save one line of code per page and view model
pair that you had registered as well as the code to register the route for
navigation.
Now that you have registered your pages, let’s take a look at how you
can actually perform navigation.

# Performing Navigation

There are multiple ways to specify the route for navigation but they all use
the Shell.Current.GoToAsync method.

So, for example, you could navigate to your FixedBoardPage with the
following:

```Csharp
await Shell.Current.GoToAsync(“fixedboard”);
```

This will result in a FixedBoardPage being created and pushed onto the
navigation stack. This is precisely the behavior that you need at the end of
your SaveCommand execution in your BoardDetailsPagesViewModel class.

# Navigating Backwards

You can also pop pages off the navigation stack by navigating backward.
This can be achieved by the following:

```Csharp
await Shell.Current.GoToAsync("..");
```
with the .. component telling Shell that it needs to go backward. In fact,
backwards and forwards navigation can be performed together:

```Csharp
await Shell.Current.GoToAsync("../board");
```

# Passing Data When Navigating

One key thing that you really need to do as part of creating your board
and navigating to the page that will render the board is to pass the context
across to that page so it knows what to render. There are multiple ways to
both send the data and also to receive it.
Let’s start with sending.
• You can pass primitive data through the query string
itself, for example
```Csharp
await Shell.Current.GoToAsync("fixedboard?boardid=1234");
```
By providing the boardid, you put the responsibility on
the receiving page (or page view model) to retrieve the
right board by using the specified ID

• More complex data can be sent as an
IDictionary<string, object> parameter in the
GoToAsync method, such as

```Csharp
await Shell.Current.GoToAsync(
 "fixedboard",
 new Dictionary<string, object>
 {
 { "Board", board}
 });
```
You can also send a complex object like the above, which means the
originating page (or page view model) is responsible for retrieving or
constructing the board and you send the whole thing to the receiving page.
Then, to receive data, you can implement the IQueryAttributable
interface provided with .NET MAUI. Shell will either call this on
the page you are navigating to, or if the BindingContext (your view
model) implements the interface, it will call it there. Add this to your
FixedBoardPageViewModel class because you are going to need to process
the data. You will be going with the complex object option because you
have already loaded the Board in your AppShellViewModel class.

```Csharp
public void ApplyQueryAttributes(IDictionary<string,
object> query)
{
 var board = query["Board"] as Board;
}
```
You aren’t going to do anything with this data just yet but it is ready
for when you start to build your board layout view in the next chapter.
For now, you will continue on with the theme of Shell and define your
flyout menu.
You will also need to make your FixedBoardPageViewModel implement
the IQueryAttributable interface. Change the class definition from

```Csharp
public class FixedBoardPageViewModel : BaseViewModel
to the following (changes in bold):
public class FixedBoardPageViewModel : BaseViewModel,
IQueryAttributable
```

# Flyout

A flyout is a menu for a Shell application that is accessible through an
icon or by swiping from the side of the screen. The flyout can consist of an
optional header, flyout items, optional menu items, and an optional footer.
For your application, you are going to provide a basic header and then
the main content will be a dynamic list of all the boards your user creates.
This means that you are going to have to override the main content but
thankfully Shell makes this an easy task.
The first thing I like to do when working on a new XAML file is to turn
on compiled bindings, which I covered earlier. If you recall, this is by
specifying the x:DataType attribute to tell the compiler the type that your
view will be binding to. Let’s do that now (in bold):

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
 x:Class="WidgetBoard.AppShell"
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 x:DataType="viewmodels:AppShellViewModel">
```

This helps you as you build the view to see what doesn’t exist in your
view model. Of course, if you prefer to build the view model first then this
also helps.
Finally, you need to add xmlns:viewmodels="clrnamespace:WidgetBoard.ViewModels" to the top of the file.

# FlyoutHeader

The FlyoutHeader can be given any control or layout and therefore you
can build a really good looking header option. For your application, you
are just going to add a title Label.
Below your ShellContent element you want to add the following:

```xml
<Shell.FlyoutHeader>
 <Label
 Text="My boards"
 FontSize="20"
 HorizontalTextAlignment="Center" />
</Shell.FlyoutHeader>

```

Hopefully the above is self-explanatory but to cover the parts I
haven’t already covered, you have the ability to specify different layout
information in a Label so you can make the text centered. It is usually
recommended that you use the HorizontrolOptions property over the
HorizontalTextAlignment property; however, if you try that here, you will
see that it doesn’t center the Label.
Now let’s add in the main part of your menu.

# FlyoutContent

First, if you want to use a static set of items in your menu, you can simply
add FlyoutItems to the content. This can work well when you have a fixed
set of pages such as Settings, Home, and so on. You will be showing the 

boards that the user creates, so you will need something dynamic. For this
you need to supply the FlyoutContent. More importantly, it’s your first
introduction to the CollectionView control.
The CollectionView allows you to define how an item will look and
then have it repeated for each item in a collection that is bound to it.
Additionally, the CollectionView provides the ability to allow the user
to select items in the collection and you can define behavior that will
be performed when that selection happens. Let’s add the following to
your Shell:

```xml
<Shell.FlyoutContent>
 <CollectionView
 ItemsSource="{Binding Boards}"
 SelectionMode="Single"
 SelectedItem="{Binding CurrentBoard}">
 <CollectionView.ItemTemplate>
 <DataTemplate x:DataType="models:Board">
 <Label
 Text="{Binding Name}"
 FontSize="20"
 Padding="10,0,0,0" />
 </DataTemplate>
 </CollectionView.ItemTemplate>
 </CollectionView>
</Shell.FlyoutContent>
```
You also need to add xmlns:models="clr-namespace:WidgetBoard.
Models" to the top of the file.
Your FlyoutContent will display a Label set to the Name of each Board
instance in the collection of Boards in your view model. Additionally, the
CurrentBoard property on your view model will be updated when the user
selects one of the Labels in this collection.

If you have added all of the parts I have discussed, you will likely notice
that the tooling is reporting that you haven’t added any of the properties
that you are binding to over in your view model. Let’s jump over to your
AppShellViewModel.cs file and add the following

# Collection of Boards

```Csharp
public ObservableCollection<Board> Boards { get; } = new
ObservableCollection<Board>();
```

The ObservableCollection is a special type of collection that
implements INotifyCollectionChanged. This means that anything
bound to it will monitor changes to the collection and update its contents
on screen.
Additionally, for now you will add a fixed entry into this Boards
collection to make it possible to interact with. Later you will be saving to
and loading from a database.

```Csharp
public AppShellViewModel()
{
 Boards.Add(
 new Board
 {
 Name = "My first board",
 Layout = new FixedLayout
 {
 NumberOfColumns = 3,
 NumberOfRows = 2
 }
 });
}
```

# Selected Board

You bind the SelectedItem property from the CollectionView to your
CurrentBoard property. When your property changes, you can navigate to
the board that was selected.

```Csharp
private Board currentBoard;
public Board CurrentBoard
{
 get => currentBoard;
 set
 {
 if (SetProperty(ref currentBoard, value))
 {
 BoardSelected(value);
 }
 }
}
```
You may recall that I discussed in Chapter 4 the potential value of
SetProperty returning a Boolean value. You have finally found a use for
it! You only want to handle a board selection change if the CurrentBoard
property really has changed.

# Navigation to the Selected Board

Following on from the “Navigation” section earlier, you will navigate to the
route “fixedboard” which your FixedBoardPage is configured to. You will
also pass in the selected board so that it can be presented on screen.

```Csharp
private async void BoardSelected(Board board)
{
 await Shell.Current.GoToAsync(
   "fixedboard",
 new Dictionary<string, object>
 {
 { "Board", board}
 });
}
   
```
Before your bindings will work you, need to make some further
changes.

# Setting the BindingContext of Your AppShell

Let’s change the constructor of your AppShell.xaml.cs file to set the
BindingContext.

```Csharp
public AppShell(AppShellViewModel appShellViewModel)
{
 InitializeComponent();
 BindingContext = appShellViewModel;
}
```

You should recall that you added the AppShellViewModel as a transient
in the MauiProgram.cs file, meaning that you will be provided with a new
instance when your AppShell class is created for you.

# Register AppShell with the MAUI App Builder

Let’s register AppShell in your MauiProgram.cs file.
```builder.Services.AddTransient<AppShell>();```

# Resolve the AppShell Instead of Creating It

Change the constructor in your App.xaml.cs file to be as follows:

```Csharp
public App(AppShell appShell)
{
 InitializeComponent();
 MainPage = appShell;
}
```
All of the above changes allow you to use AppShell just like any other
page and not have to create an instance manually

# Tabs

It is worth noting that Shell offers you more functionality than you really
need in building this application.
Shell allows you to design tab bars into your application. You can have
bottom, top, or both to give flexibility on how you lay out your content. You
have control over the styling and navigation within each of the tabs also.
I won’t be covering tabs but I thoroughly recommend checking out the
documentation provided by Microsoft at https://learn.microsoft.com/
dotnet/maui/fundamentals/shell/tabs.

# Search

Search is another useful feature that comes as part of Shell but again it is
not something that you need in this application. Shell allows you to create
your own SearchHandler, which means you can define how the results
are met with the values entered in the search box that is automatically
provided. You can even define the layout of the search results and the
behavior for when an item in the search results is selected.
I won’t be covering search but I thoroughly recommend checking out
the documentation provided by Microsoft at https://learn.microsoft.
com/dotnet/maui/fundamentals/shell/search.

# Taking Your Application for a Spin

If you run the application, you will see that you are first presented with the
screen to create a new board. You can enter the details and press Save.
Figure 5-7 shows how your application looks when it is first loaded

![image](https://user-images.githubusercontent.com/26972859/231130136-66d65bb4-ce9a-48fc-ac8a-ecc64796f8d8.png)
Figure 5-7. The application home page

Or you can slide out the menu from the left-hand side. Figure 5-8
shows the flyout menu in your application.

![image](https://user-images.githubusercontent.com/26972859/231130290-4a976a6e-6a72-47da-89ef-2a669ed52266.png)
Figure 5-8. The application flyout menu

By either selecting the board or pressing Save you will be navigated to
your FixedBoardPage. Figure 5-9 shows your FixedBoardPage displaying
with the default content. This is because you haven’t wired up the board
object that you are receiving but it proves that your navigation and Shell
setup is working

![image](https://user-images.githubusercontent.com/26972859/231130422-20a5c724-8453-47e0-8ae1-638ff6528c76.png)
Figure 5-9. The fixed board page after navigating


# Summary

In this chapter, you
• Created and applied an icon for your application
• Added some placeholder pages and view models
• Filled your first page with some UI and setup bindings
to the view model
• Covered data binding and its many uses
• Gained an understanding of XAML
• Learned about the possible layouts you can use to
group other controls
• Gained an understanding of Shell and applied this to
building your applications structure
• Applied the Shell navigation to allow you to navigate to
your next page and the next chapter
• Built your flyout menu using all the learnings in
this chapter
In the next chapter, you will
• Create your own layout.
• Make use of a variety of options when adding bindable
properties.
• Provide command support from your layout.
• Use your layout in your application.

# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch05.

# Extra Assignment

As an extra assignment, I would like you to consider how you might add a
second layout type given that you
• Have a single layout type on your BoardDetailsPage
• Have options displayed when this type is selected
• Pass a FixedLayout instance over as data to your
FixedBoardPage
I would love to see what concepts you come up with.
























