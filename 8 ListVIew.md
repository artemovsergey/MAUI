# ListView и работа с данными

## ListView

ListView представляет очень мощный элемет управления .NET MAUI, который позволяет отображать список объектов и при этом кастомизировать их отображение.

ListView связывается с набором данных через свойство ItemsSource, которое принимает объект IEnumerable<T>.

## Создание ListView в коде C#

Определим в коде C# объект ListView, который выводит массив строк:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public StartPage()
    {
        Label header = new Label{ Text = "Список пользователей" };
        // данные для ListView
        string[] people = new string[] { "Tom", "Bob", "Sam", "Alice" };
 
        ListView listView = new ListView();
        // определяем источник данных
        listView.ItemsSource = people;
        Content = new StackLayout { Children = { header, listView }, Padding=7};
    }
}
```

!(https://metanit.com/sharp/maui/pics/8.1.png)

При выводе каждого объекта ListView по умолчанию вызывает для него метод ToString(), поэтому мы увидим строковое отображение объекта.

## ListView в XAML

Аналогичный массив в XAML мы могли бы вывести следующим образом:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout>
        <Label Text="Список пользователей"/>
        <ListView>
            <ListView.ItemsSource>
                <x:Array Type="{x:Type x:String}">
                    <x:String>Tom</x:String>
                    <x:String>Bob</x:String>
                    <x:String>Sam</x:String>
                    <x:String>Alice</x:String>
                </x:Array>
            </ListView.ItemsSource>
        </ListView>
    </StackLayout>
</ContentPage>
```

Здесь напрямую определяется массив строк для свойства ListView.ItemsSource. Однако обычно данные определяются вне, например, в виде ресурса:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="people" Type="{x:Type x:String}">
                <x:String>Tomas</x:String>
                <x:String>Bob</x:String>
                <x:String>Sam</x:String>
                <x:String>Alice</x:String>
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout>
        <Label Text="Список пользователей"/>
        <ListView ItemsSource="{StaticResource people}" />
    </StackLayout>
</ContentPage>
```

## Сочетание XAML и C#. Привязка к списку

Нередко источник данных для ListView определяется в коде C#. И в этом случае мы можем использовать привязку к источнику данных. Например, в коде C# у страницы MainPage определим список строк, к которому будет производиться привязка:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public List<string> Users { get; set; }
    public MainPage()
    {
        InitializeComponent();
        Users = new List<string> { "Tom", "Bob", "Sam", "Alice" };
        BindingContext = this;
    }
}
```

Стоит отметить, что источник данных определен как публичное свойство Users. Другой важный момент - установка контекста привязки страницы: BindingContext = this;. Таким образом, в XAML мы можем обратиться ко всем публичным свойствам страницы.

В коде XAML страницы MainPage пропишем привязку к этому свойству:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout>
        <Label Text="Список пользователей"/>
        <ListView ItemsSource="{Binding Users}" />
    </StackLayout>
</ContentPage>
```

## Обработка выбора элемента

Когда пользователь нажимает на элемент списка, он выделяется, а у ListView срабатывают два события: ItemTapped и ItemSelected. Между ними есть различия. Так, повторное нажатие на один и тот же элемент не вызовет повторного события ItemSelected, так как элемент остается выбанным. А вот событие ItemTapped будет срабатывать именно столько раз сколько пользователь нажал на него, даже повторно. Также событие ItemSelected будет вызвано, если с элемента будет снято выделение.

### ItemSelected

Обработаем выделение элемента. Для этого определим следующий код в MainPage.xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="7">
        <Label x:Name="selected"/>
        <ListView x:Name="usersList" ItemSelected="usersList_ItemSelected" />
    </StackLayout>
</ContentPage>
```

А в файле кода MainPage.xaml.cs определим источник данных и обработчик события ItemSelected:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        usersList.ItemsSource = new List<string> { "Tom", "Bob", "Sam", "Alice" };
    }
    private void usersList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        selected.Text = $"Выбрано: {e.SelectedItem}";
    }
}
```

При выборе элемента мы можем получить выбранный элемент через свойство SelectedItem объекта SelectedItemChangedEventArgs, который передается в качестве параметра.

Поскольку список содержит массив строк, то каждый его элемент представляет строку, которую мы можем получить с помощью метода e.SelectedItem.ToString().

!(https://metanit.com/sharp/maui/pics/8.2.png)

## ItemTapped

Похожим образом можно обработать событие ItemTapped. Определим в XAML следующий код:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="7">
        <Label x:Name="selected"/>
        <ListView x:Name="usersList" ItemTapped="usersList_ItemTapped" />
    </StackLayout>
</ContentPage>
```

А в файле кода MainPage.xaml.cs определим источник данных и обработчик события ItemTapped:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        usersList.ItemsSource = new List<string> { "Tom", "Bob", "Sam", "Alice" };
    }
    private void usersList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        selected.Text = $"Нажато: {e.Item}";
    }
}
```

### Привязка к выбранному элементу

Обработчики событий нажатия и выбора элементы хороши, если нам надо выполнить некоторую логику, связанную с этим элементом. Однако если нам надо просто вывести выбранный элемент в метку, то мы можем просто установить привязку к выбранному в ListView элементу. В примере выше мы вполне могли бы обойтись без обработки события и просто установить привязку элемента Label к выбранному объекту списка. Выбранный элемент в ListView хранится в свойстве SelectItem:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="users" Type="{x:Type x:String}">
                <x:String>Tom</x:String>
                <x:String>Bob</x:String>
                <x:String>Sam</x:String>
                <x:String>Alice</x:String>
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="7">
        <Label Text="{Binding Source={Reference usersList}, Path=SelectedItem}" />
        <ListView x:Name="usersList" ItemsSource="{StaticResource users}" />
    </StackLayout>
</ContentPage>
```

