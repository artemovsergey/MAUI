# CollectionView

## Отображение списка с помощью CollectionView

Класс CollectionView предназначен для вывода списков на сложных макетах и предоставляет более гибкую и производительную альтернативу по сравнению с ListView.

Отличия от ListView:

- CollectionView имеет гибкую модель компоновки, позволяя отображать данные вертикально или горизонтально, в виде списка или таблицы.

- CollectionView поддерживает выбор как одного, так и нескольких элементов.

- CollectionView не имеет концепции ячеек, как ListView. Вместо этого для определения внешнего вида каждого элемента используется шаблон данных.

- CollectionView автоматически использует виртуализацию, которая предоставляется нативными нижележащими элементами используемой операционной системы

- CollectionView обладает меньшим API. Многие свойства и события из ListView отсутствуют в CollectionView.

- CollectionView не поддерживает разделители элементов.

- CollectionView сгенерирует исключение, если его свойство ItemsSource будет обновлено вне потока пользовательского интерфейса.

## Определение источника данных

Для определения источника данных применяется свойство ItemsSource, которое в качестве значения принимает коллекцию IEnumerable. Например, определим следующую страницу в коде C#

```Csharp
class StartPage : ContentPage
{
    public StartPage()
    {
        CollectionView collectionView = new CollectionView { Margin=5 };
        collectionView.ItemsSource = new string[] { "Tom", "Sam", "Bob", "Alice", "Kate" }; 
        Content =  collectionView;
    }
}
```

