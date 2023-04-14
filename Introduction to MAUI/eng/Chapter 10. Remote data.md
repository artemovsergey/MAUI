# CHAPTER 10

# Remote Data

In this chapter, you will be exploring the topic of remote data, learning
what exactly it is, types of it, how to interact with it, and what to consider
when doing so. You will then build upon this learning by building a new
widget:, the Weather Widget, to display the current weather. This will
be done by interacting directly with the Open Weather API. You will get
exposure to handling HTTP requests and responses with an API, how
to handle the response being in a JSON format and the varying levels of
flexibility when mapping to the JSON data. You will finish off by simplifying
the implementation with a fantastic NuGet package that generates source
code for you, simply from an interface you define to represent the web
service.

# What Is Remote Data?

Remote data is any data that is sourced from outside the device your
application is running on. This can range from querying a web API in order
to obtain data, utilizing a cloud-based database provider, images hosted
online, streaming video or audio data, and more.
The vast majority of applications will interact with some form of
remote endpoint in order to pull data. In this world of constantly changing
data, this becomes an essential part of practically any application.

# Considerations When Handling Remote Data

There can be quite a few concepts to consider when interacting with
remote data. You will be explicitly addressing these as you build your new
widget, but I want to draw your attention to them before you start.

# Loading Times

One of the worst experiences for a user is to tap on a button or open a new
page/application and just see the application lock up while it is loading data.
The user will think that the application has crashed and, in fact, platforms
like Android and Windows will likely indicate that the application has
crashed/locked up if the load takes too long. Thankfully .NET offers you the
async and await keywords. They are not essential but they really do make
your life easier. There could be an entire chapter or even book on this topic;
however, my good friend Brandon Minnick has already covered a lot of this
in his AsyncAwaitBestPractices repository on GitHub. If you haven’t checked
it out before, I thoroughly recommend you do if you want to dig deeper;
https://github.com/brminnick/AsyncAwaitBestPractices.
A common use case is to display to the user that the application is busy
loading. This can be with a simple ActivityIndicator, which loads the
platform-specific spinner/loading icon users should feel familiar with, or you
can make use of the animation features I covered to show something more
involved. With this loading display you then initiate your web service call. If
you get a response, you display the result of that response in your application
(e.g., items in a shopping list or, in your scenario, the user’s current weather).

# Failures

During the building of a recent application, some of the most valuable
testing I did was to install the application and then ride the London
Underground and observe just how flaky a mobile phone’s data
connection really can be.

There are two key questions to consider when dealing with network
connectivity issues:
1. What does the user need to know?
2. How does the application need to recover?

# Security

As a developer of applications, it is essential that you maintain the trust
that your users put in you with regard to keeping their data safe. With
this in mind, you should always choose HTTPS over HTTP. In fact, most
platforms won’t allow HTTP traffic by default to avoid it accidentally being
used. There are ways to disable the prevention of HTTP traffic; however, I
strongly advise against it, so I won’t cover how to do so in this book.
I strongly recommend that as you build your applications you consider
security as a top priority. The Open Web Application Security Project
(OWASP) is a non-profit foundation that works to improve the security
of software and it provides some really great resources and guidance
on what you should really consider when building websites and mobile
applications. As a good starting point, look at their Mobile Application
Security Testing Guide repository on GitHub at https://github.com/
OWASP/owasp-mastg/.
Quite often APIs will require levels of authentication that complicate
the flow to pulling data from them. This typically happens when your
application needs to consume data specific to a user and not just the API
itself. I won’t be covering this scenario in this book, but I recommend
reading up on OAuth2.0 with a good initial resource at www.oauth.com/
oauth2-servers/mobile-and-native-apps/. Additionally, specific APIs
such as the GitHub API will likely provide good documentation on how
to use their specific authentication mechanism. So with this in mind, I
recommend referring to the documentation for the API that you wish to
integrate with.

# Webservices

Webservices act as a mechanism to query or obtain data from a remote
server. They typically offer many advantages to developers building them
because they provide the ability to charge based on usage, protect the
developer’s intellectual property, and other reasons.

# The Open Weather API

