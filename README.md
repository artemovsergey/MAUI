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
![https://metanit.com/sharp/maui/pics/4.1.png]()

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
### Шрифт

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
## Button

```xml
<Button Text="Нажми" FontSize="22" BorderWidth="1"
                BackgroundColor="LightPink" TextColor="DarkRed"
        HorizontalOptions="Center" VerticalOptions="Center"
        Clicked="OnButtonClicked" />
```

## Label

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

## Поле

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

## Многострочное поле

```xml
 <Editor FontSize="16" HeightRequest="200" />
```

## BoxView

BoxView представляет прямоугольную область. Обычно BoxView применяется для создания окрашенных областей, либо в качестве декоративного примитивного графического оформления к другим элементам.

``xml
<BoxView Color="LightBlue" WidthRequest="150" HeightRequest="150" CornerRadius="8"
                 HorizontalOptions="Center" VerticalOptions="Center" />
```

## ScrollView

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

## DataPicker

```xml
<DatePicker Format="d" DateSelected="DateSelected">
    <DatePicker.MinimumDate>7/10/2022</DatePicker.MinimumDate>
    <DatePicker.MaximumDate>7/20/2022</DatePicker.MaximumDate>
</DatePicker>
```

## TimePicker

```xml
 <TimePicker x:Name="timePicker" Time="17:00:00" PropertyChanged="TimePicker_PropertyChanged" />
```

## Stepper

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

## Slider

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

## Switch

```xml
<Switch x:Name="switcher" IsToggled="True" Toggled="switcher_Toggled" />
```
```Csharp
void switcher_Toggled(object sender, ToggledEventArgs e)
    {
        label.Text = $"Значение {e.Value}";
    }
```

## CheckBox

```xml
<CheckBox x:Name="statusCheckBox" CheckedChanged="CheckBox_CheckedChanged" />
```
```Csharp
void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        statusLabel.Text = $"Статус: {(e.Value ? "женат/замужем" : "холост/не замужем")}";
    }
```

## RadioButton

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

## Picker

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

## TableView

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

## DisplayAlert

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

## DisplayActionSheet

```Csharp
    async void AlertButton_Clicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Выбрать язык", "Отмена", "Удалить", "C#", "JavaScript", "Java");
        actionLabel.Text = action;
    }
```

## DisplayPromtAsync

```xml
 async void AlertButton_Clicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Логин", "Введите имя:", "OK", "Отмена");
        nameLabel.Text = name;
    }
```

## ProgressBar

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



## ActivityIndicator

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
