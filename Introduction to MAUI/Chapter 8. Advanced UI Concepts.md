# CHAPTER 8

# Advanced UI Concepts

In this chapter, you will provide the user of your application with the ability
to add a widget to the boards they create through the use of an overlay. You
will further enhance this overlay by defining common styling techniques
and handling the differences between light and dark mode devices.
You will then take a journey into discovering how you can build an
application that feels natural and organic to your human user base. Finally,
you will look at how you can keep the animations driving the organic look
and feel cleanly separated from your business logic code.

# Adding the Ability to Add a Widget to a Board

In Chapter 6, you created your own BoardLayout and the associated
FixedLayoutManager that enabled you to show a board and added in
the ability to handle interaction events by the user. In this section, you
are going to expand on that to handle the user tapping on a widget
Placeholder and letting the user choose a widget to add to the board.

# Possible Ways of Achieving Your Goal

There are several ways you can go about adding in this piece of
functionality. Some are better suited to different scenarios and some
simply come down to a personal preference. I encourage you to
understand your goal before you embark on this journey of working out
which option will best suit your need. If you only wish to report a message
to the user or capture a choice or even a single piece of input, then you can
utilize some underlying functionality provided by .NET MAUI. The Page
class provides the ability to do each of the three items discussed; it doesn’t
solve your needs, but it really does have value in many applications. The
Microsoft documentation provides a good set of reference examples on
how to use these options at https://learn.microsoft.com/dotnet/maui/
user-interface/pop-ups.
Let’s discuss some of these options that do solve your needs and
then make a decision on which you feel is the best candidate for your
application.

# Showing a Modal Page

So far in this book you have only considered how Shell offers the
ability to navigate between ContentPages. This is the default and most
common scenario. There can be times when you wish to show a page
that is blocking and will require the user to engage with it to return to the
previous page. This type of page or display is referred to as modal. The
scenario of showing something to the user and requiring them to engage
with it could be a perfect scenario.
In order to enable this functionality in .NET MAUI, you need to set the
Shell.PresentationMode property on the ContentPage that you wish to
display. For example,

```xml
<ContentPage ...
 Shell.PresentationMode="Modal">
 ...
</ContentPage>
```
You can then call the Shell.Current.GoToAsync method with the
routing options configured for this page and it will be presented modally
instead of being navigated to.

Pro
• Keeps specific code contained
Con
• Complicates flow of code when handling a return result

# Overlaying a View

Sometimes the most straightforward way to achieve this approach is to add
another view to your page and programmatically change its visibility to
give the impression you have a modal page displaying.
Pro
• Reduces effort of page creation
Con
• Requires specific code in calling view/view model

# Showing a Popup

There is currently no explicit support in .NET MAUI for displaying popups;
however, the functionality does exist on the each of the platforms that .NET
MAUI runs on. You can go to the lengths of implementing your own ability
to display a popup but it would be rather involved. Instead, the .NET MAUI
Community Toolkit provides a Popup class that makes it straightforward for
you to display a popup in your application.

Pros
• Keeps specific code contained
• Provides easy return result handling
For further reading on how to use the toolkit and its Popup class, please
refer to the documentation at https://learn.microsoft.com/dotnet/
communitytoolkit/maui/views/popup.

# The Chosen Approach

Given the pros and cons outlined above, you might guess that you will be
using the Popup class. Nope. Let’s use the overlaying-a-view approach. This
is mainly because it will help to expose you to more .NET MAUI-specific
concepts that I believe will be extremely valuable in building applications.
However, for your own work, use the approach that best fits your scenario.
I would like to emphasize that each of the above options will achieve the
results needed. In fact, there could well be more options that I haven’t
covered, and if you find one, I would love to hear about it.

# Adding Your Overlay View

You need to add a view to your FixedBoardPage.xaml file that will present
the option to the user to add a new widget to the board. Let’s open that file
and add the following code inside the Grid and below the
</layouts:BoardLayout> line:

