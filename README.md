В .NET MAUI графический интерфейс состоит из страниц. Каждая страница представляет собой объект класса Page. Страница занимает все пространство экрана. То есть то, что мы видим на экране мобильного устройства или в окне десктопного приложения - это страница. Приложение может иметь одну или несколько страниц.


# Тематический сайт

https://www.telerik.com/blogs/going-desktop-dotnet-maui

https://dotnet.microsoft.com/en-us/learn

https://github.com/dotnet/docs/blob/main/docs/architecture/maui/micro-services.md

https://dotnet.microsoft.com/en-us/learn/dotnet/architecture-guides

https://github.com/dotnet/docs/tree/main/docs/architecture/maui

https://github.com/dotnet-architecture/eshop-mobile-client?WT.mc_id=dotnet-35129-website

MAUIMAN.dev

https://learn.microsoft.com/en-us/training/paths/build-apps-with-dotnet-maui/

https://github.com/VladislavAntonyuk/MauiSamples






# MauiProgram

```Csharp
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		#if DEBUG
		builder.Logging.AddDebug();
		#endif

		// Подключение базы данных SQL Server
		string connection = builder.Configuration.GetConnectionString("SQLite");
		//builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));
		//builder.Services.AddDbContext<UserContext>(options => options.UseSqlite(connection));
        //builder.Services.AddDbContext<DataContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 30))));
        //builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connection));
        builder.Services.AddTransient<UserContext>();
		
	//builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

	//builder.Services.AddSingleton<MainPage>();
	//builder.Services.AddSingleton<MainViewModel>();

        //builder.Services.AddTransient<DetailPage>();
        //builder.Services.AddTransient<DetailViewModel>();
	
	
        return builder.Build();
	}
}


В частности, MauiAppBuilder определяет следующие методы:

- UseMauiApp: устанавливает класс, который и определяет визуальный интерфейс и логику приложения. Данный класс должен представлять тип Microsoft.Maui.IApplication (в данном случае класс App наследуется от класса Application, который реализует интерфейс IApplication)

- ConfigureContainer: устанавливает фабрику провайдера сервисов

- ConfigureAnimations: устанавливает применяемые анимации

- ConfigureEffects: устанавливает глобальные эффекты приложения

- ConfigureFonts: настраивает глобальные шрифты приложения

- ConfigureEssentials: устанавливает ряд настроек приложения, в частности, версионирование и ключ для картографических сервисов

- ConfigureImageSources: переопределяет источник изображений

- ConfigureMauiHandlers: устанавливает кастомные обработчик для событий визуальных элементов

- LoadFromXaml: загружает определение интерфейса из файла xaml

Кроме того, класс MauiAppBuilder имеет ряд свойств:

- Configuration: позволяет управлять конфигурацией приложения

- Services: устанавливает сервисы приложения

- Logging: устанавливает настройки логгирования

```

# Context

```Csharp
public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        //public UserContext(DbContextOptions<UserContext> options) : base(options)
        //{
        //    Database.EnsureCreated();
        //}

        public UserContext()
        { 
            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            // Windows App
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                optionsBuilder.UseSqlite("Data Source=beers.db");
            }

            // Android App
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "beers.db3");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }

        }

    }
```
**Замечание**: Не работает подключение к контексту данных через ```AddDbContext``` в ```MauiProgram.cs```. Работает через
переопределение метода класса ```DbContext``` ```OnConfiguring```. Также строку подключения воспринимает через ```App.config```.
При подключении к ```PostgreSQL``` важен регистр названий таблиц и атрибутов.

# Behavior and Event

Package: ```Community.Toolkit.Maui```

```xml
<ContentPage
    x:Class="CommunityToolkit.Maui.Sample.Pages.MyPage"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit">
</ContentPage>
```

В файле MauiProgram.cs поключить ```.UseMauiCommunityToolkit()```

```Csharp
var builder = MauiApp.CreateBuilder();
builder
    .UseMauiApp<App>()
    .UseMauiCommunityToolkit()
    .ConfigureFonts(fonts =>
    {
	fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
	fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    });
```

```xml
<Button>
    <Button.Behaviors>
	<toolkit:EventToCommandBehavior
	EventName="Clicked"
	Command="{Binding TestCommand}" />
    </Button.Behaviors>
</Button>
```



# ViewModel for CommunityToolkit.Mvvm

**Замечание**: подключение пакета ```CommunityToolkit.Mvvm```

MauiProgram.cs

```Csharp
.UseMauiCommunityToolkit()
```


```Csharp
public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        string test;

        [RelayCommand]
        void SayText()
        {
            Test = "text1";
        }
    }
```

**Замечание**: команда будет называться ```SayTextCommand```, даже если метод будет называться SayTextAsync.


# Переход на новую страницу Page

```Csharp
MainPage page = new MainPage();
App.Current.MainPage = page;
```


# Развертывание приложения Android на телефоне

- подписывание приложения 

```powershell
keytool -genkey -v -keystore myapp.keystore -alias MyAppkey -keyalg RSA -keysize 2048 -validity 10000
```

- публикация

```cmd
dotnet publish -f:net7.0-android -c:Release /p:AndroidSigningKeyPass=password /p:AndroidSigningStorePass=password
```

https://www.telerik.com/blogs/publishing-dotnet-maui-app-android

# Переключение на исполняемый файл exe вместо msix

- Properties/launchSettings.json

```json
{
  "profiles": {
    "Windows Machine": {
      "commandName": "Project",
      "nativeDebugging": false
    }
  }
}
```

- Project.csproj

```xml
<WindowsPackageType>None</WindowsPackageType>
```

# Базовая ViewModel

```Csharp
public abstract class ViewModel : INotifyPropertyChanged
    {
        #region Реализация интерфейса NotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            
        }

        // разрешить кольцевые обновления свойств без зацикливания
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
        #endregion
    }
```

# Реализация команды ViewModel

```Csharp
  #region Свойство PersonAge
  private int _personAge;
  public virtual int PersonAge
  {
      get { return _personAge; }

      set
      {
          Set(ref _personAge, value);
          ((Microsoft.Maui.Controls.Command)AddCommand).ChangeCanExecute();  
      }
  }
  #endregion
```

```Csharp
  #region Команда AddCommand
  public ICommand AddCommand { get; set; }
  #endregion
```

