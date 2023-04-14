# CHAPTER 13

# Lets Get Graphical

In this chapter, you will learn what .NET MAUI Graphics is, how it can
be used, and some practical examples of why you would want to use it.
You will also gain insight into some of the power provided by .NET MAUI
Graphics and how you can use it to build your own sketch widget with the
.NET MAUI GraphicsView control.

# .NET MAUI Graphics

.NET MAUI Graphics is another one of my favorite topics! I am currently
exploring the idea of building a game engine on top of it given the amount
of power it already offers. If you are interested in the game engine, please
feel free to check out the repository on GitHub at https://github.com/
bijington/orbit.

It has the potential to offer the ability for so much to be achieved,
things like rendering chart controls or other fancy concepts all through a
cross-platform API, meaning you only really need to focus on the problems
you are trying to solve and not worry about each individual platform.
Essentially .NET MAUI Graphics offers a surface that can render pixelperfect graphics on any platform supported by .NET MAUI. Consider .NET
MAUI Graphics as an abstraction layer, like .NET MAUI itself, on top of
the platform-specific drawing libraries. So we get all the power of each
platform but with a simple unified .NET API that we as developers can
work with.

# Drawing on the Screen

.NET MAUI provides GraphicsView, which you can use to draw shapes on
the screen. You need to assign the Drawable property on GraphicsView
with an implementation that knows how to draw. This implementation
must implement the IDrawable interface that defines a Draw method.

# Updating the Surface

In order to trigger the application or GraphicsView to update what
is rendered on screen, you must call the Invalidate method on
GraphicsView. This will then cause the IDrawable.Draw method to be
invoked and your code will be given the chance to update the canvas.
The way to interact with the ICanvas implementation is to first
set the values you need such as fill color (FillColor) or stroke color
(StrokeColor) and then call the draw method you are interested in
(FillSquare() or DrawSquare(), respectively).
Let’s look at some basic examples to get a better understanding of how
to use the graphics layer.

# Drawing a Line

Inside the Draw method you can interact with the ICanvas to draw a line
using the DrawLine method. The following code shows how this can be
achieved:

```Csharp
public void Draw(ICanvas canvas, RectF dirtyRect)
{
 canvas.StrokeColor = Colors.Red;
 canvas.StrokeSize = 6;
 canvas.DrawLine(0, 20, 100, 50);
}
```
You set StrokeColor and StrokeSize before calling the DrawLine
method. Order is important and you must set these properties before you
draw. Figure 13-1 shows the result of the Draw method from above.

