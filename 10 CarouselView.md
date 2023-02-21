# CarouselView

## CarouselView. Создание зацикленного списка

Элемент CarouselView представляет компонент для отображения данных в зацикленном прокручиваемом виде, в котором пользователи могут перемещаться по набору элементов, проводя пальцем по экрану или нажимая на указатель мыши.

Для установки данных у класса CarouselView определено свойство ItemsSource, которое принимает объект IEnumerable, то есть это может быть массив или список List или другой тип коллекций. Если же необходимо динамически добавлять данные в привязанный список, тогда применяется класс ObservableCollection.

Например, пусть данные представленны следующим классом Product, который описывает товар:

```Csharp
class Product
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImagePath { get; set; } = "";
}
```

свойство Name указывает на название товара, свойство Description - на некоторое описание, а ImagePath - на путь к файлу изображений.

Для хранения файлов изображений поместим в в проект в папку Resources/Images несколько изображений-логотипов для разных товаров:

!(https://metanit.com/sharp/maui/pics/10.1.png)

Чтобы отображать объекты CarouselView, необходимо задать шаблон с помощью свойства ItemsTemplate. Данное свойство принимает объект DataTemplate, который определяет шаблн данных.

Например, определим в коде C# следующую страницу:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public StartPage()
    {
 
        CarouselView carouselView = new CarouselView
        {
            VerticalOptions = LayoutOptions.Start
        };
         
        carouselView.ItemsSource = new List<Product>
        {
            new Product {Name="Huawei P50", ImagePath="huaweip50_sm.jpg", Description = "Цена 59000" },
            new Product {Name="iPhone 14", ImagePath="iphone14_sm.jpg", Description = "Цена 65000" },
            new Product {Name="Realme GT2 Pro", ImagePath="realmegt2pro_sm.jpg", Description = "Цена 41000" },
            new Product {Name="Xiaomi 12X", ImagePath="xiaomi12x_sm.jpg", Description = "Цена 53999" },
 
        };
        // определяем шаблон данных
        carouselView.ItemTemplate = new DataTemplate(() =>
        {
            Label header = new Label
            {
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 18
            };
            header.SetBinding(Label.TextProperty, "Name");
 
            Image image = new Image { WidthRequest= 150, HeightRequest = 150};
            image.SetBinding(Image.SourceProperty, "ImagePath");
 
            Label description = new Label { HorizontalTextAlignment = TextAlignment.Center };
            description.SetBinding(Label.TextProperty, "Description");
 
            StackLayout stackLayout = new StackLayout() { header, image, description};
            Frame frame = new Frame();
            frame.Content = stackLayout;
            return frame;
        });
        Content =  carouselView;
    }
}
```

Здесь объекту CarouselView передается список из четырех товаров. Для определения шаблна отображения этих товаров свойству ItemTemplate передается объект DataTemplate. Его конструктор принимает делегат Func<object>. Этот делегат возвращает визуальный компонент, который будет создаваться для каждого элемента списка и который устанавливает, как данные будут располагаться. В качестве визуального компонента может выступать любой стандартный элемент графического интерфейса. В данном случае в качестве такого выступает элемент Frame, который, в свою очередь, располагает вложенные данные в виде вертикального стека.

Вертикальный стек содержит элементы, которые привязаны к различным свойствам отображаемого товара. Сначала идет метка, которая привязана к свойству Name отображаемого объекта Product. Дальше идет элемент Image для отображения картинки товара и затем метка Label для описания товара. пределяет структуру строки в ListView.

Конкретное отображение отличается от устройства. Например, на Windows все элементы растягиваются по ширине окна, а под элементами идет полоса прокрутки.:

!(https://metanit.com/sharp/maui/pics/10.2.png)

На Android одномоменто отображается один элемент, список зациклен, а с помощью свайпа пальцем можно прокручивать список:

!(https://metanit.com/sharp/maui/pics/10.3.png)

Аналогичный пример в коде Xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <VerticalStackLayout Padding="5">
        <CarouselView>
            <CarouselView.ItemsSource>
                <x:Array Type="{x:Type local:Product}">
                    <local:Product Name="Huawei P50" ImagePath="huaweip50_sm.jpg" Description="Цена 59000" />
                    <local:Product Name="iPhone 14" ImagePath="iphone14_sm.jpg" Description="Цена 65000" />
                    <local:Product Name="Realme GT2 Pro" ImagePath="realmegt2pro_sm.jpg" Description="Цена 41000" />
                    <local:Product Name="Xiaomi 12X" ImagePath="xiaomi12x_sm.jpg" Description="Цена 53999" />
                </x:Array>
            </CarouselView.ItemsSource>
            <CarouselView.ItemTemplate>
                <DataTemplate>
                    <Frame>
                        <VerticalStackLayout>
                            <Label Text="{Binding Name}" FontAttributes="Bold" FontSize="18" HorizontalTextAlignment="Center"/>
                            <Image WidthRequest="150" HeightRequest="150" Source="{Binding ImagePath}" />
                            <Label Text="{Binding Description}" HorizontalTextAlignment="Center" />
                        </VerticalStackLayout>
                    </Frame>
                </DataTemplate>
            </CarouselView.ItemTemplate>
        </CarouselView>
    </VerticalStackLayout>
</ContentPage>
```