```Csharp
  AddCommand = new Microsoft.Maui.Controls.Command(
  () =>
  {
      People.Add(new Person() { Name = PersonName, Age = PersonAge });
  },
  () =>  PersonAge > 2 );
```

# Подключение ViewModel в View

```xml
<ContentPage.BindingContext>
    <vm:PersonViewModel/>
</ContentPage.BindingContext>
```

# Внедрение зависимостей DI для сервиса

```Csharp
public class MainPageViewModel
{
    readonly IDataService _dataService;
    public MainPageViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }
}
```
- MainProgram.cs
```Csharp
builder.Services.AddSingleton<IDataService, DataService>();
```

# Вызов асинхронных методов

Команды также могут вызывать асинхронные методы. Это достигается использованием ключевых слов async и await при указании Execute метода:

```Csharp
DownloadCommand = new Command (async () => await DownloadAsync ());
```

Это указывает на то, что DownloadAsync метод является Task и его следует ожидать:

```Csharp
async Task DownloadAsync ()
{
    await Task.Run (() => Download ());
}

void Download ()
{
    ...
}
```

# Динамическая загрузка XAML

Метод InitializeComponent() вызывает другой метод - LoadFromXaml(). Этот метод извлекает файл XAML (или его скомпилированную бинарную версию) из файла dll, в который скомпилировано приложение.

После извлечения кода графического интерфейса метод инициализирует все объекты, определенные в файле XAML, устанавливает структуру элементов (какой элемент какие элементы содержит), прикрепляет обработчики событий, определенные в файле кода C#, и создает дерево объектов. Таким образом, код из XAML используется для построения интерфейса.

Однако мы сами можем загружать код XAML, причем динамически в процессе выполнения программы, используя тот же самый метод LoadFromXaml(). Подобный способ может применяться, например, при получении кода XAML от веб-сервиса или, когда в зависимости от хода программы необходимо предоставить альтернативные версии интерфейса. Но вместе с тем стоит отметить, что динамическая загрузка XAML снижает производительность приложения, поэтому по возможности ее следует избегать.

Например, добавим в проект новый класс StartPage и определим в нем следующий код:

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            string pageXAML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<ContentPage xmlns=\"http://schemas.microsoft.com/dotnet/2021/maui\"\n" +
                "xmlns:x=\"http://schemas.microsoft.com/winfx/2009/xaml\"\n" +
                "x:Class=\"HelloApp.StartPage\">\n" +
                "<Label Text=\"Hello METANIT.COM\" FontSize=\"22\" />" +
                "</ContentPage>";
 
            this.LoadFromXaml(pageXAML);
        }
    }
}
```

Данный класс будет представлять страницу и наследуется от класса ContentPage. В конструкторе класса сначала формируется визуальный интерфейс в виде строки, которая содержит код XML и далее этого код загружается с помощью метода LoadFromXaml().

# Контейнеры компоновки

# Stack Layout

```xml
    <StackLayout Spacing="5"
                 Orientation="Horizontal">
        <Label Text="Первая метка2" TextColor="Red" />
        <Label Text="Вторая метка" TextColor="Blue" />
        <Label Text="Третья метка" TextColor="Green" />
    </StackLayout>
```

# Absolute Layout

```xml
        <AbsoluteLayout Margin="20">
            <BoxView Color="LightPink"
                 AbsoluteLayout.LayoutBounds="30, 70, 340, 100" />
            <Label Text=".NET MAUI on METANIT.COM"
               FontSize="20"
               AbsoluteLayout.LayoutBounds="50, 100" />
        </AbsoluteLayout>
```

```xml
 <AbsoluteLayout>
        <BoxView Color="Red" AbsoluteLayout.LayoutBounds=".1, .2, 50, 80"
        AbsoluteLayout.LayoutFlags="PositionProportional" />
        <BoxView Color="Green" AbsoluteLayout.LayoutBounds="1, .2, 50, 80"
        AbsoluteLayout.LayoutFlags="PositionProportional" />
        <BoxView Color="Blue" AbsoluteLayout.LayoutBounds=".4, .8, .2, .2"
        AbsoluteLayout.LayoutFlags="All" />
    </AbsoluteLayout>
```
# Grid

```xml
    <Grid ColumnSpacing="5" RowSpacing="8">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*" />
            <ColumnDefinition Width="2*" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="2*" />
        </Grid.RowDefinitions>
        <BoxView Color="Red" Grid.Column="0" Grid.Row="0" />
        <BoxView Color="Blue" Grid.Column="0" Grid.Row="1" />
 
        <BoxView Color="Teal" Grid.Column="1" Grid.Row="0" />
        <BoxView Color="Green" Grid.Column="1" Grid.Row="1" />
 
        <BoxView Color="Olive" Grid.Column="2" Grid.Row="0" />
        <BoxView Color="Pink" Grid.Column="2" Grid.Row="1" />
    </Grid>