You will be calling the Open Weather API and specifically version 2.5 of
the OneCall API. The API is free to use with some usage limits. You can
call it up to 60 times per minute and 1,000,000 calls per month, which will
certainly be fine for this scenario.
For the initial work, you will be using a fixed latitude and longitude of
20.7984 and -156.3319, respectively, which if you look it up represents
Maui, Hawaii. You will enabling the application to use the device’s current
location information in the next chapter.

# Creating an Open Weather Account

You will be required to create an account. To do so, navigate to the website
at https://home.openweathermap.org/users/sign_up and create the
account. Note that you do not need to enter any billing details. You can
use it entirely for free. If you breach the call limits, the API will simply fail
instead of running into accidental charges.

# Creating an Open Weather API key

Next, you need to create an API key, which can be done on the following
page at https://home.openweathermap.org/api_keys. Keep a copy of this
API key ready for when you eventually use it later in this chapter. Don’t
worry too much for now as you can return to the above web page and
access the key.

# Examining the Data

Before you dive into writing some code, you should take a look at the API
and the data that it returns. In fact, the API offers a lot more detail than
you really need. You can consume the details in case you want to use them
in the future; however, this does bring in some possible drawbacks. It
increases the complexity of reading through the data if you need to debug
things, and it also increases the amount of data that needs to be retrieved
by your application. In the mobile world, this can be expensive!
Given the above, you can make the following web service call which
includes following details:
• Calls version 2.5 of the OneCall API
• Supplies latitude of 20.7984
• Supplies longitude of -156.3319
• Supplies units of metric, meaning you will receive
degrees Celsius
• Supplies exclude of minutely, hourly, daily, alerts,
meaning you will only receive the current weather data
• Supplies the api key you created in the previous section
The full URL that you need to call looks as follows:
https://api.openweathermap.org/data/2.5/onecall?lat=20.7984&
lon=-156.3319&units=metric&exclude=minutely,hourly,daily,alerts
&appid=APIKEY
You can open this in any web browser to view the following response
back. You can see the key details that you will need for your application
highlighted in bold.

```json
{
 "lat": 20.7984,
 "lon": -156.3319,
"timezone": "Pacific/Honolulu",
 "timezone_offset": -36000,
 "current": {
 "dt": 1663101650,
 "sunrise": 1663085531,
 "sunset": 1663129825,
 "temp": 20.77,
 "feels_like": 21.15,
 "pressure": 1017,
 "humidity": 86,
 "dew_point": 18.34,
 "uvi": 7.89,
 "clouds": 75,
 "visibility": 10000,
 "wind_speed": 5.66,
 "wind_deg": 70,
 "weather": [
 {
 "id": 501,
 "main": "Rain",
 "description": "moderate rain",
 "icon": "10d"
 }
 ],
 "rain": {
 "1h": 1.78
 }
 }
}
```
# Using System.Text.Json

In order to consume and deserialize the contents of the JSON returned to
you, you need to use one of the following two options:
• Newtonsoft.Json (requires a NuGet package)
• System.Text.Json
Newtonsoft has been around for many years and is a go-to option for
many developers. System.Text.Json has become its successor and is my
recommendation for this scenario, especially as it is backed by Microsoft
and James Newton-King, the author of Newtonsoft, works for Microsoft.
Let’s go ahead and use System.Text.Json as it is the recommended way
to proceed and ships with .NET MAUI out of the box.
Now that you have seen what the data looks like, you can start to build
the model classes that will allow you to deserialize the response coming
back from the API.

# Creating Your Models

I highlighted that you really don’t need all of the information that is
returned from the API. Thankfully you only need to build your model to
cover the detail that you require and allow the rest to be ignored during the
deserialization process.
Let’s create the model classes you require. You do this in the reverse
order that they appear in the JSON due to the fact that the outer elements
need to refer to the inner elements.
First, add a new folder to keep everything organized and call it
Communications.
Now, add a new class file and call it Weather.cs.
namespace WidgetBoard.Communications;

```Csharp
namespace WidgetBoard.Communications;
public class Weather
{
 public string Main { get; set; }
 public string Icon { get; set; }
 public string IconUrl => $"https://openweathermap.org/img/
wn/{Icon}@2x.png";
}
```
Your Weather class maps to the weather element in the JSON returned
from the API. You can see that you are mapping to the main and icon
elements and you have added a calculated property that returns a URL
pointing to the icon provided by the Open Weather API. The last property
you are mapping, IconUrl, is yet another great example of remote data.
The API provides you with an icon that can be rendered inside your
application representing the current weather of the location. Based on
the example in your original JSON, you see the icon value of 10d. This
represents rain.

