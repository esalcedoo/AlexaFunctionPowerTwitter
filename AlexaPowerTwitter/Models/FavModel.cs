using System;

namespace AlexaPowerTwitter.Models
{
    class FavModel
    {
        public Guid ItemInternalId { get; set; }
        public string cr825_powertweetsid { get; set; }
        public string cr825_author { get; set; }
        public string cr825_message { get; set; }
        public string entityimage { get; set; }
        public DateTime modifiedon { get; set; }
        public DateTime createdon { get; set; }
        public string lang { get; set; }

        public string ToSSML()
        {
            return $"<lang xml:lang=\"{lang}\">{cr825_message}</lang>";
        }
    }
}