```

# Размеры

```xml
<Button Text="Click" WidthRequest="100" HeightRequest="50" />
```

# Margin and Padding

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="60">
        <BoxView Color="Blue" Margin="50" HeightRequest="100" />
        <BoxView Color="Red"  Margin="50" HeightRequest="100" />
    </StackLayout>
</ContentPage>
```
![](https://metanit.com/sharp/maui/pics/4.1.png)

# Выравнивание

```xml
<Label Text="Hello METANIT.COM" VerticalOptions="Center" HorizontalOptions="Center" />
```
# Color

```xml
 <Label Text="Hello METANIT.COM"
           VerticalOptions="Center" HorizontalOptions="Center"
           BackgroundColor="LightBlue" TextColor="DarkBlue"
           />
<Label Text="Hello METANIT.COM" BackgroundColor="#B2DFDB" TextColor="#aa004D40" />

```
# Стилизация текста

```xml
<Label Text=".NET MAUI in Arial" FontFamily="Arial" />
Свойство FontSize

<Label Text="Bold" FontAttributes="Bold" />
<Label Text="Bold и Italics" FontAttributes="Bold, Italic" />

<Label Text="Hello METANIT.COM!" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" />

```
# Шрифт

```Csharp
var builder = MauiApp.CreateBuilder();
builder
    .UseMauiApp<app>()
    .ConfigureFonts(fonts =>
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
});
</app>
```
# Button

```xml
<Button Text="Нажми" FontSize="22" BorderWidth="1"
                BackgroundColor="LightPink" TextColor="DarkRed"
        HorizontalOptions="Center" VerticalOptions="Center"
        Clicked="OnButtonClicked" />
```

# Label

```xml
<Label FontSize="22" Text = "Hello METANIT.COM!"
             TextDecorations = "Underline" CharacterSpacing = "2"
             FontAttributes = "Bold" FontFamily = "Helvetica"
            HorizontalOptions="Center" VerticalOptions="Center"
         />

<Label HorizontalOptions="Center" VerticalOptions="Center">
            <Label.FormattedText>
                <FormattedString>
                    <Span Text="Сегодня " FontSize="22" />
                    <Span Text="хорошая " BackgroundColor="LightPink" TextColor="DarkRed" />
                    <Span Text="погода!" FontAttributes="Bold" />
                </FormattedString>
            </Label.FormattedText>
        </Label>

 <Label HorizontalOptions="Center" VerticalOptions="Center" FontSize="22">
            <Label.Text>
                <x:String>
Его пример другим наука;
Но, боже мой, какая скука
С больным сидеть и день и ночь,
Не отходя ни шагу прочь!
                </x:String>
            </Label.Text>
        </Label>


```

# Поле

```xml
Entry Placeholder = "Введите Email" FontFamily="Helvetica"
                FontSize="22" MaxLength ="20" />
<Entry Keyboard="Telephone" />

<Entry>
    <Entry.Keyboard>
        <Keyboard x:FactoryMethod="Create">
            <x:Arguments>
                <KeyboardFlags>Suggestions,CapitalizeCharacter</KeyboardFlags>
            </x:Arguments>
        </Keyboard>
    </Entry.Keyboard>
</Entry>

<Entry x:Name="nameEntry" FontSize="22" TextChanged="nameEntry_TextChanged" />



```

# Многострочное поле

```xml
 <Editor FontSize="16" HeightRequest="200" />
```

# BoxView

BoxView представляет прямоугольную область. Обычно BoxView применяется для создания окрашенных областей, либо в качестве декоративного примитивного графического оформления к другим элементам.

```xml
<BoxView Color="LightBlue" WidthRequest="150" HeightRequest="150" CornerRadius="8"
                 HorizontalOptions="Center" VerticalOptions="Center" />
```

# ScrollView

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <ScrollView>
        <StackLayout Padding="20">
            <Label FontSize="18">
                <Label.Text>
                    <x:String>
«Мой дядя самых честных правил,
Когда не в шутку занемог,
Он уважать себя заставил
И лучше выдумать не мог.
Его пример другим наука;
Но, боже мой, какая скука
С больным сидеть и день и ночь,
Не отходя ни шагу прочь!
Какое низкое коварство
Полуживого забавлять,
Ему подушки поправлять,
Печально подносить лекарство,
Вздыхать и думать про себя:
Когда же черт возьмет тебя!»
 
Так думал молодой повеса,
Летя в пыли на почтовых,
Всевышней волею Зевеса
Наследник всех своих родных.
Друзья Людмилы и Руслана!
С героем моего романа
Без предисловий, сей же час
Позвольте познакомить вас:
Онегин, добрый мой приятель,
Родился на брегах Невы,
Где, может быть, родились вы
Или блистали, мой читатель;
Там некогда гулял и я:
Но вреден север для меня.
                    </x:String>
                </Label.Text>
            </Label>
        </StackLayout>
    </ScrollView>
</ContentPage>
```

# Image

```xml
<Image Source="forest.png" />
<Image Source="https://news.microsoft.com/wp-content/uploads/2014/12/452292672.jpg" />

<Image>
    <Image.Source>
        <UriImageSource Uri="https://news.microsoft.com/wp-content/uploads/2014/12/452292672.jpg"
                        CacheValidity="2:00:00.0" />
    </Image.Source>
</Image>

<Image Source="forest.png" Aspect="AspectFit" />

```

# Frame

```xml
<Frame BorderColor="Gray" BackgroundColor="#e1e1e1">
    <Label HorizontalTextAlignment="Center" VerticalTextAlignment="Center" Text="Hello METANIT.COM" />
</Frame>

<Frame Margin="10" HorizontalOptions="Start"
       IsClippedToBounds="True" CornerRadius="90"
       HeightRequest="100" WidthRequest="100">
    <Image Source="forest.png" Aspect="AspectFill" Margin="-20"
	HeightRequest="100" WidthRequest="100"  />
</Frame>

<Frame BorderColor="Gray" BackgroundColor="#e1e1e1">
    <StackLayout>
	<Label HorizontalTextAlignment="Center" FontSize="22" Text=".NET MAUI" />
	<BoxView HeightRequest="2" Color="DarkGray" />
	<Label FontSize="16" Text=".NET MAUI - технология, предназначенная для создания кроссплатформенных приложений." />
    </StackLayout>
</Frame>

```

# DataPicker

```xml
<DatePicker Format="d" DateSelected="DateSelected">
    <DatePicker.MinimumDate>7/10/2022</DatePicker.MinimumDate>
    <DatePicker.MaximumDate>7/20/2022</DatePicker.MaximumDate>
</DatePicker>
```

# TimePicker

```xml
 <TimePicker x:Name="timePicker" Time="17:00:00" PropertyChanged="TimePicker_PropertyChanged" />
```

# Stepper

```xml
<Stepper Minimum ="0" Maximum="10" Increment ="0.1" VerticalOptions = "Start"
                 ValueChanged="OnStepperValueChanged" />
```

```Csharp
void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        header.Text = $"Выбрано: {e.NewValue:F1}";
    }
```

# Slider

```xml
<Slider Minimum ="0" Maximum="50" Value="30" ValueChanged="OnSliderValueChanged"
        MinimumTrackColor="DeepPink" MaximumTrackColor="LightPink" ThumbColor="DeepPink" />
```

```Csharp
void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
{
    header.Text = $"Выбрано: {e.NewValue:F1}";
}
```

# Switch

```xml
<Switch x:Name="switcher" IsToggled="True" Toggled="switcher_Toggled" />
```
```Csharp
void switcher_Toggled(object sender, ToggledEventArgs e)
    {
        label.Text = $"Значение {e.Value}";
    }
```

# CheckBox

```xml
<CheckBox x:Name="statusCheckBox" CheckedChanged="CheckBox_CheckedChanged" />
```
```Csharp
void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        statusLabel.Text = $"Статус: {(e.Value ? "женат/замужем" : "холост/не замужем")}";
    }