---
You will notice that the casing of your property names does not
match the element names in the JSON. This will actually result in the
deserialization process to mapping as you require. When you get to
the deserialization part, you will see how to handle this scenario.
---

Your next model class to add should be called Current and, similarly
to the Weather class, it will map to the element that matches its name:
current. Your Current class file should have the following contents:

```Csharp
using System.Text.Json.Serialization;
namespace WidgetBoard.Communications;
public class Current
{
 [JsonPropertyName("temp")]
 public double Temperature { get; set; }
 public int Sunrise { get; set; }
 public int Sunset { get; set; }
 public Weather[] Weather { get; set; }
}
```
This class will contain an array of Weather, the Sunset and Sunrise
times, and the current Temperature. With the Temperature property
mapping, you can see how it is possible to map from a property in your
model to an element in JSON that has a different name. This functionality
is extremely valuable when building your own models because it allows
you to name the properties to provide better context. I personally prefer to
avoid abbreviations and stick with explicit names to make the intentions of
the code clear.
Your final model class to add should be called Forecast.cs and will
have the following contents:

```Csharp
namespace WidgetBoard.Communications;
public class Forecast
{
 public string Timezone { get; set; }
 public Current Current { get; set; }
}
```

This class maps to the top-level element in the returned JSON. You are
mapping to the Timezone element and also the Current, which will contain
your previously mapped values.

Now that you have created the model classes that can be mapped to
the JSON returned from the Open Weather API, you can proceed to calling
the API in order to retrieve that JSON.

# Connecting to the Open Weather API

Before you start to build the implementation for accessing the API, you
are going to create an interface to define what it should do. This has the
added benefit that when you wish to unit test any class that depends on
the IWeatherForecastService, you can supply a mock implementation
rather than requiring that the unit tests will access the real API. I will cover
why that is a bad idea in Chapter 14, but the simple answer here is that you
have a limited number of calls you are allowed to make for free and you
don’t want unit tests eating that allowance up.

```Csharp
namespace WidgetBoard.Communications;
public interface IWeatherForecastService
{
 Task<Forecast> GetForecast(double latitude, double
longitude);
}
```

A common naming approach to classes that interact with APIs is
to add the suffix Service to show that it provides a service to the user.
Therefore let’s create your service by adding a new class file and calling it
WeatherForecastService.cs. Add the following contents:

```Csharp
using System.Text.Json;
namespace WidgetBoard.Communications;
public class WeatherForecastService : IWeatherForecastService
{
 private readonly HttpClient httpClient;
private const string ApiKey = "ENTER YOUR KEY";
 private const string ServerUrl = "https://api.open
weathermap.org/data/2.5/onecall?";
 public WeatherForecastService(HttpClient httpClient)
 {
 this.httpClient = httpClient;
 }
 public async Task<Forecast> GetForecast(double latitude,
double longitude)
 {
 var response = await httpClient
 .GetAsync($"{ServerUrl}lat={latitude}&lon={longitude}
&units=metric&exclude=minutely,hourly,daily,alerts&
appid={ApiKey}")
 .ConfigureAwait(false);
 response.EnsureSuccessStatusCode();
 var stringContent = await response.Content
 .ReadAsStringAsync()
 .ConfigureAwait(false);
 var options = new JsonSerializerOptions
 {
 PropertyNameCaseInsensitive = true
 };
 return JsonSerializer.Deserialize<Forecast>(string
Content, options);
 }
}
```
You added a fair amount into this class file so let’s walk through it step
by step and cover what it does.
First is the HttpClient backing field, which is set within the
constructor and supplied by the dependency injection layer. You also have
constants representing the URL of the API and also your API key that you
generated in the earlier sections.
Next is the main piece of functionality in the GetForecast method. The
first line in this method handles connecting to the Open Weather API and
passing your latitude, longitude, and API key values. You also make sure
to set ConfigureAwait(false) because you do not need to be called back
on the initial calling thread. This helps to boost performance a little as it
avoids having to wait until the calling thread becomes free.

