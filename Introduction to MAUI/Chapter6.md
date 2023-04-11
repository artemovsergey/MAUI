# CHAPTER 6

# Creating Our Own Layout

In the previous chapter, you learned a lot of the fundamentals of building
and binding your user interfaces. In this chapter, you will create your own
layout, make use of a variety of options when adding bindable properties,
provide command support from your layout, and make use of your layout
in your application. This will serve as the basis for adding much more
functionality as we cover a variety of different topics in future chapters.
Let’s recap what you achieved in the last chapter: you provided the
ability for a user to create a board and supply a number of columns and
rows. You now need to lay out your board with the number of columns
and rows the user has configured and populate widgets onto the board.
Figure 6-1 is a mock-up of what you will achieve by the end of this chapter.

![image](https://user-images.githubusercontent.com/26972859/231163936-8e925696-fdec-49e7-b357-8a514af45bc2.png)
Figure 6-1. Mockup of a board

At the end of the last chapter, I discussed the idea of having a second
type of layout in the “Extra Assignment” section. To continue with this
theme, I have structured the architecture of the layout to aid in this
journey. I am a fan of taking an approach like this because it allows you
to potentially replace one part of the implementation without impacting
the others.
BoardLayout will be responsible for displaying the widgets. It will be
assigned an ILayoutManager implementation, which will decide where
to place the widgets. You will be adding a FixedLayoutManager to decide
this part

# Placeholder
The first item that you need to create is the placeholder to show where a
widget will be placed. There isn’t too much to this control but creating it
allows you to group all of the related bits and pieces together. Figure 6-2
shows what your Placeholder control will look like when rendered inside
the application.

![image](https://user-images.githubusercontent.com/26972859/231164282-09388fa3-3f7c-4816-bcd4-62c45345ca51.png)
Figure 6-2. Mockup of the Placeholder control

In order to achieve the above look, you are going to make use of the
Border control. This is a really useful control. It allows you to provide 

borders, custom corner radius, shadows, and other styling options. It also
behaves much like the ContentView in that it can contain a single child
control.
Create a folder called Controls in your main project. It will house the
Placeholder control and potentially more as you build your application.
Next, add a new class to the folder and call it Placeholder. Note that
you are opting to create the control purely in C# without XAML; the main
reason is that it results in less code. I always find there is never a single
way to build things, and even if you like XAML, at times it doesn’t add any
value, just like in this scenario. Of course, if you prefer to build your UI with
XAML, you can do so.

```Csharp
namespace WidgetBoard.Controls;
public class Placeholder : Border
{
 public Placeholder()
 {
 Content = new Label
 {
 Text = "Tap to add widget",
 FontAttributes = FontAttributes.Italic,
 HorizontalOptions = LayoutOptions.Center,
 VerticalOptions = LayoutOptions.Center
 };
 }
 public int Position { get; set; }
}
```
As discussed, there isn’t too much to this implementation but let’s still
break it down. Here you have

- Created a control that inherits from Border
- Set the content of your control to be a Label showing
fixed text in an italic font and the text is centered both
horizontally and vertically
- Added a Position property to know where in the layout
it will be positioned

Now you can start building the layout that will display the placeholders
and ultimately your widgets.

# ILayoutManager

You have a slight chicken-and-egg scenario here. You need to create a
board and a layout manager, both of which need to know about the other;
therefore, let’s add in the LayoutManager parts first.
The purpose of the ILayoutManager interface is to define how the
BoardLayout will interact with a layout manager implementation.
Create a folder called Layouts in your main project. It will house the
ILayoutManager interface and more as you build your application.
Next, add a new class to the folder and call it ILayoutManager

```Csharp
namespace WidgetBoard.Layouts;
public interface ILayoutManager
{
 object BindingContext { get; set; }
 BoardLayout Board { get; set; }
 void SetPosition(BindableObject bindableObject, int
position);
}
```

Let’s break it down so you have a clear definition of what you just
created:
• The BindingContext property allows you to pass
the context down from the BoardLayout later. This is
important for allowing bindings on the layout manager.
• The Board property allows the manager to interact
directly with the board it is intended to assist.
• The SetPosition method allows the manager to use
the position parameter and set the appropriate layout
settings on the widget/placeholder.

# BoardLayout

Your BoardLayout will be the parent of your widgets. Create the layout
inside your Layouts folder.
• Right-click the Layouts folder.
• Select Add ➤ New Item.
• Select the .NET MAUI tab.
• Select the .NET MAUI ContentView (XAML) option.
• Enter the name BoardLayout.
• Click Add.
This will give you two files. You’ll modify each one individually

# BoardLayout.xaml

Modify the existing contents to the following:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Grid
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 x:Class="WidgetBoard.Layouts.BoardLayout"
 x:Name="self">
 <Grid
 x:Name="PlaceholderGrid" />
 <Grid
 x:Name="WidgetGrid"
 ChildAdded="Widgets_ChildAdded"
 BindableLayout.ItemsSource="{Binding ItemsSource,
Source={x:Reference self}}"
 BindableLayout.ItemTemplateSelector="{Binding
ItemTemplateSelector, Source={x:Reference self}}"
 InputTransparent="True"
 CascadeInputTransparent="False" />
