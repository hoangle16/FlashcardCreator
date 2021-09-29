using FlashcardCreator.Domain.Constants;
using FlashcardCreator.Domain.Entities;
using FlashcardCreator.Domain.Helpers;
using FlashcardCreator.Domain.Interfaces;
using FlashcardCreator.Domain.Models.User;
using FlashcardCreator.Infrastructure.Data;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationContext _context;
        public AuthRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<User> Authenticate(UserLoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null || !AuthUtils.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }

        public async Task<User> FacebookAuthenticate(FacebookUserData userInfo)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == userInfo.Email);
            UserLogin userLogin = null;
            if (user != null)
            {
                userLogin = await _context.UserLogins.SingleOrDefaultAsync(x => x.UserId == user.Id && x.LoginProvider == LoginProvider.Facebook);
                if (userLogin == null)
                {
                    userLogin = new()
                    {
                        UserId = user.Id,
                        ProviderKey = userInfo.Id.ToString(),
                        LoginProvider = LoginProvider.Facebook
                    };
                    _context.UserLogins.Add(userLogin);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                user = new()
                {
                    Id = Guid.NewGuid(),
                    Email = userInfo.Email,
                    FullName = userInfo.Name,
                    Avatar = userInfo.Picture.Data.Url,
                    CreatedAt = DateTime.Now,
                    Role = Role.User,
                };
                userLogin = new()
                {
                    UserId = user.Id,
                    ProviderKey = userInfo.Id.ToString(),
                    LoginProvider = LoginProvider.Facebook
                };
                _context.Users.Add(user);
                _context.UserLogins.Add(userLogin);

                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User> GoogleAuthenticate(GoogleJsonWebSignature.Payload payload)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == payload.Email);
            UserLogin userLogin = null;
            if (user != null)
            {
                userLogin = await _context.UserLogins.SingleOrDefaultAsync(x => x.UserId == user.Id && x.LoginProvider == LoginProvider.Facebook);
                if (userLogin == null)
                {
                    userLogin = new()
                    {
                        UserId = user.Id,
                        ProviderKey = payload.Subject,
                        LoginProvider = LoginProvider.Google
                    };
                    _context.UserLogins.Add(userLogin);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                user = new()
                {
                    Id = Guid.NewGuid(),
                    Email = payload.Email,
                    FullName = payload.Name,
                    Avatar = payload.Picture,
                    CreatedAt = DateTime.Now,
                    Role = Role.User,
                };
                userLogin = new()
                {
                    UserId = user.Id,
                    ProviderKey = payload.Subject,
                    LoginProvider = LoginProvider.Google
                };
                _context.Users.Add(user);
                _context.UserLogins.Add(userLogin);

                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<Guid> SignupByEmail(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");
            bool isEmailExist = await _context.Users.AnyAsync(x => x.Email == user.Email);
            if (isEmailExist)
                throw new AppException($"Email \"{user.Email}\" is already taken");

            byte[] passwordHash, passwordSalt;
            AuthUtils.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.Role = Role.User;
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