!(https://metanit.com/sharp/maui/pics/10.10.png)

## Установка шаблона данных

За установку шаблона отображения данных отвечает свойство ItemsTemplate, которое представляет объект DataTemplate и которое принимает в качестве значения одноименный объект DataTemplate. Простейший пример:

```Csharp
class StartPage : ContentPage
{
    public StartPage()
    {
        CollectionView collectionView = new CollectionView { Margin=5 };
        // определяем источник данных
        collectionView.ItemsSource = new string[] { "Tom", "Sam", "Bob", "Alice", "Kate" };
        // определяем шаблон данных
        collectionView.ItemTemplate = new DataTemplate(() => 
        {
            var personLbl = new Label { FontSize = 16, TextColor = Color.FromArgb("#1565C0") };
            personLbl.SetBinding(Label.TextProperty, new Binding("BindingContext", source: RelativeBindingSource.Self));
            return personLbl;
        });
         Content =  collectionView;
    }
}
```

Конструктор DataTemplate принимает делегат, который возвращает объект-шаблон элемента списка. В данном случае в качестве шаблона элемента выступает метка personLbl, для которой устанавливаются цвет текста и высота шрифта. Поскольку по умолчанию для объекта-шаблона в качестве контекста привязки (свойство BindingContext) используется отображаемый элемент списка, то для отображения текста в метке используется привязка к свойству BindingContext этой же метки (new Binding("BindingContext", source: RelativeBindingSource.Self)

!(https://metanit.com/sharp/maui/pics/10.11.png)

Аналогичный пример в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage" >
    <CollectionView>
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type x:String}">
                <x:String>Tom</x:String>
                <x:String>Sam</x:String>
                <x:String>Bob</x:String>
                <x:String>Alice</x:String>
                <x:String>Kate</x:String>
            </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <Label Text="{Binding}" FontSize="16" TextColor="#1565C0" />
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

## Определение шаблона для сложных данных

Как правило, объекты списока представляют более комплексные по структуре данные. Например, пусть у нас есть класс Person, который представляет пользователя:

```Csharp
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string Company { get; set; } = "";
}
```

В коде C# определим страницу, которая выводит в CollectionView список пользователей:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public StartPage()
    {
        CollectionView collectionView = new CollectionView();
        // определяем источник данных
        collectionView.ItemsSource = new List<Person>
        {
            new  Person { Name="Tom", Age=38, Company ="Microsoft" },
            new  Person { Name="Sam", Age=25,  Company ="Google" },
            new  Person { Name="Bob", Age=42,  Company ="JetBrains" },
            new  Person { Name="Alice", Age=33,  Company ="Microsoft" },
            new  Person { Name="Kate", Age=29,  Company ="Google" },
            new  Person { Name="Amelia", Age=35,  Company ="JetBrains" },
        };
        // определяем шаблон данных
        collectionView.ItemTemplate = new DataTemplate(() =>
        {
            var nameLabel = new Label { FontSize = 20, TextColor = Color.FromArgb("#006064"), Margin = 10 };
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            var ageLabel = new Label();
            ageLabel.SetBinding(Label.TextProperty, new Binding { Path = "Age", StringFormat = "Возраст: {0}" });
 
            var companyLabel = new Label();
            companyLabel.SetBinding(Label.TextProperty, "Company");
 
            return  new StackLayout
            {
                Children = { nameLabel, ageLabel, companyLabel },
                Margin = new Thickness(15, 10)
            };
        });
        Content =  collectionView ;
    }
}
```

В данном случае в качестве шаблона данных выступает объект StackLayout. И для каждого элемента создается свой StackLayout с тремя метками Label, каждая из которых привязана к определенному свойству объекта Person. На Windows это будет выглядеть так:

!(https://metanit.com/sharp/maui/pics/10.13.png)

Аналогичный пример в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <CollectionView>
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Age="38" Company="Microsoft"/>
                <local:Person Name="Sam" Age="25" Company="Google"/>
                <local:Person Name="Bob" Age="42" Company="JetBrains"/>
                <local:Person Name="Alice" Age="33" Company="Microsoft"/>
                <local:Person Name="Kate" Age="29" Company="Google"/>
                <local:Person Name="Amelia" Age="35" Company="JetBrains" />
            </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Margin="8">
                    <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064" Margin="10" />
                    <Label Text="{Binding Age, StringFormat='Возраст: {0}'}" />
                    <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

## Заголовок и футер CollectionView

С помощью свойств Header и Footer у CollectionView можно задать соответственно заголовок и футер. В качестве значения мы можем задать обычные строки:

Например, в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <CollectionView Header="Список пользователей" Footer="Данные актуальны на январь 2023г.">
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Company="Microsoft" />
                <local:Person Name="Bob" Company="Google" />
                <local:Person Name="Sam" Company="JetBrains" />
                <local:Person Name="Alice" Company="Microsoft" />
                <local:Person Name="Kate" Company="Google" />
            </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Padding="8">
                    <Label Text="{Binding Name}" FontSize="18" TextColor="#006064" />
                    <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

Но в данном случае мы увидим небольшие надписи в начале и в конце списка:

!(https://metanit.com/sharp/maui/pics/10.25.png)

Установка в коде C#:

```Csharp
CollectionView collectionView = new CollectionView();
collectionView.Header = "Список пользователей";
collectionView.Footer = "Январь 2023 г.";
```

Также CollectionView позволяет задать шаблон для их отображения:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <CollectionView>
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Company="Microsoft" />
                <local:Person Name="Bob" Company="Google" />
                <local:Person Name="Sam" Company="JetBrains" />
                <local:Person Name="Alice" Company="Microsoft" />
                <local:Person Name="Kate" Company="Google" />
            </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.Header>
            <Label Text= "Список пользователей" FontSize="18" FontAttributes="Bold" />
        </CollectionView.Header>
        <CollectionView.Footer>
            <Label Text= "Январь 2023г." FontSize="14" FontAttributes="Italic" />
        </CollectionView.Footer>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Padding="8">
                    <Label Text="{Binding Name}" FontSize="18" TextColor="#006064" />
                    <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

!(https://metanit.com/sharp/maui/pics/10.26.png)

## Расположение элементов в CollectionView

Элемент CollectionView по умолчанию располагает элементы вертикально. Однако вообще у нас есть 4 варианта расположения элементов:

- Вертикальный список — список из одного столбца

- Горизонтальный список — список из одной строки, который растет горизонтально по мере добавления новых элементов.

- Вертикальный грид — грид из нескольких столбцов, который растет вертикально по мере добавления новых элементов.

- Горизонтальный грид — грид из нескольких строкк, который увеличивается по горизонтали по мере добавления новых элементов.

С помощью свойства ItemsLayout можно настроить расположение. Это свойство представляет объект IItemsLayout. Обычно это объект класса, производного от ItemsLayout

Этот класс определяет свойство Orientation, которое представляет перечисление ItemsLayoutOrientation. В этом перечислении определены две константы:

- Vertical: элементы располагаются вертикально

- Horizontal: элементы располагаются вертикально

По умолчанию .NET MAUI предоставляет две реализации класса ItemsLayout - LinearItemsLayout (для создания списка) и GridItemsLayout (для создания грида)

## Списки

Для создания списков применяется класс LinearItemsLayout. Он определяет статические поля Vertical и Horizontal для создания вертикальных или горизонтальных списков соответственно. В качестве альтернативы можно создать объект LinearItemsLayout и передать в его конструктор одну из двух констант перечисления ItemsLayoutOrientation.

```Csharp
// вертикальный список
LinearItemsLayout layout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical);
// горизонтальный список
layout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal);
```

Кроме того, класс LinearItemsLayout определяет свойство ItemSpacing типа double, которое представояет пустое пространство вокруг каждого элемента. Значение этого свойства по умолчанию равно 0, и его значение всегда должно быть больше или равно 0.

## Вертикальный список

По умолчанию для CollectionView определяется вертикальный список. И в принципе мы можем также явным образом задать вертикальное расположение. В коде C#:

```Csharp
collectionView.ItemsLayout = LinearItemsLayout.Vertical;
// или так
collectionView.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical);
```

В коде xaml:

```xml
<CollectionView ItemsLayout="VerticalList">
    ...