</Grid>
```

You have added quite a bit to this that might not feel familiar, so again
let’s break it down.
Your main layout is a Grid and inside of it are two more Grids.
The first inner Grid (PlaceholderGrid) is where you add the
Placeholder control you created earlier in this chapter.
The second inner Grid (WidgetGrid) is where you add widgets. The
reason you have built the control this way is mainly so you can utilize a
really impressive piece of functionality that drastically reduces the amount
of code you have to write: BindableLayout.

---
You have not supplied a Grid.Row or Grid.Column to either of
your inner Grids. This results in both controls filling the space of the
parent Grid and the second one overlapping the first. This behavior
can provide some real power when building rather complex UIs.
---

# BindableLayout

BindableLayout allows you to turn a layout control into a control that
can be populated by a collection of data. BindableLayout is not a control
itself, but it provides the ability to enhance layout controls by adding an
ItemsSource property for bindings. This means that all of the layouts
you learned about in the previous chapter (e.g., Grid, AbsoluteLayout,
FlexLayout, HorizontalStackLayout, VerticalStackLayout) can be
turned into a layout that can show a specific set of controls for each item
that is provided. For this, you need to set two properties:
• BindableLayout.ItemsSource: This is the collection of
items that you wish to represent in the UI.
• BindableLayout.ItemTemplate or BindableLayout.
ItemTemplateSelector: This allows you to define
how the item will be represented. In most scenarios,
ItemTemplate is enough but this only works when you
have one type of item to display in your collection. If
you have multiple types, each widget will be a separate
type in your application, so you need to use the
ItemTemplateSelector.
I won’t actually be providing the source for these bindings just yet; this
will be done in Chapter 8. For now, you just need to make it possible to
bind them.

# BoardLayout.xaml.cs

Now that you have created your XAML representation, you need to add in
the code-behind, which will work with it. We are going to follow a slightly
different approach for this and the next section; you have a lot of code
to add now so you will add it in stages and we will talk around what you
are adding.
The initial code should look as follows:

```Csharp
namespace WidgetBoard.Layouts;
public partial class BoardLayout
{
 public BoardLayout()
 {
 InitializeComponent();
 }
}
```

# Adding the LayoutManager Property

You want to allow the consumer of your BoardLayout control to be able to
supply a LayoutManager that will control where the widgets are placed. For
this, you need to add the following:

```Csharp
private ILayoutManager layoutManager;
public ILayoutManager LayoutManager
{
 get => layoutManager;
 set
 {
 layoutManager = value;
 layoutManager.Board = this;
 }
}
```

The key detail of this implementation is how it assigns the Board
property on the LayoutManager to your BoardLayout control. This is to
allow the manager to interact with the layout.
One very important thing to consider is that when you create
properties that can be set in XAML, their setters can be called before your
control has its BindingContext property set. Therefore, you usually need
to handle both scenarios when relying on both pieces of functionality. To
give a concrete example of this, you have your LayoutManager property
that you have added. It will allow you to set bindings on it also, but it won’t
have a BindingContext passed down. For this, you need to override the
OnBindingContextChanged method in your BoardLayout class and assign
the value to your LayoutManager.

```Csharp
protected override void OnBindingContextChanged()
{
 base.OnBindingContextChanged();
 layoutManager.BindingContext = this.BindingContext;
}
```
In the past, I have found when building controls in this way, even if
you do not need to use this method for an actual implementation, it can
be really handy to debug what is going on when things don’t behave as
expected. For example, you can stick a breakpoint in to make sure that you
are being assigned a BindingContext and that it is of the correct type.

# Adding the ItemsSource Property

Your BoardLayout also needs to accept a collection of widgets that
will ultimately be displayed on screen. For controls that support 

```Csharp
using System.Collections;
```
This is to allow you to use the IEnumerable type.

```Csharp
public static readonly BindableProperty ItemsSourceProperty =
 BindableProperty.Create(
 nameof(ItemsSource),
 typeof(IEnumerable),
 typeof(BoardLayout));
 public IEnumerable ItemsSource
{
 get => (IEnumerable)GetValue(ItemsSourceProperty);
 set => SetValue(ItemsSourceProperty, value);
}
```
In the majority of scenarios, you bind an ObservableCollection to
an ItemsSource property, which is of a different type to IEnumerable. By
choosing to use IEnumerable, it allows the consumers of your layout to
provide any type that supports holding multiple items. This means that
you can supply an ObservableCollection or you can supply a List.
Finally, you need to add the using statement into your BoardLayout.
xaml.cs file at the top.

```Csharp
using System.Collections;
```
# Adding the ItemTemplateSelector Property

Now that you have a collection of items to display on screen, you
need to know how to display them. It can be common to see controls
that have an ItemsSource property also have an ItemTemplate or an
ItemTemplateSelector or even both properties. An ItemTemplate allows
a developer to define how each item in the ItemsSource will be rendered
on screen. The reason you aren’t using this approach is because you can
only define one template for all items. You will be binding your widget
view models to the ItemsSource property, which means you will have
several different views that you will want to display. This is where the
ItemTemplateSelector property comes in.

```Csharp
public static readonly BindableProperty
ItemTemplateSelectorProperty =
 BindableProperty.Create(
 nameof(ItemTemplateSelector),
 typeof(DataTemplateSelector),
 typeof(BoardLayout));