```

# RadioButton

```xml
    <StackLayout RadioButtonGroup.GroupName="languages">
        <Label x:Name="header" Text="Выберите язык программирования" />
        <RadioButton Content="C#" Value="C#" CheckedChanged="OnLanguageCheckedChanged" />
        <RadioButton Content="Java" Value="Java"  CheckedChanged="OnLanguageCheckedChanged" />
        <RadioButton Content="C++" Value="C++"  CheckedChanged="OnLanguageCheckedChanged" />
    </StackLayout>
```
```Csharp
    void OnLanguageCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton selectedRadioButton = ((RadioButton)sender);
        if(header!=null)
            header.Text = $"Выбранный язык: {selectedRadioButton.Value}";
    }
```

# Picker

```xml
<Picker x:Name="languagePicker" Title = "Язык программирования"
	SelectedIndexChanged="PickerSelectedIndexChanged">
    <Picker.Items>
	<x:String>C#</x:String>
	<x:String>JavaScript</x:String>
	<x:String>Java</x:String>
    </Picker.Items>
</Picker>
```
```Csharp
void PickerSelectedIndexChanged(object sender, EventArgs e)
    {
        header.Text = $"Вы выбрали: {languagePicker.SelectedItem}";
    }
```

# TableView

```xml
<TableView>
            <TableView.Root>
                <TableRoot Title="Разработка ПО">
                    <TableSection Title="Языки программирования">
                        <TextCell Text="Java" Detail="Был создан в 1995 году в компании Sun Microsystems" />
                        <TextCell Text="C#" Detail="Был создан в 2000 году в компании Microsoft" />
                    </TableSection>
                    <TableSection Title="Базы данных">
                        <TextCell Text="MySQL" Detail="Была создана в 1995 году в компании MySQL AB" />
                        <TextCell Text="MS SQL Server" Detail="Была создана в 1989 году в компании Microsoft" />
                    </TableSection>
                </TableRoot>
            </TableView.Root>
        </TableView>

	<TableView>
            <TableView.Root>
                <TableRoot>
                    <TableSection Title="Персональные данные">
                        <EntryCell x:Name="loginEntry" Label="Логин" Keyboard="Default" Placeholder="Введите логин" Completed="OnTextCompleted" />
                        <SwitchCell x:Name="saveSwitch" Text="Сохранить" OnChanged="OnStatusChanged" />
                    </TableSection>
                </TableRoot>
            </TableView.Root>
        </TableView>


 	<TableView>
            <TableView.Root>
                <TableRoot>
                    <TableSection Title="Языки программирования">
                        <ImageCell Text="C#" Detail="Создатель: Андерс Хейлсберг" ImageSource="csharp.jpg"  />
                        <ImageCell Text="C++" Detail="Создатель: Бьорн Страуструп" ImageSource="cpp.jpg" />
                        <ImageCell Text="Java" Detail="Создатель: Джеймс Гослинг" ImageSource="java.jpg" />
                    </TableSection>
                </TableRoot>
            </TableView.Root>
        </TableView>

```
```Csharp
    void OnTextCompleted(object sender, EventArgs e)
    {
        loginLbl.Text = loginEntry.Text;
    }
 
    void OnStatusChanged(object sender, ToggledEventArgs e)
    {
        saveLbl.Text = saveSwitch.On ? "сохранено" : "не сохранено";
    }
```

# Сообщения

# DisplayAlert

```Csharp
async void AlertButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Уведомление", "Пришло новое сообщение", "ОK");
    }
    
async void AlertButton_Clicked(object sender, EventArgs e)
{
    bool result = await DisplayAlert("Подтвердить действие", "Вы хотите удалить элемент?", "Да", "Нет");
    await DisplayAlert("Уведомление", "Вы выбрали: "+ (result ? "Удалить" : "Отменить"), "OK");
}
     
```

# DisplayActionSheet

```Csharp
    async void AlertButton_Clicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Выбрать язык", "Отмена", "Удалить", "C#", "JavaScript", "Java");
        actionLabel.Text = action;
    }
```

# DisplayPromtAsync

```xml
 async void AlertButton_Clicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Логин", "Введите имя:", "OK", "Отмена");
        nameLabel.Text = name;
    }
```

# ProgressBar

```xml
<StackLayout Padding="20">
        <Label Text="Progressbar" />
        <ProgressBar Progress="0.4" ProgressColor="SeaGreen" />
    </StackLayout>
```
```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        ProgressBar progressBar = new ProgressBar { ProgressColor = Colors.SeaGreen };
        Label label = new Label();
        public StartPage()
        {
            StackLayout stackLayout = new StackLayout { Padding = 20 };

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(progressBar);
            Content = stackLayout;
        }
        protected override async void OnAppearing()
        {
            while(progressBar.Progress < 0.9)
            {
                progressBar.Progress += 0.1;
                label.Text = $"Состояние процесса: {Math.Round(progressBar.Progress, 1) * 100} %";
                await Task.Delay(1000);
            }
            label.Text = "Процесс закончен";
            base.OnAppearing();
        }
    }
}
```

# ActivityIndicator

```xml
ActivityIndicator IsRunning="true" Color="SeaGreen" />

```
```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true, Color = Colors.SeaGreen };
        Label label = new Label();
        public StartPage()
        {
            StackLayout stackLayout = new StackLayout { Padding = 20 };
 
            stackLayout.Children.Add(label);
            stackLayout.Children.Add(activityIndicator);
            Content = stackLayout;
        }
        protected override async void OnAppearing()
        {
            int count = 0;
            while (count != 100)
            {
                label.Text = $"Состояние процесса: {count} %";
                await Task.Delay(2000);
                count +=10;
            }
            label.Text = "Процесс закончен";
            activityIndicator.IsRunning = false;
            base.OnAppearing();
        }
    }
}
```
# Ресурсы

```xml
<ContentPage.Resources>
        <ResourceDictionary>
            <Color x:Key="textColor">#004D40</Color>
            <Color x:Key="backColor">#80CBC4</Color>
            <x:Double x:Key="margin">10</x:Double>
        </ResourceDictionary>
    </ContentPage.Resources>
```

```xml
  <Button Text="iOS" TextColor="{StaticResource Key=textColor}"
```

# Стили

```xml
    <ContentPage.Resources>
        <ResourceDictionary>
            <Style x:Key="buttonStyle" TargetType="Button">
                <Setter Property="TextColor" Value="#004D40" />
                <Setter Property="BackgroundColor" Value="#80CBC4" />
                <Setter Property="Margin" Value="10" />
            </Style>
        </ResourceDictionary>
    </ContentPage.Resources>
