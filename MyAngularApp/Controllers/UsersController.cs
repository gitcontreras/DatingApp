using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAngularApp.Data;
using MyAngularApp.DTOs;
using MyAngularApp.Entities;
using MyAngularApp.Interfaces;
using System.Security.Claims;

namespace MyAngularApp.Controllers
{
    //[Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository,IMapper mapper, IPhotoService photoService)
        {
            _mapper = mapper;
            _photoService=photoService;
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


        [HttpGet("{username}", Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string  username)
        {
            return await _userRepository.GetMemberAsync(username);
                    
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
             var username = "Lisa";//User.FindFirst(ClaimTypes.Name)?.Value;
             var user=await _userRepository.GetUserByNameAsync(username);

            //_mapper.Map(memberUpdateDTO,User);

            user.Introduction = memberUpdateDTO.Introduction;
            user.LookingFor = memberUpdateDTO.LookingFor;
            user.Interests = memberUpdateDTO.Interests;
            user.City = memberUpdateDTO.City;
            user.Country= memberUpdateDTO.Country;

            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) 
        {
            var user = await _userRepository.GetUserByNameAsync("Lisa");
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var Photo=new Photo 
            {
              Url = result.SecureUrl.AbsoluteUri,
              PublicId = result.PublicId,
            };

            if (user.Photos.Count == 0)
            {
                Photo.IsMain = true;
            }

            user.Photos.Add(Photo);
            if (await _userRepository.SaveAllAsync()) 
            {
                // return CreatedAtRoute("GetUser", _mapper.Map<Photo, PhotoDto>(Photo));
                return CreatedAtRoute("GetUser",new { username=user.UserName }, _mapper.Map<Photo, PhotoDto>(Photo));
            }


            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> setMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByNameAsync("Lisa");
            var photo = user.Photos.FirstOrDefault(x=>x.Id==photoId);
            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currenMain=user.Photos.FirstOrDefault(x=>x.IsMain);
            if(currenMain!=null) currenMain.IsMain=false;
            photo.IsMain=true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByNameAsync("Lisa");
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo==null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");
            photo.IsMain = true;

            if (photo.PublicId != null) 
            {
              var result=await _photoService.DeletionPhotoAsync(photo.PublicId);
              if(result.Error!=null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok(); 

            return BadRequest("Failed to delete the photo");
        }
    }
}
