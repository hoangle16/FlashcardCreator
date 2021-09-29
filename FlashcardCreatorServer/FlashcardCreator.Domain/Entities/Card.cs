using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Word { get; set; }
        public string Definition { get; set; }
        public string Phonetic { get; set; }
        public string ImageUrl { get; set; }
        public string Example { get; set; }
        public Guid GroupId { get; set; }

        public virtual CardGroup CardGroup { get; set; }
        public ICollection<UserCardReview> UserCardReviews { get; set; }
    }
}