```
# Наследование стилей

```xml
    <ContentPage.Resources>
        <ResourceDictionary>
            <Style x:Key="baseButtonStyle" TargetType="Button">
                <Setter Property="Margin" Value="10" />
                <Setter Property="WidthRequest" Value="120" />
                <Setter Property="TextColor" Value="#01579B" />
                <Setter Property="BackgroundColor" Value="#fff" />
            </Style>
            <Style x:Key="greenButtonStyle" TargetType="Button" BasedOn="{StaticResource baseButtonStyle}">
                <Setter Property="TextColor" Value="#004D40" />
                <Setter Property="BackgroundColor" Value="#80CBC4" />
            </Style>
        </ResourceDictionary>
    </ContentPage.Resources>
```
# Внешние стили

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="TNC.MAUI.Resources.Styles.GreenButtonStyle">
    <Style x:Key="greenButtonStyle" TargetType="Button">
        <Setter Property="TextColor" Value="#004D40" />
        <Setter Property="BackgroundColor" Value="#80CBC4" />
        <Setter Property="Margin" Value="10" />
    </Style>
</ResourceDictionary>
```
```xml
    <ContentPage.Resources>
        <ResourceDictionary Source="/Resources/Styles/GreenButtonStyle.xaml" />
    </ContentPage.Resources>
```
# Css

```css
^contentpage {
    background-color: #fefefe;
}

stacklayout {
    padding: 20;
}

    stacklayout label {
        font-family: Verdana;
        margin: 5;
    }

#header {
    font-size: 18;
    font-weight: bold;
    text-decoration: underline;
}

.english {
    font-weight: bold;
    font-size: 14;
    color: darkblue;
}

.russian {
    font-size: 12;
}
```

```xml
    <ContentPage.Resources>
        <StyleSheet Source="/styles/mystyles.css" />
        <StyleSheet>
            <![CDATA[
            ^contentpage {
                background-color: lightcyan
            }
 
            stacklayout {
                margin: 25;
                padding: 10;
            }
            ]]>
        </StyleSheet>


    </ContentPage.Resources>
```

# Visual State Manager и визуальные состояния

Visual State Manager (менеджер визуальных состояний) позволяет организовать изменения визуального интерфейса. По умолчанию Visual State Manager прикрепляет к элементам управления группу из трех состояний:

- Disabled: элемент отключен для использования

- Focused: элемент получил фокус и используется в текущий момент

- Normal: стандартное состояние элемента

- PointerOver: указатель мыши находится над элементом

Состояния Normal, Disabled, Focused и PointerOver поддерживается для объектов всех классов, которые унаследованы от VisualElement (в том числе классов View и Page). Кроме того, при необходимости можно определять свои визуальные состояния.

```xml
<StackLayout>
        <Entry x:Name="entry" Text="Hello METANIT.COM!">
            <VisualStateManager.VisualStateGroups>
                <VisualStateGroup x:Name="CustomStates">
 
                    <VisualState x:Name="Focused">
                        <VisualState.Setters>
                            <Setter Property="TextColor" Value="#004D40" />
                            <Setter Property="BackgroundColor" Value="#B2DFDB" />
                        </VisualState.Setters>
                    </VisualState>
 
                    <VisualState x:Name="PointerOver">
                        <VisualState.Setters>
                            <Setter Property="TextColor" Value="#004D40" />
                            <Setter Property="FontAttributes" Value="Bold" />
                        </VisualState.Setters>
                    </VisualState>
                </VisualStateGroup>
            </VisualStateManager.VisualStateGroups>
        </Entry>
    </StackLayout>
```

Определение в ресурсах

```xml
 <ContentPage.Resources>
        <Style TargetType="Entry">
            <Setter Property="VisualStateManager.VisualStateGroups">
                <VisualStateGroupList>
                    <VisualStateGroup x:Name="CustomStates">
 
                        <VisualState x:Name="Focused">
                            <VisualState.Setters>
                                <Setter Property="TextColor" Value="#004D40" />
                                <Setter Property="BackgroundColor" Value="#B2DFDB" />
                            </VisualState.Setters>
                        </VisualState>
 
                        <VisualState x:Name="PointerOver">
                            <VisualState.Setters>
                                <Setter Property="TextColor" Value="#004D40" />
                                <Setter Property="FontAttributes" Value="Bold" />
                            </VisualState.Setters>
                        </VisualState>
                    </VisualStateGroup>
                </VisualStateGroupList>
            </Setter>
        </Style>
    </ContentPage.Resources>
```

# Доступные визуальные состояния

В .NET MAUI для ряда элементов есть дополнительные состояния:

- VisualElement определяет общие для всех элементов состояния Normal, Disabled, Focused и PointerOver

- Button определяет состояние Pressed (нажатие кнопки)

- CarouselView определяет состояния DefaultItem, CurrentItem, PreviousItem и NextItem

- CheckBox определяет состояние IsChecked (флажок отмечен)

- CollectionView определяет состояние Selected (элемент выделен)

- ImageButton определяет состояние Pressed (нажатие кнопки)

- RadioButton определяет состояния Checked (элемент выбран) и Unchecked (элемент не выбран)

- Switch определяет состояния On (включено) и Off(выключено)


# Определение своих визуальных состояний
https://metanit.com/sharp/maui/5.5.php

# Привязка

Привязка данных или data binding является одним из ключевых аспектов при создании приложений на платформе .NET Multi-platform App UI. Привязка позволяет связать два свойства разных объектов таким образом, что изменения свойства одного объекта автоматически приводили к изменению свойства другого объекта.

Привязка данных состоит из двух компонентов:

- источник привязки (source) - кто привязывается

- цель привязки (target) - к кому идет привязка

Привязка осуществляется от свойства источника к свойству цели. И когда происходит изменение источника, механизм привязки автоматически обновляет также и цель.

