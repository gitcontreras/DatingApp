﻿using Microsoft.EntityFrameworkCore;
using MyAngularApp.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MyAngularApp.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context) 
        {
            if (await context.Users.AnyAsync())
                return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach (var item in users)
            {
                using var hmac = new HMACSHA512();

                item.UserName = item.UserName.ToLower();
                item.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$word"));
                item.PasswordSalt = hmac.Key;

                context.Users.Add(item);
            }

            await context.SaveChangesAsync();   
        }
    }
}
