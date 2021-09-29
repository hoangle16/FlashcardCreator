using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Domain.Entities
{
    public class CardGroup
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public Guid OwnerId { get; set; }
        public bool IsShare { get; set; }
        public bool Editable { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<UserCardGroups> UserCardGroups { get; set; }
        public virtual ICollection<UserCardReview> UserCardReviews { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }
}