```xml
<BoxView
 BackgroundColor="Black"
 Opacity="0.5"
 IsVisible="{Binding IsAddingWidget}" />
<Border
 IsVisible="{Binding IsAddingWidget}"
        HorizontalOptions="Center"
 VerticalOptions="Center"
 Padding="10">
 <VerticalStackLayout>
 <Label
 Text="Add widget"
 FontSize="20" />
 <Label
 Text="Widget" />
 <Picker
 ItemsSource="{Binding AvailableWidgets}"
 SelectedItem="{Binding SelectedWidget}"
 SemanticProperties.Description="{Binding Text,
Source={x:Reference SelectTheWidgetLabel}}"
 SemanticProperties.Hint="Picker containing the
possible widget types that can be added to the
board. This is a required field." />
 <Label
 Text="Preview" />
 <ContentView
 WidthRequest="250"
 HeightRequest="250" />
 <Button
 Text="Add widget"
 Command="{Binding AddWidgetCommand}"
 SemanticProperties.Hint="Adds the selected widget
to the board. Requires the 'Select the widget'
field to be set." />
 </VerticalStackLayout>
</Border>
```
The code addition results in two new controls added to the parent
Grids children collection: a BoxView and a Border. The BoxView is added to
provide a semi-transparent overlay on top of the rest of the application and
the Border presents the content for selecting a new widget. Adding them
after the BoardLayout means it will be rendered on top of the BoardLayout.
This ordering is referred to as Z-index and in the majority of .NET MAUI
applications, layouts are determined by the order in which the children
are added to their parent. This means that the later the controls are added,
the higher they will appear visually. You can modify this default behavior
by using the ZIndex property where the higher the value, the higher they
will appear visually. With this knowledge, you can add a binding between
the IsVisible property of your new controls and a property on your view
model, so your view model can control whether you are adding a widget to
the board.
Let’s update your view model.

# Updating Your View Model

Since you turned on compiled bindings in a previous chapter, you will
now see that your code will not compile because you have not defined the
properties you are binding to. So open the FixedBoardPageViewModel.cs
file and make the following additions.
Add the new properties and associated backing fields into your
FixedBoardPageViewModel class.

```Csharp
private int addingPosition;
private string selectedWidget;
private bool isAddingWidget;
private readonly WidgetFactory widgetFactory;
public IList<string> AvailableWidgets => widgetFactory.AvailableWidgets;

public ICommand AddWidgetCommand { get; }
public ICommand AddNewWidgetCommand { get; }
public bool IsAddingWidget
{
 get => isAddingWidget;
 set => SetProperty(ref isAddingWidget, value);
}
public string SelectedWidget
{
 get => selectedWidget;
 set => SetProperty(ref selectedWidget, value);
}

```

Update the constructor with the new WidgetFactory dependency and
set the new commands that you have added; changes are in bold.

```Csharp
public FixedBoardPageViewModel(
 WidgetTemplateSelector widgetTemplateSelector,
 WidgetFactory widgetFactory)
{
 WidgetTemplateSelector = widgetTemplateSelector;
 this.widgetFactory = widgetFactory;
 Widgets = new ObservableCollection<IWidgetViewModel>();
 AddWidgetCommand = new Command(OnAddWidget);
 AddNewWidgetCommand = new Command<int>(index =>
 {
 IsAddingWidget = true;
 addingPosition = index;
 });
}
```
In the previous code section, you set the IsAddingWidget property to
true in order to show the overlay view and you also keep a record of the
index variable, which is the Position property from the Placeholder that
was tapped.
Provide the method implementation for the AddWidgetCommand.

```Csharp
private void OnAddWidget()
{
 if (SelectedWidget is null)
 {
 return;
 }
 var widgetViewModel = widgetFactory.CreateWidgetViewModel
(SelectedWidget);
 widgetViewModel.Position = addingPosition;
 Widgets.Add(widgetViewModel);
 IsAddingWidget = false;
}
```
Hopefully the majority of what you just added should feel familiar. The
part that most likely doesn’t is the final OnAddWidget method. Let’s take a
deeper look at this implementation.
The SelectedWidget property is bound to your Picker in the view. You
do some initial input validation to make sure that the user has chosen a
type of widget to add; otherwise, you return out of the method.
Next, you use the new dependency (widgetFactory) to create a view
model for you.
Then you set its Position based on which placeholder was tapped
initially.

Then you add your newly created widgetViewModel to the collection of
Widgets so that it can update the UI.
Finally, you set the IsAddingWidget property to false in order to hide
the overlay view again.

# Showing the Overlay View

Now you can add the ability to programmatically show the Border that
allows your users to pick a widget and add it to the board. You already
provided a large amount of this functionality inside your Placeholder and
FixedLayoutManager classes, so you just need to hook up your view model
to this functionality. You have also just set the groundwork in your view
model, so let’s hook the components up. Open the FixedBoardPage.xaml
file again and add the following bold line:

```xml
<layouts:BoardLayout
 ItemsSource="{Binding Widgets}"
 ItemTemplateSelector="{Binding WidgetTemplateSelector}">
 <layouts:BoardLayout.LayoutManager>
 <layouts:FixedLayoutManager
 NumberOfColumns="{Binding NumberOfColumns}"
 NumberOfRows="{Binding NumberOfRows}"
 PlaceholderTappedCommand="{Binding
AddNewWidgetCommand}" />
 </layouts:BoardLayout.LayoutManager>
</layouts:BoardLayout>
```
If you build and run your application, you can see that once you have
created a board, you can now tap or click on the Placeholder and observe
that your overlay displays. You will notice that there is no background
to your overlay, though, so it is really difficult for a user to understand
what to do. You can just set the BackgroundColor of your Border control;
however, this can lead to a number of issues. For example, if you fixed 