```Csharp
var response = await httpClient
 .GetAsync($"{ServerUrl}lat={latitude}&lon={longitude}
&units=metric&exclude=minutely,hourly,daily,alerts&appid=
{ApiKey}")
 .ConfigureAwait(false);
```
Then you make sure that the request was handled successfully
by calling

```Csharp
response.EnsureSuccessStatusCode();
```
---
Note that the above will throw an exception if the status code
received was not a 200 (success ok).
---

Then you extract the string content from the response.

```Csharp
var stringContent = await response.Content
 .ReadAsStringAsync()
 .ConfigureAwait(false);
```
Finally, you make use of the System.Text.Json NuGet package you
installed earlier in order to deserialize the string content into the model
classes that you created.

```Csharp
var options = new JsonSerializerOptions
{
 PropertyNameCaseInsensitive = true
};

return JsonSerializer.Deserialize<Forecast>(stringContent,
options);
```

I mentioned earlier that you had to explicitly opt-in to matching
your property names to the JSON elements case-insensitively.
You can see from the above code that you can do this through
the use of the JsonSerializerOptions class and specifically the
PropertyNameCaseInsensitive property.
Now that you have created the service, you should add your weather
widget and make use of the service.

# Creating the WeatherWidgetView

In order to create your widget, you need to add a new view. Add a
new .NET MAUI ContentView (XAML) into your Views folder and
call it WeatherWidgetView. This results in two files being created:
WeatherWidgetView.xaml and WeatherWidgetView.xaml.cs. You need to
update both files.

**WeatherWidgetView.xaml**
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:viewmodels="clr-namespace:WidgetBoard.ViewModels"
 x:Class="WidgetBoard.Views.WeatherWidgetView"
 x:DataType="viewmodels:WeatherWidgetViewModel">
 <VerticalStackLayout>
 <Label
 Text="Today"
 FontSize="20"
 VerticalOptions="Center"
 HorizontalOptions="Start"
 TextTransform="Uppercase" />
 <Label
 VerticalOptions="Center"
 HorizontalOptions="Center">
 <Label.FormattedText>
 <FormattedString>
 <Span
 Text="{Binding Temperature,
StringFormat='{0:F1}'}"
 FontSize="60"/>
 <Span
 Text="°C" />
 </FormattedString>
 </Label.FormattedText>
 </Label>
 <Label
 Text="{Binding Weather}"
 FontSize="20"
 VerticalOptions="Center"
HorizontalOptions="Center" />
 <Image
 Source="{Binding IconUrl}"
 WidthRequest="100"
 HeightRequest="100"/>
 </VerticalStackLayout>
