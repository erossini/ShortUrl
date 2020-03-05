# ShortUrl
Creating a URL shortener using ASP.NET WepAPI and MVC

In this solution, I use several techniques and tools. I use Microsoft Visual Studio 2015 and the latest version of all components.

- **ASP.NET MVC**: Microsoft’s modern web application framework. As the name says, it pushes you to use the MVC (model view controller) software design principle.
- **ASP.NET Web API**: Web API and MVC are used together in many applications. With MVC, the HTML of the web pages are rendered on the server, and with Web API you can, like the name says, create an API. Web API also uses the MVC principle, but returns XML (or JSON, or YAML, or … whatever you want) instead of HTML.
- **Microsoft SQL Server**: this is my primary database choice.
- **Entity Framework**: this is my favourite ORM (object relational mapping) framework. With Entity Framework, you can create a database “code-first”, meaning that you can create classes (called entities) that represent the database and create the database based on those entities and its relations. When you’ve updated one of these entities, you can add a new migration, which means that the changes to your entities will be written to the database.
- **Unity**: Unity is a dependency injection framework. You can read more on dependency injection and inversion of control later in this tutorial.
- **Microsoft Report**: creates reports of our stats

## Home page
This is what the public home page looks like. From here, you can create a new short url to share. What this solution is offering is:

- the format of a short link is http://psc.fyi/<your name>
- for each short url you have:
    - statistics http://psc.fyi/<your name>/stats
    - graphs
    - visitor world map
    - PDF export
    - QRCode

If you register on it:
- nobody can view your statistics
- view and edit your short url list

![Short Url Home Page](https://github.com/erossini/ShortUrl/blob/master/homepage.png)

## Statistic page

![Statistic page](https://github.com/erossini/ShortUrl/blob/master/image.png)
