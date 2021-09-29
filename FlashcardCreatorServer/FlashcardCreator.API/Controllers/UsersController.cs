using AutoMapper;
using FlashcardCreator.Domain.Constants;
using FlashcardCreator.Domain.Entities;
using FlashcardCreator.Domain.Interfaces;
using FlashcardCreator.Domain.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardCreator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAll();
            var models = _mapper.Map<IList<UserModel>>(users);

            return Ok(models);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
            {
                return Forbid();
            }
            var user = await _unitOfWork.Users.GetById(id);
            if (user == null)
                return NotFound();
            var model = _mapper.Map<UserModel>(user);

            return Ok(model);
        }
        // will add avatar upload feature later
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateModel model)
        {
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
                return Forbid();

            var user = await _unitOfWork.Users.GetById(id);
            if (!string.IsNullOrWhiteSpace(model.FullName))
                user.FullName = model.FullName;
            _unitOfWork.Users.Update(user);

            await _unitOfWork.Complete();
            return Ok();
        }
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            if (user == null)
                return NotFound();
            _unitOfWork.Users.Remove(user);

            return Ok();
        }
    }
}
