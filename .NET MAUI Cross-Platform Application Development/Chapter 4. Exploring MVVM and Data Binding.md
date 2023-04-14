# Exploring MVVM and Data Binding


In the last chapter, we learned how to build user interfaces (UIs) using XAML. In this chapter, we will
learn how to use the Model-View-ViewModel (MVVM) pattern and data binding in .NET MAUI
app development. MVVM is a UI design pattern for decoupling UI and non-UI code. data binding is
the key technology that MVVM relies on. We will improve the design of our app using MVVM and
data binding. We will also replace the data model using open source libraries.
The following topics will be covered in this chapter:

* Understanding MVVM and MVC

* Data binding

* Improving the data model and service

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC or Mac. Please refer to the Development environment setup section in Chapter 1, Getting
Started with .NET MAUI, for the details.
The source code of this chapter is available in the following branch at GitHub: https://github.
com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development/
tree/main/Chapter04.
The source code can be downloaded using the following Git command:

```
git clone -b chapter04 https://github.com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development
```

# Understanding MVVM and MVC

In software design, we usually follow and reuse good practices and design patterns. The Model-ViewController (MVC) pattern is an approach to decoupling the responsibilities of a system. It can help
to separate the implementation of the UI and the business logic into different parts.

![image](https://user-images.githubusercontent.com/26972859/231772893-fc58dc30-81ae-467f-99bd-f0944ce39284.png)

Figure 4.1: The MVC pattern

The MVC pattern, as shown in Figure 4.1, divides the responsibilities of the system into three distinct parts.
Model stores the application data and processes business logic. Model classes usually can be implemented
as Plain Old CLR Objects (POCOs) or Data Transfer Objects (DTOs). POCO is a class that doesn’t
depend on any framework-specific classes, so POCO classes can be used with LINQ or Entity Framework
well. DTO is a subset of a POCO class that only contains data without logic or behavior. DTO classes
can be used to pass data between layers. The model has no dependency on the view or the controller
so it can be implemented and tested separately.

View presents the model information to the user and interacts with the user.

Controller updates the model and view in response to the user’s action. Our understanding of the
model and the view hasn’t changed too much over time, but there have been different understandings
and implementations of the controller since the MVC pattern was introduced.

Model-View-Presenter (MVP) is one of them. Later, Microsoft used MVVM and XAML in WPF,
which is a variation of MVP. In Xamarin.Forms and .NET MAUI, XAML and the MVVM pattern
are also used.

![image](https://user-images.githubusercontent.com/26972859/231773077-96e94ea1-3cb7-4541-8e00-59d8d428b14a.png)

Figure 4.2: The MVVM pattern

As we can see in Figure 4.2, in MVVM, the view model is used to replace the controller. The differences
between MVVM and MVC are as follows:
• Decoupling of view and model. The viewmodel is used to handle the communication between
the view and the model. The view accesses the data and logic in the model via the viewmodel.
• Data binding between the view and viewmodel. Using data binding, changes to the view or
viewmodel can automatically be updated in the other one. This can help to reduce the complexity
of implementation.
• In both MVC and MVVM, the model can be tested separately. In MVVM, it is possible to
design unit tests for the viewmodel as well.
When the view changes, the changes will be reflected in the viewmodel via data binding. The viewmodel
will process the data changes in the model. Similarly, when the data changes in the model, the viewmodel
is notified to update the view. The common solution for notifications is to install event handlers to
notify the changes. With data binding, the implementation is simplified significantly


# MVVM in PassXYZ.Vault

In our app, PassXYZ.Vault, we use MVVM to handle the data exchange between the view and
the viewmodel. As we can see in Figure 4.3, we have five XAML content pages and the same number
of viewmodels defined. In our data model, we have an Item class, which is our model class, and it
can be accessed through the IDataStore interface.

![image](https://user-images.githubusercontent.com/26972859/231773278-c1cb7d23-fe90-45b5-bccc-0da7f094286c.png)

Figure 4.3: MVVM in PassXYZ.Vault

Data binding is used as the communication channel between views and viewmodels. The viewmodel
will update the Item model via the IDataStore service interface. We will learn how to use data
binding in the next section by analyzing the item detail page and viewmodel.

# Data binding

Let’s explore how MVVM and data binding works in our app. We can analyze ItemDetailPage
and ItemDetailViewModel at the beginning of our journey. The following list includes the view,
viewmodel, and model that we are going to explore:

* View – ItemDetailPage, see Listing 3.4 in the previous chapter

* Viewmodel – ItemDetailViewModel, see Listing 4.1

* Model – Item (access through interface IDataStore), see Listing 3.3 in the previous chapter
ItemDetailPage is a view used to display the content of an instance of Item. This instance
is stored in the viewmodel. The UI elements presenting the content of Item are connected to the
instance through data binding.

Data binding is used to link the properties of target and source objects. Here is a list of involved
properties of target and source objects:

* Target – This is the UI element involved and this UI element has to be a child of BindableObject. The UI element used in ItemDetailPage is Label.

* Target property – This is the property of the target object. It is a BindableProperty. If the target is Label, as we mentioned here, the target property can be the Text property of Label.

* Source – This is the source object referenced by data binding. It is ItemDetailViewModel here.

* Source object value path – This is the path to the value in the source object. Here, the path is a viewmodel property, such as Text or Description.

Let’s look at the following code in ItemDetailPage:

```xml
<StackLayout Spacing="20" Padding="15">
 <Label Text="Name:" FontSize="Medium" />
 <Label Text="{Binding Name}" FontSize="Small"/> ❶
 <Label Text="Description:" FontSize="Medium" />
 <Label Text="{Binding Description}"
 FontSize="Small"/> ❷
</StackLayout>

```
In the XAML here, there are two data binding source paths, which are Name, ❶, and Description,
❷. The binding target is Label and the target property is the Text property of Label. If we review
the inheritance hierarchy of Label, it looks like so:
```
Object -> BindableObject -> Element -> NavigableElement -> VisualElement
-> View -> Label
```
We can see that Element, VisualElement, and View are the derivatives of BindableObject.
The data binding target has to be a child of BindableObject.
The binding source is the ItemDetailViewModel viewmodel. Name, ①, and Description,
②, are properties of the viewmodel as shown in Listing 4.1 here:

**Listing 4.1: ItemDetailViewModel.cs (https://epa.ms/ItemDetailViewModel4-1)**

```Csharp
using PassXYZ.Vault.Models;
namespace PassXYZ.Vault.ViewModels {
[QueryProperty(nameof(ItemId), nameof(ItemId))]
public class ItemDetailViewModel : BaseViewModel {
 private string itemId;
 private string name;
 private string description;
 public string Id { get; set; }
 public string Name { ①
 get => name;
 set => SetProperty(ref name, value);
 }
 public string Description… ②
 public string ItemId...
 public async void LoadItemId(string itemId) { ③
 try {
 var item = await DataStore.GetItemAsync
 (itemId);
 Id = item.Id;
 Name = item.Name;
 Description = item.Description;
 }
 catch (Exception) {
 Debug.WriteLine("Failed to Load Item");
 }
 }
}
 
```
The values of Name, ①, and Description, ②, are loaded from the model in the LoadItemId()
method, ③. You may notice that the class is decorated by a QueryPropertyAttribute attribute.
This is used to pass parameters during page navigation, and it will be introduced in the next chapter.

Let’s use the following, Table 4.1, to summarize the data binding components in the code.

|Data binding elements|Example|
|:--------------------|:------|
|Target|Label|
|Target property|Text|
|Source object |ItemDetailViewModel|
|Source object value path |Name or Description|

Table 4.1: Data binding settings

Having analyzed the preceding code, let us have a look at the syntax of the binding expression:

```xml
<object property="{Binding bindProp1=value1[, bindPropN=valueN]*}" ... />
```
Binding properties can be set as a series of name-value pairs in the form of bindProp=value. For
example, see the following:

```xml
<Label Text="{Binding Path=Description}" FontSize="Small"/>
```
The Path property is the default property, and it can be omitted if it is the first one in the property
list as shown here:

```xml
<Label Text="{Binding Description}" FontSize="Small"/>
```
The Source property can be set to override BindingContext, which we will discuss shortly.
There are many binding properties, and you can find the details by referring to Microsoft document
about the Binding class here:

https://learn.microsoft.com/en-us/dotnet/api/system.windows.data.binding?view=windowsdesktop-6.0

When we set data binding to the target, we can use the following two members of the target class:

* The BindingContext property gives us the source object

* The SetBinding method specifies the target property and source property

In our case, we set the BindingContext property to an instance of ItemDetailViewModel,
❶, in the C# code-behind file of ItemDetailPage as shown in Listing 4.2 here. It is set at the page
level, and it applies to all binding targets for this page:

**Listing 4.2: ItemDetailPage.xaml.cs (https://epa.ms/ItemDetailPage4-2)**

```Csharp
using PassXYZ.Vault.ViewModels;
using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
namespace PassXYZ.Vault.Views
{
 public partial class ItemDetailPage : ContentPage
 {

  public ItemDetailPage()
   {
   InitializeComponent();
   BindingContext = new ItemDetailViewModel(); ❶
   }
   }
   void OnFieldSelected ...
  }
```
Instead of using the Binding markup extension, we can also create the binding using the SetBinding
method directly as done here:

```xml
<StackLayout Spacing="20" Padding="15">
 <Label Text="Text:" FontSize="Medium" />
 <Label x:Name="labelText" FontSize="Small"/> ❷
 <Label Text="Description:" FontSize="Medium" />
 <Label Text="{Binding Description}"
 FontSize="Small"/>
 </StackLayout>
```

❷ In the XAML code, we removed the Binding markup extension and specified the instance name
as labelText. In the C# code-behind file, we can call the SetBinding() method, ❸, in the
constructor of ItemDetailPage to create the data binding for the Text property

```xml
public ItemDetailPage()
 {
 InitializeComponent();
 BindingContext = new ItemDetailViewModel();
 labelText.SetBinding(Label.TextProperty, "Text"); ❸
 }
```

# Binding mode

In this discussion, all the UI elements are Label objects, which are not editable for the user. This is
one-way binding from the source to the target. In this kind of binding setup, we do not change target
objects. The changes in the source object will cause updates in the target object.

There are four binding modes supported in .NET MAUI. Let’s review them by referring to Figure 4.4.

![image](https://user-images.githubusercontent.com/26972859/231775839-b1b28baf-b300-48fc-aa30-9f6d7eef8a88.png)

Figure 4.4: Binding mode

These binding modes are supported in .NET MAUI:

* OneWay binding is usually used in the case of presenting data to the user. In our app, we will
retrieve a list of password entries and display this list on ItemsPage. When the user clicks
an item in the list, the password details will show on ItemDetailPage. OneWay is used
in both cases.

* TwoWay binding causes changes to either the source property or the target property to
automatically update the other. In our app, when the user edits the fields of a password entry
or when the user enters a username and password on LoginPage, the target UI Entry
component and the source view model object are set with TwoWay.

* OneWayToSource is the reverse of the OneWay binding mode. When the target property
is changed, the source property will be updated. When we add a new password entry on
NewItemPage, we can use OneWayToSource instead of the TwoWay binding mode to
improve performance.

* OneTime binding is a binding mode that is not shown in Figure 4.4. The target properties
are initialized from the source properties, but any further changes to the source properties
won’t update the target properties. It is a simpler form of the OneWay binding mode with
better performance.

If we don’t specify the binding mode in data binding, the default binding mode is used. We can
overwrite the default binding mode if it is needed.
In our ItemsPage code, we use the ListView control to display the list of password groups and
entries, so we should set the IsRefreshing attribute to the OneWay binding mode:

```xml
IsRefreshing="{Binding IsBusy, Mode=OneWay}"
```
When we add a new item in NewItemPage, we use the Entry and Editor controls to edit the
properties. We can use the OneWayToSource or TwoWay binding modes:

```xml
<Label Text="Text" FontSize="Medium" />
<Entry Text="{Binding Text, Mode=TwoWay}" FontSize="Medium"
 />
<Label Text="Description" FontSize="Medium" />
<Editor Text="{Binding Description, Mode=OneWayToSource}" AutoSize="TextChanges" FontSize="Medium" Margin="0" />
```

# Changing notifications in viewmodels

In Figure 4.4, we can see the data binding target is a derived class of BindableObject. Besides
this requirement, in the data binding setup, both the data binding target and source also need to
implement the INotifyPropertyChanged interface so that when the property changes, a
PropertyChanged event is raised to notify the change.
In an MVVM pattern, the viewmodel is usually the data binding source and we need to implement
the INotifyPropertyChanged interface in our viewmodels. If we do this for each viewmodel,
there will be a lot of duplicated code. In a Visual Studio template, a BaseViewModel class, as we
can see in Listing 4.3, is included in the boilerplate code and we use it in our app. Other viewmodels
inherit this class:

**Listing 4.3 BaseViewModel.cs (https://epa.ms/BaseViewModel4-3)**

```Csharp
namespace PassXYZ.Vault.ViewModels;
public class BaseViewModel : InotifyPropertyChanged ❶
{
 public IDataStore<Item> DataStore =>
 DependencyService.Get<IDataStore<Item>>();
 bool isBusy = false;
 public bool IsBusy {
 get { return isBusy; }
 set { SetProperty(ref isBusy, value); } ❷
 }
 string title = string.Empty;
 public string Title {
 get { return title; }
 set { SetProperty(ref title, value); }
 }
 protected bool SetProperty<T>(ref T backingStore,
 T value,
 [CallerMemberName] string propertyName = "",
 Action onChanged = null) {
 if (EqualityComparer<T>.Default.Equals
 (backingStore, value))
 return false;
 backingStore = value;
 onChanged?.Invoke();
 OnPropertyChanged(propertyName);
 return true;
 }
 #region INotifyPropertyChanged
 public event PropertyChangedEventHandler PropertyChanged;❹
 protected void OnPropertyChanged([CallerMemberName]
 string propertyName = "") { ❸
 var changed = PropertyChanged;
 if (changed == null)
 return;
changed.Invoke(this,
 new PropertyChangedEventArgs(propertyName));
 }
 #endregion
}
```
In the BaseViewModel class (Listing 4.3), we can see the following:

* ❶ BaseViewModel implements the INotifyPropertyChanged interface and this
interface defines a single event, PropertyChanged, ❹.

* ❸ When a property is changed in the setter, the OnPropertyChanged method is
called. In OnPropertyChanged, the PropertyChanged event is fired. A copy of
the PropertyChanged event handler is stored in the changed local variable, so this
implementation is safe in a multi-thread environment. When the PropertyChanged event is
fired, it needs to pass the property name as a parameter to indicate which property is changed.
The CallerMemberName attribute can be used to find the method name or property name
of the caller, so we don’t need to hardcode the property name.

* ❷ When we define a property in the viewmodel, the OnPropertyChanged method is
called in the setter – but as you can see, in our code, we call SetProperty<T> instead
of OnPropertyChanged directly. SetProperty<T> will do additional work before it
calls OnPropertyChanged. It checks whether the value is changed. If there is no change,
it will return and do nothing. If the value is changed, it will update the backing field and call
OnPropertyChanged to fire the change event.

If we recall ItemDetailViewModel in Listing 4.1, it inherits from the BaseViewModel class.
In the setter of the Name and Description properties, we call SetProperty<T> to set the
values and fire the PropertyChanged event:

```Csharp
public string Name {
 get => name;
 set => SetProperty(ref name, value);
 }
 public string Description {
 get => description;
 set => SetProperty(ref description, value);
 }  
```
In this section, we learned about data binding and the INotifyPropertyChanged interface.
We need to create boilerplate code to define a property with change notification support. To simplify
the code and autogenerate boilerplate code behind the scenes, we can use the MVVM Toolkit. Please
find more information about the MVVM Toolkit in the Further reading section.
Having introduced some basic knowledge of XAML UI design, the MVVM pattern, and data binding,
we can improve our app using the knowledge we just learned.

# Improving the data model and service

To improve our app, let us review the use cases again. We are developing a cross-platform password
manager app that is compatible with the popular KeePass database format. We have the following
use cases:
  
  * Use case 1: LoginPage – As a password manager user, I want to log in to the password
manager app so that I can access my password data

  * Use case 2: AboutPage – As a password manager user, I want to have an overview of my
database and the app that I am using

  * Use case 3: ItemsPage – As a password manager user, I want to see a list of groups and
entries so that I can explore and examine my password data

  * Use case 4: ItemDetailPage – As a password manager user, I want to see the details of a
password entry after I select it in the list of password entries

  * Use case 5: NewItemPage – As a password manager user, I want to add a password entry or
create a new group in my database

These five use cases are inherited from the Visual Studio template, and they are sufficient for the
user stories of our password manager app for the moment. We will improve our app using these user
stories in this chapter.
  
We have explored the model, view, and viewmodel, but the model given here is too simple and is not
sufficient for use in a password manager app:

```Csharp
public class Item
{
 public string Id { get; set; }
 public string Name { get; set; }
 public string Description { get; set; }
}  
```
The major functionalities of our password manager app are encapsulated in the model layer. We will
build our model using two .NET packages, KPCLib and PassXYZLib. These two packages include
all the password management features we need.

# KPCLib

The model that we will use is a library from KeePass called KeePassLib. Both KeePass and
KeePassLib are built for .NET Framework, so they can only be used on Windows. I ported
KeePassLib and rebuilt it as a .NET Standard 2.0 library packaged as KPCLib. KPCLib can be
found at NuGet and GitHub here:
  
  * NuGet: https://www.nuget.org/packages/KPCLib/
  
  * GitHub: https://github.com/passxyz/KPCLib

  KPCLib is used both as a package name and a namespace. The package of KPCLib includes two
namespaces, KeePassLib and KPCLib. The KeePassLib namespace is the original one from
KeePass with the following changes:

  * Updated and built for .NET Standard 2.0
  
  * Updated PwEntry and PwGroup to be classes derived from the Item abstract class

  In the KPCLib namespace, an Item abstract class is defined. The reason I created a new class and
made it the parent class of PwEntry and PwGroup is due to the navigation design difference between
KeePass and PassXYZ.Vault.
If we look at the UI of KeePass in Figure 4.5, we can see that it is a classic Windows desktop UI. The
navigation is designed around a tree view like Windows Explorer.

![image](https://user-images.githubusercontent.com/26972859/231781606-fbbd00ed-d0d4-4630-b075-7e022fa4c6f4.png)

  Figure 4.5: KeePass UI

Two classes, PwGroup and PwEntry, behave like directories and files. A PwGroup instance is just
like a directory, and it includes a list of children – PwGroup and PwEntry. All PwGroup instances
display in a tree view on the right-hand panel. When a PwGroup instance is selected, the list of
PwEntry in this group is shown on the right-hand panel. PwEntry includes the content of a password
entry, such as a username and password. The content of PwEntry is displayed on the bottom panel.
In the PassXYZ.Vault UI design, we use a .NET MAUI Shell template. It is an implementation of
a stacked Master-Detail pattern. In the stacked Master-Detail pattern, a single list is used to display
items. In this case, the instances of both PwGroup and PwEntry can be displayed in the same list.
After an item is selected, we will take an action according to the type of the item.

# Abstraction of PwGroup and PwEntry
  
To work with the PassXYZ.Vault UI design better, we can abstract PwGroup and PwEntry as Item
abstract class, as shown in Figure 4.6.

![image](https://user-images.githubusercontent.com/26972859/231782011-5a80c2e7-26b8-42d4-83e9-160a2a15cf4e.png)

Figure 4.6: Class diagram of Item

Referring to this UML class diagram in Figure 4.6 and the source code of Item.cs in Listing 4.4,
we can see the following properties are defined in the Item abstract class. These properties are
implemented in both PwEntry and PwGroup:


  * ① Name – the Item name

  * ② Description – the Item description

  * ③ Notes – Item comments defined by the user

  * ④ IsGroup –true if the instance is PwGroup or false if it is PwEntry

  * ⑤ Id – ID of the instance (a unique value that is like the primary key in a database)

  * ⑥ ImgSource – image source of the icon (both PwGroup and PwEntry can have an associated icon)

  * ⑦ LastModificationTime – the last modification time of the item

  * ⑧ Item implements the INotifyPropertyChanged interface, and it can work well in the MVVM model for data binding:

**Listing 4.4: Item.cs (https://epa.ms/Item4-4)**

```Csharp
using System.Text;
namespace KPCLib
{
 public abstract class Item : INotifyPropertyChanged ⑧
 {
 public abstract DateTime LastModificationTime {get;
 set;};} ⑦
 public abstract string Name { get; set; } ①
 public abstract string Description { get;} ②
 public abstract string Notes { get; set; } ③
 public abstract bool IsGroup { get; } ④
 public abstract string Id { get; } ⑤
 virtual public Object ImgSource { get; set; } ⑥
#region INotifyPropertyChanged ...
 }
}  
```

# PassXYZLib

To use KeePassLib in PassXYZ.Vault, we need to use some .NET MAUI APIs to extend the
functionalities required of our app. To separate the business logic from the UI and extend the
functionalities of KeePassLib for .NET MAUI, a .NET MAUI class library, PassXYZLib, is
created to encapsulate the extended model in a separate library. PassXYZLib is both a package
name and a namespace.
To add PassXYZLib to our project, we can add it to a PassXYZ.Vault.csproj project file,
as seen here:

```Csharp
<ItemGroup>
 <PackageReference Include="PassXYZLib"
 Version="2.0.2" />
 </ItemGroup>  
```
We can also add a PassXYZLib package from the command line here. From the command line, go to
the project folder and execute this command to add the package:

```dotnet add package PassXYZLib```

# Updating the model

After we add a PassXYZLib package to the project, we can access the KPCLib, KeePassLib, and
PassXYZLib namespaces. To replace the current model, we need to remove the Models/Item.
cs file from the project.
  
After that, we need to replace the PassXYZ.Vault.Models namespace with KPCLib.

![image](https://user-images.githubusercontent.com/26972859/231782832-e2fca22e-7462-4427-8c32-df7b356f2791.png)

Figure 4.7: Updating the model from PassXYZ.Vault.Models to KPCLib (https://bit.ly/3uXVl7H)

In the commit history, Figure 4.7, we can see that there are four view models, and three views are
changed. All changes are namespace changes so we don’t need to explain more about them.

# Updating the service
  
The major changes can be found in MockDataStore.cs. In the MockDataStore class, we
changed the namespace and the mock data initialization.

To decouple the model from the rest of the system, we use an IDataStore interface to encapsulate
the actual implementation. At this stage, we use mock data to implement the service for testing, so
the MockDataStore class is used. We will replace it with the actual implementation in Chapter 6,
Introducing Dependency Injection and Platform-Specific Services, using dependency injection.

---
Dependency inversion and dependency injection
We will learn about the Dependency Inversion Principle (DIP), which is one of the SOLID
design principles, in Chapter 6, Introducing Dependency Injection and Platform-Specific Services.
We will learn how to use dependency injection to manage the mapping of IdataStore
interface to the actual implementation.
---

In the original code, we created new instances of PassXYZ.Vault.Models.Item to initialize
mock data. After we replace the model, we cannot create KPCLib.Item directly, since it is an abstract
class. Instead, we can create new instances of PxEntry using JSON data and assign PxEntry
instances to the Item list:

```Csharp
Static string[] jsonData =…;
 readonly List<Item> items;
 public MockDataStore() {
 items = new List<Item>() {
 new PxEntry(jsonData[0]),
 new PxEntry(jsonData[1]),
 new PxEntry(jsonData[2]),
 new PxEntry(jsonData[3]),
 new PxEntry(jsonData[4])
 };
 }
```
To create the instances of an abstract class, the factory pattern can be used. To make the testing code
simple, we did not use it here. The factory pattern is used in the actual implementation later in this book.
We have replaced the model in the sample code with our own model now. With this change, we can
improve ItemsPage and ItemDetailPage to reflect the updated model.
We will update the view and viewmodel using data binding to collections in the next section.

# Binding to collections
  
In the previous section, we introduced some basic knowledge of data binding, and we also replaced
the model using PassXYZLib. When we introduced data binding, we used ItemDetailPage and
ItemDetailViewModel to explain how to bind the source property to the target property. For the item detail page, we created data binding from one source to one target. However, there are many
cases in which we need to bind a data collection to the UI, such as ListView or CollectionView,
to display a group of data. 

![image](https://user-images.githubusercontent.com/26972859/231783417-872c30e0-3b1c-448d-aaac-886aab07d564.png)

Figure 4.8: Binding to collections

As we can see in Figure 4.8, when we create a data binding from a collection object to a collection view,
the ItemsSource property is the one to use. In .NET MAUI, collection views such as ListView
and CollectionView can be used and both have an ItemsSource property.
  
For the collection object, we can use any collection that implements the IEnumerable interface. However,
the changes to the collection object may not be able to update the UI automatically. In order to update UI
automatically, the source object needs to implement the INotifyCollectionChanged interface.
  
We can implement our collection object with the INotifyCollectionChanged interface, but the
simplest approach is to use the ObservableCollection<T> class. If any item in the observable
collection is changed, the bound UI view is notified automatically.
With this in mind, let’s review the class diagram of our models, viewmodels, and views as shown in
Figure 4.9:
  
* Model: Item, PwEntry, PwGroup, Field
* View Model: ItemsViewModel, ItemDetailViewModel
* View: ItemsPage, ItemDetailPage

When we display a list of items to the user, the user may take action on the selected item. If the item
is a group, we will show the groups and entries in an instance of ItemsPage. If the item is an entry,
we will show the content of the entry on a content page, which is an instance of ItemDetailPage.
On ItemDetailPage, we display a list of fields to the user. Each field is a key value pair and is
implemented as an instance of Field class.

In summary, we display two kinds of lists to the user – a list of items or a list of fields. The list of items
is shown in ItemsPage and the list of fields is shown in ItemDetailPage.

![image](https://user-images.githubusercontent.com/26972859/231783733-8f28e4ff-78b4-4ec7-8410-b1ac2c3d52aa.png)

Figure 4.9: Class diagram of the model, view, and viewmodel

In this class diagram, we can see both PwEntry and PwGroup are derived from Item. There is a
list of items in ItemsViewModel and there is a list of fields in ItemDetailViewModel. In the
views, ItemsPage contains a reference to ItemsViewModel, and ItemDetailPage contains
a reference to ItemDetailViewModel.

  After we refine our design, we can look at the implementation. We will review the implementation of
ItemDetailViewModel and ItemDetailPage to verify the design change:

```Csharp
  [QueryProperty(nameof(ItemId), nameof(ItemId))]
public class ItemDetailViewModel : BaseViewModel {
 private string itemId;
 private string description;
 public string Id { get; set; }
 public ObservableCollection<Field> Fields { get; set; } ❶
 public string Description ...
 public string ItemId ...
  public ItemDetailViewModel() {
 Fields = new ObservableCollection<Field>(); ❷
 }
 public async void LoadItemId(string itemId) {
 try {
 var item = await DataStore.GetItemAsync(itemId);
 Id = item.Id;
 Title = item.Name;
 Description = item.Description;
 if (!item.IsGroup) {
 PwEntry dataEntry = (PwEntry)item; ❸
 Fields.Clear();
 List<Field> fields = dataEntry.GetFields(GetImage:
 FieldIcons.GetImage); ❹
 foreach (Field field in fields) {
 Fields.Add(field);
 }
 }
 }
 catch (Exception) {
 Debug.WriteLi"e("Failed to Load I"em");
 }
 }
}
```
As shown in the code here, we can see the difference in ItemDetailViewModel compared to
Listing 4.1 at the beginning of this chapter:
• ❶ A Fields property is defined as the ObservableCollection<Field> type to
hold the Field list
• ❷ The Fields variable is initialized in the constructor of ItemDetailViewModel
• ❸ The item type of variable is PwEntry here and we can cast it to a PwEntry instance
• ❹ We can get the list of fields by calling an extension method, GetFields(), which is
defined in the PassXYZLib library

---
Having reviewed the changes in ItemDetailViewModel, let’s review the changes in
ItemDetailPage in Listing 4.5:
---

**Listing 4.5: ItemDetailPage.xaml (https://epa.ms/ItemDetailPage4-5)**

```xml
<?xml versi"n="".0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 x:Class="PassXYZ.Vault.Views.ItemDetailPage"
 xmlns:local="clr-namespace:PassXYZ.Vault.ViewModels"
 xmlns:model="clr-namespace:KPCLib;assembly=KPCLib"
 x:DataType="local:ItemDetailViewModel"
 Title="{Binding Title}">
  
  <StackLayout>
 <ListView x:Name="FieldsListView"
 ItemsSource="{Binding Fields}" ②
 VerticalOptions="FillAndExpand"
 HasUnevenRows="False"
 RowHeight="84"
 IsPullToRefreshEnabled="true"
 IsRefreshing="{Binding IsBusy, Mode=
 OneWay}"
 CachingStrategy="RetainElement"
 ItemSelected="OnFieldSelected">
 <ListView.ItemTemplate...> ③
 <ListView.Footer>
 <StackLayout Padding="5" Orientation=
 "Horizontal">
 <Label Text="{Binding Description}".../>
 </StackLayout>
   </ListView.Footer>
 </ListView>
 </StackLayout>

</ContentPage>
```
  
In ItemDetailPage, we can see there are many changes compared to Listing 3.4 in Chapter 3, User
Interface Design with XAML. ListView is used to display the fields in an entry:

  * ① To use Field in DataTemplate, a xmlns:model namespace is added. Since the
Field class is in a different assembly, we need to specify the assembly’s name as follows:
 xmlns:model="clrnamespace:KPCLib;assembly=KPCLib"

  * ② We bind the Fields property to the ItemsSource property of ListView.

  * ③ DataTemplate is used to define the appearance of each item in ListView. It is collapsed in Listing 4.5.

  Let’s expand it and review the implementation of DataTemplate in this code block:

In ItemDetailPage, we can see there are many changes compared to Listing 3.4 in Chapter 3, User
Interface Design with XAML. ListView is used to display the fields in an entry:

  * ① To use Field in DataTemplate, a xmlns:model namespace is added. Since the

  Field class is in a different assembly, we need to specify the assembly’s name as follows:
 xmlns:model="clrnamespace:KPCLib;assembly=KPCLib"

  * ② We bind the Fields property to the ItemsSource property of ListView.

  * ③ DataTemplate is used to define the appearance of each item in ListView. It is collapsed
in Listing 4.5.
Let’s expand it and review the implementation of DataTemplate in this code block:

```xml
<DataTemplate>
 <ViewCell>
 <Grid Padding="10" x:DataType="model:Field" > ➊
 <Grid.RowDefinitions...>
 <Grid.ColumnDefinitions...>
 <Grid Grid.RowSpan="2" Padding="10">
 <Grid.ColumnDefinitions...>
 <Image Grid.Column="0" Source="{Binding ImgSource}"
 HorizontalOptions="Fill"
 VerticalOptions="Fill" /> ➋
 </Grid>
 <Label Text="{Binding Key}" Grid.Column="1".../> ➌
 <Label Text="{Binding Value}" Grid.Row="1"
 Grid.Column="1".../> ➍
 </Grid>
 </ViewCell>
</DataTemplate>  
```
In DataTemplate, the layout of each field is defined in a ViewCell element. In the ViewCell
element, we defined a 2x2 Grid layout. The first column is used to display the field icon. The key and
value in the field are displayed in the second column with two rows:
   
  * ➊ The x:DataType attribute in the Grid layout is set to Field and the following data
binding in Grid will refer to the property of Field. The Field class is defined in our model,
which is in the KPCLib package.

   * ➋ To display the field icon, the Source property of the Image control is set to the ImgSource
property of Field.

   * ➌,➍ Both the Key property and the Value property of Field are assigned to the Text
property of the Label control.
With this analysis, we learned how to create data binding for a collection. The data binding used in
ItemsPage and ItemsViewModel is similar to this implementation. The difference is we use a
collection of Field here and a collection of Item classes is used in ItemsPage. Having completed
the changes, we can see the improvement of the UI in Figure 4.10.

![image](https://user-images.githubusercontent.com/26972859/231786690-f2a1c18e-4403-4537-9c4c-e5a27e2da6c6.png)

Figure 4.10: Improved ItemsPage and ItemDetailPage

In the improved UI, we display a list of items on ItemsPage (on the left). The items in the list can be
entries (such as on Facebook, Twitter, or Amazon), or groups, which we will see in the next chapter.
When the user clicks on an item, such as GitHub, we will display it on ItemDetailPage (on the
right). On the item detail page, the information about this account (GitHub) is shown.
Having introduced the new data model, the design hasn’t changed much. We improved the UI to
make it more meaningful, but most of the complexity is hidden in our model libraries – KPCLib and
PassXYZLib. This is the benefit that we can see by using the MVVM pattern to separate the model
(business logic) from the UI design.

# Summary
   
In this chapter, we learned about the MVVM pattern and applied it to our app development. One key
feature of the MVVM pattern is data binding between the view and viewmodel. We learned about
data binding and used it in the implementation of our app.
We also improved the model in this chapter. We improved it by introducing two packages – KPCLib
and PassXYZLib. We replaced the model in the sample code with the model in these two packages.
We updated the UIs of ItemsPage and ItemDetailPage to reflect the changes to the model.
In the next chapter, we will refine our user stories and continue improving the UI using our knowledge
of Shell and navigation.

Further reading
   
* Introduction to the MVVM Toolkit: https://learn.microsoft.com/en-us/dotnet/
communitytoolkit/mvvm/
   
* KeePass is a free open source password manager: https://keepass.info/
