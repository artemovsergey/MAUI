# Введение в .NET MAUI

# Создание кроссплатформенных приложений на .NET

**.NET Multi-platform App UI** или сокращенно **MAUI** представляет кроссплатформенный фреймворк от компании Microsoft для создания нативных мобильных и десктопных приложений с использованием языка программирования C# и языка разметки XAML. С помощью .NET MAUI можно разрабатывать приложения под такие операционные системы как Android, iOS, macOS и Windows, используя при этом один и тот же код.

!(https://metanit.com/sharp/maui/pics/1.1.png)[]

Первая версия MAUI вышла в мае 2022 года, далее в ноябре 2022 года вышли обновления для .NET 7. Однако стоит отметить, что сам фреймворк .NET Multi-platform App UI фактически представляет эволюцию другого фреймворка - Xamarin Forms, который имеет более чем 10-летнюю историю и который с 2016 года был куплен и развивался компанией Microsoft. Поэтому, имя опыт работы с Xamarin Forms, не составит больших проблем перейти на .NET MAUI.

Также стоит отметить, что .NET MAUI - это opensource-фреймворк, код которого можно найти в репозитории на github по адресу https://github.com/dotnet/maui

Зачем использовать .NET Multi-platform App UI, какие преимущества она несет? При разработке приложения сразу под несколько платформ (iOS, Android, Windows, macOS) разработчики сталкиваются со следующими трудностями:

различие в подходах построение графического интерфейса так или иначе влияет на разработку. Разработчики вынуждены подстраивать приложение под требования к интерфейсу на конкретной платформе

разные API - различие в программных интерфейсах и реализациях тех или иных функциональностей также требует от программиста учет этих специфических особенностей

разные платформы для разработки. Например, чтобы создавать приложения для iOS нам необходима соответствующая среда - Mac OS X и ряд специальных инструментов, типа XCode. А в качестве языка программирования выбирается Objective-C или Swift. Для Android мы можем использовать самый разный набор сред - Android Studio, Intellij IDEA, Eclipse и т.д. Но здесь для подавляющего большинства приложений применяется Java или Kotlin.

А для создания приложений под Windows используется Visual Studio, а в качестве языков - C#, F#, VB.NET, C++

Такой диапазон платформ, средств разработки и языков программирования не может положительно сказываться на сроках создания приложений, и, в конечном счете, на денежных средствах, выделяемых на разработку. Было бы очень эффективно иметь один инструмент, который позволял легко и просто создавать приложения сразу для всех платформ. И именно таким инструментом и является платформа MAUI.

Преимущества разработки на MAUI:

В процессе разработки создается единый проект, который использует общий код для всех платформ

MAUI предоставляет прямой доступ к нативным API каждой платформы, в том числе к аппаратным возможностям платформ

При создании приложений мы можем использовать платформу .NET и язык программирования C# (а также F#), который является достаточно производительным, и в тоже время ясным и простым для освоения и применения

Богатая коллекция встроенных элементов управления

Поддержка привязки данных

Возможности настройки поведения визуального интерфейса и встроенного функционала

Богатые возможности по работе с графикой

Наличие hot reload, что упрощает разработку

Поддерживаемые платформы
На момент написания данной статьи .NET Multi-platform App UI (.NET MAUI) официально поддерживает следующие платформы:

Android 5.0 (API 21) и выше

iOS 10 и выше

macOS 10.13 и выше (с помощью Mac Catalyst)

Windows 11 and Windows 10 версии 1809 и выше (с использованием WinUI 3).

Кроме того, подсистема .NET MAUI Blazor часть выше поднимает планку поддерживаемых версий:

Android 7.0 (API 24) и выше

iOS 14 и выше

macOS 11 и выше (с помощью Mac Catalyst)

Также неофициально сообществом поддерживается Linux - проект maui-linux. Кроме того, компания Samsung осуществляет поддержку для ОС Tizen.

Как работает MAUI
.NET MAUI объединяет API операционных систем Android, iOS, macOS и Windows в один единый API, который позволяет написать один общий код для всех поддерживаемых операционных систем и при необходимости добавлять для каждой отдельной платформы специфические для нее функциональности. Схематически работу .NET MAUI можно представить следующим образом:

!(https://metanit.com/sharp/maui/pics/1.2.png)

.NET MAUI предоставляет единый фреймворк для создания приложений. Однако в процессе работы он опирается на ряд субплатформ, через которые идет взаимодействие с каждой отдельной операционной системой: .NET for Android, .NET for iOS, .NET for macOS и Windows UI 3 (WinUI 3). И в общем случае код приложения сначала обращается к платформе .NET MAUI, а та затем обращается к субплатформе для конкретной ОС. Хотя фреймворк также позволяет напрямую обращаться коду приложения к этим субплатформам.

Все эти субплатформы работают поверх библиотеки классов .NET 6 Base Class Library (BCL). Эта библиотека позволяет абстрагироваться от деталей реализации конкретной платформы и зависит от среды выполнения .NET, в которой выполняется код. Для Android, iOS и macOS среду выполнения для приложения предоставляет фреймворк Mono (реализация .NET). На Windows среда выполнения предоставляется Win32.

Таким образом, разработчик может определить общую единую логику для приложения для каждой отдельной платформы. И также при необходимости может задействовать специфические для каждой платформы функциональности.

# Инструменты разработки

Разрабатывать приложения формально можно на Windows и MacOS. Однако если стоит задача разрабатывать приложения под iOS и MacOS, то для этого необходим MacOS.

В качестве среды разработки используется Visual Studio 2022 и выше. На MacOS в этом случае используется Visual Studio for Mac. При большом желании также можно использовать Visual Studio Code.

# Установка .NET MAUI

Чтобы разрабтывать в Visual Studio приложения на платформе .NET MAUI в программе для установщика Visual Studio необходимо выбрать и установить элемент Разработка с помощью .NET Multi-Platform App UI"Разработка мобильных приложений на .NET":

https://metanit.com/sharp/maui/pics/1.3.png

Аналогично eсли мы хотим в Visual Studio for Mac создавать проекты под MAUI, то при установке необходимо отметить соответствующий пункт:

!(https://metanit.com/sharp/maui/pics/1.34.png)[]


# Создание проекта .NET MAUI

Создадим первый проект, который будет использовать .NET MAUI. Для этого мы будем использовать среду разработки Visual Studio 2022. Для работы с .NET MAUI в Visual Studio имеется ряд шаблонов проектов:

- .NET MAUI App: стандартный тип проекта, который использует .NET MAUI
- .NET MAUI Blazor App: тип проекта, который использует фреймворк Blazor
- .NET MAUI Class library: тип проекта, для создания библиотеки классов под .NET MAUI


Для быстрого поиска шаблона проекта можно отфильтровать шаблоны по типу "MAUI"

!(https://metanit.com/sharp/maui/pics/1.4.png)

Те же самые типы проектов доступны в Visual Studio for Mac:

!(https://metanit.com/sharp/maui/pics/1.35.png)

Для создания первого проекта выберем первый тип - .NET MAUI App.

После выбора типа проекта на следующем шаге укажем для него каталог и имя, к примеру, назовем его HelloApp:

!(https://metanit.com/sharp/maui/pics/1.5.png)

На следующем шаге укажем последнюю доступную версию .NET (в моем случае .NET 7):

!(https://metanit.com/sharp/maui/pics/1.6.png)

Нажмем на кнопку Create, и Visual Studio создаст новый проект:

!(https://metanit.com/sharp/maui/pics/1.7.png)

Вкратце рассмотрим его структуру:

Папка Platforms содержит подкаталоги для каждой отдельной платформы. Каждая папка содержит файлы кода для взаимодействия с определенной платформой

Папка Resources содержит файлы ресурсов, применяемых в приложении, в частности, файлы шрифтов, иконок, изображений и т.д.

App.xaml: определяет ресурсы, общие для всего приложения

App.xaml.cs: файл с кодом C#, с которого начинается выполнение приложения

AppShell.xaml: определяет общий визуальный интерфейс многостраничного приложения

AppShell.xaml.cs: файл с кодом C#, который связан с файлом AppShell.xaml и определяет связанную с ним программную логику

MainPage.xaml: файл с визуальным интерфейсом для единственной страницы MainPage в виде xaml

MainPage.xaml.cs: файл, который содержит логику страницы MainPage на языке C#

MauiProgram.cs: содержит класс MauiProgram, который определяет стартовый класс приложения (по умолчанию класс App) и ряд общих для приложения настроек

Общая организация приложения MAUI выглядит следующим образом:

!(https://metanit.com/sharp/maui/pics/1.8.png)

Каждая из платформ, файлы которых расположены в папке Platforms, использует класс MauiProgram для запуска приложения .NET MAUI. Класс MauiProgram устанавливает класс, который запускается при запуске приложения и который устанавливает визуальный облик приложения. Например, откроем код данного класса:

```Csharp
using Microsoft.Extensions.Logging;
 
namespace HelloApp;
 
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
 
        return builder.Build();
    }
}
```

Строка

```Csharp
.UseMauiApp<App>()
```

как раз определяет класс, запускаемый при старте приложения и задающий внешний приложения. По умолчанию это класс App из файла App.xaml.cs.

Класс App использует класс AppShell из файла AppShell.xaml.cs и связанный с ним файл AppShell.xaml для установки, как страницы будут структурированы внутри приложения. Для этого у класса App устанавливается свойство MainPage:

```Csharp

namespace HelloApp;
 
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
 
        MainPage = new AppShell();
    }
}
```

Класс AppShell представляет оболочку, которая позволяет создавать многостраничное приложение и устанавливает главную страницу приложению, которая в данном случае представлена классом MainPage из файлов MainPage.xaml и MainPage.xaml.cs. Так, обратимся к файлу AppShell.xaml

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="HelloApp.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:HelloApp"
    Shell.FlyoutBehavior="Disabled">
 
    <ShellContent
        Title="Home"
        ContentTemplate="{DataTemplate local:MainPage}"
        Route="MainPage" />
 
</Shell>
```

Данный xml-файл с помощью атрибута Route="MainPage" элемента ShellContent как раз определяет, что главной страницей, которую мы увидим при запуске, будет MainPage.

Таким образом, при запуске приложения мы увидим на экране содержимое, которое устанавливается страницей MainPage. Собственно это единственная определенная в проекте страница. Перейдем к определению этой страницы. Оно разбито на два файла. Файл MainPage.xaml представляет визуальный интерфейс страницы в виде кода XAML, который аналогичен HTML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
              
    <ScrollView>
        <VerticalStackLayout
            Spacing="25"
            Padding="30,0"
            VerticalOptions="Center">
 
            <Image
                Source="dotnet_bot.png"
                SemanticProperties.Description="Cute dot net bot waving hi to you!"
                HeightRequest="200"
                HorizontalOptions="Center" />
                 
            <Label
                Text="Hello, World!"
                SemanticProperties.HeadingLevel="Level1"
                FontSize="32"
                HorizontalOptions="Center" />
             
            <Label
                Text="Welcome to .NET Multi-platform App UI"
                SemanticProperties.HeadingLevel="Level2"
                SemanticProperties.Description="Welcome to dot net Multi platform App U I"
                FontSize="18"
                HorizontalOptions="Center" />
 
            <Button
                x:Name="CounterBtn"
                Text="Click me"
                SemanticProperties.Hint="Counts the number of times you click"
                Clicked="OnCounterClicked"
                HorizontalOptions="Center" />
 
        </VerticalStackLayout>
    </ScrollView>
  
</ContentPage>
```

Так как для создания страниц используется класс ContentPage, то в качестве корневого элемента в этом файле определен именно элемент ContentPage.

Все его содержимое состоит из элемента ScrollView, который представляет прокручиваемую область. ScrollView в свою очередь содержит элемент VerticalStackLayout, который позволяет расположить вложенные элементы в виде вертикального стека.

Внутри VerticalStackLayout определен ряд элементов, которые собственно определяют видимые на экране элемента. В данном случае в основном это элементы Label, которые служат для вывода простого или отформатированного текста на экран, элемент Image для вывода изображения и элемент Text для вывода текста.

Также в проекте есть и файл с кодом логики страницы - файл MainPage.xaml.cs:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    int count = 0;
 
    public MainPage()
    {
        InitializeComponent();
    }
 
    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;
 
        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";
 
        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
```

Стоит отметить, что чтобы связать оба определения страницы, класс MainPage определяется здесь как частичный (partial), а в файле MainPage.xaml в определении страницы ContentPage указан класс, название которого совпадает с классом из MainPage.xaml.cs:

```xml
x:Class="HelloApp.MainPage"
```

В классе определен конструктор, который с помощью вызова InitializeComponent() создает визуальный интерфейс, определенный в файле MainPage.xaml.

Кроме того в классе по умолчанию определен метод OnCounterClicked(), который выступает в качестве обработчика кнопки, которая определена в файле MainPage.xaml. При нажатии на кнопку данный метод будет увеличивать значение переменной count.

Теперь рассмотрим запуск данного кода на различных платформах.

# Первое приложение на Android

Рассмотрим запуск простейшего проекта по умолчанию на ОС Android. Но вначале рассмотрим, как приложение вообще запускается на Android. Для этого обратимся к проекте к папке Platforms/Android/, которая содержит файлы для взаимодействия с ОС Android.

!(https://metanit.com/sharp/maui/pics/1.11.png)

Здесь нас будут интересовать два файла: MainActivity.cs и MainApplication.cs.

# Взаимодействие с MAUI с Android

Одним из основных понятий приложения на Android является activity, которая обычно представляет отдельный экран. То есть то, что видим на экране мобильного устройства Android, как правило, представляет activity. И для определения интерфейса приложения Android в данной папке по умолчанию создается файл MainActivity.cs:

```Csharp
using Android.App;
using Android.Content.PM;
using Android.OS;
 
namespace HelloApp;
 
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
```
Здесь видно, что по умолчанию код класса MainActivity ничего не определяет, просто наследуется от другого класса MauiAppCompatActivity. Однако если мы обратимся к классу MauiAppCompatActivity:

```Csharp
using Android.OS;
using AndroidX.AppCompat.App;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
 
namespace Microsoft.Maui
{
    public partial class MauiAppCompatActivity : AppCompatActivity
    {
        protected virtual bool AllowFragmentRestore => false;
 
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            // содержимое опущено для краткости
            base.OnCreate(savedInstanceState);
 
            // определяем графический интерфейс окна приложения
            this.CreatePlatformWindow(MauiApplication.Current.Application, savedInstanceState);
        }
    }
}
```

Все классы MAUI, предназначенные для взаимодействия с платформой Android, можно найти на github по адресу https://github.com/dotnet/maui/tree/main/src/Core/src/Platform/Android

Класс MauiAppCompatActivity в методе OnCreate, который вызывается ОС Android при старте приложения, вызывает метод CreatePlatformWindow, который определен для одного из родительских классах (класс Activity) и в котором фактически и определяется графический интерфейс окна.

Для установки интерфейса применяется свойство MauiApplication.Current.Application, то есть что хранится в этом свойстве, то и будет фактически представлять интерфейс приложения. Значение этого свойства представляет объект класса MainApplication, который определяется в файле MainApplication.cs:

```Csharp
using Android.App;
using Android.Runtime;
 
namespace HelloApp;
 
[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }
 
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
```

Для определения интерфейса переопределяется абстрактный метод CreateMauiApp(), в котором вызывается метод MauiProgram.CreateMauiApp() (из файла MauiProgram.cs), который создает объект MauiApp и является точкой входа в приложение MAUI.

Таким образом, главный класс, который для ОС Android определяет визуальное содержимое - MainActivity использует класс MainApplication, а тот - класс MauiApp.

# Запуск проекта

Если рабочая машина, на которой ведется разработка, поддерживает виртуализацию, то мы можем для отладки приложений использовать эмуляторы: эмулятор Android от Microsoft или Android Player.

Если у нас есть устройство с ОС Android, то мы можем его использовать для тестирования. Для этого надо подключить это устройство к компьютеру с помощью USB-кабеля. А на самом мобильном устройстве установить режим разработчика в параметрах.

Для установки устройства перейдем в Visual Studio в поле устройств запуска и в выпадающем списке выберем пункт Android local Devices и в подменю для этого пункта выберем подключенное устройство с Android.

!(https://metanit.com/sharp/maui/pics/1.41.png)

Если же все настроено правильно, то в панели инструментов Visual Studio отобразит подключенное устройство. В моем случае это устройство Nexus 5X. Попутно в списке могут отображаться и другие подключенные устройства и эмуляторы.

И после запуска проекта на выполнение на мобильном устройстве отобразится интерфейс нашего приложения:

!(https://metanit.com/sharp/maui/pics/1.13.png)

# Управление настройками проекта и SDK

Если необходимо установить какую-то новую версию Android API или какие-то компоненты из Android SDK, которые должны применяться с приложением, необходимо перейти к меню Tools -> Android -> Android SDK Manager

!(https://metanit.com/sharp/maui/pics/1.19.png)

!(https://metanit.com/sharp/maui/pics/1.20.png)

Для управления настройками Android проекта можно перейти в свойства проекта. Здесь есть ряд опций для настройки. Прежде всего в секции Android Targets можно настроить целевую применяемую и минимальную версии Android API:

!(https://metanit.com/sharp/maui/pics/1.22.png)

В частности, здесь нам доступны следующие опции:

- Target the Android platform: при установке этого флажка .NET MAUI при построении проекта будет также создавать версию приложения для Android.

- Target .NET Runtime: применяемая версия .NET

- Target Android Framework: целевая версия Android

- Minimum Target Android Framework: минимальная версия Android, под которую создается приложение

Кроме того, в секции Android можно установить параметры манифеста Android, а также опции для публикации приложения.

# Первое приложение на Windows

Рассмотрим запуск простейшего проекта по умолчанию на Maui и C# на ОС Windows.

Выполнение приложения на Windows начинается с класса App, который определен в файлах App.xaml и App.xaml.cs в папке для Windows в каталоге Platforms:

!(https://metanit.com/sharp/maui/pics/1.9.png)

Например, обратимся к файлу App.xaml.cs из каталога Platforms/Windows:

```Csharp
using Microsoft.UI.Xaml;
 
namespace HelloApp.WinUI;
 
public partial class App : MauiWinUIApplication
{
    public App()
    {
        this.InitializeComponent();
    }
 
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
```

Метод CreateMauiApp() выполняет метод MauiProgram.CreateMauiApp(), который определен в файле MauiProgram.cs, и таким образом фактически запускает приложение приложение .NET MAUI

Для запуска на Windows на панели инструментов Visual Studio в поле выбора платформы для запуска установим значение Windows Machine:

!(https://metanit.com/sharp/maui/pics/1.10.png)

После запуска мы увидим окно приложения по умолчанию:

!(https://metanit.com/sharp/maui/pics/1.23.png)

# Настройка проекта

При создании приложения под Windows можно настроить в свойствах проекта ряд опций. В частности, в секции Windows Targets можно установить целевую и минимальную версию версию билда Windows, которые применяются при компиляции приложения:

!(https://metanit.com/sharp/maui/pics/1.24.png)

- Target the Windows platform: при установке этого флажка .NET MAUI при построении проекта будет также создавать версию приложения для Windows.

- Target .NET Runtime: применяемая версия .NET

- Target Windows Framework: целевая версия Windows

- Minimum Target Windows Framework: минимальная версия Windows, под которую создается приложение