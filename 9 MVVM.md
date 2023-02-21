# MVVM

## Паттерн Model-View-ViewModel

Паттерн Model-View-ViewModel (MVVM) основывается на разделении функциональной части приложения на три ключевых компонента:

- View - представление или пользовательский интерфейс

- Model - модель или данные, которые используются в приложении

- ViewModel - промежуточный слой между представлением и данными, который обеспечивает их взаимодействие

Преимуществом использования данного паттерна является меньшая связанность между компонентами и разделение ответственности между ними. То есть Model отвечает за данные, View отвечает за графический интерфейс, а ViewModel - за логику приложения.

Легкость реализации паттерна MVVM в .NET MAUI и C# стала возможной благодаря встроенному механизму привязки. Как правило, через свойство BindingContext визуального элемента устанавливается объект ViewModel. Далее через этот ViewModel идут все взаимодействия между данными и визуальным интерфейсом.

Рассмотрим простейший пример. Определим класс данных или модели:

```Csharp
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
```
Также добавим в проект класс, который назовем PersonViewModel со следующим содержимым:

```Csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
 
namespace HelloApp;
 
public class PersonViewModel : INotifyPropertyChanged
{
    Person person = new Person{Name="Tom", Age=38};
 
    public event PropertyChangedEventHandler? PropertyChanged;
 
    public string Name
    {
        get => person.Name;
        set
        {
            if (person.Name != value)
            {
                person.Name = value;
                OnPropertyChanged();
            }
        }
    }
    public int Age
    {
        get => person.Age;
        set
        {
            if (person.Age != value)
            {
               person.Age = value;
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
Это и будет компонент ViewModel, который связывает данные и визуальный интерфейс. По большому счету она представляет обертку над классом Person, определяя все те же свойства. Для упрощения задачи сам объект Person задается непосредственно в классе в качестве глобальной переменной, хотя в реальности там могла бы быть более сложная логика, например, по получению объекта из базы данных, из файла, из сетевого запроса и т.д..

Обычно класс ViewModel реализует интерфейс INotifyPropertyChanged, что позволяет уведомлять систему об изменении его свойств с помощью события PropertyChanged.

Теперь создадим визуальную часть. Определим на главной странице MainPage.xaml следующее содержимое:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <VerticalStackLayout Padding="5">
        <Label Text="Name" />
        <Label Text="{Binding Name}" FontAttributes="Bold" />
        <Label Text="Age" />
        <Label Text="{Binding Age}" FontAttributes="Bold" />
    </VerticalStackLayout>
</ContentPage>
```
Здесь определена привязка к свойствам ViewModel.

