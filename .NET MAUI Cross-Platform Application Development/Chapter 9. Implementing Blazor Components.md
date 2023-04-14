# Chapter 9. Implementing Blazor Components

In the last chapter, we learned about Blazor routing and layout. We then built a navigation framework
by creating the routing and layout of our app. After we created the navigation framework, we created
the top-level pages. With the implementation of Razor pages, we can explore the password database
in a similar way that we can explore in the XAML version. Razor pages are Razor components, but
they are not reusable. Razor components are building blocks of the Blazor UI. In this chapter, we
will introduce Razor components. To understand Razor components, we will introduce data binding
and the Razor component life cycle. After learning about these concepts, we will refine our code and
convert duplicated code into reusable Razor components. Finally, we will use the Razor components
that we build to implement CRUD operations in our app.

We will cover the following topics in this chapter:

* Introducing Razor components

* Data binding

* Understanding the life cycle of Razor components

* Implementing CRUD operations

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC or Mac. Please refer to the Development environment setup section in Chapter 1, Getting
Started with .NET MAUI, for the details.

The source code of this chapter is available in the following GitHub repository:

https://github.com/PacktPublishing/.NET-MAUI-Cross-PlatformApplication-Development/tree/main/Chapter09

The source code can be downloaded using the following git command:

```
git clone -b chapter09 https://github.com/PacktPublishing/.
NET-MAUI-Cross-Platform-Application-Development PassXYZ.Vault2
```

# Understanding Razor components

Even though we have created and used Razor components in the last two chapters, we haven’t taken a
closer look at Razor components yet. In this section, we will continue improving the app from the last
chapter and dig deeper into Razor components to learn some key concepts about these components.
Blazor apps are built using Razor components. The first Razor component in our app is Main and it
is defined in Main.razor, as shown here:

```
<Router AppAssembly="@typeof(Main).Assembly">
 <Found Context="routeData">
 <RouteView RouteData="@routeData"
 DefaultLayout="@typeof(MainLayout)" />
<FocusOnNavigate RouteData="@routeData"
 Selector="h1" />
 </Found>
 <NotFound>
 <LayoutView Layout="@typeof(MainLayout)">
 <p role="alert">
 Sorry, there's nothing at this address.
 </p>
 </LayoutView>
 </NotFound>
</Router>
```
The Router component is installed in the Main component, and it handles the routing of pages
and selects the default layout component. All other Razor pages are loaded by Router components.
The Razor pages loaded by Router have route templates defined and are used to present the UI to
the users. In our project, Razor pages are located in the Pages folder. There are also reusable Razor
components, and they are the building blocks of Razor pages. These Razor components are in the
Shared folder.

Basically, each file with the .razor file extension is a Razor component and it is compiled into a
C# class when it is executed. The class name is the filename. The folder name is used as part of the
namespace. For example, the Login Razor component is in the Pages folder so the folder name,
Pages, is used as part of the namespace. So, the full name of the Login class is PassXYZ.Vault.
Pages.Login.
We use Pascal case for the class name in C#, so the folder name and Razor filename should use Pascal
case as well.

What are Pascal case, camel case, and snake case?
Pascal case, camel case, and snake case are commonly used naming conventions in programming
languages. Here are some examples:

* Camel case uses uppercase and lowercase in the variable name. The first letter is lowercase,
such as loginUser.

* Pascal case also uses uppercase and lowercase in the variable name, but the first letter is
uppercase, such as LoginUser.

* Snake case uses lowercase only and separates each word with an underscore, such as login_
user.

Razor components can be authored in a single file or they can be split into a Razor file (.razor) and
a code-behind C# file (.cs). In the code-behind C# file, a partial class is defined to contain all the
programming logic. We did this when we created the Login component in Chapter 7, Introducing
Blazor Hybrid App Development.

