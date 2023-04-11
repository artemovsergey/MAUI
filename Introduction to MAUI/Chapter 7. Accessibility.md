# CHAPTER 7

# Accessibility

In this chapter, you will be taking a break from adding new parts to the
user interface in order to gain an understanding of what accessibility is,
why you should make your applications accessible, and how .NET MAUI
makes this easier. You will also cover some testing options to support your
journey to building accessible applications.
I wanted this chapter to appear earlier on in this book. I feel it is such
an important topic and one that you really do need to consider early on
in your projects. It has come to settle nicely in the middle of the book now
because you needed some UI to apply the concepts to.


# What Is Accessibility?

The definition of accessibility according to the Cambridge Dictionary
(https://dictionary.cambridge.org/dictionary/english/
accessibility) is
“the quality of being easy to understand.”

By considering the scenarios where your application might be less easy
to understand for a large percentage of the world’s population that have
some form of disability, you can learn to provide ways to break down the
complexities in understanding the content. This might be through the use
of assistive technologies such as voice-over assistants or screen readers,
or even providing the ability to increase the font size to make the content
easier to read.

All of this can help you as a developer learn how to build applications
that are much more inclusive of the entire population of the world.

## Why Make Your Applications Accessible?

I heard an excellent quote recently and sadly I have been unable to
discover the original author of the quote, but it is “if you don’t know
whether your application is accessible, then you can safely say that is it not.”
Essentially, if you are not putting any effort into making it accessible, then
you can almost guarantee that it is not.

According to the World Health Organization, globally at least 2.2 billion
people have a near or distance vision impairment (www.who.int/newsroom/fact-sheets/detail/blindness-and-visual-impairment).
You want to build your applications and make them as successful
as possible. Imagine immediately ruling out up to 27% of your potential
market purely based on not making your application more inclusive for
that population.

# What to Consider When Making Your Applications Accessible

There is a whole heap of things you can do in order to make your
applications more inclusive. To aid you on your journey to building
accessible applications, there is a fantastic set of guidelines known as
the Web Content Accessibility Guidelines (WCAG). There are four main
principles to consider:
• Perceivable: Making sure that you provide information
that can be perceived by the user. This can be by
providing text-based alternatives to images, suitable
contrast ratios, adaptive text sizing, and much more.

• Operable: Making sure that you provide the user
with the ability to use the application. This can be
by providing keyboard navigation, making sure they
have enough time to read and use the content, and
much more.
• Understandable: Making sure that you provide a user
interface that is understandable to the user. This can be
making sure that the content is readable, predictable
(appear and behave as expected), and helps the user
avoid making mistakes.
• Robust: Making sure the content is robust enough
that it can be interpreted by a wide variety of user
agents, including assistive technologies. This can be by
providing suitable support for assistive technologies.
To read more on these guidelines, I thoroughly recommend checking
out the Quick Reference Guide at www.w3.org/WAI/WCAG21/quickref/.

# How to Make Your Application Accessible

There are several things to consider when building an application that is
inclusive. This section will not provide a complete set of tools for building
applications inclusive for all. However, it will provide some insights to what
.NET MAUI offers and some other concepts to consider to set you off on a
journey of discovery to building much more accessible applications.

# Screen Reader Support

.NET MAUI provides great tools to provide explicit support for the screen
readers on each of the supported platforms. I feel it is worth highlighting
that point again: .NET MAUI utilizes the screen readers on each
platform. This means that they will need to be enabled by the user for the 
settings to take effect. You will dive into each concept and how it enables
you to expose information to those screen readers so you can provide a
much more informative experience for your users.
As a starting exercise, pick up your phone and turn on your screen
reader assistant. Try navigating around to get an understanding of what
the experience is like and, most importantly, try an application you built.
Does it provide a good experience?
Let’s see how you can make the WidgetBoard application more
accessible with the screen readers available. Thankfully you haven’t built
too much UI already, so you are in a good position to start. I urge you to
consider applying

# SemanticProperties

The SemanticProperties class offers a set of attached properties that can
be applied to any visual element. .NET MAUI applies these property values
on the platform-specific APIs that provide accessibility.
Let’s look through each of the properties and apply them to your
BoardDetailsPage.

# SemanticProperties.Description

