# User Interface Design with XAML

We created a new .NET MAUI project called PassXYZ.Vault in the previous chapter. We will start to
improve it with the capabilities that we will master throughout this book. In this chapter, we will use
the master-detail pattern and XAML to design and build our app user interface.

The following topics will be covered in this chapter related to user interface design with XAML:

* How to create a XAML page

* Basic XAML syntax

* XAML markup extension

* How to design the user interface with the master-detail pattern

* Localization of the .NET MAUI app

The eXtensible Application Markup Language (XAML) is an XML-based language that is used to
define user interfaces for Windows Presentation Foundation (WPF), Universal Windows Platform
(UWP), Xamarin.Forms, and .NET MAUI. The XAML dialects in these platforms share the same
syntax but differ in their vocabularies.

XAML allows developers to define user interfaces in XML-based markup language rather than code.
It is possible to write all our user interfaces in code, but the user interface design with XAML will
be more succinct and more visually coherent. XAML cannot contain code. This is a disadvantage,
but it is also an advantage as it forces the developer to separate the code logic from the user interface
design. XAML is well suited for the Model-View-ViewModel (MVVM) pattern, which we will learn
about later in this book.

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC or Mac. Please refer to the Development environment setup section in Chapter 1, Getting
Started with .NET MAUI, for the details.
The source code for this chapter is available in the following GitHub repository:
https://github.com/PacktPublishing/.NET-MAUI-Cross-PlatformApplication-Development/tree/main/Chapter03

# Creating a XAML page

Before we learn XAML syntax, let’s learn how to create a XAML page in Visual Studio and via the
dotnet command line.
To create a XAML page using Visual Studio, we can right-click on the project node. After, select Add
> New Item…; we will see what’s shown in Figure 3.1:

