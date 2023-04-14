# Understanding the Blazor Layout and Routing

In the last chapter, we learned how to design a login page using Blazor. The app layout and navigation
hierarchy are still XAML-based. Our app is using a mixed UI implementation of Blazor and XAML.
Blazor is a different choice of UI design for a .NET MAUI app. In this (second) part of the book, we will
rebuild the entire UI using Blazor. The first step of UI design usually starts from the implementation
of the layout and navigation, so in this chapter, we will introduce the layout and routing of Blazor.

We will cover the following topics in this chapter:

* Blazor routing

* Using Blazor layout components

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC or Mac. Please refer to Development environment setup in Chapter 1, Getting Started with
.NET MAUI, for the details.


The source code for this chapter is available in the following GitHub repository:

https://github.com/PacktPublishing/.NET-MAUI-Cross-PlatformApplication-Development/tree/main/Chapter08

The source code can be downloaded using the following Git command:

```
git clone -b chapter08 https://github.com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development PassXYZ.Vault2
```

# Understanding client-side routing

The routing and layout of Blazor are similar to the concept of Shell and navigation in the XAML world.
In Chapter 5, Navigation using .NET MAUI Shell and NavigationPage, when we introduced navigation
and Shell, we discussed the routing strategy of Shell. Shell provides a URI-based navigation experience
that uses routes to navigate to the selected pages. The routing of Blazor is quite similar to that.
The routing of Blazor provides a way to switch from one Razor page to another. The rendering of
Razor pages in BlazorWebView is similar to web apps running in a browser.
In a classic web application, when we load an HTML page in a browser, the HTML page is retrieved
from the web server. When we choose a different route, we load a new page from the server. For
Single-Page Applications (SPAs), things work a little differently.
Blazor WebAssembly apps are SPAs. When the app is started, the app is loaded in a browser. After that,
the navigation of pages only happens on the client side. This is so-called client-side routing. Blazor
Hybrid apps use client-side routing as well.

# Setup of BlazorWebView

To perform client-side routing, the router has to be installed at the application startup. In .NET MAUI
apps, the entry point of both XAML and Blazor is set up in App.xaml.cs. We can refer to changing
App.xaml.cs here (in the code) to switch the UI implementation from XAML to Blazor.
The Blazor Hybrid app runs inside BlazorWebView. To start the Blazor Hybrid app, we need to
set up an instance of BlazorWebView first. In the last chapter, we set it up in LoginPage and we
navigated back to Shell after logging in successfully.
To set up an instance of BlazorWebView for the entire application, we need to replace the instance
assigned to the MainPage property of the App class. To do this, we changed the constructor of the
App class (in App.xaml.cs) as follows:

```Csharp
public App()
{
 InitializeComponent();
#if MAUI_BLAZOR
 MainPage = new MainPage(); ❶
#else
 Routing.RegisterRoute(nameof(ItemsPage),
 typeof(ItemsPage));
 Routing.RegisterRoute(nameof(ItemDetailPage),
 typeof(ItemDetailPage));
 Routing.RegisterRoute(nameof(NewItemPage),
 typeof(NewItemPage));
 MainPage = new AppShell();
#endif
}
```
❶ We can define a symbol, MAUI_BLAZOR, to set up conditional compilation so that we can switch
between XAML and Blazor UI in the build. To use Blazor UI, we set the MainPage property to a
MainPage instance. In the MainPage class, we define the BlazorWebView control as follows:

```xml
<BlazorWebView HostPage="wwwroot/index.html">
 <BlazorWebView.RootComponents>
 <RootComponent Selector="#app" ComponentType="{x:Type
 local:Main}" />
 </BlazorWebView.RootComponents>
</BlazorWebView>
```
In BlazorWebView, it loads an HTML page (index.html) to start the Blazor UI setup. Let’s see
how the Router setup works.

# Setup of Router

