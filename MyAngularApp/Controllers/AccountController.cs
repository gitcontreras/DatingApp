using Microsoft.AspNetCore.Mvc;
using MyAngularApp.Data;
using MyAngularApp.Entities;
using System.Security.Cryptography;
using System.Text;

namespace MyAngularApp.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context) 
        {
          _context=context;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser {
                UserName = username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes( password)),
                PasswordSalt =hmac.Key
            };

            _context.Users.Add(user);
            _context.SaveChanges(); 
            return Ok(user);    
        }

    }
}