В данном случае свойство Text метки привязано к свойству SelectedItem элемента usersList. И при изменении выбранного элемента в ListView метка также изменить свой текст.

!(https://metanit.com/sharp/maui/pics/8.3.png)

Установка привязке в коде C#:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public StartPage()
    {
        Label header = new Label();
 
        ListView usersList = new ListView();
        // определяем источник данных
        usersList.ItemsSource = new string[] { "Tomas", "Bob", "Sam", "Alice" };
        // установка привязки к SelectedItem
        header.SetBinding(Label.TextProperty, new Binding { Source= usersList, Path ="SelectedItem" });
        Content = new StackLayout { Children = { header, usersList }, Padding=7};
    }
}
```

## Высота строк

Свойство RowHeight позволяет задать высоту строки в ListView. Но действовать оно будет только в том случае, если другое свойство в ListView - HasUnevenRows равно false (значение по умолчанию).

Установка в XAML:

```xml
<ListView RowHeight="100" >
```

В коде C#:

```Csharp
listView.RowHeight = 100;
```

## DataTemplate и сложные объекты в ListView

В прошлой теме был рассмотрен вывод в ListView простейших данных - массива строк. На самом деле в простейшем варианте ListView получает для каждого объекта списка стоковое представление и выводит его в отдельной ячейке. Однако, как правило, данные, которыми манипулируют приложения, представляют более сложные по структуре объекты, которые могут содержать множество свойств. И для организации отображения каждого объекта класс ListView определяет свойство ItemTemplate или шаблон данных. Этому свойству передаестя объект DataTemplate, который устанавливает, как объект будет отображаться в списке.

Рассмотрим, как выполнять привязку к списку объектов на примере ListView. Для создания объектов определим класс User:

```Csharp
public class User
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
```

Теперь определим следующий класс страницы:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public List<User> Users { get; set; }
    public StartPage()
    {
        Label header = new Label { Text = "Список пользователей", FontSize = 18 };
        // определяем данные
        Users = new List<User>
        {
            new User {Name="Tom", Age=38 },
            new User {Name = "Bob", Age= 42},
            new User {Name="Sam", Age = 28},
            new User {Name = "Alice", Age = 33}
        };
        ListView usersListView = new ListView();
        // определяем источник данных
        usersListView.ItemsSource = Users;
        // определяем шаблон данных
        usersListView.ItemTemplate = new DataTemplate(() =>
        {
            // привязка к свойству Name
            Label nameLabel = new Label { FontSize = 16 };
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            // привязка к свойству Age
            Label ageLabel = new Label { FontSize = 14 };
            ageLabel.SetBinding(Label.TextProperty, "Age");
 
            // создаем объект ViewCell.
            return new ViewCell
            {
                View = new StackLayout
                {
                    Padding = new Thickness(0, 5),
                    Orientation = StackOrientation.Vertical,
                    Children = { nameLabel, ageLabel }
                }
            };
        });
        Content = new StackLayout { Children = { header, usersListView }, Padding=7};
    }
}
```

С помощью свойства ItemsSource в качестве источника данных устанавливаем список объектов User.

Свойство ItemTemplate определяет, как эти данные будут отображаться. ItemTemplate представляет объект DataTemplate. Его конструктор принимает делегат Func<object>, который устанавливает, как данные будут располагаться и как будет выглядеть ячейка. Содержимое DataTemplate представляет одну из ячеек, то есть класс Cell. В данном случае это тип ViewCell, поэтому делегат возвращает объект ViewCell, который определяет структуру строки в ListView.

Внутри ViewCell внутренние элементы упавляются контейнером StackLayout, а отдельные элементы Label внутри StackLayout привязаны к отдельным свойствам объекта User.

