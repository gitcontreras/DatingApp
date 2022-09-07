using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAngularApp.Data;
using MyAngularApp.DTOs;
using MyAngularApp.Entities;
using MyAngularApp.Interfaces;

namespace MyAngularApp.Controllers
{
   // [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users= await _userRepository.GetMembersAsync();
           

            return Ok(users);

        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<AppUser>> GetUsers(int id)
        //{
        //    return await _userRepository.GetUserByIdAsync(id);

        //}


        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string  username)
        {
            return await _userRepository.GetMemberAsync(username);
                    
        }
    }
}
