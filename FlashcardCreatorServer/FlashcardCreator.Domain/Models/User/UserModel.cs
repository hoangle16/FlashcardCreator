using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Domain.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Role { get; set; }
    }
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
    public class UserToken
    {
        [Required]
        public string TokenId { get; set; }
    }
    public class UserRegisterModel
    {
        [Required]
        public string Email { get; set; }
        [MinLength(6)]
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string FullName { get; set; }
    }
    public class UserUpdateModel
    {
        public string FullName { get; set; }
    }
}