</CollectionView>
<!-- или так -->
<CollectionView>
    <CollectionView.ItemsLayout>
        <LinearItemsLayout Orientation="Vertical" />
    </CollectionView.ItemsLayout>
    ...
</CollectionView>
```

## Горизонтальный список

Горизонтальный список делается аналогично. В коде C#:

```Csharp
collectionView.ItemsLayout = LinearItemsLayout.Horizontal;
// или так
collectionView.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal);
```

В коде xaml:

```xml
<CollectionView ItemsLayout="HorizontalList">
    ...
</CollectionView>
<!-- или так -->
<CollectionView>
    <CollectionView.ItemsLayout>
        <LinearItemsLayout Orientation="Horizontal" />
    </CollectionView.ItemsLayout>
    ...
</CollectionView>
```

Рассмотрим на примере. Допустим, данные представлены классом Person:

```Csharp
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string Company { get; set; } = "";
}
```

Класс Person определяет три свойства: Name, Age и Company для хранения имени, возраста и компании пользователя соответственно.

Определим в коде C# страницу, которая выводит список подобных объектов:

```Csharp
namespace HelloApp;
class StartPage : ContentPage
{
    public StartPage()
    {
        CollectionView collectionView = new CollectionView();
        collectionView.ItemsLayout = LinearItemsLayout.Horizontal;
        // определяем источник данных
        collectionView.ItemsSource = new List<Person>
        {
            new  Person { Name="Tom", Age=38, Company ="Microsoft" },
            new  Person { Name="Sam", Age=25,  Company ="Google" },
            new  Person { Name="Bob", Age=42,  Company ="JetBrains" },
            new  Person { Name="Alice", Age=33,  Company ="Microsoft" },
            new  Person { Name="Kate", Age=29,  Company ="Google" },
            new  Person { Name="Amelia", Age=35,  Company ="JetBrains" },
        };
        // определяем шаблон данных
        collectionView.ItemTemplate = new DataTemplate(() =>
        {
            var nameLabel = new Label { FontSize = 20, TextColor = Color.FromArgb("#006064"), Margin = 10 };
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            var ageLabel = new Label();
            ageLabel.SetBinding(Label.TextProperty, new Binding { Path = "Age", StringFormat = "Возраст: {0}" });
 
            var companyLabel = new Label();
            companyLabel.SetBinding(Label.TextProperty, "Company");
 
            return  new StackLayout
            {
                Children = { nameLabel, ageLabel, companyLabel },
                Margin = 20
            };
        });
        Content =  collectionView ;
    }
}
```

Здесь шаблон элементов представлен объектом StackLayout, в котором в 3-х метках выводятся значения текущего объекта Person.

С помощью свойства ItemsLayout задаем горизонтальное направление элементов:

```Csharp
collectionView.ItemsLayout = LinearItemsLayout.Horizontal;
```

В итоге элементы будут располагаться в горизонтальный ряд:

!(https://metanit.com/sharp/maui/pics/10.14.png)

Аналогичный код в XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <CollectionView ItemsLayout="HorizontalList">
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Age="38" Company="Microsoft"/>
                <local:Person Name="Sam" Age="25" Company="Google"/>
                <local:Person Name="Bob" Age="42" Company="JetBrains"/>
                <local:Person Name="Alice" Age="33" Company="Microsoft"/>
                <local:Person Name="Kate" Age="29" Company="Google"/>
                <local:Person Name="Amelia" Age="35" Company="JetBrains" />
            </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Margin="10">
                    <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064" Margin="10" />
                    <Label Text="{Binding Age, StringFormat='Возраст: {0}'}" />
                    <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

## Отступы между элементами

С помощью свойства ItemSpacing можно задать отступы между элементами. Например, в xaml:

```xml
<CollectionView>
    <CollectionView.ItemsLayout>
        <LinearItemsLayout Orientation="Horizontal" ItemSpacing="20" />
    </CollectionView.ItemsLayout>
    .........
</CollectionView>
```

Или в коде C#

```Csharp
CollectionView collectionView = new CollectionView();
collectionView.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal){ ItemSpacing = 10 };
```

## Таблицы

Для создания гридов предназначен класс GridItemsLayout, который наследуется от класса ItemsLayout и определяет следующие свойства:

- VerticalItemSpacing: значение типа double, которое представляет пустое пространство вокруг каждого элемента сверху и снизу. По умолчанию равно 0, и это значение всегда должно быть больше или равно 0.

- HorizontalItemSpacing: значение типа double, которое представляет пустое пространство вокруг каждого элемента справа и слева. По умолчанию равно 0, и это значение всегда должно быть больше или равно 0.

- Span: значение типа int, которое представляет количество столбцов или строк, отображаемых в гриде. По умолчанию равно 1 и всегда должно быть больше или равно 1.

Эти свойства представляют BindableProperty, поэтому их можно использовать как цель привязки данных.

Для создания грида необходимо присвоить свойству ItemsLayout объекта CollectionView объект GridItemsLayout c указанием ориентации и количеством строк/столбцов. Например, в коде C#:

```Csharp
CollectionView collectionView = new CollectionView();
// 2 столбца
collectionView.ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical) { Span = 2 };
// 2 строки
collectionView.ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Horizontal) { Span = 2 };
```

В коде xaml можно использовать краткую форму

```xml
<CollectionView ItemsLayout="VerticalGrid, 2">
```

Сначала указывается ориентация - "VerticalGrid" или "HorizontalGrid", а затем количество строк/столбцов

Также можно использовать полную форму:

```xml
<CollectionView>
    <CollectionView.ItemsLayout>
        <GridItemsLayout Orientation="Vertical" Span="2" />
    </CollectionView.ItemsLayout>
    ...........