The SemanticProperties.Description property allows you to define a
short string that will be used by the screen reader to announce the element
to the user when it gains focus.
As I type this chapter, I am testing the application. The first Entry
added on the BoardDetailsPage currently results in the macOS VoiceOver
assistant announcing “edit text, is editing, blank.”
You can change the Entry to the following

```xml
 Text="{Binding BoardName}"
 SemanticProperties.Description="Enter the board name"/>
```
This now results in “Enter the board name, is editing, blank” being
announced, which is much more useful to the user.
You can take this a step further. You have a label above that just has the
Text of “Name.” If you change this to use your new descriptive text, then
you can set the SemanticProperties.Description value to its text. Let’s
do that now; the changes are highlighted in bold:

```xml
<Label
 Text="Enter the board name"
 x:Name="EnterBoardNameLabel"
 FontAttributes="Bold" />
<Entry
 Text="{Binding BoardName}"
 SemanticProperties.Description="{Binding Text,
Source={x:Reference EnterBoardNameLabel}}" />
```
The resulting code may look less appealing but it provides a number of
benefits:
• The text description is more informative on the Label.
• When you add in localization support, you will have
only one text field to update.
The macOS screen reader does provide a second announcement
following the announcement you have been improving. This follow-up is
“You are currently on a text field. To enter text in this field, type.” This isn’t
the most informative, so let’s provide a better hint to the user.

---
The act of setting the SemanticProperties.Description
property will automatically make a visual element be announced by
the screen reader. By default, an Image control is not announced but
by setting this property, the text will be announced when the control
gains semantic focus.
---

# SemanticProperties.Hint

The SemanticProperties.Hint property allows you to provide a string
that the screen reader will announce to the user so that they have a better
understanding of the purpose of the control.
Let’s add a hint to Entry with the addition in bold:

```xml
<Entry
 Text="{Binding BoardName}"
 SemanticProperties.Description="{Binding Text,
Source={x:Reference EnterBoardNameLabel}}"
 SemanticProperties.Hint="Provides a name that will be
used to identify your widget board. This is a required
field." />
```
This change results in “Provides a name that will be used to identify
your widget board. This is a required field. You are currently on a text field.
To enter text in this field, type” being announced. I think you can agree that
this adds yet more context to the user and this is a good thing.

# SemanticProperties.HeadingLevel

The SemanticProperties.HeadingLevel property allows you to mark an
element as a heading to help organize the UI and make it easier for users
to navigate. Some screen readers enable users to quickly jump between 

headings and thus providing a far more friendly navigation for those users
that rely on screen readers. Headings have a level from 1 to 9 and are
represented by the SemanticHeadingLevel enumeration.

# SemanticScreenReader

NET MAUI provides the SemanticScreenReader that enables you to
instruct a screen reader to announce some text to the user. This can work
especially well if you wish to present instructions to a user or to prompt
them if they have paused their interaction.
The SemanticScreenReader provides a static Announce method
to perform the announcements, it also provides a Default instance. I
personally like to make use of the scenarios where .NET MAUI provides
you with a Current or a Default instance and register this with the app
builder to make full use of the dependency injection support. To do this,
write the following line of code in your MauiProgram.cs file:
builder.Services.AddSingleton(SemanticScreenReader.Default);
With the screen reader registered, you can announce that the new
board was created successfully once the user has tapped on the Save
button. You need to open the BoardDetailsPageViewModel.cs file and
make the following changes.
Add the read-only field.
private readonly ISemanticScreenReader semanticScreenReader;
Assign a value in your constructor, just applying the bold code to your
existing content

```Csharp
public BoardDetailsPageViewModel(ISemanticScreenReader
semanticScreenReader)
{
 this.semanticScreenReader = semanticScreenReader;
  SaveCommand = new Command(
 () => Save(),
 () => !string.IsNullOrWhiteSpace(BoardName));
```
Call Announce in your Save method, just applying the bold code to
your existing content.

