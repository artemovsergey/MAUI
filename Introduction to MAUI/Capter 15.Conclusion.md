# CHAPTER 15

# Conclusion

Wow! If you made it this far, I want to thank you so much! I really hope that
you have enjoyed reading this book as much as I enjoyed writing it. This
book was designed to give you an insight into what .NET MAUI offers and
how you can use it to build real world applications. The sample we built
together covers a lot of the key concepts. Of course I could have filled the
book with hundreds more pages, adding in so many more widgets and
features to the application. This application is a concept that is near and
dear to my heart so I can tell you that it will continue to evolve over time. I
would love to hear where you decide to take it next, and I would love to see
what you create next.

# Looking at the Final Product

The application we just finished building together has been a pet project
of mine for years, so thank you for helping me to finally reach this dream!
Let’s take a trip down memory lane to review what exactly we have built.
Figure 15-1 shows my prototype sketch.

![image](https://user-images.githubusercontent.com/26972859/231515190-5d2348fa-972a-483d-ada8-ce2ea6bb8554.png)
Figure 15-1. Sketch prototype of the application

The process of building this application has taken you through many
different concepts including

• Creating a .NET MAUI application project

• Reviewing the possible architecture patterns you can
use to build .NET MAUI applications

• Learning about the building blocks that make up your
application’s UI

• How you can further expand on the UI through styling

• How to make your application accessible

• How to create your own layout and utilize some
cool features like BindableLayout to do a lot of the
heavy lifting

• How to store data and the scenarios around where best
to store each type of data

• How to access different types of remote data and the
scenarios around when things go wrong

• How to customize your application on a perplatform basis

• How to test your application

• The concept of distributing your application

All of the items above made for a really fun journey! And the end
result is almost identical to my original plan. Figure 15-2 shows the final
application with the widgets.

![image](https://user-images.githubusercontent.com/26972859/231515520-45ab1e05-09d4-4d3b-9355-d5edd6518397.png)
Figure 15-2. The final application showing the widgets that we have
added plus the results of some of the extra assignment sections

# Taking the Project Further
One main reason I really love this project is that I believe the possibilities
of future widgets is wide open! I could provide a list as long as my arm
on ideas that we could continue to achieve together. If I could, I would
have fit them all in this book, but I would probably never have finished. 

Here is a short list of the things that I think we could achieve based on the
knowledge that you have gathered during the book:

• Family planning calendar

• Image widget

• Slide show from device

• Slide show from external webservice

• Shopping list

• Home assistant integration

• OctoPrint status widget

• Smart meter widget

• Social media follower count widget

I am repeating myself here, but I would really love to hear from you
about your experience reading this book and where you have decided to
take our application next.

# Useful Resources

There are so many great places to find information on either building .NET
MAUI applications or solving issues that may arise during that experience.
The following list is a collection of websites that provide some really great
content along with a few specific examples of content creators on those
platforms.

# StackOverflow

StackOverflow is a question-and-answer site where you can seek
assistance for issues that you encounter. Often someone else has already
asked the question so you can find the answer you need. If you can't find a
.NET MAUI-specific question/answer, it is worth also looking for Xamarin.
Forms question/answers given that it is the predecessor to .NET MAUI.
https://stackoverflow.com

# GitHub

GitHub is where the .NET MAUI repository is hosted and the framework is
developed in the open. I strongly recommend keeping up to date with the
discussions and issues on this repository.
https://github.com/dotnet/maui

# YouTube

There are some really great content creators providing video tutorials
on how to build .NET MAUI applications. Two great creators are in fact
Microsoft employees; however, they build this content in their own free
time, which I believe goes to show just how passionate they are about the
framework.

# Gerald Versluis

www.youtube.com/c/GeraldVersluis

# James Montemagno

www.youtube.com/c/JamesMontemagno

# Social Media

There is a whole host of social media options such as LinkedIn, Discord,
Twitter, and Facebook. I urge you to find the platform that works best
for you and start finding and following people that work on or with the
technology.

# Yet More Goodness

It is impossible to provide a curated list of all the great content creators
or resources in printed form. It will instantly become outdated. In fact,
by the time you have finished this book you have may well have become
another name to add to this list! For that reason, here is a great resource
that provides a curated list: https://github.com/jfversluis/learndotnet-maui.

# Looking Forward

While .NET MAUI offers us a lot, there is still so much more that will
evolve. I fully expect there to be some extensive work applied to improving
the ability to test the user interfaces of .NET MAUI applications along with
further enhancements in the usage of .NET MAUI graphics, which has the
potential to not only render applications identically across each platform
(which is very similar to how Flutter works) but also to boost performance
by moving away from the native controls that come with Android. Some
key areas to keep an eye on are as follows.

# Upgrading from Xamarin.Forms

I would like to see where the dotnet upgrade assistant goes. This will likely
prove vital to any existing applications built against Xamarin.Forms. There
has been work on it so far, but at the time of writing it is in preview. There
are documented steps on how to manually migrate applications with the
following two links covering how this can be achieved:
https://learn.microsoft.com/dotnet/maui/get-started/migrate
https://learn.microsoft.com/dotnet/maui/migration/
I can confirm that I have followed through the above steps in order
to migrate a relatively simple application from Xamarin.Forms to .NET
MAUI. Of course, if your application is more involved and uses concepts
that are now obsolete (Renderers, Effects, etc.), then the migration process
could be much more involved. While I do expect the dotnet upgrade
assistant to make this task easier, I highly doubt it will get to a point where
it will do everything for us. In fact, I suggest you take this as a good exercise
to review what your Xamarin.Forms application currently does and
whether there are better ways to achieve things moving forwards.

# Comet

I have been surprised by what Comet has to offer despite it also being in
preview/proof of concept at the time of writing. David Ortinau, a Program
Manager on the .NET MAUI team, has praised the work that has come out
of the Comet investigation, stating that a lot of the evolutionary steps from
Xamarin.Forms to .NET MAUI were largely influenced by this work.
https://github.com/dotnet/Comet

# Testing

The topic of testing really does excite me! I have spent recent months
working with development and test teams to help build processes and
infrastructure focused heavily on testing. Having the ability to automate
testing of applications will be a huge leap forwards. As I covered in
Chapter 12, there is the Xamarin.UITest framework for testing Xamarin.
Forms applications. Like the technology it was testing, it had its failings/
challenges. Therefore, the idea of rewriting and exploring new testing
approaches is very much a good thing in my book. I won’t lie; I would have
loved it sooner–certainly before this book was published–but nonetheless
having it being worked on is positive.
I know that I will be keeping an eye on this repository, and if testing
is important to you (and it should be), you should do so also. One further
great advantage is that with the framework being developed in the
open, we as the potential consumers have the power to get involved and
influence the final result.
https://github.com/Redth/Maui.UITesting









