# Advanced Topics in Creating Razor Components

In Blazor app development, everything is a component. We learned how to create Razor components
in the last chapter. In this chapter, we will explore more advanced topics about Razor components. To
convert list groups and context menus to Razor components, we need to understand advanced topics
such as templated components and form validation. We will introduce these concepts while we are
creating more Razor components in our project.
We will cover the following topics in this chapter:


* Creating more Razor components

* Using templated components

* Built-in Razor components and input validation

# Technical requirements

To test and debug the source code in this chapter, you need to have Visual Studio 2022 installed on
your PC or Mac. Please refer to the Development environment setup section in Chapter 1, Getting
Started with .NET MAUI, for the details.


The source code of this chapter is available in the following GitHub repository:

https://github.com/PacktPublishing/.NET-MAUI-Cross-PlatformApplication-Development/tree/main/Chapter10

The source code can be downloaded using the following Git command:

```
git clone -b chapter10 https://github.com/PacktPublishing/.NET-MAUI-Cross-Platform-Application-Development PassXYZ.Vault2
```

# Creating more Razor components

We developed Modal dialog components in Chapter 9, Razor Components and Data Binding. In this
chapter, we will refine our code to remove the duplicated code in Items and ItemDetail pages
and convert the duplicated code into Razor components. We will create the following components:

* Navbar – This is a component to display a navigation bar

* Dropdown – This is a component to support the context menu

* ListView – This is a component to display a list of items

The ListView component is the most complicated one so we will leave it till the end of this section.
Let’s work on Navbar and Dropdown first.

# Creating the Navbar component

Let’s look at the navigation bar UI in Figure 10.1. We can see that the navigation bar contains a Back
button, a title, and an Add button:

![image](https://user-images.githubusercontent.com/26972859/231983192-19fee52b-9421-4702-8194-3e89f9a0bf28.png)

Figure 10.1: Navigation bar

The current code of the navigation bar is shown next, and this code snippet is duplicated on both the
Items and ItemDetail pages:

```
<div class="container">
 <div class="row">
 <div class="col-12">
 <h1>
@if (selectedItem?.GetParentLink() != null) { ❶
 <a class="btn btn-outline-dark"
 href="@selectedItem?.GetParentLink()">
 <span class="oi oi-chevron-left"
 aria-hidden="true"></span></a> ❷
 }
 @(" " + Title) ❸
 <button type="button"
 class="btn btn-outline-dark float-end"
 data-bs-toggle="modal"
 data-bs-target="#@_dialogEditId"
 @onclick="@(() => _isNewItem=true)"> ❹
 <span class="oi oi-plus" aria-hidden="true">
 </span>
 </button>
 </h1>
 </div>
 </div>
</div>
```
In the preceding code, ❶ the Back button is shown when there is a parent link. ❷ The Back button
is implemented as a ```<a>``` tag. ❸ Title is a string and is shown as part of the ```<h1>``` tag. ❹ The
Add button is implemented as a ```<button>``` tag. Bootstrap style is used to format both the Back and

Add buttons.

To convert the preceding code to a Razor component, we can create a new Razor component in
the PassXYZ.BlazorUI project and name it Navbar. Navbar can display the UI elements in
Figure 10.1, which include a Back button, a title, and an Add button. To separate the UI and logic, we
create both a Navbar.razor.cs C# code-behind file and Razor markup, Navbar.razor. We
define component parameters and event handlers in the C# code-behind file, as shown in Listing 10.1:

**Listing 10.1: Navbar.razor.cs (https://epa.ms/Navbar10-1)**

```Csharp
public partial class Navbar
{
 [Parameter]
 public string? ParentLink { get; set; } ❶
 [Parameter]
public string? DialogId { get; set; } ❷
 [Parameter]
 public string? Title { get; set; } ❸
 [Parameter]
 public EventCallback<MouseEventArgs> OnAddClick { get;
 set; } ❹
 private void OnClickClose(MouseEventArgs e) {
 OnAddClick.InvokeAsync();
 }
}
```
There are four component parameters and an event handler defined in Navbar. We can set the
parent link of the Back button with the ParentLink parameter ❶. The value of Title is set to
the Title parameter ❸. For the Add button, we need to provide an Id and an event handler for
the dialog box so the DialogId ❷ and OnAddClick ❹ parameters are used.
Now, let us look at the Razor file of Navbar in Listing 10.2:

**Listing 10.2: Navbar.razor (https://epa.ms/Navbar10-2)**

```
@namespace PassXYZ.BlazorUI
<div class="container">
 <div class="row">
 <div class="col-12">
 <h1>
 @if (ParentLink != null) { ①
 <a class="btn btn-outline-dark"
 href="@ParentLink"> ①
 <span class="oi oi-chevron-left"
 aria-hidden="true"></span>
 </a>
 }
 @(" " + Title) ③
 <button type="button"
 class="btn btn-outline-dark float-end"
 data-bs-toggle="modal"
 data-bs-target="#@DialogId" ②
 @onclick="OnClickClose"> ④
 <span class="oi oi-plus" aria-hidden="true">
 </span>
 </button>
 </h1>
 </div>
 </div>
</div>
```
We can see that the code is very similar to the one in Items and ItemDetail. The difference is
that we replaced the hardcode value with component parameters (ParentLink ①, DialogId ②,
Title ③, and OnClickClose ④). With this new Navbar component, we can replace the code
in Items using the Navbar component as follows:

```
<Navbar ParentLink="@selectedItem?.GetParentLink()"
 Title="@Title" DialogId="@_dialogEditId"
 OnAddClick="@(() => {_isNewItem=true;})" />
```
And we can replace the code in ItemDetail as follows:

```
<Navbar ParentLink="@selectedItem?.GetParentLink()"
Title="@selectedItem?.Name" DialogId="@_dialogEditId"
OnAddClick="@(() => {_isNewField=true;})" />
```
As we can see, we refined the code by removing duplicated code, and the new code looks much more
elegant and concise.
We have done the work for Navbar. Now let’s move to the Dropdown component.

# Creating a Dropdown component for the context menu

To create a component similar to the context menu, we can reuse the Bootstrap Dropdown component.
As we can see in Figure 10.2, a context menu includes a context menu button and a list of menu items.
When users click on the context menu button, a list of menu items is displayed:

![image](https://user-images.githubusercontent.com/26972859/231984248-0b203f6a-3c83-4d3a-a3e9-86808186ae0a.png)

Figure 10.2: Context menu

The current code of the context menu is duplicated on both the Items and ItemDetail pages,
which are shown here:

```
<button class="opacity-50 btn btn-light dropdown-toggle"
 type="button" id="itemsContextMenu"
 data-bs-toggle="dropdown"
 aria-expanded="false"
 @onclick="@(() => listGroupItem=item)">
 <span class="oi oi-menu" aria-hidden="true"></span>
</button>
<ul class="dropdown-menu"
 aria-labelledby="itemsContextMenu">
 <li><button class="dropdown-item" data-bs-toggle="modal"
 data-bs-target="#@_dialogEditId"
 @onclick="@(() => _isNewItem=false)">
 Edit
 </button></li>
 <li><button class="dropdown-item" data-bs-toggle="modal"
 data-bs-target="#@_dialogDeleteId">
 Delete
 </button></li>
</ul>
```
The Dropdown component of Bootstrap includes a button and an unordered list. We need to define
an event handler for the button to take action. In the preceding code, we set the item variable to
listGroupItem. For the menu items, each menu item is implemented as a ```<button>``` tag and it
accepts a dialog ID and an event handler as parameters. When a menu item is clicked, the corresponding
modal dialog will be shown.
We can create two new Razor components in the PassXYZ.BlazorUI project and name them as
Dropdown and MenuItem. We can also implement them in the C# code-behind file (Listing 10.4)
and Razor file (Listing 10.3) to separate the UI and logic, which we’ll be doing now.
Let’s review the Dropdown component UI first in Listing 10.3:

**Listing 10.3: Dropdown.razor (https://epa.ms/Dropdown10-3)**

```
@namespace PassXYZ.BlazorUI
<button class="opacity-50 btn btn-light dropdown-toggle"
 type="button" id="itemDetailContextMenu"
 data-bs-toggle="dropdown"
 aria-expanded="false" @onclick="OnClick">
 <span class="oi oi-menu" aria-hidden="true"></span>
</button> ❶
<ul class="dropdown-menu"
 aria-labelledby="itemDetailContextMenu">
 @ChildContent
</ul> ❷
```
In the Dropdown component, we define a button ❶ and an unordered list ❷. The click event of the
button is defined as an OnClick event handler. The items in the unordered list are displayed as child
content of the Dropdown component. The component parameters are defined in the C# Dropdown.
razor.cs code-behind file in Listing 10.4:

**Listing 10.4: Dropdown.razor.cs (https://epa.ms/Dropdown10-4)**

```
namespace PassXYZ.BlazorUI;
public partial class Dropdown
{
 [Parameter]
 public EventCallback<MouseEventArgs> OnClick {get;set;}①
 [Parameter]
public RenderFragment ChildContent { get; set; } ②
}
```
In Dropdown.razor.cs, two component parameters – OnClick ① and ChildContent
② – are defined.
The MenuItem component can be displayed as the child content of the Dropdown component. We
can see the UI code of MenuItem in Listing 10.5:

**Listing 10.5: MenuItem.razor (https://epa.ms/MenuItem10-5)**

```
@namespace PassXYZ.BlazorUI
<li>
 <button class="dropdown-item" data-bs-toggle="modal"
 data-bs-target="#@Id" @onclick="OnClick">
 @ChildContent
 </button>
</li>
```
The MenuItem component defines three component parameters – Id, OnClick, and ChildContent.
These parameters are defined in MenuItem.razor.cs in Listing 10.6:

**Listing 10.6: MenuItem.razor.cs (https://epa.ms/MenuItem10-6)**

```
namespace PassXYZ.BlazorUI;
public partial class MenuItem
{
 [Parameter]
 public string? Id { get; set; } ❶
 [Parameter]
 public EventCallback<MouseEventArgs> OnClick {get; set;} ❷
 [Parameter]
 public RenderFragment ChildContent { get; set; } ❸
}
```
❶ The Id parameter is used to specify the dialog ID when the menu item is clicked. ❷ OnClick
is used to register an event handler for a button click event. ❸ ChildContent is used to display
child content such as the menu item name.

❶ The Id parameter is used to specify the dialog ID when the menu item is clicked. ❷ OnClick
is used to register an event handler for a button click event. ❸ ChildContent is used to display
child content such as the menu item name.

```xml
<Dropdown OnClick="@(() => currentItem.
Data=listGroupItem=item)">
 <MenuItem Id="@_dialogEditId"
 OnClick="@(() => _isNewItem=false)">Edit</MenuItem>
 <MenuItem Id="@_dialogDeleteId">Delete</MenuItem>
</Dropdown>
```
On the ItemDetail page, the context menu is implemented as follows:

```
<Dropdown OnClick="@(() = >
 {currentField.Data=listGroupField=field;})">
 <MenuItem Id="@_dialogEditId"
 OnClick="@(() => _isNewField=false)">Edit</MenuItem>
 <MenuItem Id="@_dialogDeleteId">Delete</MenuItem>
 @if (field.IsProtected) {
<MenuItem OnClick="OnToggleShowPassword">
 @(field.IsHide ? "Show":"Hide")
</MenuItem>
 }
</Dropdown>
```
After we refined the code of the Items and ItemDetail pages, we created a modal dialog,
navigation bar, and context menu components. The code looks much more elegant and concise now.
However, we still have room to refine the code further. The main UI logic in both the Items and
ItemDetail pages is a list view. We can refine this part of the code as a ListView component. To
create a ListView component, we need to use an advanced feature called templated components.

# Using templated components

To build a Razor component, component parameters are the channels for parent and child communication.
In Chapter 9, Razor Components and Data Binding, we introduced nested components. We mentioned
a special ChildContent component parameter of the RenderFragment type. The parent
component can set the content of the child component with this parameter. For example, the content
of MenuItem in the following code can be set to an HTML string:

```
<MenuItem Id="@_dialogDeleteId">
 <strong>Delete</strong>
</MenuItem>
```
We can do this because MenuItem defines the following component parameter as we can see in
Listing 10.6:

```Csharp
[Parameter]
public RenderFragment ChildContent { get; set; }
```
If we want to explicitly specify the ChildContent parameter, we can do this as well:

```xml
<MenuItem Id="@_dialogDeleteId">
 <ChildContent>
 <strong>Delete</strong>
 </ChildContent>
</MenuItem>
```
ChildContent is a special component parameter that we can implicitly use in the markup language. To
use ChildContent, we define a component that can accept a UI template of the RenderFragment
type as a component parameter. We can define more than one UI template as a parameter when we
create a new component. This kind of component is called a templated component.
A render fragment of the RenderFragment type represents a segment of the UI to render. There
is also a generic version, ```RenderFragment<TValue>```, which takes a type parameter. We can
specify a type when RenderFragment is invoked.

# Creating a ListView component

To create ListView, we need to use multiple UI templates as component parameters. We can create
a new Razor component in the PassXYZ.BlazorUI project and name it ListView. As we did
for Navbar and the context menu, we separate the UI and code in a Razor file (Listing 10.7) and a
C# code-behind file (Listing 10.8):

# Listing 10.7: ListView.razor (https://epa.ms/ListView10-7)

```
@namespace PassXYZ.BlazorUI
@typeparam TItem
<div class="list-group">
 @if (Header != null) {
 @Header ❶
 }
 @if (Row != null && Items != null) {
 @foreach (var item in Items) {
<div class="dropdown list-group-item
 list-group-item-action
 d-flex gap-1 py-2" style="border: none"
 aria-current="true">
 @Row.Invoke(item) ❷
 </div>
 }
 }
 @if (Footer != null) {
 <div class="container">
 <article>@Footer</article>
 </div> ❸
 }
</div>
```

In the ListView Razor file, we define three UI templates – Header ❶, Row ❷, and Footer ❸.
We render Header and Footer the same as ChildContent, but the Row component parameter
looks different. We render it as follows:

```@Row(item)```

Or we can render it like this:

```@Row.Invoke(item)```

We render it with an item argument. The type of Row is ```RenderFragment<TValue>```, as we
can see in Listing 10.8:

**Listing 10.8: ListView.razor.cs (https://epa.ms/ListView10-8)**

```
namespace PassXYZ.BlazorUI;
public partial class ListView<TItem>
{
 [Parameter]
 public RenderFragment? Header { get; set; } ①
 [Parameter]
 public RenderFragment<TItem>? Row { get; set; } ②
 [Parameter]
 public IEnumerable<TItem>? Items { get; set; } ③
 [Parameter]
 public RenderFragment? Footer { get; set; } ④
}
```
We define ListView as a generic ````ListView<TItem>``` type with the TItem type parameter. In the
ListView component, we can define a list view header using the Header ① parameter and the footer
using Footer ④. ListView can be bound to any data collection of the ```IEnumerable<TItem>```
type using the Items parameter ③. The Row parameter ② can be used to define the UI template
for an individual item in the foreach loop.

# Using the ListView component

Now, we can look at the usage of the ListView component in the Items and ItemDetail pages.
We use the ItemDetail page as an example here:

```
<ListView Items="fields"> ❶
 <Row Context="field"> ❷
 @if (field.ShowContextAction == null) {
 <span class="oi oi-pencil" aria-hidden="true"></span>
 <div class="d-flex gap-2 w-100
 justify-content-between">
 <div>
 <h6 class="mb-0">@field.Key</h6>
 <p class="mb-0">@field.Value</p>
 </div>
 </div>
 <Dropdown
 OnClick="@(() =>
 {currentField.Data=listGroupField=field;})">
 <MenuItem Id="@_dialogEditId"
 OnClick="@(() => _isNewField=false)">
 Edit
 </MenuItem>
 <MenuItem Id="@_dialogDeleteId">Delete</MenuItem>
 @if (field.IsProtected) {
 <MenuItem OnClick="OnToggleShowPassword">
 @(field.IsHide ? "Show":"Hide")
 </MenuItem>
 }
 </Dropdown>
 }
 </Row>
 <Footer>
 @((MarkupString)notes)
 </Footer>
</ListView>
```
Since we define Header, Row, and Footer as optional parameters, we don’t have to specify all of
them. In the ItemDetail page, we use Row and Footer. ❶ We need to pass the list of fields to the
Items parameter first. ❷ Each field in the foreach loop is passed to ListView as an argument
to Row, which is defined here:

```<Row Context="field">```

The "field" value of the Context property is used to specify the argument to Row. Inside the UI
template of Row, we display the key value of field and create a context menu using the Dropdown
and MenuItem components that we implemented in the last section.
Using the ListView component, we can see that the implementation of the ItemDetail page
looks much better now.
  
We have made the improvement by creating our own Razor components.

In Blazor, there are a few options for us to develop a UI. We can use HTML/CSS for the UI design. On
top of HTML/CSS, we can build our own Razor components to improve the design and implementation.
There are also many third-party Razor component libraries available for us, and we can also use ASP.
NET Core built-in Razor components.
In the next section, we will support data validation using ASP.NET built-in Razor components.

# Built-in components and validation

In our app, we implement EditorDialog as a key value editor. When we create or edit an item or a
field, we use it to edit a pair of key-value data. EditorDialog is built using the Bootstrap framework.
One key feature missing in EditorDialog is that it doesn’t support data validation. Data validation
includes two layers – the UI layer and logic. We can implement simple data validation logic in C#, but
the UI of data validation is much more complicated. We haven’t done it in the UI layer yet. In both 

the Items and ItemDetail pages, we implemented simple data validation logic. We can review
the data validation logic in the UpdateItemAsync() method of the Items page:

```Csharp
private async Task<bool> UpdateItemAsync(string key, string
value)
{
 if (listGroupItem == null) return false;
 if (string.IsNullOrEmpty(key) ||
 string.IsNullOrEmpty(value))
 return false;
 listGroupItem.Name = key;
 listGroupItem.Notes = value;
 if (_isNewItem) {…}
 else {...}
 StateHasChanged();
 return true;
}
```
In EditorDialog, after we complete the editing, UpdateItemAsync() is invoked to save
data. We check the value of the key and value arguments first before we continue. If any of them
is null, we just return false. There is no problem in the program logic here, but we should notify
users about the error, which is due to data saving. With ASP.NET built-in components, data validation
is supported at the UI layer. The user can get instant feedback about the error.

# Using built-in components

We actually used built-in components in previous chapters. When we introduced routing and the
layout of Razor components, we used the Router, RouteView, LayoutView, and MainLayout
components in Main.razor. They are all built-in components.
In this section, we will explore built-in input components, which we can use to enhance the experience
of editing components with data validation support. The following table is a list of the built-in
input components:
  
|Input component|HTML tag|
|:--------------|:-------|
|InputCheckbox|```<input type="checkbox">```|
|```InputDate<TValue>```|```<input type="date">```|
|InputFile|```<input type="file">```|
|```InputNumber<TValue>```|```<input type="number">```|
|```InputRadio<TValue>```|```<input type="radio">```|
|```InputRadioGroup<TValue>``` |Group of child ```InputRadio<TValue>```|
|```InputSelect<TValue>```|```<select>```|
|InputText|```<input>```|
|InputTextArea |```<textarea>```|
|EditForm|```<form>```|
  
Table 10.1: Built-in components

To find detailed information about built-in input components, you can check the following
Microsoft document:
  
https://learn.microsoft.com/en-us/aspnet/core/blazor/forms-and-inputcomponents?view=aspnetcore-6.0

In Table 10.1, we can see a list of input components and an EditForm component. The built-in
input components are enhanced versions of the corresponding HTML elements listed in the righthand column. When we use built-in input components with EditForm, EditForm can coordinate
both validation and submission events. The built-in input components can validate user input when
a form is submitted.

# Using the EditForm component

The EditForm component is an enhanced version of the HTML <form> element. To use EditForm,
we can refer to the following code, which shows an empty EditForm component:

```
<EditForm Model="ModelData" OnSubmit="HandleSubmit"
OnInValidSubmit="HandleInValidSubmit"
OnValidSubmit="HandleValidSubmit">
</EditForm> 
```
Or, it can be used in another way:

```
<EditForm EditContext="_editContext" OnSubmit="HandleSubmit"
 OnInValidSubmit="HandleInValidSubmit"
 OnValidSubmit="HandleValidSubmit">
</EditForm>
```
We can pass data to it using the Model parameter or EditContext.

We can specify an instance of a class as Model to be edited in EditForm. An instance of EditContext
will be created based on the assigned model instance by EditForm. EditContext is used as a
cascading value for other components in the form. We can also specify an EditContext instance
directly if we want to take control of it.
  
To handle the result of form editing, we can register the following callback functions:

  * OnInvalidSubmit – This callback will be invoked when the form is submitted and EditContext is invalid

  * OnSubmit – This callback will be invoked when the form is submitted

  * OnValidSubmit – This callback will be invoked when the form is submitted and EditContext is valid

  We will use the EditForm component and built-in input components to create a new EditFormDialog
component to enhance our key value editor with data validation support.

# Creating an EditFormDialog component

To support data validation, the EditForm component needs to be bound to a model that uses data
annotations. The built-in input components should be used for the form editing so that enhanced
features such as data validation can be used.
First, we need to create a model class, KeyValueData, that can be used for key-value editing in the
PassXYZ.BlazorUI project, as shown in Listing 10.9:

**Listing 10.9: KeyValueData.cs (https://epa.ms/KeyValueData10-9)**

```
using System.ComponentModel.DataAnnotations; ❸
using KPCLib;
using PassXYZLib;
namespace PassXYZ.BlazorUI;
public class KeyValueData<T> : IKeyValue {
 [Required(ErrorMessage = "{0} cannot be empty.")]
 [Display(Name = "This field")]
 public string Key { ❶
 get {
 if (Data is Item item) { return item.Name; }
 if (Data is Field field) { return field.Key; }
 return string.Empty;
}
 set {
 if (Data is Item item) { item.Name = value;
 IsChanged = true; }
 if (Data is Field field) { field.Key = value;
 IsChanged = true; }
 }
 }
 [Required(ErrorMessage = "{0} cannot be empty.")]
 [Display(Name = "This field")]
 public string Value { ❷
 get {
 if (Data is Item item) { return item.Notes; }
 if (Data is Field field) { return field.EditValue; }
 return string.Empty;
 }
 set {
 if (Data is Item item) { item.Notes = value;
 IsChanged = true; }
 if (Data is Field field) { field.EditValue = value;
 IsChanged = true; }
 }
 }
 public bool IsChanged { get; set; } = false;
 public bool IsValid {
 get {
 return !(string.IsNullOrEmpty(Key) ||
 string.IsNullOrEmpty(Value));
 }
 }
 public T? Data { get; set; } ❹
 public KeyValueData() {
 if (Data is Item item) { item = new NewItem(); }
 if (Data is Field field) { field = new NewField(); }
 }
```
The KeyValueData<T> class is used to create the model instance for EditForm. It is a generic
type and implements an IKeyValue interface, which defines properties that need to be implemented.
In the KeyValueData<T> class, we define the Key ❶ and Value ❷ properties, IsChanged,
IsValid, and Data ❹. We can see that we mark the Key and Value properties with data annotation
[Required] and [Display] attributes. The [Required] attribute indicates the user must enter
a value. The [Display] attribute defines the name to display for the property in the error message
of data validation, as shown in Figure 10.3.
Both [Required] and [Display] are defined in the System.ComponentModel.
DataAnnotations namespace ❸.
The KeyValueData<T> class is a generic class and takes a T type parameter to specify the actual
data type for editing. The Data property ❹ is defined as the T type. We can pass either the Item or
Field class as a type parameter to the Data property. The Key or Value property is bound to the
property of the Item or Field instance stored in the Data property

![image](https://user-images.githubusercontent.com/26972859/231991559-507ad884-5323-4d35-9960-924ff4ffa203.png)

Figure 10.3: EditFormDialog (adding a new item)

We can see in Figure 10.3 that the error messages will be displayed when users submit the form with
an empty value in either the key or value field. To enable data validation, we also need to add the
<DataAnnotationsValidator/> component to EditForm, as shown here:

```
<EditForm class="row gx-2 gy-3" Model="@ModelData"
 OnValidSubmit="@HandleValidSubmit">
 <DataAnnotationsValidator />
 ...
</EditForm>
```
Using EditForm, it is quite easy to support data validation. To enable data validation using EditForm,
follow these steps:

1. Bind EditForm to a model that uses data annotations attributes.
2. Add the <DataAnnotationsValidator/> component to EditForm.
3. Use built-in input components for the editing

There is a long list of built-in attribute-defined validations. You can find out more information about
them in the Microsoft documentation in the Further reading section. Some of them are listed here:

• [ValidateNever]: Indicates that a property or parameter should be excluded from validation
• [Compare]: Validates that two properties in a model match
• [EmailAddress]: Validates that the property has an email format
• [Phone]: Validates that the property has a telephone number format
• [Range]: Validates that the property value falls within a specified range
• [RegularExpression]: Validates that the property value matches a specified regular expression
• [Required]: Validates that the field isn’t null
• [StringLength]: Validates that a string property value doesn’t exceed a specified length limit
• [Url]: Validates that the property has a URL format

With the introduction of built-in components, we can create a new Razor component, EditFormDialog,
in the PassXYZ.BlazorUI project. The implementation of the EditFormDialog<TItem>
component can be found in Listing 10.10 and Listing 10.11:

**Listing 10.10: EditFormDialog.razor (https://epa.ms/EditFormDialog10-10)**

```
@namespace PassXYZ.BlazorUI
@typeparam TItem
<div class="modal fade" id=@Id tabindex="-1"
 aria-labelledby="ModelLabel" aria-hidden="true">
 <div class="modal-dialog">
 <div class="modal-content">
 <div class="modal-header">
 <h5 class="modal-title" id="ModelLabel">
 @ModelData.Key</h5>
<button type="button" class="btn-close"
 data-bs-dismiss="modal"
 aria-label="Close"></button>
 </div>
 <div class="modal-body">
 <EditForm class="row gx-2 gy-3" Model="@ModelData" ❶
 OnValidSubmit="@HandleValidSubmit">
 <DataAnnotationsValidator /> ❷
 @if (IsKeyEditingEnable) {
 <div class="form-group">
 <InputText id="Name" class="form-control" ❸
 @oninput="KeyHandler"
 placeholder=@KeyPlaceHolder
 @bind-Value="ModelData.Key" />
 <ValidationMessage For="()=>ModelData.Key"/> ❺
 </div>
 }
 @ChildContent
 <div class="form-group">
 <InputTextArea id="Value" class="form-control" ❹
 @oninput="KeyHandler"
 placeholder=@ValuePlaceHolder
 @bind-Value="ModelData.Value" />
 <ValidationMessage For="()=>ModelData.Value"/> ❺
 </div>
 <div class="col-12">
 <button type="button" class="btn btn-secondary"
 data-bs-dismiss="modal"
 @onclick="OnClickClose">
 @CloseButtonText
 </button>
 <button type="submit" class="btn btn-primary"
 data-bs-dismiss="@dataDismiss"
 @onclick="OnClickSave">
 @SaveButtonText
 </button>
</div>
 </EditForm>
 </div>
 </div>
 </div>
</div>
```
As we can see in the Razor markup in Listing 10.10, we build the EditFormDialog<TItem>
component using Bootstrap’s Modal. It includes the EditForm ❶ component in the modal body.
The model bound to EditForm is ModelData of the KeyValueData<TItem> type defined in
Listing 10.11. The DataAnnotationsValidator component ❷ is defined for the data validation.
We use InputText ❸ for key editing and InputTextArea ❹ for value editing. The
ValidationMessage validation component ❺ is used to define the error message for validation.
In ValidationMessage, we need to specify the field using the For property as follows:

```<ValidationMessage For="() => ModelData.Key" />```

The format of the error message is defined using the ErrorMessage property of the [Required]
attribute as follows:

```
[Required(ErrorMessage = "{0} cannot be empty.")]
 [Display(Name = "This field")]
 public string Value { ... }
```
As we can see in Listing 10.11, component parameters and callback functions for the event handling
are defined in EditFormDialog.razor.cs:

**Listing 10.11: EditFormDialog.razor.cs (https://epa.ms/EditFormDialog10-11)**

```
namespace PassXYZ.BlazorUI;
public partial class EditFormDialog<TItem> {
 [Parameter]
 public string Id { get; set; } = default!;
 [Parameter]
 public RenderFragment ChildContent { get; set; }
 [Parameter]
 public Action? OnClose { get; set; }
 [Parameter]
public Func<string, string, Task<bool>>? OnSaveAsync {
 get; set; }
 [Parameter]
 [NotNull]
 public string CloseButtonText { get; set; } = "Cancel";
 [Parameter]
 [NotNull]
 public string SaveButtonText { get; set; } = "Save";
 [Parameter]
 public KeyValueData<TItem> ModelData { get; set; }
 [Parameter]
 public string? KeyPlaceHolder { get; set; }
 [Parameter]
 public string? ValuePlaceHolder { get; set; }
 bool _isKeyEditingEnable = false;
 [Parameter]
 public bool IsKeyEditingEnable ...
 [Parameter]
 public EventCallback<bool>?
 IsKeyEditingEnableChanged{get; set;}
 private string dataDismiss = string.Empty;
 public EditFormDialog() {
 ModelData = new();
 }
 private void OnClickClose() {
 OnClose?.Invoke();
 }
 private void OnClickSave() {
 SetSaveButtonText();
 }
 private async Task HandleValidSubmit() {
 if (OnSaveAsync != null && ModelData.IsChanged) {
 await OnSaveAsync(ModelData.Key, ModelData.Value);
 ModelData.IsChanged = false;
 }

 private void KeyHandler() {
 SetSaveButtonText(true);
 }
 private void SetSaveButtonText(bool changed = false) {
 if (ModelData == null) return;
 if (!ModelData.IsValid || changed) {
 dataDismiss = string.Empty;
 SaveButtonText = "Save";
 }
 else {
 dataDismiss = "modal";
 SaveButtonText = "Close";
 }
 }
}
```
We can refer to the component parameters of EditFormDialog<TItem> in Table 10.2

|Parameter|Description|
|:--------|:----------|
|Id|The ID of the dialog. This is used by Bootstrap.|
|ChildContent|Child content render fragments.|
|OnClose|Callback for the Close button.|
|OnSaveAsync|Callback for the Save button.|
|CloseButtonText|Text of the Close button.|
|SaveButtonText|Text of the Save button.|
|ModelData|Model data bound to EditForm.|
|KeyPlaceHolder|A Key placeholder.|
|ValuePlaceHolder|A Value placeholder|
|IsKeyEditingEnable|Enable or disable key editing|
|IsKeyEditingEnableChanged |Callback for two bindings of IsKeyEditingEnable|

Table 10.2: EditFormDialog<TItem> component parameters

After we have created the EditFormDialog<TItem> component, we have done all the optimization
of our code. Let us review the final code of the Items (Listing 10.12) and ItemDetail (Listing
10.13) pages:

**Listing 10.12: Items.razor (https://epa.ms/Items10-12)**

```
@page "/group"
@page "/group/{SelectedItemId}"
@using System.Diagnostics
@using PassXYZLib
<Navbar ParentLink="@selectedItem?.GetParentLink()"
 Title="@Title" ❶
 DialogId="@_dialogEditId"
 OnAddClick="@(() =>
 {_isNewItem=true;
 currentItem.Data=listGroupItem=_newItem;
 _newItem.Name="";_newItem.Notes="";})" />
<ListView Items="items"> ❷
 <Row Context="item">
<img src="@item.GetIcon()" alt="twbs"
 width="32" height="32"
 class="rounded-circle flex-shrink-0 float-start">
<a href="@item.GetActionLink()"
 class="list-group-item ...">
 <div class="d-flex"><div>
 <h6 class="mb-0">@item.Name</h6>
 <p class="mb-0 opacity-75">@item.Description</p>
 </div></div>
 </a>
<Dropdown OnClick="@(() => ❸
 currentItem.Data=listGroupItem=item)">
 <MenuItem Id="@_dialogEditId" OnClick="@(() =>
 _isNewItem=false)">Edit</MenuItem>
 <MenuItem Id="@_dialogDeleteId">Delete</MenuItem>
 </Dropdown>
 </Row>
</ListView>
<EditFormDialog Id="@_dialogEditId" ❹
 ModelData="@currentItem" IsKeyEditingEnable="true"
 OnSaveAsync="UpdateItemAsync" KeyPlaceHolder="Item name"
 ValuePlaceHolder="Pleae provide a description">
 @if (_isNewItem) {
<InputSelect @bind-Value="_newItem.SubType"
 class="form-select">
 <option selected value=@ItemSubType.Group>
 @ItemSubType.Group</option>
 <option value=@ItemSubType.Entry>
 @ItemSubType.Entry</option>
 <option value=@ItemSubType.PxEntry>
 @ItemSubType.PxEntry</option>
 <option value=@ItemSubType.Notes>
 @ItemSubType.Notes</option>
 </InputSelect>
 }
</EditFormDialog>
<CascadingValue Value="@_dialogDeleteId" Name="Id">
<ConfirmDialog Title=@listGroupItem.Name ❺
 OnConfirmClick="DeleteItemAsync" />
</CascadingValue>
```

In the final code of Items.razor, we use Navbar ❶ to create the navigation bar. In the body of the
page, we use a ListView ❷ component to display the list of items. For each item, the UI template is
defined in the Row property. In the UI template, we display the item name, description, and context
menu. In the context menu, we use the Dropdown ❸ component to support editing or deleting the
item. For Add, Edit, or Delete actions, EditorFormDialog ❹ or ConfirmDialog ❺ is
used to perform the respective actions.
In ItemDetail.razor, the code is similar to Items.razor:

**Listing 10.13: ItemDetail.razor (https://epa.ms/ ItemDetail10-13)**

```
@page "/entry/{SelectedItemId}"
@namespace PassXYZ.Vault.Pages

```
<!-- Back button and title -->
<Navbar ParentLink="@selectedItem?.GetParentLink()" ①
 Title="@selectedItem?.Name" DialogId="@_dialogEditId"
 OnAddClick="@(() => {_isNewField=true;
 currentField.Data=listGroupField=_newField;_
 newField.Key="";_newField.Value="";})" />
<!-- List view with context menu -->
<ListView Items="fields"> ②
 <Row Context="field"> ③
 @if (field.ShowContextAction == null) {
 <span class="oi oi-pencil" aria-hidden="true"></span>
 <div class="d-flex gap-2 w-100
 justify-content-between">
 <div>
 <h6 class="mb-0">@field.Key</h6>
 <p class="mb-0">@field.Value</p>
 </div>
 </div>
 <Dropdown OnClick="@(() =>
 {currentField.Data=listGroupField=field;})">
 <MenuItem Id="@_dialogEditId" OnClick="@(() =>
 _isNewField=false)">Edit</MenuItem>
 <MenuItem Id="@_dialogDeleteId">Delete</MenuItem>
 @if (field.IsProtected) {
 <MenuItem OnClick="OnToggleShowPassword">
 @(field.IsHide ? "Show":"Hide")</MenuItem>
 }
 </Dropdown>
 }
 </Row>
 <Footer>@((MarkupString)notes)</Footer> ④
</ListView>
<!-- Editing Modal -->
<EditFormDialog Id="@_dialogEditId" ⑤
 ModelData="@currentField"
IsKeyEditingEnable=@_isNewField
 OnSaveAsync="UpdateFieldAsync"
 KeyPlaceHolder="Field name"
 ValuePlaceHolder="Field content">
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
</EditFormDialog>
<!-- Deleting Modal -->
<CascadingValue Value="@_dialogDeleteId" Name="Id">
 <ConfirmDialog Title=@listGroupField.Key ⑥
 OnConfirmClick="DeleteFieldAsync" />
</CascadingValue>
```
There is a navigation bar using Navbar ①. We use ListView ② to display the fields of an entry,
which include a Row template ③ and a Footer template ④. In the Row template, the content of
the field is displayed, and each row has a context menu. The context menu includes the menu of three
actions (Edit, Delete, and Show). The Edit action is associated with the EditFormDialog
dialog ⑤ and the Delete action is associated with the ConfirmDialog dialog ⑥.
We now have a working password manager app built using Blazor UI. With this, we conclude this
chapter and Part 2 of this book.

# Summary

In this chapter, we continue our work of optimizing our UI implementation by creating more Razor
components. We introduced advanced topics such as templated components and using generic types
in the Razor component. We also introduced Blazor built-in components and used these components
to support data validation in our app. We developed an enhanced version of the key value editor using
EditForm. With an understanding of the advanced topics about Razor components, we can now
create more powerful Razor components.

We have completed Part 2 of this book. We will move to Part 3 of this book to introduce unit tests
and deployment of .NET MAUI applications. We will learn how to implement unit tests for the .NET
MAUI app in the next chapter, and how to deploy our app to different app stores in Chapter 12,
Preparing for Deployment in App Stores.
Please stay tuned!

# Further reading

  * Dropdown is a Bootstrap component

  * https://getbootstrap.com/docs/5.1/components/dropdowns/

  * Modal is the Bootstrap modal component

  * https://getbootstrap.com/docs/5.1/components/modal/

  * ASP.NET Core Blazor forms and input components

  * https://learn.microsoft.com/en-us/aspnet/core/blazor/forms-andinput-components?view=aspnetcore-6.0











