```Csharp
private async void Save()
{
 var board = new Board
 {
 Name = BoardName,
 Layout = new FixedLayout
 {
 NumberOfColumns = NumberOfColumns,
 NumberOfRows = NumberOfRows
 }
 };
 semanticScreenReader.Announce($"A new board with the name
{BoardName} was created successfully.");
 await Shell.Current.GoToAsync(
 "fixedboard",
 new Dictionary<string, object>
 {
 { "Board", board}
 });
}
```
If you run your application and save a new board called “My work
board,” you will observe that the screen reader will announce “A new
board with the name My work board was created successfully.” This gives
the user some valuable audible feedback. If you expect the save process to
take some time, you can also perform an announcement at the start of the
process to keep the user informed.

# AutomationProperties

AutomationProperties are the old Xamarin.Forms way of exposing
information to the screen readers on each platform. I won’t cover all of the
options because some have been replaced by the SemanticProperties
section that you just learned about. The following are the important ones
that provide a different set of functionality.

# AutomationProperties.ExcludedWithChildren

The AutomationProperties.ExcludeWithChildren property allows
developers to exclude the element supplied and all its children from the
accessibility tree. Setting this property to true will exclude the element and
all of its children from the accessibility tree.

# AutomationProperties.IsInAccessibleTree

The AutomationProperties.IsInAccessibleTree property allows
developers to decide whether the element is visible to screen readers.
A common scenario for this feature is to hide controls such as Label or
Image controls that serve a purely decorative purpose (e.g., a background
image). Setting this property to true will exclude the element from the
accessibility tree.

# Suitable Contrast

WCAG states in guideline 1.4.3 Contrast (Minimum) – Level AA that the
visual presentation of text and images of text has a contrast ratio of at least
4.5:1, except for the following:

Large Text: Large-scale text and images of large-scale
text have a contrast ratio of at least 3:1.
• Incidental: Text or images of text that are part of
an inactive user interface component, that are pure
decoration, that are not visible to anyone, or that are
part of a picture that contains significant other visual
content, have no contrast requirement.
• Logotypes: Text that is part of a logo or brand name
has no contrast requirement.

This all boils down to calculating the difference between the lighter
and darker colors in your application when displaying text. If that contrast
ratio is 4.5:1 or higher, it’s suitable. Let’s look at how this is calculated:

```(L1 + 0.05) / (L2 + 0.05)```

where L1 is the relative luminance of the lighter color and L2 is the
relative luminance of the darker color. Relative luminance is defined as
the relative brightness of any point in a colorspace, normalized to 0 for
darkest black and 1 for lightest white. Relative luminance can be further be
calculated as

For the sRGB colorspace, the relative luminance of a color is
defined as L = 0.2126 * R + 0.7152 * G + 0.0722 * B where R, G
and B are defined as:

```
if RsRGB <= 0.03928 then R = RsRGB/12.92 else R = ((RsRGB+0.055)/1.055) ^ 2.4
if GsRGB <= 0.03928 then G = GsRGB/12.92 else G =
((GsRGB+0.055)/1.055) ^ 2.4
if BsRGB <= 0.03928 then B = BsRGB/12.92 else B =
((BsRGB+0.055)/1.055) ^ 2.4
and RsRGB, GsRGB, and BsRGB are defined as:
RsRGB = R8bit/255
GsRGB = G8bit/255
BsRGB = B8bit/255
```
The "^" character is the exponentiation operator.

These formulas are taken from www.w3.org/TR/WCAG21/#dfnrelative-luminance. Let’s turn this into some C# to make it a little easier
to follow and something that you can use to test your color choices.

```Csharp
private static double GetContrastRatio(Color lighterColor,
Color darkerColor)
{
 var l1 = GetRelativeLuminance(lighterColor);
 var l2 = GetRelativeLuminance(darkerColor);
 return (l1 + 0.05) / (l2 + 0.05);
}
private static double GetRelativeLuminance(Color color)
{
 var r = GetRelativeComponent(color.Red);
 var g = GetRelativeComponent(color.Green);
 var b = GetRelativeComponent(color.Blue);
 return
 0.2126 * r +
 0.7152 * g +
 0.0722 * b;
}

private static double GetRelativeComponent(float component)
{
 if (component <= 0.03928)
 {
 return component / 12.92;
 }
 return Math.Pow(((component + 0.055) / 1.055), 2.4);
}

```
If you take a look at the colors you are using for your text controls and
the background colors, you can work out whether you need to improve on
the contrast ratio. You can see by checking in your Styles.xaml file that
your Label control uses Gray900 for the text color. Checking in the Colors.
xaml file, you can see that this Gray900 color has a value of #212121.
Therefore, you can use your methods to calculate the contrast ratio with