public DataTemplateSelector ItemTemplateSelector
{
 get => (DataTemplateSelector)GetValue(ItemTemplateSelector
Property);
 set => SetValue(ItemTemplateSelectorProperty, value);
}
```

You make use of the DataTemplateSelector type for your property
here. You will create an implementation a little later in this chapter but for
now it allows you to override the OnSelectTemplate method and provide a
suitable template for the item that is passed in.

# Handling the ChildAdded Event

I discussed earlier how the BindableLayout feature allows you to populate
a control with multiple views based on bindings. You need to hook into
the ChildAdded event so that your LayoutManager implementation can
determine where the new child should be positioned.

```Csharp
private void Widgets_ChildAdded(object sender,
ElementEventArgs e)
{
 if (e.Element is IWidgetView widgetView)
 {
 LayoutManager.SetPosition(e.Element, widgetView.
Position);
 }
}
```
This handler checks to see if the new child being added is of the
IWidgetView type, and if it is, it delegates out to the LayoutManager
implementation to set the widget’s position.

# Adding Remaining Bits

You have a few extra methods and properties to add in that will be used
by the FixedLayoutManager. Let’s add them and discuss their purpose
as you go.
Add the using statement at the top of the file.

```Csharp
using WidgetBoard.Controls;
```
Then add the first new method.

```Csharp
public void AddPlaceholder(Placeholder placeholder) =>
PlaceholderGrid.Children.Add(placeholder);
```
This method allows the caller to pass a placeholder that will be added
to PlaceholderGrid. This is useful when first loading a board or when
dealing with a widget being removed from a specific position.

```Csharp
public void RemovePlaceholder(Placeholder placeholder) =>
PlaceholderGrid.Children.Remove(placeholder);
```
This method allows the caller to pass a placeholder that will be
removed from the PlaceholderGrid. This is useful for when dealing with a
widget being added to a specific position.

```Csharp
public void AddColumn(ColumnDefinition columnDefinition)
{
 PlaceholderGrid.ColumnDefinitions.Add(columnDefinition);
 WidgetGrid.ColumnDefinitions.Add(columnDefinition);
}
```
This method allows for the board’s columns to be defined on both the
PlaceholderGrid and WidgetGrid.

```Csharp
public void AddRow(RowDefinition rowDefinition)
{
 PlaceholderGrid.RowDefinitions.Add(rowDefinition);
 WidgetGrid.RowDefinitions.Add(rowDefinition);
}
```
This method allows for the board’s rows to be defined on both the
PlaceholderGrid and WidgetGrid.

```Csharp
public IReadOnlyList<Placeholder> Placeholders =>
PlaceholderGrid.Children.OfType<Placeholder>().ToList();
```
This property provides all children from the PlaceholderGrid that are
of type Placeholder. This is to allow for determining which placeholder
needs to be removed when adding a widget.

# FixedLayoutManager

The final part for you to create is the FixedLayoutManager class. This will
provide the logic to
• Accept the number of rows and columns for a board.
• Provide tap/click support through a command.
• Build the board layout.
• Set the correct row/column position for each widget.
Create the file and then you can work through adding each of
the above pieces of functionality. Let’s add a new class file and call it
FixedLayoutManager.cs. Add the following content:

```Csharp
namespace WidgetBoard.Layouts;
public class FixedLayoutManager
{
}
```
To start, you are going to want to add the following using statements:

```Csharp
using System.Windows.Input;
using WidgetBoard.Controls;
```
And also make your class inherit from BindableObject and implement
your ILayoutManager interface. Your class should now look as follows:

```Csharp
using System.Windows.Input;
using WidgetBoard.Controls;

