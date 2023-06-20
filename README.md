# Miniblogs.

## Implementation of miniblogs using .NET Core 7 Razor pages.

### Description

Hi! This is an application that implements miniblogs functionality in the fashion of wykop mikroblog subsite. It let's users create blog posts that are visible on the main site and filtered on their own account. The closest thing to it as I said would be mikroblog on wykop.pl but without ads 😉.

[Siedlisko zła w internecie na którym można powiedzieć, że się wzorowaliśmy xD](wykop.pl/mikroblog)

### Cloning

Feel free to clone this repo. You can do this via

- HTTPS

```bash
git git@github.com:KamilRaszkiewicz/ProjektNaZajeciaDotNet.git
```

- SSH

```bash
git clone git@github.com:KamilRaszkiewicz/ProjektNaZajeciaDotNet.git
```

Make sure you have git installed on your system and if you intend to clone this app via SSH you might need to setup SSH key for your github account. [Nice tutorial that shows how to do it ;)](https://www.youtube.com/watch?v=8X4u9sca3Io&amp;t=9s)

### Installation instructions

In order to run this app the way it was intented and is proven to work flawlessly make sure you have these things installed and configured:

- Visual Studio 2022
- .NET 7.0 SDK
- SQL Server 2022
- SQL Server Management Studio 

Jump into the directory and click on solution file. This should run IDE opened with this project.

```bash
Projekt.sln	<-- Solution file
```

After you open up the project you need to add connection string in order to connect to the database installed onto your system.

This is how it should look like with SQL Server:

```json
    "DefaultConnection": "Server={server name of your database};Database={name of the database on your system};Trusted_Connection=True;TrustServerCertificate=True"
```

- **Server** <- this should contain the server name you see when you install sql server if you don't remember it you can run Query below in order to get the name:

```sql
SELECT @@SERVERNAME
```

- **Database** <- As there will be no database for our application at this moment you can give it any name you want F.E. *PlatformaBlogowaDb*

After you have done this steps it's time to run Entity Framework database creation.

Run these commands in order to install or update dotnet-ef tools.

```C#
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

Now run these command in order to create the database:

```c#
dotnet ef database update
```

### Optional

In order to get emails with:

- password reset
- account confirmation

You need to run some kind of SMTP server. The one we used is [Papercut](https://github.com/ChangemakerStudios/Papercut-SMTP).

You can configure

- host
- smtp port
- email address used to send emails

in ```Program.cs``` file by changing SmtpOptions

Default SmtpOptions:

```C#
public class SmtpOptions {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 25;
        public string SenderEmail { get; set; } = "I-ll-Have-Two-Number-9s-A-Number-6-With-Extra-Dip@And-A-Large-														Soda.com";
}
```

Example of changing SmtpOptions (this has to be added to Program.cs in order to work!):

```C#
builder.Services.Configure<SmtpOptions>(options =>
{
    options.Host = "smtp.gmail.com";
    options.Port = 587;
    options.EnableSsl = true;
    options.UserName = "your-email@gmail.com";
    options.Password = "your-password";
});
```

If you want to use SMTP server make sure to run this app without debugging.

TODO: FIX IT (We won't ;p)

### Default defined users

| Login                                                       | Password   |
| ----------------------------------------------------------- | ---------- |
| [admin@skupzywca.pl](mailto:admin@skupzywca.pl)             | Haslo1234! |
| [user@hehe.pl](mailto:user@hehe.pl)                         | Haslo1234! |
| [vateusz@ojciecmateusz.pl](mailto:vateusz@ojciecmateusz.pl) | Haslo1234! |

- First account has a role of admin.