Blazor UI is an HTML page-based UI design. It is similar to an SPA and starts from a static HTML
page. In BlazorWebView, the HTML page to be loaded is index.html, which is very similar to
the login.html page that we introduced in the last chapter. The top-level Razor component that
is loaded in RootComponent is the Main component that we can see here:

**Listing 8.1: Main.razor (https://epa.ms/Main8-1)**

```xml
<Router AppAssembly="@typeof(Main).Assembly"> ①
 <Found Context="routeData"> ②
 <RouteView RouteData="@routeData"
 DefaultLayout="@typeof(MainLayout)" />
 <FocusOnNavigate RouteData="@routeData" Selector="h1" />
 </Found>
 <NotFound> ③
 <LayoutView Layout="@typeof(MainLayout)">
 <p role="alert">Sorry, there's nothing at this
 address.</p>
 </LayoutView>
 </NotFound>
</Router>
```
As we can see in Listing 8.1, we set up the Router component, ①, in Main.razor.
In the Router component, it uses reflection to scan all the page components to build a routing table.

The AppAssembly parameter specifies the assemblies to be scanned.

If there is a navigation event, the router checks the routing table for a matching route. The Router
component is a templated component. We will discuss what a templated component is in a later
chapter. When a route is found, the Found template is used. Otherwise, the NotFound template is
used when there are no matching routes.

The Found template, ②, uses a RouteView component to render the selected component with its
layout. The layout is specified in the DefaultLayout attribute. We will discuss the layout in the
next section. The new page to be loaded, along with any route parameters, is passed using an instance
of the RouteData class.

If a match could not be found, the NotFound template, ③, is rendered. The NotFound template
uses a LayoutView component to display error messages. The layout used by LayoutView is
specified using a Layout attribute.

# Defining routes

Once we have set up the router, we can create pages and define route templates in pages. The router
will scan the route templates defined in pages to build a routing table.
At a high level, we can create the navigation hierarchy and route templates of our app referring to Figure 8.1.

![image](https://user-images.githubusercontent.com/26972859/231964172-912c7d9f-9933-4094-a550-7b32ed2cb1c8.png)

Figure 8.1: Navigation hierarchy of Razor pages

We listed the major pages in our app in Figure 8.1. Each page has a name that is also the class name
of a Razor page. The path under the name is the route template. For example, for the About page,
we can declare the route template as follows:

```
@page "/about"
```
The @page directive includes two parts – the directive name, and the route template. In this example,
the route template is "/about", which must be in quotes and always starts with a forward slash (/).
Since the final output of a Razor page is an HTML page, we can navigate to a Razor page just like a
web page using an anchor tag, <a>, as shown here:

```
<a href="/about">About</a>
```

## Passing data using route parameters

When we navigate to a page using a route template, we can pass data to the page using route parameters.
If we recall how to pass data with the query parameter in Shell, the usage of the route parameter is
similar to the query parameter.

As we can see in Figure 8.1, after we log in successfully, the Items page is shown and a list of items
in the root group is displayed as in Figure 8.2. On this page, if we click on an item, we can navigate
to the selected item based on the item type. To find the selected item, an item Id value is passed to
the new page as a parameter.

![image](https://user-images.githubusercontent.com/26972859/231964651-0375bb82-2280-4c42-a75e-b7741dcaa54e.png)

Figure 8.2: Items page in a Blazor Hybrid app

In the Items page, we have the following route templates defined:

```
@page "/group"
@page "/group/{SelectedItemId}"  
```
The first route template is used when we display the root page. The second one is used when we select a
group. The group Id value is passed to the Items page using the SelectedItemId route parameter.
To specify the type of route parameter, we can add constraints to it with the data type as shown here:

@page "/user/{Id:int}"

In the preceding page directive, we specify the data type of Id as an integer. Please refer to the
corresponding Microsoft documents to find more details about route constraints. You can find the
relevant document here:

https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/
routing?view=aspnetcore-6.0#route-constraints

# Navigating with NavigationManager

In a Razor page, although we can generally navigate to another page using an <a> anchor tag,
sometimes, we might need to do it using code. For example, when we handle an event, we may redirect
to a page in the event handler. This is exactly the case on our Login page. Let’s see how to navigate
to the Items page using NavigationManager after logging in successfully.
In our app, we need to redirect to the Items page to display the root group after login. The UI of the
Login page is the same as the one in the last chapter, but we changed the event handle in Login.
razor.cs to the following one:

```Csharp
namespace PassXYZ.Vault.Pages;
public partial class Login : ComponentBase {
 [Inject]
 private IUserService<User> userService { get; set; } =
 default!;
 [Inject]
 private IDataStore<Item> dataStore { get; set; } =
 default!;
 [Inject]
 private NavigationManager navigationManager {get; set;} ❶
 private LoginUser currentUser => LoginUser.Instance;
 private async void OnLogin(MouseEventArgs e) {
 bool status = await userService.LoginAsync
 (currentUser);
 if (status) {
 string path = Path.Combine(PxDataFile.TmpFilePath,
 currentUser.FileName);
 if (File.Exists(path)) {
 bool result = await dataStore.MergeAsync(path);
 }
 navigationManager.NavigateTo("/group"); ❷
 }
 }
}
```
❶ We get an instance of NavigationManager using dependency injection.
❷ We call the NavigateTo("/group") method of NavigationManager to navigate to the
Items page.
We learned how to use routing and navigation in this section. In the next step, we can implement a
navigation hierarchy similar to what we did with Shell navigation with Blazor UI.
The top level of the HTML page navigation hierarchy includes a header, toolbar, menu, and footer.
We can implement the layout using a Blazor layout component. It is something similar to the flyout
and menu items in Shell, which we introduced in Chapter 5, Navigation using .NET MAUI Shell
and NavigationPage.

# Using Blazor layout components

Most web pages usually contain fixed parts, such as a header, footer, or menu. We can design a page
using a layout together with the page content to reduce the redundant code. The page itself contains
the content that we want to show the users and the layout helps to build the styles and provide
navigation methods.
Blazor layout components are a class derived from LayoutComponentBase. Anything we can do
with a regular Razor component we can do to the layout components as well.
In Listing 8.1, we can see that MainLayout is used as the default layout of the pages. It is defined
in Listing 8.2 here:

**Listing 8.2: MainLayout.razor (https://epa.ms/MainLayout8-2)**

```
@inherits LayoutComponentBase ❶
<div class="page">
 <div class="sidebar">
 <NavMenu /> ❷
</div>
 <main>
 @Body ❸
 </main>
</div>
```
In the MainLayout component, ❶, it inherits the LayoutComponentBase class. ❷ It includes
a NavMenu component to define the menu for navigation. ❸ Inside the <main> tag, it uses the @
Body Razor syntax to specify the location in the layout markup where the content is rendered.

  Let’s review the NavMenu component in detail since it is the top-level navigation method in our app.
Please refer to Figure 8.3 to see the UI of NavMenu before we review the code. NavMenu includes
three menu items Home, About, and Logout.

![image](https://user-images.githubusercontent.com/26972859/231965554-be541599-efa4-42e1-8ea6-14948bf09a24.png)

Figure 8.3: NavMenu

NavMenu is a Razor component that defines the links for navigation. We can review the source code
of NavMenu in Listing 8.3 here:

**Listing 8.3: NavMenu.razor (https://epa.ms/NavMenu8-3)**

```xml
<div class="top-row ps-3 navbar navbar-dark"> ①
 <div class="container-fluid">
 <a class="navbar-brand" href="">PassXYZ.Vault</a>
<button title="Navigation menu" class="navbar-toggler"
 @onclick="ToggleNavMenu"> ②
 <span class="navbar-toggler-icon"></span>
 </button>
 </div>
</div>
<div class="@NavMenuCssClass" @onclick="ToggleNavMenu">
 <nav class="flex-column">
 <div class="nav-item px-3"> ③
 <NavLink class="nav-link" href="/group"> ④
 <span class="oi oi-home" aria-hidden="true"></span>
 Home
 </NavLink>
 </div>
 <div class="nav-item px-3">
 <NavLink class="nav-link" href="/about">
 <span class="oi oi-plus" aria-hidden="true"></span>
 About
 </NavLink>
 </div>
 <div class="nav-item px-3">
 <NavLink class="nav-link" href="" Match=
 "NavLinkMatch.All">
 <span class="oi oi-list-rich" aria-hidden="true">
 </span>
 Logout
 </NavLink>
 </div>
 </nav>
</div>
@code {
 private bool collapseNavMenu = true;
 private string NavMenuCssClass => collapseNavMenu ?
 "collapse" : null;
 private void ToggleNavMenu() {
 collapseNavMenu = !collapseNavMenu;
 }
}
```
In the source code of the NavMenu component, we can see that it is a Bootstrap navBar component
with some C# logic in the code block. ① NavBar is defined using a navbar Bootstrap class in the
<div> tag as follows:

```<div class="top-row ps-3 navbar navbar-dark">```

As we can see in Figure 8.3, there is a hamburger icon, ②, in the top left of the screen using a <button>
tag to toggle NavMenu. The hamburger button UI is implemented using the navbar-toggler
Bootstrap class as follows:

```xml
<button title="Navigation menu" class="navbar-toggler"
 @onclick="ToggleNavMenu">
 <span class="navbar-toggler-icon"></span>
 </button>
```
There are three links defined as the menu items using the nav-item Bootstrap class, ③. The link is
defined using NavLink ④ instead of the anchor tag, <a>. The NavLink component behaves like
<a> except it toggles an active CSS class based on whether its href matches the current URL
as we can see here:

```xml
<div class="nav-item px-3">
 <NavLink class="nav-link" href="/group">
 <span class="oi oi-home" aria-hidden="true"></span>
 Home
 </NavLink>
 </div>
```
We explained MainLayout, which is the default layout in our app. Let us look at how to apply the
layout to a component.

# Applying a layout to a component

MainLayout is used as the default layout component, so it will apply to all pages if we don’t specify
a layout. Sometimes, we need to use a specific layout instead of the default layout. For example, in our
app, we use a different layout component on the Login page instead of the default layout (see Listing
8.4). MainLayout includes a NavMenu component. We do not want to show it on the Login page
since we don’t allow the user to see any other content before logging in. Let us look at the changes to
the Login page after we apply a specific layout in Listing 8.4:

# Listing 8.4: Login.razor (https://epa.ms/Login8-4)

```
@page "/"
@layout LogoutLayout ❶
@namespace PassXYZ.Vault.Pages
<div class="text-center">
 <main class="form-signin">
 <form>
 <img id="first" class="mb-4" src=
 "passxyz-blue.svg"...>
 <h1 class="h3 mb-3 fw-normal">Please sign in</h1>
 <div class="form-floating">
 <label for="floatingInput">Username</label>
 <input type="text"
 @bind="@currentUser.Username"...>
 </div>
 <div class="form-floating">
 <label for="floatingPassword">Password</label>
 <input type="password" @bind=
 "@currentUser.Password"...>
 </div>
 <div class="checkbox mb-3">
<label>
 <input type="checkbox" value="remember-me">
 Remember me
 </label>
 </div>
 <button...>Sign in</button>
 <p class="mt-5 mb-3 text-muted">&copy; 2021–2022</p>
 </form>
 </main>
</div>
```
To use a specific layout, we can use the @layout Razor directive, ❶. In the Login page, we use the
LogoutLayout layout. The code of LogoutLayout is shown in Listing 8.5 here:

**Listing 8.5: LogoutLayout.razor (https://epa.ms/LogoutLayout8-5)**

```
@inherits LayoutComponentBase
<div class="page">
 <main>
 <div class="top-row px-4">
 <a href="#" target="_blank">Sign-in</a>
 </div>
 <article class="content px-4">
 @Body
 </article>
 </main>
</div>
```
In LogoutLayout, we removed the NavMenu element and added a sign-in link to allow a new
user to sign up.

# Nesting layouts

Layout components can also be nested. In MainLayout, we did not specify any margin for the content.
MainLayout is suitable for a content list view on an items page or item details page. However, it
does not look good for content pages, such as the About page. We can use a different layout for an
About page and this layout is nested in MainLayout. We can call it PageLayout and we can
see an implementation in Listing 8.6:

# Listing 8.6: PageLayout.razor (https://epa.ms/PageLayout8-6)

```
@inherits LayoutComponentBase
@layout MainLayout
<article class="content px-4">
 @Body
</article>

```
PageLayout is a layout component that uses MainLayout. It puts @Body in an <article>
tag with the "content px-4" style applied so that the content can apply the style that is suitable
for a paragraph of text.
In the About page, we can set the layout to PageLayout like so:

```
@page "/about"
@layout PageLayout
```
We have now introduced the routing and layout of Blazor. With this knowledge, it’s time to implement
the navigation elements of our app.

# Implementing navigation elements

In Chapter 5, Navigation using .NET MAUI Shell and NavigationPage, when we introduced Shell, we
mentioned the absolute route and relative route in Shell. We can define absolute routes in a visual
navigation hierarchy and navigate to the relative route through query parameters.
This navigation strategy is similar in the Blazor version of our app. As we can see in Figure 8.4, we
implement Blazor UI elements in the same way as our XAML version.

![image](https://user-images.githubusercontent.com/26972859/231966410-957e2ae2-8179-435f-9625-0b8a2f6e8453.png)

Figure 8.4: Navigation elements

The Items page is the main page of our app after login. In the Items page, in which the list of items
is displayed, the following UI elements are related to the navigation:
  

  * A list view – The user can navigate through the list and select an item.

  * Context menu – It is associated with each item in the list view. The user can edit or delete an
item using the context menu.

  * Back button – The user can use it to navigate back.

  * Add button – The user can use it to add new items.
In this section, we will implement the preceding navigation elements using the knowledge we have gained.

# Implementing a list view

In the XAML version, our navigation starts from a list of items after the user logs in to the app. The list
view is implemented using a .NET MAUI ListView control, which uses the underlying platformspecific UI component, so the look and feel are the same as the platform-specific one. In the Blazor
version, we use a web UI, so the look and feel are the same on different platforms.
To implement a list view using a web UI, we have many choices. In this book, we stick with the
Bootstrap framework. The way that we are going to do so is the same as in the last chapter. We can
reuse the UI design from the Bootstrap examples. We are using Bootstrap 5.1 in this book, so we can
refer to the following list group example shown in Figure 8.5.

![image](https://user-images.githubusercontent.com/26972859/231966606-f2fa80af-ea7a-488b-b74e-b3e14bfc72e2.png)

Figure 8.5: Bootstrap list group 

The preceding example can be found at the following URL:

https://getbootstrap.com/docs/5.1/components/list-group/
 
A Bootstrap list group can be used to build a UI-like ListView component in XAML. To do this,
we can apply a CSS class, list-group, to HTML tags, such as <ul> or <div>, to create a list
group. Inside the list group, the list-group-item CSS class is applied to the list item in the group.

In the XAML version, we support CRUD operations using the context menu. However, there is no
context menu available in the Bootstrap list group, so we need to implement a context menu by ourselves.
To implement a context menu in the list group, we can use the dropdown component of Bootstrap.
To use the dropdown component, we need to include the JavaScript dependency in index.html
as follows:

```xml
<script src="_framework/blazor.webview.js"
 autostart="false"></script>
<script src="css/bootstrap/bootstrap.bundle.min.js">
 </script>  
```
We added a JavaScript file, bootstrap.bundle.min.js, after blazor.webview.js. The
bootstrap.bundle.min.js JavaScript file is part of the Bootstrap release package.

To create a new Razor component, Items, we can right-click on the Pages folder in Visual Studio
and select Add -> Razor Component… to create it. We add the following code in Listing 8.7 and
name the Razor file Items.razor:

**Listing 8.7: Items.razor (https://epa.ms/Items8-7)**

```
@page "/group"
@page "/group/{SelectedItemId}"
<!-- Back button and title -->
<div class="container">...
<!-- List view with context menu -->
<div class="list-group"> ❶
 @foreach (var item in items) {
 <div class="dropdown list-group-item
 list-group-item-action...> ❷
 <img src="@item.GetIcon()"...>
 <a href="@item.GetActionLink()"
 class="list-group-item...>
 <div class="d-flex">
 <div>
 <h6 class="mb-0">@item.Name</h6>
 <p class="mb-0 opacity-75">@item.Description
 </p>
 </div>
 </div>
 </a>
 <button class="opacity-50 btn btn-light
 dropdown-toggle"
 type="button" id="itemsContextMenu"
 data-bs-toggle="dropdown" aria-expanded="false">
 <span class="oi oi-menu" aria-hidden="true"></span>
 </button> ❸
 <ul class="dropdown-menu"
 aria-labelledby="itemsContextMenu"> ❹
 <li>
 <button class="dropdown-item"
 data-bs-toggle="modal"
 data-bs-target="#editModel"> Edit </button>
 </li>
 <li>
 <button class="dropdown-item"
 data-bs-toggle="modal"
 data-bs-target="#deleteModel"> Delete </button>
 </li>
 </ul>
 </div>
 }
</div>
<!-- Editing Modal -->
<div class="modal fade" id="editModel" tabindex="-1"
 aria-labelledby="editModelLabel" aria-hidden="true">...
<!-- Deleting Modal -->
<div class="modal fade" id="deleteModel" tabindex="-1"
 aria-labelledby="deleteModelLabel" aria-hidden="true">...
<!-- New Modal -->
<div class="modal fade" id="newItemModel" tabindex="-1" arialabelledby="newItemModelLabel" aria-hidden="true">...
```
In Items.razor, ❶, we can the copy Bootstrap list group sample code that uses the <div> tag
by applying the list-group CSS class to it.
  
❷ We customize the list group item to meet our requirements as you can see in Figure 8.6. The list
group item is created inside a foreach loop using the <div> tag, which includes an icon, a name,
a description, and a context menu:

```
<div class="dropdown list-group-item list-group-item-action...>
```
We apply dropdown, list-group-item, and list-group-item-action, CSS classes, to
the <div> tag, so it is a list group item including a context menu using a dropdown.

![image](https://user-images.githubusercontent.com/26972859/231967189-602c82a2-7c6b-4b22-9185-036cba73db0c.png)

Figure 8.6: List group item

Inside the list group item, we use an <img> tag to display the item icon:

```
<img src="@item.GetIcon()"...> 
```
We can get the icon source using an extension method, GetIcon(), from the Item class. To create
the extension method, we add a new class file under the Shared folder and name it ItemEx.cs
as shown in Listing 8.8.
                          
An <a> anchor tag is used to display the name and description of the item. Inside <a>, the name
and description are defined like so:

```
<a href="@item.GetActionLink()" class=
 "list-group-item...>
 <div class="d-flex">
 <div>
 <h6 class="mb-0">@item.Name</h6>
 <p class="mb-0 opacity-75">@item.Description
 </p>
 </div>
 </div>
 </a>
```
We can get the link of the item using the GetActionLink() extension method, which is also
defined in Listing 8.8.
The context menu is a Bootstrap dropdown component including a <button> tag, ❸, and an
unordered list using the <ul> tag, ❹. This button is displayed as a hamburger icon using Open
Iconic font.

Open Iconic icons
  
We use Open Iconic icons in Blazor UI design. Open Iconic is an open source icon set with
223 icons in SVG, web font, and raster formats. In XAML design, we use FontAwesome, which
can also be used in Blazor with Bootstrap. However, we need extra configuration before we
can use it. Open Iconic is included in the Blazor template together with Bootstrap. We can use
it directly without any extra configuration. For example, to display a hamburger icon in the
context menu, we can use the following HTML tag:

```<span class="oi oi-menu" aria-hidden="true"></span>```

In the drop-down menu, there are two context action buttons, Edit and Delete. We apply the
dropdown-item CSS class to the buttons. The context action button triggers a dialog box to perform
the CRUD operations so that there are two Bootstrap modal CSS attributes, data-bs-toggle and
data-bs-targe, applied to it. We will discuss how to handle CRUD operations in the next chapter.

Let us review the extension methods of Item that we will use to support a list view UI in Listing 8.8:

**Listing 8.8: ItemEx.cs (https://epa.ms/ItemEx8-8)**

```Csharp
using KeePassLib;
using KPCLib;
using PassXYZLib;
namespace PassXYZ.Vault.Shared {
 public static class ItemEx {
 public static string GetIcon(this Item item) { ①
 if(item.IsGroup) {
 // Group
if(item is PwGroup group) { if(group.
CustomData.Exists(PxDefs.PxCustomDataIconName)) {
 return $"/images/{group.CustomData.Get
 (PxDefs.PxCustomDataIconName)}";
 }
 }
 }
 else {
 // Entry
 if(item is PwEntry entry) {
 if(entry.CustomData.Exists
 (PxDefs.PxCustomDataIconName)) {
 Return $"/images/{entry.CustomData.Get
 (PxDefs.PxCustomDataIconName)}";
 }
 }
 }
 // 2. Get custom icon
 return item.GetCustomIcon();
 }
 /// <summary>
 /// Get the action link of an item.
 /// </summary>
 public static string GetActionLink(this Item
 item, string? action = default) { ②
 string itemType = (item.IsGroup) ?
 PxConstants.Group : PxConstants.Entry;
 return (action == null) ? $"/{itemType}/{item.Id}" :
 $"/{itemType}/{action}/{item.Id}";
 }
 /// <summary>
 /// Get the parent link of an item.
 /// </summary>
 public static string? GetParentLink(this Item item) {③
Item? parent = default;
 if (item == null) return null;
 if(item.IsGroup) {
 PwGroup group = (PwGroup)item;
 if (group.ParentGroup == null) return null;
 parent = group.ParentGroup;
 }
 else {
 PwEntry entry = (PwEntry)item;
 if (entry.ParentGroup == null) return null;
 parent = entry.ParentGroup;
 }
 return $"/{PxConstants.Group}/{parent.Id}";
}
 }
}
```
In Listing 8.8, we define a static class, ItemEx, to implement an extension method of the Item class.
In this class, we defined three extension methods to get the URL needed for navigation:

  * GetIcon(), ① – returns the URL of the icon image

  * GetActionLink(), ② – return the URL of a selected item based on the item type

  * GetParentLink(), ③ – returns the URL of the parent item

  With the preceding implementation of a list view UI, we have a list, including password entries and
groups. When an item is selected, we actually click an anchor tag, <a>. The href property of <a> is
set to the return value of the GetActionLink() method. The return value of this method is
in the format of a "/{itemType}/{item.Id}" route template, which can be used to navigate
to the selected item. At the right-hand side of each item, there is a context menu button. If we click
on it, a list of context actions is shown, and we can select an action to edit or delete the current item.
We can handle most of the navigation actions now, but there are still two actions missing. When we
enter a child group, we cannot navigate back, and we also cannot add a new item. We will add these
two functions in the next section.

# Adding a new item and navigating back

To support navigating back and adding a new item, we can add a Back button and an Add button in
the title bar to simulate the navigation page of the XAML version as shown in Figure 8.7:

![image](https://user-images.githubusercontent.com/26972859/231967775-3838e1fa-332c-4e01-843b-6e3d72e38929.png)

Figure 8.7: The Title bar of the Items page

As we can see in the title bar here, three UI elements are included:
  
   * Title – the title of the current item group

  * The Back button – the button can be used to navigate back, but it won’t be shown when there is no parent group

  * The Add button – the button can be used to add a new item

  To review the implementation, we can expand the code of the Back button and the Title part in Listing 8.7 as shown here:

```
<!-- Back button and title -->
<div class="container">
 <div class="row">
 <div class="col-12">
 <h1>
 @if (selectedItem!.GetParentLink() != null) {
 <a class="btn btn-outline-dark" href=
 "@selectedItem!.GetParentLink()"><span
 class="oi oi-chevron-left"
 aria-hidden="true"></span></a> ❶
 }
 @(" " + Title)
 <button type="button" class="btn btn-outline-dark
 float-end" data-bs-toggle="modal" data-bs-
 target="#newItemModel"><span class="oi
 oi-plus" aria-hidden="true"></span></button> ❷
 </h1>
 </div>
</div>
</div>
```
The Back button, ❶, is implemented as an anchor tag, <a>. The href attribute of the anchor tag is
set to the return value of the Item extension method, GetParentLink(). This function returns
the link of the parent item in the route template format, so we can navigate back using this link. If
there is no parent group, such as a root group, the Back button is not shown.

The Add button, ❷, is implemented using a <button> tag. The Add button is displayed on the
right-hand side of the title bar. To push this button to the right of the screen, we can use a Bootstrap
class, float-end. When the user clicks on this button, a new item dialog box is shown. The dialog
box is set using the following attributes:

```data-bs-toggle="modal" data-bs-target="#newItemModel"```

There are three Bootstrap modal dialogs used in Items.razor as shown in Listing 8.7:

```
<!-- Editing Modal -->
<div class="modal fade" id="editModel" tabindex="-1"
 aria-labelledby="editModelLabel" aria-hidden="true">...
<!-- Deleting Modal -->
<div class="modal fade" id="deleteModel" tabindex="-1"
 aria-labelledby="deleteModelLabel" aria-hidden="true">...
<!-- New Modal -->
<div class="modal fade" id="newItemModel" tabindex="-1" arialabelledby="newItemModelLabel" aria-hidden="true">...
```
We use these dialogs to perform CRUD operations. To implement these dialogs, we also reuse code
from Bootstrap. It is quite straightforward to do it this way, but there is a lot of duplicated code involved.
To save space, they are collapsed in Listing 8.7. In the next chapter, we will explain the implementation
of model dialogs and convert the code into reusable Razor components.

# Summary

In this chapter, we introduced the routing and layout of Blazor. These are the components that we
can use to build the navigation hierarchy of our app. By the end of this chapter, we can now perform
basic navigation as in the XAML version of our app.
When we built the UIs in this chapter, we saw that the UI design technique of Blazor is the same as
web UI design. We can reuse the code from an existing framework, such as Bootstrap

If we want to create the UI on our own, we can work on the initial design in a playground first. After
we are satisfied with the UI design, we can copy HTML and CSS code into our Razor file to build
a Razor component. There are many playgrounds used by frontend developers, such as CodePen,
JSFiddle, CodeSandbox, and StackBlitz.

In this chapter, we reused Bootstrap examples to build our UIs. Even though this is a straightforward
way to implement a web UI, there are many duplicated codes created. In the next chapter, we will
refine our code and convert the code into reusable Razor components. We will implement the CRUD
operations to add, edit, and delete items using these Razor components.






































































































































































































