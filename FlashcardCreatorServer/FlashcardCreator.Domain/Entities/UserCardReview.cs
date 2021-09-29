using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Domain.Entities
{
    public class UserCardReview
    {
        public Guid UserId { get; set; }
        public Guid CardId { get; set; }
        public int TimesLearned { get; set; }
        public DateTime LearnedAt { get; set; }
        public Guid GroupId { get; set; }

        public virtual User User { get; set; }
        public virtual Card Card { get; set; }
        public virtual CardGroup CardGroup { get; set; }
    }
}
