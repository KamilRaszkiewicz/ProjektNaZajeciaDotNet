﻿using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared;
using Projekt.Interfaces;
using Projekt.Models.Entities;

namespace Projekt.Extensions
{
    public static class Setup
    {
        public static async Task SeedDatabase(this IServiceScope serviceScope)
        {
            var provider = serviceScope.ServiceProvider;

            var usersRepository = provider.GetRequiredService<IRepository<User>>();

            if (usersRepository.GetAll().Any()) return;

            var userManager = provider.GetRequiredService<UserManager<User>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            await roleManager.CreateAsync(new IdentityRole<int>("admin"));

            var admin = new User
            {
                UserName = "admin@skupzywca.pl",
                Email = "admin@skupzywca.pl"
            };

            var user1 = new User
            {
                UserName = "user@hehe.pl",
                Email = "user@hehe.pl"
            };

            var user2 = new User
            {
                UserName = "vateusz@ojciecmateusz.pl",
                Email = "vateusz@ojciecmateusz.pl"
            };

        Task.WaitAll(
            userManager.CreateAsync(admin, "Haslo1234!"),
            userManager.CreateAsync(user1, "Haslo1234!"),
            userManager.CreateAsync(user2, "Haslo1234!"));

        await userManager.AddToRoleAsync(admin, "admin");

            admin.Posts = new List<Post>
            {
                new Post
                {
                    Title = "Skup żywca!!!",
                    Description = "fax: 8103574183927514268730",
                    CreatedAt = DateTime.Now,

                   Tags = new List<Tag>
                   {
                       new Tag
                       {
                           Name = "reklama"
                       }
                   }
                }
            };

            user2.Posts = new List<Post>
            {
                new Post
                {
                    Title="Tytuuuuł",
                    Description= "Treeeeść",
                    CreatedAt = DateTime.Now
                }
            };

            user1.Posts = new List<Post>
            {
                new Post
                {
                    Title = "Próbuję wytłumaczyć kolegom z Mołdawii...",
                    Description = "... ja mam tutaj akumulator na kablu i nie wiem czy w nich wjeżdżać teraz, czy za chwilę",
                    CreatedAt = DateTime.Now,
                    Images = new List<Image>
                    {
                        new Image
                        {
                            ImagePath = "images/HaARjmogQcMQtuOsauj-2Mt_TtL_yIFquPO-AwJshko.jpg"
                        }
                    },
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            AuthorId = admin.Id,
                            Content = "Skup żywca!!! fax: 8103574183927514268730",
                            IP = "172.67.223.114",
                            CreatedAt = DateTime.Now
                        },
                        new Comment
                        {
                            AuthorId = user2.Id,
                            Content = "Pchasz się w drugi próg podatkowy kolego",
                            IP = "194.181.92.98",
                            CreatedAt = DateTime.Now
                        },
                    },
                    Tags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "polskadlapolakow"
                        },
                        new Tag
                        {
                            Name = "ziemiadlaziemniakow"
                        },
                    }
                }
            };
            usersRepository.Update(admin);
            usersRepository.Update(user1);
            usersRepository.Update(user2);

            usersRepository.SaveChanges();
        }


    }
}
