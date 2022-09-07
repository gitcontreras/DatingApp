using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAngularApp.Controllers;
using MyAngularApp.Data;
using MyAngularApp.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myDatingApp_test
{
    public class TestinControllerMethods
    {
        private static DbContextOptions<DataContext> dbContextOptions = new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase(databaseName: "datingControllerapp")
    .Options;


        DataContext context;

        UsersController testUser;

        //[OneTimeSetUp]
        //public void Setup()
        //{
        //    context = new DataContext(dbContextOptions);
        //    context.Database.EnsureCreated();


        //    SeedDatabase();

        //    testUser =new UsersController(context);
        //}


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

  

        [Test]
        public async Task HTTPGetUsers()
        {
            var users = await testUser.GetUsers();
            Assert.AreEqual(users.Value.Count(), 3);
        }

    }
}
