# CHAPTER 9

# Local Data

In this chapter, you will learn about the different types of local data, what
they are best used for, and how to apply them in your application. The
options will include understanding when and where to store data that
needs to be kept secure.
You will modify your application to store the boards that your user
creates so that they can be displayed in the slide-out menu and also be
opened. You will also record the last opened board so that when returning
to the application this board will be presented to the user.

# What Is Local Data?

When building an application, whether it is targeted for a single or
multiple platforms, you will very likely need to store some data about the
state of the application. The types of data you will need to store can vary
between storing “simple” settings, caching files/data, or even storing a
full set of data inside a local database. These types of data are called local
data since they live on the device that your application is running on. Data
that comes from a remote endpoint is called remote data and this will be
covered in Chapter 10.
.NET MAUI provides multiple options when you want to store data
locally on a device. Each option is better suited to a specific purpose and
size of data. Here is a brief overview of those options:

File system: Stores loose files directly on the device
through file system access

• Database: Stores data in a file optimized for access

• Preferences: Stores data in key-value pairs

• Secure storage: Stores data in key-value pairs like
preferences but stores them in a secure location on
the device

# File System

NET MAUI provides some helpful abstractions over the multiple platforms
that it supports. One such abstraction is the FileSystem helper class. It
comes from the old Xamarin.Essentials library and now is a core part of
.NET MAUI. It allows you to obtain useful bits of information to help with
common tasks involving the file system.
Let’s take a look at the properties the FileSystem class offers you as it
helps to know when they should be used and for what type of data.

# Cache Directory

You have no need to cache anything as part of the application we’re
building in this book; however, I feel this is a valuable piece of information
to mention. This property enables you to get the most appropriate location
to store cache data. You can store any type of data in this directory.
Typically you store it when you want to persist it longer than just holding
it in memory. Your application should not rely on this data to function
because the operating system can and will clear this storage down.

# App Data Directory

The AppDataDirectory property provides the app's top-level directory
for storing any files. These files are backed up with the operating system
syncing framework.
This property is precisely what you are going to need to use when
creating and opening your database files in the next section. So, let’s set up
the bits that you will need.
The FileSystem helper class provides a set of static properties,
meaning you can simply write

```var appDataDirectory = FileSystem.AppDataDirectory;```

However, as you have discovered already in this book, it does not
lend itself well to unit testing. Instead, you can rely on the IFileSystem
interface and register the .NET MAUI implementation with your app
builder. Let’s open up your MauiProgram.cs file and add the following line
into the CreateMauiApp method:

```builder.Services.AddSingleton(FileSystem.Current);```

This will register the FileSystem.Current property as the IFileSystem
interface so whenever you state that your classes depend on IFileSystem,
they will be provided with the FileSystem.Current instance.
Now that you have covered FileSystem and are ready to create your
database files, you can learn about database access in .NET MAUI.

# Database

A database is a collection of data that is organized. In a database, data
is organized or structured into tables consisting of rows and columns.
Databases are a much better approach than storing data in files. The
ability to index the data makes it easier to query and manipulate. There 
are different kinds of databases, ranging from relational databases to
distributed databases, cloud databases, and NoSQL databases. In this
chapter, you will focus on relational and NoSQL databases.
Every application I have ever built has required some form of database,
and I suspect that most of the applications that you will build will also
require one. A database really provides value when you need to link data
together or filter and sort the data in an efficient manner.
In your application, you are going to provide the ability to save a
board and return a list of boards that the user has created. To abstract this
slightly, you will be using the repository pattern. You will also provide the
ability to store where the widgets have been placed so that they will be
remembered when a user loads the board back up. Figure 9-1 shows the
entity relationship diagram for the database you will be creating.

