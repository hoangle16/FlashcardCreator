using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Domain.Entities
{
    public class UserCardGroups
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }

        public virtual CardGroup CardGroup { get; set; }
        public virtual User User { get; set; }
    }
}
