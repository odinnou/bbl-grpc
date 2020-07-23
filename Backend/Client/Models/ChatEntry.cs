using System;

namespace Client.Models
{
    public class ChatEntry
    {
        public DateTime DateCreated { get; set; }
        public string Login { get; set; }
        public string Message { get; set; }
    }
}
