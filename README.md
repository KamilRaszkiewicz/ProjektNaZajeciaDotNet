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

### Implemented functionality

#### User profile

- [x] As a not signed in user I can register to the service.
- [x] As a not signed in user I can reset my password, password recovery procedure should be safe.
- [x] As a signed in user I can edit my personal data.
- [x] As a not signed in user I can log in and log out from into/from the service
- [x] As a user I can have an account with admin permissions
- [x] As an admin I can delete any user.

#### Blog

- [x] As a signed in user I can create posts. Post has a label, body up to 256 chars and tags. One post can have many tags. There can be many photos.
- [x] As a signed in user i can add, delete, edit tags.
- [x] As a post owner i can edit it with tag, photo changes.
- [x] As a post owner i can delete it.
- [x] Any user sees 10 main posts.
- [x] Any user after going to user personal page sees their posts.

#### Comments

- [x] As a post owner and comment owner or Admin I can delete selected comment.

### Other

- [x] Aesthethic look to the website

Oskar gave all of his heart power into this CSS custom styling and after seeing how beautiful it turned out we are not even mad anymore at  the fact that he removed bootstrap early in the development of the project.

- [x] Clean Architecture

  - [x] Viewmodels

  They are stored in Dtos catalogue.

  ```
  Dtos
  ├── Comments
  │   ├── CommentDto.cs
  │   ├── CreateCommentDto.cs
  │   └── DeleteCommentDto.cs
  ├── ImageDto.cs
  └── Posts
      ├── CreatePostDto.cs
      ├── DeletePostDto.cs
      └── PostDto.cs
  ```

  ```C#
  # Create.cshtml.cs
  .
  .
  .
  [BindProperty]
  public CreateCommentDto Form { get; set; }
  .
  .
  .
  ```

  Thanks to model binding when OnPost() is invoked our viewmodel gets the information first and then it sends them over to proper entity ones stored in Models/Entities catalogue.

  ```
  Models
  ├── ApplicationDbContext.cs
  └── Entities
      ├── Comment.cs
      ├── Image.cs
      ├── Post.cs
      ├── Tag.cs
      └── User.cs
  ```

  - [x] Services

  Various services are used in the project. One using mentioned SMTP Server is EmailSender which function is well... sending emails. It provides function that gets email, subject, message and it does what it should. It implements IEmailSender interface that is a part of ASP.NET Core identity.

  Services are implemented in catalogue Services and its interfaces are in catalogue Interfaces:

  ```bash
  Services
  ├── AdminService.cs
  ├── EmailSender.cs
  ├── ImagesService.cs
  ├── PostsService.cs
  └── Repository.cs
  Interfaces
  ├── IAdminService.cs
  ├── IImagesService.cs
  ├── IPostsService.cs
  └── IRepository.cs
  ```

  We made sure to use proper "life of the object" used by IoC.

  ```C#
  # Program.cs
  .
  .
  .
  builder.Services.AddScoped<IPostsService, PostsService>();
  builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
  builder.Services.AddSingleton<IImagesService, ImagesService>();
  builder.Services.AddScoped<IAdminService, AdminService>();
  .
  .
  .
  ```

  - [x] Repositories

  Repositories are implemented via IRepository service which gets TEntity type.

  - [x] Naming

​		We made sure to follow C# and .NET style guides both following syntax, naming etc. [Common C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).