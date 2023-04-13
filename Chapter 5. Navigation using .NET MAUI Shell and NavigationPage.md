# Navigation using .NET MAUI Shell and NavigationPage

In the previous chapter, we introduced the MVVM pattern and data binding. We improved the user
interface design and introduced our data model. In our app, we can select a page from the flyout
menu, and we can switch to the item detail when an item is selected. This is part of the navigation
mechanism in .NET MAUI. In this chapter, we will dive deeper into the navigation design, and we
will learn how navigation works in .NET MAUI.

The following topics will be covered in this chapter:

* Implementing navigation

* Using Shell

* Improving design and navigation

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC or Mac. Please refer to the Development environment setup section in Chapter 1, Getting
Started with .NET MAUI, for the details.
The source code for this chapter is available in the following branch on GitHub: https://github.
com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development/
tree/main/Chapter05.
The source code can be downloaded using the following git command:

```xml
git clone -b chapter05 https://github.com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development
```

# Implementing navigation

In this chapter, we are going to implement the navigation logic of our password manager app. It will
include the following functionalities:

* Logging in and connecting to the database

* Exploring data in the password database

Navigation design has a significant impact on the user experience. In .NET MAUI, there is a built-in
mechanism to help developers implement the navigation efficiently. As we saw in the previous
chapters, we can use Shell in our app. In this chapter, we will learn about Shell and improve our
app with features provided by Shell. Before we dive into Shell, we will learn the basic navigation
mechanism in .NET MAUI.

There are two most common ways to implement navigation – hierarchical and modal:

* Hierarchical navigation provides a navigation experience where the user can navigate through
pages, both forward and backward. This pattern typically uses a toolbar or navigation bar at the
top of the screen to display an Up or Back button in the top-left corner. It usually maintains a
LIFO stack of pages to handle the navigation. LIFO stands for last in, first out, which means
the last page to enter is the first one to pop out.

* Modal navigation is different from hierarchical navigation in terms of how users can respond
to it. If a modal page is displayed on the screen, the users must complete or cancel the required
task on the page before they can take other actions. The users cannot navigate away from modal
pages before the required task is completed or canceled.

# INavigation interface and NavigationPage

In .NET MAUI, both hierarchical navigation and model navigation are supported through the
INavigation interface. The INavigation interface is supported by a special page called
NavigationPage. NavigationPage is used to manage the navigation of a stack of other pages.
The inheritance hierarchy of NavigationPage looks like this:

```
Object > BindableObject > Element > NavigableElement > VisualElement >
Page > NavigationPage
```
NavigableElement defines a property called Navigation that implements the INavigation
interface. This inherited property can be called from any VisualElement or Page for navigation
purposes, as shown here:

```Csharp
public Microsoft.Maui.Controls.INavigation Navigation { get; }
```
To use NavigationPage, we must add the first page to a navigation stack as the root page of the
application. We can see an example of this in the following code snippet:

```Csharp
public partial class App : Application
{
 ...
 public App ()
 {
 InitializeComponent();
 MainPage = new NavigationPage (new TheFirstPage());
 }
 ...
}
```
We build the navigation stack in the constructor of the App class, which is a derived class of
Application. TheFirstPage, which is a derived class of ContentPage, is pushed onto the
navigation stack.

# Using the navigation stack

There are two ways to navigate to or from a page. When we want to browse a new page, we can add
the new page to the navigation stack. This action is called a push. If we want to go back to the previous
page, we can pop the previous page from the stack:

![image](https://user-images.githubusercontent.com/26972859/231789680-d873148c-665d-4b4a-83ad-92cda6551d19.png)

Figure 5.1: Push and pop

As shown in Figure 5.1, we can use the PushAsync() or PopAsync() method in the INavigation
interface to change to a new page or go back to the previous page, respectively.
If we are on Page1, we can change to Page2 with the GotoPage2() event handler. In this function,
we are pushing the new page, Page2, to the stack:

```Csharp
async void GotoPage2 (object sender, EventArgs e) {
 await Navigation.PushAsync(new Page2());
}
```
Once we are on Page2, we can go back with the BackToPage1() event handler. In this function,
we are popping the previous page from the stack:

```Csharp
async void BackToPage1 (object sender, EventArgs e) {
 await Navigation.PopAsync();
}
```
In the preceding example, we navigated to a new page using the hierarchical navigation method. To
display a modal page, we can use the modal stack. For example, in our app, if we want to create a new
item in ItemsPage, we can call PushModalAsync() in ItemsViewModel:

```Csharp
await Shell.Current.Navigation.
PushModalAsync(NewItemPage(type));
```
After the new item has been created, we can call PopModalAsync() in NewItemViewModel:

```Csharp
_ = await Shell.Current.Navigation.PopModalAsync();
```
On the model pages NewItemPage, we cannot navigate to other pages before we complete or
cancel the task. Both PopAsync() and PopModalAsync() return an awaitable task of the
```Task<Page>``` type.

# Manipulating the navigation stack

In hierarchical navigation, we can not only push or pop pages from the stack, but we can also manipulate
the navigation stack.

# Inserting a page

We can insert a page into the stack using the InsertPageBefore() method:

```Csharp
public void InsertPageBefore (Page page, Page before);  
```

The following are two parameters of InsertPageBefore():

  * page: This is the page to be added

  * before: This is the page before which the page is inserted

  In Figure 5.1, when we are at Page2, we can insert another page, Page1, before it:
Navigation.InsertPageBefore(new Page1(), this);

# Removing a page

We can also remove a specific page from the stack using the RemovePage() method:

```Csharp
public void RemovePage (Page page);
```
In Figure 5.1, given we have a reference of Page2 when we are at Page3, we can remove Page2
from the stack. After PopAsync() is called, we will be back at Page1:

```Csharp
// the reference page2 is an instance of Page2
Navigation.RemovePage(page2);
await Navigation.PopAsync();  
```
With that, we have learned how to build a navigation stack using NavigationPage. Once we
have a navigation stack, we can use the INavigation interface to perform navigation actions. For
a simple application, this may be good enough. However, there will be a lot of work involved for a
complex application. We have a better choice in .NET MAUI known as Shell. With Shell, we can
provide a better navigation experience to the users with less work.

# Using Shell

The INavigation interface and NavigationPage provide basic navigation functionalities. If we
rely on them only, we have to create complicated navigation mechanisms by ourselves. In .NET MAUI,
there are built-in page templates to choose from, and they can provide different navigation experiences.
As shown in the class diagram in Figure 5.2, there are built-in pages available for different use cases.
All these pages – TabbedPage, ContentPage, FlyoutPage, NavigationPage, and Shell
– are derived classes of Page:

![image](https://user-images.githubusercontent.com/26972859/231790667-363a88b4-4a0a-4753-8b02-0805c651c7a0.png)

Figure 5.2: Class diagram of the built-in pages in .NET MAUI

ContentPage, TabbedPage, and FlyoutPage can be used to create various user interfaces
per your requirements.

  * ContentPage is the most used page and can include any layout and view elements. It is
suitable in the case of a single-page design.
  * TabbedPage can be used to host multiple pages. Each child page can be selected by a series
of tabs at the top or bottom of the page.
  * Flyoutpage can display a list of items, which is similar to the menu items in a desktop
application. The user can navigate to individual pages through the items in the menu.

  Even though Shell is also a derived class of Page, it includes a common navigation user experience,
which can make developers’ lives easier. It helps the developers by reducing the complexity of application
development with highly customizable and rich features in one place.
Shell provides the following features:

  * A single place to describe the visual hierarchy of an app
  
  * A highly customizable common navigation user experience

  * A URI-based navigation scheme that is very similar to what we have in a web browser
  
  * An integrated search handler

  The top-level building blocks of Shell are flyouts and tabs. We can use flyouts and tabs to create the
navigation structure of our app.

# Flyout

A flyout can be used as the top-level menu of a Shell app. In our app, we must use both flyouts
and tabs to create the top-level navigation design. We will explore flyouts in this section; in the next
section, we will discuss how to use tabs in our app.
In Figure 5.3, we can see what the flyout looks like in our app. From the flyout menu, we can switch
to AboutPage, ItemsPage, or LoginPage. To access the flyout menu, we can either swipe from
the left of the screen or click the flyout icon, which is the hamburger icon ①. When we click Root
Group ② in the flyout menu, we will see a list of password entries or groups:

![image](https://user-images.githubusercontent.com/26972859/231791165-17ad57b0-a654-4dcc-b9a1-2e3bf0c1bf73.png)

Figure 5.3: Flyout

The flyout menu consists of flyout items or menu items. In Figure 5.3, About and Root Group are
flyout items, while Logout is a menu item.

# Flyout items
  
Each flyout item is a FlyoutItem object that contains a ShellContent object. We can define
flyout items like so in the AppShell.xaml file. We assign a string resource to the Title ①
attribute and an ImageSource to the Icon ② attribute. These correspond to the properties of the
FlyoutItem class:

```xml
<FlyoutItem ① Title="{x:Static resources:Resources.About}" ②
Icon="tab_info.png" >
 <Tab>
 <ShellContent Route="AboutPage" ContentTemplate=
 "{DataTemplate local:AboutPage}" />
 </Tab>
</FlyoutItem>
<FlyoutItem x:Name="RootItem" Title="Browse"
 Icon="tab_home.png">
 <Tab>
 <ShellContent Route="RootPage" ContentTemplate=
 "{DataTemplate local:ItemsPage}" />
 </Tab>
</FlyoutItem>  
```
Shell has implicit conversion operators that can be used to remove the FlyoutItem and Tab
objects so that the preceding XAML code can also be simplified, like so:

```xml
<ShellContent Title="{x:Static resources:Resources.
About}" Icon="tab_info.png" Route="AboutPage"
ContentTemplate="{DataTemplate local:AboutPage}" />
<ShellContent x:Name="RootItem" Title="Browse" Icon="tab_
home.png" Route="RootPage" ContentTemplate="{DataTemplate
local:ItemsPage}" />  
```

# Menu items

Flyout items can be used to navigate to a content page, but sometimes, we may want to take an action
instead of navigating to a content page. In this case, we can use menu items. In our case, we have
defined Logout as a menu item:  

```xml
 <MenuItem Text="Logout" IconImageSource="tab_login.png"
 Clicked="OnMenuItemClicked">
</MenuItem>
```
As we can see from the preceding XAML code, each menu item is a MenuItem object. The MenuItem
class has a Clicked event and a Command property. When MenuItem is tapped, we can execute
an action. In the preceding menu item, we assigned OnMenuItemClicked() as the event handler.
Let’s review AppShell.xaml in our app in Listing 5.1. Here, we defined two flyout items and one
menu item. We can select AboutPage ➊ and ItemsPage ➋ with flyout items and log out ➌
through the menu item:  
  
**Listing 5.1: AppShell.xaml in PassXYZ.Vault (https://epa.ms/AppShell5-1)**  

```xml
<Shell xmlns="http://schemas.microsoft.com
 /dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com
 /winfx/2009/xaml"
 xmlns:local="clr-namespace:PassXYZ.Vault.Views"
 xmlns:style="clr-namespace:PassXYZ.
 Vault.Resources.Styles"
 xmlns:resources="clr-namespace:PassXYZ.
 Vault.Properties"
 xmlns:app="clr-namespace:PassXYZ.Vault"
 Title="PassXYZ.Vault"
 x:Class="PassXYZ.Vault.AppShell">
 <Shell.Resources...>
 <TabBar> ➍
 <Tab Title="{x:Static resources:Resources.
 action_id_login}" Icon="tab_login.png">
 <ShellContent Route="LoginPage" ContentTemplate=
 "{DataTemplate local:LoginPage}" />
 </Tab>
 <Tab Title="{x:Static resources:Resources.menu
 _id_users}" Icon="tab_users.png">
 <ShellContent Route="SignUpPage" ContentTemplate=
 "{DataTemplate local:SignUpPage}" />
 </Tab>
 </TabBar>
 <FlyoutItem Title="{x:Static resources:Resources.About}"
 Icon="tab_info.png" >
 <ShellContent Route="AboutPage" ContentTemplate=
 "{DataTemplate local:AboutPage}" /> ➊
 </FlyoutItem>
 <FlyoutItem x:Name="RootItem" Title="Browse"
 Icon="tab_home.png">
 <ShellContent Route="RootPage" ContentTemplate=
 "{DataTemplate local:ItemsPage}" /> ➋
 </FlyoutItem>
 <MenuItem Text="Logout" IconImageSource="tab_login.png"
 Clicked="OnMenuItemClicked"> ➌
 </MenuItem>
</Shell>
```
There is also a TabBar ➍ defined for LoginPage and SignUpPage. Let’s review tabs now.  
  
# Tabs

When we use tabs, Shell can create a navigation experience similar to TabbedPage. As shown
in Figure 5.4, there are two tabs at the bottom tab bar on the Android and iOS platforms, but it looks
different on the Windows platform: 
  
![image](https://user-images.githubusercontent.com/26972859/231792921-49ce98c4-cb02-4d7c-8b16-01d4e769e375.png)
  
Figure 5.4: TabBar and tabs on Android
 
As we can see in Figure 5.5, on Windows, the tab bar is at the top:  
  
![image](https://user-images.githubusercontent.com/26972859/231793147-763895be-c97b-44d0-bf4c-36c995732ea4.png)
  
 Figure 5.5: TabBar and tabs on Windows
  
 To create tabs in our app, we must define a TabBar object. A TabBar object can contain one or
more Tab objects and each Tab object represents a tab on the tab bar. Each Tab object can contain
one or more ShellContent objects. The following XAML code shows that it is very similar to the
one we get when we define a flyout: 

```xml
<TabBar>
 <Tab Title="{x:Static resources:Resources.
 action_id_login}" Icon="tab_login.png">
 <ShellContent Route="LoginPage" ContentTemplate=
 "{DataTemplate local:LoginPage}" />
 </Tab>
 <Tab Title="{x:Static resources:Resources.menu_id_users}"
 Icon="tab_users.png">
 <ShellContent Route="SignUpPage" ContentTemplate=
 "{DataTemplate local:SignUpPage}" />
 </Tab>
</TabBar>  
```
Just like what we did in the flyout XAML code, we can make the preceding code a little simpler by
removing Tab tags. We can use the implicit conversion operators of Shell to remove Tab objects. As
we can see, we can remove Tab tags and define Title and Icon attributes in ShellContent tags:
  
```xml
<TabBar>
 <ShellContent Title="{x:Static resources:Resources.
 action_id_login}" Icon="tab_login.png"
 Route="LoginPage" ContentTemplate="{DataTemplate
 local:LoginPage}" />
 <ShellContent Title="{x:Static resources:Resources.
 menu_id_users}" Icon="tab_users.png"
 Route="SignUpPage" ContentTemplate="{DataTemplate
 local:SignUpPage}" />
</TabBar>  
```
If we define both TabBar objects and FlyoutItem objects in AppShell.xaml, TabBar disables
the flyout items. That’s the reason why, when we start our app, we can see a screen of tabs showing
login or signup pages. After the user logs in successfully, we can bring the user to RootPage, which
is the registered route in Listing 5.1. We’ll learn how to register routes and navigate using registered
routes in the next section. 
  
# Shell navigation  
  
In Shell, we can navigate to pages through registered routes. There are two ways to register routes.
The first way is to register routes in Shell’s visual hierarchy. The second way is to register them explicitly
using the RegisterRoute() static method of the Routing class.  

# Registering absolute routes  

We can register routes in Shell’s visual hierarchy as we did in Listing 5.1. We can specify a route through
the Route property of FlyoutItem, TabBar, Tab, or ShellContent. In AppShell.xaml,
we registered the following routes:  
  
|Route|Page|Description|
|:----|:---|:----------|
|LoginPage|LoginPage|This route displays a page for user login|
|SignUpPage|SignUpPage|This route displays a page for user signup|
|AboutPage|AboutPage|This route displays a page about our app|
|RootPage|ItemsPage|This route displays a page for navigating the password database|  
  
 Table 5.1: Registered routes in the visual hierarchy 
  
 To navigate to a route in Shell’s visual hierarchy, we can use an absolute route URI, such as //
LoginPage.
  
# Registering relative routes  

We can also navigate to a page without pre-defining it in the visual hierarchy. For example, we can
navigate to the password detail page, ItemDetailPage, at any hierarchy level of the password
groups. In our app, we can register the following routes explicitly using RegisterRoute() in App.
xaml.cs:  

```Csharp
public App()
{
 InitializeComponent();
 Routing.RegisterRoute(nameof(ItemsPage),
 typeof(ItemsPage));
 Routing.RegisterRoute(nameof(ItemDetailPage),
 typeof(ItemDetailPage));
 Routing.RegisterRoute(nameof(NewItemPage),
 typeof(NewItemPage));
 DependencyService.Register<MockDataStore>();
 DependencyService.Register<UserService>();
 MainPage = new AppShell();
} 
```
In the preceding code, we defined the following routes:  
  
|Route|Page|Description|
|:----|:---|:----------|
|ItemDetailPage|ItemDetailPage|This is the route to display details about a password entry|
|NewItemPage|NewItemPage|This is the route to add a new item (entry or group)| 
|ItemsPage|ItemsPage|This is the route to display a page for navigating the password database|  
  
Table 5.2: Registered detail page routes
  
To demonstrate how to use relative routes, we will add a new item. When we want to add a new item,
we can navigate to NewItemPage using a relative route, like so: 
  
```Csharp
await Shell.Current.GoToAsync(nameof(NewItemPage));  
```
In this case, the NewItemPage route is searched and if the route is found, the page will be displayed
and pushed to the navigation stack. The navigation stack here is the same as the one when we explained
basic navigation using the INavigation interface. When we define a relative route and navigate
to it, we pass a string as the name of the route. To avoid typos, we can use the class name as the route
name by using the nameof expression.
 
After we have filled in the information for the new item in NewItemPage, we may click the Save
or Cancel button. In the event handler of the Save or Cancel button, we can navigate back to the
previous page using the following code:
  
```await Shell.Current.Navigation.PopModalAsync();```

  Alternatively, we can use the following code: 
 
```await Shell.Current.GoToAsync("..");```

As you can see from the preceding code, there are two ways we can navigate back. The first one uses
the PopModalAsync() method of the INavigation interface. Since Shell itself is a derived
class of Page, it implements the INavigation interface through the inherited Navigation
property. We can call the modal PopModalAsync() navigation method to navigate back. Here,
NewItemPage is a modal page.  
  
The second approach is that we can use the GoToAsync() method to navigate back. Since
NewItemPage is a modal page, you may be wondering how we can differentiate whether a page
is a modal page or not when we call GoToAsync(). In Shell navigation, this is defined through
page presentation mode. The content page of NewItemPage is defined like so:  
  
```xml
<?xml version="1.0" encoding="UTF-8"?>
<ContentPage xmlns="http://schemas.microsoft.com
 /dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com
 /winfx/2009/xaml"
 x:Class="PassXYZ.Vault.Views.NewItemPage"
 Shell.PresentationMode="ModalAnimated" ➊
 Title="New Item">
 <ContentPage.Content...>
</ContentPage >  
```

As we can see, the Shell.PresentationMode ➊ property is defined on the content page.
Depending on whether we want to use animation, we can set a different value for this property. For a
normal content page, we can set it to NotAnimated or Animated. For a modal page, we can set it to
Modal, ModalAnimated, or ModalNotAnimated. If we use the default value, Animated is set.
To navigate back, the GoToAsync() method is used with the route set to ... This is a similar
mechanism to filesystem navigation or browser URL navigation. The relative route, .., means navigating
back to the parent route. It can be combined with a route to navigate a page at the parent level, like so:

```Csharp
await Shell.Current.GoToAsync("../AboutPage");
```
In Table 5.1 and Table 5.2, you will see that ItemsPage is registered as both the absolute route
RootPage and relative route ItemsPage. ItemsPage may contain password groups at different
levels. At the top level, it is an absolute route, but it is a relative route at all other navigation hierarchy levels.  
  
# Passing data to pages
  
To further explain why we register ItemsPage as both absolute and relative routes, let’s review the
navigation hierarchy of our app, as shown in Figure 5.6:  
  
![image](https://user-images.githubusercontent.com/26972859/231826996-3567f415-1b79-4335-b30e-22c9fb60fcee.png)
  
 Figure 5.6: Navigation hierarchy 
  
In our app, after successfully logging in, the main page displays a list of entries and groups at the top
level of the password database called Root Group. This is similar to the navigation of the filesystem.
At the root of the filesystem, the top level of files and folders are displayed.
The first instance of ItemsPage uses the RootPage route, which we can access through the
flyout item. Let’s say there are sub-groups called Group1 and Group2 in the Root Group, as shown in
Figure 5.6. We can navigate to these sub-groups, which are instances of ItemsPage. These instances of
ItemsPage cannot be pre-defined as they are relative routes and are pushed to the navigation stacks
on demand. These navigation stacks can be as deep as the actual data stored in the password database.
These two different routes of ItemsPage are defined in AppShell.xaml and App.xaml.cs
like so:
  
 1. The RootPage route (absolute route): 
  
 ```xml
  <FlyoutItem x:Name="RootItem" Title="Browse" Icon="tab_
home.png">
 <ShellContent Route="RootPage" ContentTemplate=
 "{DataTemplate local:ItemsPage}" />
</FlyoutItem>
 ```
2. The ItemsPage route (relative route):
  
```Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));```  
  
Here, you may be wondering how we can navigate to Group1 or Group2 from Root Group. If ItemsPage
can be used to display the content of either Group1 or Group2, how can we tell ItemsPage which
group to display?
  
In Shell navigation, data can be passed to a content page through query parameters. The syntax
is similar to what we pass to a URL in the web browser. For example, we can use the following URL
to search for .net in Google search: https://www.google.com.hk/search?q=.net.
This is achieved by appending ? after a route with a pair of query parameter IDs and the value. In the
preceding example, the key is q and the value is .net.
When an item in the list of Root Groups is selected, it can be an entry or a group. The click event
triggers the OnItemSelection() method in ItemsViewModel, as shown in Listing 5.2: 
  
**Listing 5.2: ItemsViewModel.cs (https://epa.ms/ItemsViewModel5-2)**  

```Csharp
using PassXYZ.Vault.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using KPCLib;
using PassXYZLib;
namespace PassXYZ.Vault.ViewModels;
[QueryProperty(nameof(ItemId), nameof(ItemId))] ➊
public class ItemsViewModel : BaseViewModel {
 private Item? _selectedItem = default;
 public ObservableCollection<Item> Items { get; }
 public Command LoadItemsCommand { get; }
 public Command AddItemCommand { get; }
 public Command<Item> ItemTapped { get; }
 public string ItemId { ➋
 get {
 return _selectedItem == null ? string.Empty :
 _selectedItem.Id;
 }
 set {
 if (!string.IsNullOrEmpty(value)) {
 var item = DataStore.GetItem(value, true);
 if (item != null) {
 _selectedItem = DataStore.CurrentGroup =
 DataStore.GetItem(value, true);
 }
 else {
 throw new ArgumentNullException("ItemId");
 }
 }
 else {
 _selectedItem = null;
 DataStore.CurrentGroup = DataStore.RootGroup;
 }
 ExecuteLoadItemsCommand();
 }
 }
 public ItemsViewModel()...
 ~ItemsViewModel()...
 public async Task ExecuteLoadItemsCommand()...
 async public void OnAppearing()...
 public Item? SelectedItem...
 private async void OnAddItem(object obj)...
 public async void OnItemSelected(Item item) {
 if (item == null) return;
 if (item.IsGroup) {
 await Shell.Current.GoToAsync
 ($"{nameof(ItemsPage)}?
 {nameof(ItemsViewModel.ItemId)}={item.Id}"); ➌
 }
 else {
 await Shell.Current.GoToAsync
 ($"{nameof(ItemDetailPage)}?
 {nameof(ItemDetailViewModel.ItemId)}={item.Id}");➍
 }
 }
}
```
According to the type of item, we may navigate to ItemsPage ➌ or ItemDetailPage ➍.In both
cases, we pass the Id item to the ItemId query parameter, which is defined in both ItemsViewModel
and ItemDetailViewModel.
In Listing 5.2, ➊ ItemId is defined in ItemsViewModel as the QueryPropertyAttribute
attribute. The first argument of QueryPropertyAttribute is the name of the property that will
receive the data. It is ItemId ➋ in this case.
The second argument is the id parameter. When we select a group from the list, the view model’s
OnItemSelected() method is invoked ➌ and the item Id of the selected group is passed as the
value of the ItemId query parameter.
When ItemsPage is loaded with the ItemId query parameter, the ItemId ➋ property is set.
In the setter of the ItemId property, we check whether the query parameter value is empty. If it
is empty, it could be the first time we navigate to the RootPage route without a query parameter.
In this case, we set CurrentGroup of the data service to RootGroup. If it is not empty, we will
find the item and set it to CurrentGroup. The content of CurrentGroup is loaded using the
ExecuteLoadItemsCommand() method.
➍ If we select an entry from the list, we can navigate to ItemDetailPage with the item Id as
the value of the query parameter. We can change ItemDetailViewModel like so to handle this
query parameter:  
  
```Csharp
public string? ItemId { ①
 get {
 return itemId;
 }
 set {
 if (value == null) throw new ArgumentNullException
 (nameof(value));
 itemId = value;
 LoadItemId(value); ②
 }
}
public ItemDetailViewModel() {
Fields = new ObservableCollection<Field>();
 Id = default;
}
public async void LoadItemId(string itemId) {
 try {
 var item = await DataStore.GetItemAsync(itemId); ③
 if (item == null) {
 throw new ArgumentNullException(nameof(itemId));
 }
 Id = item.Id;
 Title = item.Name;
 Description = item.Description;
 PwEntry dataEntry = (PwEntry)item; ④
 Fields.Clear();
 List<Field> fields = dataEntry.GetFields(GetImage:
 FieldIcons.GetImage); ⑤
 foreach (Field field in fields) {
 Fields.Add(field);
 }
 }
 catch (Exception) {
 Debug.WriteLine("Failed to Load Item");
 }
}
```
In the ItemDetailViewModel class, we have the following:
• ItemId ① is the property that receives the query parameter.
• In the setter of ItemId, we call the LoadItemId() method ② to load the item.
• In LoadItemId(), we can call the data service GetItemAsync() method ③ to get the
item using the item Id.
• Here, the item is an instance of PwEntry ④, so we can cast it as a PwEntry.
• We have an extension method called GetFields()⑤ in PassXYZLib. We use this method
to update the list of fields.

We learned about basic navigation and Shell navigation in the last two sections. We also improved
our navigation design using Shell. Now, it’s time to review the MVVM pattern and refine our data
model again to make our password manager app better.
  
# Improving our model  
  
We studied use cases and created some in Chapter 4, Exploring MVVM and Data Binding In this
section, we will enhance existing use cases and implement new use cases using the knowledge that
we’ve learned.
We will work on the following use cases.
Use Case 1: As a password manager user, I want to log in to the password manager app so that I can
access my password data.
For this use case, we haven’t fully implemented the user login yet; we will complete this in the next
chapter. In this chapter, we will implement some pseudo logic that includes everything except the
data layer.
In Chapter 4, Exploring MVVM and Data Binding, we have the following use case, which can support
one level of navigation.
Use case 3: As a password manager user, I want to see a list of groups and entries so that I can explore
my password data.
To support multiple levels of navigation, we will implement the following use case in this section.
Use case 6: As a password manager user, when I click a group in the current list, I want to see the
groups and entries belonging to this group.
Use case 7: As a password manager user, when I navigate my password data, I want to navigate back
to the previous group or parent group.
In use cases 6 and 7, we want to navigate forward or backward using the relative routes.
In the MVVM pattern, we access our model through services. These services are usually abstracted
as interfaces so that they’re separate from the actual implementation. The IDataStore interface is
one of them. To support use case 6 and improve use case 1, we need to create a new interface called
IUserService to support user login.  
  
# Understanding the data model and its services
  
To understand the services and the enhanced model, let’s review the enhanced design in Figure 5.7:  
  
![image](https://user-images.githubusercontent.com/26972859/231827995-9736dfda-048e-450b-b41f-0945214e3fb1.png)
  
 Figure 5.7: Model and service in MVVM 
  
Figure 5.7 is a class diagram that depicts most of our design in the MVVM pattern. We can read this
class diagram together with the following table to understand the MVVM pattern in our app. For
simplicity, I have not included everything. For example, you can add NewItemPage or SignUpPage
to Figure 5.7 and Table 5.3 by yourself: 
  
![image](https://user-images.githubusercontent.com/26972859/231828127-ce91a80e-7fb1-485e-9a16-0756d9e795de.png)
  
 Table 5.3: Classes and interfaces in the MVVM pattern 
  
To store application data, we usually store data in a database, which can be a relational database or
NoSQL database. In our case, our password database is not a relational database. However, when we
work on our design, we can still use the similar logic of relational databases to design our business
logic. We have three classes to represent our model – User, Item, and Field.
Item and Field are used to represent a password entry and the content of an entry. We can imagine
that an entry looks like a row in a table and that a Field is similar to a cell. We use PwEntry in
KeePassLib to model a password entry. A list of entries is a group and we use PwGroup to model
a group. Here, a group is similar to a table in a database. Fields with the same key values in a group
are similar to a column. To design the interface of our data services, we can use a similar strategy to
process data in our database.
How do we handle data in a database? You may have heard about CRUD operations. In our case, we
can use the enhanced Create, Read, Update, Delete, and List (CRUDL) operations to define the
interface of our service.
To process password entries and groups, we can use the IDataStore interface:  
  
```Csharp
public interface IDataStore<T>
{
 T? GetItem(string id, bool SearchRecursive = false);
 Task<T?> GetItemAsync(string id, bool SearchRecursive =
 false);
 Task AddItemAsync(T item);
 Task UpdateItemAsync(T item);
 Task<bool> DeleteItemAsync(string id);
 Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh =
 false);
}
```

  In the IDataStore interface, we define the following CRUDL operations:

  * Create: We use AddItemAsync() to add an entry or a group

  * Read: We use GetItem() or GetItemAsync() to read an entry or a group

  * Update: We use UpdateItemAsync() to update an entry or a group

  * Delete: We use DeleteItemAsync() to delete an entry or a group

  * List: We use GetItemsAsync() to get a list of entries and groups in the current group  
  
 To process users, we can use the IUserService interface: 
  
```Csharp
public interface IUserService<T>
{
 T GetUser(string username);
 Task AddUserAsync(T user);
 Task UpdateUserAsync(T user);
 Task DeleteUserAsync(T user);
 List<string> GetUsersList();
 Task<bool> LoginAsync(T user);
 void Logout();
}  
```

We can define a set of CRUDL operations to handle users as well:

  * Create: We can create a new user using AddUserAsync()

  * Read: We can get the user information using GetUser()

  * Update: We can update a user using UpdateUserAsync()

  * Delete: We can delete a user using DeleteUserAsync()

  * List: We can get a list of users using GetUsersList()

  * Login and Logout: We can log in or log out using an instance of User

  To further separate the dependency of model and service, we can use generic types in the interface
definition instead of concrete types. We use these services in our view models to manage our models. To
improve the efficiency of our code, we will initialize the IDataStore service in BaseViewModel
so that they are available in all derived view models automatically:  
  
```Csharp
public class BaseViewModel : INotifyPropertyChanged {
 public static IDataStore<Item> DataStore =>
 DependencyService.Get<IDataStore<Item>>();
 bool isBusy = false;
 public bool IsBusy...
 string title = string.Empty;
 public string Title...
 ...
}
```
In the BaseViewModel class, we initialize the IDataStore service through a dependency service.
We will explain dependency services and dependency injection in the next chapter.  
  
# Improving the login process  
  
For user management, we may add new users or delete obsolete users in the system. We only have
one user who can log in to our app at a time, so we must define a singleton class called LoginUser
to model this case:
  
**Listing 5.3: LoginUser.cs (https://epa.ms/LoginUser5-3)**  
  
```Csharp
using System.Diagnostics;
using PassXYZLib;
namespace PassXYZ.Vault.Services;
public class LoginUser : PxUser
{
 private const string PrivacyNotice = "Privacy Notice";
 public static bool IsPrivacyNoticeAccepted...
 private bool _isFingerprintEnabled = false;
 public bool IsFingerprintEnabled =>
 _isFingerprintEnabled;
 public static IUserService<User> UserService =>
 DependencyService.Get<IUserService<User>>(); ➊
 public override void Logout() {
 UserService.Logout();
 }
 public async Task<string> GetSecurityAsync()...
 public async Task SetSecurityAsync(string passwd)...
 public async Task<bool> DisableSecurityAsync()...
 private LoginUser() { } ➋
 private static LoginUser? instance = null;
 public static LoginUser Instance { ➌
 get {
 if (instance == null) { instance = new
 LoginUser(); }
 return instance;
 }
 } } }
}
```
LoginUser is inherited from the User class through a sub-class called PxUser.
➊ In LoginUser, we initialize the IUserService interface through a dependency service.
➋ To implement a singleton class, we make the default constructor private to disable the creation of
this class using a new operator.
➌ To get an instance of LoginUser, we must define a static property Instance. Please be aware
that I chose to use a no-thread-safe implementation here to keep the code simple. In production
implementation, we should use a lock to make it thread-safe.
Once we have implemented the IUserService interface and the LoginUser class, we can improve
the login process. So far, we only have one Button on the login page. Let’s add a username field and
a password field to LoginPage.xam, as shown in Listing 5.4:  
  
**Listing 5.4: LoginPage.xaml (https://epa.ms/LoginPage5-4)**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage...>
 <ContentPage.Content>
 <StackLayout Padding="30" Spacing="10">
 <Image HorizontalOptions="Center"
 HeightRequest="96"...>
 <Label FontSize="Small".../>
 <Frame Margin="10">
 <Grid x:DataType="viewmodels:LoginViewModel"...> ➊
 <Grid.RowDefinitions...>
 <Grid.ColumnDefinitions...>
 <!-- Row 1 (Username) -->
 <Image Grid.Row="0" Grid.Column="0"...>
 <Entry x:Name="usernameEntry" Placeholder="{x:Static
resources:Resources.
 field_id_username}" ReturnType="Next" Text="{Binding
 CurrentUser.Username}" HorizontalOptions="Fill"
 Grid.Row="0" Grid.Column="1" /> ➋
 <ImageButton x:Name="switchUsersButton"...>
 <!-- Row 2 (Password) -->
 <Image Grid.Row="1" Grid.Column="0"...>
 <Entry x:Name="passwordEntry" Placeholder="{x:Static
 resources:Resources.
 field_id_password}" IsPassword="true" Text="{Binding
 CurrentUser.Password}" HorizontalOptions="Fill"
 Grid.Row="1" Grid.Column="1" /> ➌
 <!-- Row 3 (ActivityIndicator ) -->
 <ActivityIndicator IsRunning="{Binding IsBusy}"
 Grid.Row="2" Grid.Column="1" IsVisible="{Binding
 IsBusy}" /> ➍
 <!-- Row 4 (Login Button) -->
 <Button Text="{x:Static resources:Resources.
 LoginPageTitle}" HorizontalOptions=
 "CenterAndExpand" Command="{Binding
 LoginCommand}" Grid.Row="3" Grid.Column="1" /> ❺
 </Grid>
 </Frame>
 <Label x:Name="messageLabel" />
 </StackLayout>
 </ContentPage.Content>
</ContentPage>
```
In this new user interface design, we did the following:
  
   * ➊ We created a 4x3 grid layout in a frame.

   * In the first two rows, we used two instances of Entry to hold the username ➋ and password

   * ➌. We created a data binding between the Text fields of Entry and the properties of CurrentUser, which is defined in the view model. It is an object of LoginUser.

   * ➍ In the third row, we used an ActivityIndicator control to show the login status,
which is bound to the IsBusy property of the view model.

   * ➎ In the last row, we defined a Button control for the login activity. There is a Command
property defined in Button that implements the ICommand interface. We used data binding
to link this Command property to the method in the view model to perform the login activity.
We can see the improved login user interface in Figure 5.8: 
  
![image](https://user-images.githubusercontent.com/26972859/231829440-dd737dd2-9f5d-4702-a2ea-4faa14828dc7.png)
  
 Figure 5.8: LoginPage 
  

 # The Command interface 
  
Without Command property support, we must create an event handler of the Clicked event in the
code-behind file of LoginPage. In the event handler, we call a method defined in the view model
that invokes LoginAsync() in IUserService to process login activity. Using the Command
property, we can bind to the LoginCommand property in the view model directly, so we don’t need to
create the event handler in the code-behind file of LoginPage. The code looks cleaner and simpler.  
  
Let’s review the improved LoginViewModel.cs file to find out more details:  
  
**Listing 5.5: LoginViewModel.cs (https://epa.ms/LoginViewModel5-5)**  
  
```Csharp
public class LoginViewModel : BaseViewModel
{
 readonly IUserService<User> userService = LoginUser.
UserService;
 private Action<string> _signUpAction;
 public Command LoginCommand { get; }
 public Command SignUpCommand { get; }
 public Command CancelCommand { get; }
 public LoginUser CurrentUser => LoginUser.Instance;
 public ObservableCollection<User>? Users ...
 public LoginViewModel() {
 LoginCommand = new Command(OnLoginClicked,
 ValidateLogin); ➊
 SignUpCommand = new Command(OnSignUpClicked,
 ValidateSignUp);
 CancelCommand = new Command(OnCancelClicked);
 CurrentUser.PropertyChanged +=
 (_, __) => LoginCommand.ChangeCanExecute(); ➋
 CurrentUser.PropertyChanged +=
 (_, __) => SignUpCommand.ChangeCanExecute();
 }
 public LoginViewModel(Action<string> signUpAction) ...
 private bool ValidateLogin() { ➌
 return !string.IsNullOrWhiteSpace(CurrentUser.Username)
 && !string.IsNullOrWhiteSpace(CurrentUser.Password);
 }
 private bool ValidateSignUp() ...
 public void OnAppearing() ...
 public async void OnLoginClicked() {
 try {
 IsBusy = true;
 if (string.IsNullOrWhiteSpace
 (CurrentUser.Password))...
 bool status = await userService.LoginAsync
 (CurrentUser); ➍
 if (status) {
 if (AppShell.CurrentAppShell != null) {
 AppShell.CurrentAppShell.SetRootPageTitle
 (DataStore.RootGroup.Name);
 string path = Path.Combine
 (PxDataFile.TmpFilePath,
 CurrentUser.FileName);
 if (File.Exists(path))...
 await Shell.Current.GoToAsync($"//RootPage");
 }
 else {
 throw (new NullReferenceException
 ("CurrentAppShell is null"));
 }
 }
 IsBusy = false;
 }
 catch (Exception ex) {
 IsBusy = false;
 string msg = ex.Message;
 if (ex is System.IO.IOException ioException) {
 msg = Properties.Resources.message_
 id_recover_datafile;
 }
 Await Shell.Current.DisplayAlert
 (Properties.Resources.LoginErrorMessage, msg,
 Properties.Resources.alert_id_ok);
 }
 }
}
```
In LoginViewModel (Listing 5.5), we defined a few Command properties. Let’s look at LoginCommand
to understand how to use the ICommand interface.  
  
In .NET MAUI, the Command class is defined like so:

```Csharp
public class Command : System.Windows.Input.ICommand
```   
➊ We use the following constructor to initialize LoginCommand:   
   
```Csharp
public Command (Action execute, Func<bool> canExecute);
```
The execute parameter, which is of the Action type, is the action to be invoked. Here, the
OnLoginClicked() method is assigned. When the user clicks the button, it will be executed.
We also assigned another method called ValidateLogin()➌ to the canExecute parameter. This
parameter is used to indicate whether this Command can be executed. In the ValidateLogin()
method, we check whether the username or password in CurrentUser is empty. If either of them
is empty, this Command cannot be executed, and the button should be grayed out.
To detect a change in the username or password, we can register ChangeCanExecute() to the
PropertyChanged handler ➋.
After the user clicks the button, the OnLoginClicked() method is called. In this method, we pass
CurrentUser to the IUserService method’s LoginAsync()➍ to handle the login process.
So far, we have improved our model and service to enhance the login process. If we recall the
class diagram in Figure 5.7, we have changed the source code at the view, view model, and service
layers to enhance our app. The actual model is encapsulated in two libraries called KPCLib and
PassXYZLib. We exposed the functionalities of these two libraries through the IdataStore and
IuserService interfaces. We enhanced our model by building the actual implementation classes
of these two interfaces. In the next chapter, we will continue improving these two service interfaces
and build a fully functional app.   
   
# Summary

   In this chapter, we learned about basic navigation and Shell. We used Shell as the navigation
framework in our app design. We explored the features of Shell and explained how to use it in our
app design.
After we completed most of the user interface design, we enhanced our model by making changes to
two service interfaces: IDataStore and IUserService. We improved the login process after
making changes in the view, view model, and service layers. In the service layer, we are still using the
MockDataStore class. However, we haven’t finalized the implementation in the IDataStore
service to perform the actual login activities yet. We will leave this to the next chapter.   
In the next chapter, we will explain dependency injection in .NET MAUI, which is a major difference
compared to Xamarin.Forms. We will learn how to register our services via dependency injection and
how to initialize our service through constructor injection or property injection. We will also create
the actual service to replace MockDataStore.   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
   
  
  
  
  
  
  
  
  