namespace WidgetBoard.Layouts;
public class FixedLayoutManager : BindableObject,
ILayoutManager
{
}
```
The reason for inheriting from BindableObject is down to the fact
that you need to add some bindable properties onto this class so that
developers using this implementation can bind values to the properties.

# Accepting the Number of Rows and Columns for a Board

You need to add the ability to set the number of rows and columns to be
displayed in your fixed layout board. For this, you are going to add two
bindable properties to your FixedLayoutManager class.

# Adding the NumberOfColumns Property

```Csharp
public static readonly BindableProperty
NumberOfColumnsProperty =
 BindableProperty.Create(
 nameof(NumberOfColumns),
 typeof(int),
 typeof(FixedLayoutManager),
 defaultBindingMode: BindingMode.OneWay,
 propertyChanged: OnNumberOfColumnsChanged);
public int NumberOfColumns
{
 get => (int)GetValue(NumberOfColumnsProperty);
 set => SetValue(NumberOfColumnsProperty, value);
}
static void OnNumberOfColumnsChanged(BindableObject bindable,
object oldValue, object newValue)
{
 var manager = (FixedLayoutManager)bindable;
 manager.InitialiseGrid();
}
```
The key difference with this implementation over the previous bindable
properties that you created is the use of the propertyChanged parameter. It
allows you to define a method (see OnNumberOfColumnsChanged) that will be
called whenever the property value changes.

---
The property changed method will only be called when the value
changes. This means that it may not be called initially if the value
does not change from the default value.
---

# Adding the NumberOfRows Property

```Csharp
public static readonly BindableProperty NumberOfRowsProperty =
 BindableProperty.Create(
 nameof(NumberOfRows),
 typeof(int),
 typeof(FixedLayoutManager),
 defaultBindingMode: BindingMode.OneWay,
 propertyChanged: OnNumberOfRowsChanged);
