using Microsoft.EntityFrameworkCore;
using MyAngularApp.Data;
using MyAngularApp.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace myDatingApp_test
{
    public class Tests
    {


        private static DbContextOptions<DataContext> dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "datingapp")
            .Options;


        DataContext context;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new DataContext(dbContextOptions);
            context.Database.EnsureCreated();


        }

        [OneTimeTearDown]
        public void CleanUp() 
        {
           context.Database.EnsureDeleted();
        }

        private void SeedDatabase() 
        {
            List<AppUser> ListUser = new List<AppUser> {

            new AppUser { Id = 1, UserName="Alex"},
            new AppUser { Id = 2,UserName="Jorge"},
            new AppUser { Id = 3,UserName="Delfino"}

            };

            context.Users.AddRange(ListUser);

            context.SaveChanges();
        }

    }
}