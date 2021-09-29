using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Role { get; set; }
        
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<CardGroup> CardGroups { get; set; }
        public virtual ICollection<UserCardGroups> UserCardGroups { get; set; }
        public virtual ICollection<UserCardReview> UserCardReviews { get; set; }
    }
}