</CollectionView>
```

## Вертикальный грид

Вертикальный грид представляет расположение элементов в несколько колонок/столбцов. Например, определим в C# вертикальный грид из двух колонок:

```Csharp
namespace HelloApp;
class StartPage : ContentPage
{
    public StartPage()
    {
        CollectionView collectionView = new CollectionView { VerticalOptions = LayoutOptions.Start};
        // 2 столбца
        collectionView.ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical) { Span = 2 };
        // определяем источник данных
        collectionView.ItemsSource = new List<Person>
        {
            new  Person { Name="Tom", Age=38, Company ="Microsoft" },
            new  Person { Name="Sam", Age=25,  Company ="Google" },
            new  Person { Name="Bob", Age=42,  Company ="JetBrains" },
            new  Person { Name="Alice", Age=33,  Company ="Microsoft" },
            new  Person { Name="Kate", Age=29,  Company ="Google" },
            new  Person { Name="Amelia", Age=35,  Company ="JetBrains" },
        };
        // определяем шаблон данных
        collectionView.ItemTemplate = new DataTemplate(() =>
        {
            var nameLabel = new Label { FontSize = 20, TextColor = Color.FromArgb("#006064"), Margin = 10 };
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            var ageLabel = new Label();
            ageLabel.SetBinding(Label.TextProperty, new Binding { Path = "Age", StringFormat = "Возраст: {0}" });
 
            var companyLabel = new Label();
            companyLabel.SetBinding(Label.TextProperty, "Company");
 
            return  new StackLayout
            {
                Children = { nameLabel, ageLabel, companyLabel },
                Margin = 20
            };
        });
        Content =  collectionView ;
    }
}
```

!(https://metanit.com/sharp/maui/pics/10.15.png)

Аналогичный код в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <CollectionView ItemsLayout="VerticalGrid, 2">
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Age="38" Company="Microsoft"/>
                <local:Person Name="Sam" Age="25" Company="Google"/>
                <local:Person Name="Bob" Age="42" Company="JetBrains"/>
                <local:Person Name="Alice" Age="33" Company="Microsoft"/>
                <local:Person Name="Kate" Age="29" Company="Google"/>
                <local:Person Name="Amelia" Age="35" Company="JetBrains" />
            </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Margin="8">
                    <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064" Margin="10" />
                    <Label Text="{Binding Age, StringFormat='Возраст: {0}'}" />
                    <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

## Горизонтальный грид

Определим горизонтальный грид из 3 строк в коде C#:

```Csharp
namespace HelloApp;
class StartPage : ContentPage
{
    public StartPage()
    {
        CollectionView collectionView = new CollectionView { VerticalOptions = LayoutOptions.Start};
        // три строки
        collectionView.ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Horizontal) { Span = 3 };
        // определяем источник данных
        collectionView.ItemsSource = new List<Person>
        {
            new  Person { Name="Tom", Age=38, Company ="Microsoft" },
            new  Person { Name="Sam", Age=25,  Company ="Google" },
            new  Person { Name="Bob", Age=42,  Company ="JetBrains" },
            new  Person { Name="Alice", Age=33,  Company ="Microsoft" },
            new  Person { Name="Kate", Age=29,  Company ="Google" },
            new  Person { Name="Amelia", Age=35,  Company ="JetBrains" },
            new  Person { Name="Mike", Age=36,  Company ="Google" },
        };
        // определяем шаблон данных
        collectionView.ItemTemplate = new DataTemplate(() =>
        {
            var nameLabel = new Label { FontSize = 20, TextColor = Color.FromArgb("#006064")};
            nameLabel.SetBinding(Label.TextProperty, "Name");
 
            var ageLabel = new Label();
            ageLabel.SetBinding(Label.TextProperty, new Binding { Path = "Age", StringFormat = "Возраст: {0}" });
 
            var companyLabel = new Label();
            companyLabel.SetBinding(Label.TextProperty, "Company");
 
            return  new StackLayout
            {
                Children = { nameLabel, ageLabel, companyLabel },
                Margin = 8,
            };
        });
        Content =  collectionView ;
    }
}
```

!(https://metanit.com/sharp/maui/pics/10.12.png)

Аналогичный код в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <CollectionView ItemsLayout="HorizontalGrid, 3" VerticalOptions="Start">
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Age="38" Company="Microsoft"/>
                <local:Person Name="Sam" Age="25" Company="Google"/>
                <local:Person Name="Bob" Age="42" Company="JetBrains"/>
                <local:Person Name="Alice" Age="33" Company="Microsoft"/>
                <local:Person Name="Kate" Age="29" Company="Google"/>
                <local:Person Name="Amelia" Age="35" Company="JetBrains" />
                <local:Person Name="Mike" Age="36" Company="Google"/>
            </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Margin="8">
                    <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064"/>
                    <Label Text="{Binding Age, StringFormat='Возраст: {0}'}" />
                    <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

## Выбор элементов в CollectionView

Для управления выбором элементов класс CollectionView определяет следующие свойства:

SelectionMode: режим выбора, представляет перечисление SelectionMode:

None: элементы нельзя выбрать. Значение по умолчанию

Single: можно выбрать один элемент

Multiple: можно выбрать несколько элементов

SelectedItem: выбранный элемент

SelectedItems: выбранные элементы в списке в виде объекта IList<object>

Рассмотрим получение выбранного элемента. Пусть у нас данные описываются следующим классом Person:

```Csharp
public class Person
{
    public string Name { get; set; } = "";
    public string Company { get; set; } = "";
}
```

В коде C# определим CollectionView и выведем текущий выделенный элемент в метку:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public StartPage()
    {
        CollectionView collectionView = new CollectionView { VerticalOptions = LayoutOptions.Start};
        // устанавливаем режим выбора
        collectionView.SelectionMode = SelectionMode.Single;
        // определяем источник данных
        collectionView.ItemsSource = new List<Person>
        {
            new  Person { Name="Tom", Company ="Microsoft" },
            new  Person { Name="Sam", Company ="Google" },
            new  Person { Name="Bob", Company ="JetBrains" },
            new  Person { Name="Alice", Company ="Microsoft" },
            new  Person { Name="Kate", Company ="Google" },
            new  Person { Name="Amelia", Company ="JetBrains" }
        };
        // определяем шаблон данных
        collectionView.ItemTemplate = new DataTemplate(() =>
        {
            var nameLabel = new Label { FontSize = 20, TextColor = Color.FromArgb("#006064")};
            nameLabel.SetBinding(Label.TextProperty, "Name"); 
 
            var companyLabel = new Label();
            companyLabel.SetBinding(Label.TextProperty, "Company");
 
            return  new StackLayout
            {
                Children = { nameLabel, companyLabel },
                Margin = 8
            };
        });
        // метка для вывода выбранного элемента
        Label selectedLabel= new Label { Margin= 8, FontSize=18 };
        selectedLabel.SetBinding(Label.TextProperty, 
            new Binding { Source = collectionView, Path = "SelectedItem.Name", StringFormat="Selected: {0}" });
        Content = new StackLayout { selectedLabel,  collectionView} ;
    }
}
```