!(https://metanit.com/sharp/maui/pics/8.4.png)

## DataTemplate в Xaml

Определение DataTemplate в коде XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="users" Type="{x:Type local:User}">
                <local:User Name="Tom" Age="38" />
                <local:User Name="Bob" Age="42" />
                <local:User Name="Sam" Age="28" />
                <local:User Name="Alice" Age="33" />
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="7">
        <Label Text="Список пользователей" FontSize="18" />
        <!--<Label Text="{Binding Source={Reference usersList}, Path=SelectedItem}" />-->
        <ListView ItemsSource="{StaticResource users}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <ViewCell.View>
                            <StackLayout>
                                <Label Text="{Binding Name}" FontSize="16" />
                                <Label Text="{Binding Age}" FontSize="14" />
                            </StackLayout>
                        </ViewCell.View>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

Здесь практически тоже самое. Для простоты набор данных определен в виде ресурса users непосредственно в XAML, хотя также можно было бы задать список объектов User в коде C#. Для определения визуального шаблона выводимых данных применяется объект DataTemplate, в котором через элемент ViewCell устанавливается формат отображения - в виде вертикального стека элементов Label, каждый из которых привязан к одному из свойств объекта User:

```xml
<DataTemplate>
   <ViewCell>
      <ViewCell.View>
        <StackLayout>
           <Label Text="{Binding Name}" FontSize="16" />
           <Label Text="{Binding Age}" FontSize="14" />
        </StackLayout>
      </ViewCell.View>
   </ViewCell>
</DataTemplate>
```

## Получение выделенного элемента

В этом случае может возникнуть вопрос, а как же обрабатывать выбор подобных сложных данных? В случае выше каждый элемент в ListView будет представлять объект User, поэтому свойство SelectedItem будет представлять выбранный объект User. Обращаясь к свойствам этого элемента, мы можем привязать элементы интерфейса к отдельным свойствам выделенного объекта. Например, в примере выше для XAML изменим код StackLayout:

```xml
<StackLayout Padding="7">
    <Label Text="{Binding Source={Reference usersListView}, Path=SelectedItem.Name, StringFormat='Selected: {0}'}"
            FontSize="18"/>
    <ListView x:Name="usersListView" ItemsSource="{StaticResource users}">
        <ListView.ItemTemplate>
            <DataTemplate>
                <ViewCell>
                    <ViewCell.View>
                        <StackLayout>
                            <Label Text="{Binding Name}" FontSize="16" />
                            <Label Text="{Binding Age}" FontSize="14" />
                        </StackLayout>
                    </ViewCell.View>
                </ViewCell>
            </DataTemplate>
          </ListView.ItemTemplate>
    </ListView>
</StackLayout>
```

Здесь текст метки привязан к свойству Name выделенного элемента:

```xml
<Label Text="{Binding Source={Reference usersListView}, Path=SelectedItem.Name, StringFormat='Selected: {0}'}"
```

Причем при привязке будет идти форматирование.

!(https://metanit.com/sharp/maui/pics/8.5.png)

Если бы мы просто указали бы "SelectedItem", которая метка пыталась получить строковое представление объекта и отобразить его

```xml
<Label Text="{Binding Source={Reference usersListView}, Path=SelectedItem}"
```

Аналогичный пример установки привязки в коде C#:


```xml
namespace HelloApp;
 
class StartPage : ContentPage
{
    public List<User> Users { get; set; } = new();
    public StartPage()
    {
        // определяем данные
        Users = new List<User>
        {
            new User {Name="Tom", Age=38 },
            new User {Name = "Bob", Age= 42},
            new User {Name="Sam", Age = 28},
            new User {Name = "Alice", Age = 33}
        };
        ListView usersListView = new ListView();
        // определяем источник данных
        usersListView.ItemsSource = Users;
 
        // определяем шаблон данных
        usersListView.ItemTemplate = new DataTemplate(() =>
        {
            // привязка к свойству Name
            Label nameLabel = new Label { FontSize = 16 };
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            // привязка к свойству Age
            Label ageLabel = new Label { FontSize = 14 };
            ageLabel.SetBinding(Label.TextProperty, "Age");
 
            // создаем объект ViewCell.
            return new ViewCell
            {
                View = new StackLayout
                {
                    Padding = new Thickness(0, 5),
                    Orientation = StackOrientation.Vertical,
                    Children = { nameLabel, ageLabel }
                }
            };
        });
        Label header = new Label { FontSize = 18 };
        // определяем привязку к выбранному элементу
        Binding userBinding = new Binding { StringFormat = "Selected: {0}", Path="SelectedItem.Name", Source = usersListView };
        header.SetBinding(Label.TextProperty, userBinding);
        Content = new StackLayout { Children = { header, usersListView }, Padding=7};
    }
}
```

## Обработка выбора элемента

При нажатии или выделении элемента в списке в обработчиках событий ItemTapped и ItemSelected нажатый/выделенный объект будет представлять тип объектов списка, то есть в случаях выше типа User. Например, определим в C# следующую страницу:

```Csharp
using Microsoft.Maui.Controls.Internals;
 
namespace HelloApp;
 
class StartPage : ContentPage
{
    public List<User> Users { get; set; } = new();
    Label selectedItemHeader = new Label { FontSize = 18 };
    Label tappedItemHeader = new Label { FontSize = 18 };
    public StartPage()
    {
        // определяем данные
        Users = new List<User>
        {
            new User {Name="Tom", Age=38 },
            new User {Name = "Bob", Age= 42},
            new User {Name="Sam", Age = 28},
            new User {Name = "Alice", Age = 33}
        };
        ListView usersListView = new ListView();
        // определяем источник данных
        usersListView.ItemsSource = Users;
 
        // определяем шаблон данных
        usersListView.ItemTemplate = new DataTemplate(() =>
        {
            // привязка к свойству Name
            Label nameLabel = new Label { FontSize = 16 };
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            // привязка к свойству Age
            Label ageLabel = new Label { FontSize = 14 };
            ageLabel.SetBinding(Label.TextProperty, "Age");
 
            // создаем объект ViewCell.
            return new ViewCell
            {
                View = new StackLayout
                {
                    Padding = new Thickness(0, 5),
                    Orientation = StackOrientation.Vertical,
                    Children = { nameLabel, ageLabel }
                }
            };
        });
        usersListView.ItemSelected += UsersListView_ItemSelected;
        usersListView.ItemTapped += UsersListView_ItemTapped;
        Content = new StackLayout { Children = { selectedItemHeader, tappedItemHeader, usersListView }, Padding=7};
    }
 
    private void UsersListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var tappedUser = e.Item as User;
        if (tappedUser != null) 
            tappedItemHeader.Text = $"Нажато: {tappedUser.Name}";
    }
 
    private void UsersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedUser = e.SelectedItem as User;
        if (selectedUser != null)
            selectedItemHeader.Text = $"Выбрано: {selectedUser.Name}";
    }
}
```

В обработчиках событий мы можем привести объект, для которого сработало событие, к его изначальному типу:

```Csharp
// событие выделения объекта
var selectedUser = e.SelectedItem as User;
// событие нажатия объекта
var tappedUser = e.Item as User;
```

Для целей тестирования обработчиков события здесь определены две отдельные метки, которые выводят данные нажатого элемента:

!(https://metanit.com/sharp/maui/pics/8.6.png)

## TextCell

В прошлой теме для настройки вывода списка в ListView использовался объект ViewCell. Этот объект представляет ячейку, которую мы можем настроить по своему усмотрению, опеределить в ней различные элементы. Однако кроме ViewCell в ListView для отображения данных можно применять еще несколько типов ячеек:

- TextCell: выводит заголовок и некоторое детальное описание

- ImageCell: выводит рядом с заголовком и детальным описанием изображение

Для простых случаев, когда для каждого объекта в списке необходимо вывести заголовок и некоторую аннотацию к нему, можно использовать класс TextCell. Его основные свойства, которые могут нам пригодиться:

- Text: основной текст, выводится большим шрифтом

- Detail: детальное описание, выводится меньшим шрифтом

- TextColor: цвет текста

- DetailColor: цвет детального описания

Пусть у нас определен в проекте класс User

```Csharp
public class User
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
```

Используем TextCell для отображения одного объекта User:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="users" Type="{x:Type local:User}">
                <local:User Name="Tom" Age="38" />
                <local:User Name="Bob" Age="40" />
                <local:User Name="Sam" Age="28" />
                <local:User Name="Alice" Age="35" />
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="7">
        <Label Text="Список пользователей" FontSize="18" TextColor="#004D40"/>
        <ListView x:Name="usersListView" ItemsSource="{StaticResource users}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <TextCell
                        Text="{Binding Name}"
                        Detail="{Binding Age, StringFormat='{0} лет'}"
                        TextColor="#004D40"
                        DetailColor="#26A69A"
                        />
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

В данном случае определяем список объектов User в виде ресурса и передаем в ListView на отображение. В TextCell в качестве основного текста отображается имя пользователя, а в качестве детального описания - возраст с форматированием:

!(https://metanit.com/sharp/maui/pics/8.7.png)

Таким образом, для вывода каких-то простых данных TextCell подходит, но естественно возможностей по кастомизации меньше, чем в случае с ViewCell.

Аналогичный пример в коде C#:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public StartPage()
    {
        ListView usersListView = new ListView();
        // определяем источник данных
        usersListView.ItemsSource = new List<User>
        {
            new User {Name="Tom", Age=38 },
            new User {Name = "Bob", Age= 40},
            new User {Name="Sam", Age = 28},
            new User {Name = "Alice", Age = 35}
        };
        // определяем шаблон данных
        usersListView.ItemTemplate = new DataTemplate(() =>
        {
            // создаем объект TextCell
            TextCell textCell = new TextCell 
            { 
                TextColor = Color.FromArgb("#01579B"), 
                DetailColor = Color.FromArgb("#0288D1") 
            };
            // привязка к свойству Name
            textCell.SetBinding(TextCell.TextProperty, "Name");
            // привязка к свойству Age
            Binding ageBinding = new Binding { Path = "Age", StringFormat = "{0} лет" };
            textCell.SetBinding(TextCell.DetailProperty, ageBinding);
            return textCell;
        });
 
        Label header = new Label { FontSize = 18, Text = "Список пользователей" };
        Content = new StackLayout { Children = { header, usersListView }, Padding=7};
    }
}
```

## Изображения в ListView. ImageCell и ViewCell

Для отображения изображений в ListView в простых случаях мы можем использовать класс ImageCell, а в более сложных можно воспользоваться ViewCell.

Класс ImageView расширяет класс TextCell, добавляя свойство ImageSource, которое указывает на источник изображения.

Например, пусть у нас есть класс Language, который представляет язык программирования:

```Csharp
class Language
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImagePath { get; set; } = "";
}
```

Свойство Name представляет название языка программирования, Description - некоторое краткое описание, а ImagePath - путь к изображению.
Ранее в теме Работа с изображениями. Элемент Image уже рассматривалось, как добавлять изображения проект и выводить их на страницу. И аналогичным образом добавим в проект в папку Resources/Images несколько изображений, которые представляют логотипы языков программирования:

!(https://metanit.com/sharp/maui/pics/8.9.png)

Теперь определим класс страницы, которая будет выводить эти изображения через ImageCell:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public StartPage()
    {
        ListView listView = new ListView();
        // определяем источник данных
        listView.ItemsSource = new List<Language>
        {
            new Language {Name="C#", ImagePath="csharp_sm.jpg", Description = "Последняя версия C# 11" },
            new Language {Name="C++", ImagePath="cpp_sm.jpg", Description = "Последняя версия C++23" },
            new Language {Name="Java", ImagePath="java_sm.jpg", Description = "Последняя версия Java 19" },
            new Language {Name="Python", ImagePath="python_sm.png", Description = "Последняя версия Python 3.11" },
        };
        // определяем шаблон данных
        listView.ItemTemplate = new DataTemplate(() =>
        {
            ImageCell imageCell = new ImageCell 
            { 
                TextColor = Color.FromArgb("#455A64"), 
                DetailColor = Color.FromArgb("#90A4AE"),
            };
            imageCell.SetBinding(ImageCell.TextProperty, "Name");
            imageCell.SetBinding(ImageCell.DetailProperty, "Description");
            imageCell.SetBinding(ImageCell.ImageSourceProperty, "ImagePath");
            return imageCell;
        });
 
        Label header = new Label { FontSize = 18, Text = "Языки программирования" };
        Content = new StackLayout { Children = { header, listView }, Padding=7};
    }
}
```

По сравнению с прошлой темой в класс Phone было добавлено свойство ImagePath, которое хранит путь к изображению в проекте. К этому свойству осуществляется привязка свойства ImageSource объекта ImageCell.

!(https://metanit.com/sharp/maui/pics/8.8.png)

Аналогичный пример в Xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="langs" Type="{x:Type local:Language}">
                <local:Language Name="C#" ImagePath="csharp_sm.jpg" Description="Последняя версия C# 11" />
                <local:Language Name="C++" ImagePath="cpp_sm.jpg" Description="Последняя версия C++23" />
                <local:Language Name="Java" ImagePath="java_sm.jpg" Description="Последняя версия Java 19" />
                <local:Language Name="Python" ImagePath="python_sm.png" Description="Последняя версия Python 3.11" />
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="7">
        <Label Text="Языки программирования" FontSize="18" TextColor="#004D40"/>
        <ListView ItemsSource="{StaticResource langs}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ImageCell
                        ImageSource="{Binding ImagePath}"
                        Text="{Binding Name}"
                        Detail="{Binding Description}"
                        TextColor="#004D40"
                        DetailColor="#26A69A"
                    />
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

## Вывод изображения в ViewCell

Хотя ImageCell и позволяет вывести изображения, ViewCell в данном случае имеет преимущество - при выводе элементов списка через ViewCell можно задать у элемента Image явным образом высоту и ширину:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="langs" Type="{x:Type local:Language}">
                <local:Language Name="C#" ImagePath="csharp_sm.jpg" Description="Последняя версия C# 11" />
                <local:Language Name="C++" ImagePath="cpp_sm.jpg" Description="Последняя версия C++23" />
                <local:Language Name="Java" ImagePath="java_sm.jpg" Description="Последняя версия Java 19" />
                <local:Language Name="Python" ImagePath="python_sm.png" Description="Последняя версия Python 3.11" />
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="7">
        <Label Text="Языки программирования" FontSize="18" TextColor="#004D40"/>
        <ListView ItemsSource="{StaticResource langs}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <StackLayout Orientation="Horizontal" Margin="5">
                            <Image Source="{Binding ImagePath}" WidthRequest="30" HeightRequest="30" />
                            <StackLayout>
                                <Label Text="{Binding Name}" FontSize="16" TextColor="#004D40" />
                                <Label Text="{Binding Description}" TextColor="#26A69A" FontSize="13" />
                            </StackLayout>
                        </StackLayout>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

!(https://metanit.com/sharp/maui/pics/8.10.png)

## Создание класса ячейки для ListView

Если визуальное представление, используемое для вывода данных в ListView, повторяется и применяется в отдельных частях приложения, то мы можем вынести это представление в отдельный класс, который унаследован от ViewCell.

Например, необходимо, чтобы каждая ячейка отображала данные абстрактного товара и была наподобие ImageCell, только с возможностью устанавливать высоту и ширину изображения.

Для этого добавим в проект новый класс CustomCell, который будет представлять отдельную ячейку:

```Csharp
public class CustomCell : ViewCell
{
    Label titleLabel, detailLabel;
    Image image;
 
    public CustomCell()
    {
        titleLabel = new Label { FontSize = 18 };
        detailLabel = new Label();
        image = new Image();
 
        StackLayout cell = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Margin = new Thickness(5, 8)
        };
 
        StackLayout titleDetailLayout = new StackLayout { Margin = 5 };
        titleDetailLayout.Children.Add(titleLabel);
        titleDetailLayout.Children.Add(detailLabel);
 
        cell.Children.Add(image);
        cell.Children.Add(titleDetailLayout);
        View = cell;
    }
 
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create("Title", typeof(string), typeof(CustomCell), "");
 
    public static readonly BindableProperty ImagePathProperty =
        BindableProperty.Create("ImagePath", typeof(ImageSource), typeof(CustomCell), null);
 
    public static readonly BindableProperty ImageWidthProperty =
        BindableProperty.Create("ImageWidth", typeof(int), typeof(CustomCell), 100);
 
    public static readonly BindableProperty ImageHeightProperty =
        BindableProperty.Create("ImageHeight", typeof(int), typeof(CustomCell), 100);
 
    public static readonly BindableProperty DetailProperty =
        BindableProperty.Create("Detail", typeof(string), typeof(CustomCell), "");
 
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
 
    public int ImageWidth
    {
        get { return (int)GetValue(ImageWidthProperty); }
        set { SetValue(ImageWidthProperty, value); }
    }
    public int ImageHeight
    {
        get { return (int)GetValue(ImageHeightProperty); }
        set { SetValue(ImageHeightProperty, value); }
    }
 
    public ImageSource ImagePath
    {
        get { return (ImageSource)GetValue(ImagePathProperty); }
        set { SetValue(ImagePathProperty, value); }
    }
 
    public string Detail
    {
        get { return (string)GetValue(DetailProperty); }
        set { SetValue(DetailProperty, value); }
    }
 
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
 
        if (BindingContext != null)
        {
            titleLabel.Text = Title;
            detailLabel.Text = Detail;
            image.Source = ImagePath;
            image.WidthRequest = ImageWidth;
            image.HeightRequest = ImageHeight;
        }
    }
}
```

Ячейка будет состоять из двух элементов Label, расположенных вертикально, и элемента Image.

Для получения данных извне и привязки данных к элементам Label и Image определены свойства BindableProperty.

Для обработки изменения контекста привязки переопределен метод OnBindingContextChanged.

Пусть данные описываются следующим классом Language, который представляет язык программирования:

```Csharp
class Language
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImagePath { get; set; } = "";
}
```

Добавим в проект в папку Resources/Images несколько изображений, которые представляют логотипы языков программирования:

!(https://metanit.com/sharp/maui/pics/8.9.png)

Определим в коде C# страницу, которая будет выводить список:

```Csharp
namespace HelloApp;
class StartPage : ContentPage
{
    public StartPage()
    {
        ListView listView = new ListView();
        // определяем источник данных
        listView.ItemsSource = new List<Language>
        {
            new Language {Name="C#", ImagePath="csharp_sm.jpg", Description = "Последняя версия C# 11" },
            new Language {Name="C++", ImagePath="cpp_sm.jpg", Description = "Последняя версия C++23" },
            new Language {Name="Java", ImagePath="java_sm.jpg", Description = "Последняя версия Java 19" },
            new Language {Name="Python", ImagePath="python_sm.png", Description = "Последняя версия Python 3.11" },
        };
        // определяем шаблон данных
        listView.ItemTemplate = new DataTemplate(() =>
        {
            CustomCell customCell = new CustomCell { ImageHeight = 40, ImageWidth = 40 };
            customCell.SetBinding(CustomCell.TitleProperty, "Name");
            customCell.SetBinding(CustomCell.DetailProperty, "Description");
            customCell.SetBinding(CustomCell.ImagePathProperty, "ImagePath");
            return customCell;
        });
 
        Label header = new Label { FontSize = 18, Text = "Языки программирования" };
        Content = new StackLayout { Children = { header, listView }, Padding=7};
    }
}
```

!(https://metanit.com/sharp/maui/pics/8.11.png)

Использование CustomCell в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="langs" Type="{x:Type local:Language}">
                <local:Language Name="C#" ImagePath="csharp_sm.jpg" Description="Последняя версия C# 11" />
                <local:Language Name="C++" ImagePath="cpp_sm.jpg" Description="Последняя версия C++23" />
                <local:Language Name="Java" ImagePath="java_sm.jpg" Description="Последняя версия Java 19" />
                <local:Language Name="Python" ImagePath="python_sm.png" Description="Последняя версия Python 3.11" />
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="7">
        <Label Text="Языки программирования" FontSize="18" />
        <ListView ItemsSource="{StaticResource langs}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <local:CustomCell
                        ImagePath="{Binding ImagePath}"
                        ImageWidth="40"
                        ImageHeight="40"
                        Title="{Binding Name}"
                        Detail="{Binding Description}"/>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

## ObservableCollection

При работе со списком объектов мы можем столкнуться с проблемой добавления или удаления объектов в этом списке. Так, допусти, у нас данные представлены классом User, который представляет пользователя:

```Csharp
public class User
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
```

Определим на странице элемент ListView для вывода списка объектов User и поля для добавления одного объекта в список:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public List<User> Users { get; set; }
    Entry nameEntry, ageEntry;
 
    public StartPage()
    {
        // определяем данные
        Users = new List<User>
        {
            new User {Name="Tom", Age=38 },
            new User {Name = "Bob", Age = 42}
        };
        ListView listView = new ListView();
        // определяем источник данных
        listView.ItemsSource = Users;
        // определяем шаблон данных
        listView.ItemTemplate = new DataTemplate(() =>
        {
            // привязка к свойству Name
            Label nameLabel = new Label { FontSize = 16 };
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            // привязка к свойству Age
            Label ageLabel = new Label { FontSize = 14 };
            ageLabel.SetBinding(Label.TextProperty, "Age");
 
            // создаем объект ViewCell.
            return new ViewCell
            {
                View = new StackLayout
                {
                    Padding = new Thickness(0, 5),
                    Orientation = StackOrientation.Vertical,
                    Children = { nameLabel, ageLabel }
                }
            };
        });
 
        // поля для добавления нового объекта User
        nameEntry = new Entry { Placeholder = "Enter name", Margin=5 };
        ageEntry = new Entry { Placeholder = "Enter age", Margin=5 };
        Button saveButton = new Button { Text = "Save", WidthRequest=100, Margin=5, HorizontalOptions = LayoutOptions.Start };
        saveButton.Clicked += SaveButton_Clicked;
        StackLayout form = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Children = { nameEntry, ageEntry, saveButton }
        };
        Content = new StackLayout { Children = { form, listView }, Padding=7};
    }
 
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        int.TryParse(ageEntry.Text, out var age);
        Users.Add(new User { Name = nameEntry.Text, Age = age });
        nameEntry.Text = ageEntry.Text = "";
    }
}
```

На странице определена форма для добавления в список Users. В данном случае типом коллекции Users является стандартный класс List, который поддерживает добавление с помощью методов Add(). По нажатию кнопки срабатывает обработчик SaveButton_Clicked, в котором берем из текстовых полей введенные значения, на их основе создаем объект User и добавляем его в список. Мы ожидаем, что после добавления нового пользователя в список Users также обновится и ListView.

!(https://metanit.com/sharp/maui/pics/8.12.png)

Однако после добавления ListView не будет отображать добавленные объекты.

Чтобы решить эту проблемы в качестве типа коллекции, как правило, используется не класс List, а класс ObservableCollection из пространства имен System.Collections.ObjectModel. За счет реализации интерфейса INotifyCollectionChanged при добавлении или удалении объектов в ObservableCollection автоматически будут изменяться все привязанные к этой коллекции объекты, в том числе и ListView.

Итак, изменим тип коллекции Users:

```Csharp
using System.Collections.ObjectModel;
 
namespace HelloApp;
 
class StartPage : ContentPage
{
    public ObservableCollection<User> Users { get; set; }
    Entry nameEntry, ageEntry;
 
    public StartPage()
    {
        // определяем данные
        Users = new ObservableCollection<User>
        {
            new User {Name="Tom", Age=38 },
            new User {Name = "Bob", Age = 42}
        };
        ListView listView = new ListView();
        // определяем источник данных
        listView.ItemsSource = Users;
        // определяем шаблон данных
        listView.ItemTemplate = new DataTemplate(() =>
        {
            // привязка к свойству Name
            Label nameLabel = new Label { FontSize = 16 };
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            // привязка к свойству Age
            Label ageLabel = new Label { FontSize = 14 };
            ageLabel.SetBinding(Label.TextProperty, "Age");
 
            // создаем объект ViewCell.
            return new ViewCell
            {
                View = new StackLayout
                {
                    Padding = new Thickness(0, 5),
                    Orientation = StackOrientation.Vertical,
                    Children = { nameLabel, ageLabel }
                }
            };
        });
 
        // поля для добавления нового объекта User
        nameEntry = new Entry { Placeholder = "Enter name", Margin=5 };
        ageEntry = new Entry { Placeholder = "Enter age", Margin=5 };
        Button saveButton = new Button { Text = "Save", WidthRequest=100, Margin=5, HorizontalOptions = LayoutOptions.Start };
        saveButton.Clicked += SaveButton_Clicked;
        StackLayout form = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Children = { nameEntry, ageEntry, saveButton }
        };
        Content = new StackLayout { Children = { form, listView }, Padding=7};
    }
 
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        int.TryParse(ageEntry.Text, out var age);
        Users.Add(new User { Name = nameEntry.Text, Age = age });
        nameEntry.Text = ageEntry.Text = "";
    }
}
```

Теперь все будет нормально добавляться

!(https://metanit.com/sharp/maui/pics/8.13.png)

## Пример в xaml

Аналогичный пример в xaml. В коде страницы MainPage.xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="7">
        <StackLayout Orientation="Vertical">
            <Entry x:Name="nameEntry" Margin="5" Placeholder = "Enter name" />
            <Entry x:Name="ageEntry" Margin="5" Placeholder = "Enter age" />
            <Button Clicked="SaveButton_Clicked" Text="Save" WidthRequest="100" HorizontalOptions="Start" />
        </StackLayout>
        <ListView ItemsSource="{Binding Users}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <ViewCell.View>
                            <StackLayout>
                                <Label Text="{Binding Name}" FontAttributes="Bold" />
                                <Label Text="{Binding Age}" />
                            </StackLayout>
                        </ViewCell.View>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

В файл кода MainPage.xaml.cs добавим к странице обработчики кнопок и привязку к списку :

```Csharp
using System.Collections.ObjectModel;
 
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public ObservableCollection<User> Users { get; set; }
    public MainPage()
    {
        InitializeComponent();
        Users = new ObservableCollection<User>
        //Users = new List<User>
        {
            new User {Name="Tom", Age=38 },
            new User {Name = "Bob", Age = 42}
        };
        BindingContext = this; // привязка к текущему объекту
    }
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        int.TryParse(ageEntry.Text, out var age);
        Users.Add(new User { Name = nameEntry.Text, Age = age });
        nameEntry.Text = ageEntry.Text = "";
    }
}
}
```

## Заголовок и футер в ListView

С помощью свойств Header и Footer у ListView можно задать соответственно заголовок и футер. В качестве значения мы можем задать обычные строки:

```Csharp
namespace HelloApp;
class StartPage : ContentPage
{
    public StartPage()
    {
        ListView listView = new ListView();
        listView.RowHeight = 25;
        listView.Header = "Список пользователей";
        listView.Footer = "январь 2023";
        // определяем источник данных
        listView.ItemsSource = new List<string>() { "Tom", "Sam", "Bob", "Alice" };
        Content = new StackLayout { Children = { listView }, Padding=7};
    }
}
```

Аналог в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="users" Type="{x:Type x:String}">
                <x:String>Tom</x:String>
                <x:String>Bob</x:String>
                <x:String>Sam</x:String>
                <x:String>Alice</x:String>
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="7">
        <ListView ItemsSource="{StaticResource users}"
                  Header="Список пользователей" Footer="январь 2023" RowHeight="25" />
    </StackLayout>
</ContentPage>
```