</ContentView>
```
Some of the above XAML should feel familiar based on the previous
code you have written. Some bits are new, so let’s cover them.
Label.FormattedText enables you to define text of varying formats
inside a single Label control. This can be helpful especially when parts of
the text changes dynamically in length and therefore result in the contents
moving around. In your example, you are adding a Span with a text binding
to your Temperature property in the view model and a second Span with
the degrees Celsius symbol.
The second new concept is the use of the Image control. The binding
on the Source property looks relatively straightforward; however, it is
worth noting that .NET MAUI works some magic for you here. You are
binding a string to the property. Under the hood, .NET MAUI converts
the string into something that can resemble an image source. In fact, the
underlying type is called ImageSource. Further to this, it will inspect your
string and if it contains a valid URL (e.g., starts with https://), then it will
aim to load it as a remote image rather than looking in the applications set
of compiled resources. .NET MAUI will also potentially handle caching of
images for you to help reduce the amount of requests sent in order to load
images from a remote source. In order to make use of this functionality,
you need to provide a UriImageSource property on your view model rather
than the string property.
The process of converting from one type to another is referred to as
TypeConverters and can be fairly common in .NET MAUI. I won’t go into
detail on how they work, so please go to the Microsoft documentation site
at https://learn.microsoft.com/dotnet/api/system.componentmodel.
typeconverter.

**WeatherWidgetView.xaml.cs**

You also need to make the following adjustments to the
WeatherWidgetView.xaml.cs file. This part is required because you
haven’t created a common base class for the widget views. At times there
can be good reason to create them; however, because you want to keep
the visual tree as simple as possible, there isn’t a common visual base
class to use.

```Csharp
using WidgetBoard.ViewModels;
namespace WidgetBoard.Views;
public partial class WeatherWidgetView : ContentView,
IWidgetView
{
 public WeatherWidgetView()
 {
 InitializeComponent();
 }
 public IWidgetViewModel WidgetViewModel
 {
 get => BindingContext as IWidgetViewModel;
 set => BindingContext = value;
 }
}
```

Now that you have created your widget view, you should create the
view model that will be paired with it.

# Creating the WeatherWidgetViewModel

The view model that you need to create in order to represent the weatherrelated data that can be bound to the UI requires some work that you
are familiar with and some that you are not as familiar with. Let’s
proceed to adding the familiar bits and then walk through the newer
concepts. First, add a new class file in the ViewModels folder and call it
WeatherWidgetViewModel.cs. The initial contents should be modified to
look as follows:

```Csharp
using WidgetBoard.Communications;
namespace WidgetBoard.ViewModels;
public class WeatherWidgetViewModel : BaseViewModel,
IWidgetViewModel
{
 public const string DisplayName = "Weather";
 public int Position { get; set; }
 public string Type => DisplayName;
}
```
The above should look familiar as it is very similar to the
ClockWidgetViewModel you created earlier on in the book. Now you need
to add in the weather-specific bits.
First, add a dependency on the IWeatherForecastService you created
a short while ago.

```Csharp
private readonly IWeatherForecastService
weatherForecastService;
public WeatherWidgetViewModel(IWeatherForecastService
weatherForecastService)
{
 this.weatherForecastService = weatherForecastService;
 Task.Run(async () => await LoadWeatherForecast());
}
private async Task LoadWeatherForecast()
{
 var forecast = await weatherForecastService.
GetForecast(20.798363, -156.331924);
 Temperature = forecast.Current.Temperature;
 Weather = forecast.Current.Weather.First().Main;
 IconUrl = forecast.Current.Weather.First().IconUrl;
}
```
Inside of your constructor you keep a copy of the service and you also
start a background task to fetch the forecast information. Quite often you
wouldn’t start something like this from within a constructor; however,
given that you know your view model will only be created when it is being
added to the UI, this is perfectly acceptable.
Finally, you need to add the properties that your view wants to bind to.

```Csharp
private string iconUrl;
private double temperature;
private string weather;
public string IconUrl
{
 get => iconUrl;
 set => SetProperty(ref iconUrl, value);
}
public double Temperature
{
 get => temperature;
 set => SetProperty(ref temperature, value);
}
public string Weather
{
 get => weather;
 set => SetProperty(ref weather, value);
}
That’s all you need in the view model for now. You can now register the
widget and get it ready for your first test run.
```
# Registering Your Widget

You first need to make use of a NuGet package in order to follow some
recommended practices for the registration and usage of the HttpClient
class. Go ahead and add the Microsoft.Extensions.Http NuGet package and
then take a look at how to use it.

Right-click the WidgetBoard solution.

• Select Manage NuGet Packages.

• Search for Microsoft.Extensions.Http.

• Select the correct package.

• Click Add Package.

Inside your MauiProgram.cs file you need to add the following lines
into the CreateMauiApp method:

```Csharp
builder.Services.AddHttpClient<WeatherForecastService>();
builder.Services.AddSingleton<IWeatherForecastService,
WeatherForecastService>();
WidgetFactory.RegisterWidget<WeatherWidgetView, WeatherWidgetVi
ewModel>(WeatherWidgetViewModel.DisplayName);
builder.Services.AddTransient<WeatherWidgetView>();
builder.Services.AddTransient<WeatherWidgetViewModel>();
```
The above code registers your widget’s view and view models with the
dependency injection layer and also registers it with your WidgetFactory,
meaning it can be created from your add widget overlay.

# Testing Your Widget

If you run your application and add a weather widget, you can see the
result in Figure 10-1.

![image](https://user-images.githubusercontent.com/26972859/231479651-c8fff206-c623-4c9d-997e-374a20d832c9.png)
Figure 10-1. Application running and showing your weather widget
rendering correctly

This works fine provided you have a good network connection. The
moment you have a slow connection or even no connection, you will
notice that things don’t load quite as expected. In fact, you will likely
observe a crash. You knew this could happen based on your earlier
investigation into the things you need to consider when handling remote
data. Let’s now apply some techniques to handle these scenarios.

# Adding Some State

The first thing you want to do is to consider the different possible states
that your process can be in. There are three key scenarios that you need to
handle and provide visual feedback to your users on:

1. The widget is loading the data.

2. The widget has the data.

3. The widget has encountered an issue loading the data.

Let’s handle these three scenarios.
First, create an enum that will represent the above scenarios.

```Csharp
public enum State
{
 None = 0,
 Loading = 1,
 Loaded = 2,
 Error = 3
}
```
You also want to modify your loading code in the view model to make
use of this new State.

```Csharp
private async Task LoadWeatherForecast()
{
try
 {
 State = State.Loading;
 var forecast = await weatherForecastService.GetForecast
(20.798363, -156.331924);
 Temperature = forecast.Current.Temperature;
 Weather = forecast.Current.Weather.First().Main;
 IconUrl = forecast.Current.Weather.First().IconUrl;
 State = State.Loaded;
 }
 catch (Exception ex)
 {
 State = State.Error;
 }
}
```
And you also need to add the State property and backing field.

```Csharp
private State state;
public State State
{
 get => state;
 set => SetProperty(ref state, value);
}
```
# Converting the State to UI

This section may well deserve a more prominent setting; however, to allow
the content to flow through this book, I opted to only expose parts based
on the context of the topics you are learning as you build your application. 
Quite often in .NET MAUI there are scenarios where you wish to bind a
piece of data to the UI but that data type does not match the desired type in
the UI. To avoid having to add additional properties and potentially adding
view-related information into your view models, you can make use of a
concept called converters. A converter enables you to define how a specific
data type can be converted from its type to another type. I always find the
best way to cover something like this is to see it in action so let’s create a
converter to convert from your new State enum above into a bool value
ready for binding to the IsVisible property in your view.
Add a new folder and call it Converters and then add a new class
file and call it IsEqualToStateConverter.cs and then you can add the
following contents:

```Csharp
using System.Globalization;
using WidgetBoard.ViewModels;
namespace WidgetBoard.Converters;
public class IsEqualToStateConverter : IValueConverter
{
 public State State { get; set; }
 public object Convert(object value, Type targetType, object
parameter, CultureInfo culture)
 {
 if (value is State state)
 {
 return state == State;
 }
 return value;
 }
 public object ConvertBack(object value, Type targetType,
object parameter, CultureInfo culture)
{
 throw new NotImplementedException();
 }
}
```
The IValueConverter interface allows you to define how a value
passed in can be converted. Implementations of this interface are for use
within a binding using the Converter property.

# Displaying the Loading State

It is worth noting that at times data can be loaded very quickly and the act
of showing a spinner can provide a negative experience if it flashes very
quickly. Of course, it is impossible to know which calls will take longer
than others as there are so many factors which can affect the network. At
times like this, I like to make sure that there is always a minimum amount
of time that you display the spinner so that there isn’t this weird flash to
the user.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView
 xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
 xmlns:viewmodels="clr-namespace:WidgetBoard.ViewModels"
 xmlns:converters="clr-namespace:WidgetBoard.Converters"
 x:Class="WidgetBoard.Views.WeatherWidgetView"
 x:DataType="viewmodels:WeatherWidgetViewModel">
 <ContentView.Resources>
 <converters:IsEqualToStateConverter
 x:Key="IsLoadingConverter"
 State="Loading" />
 </ContentView.Resources>
  <VerticalStackLayout>
 <Label
 Text="Today"
 FontSize="20"
 VerticalOptions="Center"
 HorizontalOptions="Start"
 TextTransform="Uppercase" />
 <!-- Loading -->
 <VerticalStackLayout
 IsVisible="{Binding State, Converter={StaticResource
IsLoadingConverter}}">
 <ActivityIndicator
 IsRunning="{Binding State, Converter={Static
Resource IsLoadingConverter}}" />
 <Label
 Text="Loading weather data" />
 </VerticalStackLayout>
 </VerticalStackLayout>
</ContentView>
```

