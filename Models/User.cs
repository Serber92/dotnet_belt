using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belt.Models
{
    public class User
    {
        [Key]
        public int UserId {set; get; }
        [Required]
        [MinLength(2)]
        public string Name {get; set; }
        [Required]
        [EmailAddress]
        public string Email {get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password {get; set; }
        [Required]
        [DataType(DataType.Password)]
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword {set; get; }
        public DateTime CreatedAt {get; set; } = DateTime.Now;
        public DateTime UpdatedAt {get; set; } = DateTime.Now;
        public List<_Activity> CreatedActivity {get; set; }
        public List<Participant> ActivitiesJoined {get; set; }
    }
}