![](https://metanit.com/sharp/maui/pics/5.1.png)

Объект-цель привязки должен представлять объект BindableObject, а свойство, к которому осуществляется привязка, должно быть свойством BindableProperty. Поскольку большинство визуальных элементов в .NET MAUI и C# наследуются от класса BindableObject, то в качестве цели привязки будут, как правило, выступать визуальные элементы.

А вот источником привязки может выступать любой объект языка C#. Однако, надо понимать, что цель привязки должна автоматически изменяться при изменении источника, поэтому нам нужно извещать систему о изменении свойств источника привязки. В .NET MAUI, да и вообще на платформе .NET, в качестве подобного механизма извещения выступает интерфейс INotifyPropertyChanged. То есть нужно реализовать данный интерфейс в объекте-источнике.

Объект BindableObject как раз реализует INotifyPropertyChanged. Поэтому если источником привязки является стандартный визуальный элемент из .NET MAUI, то автоматически будет изменяться и цель привязки. Но если в качестве источника выступает не BindableObject, а какой-нибудь объект простого класса C#, то, как писалось выше, этот класс должен реализовать INotifyPropertyChanged.

Есть два способа установить привязку - с помощью свойства BindingContext и с помощью объекта Binding. Рассмотрим на примерах, как устанавливается привязка в коде C# и в коде XAML.

# Свойство BindingContext

Для установки привязки у объекта-цели устанавливается свойство BindingContext. В качестве значения оно принимает источник привязки.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
 
    <StackLayout Padding="20">
        <Label BindingContext="{x:Reference entryBox}" Text="{Binding Text}" />
        <Entry x:Name="entryBox" />
    </StackLayout>
 
</ContentPage>
```

# Свойство Binding

```xml
    <StackLayout Padding="20">
        <Label x:Name="label" Text="{Binding Source={x:Reference entry}, Path=Text}" />
        <Entry x:Name="entry" />
    </StackLayout>
```

# StringFormat

```xml
<Entry WidthRequest="100" HeightRequest="20" Placeholder="Name" x:Name="textBox"/>

<Entry WidthRequest="100" HeightRequest="20" Placeholder="Name"
       Text="{Binding Source={x:Reference textBox}, Path=Text, StringFormat='Текст: {0}',Mode=OneWay}"
 />
```

# Конвертер значений

Прежде всего класс конвертера значений должен реализовать интерфейс IValueConverter. Этот интерфейс определяет два метода: Convert(), который преобразует пришедшее от привязки значение в тот тип, который понимается приемником привязки, и ConvertBack(), который выполняет противоположную операцию.

Оба метода принимают четыре параметра:

object value: значение, которое надо преобразовать

Type targetType: тип, к которому надо преобразовать значение value

object parameter: вспомогательный параметр

CultureInfo culture: текущая культура приложения

Метод Convert вызывается при передаче данных от источника привязки к цели, когда действуют режимы привязки OneWay и TwoWay. Параметр value представляет значение, которое пришло от источника. Метод должен возвратить значение к типу привязанного свойства цели.

Метод ConvertBack() вызывается, когда данные передаются от цели привязки к источнику при режимах привязки TwoWay и OneWayToSource. ConvertBack выполняет обратное преобразование к типу привязанного свойства источника.

```Csharp
public class DateTimeToLocalDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("dd.MM.yyyy");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse(value.ToString());
        }
    }
```
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:DateTimeToLocalDateConverter x:Key="dateConverter" />
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="20">
        <Label Text="{Binding Source={x:Reference Name=datePicker},  
                        Path=Date, 
                        Converter={StaticResource dateConverter}}" />
        <DatePicker x:Name="datePicker" />
    </StackLayout>
</ContentPage>
```

# Пример использования конвертера

```Csharp
public class StringToStatusConverter : IValueConverter
    {
        public string ApprovedStatus { get; set; } = "Approved";
        public string DeniedStatus { get; set; } = "Denied";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string username = (string)value;
            if (username == "admin") return ApprovedStatus;
            return DeniedStatus;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value.ToString();
 
            if (status == ApprovedStatus) return "admin";
            return "user";
        }
    }
```

```xml
        <Entry x:Name="entry" />
        <Label>
            <Label.Text>
                <Binding Source="{x:Reference entry}" Path="Text">
                    <Binding.Converter>
                        <local:StringToStatusConverter ApprovedStatus="Доступ разрешен" DeniedStatus="Доступ запрещен" />
                    </Binding.Converter>
                </Binding>
            </Label.Text>
        </Label>
```

# Параметры конвертера

```Csharp
public class StringToCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter.ToString()== "euro")
            {
                return $"{value} €";
            }
            return $"{value} $";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Replace("  €", "").Replace(" $", "");
        }
    }
```
```xml
 <Binding Source="{x:Reference entry}" Path="Text" ConverterParameter="euro">
                    <Binding.Converter >
                        <local:StringToCurrencyConverter />
                    </Binding.Converter>
                </Binding>
```

# Относительная привязка

https://metanit.com/sharp/maui/6.8.php

# Тригеры

```xml
<Entry TextColor="LightGray" Text="Hello METANIT.COM">
    <Entry.Triggers>
	<Trigger Property="IsFocused" Value="True" TargetType="Entry">
	    <Setter Property="TextColor" Value="Green" />
	</Trigger>
    </Entry.Triggers>
</Entry>
```
# Тригеры как стили

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <Style x:Key="entryStyle" TargetType="Entry">
                <Style.Triggers>
                    <Trigger Property="IsFocused" Value="True" TargetType="Entry">
                        <Setter Property="TextColor" Value="#000" />
                    </Trigger>
                </Style.Triggers>
            </Style>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="20">
        <Entry TextColor="LightGray" Text="Hello METANIT.COM" Style="{StaticResource entryStyle}" />
    </StackLayout>
</ContentPage>
```

# Тригеры событий

```Csharp
public class EntryValidation : TriggerAction<Entry>
{
    protected override void Invoke(Entry sender)
    {
        int number;
        if (!Int32.TryParse(sender.Text, out number))
            sender.BackgroundColor = Color.Red;
        else
            sender.BackgroundColor = Color.Default;
    }
}
```
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="20">
        <Entry>
            <Entry.Triggers>
                <EventTrigger Event="TextChanged">
                    <local:EntryValidation />
                </EventTrigger>
            </Entry.Triggers>
        </Entry>
    </StackLayout>
</ContentPage>
```
# Триггеры данных

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="10">
        <Entry x:Name="entry" Text="" Margin="10" />
        <Button Text="Save" TextColor="#01579B" BackgroundColor="#fff" Margin="10">
            <Button.Triggers>
                <DataTrigger TargetType="Button"
                     Binding="{Binding Source={x:Reference entry}, Path=Text.Length}"
                     Value="0">
                    <Setter Property="IsEnabled" Value="False" />
                    <Setter Property="BackgroundColor" Value="LightGray"/>
                    <Setter Property="TextColor" Value="Gray"/>
                </DataTrigger>
            </Button.Triggers>
        </Button>
    </StackLayout>
