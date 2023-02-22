В .NET MAUI графический интерфейс состоит из страниц. Каждая страница представляет собой объект класса Page. Страница занимает все пространство экрана. То есть то, что мы видим на экране мобильного устройства или в окне десктопного приложения - это страница. Приложение может иметь одну или несколько страниц.


# Тематический сайт

MAUIMAN.dev

https://vladislavantonyuk.github.io/

https://jesseliberty.com/

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

Команды также могут вызывать асинхронные методы. Это достигается использованием ключевых слов asyncи awaitпри указании Executeметода:

```Csharp
DownloadCommand = new Command (async () => await DownloadAsync ());
```

Это указывает на то, что DownloadAsyncметод является Taskи его следует ожидать:

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

# MauiProgram.cs

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


            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<PersonViewModel>();

            return builder.Build();
        }
    }
    
    
```

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

### Stack Layout

```xml
    <StackLayout Spacing="5"
                 Orientation="Horizontal">
        <Label Text="Первая метка2" TextColor="Red" />
        <Label Text="Вторая метка" TextColor="Blue" />
        <Label Text="Третья метка" TextColor="Green" />
    </StackLayout>
```

### Absolute Layout

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
### Grid

```xml
        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition />
                <ColumnDefinition />
                <ColumnDefinition />
            </Grid.ColumnDefinitions>
            <Grid.RowDefinitions>
                <RowDefinition />
                <RowDefinition />
            </Grid.RowDefinitions>
        </Grid>
```