```GetContrastRatio(Colors.White, Color.FromArgb("#212121"); ```

This gives you a contrast ratio of 16.10:1, which means this is providing
a very good contrast ratio. The best possible contrast is black on white,
which gives a contrast ratio of 21:1. Therefore, you do not need to make
any changes to your color scheme, which shows that .NET MAUI ships
with default color options that are suitable for building accessible
applications.

# Dynamic Text Sizing

WCAG states in guideline 1.4.4 Resize text – Level AA that except for
captions and images of text, text can be resized without assistive
technology up to 200 percent without loss of content or functionality.

This guideline mainly focuses on highlighting the fact that there is still
a large percentage of users that do not rely on accessibility features such
as screen readers or screen magnification when they could benefit from
them. The guideline further states that, as a developer, you should provide
the ability to scale the text in your application up to 200% without relying
on the operating system to perform the scaling.
In this section, I am not going to focus on adding that specific feature;
however, I will be discussing some approaches that will aid this feature as
well as using the assistive technology options.

# Avoiding Fixed Sizes

Wherever possible you want to avoid setting the WidthRequest and
HeightRequest properties for any control that can contain text.
Imagine you set WidthRequest="200" and HeightRequest="30" on
the Label controls in your BoardDetailsPage.xaml file. What you would
initially see is that the text fits nicely using the standard font scaling
options. Figure 7-1 shows your application with fixed size controls and a
small font size.

