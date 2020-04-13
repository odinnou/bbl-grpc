using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class ChatEntry
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Login { get; set; }
        public string Message { get; set; }
    }
}