## IndicatorView

Свойство IndicatorView позволяет установить индикатор перемещения по списку. Так, определим в XAML следующую страницу:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <VerticalStackLayout Padding="5">
        <CarouselView IndicatorView="indicatorView">
            <CarouselView.ItemsSource>
                <x:Array Type="{x:Type local:Product}">
                    <local:Product Name="Huawei P50" ImagePath="huaweip50_sm.jpg" Description="Цена 59000" />
                    <local:Product Name="iPhone 14" ImagePath="iphone14_sm.jpg" Description="Цена 65000" />
                    <local:Product Name="Realme GT2 Pro" ImagePath="realmegt2pro_sm.jpg" Description="Цена 41000" />
                    <local:Product Name="Xiaomi 12X" ImagePath="xiaomi12x_sm.jpg" Description="Цена 53999" />
                </x:Array>
            </CarouselView.ItemsSource>
            <CarouselView.ItemTemplate>
                <DataTemplate>
                    <Frame>
                        <VerticalStackLayout>
                            <Label Text="{Binding Name}" FontAttributes="Bold" FontSize="18" HorizontalTextAlignment="Center"/>
                            <Image WidthRequest="150" HeightRequest="150" Source="{Binding ImagePath}" />
                            <Label Text="{Binding Description}" HorizontalTextAlignment="Center" />
                        </VerticalStackLayout>
                    </Frame>
                </DataTemplate>
            </CarouselView.ItemTemplate>
        </CarouselView>
        <IndicatorView Margin="0, 10, 0, 0" x:Name="indicatorView"
                   IndicatorColor="LightGray"
                   SelectedIndicatorColor="DarkGray"
                   HorizontalOptions="Center" />
    </VerticalStackLayout>
