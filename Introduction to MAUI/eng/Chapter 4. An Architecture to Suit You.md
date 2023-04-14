# CHAPTER 4

# An Architecture to Suit You

In this chapter, you will look through some possible architectural patterns
that can be used to build .NET MAUI applications. The objective is to
provide you with enough detail to help you find the architecture that best
fits you. I want to point out that there are no right answers concerning
which architecture to choose. The best option is to go with one that you
feel will benefit you and your team.
I aim to quash the following myths throughout the course of this
chapter:
“You are forced to use XAML.”
“You are forced to use MVVM.”
There seems to be a common misconception that Xamarin.Forms and
.NET MAUI are built largely around using only XAML and MVVM. While
this is the most common approach taken by developers, it is not forced
upon us.

## A Measuring Stick

You will build the same control with each of the options to provide a way to
compare the differences. The control you will be building is a ClockWidget.
The purpose of this control is to do the following:

- Display the current time in your app.
- Update the time every minute.

Figure 4-1 shows a very rough layout of the control with the current
date and time. You will tidy this up later with the ability to format the date
and time information in Chapter 5, but for now let’s just focus on a limited
example to highlight the differences in options. Figure 4-1 shows how the
ClockWidget will render in your application when you have finished with
this chapter.

![image](https://user-images.githubusercontent.com/26972859/231101118-c96f6a35-9a87-4899-9c95-3c9ec04a662c.png)
Figure 4-1. Sketch of how the ClockWidget control will render

## Prerequisites

Before you get started with each of the architectures you will be reviewing
in this chapter, you need to do a little bit of background setup to prepare.
You need to add a single new class. This implementation will allow
your widgets to schedule an action of work to be performed after a specific
period of time. In your scenario of the ClockWidget, you can schedule an
update of the UI. Let’s add this Scheduler class into your project.

- Right-click the WidgetBoard project.
- Select Add ➤ Class.
- Give it the name of Scheduler.
- Click Add.

You want to modify the contents of the file to look as follows:

```Csharp
using System.Threading.Tasks;
public class Scheduler
{
 public void ScheduleAction(TimeSpan timeSpan,
Action action)
 {
 Task.Run(async () =>
 {
 await Task.Delay(timeSpan);
 action.Invoke();
 });
 }
}
```

In the following sections you will be looking at code examples rather
than implementing them directly. This is aimed at providing some
comparisons to allow you to find out what will be a good fit for you as you
build your applications and grow as a cross-platform developer. At the end
of the chapter, you will take your chosen approach and add it into your
application so you can see the final result of your ClockWidget.

# Model View ViewModel (MVVM)

Model View ViewModel is a software design pattern that focuses on
separating the user interface (View) from the business logic (Model). It
achieves this with the use of a layer in between (ViewModel). MVVM
allows a clean separation of presentation and business logic. Figure 4-2
shows the clean separation between the components of the MVVM
architecture.

![image](https://user-images.githubusercontent.com/26972859/231101565-52e3465a-79fc-4f29-ae59-6c08bccd94fb.png)
Figure 4-2. An overview of the MVVM pattern

The result of creating this separation between UI and business logic
brings several benefits:

- Makes unit testing easier
- Allows for Views to be swapped out or even rewritten
without impacting the other parts
- Encourages code reuse
- Provides the ability to separate UI development from
the business logic development

A key part to any design pattern is knowing where to locate parts of
your code to make it fit and abide by the rules. Let’s take a deeper look at
each of the three key parts of this pattern.

# Model

The Model is where you keep your business logic. It is typically loaded
from a database/webservice among many other things.
For your business logic, you are going to rely on the Scheduler class
that you created earlier in the “Prerequisites” section of this chapter.

# View

The View defines the layout and appearance of the application. It is what
the user will see and interact with. In .NET MAUI, a View is typically
written in XAML where possible, but there will be occasions when logic
in the code-behind will need to be written. You will learn this later in this
chapter; you don’t have to use XAML at all so if you don’t feel XAML is
right for you, fear not.
A View in .NET MAUI is typically a ContentPage or an implementation
that will inherit from ContentPage or ContentView. You use a ContentPage if
you want to render a full page in your application (basically a view that will fill
the application). You use a ContentView for something smaller (like a widget!).
For your implementation you will be inheriting from a ContentView.
I discussed in Chapter 2 that the majority of XAML files come with an
associated C# file. A XAML-based view is no exception to this rule. With this
in mind, let’s take a look at the contents you need to place in each of the files.

# XAML

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/
dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/
winfx/2009/xaml"
 xmlns:viewmodels="clr-namespace:WidgetBoard.
ViewModels"
 x:Class="WidgetBoard.ClockWidget">
 <ContentView.BindingContext>
 <viewmodels:ClockWidgetViewModel />
 </ContentView.BindingContext>
 <Label Text="{Binding Time}"
FontSize="80"
 VerticalOptions="Center"
 HorizontalOptions="Center" />
</ContentView>
```

## C# (Code-Behind)

The following code will have already been created for you by the .NET
MAUI template. It is included for reference.

```Csharp
namespace WidgetBoard;
public partial class ClockWidget : ContentView
{
 public ClockWidget()
 {
 InitializeComponent();
 }
}
```
The InitializeComponent method call above is essential when
building XAML-based views. It results in the XAML being loaded and
parsed into an instance of the controls that have been defined in the
XAML file.

# ViewModel

The ViewModel acts as the bridge between the View and the Model. You
expose properties and commands on the ViewModel that the View will
bind to. To make a comparison to building applications with just codebehind, we could state that properties basically map to references of
controls and commands are events. A binding provides a mechanism for
both the View and ViewModel to send and receive updates.

For your ViewModel to notify the View that a property has changed
and therefore the View will refresh the value displayed on screen, you
need to make use of the INotifyPropertyChanged interface. This offers
a single PropertyChanged event that you must implement and ultimately
raise when your data-bound value has changed. This is all handled by the
XAML binding engine, which you will look at in much more detail in the
next chapter. Let’s create your ViewModel class and then break down what
is going on.

```Csharp
public class ClockWidgetViewModel : INotifyPropertyChanged
{
 public event PropertyChangedEventHandler PropertyChanged;
 private readonly Scheduler scheduler = new();
 private DateTime time;
 public DateTime Time
 {
 get
 {
 return time;
 }
 set
 {
 if (time != value)
 {
 time = value;
 PropertyChanged?.Invoke(this, new PropertyChang
edEventArgs(nameof(Time)));
 }
 }
 
 }
 public ClockWigetViewModel()
 {
 SetTime(DateTime.Now);
 }
 public void SetTime(DateTime dateTime)
 {
 Time = dateTime;
 scheduler.ScheduleAction(
 TimeSpan.FromSeconds(1),
 () => SetTime(DateTime.Now));
 }
}
```

You have
• Created a class called ClockWidgetViewModel
• Implemented the INotifyPropertyChanged interface
• Added a property that when set will check whether
it’s value really has changed, and if it has, raise the
PropertyChanged event with the name of the property
that has changed
• Added a method to set the Time property and repeat
every second so that the widget looks like a clock
counting

# Model View Update (MVU)

Model View Update is a software design pattern for building interactive
applications. The concept originates from the Elm programming language.
As the name suggests, there are three key parts to MVU:
• Model: This is the state of your application.
• View: This is a visual representation of your state.
• Update: This is a mechanism to update your state.
Figure 4-3 shows how each of these components relate and interact
with each other.  

![image](https://user-images.githubusercontent.com/26972859/231108112-c6062936-3975-4593-919e-f7a61de4c338.png)
Figure 4-3. An overview of the MVU pattern

This pattern offers several benefits:
• Clearly defined rules around where state is allowed to
be updated
• Ease of testing
A key part to any design pattern is knowing where to locate parts of
your code to make it fit and abide by the rules. Let’s take a deeper look at
each of the three key parts of this pattern.

# Getting Started with Comet

First, you must install the Comet project templates. To do this, open a
terminal window and run the following command:

```dotnet new –install Clancey.Comet.Templates.Multiplatform```

This will install the template so that you can create a new project.
Sadly, this is different enough to the WidgetBoard project that you have
been working with so far.
Next, you need to create the project. This is again done via the terminal
for now:

```dotnet new comet -–name WidgetBoard.Mvu```

This will create a new project that you can start modifying.

## Adding Your MVU Implementation

Go ahead and open the project you just created.
The first thing you need to do is make use of the same Scheduler
class that you created in the MVVM Model example for your MVU
implementation. Here it is again to make life easier:

```Csharp
public class Scheduler
{
 public void ScheduleAction(TimeSpan timeSpan,
Action action)
 {
 Task.Run(async () =>
 {
 await Task.Delay(timeSpan);
 action.Invoke();
 }
 }
}
```
Finally, go ahead and create your ClockWidget class:

```Csharp
public class ClockWidget : View
{
 [State]
 readonly Clock clock = new();
 [Body]
 View body()
 => new Text(() => $"{clock.Time}")
 .FontSize(80)
 .HorizontalLayoutAlignment(LayoutAlignment.Center)
 .VerticalLayoutAlignment(LayoutAlignment.Center);
 public class Clock : BindingObject
 {
 readonly Scheduler scheduler = new();
 public DateTime Time
 {
 get => GetProperty<DateTime>();
 set => SetProperty(value);
 }
 public Clock()
 {
 SetTime(DateTime.Now);
 }
 void SetTime(DateTime dateTime)
 {
 Time = dateTime;
 scheduler.ScheduleAction(
 TimeSpan.FromSeconds(1),
 () =>
 {
 SetTime(DateTime.Now);
 });
 }
 }
}
 
```

Now that you have added a load of code, let’s summarize what you
have done.
• You have created a new class named ClockWidget.
• You have defined your state type as Clock.
• You have initialized (known as init in the MVU
pattern) your model field clock.
• You have defined your view with the body() function.
• You have defined your update function in the form of
the SetTime method.
Note that there are two common scenarios when an update is called:
when there is user interaction (e.g., a click/tap of a button) and around
asynchronous background work. Your example here applies to the second
scenario.

# XAML vs. C# Markup

XAML has proven to be a big part of building application UIs in Xamarin.
Forms and it will likely continue in .NET MAUI, but I want to make it clear
that you do not have to use it. So, if like some friends and colleagues, the
verbosity of XAML makes you feel queasy, there is a solution!

Anything that you can create in XAML can ultimately be created in
C#. Furthermore, there are ways to improve on the readability of the C#
required to build UIs.
Some benefits of building user interfaces solely with C# are
• A single file for a view. No pairing of .xaml.cs and
.xaml files.
• Better refactoring options so renaming properties or
commands in XAML won’t update the C#.
Let’s work through how you can build your ClockWidget in C# in all its
verbosity and then I will show how you can simplify it using C# Markup. (I
must add this is an open-source package that you need to bring in). Also,
these examples are still built using MVVM.

# Plain C#

As mentioned, anything you can build in XAML can also be built in C#.
The following code shows how the exact same XAML definition of your
ClockWidget can be built using just C#:

```Csharp
using WidgetBoard.ViewModels;
namespace WidgetBoard.Views;
public class ClockWidget : ContentView
{
 public ClockWidget()
 {
 BindingContext = new ClockWidgetViewModel();
 var label = new Label
 {
 FontSize = 80,
 HorizontalOptions = LayoutOptions.Center,
 VerticalOptions = LayoutOptions.Center
 };
 label.SetBinding(
 Label.TextProperty,
 nameof(ClockWidgetViewModel.Time));
 Content = label;
 }
}
```

The code above does the following things:
• Creates a single file representing your ClockWidget
• Points your widget’s BindingContext to the
ClockWidgetViewModel
• Creates a label and set its Text property to be bound to
the view models Time property
• Assigns the label to the content of the view

# C# Markup

I have recently come to appreciate the value of being able to fluently
build UIs. I don’t tend to do it often because I personally feel comfortable
building with XAML or perhaps it is Stockholm Syndrome kicking in ☺
(I’ve been working with XAML for well over 10 years now). When I do, it
needs to be as easy to read and build as possible given it is not something I
do often.

As a maintainer on the .NET MAUI Community Toolkit, one of the
packages we provide is CommunityToolkit.Maui.Markup. It provides a set
of extension methods and helpers to build UIs fluently.

```Csharp
using CommunityToolkit.Maui.Markup;
using WidgetBoard.ViewModels;
namespace WidgetBoard.Views;
public class ClockWidget : ContentView
{
 public ClockWidget()
 {
 BindingContext = new ClockWidgetViewModel();
 Content = new Label()
 .Font(size: 80)
 .CenterHorizontal()
 .CenterVertical()
 .Bind(Label.TextProperty,
nameof(ClockWidgetViewModel.Time));
 }
}
```

This code performs the same steps as the plain C# example; however,
the code is much easier to read. I am sure you can imagine that when the
complexity of the UI increases, this fluent approach can really start to
benefit you.

# Chosen Architecture for This Book

Throughout this book, we will be using the MVVM-based architecture
while building the UI through XAML.

My reasons for choosing MVVM are as follows:
• I have spent the last 10+ years using this architecture so
it certainly feels natural to me.
• It has been a very common way of building applications
over the past decade so there is an abundance of
resources online to assist in overcoming issues
around it.
• It is a common pattern in all Microsoft products and
has a proven track record.
Now that I have covered the various architecture options and
decided on using MVVM, let’s proceed to adding in the specific Views
and ViewModels so that it can be used inside the application. Then I will
show how to start simplifying the implementation so that the code really
only needs to include the core logic by avoiding having to add a lot of the
boilerplate code.

# Adding the ViewModels

First, add a new folder to your project.
• Right-click the WidgetBoard project.
• Select Add ➤ New Folder.
• Enter the name ViewModels.
• Click Add.
This folder will house your application’s view models. Let’s proceed to
adding the first one.

# Adding IWidgetViewModel

The first item you need to add is an interface. It will represent all widget
view models that you create in your application.
• Right-click the ViewModels folder.
• Select Add ➤ New Item.
• Select the Interface type.
• Enter the name IWidgetViewModel.
• Click Add.
Modify this file to the following:

```Csharp
namespace WidgetBoard.ViewModels;
public interface IWidgetViewModel
{
 int Position { get; set; }
 string Type { get; }
}
```

# Adding BaseViewModel

This will serve as the base class for all of your view models so that you only
have to write some boilerplate code once. Don’t worry; you will see how to
optimize this even further!
• Right-click the ViewModels folder.
• Select Add ➤ Class.
• Enter the name BaseViewModel.
• Click Add.

You can replace the contents of the class file with the following code:

```Csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace WidgetBoard.ViewModels;
public abstract class BaseViewModel : INotifyPropertyChanged
{
 public event PropertyChangedEventHandler PropertyChanged;
 protected void OnPropertyChanged([CallerMemberName] string
propertyName = "")
 {
 PropertyChanged?.Invoke(this, new PropertyChangedEventA
rgs(propertyName));
 }
 protected bool SetProperty<TValue>(ref TValue backingField,
TValue value, [CallerMemberName] string propertyName = "")
 {
 if (Comparer<TValue>.Default.Compare(backingField,
value) == 0)
 {
 return false;
 }
 backingField = value;
 OnPropertyChanged(propertyName);
 return true;
 }
}
```
You should be familiar with the first line inside the class:

```public event PropertyChangedEventHandler PropertyChanged;```

This is the event definition that you must add as part of implementing
the INotifyPropertyChanged interface and it serves as the mechanism for
your view model to update the view.
The next method provides a mechanism to easily raise the
PropertyChanged event:

```Csharp
protected void OnPropertyChanged([CallerMemberName] string
propertyName = "")
{
 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(
propertyName));
}
```
The OnPropertyChanged method can be called with or without passing
in a value for propertyName. By passing a value in, you are indicating
which property name on your view model has changed. If you do not, then
the [CallerMemberName] attribute indicates that the name of caller will be
used. Don’t worry if this is a little unclear right now; it will become much
clearer when you add your property into your ClockWidgetViewModel so
just bear with me.
The final method adds a lot of value:

```Csharp
protected bool SetProperty<TValue>(
 ref TValue backingField,
 TValue value,
 [CallerMemberName] string propertyName = "")
{
 if (Comparer<TValue>.Default.Compare(backingField,
value) == 0)
 {
 
 return false;
 }
 backingField = value;
 OnPropertyChanged(propertyName);
 return true;
}
 
```

The SetProperty method does the following:
• Allows you to call it from a property setter, passing in
the field and value being set
• Checks whether the value is different from the backing
field, basically determining whether the property has
really changed
• If it has changed, it fires the PropertyChanged event
using your new OnPropertyChanged method
• Returns a Boolean indicating whether the value did
really change. This can be really useful when needing
to update other properties or commands!
This concludes the base view model implementation. Let’s proceed to
using it as the base for the ClockWidgetViewModel to really appreciate the
value it is providing.

# Adding ClockWidgetViewModel

Let’s add a new class file into your ViewModels folder as you did for the
BaseViewModel.cs file. Call this file ClockWidgetViewModel and modify
the contents to the following:

```
using System;

using System.ComponentModel;
namespace WidgetBoard.ViewModels;
public class ClockWidgetViewModel : BaseViewModel,
IWidgetViewModel
{
 private readonly Scheduler scheduler = new();
 private DateTime time;
 public DateTime Time
 {
 get => time;
 set => SetProperty(ref time, value);
 }
 public int Position { get; set; }
 public string Type => "Clock";
 public ClockWidgetViewModel()
 {
 SetTime(DateTime.Now);
 }
 public void SetTime(DateTime dateTime)
 {
 Time = dateTime;
 scheduler.ScheduleAction(
 TimeSpan.FromSeconds(1),
 () => SetTime(DateTime.Now));
 }
}

```
The above code should be familiar. You saw it when reviewing
MVVM. The optimization made here is to reduce the size of the Time
property down to just 5 lines where the original example was 16 lines
of code.

# Adding Views

First, add a new folder to your project.
• Right-click the WidgetBoard project.
• Select Add ➤ New Folder.
• Enter the name Views.
• Click Add.
This folder will house your application’s views. Let’s proceed to adding
your first one.

# Adding IWidgetView
The first item you need to add is an interface to represent all widget view
models that you create in your application.
• Right-click the Views folder.
• Select Add ➤ New Item.
• Select the Interface type.
• Enter the name IWidgetView.
• Click Add.

Modify the contents of this file to the following

```Csharp
using WidgetBoard.ViewModels;
namespace WidgetBoard.Views;
public interface IWidgetView
{
 int Position { get => WidgetViewModel.Position; set =>
WidgetViewModel.Position = value; }
 IWidgetViewModel WidgetViewModel { get; set; }
}
```

# Adding ClockWidgetView

The next item you need to add is a ContentView. This is the first time you
are doing this, so use the following steps:
• Right-click the Views folder.
• Select Add ➤ New Item.
• Select the .NET MAUI tab.
• Select the .NET MAUI ContentView (XAML) option.
• Enter the name ClockWidgetView.
• Click Add.
Observe that two new files have been added to your project:
ClockWidgetView.xaml and ClockWidgetView.xaml.cs. You may
notice that the ClockWidgetView.xaml.cs file is hidden in the Solution
Explorer panel and that you need to expand the arrow to the left of the
ClockWidgetView.xaml file.
Let’s update both files to match what was in the original examples.

Open the ClockWidgetView.xaml file and modify the contents to the
following:

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
</Label>
```
Open the ClockWidgetView.xaml.cs file and modify the contents to
the following:

```Csharp
using WidgetBoard.ViewModels;
namespace WidgetBoard.Views;
public partial class ClockWidgetView : Label, IWidgetView
{
 public ClockWidgetView()
 {
 InitializeComponent();
 WidgetViewModel = new ClockWidgetViewModel();
 BindingContext = WidgetViewModel;
 }
 public IWidgetViewModel WidgetViewModel { get; set; }
}
```

This completes the work to add the ClockWidget into your codebase.
Now you need modify your application so that you can see this widget
in action!

# Viewing Your Widget

In order to view your widget in your application, you need to make some
changes to the MainPage.xaml and MainPage.xaml.cs files that were
generated when you first created your project.

## Modifying MainPage.xaml
Simply replace the contents of the file with the following.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/
dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/
winfx/2009/xaml"
 xmlns:views="clr-namespace:WidgetBoard.Views"
 x:Class="WidgetBoard.MainPage">
 <views:ClockWidgetView />
</ContentPage>
```
The original file had a basic example that ships with the .NET MAUI
template, but it wasn’t of much use in this application.

## Modifying MainPage.xaml.cs

You need to modify the contents of this file because you deleted some
controls from the MainPage.xaml file. If you don’t update this file, Visual
Studio will report compilation errors. You can replace the entire contents 

of the MainPage.xaml.cs file with the following to remove references to the
controls you deleted from XAML file:

```Csharp
namespace WidgetBoard;
public partial class MainPage : ContentPage
{
 public MainPage()
 {
 InitializeComponent();
 }
}
```

This concludes the changes that you need to make in your application.
Let’s see what your application looks like now!

## Taking the Application for a Spin

If you build and run your application just like you learned to in Chapter 2,
you can see that it renders the ClockWidget just as I originally designed.
Figure 4-4 shows the clock widget rendered in the application running
on macOS.

![image](https://user-images.githubusercontent.com/26972859/231113621-1d4143c8-c703-431e-8177-4a9651cbb16c.png)
Figure 4-4. The clock widget rendered in the application running
on macOS

You have looked at ways to optimize your codebase when using MVVM
but I would like to provide some further details on how you can leverage
the power of the community in order to further improve your experience.

## MVVM Enhancements

There are two key parts I will cover regarding how you can utilize existing
packages to reduce the amount of code you are required to write.

# MVVM Frameworks

There are several MVVM frameworks that can expand on this by providing
a base class implementation for you with varying levels of other extra
features. To list a few,
• CommunityToolkit.Mvvm
• MVVMLight
• FreshMVVM
• Prism
• Refractored.MVVMHelpers
• ReactiveUI

These packages will ultimately provide you with a base class very
similar to the BaseViewModel class that you created earlier. For example,
the Prism library provides the BindableBase class that you could use. It
offers yet another optimization in terms of less code that you need to write
and ultimately maintain.
You can go a step further, but you need to believe…

# Magic

Yes, that’s right: magic is real! These approaches involve auto generating
the required boilerplate code so that we as developers do not have to do it.
There are two main packages that offer this functionality. They provide it
through different mechanisms, but they work equally well.
• Fody: IL generation, https://github.com/Fody/Home
• CommunityToolkit.Mvvm: Source generators (yes, this
gets a second mention), https://learn.microsoft.
com/dotnet/communitytoolkit/mvvm/
In the past, I was skeptical of using such packages. I felt like I was
losing control of parts that I needed to hold on to. Now I can appreciate
that I was naïve, and this is impressive.
Let’s look at how these packages can help to further reduce the
code. This example uses CommunityToolkit.Mvvm, which provides the
ObservableObject base class and a wonderful way of adding attributes
([ObservableProperty]) to the fields you wish to trigger PropertyChanged
events when their value changes. This will then generate a property with
the same name as the field but with a capitalized first character, so time
becomes Time.

```Csharp
public partial class ClockWidgetViewModel : ObservableObject
{
 [ObservableProperty]
 private DateTime time;
 public ClockWigetViewModel()
 {
 SetTime(DateTime.Now);
 }
 public void SetTime(DateTime dateTime)
 {
 Time = dateTime;
 scheduler.ScheduleAction(
 TimeSpan.FromSeconds(1),
 () => SetTime(DateTime.Now));
 }
}
```

That’s 17 lines down to 2 from the original example! The part that I
really like is that it reduces all the noise of the boilerplate code so there is a
bigger emphasis on the code that we need to write as developers.
You may have noticed that you are still referring to the Time property
in the code but you haven’t supplied the definition for this property. This
is where the magic comes in! If you right-click the Time property and select
Go to Definition… it will open the following source code so you can view
what the toolkit has created for you:

```Csharp
// <auto-generated/>
#pragma warning disable
#nullable enable
namespace WidgetBoard.ViewModels

{
 partial class ClockWidgetViewModel
 {
 /// <inheritdoc cref="time"/>
 [global::System.CodeDom.Compiler.
GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.
ObservablePropertyGenerator", "8.0.0.0")]
 [global::System.Diagnostics.CodeAnalysis.
ExcludeFromCodeCoverage]
 public global::System.DateTime Time
 {
 get => time;
 set
 {
 if (!global::System.Collections.Generic.
EqualityComparer<global::System.DateTime>.
Default.Equals(time, value))
 {
 OnTimeChanging(value);
 OnPropertyChanging(global::CommunityToo
lkit.Mvvm.ComponentModel.__Internals.__
KnownINotifyPropertyChangingArgs.Time);
 time = value;
 OnTimeChanged(value);
 OnPropertyChanged(global::CommunityTool
kit.Mvvm.ComponentModel.__Internals.__
KnownINotifyPropertyChangedArgs.Time);
 }
 }
 }
 
 /// <summary>Executes the logic for when <see
cref="Time"/> is changing.</summary>
 [global::System.CodeDom.Compiler.
GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.
ObservablePropertyGenerator", "8.0.0.0")]
 partial void OnTimeChanging(global::System.
DateTime value);
 /// <summary>Executes the logic for when <see
cref="Time"/> just changed.</summary>
 [global::System.CodeDom.Compiler.
GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.
ObservablePropertyGenerator", "8.0.0.0")]
 partial void OnTimeChanged(global::System.
DateTime value);
 }
}
 
```
You can see that the generated source code looks a little noisy, but it
does in fact generate the property you need. View the section highlighted
in bold above.
I have only really scratched the surface regarding the functionality
that the CommunityToolkit.Mvvm offers. I strongly urge you to refer
to the documentation at https://learn.microsoft.com/dotnet/
communitytoolkit/mvvm/ to learn how it can further aid your application
development.

# Summary

I hope I have made it clear that there is no single right way to do things or
build applications. You should pick and choose what approaches will best
suit your environment. With this point in mind, the goal of this chapter was
to give you a good overview of several different approaches to architecting 
your application. There are always a lot of opinions floating around to
indicate which architectures people prefer but I strongly urge you to
evaluate which will help you to achieve your goals best.
In this chapter, you have
• Learned about the different possibilities you have to
architect your applications
• Decided on what architecture to use
• Walked through a concrete example by creating the
ClockWidget
• Learned how to further optimize your implementation
using NuGet packages
In the next chapter, you will
• Create and apply an icon in your application
• Add some placeholder pages and view models
• Fill your first page with some UI and set up bindings to
the view model
• Explore data binding and its many uses
• Gain an understanding of XAML
• Learn about the possible layouts you can use to group
other controls
• Gain an understanding of Shell and apply this to
building your application’s structure
• Apply the Shell navigation to allow you to navigate
• Build your flyout menu

# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch04.