the BackgroundColor to white and a user switches on dark mode on their
device, they would have a rather unpleasant experience. Figure 8-1 shows
how the application currently looks and highlights the issue.

![image](https://user-images.githubusercontent.com/26972859/231236048-1bbf7374-65ff-4646-82d5-6c2071843bd1.png)
Figure 8-1. The application showing the overlay with a poor user experience

Let’s look at how .NET MAUI provides the ability to style your
applications, which includes supporting light and dark modes.

# Styling

.NET MAUI provides the ability to style your applications. Styling in .NET
MAUI offers many advantages:
• Central definition of look and feel
• Less verbosity in your XAML/code
• Style inheritance
Styles in .NET MAUI can be defined at many different
levels and where they are defined is extremely important
when understanding what impact they will have. The two
key distinctions between where they are defined can be
considered as

• Globally : These styles are added to the application’s
resources. You can see an example of this if you open
the App.xaml file. The line in bold shows that another
file (Styles.xaml) containing the styles is loaded into
the Application.Resources property. These styles
apply to all controls in the application unless otherwise
explicitly overridden.

```xml
<Application xmlns="http://schemas.microsoft.com/
dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/
winfx/2009/xaml"
 xmlns:local="clr-namespace:WidgetBoard"
 x:Class="WidgetBoard.App">
 <Application.Resources>
 <ResourceDictionary>
 <ResourceDictionary.MergedDictionaries>
 <ResourceDictionary
Source="Resources/Styles/Colors.
xaml" />
 <ResourceDictionary Source="Resources/
Styles/Styles.xaml" />
 </ResourceDictionary.MergedDictionaries>
 </ResourceDictionary>
 </Application.Resources>
</Application>
```

Locally: These styles are added to a view or page
resources property. Styles defined in this way will apply
to all controls that are children of the view or page they
are defined in.

Your global example refers to the Styles.xaml file. This is a file that
comes with a new .NET MAUI project.

# Examining the Default Styles

You can view this file under Resources/styles.xaml. Let’s take a look at
the style for Border in this file:

```xml
<Style TargetType="Border">
 <Setter Property="Stroke" Value="{AppThemeBinding
Light={StaticResource Gray200}, Dark={StaticResource
Gray500}}" />
 <Setter Property="StrokeShape" Value="Rectangle"/>
 <Setter Property="StrokeThickness" Value="1"/>
</Style>
```
The XAML syntax used to define a style looks rather different to
the XAML you have written so far. Let’s break it down to gain a better
understanding of what it all means.

# TargetType

To start, when defining a Style, you must define the TargetType. This
property defines which type of control the style definition targets and
therefore applies to. Defining a Style with only the TargetType property
set will apply to all controls of that type within the scope it is defined. This
is referred to as implicit styling.

If you wish to explicitly style a control, you can also add the x:Key
property. This is referred to as explicit styling. You are then required to set
the Style property on any control that wishes to use this explicit style that
you have created. You will be creating an explicit style in the “Creating a
style” section following shortly.

# ApplyToDerivedTypes

By default, styles created explicitly apply to the type defined in the
TargetType property I just covered. If you wish to allow derived classes to
also inherit this style, you need to set the ApplyToDerivedTypes property
to true.

# Setter

This is the part that looks and feels quite a bit different to the previous
XAML you have written. Since you are not creating controls but defining
how they will look, you must follow this syntax. Let’s look at the following
example:

```xml
<Style TargetType="Label">
 <Setter Property="TextColor" Value="Black" />
</Style>
```
The above is not a style you would include in an application; however,
as an example it allows you to say
The Style for Label controls will set the TextColor property to Black.
Now that you have had a look at some of the key concepts that make up
a style in .NET MAUI, let’s create your own style for your overlay.

# Creating a Style

Let’s view this in action by adding the following to the Styles.xaml file.
Add this just below the existing ```<Style TargetType=="Border">``` entry.

```xml
<Style TargetType="Border" x:Key="OverlayBorderStyle">
 <Setter Property="BackgroundColor" Value="White"
/> <Setter Property="Stroke" Value="{AppThemeBinding
Light={StaticResource Gray200}, Dark={StaticResource
Gray500}}" />
 <Setter Property="StrokeShape" Value="Rectangle"/>
 <Setter Property="StrokeThickness" Value="1"/>
</Style>
```
The above looks very similar to the default Border style already defined
with the addition of the BackgroundColor setter.
It is also worth noting that you only need to set the values that you
wish to change from the implicit style. Therefore, your explicit style can be
reduced down to

```xml
<Style TargetType="Border" x:Key="OverlayBorderStyle">
 <Setter Property="BackgroundColor" Value="White" />
</Style>
```
The Stroke, StrokeShape, and StrokeThickness properties will all be
inherited from the implicit global style. This provides yet another great way
to reduce the amount of code you need to write.
Now you can use this style in your application. Open the
FixedBoardPage.xaml file and add the following line to your Border
element (change in bold):

```xml
<Border
 IsVisible="{Binding IsAddingWidget}"
 HorizontalOptions="Center"
 VerticalOptions="Center"
 Padding="10"
 Style="{StaticResource OverlayBorderStyle}">     
```
This will result in your overlay looking far better to the user
now because it is no longer transparent. Also, consider moving the
HorizontalOptions, VerticalOptions, and Padding properties over to the
style definition. Figure 8-2 shows how much better the overlay now looks.

![image](https://user-images.githubusercontent.com/26972859/231237147-c32df356-9fe8-48c0-b0f8-d54eb77c3e83.png)
Figure 8-2. The overlay with a much clearer background

What you have done here is considered bad practice, though! You
have hardcoded the BackgroundColor of your Border control in the style
definition so your application will look great on a device running in light
mode. However, as soon as the user switches to dark mode, they will have a
glaring white border showing.

---
The repercussions of using fixed values can include text or content
disappearing entirely from the application. Imagine that the text color
switches to white in dark mode, with you having hardcoded to a
white background of the overlay view, so the user would see no text
on screen. This would result in a terrible user experience.
---

.NET MAUI provides the ability to handle the different modes that a
device can run under.

# AppThemeBinding

This is an extremely valuable concept. It allows you to define different
values based on whether the device your application is running on is set
to light or dark mode. Taking the example of the OverlayBorderStyle you
previously created, you can modify the Setter for BackgroundColor to

```xml
<Setter Property="BackgroundColor" Value="{AppThemeBinding
Light={StaticResource White}, Dark={StaticResource Black}}" />
```
Now if a user is running in dark mode, the border overlay will be black
and the text will be visible.
You only need to apply AppThemeBinding to properties that require a
visual distinction between light and dark mode. This typically applies to all
Brush/Color properties; however, you could conceivably decide to change
the StrokeThickness of your Border control, for example.

# Further Reading

It is worth noting that this book is limited to covering the styling options
in XAML. However, .NET MAUI does provide support for CSS-based
stylesheets. Go to https://docs.microsoft.com/dotnet/maui/userinterface/styles/css.

# Triggers

.NET MAUI provides a concept called triggers. They enable you to further
enhance how your views react to changes in the view model. You are given
the ability to define actions that can modify the appearance of the UI
based on event or data changes. Triggers provide us with another way of
changing the visibility of our border overlay for adding a new widget. The
initial work will appear more verbose in the short term but do bear with
me - it will result in a much better outcome!
There are a number of different types of triggers that can be attached
to a control, each with a varying level of functionality. You will take a brief
look at them and then dig into the one that you need for your scenario.

• Trigger: A Trigger represents a trigger that applies
property values, or performs actions, when the
specified property meets a specified condition.

• DataTrigger: A DataTrigger represents a trigger that
applies property values, or performs actions, when the
bound data meets a specified condition. The Binding
markup extension is used to monitor for the specified
condition.

• EventTrigger: An EventTrigger represents a trigger
that applies a set of actions in response to an event.
Unlike Trigger, EventTrigger has no concept
of termination of state, so the actions will not be
undone once the condition that raised the event is no
longer true.

• MultiTrigger: A MultiTrigger represents a trigger that
applies property values, or performs actions, when a set
of conditions are satisfied. All the conditions must be
true before the Setter objects are applied.

# Creating a DataTrigger

In this chapter, you have added your overlay Border control and are
currently changing its visibility through a binding direct to the IsVisible
property. You can write this differently with a DataTrigger. Let’s open the
FixedBoardPage.xaml file and modify the Border control to the following:

```xml
<Border
 IsVisible="False"
 HorizontalOptions="Center"
 VerticalOptions="Center"
 Padding="10"
 Style="{StaticResource OverlayBorderStyle}">
 <Border.Triggers>
 <DataTrigger
 TargetType="Border"
 Binding="{Binding IsAddingWidget}"
 Value="True">
 <Setter
 Property="IsVisible"
 Value="True" />
 </DataTrigger>
 </Border.Triggers>
```

Notice that the syntax for a Trigger is very similar to a Style. You
will also notice that it looks a lot more verbose than your original simple
binding approach. If you simply want to control the IsVisible property of
a control, a trigger is overkill, in my opinion. You will not be ending here,
though, so bear with me. First, let’s break down what you have added and
then look to how you can enhance it.

First, you modify the IsVisible property binding to false. This is the
initial state of the visibility of your view.

```xml
IsVisible="False"
```
Next, you add the DataTrigger to the Border.Triggers property.

```xml
<DataTrigger
 TargetType="Border"
 Binding="{Binding IsAddingWidget}"
 Value="True">
```
Much like with styles, you define the type of control the DataTrigger
applies to. You also set the Binding property to bind to the IsAddingWidget
property on your view model. Finally, you set the Value property of true.
This all means that when the IsAddingWidget property value is set to true,
the contents of the DataTrigger will be applied.
This leads you onto the final change, which is the setter

```xml
<Setter
 Property="IsVisible"
 Value="True" />
```
To repeat myself, all of this is rather verbose up until you consider
that you can define actions that can be performed when your state is
entered/exited.

# EnterActions and ExitActions

As an alternative to simply defining values for properties to be set when
the IsAddingWidget property value becomes true, like in your previous
example, you can define actions that will be performed when the value
enters or exits a specific state. What exactly does this mean? Let’s take a
look at an example. You can rewrite the trigger usage from the previous
example as

```xml
<DataTrigger
 TargetType="Border"
 Binding="{Binding IsAddingWidget}"
 Value="True">
 <DataTrigger.EnterActions>
 <!—-action to perform-->
 </DataTrigger.EnterActions>
 <DataTrigger.ExitActions>
 <!—-action to perform-->
 </DataTrigger.ExitActions>
</DataTrigger>
```
Given the above, you can state the following:
When the property (IsAddingWidget) in the Binding enters the state
defined in Value (True), the EnterActions will be performed.
When the property (IsAddingWidget) in the Binding exits the state
defined in Value (False), the ExitActions will be performed.
You need to define an action to be performed for these scenarios now.

# Creating a TriggerAction

.NET MAUI provides the TriggerAction<T> base class that allows you to
define an action that will be performed in the enter or exit scenario. This
enables you to build a more complex behavior that can be performed when
a value changes. When creating a trigger action, you can use the base class
TriggerAction<T> provided by .NET MAUI and then you need to override the
Invoke method. It is this method that defines what action will be performed
when the value changes. Let’s create your own action that you can use.

# Creating ShowOverlayTriggerAction

  First, you need to find a place to locate this action. Create a new folder
in the root project called Triggers and then add a new class file called
```ShowOverlayTriggerAction.cs```. Then you can adding the following code:

```Csharp
public class ShowOverlayTriggerAction :
TriggerAction<VisualElement>
{
 public bool ShowOverlay { get; set; }
 protected override void Invoke(VisualElement sender)
 {
 sender.IsVisible = ShowOverlay;
 }
}  
```
This code doesn’t do too much right now. It will just change the
IsVisible property of the control it is attached to when the value changes.
Now you need to attach it to your AddWidgetFrame control.

# Using ShowOverlayTriggerAction

You can now add in the action to perform sections that you left when
first adding a DataTrigger to your control. Modify your code in the
FixedBoardPage.xaml file, with the changes in bold.

```xml
<DataTrigger
 TargetType="Border"
 Binding="{Binding IsAddingWidget}"
 Value="True">
 <DataTrigger.EnterActions>
 <triggers:ShowOverlayTriggerAction ShowOverlay="True" />
 </DataTrigger.EnterActions>
  <DataTrigger.ExitActions>
 <triggers:ShowOverlayTriggerAction ShowOverlay="False" />
 </DataTrigger.ExitActions>
</DataTrigger>
```
 This can now be interpreted as, when the IsAddingWidget property
value changes to true, a ShowOverlayTriggerAction will be invoked with
ShowOverlay set to true. This will result in the AddWidgetFrame control
becoming visible. Then, when the IsAddingWidget property value changes to
false, a ShowOverlayTriggerAction will be invoked with ShowOverlay set to
false. This will result in the AddWidgetFrame control becoming invisible.
It is also worth noting that you can define triggers in styles, meaning
this type of functionality can be reused multiple times without having to
duplicate the code.
Let’s take a break from triggers for now to take a look at how you can
animate controls in .NET MAUI. Then you will return and combine triggers and
animations together to really show off the power of the action you just created. 
  
# Further Reading
  
You have only scratched the surface on the functionality that
can be achieved with triggers. I recommend checking out the
Microsoft documentation to see more ways triggers can be useful:
https://learn.microsoft.com/dotnet/maui/fundamentals/triggers.  

# Animations

This feels like it could be a challenging topic to show off in printed form
given the dynamic nature of an animation but it is one of my favorite topics
so I am going to show it off as best I can. Animations provide you with the
building blocks to make your applications feel much more natural and
organic.
  
.NET MAUI provides two main ways to perform an animation against
any VisualElement. You will take a look at each approach and how some
animations can be built using them.
  
# Basic Animations

  .NET MAUI ships with a set of prebuilt animations available via extension
methods. These methods provide the ability to rotate, translate, scale, and
fade a VisualElement over a period of time. Each of these methods have a
To suffix, for example ScaleTo. It is worth noting that each of the methods
for animating are asynchronous and will therefore need to be awaited
if you wish to know when they have finished. The full list of animation
methods are as follows:
  
|Method   |Description|
|:--------|:----------|
|FadeTo   |Animates the Opacity property of a VisualElement|  
|RelScaleTo|Applies an animated incremental increase or decrease to the Scale property of a VisualElement|  
|RotateTo|Animates the Rotation property of a VisualElement|
|RelRotateTo|Applies an animated incremental increase or decrease to the Rotation property of a VisualElement|
|RotateXTo|Animates the RotationX property of a VisualElement|
|RotateYTo|Animates the RotationY property of a VisualElement|
|ScaleTo|Animates the Scale property of a VisualElement|
|ScaleXTo|Animates the ScaleX property of a VisualElement|
|ScaleYTo|Animates the ScaleY property of a VisualElement|
|TranslateTo|Animates the TranslationX and TranslationY properties of a VisualElement|
  
The overlay view you added in the previous section just shows
immediately and disappears immediately based on the IsVisible binding
you created. What if you animate your overlay to grow from nothing up to
the required size? Don’t worry about adding this code to your application
just yet. You will look over some examples and then add it to Visual Studio
in the “Combining Triggers and Animations” section. The main reason for
not adding it immediately is because the animations API relies on direct
access to the view-related information, and this breaks the MVVM pattern.
However, once you look over how to animate, you can take this learning
and add it into your ShowOverlayTriggerAction implementation.
The code to animate a VisualElement is surprisingly small, as you can
see in the following example:
  
AddWidgetFrame.Scale = 0;
await AddWidgetFrame.ScaleTo(1, 500);
  
First, you make sure that the AddWidgetFrame has a Scale of 0 and then
you call ScaleTo, telling it to grow to a Scale of 1 (which is 100%) over a
duration of 500 milliseconds.
 
---
All of the prebuilt animation methods apart from the ones that start
with Rel perform the animation against the VisualElements
existing value (e.g., for ScaleTo it will change from the existing
Scale property value). This means that it is entirely possible that no
animation will take place if both the existing property and the value
provided to the method are the same.
---
  
# Combining Basic Animations
  
It is entirely possible to combine the basic animations to provide much
more complex animations. There are two main ways of achieving this.
  
# Chaining Animations 
  
You can chain animations together into a sequence. A common example
here is to provide the appearance of a tile being flipped over and giving a
3D effect to the user. The key detail when chaining animations is that you
await each animation method call to make sure that one animation has
finished before the next one begins.  
  
```Csharp
await frame.RotateXTo(90, 100);
frame.Content.IsVisible = tileViewModel.IsSelected;
await frame.RotateXTo(0, 100);  
```
  
# Concurrent Animations  
  
In a similar way to chaining, you can perform multiple animations
concurrently by simply not awaiting each method call or alternatively
awaiting all of the calls.  
  
```Csharp
AddWidgetFrame.Scale = 0;
AddWidgetFrame.IsVisible = true;
AddWidgetFrame.Opacity = 0;
await Task.WhenAll(
AddWidgetFrame.FadeTo(1),
AddWidgetFrame.ScaleTo(1, 500));  
```
In fact, this animation looks like a very good contender for your actual
implementation in the ShowOverlayTriggerAction implementation.  
  
# Cancelling Animations
  
Providing the ability to cancel an animation can be an extremely valuable
feature for a user. Quite often in applications, and predominantly games,
an animation will show when an action completes. Animations like this if
blocking can become tiresome for users especially if the same animation
repeats frequently. Therefore, a common pattern to follow is when the user
taps on the control being animated, it cancels the animation.
If you wish to cancel an animation, you can call the CancelAnimations
extension method on the VisualElement that you are animating.  
  
```AddWidgetFrame.CancelAnimations();```  
  
# Easings  
  
Animations in general will move mechanically as a computer changes a
value over time. Easings allow you to move away from a linear update of
those values in order to provide a much more organic and natural motion.
.NET MAUI offers a whole host of prebuilt easings, plus there is even the
ability to build your own if you really wish to do so. Let’s take a look at the
options that .NET MAUI provides out of the box:  
  
|Easing function    |Description|
|:--------|:----------|
|BounceIn   |Bounces the animation at the beginning|    
|BounceOut   |Bounces the animation at the end|    
|CubicIn   |Slowly accelerates the animation|    
|CubicInOut   |Accelerates the animation at the beginning and decelerates the animation at the end|  
|CubicOut   |Quickly decelerates the animation|  
|Linear   |Uses a constant velocity and is the default easing function|  
|SinIn   |Smoothly accelerates the animation|    
|SinInOut   |Smoothly accelerates the animation at the beginning and smoothly decelerates the animation at the end|    
|SinOut   |Smoothly decelerates the animation|    
|SpringIn   |Causes the animation to very quickly accelerate towards the end|    
|SpringOut   |Causes the animation to quickly decelerate towards the end|    
  
As a general guide, an easing ending with the In suffix will start the
animation slowly and speed up as it comes to a finish. An easing ending
with the Out suffix will start off quickly and slow down towards the end. 
  
# Complex Animations
  
NET MAUI provides the Animation class. This enables you to define
complex animation sequences. In fact, the prebuilt animations that you
covered in the “Basic Animations” section are built using this class inside
the .NET MAUI code. Using this class, it is possible to animate any visual
property of a VisualElement; for example, you can animate a change in
BackgroundColor or TextColor.
The Animation class provides the ability to define simple animations
through to really quite complex animations. Take a quick look at how
the ScaleTo animation can be implemented to understand what the
class offers.  
  
 # Recreating the ScaleTo Animation
  
 You can also animate the scale of your AddWidgetFrame control with the
following: 
  
```Csharp
public void ScaleTo()
{
 var animation = new Animation(v => AddWidgetFrame.Scale =
v, 0, 1);
 animation.Commit(AddWidgetFrame, "ScaleTo");
}
```
When creating an instance of the Animation class, you provide the
following parameter:  
  
```v => AddWidgetFrame.Scale = v```  
  
This is the callback parameter and it allows you to define what
property is set during the animation.
The next parameter is start. This is the starting value that will be
passed into the callback lambda you defined in the first parameter. In your
example, you set it to 0, meaning the AddWidgetFrame control will not be
visible because it has a scale of 0.
The final parameter you pass in is end. This is the resulting value that
will be passed into the callback lambda.
The animation will only begin when you call the Commit method. This
method also allows you to define how long it should take as well as how
often to call the callback parameter you defined.  
  
```animation.Commit(AddWidgetFrame, "ScaleTo", length: 2000);```  

This code shows the simplest type of animation you can create
within .NET MAUI. It is entirely possible to create much more complex
animations. To achieve this, you need to create an animation and then
add child animations in order to define the changes for each property and
different sequences in the animation.
  
# Creating a Rubber Band Animation

As an example on how to build a complex animation, I would like to show
you one of my favorite animations, the rubber band animation. This
animation simulates the VisualElement being pulled horizontally, letting
go, and then bouncing back to its original shape just like a rubber band
would. Figure 8-3 shows what it would look like, albeit in motion.
  
![image](https://user-images.githubusercontent.com/26972859/231241929-f63ccb12-3b27-4901-9b15-b6b14f1d9eeb.png)
Figure 8-3. The distinguishing frames from the animation you will
be building  
  
Let’s build the animation with the Animation class using the
understanding you gained in the previous section. 
  
```Csharp
public void Rubberband(VisualElement view)
{
 var animation = new Animation();
 animation.Add(0.00, 0.30, new Animation(v =>
view.ScaleX = v, 1.00, 1.25));
 animation.Add(0.00, 0.30, new Animation(v =>
view.ScaleY = v, 1.00, 0.75));
animation.Add(0.30, 0.40, new Animation(v =>
view.ScaleX = v, 1.25, 0.75));
 animation.Add(0.30, 0.40, new Animation(v =>
view.ScaleY = v, 0.75, 1.25));
 animation.Add(0.40, 0.50, new Animation(v =>
view.ScaleX = v, 0.75, 1.15));
 animation.Add(0.40, 0.50, new Animation(v =>
view.ScaleY = v, 1.25, 0.85));
 animation.Add(0.50, 0.65, new Animation(v =>
view.ScaleX = v, 1.15, 0.95));
 animation.Add(0.50, 0.65, new Animation(v =>
view.ScaleY = v, 0.85, 1.05));
 animation.Add(0.65, 0.75, new Animation(v =>
view.ScaleX = v, 0.95, 1.05));
 animation.Add(0.65, 0.75, new Animation(v =>
view.ScaleY = v, 1.05, 0.95));
 animation.Add(0.75, 1.00, new Animation(v =>
view.ScaleX = v, 1.05, 1.00));
 animation.Add(0.75, 1.00, new Animation(v =>
view.ScaleY = v, 0.95, 1.00));
 animation.Commit(view, "RubberbandAnimation",
length: 2000);
}
```
Yes, I know this looks quite different to the previous animation you
built. Let’s deconstruct the parts that feel unfamiliar.  
  
```  
animation.Add(0.00, 0.30, new Animation(v => view.ScaleX = v,
1.00, 1.25));
animation.Add(0.00, 0.30, new Animation(v => view.ScaleY = v,
1.00, 0.75));  
```  
The two lines above define the first transition in your animation. You
see that the ScaleX property will change from 1.00 (100%) to 1.25 (125%)
and the ScaleY property will change from 1.00 (100%) to 0.75% (75%) of
the control’s current size. This provides the appearance that the view is
being stretched. The key new part for you is the use of the Add method and
the first two parameters. This allows you to add the animation defined
as the third parameter as a child of the animation it is being added to.
The result is that when you Commit the main animation, all of the child
animations will be executed based on the sequence you defined in these
two first parameters. Let’s cover what these parameters mean.
The first parameter is the beginAt parameter. This determines when
the child animation being added will begin during the overall animation
sequence. So, in the example of your first line, you define 0.00, meaning it
will begin as soon as the animation starts.
The second parameter is the finishAt parameter. This determines
when the child being added will finish during the overall animation
sequence. So, in the example of your first line, you define 0.30, meaning it
will end 30% into the animation sequence.  
  
---
Both the beginAt and finishAt parameters should be supplied as
a value between 0 and 1 and considered a percentage in the overall
animation sequence. You will also notice that I tend to include the
decimal places even when they are 0; this really makes it easier to
read the animation sequence as it ensures that all of the code is
indented in the same way.
---

Finally, you call the Commit method as before to begin the animation
sequence.  
  
Now that you have covered building animations and some possible
examples of using them, let’s combine them with your trigger knowledge
to really make your AddWidgetFrame look great when it becomes visible.  
  
# Combining Triggers and Animations
  
Animations are a really powerful tool but they require view knowledge.
This is where having the ability to trigger them from a trigger allows you
to keep with the MVVM approach and keep your view and view model
cleanly separated.
Now that you have covered how to apply an animation to your overlay
view and looked at separating view from view model through the use of
triggers, you can combine the two together to trigger the animation and
keep the separation.
Let’s return to the ShowOverlayTriggerAction.cs file and add in
the animation from the “Concurrent animations” section (changes are
in bold).  
  
```Csharp
namespace WidgetBoard.Triggers;
public class ShowOverlayTriggerAction :
TriggerAction<VisualElement>
{
 public bool ShowOverlay { get; set; }
 protected async override void Invoke(VisualElement sender)
 {
 if (ShowOverlay)
 {
 sender.Scale = 0;
 sender.IsVisible = true;
 sender.Opacity = 0;
 await Task.WhenAll(
 sender.FadeTo(1),
 sender.ScaleTo(1, 500, Easing.SpringOut));
 }
 else
 {
 await sender.ScaleTo(0, 500, Easing.SpringIn);
 sender.Opacity = 0;
 sender.IsVisible = false;
 }
 }
}
```
  
The trigger action now provides two key visual changes when the
ShowOverlay property value changes. When the property becomes true,
the AddWidgetFrame control will both fade in over 250ms and scale up from
0 to 1 over 500ms. You also make use of the Easing.SpringOut option to
give a slightly more fluid feel to the changes in the animation.
When ShowOverlay becomes false, you just reverse the scale
animation to show it shrink. Once the animation has completed, you then
make sure that the control is no longer visible.
This concludes the sections on triggers and animations. You have seen
how they can help to both simplify the views and view models you create
while at the same time provide some really great functionality to make
your applications feel alive. I would recommend taking the application for
a spin and observing the animations in action, sadly we can’t show that
functionality off in printed form.  
  
# Summary  
  
In this chapter, you have

  • Provided the ability to add a widget to a board

  • Covered the different options available when showing
an overlay

  • Explored how you can define styling information for
your application

  • Learned how to handle devices running in light and
dark modes

  • Learned how to apply triggers to enhance your UI

  • Covered how to animate parts of your application

  • Explored what happens when you combine triggers
and animations

  In the next chapter, you will

  • Learn about the different types of local data.

  • Discover what .NET MAUI offers in terms of local file
storage locations and when to use each one.

  • Gain an understanding of database technologies and
apply two different options.

  • Modify your application to save and load the boards
your users create.

  • Gain an understanding of the options for storing small
bits of data or preferences.

  • Add the ability to record the last opened board.

  • Gain an understanding of the options for storing small
bits of data securely or SecureStorage.  
  
# Source Code  

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch08.  
  
# Extra Assignment  
  
I think you can take these animations to another level and really make your
application feel alive! Try the following possible extensions!  
  
# Animate the BoxView Overlay  
  
You’ve added an animation to present your Border with the widget
selection details inside. A nice further enhancement on this would be
to also animate the BoxView that you are using as your semi-transparent
overlay. I personally think a nice FadeTo animation would work well but I
would love to hear what works best for you.  
  
# Animate the New Widget  
  
To really make the application feel alive, you could consider animating
each widget as it is added onto the board. You have the Widgets_
ChildAdded method inside your BoardLayout.xaml.cs file where you set
the Position. You could consider expanding this method implementation
to also animate the new widget. Perhaps you could make the new widget
scale up similar to how your Border presents.  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  