А в конструкторе страницы в файле кода MainPage.xaml.cs пропишем в качестве контекста данных для страницы определенную ранее ViewModel:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        // привязка к ViewModel
        BindingContext = new PersonViewModel();
    }
}
```

И при запуске приложение выведет нам все данные о viewmodel, переданной во view:

![](https://metanit.com/sharp/maui/pics/9.1.png)

При использовании паттерна в качестве представления обычно выступает страница, и, как правило, одно представление (одна страница) связана с одной моделью представления (ViewModel). При этом одну и ту же VıewModel могут использовать несколько представлений.

Стоит отметить, что в этом отношении ViewModel ничего не знает о представлении, что это, какие элементы управления оно содержит. ViewModel только определяет логику обработку без какой-либо связи с графическим интерфейсом.

Для подобным очень простых сценариев вряд ли потребуется определять дополнительную сущность в виде ViewModel, так как проще привязать элементы управления неспосредственно к свойствам объекта Person. Однако на дальней дистанции при развитии приложения это может помочь в более структурной организации логики приложения и упрощении работы по его развитию и поддержке.

## Команды и взаимодействие с пользователем в MVVM

В прошлой теме описывался паттерн MVVM, позволяющий отображать связанные данные. Однако, как правило, необходимо не только отображать данные, но и использовать какую-то логику взаимодействия с пользователем, обрабатывать пользовательский ввод. Рассмотрим, как это сделать в рамках паттерна MVVM.

Ключевой идеей паттерна является взаимодействие с моделью через ViewModel, то есть в данном случае использование событий визуальных компонентов и их обработчиков нежелательно. Чтобы решить эту задачу, в платформе .NET MAUI имеется механизм команд, которые представляют реализацию интерфейса System.Windows.Input.ICommand:

```Csharp
public interface ICommand 
{ 
    void Execute(object? arg); 
    bool CanExecute(object? arg); 
    event EventHandler? CanExecuteChanged; 
}
```

Метод Execute() выполняет команду.

Метод CanExecute возвращает true, если команда может быть выполнена.

Событие CanExecuteChanged генерируется при изменениях, которые могут повлиять на возможность выполнения команды.

Для создания команды можно использовать, как минимум, два способа

Определить класс, который реализует интерфейс ICommand, и затем создать конкретные команды - объекты этого класса.

Использовать класс Command или его обобщенную версию Command<T>, которые включенны в .NET MAUI и которые реализуют интерфейс ICommand

ViewModel может определять свойства типа ICommand. Затем подобные свойства можно привяать к свойству Command кнопки или другого визуального компонента, который поддерживает комманды. Например, когда пользователь нажимает кнопку, внутри Button вызывается метод Execute соответствующей команды. И мы можем либо использовать обработку события нажатия кнопки, либо привязать команду и также выполнять некоторые действия.

## Реализация ICommand

Рассмотрим простейшее применение команд. Допустим, данные у нас представлены классом Person:

```Csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Person(string name, int age)
    {
        Name = name; Age = age;
    }
}
```

В качестве ViewModel определим класс MainViewModel:

```Csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
 
namespace HelloApp;
 
public class MainViewModel : INotifyPropertyChanged
{
    string name = "";
    int age;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand AddCommand { get; set; }
    public ObservableCollection<Person> People { get; } = new();
 
    public MainViewModel()
    {
        // устанавливаем команду добавления
        AddCommand = new Command(() =>
        {
            People.Add(new Person(Name, Age));
            Name = "";
            Age = 0;
        });
    }
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

Класс MainViewModel хранит поля name и age и надстройки над ними - свойства Name и Age, через которые пользователь будет ввводить новые данные. Также класс определяет свойство People - коллекцию ObservableCollection, в которую будут добавляться данные.

Для добавления данных определено свойство-команда AddCommand, которая устанавливается в конструкторе

```Csharp
AddCommand = new Command(() =>
{
    People.Add(new Person(Name, Age));
    Name = "";
    Age = 0;
});
```

Для установки команды в конструктор класса Command передается делегат Action, который представляет выполняемое командой действие. В данном случае берем значения свойств Name и Age (которые представляют введенные пользователем данные), создаем по ним объект Person и добавляем его в коллекцию People. Таким образом, будет выполняться добавление данных. После добавления сбрасываем значения свойств Name и Age.

Стоит отметить, что конструктор Command может имеет несколько версий с разным набором параметров.

В классе странице MainPage выполним привязку к этой модели представления:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}
```

А в коде MainPage.xaml установим привязку отдельных элементов графического инстерфейса к свойствам MainViewModel:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <VerticalStackLayout Padding="5">
        <VerticalStackLayout>
            <Entry Placeholder="Enter name" Text="{Binding Name}" />
            <Entry Placeholder="Enter age" Text="{Binding Age}"/>
            <Button Text="Save" WidthRequest="100" HorizontalOptions="Start"
                        Command="{Binding AddCommand}"  />
        </VerticalStackLayout>
        <ListView ItemsSource="{Binding People}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <VerticalStackLayout>
                            <Label Text="{Binding Name}" />
                            <Label Text="{Binding Age}" />
                        </VerticalStackLayout>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </VerticalStackLayout>
</ContentPage>
```
Вначале страницы определено два текстовых поля, которые привязаны к свойствам Name и Age. А по нажатию кнопки Add срабатывает команда AddCommand. Для привязки команды у кнопки, как и у ряда других элементов есть свойство Command:

```xml
<Button Text="Save" WidthRequest="100" HorizontalOptions="Start"
    Command="{Binding AddCommand}"  />
```