</ContentPage>
```

Значение свойства IndicatorView - объект типа IndicatorView, в данном случае это элемент "indicatorView", который определен на странице после CarouselView.

!(https://metanit.com/sharp/maui/pics/10.4.png)

## Ориентация CarouselView

По умолчанию CarouselView располагает элементы горизонтально. Но также можно расположить список вертикально. Для этого нам надо установить свойство ItemsLayout. В качестве значения это свойство принимает объект LinearItemsLayout. Класс LinearItemsLayout предоставляет свойство Orientation, которое задает направление элементов. Оно представляет перечисление ItemsLayoutOrientation, в котором определены две константы

Horizontal: расположение по горизонтали

Vertical: расположение по вертикали

Пример, вертикальной ориентации в XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <CarouselView>
        <CarouselView.ItemsLayout>
            <LinearItemsLayout Orientation="Vertical" />
        </CarouselView.ItemsLayout>
        <CarouselView.ItemsSource>
            <x:Array Type="{x:Type local:Product}">
                <local:Product Name="Huawei P50" ImagePath="huaweip50_sm.jpg" Description="Цена 59000" />
                <local:Product Name="iPhone 14" ImagePath="iphone14_sm.jpg" Description="Цена 65000" />
                <local:Product Name="Realme GT2 Pro" ImagePath="realmegt2pro_sm.jpg" Description="Цена 41000" />
                <local:Product Name="Xiaomi 12X" ImagePath="xiaomi12x_sm.jpg" Description="Цена 53999" />
            </x:Array>
        </CarouselView.ItemsSource>
        <CarouselView.ItemTemplate>
            <DataTemplate>
                <Frame Margin="20">
                    <VerticalStackLayout VerticalOptions="Center">
                        <Label Text="{Binding Name}" FontAttributes="Bold" FontSize="18" HorizontalTextAlignment="Center"/>
                        <Image WidthRequest="150" HeightRequest="150" Source="{Binding ImagePath}"   />
                        <Label Text="{Binding Description}" HorizontalTextAlignment="Center" />
                    </VerticalStackLayout>
                </Frame>
            </DataTemplate>
        </CarouselView.ItemTemplate>
    </CarouselView>
</ContentPage>
```