</ContentPage>
```

# Мультитриггеры

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="10">
        <Entry x:Name="email" Text="" Margin="6" />
        <Entry x:Name="phone" Text="" Margin="6"/>
        <Button Text="Send" TextColor="#01579B" BackgroundColor="#fff" >
            <Button.Triggers>
                <MultiTrigger TargetType="Button">
                    <MultiTrigger.Conditions>
                        <BindingCondition Binding="{Binding Source={x:Reference email}, Path=Text.Length}"
                                  Value="0" />
                        <BindingCondition Binding="{Binding Source={x:Reference phone}, Path=Text.Length}"
                                  Value="0" />
                    </MultiTrigger.Conditions>
                    <Setter Property="IsEnabled" Value="False" />
                    <Setter Property="BackgroundColor" Value="LightGray" />
                    <Setter Property="TextColor" Value="Gray" />
                </MultiTrigger>
            </Button.Triggers>
        </Button>
    </StackLayout>
</ContentPage>
```

# ListView

# Обработка выбора элемента

Когда пользователь нажимает на элемент списка, он выделяется, а у ListView срабатывают два события: ItemTapped и ItemSelected. Между ними есть различия. Так, повторное нажатие на один и тот же элемент не вызовет повторного события ItemSelected, так как элемент остается выбанным. А вот событие ItemTapped будет срабатывать именно столько раз сколько пользователь нажал на него, даже повторно. Также событие ItemSelected будет вызвано, если с элемента будет снято выделение.

```xml
        <Label Text="{Binding Source={Reference listView}, Path=SelectedItem.Name}" />
        <ListView RowHeight="50" x:Name="listView" ItemsSource="{Binding People}" >
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

```
```Csharp
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
```

В ListView для отображения данных можно применять еще несколько типов ячеек:

- TextCell: выводит заголовок и некоторое детальное описание

- ImageCell: выводит рядом с заголовком и детальным описанием изображение

Для простых случаев, когда для каждого объекта в списке необходимо вывести заголовок и некоторую аннотацию к нему, можно использовать класс TextCell. Его основные свойства, которые могут нам пригодиться:

- Text: основной текст, выводится большим шрифтом

- Detail: детальное описание, выводится меньшим шрифтом

- TextColor: цвет текста

- DetailColor: цвет детального описания

```xml
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
```

```xml
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
```
```xml
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
```
# Header and Footer in ListView

```xml
<ListView RowHeight="50" x:Name="listView" ItemsSource="{Binding People}" >

            <ListView.Header>
                <Label Text= "Список пользователей" FontSize="18" />
            </ListView.Header>
            
            <ListView.Footer>
                <VerticalStackLayout>
                    <Label FontSize="12" Text="METANIT.COM" />
                    <Label FontSize="12" Text="январь 2023" />
                </VerticalStackLayout>
            </ListView.Footer>

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
```

# Cashing

https://metanit.com/sharp/maui/8.9.php

# CarouselView

```xml
<VerticalStackLayout Padding="5">

        <Label x:Name="header" FontSize="18" Text="{Binding Source={x:Reference carouselView}, Path=CurrentItem.Name}" />

        <CarouselView x:Name="carouselView"
                      IndicatorView="indicatorView"
                      CurrentItemChangedCommand="{Binding SelectCommand}"
                      CurrentItemChangedCommandParameter="{Binding Source={x:Reference carouselView}, Path=CurrentItem}"
                      >
                      <!-- Команда срабатывает на изменение продукта -->

            <CarouselView.ItemsLayout>
                <LinearItemsLayout Orientation="Horizontal" />
            </CarouselView.ItemsLayout>

            <CarouselView.ItemsSource>
                <x:Array Type="{x:Type local:Product}">
                    <local:Product Name="Huawei P50" ImagePath="image.jpg" Description="Цена 59000" />
                    <local:Product Name="iPhone 14" ImagePath="image.jpg" Description="Цена 65000" />
                    <local:Product Name="Realme GT2 Pro" ImagePath="image.jpg" Description="Цена 41000" />
                    <local:Product Name="Xiaomi 12X" ImagePath="image.jpg" Description="Цена 53999" />
                </x:Array>
            </CarouselView.ItemsSource>
            
            <CarouselView.ItemTemplate>
                <DataTemplate>
                    <Frame>
                        <VerticalStackLayout VerticalOptions="Center">
                            <Label Text="{Binding Name}" FontAttributes="Bold" FontSize="18" HorizontalTextAlignment="Center"/>
                            <Image WidthRequest="150" HeightRequest="150" Source="{Binding ImagePath}" />
                            <Label Text="{Binding Description}" HorizontalTextAlignment="Center" />
                        </VerticalStackLayout>

                        <!-- Выбор механизма выбора элемента по клику -->
                        <Frame.GestureRecognizers>
                            <TapGestureRecognizer
                                Command="{Binding Source={x:Reference carouselView}, Path=BindingContext.SelectCommand}"
                                CommandParameter="{Binding}"/>
                        </Frame.GestureRecognizers>


                    </Frame>
                    

                    
                </DataTemplate>
            </CarouselView.ItemTemplate>
        </CarouselView>

        <IndicatorView Margin="0, 10, 0, 0" x:Name="indicatorView"
                   IndicatorColor="LightGray"
                   SelectedIndicatorColor="Red"
                   HorizontalOptions="Center" />


    </VerticalStackLayout>
```

# Сообщение из ViewModel

```Csharp
            SelectCommand = new Command<Product>(async p =>
            {
                //await DisplayAlert("Notification", $"You have selected: {p.Name}", "ОK");

                await Application.Current.MainPage.DisplayAlert("Notification", $"You have selected: {p.Name}", "ОK");

            });
```

# Событие изменение элемента в карусели 

```Csharp
    private void carouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        Product? current = e.CurrentItem as Product;
        Product? previous = e.PreviousItem as Product;
        header.Text = $"Current: {current?.Name}  Previous: {previous?.Name}";
    }
```

# CollectionView