![image](https://user-images.githubusercontent.com/26972859/231178603-954cd72a-85a1-4f89-a9a1-8379971f11f4.png)
Figure 7-1. Your application with fixed sizing and a small font size

However, if you up the scaling to 200%, you will see a rather unpleasant
screen. Figure 7-2 shows your application with fixed size controls and a
large font size, highlighting that the text becomes clipped and unreadable.

![image](https://user-images.githubusercontent.com/26972859/231178843-1d2be63a-6b89-4b4a-b988-34a81a09dc70.png)
Figure 7-2. Your application with fixed sizing and a large font size

It actually appears that your initial changes without the WidthRequest
and HeightRequest values on the Label controls gives the best experience.
Figure 7-3 shows your application responding to font size changes when
control sizes are not fixed.

![image](https://user-images.githubusercontent.com/26972859/231178980-c2478ff4-c412-45b6-910a-0ab83cd9aeb0.png)
Figure 7-3. Your application showing responsiveness to font scaling

# Preferring Minimum Sizing

Where possible, you should use MinimumWidthRequest and
MinimumHeightRequest over WidthRequest and HeightRequest,
respectively. This allows for controls to grow. There may be scenarios
where a combination of Minimum and Maximum property values will give
a good experience when scaling is introduced.

# Font Auto Scaling

By default, all controls that render text in a .NET MAUI application have
the FontAutoScalingEnabled property set to true. This means that the
controls automatically scale their font size accordingly when the operating
systems font scaling settings are changed.

There can be scenarios when disabling this feature can provide a
more accessible experience. One example is in a wordsearch application
I built. The application made the letters appear as big as possible, so any
additional scaling by the operating system would result in parts of the text
being cut off. I advise using this option sparingly.

# Testing Your Application’s Accessibility

Each platform supported by .NET MAUI has its own set of guidelines
around testing for accessibility and even tools to aid that journey. In
this section, you are going to take a brief look at what each platform
provider offers.

# Android

Google, much like each of the other platform providers, does recommend
that you perform a manual test, such as turning on TalkBack and verifying
that the user experience is as you have designed.
Google also offers some analysis tools to detect whether any
accessibility guidelines are not being met. There is a good list provided
by Google with a breakdown of the functionality provided by each tool at
https://developer.android.com/guide/topics/ui/accessibility/
testing#analysis.

# iOS
Apple doesn’t offer as much as Google on this front. There is the
Accessibility Inspector but it only focuses on allowing you to view
the information that the screen reader will be provided. I don’t
feel this is as good as taking a dry run through your application
with the VoiceOver assistant turned on. Further information on 

Apple’s offering can be found at https://developer.apple.com/
library/archive/technotes/TestingAccessibilityOfiOSApps/
TestAccessibilityiniOSSimulatorwithAccessibilityInspector/
TestAccessibilityiniOSSimulatorwithAccessibilityInspector.html.

# macOS

Apple provides a little extra functionality when testing on macOS. It
does provide the Accessibility Inspector as per iOS and well as the
Accessibility Verifier. This tool allows you to run tests against your
application to verify items like the accessibility description have been
defined on all required elements. Further information on these features
can be found at https://developer.apple.com/library/archive/
documentation/Accessibility/Conceptual/AccessibilityMacOSX/
OSXAXTestingApps.html.

# Windows

Microsoft offers the biggest amount of options when it comes to testing the
accessibility of your applications. The Windows Software Development Kit
(SDK) provides several tools such as the ability to inspect an application
and view all related properties as plus automation tests that verify the
state of accessibility. All details of the tools can be found at https://docs.
microsoft.com/windows/apps/design/accessibility/accessibilitytesting.

# Accessibility Checklist

The following checklist is provided by Microsoft on their documentation
site at https://docs.microsoft.com/dotnet/maui/fundamentals/
accessibility#accessibility-checklist. I haven’t added to it or 

reworded because I believe it provides an excellent breakdown of the
possible ways to provide accessible support.
Follow these tips to ensure that your .NET MAUI apps are accessible to
the widest audience possible:
• Ensure your app is perceivable, operable,
understandable, and robust for all by following the Web
Content Accessibility Guidelines (WCAG). WCAG is
the global accessibility standard and legal benchmark
for web and mobile. For more information, see Web
Content Accessibility Guidelines (WCAG) Overview.
• Make sure the user interface is self-describing. Test
that all the elements of your user interface are screen
reader accessible. Add descriptive text and hints when
necessary.
• Ensure that images and icons have alternate text
descriptions.
• Support large fonts and high contrast. Avoid
hardcoding control dimensions, and instead prefer
layouts that resize to accommodate larger font sizes.
Test color schemes in high-contrast mode to ensure
they are readable.
• Design the visual tree with navigation in mind. Use
appropriate layout controls so that navigating between
controls using alternate input methods follows the
same logical flow as using touch. In addition, exclude
unnecessary elements from screen readers (for
example, decorative images or labels for fields that are
already accessible).

• Don't rely on audio or color cues alone. Avoid
situations where the sole indication of progress,
completion, or some other state is a sound or color
change. Either design the user interface to include clear
visual cues, with sound and color for reinforcement
only, or add specific accessibility indicators. When
choosing colors, try to avoid a palette that is hard to
distinguish for users with color blindness.
• Provide captions for video content and a readable
script for audio content. It's also helpful to provide
controls that adjust the speed of audio or video content,
and ensure that volume and transport controls are easy
to find and use.
• Localize your accessibility descriptions when the app
supports multiple languages.
• Test the accessibility features of your app on each
platform it targets. For more information, see Testing
accessibility.

# Summary

In this chapter, you have
• Gained an understanding of what accessibility is
• Learned why it is important to build inclusive
applications
• Looked at how you can make use of .NET MAUI
functionality
• Considered other scenarios and how to support them
• Looked over some testing options to support your
journey to building accessible applications
In the next chapter, you will
• Add a widget to a board.
• Explore the different options available when showing
an overlay.
• Explore how you can define styling information for
your application.
• Learn how to handle devices running in light and
dark modes.
• Learn how to apply triggers to enhance your UI.
• Explore how to animate parts of your application.
• Explore what happens when you combine triggers and
animations together.

# Source Code

The resulting source code for this chapter can be found on the GitHub
repository at https://github.com/Apress/Introducing-MAUI/tree/
main/ch07.

# Extra Assignment

Take one of your favorite applications that you are completely familiar with
because you know the layout and how to use it. Then proceed to
• Turn on the screen reading assistant on your phone.

• Try to navigate your way around this application.

• Better still, try to impact your vision with a blindfold or
remove any glasses if you use them. Try to rely entirely
on the screen reader.

• Perhaps try the same but modify the device font scaling
and see if the application is able to handle increases in
text size, or if it even allow this option.

The objective is to gain a sense of the experience users with limited
vision have when using the same application. Take notes on how well
applications do things and how poorly they do other things. This can be a
really great learning exercise for you all!


















