using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belt.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId {set; get; }
        public int UserId {set; get; }
        public User User {set; get; }
        public int ActivityId {set; get; }
        public _Activity Activity {get; set; }
        public DateTime CreatedAt {get; set; } = DateTime.Now;
        public DateTime UpdatedAt {get; set; } = DateTime.Now;
    }
}