public int NumberOfRows
{
 get => (int)GetValue(NumberOfRowsProperty);
 set => SetValue(NumberOfRowsProperty, value);
}
static void OnNumberOfRowsChanged(BindableObject bindable,
object oldValue, object newValue)
{
 var manager = (FixedLayoutManager)bindable;
 manager.InitialiseGrid();
}
```
This is virtually identical to the NumberOfColumns property that you just
added, except for the NumberOfRows value.

# Providing Tap/Click Support Through a Command

The next item on your list is to provide the ability to handle tap/click
support. This is your first time providing command support; you used
commands in your bindings, but that was on the source side rather than
the target side like here.
First, you need to add the bindable property, which should start to feel
rather familiar.

```Csharp
public static readonly BindableProperty
PlaceholderTappedCommandProperty =
 BindableProperty.Create(
 nameof(PlaceholderTappedCommand),
 typeof(ICommand),
 typeof(FixedLayoutManager));
public ICommand PlaceholderTappedCommand
{
 get => (ICommand)GetValue(PlaceholderTappedCommand
Property);
 set => SetValue(PlaceholderTappedCommandProperty, value);
}
```
Next, you need to add the code that will execute the command. You
will be relying on the use of a TapGestureRecognizer by adding one to
your Placeholder control inside your InitialiseGrid method that you
will be adding in the next section. For now, you can add the method that
will be used so that you can focus on how to execute the command. Let’s
add the code and then look over the details.

```Csharp
private void TapGestureRecognizer_Tapped(object sender,
EventArgs e)
{
 if (sender is Placeholder placeholder)
 {
 if (PlaceholderTappedCommand?.CanExecute(placeholder.
Position) == true)
 {
 PlaceholderTappedCommand.Execute(placeholder.
Position);
 }
 }
}
```
You can see from the implementation that there are three main parts to
the command execution logic:
• First, you make sure that command has a value.
• Second, you check that you can execute the command.
If you recall back in Chapter 5 you provided a method
to prevent the command from executing if the user
hadn’t entered a BoardName.
• Finally, you execute the command and pass in the
command parameter. For this scenario, you will be
passing in the current position of the placeholder so
when a widget is added, it can be placed in the same
position.

# Building the Board Layout

Now you can focus on laying out the underlying Grids so that they display
as per the user’s entered values for rows and columns.

First, add in a property to store the current Board because you need to use
it when building the layout. You also need to record whether you have built the
layout to prevent any unnecessary updates rebuilding the user interface.

```Csharp
private BoardLayout board;
private bool isInitialised;
public BoardLayout Board
{
 get => board;
 set
 {
 board = value;
 InitialiseGrid();
 }
}
```
Your method to build the grid layout has several parts, so let’s add
them as you go and discuss their value. You initially need to make sure that
you have valid values for the Board, NumberOfRows and NumberOfColumns
properties plus you haven’t already built the UI.

```Csharp
private void InitialiseGrid()
{
 if (Board is null ||
 NumberOfColumns == 0 ||
 NumberOfRows == 0 ||
 isInitialised == true)
 {
 return;
 }
 isInitialised = true;
}
```
The next step is to use the NumberOfColumns value and add them to
your Board. Let’s add this to the end of the InitialiseGrid method.

```Csharp
for (int i = 0; i < NumberOfColumns; i++)
{
 Board.AddColumn(new ColumnDefinition(new GridLength(1,
GridUnitType.Star)));
}
```
The GridUnitType.Star value means that each column will have an
even share of the width of the grid. So, if the Grid is 300 pixels wide and
you have 3 columns, then each column has a resulting width of 100 pixels.
The next step is to use the NumberOfRows value and add them to your
Board. Let’s add this to the end of the InitialiseGrid method.

```Csharp
for (int i = 0; i < NumberOfRows; i++)
{
 Board.AddRow(new RowDefinition(new GridLength(1,
GridUnitType.Star)));
}
```
The final step in your InitialiseGrid method is to populate each cell
(row and column) combination with a Placeholder control.

```Csharp
for (int column = 0; column < NumberOfColumns; column++)
{
 for (int row = 0; row < NumberOfRows; row++)
 {
 var placeholder = new Placeholder();
 placeholder.Position = row * NumberOfColumns + column;
 var tapGestureRecognizer = new TapGestureRecognizer();
tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
 placeholder.GestureRecognizers.Add(tapGesture
Recognizer);
 Board.AddPlaceholder(placeholder);
 Grid.SetColumn(placeholder, column);
 Grid.SetRow(placeholder, row);
 }
}
```
In the above code, you
• Looped through the combinations of rows/columns
• Created a Placeholder control
• Set its position for use later
• Added a TapGestureRecognizer to handle user
interaction
• Added the Placeholder to the Board
• Positioned the Placeholder to the correct column and
row position

# Setting the Correct Row/Column Position for Each Widget

The final part in building the board layout is to provide the method
required by the ILayoutManager interface that your FixedLayoutManager
is implementing. This method will

• Calculate the column/row value based on the position
parameter passed in.
• Position the bindableObject parameter passed into the
calculated column and row position.
• Remove any existing Placeholder in the position.

```Csharp
public void SetPosition(BindableObject bindableObject, int
position)
{
 if (NumberOfColumns == 0)
 {
 return;
 }
 int column = position % NumberOfColumns;
 int row = position / NumberOfColumns;
 Grid.SetColumn(bindableObject, column);
 Grid.SetRow(bindableObject, row);
 var placeholder = Board.Placeholders.Where(p => p.Position
== position).FirstOrDefault();
 if (placeholder is not null)
 {
 Board.RemovePlaceholder(placeholder);
 }
}
```
Now that you have completed the work of providing a BoardLayout
and managing its layout with your FixedLayoutManager class, you should
go ahead and use it in your application.

# Using Your Layout

Before you can jump in and start using the BoardLayout you have created,
there is a little bit more work to be done. You need to
• Add a factory that will create instances of your widgets.
• Add in the DataTemplateSelector that I referred to
earlier on.
• Update your FixedBoardPageViewModel so your
bindings will work.

# Adding a Factory That Will Create Instances of Your Widgets

For this, you are going to create a new class called WidgetFactory in the
root of your project.

```Csharp
using WidgetBoard.ViewModels;
using WidgetBoard.Views;
namespace WidgetBoard;
public class WidgetFactory
{
}
```
There are three main purposes for this factory:
• Allows for the registration of widget views and
view models
• Creation of a widget view
• Creation of a widget view model
So, let’s support these three requirements.

# Allowing for the Registration of Widget Views and View Models

You need to add the following code:

```Csharp
private static IDictionary<Type, Type> widgetRegistrations =
new Dictionary<Type, Type>();
private static IDictionary<string, Type>
widgetNameRegistrations = new Dictionary<string, Type>();
public static void RegisterWidget<TWidgetView,
TWidgetViewModel>(string displayName) where TWidgetView :
IWidgetView where TWidgetViewModel : IWidgetViewModel
{
 widgetRegistrations.Add(typeof(TWidgetViewModel),
typeof(TWidgetView));
 widgetNameRegistrations.Add(displayName,
typeof(TWidgetViewModel));
}
public IList<string> AvailableWidgets =>
widgetNameRegistrations.Keys.ToList();
```
The above may look a little complicated but if you break it down,
hopefully it should become clear. You have added two fields that will store
the type information and name information needed for when you create
the instances of widgets.
The RegisterWidget method takes a display name parameter and
two types:
• TWidgetView: This must implement your IWidgetView
interface.
• TWidgetViewModel: This must implement your
IWidgetViewModel interface.

You then store a mapping between the view model type and the view
type (widgetRegistrations). This allows you to create a view when you
pass in a view model. This really helps you to keep a clean separation
between your view and view model.
You also store a mapping between the display name and the view
model type (widgetNameRegistrations). This will allow you to present an
option on screen to the user. Once they choose the name of the widget they
would like to add, the factory will create an instance of it.

# Creation of a Widget View

You first need to add a dependency to your constructor.

```Csharp
private readonly IServiceProvider serviceProvider;
public WidgetFactory(IServiceProvider serviceProvider)
{
 this.serviceProvider = serviceProvider;
}
```
The IServiceProvider will allow you to create a new instance
of your widgets and make sure that they are provided with all of
their dependencies. Don’t worry about needing to register the
IServiceProvider implementation with your MauiAppBuilder as you
have done with other dependencies that you require. This is automatically
provided by .NET MAUI.
Now let’s add the ability to create the widget view.

```Csharp
public IWidgetView CreateWidget(IWidgetViewModel
widgetViewModel)
{
 if (widgetRegistrations.TryGetValue(widgetViewModel.
GetType(), out var widgetViewType))
 {
   var widgetView = (IWidgetView)serviceProvider.GetRequir
edService(widgetViewType);
 widgetView.WidgetViewModel = widgetViewModel;
 return widgetView;
 }
 return null;
}
```
Breaking this down,
• You check whether the supplied widgetViewModels
type has been registered with the factory.
• If it has, you use the IServiceProvider to get an
instance of the associated widget view.
• You assign the widgetViewModel parameter value to the
WidgetViewModel property on the widget view. This is
to allow for the setting of the widgets BindingContext
property.

# Creation of a Widget View Model

You also need to provide the ability to create the widget view model
because this is required in your view model.

```Csharp
public IWidgetViewModel CreateWidgetViewModel(string
displayname)
{
 if (widgetNameRegistrations.TryGetValue(displayname, out
var widgetViewModelType))
 {
   return (IWidgetViewModel)serviceProvider.GetRequiredSer
vice(widgetViewModelType);
 }
 return null;
}
```
Breaking this down,
• You check whether the supplied displayname has been
registered with the factory.
• If it has, you use the IServiceProvider to get an
instance of the associated widget view model.

# Registering the Factory with MauiAppBuilder

Inside your MauiProgram.cs file, you need to register your WidgetFactory
with the MauiAppBuilder to make sure any dependencies can resolve it.
Open that file and add the following line into the CreateMauiApp method:

```Csharp
builder.Services.AddSingleton<WidgetFactory>();
```
# Registering Your ClockWidget with the Factory

Now that you have your WidgetFactory, you need to modify it so that the
factory can create the widget for you. This requires a number of steps, so
let’s walk through it.
First, open the ClockWidgetView.xaml.cs file and change it to the
following:

```Csharp
using WidgetBoard.ViewModels;
namespace WidgetBoard.Views;
public partial class ClockWidgetView : Label, IWidgetView
{
public ClockWidgetView(ClockWidgetViewModel
clockWidgetViewModel)
 {
 InitializeComponent();
 WidgetViewModel = clockWidgetViewModel;
 BindingContext = clockWidgetViewModel;
 }
 public IWidgetViewModel WidgetViewModel { get; set; }
}
```
This results in your ClockWidgetView taking a dependency on
ClockWidgetViewModel.
Next, you need to register your widget with the factory. Open
your MauiProgram.cs file and add the following lines to the
CreateMauiApp method:

```Csharp
WidgetFactory.RegisterWidget<ClockWidgetView, ClockWidgetView
Model>("Clock");
builder.Services.AddTransient<ClockWidgetView>();
builder.Services.AddTransient<ClockWidgetViewModel>();
```
This will enable the WidgetFactory to return the clock widget as an
option when presented in your overlay.

# WidgetTemplateSelector

The main purpose of this implementation is to provide a conversion
between the widget view models that you will be storing on your
FixedBoardPageViewModel and something that can actually be rendered
on the screen. You are going to depend on the WidgetFactory you have
just created. Create the class under the root project folder.

```Csharp
using WidgetBoard.ViewModels;
namespace WidgetBoard.Views;
public class WidgetTemplateSelector : DataTemplateSelector
{
 private readonly WidgetFactory widgetFactory;
 public WidgetTemplateSelector(WidgetFactory widgetFactory)
 {
 this.widgetFactory = widgetFactory;
 }
 protected override DataTemplate OnSelectTemplate(object
item, BindableObject container)
 {
 if (item is IWidgetViewModel widgetViewModel)
 {
 return new DataTemplate(() => widgetFactory.Create
Widget(widgetViewModel));
 }
 return null;
 }
}
```
The main part you need to focus on here is the OnSelectTemplate
method. I did discuss the purpose of this method briefly earlier on; let’s
take a deeper look now. Its main purpose is to provide a DataTemplate and
something that can be rendered on screen. This is a great way to keep the
separation between view and view model.
In your implementation, you can see that
• You check whether the item passed in implements your
IWidgetViewModel interface.
• If so, then you create a new DataTemplate and rely on
the WidgetFactory to return the widget view that is
mapped to the view models type.

# Registering the Template Selector with MauiAppBuilder

Inside your MauiProgram.cs file you need to register your
WidgetTemplateSelector with the MauiAppBuilder to make sure any
dependencies can resolve it. Open that file and add the following line into
the CreateMauiApp method:

```Csharp
builder.Services.AddSingleton<WidgetTemplateSelector>();
```
# Updating FixedBoardPageViewModel

You need to add in the properties that you can bind to in your view.

```Csharp
private string boardName;
private int numberOfColumns;
private int numberOfRows;
public string BoardName
{
 get => boardName;
 set => SetProperty(ref boardName, value);
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
public ObservableCollection<IWidgetViewModel> Widgets { get; }
public WidgetTemplateSelector WidgetTemplateSelector { get; }
```
Notice that the Widgets and WidgetTemplateSelector properties
do not call the SetProperty method to notify the UI of changes. This
is a perfectly valid scenario. You know that the value will be set in the
constructor and therefore the value will be set before the binding is
applied.
You also need to add in the remaining code to your
ApplyQueryAttributes method that you added in the last chapter. It
should now look like the following:

```Csharp
public void ApplyQueryAttributes(IDictionary<string,
object> query)
{
 var board = query["Board"] as Board;
 BoardName = board.Name;
 NumberOfColumns = ((FixedLayout)board.Layout).
NumberOfColumns;
 NumberOfRows = ((FixedLayout)board.Layout).NumberOfRows;
} 
```
Finally, you need to add the WidgetTemplateSelector as a
dependency in your constructor. It should now look like the following:

```Csharp
public FixedBoardPageViewModel(
 WidgetTemplateSelector widgetTemplateSelector
)
{
 WidgetTemplateSelector = widgetTemplateSelector;
 Widgets = new ObservableCollection<IWidgetViewModel>();
}
```
You are now ready to add the layout to your page.

# Finally Using the Layout

Now that you have built your layout, you should go ahead and use it. You
previously added the FixedBoardPage so you can go ahead and change it
to the following:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:layouts="clr-namespace:WidgetBoard.Layouts"
 xmlns:viewmodels="clr-namespace:WidgetBoard.ViewModels"
 x:Class="WidgetBoard.Pages.FixedBoardPage"
 Title="FixedBoardPage"
 x:DataType="viewmodels:FixedBoardPageViewModel">
  <layouts:BoardLayout
 ItemsSource="{Binding Widgets}"
 ItemTemplateSelector="{Binding Widget
TemplateSelector}">
 <layouts:BoardLayout.LayoutManager>
 <layouts:FixedLayoutManager
 NumberOfColumns="{Binding NumberOfColumns}"
 NumberOfRows="{Binding NumberOfRows}" />
 </layouts:BoardLayout.LayoutManager>
 </layouts:BoardLayout>
</ContentPage>
```
This now includes your shiny new BoardLayout complete with all the
bindings you have created to make it functional.

# Summary

In this chapter, you
• Created your own layout
• Made use of a variety of options when adding bindable
properties
• Provided command support from your layout
• Used your layout in your application
In the next chapter, you will
• Gain an understanding of what accessibility is
• Learn why it is important to build inclusive
applications
•Look at how you can make use of .NET MAUI
functionality
• Consider other scenarios and how to support them
• Look over some testing options to support your journey
to building accessible applications

# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch06.

# Extra Assignment

You will have noticed how a lot of the naming includes the word Fixed.
Let’s continue the extra assignment from the previous chapter and build
a board that is a variation of this approach. I really like the idea of a
freeform board where the user can position their widgets wherever they
like. This is a little more involved but if you consider how the BoardLayout
can use AbsoluteLayouts rather than Grids, then a new ILayoutManager
implementation should hopefully be where the alternative logic will need
to be applied. If you do embark on this journey, please feel free to share
your experience and findings.












