# Displaying the Loaded State

In order to handle the error state, you need to add another instance of
your IsEqualToStateConverter, this time with the State property set
to Loaded.

```xml
<converters:IsEqualToStateConverter
 x:Key="HasLoadedConverter"
 State="Loaded" />
```
You can then use this converter in a binding to show/hide the
following UI:

```xml
<!-- Loaded -->
<VerticalStackLayout
 IsVisible="{Binding State, Converter={StaticResource
HasLoadedConverter}}">
 <Label
 VerticalOptions="Center"
 HorizontalOptions="Center">
 <Label.FormattedText>
 <FormattedString>
 <Span
 Text="{Binding Temperature, String
Format='{0:F1}'}"
 FontSize="60"/>
 <Span
 Text="°C" />
 </FormattedString>
 </Label.FormattedText>
 </Label>
 <Label
 Text="{Binding Weather}"
 FontSize="20"
 VerticalOptions="Center"
 HorizontalOptions="Center" />
 <Image
 Source="{Binding IconUrl}"
 WidthRequest="100"
 HeightRequest="100"/>
</VerticalStackLayout>
```

# Displaying the Error State

In order to handle the error state, you need to add another instance of your
IsEqualToStateConverter, this time with the State property set to Error.

```xml
<converters:IsEqualToStateConverter
 x:Key="HasErrorConverter"
 State="Error" />
```
You can then use this converter in a binding to show/hide the
following UI:

```xml
<!-- Error -->
<VerticalStackLayout
 IsVisible="{Binding State, Converter={StaticResource
HasErrorConverter}}">
 <Label
 Text="Unable to load weather data" />
 <Button
 Text="Retry"
 Command="{Binding LoadWeatherCommand}" />
</VerticalStackLayout>
```
You may have noticed that you have added a Button and bound its
command to the view model. You need to add this to your view model if
you wish to compile and run the application. The aim of the Button is to
allow the user to request a retry of loading the weather information if the
Error state is being shown.
Inside your WeatherWidgetViewModel.cs file you need to make the
following change:

```Csharp
public ICommand LoadWeatherCommand { get; }
```
Then you need to update the constructor with the changes in bold:

```Csharp
public WeatherWidgetViewModel(WeatherForecastService weatherForecastService)
{
 this.weatherForecastService = weatherForecastService;
 LoadWeatherCommand = new Command(async () => await
LoadWeatherForecast());
 Task.Run(async () => await LoadWeatherForecast());
}
```
This means that when a load fails for whatever reason, the user will
have the option to press the retry button and the widget will attempt to
load the weather details again. It will walk through the states you added, so
the UI will show the different UI options to the user as this happens.
This type of failure handling is considered manual. There are ways to
automatically handle retries through a package called Polly.

# Simplifying Webservice Access

The previous sections covered how you can interact directly with a web
service at the most basic level. It requires a bit of setup but thankfully in
your scenario this wasn’t too complicated. Some web services can require
a lot more setup or even return a lot more data.
When building your applications, the aim is to write as little code
as possible as it reduces the amount of code you need to maintain. This
statement isn’t advocating for writing shortened code that can be difficult
for a human to understand but instead stating that you want to focus on
the details that are core to the application that you are building and not
things like consuming a web service. Sure, you want to know that you are
but having to write the underlying bits through the use of HttpClient can
become cumbersome. Thankfully there are packages out there that can
help you!

# Prebuilt Libraries

I first recommend that you investigate whether the web service provider
also provides a client library to make the consumption easier. Quite
often providers supply a library, especially when there is a layer of
authentication required. There are no official client libraries for the Open
Weather API; however, there are a number of NuGet packages that provide
some support for using the API.

# Code Generation Libraries

If no client library is available, you can look to using an auto generation
package to reduce the amount of code you need to write. Refit is a fantastic
package for this purpose. It allows you to define an interface representing
the web service call and then Refit will do the rest.
So why didn’t I just start here? In a new project, you probably would
do so, but I always strongly feel that you need to gain an understanding of
what packages like Refit are doing before you really start to use them. This
can be invaluable when things go wrong and you have to debug exactly
what and why things are going wrong!

# Adding the Refit NuGet Package

Let’s go ahead and add the Refit.HttpClientFactory NuGet package and
then take a look at how to use it.

• Right-click the WidgetBoard solution.

• Select Manage NuGet Packages.

• Search for Refit.HttpClientFactory.

• Select the correct package.

• Click Add Package.

Now that you have the NuGet package installed, you can use it.
Open your IWeatherForecastService.cs file and make the following
modifications shown in bold:

```Csharp
using Refit;
namespace WidgetBoard.Communications;
public interface IWeatherForecastService
{
 [Get("/onecall?lat={latitude}&lon={longitude}&units=metric&
exclude=minutely,hourly,daily,alerts&appid=APIKEY")]
 Task<Forecast> GetForecast(double latitude, double
longitude);
}
```
The fantastic part of the above code is that you do not need to write
the implementation. Refit uses source code generators to do it for you! In
fact, it means you can delete your WeatherForecastService class as it is no
longer required.
The final change you are required to make is to change how you
register the IWeatherForecastService with your MauiAppBuilder in the
MauiProgram.cs file. Open it up and make the following changes.
First, add the using statement.

```Csharp
using Refit;
```
Then replace

```Csharp
builder.Services.AddSingleton<IWeatherForecastService,WeatherForecastService>();
```
with

```Csharp
builder.Services
 .AddRefitClient<IWeatherForecastService>()
 .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://
api.openweathermap.org/data/2.5"));
```
This new line of code makes use of the Refit extension methods that
enable you to consume an implementation of IWeatherForecastService
whenever you register a dependency on that interface. It is worth
reiterating that the implementation for the IWeatherForecastService
is automatically generated for you through the Refit package. For further
reading on this package, I thoroughly recommend their website at
https://reactiveui.github.io/refit/.

# Further Reading

You have added some complexities into your application in order to
handle the scenario when webservice access doesn’t load as expected.
There are two really great libraries that can really help to reduce the
amount of code you need to write around these parts.

# Polly

To quote the about section on the GitHub repository,
Polly is a .NET resilience and transient-fault-handling library
that allows developers to express policies such as Retry, Circuit
Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner.
Polly can really help to reduce writing complex code around the failure
scenarios of webservice access. I thoroughly recommend checking out the
GitHub repository at https://github.com/App-vNext/Polly.

# StateContainer from CommunityToolkit.Maui

You had to build in converters and apply IsVisible bindings to control
which view is being displayed when your widget is in a specific state. The
StateContainer reduces that overhead so you “just” need to define the
states and the views for those states.
If you love to write less code, I recommend checking out the
Microsoft documentation at https://learn.microsoft.com/dotnet/
communitytoolkit/maui/layouts/statecontainer.

# Summary

In this chapter, you

• Learned about remote data

• Learned how you can interact with it

• Covered the common considerations

• Looked a concrete example with the Open Weather API

• Built your own implementation to consume the Open
Weather API

• Covered how to consume the data returned

• Talked through scenarios where things can go wrong

• Provided implementations to handle those scenarios

• Looked at how you can reduce the complexity of your
implementation with Refit

• Added in your Weather Widget

In the next chapter, you will

• Learn about permissions on the various platforms and
how to request them.

• Learn how to use the Geolocation API.

• Cover how to write your own platform-specific
interaction when necessary.

• Discover how to tweak the UI based on the platform on
which your application is running.

• Learn to tweak the UI through the use of the handler
architecture.

# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch10.

# Extra Assignment

There are so many possibilities for accessing remote data in your
application! Here are some extra widgets I would like you to consider
creating

# TODO Widget

The go-to example application to build in tutorials is a TODO application.
I would like you to expand upon this idea and add a TodoWidget into your
application. There are several TODO APIs that you could utilize to do this.
Do you have a favorite TODO service that you use? I personally like the 
Microsoft TODO option. There is some good documentation over on the
Microsoft pages to help get you started at https://learn.microsoft.com/
graph/todo-concept-overview

# Quote of the Day Widget

I know I certainly like to be inspired with a feel-good quote. Why don’t
you consider building a widget to refresh daily and show you a quote of
the day?
The They Said So Quotes API offers a good API for doing this exact job
with the documentation hosted at https://quotes.rest/.
The other concept that you will need to consider is how to trigger your
Scheduler class to trigger the refresh at midnight.

# NASA Space Image of the Day Widget

I love some of the images that come from NASA. It is so cool to be able
to see into the reaches of space! Quite handily, they have a decent set of
APIs that can enable you to build a widget and show off these images! The
documentation on the NASA website really is great and should be able to
guide you through the process of accessing the data you need. The NASA
API documentation can be found at https://api.nasa.gov/.
I really can’t wait to see these widgets in action!


































