# Привязка

## Введение в привязку. BindingContext и объект Binding

Привязка данных или data binding является одним из ключевых аспектов при создании приложений на платформе .NET Multi-platform App UI. Привязка позволяет связать два свойства разных объектов таким образом, что изменения свойства одного объекта автоматически приводили к изменению свойства другого объекта.

Привязка данных состоит из двух компонентов:

- источник привязки (source) - кто привязывается

- цель привязки (target) - к кому идет привязка

Привязка осуществляется от свойства источника к свойству цели. И когда происходит изменение источника, механизм привязки автоматически обновляет также и цель.

!(https://metanit.com/sharp/maui/pics/5.1.png)

Объект-цель привязки должен представлять объект BindableObject, а свойство, к которому осуществляется привязка, должно быть свойством BindableProperty. Поскольку большинство визуальных элементов в .NET MAUI и C# наследуются от класса BindableObject, то в качестве цели привязки будут, как правило, выступать визуальные элементы.

А вот источником привязки может выступать любой объект языка C#. Однако, надо понимать, что цель привязки должна автоматически изменяться при изменении источника, поэтому нам нужно извещать систему о изменении свойств источника привязки. В .NET MAUI, да и вообще на платформе .NET, в качестве подобного механизма извещения выступает интерфейс INotifyPropertyChanged. То есть нужно реализовать данный интерфейс в объекте-источнике.

Объект BindableObject как раз реализует INotifyPropertyChanged. Поэтому если источником привязки является стандартный визуальный элемент из .NET MAUI, то автоматически будет изменяться и цель привязки. Но если в качестве источника выступает не BindableObject, а какой-нибудь объект простого класса C#, то, как писалось выше, этот класс должен реализовать INotifyPropertyChanged.

Есть два способа установить привязку - с помощью свойства BindingContext и с помощью объекта Binding. Рассмотрим на примерах, как устанавливается привязка в коде C# и в коде XAML.

## Свойство BindingContext

Для установки привязки у объекта-цели устанавливается свойство BindingContext. В качестве значения оно принимает источник привязки.

### Привязка в коде C#

Для привязки в коде после установки свойства BindingContext у объекта-цели привязки необходимо также вызвать метод SetBinding(), который свяжет свойство объекта-цели со свойством объекта-источника.

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Label label = new Label();
            Entry entry = new Entry();
 
            // Устанавливаем привязку
            // источник привязки - entry, цель привязки - label
            label.BindingContext = entry;
            // Связываем свойства источника и цели
            label.SetBinding(Label.TextProperty, "Text");
 
            StackLayout stackLayout = new StackLayout
            {
                Children = { label, entry },
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
```
Здесь объект-цель привязки - label, а источник привязки - entry. То есть привязка идет от label к entry. Выражение

```Csharp
label.SetBinding(Label.TextProperty, "Text")
```
устанавливает привязку свойства TextProperty элемента label к свойству Text источника привязки - элемента Entry.

Причем, что важно у объекта цели Label привязка устанавливается именно у свойства BindableProperty, которым является TextProperty, а не просто для свойства Text.

В итоге при вводе данных в текстовое поле будет изменяться значение свойства Text у объекта entry. А это изменение автоматически скажется на объекте label и изменит его свойство TextProperty.

!(https://metanit.com/sharp/maui/pics/5.3.png)

И таким образом, нам не надо вручную обрабатывать изменения текста в текстовом поле Entry, все за нас делает механизм привязки данных.

## Привязка в XAML

Аналогично можно установить привязку и в xaml-коде.

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

В XAML действует тот же принцип. Для установки привязки для объекта цели задается свойство BindingContext. Но теперь его значение имеет другую форму: {x:Reference entryBox} - расширению разметки x:Reference передается имя элемента источника. То есть в данном случае источником выступает объект с именем entryBox.

Далее устанавливается сама привязка к свойству:

```xml
Text="{Binding Text}"
```

Здесь также устанавливается привязка к свойству Text. Выражение привязки заключается в фигурные скобки и состоит из слова Binding, после которого указывается свойство источника-привязки. То есть свойство Text объекта label привязано к свойству Text объекта entryBox.

## Объект Binding

Другой способ привязки представляет объект Binding. Например:

```Csharp
class StartPage : ContentPage
{
    public StartPage()
    {
        Label label = new Label();
        Entry entry = new Entry();
 
        // определяем объект привязки: Source - источник, Path - его свойство
        Binding binding = new Binding { Source = entry, Path = "Text" };
        // установка привязки для свойства TextProperty
        label.SetBinding(Label.TextProperty, binding);
 
        StackLayout stackLayout = new StackLayout()
        {
            Children = { entry, label},
            Padding = 20
        };
        Content = stackLayout;
    }
}
```

Здесь у объекта Binding настраиваются ряд свойств. В частности, свойство Source указывает на источник привязки, а свойство Path - на свойство источника. В данном случае источником служит объект Entry, а свойством - его свойство Text.

Далее в метод SetBinding() в качестве второго параметра передается объект Binding, инкапсулирующий все опции привязки.

В итоге мы получаем то же самое, что и в прошлой теме: свойство Text объекта Label привязано к свойству Text объекта Entry.

!(https://metanit.com/sharp/maui/pics/5.5.png)

Тоже самое можно прописать и в XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="20">
        <Label x:Name="label" Text="{Binding Source={x:Reference entry}, Path=Text}" />
        <Entry x:Name="entry" />
    </StackLayout>
</ContentPage>
```

Таким образом, нам необязательно использовать свойство BindingContext для установки привязки к источнику. Более того использование объекта Binding имеет свои преимущества. В частности, с помощью BindingContext мы можем установить только один источник привязки для какого-либо элемента. Но что если мы хотим, чтобы одно свойство у Label было привязано к одному элемену, другое свойство - к другому элементу? В этом случае можно использовать только вышеописанный способ.

Также мы можем одновременно устанавливать и BindingContext, и использовать объект Binding для определения опций привязки, и в этом случае объект Binding будет переопределять BindingContext, если, к примеру, в BindingContext указан один источник привязки, а в объекте Binding другой.

## BindableObject и BindableProperty

Объект, который выступает в качестве привязки данных, то есть к которому идет привязка, в .NET MAUI должен представлять класс BindableObject. В реальности стандартные визуальные элементы в .NET MAUI, например, кнопки, метки, текстовые поля, контейнеры компоновки и прочие наследуются от этого класса. Поэтому мы можем привязывать свойства различных объектов к свойствам визуальных элементов.

Отличительной особенностью класса BindableObject является то, что он содержит специальные типы свойств BindableProperty. обычные свойства по сути представляют обертку над BindableProperty. В другой технологии на платформе .NET - WPF есть похожая концепция - свойства зависимости (dependency property), которые имеют похожее назначение.

Например, в .NET MAUI есть класс Label, который является потомком класса BindableObject и у которого есть свойство Text. Через это свойство мы можем установить текст метки. Но в реальности это свойство выглядит следующим образом:

```Csharp
public static readonly BindableProperty TextProperty = 
    BindableProperty.Create(nameof(Text), typeof(string), typeof(Label), default(string), propertyChanged: OnTextPropertyChanged);
     
public string Text
{
    get { return (string)GetValue(TextProperty); }
    set { SetValue(TextProperty, value); }
}
```

Как правило, для каждого свойства BindableProperty создается обертка-обычное свойство. И название BindableProperty обычно имеет название обычного свойства + суффикс Property. Например, Text и TextProperty или TextColor и TextColorProperty.

Поэтому, если, допустим, у нас есть элемент Label, и мы хотим присвоить ему некоторый текст, то мы можем сдеать это двумя способами:

```Csharp
Label label = new Label();
// 1 способ - обычное свойство
label.Text = "Hello";
// 2 способ - через BindableProperty
label.SetValue(Label.TextProperty, "Hello METANIT.COM");
```

Для установки значения свойства через BindableProperty у объекта BindableObject вызывается метод SetValue(). В качестве первого параметра в метод передается само свойство (то есть в данном случае Label.TextProperty). Второй параметр передает значение для этого свойства.

Аналогично для получения значения свойства мы также можем применять два способа:

```Csharp
// 1 способ - через обычное свойство
string text = label.Text;
// 2 способ - через BindableProperty
text = (string)label.GetValue(Label.TextProperty);
```

Таким образом, определяются BindableObject и BindableProperty.

## Создание свойства BindableProperty

Допустим, мы хотим определить свое свойство BindableProperty в каком-то своем классе. Например, мы хотим расширить функционал класса Label, чтобы он включал некоторый тег, который присваивается метке. Для этого своздадим свой класс, производный от Label:

```Csharp
public class TagLabel : Label
{
    public static readonly BindableProperty TagProperty  = 
        BindableProperty.Create("Tag", // название обычного свойства
            typeof(string), // возвращаемый тип 
            typeof(TagLabel), // тип,  котором объявляется свойство
            "0"// значение по умолчанию
        );
    public string Tag
    {
        set
        {
            SetValue(TagProperty, value);
        }
        get
        {
            return (string)GetValue(TagProperty);
        }
    }
}
```

Данный класс располагается в главном проекте решения.

Для определения свойства BindableProperty используется метод BindableProperty.Create(). Этот метод возвращает объект BindableProperty и принимает в данном случае четыре параметра по порядку:

- Имя обычного свойства, которое будет оберткой. В данном случае свойство будет называться "Tag"

- Возвращаемый тип свойства. В данном случае тип string

- Название типа, в котором объявляется это свойство. Здесь тип TagLabel

- Значение по умолчанию. Здесь строка "0"

Это не все возможные параметры. Другие перегруженные версии метода BindableProperty.Create() могут принимать еще шесть параметров по порядку:

- defaultBindingMode - режим привязки

- validateValue - метод, который проверяет новое значение на корректность

- propertyChanged - метод, который вызывается при изменении свойства

- propertyChanging - метод, который вызывается перед изменением свойства

- coerceValue - метод корректировки нового значения

- defaultValueCreator - метод-генератор значения по умоланию

После определения класса и свойства они могут участвовать в привязке данных. Так, пусть у нас будет следующий код страницы:

```Csharp
class StartPage : ContentPage
{
    public StartPage()
    {
        TagLabel tagLabel = new TagLabel();
        Entry entry = new Entry();
        // Устанавливаем привязку
        // источник привязки - entry, цель привязки - tagLabel
        tagLabel.BindingContext = entry;
        // Связываем свойства источника и цели
        tagLabel.SetBinding(TagLabel.TagProperty, "Text");
        tagLabel.SetBinding(TagLabel.TextProperty, "Text");
 
        Label simpleLabel = new Label();
 
        simpleLabel.BindingContext = tagLabel;
        // устанавливаем привязку к свойству Tag объекта tagLabel
        simpleLabel.SetBinding(Label.TextProperty, "Tag");
        StackLayout stackLayout = new StackLayout()
        {
            Children = { tagLabel, entry, simpleLabel },
            Padding = 20
        };
        Content = stackLayout;
    }
}
```

Xamarin
Итак, здесь объект нашего класса TagLabel привязан к объекту entry. Причем сразу два свойства - Text и Tag у объекта TagLabel привязаны к свойству Text объекта Entry.

Также здесь есть простой объект Label, свойство Text которого привязано к свойству Tag объекта TagLabel. Поэтому при вводе символов в текстовое поле Entry синхронно будет изменяться значение свойства Tag у tagLabel, а это в свою очередь вызовет изменение свойства Text у простого объекта Label.


!(https://metanit.com/sharp/maui/pics/5.4.png)

Таким образом, определив свое свойство по типу BindableProperty впоследствии мы сможем осуществлять к нему привязку.

Определение аналогичной функциональности в XAML-коде:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="20">
        <local:TagLabel x:Name="tagLabel"
           BindingContext="{x:Reference entryBox}"
           Text="{Binding Text}"
           Tag="{Binding Text}" />
 
        <Entry x:Name="entryBox" />
 
        <Label x:Name="simpleLabel"
           BindingContext="{x:Reference tagLabel}"
           Text="{Binding Tag}" />
    </StackLayout>
</ContentPage>
```

# Режим привязки

Режим привязки определяет в какую сторону будет работать привязка. Все возможные режимы задаются с помощью значений из перечисления BindingMode:

- Default: значение по умолчанию. Для разных элементов и свойств может отличаться

- OneWay: при изменении в источнике изменяется цель (от источника к цели)

- OneTime: привязка установливается только однократно и больше не действует. То есть цель привязки получает начальное значение из источника привязки. Но при дальнейшем изменении в источнике привязка больше не действует

- OneWayToSource: при изменении в объекте-цели привязки изменяется объект-источник (то есть обратная привязка от цели к источнику)

- TwoWay: изменения в источнике воздействуют на цель привязки, а изменении в цели воздействуют на источник (то есть двусторонняя привязка)

По умолчанию стандартным значением для всех элементов и их свойств является значение OneWay, то есть односторонняя привязка от источника к цели. Однако ряд элементов и их свойств по умолчанию используют двустороннюю привязку:

- Свойство Date элемента DatePicker

- Свойство Text элементов Editor, Entry, SearchBar, и EntryCell

- Свойство IsRefreshing элемента ListView

- Свойство SelectedItem элемента MultiPage

- Свойства SelectedIndex и SelectedItem элемента Picker

- Свойство Value элементов Slider и Stepper

- Свойство IsToggled элементов Switch

- Свойство On элемента SwitchCell

- Свойство Time элемента TimePicker

Режим привязки по умолчанию в каждом свойстве BindingProperty хранится в свойстве DefaultBindingMode, например, получим режим привязки по умолчанию для текста элемента Label:

```Csharp
BindingMode labelTextBindingMode = Label.TextColorProperty.DefaultBindingMode;
```
Все типы привязки относительно просты и понятны. Поэтому рассмотрим только применение двусторонней привязки

Например, пусть у нас будет два текстовых поля Entry, которые связаны двусторонней привязкой. Для примера определим следуюшую страницу:

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Entry entry1 = new Entry { Margin = 10 };
            Entry entry2 = new Entry { Margin = 10 };
 
            entry2.BindingContext= entry1;
 
            entry2.SetBinding(Entry.TextProperty, "Text", BindingMode.TwoWay);
            StackLayout stackLayout = new StackLayout()
            {
                Children = { entry1, entry2},
                Padding = 10
            };
            Content = stackLayout;
        }
    }
}
```

Здесь для поля entry2 в качестве контекста привязки (BindingContext) выступает другое текстовое поле - entry1. Свойство Text у entry2 привязано к свойству Text элемента entry1. В качестве третьего параметра в метод element.SetBinding() передается значение BindingMode (в данном случае ). Стоит отметить, что поскольку для свойства Text по умолчанию действует двусторонняя привязка, то мы можем не указывать явным образом режим привязки:

```Csharp
entry2.SetBinding(Entry.TextProperty, "Text");
```

В итоге при вводе текста в один из текстовых полей автоматически будет изменяться и текст в другом поле.

!(https://metanit.com/sharp/maui/pics/5.6.png)

Если для установки привязки применяется объект Binding, то мы можем задать режим привязки через свойство Mode:

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Entry entry1 = new Entry { Margin = 10 };
            Entry entry2 = new Entry { Margin = 10 };
 
            // определяем объект привязки: Source - источник, Path - его свойство, Mode - режим привязки 
            Binding binding = new Binding { Source = entry1, Path = "Text", Mode = BindingMode.TwoWay };
            entry2.SetBinding(Entry.TextProperty, binding);
            StackLayout stackLayout = new StackLayout()
            {
                Children = { entry1, entry2},
                Padding = 10
            };
            Content = stackLayout;
        }
    }
}
```

Результат будет тот же самый, что и в предыдущем примере.

Установка режима привязки в XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="10">
        <Entry x:Name="entry1" Margin="10"/>
        <Entry x:Name="entry2" Margin="10"
           BindingContext="{x:Reference entry1}"
           Text="{Binding Path=Text, Mode=TwoWay}"/>
    </StackLayout>
</ContentPage>
```

Для установки режима привязки у расширения Binding применяется свойство Mode, которое принимает одну из констант BindingMode:

```xml
Text="{Binding Path=Text, Mode=TwoWay}"/>
```

Аналогичный пример без BindingContext:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="10">
        <Entry x:Name="entry1" Margin="10"/>
        <Entry x:Name="entry2" Margin="10"
           Text="{Binding Source={x:Reference Name=entry1}, Path=Text, Mode=TwoWay}"/>
    </StackLayout>
</ContentPage>
```

Но опять же, поскольку двусторонняя привязка для свойства Text элемента Entry действует по умолчанию, то нам не надо ее устанавливаеть явным образом. Тем не менее если вдруг мы хотим переопределить это поведение по умолчанию, то тогда как раз можно явным образом указать режим привязки. Однако надо учитывать, что не всегда режим привязки работает должным образом, например:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="10">
        <Entry x:Name="entry1" Margin="10"/>
        <Entry x:Name="entry2" Margin="10"
           Text="{Binding Source={x:Reference Name=entry1}, Path=Text, Mode=OneWay}"/>
    </StackLayout>
</ContentPage>
```

## Форматирование значения привязки и StringFormat

Встроенный механизм привязки позволяет форматировать привязанное значение. Для этого можно использовать либо свойство StringFormat класса Binding, либо параметр stringFormat метода SetBinding(). Но стоит отметить, что подобное форматирование имеет смысл, когда привязка идет к строковому свойству, либо к свойству, значение которого можно легко преобразовать к строке, например, числовые значения.

### Форматирование в SetBinding

Определим в коде C# следующую страницу:

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Entry entry = new Entry();
            Label label = new Label();
 
            label.BindingContext= entry;
            label.SetBinding(Label.TextProperty,"Text", stringFormat: "Сообщение: {0}");
            StackLayout stackLayout = new StackLayout()
            {
                Children = { entry, label},
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
```

Здесь текст метки привязан к тексту поля ввода. В методе SetBinding() параметру stringFormat передается следующее значение:

```xml
 stringFormat: "Сообщение: {0}"
 ```

 Плейсхолдер {0} указывает на начальное значение, которое передается от привязки.

 !(https://metanit.com/sharp/maui/pics/5.7.png)

 Аналогичный пример в XAML

 ```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="20">
        <Entry x:Name="entry" />
        <Label BindingContext="{x:Reference entry}" Text="{Binding Path=Text, StringFormat='Сообщение: {0}'}"/>
    </StackLayout>
</ContentPage>
 ```

 Свойству StringFormat передается некоторая строка в ординарных кавычках, которая указывает, как будет форматироваться значение.

 ## Свойство StringFormat объекта Binding

 В качестве альтернативы можно задать форматирование с помощью свойства StringFormat объекта Binding

 ```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Entry entry = new Entry();
            Label label = new Label();
 
            // свойство StringFormat форматирует значение
            Binding binding = new Binding { Source = entry, Path = "Text", StringFormat= "Сообщение: {0}" };
            label.SetBinding(Label.TextProperty, binding);
            StackLayout stackLayout = new StackLayout()
            {
                Children = { entry, label},
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
 ```

 Аналогичный пример в xaml:

 ```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="20">
        <Entry x:Name="entry" />
        <Label Text="{Binding Source={x:Reference Name=entry}, Path=Text, StringFormat='Сообщение: {0}'}"/>
    </StackLayout>
</ContentPage>
 
     
    <div style="margin-top:25px;">
<style>
    #yandex_rtb_R-A-201190-3{ width: 100%; height: 300px;overflow:hidden; }
     
    @media(min-width: 760px) { #yandex_rtb_R-A-201190-3{ max-width: 728px;  } }
    @media(min-width: 900px) { #yandex_rtb_R-A-201190-3{ max-width: 336px; } }
    @media(min-width: 1100px) { #yandex_rtb_R-A-201190-3{ max-width: 728px; } }
    @media(min-width: 1400px) { #yandex_rtb_R-A-201190-3{ max-width: 970px;} }
    </style>
    <div id="yandex_rtb_R-A-201190-3"></div>
    </div>
 
    <div class="nav"><p><a href="./6.3.php">Назад</a><a href="./">Содержание</a><a href="./6.5.php">Вперед</a></p></div>
    <div class="socBlock">
    <div class="share soctop">
    <ul>
    <li><a title="Поделиться в Вконтакте" rel="nofollow" class="fa fa-lg fa-vk"></a></li>
    <li><a title="Поделиться в Телеграм" rel="nofollow" class="fa fa-lg fa-telegram"></a></li>
    <li><a title="Поделиться в Одноклассниках" rel="nofollow" class="fa fa-lg fa-odnoklassniki"></a></li>
    <li><a title="Поделиться в Твиттере" rel="nofollow" class="fa fa-lg fa-twitter"></a></li>
    <li><a rel="nofollow" class="fa fa-lg fa-facebook"></a></li>
    </ul>
    </div>
    </div>
 
    <div class="commentABl" style="clear:both;margin: 25px 5px 15px 5px;">
    <style>
    #yandex_rtb_R-A-201190-8{ width: 100%; height: 250px; overflow:hidden;}
    @media(min-width: 500px) { #yandex_rtb_R-A-201190-8{ max-width: 336px; height: 280px; } }
    @media(min-width: 760px) { #yandex_rtb_R-A-201190-8{ max-width: 728px; height: 90px;  } }
    @media(min-width: 900px) { #yandex_rtb_R-A-201190-8{ max-width: 336px; height: 280px;  } }
    @media(min-width: 1100px) { #yandex_rtb_R-A-201190-8{ max-width: 728px; height: 90px;} }
    @media(min-width: 1400px) { #yandex_rtb_R-A-201190-8{ max-width: 970px; height: 90px;} }
    </style>
    <div id="yandex_rtb_R-A-201190-8"></div>
     
    </div>
 
    <div id="disqus_recommendations" style="margin-bottom: 12px;"><iframe id="dsq-app5717" name="dsq-app5717" allowtransparency="true" frameborder="0" scrolling="no" tabindex="0" title="Disqus" width="100%" src="https://disqus.com/recommendations/?base=default&f=metanitcom&t_u=https%3A%2F%2Fmetanit.com%2Fsharp%2Fmaui%2F6.4.php&t_d=.NET%20MAUI%20%D0%B8%20C%23%20%7C%20%D0%A4%D0%BE%D1%80%D0%BC%D0%B0%D1%82%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5%20%D0%B7%D0%BD%D0%B0%D1%87%D0%B5%D0%BD%D0%B8%D1%8F%20%D0%BF%D1%80%D0%B8%D0%B2%D1%8F%D0%B7%D0%BA%D0%B8%20%D0%B8%20StringFormat&t_t=.NET%20MAUI%20%D0%B8%20C%23%20%7C%20%D0%A4%D0%BE%D1%80%D0%BC%D0%B0%D1%82%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5%20%D0%B7%D0%BD%D0%B0%D1%87%D0%B5%D0%BD%D0%B8%D1%8F%20%D0%BF%D1%80%D0%B8%D0%B2%D1%8F%D0%B7%D0%BA%D0%B8%20%D0%B8%20StringFormat#version=eae384b350ceffb6029a893a061f19bd" style="width: 100% !important; border: none !important; overflow: hidden !important; height: 0px !important; display: inline !important; box-sizing: border-box !important;" horizontalscrolling="no" verticalscrolling="no"></iframe></div><div id="disqus_thread" style="margin-left:8px;margin-right:8px;clear:both;"><iframe id="dsq-app358" name="dsq-app358" allowtransparency="true" frameborder="0" scrolling="no" tabindex="0" title="Disqus" width="100%" src="https://disqus.com/embed/comments/?base=default&f=metanitcom&t_u=https%3A%2F%2Fmetanit.com%2Fsharp%2Fmaui%2F6.4.php&t_d=.NET%20MAUI%20%D0%B8%20C%23%20%7C%20%D0%A4%D0%BE%D1%80%D0%BC%D0%B0%D1%82%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5%20%D0%B7%D0%BD%D0%B0%D1%87%D0%B5%D0%BD%D0%B8%D1%8F%20%D0%BF%D1%80%D0%B8%D0%B2%D1%8F%D0%B7%D0%BA%D0%B8%20%D0%B8%20StringFormat&t_t=.NET%20MAUI%20%D0%B8%20C%23%20%7C%20%D0%A4%D0%BE%D1%80%D0%BC%D0%B0%D1%82%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5%20%D0%B7%D0%BD%D0%B0%D1%87%D0%B5%D0%BD%D0%B8%D1%8F%20%D0%BF%D1%80%D0%B8%D0%B2%D1%8F%D0%B7%D0%BA%D0%B8%20%D0%B8%20StringFormat&s_o=default#version=eac01b9b5184420d2a458e5de23912b6" style="width: 1px !important; min-width: 100% !important; border: none !important; overflow: hidden !important; height: 0px !important;" horizontalscrolling="no" verticalscrolling="no"></iframe></div>
    <script type="text/javascript">
    var disqus_shortname = 'metanitcom';
    (function() {
    var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
    dsq.src = '//' + disqus_shortname + '.disqus.com/embed.js';
    (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
    })();
    </script>
 ```

 ## Конвертеры значений

 Привязка позоляет связать два свойства, которые совпадают по типу данных. Например, свойство Text у класса Entry и свойство Text класса Label представляют тип string. Поэтому тут привязка проходит без проблем. Но в других случаях тип связанных свойств может отличаться. В большинстве случаев инфраструктра .NET MAUI сама знает, как конвертировать данные простейших типов. Однако в каких-то ситуациях этого оказывается недостаточно, особенно когда необходимо применить к значению привязки дополнительное форматирование.

Например, пусть у нас элемент Label привязан к элементу DatePicker и выводит выбранную дату. По умолчанию дата выводится в формате "MM/dd/yyyy hh:mm:ss":

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Label label = new Label();
            DatePicker datePicker = new DatePicker();
            label.BindingContext= datePicker;
            label.SetBinding(Label.TextProperty, "Date");
            StackLayout stackLayout = new StackLayout()
            {
                Children = { label, datePicker},
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
```

!(https://metanit.com/sharp/maui/pics/5.8.png)

Это не очень удобный формат. Возможно, мы захотим использовать совсем другие форматы.

Для преобразования одного значения к другому в процессе привязки используются специальные классы - конвертеры значений. К примеру, добавим в наш проект новый класс конвертера значений:

```Csharp
using System.Globalization;
 
namespace HelloApp
{
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
}
```

Прежде всего класс конвертера значений должен реализовать интерфейс IValueConverter. Этот интерфейс определяет два метода: Convert(), который преобразует пришедшее от привязки значение в тот тип, который понимается приемником привязки, и ConvertBack(), который выполняет противоположную операцию.

Оба метода принимают четыре параметра:

- object value: значение, которое надо преобразовать

- Type targetType: тип, к которому надо преобразовать значение value

- object parameter: вспомогательный параметр

- CultureInfo culture: текущая культура приложения

Метод Convert вызывается при передаче данных от источника привязки к цели, когда действуют режимы привязки OneWay и TwoWay. Параметр value представляет значение, которое пришло от источника. Метод должен возвратить значение к типу привязанного свойства цели.

Метод ConvertBack() вызывается, когда данные передаются от цели привязки к источнику при режимах привязки TwoWay и OneWayToSource. ConvertBack выполняет обратное преобразование к типу привязанного свойства источника.

В примере выше метод Convert возвращает дату в виде строки в формате "dd.MM.yyyy". То есть мы ожидаем, что в качесве параметра value будет передаваться объект DateTime.

Метод ConvertBack в данном случае не имеет значения, и поэтому он просто возвращает строковое представление текущей даты.

Для установки конвертере значений есть два способа: либо в методе SetBinding() устанавливается параметр converter, либо у объекта Binding устанавливается свойство :

Используем метод SetBinding():

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Label label = new Label();
            DatePicker datePicker = new DatePicker();
            label.BindingContext= datePicker;
            label.SetBinding(Label.TextProperty, "Date", converter: new DateTimeToLocalDateConverter());
            StackLayout stackLayout = new StackLayout()
            {
                Children = { label, datePicker},
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
```

!(https://metanit.com/sharp/maui/pics/5.9.png)

Аналогичный пример с объектом Binding:

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Label label = new Label();
            DatePicker datePicker = new DatePicker();
            Binding binding = new Binding { Source = datePicker, Path = "Date", Converter = new DateTimeToLocalDateConverter()};
            label.SetBinding(Label.TextProperty, binding);
            StackLayout stackLayout = new StackLayout()
            {
                Children = { label, datePicker},
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
```

Аналогичный пример в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
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

Так как класс конвертера располагается в пространстве имен текущего проекта, то, чтобы класс был доступен в xaml, надо подключить текущее пространство имен.

```xml
xmlns:local="clr-namespace:HelloApp"
```

В моем случае пространством имен является HelloApp, и оно проецируется на суффикс local

Затем в ресурсах создается объект привязки с ключом dateConverter.

И далее в самом выражении привязки мы подключем данный ресурс в качестве конвертера:

```xml
Converter={StaticResource dateConverter}
```

### Второй пример

Возьмем другой случай. Допустим, нам надо привязать цвет метки к вводимому в текстовое поле значение. Для этого определим следующий конвертер:

```Csharp
using System.Globalization;
 
namespace HelloApp
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorStr = value?.ToString().Trim().ToLower();
            switch (colorStr)
            {
                case "red": return Colors.Red;
                case "green": return Colors.Green; 
                case "blue": return Colors.Blue;
                default: return Colors.Gray;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as Color;
            var colorHex = color.ToHex();
            switch (colorHex)
            {
                case "#FF0000": return "red";
                case "#00FF00": return "green";
                case "#0000FF": return "blue";
                default: return "gray";
            }
        }
    }
}
```
Если вводится определенная строка, то конвертер возвращает определенный цвет. Теперь применим этот конвертер в XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:StringToColorConverter x:Key="colorConverter" />
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="20">
        <Entry x:Name="entry" />
        <Label Text="Hello METANIT.COM"
            TextColor="{Binding Source={x:Reference Name=entry}, 
                                Converter={StaticResource colorConverter},  
                                Path=Text}" />
    </StackLayout>
</ContentPage>
```

!(https://metanit.com/sharp/maui/pics/5.10.png)

Аналогичный пример в коде C#:

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Label label = new Label { Text = "Hello METANIT.COM" };
            Entry entry= new Entry();
            Binding binding = new Binding { Source = entry, Path = "Text", Converter = new StringToColorConverter()};
            label.SetBinding(Label.TextColorProperty, binding);
            StackLayout stackLayout = new StackLayout()
            {
                Children = { entry, label},
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
```

## Свойства и параметры конвертеров

### Свойства конвертеров

С помощью свойств конвертеров мы можем передать в ковертеры извне некоторые значения, которые затем можно использовать в процессе привязки. Например, определим следующий конвертер, который конвертирует имя пользователя в условный статус:

```Csharp
using System.Globalization;
 
namespace HelloApp
{
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
}
```

Конвертер содержит два свойства. Если свойство источника-привязки содержит значение "admin", то возвращаем значение свойства Approved, иначе возвращаем значение свойства Denied.

Применим этот конвертер на странице

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Label label = new Label();
            Entry entry= new Entry();
            Binding binding = new Binding { Source = entry, Path = "Text", 
                Converter = new StringToStatusConverter() { ApprovedStatus = "Доступ одобрен", DeniedStatus = "Доступ отклонен" } };
 
            label.SetBinding(Label.TextProperty, binding);
            StackLayout stackLayout = new StackLayout()
            {
                Children = { entry, label},
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
```

здесь элемент Label привязан к текстовому полю и использует конвертер привязки StringToStatusConverter, у которого свойства Approved и Denied хранят определенные сообщения. В итоге при вводе в текстовое поле мы увидим одно из этих сообщений

!(https://metanit.com/sharp/maui/pics/5.11.png)

Аналогичный пример в XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="20">
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
    </StackLayout>
</ContentPage>
```

### Состояние и стилизация элементов с помощью конвертеров

Но мы можем пойти дальше и использовать свойства не просто как хранилище некоторых значений, но и как хранилища некоторого состояния элемента. Например, настроим с помощью свойств конвертера стилизацию. Для этого определим следующий конвертер StringToStyleConverter:

```Csharp
using System.Globalization;
 
namespace HelloApp
{
    public class StringToStyleConverter : IValueConverter
    {
        public Style ApprovedStatus { get; set; }
        public Style DeniedStatus { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string username = (string)value;
            if (username == "admin") return ApprovedStatus;
            return DeniedStatus;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as Style;
 
            if (status == ApprovedStatus) return "admin";
            return "user";
        }
    }
}
```

Этот конвертер аналогичен предыдущему, только теперь его свойства представлять стиль - объект Style.

Применим этот конвертер в коде xaml на странице:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="20">
        <Entry x:Name="entry" />
        <Label>
            <Label.Style>
                <Binding Source="{x:Reference entry}" Path="Text">
                    <Binding.Converter>
                        <local:StringToStyleConverter>
                            <local:StringToStyleConverter.ApprovedStatus>
                                <Style TargetType="Label">
                                    <Setter Property="Text" Value="Доступ разрешен" />
                                    <Setter Property="TextColor" Value="Green" />
                                </Style>
                            </local:StringToStyleConverter.ApprovedStatus>
 
                            <local:StringToStyleConverter.DeniedStatus>
                                <Style TargetType="Label">
                                    <Setter Property="Text" Value="Доступ запрещен" />
                                    <Setter Property="TextDecorations" Value="Underline" />
                                    <Setter Property="TextColor" Value="Red" />
                                </Style>
                            </local:StringToStyleConverter.DeniedStatus>
                        </local:StringToStyleConverter>
                    </Binding.Converter>
                </Binding>
            </Label.Style>
        </Label>
    </StackLayout>
</ContentPage>
```

Здесь свойство Style элемента Label привязано к свойству Text элемента Entry. Благодаря конвертеру мы можем транслировать введенный текст в конкретный стиль. С помощью свойств конвертера в коде xaml задаем два стиля, в частности, текст, его цвет и атрибуты шрифта. В результате в зависимости от введенного текста мы увидим различную стилизацию метки:

!(https://metanit.com/sharp/maui/pics/5.12.png)

## Параметры конвертера

Класс Binding имеет свойство ConverterParameter, через которое можно передать данные в конвертер через третий параметр методов Convert() и ConvertBack(). К примеру, определим следующий конвертер:

```Csharp
using System.Globalization;
 
namespace HelloApp
{
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
}
```

В обоих методах через параметр parameter можно получить переданное извне значение, которое может представлять любой тип. Для примера, выполняем форматирование валют - если передается строка "euro", то добавляем в конец значения символ евро, иначе добавляем символ доллара.

Используем конвертер в коде C# страницы:

```Csharp
namespace HelloApp
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Entry entry= new Entry();
            Label euroLabel = new Label();
            Label dollarLabel = new Label();
            Binding euroBinding = new Binding 
            { 
                Source = entry, 
                Path = "Text", 
                Converter = new StringToCurrencyConverter(),
                ConverterParameter = "euro"
            };
            Binding dollarBinding = new Binding
            {
                Source = entry,
                Path = "Text",
                Converter = new StringToCurrencyConverter(),
                ConverterParameter = "dollar"
            };
 
            euroLabel.SetBinding(Label.TextProperty, euroBinding);
            dollarLabel.SetBinding(Label.TextProperty, dollarBinding);
            StackLayout stackLayout = new StackLayout()
            {
                Children = { entry, euroLabel, dollarLabel},
                Padding = 20
            };
            Content = stackLayout;
        }
    }
}
```
В данном случае для наглядности определяем две метки с разными привязками. Обе привязки используют один и тот же конвертер, однако передают ему разные значения для параметра

!(https://metanit.com/sharp/maui/pics/5.13.png)

Аналогичный пример в xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <StackLayout Padding="20">
        <Entry x:Name="entry" />
        <Label>
            <Label.Text>
                <Binding Source="{x:Reference entry}" Path="Text" ConverterParameter="euro">
                    <Binding.Converter >
                        <local:StringToCurrencyConverter />
                    </Binding.Converter>
                </Binding>
            </Label.Text>
        </Label>
        <Label>
            <Label.Text>
                <Binding Source="{x:Reference entry}" Path="Text" ConverterParameter="dollar">
                    <Binding.Converter >
                        <local:StringToCurrencyConverter />
                    </Binding.Converter>
                </Binding>
            </Label.Text>
        </Label>
    </StackLayout>
</ContentPage>
```

## Привязка к объектам. Интерфейс INotifyPropertyChanged

По умолчанию в качестве цели привязки обязательно должен выступать объект класса BindableObject, то источником привязки может быть любой объект любого класса. Однако если источником привязки является объект класса, который НЕ наследуется от BindableObject, то мы можем столкнуться с проблемой обновления данных при привязке. Например, пусть у нас в главном проекте есть класс Person:

```Csharp
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
```

Осущестим привязку к объекту класса Person:

```Csharp
namespace HelloApp;
 
class StartPage : ContentPage
{
    public StartPage()
    {
        Person tom = new Person { Name = "Tom", Age = 38 };
        Label nameHeaderLabel = new Label { Text = "Name" };
        Label ageHeaderLabel = new Label { Text = "Age" };
        Label nameValueLabel = new Label { FontAttributes = FontAttributes.Bold };
        Label ageValueLabel = new Label { FontAttributes = FontAttributes.Bold };
 
 
        Binding nameBinding = new Binding { Source = tom, Path = "Name" };
        Binding ageBinding = new Binding { Source = tom, Path = "Age" };
 
        nameValueLabel.SetBinding(Label.TextProperty, nameBinding);
        ageValueLabel.SetBinding(Label.TextProperty, ageBinding);
 
        Button btn = new Button { Text = "+", WidthRequest = 60, HorizontalOptions = LayoutOptions.Start };
        Label testLabel = new Label();
        // по нажатию увеличиваем возраст на 1
        btn.Clicked += (o, e) =>
        {
            tom.Age++;
            testLabel.Text = tom.Age.ToString();
        };
 
        StackLayout stackLayout = new StackLayout()
        {
            Children = { nameHeaderLabel, nameValueLabel, ageHeaderLabel, ageValueLabel, btn, testLabel },
            Padding = 20
        };
        Content = stackLayout;
    }
}
```

Здесь на странице элемент Label ageValueLabel привязан к свойству Age объекта Person. По нажатию на кнопку увеличиваем значение свойства Age у объекта tom и выводим это значение в другую метку - testLabel. В итоге при запуске приложения ageValueLabel отобразит значение свойства Age:

!(https://metanit.com/sharp/maui/pics/5.14.png)

Однако если мы нажмем на кнопку, то мы не увидим никаких изменений в тексте ageValueLabel. Хотя обработчик кнопки изменит значение свойства Age объекта Person, и метка testLabel отобразит это значение. То есть привязанные метки никак не могут узнать об изменениях в объекте Person.

И чтобы уведомить цель привязки об изменении значений источник привязки должен реализовать интерфейс INotifyPropertyChanged. Для этого изменим класс Person следующим образом:

```Csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
 
namespace HelloApp;
 
public class Person : INotifyPropertyChanged
{
    string name = "";
    int age;
 
    public event PropertyChangedEventHandler PropertyChanged;
 
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }
    public int Age
    {
        get => age;
        set
        {
            if (age != value)
            {
                age = value;
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

При этом в коде самой страницы ничего менять не надо. И если теперь мы нажмем на кнопку, то элемент Label мгновенно отреагирует на изменение свойства Age объекта Person.

!(https://metanit.com/sharp/maui/pics/5.15.png)

Аналогичный пример в XAML:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:Person x:Key="person" Name="Tom" Age="38" />
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout Padding="20" BindingContext="{StaticResource person}">
        <Label Text="Name" />
        <Label Text="{Binding Path=Name}" FontAttributes="Bold" />
        <Label Text="Age" />
        <Label Text="{Binding Path=Age}" FontAttributes="Bold"  />
        <Button Text="+" WidthRequest="60" HorizontalOptions="Start" Clicked="UpdateAge" />
    </StackLayout>
</ContentPage>
```

Здесь объект Person определяется как статический ресурс. Затем мы можем задать его как контекст для всего элемента StackLayout, а в отдельных элементах Label прописать привязку к свойствам этого объекта.

А в классе связанного кода пропишем обработчик кнопки, который будет менять значение свойства Age:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private void UpdateAge(object sender, EventArgs e)
    {
        var person = Resources["person"] as Person;
        person.Age ++;
    }
}
```

## Относительная привязка

Относительная привязка в .NET MAUI позволяют задать источник привязки относительно положения целевого объекта привязки в визуальном дереве элементов.

Для создания относительной привязки применяется расширения разметки RelativeSource. оно имеет следующие свойства:

- Mode: представляет тип RelativeBindingSourceMode и устанавливает режим привязки. Данное свойство может принимать следующие значения:

* TemplatedParent: применяется для установки привязки внутри шаблона элемента.

* Self: указывает на привязку к самому себе

* FindAncestor: указывает на контейнер в визуальном дереве элементов, где надо искать объект-источник привязки.

* FindAncestorBindingContext: привзяка будет идти к свойству BindingContext контейнера.

- AncestorLevel: представляет уровень элементов в визуальном дереве относительно контейнера, где будет идти поиск объекта-источника привязки (если свойство Mode имеет значение FindAncestor)

- AncestorType: представляет тип Type и указывает на тип элементов, среди которых будет идти поиск объекта-источника привязки (если свойство Mode имеет значение FindAncestor)

### Привязка к самому себе

Для привязки к самому себе применяется режим Self. Например, пусть у нас есть класс конвертера из строки в цвет:

```Csharp
using System.Globalization;
 
public class StringToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (Color.TryParse(value.ToString(), out var color)) return color;
        else return Colors.Black;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((Color)value).ToHex();
    }
}
```

Привяжем свойство TextColor элемента Entry к его свойству Text и для конвертации строки в цвет используем выше определенный конвертер:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <local:StringToColorConverter x:Key="converter" />
    </ContentPage.Resources>
    <StackLayout Padding="10">
        <Entry Text="#000"
               TextColor="{Binding 
                        Source={RelativeSource Self}, 
                        Path=Text, 
                        Converter={StaticResource converter}}"/>
    </StackLayout>
</ContentPage>
```

Благодаря выражению Source={RelativeSource Self} свойство TextColor будет привязано к свойству Text этого же элемента:

!(https://metanit.com/sharp/maui/pics/6.23.png)

### Привязка к контейнеру и его контексту

Режимы FindAncestor и FindAncestorBindingContext применяются для привязки к родительским элементам определенного типа в визуальном дереве. Режим FindAncestor используется для привязки к родительскому элементу, производному от типа Element. Режим FindAncestorBindingContext используется для привязки к BindingContext родительского элемента.

\
Если при установи привязки свойство Mode явным образом не задано, то при установке свойства AncestorType типа, производным от Element, свойство Mode неявно получает значение FindAncestor.

Если свойство Mode явным образом не задано, то при установке свойства AncestorType типа, НЕпроизводного от Element, свойство Mode неявно получает значение FindAncestorBindingContext.

Допустим, у нас есть следующий класс Person:

```Csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
 
namespace HelloApp;
 
public class Person : INotifyPropertyChanged
{
    string name = "";
    int age;
 
    public event PropertyChangedEventHandler PropertyChanged;
 
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }
    public int Age
    {
        get => age;
        set
        {
            if (age != value)
            {
                age = value;
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

Пусть, контейнер StackLayout привязан к объекту Person, а вложенные элементы Label привязаны к StackLayout

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:HelloApp"
             x:Class="HelloApp.MainPage">
    <ContentPage.Resources>
        <local:Person x:Key="person" Name="Tom" Age="38" />
    </ContentPage.Resources>
    <StackLayout Padding="10" BindingContext="{x:StaticResource person}">
        <Label Text="{Binding Source={RelativeSource AncestorType={x:Type local:Person}}, Path=Name}"/>
        <Label Text="{Binding Source={RelativeSource AncestorType={x:Type local:Person}}, Path=Age}"/>
    </StackLayout>
</ContentPage>
```

Здесь режим относительной привязки явным образом не задан, а в качестве типа указан локальный тип Person:

```xml
AncestorType={x:Type local:Person}
```

Класс Person является производным от класса Element, поэтому привязка будет идти к свойству BindingContext (точнее к его значению) контейнера StackLayout. А это свойство, в свою очередь, привязано к ресурсу person

```xml
<local:Person x:Key="person" Name="Tom" Age="38" />
```
Соответственно метки Label в реальности будут привязаны к этому объекту.

!(https://metanit.com/sharp/maui/pics/6.24.png)