Таким образом, по нажатию на кнопку введенные в текстовые поля данные будут добавлены в список People в MainViewModel.

Чуть ниже определен элемент ListView, который выводит список People.

Результат работы приложения:

```Csharp
https://metanit.com/sharp/maui/pics/9.3.png
```

Таким образом, мы не используем события, не обращаемся явным образом к элементам интерфейса для получения или установки их значений, а взаимодействуем через привязку данных и команды.

## Параметры команды

Команда может принимать параметры, через которые в команду можно передать некоторые данные извне. В частности, если мы посмотрим на определение метода Execute, то увидим, что он принимает параметр args, через который в команду передаются данные любого типа:

```Csharp
void Execute(object? arg);
```

Для передачи данных в команду элементы графического интерфейса, которые поддерживают команды, как правило, имеют свойство CommandParameter. Это свойство и представляет передаваемые в команду данные.

Рассмотрим применение параметров команды. Пусть данные представлены следующим классом Person:

```Csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Person(string name, int age)
    {
        Name = name; Age = age;
    }
}
```

Определим следующий класс модели представления MainViewModel:

```Csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
 
namespace HelloApp;
 
public class MainViewModel : INotifyPropertyChanged
{
    string name = "";
    int age;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand AddCommand { get; set; }
    public ICommand RemoveCommand { get; set; }
    public ObservableCollection<person> People { get; } = new();
 
    public MainViewModel()
    {
        // устанавливаем команду добавления
        AddCommand = new Command(() =>
        {
            People.Add(new Person(Name, Age));
            Name = "";
            Age = 0;
        });
        // устанавливаем команду удаления
        RemoveCommand = new Command((object? args) => 
        { 
            if(args is Person person) People.Remove(person);
        });
    }
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

Модель представления определяет свойство People, которое представляет коллекцию объектов, а также свойства Name и Age, через которые пользователь будет добавлять в список People новые данные.

MainViewModel определяет две команды. Команда AddCommand по свойствам Name и Age создает объект Person и добавляет его в коллекцию Person.

Команда RemoveCommand получает параметр:

```Csharp
RemoveCommand = new Command((object? args) =>
{ 
    if(args is Person person) People.Remove(person);
});
```

Для получения параметра в конструктор класса Command передается делегат с одним параметров типа object?. Такой параметр может представлять любой тип. И в данном случае мы ожидаем, что будет передаваться удаляемый объект Person. Поэтому преобразуем параметр args к типу Person и удаляем из коллекции People.

В файле MainPage.xaml.cs установим данную модель представления в качестве констекста привязки:


```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}
```

В файле MainPage.xaml установим привязки свойств и команд модели представления к визуальным элементам:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <VerticalStackLayout Padding="5">
        <VerticalStackLayout>
            <Entry Placeholder="Enter name" Text="{Binding Name}" />
            <Entry Placeholder="Enter age" Text="{Binding Age}" />
            <Button Text="Save" WidthRequest="100" HorizontalOptions="Start"
                        Command="{Binding AddCommand}"  />
        </VerticalStackLayout>
        <ListView x:Name="peopleListView" ItemsSource="{Binding People}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <VerticalStackLayout>
                            <Label Text="{Binding Name}" />
                            <Label Text="{Binding Age}" FontSize="14" />
                        </VerticalStackLayout>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
        <Button Text="Remove" WidthRequest="100" HorizontalOptions="Start"
                Command="{Binding RemoveCommand}"
                CommandParameter="{Binding Source={x:Reference Name=peopleListView}, Path=SelectedItem}"  />
    </VerticalStackLayout>
</ContentPage>
```

Вначале страницы определено два текстовых поля, которые привязаны к свойствам Name и Age. А по нажатию кнопки Add срабатывает команда AddCommand, которая добавляет введенные в текстовые поля данные в виде объекта Person в список People.

Чуть ниже определен элемент ListView, который выводит список People. И под ListView расположена кнопка удаления, которая привязана к команде RemoveCommand и которая в качестве параметра команды передает выделенный объект в ListView:


```xml
<Button Text="Remove" WidthRequest="100" HorizontalOptions="Start"
    Command="{Binding RemoveCommand}"
    CommandParameter="{Binding Source={x:Reference Name=peopleListView}, Path=SelectedItem}"  />
```

