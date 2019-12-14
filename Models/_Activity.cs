using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belt.Models
{
    public class _Activity
    {
        [Key]
        public int ActivityId {set; get; }
        [Required]
        public string Title {get; set; }
        [Required]
        public string Date {get; set; }
        public DateTime DateConverted {get; set; }  = DateTime.Now;
        [Required]
        public string Time {get; set; }
        [Required]
        public string Description {get; set; }
        [Required]
        public int Duration {get; set; }
        public string DurationType {get; set; }
        public DateTime CreatedAt {get; set; } = DateTime.Now;
        public DateTime UpdatedAt {get; set; } = DateTime.Now;
        public int UserId {get; set; }
        public User Creator {get; set; }
        public List<Participant> Participants {get; set; }
    }
}