![image](https://user-images.githubusercontent.com/26972859/231508864-5ace6d9a-dd35-4fe4-b873-de20e28904ad.png)
Figure 13-1. Drawing a line in .NET MAUI Graphics

In addition to drawing lines, you can draw many different shapes such
as ellipse, rectangle, rounded rectangle, and arc. You can draw even more
complex shapes through paths.

# Drawing a Path

Paths are not to be confused with the Shapes API provided with .NET
MAUI. Paths in .NET MAUI Graphics enable you to build up a set of
coordinates in order to draw a more complex shape.

```Csharp
public void Draw(ICanvas canvas, RectF dirtyRect)
{
 PathF path = new PathF();
 path.MoveTo(40, 10);
 path.LineTo(70, 80);
 path.LineTo(10, 50);
 path.Close();
 canvas.StrokeColor = Colors.Red;
 canvas.StrokeSize = 6;
 canvas.DrawPath(path);
}
```
You first build up a PathF through the MoveTo, LineTo, and Close
methods. The MoveTo method moves the current location of the path to the 
specified coordinates, and then the LineTo method draws a line from the
current location that you just set in MoveTo to the coordinates specified in
the LineTo method call. Finally, the Close method allows you to close the
path. This means that the final location will have a line added back to the
starting location. Notice that you didn’t explicitly add a LineTo(40, 10)
method call in; Close does this for you. Then you set the StrokeColor and
StrokeSize before calling the DrawPath method. Figure 13-2 shows the
result of the Draw method from above.

![image](https://user-images.githubusercontent.com/26972859/231509191-7a64d37f-a9e7-401e-b8c0-a0a8cf07a14a.png)
Figure 13-2. Drawing a path in .NET MAUI Graphics

It is this DrawPath method that you will be utilizing in the new widget
you will be building as part of this chapter.

# Maintaining the State of the Canvas

There can be times when you want to preserve some of the settings
that you apply to the canvas, such as properties like StrokeColor and
FillColor. All properties related to Stroke and Fill, plus others like
transformation properties, can be preserved. This can be done through
the SaveState method, which will save the current state. This saved state
can then be restored through the RestoreState method. It is also possible
to reset the current graphics state back to the default values with the
ResetState method. These three methods can provide a large amount
of functionality in specific scenarios. Say you have implemented a chart
rendering control where the chart is rendered and then each individual
series is rendered separately. You want to preserve the state of the charts
graphics settings but wish to reset each time you render a series (e.g., each
column in a bar chart).

# Further Reading

You have only scratched the surface of what is possible with the .NET
MAUI Graphics layer. I strongly recommend that you refer to the Microsoft
documentation at https://learn.microsoft.com/dotnet/maui/userinterface/graphics/ where it shows much more complex scenarios such
as painting patterns, gradients, images, rendering text, and much more.

# Building a Sketch Widget

My daughters love to doodle and leave me little notes when I am away
from my desk, so I thought why not give them the ability to draw digital
sketches and help save some trees. Let’s create a new widget and then
piece together this new drawing mechanic

# Creating the SketchWidgetViewModel

As with all of the widgets, you want to create a view model to accompany
the view. Let’s add a new class file into the ViewModels folder and call it
SketchWidgetViewModel.cs. Modify it with the following contents:

```Csharp
namespace WidgetBoard.ViewModels;
public class SketchWidgetViewModel : IWidgetViewModel
{
 public const string DisplayName = "Sketch";
 public int Position { get; set; }
 public string Type => DisplayName;
 public Task InitializeAsync() => Task.CompletedTask;
}
```
The view model is relatively simple as it only really needs to implement
the basics of the IWidgetViewModel interface. If you decided to add more
functionality into your widget, you have the infrastructure in place to do so.
Let’s now deal with the view and user interaction.


# Representing a User Interaction

When a user interacts with the new widget, they will be drawing on the
screen. You will need to record this interaction so that it can be rendered
inside the Draw method that the SketchWidgetView implements through
the IDrawable interface. Add a new a new class file, call it DrawingPath.cs
in the root of the project, and modify it to have the following contents:

```Csharp
public class DrawingPath
{
 public DrawingPath(Color color, float thickness)
 {
 Color = color;
 Thickness = thickness;
 Path = new PathF();
 }
 public Color Color { get; }
 public PathF Path { get; }
 public float Thickness { get; }
 public void Add(PointF point) => Path.LineTo(point);
}
```

The class has three main properties:

• Color represents the color of the line being drawn.

• Thickness represents how thick the line is.

• Path contains the points that make up the line.

You also have a single method that adds a new point into the Path
property. This ties in well with the .NET MAUI Graphics layer as you
receive the point when the user interacts with the surface and then you can
also use the same type to render the line on the screen.
Let’s create the widget view that will make use of this class.

# Creating the SketchWidgetView

As with each of the widget views, you will be creating a XAML-based view.
It will be inside the view where most of the logic resides because this
widget is largely view-related.
Add a new .NET MAUI ContentView (XAML) to your Views folder and
call it SketchWidgetView.

# Modifying the SketchWidgetView.xaml

The contents of the SketchWidgetView.xaml file should be modified to
the following. Remember that you want to keep your visual tree as simple
as possible. You only need to declare the GraphicsView itself and no other
container controls.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<GraphicsView
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 x:Class="WidgetBoard.Views.SketchWidgetView"
 StartInteraction="GraphicsView_StartInteraction"
 DragInteraction="GraphicsView_DragInteraction"
 EndInteraction="GraphicsView_EndInteraction" />
```
The GraphicsView provides several events that you can subscribe to
in order to handle the user’s interaction with the surface. You are only
interested in the following:

StartInteraction: This is when the user first interacts,
so basically when the first touch/mouse click happens.

• DragInteraction: This follows from the start and
involves the touch/mouse moving around on the
surface.

• EndInteraction: This is when the user lifts their finger
from the screen or mouse button.

When you add these events in the XAML file, it will automatically
create some C# code in the SketchWidgetView.xaml.cs file that you will
expand on shortly.

# Modifying the SketchWidgetView.xaml.cs

Visual Studio will have created this file for you already so you need to open
it and modify it to the following:

---
Note that the types in the event handlers have been shortened (e.g.,
from System.Object to object). This is mainly to make it clearer to read.
---

```Csharp
Using Microsoft.Maui.Controls;
using WidgetBoard.ViewModels;
namespace WidgetBoard.Views;
public partial class SketchWidgetView : GraphicsView,
IWidgetView, IDrawable
{
 public SketchWidgetView()
 {
 InitializeComponent();
 this.Drawable = this;
 }
 public IWidgetViewModel WidgetViewModel
 {
 get => BindingContext as IWidgetViewModel;
 set => BindingContext = value;
 }
 private void GraphicsView_StartInteraction(object sender,
TouchEventArgs e)
 {
 }
 private void GraphicsView_DragInteraction(object sender,
TouchEventArgs e)
 {
 }
 private void GraphicsView_EndInteraction(Object sender,
TouchEventArgs e)
 {
 }
 public void Draw(Icanvas canvas, RectF dirtyRect)
 {
 throw new NotImplementedException();
 }
}
```
---
Each of the event handles and the Draw method have the blank or
default implementation. Let’s build this file up slowly and discuss the
key parts as you do so.
---

First, you need to add the backing fields to store the interactions from
the user.

```Csharp
private DrawingPath currentPath;
private readonly IList<DrawingPath> paths = new
List<DrawingPath>();
```
The first event handler to modify is for the StartInteraction event.

```Csharp
private void GraphicsView_StartInteraction(object sender,
TouchEventArgs e)
{
 currentPath = new DrawingPath(Colors.Black, 2);
 currentPath.Add(e.Touches.First());
 paths.Add(currentPath);
 Invalidate();
}
```
In this method, you first create a new instance of the DrawingPath class,
assigning a color and thickness. They can, of course, be expanded to allow
selections from the user so they can have custom colors. Next, you add the
first touch into the current path so you have your first point of interaction.
Then you add the current path to the list of all paths so that they can
eventually be rendered on screen. Finally, you call Invalidate, which will
trigger the Draw method to be called and the paths can be drawn.
The next event handler to modify is for the DragInteraction event.

```Csharp
private void GraphicsView_DragInteraction(object sender,
TouchEventArgs e)
{
 currentPath.Add(e.Touches.First());
 Invalidate();
}
```
In this method, you add the current touch to the current path and
again call Invalidate to cause the Draw method to be called.
The final event handler to modify is for the EndInteraction event.

```Csharp
private void GraphicsView_EndInteraction(object sender,
TouchEventArgs e)
{
 currentPath.Add(e.Touches.First());
 Invalidate();
}
```
This has the exact same implementation as the DragInteraction event
handler.
The final set of changes to make is inside the Draw method so you can
actually see something on the screen.

```Csharp
public void Draw(ICanvas canvas, RectF dirtyRect)
{
 foreach (var path in paths)
 {
 canvas.StrokeColor = path.Color;
 canvas.StrokeSize = path.Thickness;
 canvas.StrokeLineCap = LineCap.Round;
 canvas.DrawPath(path.Path);
 }
}
```
This method loops through all of the paths that you have created
from the user interactions, setting the stroke color, size, and then drawing
the path that was built up by the three event handlers that you just
implemented.

# Registering Your Widget

The last part in your implementation of the sketch widget is to
register your view and view model with the MauiAppBuilder. Let’s
open up the MauiProgram.cs file and add the following lines into the
CreateMauiApp method:

```Csharp
WidgetFactory.RegisterWidget<SketchWidgetView, SketchWidgetView
Model>(SketchWidgetViewModel.DisplayName);
builder.Services.AddTransient<SketchWidgetView>();
builder.Services.AddTransient<SketchWidgetViewModel>();
```
# Taking Your Widget for a Test Draw

You should be able to run your application on all platforms, add a widget
of type Sketch to a board, and then interact with the widget to leave a fancy
doodle. Figure 13-3 shows the new sketch widget rendered on a board.

![image](https://user-images.githubusercontent.com/26972859/231511276-f60bea00-acb1-4c59-8953-b50a0c66f080.png)
Figure 13-3. The sketch widget showing my terrible doodling skills
running on macOS

# Summary

In this chapter, you have

• Learned what .NET MAUI Graphics is

• Gained an insight into some of the power provided by
.NET MAUI Graphics

• Built your own sketch widget with the .NET MAUI
GraphicsView control

In the next chapter, you will

• Explore the concepts of distributing your application

• Learn about concepts like continuous integration and
continuous delivery to improve your development
processes

• Learn about linking, what it is, and how it can benefit/
hinder you

• Learn why it is important to collect analytical and crash
information

• Explore why you might want to consider obfuscating
your code

# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch13.

# Extra Assignment

Think of another concept where you can use .NET MAUI graphics. Maybe
the chart control idea I discussed or even just showing the battery level in a
widget or other device information.





































