![image](https://user-images.githubusercontent.com/26972859/231754789-552047a2-6e00-4a53-ba64-d6b597870a74.png)

Figure 3.1: Adding a XAML page

On this screen, select Content Page from the templates and click Add. This will create a pair of files
– a XAML file and a C# code-behind file.

We can do the same using the dotnet command.

To find all .NET MAUI templates, we can use the dotnet command like so in a PowerShell console:

```
dotnet new --list | findstr -i maui
.NET MAUI App maui [C#] MAUI/...
.NET MAUI Blazor App maui-blazor [C#] MAUI/...
.NET MAUI Class Library mauilib [C#] MAUI/...
.NET MAUI ContentPage (C#) maui-page-csharp [C#] MAUI/...
.NET MAUI ContentPage (XAML) maui-page-xaml [C#] MAUI/...
.NET MAUI ContentView (C#) maui-view-csharp [C#] MAUI/...
.NET MAUI ContentView (XAML) maui-view-xaml [C#] MAUI/...
.NET MAUI ResourceDictionary (XAML) maui-dict-xaml [C#] MAUI/...

```

From the preceding output, we can see that the short name of the XAML content page is
maui-page-xaml. We can create a XAML page using the following command:

```
dotnet new maui-page-xaml -n ItemsPage
The template ".NET MAUI ContentPage (XAML)" was created
successfully.
Processing post-creation actions…
The post action 84c0da21-51c8-4541-9940-6ca19af04ee6 is not
supported.
Description: Opens NewPage1.xaml in the editor.
```
Two files called ItemsPage.xaml and ItemsPage.xaml.cs will be created by the
preceding command.

# XAML syntax

Since XAML is an XML-based language, we need to understand basic XML syntax. An XML or
XAML file includes a hierarchy of elements. Each element may have attributes associated with it.
Let’s review App.xaml in the project that we created in Chapter 2, Building Our First .NET MAUI
App, as an example:

**Listing 3.1: App.xaml (https://epa.ms/App3-1)**

```xml
<?xml version = "1.0" encoding = "UTF-8" ?>
<Application xmlns="http://schemas.microsoft.com
 /dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/
 winfx/2009/xaml"
 xmlns:local="clr-namespace:PassXYZ.Vault"
 x:Class="PassXYZ.Vault.App">
 <Application.Resources>
 <ResourceDictionary...>
 </Application.Resources>
</Application>
```
As shown in Listing 3.1, we have an element called Application and its associated attributes, x:Class
and xmlns:{prefix}. Let’s analyze this example to understand XML elements and attributes.

# Element

The element syntax includes a start and end tag, such as the Application tag:

```xml
<Application></Application>
```
For an empty element, the end tag can be omitted by adding a forward slash at the end of the start
tag, like so:

```<Application />```

When we mention an XML element, we may use element, node, and tag as terms. When we say
element, we are referring to the start tag and the end tag of that element together. When we say tag,
we are referring to either the start or end tag of the element. When we say node, we are referring to
an element and all its inner content, including all child elements.

A XAML document is comprised of many nested elements. There is only one top element, which is
called the root element. In .NET MAUI or Xamarin.Forms, the root element is usually Application,
ContentPage, Shell, or ResourceDictionary.
For each XAML file, we usually have a corresponding C# code-behind file. Let’s review the codebehind file in Listing 3.2:

**Listing 3.2: App.xaml.cs (https://epa.ms/App3-2)**

```Csharp
using PassXYZ.Vault.Services;
using PassXYZ.Vault.Views;
namespace PassXYZ.Vault;
public partial class App : Application { ➊
 public App() {
 InitializeComponent(); ➋
 Routing.RegisterRoute(nameof(ItemDetailPage),
 typeof(ItemDetailPage));
 Routing.RegisterRoute(nameof(NewItemPage),
 typeof(NewItemPage));
 DependencyService.Register<MockDataStore>();
 MainPage = new AppShell();
 }
 private async void OnMenuItemClicked(System.Object
 sender, System.EventArgs e) {
 await Shell.Current.GoToAsync("//LoginPage");
 }
}
```
In XAML, elements usually represent actual C# classes that are instantiated to objects at runtime.
Together, the XAML and code-behind files define a complete class. For example, App.xaml (Listing
3.1) and App.xaml.cs (Listing 3.2) define the App class, which is a sub-class of Application.
➊ The App class, whose full name is PassXYZ.Vault.App, is the same as the one defined in the
XAML file using the x:Class attribute:

```
x:Class="PassXYZ.Vault.App"
```

➋ In the constructor of the App class, the InitializeComponent() method is called to load
the XAML and parse it. UI elements defined in the XAML file are created at this point. We can access
these UI elements by the name defined with the x:Name attribute, as we’ll see shortly.

# Attribute

An element can have multiple unique attributes. An attribute provides additional information about
XML elements. An XML attribute is a name-value pair attached to an element. In XAML, an element
represents a C# class and attributes represent the members of this class:

```xml
<Button x:Name="loginButton" VerticalOptions="Center" IsEnabled="True" Text="Login"/>
```
As we can see, four attributes – x:Name, VerticalOptions, IsEnabled, and Text – are
defined for the Button element. To define an attribute, we need to specify the attribute’s name and
value with an equal sign. We need to put the attribute value in double or single quotes. For example,
IsEnabled is the attribute name and "True" is the attribute value.
In this example, the x:Name attribute is a special one. It does not refer to a member of the Button
class, but it refers to the variable holding the instance of the Button class. Without the x:Name
attribute, an anonymous instance of the Button class will be created. With the x:Name attribute
declared, we can refer to the instance of the Button class using the loginButton variable in the
code-behind file.

# XML namespaces and XAML namespaces

In XML or XAML, we can declare namespaces just like we do in C#. Namespaces help to group elements
and attributes to avoid name conflicts when the same name is used in a different scope. Namespaces
can be defined using the xmlns attribute with the following syntax:

```xmlns:prefix="identifier"```

The XAML namespace definition has two components: a prefix and an identifier. Both the prefix and
the identifier can be any string, as allowed by the W3C namespaces in the XML 1.0 specification. If
the prefix is omitted, the namespace is the default namespace. In Listing 3.1, the following namespace
is the default one:

```xmlns="http://schemas.microsoft.com/dotnet/2021/maui"````

This default namespace allows us to refer to .NET MAUI classes without a prefix, such as ContentPage,
Label, or Button.

For the namespace declaration, use the x prefix, like so:

```xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"```

The xmlns:x namespace declaration specifies elements and attributes that are intrinsic to XAML.
This namespace is one of the most important ones that we will use in the UI design with XAML. To
understand how to use it, we can create a content page with the same structure using both C# and XAML.
To create a content page in XAML, we can use the dotnet command, as we did previously:

```
dotnet new maui-page-xaml -n NewPage1
The template ".NET MAUI ContentPage (XAML)" was created
successfully.
Processing post-creation actions...
The post action 84c0da21-51c8-4541-9940-6ca19af04ee6 is not
supported.
Description: Opens NewPage1.xaml in the editor.

```

The preceding command generates a XAML file (NewPage1.xaml) and a C# code-behind file
(NewPage1.xaml.cs). We can update the XAML file to the following. Since we aren’t adding any
logic, we can ignore the code-behind file (NewPage1.xaml.cs) in this example:

**NewPage1.xaml**

<ContentPage
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 x:Class="MauiApp1.NewPage1" ❶
 Title="NewPage1">
 <StackLayout x:Name="layout"> ➋
 <Label Text="Welcome to .NET MAUI!"
 VerticalOptions="Center"
 HorizontalOptions="Center" />
<BoxView HeightRequest="150" WidthRequest="150"
 HorizontalOptions="Center">
 <BoxView.Color>
 <Color x:FactoryMethod="FromRgba"> ➌
 <x:Arguments> ➍
 <x:Int32>192</x:Int32> ❺
 <x:Int32>75</x:Int32>

<x:Int32>150</x:Int32>
 <x:Int32>128</x:Int32>
 </x:Arguments>
 </Color>
 </BoxView.Color>
 </BoxView>
 </StackLayout>
</ContentPage>

```

**NewPage1.xaml.cs**

```Csharp
namespace MauiApp1;
public partial class NewPage1 : ContentPage {
 public NewPage1() {
 InitializeComponent();
 }
}
```
We can also generate a content page in C# code only. Let’s create a content page using the
following command:

```Csharp
dotnet new maui-page-csharp -n NewPage1
The template ".NET MAUI ContentPage (C#)" was created
successfully.
Processing post-creation actions...
The post action 84c0da21-51c8-4541-9940-6ca19af04ee6 is not
supported.
Description: Opens NewPage1.cs in the editor.
```
The preceding command generates a content page in the NewPage1.cs C# file. We can implement
the same logic in C# like so:

**NewPage1.cs**

```Csharp
namespace MauiApp1;
public class NewPage1 : ContentPage { ➀
 public NewPage1() {
 var layout = new StackLayout ➁
 {
 Children = {
 new Label { Text = "Welcome to .NET MAUI!" },
 new BoxView {
 HeightRequest = 150,
 WidthRequest = 150,
 HorizontalOptions = LayoutOptions.Center,
 Color = Color.FromRgba(192, 75, 150, 128) ➂
 }
 }
 };
 Content = layout;
 }
}
```
Here, we have created the same content page (NewPage1) twice in both XAML and C#. XAML cannot
contain programming logic, but it can be used to declare user interface elements and put the logic
in the C# code-behind file. Inside NewPage1, we created a content page that contains Label and
BoxView elements. In the XAML version, we used attributes defined in the xmlns:x namespace
to specify the UI elements:
* ❶ A content page called NewPage1 is created in XAML. The x:Class attribute specifies
the class name – that is, NewPage1. In the C# code-behind file, a partial class of NewPage1
is defined. In the constructor, the InitializeComponent() method is invoked to load
the UI defined in XAML.

* ➀ We can create the same content page, NewPage1, using C# directly as a derived class
of ContentPage.
We defined a StackLayout in the content page and the variable name referring to it is layout
in both the XAML and C# versions:

* ➋ In XAML, x:Name specifies the variable name of StackLayout.

* ➁ In C#, we can declare the variable as layout.

* ➌ x:FactoryMethod specifies a factory method that can be used to initialize an object.

* ➂ In C# code, we can call the Color.FromRgba() function directly, but we have to use
the x:FactoryMethod attribute in XAML to do the same.

* ➍ x:Arguments is used to specify arguments when we call Color.FromRgba() in XAML.

* ❺ x:Int is used to specify integer arguments. For other data types, we can use x:Double,
xChar, or x:Boolean.

```xmlns:local="clr-namespace:PassXYZ.Vault"```

To declare a CLR namespace, we can use clr-namespace: or using:. If the CLR namespace
is defined in a different assembly, assembly= is used to specify the assembly that contains the
referenced CLR namespace. The value is the name of the assembly without the file extension. In our
case, it has been omitted since the PassXYZ.Vault namespace is within the same assembly as our
application code.
We will see more uses of namespaces later in this chapter.

## XAML markup extensions

Even though we can initialize class instances using XAML elements and set class members using
XAML attributes, we can only set them as predefined constants in a XAML document.
To enhance the power and flexibility of XAML by allowing element attributes to be set from a variety
of sources, we can use XAML markup extensions. With XAML markup extensions, we can set an
attribute to values defined somewhere else, or a result processed by code at runtime.
XAML markup extensions can be specified in curly braces, as shown here:

```xml
<Button Margin="0,10,0,0" Text="Learn more"
 Command="{Binding OpenWebCommand}"
 BackgroundColor="{DynamicResource PrimaryColor}"
 TextColor="White" />
```
In the preceding code, both the BackgroundColor and Command attributes have been set to
markup extensions. BackgroundColor has been set to DynamicResource and Command has
been set to the OpenWebCommand method defined in the view model.
We will use markup extensions in the next few chapters to support data binding and ResourceDictionary.
We will learn more about markup extensions when we use them later. Please refer to the following
Microsoft documentation to find out more information about it: https://learn.microsoft.
com/en-us/dotnet/maui/xaml/markup-extensions/consume.
Now that we’ve learned the basics about XAML, we can use it to work on our user interface design.

# Master-detail UI design

We have learned basic knowledge about XAML in the previous sections. Now, let’s spend some time
exploring the application that we are going to develop.
The master-detail pattern is commonly used in user interface design. Many examples can be found
in frequently used apps. For example, in the Mail app of Windows, a list of emails is displayed in the
master view, as well as the details of the selected email:

![image](https://user-images.githubusercontent.com/26972859/231760460-4e03aca0-7eb4-4c38-8a58-6927bb9bd13c.png)

Figure 3.2: Mail in Windows

In Figure 3.2, there are three panels in the design. The left panel looks like a navigation drawer. When
we select a folder from the left panel, a list of emails is displayed in the middle panel. The currently
selected email is displayed in the right panel.

Note

Navigation drawers provide access to destinations and app functionality, such as the menu in
the desktop environment. It typically slides in from the left and is triggered by tapping an icon
in the top-left corner of the screen. It displays a list of choices to navigate to and is widely used
in mobile and web user interface design. Xamarin.Forms or .NET MAUI Shell uses navigation
drawers as their top-level navigation methods.

The original KeePass UI design, shown in Figure 3.3, also uses three panels (left, right, and bottom) on
the main page. The left panel is a classic tree view that acts like a navigation drawer. The right panel is
used to display the list of password entries. The bottom panel is used to display the details of an entry:

![image](https://user-images.githubusercontent.com/26972859/231760871-93e44fd3-fe9a-4ff9-86cf-feebcc8758dd.png)

Figure 3.3: KeePass UI design

The master-detail pattern works well on a wide range of device types and display sizes.
Considering different display sizes, two popular modes can be used:

* Side-by-side

* Stacked

# Side-by-side

When we have plenty of horizontal space with a large display, the side-by-side approach is usually a
good choice. The Mail app in Figure 3.2 and the KeePass app in Figure 3.3 are good examples. In this
mode, we can see both the master view and the detail view at the same time.

# Stacked

When we use a mobile device, we usually have a smaller screen size and the vertical space is larger
than the horizontal one. The stacked approach is a better choice in this case.
In stacked mode, the master view gets the full-screen space. Then, when a selection is made, the detail
view gets the full-screen space:

![image](https://user-images.githubusercontent.com/26972859/231761104-5a333eb2-40a8-4919-84cc-18bce6af8565.png)

Figure 3.4: PassXYZ.Vault

In Figure 3.4, we can see how we can navigate our app from the user’s point of view. We have a list of
flyout items that we can choose from:

* About

* Browse

* Logout

When we choose Browse, we can see the list of items on the master page (ItemsPage). On this
page, if we choose an item, we will go to the item’s detail page (ItemDetailPage). If we want to
choose another item, we must go back to the master page and select another item.
We will discuss flyout items in Chapter 5, Introducing Shell and Navigation. In this section, we will
review the implementation of ItemsPage and ItemDetailPage. However, before we go into
the details, let’s study the layout, which is the container of user interface elements.

# Controls in .NET MAUI

The user interface of the .NET MAUI app is created using controls. These controls can be categorized
as pages, layouts, and views.

A page is the top-level user interface element that usually occupies all the screens or windows. We
introduced how to create pages using the Visual Studio template or dotnet command at the beginning
of this chapter. Each page typically contains at least one layout element, which is used to organize the
design of controls on a page.

Views are the UI objects to present, edit, or initiate commands in the user interface design. Please
refer to the following Microsoft document about the controls in .NET MAUI: https://learn.
microsoft.com/en-us/dotnet/maui/user-interface/controls/.

We will introduce layouts in the next section. In this section, we’ll go through some controls that will
be frequently used in this book. Please refer to the preceding link for more details.

**Label**

Label is used to display single-line or multi-line text. It can display text with a certain format, such
as color, space, text decorations, and even HTML text. To create a Label, we can use the simplest
format, like so:

```<Label Text="Hello world" />```

**Image**

In the user interface design, we usually use icons to decorate other controls or display images as
backgrounds. The Image control can display an image from a local file, a URI, an embedded
resource, or a stream. The following code shows an example of how to create an Image control in
the simplest form:

```<Image Source="dotnet_bot.png" />```

**Editor**

In our app, the users need to enter or edit a single line of text or multiple lines of text. We have two
controls to serve this purpose: Editor and Entry.

Editor can be used to enter or edit multiple lines of text. The following is an example of the
Editor control:

```<Editor Placeholder="Enter your description here" />```

**Entry**

Entry can be used to enter or edit a single line of text. To design a login page, we can use Entry
controls to enter a username and password. When users interact with an Entry, the behavior of the
keyboard can be customized through the Keyboard property. When users enter their passwords,
the IsPassword property can be set to reflect the typical behavior on a login page. The following
is an example of a password entry:

```<Entry Placeholder="Enter your password" Keyboard="Text" IsPassword="True" />```

**ListView**

In the user interface design, a common use case is to display a collection of data. In .NET MAUI, a
few controls can be used to display a collection of data, such as CollectionView, ListView,
and CarouselView. In our app, we will use ListView to display password entries, groups, and
the content of an entry. We will introduce the usage of ListView when we introduce ItemsPage.

# Layouts in .NET MAUI

To design user interface elements in a view or page, we usually use layout control as a container to
define the presentation format. There are a few layouts that are core to .NET MAUI.

# StackLayout

StackLayout organizes elements in a one-dimensional stack, either horizontally or vertically. It is
often used as a parent layout, which contains other child layouts. The default orientation is vertical.
However, we should not use StackLayout to generate a layout similar to a table by using nested
StackLayout horizontally and vertically. The following code shows an example of bad practice:

```xml
<StackLayout>
 <StackLayout Orientation="Horizontal">
 <Label Text="Name:" />
 <Entry Placeholder="Enter your name" />
 </StackLayout>
 <StackLayout Orientation="Horizontal">
 <Label Text="Age:" />
 <Entry Placeholder="Enter your age" />
  </StackLayout>
 <StackLayout Orientation="Horizontal">
 <Label Text="Address:" />
 <Entry Placeholder="Enter your address" />
 </StackLayout>
</StackLayout>
```
In the preceding code, we used a StackLayout as the parent layout, where the default orientation is
vertical. Then, we nested multiple StackLayout controls with a horizontal orientation to generate
a form to fill in. We should use the Grid control to do this.
StackLayout is a frequently used layout control. There are two sub-types of StackLayout that
help us directly design the layout horizontally or vertically

**HorizontalStackLayout**

HorizontalStackLayout is a one-dimensional horizontal stack. For example, we can generate
a row like so:

```xml
 <HorizontalStackLayout>
 <Label Text="Name:" />
 <Entry Placeholder="Enter your name" />
 </HorizontalStackLayout>
VerticalStackLayout
VerticalStackLayout is a one-dimensional vertical stack. For example, we can display an error
message after a form is submitted with an error like so:
<VerticalStackLayout>
 <Label Text="The Form Is Invalid" />
 <Button Text="OK"/>
</VerticalStackLayout>
```

**Grid**

Grid organizes elements in rows and columns. We can specify rows and columns with the
RowDefinitions and ColumnDefinitions properties. In the previous example, we created
a form where the user can enter their name, age, and address using a nested StackLayout. We can
do this in the Grid layout like so:

```xml
<Grid>
 <Grid.RowDefinitions>
<RowDefinition Height="50" />
 <RowDefinition Height="50" />
 <RowDefinition Height="50" />
 </Grid.RowDefinitions>
 <Grid.ColumnDefinitions>
 <ColumnDefinition Width="Auto" />
 <ColumnDefinition />
 </Grid.ColumnDefinitions>
 <Label Text="Name:" />
 <Entry Grid.Column="1"
 Placeholder="Enter your name" />
 <Label Grid.Row="1" Text="Age:" />
 <Entry Grid.Row="1" Grid.Column="1"
 Placeholder="Enter your age" />
 <Label Grid.Row="2" Text="Address:" />
 <Entry Grid.Row="2"
 Grid.Column="1"
 Placeholder="Enter your address" />
</Grid>
```
In the preceding example, we created a Grid layout with two columns and three rows.

# FlexLayout

FlexLayout is similar to a StackLayout in that it displays child elements either horizontally
or vertically in a stack. The difference is a FlexLayout can also wrap its children if there are too
many to fit in a single row or column. As an example, we can create a FlexLayout with five labels
in a row. If we specify the Direction property as Row, these labels will be displayed in one row.
We can also specify the Wrap property, which can cause the items to wrap to the next row if there
are too many items to fit in a row:

```xml
<FlexLayout Direction="Row" Wrap="Wrap">
 <Label Text="Item 1" Padding="10"/>
 <Label Text="Item 2" Padding="10"/>
 <Label Text="Item 3" Padding="10"/>
 <Label Text="Item 4" Padding="10"/>
 <Label Text="Item 5" Padding="10"/>
 </FlexLayout>
```

# AbsoluteLayout

AbsoluteLayout is a layout type that we can use to position elements using X, Y, width, and height.
The X and Y positions are relative to the top-left corner of the parent element. Width and height are
concerned with the size of the child element.

In the following example, we are creating a BoxView control in the layout at (0, 0) with both width
and height equal to 10:

```xml
<AbsoluteLayout Margin="20">
 <BoxView Color="Silver"
 AbsoluteLayout.LayoutBounds="0, 0, 10, 10" />
</AbsoluteLayout>
```
# Navigation in the master-detail UI design

As shown in Figure 3.4, we are using a stacked master-detail pattern in our navigation. There is a
flyout menu to display a list of pages. In the list of pages, a page of the ItemsPage type is used to
display a list of password entries. When users click an entry, details about the password entry are
shown in ItemDetailPage.

Let’s review the implementation of ItemsPage and ItemDetailPage.

# ItemDetailPage

In our app, ItemDetailPage is the detail page of the master-detail pattern, and it shows the content
of an item. In ItemDetailPage, we simply present the Item data model. It looks very simple at
the moment; we will enhance it gradually throughout this book:

**Listing 3.3: Item.cs (https://epa.ms/Item3-3)**

```xml
using System;
namespace PassXYZ.Vault.Models {
 public class Item {
 public string Id { get; set; }
 public string Text { get; set; }
 public string Description { get; set; }
 }
}
```

As shown in Listing 3.3, the Item class includes three properties called ID, Text, and Description.
The instance of Item is loaded by the LoadItemId() function in ItemDetailViewModel, as
shown here. We will discuss the MVVM pattern in the next chapter:

```Csharp
public async void LoadItemId(string itemId) {
 try {
 var item = await DataStore.GetItemAsync
 (itemId);
 Id = item.Id;
 Text = item.Text;
 Description = item.Description;
 }
 catch (Exception) {
 Debug.WriteLine("Failed to Load Item");
 }
 }
```
Once the data has been loaded, we can present the data to the user in ItemDetailPage.xaml,
as shown in Listing 3.4:

**Listing 3.4: ItemDetailPage.xaml (https://epa.ms/ItemDetailPage3-4)**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com
 /dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com
 /winfx/2009/xaml"
 x:Class="PassXYZ.Vault.Views.ItemDetailPage"
 Title="{Binding Title}">
 <StackLayout Spacing="20" Padding="15">
 <Label Text="Text:" FontSize="Medium" />
 <Label Text="{Binding Text}" FontSize="Small"/>
 <Label Text="Description:" FontSize="Medium" />
 <Label Text="{Binding Description}"
 FontSize="Small"/>
 </StackLayout>
</ContentPage>
```

Listing 3.4 is the XAML file of ItemDetailPage. The content page of the item detail includes an
instance of StackLayout and four instances of Label.

In StackLayout, the default orientation is Vertical, so the Label controls are organized vertically
on the item detail page (see Figure 3.4). Both Text and Description are connected to the model
data in the view model through data binding. We will introduce data binding in the next chapter.

**ItemsPage**

ItemsPage is the master page of the master-detail pattern in our app. It displays a list of items that
we can explore.

Listing 3.5 is the implementation of ItemsPage. To display a list of items, a ListView control is
used. ListView is a control used to display a scrollable vertical list of selectable data items:

**Listing 3.5: ItemsPage.xaml (https://epa.ms/ItemsPage3-5)**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com
 /dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com
 /winfx/2009/xaml"
 x:Class="PassXYZ.Vault.Views.ItemsPage" ➊
 Title="{Binding Title}"
 xmlns:local="clr-namespace:
 PassXYZ.Vault.ViewModels" ❺
 xmlns:model="clr-namespace:PassXYZ.
 Vault.Models" ❻
 x:DataType="local:ItemsViewModel" ➋
 x:Name="BrowseItemsPage"> ➌
 <ContentPage.ToolbarItems...>
 <StackLayout>
 <ListView x:Name="ItemsListView" ➍
 ItemsSource="{Binding Items}"
 VerticalOptions="FillAndExpand"
 HasUnevenRows="False"
 RowHeight="84"
 RefreshCommand="{Binding LoadItems
 Command}"
 IsPullToRefreshEnabled="true"
 IsRefreshing="{Binding IsBusy,
 Mode=OneWay}"
 CachingStrategy="RetainElement"
 ItemSelected="OnItemSelected">
 <ListView.ItemTemplate>
 <DataTemplate...>
 </ListView.ItemTemplate>
 </ListView>
 </StackLayout>
</ContentPage>          
```
Let’s look at this code in more detail:

➊ x:Class: This is used to define the class name of a partial class between the markup and codebehind file. PassXYZ.Vault.Views.ItemsPage is the class name defined here.

➌ x:Name: While x:Class defines the class name in XAML, x:Name defines the instance name.
We can refer to the BrowseItemsPage instance name in the code-behind file.

➋ x:DataType: When we set x:DataType to the appropriate type defined in the view model,
compiled binding can be turned on, which can improve performance significantly. The view model
that we refer to here is ItemsViewModel.
Besides the standard namespace, we defined two more namespaces so that we can refer to the objects
in the view model ❺ and model ❻. We will discuss the view model and model in the next chapter.

➍ We define a ListView control to display the list of items. There are many properties in the
ListView control. The following properties are the ones we have to define to use the ListView control:

• ItemsSource, of the Ienumerable type, specifies the collection of items to be displayed.
It binds to Items, which is defined in the view model.

• ItemTemplate, of the DataTemplate type, specifies the template to apply to each item
in the collection of items to be displayed.

In Listing 3.5, DataTemplate is collapsed. If we expand it, we will see the following code snippet.

This is the default implementation from the Visual Studio template. The look and feel of this data
template are not good enough, but we can improve it:

```xml
<DataTemplate>
 <ViewCell>
   <StackLayout Padding="10" x:DataType="model:Item">
 <Label Text="{Binding Text}"
 LineBreakMode="NoWrap"
 Style="{DynamicResource ListItemTextStyle}"
 FontSize="16" />
 <Label Text="{Binding Description}"
 LineBreakMode="NoWrap"
 Style="{DynamicResource
 ListItemDetailTextStyle}"
 FontSize="13" />
 </StackLayout>
 </ViewCell>
</DataTemplate>
```
This DataTemplate implementation includes a ViewCell consisting of a StackLayout with
two Label controls. We can see the preview in Figure 3.4.

The DataTemplate implementation must reference a Cell class to display items. There are built-in
cells that can be used, as follows:

• TextCell, which displays primary and secondary text on separate lines.

• ImageCell, which displays an image with primary and secondary text on separate lines.

• SwitchCell, which displays text and a switch that can be switched on or off.

• EntryCell, which displays a label and text that’s editable.

• ViewCell, which is a custom cell whose appearance is defined by a View. This cell type
should be used when you want to fully define the appearance of each item in a ListView.
Typically, SwitchCell and EntryCell are only used in a TableView and won’t be used in
a ListView.

The preview of ViewCell in the preceding code snippet doesn’t look very good. It is not easy to
differentiate between Text and Description. In KeePass, we usually attach an icon to the password
entry. We can enhance it using the new data template, like so:

```xml
<DataTemplate>
 <ViewCell>
 <Grid Padding="10" x:DataType="model:Item" > ①
 <Grid.RowDefinitions> ②
 <RowDefinition Height="32" />
   <RowDefinition Height="32" />
 </Grid.RowDefinitions>
 <Grid.ColumnDefinitions>
 <ColumnDefinition Width="Auto" />
 <ColumnDefinition Width="Auto" />
 </Grid.ColumnDefinitions>
 <Grid Grid.RowSpan="2" Padding="10"> ③
 <Grid.ColumnDefinitions>
 <ColumnDefinition Width="32" />
 </Grid.ColumnDefinitions>
 <Image Grid.Column="0" Source="passxyz_logo.png"
 HorizontalOptions="Fill" VerticalOptions="Fill" />
 </Grid>
 <Label Text="{Binding Text}" Grid.Column="1"
 LineBreakMode="NoWrap" MaxLines="1"
 Style="{DynamicResource ListItemTextStyle}"
 FontAttributes="Bold" FontSize="Small" />
 <Label Text="{Binding Description}"
 Grid.Row="1" Grid.Column="1"
 LineBreakMode="TailTruncation" MaxLines="1"
 Style="{DynamicResource ListItemDetailTextStyle}"
 FontSize="Small" />
 </Grid>
 </ViewCell>
</DataTemplate>
```
Let’s look at this code in more detail:

• To make ViewCell look and feel better, we replaced the layout class from StackLayout
to Grid. Grid is a layout that organizes its children into rows and columns.

• ② Since we want to display two rows with an icon at the left, we created a grid with two
columns and two rows, as shown here:

![image](https://user-images.githubusercontent.com/26972859/231764190-9a9ae4cc-b123-46be-b4cb-b81e76de60e1.png)

Figure 3.5: Layout of an entry or a group

We can use different font styles for Text and Description so that users can easily differentiate
them with visual effects.

③ To display the icon at the center of the first two columns, we merged the two rows into a
Grid control. We can use the attached Grid.RowSpan property to merge rows.

A Grid can be used as a parent layout that contains other child layouts. To make the icon a specific
size and at the center of the merged cell, we can use another Grid as the parent of the Image control.

This child Grid contains only one row and one column with a specific size.

In the Image control, we can use a default image (passxyz_logo.png) from the resource. It can
be customized after we introduce our model in the next chapter.

We can see the improved preview in Figure 3.6:

![image](https://user-images.githubusercontent.com/26972859/231764546-b81ccb06-ebcc-46d8-be24-c8a968475d39.png)

Figure 3.6: Improved ItemsPage

With that, we’ve learned the basics of user interface design using XAML. One common issue in the
user interface design is supporting multiple languages. In the remainder of this chapter, we will learn
how to support multiple languages in XAML user interface design.

# Supporting multiple languages – localization

To support multiple languages, we can use the .NET built-in mechanism for localizing applications. In
a XAML file, we can use the x:Static markup extension to use the string defined in resource files.

## Creating a .resx file

Resource files are XML files with a .resx extension that are compiled into binary resource files during
the build process. A resource file can be added by right-clicking the project node and selecting Add
> New Item... > Resources File, as shown in Figure 3.7:

![image](https://user-images.githubusercontent.com/26972859/231766183-fb932f72-f272-45e4-8864-35c65a089856.png)

Figure 3.7: Creating a Resources File

We can create the Resources.resx resource file in the Properties folder.
To support different cultures, we can add additional resource files with cultural information as part
of the resource file’s name:

* Resources.resx: The resource file for the default culture, which we will set to en-US
(US English) later

* Resources.zh-Hans.resx: The resource file for the zh-Hans culture, which is
simplified Chinese

* Resources.zh-Hant.resx: The resource file for the zh-Hant culture, which is
traditional Chinese

Once the resource file has been created, the following ItemGroup will be added to the project file:

```xml
<ItemGroup>
 <Compile Update="Properties\Resources.Designer.cs">
 <DesignTime>True</DesignTime>
 <AutoGen>True</AutoGen>
 <DependentUpon>Resources.resx</DependentUpon>
 </Compile>
</ItemGroup>
<ItemGroup>
 <EmbeddedResource Update="Properties\Resources.resx">
 <Generator>ResXFileCodeGenerator</Generator>
 <LastGenOutput>Resources.Designer.cs</LastGenOutput>
 </EmbeddedResource>
</ItemGroup>
```
To edit the resource file, we can click a resource file and edit it in the resource editor, as shown in
Figure 3.8:

![image](https://user-images.githubusercontent.com/26972859/231766499-cbd73e84-3425-4cea-bced-1edf5894c637.png)

Figure 3.8: Resource editor

The resource file includes a list of key-value pairs for different languages:

* The Name field is the string name that we can refer to in either XAML or C# files

* The Value field contains the language-specific string that will be used according to the system
language settings

* The Comment field is just used as a remark for the key-value pair

To specify the default language, we need to set the value of NeutralLanguage in <PropertyGroup> in the project file, as shown here:

```xml
<PropertyGroup>
  <NeutralLanguage>en-US</NeutralLanguage>
…
</PropertyGroup>
```
In our project, we will use US English as the default culture, so NeutralLanguage is set to en-US.

# Localizing text

Once we have set up the resource files, we can use localized content in our XAML file or C# files. We
have five content pages in our project now. Let’s modify AboutPage with localization support, as
shown in Listing 3.6:

**Listing 3.6: AboutPage.xaml (https://epa.ms/AboutPage3-6)**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com
 /dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 x:Class="PassXYZ.Vault.Views.AboutPage"
 xmlns:res="clr-namespace:PassXYZ.Vault.Properties" ➊
 Title="{Binding Title}">
 <ContentPage.Resources…>
 <ScrollView>
 <StackLayout Margin="20">
 <Grid Padding="10"...>
 <StackLayout Padding="10" >
 <Label HorizontalOptions="Center"
 Text="{x:Static res:Resources.Appname}" ➋
 FontAttributes="Bold" FontSize="22" />
 <Label x:Name="AppVersion" HorizontalOptions
 ="Center"
 FontSize="Small" />
 <Grid HorizontalOptions="Center"...>
 <StackLayout...>
 </StackLayout>
 </StackLayout>
 </ScrollView>
</ContentPage>  
```
Text is localized using the generated Resources class. This class is named based on the default resource
file name. In Listing 3.6 AboutPage.xaml, we added a new namespace ➊ for the Resources class:

```xml
xmlns:res ="clr-namespace:PassXYZ.Vault.Properties "   
```
In the Label control ➋, to display our application name, we can refer to the resource string using
the x:Static XAML markup extension, like so:

```xml
<Label HorizontalOptions="Center"
 Text="{x:Static res:Resources.Appname}"
 FontAttributes="Bold" FontSize="22" />   
```
In Listing 3.6, we collapsed most of the source code to be concise. Please refer to the short URL of this
book’s GitHub repository to review the full source code.
We can use localized text in both XAML and C#. To use a resource string in C#, we can look at the
Title property in Listing 3.6. The Title property of AboutPage is connected to the Title
property in the AboutViewModel class. Let’s see how we can use a resource string in Listing 3.7:

**Listing 3.7: AboutViewModel.cs (https://epa.ms/AboutViewModel3-7)**

```Csharp
using System;
using System.Windows.Input;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Controls;
using PassXYZ.Vault.Properties; ①
namespace PassXYZ.Vault.ViewModels {
 public class AboutViewModel : BaseViewModel {
 public AboutViewModel() {
 Title = Properties.Resources.About; ②
 OpenWebCommand = new Command(async () => await
 Browser.OpenAsync(Properties.Resources.about_url));
 }
 public ICommand OpenWebCommand { get; }
 public string GetStoreName()...
 public DateTime GetStoreModifiedTime()...
 }
}   
```
As shown in Listing 3.7, ① we added the PassXYZ.Vault.Properties namespace first. ② We
refer to the resource string as Properties.Resources.About.
After we update AboutPage with localization support, we can test it in the supported languages,
as shown in Figure 3.9:

![image](https://user-images.githubusercontent.com/26972859/231767258-66d9a3b9-5267-45eb-aa10-d752bae972d6.png)

Figure 3.9: AboutPage in different languages

In AboutPage, many resource strings are used for localization. In Listing 3.6 and Listing 3.7, we
collapsed most of the code; you can refer to the short URL for this book’s GitHub repository to review
the source code online.

# Summary

We learned about XAML syntax in this chapter. We used the knowledge we learned to improve the
look and feel of ItemsPage. We will continue improving the user interface of other pages throughout
this book. To support multiple languages, we learned how to support localization in .NET. We created
.resx resource files for the US-en, zh-Hans, and zh-Hant cultures and used a XAML markup
extension to enable multi-language support. Finally, we used AboutPage as an example to explain
how to use localized text in both XAML and C#.
   
In the next chapter, we will continue improving our app by introducing MVVM and data binding.

Further reading

   * .NET Multi-platform App UI documentation

   * https://learn.microsoft.com/en-us/dotnet/maui/

   * XAML - .NET MAUI

   * https://learn.microsoft.com/en-us/dotnet/maui/xaml/

   * XAML markup extensions

   * https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/
markup-extensions

   * KeePass – An open source password manager

   * https://keepass.info/

