Свойство CommandParameter также определяется через привязку. Поскольку глобальным констекстом привязки (в том числе контекстом привязки для кнопки удаления) является модель представления MainViewModel, то в данном случае переопределяем источник привязки на ListView: Source={x:Reference Name=peopleListView} и привязываемся к его свойству SelectedItem.

Таким образом, после выделения объекта в ListView и нажатия на кнопку выделенный объект будет удален:


!(https://metanit.com/sharp/maui/pics/9.4.png)

## Типизация команды

В примере выше для создания команды использвался необобщенный класс command. Однако можно также использовать его обобщенную версию и типизировать объект Command типом параметра (в данном случае типом Person):

```Csharp
RemoveCommand = new Command<Person>((Person person) =>
{ 
    People.Remove(person);
});
```

В данном случае команда типизированна типом Person, поэтому параметр команды также представляет тип Person. Соответственно мы можем избежать преобразования данных в тип Person.

## Передача параметра

В примере выше в качестве параметра передавался выделенный объект с помощью выражения привязки. Однако в реальности необязательно использовать привязку. Можно передавать любое значение. Например, передача простого числа

```xml
<Button Text="6" Command="{Binding SomeCommand}" CommandParameter="6" />
```

В данном случае некоторая команда SomeCommand в качестве параметра получит число 6.

## Отключение команды

Иногда необходимо, чтобы при некоторых условиях команда не срабатывала. Например, при наличии некорректных данных или в силу какаих-то иных причин. В этом случае можно отключать команду. Для этой цели в интерфейсе ICommand определен метод

```Csharp
bool CanExecute(object? arg);
```

Если он возвращает true, то команда будет доступна, если false, то не доступна.

Рассмотрим простейший пример. Пусть у нас есть следующая модель данных Person:

```Csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Person(string name, int age)
    {
        Name = name; Age = age;
    }
}
```

Свойство Name здесь представляет имя, свойство Age - возраст человека. Однако теоретически этим свойствам могут быть переданы не совсем корректные данные. Например, свойству Age можно присвоить негативное или очень большое число, например, 11500. Аналогично свойству Name можно передать пустую строку или какое-нибудь нехорошее слово. И было бы неплохо, если пользователь вводит подобные некорректные значения, то они не могли бы использованы для свойств Person.

Допустим нам надо вывести на страницу список объектов Person и определить функционал по добавлению новых объектов Person в этот список. Для этого определим следущий класс MainViewModel:

```Csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
 
namespace HelloApp;
 
public class MainViewModel : INotifyPropertyChanged
{
    string name = "";
    int age;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand AddCommand { get; set; }
    public ObservableCollection<Person> People { get; } = new();
 
    public MainViewModel()
    {
        // устанавливаем команду добавления
        AddCommand = new Command(() =>
        {
            People.Add(new Person(Name, Age));
            Name = "";
            Age = 0;
        },
        () => Age > 0 && Age < 110 && Name.Length > 2);
    }
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
        ((Command)AddCommand).ChangeCanExecute();
    }
}
```

Модель представления определяет свойство People, которое представляет коллекцию объектов, а также свойства Name и Age, через которые пользователь будет добавлять в список People новые данные.

Для добавления новых объектов Person в коллекцию People MainViewModel определяет команду AddCommand. Причем для создании команды применяется конструктор класса Command, который принимает два параметра:


```Csharp
AddCommand = new Command(() =>
{
    People.Add(new Person(Name, Age));
    Name = "";
    Age = 0;
},
() => Age > 0 && Age < 110 && Name.Length > 2);
```

Первый параметр - делегат Action собственно представляет команду, которая, используя свойства Name и Age, создает объект Person и добавляет его в коллекцию People.

Второй параметр - делегат Func<bool> определяет условие, при котором команда будет активна. Результат этого делегата представляет значение типа bool - если оно равно true, то команда активна. в данном случае возвращается true, если свойство Age больше 0 и меньше 110 и длина имени больше 2 символов. Во всех остальных случаях команда будет не активна.

То есть активность команды зависит от свойств Name и Age. Однако их значение может меняться по мере ввода символов, но активность команды автоматически не изменится. Для этого нам надо известить систему вручную. Для этой цели в классе Command опредлен метод ChangeCanExecute(), соответственно нам надо его вызвать. Поскольку при изменении свойств срабатывает метод OnPropertyChanged(), то именно в этом методе и вызываем метод ChangeCanExecute():

```Csharp
public void OnPropertyChanged([CallerMemberName] string prop = "")
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    ((Command)AddCommand).ChangeCanExecute();
}
```

В файле MainPage.xaml.cs установим данную модель представления в качестве констекста привязки:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}
```

В файле MainPage.xaml установим привязки свойств и команд модели представления к визуальным элементам:


```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <VerticalStackLayout Padding="5">
        <VerticalStackLayout>
            <Entry Placeholder="Enter name" Text="{Binding Name}" />
            <Entry Placeholder="Enter age" Text="{Binding Age}"/>
            <Button Text="Save" WidthRequest="100" HorizontalOptions="Start"
                        Command="{Binding AddCommand}"  />
        </VerticalStackLayout>
        <ListView x:Name="peopleListView" ItemsSource="{Binding People}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <VerticalStackLayout>
                            <Label Text="{Binding Name}" />
                            <Label Text="{Binding Age}" />
                        </VerticalStackLayout>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </VerticalStackLayout>