В данном случае установлен режим выбора одного элемента:

```Csharp
collectionView.SelectionMode = SelectionMode.Single;
```

К этому элементу привязана метка над CollectionView. Причем текст метки привязан именно к свойству Name выбранного элемента:

```Csharp
selectedLabel.SetBinding(Label.TextProperty, 
    new Binding { Source = collectionView, Path = "SelectedItem.Name", StringFormat="Selected: {0}" });
```

!(https://metanit.com/sharp/maui/pics/10.16.png)

Аналогичный пример в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <StackLayout>
        <Label FontSize="18" Margin="5"
            Text="{Binding Source={x:Reference collectionView}, Path=SelectedItem.Name, StringFormat='Selected: {0}'}" />
        <CollectionView x:Name="collectionView" SelectionMode="Single">
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Company="Microsoft"/>
                <local:Person Name="Sam" Company="Google"/>
                <local:Person Name="Bob" Company="JetBrains"/>
                <local:Person Name="Alice" Company="Microsoft"/>
                <local:Person Name="Kate" Company="Google"/>
                <local:Person Name="Amelia" Company="JetBrains" />
             </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Margin="8">
                        <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064"  />
                        <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
    </StackLayout>
</ContentPage>
```

## Событие SelectionChanged

Событие SelectionChanged класса CollectionView позволяет отследить изменение выделенных элементов. В качестве параметра это событие принимает объект SelectionChangedEventArgs, который предоставляет два свойства

- CurrentSelection: текущие выделенные элементы

- PreviousSelection: ранее выбранные элементы

Поскольку CollectionView поддерживает множественный выбор, то оба свойства представляют список IReadOnlyList. Рассмотрим обработку этого события и для этого определим в MainPage.xaml следующий код:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <StackLayout>
        <Label x:Name="selectedLabel" FontSize="18" Margin="5" />
        <CollectionView x:Name="collectionView" SelectionMode="Single"
                        SelectionChanged="OnCollectionViewSelectionChanged">
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Company="Microsoft"/>
                <local:Person Name="Sam" Company="Google"/>
                <local:Person Name="Bob" Company="JetBrains"/>
                <local:Person Name="Alice" Company="Microsoft"/>
                <local:Person Name="Kate" Company="Google"/>
                <local:Person Name="Amelia" Company="JetBrains" />
             </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Margin="8">
                        <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064"  />
                        <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
    </StackLayout>
</ContentPage>
```

Здесь для события SelectionChanged установлен обработчик "OnCollectionViewSelectionChanged". Определим этот обработчик в файле MainPage.xaml.cs

```Csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
 
    void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string labelText = "";
        if (e.CurrentSelection.FirstOrDefault() is Person current)
            labelText = $"Current: {current.Name}";
        if (e.PreviousSelection.FirstOrDefault() is Person previous)
            labelText = $"{labelText}\nPrevious: {previous.Name}";
        selectedLabel.Text = labelText;
    }
}
```

В обработчике OnCollectionViewSelectionChanged получаем первый (и единственный) элемент из обоих коллекций и выводим информацию в метку:

!(https://metanit.com/sharp/maui/pics/10.17.png)

## Команда выбора элемента

Также CollectionView предоставляет команду SelectionChangedCommand, которая выполняется при изменении выбранного элемента. А через свойство SelectionChangedCommandParameter класса CollectionView в команду SelectionChangedCommand можно передать какой-нибудь параматр.

Например, в файле MainPage.xaml.cs определим команду, которая будет получать выбранный элемент:

```Csharp
using System.Windows.Input;
 
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public ICommand SelectCommand { get; set; }
    public MainPage()
    {
        InitializeComponent();
        SelectCommand = new Command<Person?>(p =>
        {
            selectedLabel.Text = $"Selected: {p?.Name}";
        });
        BindingContext = this;
    }
}
```

В данном случае команда типизированная типом Person? и получает параметр этого типа - выбранный элемент и выводит его свойство Name в метку.

В файле MainPage.xaml выполним привязку команды:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <StackLayout>
        <Label x:Name="selectedLabel" FontSize="18" Margin="5" />
        <CollectionView x:Name="collectionView" SelectionMode="Single"
                        SelectionChangedCommand="{Binding SelectCommand}"
                        SelectionChangedCommandParameter="{Binding Source={RelativeSource Self}, Path=SelectedItem}">
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Company="Microsoft"/>
                <local:Person Name="Sam" Company="Google"/>
                <local:Person Name="Bob" Company="JetBrains"/>
                <local:Person Name="Alice" Company="Microsoft"/>
                <local:Person Name="Kate" Company="Google"/>
                <local:Person Name="Amelia" Company="JetBrains" />
             </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Margin="8">
                        <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064"  />
                        <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
    </StackLayout>
</ContentPage>
```

Поскольку в коде странице в c# в качестве контекста привязки задана сама страница, то мы можем привязать команду SelectCommand

```Csharp
SelectionChangedCommand="{Binding SelectCommand}"
```

А параметр команды привязан к выбранному элементу в CollectionView:

```Csharp
SelectionChangedCommandParameter="{Binding Source={RelativeSource Self}, Path=SelectedItem}"
```

!(https://metanit.com/sharp/maui/pics/10.18.png)

Установка команды в коде C#:

```Csharp
collectionView.SelectionChangedCommand = new Command<Person?> (p =>
{
    selectedLabel.Text = $"Selected: {p?.Name}";
});
collectionView.SetBinding(CollectionView.SelectionChangedCommandParameterProperty, new Binding("SelectedItem", 
    source: RelativeBindingSource.Self));
```

## Обработка множественного выбора

Для обработки множественного выбора мы можем отслеживать свойство SelectedItems, можем обрабатывать событие SelectionChanged, можем выполнять команду SelectionChangedCommand. Рассмотрим последний вариант. Определим в файле MainPage.xaml определим следующий интерфейс:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <StackLayout>
        <Label x:Name="selectedLabel" FontSize="18" Margin="5" />
        <CollectionView x:Name="collectionView" SelectionMode="Multiple"
                        SelectionChangedCommand="{Binding SelectCommand}"
                        SelectionChangedCommandParameter="{Binding Source={RelativeSource Self}, Path=SelectedItems}">
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Company="Microsoft"/>
                <local:Person Name="Sam" Company="Google"/>
                <local:Person Name="Bob" Company="JetBrains"/>
                <local:Person Name="Alice" Company="Microsoft"/>
                <local:Person Name="Kate" Company="Google"/>
                <local:Person Name="Amelia" Company="JetBrains" />
             </x:Array>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Margin="8">
                        <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064"  />
                        <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
    </StackLayout>
</ContentPage>
```

В данном случае для CollectionView установлен режим множественного выбора, а в команду SelectionChangedCommand передается список SelectedItems.

Определим в MainPage.xaml.cs следующий код:

```Csharp
using System.Windows.Input;
 
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public ICommand SelectCommand { get; set; }
    public MainPage()
    {
        InitializeComponent();
        SelectCommand = new Command<IList<object>>(people =>
        { 
            string text = "Selected: ";
             
            foreach (var p in people)
            {
                if(p is Person person) text = $"{text} {person.Name},";
            }
            selectedLabel.Text = text;
        });
        BindingContext = this;
    }
}
```

Здесь команда SelectCommand получает список объектов, проходит по этому списку, преобразует каждый объект к типу Person и выводит значение свойства Name в метку.

!(https://metanit.com/sharp/maui/pics/10.19.png)

## Программное выделение элементов

Элемент CollectionView поддерживает программную установку выделенных элементов. Для этого его свойству SelectedItem (выбранный элемент) и SelectedItems (список выбранных элементов) можно присвоить соответствующие значение. Также эти свойства поддерживают двустороннюю привязку.

Например, пусть у нас данные представляет класс Person:

```Csharp
public class Person
{
    public string Name { get; set; } = "";
    public string Company { get; set; } = "";
}
```

Пусть CollectionView отображает список объектов Person. Программно выберем второй элемент списка:

```Csharp
class StartPage : ContentPage
{
    public StartPage()
    {
        var people = new List<Person>
        {
            new  Person { Name="Tom", Company ="Microsoft" },
            new  Person { Name="Sam", Company ="Google" },
            new  Person { Name="Bob", Company ="JetBrains" },
            new  Person { Name="Alice", Company ="Microsoft" },
            new  Person { Name="Kate", Company ="Google" },
            new  Person { Name="Amelia", Company ="JetBrains" }
        };
        CollectionView collectionView = new CollectionView { VerticalOptions = LayoutOptions.Start };
        // устанавливаем режим выбора
        collectionView.SelectionMode = SelectionMode.Single;
        // определяем источник данных
        collectionView.ItemsSource = people;
        // выбираем второй элемент
        collectionView.SelectedItem = people[1];
        // определяем шаблон данных
        collectionView.ItemTemplate = new DataTemplate(() =>
        {
            var nameLabel = new Label { FontSize = 20, TextColor = Color.FromArgb("#006064")};
            nameLabel.SetBinding(Label.TextProperty, "Name");
            var companyLabel = new Label();
            companyLabel.SetBinding(Label.TextProperty, "Company");
            return new StackLayout { Children = { nameLabel, companyLabel }, Padding=10 };
        });
         
        Content = collectionView;
    }
}
```

В качестве источника данных выступает список people. А в качестве выделенного элемента - второй элемент этого списка:

```Csharp
collectionView.SelectedItem = people[1];
```

## Выделение элементов через двустороннюю привязку к SelectedItem

Поскольку свойство SelectedItem поддерживает двустороннюю привзку, то мы можем динамический менять значение этого свойства через привязанный объект. Например, определим в файле MainPage.xaml.cs следующую страницу:

```Csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
 
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new ViewModel();
    }
}
public class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    Person? selectedPerson;
    public ObservableCollection<Person> People { get;}
    public ViewModel()
    {
        People = new ObservableCollection<Person>
        {
            new  Person { Name="Tom", Company ="Microsoft" },
            new  Person { Name="Sam", Company ="Google" },
            new  Person { Name="Bob",  Company ="JetBrains" },
            new  Person { Name="Alice", Company ="Microsoft" },
            new  Person { Name="Kate", Company ="Google" },
            new  Person { Name="Amelia", Company ="JetBrains" }
        };
        SelectedPerson = People[1];
    }
    public Person? SelectedPerson
    {
        get => selectedPerson;
        set
        {
            if (selectedPerson != value)
            {
                selectedPerson = value;
                OnPropertyChanged();
            }
        }
    }
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
```

Здесь для страницы в качестве контекста привязки устанавливаливается модель представления ViewModel, которая содержит источник данных - коллекцию People и выбранный элемент - свойство SelectedPerson. По умолчанию в качестве выбранного элемента применяется второй элемент коллекции People:

```Csharp
SelectedPerson = People[1];
```

В файле MainPage.xaml определим следующий интерфейс:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage" >
    <StackLayout>
        <Label Text="{Binding SelectedPerson.Name, StringFormat='Selected: {0}'}" Margin="8" FontSize="20" />
        <CollectionView SelectionMode="Single" ItemsSource="{Binding People}" SelectedItem="{Binding SelectedPerson}" >
            <CollectionView.ItemTemplate>
            <DataTemplate>
                <StackLayout Padding="8">
                    <Label Text="{Binding Name}" FontSize = "20"  TextColor = "#006064"  />
                    <Label Text="{Binding Company}" />
                </StackLayout>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
    </StackLayout>
</ContentPage>
```

Здесь к свойство SelectedItem элемента CollectionView привязано к свойству SelectedPerson модели представления. Соответственно после присвоения свойству SelectedPerson нового значения, также изменится и выделенный элемент в CollectionView.

Для большей показательности над CollectionView также определена метка, текст которой также привязан к свойству SelectedPerson:

!(https://metanit.com/sharp/maui/pics/10.21.png)

## Группировка в CollectionView

Элемент CollectionView поддерживает группировку данных. Для этого класс предоставляет ряд свойств:

- IsGrouped: при значении true будет применяться группировка. Значение по умолчанию - false.

- GroupHeaderTemplate: шаблон DataTemplate для отображения заголовка группы.

- GroupFooterTemplate: шаблон DataTemplate для отображения футтера группы.

Рассмотрим, как мы можем сгруппировать элементы в списке.

Допустим, наши данные представлены классом User:

```Csharp
public class Person
{
    public string Name { get; set; } = "";
    public string Company { get; set; } = "";
}
```

Класс Person определяет два свойства: для имени и для компании пользователя. Так как компания может совпадать у разных пользователей, то можно по этому признаку сгруппировать объекты Person.

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
    public ObservableCollection<Grouping<string, Person>> PeopleGroups { get; set; }
    public MainPage()
    {
        InitializeComponent();
 
        // начальные данные
        var people = new List<Person>
        {
                new Person {Name="Tom", Company="Microsoft" },
                new Person {Name="Sam", Company="Google" },
                new Person {Name="Alice", Company="Microsoft" },
                new Person {Name="Bob", Company="JetBrains" },
                new Person {Name="Kate", Company="Google" },
        };
        // получаем группы
        var groups = people.GroupBy(p => p.Company).Select(g => new Grouping<string, Person>(g.Key, g));
        // передаем группы в PeopleGroups
        PeopleGroups = new ObservableCollection<Grouping<string, Person>>(groups);
        BindingContext = this;
    }
}
```

В конструкторе переменная people определяет общие данные, по которым создается коллекция групп в виде свойства PeopleGroups. Группировка в данном случае идет по свойству Company объекта User.

А в коде xaml у MainPage пропишем выражения привязки:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage" >
    <StackLayout>
        <CollectionView ItemsSource="{Binding PeopleGroups}" IsGrouped="True">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                        <StackLayout Padding="8">
                            <Label Text="{Binding Name}" FontSize="20"  TextColor="#006064"  />
                            <Label Text="{Binding Company}"  />
                    </StackLayout>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </StackLayout>
</ContentPage>
```
Здесь CollectionView привязан к PeopleGroups, которое содержит группы. Установка свойства IsGrouped="True" добавляет в CollectionView поддержку групп.

И после запуска приложения все данные в списке будут сгруппированы по компаниям:

!(https://metanit.com/sharp/maui/pics/10.22.png)

## Настройка шаблона заголовков группы

По умолчанию заголовки групп не отображаются, поэтому посмотрим, как их настроить. Например, отобразим в качестве заголовка название группы. Для этого изменим разметку xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage" >
    <StackLayout>
        <CollectionView ItemsSource="{Binding PeopleGroups}" IsGrouped="True">
            <CollectionView.GroupHeaderTemplate>
                <DataTemplate>
                    <Label Text="{Binding Name}" FontSize="22"  TextColor="#006064" />
                </DataTemplate>
            </CollectionView.GroupHeaderTemplate>
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <StackLayout Padding="8">
                        <Label Text="{Binding Name}" FontSize="18"  />
                    </StackLayout>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </StackLayout>
</ContentPage>
```

Свойство GroupHeaderTemplate получает шаблон DataTemplate, где мы можем настроить отображение. В данном случае заголовок будет представлять элемент Label, который привязан к названию группы:

!(https://metanit.com/sharp/maui/pics/10.23.png)

Подобным образом можно настроить шаблон для отображения футера с помощью свойства GroupFooterTemplate:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <StackLayout>
        <CollectionView ItemsSource="{Binding PeopleGroups}" IsGrouped="True">
            <CollectionView.GroupHeaderTemplate>
                <DataTemplate>
                    <Label Text="{Binding Name}" FontSize="22"  TextColor="#006064" />
                </DataTemplate>
            </CollectionView.GroupHeaderTemplate>
            <CollectionView.GroupFooterTemplate>
                <DataTemplate>
                    <Label Text="{Binding Count, StringFormat='Total employees: {0:D}'}"
                   Margin="0,0,0,10" />
                </DataTemplate>
            </CollectionView.GroupFooterTemplate>
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <StackLayout Padding="8">
                        <Label Text="{Binding Name}" FontSize="18"  />
                    </StackLayout>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </StackLayout>
</ContentPage>
```

Футер также представляет метку, текст которой привязан к свойству Count (поскольку коллекция PeopleGroups представляет тип ObservableCollection, то она имеет свойство Count, которое возвращает количество элементов).

!(https://metanit.com/sharp/maui/pics/10.24.png)