!(https://metanit.com/sharp/maui/pics/10.9.png)

Установка вертикальной ориентации в коде C#:

```Csharp
CarouselView carouselView = new CarouselView();
carouselView.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical);
```

## Взаимодействие с CarouselView. Обработка выбора элемента

Рассмотрим, как взаимодействовать с элементами в CarouselView. Пусть данные представленны следующим классом Product, который описывает товар:

```Csharp
class Product
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImagePath { get; set; } = "";
}
```

свойство Name указывает на название товара, свойство Description - на некоторое описание, а ImagePath - на путь к файлу изображений.

Для хранения файлов изображений поместим в в проект в папку Resources/Images несколько изображений-логотипов для разных товаров:

!(https://metanit.com/sharp/maui/pics/10.1.png)

## Текущий элемент

Свойство CurrentItem позволяет получить текущий отображаемый элемент. Это свойство поддерживает двустороннюю привязку и имеет значение null, если никакие данные не отображаются.

Например, определим в коде MainPage.xaml следующую страницу:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <VerticalStackLayout Padding="5">
        <Label Text="{Binding Source={x:Reference carouselView}, Path=CurrentItem.Name}" />
        <CarouselView x:Name="carouselView">
            <CarouselView.ItemsSource>
                <x:Array Type="{x:Type local:Product}">
                    <local:Product Name="Huawei P50" ImagePath="huaweip50_sm.jpg" Description="Цена 59000" />
                    <local:Product Name="iPhone 14" ImagePath="iphone14_sm.jpg" Description="Цена 65000" />
                    <local:Product Name="Realme GT2 Pro" ImagePath="realmegt2pro_sm.jpg" Description="Цена 41000" />
                    <local:Product Name="Xiaomi 12X" ImagePath="xiaomi12x_sm.jpg" Description="Цена 53999" />
                </x:Array>
            </CarouselView.ItemsSource>
            <CarouselView.ItemTemplate>
                <DataTemplate>
                    <Frame>
                        <VerticalStackLayout>
                            <Label Text="{Binding Name}" FontAttributes="Bold" FontSize="18" HorizontalTextAlignment="Center"/>
                            <Image WidthRequest="150" HeightRequest="150" Source="{Binding ImagePath}"   />
                            <Label Text="{Binding Description}" HorizontalTextAlignment="Center" />
                        </VerticalStackLayout>
                    </Frame>
                </DataTemplate>
            </CarouselView.ItemTemplate>
        </CarouselView>
    </VerticalStackLayout>
</ContentPage>
```

Здесь метка верхняя Label привязана к текущему выбранному элементу:

```xml
<Label Text="{Binding Source={x:Reference carouselView}, Path=CurrentItem.Name}" />
```

Стоит отметить, что привязка идет к свойству "Name". Хотя по умолчанию свойство CurrentItem представляет тип object, но .NET MAUI может преобразовать объект в его непосредственный тип - в данном случае тип Product и соответственно получить значение свойства Name.

И при перемещении это значение будет меняться

!(https://metanit.com/sharp/maui/pics/10.6.png)

## Обработка изменения текущего элемента

С помощью события CurrentItemChanged можно отследить выбор нового элемента. В качестве параметра это событие принимает объект типа CurrentItemChangedEventArgs, который имеет два свойства:

- PreviousItem: элемент, который ранее был выбран

- CurrentItem: текущий выбранный элемент

Например, обработаем выбор элемента. Для этого определим в файле MainPage.xaml следующий интерфейс:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <VerticalStackLayout Padding="5">
        <Label x:Name="header" FontSize="18"/>
        <CarouselView x:Name="carouselView" CurrentItemChanged="carouselView_CurrentItemChanged">
            <CarouselView.ItemsSource>
                <x:Array Type="{x:Type local:Product}">
                    <local:Product Name="Huawei P50" ImagePath="huaweip50_sm.jpg" Description="Цена 59000" />
                    <local:Product Name="iPhone 14" ImagePath="iphone14_sm.jpg" Description="Цена 65000" />
                    <local:Product Name="Realme GT2 Pro" ImagePath="realmegt2pro_sm.jpg" Description="Цена 41000" />
                    <local:Product Name="Xiaomi 12X" ImagePath="xiaomi12x_sm.jpg" Description="Цена 53999" />
                </x:Array>
            </CarouselView.ItemsSource>
            <CarouselView.ItemTemplate>
                <DataTemplate>
                    <StackLayout>
                        <Frame BackgroundColor="White">
                            <VerticalStackLayout>
                                <Label Text="{Binding Name}" FontAttributes="Bold" FontSize="18" HorizontalTextAlignment="Center"/>
                                <Image WidthRequest="150" HeightRequest="150" Source="{Binding ImagePath}"   />
                                <Label Text="{Binding Description}" HorizontalTextAlignment="Center" />
                            </VerticalStackLayout>
                        </Frame>
                    </StackLayout>
                </DataTemplate>
            </CarouselView.ItemTemplate>
        </CarouselView>
    </VerticalStackLayout>
</ContentPage>
```

Здесь для события CurrentItemChanged устанавливается обработчик carouselView_CurrentItemChanged. И в файле MainPage.xaml.cs определим данный обработчик :

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
 
    private void carouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        Product? current = e.CurrentItem as Product;
        Product? previous = e.PreviousItem as Product;
        header.Text=$"Current: {current?.Name}  Previous: {previous?.Name}";
    }
}
```

В обработчике получаем текущий и ранее нажатый элементы и выводим их в метке Label.

!(https://metanit.com/sharp/maui/pics/10.7.png)

## Обработка команды CurrentItemChangedCommand

Для получения нового выбранного элемента у CarouselView также можно использовать свойство CurrentItemChangedCommand, которое представляет команду, вызываемую при изменении текущего элемента. Кроме того, с помощью свойства CurrentItemChangedCommandParameter можно передать в команду некоторое значение.

Так, определим в файле MainPage.xaml.cs команду для обработки выбора элемента:

```Csharp
using System.Windows.Input;
 
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public ICommand SelectCommand { get; set; }
    public MainPage()
    {
        InitializeComponent();
        SelectCommand = new Command<Product?>(p =>
        {
            header.Text=$"You have selected: {p?.Name}";
        });
        BindingContext = this;
    }
}
```

В данном случае команда получает выбранный объект Product и отображает его название во всплывающем сообщении. Благодаря тому, что в качестве контекста привязки страницы выступает сама эта страница, то в коде xaml мы сможем выполнить привязку к команде.

А в файле MainPage.xaml определим следующий интерфейс:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <VerticalStackLayout Padding="5">
        <Label x:Name="header" FontSize="18"/>
        <CarouselView CurrentItemChangedCommand="{Binding SelectCommand}"
                      CurrentItemChangedCommandParameter="{Binding Source={RelativeSource Self}, Path=CurrentItem}">
            <CarouselView.ItemsSource>
                <x:Array Type="{x:Type local:Product}">
                    <local:Product Name="Huawei P50" ImagePath="huaweip50_sm.jpg" Description="Цена 59000" />
                    <local:Product Name="iPhone 14" ImagePath="iphone14_sm.jpg" Description="Цена 65000" />
                    <local:Product Name="Realme GT2 Pro" ImagePath="realmegt2pro_sm.jpg" Description="Цена 41000" />
                    <local:Product Name="Xiaomi 12X" ImagePath="xiaomi12x_sm.jpg" Description="Цена 53999" />
                </x:Array>
            </CarouselView.ItemsSource>
            <CarouselView.ItemTemplate>
                <DataTemplate>
                    <Frame>
                        <VerticalStackLayout>
                            <Label Text="{Binding Name}" FontAttributes="Bold" FontSize="18" HorizontalTextAlignment="Center"/>
                            <Image WidthRequest="150" HeightRequest="150" Source="{Binding ImagePath}"   />
                            <Label Text="{Binding Description}" HorizontalTextAlignment="Center" />
                        </VerticalStackLayout>
                    </Frame>
                </DataTemplate>
            </CarouselView.ItemTemplate>
        </CarouselView>
    </VerticalStackLayout>
</ContentPage>
```