</ContentPage>
```

Вначале страницы определено два текстовых поля, которые привязаны к свойствам Name и Age. А по нажатию кнопки Add срабатывает команда AddCommand. Таким образом, по нажатию на кнопку введенные в текстовые поля данные будут добавлены в список People в MainViewModel.

Чуть ниже определен элемент ListView, который выводит список People.

Однако если в текстовые поля будут введены некорреткные значения, то команда AddCommand будет недоступна, соответственно и кнопка будет недоступна:

!(https://metanit.com/sharp/maui/pics/9.5.png)

## Организация моделей представления

Одни модели представления могут использовать другие. Разбиение функционала на несколько моделей представления позволяет более четко очертить задачу для определенной ViewModel, сделать логику приложения более гибкой в плане поддержки и дальнейшего развития.

Например, нам надо совместить на странице отображение и редактирование объектов следующего класса Person:

```Csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Person(string name, int age)
    {
        Name = name; Age = age;
    }
}
```

Для этого определим в проекте следующий класс MainViewModel:

```Csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
 
namespace HelloApp;
 
public class MainViewModel : INotifyPropertyChanged
{
    Person person = new Person("Tom", 38);
    string name = "";
    int age;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand SaveCommand { get; set; }
    public ICommand EditCommand { get; set; }
 
    public MainViewModel()
    {
        SaveCommand = new Command (() =>
        {
            PersonName = Name;
            PersonAge= Age;
            Name = "";
            Age = 0;
        });
        EditCommand = new Command(() =>
        {
            Name = PersonName;
            Age = PersonAge;
        });
    }
    public string PersonName
    {
        get => person.Name;
        set
        {
            if (person.Name != value)
            {
                person.Name = value;
                OnPropertyChanged();
            }
        }
    }
    public int PersonAge
    {
        get => person.Age;
        set
        {
            if (person.Age != value)
            {
                person.Age = value;
                OnPropertyChanged();
            }
        }
    }
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

Здесь модель представления хранит сами данные в виде объекта Person, а также переменные name и age, которые будут хранить редактируемые данные.

Команда EditCommand присваивает значения свойств объекта Person свойствам Name и Age. Таким образом, данные будут передаваться на редактирование.

Команда SaveCommand, наоборот, присваивает значения свойств Name и Age свойствам объекта Person. То есть таким образом, сохраняем сделанные изменения.

В коде MainPage.xaml.cs определяем объект MainViewModel в качестве контекста привязки:

```Csharp
namespace HelloApp;
 
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}
```

А в коде MainPage.xaml привязываем свойства MainViewModel к элементам графического интерфейса:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <VerticalStackLayout Padding="5">
         