Но в данном случае мы увидим небольшие надписи в начале и в конце списка:

!(https://metanit.com/sharp/maui/pics/8.14.png)

## Настройка шаблона для заголовка и футера

Однако ListView позволяет задать шаблон для их отображения. Эти свойства принимают визуальные элементы, которые будут выполнять роль заголовка или футера. Например:

```Csharp
namespace HelloApp;
class StartPage : ContentPage
{
    public StartPage()
    {
        ListView listView = new ListView();
        listView.RowHeight = 25;
        listView.Header = new Label{Text= "Список пользователей", FontSize=18};
        listView.Footer = new VerticalStackLayout {
            Children = {
                new Label { Text = "METANIT.COM", FontSize = 12 },
                new Label { Text = "январь 2023", FontSize = 12 },
            }
        };
        // определяем источник данных
        listView.ItemsSource = new List<string>() { "Tom", "Sam", "Bob", "Alice" };
        Content = new StackLayout { Children = { listView }, Padding=7};
    }
}
```

Здесь в качестве заголовка выступает метка Label, а в качестве футера - элемент StackLayout, который, в свою очередь, содержит две метки.

!(https://metanit.com/sharp/maui/pics/8.15.png)

Аналогичный пример в XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <x:Array x:Key="users" Type="{x:Type x:String}">
                <x:String>Tom</x:String>
                <x:String>Bob</x:String>
                <x:String>Sam</x:String>
                <x:String>Alice</x:String>
            </x:Array>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="7">
        <ListView ItemsSource="{StaticResource users}" RowHeight="25">
            <ListView.Header>
                <Label Text= "Список пользователей" FontSize="18" />
            </ListView.Header>
            <ListView.Footer>
                <VerticalStackLayout>
                    <Label FontSize="12" Text="METANIT.COM" />
                    <Label FontSize="12" Text="январь 2023" />
                </VerticalStackLayout>
            </ListView.Footer>
        </ListView>
    </StackLayout>
</ContentPage>
```

## Группировка в ListView

Элемент ListView в Xamarin поддерживает возможности группировки. Рассмотрим, как мы можем сгруппировать элементы в списке.

Допустим, наши данные представлены классом User:

```Csharp
public class User
{
    public string Name { get; set; } = "";
    public string Company { get; set; } = "";
}
```

Класс User определяет два свойства: для имени и для компании пользователя. Так как компания может совпадать у разных пользователей, то можно по этому признаку сгруппировать объекты User.

Для группировки в начале добавим в проект вспомогательный класс, который назовем Grouping:

```Csharp
using System.Collections.ObjectModel;
 
namespace HelloApp;
 
public class Grouping<K, T> : ObservableCollection<T>
{
    public K Name { get; private set; }
    public Grouping(K name, IEnumerable<T> items) : base(items)
    {
         Name = name;
    }
}
```

Класс Grouping типизирован двумя параметрами. Параметр K представляет тип ключа группы, который будет храниться в свойстве Name. А параметр T представляет тип объектов, которые будут храниться в коллекции Items. Это свойство-коллекция унаследовано от базового класса ObservableCollection. А в конструкторе мы получаем все необходимые данные.

В коде страницы MainPage.xaml.cs создадим список групп:

```Csharp
using System.Collections.ObjectModel;
 
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    // список групп, к которым идет привязка
    public ObservableCollection<Grouping<string, User>> UserGroups { get; set; }
    public MainPage()
    {
        InitializeComponent();
 
        // начальные данные
        var users = new List<User>
        {
                new User {Name="Tom", Company="Microsoft" },
                new User {Name="Sam", Company="Google" },
                new User {Name="Alice", Company="Microsoft" },
                new User {Name="Bob", Company="JetBrains" },
                new User {Name="Kate", Company="Google" },
        };
        // получаем группы
        var groups = users.GroupBy(p => p.Company).Select(g => new Grouping<string, User>(g.Key, g));
        // передаем группы в UserGroups
        UserGroups = new ObservableCollection<Grouping<string, User>>(groups);
        BindingContext = this;
    }
}
```

В конструкторе переменная users определяет общие данные, по которым создается коллекция групп в виде свойства UserGroups. Группировка в данном случае идет по свойству Company объекта User.

А в коде xaml у MainPage пропишем выражения привязки:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout>
        <ListView  GroupDisplayBinding="{Binding Name}"
              ItemsSource="{Binding UserGroups}"
              IsGroupingEnabled="True">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <Label Text="{Binding Name}" />
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

Привязка ListView здесь идет к свойству UserGroups, которое содержит группы. Установка свойства IsGroupingEnabled="True" добавляет в ListView поддержку групп.

С помошью свойства GroupDisplayBinding можно задать то значение, которое будет отображаться для каждой группы. В нашем случае идет привязка к имени группы, которое представляет критерий группировки.

И после запуска приложения все данные в списке будут сгруппированы по компаниям:

!(https://metanit.com/sharp/maui/pics/8.16.png)

## Настройка шаблона заголовков группы

ListView с помощью свойства GroupHeaderTemplate позволяет настроить шаблон отображения заголовков групп. Для этого изменим разметку xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="5">
        <ListView ItemsSource="{Binding UserGroups}" IsGroupingEnabled="True">
            <ListView.GroupHeaderTemplate>
                <DataTemplate>
                    <ViewCell>
                        <Label Text="{Binding Name}"
                               BackgroundColor="LightGray"
                                FontSize="18"
                                FontAttributes="Bold" />
                    </ViewCell>
                </DataTemplate>
            </ListView.GroupHeaderTemplate>
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <Label Text="{Binding Name}" />
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

Свойство GroupHeaderTemplate получает шаблон DataTemplate, где мы можем настроить отображение. В данном случае заголовок будет представлять элемент Label, который привязан к названию группы:

!(https://metanit.com/sharp/maui/pics/8.17.png)

## Стратегии кэширования элементов ListView

Хотя ListView является двольно мощным элементом для отображения данных, он все таки имеет некоторые ограничения. При определении сложного содержимого, сложной структуры ячеек в ListView может ухудшиться скроллинг, поскольку при пролистывании системе нужно будет выполнить множество вычислений. Однако существуют приемы и способы, которые позволят повысить производительность и постараться нивелировать возможный отрицательный эффект при подобных действиях.

Элемент ListView в .NET MAUI опирается на нативные реализации этого элемента на Android, iOS, WinUI, и на каждой из этих платформ реализации ListView имеют встроенные возможности по кэшированию ранее использованных строк. То есть, как правило, в память загружаются только те ячейки ListView, которые в текущий момент видны на экране. Соответственно контент загружается только в эти ячейки. Это позволяет не создавать тысячи объектов, которые номинально имеются в списке, тем самым уменьшая потребление памяти.

В .NET MAUI существуют две стратегии кэширования, которые описываются перечислением ListViewCachingStrategy. Для каждой стратегии в этом перечислении определено соответствующее значение:

- RetainElement

- RecycleElement

### RetainElement

По умолчанию к ListView в .NET MAUI применяется значение RetainElement. Оно означает, что ListView будет создавать ячейку для каждого элемена в списке.

В каких случаях данная стратегия может быть предпочтительной:

- Когда каждая ячейка ListView имеет большое количество привязок (20-30 и более)

- Когда шаблон ячейки часто меняется

- Когда другая стратегия RecycleElement при тех же условиях показывает худшие результаты

### RecycleElement

RecycleElement позволяет повторно использовать одни и те же ячейки, вместо их создания каждый раз, когда они попадают в зону видимости. Эта стратегия может быть предпочтительной в следующих случаях:

- Когда каждая ячейка ListView имеет небольшое количество привязок

- Когда свойство BindingContext ячейки определяет все ее данные

- Когда ячейки во многом аналогичны, а их шаблон остается неизменным

Для установки стратегии кэширования у ListView в XAML применяется атрибут CachingStrategy:

```xml
<ListView CachingStrategy="RecycleElement">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
              ...
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

В коде C# аналогом является передача в конструктор значения ListViewCachingStrategy:

```Csharp
ListView listView = new ListView(ListViewCachingStrategy.RecycleElement);
```