using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAngularApp.Data;
using MyAngularApp.DTOs;
using MyAngularApp.Entities;
using MyAngularApp.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MyAngularApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context,ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _tokenService = tokenService;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {


            if (await UserExists(registerDto.UserName))
            {
                return BadRequest("Username is taken");
            }
            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            user.UserName = registerDto.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                user.PasswordSalt = hmac.Key;
            user.Interests = "";
            user.Introduction = "";
            user.LookingFor = "";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            //return Ok(user);

            return new UserDto
            {
                UserName = registerDto.UserName,
                Token = _tokenService.CreateToken(user),
              KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x=>x.UserName==loginDto.UserName.ToLower());


            if(user==null)
                return Unauthorized("invalid login");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return BadRequest("Invalid login");
            }

            return new UserDto
            {
                UserName = loginDto.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

        private async Task<bool> UserExists(string userName) 
        {
            return await _context.Users.AnyAsync(x=>x.UserName== userName.ToLower());
        }

    }
}