Здесь прежде всего устанавливается привязка к команде SelectCommand:

```xml
CurrentItemChangedCommand="{Binding SelectCommand}"
```

А в качестве параметра в команду передается текущий выбранный элемент из свойства CurrentItem:

```xml
CurrentItemChangedCommandParameter="{Binding Source={RelativeSource Self}, Path=CurrentItem}"

```

!(https://metanit.com/sharp/maui/pics/10.8.png)

Аналогичный пример в коде C#:

```Csharp
namespace HelloApp;
class StartPage : ContentPage
{
    public StartPage()
    {
        Label currentLbl = new Label(); // для вывода текущего элемента
        CarouselView carouselView = new CarouselView
        {
            VerticalOptions = LayoutOptions.Start
        };
         
        carouselView.ItemsSource = new List<Product>
        {
            new Product {Name="Huawei P50", ImagePath="huaweip50_sm.jpg", Description = "Цена 59000" },
            new Product {Name="iPhone 14", ImagePath="iphone14_sm.jpg", Description = "Цена 65000" },
            new Product {Name="Realme GT2 Pro", ImagePath="realmegt2pro_sm.jpg", Description = "Цена 41000"},
            new Product {Name="Xiaomi 12X", ImagePath="xiaomi12x_sm.jpg", Description = "Цена 53999" },
 
        };
        // определяем шаблон данных
        carouselView.ItemTemplate = new DataTemplate(() =>
        {
            Label header = new Label
            {
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 18
            };
            header.SetBinding(Label.TextProperty, "Name");
 
            Image image = new Image { WidthRequest= 150, HeightRequest = 150 };
            image.SetBinding(Image.SourceProperty, "ImagePath");
 
            Label description = new Label { HorizontalTextAlignment = TextAlignment.Center };
            description.SetBinding(Label.TextProperty, "Description");
 
            StackLayout stackLayout = new StackLayout() { header, image, description};
            Frame frame = new Frame();
            frame.Content = stackLayout;
            return frame;
        });
 
        // определяем команду и параметр
        carouselView.CurrentItemChangedCommand = new Command<Product?>(p =>
        {
            currentLbl.Text= $"You have selected: {p?.Name}";
        });
        carouselView.SetBinding(CarouselView.CurrentItemChangedCommandParameterProperty, new Binding("CurrentItem", source: RelativeBindingSource.Self));
        Content =  new VerticalStackLayout { currentLbl, carouselView };
    }
}
```

## Кастомная обработка выбора элемента

Стоит отметить, что между операционными системами могут наблюдаться различия в поведении. Например, на Windows на момент написания текущей статьи свойство CurrentItem никак не изменяется, если мы перемещаемся по списку с помощью клавиатуры или наживаем на элементы. Кроме того, может возникнуть необходимость определить свой механизм выбора элемента. Для этого мы можем обращаться непосредственно к элементам внутри CarouselView. Например, обработаем нажатие на элемент. Для этого определим в файле MainPage.xaml.cs следующий код:

```Csharp
using System.Windows.Input;
 
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public ICommand SelectCommand { get; set; }
    public MainPage()
    {
        InitializeComponent();
        SelectCommand = new Command<Product>(async p =>
        {
            await DisplayAlert("Notification", $"You have selected: {p.Name}", "ОK");
        });
        BindingContext = this;
    }
}
```

Здесь в классе страницы определена команда, которая получает нажатый элемент и выводит его название в сообщении.

Теперь определим код MainPage.xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage"
             xmlns:local="clr-namespace:HelloApp">
    <CarouselView x:Name="carouselView"  VerticalOptions="Start">
        <CarouselView.ItemsSource>
            <x:Array Type="{x:Type local:Product}">
                <local:Product Name="Huawei P50" ImagePath="huaweip50_sm.jpg" Description="Цена 59000" />
                <local:Product Name="iPhone 14" ImagePath="iphone14_sm.jpg" Description="Цена 65000" />
                <local:Product Name="Realme GT2 Pro" ImagePath="realmegt2pro_sm.jpg" Description="Цена 41000" />
                <local:Product Name="Xiaomi 12X" ImagePath="xiaomi12x_sm.jpg" Description="Цена 53999" />
            </x:Array>
        </CarouselView.ItemsSource>
        <CarouselView.ItemTemplate>
            <DataTemplate>
                <Frame>
                    <VerticalStackLayout>
                        <Label Text="{Binding Name}" FontAttributes="Bold" FontSize="18" HorizontalTextAlignment="Center"/>
                        <Image WidthRequest="150" HeightRequest="150" Source="{Binding ImagePath}"   />
                        <Label Text="{Binding Description}" HorizontalTextAlignment="Center" />
                    </VerticalStackLayout>
                    <Frame.GestureRecognizers>
                        <TapGestureRecognizer
                                Command="{Binding Source={x:Reference carouselView}, Path=BindingContext.SelectCommand}"
                                CommandParameter="{Binding}"/>
                    </Frame.GestureRecognizers>
                </Frame>
            </DataTemplate>
        </CarouselView.ItemTemplate>
    </CarouselView>
</ContentPage>
```

Здесь каждый отдельный элемент фактически представлен визуальным элементом Frame. и как и в общем случае мы можем установить для этого элемента (как и для других элементов) объект TapGestureRecognizer для обработки нажатия по этого элементу:

```xml
<Frame.GestureRecognizers>
    <TapGestureRecognizer
        Command="{Binding Source={x:Reference carouselView}, Path=BindingContext.SelectCommand}"
        CommandParameter="{Binding}"/>
</Frame.GestureRecognizers>
```

Здесь привязка к команде SelectCommand. Поскольку она определена в контексте элемента CarouselView, то для привязки к ней идет обращение к контексту привязки CarouselView Path=BindingContext.SelectCommand

В качестве параметра в команду будет передаваться текущий элемент (CommandParameter="{Binding}")

Таким образом, по нажатию на элемент будет срабатывать команда SelectCommand:

!(https://metanit.com/sharp/maui/pics/10.5.png)