```xml
<CollectionView ItemsLayout="HorizontalGrid, 2" VerticalOptions="Start" >

        <!--
        <CollectionView.ItemsLayout>
            <LinearItemsLayout  ItemSpacing="20"  />
        </CollectionView.ItemsLayout>
        -->
        
        <CollectionView.ItemsSource>
            <x:Array Type="{x:Type local:Person}">
                <local:Person Name="Tom" Age="1" />
                <local:Person Name="Bob" Age="2" />
                <local:Person Name="Sam" Age="3" />
                <local:Person Name="Alice" Age="4" />
                <local:Person Name="Kate" Age="5" />
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
```

# Команда для CollectionView

```Csharp
SelectCommand = new Command<Person?>(p =>
        {
            selectedLabel.Text = $"Selected: {p?.Name}";
        });
```

```xml
  <CollectionView x:Name="collectionView" SelectionMode="Single"
                        SelectionChangedCommand="{Binding SelectCommand}"
                        SelectionChangedCommandParameter="{Binding Source={RelativeSource Self}, Path=SelectedItem}">
```

# Навигация по страницам Page

```Csharp
 [RelayCommand]
        async Task StatementAsync()
        {
           App.Current.MainPage.DisplayAlert("Подключение", "Переход", "Ок");
           StatementPage page = new StatementPage();
           App.Current.MainPage = page;
	}
```

```App.xaml.cs```

```Csharp
MainPage = new MainPage();
```




```Csharp
 [RelayCommand]
        async Task StatementAsync()
        {
            await Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PushAsync(

                new NavigationPage(new StatementPage())
                {
                    BarBackground = Brush.Yellow,
                    BarBackgroundColor = Color.FromArgb("#2980B9"),
                    BarTextColor = Colors.White
                }

            );   
        }
```

```App.xaml.cs```

```Csharp
MainPage = new NavigationPage(new MainPage());
```

**Замечание**: можно применить стандартное определение команды, для навигации назад по стеку.

```Csharp
    BackCommand = new Command(
	async () =>
	{
	await Application.Current.MainPage.Navigation.PopAsync();

	}

	);
```

https://metanit.com/sharp/maui/12.3.php

# Навигация через AppShell

```Csharp
        [RelayCommand]
        async Task Tap()
        {
            await Shell.Current.GoToAsync("//PageNew");
            //await Shell.Current.GoToAsync("PageNew");

        }

        [RelayCommand]
        async Task TapBack()
        {
            //await Shell.Current.GoToAsync("//MainPage");
            await Shell.Current.GoToAsync("//MainPage");
            // ..
            // //MainPage
        }
```

```Csharp
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PageNew), typeof(PageNew));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
```

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="TaskApp.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:TaskApp"
    Shell.FlyoutBehavior="Disabled">

    <ShellContent
        Title="Home"
        ContentTemplate="{DataTemplate local:MainPage}"
        Route="MainPage" />

    <ShellContent
        Title="PageNew"
        ContentTemplate="{DataTemplate local:PageNew}"
        Route="PageNew" />



</Shell>
```
```Csharp
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
```
https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-7.0

https://github.com/dotnet/docs-maui/blob/main/docs/fundamentals/shell/navigation.md



# Tap

```xml
        <Frame BackgroundColor="Yellow" WidthRequest="200" HeightRequest="100">
            <Frame.GestureRecognizers>
                <TapGestureRecognizer NumberOfTapsRequired="2" Command="{Binding TapCommand}"/>
            </Frame.GestureRecognizers>
            <Label Text="Нажать"/>
        </Frame>
```

# Удаление с помощью Swipe (Android)

```xml
<CollectionView.ItemTemplate>
                <DataTemplate x:DataType="{x:Type x:String}">

                    <SwipeView>
                        <SwipeView.RightItems>
                            <SwipeItems>
                                <SwipeItem Text="Delete"
                                           BackgroundColor="red"
                                           Command="{Binding Source={RelativeSource AncestorType={x:Type viewmodel:MainViewModel}},Path=DeleteCommand}"
                                           CommandParametr="{Binding .}"
                            />
                            </SwipeItems>
                        </SwipeView.RightItems>

                        <StackLayout Padding="0" VerticalOptions="Center">

                            <Entry Placeholder="..." Text="{Binding Test, Mode=TwoWay}"/>
                           
                            <Label Text="{Binding Test, StringFormat='Fruit: {0}',Mode=TwoWay}"
                               FontSize="20"/>

                            <Button WidthRequest="200" HeightRequest="30"
                                    Text="Написать"
                                    Command="{Binding SayTextCommand}"/>

                        </StackLayout>

                    </SwipeView>
                            
                </DataTemplate>
            </CollectionView.ItemTemplate>
```

# Тип данных ViewModel для Page

```xml
x:DataType="viewmodel:MainViewModel"
```

# Проверка доступа в интернет

```Csharp
 if(Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
    {
	Shell.Current.DisplayAlert("Uh Oh!","No Internet","Ok");
    }
```

# Animation

```Csharp
protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (this.AnimationIsRunning("TransitionAnimation"))
            return;

        var parentAnimation = new Animation();

        //Planets Animation
        parentAnimation.Add(0, 0.2, new Animation(v => imgMercury.Opacity = v, 0, 1, Easing.CubicIn));
        parentAnimation.Add(0.1, 0.3, new Animation(v => imgVenus.Opacity = v, 0, 1, Easing.CubicIn));
        parentAnimation.Add(0.2, 0.4, new Animation(v => imgEarth.Opacity = v, 0, 1, Easing.CubicIn));
        parentAnimation.Add(0.3, 0.5, new Animation(v => imgMars.Opacity = v, 0, 1, Easing.CubicIn));
        parentAnimation.Add(0.4, 0.6, new Animation(v => imgJupiter.Opacity = v, 0, 1, Easing.CubicIn));
        parentAnimation.Add(0.5, 0.7, new Animation(v => imgSaturn.Opacity = v, 0, 1, Easing.CubicIn));
        parentAnimation.Add(0.6, 0.8, new Animation(v => imgNeptune.Opacity = v, 0, 1, Easing.CubicIn));
        parentAnimation.Add(0.7, 0.9, new Animation(v => imgUranus.Opacity = v, 0, 1, Easing.CubicIn));

        //Intro Box Animation
        parentAnimation.Add(0.7, 1, new Animation(v => frmIntro.Opacity = v, 0, 1, Easing.CubicIn));

        //Commit the animation
        parentAnimation.Commit(this, "TransitionAnimation", 16, 3000, null, null);
    }
```





