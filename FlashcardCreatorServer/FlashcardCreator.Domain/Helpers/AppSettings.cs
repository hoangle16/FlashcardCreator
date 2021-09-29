using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardCreator.Domain.Helpers
{
    public class AppSettings
    {
        public string JwtSecret { get; set; }
        // Google 
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        // Facebook
        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }
    }
}