![image](https://user-images.githubusercontent.com/26972859/231455605-31c043ce-716f-4b2f-b172-d0a7ee33a5d5.png)
Figure 9-1. The entity relationship diagram of your database models

# Repository Pattern

The repository pattern allows you to hide all the logic that deals with
creating, reading, updating, and deleting (also known as CRUD) entities
within your application. By using this pattern, it allows you to keep all
the knowledge around how entities are loaded, saved, and more in a
single place. This has the added benefit that if you want to completely
change where your data is loaded from, you only need to change the
implementation inside the repository. It also allows you to provide mock
implementations when wanting to perform things like unit testing and you
don’t want to have to rely on an actual database existing.

Let’s add a new folder called Data and then add an interface for your
repository to that folder called IBoardRepository. Change the code to look
as follows:

```Csharp
using WidgetBoard.Models;
namespace WidgetBoard.Data;
public interface IBoardRepository
{
 void CreateBoard(Board board);
 void CreateBoardWidget(BoardWidget boardWidget);
 void DeleteBoard(Board board);
 IReadOnlyList<Board> ListBoards();
 Board LoadBoard(int boardId);
 void UpdateBoard(Board board);
}
```
Now that you have defined your interface, you can update your
application’s codebase to use this interface when loading and saving
your boards.

# Creating a Board

The first place to update is your BoardDetailsPageViewModel class, which
provides support for creating a new board. Open up the class and make the
following modifications.
Add a new IBoardRepository field.

```private readonly IBoardRepository boardRepository;```

Assign a valid instance to the boardRepository field; the modifications
are in bold.

```Csharp
public BoardDetailsPageViewModel(
 ISemanticScreenReader semanticScreenReader,
 IBoardRepository boardRepository)
{
 this.semanticScreenReader = semanticScreenReader;
 this.boardRepository = boardRepository;
 SaveCommand = new Command(
 () => Save(),
 () => !string.IsNullOrWhiteSpace(BoardName));
}
```
Use the boardRepository field when saving; the modifications are
in bold.

```Csharp
private async void Save()
{
 var board = new Board
 {
 Name = BoardName,
 NumberOfColumns = NumberOfColumns,
 NumberOfRows = NumberOfRows
 };
 this.boardRepository.CreateBoard(board);
 semanticScreenReader.Announce($"A new board with the name
{BoardName} was created successfully.");
 await Shell.Current.GoToAsync(
 "fixedboard",
 new Dictionary<string, object>
 {
 { "Board", board}
 });
}
```

# Listing Your Boards

In the previous chapters, you just added a fixed board and added it to the
Boards collection in your AppShellViewModel class. Now you are going to
modify it so that it can be populated by the boards the user creates and you
store in the database. Open the AppShellViewModel.cs file and make the
following changes.
Add a field for your IBoardRepository.

```private readonly IBoardRepository boardRepository;```

Modify your constructor to use the IBoardRepository as a
dependency.

```Csharp
public AppShellViewModel(
 IBoardRepository boardRepository)
{
 this.boardRepository = boardRepository;
}
```
Load the list of boards and populate your collection.

```Csharp
public void LoadBoards()
{
 var boards = this.boardRepository.ListBoards();
 foreach (var board in boards)
 {
 Boards.Add(board);
 }
}
```
There is a further change that you need to make in order to allow your
AppShellViewModel class to actually load the board. You need to hook into
some of the lifecycle events that apply to Pages in .NET MAUI. AppShell
inherits from Page, which means you get full access to those lifecycle
events. The specific event you care about now is the OnAppearing event. It
is called when your page is displayed on screen.

---
The OnAppearing method can be called multiple times during the
lifetime of the page, so it is recommended to make your method
idempotent or check whether it has been called before in order to
prevent odd behavior when called a second time.
---

OnAppearing is a great choice for your scenario because it will result
in your code being executed every time the view appears; this can be
every time your flyout menu is opened. This provides you with the ability
to refresh your list of boards every time the user opens the flyout menu.
The main reason it is fine for your scenario is because you will be loading
data from a local database with a limited number of boards to load, so it
will be pretty quick. In scenarios where you are loading from an external
webservice, it can take much more time to perform it and therefore
you may wish to maintain some level of caching and prevent calling
the webservice every time the view appears. A better option under this
scenario and probably most typical scenarios in .NET MAUI applications is
to use the OnNavigatedTo method.
Let’s open your AppShell.xaml.cs file and make use of this
lifecycle method.

```Csharp
protected override void OnAppearing()
{
 base.OnAppearing();
 ((AppShellViewModel)BindingContext).LoadBoards();
}
```
When the method gets called, you use the newly added LoadBoards
method on your view model. The main reason you hook into this lifecycle
event is when you eventually try to navigate to the last used board in the
LoadBoards method, you need to make sure the application has started
rendering; otherwise the navigation will fail.

# Loading a Board

Up until this point you have relied on passing the Board into the
FixedBoardPageViewModel and displaying the details of that. The loading
process would become rather inefficient if you were to load all boards and
the associated BoardWidgets when listing all boards in the system, so you
need to do this in a two-step process: first, list the boards as you did in the
previous section and, second, load the board in the view model. This will
be a slightly involved process so let’s walk through it step-by-step. Open
the FixedBoardPageViewModel.cs file and make the following changes
Add the following fields to store the board that is loaded and the
repository to perform the load:

```Csharp
private Board board;
private readonly IBoardRepository boardRepository;
```

In your constructor, add the board repository dependency and assign
to the newly created field. Changes are in bold.

```Csharp
public FixedBoardPageViewModel(
 WidgetTemplateSelector widgetTemplateSelector,
 WidgetFactory widgetFactory,IBoardRepository boardRepository)
{
 WidgetTemplateSelector = widgetTemplateSelector;
 this.widgetFactory = widgetFactory;
 this.boardRepository = boardRepository;
 Widgets = new ObservableCollection<IWidgetViewModel>();
 AddWidgetCommand = new Command(OnAddWidget);
 AddNewWidgetCommand = new Command<int>(index =>
 {
 IsAddingWidget = true;
 addingPosition = index;
 });
}
```
Now let’s load the Board inside your ApplyQueryAttributes method.
The changes are in bold.

```Csharp
public void ApplyQueryAttributes(IDictionary<string,
object> query)
{
 var boardParameter = query["Board"] as Board;
 board = boardRepository.LoadBoard(boardParameter.Id);
 BoardName = board.Name;
 NumberOfColumns = board.NumberOfColumns;
 NumberOfRows = board.NumberOfRows;
 foreach (var boardWidget in board.BoardWidgets)
 {
 var widgetViewModel = widgetFactory.CreateWidgetViewMod
el(boardWidget.WidgetType);
 widgetViewModel.Position = boardWidget.Position;
 Widgets.Add(widgetViewModel);
 }
}
```
Next, add the ability to save a widget’s position on the board.

```Csharp
private void SaveWidget(IWidgetViewModel widgetViewModel)
{
 var boardWidget = new BoardWidget
 {
 BoardId = board.Id,
 Position = widgetViewModel.Position,
 WidgetType = widgetViewModel.Type
 };
 boardRepository.CreateBoardWidget(boardWidget);
}
```
The above method will create a new BoardWidget model class and save
it into the database for you.
Finally, you need to call the SaveWidget method. For the purpose of
your application, you are going to provide an auto save feature, so each
time a widget is added to the board, you will save it immediately to the
database. In order to achieve this, you just need to add the bold line into
your AddWidget method.

```Csharp
private void OnAddWidget()
{
 if (SelectedWidget is null)
 {
 return;
 }
 var widgetViewModel = widgetFactory.CreateWidgetViewModel(S
electedWidget);
 widgetViewModel.Position = addingPosition;
 Widgets.Add(widgetViewModel);
 SaveWidget(widgetViewModel);
 IsAddingWidget = false;
}
```
You can’t run your code yet because you don’t have an implementation
of your IBoardRepository interface so let’s look at two different database
options that will allow you to provide an implementation for your
IBoardRepository.

# SQLite

SQLite is a lightweight cross-platform database that has become the
go-to option for providing database support in mobile applications. The
database is stored locally in a single file on the device's file system.
SQLite is supported natively by Android and iOS; however, they require
access via C++. There are several C# wrappers around the native SQLite
engine that .NET developers can use. The most popular choice is the C#
wrapper called SQLite-net.

# Installing SQLite-net

In order to install and use Sqlite-net, you need to install the NuGet package
called Sqlite-net-pcl. You may notice the extra -pcl suffix in the NuGet
package name and find this confusing. This is an artifact of an old piece
of technology used in Xamarin.Forms applications. The name has been
retained but don’t worry; this is the correct package for adding to a .NET
MAUI project.
You can do this by following these steps.
1. Right-click the WidgetBoard project.
2. Click Manage NuGet Packages.
3. In the Search field, enter Sqlite-net-pcl.
4. Select the Sqlite-net-pcl package and select Add
Package.
5. A confirmation dialog will show. Review and accept
the license details if you are happy.
6. Repeat the above steps for the following packages:
a. SQLitePCLRaw.bundle_green
b. SQLitePCLRaw.provider.dynamic_cdecl
c. SQLitePCLRaw.provider.sqlite3

# Using Sqlite-net

The first step is to create your IBoardRepository implementation. Add a
new class file called SqliteBoardRepository in your Data folder, and make
it implement your IBoardRepository interface.

```Csharp
using SQLite;
using WidgetBoard.Models;
namespace WidgetBoard.Data;
public class SqliteBoardRepository : IBoardRepository
{
 public void CreateBoard(Board board)
 {
 throw new NotImplementedException();
 }
 public void CreateBoardWidget(BoardWidget boardWidget)
 {
 throw new NotImplementedException();
 }
 public void DeleteBoard(Board board)
 {
 throw new NotImplementedException();
 }
 public IReadOnlyList<Board> ListBoards()
 {
 throw new NotImplementedException();
 }
 public Board LoadBoard(int boardId)
 {
 throw new NotImplementedException();
 }
 public void UpdateBoard(Board board)
 {
 throw new NotImplementedException();
 }
}
```
You also need to register your implementation with the app builder in
MauiProgram.cs. You can add the following line

```Csharp
builder.Services.AddTransient<IBoardRepository,
SqliteBoardRepository>();
```
# Connecting to an SQLite database

As mentioned, an SQLite database is contained within a single file, so
when connecting to the database you need to provide the path to that file.
You can do this through the SqliteConnection class. Note that if you wish
to make use of async/await, you can use the SqliteAsyncConnection class.
Let’s edit your repository class to support opening a connection to your
database.
Add a field for the database connection.

```private readonly SQLiteConnection database;```

Add a constructor to open the connection.

```Csharp
public SqliteBoardRepository(IFileSystem fileSystem)
{
 var dbPath = Path.Combine(fileSystem.AppDataDirectory,
"widgetboard_sqlite.db");
 database = new SQLiteConnection(dbPath);
}
```
Here you make use of the IFileSystem implementation you registered
in the previous section. Then you make use of it to determine where to
store your database file. Finally, you open a connection using the path to
your database file. Note that if the file does not exist, one will be created
for you.

# Mapping Your Models

Sqlite-net provides the ability to define mapping information in your
model classes that will ultimately be used to create your table definition
automatically for you. There is a rich set of options ranging from setting
a PrimaryKey through to defining if a column has a MaxLength or even if
it needs to be Unique. Open your Board.cs file and make the following
modifications in bold:

```Csharp
using SQLite;
namespace WidgetBoard.Models;
public class Board
{
 [PrimaryKey, AutoIncrement]
 public int Id { get; set; }
 public string Name { get; init; }
 public int NumberOfColumns { get; init; }
 public int NumberOfRows { get; init; }
}
```
You add a new Id column, marking it as the PrimaryKey, and state that
it will AutoIncrement, meaning that Sqlite will manage the id generation
for you.
Your second model class is in the BoardWidget.cs file. This represents
each widget that is placed on the board and where it is positioned.

```Csharp
using SQLite;
namespace WidgetBoard.Models;
public class BoardWidget
{
 [PrimaryKey, AutoIncrement]
 public int Id { get; set; }
 public int BoardId { get; set; }
 public int Position { get; set; }
 public string WidgetType { get; set; }
}
```

# Creating Your Tables

You can inform the Sqlite-net connection to create a table for you. This
can be done by calling the CreateTable<T> method and passing the
appropriate model type. Note that CreateTable is idempotent, so unless
you change your model, calling CreateTable a second time will have
no impact. You can modify your SqliteBoardRepository to call the
CreateTable method in its constructor as follows; changes in bold.
  
```Csharp
public SqliteBoardRepository(IFileSystem fileSystem)
{
 var dbPath = Path.Combine(fileSystem.AppDataDirectory,
"widgetboard_sqlite.db");
 connection = new SQLiteConnection(dbPath);
 connection.CreateTable<Board>();
 connection.CreateTable<BoardWidget>();
}  
```

# Inserting into an SQLite Database

You can now add in the ability to insert a board into your database by
supplying the following implementation into the CreateBoard method:

```Csharp  
public void CreateBoard(Board board)
{
 connection.Insert(board);
}
```
  
# Reading a Collection from an SQLite Database

You only need to return a list of the boards your user has created in the
application.
 
```Csharp
public IReadOnlyList<Board> ListBoards()
{
 return connection.Table<Board>()
 .ToList();
}
```

Perhaps you should consider sorting these boards alphabetically.
Sqlite-net offers a rich set of functionality when querying data in the
database. You can make use of LINQ-based expressions, which gives you
the following (the addition in bold):

```Csharp
public IReadOnlyList<Board> ListBoards()
{
 return connection.Table<Board>()
 .OrderBy(b => b.Name)
 .ToList();
}
```

# Reading a Single Entity from an SQLite Database

When reading a Board from the database, you also need to load any
BoardWidgets that relate to it. For this you can write the following:

```Csharp
public Board LoadBoard(int boardId)
{
 var board = connection.Find<Board>(boardId);
 if (board is null)
 {
 return null;
 }
 var widgets = connection.Table<BoardWidget>().Where(w =>
w.BoardId == boardId).ToList();
 board.BoardWidgets = widgets;
 return board;
}  
```
The first line calling Find allows you to find an entity with the supplied
primary key value. This retrieves the Board. Next, you need to retrieve the
collection of BoardWidgets. This is performed in a very similar manner
to loading your collection of Boards. Finally, you assign the widgets you
loaded into the board before returning it to the caller.
It is worth noting that the sqlite-net-pcl package does not provide more
complex querying operations such as joins. If this is something that you
still require, it is possible to write the SQL directly and execute against
the connection. If you wish to join your Board and BoardWidget tables
together, you can achieve this as follows:

```Csharp
var board = connection.Query<Board>("SELECT B.* FROM Board B
JOIN BoardWidget BW ON BW.BoardId = B.BoardId WHERE B.BoardId =
?", boardId);  
```
Note that the above query is purely aimed at showing how joins work,
it does not provide you with any particularly useful in the context of your
application.

# Deleting from an SQLite Database


  While I haven’t focused on providing this functionality just yet, it is a very
common use case.
  
```Csharp
public void DeleteBoard(Board board)
{
 connection.Delete(board);
}
```
# Updating an Entity in an SQLite Database

While I haven’t focused on providing this functionality just yet, it is a very
common use case.
 
```Csharp
public void UpdateBoard(Board board)
{
 connection.Update(board);
}
```
# LiteDB

LiteDB is a simple, fast, and lightweight embedded .NET document
database. LiteDB was inspired by the MongoDB database and its API is
very similar to the official MongoDB .NET API.

# Installing LiteDB

In order to install and use LiteDB, you need to install the NuGet package
called LiteDB. Don’t worry; it is perfectly fine to install both the LiteDB
and SQLite packages side by side into your project. In fact, that is precisely
what you will do here.
You can do this by following these steps.
1. Right-click the WidgetBoard project.
2. Click Manage NuGet Packages.
3. In the Search field, enter LiteDB.
4. Select the LiteDB package and select Add Package.
5. A confirmation dialog will show. Review and accept
the license details if you are happy

# Using LiteDB

The first step is to create your IBoardRepository implementation. Add a
new class file called LiteDBBoardRepository in your Data folder, and make
it implement your IBoardRepository interface.

```Csharp
using LiteDB;
using WidgetBoard.Models;
namespace WidgetBoard.Data;
public class LiteDBBoardRepository : IBoardRepository
{
 public void CreateBoard(Board board)
 {
 throw new NotImplementedException();
 } 
 public void CreateBoardWidget(BoardWidget boardWidget)
 {
 throw new NotImplementedException();
 }
 public void DeleteBoard(Board board)
 {
 throw new NotImplementedException();
 }
 public IReadOnlyList<Board> ListBoards()
 {
 throw new NotImplementedException();
 }
 public Board LoadBoard(int boardId)
 {
 throw new NotImplementedException();
 }
 public void UpdateBoard(Board board)
 {
 throw new NotImplementedException();
 }
} 
```
You also need to register your implementation with the app
builder in MauiProgram.cs. You can add the following line. Just make
sure that you have removed or commented out the line to register the
SqliteBoardRepository implementation.

```Csharp
builder.Services.AddTransient<IBoardRepository,
LiteDBBoardRepository>();  
```
# Connecting to a LiteDB database

LiteDB stores all its data in a single file on disk, so your first task is to
specify where this file exists so that you can create and open the file for
users within your application. For this part, you will borrow a concept from
a little further ahead in this chapter (the “File System” section).
Edit your repository class to support opening a connection to your
database.
Add a field to hold the database access details.

```Csharp
private readonly LiteDatabase database;  
```
Add a constructor to open the connection.

```Csharp
public LiteDBBoardRepository(IFileSystem fileSystem)
{
 var dbPath = Path.Combine(fileSystem.AppDataDirectory,
"widgetboard_litedb.db");
 database = new LiteDatabase(dbPath);
}  
```
The above should look very similar to the Sqlite way of accessing the
database. Here you make use of the IFileSystem implementation you
registered in the previous section. Then you make use of that to determine
where to store your database file. Finally, you open a connection using the
path to your database file. Note that if the file does not exist, one will be
created for you.

# Mapping Your Models

First, you need to add a field to hold a collection of boards and one for the
collection of board widgets

```Csharp
private readonly ILiteCollection<Board> boardCollection;
private readonly ILiteCollection<BoardWidget>
boardWidgetCollection;
```
Then you need to get access to that collection in order to allow you to
perform your operations against it.

```Csharp
boardCollection = database.GetCollection<Board>("Boards");
boardCollection = database.GetCollection<BoardWidget>("Board
Widgets");
```
The final part of your mapping setup is to define indexing information
about your model. For this you use the EnsureIndex method.

```Csharp
boardCollection.EnsureIndex(b => b.Id, true);
```
In LiteDB, any property that you wish to be unique or want
to query against needs to have a definition provided through the
EnsureIndex method.

# Creating Your Tables
  
You don’t actually need to do anything to create your tables here. The key
difference between LiteDB and other databases that you might use is that
the schema of the data is held with the data.

# Inserting into a LiteDB Database

You can now add in the ability to insert a board into your database by
supplying the following implementation into the CreateBoard method:

```Csharp
public void CreateBoard(Board board)
{
 boardCollection.Insert(board);
}
```

# Reading a Collection from a LiteDB Database
  
 You only need to return a list of the boards your user created in the
application. 
  
```Csharp
public IReadOnlyList<Board> ListBoards()
{
 return boardCollection.Query()
 .ToList();
}  
```
  
Perhaps you should consider sorting these boards alphabetically.
LiteDB offers a similar set of functionality that you looked at with Sqlitenet. LINQ-based expressions can be used to order your boards, which
gives you the following (the addition is in bold):  
  
```Csharp
public IReadOnlyList<Board> ListBoards()
{
 return connection.Table<Board>()
 .OrderBy(b => b.Name)
 .ToList();
}
```
You also need to add the following line to your constructor to make
sure querying is possible:  
  
```Csharp
boardCollection.EnsureIndex(b => b.Name, false);
```
  
# Reading a Single Entity from a LiteDB Database  
  
When reading a Board from the database, you also need to load any
BoardWidgets that relate to it. For this you can write the following:

```Csharp
public Board LoadBoard(int boardId)
{
var board = boardCollection.FindById(boardId);
 var boardWidgets = boardWidgetCollection.Find(w =>
w.BoardId == boardId).ToList();
 board.BoardWidgets = boardWidgets;
 return board;
}
```
The first line calls FindById, which allows you to find an entity with
the supplied primary key value. This retrieves the Board. Next, you need
to retrieve the collection of BoardWidgets. This is performed in a very
similar manner to loading your collection of Boards. Finally, you assign the
widgets you loaded into the board before returning it to the caller.
  
# Deleting from a LiteDB Database

While I haven’t focused on providing this functionality, it is a very common
use case.

```Csharp
public void DeleteBoard(Board board)
{
 boardCollection.Delete(board.Id);
}
```

# Updating an Entity in a LiteDB Database

While I haven’t focused on providing this functionality, it is a very common
use case.  
  
```Csharp
public void UpdateBoard(Board board)
{
 boardCollection.Update(board);
}
```

# Database Summary
  
There is an abundance of options when it comes to choosing not only
which database but then also the ORM layer on top of it. The aim of this
section is to give a taste of what some options offer and to encourage you
to decide which will benefit your application and team most.
Both options I covered provide support for encryption.  
  
---
I strongly encourage you to evaluate which database will provide
you with the best development experience and the users of your
application with the best user experience. Some databases perform
better in different scenarios.
---
  
Moving forward with this application you will continue to use LiteDB.  
  
# Application Settings (Preferences)

Quite often you will want to persist data about your application that you
really do not need a database for. I like to refer to these bits of data as
application settings. If you have previous experience with building .NET
applications, this would be similar to an app.config or appsettings.json
file. The .NET MAUI term is Preferences, though, and this is the API that
you will look at accessing.
An item in Preferences is stored as a key-value pair. The key is a string
and it is recommended to keep the name short in length.
As with all of the other APIs provided by .NET MAUI, you register the
Preferences implementation with the app builder in the MauiProgram.cs
file. You can add the following line into the CreateMauiApp method:  
  
```Csharp
builder.Services.AddSingleton(Preferences.Default);
```

# What Can Be Stored in Preferences?
  
There is a limitation on the type of data that can be stored in Preferences.
The API provides the ability to store the following .NET types:  
  
Boolean

  • Double

  • Int32

  • Single

  • Int64

  • String

  • DateTime

  Having the ability to provide a String value surely means you could
in theory store anything in there, right? While this is technically possible
it is highly recommended that you only store small amounts of text.
Otherwise the performance of storing and retrieval can be impacted in
your applications.  
  
 # Setting a Value in Preferences
  
You can store a value in Preferences through the use of the Set method.
You can provide a key, the value, and also an optional sharedName. The
preferences stored in your application are only visible to that application.
You can also create a shared preference that can be used by other
extensions or a watch application.
A perfect use case for your application is to store the id of the last
accessed board and open it the next time the application loads. Let’s store
the id initially. Inside your FixedBoardPageViewModel class you can make
the following changes.
Add the preferences field. 
  
```Csharp
private readonly IPreferences preferences;
Update the constructor to set the preferences field; changes in bold.
public FixedBoardPageViewModel(
 WidgetTemplateSelector widgetTemplateSelector,
 IPreferences preferences)
{
 WidgetTemplateSelector = widgetTemplateSelector;
 this.preferences = preferences;
 Widgets = new ObservableCollection<IWidgetViewModel>();
}
```
Finally, record the id of the board that was supplied when
navigating to the page. You can do this by adding the bold line to your
ApplyQueryAttributes method:
  
```Csharp
public void ApplyQueryAttributes(IDictionary<string,
object> query)
{
 var board = query["Board"] as Board;
 preferences.Set("LastUsedBoardId", board.Id);
 BoardName = board.Name;
 NumberOfColumns = board.NumberOfColumns;
 NumberOfRows = board.NumberOfRows;
}
```

This means that every time a user opens a board to view it, the id will
be remembered in Preferences. When the application is opened again in
the future, it will use that id to open the last viewed board.  
  
A possible alternative way of achieving this type of functionality could
be to maintain a last opened column in the database and always find the
latest of that set.  
  
# Getting a Value in Preferences
  
You can retrieve a value from Preferences using the Get method. You are
required to supply the key identifying the setting and a default value to be
returned if the key does not exist. You can optionally provide a sharedName,
much like with the Set method covered in the previous section.
You have already written the code to store your LastUsedBoardId in
Preferences so let’s read it back when loading your boards up to display.
Open up your AppShellViewModel.cs file and make the following changes
Add the preferences field.  
  
```Csharp
private readonly IPreferences preferences;
```
Set the preferences field in the constructor; changes in bold.  
  
```Csharp
public AppShellViewModel(
 IBoardRepository boardRepository,
 IPreferences preferences)
{
 this.boardRepository = boardRepository;
 this.preferences = preferences;
}
```
Update your LoadBoards method to support navigating to the last used
board; changes in bold.  
  
```Csharp
public void LoadBoards()
{
 var boards = this.boardRepository.ListBoards();
var lastUsedBoardId = preferences.
Get("LastUsedBoardId", -1);
 Board lastUsedBoard = null;
 foreach (var board in boards)
 {
 Boards.Add(board);
 if (lastUsedBoardId == board.Id)
 {
 lastUsedBoard = board;
 }
 }
 if (lastUsedBoard is not null)
 {
 Dispatcher.GetForCurrentThread().Dispatch(() =>
 {
 BoardSelected(lastUsedBoard);
 });
 }
}
```
There are a few new concepts here, so let’s break them down in to
understandable chunks.
Use of the preferences.Get method, as you learned about before
writing the above code. You supply the key name and the default value to
be returned if the key does not exist. You use -1 for the default because it is
not a valid id for a database key.
The final new concept is the use of Dispatcher. This allows you to
trigger a deferred action and make sure that it is dispatched onto the UI
thread. Your method will be called on the UI thread, but you want the
OnAppearing logic to finish before you attempt to navigate somewhere, by  
calling Dispatcher.GetForCurrentThread().Dispatch you are queuing
up an action to be performed once the UI thread is no longer busy. .NET
MAUI does handle a lot of dispatching for you when you trigger updates
in bindings, but there are times when you need to make sure that you are
updating things on the UI thread.
If you run your code now, you can create a new board and view it once
saved. If you then close and reopen the application, you will see that the
board you created is now shown for you. Providing an experience like
this can go a long way to an enjoyable user experience (UX) as they are
returning to where they were previously.  
  
# Checking if a Key Exists in Preferences
  
There can be times when you are unable to supply a suitable default
value to the Get method in order to know whether a value has been
set, for example using a Boolean. false is a valid value and therefore the
default value would not be able to distinguish whether it was set as false
or the default value of false. In this scenario, you can make use of the
ContainsKey method. So instead of writing
  
```Csharp
var lastUsedBoardId = preferences.Get("LastUsedBoardId", -1);
you could have first checked whether the key existed, like
if (preferences.ContainsKey("LastUsedBoardId"))
{
 // Perform your logic
}
```

# Removing a Preference

There may be times when you need to remove an option from the
Preferences store or even remove all options. If you want to remove your
LastUsedBoardId preference, you can write
  
```Csharp
Preferences.Remove("LastUsedBoardId");
```
  
If you want to remove all options, you can write  

```Csharp
Preferences.Clear();  
```
# Secure Storage  
  
When building an application, there will quite often be an occasion where
you need to store an API token or some form of data that needs to be held
securely. .NET MAUI provides another API that makes sure that the values
you supply are held securely on each of the platforms’ secure storage
locations.
As always with a new API provided by .NET MAUI, you must register it
with the MauiAppBuilder in your MauiProgram.cs file, so let’s open up that
file and add the following line into the CreateMauiApp method:  
  
```Csharp
builder.Services.AddSingleton(SecureStorage.Default);
```
This will allow you to declare a dependency on ISecureStorage in
your class constructors and have it provided for you.  
  
# Storing a Value Securely  
  
You don’t currently have a need to write a secure value just yet. It will
follow in the next chapter, but to give a brief explanation of this type of
local data, you can take a look at an example.  
To save a value in secure storage with the key of apiToken and a value
of 1234567890, you can write the following:  
  
```Csharp
await SecureStorage.Default.SetAsync("apiToken", "1234567890");
```
# Reading a Secure Value
  
It is also possible to retrieve the value you have stored securely by using the
GetAsync method and passing in the key. It is worth noting that if the key
does not exist, the method will return null.
To retrieve a value in secure storage with the key of apiToken, you can
write the following:  
  
```Csharp
string apiToken = await SecureStorage.Default.
GetAsync("apiToken");
if (apiToken is not null)
{
}
```

# Removing a Secure Value  
  
As with Preferences, you can remove and remove all secure values.
To remove a specific value, remove the key:
  
```Csharp
bool success = SecureStorage.Default.Remove("apiToken");  
```  
To remove all values, use the RemoveAll method:  
  
```Csharp
SecureStorage.Default.RemoveAll();
```

# Platform specifics  
  
As mentioned, the SecureStorage API makes use of each of the platformspecific APIs to handle the actual storage of the data you pass in. It is worth
noting that the implementations for each individual platform are different
and may change in the operating systems but SecureStorage will leverage
whatever is in the operating system and therefore will always be the most
secure option. This section explains how.  
  
# Android  
  
The data you pass in is encrypted with the Android
EncryptedSharedPreferences class, from the Android Security library,
which automatically encrypts keys and values using a two-scheme
approach:
1. Keys are deterministically encrypted, so that the key
can be encrypted and properly looked up.
2. Values are non-deterministically encrypted using
AES-256 GCM.
The Android Security library provides an implementation of the
security best practices related to reading and writing data at rest, as well as
key creation and verification.
Since Google introduced Android 6.0 (API level 23), the operating
system offers the ability to back up the user’s data. This includes the
Preferences and also the SecureStorage that .NET MAUI offers. It is
entirely possible and in fact I recommend that you disable this backup
functionality when using SecureStorage.
In order to disable the auto backup feature, you need to set the
android:allowBackup to false in the AndroidManifest.xml file under the
Platforms/Android folder. The resulting change should look something
like the following  
  
```xml
<manifest ... >
 ...
 <application android:allowBackup="false" ... >
 ...
 </application>
</manifest>
```

# iOS and macOS

Data passed into SecureStorage on iOS and macOS is encrypted through
the Keychain API. To quote Apple,
The keychain is the best place to store small secrets, like passwords and cryptographic keys. You use the functions of the
keychain services API to add, retrieve, delete, or modify
keychain items.
For further reading, refer to the Apple documentation at https://
developer.apple.com/documentation/security/certificate_key_and_
trust_services/keys/storing_keys_in_the_keychain.
  
---
In some cases, keychain data is synchronized with iCloud, and
uninstalling the application may not remove the secure values from
user devices. I have certainly observed this in some applications I
have built, so it is best to plan around this possibility.
---
  
# Windows  
  
SecureStorage on Windows uses the DataProtectionProvider class to
encrypt values securely. The .NET MAUI implementation allows for the
data to be protected against the local user or computer account.
For further reading, refer to the Microsoft documentation at  
  
 https://docs.microsoft.com/uwp/api/windows.security.cryptography.dataprotection.dataprotectionprovider?view=winrt-22621 
  
 # Viewing the Result 
  
Now when running your application you will see that not only does the last
board that you create get loaded back up but it also shows the widgets you
previously added. Figure 9-2 shows an example of the results. 
  
![image](https://user-images.githubusercontent.com/26972859/231465061-eb1feeb6-21be-4190-bd77-7f49ff7fa6c1.png)
Figure 9-2. The application loads back up and shows the previously
added widgets  
  
# Summary  
  
In this chapter, you

  • Learned about the different types of local data

  • Discovered what .NET MAUI offers in terms of local file
storage locations and when to use each one

  • Gained an understanding of database technologies and
applied two different options

  • Modified your application to save and load the boards
your users create

  • Gained an understanding of the options for storing
small bits of data or Preferences

  • Added the ability to record the last opened board

  • Gained an understanding of the options for storing
small bits of data securely or SecureStorage

  In the next chapter, you will

  • Learn about remote data.

  • Learn how you can interact with it.

  • Cover the common considerations.

  • Look a concrete example with the Open Weather API.

  • Build your own implementation to consume the Open Weather API.

  • Cover how to consume the data returned.

  • Talk through scenarios where things can go wrong.

  • Provide implementations to handle those scenarios.

  • Look at how you can reduce the complexity of your implementation with Refit.

  • Add in your Weather Widget.  
  
# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch09.
  
# Extra Assignment

You have provided the ability to users to add widgets to their boards
and automatically save them so when they next load the board it will be
remembered for them. I would like to see if you can add the ability to
remove the widgets from the board and the database.  
  
  
  
  
  
  
  
  
  
  
  
