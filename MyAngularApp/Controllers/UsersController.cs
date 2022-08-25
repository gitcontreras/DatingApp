﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAngularApp.Data;
using MyAngularApp.Entities;

namespace MyAngularApp.Controllers
{

    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();

        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUsers(int id)
        {
            return await _context.Users.FindAsync(id);

        }
    }
}