        <Label Text="Name" />
        <Label Text="{Binding PersonName}" FontAttributes="Bold" />
        <Label Text="Age" />
        <Label Text="{Binding PersonAge}" FontAttributes="Bold" />
        <Button Text="Edit" WidthRequest="100" HorizontalOptions="Start"
                        Command="{Binding EditCommand}"  />
        <VerticalStackLayout>
            <Entry Placeholder="Enter name" Text="{Binding Name}" />
            <Entry Placeholder="Enter age" Text="{Binding Age}"/>
            <Button Text="Save" WidthRequest="100" HorizontalOptions="Start"
                        Command="{Binding SaveCommand}"  />
        </VerticalStackLayout>
    </VerticalStackLayout>
</ContentPage>
```

Здесь вначале расположены метки, которые выводят значения свойств объекта Person. По нажатию на кнорку Edit эти данные передаются в текстовые поля ниже (то есть значения свойств PersonName и PersonAge копируются в свойства Name и Age).

По нажатию на кнопку Save срабатывает команда SaveCommand, которая, наборот, копирует значения Name и Age в PersonName и PersonAge соответственно, тем самым сохраняя отредактированные данные.

!(https://metanit.com/sharp/maui/pics/9.2.png)

Хотя приложение работает, но класс MainViewModel фактически представляет две функции - отображение и редактирование данных. Гораздо проще этими данными управлять по отдельности. Так, изменим класс MainViewModel следующим образом:

```Csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
 
namespace HelloApp;
 
public class DisplayViewModel : INotifyPropertyChanged
{
    Person person;
    public event PropertyChangedEventHandler? PropertyChanged;
    public DisplayViewModel(Person person) => this.person = person;
    public string Name
    {
        get => person.Name;
        set
        {
            if (person.Name != value)
            {
                person.Name = value;
                OnPropertyChanged();
            }
        }
    }
    public int Age
    {
        get => person.Age;
        set
        {
            if (person.Age != value)
            {
                person.Age = value;
                OnPropertyChanged();
            }
        }
    }
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
public class EditViewModel : INotifyPropertyChanged
{
    string name = "";
    int age;
    public event PropertyChangedEventHandler? PropertyChanged;
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
public class MainViewModel
{
    public ICommand SaveCommand { get; set; }
    public ICommand EditCommand { get; set; }
    public DisplayViewModel Person { get; set; }
    public EditViewModel EditData { get; set; }
 
    public MainViewModel()
    {
        Person = new DisplayViewModel(new Person("Tom",38));
        EditData = new EditViewModel();
        SaveCommand = new Command (() =>
        {
            Person.Name = EditData.Name;
            Person.Age= EditData.Age;
            EditData.Name = "";
            EditData.Age = 0;
        });
        EditCommand = new Command(() =>
        {
            EditData.Name = Person.Name;
            EditData.Age = Person.Age;
        });
    }
}
```

Теперь для отображения данных применяется класс DisplayViewModel, который хранит отображаемые данные объекта Person. А данные для редактирования перемещены в класс EditViewModel. Основная же модель представления - MainViewModel по прежнему определяет команды и выстпает посредником между DisplayViewModel и EditViewModel. Таким кодом легче управлять и обновлять.

Для применения изменений обновим код страницы MainPage.xaml:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HelloApp.MainPage">
    <VerticalStackLayout Padding="5">
         
        <Label Text="Name" />
        <Label Text="{Binding Person.Name}" FontAttributes="Bold" />
        <Label Text="Age" />
        <Label Text="{Binding Person.Age}" FontAttributes="Bold" />
        <Button Text="Edit" WidthRequest="100" HorizontalOptions="Start"
                        Command="{Binding EditCommand}"  />
        <VerticalStackLayout>
            <Entry Placeholder="Enter name" Text="{Binding EditData.Name}" />
            <Entry Placeholder="Enter age" Text="{Binding EditData.Age}"/>
            <Button Text="Save" WidthRequest="100" HorizontalOptions="Start"
                        Command="{Binding SaveCommand}"  />
        </VerticalStackLayout>
    </VerticalStackLayout>
</ContentPage>
```

И результат приложения будет тем же, что и ранее, но код теперь будет чуть более структурированным.