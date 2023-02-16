В .NET MAUI графический интерфейс состоит из страниц. Каждая страница представляет собой объект класса Page. Страница занимает все пространство экрана. То есть то, что мы видим на экране мобильного устройства или в окне десктопного приложения - это страница. Приложение может иметь одну или несколько страниц.


# Тематический сайт

MAUIMAN.dev
https://vladislavantonyuk.github.io/

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















