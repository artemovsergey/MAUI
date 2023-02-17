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