![image](https://user-images.githubusercontent.com/26972859/231969636-33f60dcd-f112-4f4f-bd88-77525f5cd7d2.png)

Figure 9.1: Razor component naming convention

When we created the Login component, we used Bootstrap CSS style for the styling. Razor components
can support CSS isolation, which can simplify CSS and avoid collisions with other components or
libraries. Additionally, it can include its own CSS style in a .razor.css file.

# Inheritance

Since a Razor component is a C# class, it includes all features of a C# class. A Razor component can
be a child class of another Razor component. In Chapter 8, Understanding Blazor Layout and Routing,
when we created layout components, we could see that all layout components are derived classes of
LayoutComponentBase. As we can see in MainLayout.razor in the following code, we use
the @inherits directive to specify the LayoutComponentBase base class:

```
@inherits LayoutComponentBase
<div class="page">
 <div class="sidebar"><NavMenu/></div>
 <main>@Body</main>
</div>
```
All Razor components derive from the ComponentBase class, so it is possible to create a Razor
component derived from the ComponentBase class using the C# file without the Razor markup
file. For example, we can create a Razor component called AppName in a C# class, as shown here:

```Csharp
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
namespace PassXYZ.Vault.Pages;
public class AppName : ComponentBase
{
 protected override void BuildRenderTree
 (RenderTreeBuilder builder)
 {
 base.BuildRenderTree(builder);
 builder.OpenElement(0, "div");
 builder.AddContent(1, "PassXYZ.Vault");
 builder.CloseElement();
 }
}
```
AppName is a Razor component created without a Razor markup file (.razor), but it is the same
as other Razor components, as shown here:

```
...
<AppName/>
...
```
We introduced Razor components in this section. We will learn how to package Razor components
in a library in the next section.

# Creating a Razor class library

In our project, we create reusable components in the Shared folder. These components can be reused
by other components, such as layout components or NavMenu.

We can also encapsulate Razor components in a separate library in the form of the Razor class library.
The components in the Razor class library are not project-specific, so they can be used in any Blazor
project. We can use them in Blazor Hybrid, Blazor WebAssembly, or Blazor Server apps.

In this book, we build Razor components using Bootstrap. There are many open source Razor class
libraries built on top of Bootstrap in GitHub. Some of them are good enough for commercial product
development. Here are some examples:

* BootstrapBlazor – https://github.com/dotnetcore/BootstrapBlazor
* Blazorise – https://github.com/Megabit/Blazorise
* Havit.Blazor – https://github.com/havit/Havit.Blazor/

These open source projects are built as Razor class libraries so that they can be reused similarly to
other .NET libraries. Razor class libraries can be published as NuGet packages so we can import them
into our Blazor projects.

In this section, we will create a Razor class library similar to the previously mentioned open source
projects. We will put Razor components that can be reused in our Razor class library. This library can
be published as a NuGet package.

We can create a Razor class library using Visual Studio or the dotnet command line.
To create a Razor class library using Visual Studio, we can add a new project to our solution, as shown
in Figure 9.2, by following these steps:

1. Search for and select Razor Class Library from the project templates.
2. Click Next and name the project PassXYZ.BlazorUI.
3. On the next screen, click Create to create it.

![image](https://user-images.githubusercontent.com/26972859/231970087-dde84124-64d7-440d-947d-244ea8faba1e.png)

Figure 9.2: Creating a Razor class library

To create the project using a dotnet command line, we can change the directory to the solution folder
and execute the following command in Command Prompt:

```dotnet new razorclasslib -n PassXYZ.BlazorUI```

The dotnet new command will create a new project using the razorclasslib template
and name the project PassXYZ.BlazorUI. To add the project to the solution, we can use the
following command:

```dotnet sln add PassXYZ.BlazorUI\PassXYZ.BlazorUI.csproj```

We need to remove the unused Component1.* and ExampleJsInterop.cs files from the
PassXYZ.BlazorUI project.

To use Razor components in our project, we need to add the project reference into the PassXYZ.
Vault project. We can add it in Visual Studio by right-clicking the project node and selecting Add
-> Project Reference. We can also edit the PassXYZ.Vault.csproj project file to add the
following line:

```xml
<ItemGroup>
 <ProjectReference
 Include="..\PassXYZ.BlazorUI\PassXYZ.BlazorUI.csproj"
 />
</ItemGroup>
```
To use this library, we need to update the PassXYZ.Vault\_Imports.razor file to add the
following line:

```@using PassXYZ.BlazorUI```

# Using static assets in the Razor class library

We use Bootstrap in our Razor components, so we need to include Bootstrap CSS and JavaScript files
in the Razor class library. From the Blazor app point of view, we can put these static assets in either
the project’s wwwroot folder or the component library’s wwwroot folder. Using the Bootstrap CSS
file as an example, if we put it in the wwwroot project, we can refer to it in index.html with the
following path:

```<script src="css/bootstrap/bootstrap.bundle.min.js"/>```

If we choose to put it in the component library’s wwwroot folder, we can refer to it with the following path:

```<script src="_content/PassXYZ.BlazorUI/css/bootstrap/bootstrap.bundle.min.js"/>```

The difference is that we need to refer to the URL in the component library starting with _content/{LibraryProjectName}.

After we have created a Razor class library project, we can add more components to it.

# Creating reusable Razor components

In this section, we can create reusable components by optimizing our code. Throughout the process, we
can get a better understanding of the features of Razor components and how to make them reusable.
We created the Blazor Hybrid version of our app, PassXYZ.Vault in Chapter 7, Introducing Blazor Hybrid
App Development, and we added layout and routing functionalities to it in Chapter 8, Understanding

the Blazor Layout and Routing. Our app can browse and update the password database now. So far, we
haven’t implemented most of the CRUD operations. We will add these functionalities after we refine
our Razor components in this chapter.

To navigate the password database, we created two Razor components – Items and ItemDetail
– in Chapter 7, Introducing .NET MAUI Blazor. The Items class is used to display a list of password
entries and groups in the current group, and the ItemDetail class is used to display the content
of a password entry.

If we look at the layout of Items and ItemDetail as shown in Figure 9.3, the look and feel of
both pages are quite similar:

![image](https://user-images.githubusercontent.com/26972859/231970589-85462e26-5bd1-4c75-b7da-f8717e51e4ff.png)

Figure 9.3: UI layout of Items and ItemDetail

The layout of both pages includes a sidebar, a header, and a list view. The sidebar is defined in the
layout component. The header and list view are implemented in both Items and ItemDetail
with partially duplicated code there. We will optimize our code and abstract the duplicated code into
reusable components in this chapter and the next chapter.

There are two buttons, Add and Back, in the header. The Back button can be used to navigate back
to the parent group, and the Add button can be used to add a new item or field.

In the list view item, we can use the context menu to perform item-level operations, such as edit or
delete. The context menu includes menu items to perform specific actions related to the selected
item or field. For the edit or delete CRUD operations, after the menu item is selected, a modal dialog
related to the action is displayed.

In the current implementation, both Items and ItemDetail include all UI elements in one
Razor markup. We will start to refine the code into smaller reusable components to make our
implementation clean.

We will convert modal dialogs into Razor components in this chapter and convert the header and
list view into Razor components in the next chapter. Let us start with modal dialogs. To support add,
edit, and delete operations, we need two kinds of dialog boxes:

* Editor dialog – adding or editing items or fields

* Confirmation dialog – to confirm before deleting an item or a field

In Chapter 8, Understanding the Blazor Layout and Routing, we used the HTML and CSS code from
Bootstrap examples to implement modal dialogs. We haven’t investigated them in detail yet, since
our markup files look long and complex. We will analyze the code and turn it into Razor components
in this chapter.

# Creating a base modal dialog component

To refine Editor and Confirmation dialogs, we can build a base modal dialog first. Using this base
modal dialog, we can create either Editor or Confirmation dialogs.
To create a new Razor component in the PassXYZ.BlazorUI project, we can right-mouse-click
on the project node and select Add -> New Item… -> Razor Component in the project template. We
name the Razor component as ModalDialog and create a C# code-behind file for it. After that, we
type the code in Listing 9.1 to ModalDialog.razor and Listing 9.2 to ModalDialog.razor.cs.
The UI code is extracted from the Items or ItemDetail code in Chapter 8, Understanding the
Blazor Layout and Routing, as shown in Listing 9.1:

**Listing 9.1: ModalDialog.razor (https://epa.ms/ModalDialog9-1)**

```
<div class="modal fade" id=@Id tabindex="-1"
 aria-labelledby="ModelLabel" aria-hidden="true">
 <div class="modal-dialog"><div class="modal-content">
 <div class="modal-header"> ❶
 <h5 class="modal-title" id="ModelLabel">@Title</h5> ❷
 <button type="button" class="btn-close" ❸
 data-bs-dismiss="modal" aria-label="Close"/>
</div>
 <div class="modal-body"> ❹
 <form class="row gx-2 gy-3">
 @ChildContent ❺
 <div class="col-12">
 <button type="button" class="btn btn-secondary"
 data-bs-dismiss="modal" @onclick=
 "OnClickClose">
 @CloseButtonText ❻
 </button>
 <button type="submit" class="btn btn-primary"
 data-bs-dismiss="modal" @onclick=
 "OnClickSave">
 @SaveButtonText ❼
 </button>
 </div>
 </form>
 </div>
 </div></div>
</div>
```
From the markup code in Listing 9.1, we can see that this is a typical HTML code snippet in Bootstrap
style. We embedded C# variables in HTML to create the component UI.
This base dialog UI includes a header ❶ and body ❹. There is a title ❷ and a close button ❸ in the
header. Inside the body, we can see a child content area ❺ and two buttons (Close ❻/Save ❼). We
can refer to Figure 9.4 to see the layout of this base modal dialog

![image](https://user-images.githubusercontent.com/26972859/231970938-5e06e4d7-5dcf-4883-8ede-89b2fb5076df.png)

Figure 9.4: Base dialog

Even though the HTML and CSS code is very similar to the Bootstrap example, we replaced all the
hardcode content with C# variables. If we use this modal dialog component to build a new component,
the following is an example:

```xml
<ModalDialog Id=@id Title="Please confirm" OnSaveAsync=
 @OnDelete
 SaveButtonText="Save" CloseButtonText="Close">
 Do you want to delete UserName?
</ModalDialog>
<button class="dropdown-item" data-bs-toggle="modal"
 data-bs-target="#@Id">Please confirm</button>
```
In the preceding markup code, we define the modal dialog using the <ModalDialog> component
tag. Each modal dialog has a unique ID to identify it. We can show the dialog box after clicking a
button. In the button, we provide the modal dialog ID to identify it.
Inside the <ModalDialog> component tag, we assigned the value of multiple attributes defined
in the ModalDialog component, such as the ID, title, text of buttons, event handler, and so on.

# Data binding

Instead of assigning a string or data directly to the attribute of an HTML element, we can assign a
variable to it. This is the data binding feature of the Razor component. We will learn how to use data
binding in this section.
In data binding, when we assign a variable to the attribute of the DOM element, the data flows from
Razor components to DOM elements. When we respond to the DOM event, the data flows from DOM
elements to Razor components. Since we can use a Razor component just like a DOM element, the
data flow between child and parent Razor components is similar to the data exchange between Razor
components and DOM elements.
For example, we can bind the id variable to the Id attribute of ModalDialog and we can handle
the button click event using the OnDelete event handler:

```
<ModalDialog Id=@id Title="Please confirm" OnSaveAsync=@OnDeleteSaveButtonText="Save" CloseButtonText="Close">
```
In the preceding example, the data flows from the id variable to the Id attribute of ModalDialog.
When the OnDelete event handler is invoked, the data flows from ModalDialog back to the
current context. The ModalDialog attributes, Id and OnSaveAsync, are defined in the C# codebehind file. Let’s review the C# code-behind file of ModalDialog in the next section.

# Component parameters
  
We can define the attributes of Razor components using component parameters. To define component
parameters, we must create public properties with the [Parameter] attribute.
In the ModalDialog class, as shown in Listing 9.2, we declare seven component parameters, Id, Title,
ChildContent, OnClose, OnSaveAsync, CloseButtonText, and SaveButtonText.
We can use these component parameters in data binding:

**Listing 9.2: ModalDialog.razor.cs (https://epa.ms/ModalDialog9-2)**

```Csharp
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
namespace PassXYZ.BlazorUI;
public partial class ModalDialog : IDisposable
{
 [Parameter]
 public string? Id { get; set; } ❶
 [Parameter]
 public string? Title { get; set; } ❷
 [Parameter]
 public RenderFragment ChildContent { get; set; } ❸
 [Parameter]
 public Func<Task>? OnClose { get; set; } ❹
 [Parameter]
 public Func<Task<bool>>? OnSaveAsync { get; set; } ❺
 [Parameter]
 [NotNull]
 public string? CloseButtonText { get; set; } ❻
 [Parameter]
 [NotNull]
 public string? SaveButtonText { get; set; } ❼
 private async Task OnClickClose() {
 if (OnClose != null) { await OnClose(); }
 }
 private async Task OnClickSave() {
 if (OnSaveAsync != null) { await OnSaveAsync(); }
 }
 void IDisposable.Dispose() {
 GC.SuppressFinalize(this);
 }
}
```
The component parameters of ModalDialog are defined as follows:

  * Id ❶ – This is used to identify a modal dialog

  * Title ❷ – This is the title of the modal dialog

  * ChildContent ❸ – This is where the content of the child component should be inserted

Two event handlers – OnClose ❹ and OnSaveAsync ❺ – are defined to handle button click actions.
We can customize the text of both buttons using CloseButtonText ❻ and SaveButtonText ❼.
We can treat component parameters just like HTML attributes. We can assign a C# field, property, or
return value of a method to the component parameter of ModalDialog.
After we create the ModalDialog base component, we can create Editor and Confirmation dialog
components using it.

Let’s create a new modal dialog, ConfirmDialog, to ask for the confirmation of deleting an item.
To create a new ConfirmDialog componwent in the PassXYZ.BlazorUI project, we can
right-mouse-click on the project node and select Add -> New Item… -> Razor Component in the
project template. We can name the Razor component ConfirmDialog and type the following code
as shown in Listing 9.3:

**Listing 9.3: ConfirmDialog.razor (https://epa.ms/ConfirmDialog9-3)**

```xml
<ModalDialog Id=@Id Title=@($w"Deleting {Title}") OnSaveAsync=
@OnSavew
 SaveButtonText="Confirm" CloseButtonText="Cancel">
 Please confirm to delete @Title?
</ModalDialog>
@code {w
 [Parameter]
 public string Id { get; set; } = "confirmDialog"w; ①
 [Parameter]
public string? Title { get; set; } ②
 [Parameter]
 public Action? OnConfirmClick { get; set; }
 async Task<bool> OnSave() {
 OnConfirmClick?.Invoke();
 return true;
 }
}
```

We define the Id ① and Title ② component parameters in ConfirmDialog and pass their
values to the base class through data binding. We also subscribe to the OnSaveAsync event using the
OnSave event handler. We also define our own event handler, OnConfirmClick, as a component
parameter to which other components can subscribe.
  
In ConfirmDialog, we actually bind parameters through nested components. In this case, the data
should flow in the directions suggested here:

  * Change notifications flow up the hierarchy

  * New parameter values flow down the hierarchy

  The values of the Id and Title attributes are assigned by the components that use ConfirmDialog,
and their values flow down to ModalDialog. The Save or Close button events are triggered in
the ModalDialog component, and they flow up the chain to ConfirmDialog and upper-level
components. If we use the Save button as an example, the event flows up in the direction shown here:
onclick (DOM) ->se OnSaveAsync (ModalDialog) -> OnConfirmClick
(ConfirmDialog)

  It starts from the onclick event in DOM. ModalDialog defines its own event, OnSaveAsync,
which is triggered by the onclick event handler. ConfirmDialog defines its own event,
OnConfirmClick, which is triggered by the OnSaveAsync event handler.

# Nested components

ConfirmDialog is one of the examples of nested components. As we can see, we can embed
components inside components by declaring them using HTML syntax. The embedded components
look like HTML tags, where the name of the tag is the component type. For example, we can use
ModalDialog inside ConfirmDialog, as shown here:

```xml
<ModalDialog ...>Please confirm to delete @Title?</ModalDialog>  
```

Nested components are the way to build the component hierarchy in Blazor. Inheritance and composition
are the two ways that we can extend and reuse a class in an object-oriented programming language. In
Blazor, composition is used in nested components to extend the functionalities. Inheritance is an is-a
relationship, while composition is a has-a relationship. In nested components, the parent component
has a child component in it.
  
In Microsoft Blazor and ASP.NET Core documents, the terms ancestor and descendant or parent
and child are used to explain the relationship of nested components. Here, parent and child is not an
inheritance relationship, but a composition relationship. A better term could be an outer component
or an inner component. Nevertheless, to be consistent with the Microsoft documentation, I won’t
choose a different term in the discussion. Please just be aware that when we discuss nested components
and data binding, the ancestor and descendant relationship is a has-a relationship or composition.
In our previous example, the ConfirmDialog component is the outer component, while ModalDialog
is the inner component. The relationship is that ConfirmDialog, has ModalDialog in it.

# Child content rendering

When we build nested components, there are many cases in which one component can set the content
of another component. The outer component provides the content between the inner component’s
opening and closing tags. In ConfirmDialog, it sets the content of ModalDialog as follows:

```xml
<ModalDialog Id=@Id Title=@($"Deleting {Title}")
 OnSaveAsync=@OnSave
 SaveButtonText="Confirm" CloseButtonText="Cancel">
 Please confirm to delete @Title?
</ModalDialog>  
```
This is done by using a special component parameter called ChildContent, which is of the
RenderFragment type. In the preceding code, the Please confirm to delete @Title?
string is set to the ChildContent parameter of ModalDialog.
ConfirmDialog is still a relatively simple example of nested components. Let’s look at another
example, EditorDialog, to explore more Razor component features. As we mentioned earlier, we
need two dialog boxes to handle add, edit, and delete actions. ConfirmDialog is used to confirm
with users before deleting an item or a field. To add or edit an item or a field, we need a dialog box
that can provide editing features.
We can do the same to create the new component, EditorDialog. After selecting Add -> New Item…
-> Razor Component in the project template, we can name the Razor component EditorDialog
and create a C# code-behind file for it. After that, we type the code in Listing 9.4 to EditorDialog.
razor and Listing 9.5 to EditorDialog.razor.cs.

Let’s review the Razor markup code of EditorDialog as shown in Listing 9.4:

**Listing 9.4: EditorDialog.razor (https://epa.ms/EditorDialog9-4)**

```xml
<ModalDialog Id=@Id Title=@Key OnSaveAsync=@OnSaveClicked
 SaveButtonText ="Save" CloseButtonText="Close">
 @if (IsKeyEditingEnable) { ❶
 <input type="text" class="form-control" id="keyField"
 @bind="Key" placeholder=@KeyPlaceHolder required> ❷
 }
 @ChildContent
 <div>
 <textarea class="form-control" id="valueField"
 style="height: 100px"
 placeholder=@ValuePlaceHolder
 @bind="Value" required /> ❸
 </div>
</ModalDialog>
```
EditorDialog is built using ModalDialog. It can be used to edit a key-value pair. When we
create a new key-value pair, we want to edit both the key and the value. When we edit an existing
key-value pair, we may want to make a change to the value field only. These are the two use cases that
we want to support in EditorDialog. The condition is detected using a component parameter
called IsKeyEditingEnable ❶. The key portion of the UI is rendered as an <input> ❷
element when we want to create a new key-value pair. When we edit an existing key-value pair, the
key is displayed as the title in the header area, and we edit the value in the <textarea> ❸ element.
This is the main functionality of our EditorDialog component.
We can see the UI in Figure 9.5. On the left-hand side, it shows the dialog when we want to add a new
field. We need to provide the field name and content. On the right-hand side, it shows the dialog when
we want to edit an existing URL field. The field name is displayed in the title, and we can change the
content in <textarea>:

![image](https://user-images.githubusercontent.com/26972859/231972261-4346f582-03ed-4816-9e4a-e1da0d9eaedb.png)

Figure 9.5: Editing a field
  
In the EditorDialog component, when we edit the key and value using the <input> and
<textarea> HTML elements, the initial value is displayed. The initial value sets from the Razor
component to the DOM. After we make the changes, the data flows from the DOM to the Razor
component. This is two-way data binding

# Two-way data binding
  
Two-way data binding can be created with the @bind Razor directive attribute. With this syntax, an
HTML element attribute can bind to a field, property, expression value, or result of a method. In Listing
9.4, the <input> element value binds to the Key property in the EditorDialog component:

```
<input type="text" class="form-control" id="keyField"
 @bind="Key" placeholder=@KeyPlaceHolder required>
```
With two-way data binding, the DOM element <input> value is updated whenever the Key property
is changed. The Key property is updated as well when the user updates the <input> value in the DOM.
In the preceding example, instead of using the @bind directive attribute, we can replace the @bind
directive attribute with two one-way data bindings, as you can see in the following code:

```
<input type="text" class="form-control" id="keyField"
 value="@Key"
 @onchange="@((ChangeEventArgs e) => Key = e?.Value?
 .ToString())"
 placeholder=@KeyPlaceHolder required>
```
When our EditorDialog component is rendered, the value of the <input> element comes from
the Key property. When the user enters a value in the textbox and changes the element focus, the
onchange event is fired and the Key property is set to the changed value.
For the <input> element, the default event of the @bind directive attribute is the onchange event.
We can change the event with an @bind:event="{event}" attribute. The {event} placeholder
should be a DOM event. For example, we can change the onchange event to an oninput event
with the following code snippet:

```
<input type="text" class="form-control" id="keyField"
 @bind="Key" @bind:event="oninput" placeholder=@KeyPlaceHolder
required>
```

## Binding with component parameters

In the previous section, we discussed two-way data binding between a Razor component and a DOM
element. Since the Razor component can be used in a similar way as the DOM element, we can create
a two-way data binding between two Razor components as well. This is usually the case when we need
to communicate between parent and child (inner or outer) components.
We can bind a component parameter of an inner component to the property of an outer component
with the @bind-{PROPERTY} syntax. The {PROPERTY} placeholder is the property to bind. We
explained that the @bind directive attribute can be replaced by two one-way data binding setups, 
which include assigning a variable to the <input> value attribute and assigning an event handler to
the onchange event. The event handler can be added automatically for @bind by the compiler, but
not for @bind-{PROPERTY}. We need to define our own event of the EventCallback<TValue>
type to bind with component parameters. The event name must be {PARAMETER NAME}Changed.
Let’s use our EditorDialog component to explain how to use the @bind-{PROPERTY}
directive attribute.
In our code, we edit a field using EditorDialog in the ItemDetail component or edit an item
using the same in the Items component. Let’s use field editing as an example:

```xml
<EditorDialog Id=@_dialogEditId
 @bind-Key="listGroupField.Key" ❶
 @bind-Value="listGroupField.Value" ❷
 IsKeyEditingEnable=@_isNewField OnSave="UpdateFieldAsync"
 KeyPlaceHolder="Field name" ValuePlaceHolder="Field
 content">
 @if (_isNewField) {
 <div class="form-check">
 <input class="form-check-input" type="checkbox"
 @bind="listGroupField.IsProtected"
 id="flexCheckDefault">
 <label class="form-check-label"
 for="flexCheckDefault">
 Password
 </label>
 </div>
 }
</EditorDialog>
```
In the preceding code of the ItemDetail component, we can create data binding of Key ❶ and
Value ❷ to the listGroupField of the Field type. We need to implement the {PARAMETER
NAME}Changed events in C# code-behind of EditorDialog, as shown here in Listing 9.5:

**Listing 9.5: EditorDialog.razor.cs (https://epa.ms/EditorDialog9-5)**

```Csharp
namespace PassXYZ.BlazorUI;
public partial class EditorDialog {
 [Parameter]
public string? Id { get; set; }
 bool _isKeyEditingEnable = false;
 [Parameter]
 public bool IsKeyEditingEnable ...
 [Parameter]
 public EventCallback<bool>? IsKeyEditingEnableChanged {
 get; set; }
 string _key = string.Empty;
 [Parameter]
 public string Key { ❶
 get => _key;
 set {
 if(_key != value) {
 _key = value;
 KeyChanged?.InvokeAsync(_key); ❸
 }
 }
 }
 [Parameter]
 public EventCallback<string>? KeyChanged { get; set; } ❷
 [Parameter]
 public string? KeyPlaceHolder { get; set; }
 string _value = string.Empty;
 [Parameter]
 public string Value ...
 [Parameter]
 public EventCallback<string>? ValueChanged { get; set; }
 [Parameter]
 public string? ValuePlaceHolder { get; set; }
 [Parameter]
 public RenderFragment ChildContent { get; set; } =
 default!;
 [Parameter]
 public Action<string, string>? OnSave { get; set; }
 async Task<bool> OnSaveClicked() {
 OnSave?.Invoke(Key, Value);
 return true;
 }
}
```
In Listing 9.5, we use the Key property as an example to explain the process of component parameter
binding. The Key property is defined as a component parameter with the [Parameter] attribute.
An associated event is defined as KeyChanged of the EventCallback<TValue> type. When the
user changes the text input and changes the element focus, the setter of the Key property is invoked.
Inside the setter of the Key property, it fires the KeyChanged event, which will inform the outer
ItemDetail component. As a result, the listGroupField.Key linked variable is updated.

# Communicating with cascading values and parameters
  
We can use data binding to pass data between parent and child components. Data binding is good to
pass data to the intermediate child component. Sometimes, we may want to pass data to components
several levels deep. If we use data binding in this situation, then we have to create multiple levels of
chained data binding. The complexity increases with the chained levels. For example, if we want to pass
data from Items to ModalDialog, we have to create a data binding to ConfirmDialog first. Then,
another level of data binding needs to be created between ConfirmDialog and ModalDialog.
In Items, we need to pass the Id dialog to ModalDialog. We need to use an Id dialog to identify the
dialog instance that we want to display. As we can see next, we define ConfirmDialog in the Items
component. Id is defined in Items and passes to ConfirmDialog using the component parameter:

```xml
<ConfirmDialog Id="@_dialogDeleteId" Title=
 @listGroupItem.Name
 OnConfirmClick="DeleteItemAsync" />  
```
Then, ConfirmDialog has to pass it to ModalDialog:

```xml
<ModalDialog Id=@Id Title=@($"Deleting {Title}")
 OnSaveAsync=@OnSave
 SaveButtonText="Confirm" CloseButtonText="Cancel">
 Please confirm to delete @Title?
</ModalDialog>
```
In ModalDialog, Id is used as an attribute of the <div> element:

```xml
<div class="modal fade" id=@Id tabindex="-1"
 aria-labelledby="ModelLabel" aria-hidden="true"> ...
```
To avoid multiple levels of data binding, we can use cascading values and parameters as a method to
flow data down a component hierarchy.

CascadingValue is a component of the Blazor framework. The outer component provides
a cascading value using CascadingValue, and the inner component can receive it using the
[CascadingParameter] attribute. To demonstrate the usage, we can change the code of the
Items component as follows:

```xml
<CascadingValue Value="@_dialogDeleteId" Name="Id">
 <ConfirmDialog Title=@listGroupItem.Name
 OnConfirmClick="DeleteItemAsync" />
</CascadingValue>
```
We use cascading value with the <CascadingValue> tag. In the <CascadingValue> tag, we
pass the_dialogDeleteId variable to the Value attribute and the "Id" string to the Name
attribute. Since this Id is not used by ConfirmDialog directly, the Id component parameter can
be removed from ConfirmDialog.
In ModalDialog, we change the Id property from a component parameter to a parameter using
the [CascadingParameter] attribute:

```
[CascadingParameter(Name = "Id")]
public string Id { get; set; } = default!;
```
If we have only one cascading value, we don’t have to specify the cascading value name. The compiler
can help us to find it by data type. However, to avoid ambiguities, we can name the cascading value
using the Name attribute. Let’s look at the final changes in the Items component using the cascading
value for both ConfirmDialog and EditorDialog:

```xml
<CascadingValue Value="@_dialogEditId" Name="Id">
 <EditorDialog @bind-Key="listGroupItem.Name"
 @bind-Value="listGroupItem.Notes"
 IsKeyEditingEnable=true
 OnSave="UpdateItemAsync" KeyPlaceHolder="Item name"
 ValuePlaceHolder="Please provide a description">
 @if (_isNewItem) {
 <select @bind="newItem.SubType" class="form-select"
 aria-label="Group">
 <option selected value=@ItemSubType.Group>
 @ItemSubType.Group</option>
 <option value=@ItemSubType.Entry>
 @ItemSubType.Entry</option>
 <option value=@ItemSubType.PxEntry>
 @ItemSubType.PxEntry</option>
 <option value=@ItemSubType.Notes>
 @ItemSubType.Notes</option>
 </select>
 }
 </EditorDialog>
</CascadingValue>
<CascadingValue Value="@_dialogDeleteId" Name="Id">
 <ConfirmDialog Title=@listGroupItem.Name
 OnConfirmClick="DeleteItemAsync" />
</CascadingValue>
```
As we can see, after we use a cascading value, ConfirmDialog and EditorDialog don’t need
to handle the Id field directly. The code is more concise than the previous version.
In this section, we discussed how to create reusable components. Some Razor components may have
dependencies on data or network services. We need to take extra actions during the creation or the
destruction of the components. We can do these as part of the life cycle management of Razor components.
Let us review the life cycle of Razor components in the next section.
  
# Understanding the component lifecycle

A Razor component has a lifecycle just like any other object. There is a set of synchronous and
asynchronous lifecycle methods that can be overridden to help developers perform additional operations
during component initialization and rendering.

We can review the Razor component lifecycle in Figure 9.6:

![image](https://user-images.githubusercontent.com/26972859/231973462-56886bf8-f528-47f9-ade5-7a4391694eea.png)

Figure 9.6: Razor component lifecycle

In Figure 9.6, we can see that we can add hooks during the initialization and rendering phases. The
following methods can be overridden to catch initialization events:

* SetParametersAsync
* OnInitialized and OnInitializedAsync
* OnParametersSet and OnParametersSetAsync

SetParametersAsync and OnInitialized(Async) are invoked only in the first render.
OnParametersSet(Async) is called every time a parameter is changed.
The following methods can be overridden to customize rendering:
  
* ShouldRender
* OnAfterRender and OnAfterRenderAsync

We will review these lifecycle methods in detail and show how we use them in our code.

# SetParametersAsync

SetParametersAsync is the first hook after the object is created and it has the following signature:

```
public override Task SetParametersAsync(ParameterView parameters)
```
The ParameterView parameter contains component parameters or cascading parameter
values. SetParametersAsync sets the value of each property with the [Parameter] or
[CascadingParameter] attribute. This function can be overridden to add logic that needs
to be executed before the parameters are set. The next hook after SetParametersAsync
is OnInitialized{Async}.

# OnInitialized and OnInitializedAsync

OnInitialized and OnInitializedAsync are invoked when the component is initialized.
They have the following signatures, respectively:

```
protected override void OnInitialized()
protected override async Task OnInitializedAsync()
```
By overriding these two functions, we can add logic to initialize our component here. Please be
aware that they are only called once, right after the creation of the component. For time-consuming
initialization tasks, the asynchronous method can be used, such as downloading data using RESTful
API calls. As we can see in Figure 9.6, after an asynchronous method is completed, the DOM needs
to be rendered again.

OnParametersSet and OnParametersSetAsync

When component parameters are set or changed, OnParametersSet and OnParametersSetAsync
are invoked. We can see that there are two versions to handle both synchronous and asynchronous
cases. The asynchronous version of OnParametersSetAsync can be used to handle timeconsuming tasks. Once the asynchronous task is completed, the DOM needs to be rendered again
to reflect any changes.
  
The methods have the following signatures, respectively:

```
protected override void OnParametersSet()
protected override async Task OnParametersSetAsync()
```

These two methods will be invoked whenever component parameters or cascading parameters are
changed. They can be called multiple times, while OnInitialized{Async} is only called once.
As we can see in Figure 9.6, the DOM can be rendered multiple times during the initialization phase
due to which asynchronous calls may be invoked. The methods involved in the rendering process are
ShouldRender and OnAfterRender{Async}.

# ShouldRender

The ShouldRender method returns a Boolean value, indicating whether the component should
be rendered. As we can see in Figure 9.6, the first render ignores this method, so a component should
be rendered at least once. This method has the following signature:

```
protected override bool ShouldRender()
```

# OnAfterRender and OnAfterRenderAsync

OnAfterRender and OnAfterRenderAsync are called after a component has finished rendering.
They have the following signatures, respectively:

```Csharp
protected override void OnAfterRender(bool firstRender)
protected override async Task OnAfterRenderAsync(bool firstRender)
```
They can be used to perform additional initialization tasks with the rendered content, such as invoking
JavaScript code in the component. This method has a Boolean firstRender parameter, which allows
us to attach JavaScript event handlers only once. There is an asynchronous version of this method,
but the framework won’t schedule a further render cycle after the asynchronous task is completed.
To have a look at the effect of lifecycle methods, we can run a test to add all lifecycle methods in the
ConfirmDialog component, as you can see here:

```
public ConfirmDialog()
{
 Debug.WriteLine($"ConfirmDialog-{Id}: is created");
}
public override Task SetParametersAsync
 (ParameterView parameters)
{
 Debug.WriteLine($"ConfirmDialog-{Id}:
 SetParametersAsync called");
 return base.SetParametersAsync(parameters);
}
protected override void OnInitialized()
 => Debug.WriteLine($"ConfirmDialog-{Id}: OnInitialized
 called - {Title}");
protected override async Task OnInitializedAsync() =>
 await Task.Run(() => {
 Debug.WriteLine($"ConfirmDialog-{Id}: OnInitializedAsync
 called - {Title}");
});
protected override void OnParametersSet()
 => Debug.WriteLine($"ConfirmDialog-{Id}: OnParametersSet
 called - {Title}");
protected override async Task OnParametersSetAsync() =>
 await Task.Run(() => {
Debug.WriteLine($"ConfirmDialog-{Id}:
 OnParametersSetAsync called - {Title}");
});
protected override void OnAfterRender(bool firstRender)
 => Debug.WriteLine($"ConfirmDialog-{Id}: OnAfterRender
 called with firstRender = {firstRender}");
protected override async Task OnAfterRenderAsync(bool
 firstRender) => await Task.Run(() => {
 Debug.WriteLine($"ConfirmDialog-{Id}:
 OnAfterRenderAsync called - {Title}");
});
protected override bool ShouldRender() {
 Debug.WriteLine($"ConfirmDialog-{Id}: ShouldRender called
 - {Title}");
 return true;
}
```
We override all lifecycle methods in ConfirmDialog and add debug output to show the progress.
After we launch our app, we can see the following output:

```
ConfirmDialog-: is created
ConfirmDialog-: SetParametersAsync called
ConfirmDialog-deleteModel: OnInitialized called -
ConfirmDialog-deleteModel: OnInitializedAsync called -
ConfirmDialog-deleteModel: OnParametersSet called -
ConfirmDialog-deleteModel: OnParametersSetAsync called -
ConfirmDialog-deleteModel: ShouldRender called -
ConfirmDialog-deleteModel: ShouldRender called -
ConfirmDialog-deleteModel: OnAfterRender called with
 firstRender = True
ConfirmDialog-deleteModel: OnAfterRenderAsync called -
ConfirmDialog-deleteModel: OnAfterRender called with
 firstRender = False
ConfirmDialog-deleteModel: OnAfterRenderAsync called -
ConfirmDialog-deleteModel: OnAfterRender called with
 firstRender = False
ConfirmDialog-deleteModel: OnAfterRenderAsync called -
```
The preceding output is the one when we just launch our app and the Items page is shown. We can
see that the Id cascading parameter is not set before the SetParametersAsync method is called.
Since we override the asynchronous methods, there are multiple render cycles scheduled in parallel.
ShouldRender and OnAfterRender{Async} are invoked multiple times due to rendering
occurring in parallel.
Let’s look at another case when we click on the context menu on the Items page. When we click on
the context menu of an item, such as Google, ConfirmDialog is initialized again. The output is
as follows:

```
ConfirmDialog-deleteModel: SetParametersAsync called
ConfirmDialog-deleteModel: OnParametersSet called - Google
ConfirmDialog-deleteModel: ShouldRender called - Google
ConfirmDialog-deleteModel: OnParametersSetAsync called –
 Google
ConfirmDialog-deleteModel: ShouldRender called - Google
ConfirmDialog-deleteModel: OnAfterRender called with
 firstRender = False
ConfirmDialog-deleteModel: OnAfterRenderAsync called –
 Google
ConfirmDialog-deleteModel: OnAfterRender called with
 firstRender = False
ConfirmDialog-deleteModel: OnAfterRenderAsync called –
 Google
```
The SetParametersAsync method is called again since the Title component parameter is
changed. We can see that the Title component parameter is set to Google in the subsequent calls.

In our code, we use OnParametersSet to load the list of items in Items.razor.cs and load a
list of Field in ItemDetail.razor.cs. Let’s review OnParametersSet in ItemDetail.
razor.cs:

```
protected override void OnParametersSet() {
 base.OnParametersSet();
 if (SelectedItemId == null) { ❶
throw new InvalidOperationException(
 "ItemDetail: SelectedItemId is null");
 }
 selectedItem = DataStore.GetItem(SelectedItemId, true); ❷
 if (selectedItem == null) {
throw new InvalidOperationException(
 "ItemDetail: entry cannot be found with SelectedItemId");
 }
 else {
 if (selectedItem.IsGroup) {
 throw new InvalidOperationException(
 "ItemDetail: SelectedItemId should not be a group
 here.");
 }
 fields.Clear();
 List<Field> tmpFields = selectedItem.GetFields(); ❸
 foreach (Field field in tmpFields) {
 fields.Add(field);
 }
 notes = selectedItem.GetNotesInHtml();
 }
}
```
❶ In OnParametersSet, we check whether the SelectedItemId component parameter is
null. This is the ID of the selected item. ❷ If it is not null, we can find the item by calling the
IDataStore method called GetItem(). ❸ Once we get the instance of the selected item, we can
get a list of fields by calling the GetFields() method.

In Items.razor.cs, the implementation of OnParametersSet is very similar to this. You can
refer to the following GitHub link to find out the details:
https://epa.ms/Items9-6
So far, we have an almost full-function password manager app, and the UI of this app is built with
Blazor. We created reusable modal dialog components to support the context menu so we can perform
CRUD operations. The last piece of the puzzle is the implementation of CRUD operations.

# Implementing CRUD operations

Once we have prepared modal dialogs, which will be used in CRUD operations from previous sections,
we can implement CRUD operations in this section.

# CRUD operations of items

To add or update an item, we can use the UpdateItemAsync() method in Items.razor.cs
to handle both cases. To detect whether we want to create a new item or update an existing item, we
define a private _isNewItem field as follows:

```
bool _isNewItem = false;
```

Next, we’ll see how to add or edit an item.

# Adding a new item

To add a new item, we can click the + button in the header of the Items page, as shown in Figure 9.7:

![image](https://user-images.githubusercontent.com/26972859/231975375-81c64323-505d-4b88-8223-e8dd86d9cf53.png)

Figure 9.7: Adding a new item

The Razor markup of this page header can be reviewed here:

```
<div class="container"><div class="row">
 <div class="col-12"><h1>
 @if (selectedItem?.GetParentLink() != null) {
<a class="btn btn-outline-dark"
 href="@selectedItem?.GetParentLink()">
 <span class="oi oi-chevron-left"
 aria-hidden="true"></span></a> ❶
 }
 @(" " + Title) ❷
<button type="button"
 class="btn btn-outline-dark float-end"
 data-bs-toggle="modal"
 data-bs-target="#@_dialogEditId"
 @onclick="@(() => _isNewItem=true)">
 <span class="oi oi-plus" aria-hidden="true">
 </span></button> ❸
 </h1></div>
</div></div>
```
The page header displays the Back button ❶, Title ❷, and the Add button ❸. The Back button is
displayed if the parent link exists.

When the Add button is clicked, it will display a modal dialog with Id defined in the _dialogEditId
variable. The onclick event handler just sets _isNewItem to true so the modal dialog event
handler knows this is an action to add a new item.

# Editing or deleting an item

To edit or delete an item, we can click on the context menu on the item, as shown in Figure 9.8:

![image](https://user-images.githubusercontent.com/26972859/231975651-7e855be4-f06f-4722-ad99-fcb8361293bf.png)

Figure 9.8: Editing or deleting an item

After we click on the context menu button, a list of menu items will be displayed. Let’s review the
markup for the context menu in Items.razor as follows:

```
<div class="list-group">
 @foreach (var item in items) {
<div class="dropdown list-group-item list-group-item-action
 d-flex gap-1 py-2" aria-current="true">
 <img src="@item.GetIcon()" alt="twbs" width="32"
 height="32"
 class="rounded-circle flex-shrink-0 float-start">
 <a href="@item.GetActionLink()" class="..."> ...
 <button class="opacity-50 btn btn-light
 dropdown-toggle" type="button"
 id="itemsContextMenu"
 data-bs-toggle="dropdown" aria-expanded="false"
 @onclick="@(() => listGroupItem=item)"> ❶
 <span class="oi oi-menu" aria-hidden="true"></span>
 </button>
 <ul class="dropdown-menu" aria-labelledby=
 "itemsContextMenu">
 <li><button class="dropdown-item"
 data-bs-toggle="modal"
 data-bs-target="#@_dialogEditId"
 @onclick="@(() => _isNewItem=false)"> ❷
 Edit</button></li>
 <li><button class="dropdown-item"
 data-bs-toggle="modal"
 data-bs-target="#@_dialogDeleteId"> ❸
 Delete</button></li>
 </ul>
 </div>
 }
</div>  
```
There is a context menu button ❶ defined in the preceding markup code. When this button is clicked,
two menu items, Edit ❷ and Delete ❸, will be displayed. Since the markup code of the context
menu runs in a foreach loop, we need to get a reference of the selected item to edit or delete it. In
the logic of C# code-behind, the listGroupItem variable is used to refer to the selected item. We
can catch the reference in the onclick event handler of the context menu button.
When the Edit menu item is selected, we need to set the _isNewItem variable to false so the
event handler of the modal dialog can know we are editing an existing item.
With all the previous setup, let’s review the event handler in modal dialogs. Let’s review the
UpdateItemAsync() event handler in Items.razor.cs first:

```Csharp
private async void UpdateItemAsync(string key, string value) {
 if (listGroupItem == null) { return; }
 if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty
 (value))
 { return; }
 listGroupItem.Name = key;
 listGroupItem.Notes = value;
 if (_isNewItem) { ①
 // Add new item
 if (listGroupItem is NewItem aNewItem) {
 Item? newItem = DataStore.CreateNewItem
 (aNewItem.SubType);
 if (newItem != null) {
 newItem.Name = aNewItem.Name;
 newItem.Notes = aNewItem.Notes;
 items.Add(newItem);
 await DataStore.AddItemAsync(newItem);
 }
 }
 }
 else {
 // Update the current item
 await DataStore.UpdateItemAsync(listGroupItem);
 }
}
```

The UpdateItemAsync() event handler can handle both adding and editing an item. As we can
see, it checks the _isNewItem variable ① to detect whether we want to add or edit an item. After
that, it calls IDataStore methods to process add or update actions.
Next, let’s review the event handler of deleting an item:

```Csharp
private async void DeleteItemAsync() {
 if (listGroupItem == null) return;
 if (items.Remove(listGroupItem)) {
 _ = await DataStore.DeleteItemAsync
 (listGroupItem.Id);
 }
}
```
In the DeleteItemAsync()event handler, it just removes the item from the list and calls
IDataStore methods to process the delete action.

# CRUD operations of fields

The CRUD operations of fields are similar to what we have done for items. To add or update a field,
we can use the UpdateFieldAsync() method in ItemDetail.razor.cs to handle both
cases. To detect whether we want to create a new field or update an existing field, we define a private
_isNewField field as follows:

```Csharp
bool _isNewField = false;
```
The UI of CRUD operations is also similar to what we have explained in the previous section. Please
refer to Figure 9.9 to see the Add button and context menu items:

![image](https://user-images.githubusercontent.com/26972859/231976158-e0d39a4b-416a-4b19-b968-5a2d17828dac.png)

Figure 9.9: Add, edit, or delete a field

We can review the Razor markup code of the page header in IwwtemDetail.razor as follows:

```
<div class="container">
 <div class="row"><div class="col-12">
 <h1>
 @if (selectedItem?.GetParentLink() != null) {
 <a class="btn btn-outline-dark"
 href="@selectedItem?.GetParentLink()">
 <span class="oi oi-chevron-left"
 aria-hidden="true"></span></a>
 }
 @(" " + selectedItem!.Name)
 <button type="button" class="btn btn-outline-dark
 float-end"
 data-bs-toggle="modal" data-bs-
 target="#@_dialogEditId"
 @onclick="@(() => _isNewField=true)">
 <span class="oi oi-plus"
 aria-hidden="true"></span></button>
 </h1>
 </div></div>
</div>

```

As we can see, the preceding source code is also similar to the one in Items.razor except the
_isNewItem variable is replaced by _isNewField. We can refine this page header to a reusable
component later.
Just like in the previous section, let’s review the source code of the list group and context menu:

```
<div class="list-group">
 @foreach (var field in fields) {
 @if(field.ShowContextAction == null) {
 <div class="dropdown list-group-item ...
 aria-current="true">
 <span class="oi oi-pencil" aria-hidden="true">
 </span>
 <div class="d-flex gap-2 w-100
 justify-content-between"> ...
 <button class="opacity-50 btn btn-light
 dropdown-toggle" type="button"
 id="itemDetailContextMenu"
 data-bs-toggle="dropdown" aria-expanded="false"
 @onclick="@(() => listGroupField=field)"> ❶
 <span class="oi oi-menu" aria-hidden="true">
 </span>
 </button>
 <ul class="dropdown-menu"
 aria-labelledby="itemDetailContextMenu">
 <li><button class="dropdown-item"
 data-bs-toggle="modal"
 data-bs-target="#@_dialogEditId"
 @onclick="@(() => _isNewField=false)"> ❷
 Edit
 </button></li>
 <li><button class="dropdown-item"
 data-bs-toggle="modal"
 data-bs-target="#@_dialogDeleteId"> ❸
 Delete
 </button></li>
 @if (field.IsProtected) {
 <li><button class="dropdown-item"
 @onclick="OnToggleShowPassword"> ❹
 @if (field.IsHide) { <span>Show</span> }
 else { <span>Hide</span> }
 </button></li>
 }
 </ul>
 </div>
 }
 }
</div>
```
The preceding source code of ItemDetail.razor includes a context menu button ❶ and three
buttons for the Add ❷, Edit ❸, and Show ❹ menu items. You can see that the source code is also
similar to the one in Items.razor, which includes a list group and a context menu. We will refine
this to a reusable component in the next chapter. The difference in the context menu is we add a
menu item to show or hide the field if the field is a protected field, such as a password. We use the
onclick event handler, OnToggleShowPassword(), to set the IsHide field property to toggle
the visibility of the password field.
Finally, let’s review the event handlers of modal dialogs in ItemDetail.razor.cs:

```Csharp
private async void UpdateFieldAsync(string key, string
 value) {
 if (selectedItem == null || listGroupField == null) {
 throw new NullReferenceException("Selected item is
 null");
 }
 if (string.IsNullOrEmpty(key) ||
 string.IsNullOrEmpty(value)) { return; }
 listGroupField.Key = key;
 listGroupField.Value = value;
 if (_isNewField) {
 // Add a new field
Field newField =
 selectedItem.AddField(listGroupField.Key,
 ((listGroupField.IsProtected) ?
 listGroupField.EditValue :
 listGroupField.Value),
 listGroupField.IsProtected);
 fields.Add(newField);
 }
 else {
 // Update the current field
 var newData = (listGroupField.IsProtected) ?
 listGroupField.EditValue : listGroupField.Value;
selectedItem.UpdateField(listGroupField.Key,
 newData, listGroupField.IsProtected);
 }
 await DataStore.UpdateItemAsync(selectedItem);
}
```
The UpdateFieldAsync() event handler handles both adding and editing a field. It is called with
two parameters – key and value. The corresponding arguments are passed from the modal dialog
and we use them to set the field of listGroupField. The handler checks the _isNewField
variable to detect whether we want to add or edit a field. After that, it calls IDataStore methods
to process add or update actions.
To remove a field, the following DeleteFieldAsync() event handler is invoked:

```Csharp
private async void DeleteFieldAsync() {
 if (listGroupField == null || selectedItem == null) {
 throw new NullReferenceException(
 "Selected item or field is null");
 }
 listGroupField.ShowContextAction = listGroupField;
 selectedItem.DeleteField(listGroupField);
 await DataStore.UpdateItemAsync(selectedItem);
}
```
In the DeleteFieldAsync() event handler, we just delete the field from the selected item and
call the IDataStore method to update the database.
With the implementation of CRUD operations, we have concluded this section. Now, we have a new
version of the password manager app using Blazor UI. The difference between this version and the
one in Part 1 of this book is that we use Blazor to build all UIs. The look and feel of Blazor UI are
similar to web apps, while XAML UI is the same as native apps.

# Summary

In this chapter, we introduced how to create Razor components. We learned about data binding
and the component lifecycle. After that, we created a set of modal dialog components to clean up
our code. With Razor components, we can remove duplicated code and improve the UI design. We
implemented CRUD operations in the event handlers of modal dialogs. We now have a new version
of the password manager app.
During the code analysis, we can see that we still have redundant code in the two main components,
Items and ItemDetail. Even though we optimized modal dialogs, we still have duplicated code
in the list group and context menu. We will convert them to Razor components in the next chapter.















































































































