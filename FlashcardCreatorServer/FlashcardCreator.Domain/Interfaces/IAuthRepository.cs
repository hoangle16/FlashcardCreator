using FlashcardCreator.Domain.Entities;
using FlashcardCreator.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace FlashcardCreator.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GoogleAuthenticate(Payload payload);
        Task<User> FacebookAuthenticate(FacebookUserData userInfo);
        Task<User> Authenticate(UserLoginModel model);
        Task<Guid> SignupByEmail(User user, string password);
    }
}
